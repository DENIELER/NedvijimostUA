using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseProjectModels
{
    public class Advertisment
    {
        public Advertisment()
        {
            
        }

        public int ID { get; set; }
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public int? SearchResultId { get; set; }

        public string Link { get; set; }
        public string SiteName { get; set; }

        public IList<AdvertismentPhone> Phones { get; set; }

        public void Save()
        {
            var context = new Model.NedvijimostDBEntities();
            var model = new Model.Advertisment();
            
            model.Id = ID;
            model.text = Text;

            model.link = Link;
            model.siteName = SiteName;

            model.createDate = CreateDate;
            model.modifyDate = ModifyDate;

            model.searchresult_id = SearchResultId;

            context.Advertisments.AddObject(model);
            context.SaveChanges();
        }
    }
}