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
        public ActionResult Index(string actionName, string controllerName, RouteValueDictionary routeData)
        {
            var filterViewModel = new FilterViewModel();
            filterViewModel.ActionName = actionName;
            filterViewModel.ControllerName = controllerName;
            filterViewModel.RouteData = routeData;
            
            ViewBag.RoomsFilterList = filterViewModel.GetRoomsCountCategoriesList();

            return View(filterViewModel);
        }
    }
}
