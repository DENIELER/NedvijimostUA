using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseProjectModels
{
    public class AdvertismentPhone
    {
        public AdvertismentPhone()
        {
            
        }

        public int ID { get; set; }
        public string Phone { get; set; }
        public int AdvertismentId { get; set; }

        public void Save()
        {
            var context = new Model.NedvijimostDBEntities();
            var model = new Model.AdvertismentPhone();

            model.Id = ID;
            model.phone = Phone;

            model.AdvertismentId = AdvertismentId;
            
            context.AdvertismentPhones.AddObject(model);
            context.SaveChanges();
        }
    }
}