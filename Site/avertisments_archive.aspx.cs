using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class avertisments_archive : System.Web.UI.Page
{
    protected string AdvSectionName { get; set; }
    private int AdvSectionID;
    private int? AdvSubSectionID;

    private AdvertismentsState AdvertismentsMode { get; set; }

    protected DateTime Date;

    protected void Page_Load(object sender, EventArgs e)
    {
        //--- default
        AdvSectionID = 1;
        AdvSubSectionID = 1;
        AdvertismentsMode = AdvertismentsState.NotSubpurchase;
        //---

        string parameterDate = Request["date"];
        if (!DateTime.TryParse(parameterDate, out Date))
            Date = DateTime.Now;

        string advSection = null;
        if (Page.RouteData.Values["Section"] != null)
            advSection = Page.RouteData.Values["Section"].ToString();
        switch (advSection)
        {
            case "Sdam-kvartiru":
                AdvSectionName = "Сдам квартиру";
                AdvSectionID = 1;
                AdvSubSectionID = 1;
                break;
            case "Snimu-kvartiru":
                AdvSectionName = "Сниму квартиру";
                AdvSectionID = 1;
                AdvSubSectionID = 2;
                break;
            case "Prodam-kvartiru":
                AdvSectionName = "Продам квартиру";
                AdvSectionID = 3;
                AdvSubSectionID = 3;
                break;
            case "Kuplu-kvartiru":
                AdvSectionName = "Куплю квартиру";
                AdvSectionID = 3;
                AdvSubSectionID = 4;
                break;
            case "Obyavleniya-Doma-Dachi":
                AdvSectionName = "Дома, дачи";
                AdvSectionID = 6;
                AdvSubSectionID = null;
                break;
            case "Arenda-ofisov":
                AdvSectionName = "Аренда офисов";
                AdvSectionID = 4;
                AdvSubSectionID = null;
                break;
            case "Prodam-kommercheskuu-nedvijimost":
                AdvSectionName = "Продажа коммерческой недвижимости";
                AdvSectionID = 5;
                AdvSubSectionID = null;
                break;
        }

        AdvertismentsViewControl.Settings.SectionId = AdvSectionID;
        AdvertismentsViewControl.Settings.SubSectionId = AdvSubSectionID;
        AdvertismentsViewControl.Settings.State = AdvertismentsMode;
        AdvertismentsViewControl.Settings.Limit = Constants.Max_Advertisments_View_Count;
        AdvertismentsViewControl.Settings.Date = Date.ToShortDateString();
    }
}