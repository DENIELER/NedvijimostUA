using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Areas.Controls.Controllers
{
    public class AdvertismentsFilterController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.RoomsFilterList = new List<SelectListItem>();
            
            return View();
        }

        public ActionResult Filter()
        {
            return RedirectToAction();
        }
    }
}
