using SiteMVC.Models;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers.Controls
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

            if (request.Date == null)
            {
                SetTodayDate(request);

                advertisments = LoadAdversitments(request);
                if (!IsLoaded(advertisments))
                {
                    SetYesterdayDate(request);
                    advertisments = LoadAdversitments(request);
                }
            }
            else
                advertisments = LoadAdversitments(request);

            advertisments.Offset = request.Offset;
            advertisments.Limit = request.Limit;
            
            return PartialView("~/Views/Controls/AdvertismentsList.cshtml", advertisments);
        }

        #region Advertisments Loading
        private AdvertismentsList LoadAdversitments(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            IQueryable<Models.Advertisment> advertisments = LoadAdvertismentsByDate(request.DateFrom.Value, request.DateTo.Value);
            int advertismentsCount = 0;
            int advertismentsToShowCount = 0;
            if (advertisments != null)
            {
                advertisments = advertisments.Where(adv => adv.AdvertismentSection_Id == request.SectionId);

                if (request.SubSectionId != null)
                    advertisments = advertisments.Where(adv => adv.AdvertismentSubSection_Id == request.SubSectionId.Value);

                advertismentsCount = advertisments.Count();

                switch (request.State)
                {
                    case State.JustParsed:
                        advertisments = advertisments.Where(
                            adv => adv.subpurchaseAdvertisment && adv.SubPurchase_Id == null
                            );
                        break;
                    case State.NotSubpurchase:
                        advertisments = advertisments.Where(
                            adv => !adv.subpurchaseAdvertisment
                            );
                        break;
                    case State.Subpurchase:
                        advertisments = advertisments.Where(
                            adv => adv.subpurchaseAdvertisment && adv.SubPurchase_Id != null
                            );
                        break;
                    case State.SubpurchaseWithNotSubpurchase:
                        advertisments = advertisments.Where(
                            adv => !adv.subpurchaseAdvertisment || adv.SubPurchase_Id != null
                            );
                        break;
                }

                advertismentsToShowCount = advertisments.Count();

                //--- special filters
                if (request.OnlyWithPhotos)
                    advertisments = advertisments.Where(adv => adv.AdvertismentsPhotos.Any());

                advertisments = advertisments.Skip(request.Offset).Take(request.Limit);
            }

            DateTime lastTimeUpdated;
            if (advertisments != null && advertisments.Any())
                lastTimeUpdated = advertisments.Max(adv => adv.createDate);
            else lastTimeUpdated = DateTime.Now;

            return new AdvertismentsList(advertisments, advertismentsCount, advertismentsToShowCount, lastTimeUpdated);
        }

        private IQueryable<Models.Advertisment> LoadAdvertismentsByDate(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var dataModel = new Models.DataModel();

            var specialFromDateTime = dateTimeFrom.Date.AddDays(-7);

            IQueryable<Models.Advertisment> searchResults =
                dataModel.Advertisments
                .Where(a =>
                    ((!a.isSpecial && a.createDate >= dateTimeFrom.Date) || (a.isSpecial && a.createDate >= specialFromDateTime.Date))
                    && a.createDate < dateTimeTo.Date
                    && !a.not_realestate
                    && !a.not_show_advertisment)
                .OrderByDescending(a => a.isSpecial)
                .OrderByDescending(a => a.createDate);
            return searchResults;
        }

        private bool IsLoaded(AdvertismentsList response)
        {
            return response != null
                   && response.Advertisments != null
                   && response.Advertisments.Count > 0
                   && !response.Advertisments.All(a => a.IsSpecial);
        }

        private void SetTodayDate(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            request.DateFrom = SystemUtils.Utils.Date.GetUkranianDateTimeNow().Date;
            request.DateTo = SystemUtils.Utils.Date.GetUkranianDateTimeNow().Date.AddDays(1);
        }

        private void SetYesterdayDate(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            request.DateFrom = SystemUtils.Utils.Date.GetUkranianDateTimeNow().AddDays(-1).Date;
            request.DateTo = SystemUtils.Utils.Date.GetUkranianDateTimeNow().Date;
        }
        #endregion Advertisments Loading

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
                            "http://nedvijimost-ua.com/WebServices/AdvertismentsService.asmx/ForceMarkAsNotByTheme?password=gtycbz1990&advertisment_id=" + advertisment.Id));
                }

                return Json("success");
            }
            catch
            {
                return Json("failed");
            }
        }
    }
}
