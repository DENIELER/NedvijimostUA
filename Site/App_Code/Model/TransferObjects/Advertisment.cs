using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nedvijimost.DataTransferObject
{
    public class Advertisment
    {
        //private Model.Advertisment x;

        public Advertisment(Model.Advertisment entity)
        {
            //this.x = x;

            this.Id = entity.Id;
            this.text = entity.text;
            this.createDate = entity.createDate;
            this.modifyDate = entity.modifyDate;
            this.searchresult_id = entity.searchresult_id;
            this.link = entity.link;
            this.siteName = entity.siteName;
            this.isSpecial = entity.isSpecial;

            this.Phones = entity.AdvertismentPhones.Select(p => new Phone(p)).ToList();
            this.Photos = entity.AdvertismentsPhotos.Select(p => new Photo(p)).ToList();
        }

        public int Id { get; set; }
        public string text { get; set; }
        public DateTime createDate { get; set; }
        public string createDateFormated 
        {
            get
            {
                return createDate.ToString("g");
            }
        }
        public DateTime modifyDate { get; set; }
        public int? searchresult_id { get; set; }
        public string link { get; set; }
        public string siteName { get; set; }
        public bool isSpecial { get; set; }

        public List<Phone> Phones { get; set; }
        public List<Photo> Photos { get; set; }
    }
}