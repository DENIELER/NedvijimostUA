using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

public class SiteSettingsWorkflow
{
    public string fileName;
    public string mainSection;

    public SiteSettingsWorkflow(string fileName, string mainSection)
    {
        this.fileName = fileName;
        this.mainSection = mainSection;
    }

    public SiteSettingsWorkflow(string fileName, string mainSection, int processorPart)
    {
        this.fileName = fileName;
        this.mainSection = mainSection + "_" + processorPart.ToString();
    }

    public IList<SiteSetting> getSiteSettings()
    {
        if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(mainSection))
        {
            throw new Exception("Not right configured SiteSettings workflow class.");
        }

        List<SiteSetting> siteInfos = new List<SiteSetting>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(fileName));

        XmlNodeList sites = xmlDocument.DocumentElement.SelectSingleNode(mainSection).SelectNodes("site");
        foreach (XmlNode site in sites)
        {
            SiteSetting info = new SiteSetting();
            info.name = site["name"].InnerText;
            info.url = site["url"].InnerText;
            info.nextPageUrl = site["nexPageUrl"].InnerText;

            var startIndexPageXmlElement = site["startPageIndex"];
            if(startIndexPageXmlElement != null)
                info.startPageIndex = Convert.ToInt32(startIndexPageXmlElement.InnerText);

            var parsingOptionsXmlElement = site["parsingOptions"];
            if (parsingOptionsXmlElement != null)
            {
                info.encodingName = parsingOptionsXmlElement["encoding"].InnerText;

                var containerListDivXmlElement = parsingOptionsXmlElement["containerListDiv"];
                if (containerListDivXmlElement != null)
                    info.containerListDiv = containerListDivXmlElement.InnerText;
                var containerListCurrentAdvUrlXmlElement = parsingOptionsXmlElement["containerListCurrentAdvUrl"];
                if (containerListCurrentAdvUrlXmlElement != null)
                    info.containerListCurrentAdvUrl = containerListCurrentAdvUrlXmlElement.InnerText;
                var containerListCurrentAdvUrlHostXmlElement = parsingOptionsXmlElement["containerListCurrentAdvUrlHost"];
                if (containerListCurrentAdvUrlHostXmlElement != null)
                    info.containerListCurrentAdvUrlHost = containerListCurrentAdvUrlHostXmlElement.InnerText;
                
                info.containerDiv = parsingOptionsXmlElement["containerDiv"].InnerText;
                info.regexTemplate = parsingOptionsXmlElement["regexTemplate"].InnerText;
                var phoneRegexTemplateXmlElement = parsingOptionsXmlElement["phonesRegexTemplate"];
                if (phoneRegexTemplateXmlElement != null)
                {
                    info.phonesRegexTemplate = phoneRegexTemplateXmlElement.InnerText;
                }

                info.excludeTags = parsingOptionsXmlElement["excludeTags"] != null 
                    && Convert.ToBoolean(parsingOptionsXmlElement["excludeTags"].InnerText);

                info.pagesCount = (parsingOptionsXmlElement["pagesCount"] != null &&
                                   !string.IsNullOrWhiteSpace(parsingOptionsXmlElement["pagesCount"].InnerText))
                                      ? Convert.ToInt16(parsingOptionsXmlElement["pagesCount"].InnerText)
                                      : 0;
            }
            siteInfos.Add(info);
        }

        return siteInfos;
    }
}

public struct SiteSetting
{
    public string name { get; set; }
    public string url { get; set; }
    public string nextPageUrl { get; set; }
    public int startPageIndex { get; set; }

    public string encodingName { get; set; }

    public string containerListDiv { get; set; }
    public string containerListCurrentAdvUrl { get; set; }
    public string containerListCurrentAdvUrlHost { get; set; }

    public string containerDiv { get; set; }
    public string regexTemplate { get; set; }
    public string phonesRegexTemplate { get; set; }

    public bool excludeTags { get; set; }

    public int pagesCount { get; set; }
}