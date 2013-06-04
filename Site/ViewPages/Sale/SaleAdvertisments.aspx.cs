using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class SaleAdvertisments : System.Web.UI.Page
{
    #region Variables

    protected const string SubSectionName = "Продам квартиру";

    protected const int SectionId = 3; //-- sale section
    protected const int SubSectionId = 3; //-- sale sub section

    protected AdvertismentsState AdvertismentsMode { get; set; }

    private const string ROUTE_SALE_SALE_SUBSECTION = "Route_Sale_SaleSubSection";
    private const string SUBPURCHASE_MODE = "SubpurchaseMode";
    private const string BEZ_POSREDNIKOV = "Bez-posrednikov";
    private const string VKLUCHAYA_POSREDNIKOV = "Vkluchaya-posrednikov";

    #endregion Variables

    protected void Page_Load(object sender, EventArgs e)
    {
        CapturePageRequestParameters();

        AdvertismentsViewControl.Settings.SectionId = SectionId;
        AdvertismentsViewControl.Settings.SubSectionId = SubSectionId;
        AdvertismentsViewControl.Settings.Filter = AdvertismentsMode;
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
                lnkWithSubpurchases.HRef = Page.GetRouteUrl(ROUTE_SALE_SALE_SUBSECTION, new System.Web.Routing.RouteValueDictionary()
                {
                    {SUBPURCHASE_MODE, BEZ_POSREDNIKOV}
                });
                //AdvertismentsView.NotShowSubpurchaseCheckbox = true;
                break;
            case BEZ_POSREDNIKOV:
            default:
                AdvertismentsMode = AdvertismentsState.NotSubpurchase;
                lnkWithSubpurchases.InnerText = Resources.Resource.WithoutSubpurchasesLinkText;
                lnkWithSubpurchases.HRef = Page.GetRouteUrl(ROUTE_SALE_SALE_SUBSECTION, new System.Web.Routing.RouteValueDictionary()
                {
                    {SUBPURCHASE_MODE, VKLUCHAYA_POSREDNIKOV}
                });
                //AdvertismentsView.NotShowSubpurchaseCheckbox = false;
                break;
        }
    }
}