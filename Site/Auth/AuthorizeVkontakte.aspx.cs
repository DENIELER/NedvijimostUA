using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auth_AuthorizeVkontakte : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["action"] == "AuthorizationVkontakte")
        {
            string uid = Request["uid"];
            string first_name = Server.UrlDecode(Request["first_name"]);
            string last_name = Server.UrlDecode(Request["last_name"]);
            string photo = Request["photo"];
            string photo_rec = Request["photo_rec"];
            string hash = Request["hash"];

            if (Request["hash"] == CalculateMD5Hash("3111027" + uid + "FSApuxGl1jU3CpgG1Fra"))
            {
                var authorization = new Authorization();
                if (!authorization.AuthorizeVkontakteUser(uid, first_name, last_name, photo, photo_rec, hash))
                    Response.Redirect("AuthorizationVkontakteFailed.aspx", true);
            }
            else
            {
                Response.Redirect("AuthorizationVkontakteFailed.aspx", true);
            }

            string strRedirect;
            strRedirect = Request["ReturnUrl"];
            if (strRedirect == null)
                strRedirect = "/";
            Response.Redirect(strRedirect, true);
        }
        else
        {
            Response.Redirect("AuthorizationVkontakteFailed.aspx", true);
        }
    }

    protected string CalculateMD5Hash(string input)
    {
        // step 1, calculate MD5 hash from input
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
 
        // step 2, convert byte array to hex string
        var sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString().ToLower();
    }

    [WebMethod(EnableSession=true)]
    public static bool LogOff()
    {
        var authorization = new Authorization();
        authorization.LogoffVkontakteUser();
        return true;
    }
}