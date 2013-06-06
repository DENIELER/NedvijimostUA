using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyAdvertisments : System.Web.UI.Page
{
    protected int? editAdvertismentID = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string editAdvIDText = Request["edit"];
        if(!string.IsNullOrEmpty(editAdvIDText))
        {
            int editAdvID;
            if (int.TryParse(editAdvIDText, out editAdvID))
            {
                var workflow = new AdvertismentsWorkflow();
                if (workflow.ExistsAdvertisment(editAdvID, Authorization.Authorization.CurrentUser_UserID()))
                    editAdvertismentID = editAdvID;
            }
        }

        if (editAdvertismentID == null)
        {
            ldsAdvertisments.WhereParameters["UserID"].DefaultValue = Authorization.Authorization.CurrentUser_UserID().ToString();
        }
    }

    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        int advertismentID;
        if (!int.TryParse(e.CommandArgument.ToString(), out advertismentID))
            return;

        var workflow = new AdvertismentsWorkflow();
        workflow.HideAdvertisment(advertismentID);

        Response.Redirect(Request.RawUrl);
    }
}