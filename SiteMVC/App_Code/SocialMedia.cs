using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.App_Code
{
    public abstract class SocialMedia
    {
        protected string AppID { get; set; }
        protected string AppSecret { get; set; }

        protected string AccessToken { get; set; }

        protected virtual string GetAccessToken()
        {
            return null;
        }
    }
}