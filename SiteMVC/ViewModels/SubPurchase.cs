using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels
{
    public class SubPurchase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhonesByCommas { get; set; }

        public string[] Phones
        {
            get
            {
                return PhonesByCommas.Split(
                    new string[] 
                    { 
                        Environment.NewLine,
                        ","
                    }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}