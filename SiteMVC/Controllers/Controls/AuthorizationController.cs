using SiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers.Controls
{
    public class AuthorizationController : Controller
    {
        public ActionResult AuthorizationControl()
        {
            var dataModel = new DataModel();
            var authorization = new ViewModels.Controls.Authorization();
            authorization.IsAuthorized = SystemUtils.Authorization.IsAuthorized;
            if (authorization.IsAuthorized)
            {
                authorization.Login = SystemUtils.Authorization.Login;
                authorization.Phone = SystemUtils.Authorization.Phone;

                authorization.UserID = SystemUtils.Authorization.UserID;
                if (authorization.UserID != null)
                    authorization.AdvertismentsCount = dataModel.Advertisments
                                                       .Count(a => a.UserID == authorization.UserID);

                authorization.IsAdmin = SystemUtils.Authorization.IsAdmin;
            }

            return PartialView("~/Views/Controls/AuthorizationControl.cshtml", authorization);
        }

        public ActionResult LogIn(string Login, string Password, bool RememberMe)
        {
            if (SystemUtils.Authorization.LogIn(Login, Password, RememberMe))
            {
                string returnUrl = Request["ReturnUrl"];

                if (returnUrl == null) returnUrl = "~/";
                return Redirect(returnUrl);
            }

            return Redirect("~/Failed-authorization");
        }
        public ActionResult LogOut()
        {
            SystemUtils.Authorization.LogOut();
            return Redirect("~/");
        }

        public ActionResult FailedAuthorization()
        {
            return View();
        }

    }
}
