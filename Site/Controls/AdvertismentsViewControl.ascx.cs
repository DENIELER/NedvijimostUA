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
using Newtonsoft.Json.Linq;

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
        GetQueryParameters();

        LoadAdvertisments();
    }

    private void GetQueryParameters()
    {
        if (!string.IsNullOrEmpty(Request["page"]))
        {
            if (!int.TryParse(Request["page"], out ViewPage) || ViewPage < 1)
                ViewPage = 1;

            this.Settings.Offset = (ViewPage - 1) * this.Settings.Limit;
        }
    }

    private void LoadAdvertisments()
    {
        //--- execute service to get advertisments
        string parameters = FormGetAdvertismentsUrl();
        var serviceUrl = new System.Uri(Page.Request.Url, "/WebServices/Content/AdvertismentsContentLoader.ashx?" + parameters).AbsoluteUri;
        var result = GetServiceResult(serviceUrl);
        var jsonObject = JObject.Parse(result);

        //--- header
        advertisments_header.InnerHtml += (string)jsonObject["Header"];

        //--- advertisments
        advertisments.InnerHtml = "";
        advertisments.InnerHtml += (string)jsonObject["Advertisments"];
        advertisments.InnerHtml += (string)jsonObject["Pagging"];
    }
    private string FormGetAdvertismentsUrl()
    {
        string subSectionId = Settings.SubSectionId != null
                                ? "&subSectionId=" + Settings.SubSectionId.ToString()
                                : "";
        string date = Settings.Date != null
                                ? "&date=" + Settings.Date.Value.ToShortDateString()
                                : "";
        string parameters = "sectionId=" + Settings.SectionId +
            subSectionId +
            "&filter=" + Convert.ToInt32(Settings.Filter) +
            "&offset=" + Settings.Offset +
            "&limit=" + Settings.Limit +
            date +
            "&url=" + Request.RawUrl;
        return parameters;
    }

    private string GetServiceResult(string serviceUrl)
    {
        HttpWebRequest HttpWReq;
        HttpWebResponse HttpWResp;

        HttpWReq = (HttpWebRequest)WebRequest.Create(serviceUrl);
        HttpWReq.Method = "GET";
        HttpWReq.ContentType = "application/json; charset:utf-8";
        
        HttpWReq.CookieContainer = new CookieContainer();
        if (Request.Cookies != null)
        {
            for (int i = 0; i < Request.Cookies.Count; i++)
            {
                HttpCookie oCookie = Request.Cookies.Get(i);
                Cookie oC = new Cookie()
                {
                    Domain = HttpWReq.RequestUri.Host,
                    Expires = oCookie.Expires,
                    Name = oCookie.Name,
                    Path = oCookie.Path,
                    Secure = oCookie.Secure,
                    Value = oCookie.Value
                };

                HttpWReq.CookieContainer.Add(oC);
            }
        }
        
        //using (var writer = new StreamWriter(HttpWReq.GetRequestStream()))
        //{
        //    writer.Write(data);
        //    writer.Flush();
        //    writer.Close();
        //}

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