using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Entities
{
    public class Advertisment
    {
        public string Text { get; set; }
        public List<string> Phones { get; set; }
        public List<string> PhotoUrls { get; set; }

        public string Link { get; set; }
        public string SiteName { get; set; }

        public int? SectionID { get; set; }
        public int? SubSectionID { get; set; }

        public decimal? Price { get; set; }
        public string Address1 { get; set; }

        public Advertisment()
        {

        }
    }
}