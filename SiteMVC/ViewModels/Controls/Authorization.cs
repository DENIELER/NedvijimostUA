using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels.Controls
{
    public class Authorization
    {
        public bool IsAuthorized { get; set; }
        public bool IsAdmin { get; set; }

        public string Login { get; set; }
        public string LoginDisplay 
        {
            get
            {
                if (this.Login.Length > 19)
                    return this.Login.Substring(0, 17) + "..";
                else return this.Login;
            }
        }
        
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