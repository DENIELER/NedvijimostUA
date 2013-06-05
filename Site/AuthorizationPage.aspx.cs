using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AuthorizationPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.Form["login"]))
        {
            string login = Request.Form["login"];
            string password = Request.Form["password"];
            bool rememberMe = false;
            bool.TryParse(Request.Form["rememberMe"], out rememberMe);

            Authorization.Authorization.Login(login, password, rememberMe);
        }
        else
        {
            Authorization.Authorization.Logout();
        }
    }
}