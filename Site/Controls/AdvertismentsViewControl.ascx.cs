using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using System.Web.UI.HtmlControls;
using DTO = Nedvijimost.DataTransferObject;
using System.Net;
using System.Xml;
using System.IO;

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

        //--- execute service to get advertisments
        var subSectionId = Settings.SubSectionId != null ? Settings.SubSectionId.ToString() : "null";
        var date = Settings.Date != null ? Settings.Date.Value.ToShortDateString() : "";
        string jsonRequest = 
        "{" +
                "\"sectionId\":" + Settings.SectionId.ToString() + "," +
                "\"subSectionId\":" + subSectionId + "," +
                "\"filter\":" + Convert.ToInt32(Settings.Filter).ToString() + "," +
                "\"offset\":" + Settings.Offset.ToString() + "," +
                "\"limit\":" + Settings.Limit.ToString() + "," +
                "\"date\":'" + date + "'" +
        "}";
        var serviceUrl = new System.Uri(Page.Request.Url, "/WebServices/AdvertismentsService.asmx/GetAdvertismentsHtml").AbsoluteUri;
        var result = GetServiceResult(serviceUrl, jsonRequest);
        Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.Parse(result);
        
        advertisments_header.InnerHtml += (string)jsonObject["d"]["Header"];

        advertisments.InnerHtml = "";
        advertisments.InnerHtml += (string)jsonObject["d"]["Advertisments"];
        advertisments.InnerHtml += (string)jsonObject["d"]["Pagging"];
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

    public string GetServiceResult(string serviceUrl, string data)
    {
        HttpWebRequest HttpWReq;
        HttpWebResponse HttpWResp;
        HttpWReq = (HttpWebRequest)WebRequest.Create(serviceUrl);
        HttpWReq.Method = "POST";
        HttpWReq.ContentType = "application/json; charset:utf-8";

        using (var writer = new StreamWriter(HttpWReq.GetRequestStream()))
        {
            writer.Write(data);
            writer.Flush();
            writer.Close();
        }

        HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
        if (HttpWResp.StatusCode == HttpStatusCode.OK)
        {
            //Consume webservice with basic XML reading, assumes it returns (one) string
            StreamReader reader = new StreamReader(HttpWResp.GetResponseStream());
            return reader.ReadToEnd();
        }
        else
        {
            throw new Exception("Error: " + HttpWResp.StatusCode.ToString());
        }
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