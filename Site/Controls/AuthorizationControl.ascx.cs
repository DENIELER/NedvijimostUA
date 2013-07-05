using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_AuthorizationControl : System.Web.UI.UserControl
{
    protected int UserAdvertismentsCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        var userID = Authorization.Authorization.CurrentUser_UserID();
        if (Authorization.Authorization.IsUserAuthorized()
            && userID.HasValue)
        {
            var dataModel = new Model.DataModel();
            UserAdvertismentsCount = dataModel.Advertisments.Count(a => a.UserID == userID.Value);
        }
    }
}