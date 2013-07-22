using SiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Registration
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
        #endregion Registration

        public ActionResult NotAuthorizedUser()
        {
            return View();
        }
        public ActionResult NotAdminUser()
        {
            return View();
        }

        #region User Actions
        public ActionResult UserOptions()
        {
            if (!SystemUtils.Authorization.IsAuthorized)
                return RedirectToAction("NotAuthorizedUser");

            var dataModel = new DataModel();
            var user = dataModel.Users
                .FirstOrDefault(u => u.UserID == SystemUtils.Authorization.UserID);

            return View(user);
        }
        [HttpPost]
        public ActionResult SaveUserOptions(User user)
        {
            try
            {
                var dataModel = new DataModel();

                var dbUser = dataModel.Users
                    .FirstOrDefault(u => u.UserID == SystemUtils.Authorization.UserID);

                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Phone = user.Phone;

                dataModel.SubmitChanges();

                return PartialView("~/Views/Controls/FormSubmitSaveSuccess.cshtml");
            }
            catch
            {
                return PartialView("~/Views/Controls/FormSubmitSaveFailed.cshtml");
            }
        }

        public ActionResult UserAdvertisments()
        {
            var currentUserID = SystemUtils.Authorization.UserID;

            var dataModel = new DataModel();

            var advertisments = dataModel.Advertisments
                .Where(a => a.UserID == currentUserID
                            && !a.not_show_advertisment);

            return View(advertisments);
        }
        #endregion User Actions
    }
}
