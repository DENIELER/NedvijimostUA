using SiteMVC.Models;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class AdvertismentsController : Controller
    {
        public ActionResult Index(string sectionUrl, string subSectionUrl, string subpurchaseMode, [System.Web.Http.FromUri] Models.Engine.AdvertismentsFilter advertismentsFilter, string city = null)
        {
            var result = new SiteMVC.ViewModels.Advertisments.AdvertismentsPageViewModel();
            result.Request = new SiteMVC.Models.Engine.AdvertismentsRequest();
            result.Request.Filter = advertismentsFilter;

            var dataModel = new DataModel();

            var section = dataModel.AdvertismentSections
                .FirstOrDefault(s => s.friendlyUrl == sectionUrl);
            if (section == null)
                throw new Exception("Not founded section with corresponded url.");
            else
            {
                result.Request.SectionId = section.Id;
                result.Request.SectionName = section.displayName;

                result.SectionTitle = section.Title;
                result.SectionDescription = section.Description;
                result.SectionKeywords = section.Keywords;
            }

            var subSection = dataModel.AdvertismentSubSections
                .FirstOrDefault(s => s.friendlyUrl == subSectionUrl
                                    && s.AdvertismentSection_Id == section.Id);
            if (subSection == null)
                result.Request.SubSectionId = null;
            else
            {
                result.Request.SubSectionId = subSection.Id;
                if (!string.IsNullOrEmpty(subSection.displayName))
                    result.Request.SectionName = subSection.displayName;

                if (!string.IsNullOrEmpty(subSection.Title))
                    result.SectionTitle = subSection.Title;
                if (!string.IsNullOrEmpty(subSection.Description))
                    result.SectionDescription = subSection.Description;
                if (!string.IsNullOrEmpty(subSection.Keywords))
                    result.SectionKeywords = subSection.Keywords;
            }
        
            result.Request.State = State.NotSubpurchase;
            if (subpurchaseMode == Resources.Resource.VKLUCHAYA_POSREDNIKOV)
                result.Request.State = State.SubpurchaseWithNotSubpurchase;

            result.Request.Url = Request.RawUrl;
            result.Request.City = city;

            return View(result);
        }

        [HttpPost]
        public JsonResult RemoveAdvertisment(int advertismentID)
        {
            var dataModel = new DataModel();

            var advertisment = dataModel.Advertisments
                .SingleOrDefault(a => a.Id == advertismentID);
            if (advertisment == null)
                return Json("failed");

            advertisment.not_show_advertisment = true;

            dataModel.SubmitChanges();

            return Json("success");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditAdvertisment(int advertismentID)
        {
            var dataModel = new DataModel();

            var advertisment = dataModel.Advertisments
                .SingleOrDefault(a => a.Id == advertismentID);
            if (advertisment == null)
                throw new Exception("Can not find advertisment with current ID.");

            return View(advertisment);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAdvertisment(Advertisment advertisment)
        {
            var dataModel = new DataModel();

            try
            {
                var dbAdvertisment = dataModel.Advertisments
                    .SingleOrDefault(a => a.Id == advertisment.Id);
                if (dbAdvertisment == null)
                    throw new Exception("Can not find advertisment with current ID.");

                dbAdvertisment.modifyDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow();
                dbAdvertisment.Address1 = advertisment.Address1;
                dbAdvertisment.text = advertisment.text;

                dataModel.SubmitChanges();
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return PartialView("~/Views/Controls/FormSubmitSaveFailed.cshtml");
            }

            return PartialView("~/Views/Controls/FormSubmitSaveSuccess.cshtml");
        }
    }
}
