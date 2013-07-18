using SiteMVC.Models.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Areas.Controls.ViewModel
{
    public class FilterViewModel
    {
        public FilterViewModel()
        {
            Filter = new AdvertismentsFilter();
        }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public AdvertismentsFilter Filter { get; set; }
    }
}