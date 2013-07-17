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
        public ActionResult Index(string sectionUrl, string subSectionUrl, string subpurchaseMode)
        {
            var result = new SiteMVC.Models.Engine.AdvertismentsRequest();

            var dataModel = new DataModel();

            var section = dataModel.AdvertismentSections
                .FirstOrDefault(s => s.friendlyUrl == sectionUrl);
            if (section == null)
                throw new Exception("Not founded section with corresponded url.");
            else
            {
                result.SectionId = section.Id;
                result.SectionName = section.displayName;
                result.SectionTitle = section.Title;
                result.SectionDescription = section.Description;
                result.SectionKeywords = section.Keywords;
            }

            var subSection = dataModel.AdvertismentSubSections
                .FirstOrDefault(s => s.friendlyUrl == subSectionUrl
                                    && s.AdvertismentSection_Id == section.Id);
            if (subSection == null)
                result.SubSectionId = null;
            else
            {
                result.SubSectionId = subSection.Id;
                if (!string.IsNullOrEmpty(subSection.displayName))
                    result.SectionName = subSection.displayName;
                if (!string.IsNullOrEmpty(subSection.Title))
                    result.SectionTitle = subSection.Title;
                if (!string.IsNullOrEmpty(subSection.Description))
                    result.SectionDescription = subSection.Description;
                if (!string.IsNullOrEmpty(subSection.Keywords))
                    result.SectionKeywords = subSection.Keywords;
            }

            result.State = State.NotSubpurchase;
            if (subpurchaseMode == Resources.Resource.VKLUCHAYA_POSREDNIKOV)
                result.State = State.SubpurchaseWithNotSubpurchase;

            result.Url = Request.RawUrl;

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

        public ActionResult EditAdvertisment(int advertismentID)
        {
            var dataModel = new DataModel();

            var advertisment = dataModel.Advertisments
                .SingleOrDefault(a => a.Id == advertismentID);
            if (advertisment == null)
                throw new Exception("Can not find advertisment with current ID.");

            return View(advertisment);
        }
    }
}
