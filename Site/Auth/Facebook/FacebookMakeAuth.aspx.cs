using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auth_Facebook_FacebookMakeAuth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["code"] != null)
        {
            string code = Request["code"];

            GetAuthInfo(code);
        }
        else
        {
            if (Request["error"] != null)
            {
                string error = Request["error"];
                Response.Write("Error: " + error);
            }
        }
    }

    private void GetAuthInfo(string code)
    {
        FacebookAccessToken accessTokenInfo = GetAccessTokenInfo(code);

        Response.Write(accessTokenInfo.access_token);
    }

    FacebookAccessToken GetAccessTokenInfo(string code)
    {
        string redirectFacebookUrl = Request.Url.Scheme + "://" + Request.Url.Host + @"/Auth/Facebook/FacebookMakeAuth.aspx";

        string getAccessCodeUrl =
                string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                              "253985454612657",//Resources.OAuth.Facebook_App_ID,
                              redirectFacebookUrl,
                              "9289693e43cb1525f9f0545861d484a8",//Resources.OAuth.Facebook_App_SecretKey,
                              code);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getAccessCodeUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Encoding enc = Encoding.GetEncoding("utf-8");
        StreamReader configStream = new StreamReader(response.GetResponseStream(), enc);
        string accessTokenJson = configStream.ReadToEnd();

        FacebookAccessToken accessTokenInfo = new FacebookAccessToken();

        string[] splitString = accessTokenJson.Split('&');
        accessTokenInfo.access_token = splitString[0].Replace("access_token=", "");
        accessTokenInfo.expires_in = splitString[1].Replace("expires=", "");

        return accessTokenInfo;
    }
}

internal class FacebookAccessToken
{
    public string access_token { get; set; }
    public string expires_in { get; set; }
}
