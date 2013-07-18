using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.Engine
{
    public class AdvertismentsFilter
    {
        /// <summary>
        /// Show only advertisments with photos
        /// </summary>
        public bool? OnlyWithPhotos { get; set; }
    }
}