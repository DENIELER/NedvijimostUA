using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

using DTO = Nedvijimost.DataTransferObject;

public partial class SaleCommercialAdvertisments : System.Web.UI.Page
{
    #region Variables

    protected const string SubSectionName = "Продажа коммерческой недвижимости";

    protected const int SectionId = 5; //-- sale_commercial section
    
    protected AdvertismentsState AdvertismentsMode { get; set; }

    private const string ROUTE_SALECOMMERCIAL_SECTION = "Route_SaleCommercialSection";
    private const string SUBPURCHASE_MODE = "SubpurchaseMode";
    private const string BEZ_POSREDNIKOV = "Bez-posrednikov";
    private const string VKLUCHAYA_POSREDNIKOV = "Vkluchaya-posrednikov";

    #endregion Variables

    protected void Page_Load(object sender, EventArgs e)
    {
        CapturePageRequestParameters();

        AdvertismentsViewControl.Settings.SectionId = SectionId;
        AdvertismentsViewControl.Settings.SubSectionId = null;
        AdvertismentsViewControl.Settings.State = AdvertismentsMode;
        AdvertismentsViewControl.Settings.Limit = Constants.Max_Advertisments_View_Count;
    }
    private void CapturePageRequestParameters()
    {
        string advertismentsMode = null;
        if (Page.RouteData.Values[SUBPURCHASE_MODE] != null)
            advertismentsMode = Page.RouteData.Values[SUBPURCHASE_MODE].ToString();
        switch (advertismentsMode)
        {
            case VKLUCHAYA_POSREDNIKOV:
                AdvertismentsMode = AdvertismentsState.SubpurchaseWithNotSubpurchase;
                lnkWithSubpurchases.InnerText = Resources.Resource.WithSubpurchasesLinkText;
                lnkWithSubpurchases.HRef = Page.GetRouteUrl(ROUTE_SALECOMMERCIAL_SECTION, new System.Web.Routing.RouteValueDictionary()
                {
                    {SUBPURCHASE_MODE, BEZ_POSREDNIKOV}
                });
                //AdvertismentsView.NotShowSubpurchaseCheckbox = true;
                break;
            case BEZ_POSREDNIKOV:
            default:
                AdvertismentsMode = AdvertismentsState.NotSubpurchase;
                lnkWithSubpurchases.InnerText = Resources.Resource.WithoutSubpurchasesLinkText;
                lnkWithSubpurchases.HRef = Page.GetRouteUrl(ROUTE_SALECOMMERCIAL_SECTION, new System.Web.Routing.RouteValueDictionary()
                {
                    {SUBPURCHASE_MODE, VKLUCHAYA_POSREDNIKOV}
                });
                //AdvertismentsView.NotShowSubpurchaseCheckbox = false;
                break;
        }
    }
}