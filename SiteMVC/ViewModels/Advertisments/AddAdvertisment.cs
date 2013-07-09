using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels.Advertisments
{
    public class AddAdvertisment
    {
        public IEnumerable<Models.AdvertismentSection> Sections { get; set; }
        public IEnumerable<Models.AdvertismentSubSection> SubSections { get; set; }
    }
}