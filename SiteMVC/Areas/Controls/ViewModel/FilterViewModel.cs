using SiteMVC.Models.Engine;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace SiteMVC.Areas.Controls.ViewModel
{
    public class FilterViewModel
    {
        public FilterViewModel()
        {
            Filter = new AdvertismentsFilter();
        }

        #region Redirect parameters
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public RouteValueDictionary RouteData { get; set; }
        #endregion Redirect parameters

        public AdvertismentsFilter Filter { get; set; }

        internal List<System.Web.Mvc.SelectListItem> GetRoomsCountCategoriesList()
        {
            var list = new List<System.Web.Mvc.SelectListItem>();
            list.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = "1 комн.", Value = ((int)RoomsCountCategory.OneRoom).ToString() });
            list.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = "2 комн.", Value = ((int)RoomsCountCategory.TwoRooms).ToString() });
            list.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = "3 комн.", Value = ((int)RoomsCountCategory.ThreeRooms).ToString() });
            list.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = "4+ комн.", Value = ((int)RoomsCountCategory.FourOrMoreRooms).ToString() });

            return list;
        }
    }
}