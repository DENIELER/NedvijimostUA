using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using System.Web.Script.Serialization;

public partial class Auth_Vkontakte_VkontakteMakeAuth : System.Web.UI.Page
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
        VkontakteAccessToken accessTokenInfo = GetAccessTokenInfo(code);
        VkontakteUserProfile userProfile = GetUserInfo(accessTokenInfo.user_id, accessTokenInfo.access_token);
        Response.Write("FirtName:" + userProfile.response[0].first_name);
        Response.Write("Sex:" + userProfile.response[0].sex);
        Response.Write("City:" + userProfile.response[0].city);
        Response.Write("Photo:" + userProfile.response[0].photo);
    }

    VkontakteAccessToken GetAccessTokenInfo(string code)
    {
        string getAccessCodeUrl =
                string.Format("https://api.vkontakte.ru/oauth/access_token?client_id={0}&client_secret={1}&code={2}",
                              "2423632",//Resources.OAuth.Vkontakte_App_ID,
                              "31ovmoo1klYYzhXIfyVL",//Resources.OAuth.Vkontakte_App_SecretKey,
                              code);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getAccessCodeUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Encoding enc = Encoding.GetEncoding("utf-8");
        StreamReader configStream = new StreamReader(response.GetResponseStream(), enc);
        string accessTokenJson = configStream.ReadToEnd();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        VkontakteAccessToken accessToken = serializer.Deserialize<VkontakteAccessToken>(accessTokenJson);

        return accessToken;
    }

    VkontakteUserProfile GetUserInfo(string user_id, string access_token)
    {
        string getUserProfileUrl =
                string.Format("https://api.vkontakte.ru/method/getProfiles?uid={0}&fields={1}&access_token={2}",
                              user_id,
                              "first_name,last_name,nickname,sex,bdate,city,country,photo",
                              access_token);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUserProfileUrl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Encoding enc = Encoding.GetEncoding("utf-8");
        StreamReader configStream = new StreamReader(response.GetResponseStream(), enc);
        string profileJson = configStream.ReadToEnd();
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        VkontakteUserProfile userProfile = serializer.Deserialize<VkontakteUserProfile>(profileJson);

        return userProfile;

    }
}

internal class VkontakteAccessToken
{
    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string user_id { get; set; }
}

internal class VkontakteUserProfile
{
    public VkontakteUserProfile()
    {
        response = new List<VkontakteUserProfileResponse>();
    }

    public List<VkontakteUserProfileResponse> response { get; set; }
}

internal class VkontakteUserProfileResponse
{
    public int uid { get; set; }

    public string first_name { get; set; }
    public string last_name { get; set; }
    public string nickname { get; set; }

    public string sex { get; set; }
    public string bdate { get; set; }
    public string city { get; set; }
    public string country { get; set; }

    public string photo { get; set; }
}
