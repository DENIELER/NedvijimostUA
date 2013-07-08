using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.ModelInterlayerObjects
{
    public class Phone
    {
        //private Model.AdvertismentPhone p;

        public Phone(Models.AdvertismentPhone entity)
        {
            //this.p = p;

            this.Id = entity.Id;
            this.phone = entity.phone;
        }

        public int Id { get; set; }
        public string phone { get; set; }
    }
}