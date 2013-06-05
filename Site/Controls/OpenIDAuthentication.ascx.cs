using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

public partial class Controls_OpenIDAuthentication : System.Web.UI.UserControl
{
    protected string LoginName = string.Empty;
    protected string UserUid = string.Empty;
    protected string Photo = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Authorization.Authorization.IsUserVkontakteAuthorized())
        {
            LoginName = Authorization.Authorization.GetVkontakteUserName();
            Photo = Authorization.Authorization.GetVkontakteUserSmallPhoto();
            UserUid = Authorization.Authorization.GetVkontakteUserUid();
        }
    }
}