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
            var dataModel = new Models.DataModel();

            try
            {
                var dbUser = new Models.User()
                {
                    Email = user.Email,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    IsAdmin = false,
                    IsSubPurchase = user.IsSubPurchase,
                    Login = user.Login,
                    Password = SystemUtils.Utils.CalculateMD5Hash(user.Password),
                    Phone = user.Phone,
                    SubPurchaseID = null,
                    VkontakteID = null
                };

                dataModel.Users.InsertOnSubmit(dbUser);
                dataModel.SubmitChanges();

                return RedirectToAction("SuccessRegistration");
            }
            catch
            {
                return RedirectToAction("FailedRegistration");
            }
        }

        public ActionResult SuccessRegistration()
        {
            return View();
        }

        public ActionResult FailedRegistration()
        {
            return View();
        }
    }
}
