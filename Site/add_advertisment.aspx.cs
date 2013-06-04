using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class add_advertisment : System.Web.UI.Page
{
    private string uploadedPhotosSessionKey = "AddAdvertisment_UploadedPhotos";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Request.ContentType.Contains("multipart/form-data"))
            {
                var photoResponsesList = new List<PhotoResponse>();
                foreach (string file in Request.Files)
                {
                    var hpf = Request.Files[file] as HttpPostedFile;
                    if (hpf.ContentLength == 0)
                        continue;
                    string pathPhotos = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "files");
                    string savedFileName = Path.Combine(
                        pathPhotos,
                        Path.GetFileName(hpf.FileName));
                    hpf.SaveAs(savedFileName);

                    //--- make response
                    var photoResponse = new PhotoResponse();
                    photoResponse.name = Path.GetFileName(hpf.FileName);
                    photoResponse.size = hpf.ContentLength;
                    photoResponse.url = string.Format("{0}://{1}/files/{2}",    
                                                      Request.Url.Scheme,
                                                      Request.Url.Host,
                                                      Path.GetFileName(hpf.FileName));
                    photoResponse.thumbnail_url = savedFileName;
                    photoResponse.delete_url = savedFileName;
                    photoResponse.delete_type = "POST";

                    photoResponsesList.Add(photoResponse);
                }

                //--- save into session
                if(Session[uploadedPhotosSessionKey] != null)
                {
                    var list = Session[uploadedPhotosSessionKey] as List<PhotoResponse>;
                    list.AddRange(photoResponsesList);

                    Session[uploadedPhotosSessionKey] = list;
                }else
                {
                    Session.Add(uploadedPhotosSessionKey, photoResponsesList);
                }

                Response.Clear();
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(photoResponsesList));
                Response.End();
            }
        }else
        {
            Session.Remove(uploadedPhotosSessionKey);
        }
    }

    protected void AddNewAdvertisment(object sender, EventArgs e)
    {
        int sectionID;
        int.TryParse(ddlAdvSection.SelectedValue, out sectionID);

        int? subsectionID = null;
        int _subsectionID;
        if (int.TryParse(ddlAdvSubSection.SelectedValue, out _subsectionID))
            subsectionID = _subsectionID;

        string address = inputAddress.Value;
        string phone = inputPhone.Value;
        string text = inputAdvText.Value;

        List<PhotoResponse> photosList = null;
        var photosListObject = Session[uploadedPhotosSessionKey];
        if (photosListObject != null && photosListObject is List<PhotoResponse>)
        {
            photosList = photosListObject as List<PhotoResponse>;
        }

        SaveAdvertismentInfoDB(sectionID, subsectionID, address, phone, text, photosList);
    }

    private void SaveAdvertismentInfoDB(int sectionID, int? subsectionID, string address, string phone, string text, List<PhotoResponse> photosList)
    {
        var context = new NedvijimostDBEntities();

        var advertisment = new Advertisment();
        advertisment.createDate = Utils.GetUkranianDateTimeNow();
        advertisment.modifyDate = Utils.GetUkranianDateTimeNow();
        advertisment.text = text;
        advertisment.AdvertismentSection = context.AdvertismentSections.FirstOrDefault(s => s.Id == sectionID);
        advertisment.AdvertismentSubSection = context.AdvertismentSubSection.FirstOrDefault(s => s.Id == subsectionID);
        advertisment.link = string.Empty;
        advertisment.siteName = "Nedvijimost-UA";
        advertisment.SubPurchase = null;
        advertisment.subpurchaseAdvertisment = false;
        context.AddToAdvertisments(advertisment);
        context.SaveChanges();

        string[] phonesSplited = phone.Split(',');
        foreach (var phoneSplited in phonesSplited)
        {
            var advPhone = new AdvertismentPhone();
            advPhone.phone = phoneSplited;
            advPhone.Advertisment = advertisment;
            context.AddToAdvertismentPhones(advPhone);
            context.SaveChanges();
        }

        if (photosList != null)
        {
            foreach (var photo in photosList)
            {
                var advertismentsPhoto = new AdvertismentsPhoto();
                advertismentsPhoto.filename = photo.url;
                advertismentsPhoto.createDate = Utils.GetUkranianDateTimeNow();
                advertismentsPhoto.Advertisment = advertisment;
                
                context.AddToAdvertismentsPhotoes(advertismentsPhoto);
                context.SaveChanges();
            }
        }

        Session.Remove(uploadedPhotosSessionKey);
        Response.Redirect("/add_advertisment.aspx?success=1");
    }

    protected void ddlAdvType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAdvSection = (DropDownList)sender;
        if (ddlAdvSection != null)
        {
            int sectionID;
            if (int.TryParse(ddlAdvSection.SelectedValue, out sectionID))
            {
                var context = new NedvijimostDBEntities();
                if (context.AdvertismentSubSection.Any(x => x.AdvertismentSection.Id == sectionID))
                {
                    ddlAdvSubSection.DataSource = context.AdvertismentSubSection
                                        .Where(x => x.AdvertismentSection.Id == sectionID)
                                        .Select(x => new { Id = x.Id, displayName = x.displayName });
                    ddlAdvSubSection.DataBind();

                    pnlAdvSubSections.Visible = true;
                }
                else
                {
                    pnlAdvSubSections.Visible = false;
                }
            }
        }
    }

    [Serializable()]
    class PhotoResponse
    {
        public string name { get; set; }
        public int size { get; set; }

        public string url { get; set; }
        public string thumbnail_url { get; set; }

        public string delete_url { get; set; }
        public string delete_type { get; set; }
    }
}