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
        
        [HttpPost]
        public ActionResult Add(SiteMVC.ViewModels.Advertisments.Advertisment advertisment, IEnumerable<HttpPostedFileBase> files)
        {
            List<AdvertismentPhone> phones = GetAdvertismentPhones(advertisment.Phones);
            List<AdvertismentsPhoto> photos = GetAdvertismentPhotos(files);

            if (!phones.Any())
                return RedirectToAction("PhonesNotFound");

            var newAdvertisment = new Models.Advertisment();
            newAdvertisment.AdvertismentSection_Id = advertisment.AdvertismentSection_Id;
            newAdvertisment.AdvertismentSubSection_Id = advertisment.AdvertismentSubSection_Id;
            
            newAdvertisment.AdvertismentPhones.AddRange(phones);
            newAdvertisment.AdvertismentsPhotos.AddRange(photos);
            
            newAdvertisment.createDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow();
            newAdvertisment.modifyDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow();

            newAdvertisment.isSpecial = false;
            newAdvertisment.isSpecialDateTime = null;
            newAdvertisment.link = "http://nedvijimost-ua.com";
            newAdvertisment.not_realestate = false;
            newAdvertisment.not_show_advertisment = false;
            newAdvertisment.siteName = "Nedvijimost-UA";
            newAdvertisment.subpurchaseAdvertisment = false;
            newAdvertisment.SubPurchase_Id = null;

            newAdvertisment.text = advertisment.Text;
            //--- newAdvertisment.Address1 = advertisment.Address;

            if (SystemUtils.Authorization.IsAuthorized)
                newAdvertisment.UserID = SystemUtils.Authorization.UserID;
            else
                newAdvertisment.UserID = null;

            var dataModel = new DataModel();
            dataModel.Advertisments.InsertOnSubmit(newAdvertisment);
            dataModel.SubmitChanges();

            return RedirectToAction("SuccessfulyAdd");
        }

        public ActionResult PhonesNotFound()
        {
            return View();
        }

        public ActionResult SuccessfulyAdd()
        {
            return View();
        }

        private List<AdvertismentPhone> GetAdvertismentPhones(string phones)
        {
            string[] phonesArray = phones.Split(',');
            return phonesArray
                .Select(p => new AdvertismentPhone() 
                            { 
                                phone = p 
                            })
                .ToList();
        }
        private List<AdvertismentsPhoto> GetAdvertismentPhotos(IEnumerable<HttpPostedFileBase> files)
        {
            var photos = new List<AdvertismentsPhoto>();

            if(files != null)
                foreach (HttpPostedFileBase file in files)
                {
                    if (file.ContentLength == 0)
                        continue;
                    string pathPhotos = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "Data/AdvertismentsPhotos");
                    string savedFileName = Path.Combine(
                            pathPhotos,
                            Path.GetFileName(file.FileName));
                    file.SaveAs(savedFileName);

                    //--- make response
                    var photoResponse = new Models.AdvertismentsPhoto();
                    photoResponse.filename = Path.GetFileName(file.FileName);

                    photos.Add(photoResponse);
                }

            return photos;
        }
    }
}
