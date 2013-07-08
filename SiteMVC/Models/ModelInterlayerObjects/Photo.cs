using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.ModelInterlayerObjects
{
    public class Photo
    {
        public Photo(Models.AdvertismentsPhoto entity)
        {
            this.Id = entity.Id;
            this.filename = entity.filename;
        }

        public int Id { get; set; }
        public string filename { get; set; }

        public string filenameFormated
        {
            get
            {
                if (filename.Contains("thumb=1&"))
                    return filename.Replace("thumb=1&", "");
                else return filename;
            }
        }
    }
}