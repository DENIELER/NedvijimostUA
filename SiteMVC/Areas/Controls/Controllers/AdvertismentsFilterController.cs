using SiteMVC.Areas.Controls.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SiteMVC.Areas.Controls.Controllers
{
    public class AdvertismentsFilterController : Controller
    {
        public ActionResult Index(SiteMVC.Models.Engine.AdvertismentsFilter filter)
        {
            var filterViewModel = new FilterViewModel();

            //--- copy filter from original
            if (filter != null)
                filterViewModel.Filter = filter;
            
            ViewBag.RoomsFilterList = filterViewModel.GetRoomsCountCategoriesList();

            return View(filterViewModel);
        }
    }
}
