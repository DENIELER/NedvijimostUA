using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class ArchiveController : Controller
    {

        public ActionResult Sections()
        {
            return View();
        }

        public ActionResult Section(string section, string subpurchaseMode, [System.Web.Http.FromUri] Models.Engine.AdvertismentsFilter advertismentsFilter)
        {
            string _sectionName;
            int _sectionID;
            int? _subSectionID;

            string _date = Request["date"];

            #region Section switch
            switch (section)
            {
                case "Sdam-kvartiru":
                    _sectionName = "Сдам квартиру";
                    _sectionID = 1;
                    _subSectionID = 1;
                    break;
                case "Snimu-kvartiru":
                    _sectionName = "Сниму квартиру";
                    _sectionID = 1;
                    _subSectionID = 2;
                    break;
                case "Prodam-kvartiru":
                    _sectionName = "Продам квартиру";
                    _sectionID = 3;
                    _subSectionID = 3;
                    break;
                case "Kuplu-kvartiru":
                    _sectionName = "Куплю квартиру";
                    _sectionID = 3;
                    _subSectionID = 4;
                    break;
                case "Obyavleniya-Doma-Dachi":
                    _sectionName = "Дома, дачи";
                    _sectionID = 6;
                    _subSectionID = null;
                    break;
                case "Arenda-ofisov":
                    _sectionName = "Аренда офисов";
                    _sectionID = 4;
                    _subSectionID = null;
                    break;
                case "Prodam-kommercheskuu-nedvijimost":
                    _sectionName = "Продажа коммерческой недвижимости";
                    _sectionID = 5;
                    _subSectionID = null;
                    break;
                default:
                    _sectionName = "Сдам квартиру";
                    _sectionID = 1;
                    _subSectionID = 1;
                    break;
            }
            #endregion Section switch

            var advertisments = new SiteMVC.Models.Engine.AdvertismentsRequest()
            {
                SectionId = _sectionID,
                SubSectionId = _subSectionID,

                SectionName = _sectionName,
                Date = _date
            };
            advertisments.Filter = advertismentsFilter;

            return View(advertisments);
        }

    }
}
