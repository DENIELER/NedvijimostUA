using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels
{
    public class Article
    {
        public Article(Models.Article a)
        {
            this.CreateDate = a.createDate;

            this.Link = a.link;
            this.Header = a.header;
            this.Description = a.description;

            this.Keywords = a.keywords;

            this.Text = a.text;
        }

        public DateTime CreateDate { get; set; }

        public string Link { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }

        public string HeaderWithoutTags
        {
            get
            {
                return SystemUtils.Utils.StripTagsRegex(Header);
            }
        }

        public string Keywords { get; set; }
    }
}