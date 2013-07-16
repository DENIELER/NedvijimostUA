using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class AuthenticationController : Controller
    {
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ViewModels.User user)
        {
            return View();
        }
    }
}
