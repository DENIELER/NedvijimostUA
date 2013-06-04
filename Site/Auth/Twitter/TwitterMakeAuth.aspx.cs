using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twitterizer;

public partial class Auth_Twitter_TwitterMakeAuth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string consumerKey = "lGHwH8UWgDc4O98M3ocGYg"; //Resources.OAuth.Twitter_App_ID;
        string consumerSecret = "Ha2c5PNlKS9N9Vh04lXBquERBdhL0kUw0PQcc"; //Resources.OAuth.Twitter_App_SecretKey;

        if (Request["oauth_verifier"] == null)
        {
            string redirectTwitterUrl = Request.Url.Scheme + "://" + Request.Url.Host +
                                        @"/Auth/Twitter/TwitterMakeAuth.aspx";

            // Obtain a request token
            OAuthTokenResponse requestToken = OAuthUtility.GetRequestToken(consumerKey, consumerSecret,
                                                                           redirectTwitterUrl);

            // Direct or instruct the user to the following address:
            Uri authorizationUri = OAuthUtility.BuildAuthorizationUri(requestToken.Token);

            Response.Redirect(authorizationUri.AbsoluteUri);
        }
        else if (Request["oauth_token"] != null && Request["oauth_verifier"] != null)
        {
            string oauth_token = Request["oauth_token"];
            string oauth_verifier = Request["oauth_verifier"];

            OAuthTokenResponse userInfo = OAuthUtility.GetAccessToken(consumerKey, consumerSecret, oauth_token,
                                                                      oauth_verifier);
            string userName = userInfo.ScreenName;

            TwitterResponse<TwitterUser> user = TwitterUser.Show(userName);
            Response.Write(user.ResponseObject.Name);

        }
    }

}
