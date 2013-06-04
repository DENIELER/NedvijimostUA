using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class Controls_AdvertismentsWithPhotosRotator : System.Web.UI.UserControl
{
    public NedvijimostDBEntities DBContext
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (DBContext != null)
        {
            rptAdvertisments.DataSource = (from advertismentWithPhoto in DBContext.viewAdvertismentPhotos
                                           where advertismentWithPhoto.subpurchaseAdvertisment == false
                                                && advertismentWithPhoto.SubPurchase_Id == null
                                           orderby advertismentWithPhoto.createDate descending
                                            select advertismentWithPhoto).Take(10).ToList();
            rptAdvertisments.DataBind();
        }
    }

    protected string FormatPhotoFileName(string filename)
    {
        return filename.Replace("&", "&amp;").Replace(" ", "%20");
    }
}