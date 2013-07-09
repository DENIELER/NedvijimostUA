using SiteMVC.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult GetSubSections(int sectionID)
        {
            var dataModel = new DataModel();

            var advertismentSections = dataModel.AdvertismentSubSections
                                       .Where(s => s.AdvertismentSection_Id == sectionID)
                                       .OrderBy(s => s.displayName)
                                       .ToList();

            return View(advertismentSections);
        }
    }
}
