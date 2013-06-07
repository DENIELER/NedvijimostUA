using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyAdvertisments : System.Web.UI.Page
{
    protected int? editAdvertismentID = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string editAdvIDText = Request["edit"];
        if(!string.IsNullOrEmpty(editAdvIDText))
        {
            int editAdvID;
            if (int.TryParse(editAdvIDText, out editAdvID))
            {
                var workflow = new AdvertismentsWorkflow();
                if (workflow.ExistsAdvertisment(editAdvID, Authorization.Authorization.CurrentUser_UserID()))
                    editAdvertismentID = editAdvID;
            }
        }

        if (editAdvertismentID != null)
        {
            if (Request.Form.Count == 0)
            {
                var dataModel = new Model.NedvijimostDBEntities();

                txtText.Text = dataModel.Advertisments
                    .Where(a => a.Id == editAdvertismentID)
                    .Select(a => a.text)
                    .FirstOrDefault();

                rptPhones.DataSource = dataModel.AdvertismentPhones
                                        .Where(p => p.AdvertismentId == editAdvertismentID);
                rptPhones.DataBind();
            }
            else
            {
                var dataModel = new Model.NedvijimostDBEntities();

                int? authorizedUserID = Authorization.Authorization.CurrentUser_UserID();
                if(!authorizedUserID.HasValue)
                    return;

                var advertisment = dataModel.Advertisments
                    .FirstOrDefault(a => a.Id == editAdvertismentID
                    && a.UserID == authorizedUserID.Value);

                if (advertisment != null)
                {
                    advertisment.text = txtText.Text;

                    string phoneIDsForm = Request.Form["phoneId"];
                    string[] phoneIDs = phoneIDsForm.Split(',');

                    foreach (string phoneID in phoneIDs)
                    {
                        int phoneIDInt;
                        if (!int.TryParse(phoneID, out phoneIDInt))
                            continue;

                        string phoneEdited = Request.Form["phones[" + phoneID + "].phone"];
                        var phone = dataModel.AdvertismentPhones
                            .FirstOrDefault(p => p.Id == phoneIDInt
                                && p.AdvertismentId == editAdvertismentID);
                        if (phone != null)
                        {
                            phone.phone = phoneEdited;
                        }
                    }

                    dataModel.SaveChanges();
                    Response.Redirect(Request.Url.AbsolutePath);
                }
            }
        }else
            ldsAdvertisments.WhereParameters["UserID"].DefaultValue = Authorization.Authorization.CurrentUser_UserID().ToString();
    }

    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        int advertismentID;
        if (!int.TryParse(e.CommandArgument.ToString(), out advertismentID))
            return;

        var workflow = new AdvertismentsWorkflow();
        workflow.HideAdvertisment(advertismentID);

        Response.Redirect(Request.RawUrl);
    }
}