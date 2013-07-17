using SiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult AddSubPurchases()
        {
            if (!SystemUtils.Authorization.IsAdmin)
                return RedirectToAction("NotAdminUser", "Authentication");

            return View();
        }

        [HttpPost]
        public ActionResult AddSubpurchaseAndPhones(SiteMVC.ViewModels.SubPurchase subpurchase)
        {
            if (!SystemUtils.Authorization.IsAdmin)
                return RedirectToAction("NotAdminUser", "Authentication");

            var dataModel = new DataModel();

            Guid subpurchaseID = Guid.NewGuid();
            var dbSubpurchase = new SubPurchase()
            {
                id = subpurchaseID,
                createDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                modifyDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                name = subpurchase.FirstName,
                surname = subpurchase.LastName,
                phone = null,
                not_checked = false
            };
            dataModel.SubPurchases.InsertOnSubmit(dbSubpurchase);

            if (subpurchase.Phones.Any())
            {
                var phonesList = new List<string>();
                foreach (var phone in subpurchase.Phones)
                {
                    var tempList = dbSubpurchase.GetPhoneFormatsList(phone);
                    phonesList.AddRange(
                        tempList.Where(p => !phonesList.Contains(p))
                    );
                }

                foreach (var phone in phonesList)
                {
                    var dbSubpurchasePhone = new SubPurchasePhone()
                    {
                        Id = Guid.NewGuid(),
                        phone = phone,
                        createDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                        SubPurchaseId = subpurchaseID
                    };
                    dataModel.SubPurchasePhones.InsertOnSubmit(dbSubpurchasePhone);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Не найдено ни одного телефона для добавления.";
                return PartialView("~/Views/Controls/FormSubmitSaveFailed.cshtml");
            }

            dataModel.SubmitChanges();
            return PartialView("~/Views/Controls/FormSubmitSaveSuccess.cshtml");
        }

        public ActionResult CheckSubPurchases()
        {
            if (!SystemUtils.Authorization.IsAdmin)
                return RedirectToAction("NotAdminUser", "Authentication");

            var dataModel = new DataModel();

            var notCheckedSubpurchasePhones = dataModel.SubPurchases
                .GroupJoin(
                    dataModel.SubPurchasePhones,
                    sp => sp.id,
                    spp => spp.SubPurchaseId,
                    (sp, spp) => new { Subpurchase = sp, Phones = spp }
                )
                .Where(x => !x.Subpurchase.not_checked.HasValue || x.Subpurchase.not_checked.Value)
                .SelectMany(x => x.Phones);

            return View(notCheckedSubpurchasePhones);
        }

        [HttpPost]
        public ActionResult SetSubpurchaseChecked(Guid subpurchaseID, string phone)
        {
            if (!SystemUtils.Authorization.IsAdmin)
                return RedirectToAction("NotAdminUser", "Authentication");

            var dataModel = new DataModel();

            var subpurchase = dataModel.SubPurchases
                .SingleOrDefault(s => s.id == subpurchaseID);

            if (subpurchase == null)
            {
                ViewBag.ErrorMessage = "Посредник не найден по текущему ID.";
                return PartialView("~/Views/Controls/FormSubmitSaveFailed.cshtml");
            }

            subpurchase.not_checked = false;

            var subpurchasePhones = dataModel.SubPurchasePhones
                .Where(sp => sp.SubPurchaseId == subpurchaseID)
                .Select(sp => sp.phone);
            if (subpurchasePhones.Any())
            {
                var phonesList = new List<string>();
                foreach (var _phone in subpurchasePhones)
                {
                    var tempList = subpurchase.GetPhoneFormatsList(_phone);
                    phonesList.AddRange(
                        tempList.Where(p => !phonesList.Contains(p) && !subpurchasePhones.Contains(p))
                    );
                }

                foreach (var _phone in phonesList)
                {
                    var dbSubpurchasePhone = new SubPurchasePhone()
                    {
                        Id = Guid.NewGuid(),
                        phone = _phone,
                        createDate = SystemUtils.Utils.Date.GetUkranianDateTimeNow(),
                        SubPurchaseId = subpurchaseID
                    };
                    dataModel.SubPurchasePhones.InsertOnSubmit(dbSubpurchasePhone);
                }
            }

            dataModel.SubmitChanges();

            //--- remove advertisments
            var subpurchaseAdvertisments = dataModel.Advertisments
                .GroupJoin(
                    dataModel.AdvertismentPhones,
                    a => a.Id,
                    ap => ap.AdvertismentId,
                    (a, ap) => new { Advertisment = a, Phones = ap }
                )
                .Where(x => x.Phones.Any(p => p.phone == phone))
                .Select(x => x.Advertisment);

            foreach(var advertisment in subpurchaseAdvertisments)
            {
                advertisment.SubPurchase_Id = subpurchaseID;
                advertisment.subpurchaseAdvertisment = true;
            }

            dataModel.SubmitChanges();

            return PartialView("~/Views/Controls/FormSubmitSaveSuccess.cshtml");
        }

        [HttpPost]
        public ActionResult SetSubpurchaseNotChecked(Guid subpurchaseID)
        {
            if (!SystemUtils.Authorization.IsAdmin)
                return RedirectToAction("NotAdminUser", "Authentication");

            var dataModel = new DataModel();

            var subpurchasePhones = dataModel.SubPurchasePhones
                .Where(sp => sp.SubPurchaseId == subpurchaseID);
            dataModel.SubPurchasePhones.DeleteAllOnSubmit(subpurchasePhones);

            var subpurchase = dataModel.SubPurchases
                .SingleOrDefault(s => s.id == subpurchaseID);
            if (subpurchase == null)
            {
                ViewBag.ErrorMessage = "Посредник не найден по текущему ID.";
                return PartialView("~/Views/Controls/FormSubmitSaveFailed.cshtml");
            }
            dataModel.SubPurchases.DeleteOnSubmit(subpurchase);

            dataModel.SubmitChanges();
            return PartialView("~/Views/Controls/FormSubmitSaveSuccess.cshtml");
        }

        [HttpPost]
        public JsonResult GetAdvertismentsBySubpurchase(Guid subpurchaseID, string phone)
        {
            var dataModel = new DataModel();

            var subpurchaseAdvertismentsOld = dataModel.Advertisments
                .Where(a => a.SubPurchase_Id == subpurchaseID)
                .Select(a => new { a.text, state = "old", date = a.createDate });

            var subpurchaseAdvertismentsNew = dataModel.Advertisments
                .GroupJoin(
                    dataModel.AdvertismentPhones,
                    a => a.Id,
                    ap => ap.AdvertismentId,
                    (a, ap) => new { Advertisment = a, Phones = ap}
                )
                .Where(x => x.Phones.Any(p => p.phone == phone))
                .Select(a => new { a.Advertisment.text, state = "new", date = a.Advertisment.createDate });

            var subpurchaseAdvertisments = subpurchaseAdvertismentsOld
                .Union(subpurchaseAdvertismentsNew)
                .OrderBy(x => x.state)
                .OrderByDescending(x => x.date);
            if (subpurchaseAdvertisments.Any())
                return Json(subpurchaseAdvertisments);
            else return Json(new { text = "Нет объявлений", state = "new", date = "none" });
        }
    }
}
