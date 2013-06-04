using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auth : System.Web.UI.Page
{
    public string VkontakteHref = string.Empty;
    public string FacebookHref = string.Empty;
    public string TwitterHref = string.Empty;
    public string GoogleHref = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //vkontakte
        string redirectVkontakteUrl = Request.Url.Scheme + "://" + Request.Url.Host + @"/Auth/Vkontakte/VkontakteMakeAuth.aspx";
        VkontakteHref = string.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}&response_type=code",
            "2423632",//Resources.OAuth.Vkontakte_App_ID,
            "notify,friends,offers,wall",
            redirectVkontakteUrl);

        //facebook
        string redirectFacebookUrl = Request.Url.Scheme + "://" + Request.Url.Host + @"/Auth/Facebook/FacebookMakeAuth.aspx";
        FacebookHref = string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}",
            "253985454612657",//Resources.OAuth.Facebook_App_ID,
            redirectFacebookUrl);

        //twitter
        string redirectTwitterUrl = Request.Url.Scheme + "://" + Request.Url.Host + @"/Auth/Twitter/TwitterMakeAuth.aspx";
        TwitterHref = redirectTwitterUrl;

        //google
        string redirectGoogleUrl = Request.Url.Scheme + "://" + Request.Url.Host + @"/Auth/Google/GoogleMakeAuth.aspx";
        GoogleHref = string.Format("https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri={1}&scope=https://www.googleapis.com/auth/userinfo.profile&response_type=code",
            "946891151573.apps.googleusercontent.com", //Resources.OAuth.Google_App_ID,
            redirectGoogleUrl);
    }
}