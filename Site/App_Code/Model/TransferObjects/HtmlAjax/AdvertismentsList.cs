using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nedvijimost.DataTransferObject.Html
{
    public class AdvertismentsList
    {
        public AdvertismentsList()
        {
        }

        public string Header { get; set; }
        public string Advertisments { get; set; }
        public string Pagging { get; set; }
    }
}