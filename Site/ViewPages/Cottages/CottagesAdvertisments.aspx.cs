using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class RentAdvertisments : System.Web.UI.Page
{
    #region Variables

    protected const string SubSectionName = "Продам, куплю дом, дачу, участок";

    protected const int SectionId = 6; //-- cottages section
    
    protected AdvertismentsState AdvertismentsMode { get; set; }

    private const string ROUTE_COTTAGESHOUSES_SECTION = "Route_CottagesHousesSection";
    private const string SUBPURCHASE_MODE = "SubpurchaseMode";
    private const string BEZ_POSREDNIKOV = "Bez-posrednikov";
    private const string VKLUCHAYA_POSREDNIKOV = "Vkluchaya-posrednikov";

    #endregion Variables

    protected void Page_Load(object sender, EventArgs e)
    {
        CapturePageRequestParameters();

        AdvertismentsViewControl.Settings.SectionId = SectionId;
        AdvertismentsViewControl.Settings.SubSectionId = null;
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
                lnkWithSubpurchases.HRef = Page.GetRouteUrl(ROUTE_COTTAGESHOUSES_SECTION, new System.Web.Routing.RouteValueDictionary()
                {
                    {SUBPURCHASE_MODE, BEZ_POSREDNIKOV}
                });
                //AdvertismentsView.NotShowSubpurchaseCheckbox = true;
                break;
            case BEZ_POSREDNIKOV:
            default:
                AdvertismentsMode = AdvertismentsState.NotSubpurchase;
                lnkWithSubpurchases.InnerText = Resources.Resource.WithoutSubpurchasesLinkText;
                lnkWithSubpurchases.HRef = Page.GetRouteUrl(ROUTE_COTTAGESHOUSES_SECTION, new System.Web.Routing.RouteValueDictionary()
                {
                    {SUBPURCHASE_MODE, VKLUCHAYA_POSREDNIKOV}
                });
                //AdvertismentsView.NotShowSubpurchaseCheckbox = false;
                break;
        }
    }
}