using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels.Advertisments
{
    public class AdvertismentsPageViewModel
    {
        /// <summary>
        /// Can be set title of page
        /// </summary>
        public string SectionTitle { get; set; }
        /// <summary>
        /// Page desciption
        /// </summary>
        public string SectionDescription { get; set; }
        /// <summary>
        /// Page Keywords
        /// </summary>
        public string SectionKeywords { get; set; }

        /// <summary>
        /// Request to advertisments list
        /// </summary>
        public Models.Engine.AdvertismentsRequest Request { get; set; }
    }
}