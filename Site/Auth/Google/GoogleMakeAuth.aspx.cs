using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auth_Google_GoogleMakeAuth : System.Web.UI.Page
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
        GoogleAccessToken accessTokenInfo = GetAccessTokenInfo(code);

        GoogleUserProfile userInfo = GetUserInfo(accessTokenInfo.access_token);

        Response.Write(userInfo.given_name + " " + userInfo.family_name);
    }

    GoogleAccessToken GetAccessTokenInfo(string code)
    {
        string redirectGoogleUrl = Request.Url.Scheme + "://" + Request.Url.Host + @"/Auth/Google/GoogleMakeAuth.aspx";

        string getAccessCodeUrl =
                string.Format("https://accounts.google.com/o/oauth2/token");
        
        string data = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                               code,
                               "946891151573.apps.googleusercontent.com", //Resources.OAuth.Google_App_ID,
                               "YLjmACkeCD2LmrJDrYa7EuGT", //Resources.OAuth.Google_App_SecretKey,
                               redirectGoogleUrl
                              );

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getAccessCodeUrl);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        byte[] dataArray = Encoding.UTF8.GetBytes(data);
        request.ContentLength = dataArray.Length;
        Stream dataStream = request.GetRequestStream();
        dataStream.Write(dataArray, 0, dataArray.Length);
        dataStream.Close();
        
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Encoding enc = Encoding.GetEncoding("utf-8");
        StreamReader configStream = new StreamReader(response.GetResponseStream(), enc);
        string accessTokenJson = configStream.ReadToEnd();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        GoogleAccessToken accessToken = serializer.Deserialize <GoogleAccessToken>(accessTokenJson);

        return accessToken;
    }

    GoogleUserProfile GetUserInfo(string access_token)
    {
        string getUserProfileUrl =
            string.Format("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={0}", access_token);
                              //, access_token);
                              //https://www.google.com/m8/feeds/contacts/default/full?access_token={0}", access_token);
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUserProfileUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Encoding enc = Encoding.GetEncoding("utf-8");
        StreamReader configStream = new StreamReader(response.GetResponseStream(), enc);
        string profileJson = configStream.ReadToEnd();
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        GoogleUserProfile userProfile = serializer.Deserialize<GoogleUserProfile>(profileJson);

        return userProfile;
    }
}

internal class GoogleAccessToken
{
    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string refresh_token { get; set; }
}

internal class GoogleUserProfile
{
    public string id { get; set; }
    public string name { get; set; }
    public string given_name { get; set; }
    public string family_name { get; set; }
}