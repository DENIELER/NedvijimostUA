using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.Engine
{
    public class AdvertismentsFilter
    {
        public string Text { get; set; }

        /// <summary>
        /// Show only advertisments with photos
        /// </summary>
        public bool OnlyWithPhotos { get; set; }
        public bool NearUndeground { get; set; }

        public RoomsCountCategory Rooms { get; set; }
    }
}