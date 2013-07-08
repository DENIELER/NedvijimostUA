using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.UI.Controls
{
    public class Authorization
    {
        public bool IsAuthorized { get; set; }
        public bool IsAdmin { get; set; }

        public string Login { get; set; }

        private string _phone;
        public string Phone 
        {
            get { return _phone ?? "не указан"; }
            set { _phone = value; }
        }

        public int AdvertismentsCount { get; set; }

        public int? UserID { get; set; }
    }
}