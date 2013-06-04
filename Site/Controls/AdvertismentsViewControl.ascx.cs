using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using System.Web.UI.HtmlControls;

public partial class AdvertismentsViewControl : System.Web.UI.UserControl
{
    #region Properties

    public IList<Advertisment> ResultsToShow = null;

    public AdvertismentsViewSettings _settings;
    public AdvertismentsViewSettings Settings 
    {
        get
        {
            if (_settings == null)
                _settings = new AdvertismentsViewSettings();
            return _settings;
        }

        set
        {
            _settings = value;
        }
    }

    private int ViewPage;

    #endregion Properties

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (ResultsToShow != null)
        //    lvAdvertisments.DataSource = ResultsToShow.ToList();
        //else
        //    lvAdvertisments.DataSource = null;
        //lvAdvertisments.DataBind();

        if (!string.IsNullOrEmpty(Request["page"]))
        {
            if (!int.TryParse(Request["page"], out ViewPage) || ViewPage < 1)
                ViewPage = 1;

            this.Settings.Offset = (ViewPage - 1) * this.Settings.Limit;
        }
    }

    protected void lvAdvertisments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlAnchor howToMakeSpecialLink = (HtmlAnchor)e.Item.FindControl("lnkHowToMakeSpecial");
            HtmlControl divSubpurchaseCheckbox = (HtmlControl)e.Item.FindControl("divSubpurchaseCheckBox");
            HtmlControl divNotThemeCheckbox = (HtmlControl)e.Item.FindControl("divNotThemeAdvertismentCheckBox");
            bool isSpecial = (bool)DataBinder.Eval(e.Item.DataItem, "isSpecial");
            if (isSpecial
                && howToMakeSpecialLink != null
                && divSubpurchaseCheckbox != null
                && divNotThemeCheckbox != null)
            {
                howToMakeSpecialLink.Visible = true;

                divSubpurchaseCheckbox.Visible =
                divNotThemeCheckbox.Visible = false;
            }
        }
    }

    protected string CutToFirstSpace(string str)
    {
		if(!string.IsNullOrEmpty(str))
		{
			var spaceIndex = str.IndexOf('|');
			if (spaceIndex > 0)
			{
				return str.Remove(spaceIndex, str.Length - spaceIndex);
			}
		}

        return str;
    }
    protected string FormatGalleryPhoto(string photoUrl)
    {
        if (photoUrl.Contains("thumb=1&"))
            return photoUrl.Replace("thumb=1&", "");
        else return photoUrl;
    }
}

public class AdvertismentsViewSettings
{
    public AdvertismentsViewSettings()
    {
        SectionId = 1;
    }

    public int SectionId { get; set; }
    public int? SubSectionId { get; set; }

    //private const string SessionSavedConjecturedSubpurchases = "SavedConjecturedSubpurchases";
    //public bool NotShowSubpurchaseCheckbox { get; set; }

    public AdvertismentsState Filter { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }

    public DateTime? Date { get; set; }
}