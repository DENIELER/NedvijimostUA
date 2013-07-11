using SiteMVC.Models;
using SiteMVC.ViewModels.Advertisments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class AdvertismentsController : Controller
    {
        public ActionResult Index()
        {
            var dataModel = new DataModel();
            
            var advertismentSections = dataModel.AdvertismentSections
                                       .OrderBy(s => s.displayName)
                                       .ToList();

            var viewModel = new ViewModels.Advertisments.AddAdvertisment()
                {
                    Sections = advertismentSections
                };

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSubSectionsEditList(string sectionID)
        {
            int _sectionID;
            if (string.IsNullOrEmpty(sectionID) || !int.TryParse(sectionID, out _sectionID))
                return PartialView(new List<SiteMVC.Models.AdvertismentSubSection>());

            var dataModel = new DataModel();

            var advertismentSections = dataModel.AdvertismentSubSections
                                       .Where(s => s.AdvertismentSection_Id == _sectionID)
                                       .OrderBy(s => s.displayName)
                                       .ToList();

            return PartialView(advertismentSections);
        }
        
        private string uploadedPhotosSessionKey = "AddAdvertisment_UploadedPhotos";

        [HttpPost]
        public ActionResult Add(IEnumerable<HttpPostedFileBase> files)
        {
            var photoResponsesList = new List<AdvertismentPhotoResponse>();

            if (Request.ContentType.Contains("multipart/form-data"))
            {
                foreach (string file in Request.Files)
                {
                    var hpf = Request.Files[file] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;
                    string pathPhotos = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "Data/AdvertismentsPhotos");
                    string savedFileName = Path.Combine(
                            pathPhotos,
                            Path.GetFileName(hpf.FileName));
                    hpf.SaveAs(savedFileName);

                    //--- make response
                    var photoResponse = new AdvertismentPhotoResponse();
                    photoResponse.Name = Path.GetFileName(hpf.FileName);
                    photoResponse.Size = hpf.ContentLength;
                    photoResponse.Url = string.Format("{0}://{1}/files/{2}",
                                                      Request.Url.Scheme,
                                                      Request.Url.Host,
                                                      Path.GetFileName(hpf.FileName));
                    photoResponse.Thumbnail_url = savedFileName;
                    photoResponse.Delete_url = savedFileName;
                    photoResponse.Delete_type = "POST";

                    photoResponsesList.Add(photoResponse);
                }

                //--- save into session
                if (Session[uploadedPhotosSessionKey] != null)
                {
                    var list = Session[uploadedPhotosSessionKey] as List<AdvertismentPhotoResponse>;
                    list.AddRange(photoResponsesList);

                    Session[uploadedPhotosSessionKey] = list;
                }
                else
                    Session.Add(uploadedPhotosSessionKey, photoResponsesList);
            }

            return View();
        }
    }
}
