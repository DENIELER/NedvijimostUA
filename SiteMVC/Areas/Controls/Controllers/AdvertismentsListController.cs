using SiteMVC.App_Code;
using SiteMVC.Models;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Areas.Controls.Controllers
{
    public class AdvertismentsListController : Controller
    {
        public ActionResult AdvertismentsList(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            AdvertismentsList advertisments;

            if (!string.IsNullOrEmpty(Request["page"]))
            {
                int currentPage;
                if (!int.TryParse(Request["page"], out currentPage) || currentPage < 1)
                    currentPage = 1;

                request.Offset = (currentPage - 1) * request.Limit;
            }

            var advertismentsLoader = new AdvertismentsLoader();
            if (request.Date == null)
            {
                advertismentsLoader.SetTodayDate(request);

                advertisments = advertismentsLoader.LoadAdversitments(request);
                if (!advertismentsLoader.IsLoaded(advertisments))
                {
                    advertismentsLoader.SetYesterdayDate(request);
                    advertisments = advertismentsLoader.LoadAdversitments(request);
                }
            }
            else
                advertisments = advertismentsLoader.LoadAdversitments(request);

            advertisments.Offset = request.Offset;
            advertisments.Limit = request.Limit;
            
            return PartialView(advertisments);
        }

        #region Ajax Action handlers for advertisments
        [HttpPost]
        public JsonResult AdminRemoveAdvertisment(int advertismentID)
        {
            if (!SystemUtils.Authorization.IsAdmin)
                return Json("Access denied.");

            var dataModel = new DataModel();

            var advertisment = dataModel.Advertisments
                .SingleOrDefault(a => a.Id == advertismentID);

            if (advertisment == null)
                throw new Exception("Advertisment not founded to remove");

            advertisment.not_show_advertisment = true;

            dataModel.SubmitChanges();

            return Json("success");
        }

        [HttpPost]
        public JsonResult NotifySubpurchaseAdvertisment(int advertismentID)
        {
            var dataModel = new DataModel();

            var advertismentPhones = dataModel.AdvertismentPhones
                .Where(ap => ap.AdvertismentId == advertismentID);

            bool isExists = true;
            foreach (var advertismentPhone in advertismentPhones)
            {
                if (!dataModel.SubPurchasePhones
                    .Any(s => s.phone == advertismentPhone.phone))
                {
                    isExists = false;
                    break;
                }
            }

            if (!isExists)
            {
                Guid subpurchaseID = Guid.NewGuid();
                var subpurchase = new SubPurchase()
                {
                    id = subpurchaseID,
                    createDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                    modifyDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                    not_checked = true
                };
                dataModel.SubPurchases.InsertOnSubmit(subpurchase);

                foreach (var advertismentPhone in advertismentPhones)
                {
                    var subpurchasePhone = new SubPurchasePhone()
                    {
                        Id = Guid.NewGuid(),
                        createDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                        phone = advertismentPhone.phone,
                        SubPurchaseId = subpurchaseID
                    };
                    dataModel.SubPurchasePhones.InsertOnSubmit(subpurchasePhone);
                }

                dataModel.SubmitChanges();
            }
            else
            {
                if (SystemUtils.Authorization.IsAdmin)
                {
                    var advertisment = dataModel.Advertisments
                                       .SingleOrDefault(a => a.Id == advertismentID);
                    if (advertisment != null)
                    {
                        advertisment.not_show_advertisment = true;
                        dataModel.SubmitChanges();
                    }
                    return Json("Already exists in db");
                }
            }

            return Json("success");
        }

        [HttpPost]
        public JsonResult NotifyNotByThemeAdvertisment(int advertismentID)
        {
            var dataModel = new DataModel();

            var advertisment = dataModel.Advertisments
                .SingleOrDefault(a => a.Id == advertismentID);

            if (advertisment == null)
                throw new Exception("Advertisment not founded to remove");

            try
            {
                if (SystemUtils.Authorization.IsAdmin)
                {
                    advertisment.not_realestate = true;
                    dataModel.SubmitChanges();
                }
                else
                {
                    var emailWorkflow = new SystemUtils.Email();
                    emailWorkflow.SendMail("danielostapenko@gmail.com", "Отмечено новое объявление",
                        string.Format(
    @"Отмечено объявление меткой ""Не по теме"".
Текст объявления: {0},
Номер объявления: {1}
Линк подтверждения: {2}",
                            advertisment.text,
                            advertisment.Id,
                            "http://nedvijimost-ua.com/WebServices/AdminService.svc/RemoveAdvertisment/"+ advertisment.Id + "/gtycbz"));
                }

                return Json("success");
            }
            catch
            {
                return Json("failed");
            }
        }
        #endregion Ajax Action handlers for advertisments
    }
}
