using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

public class CrawlWorkflow : BaseContextWorkflow
{
    #region Variables
    private const int DelayBetweenSitesParsing = 700;
    private const int DelayBetweenPagesParsing = 300;

    private string sectionCode;
    private List<string> settingsPhotosUrlsForRemoving;
    #endregion Variables

    #region Ctor
    public CrawlWorkflow(string sectionCode)
	{
        this.sectionCode = sectionCode;
	}
    #endregion Ctor

    #region Public Methods
    public int Crawl(IList<SiteSetting> siteSettings)
    {
        //-- create new SearchResult
        var searchResults = new SearchResults(sectionCode, context);
        Model.SearchResult searchResult = searchResults.AddSearchResult();

        var advertisments = new List<Server.Entities.Advertisment>();
        foreach (SiteSetting siteSetting in siteSettings)
        {
            try
            {
                GetAdvertisments(siteSetting, ref advertisments);
                PrepairAdvertisments(sectionCode, context, ref advertisments);
            }
            catch (Exception e)
            {
                if (e is ThreadAbortException)
                    WriteLog("Crawler ThreadAbortException.");
                WriteLog("Site parser full error." + Environment.NewLine +
                        "Site: " + siteSetting.name + Environment.NewLine
                        + "Error: " + e.Message + Environment.NewLine
                        + "Trace:" + e.StackTrace);
                if (e.InnerException != null)
                    WriteLog("Inner exception: " + e.InnerException.Message);
            }
        }

        searchResult.allParsedAdvertismentsCount = SaveAdvertisments(searchResult, advertisments);
        context.SubmitChanges();
         
        return advertisments.Count;
    }
    #endregion Public Methods

    #region Private Methods
    private void GetAdvertisments(SiteSetting siteSetting, ref List<Server.Entities.Advertisment> advertisments)
    {
        WriteLog("Start crawling from " + Environment.NewLine +
                        siteSetting.name);
        CrawlAdvertisements(siteSetting, ref advertisments);
        WriteLog("Crawled all advertisments.");
    }
    #region Get Advertisments
    private int CrawlAdvertisements(SiteSetting siteSetting, ref List<Server.Entities.Advertisment> advertisments)
    {
        bool advExists = false;
        int pageNum = siteSetting.startPageIndex;

        //--- load photo settings
        var photoSettings = new PhotoSettings();
        settingsPhotosUrlsForRemoving = photoSettings.getPhotoUrlsToRemove();
        //--- end load photo settings

        try
        {
            do
            {
                System.Threading.Thread.Sleep(DelayBetweenSitesParsing);

                string webPageUrl = pageNum == siteSetting.startPageIndex
                    ? siteSetting.url
                    : string.Format(siteSetting.nextPageUrl, pageNum.ToString());

                //--- decide parse list or current page for advertisments
                var urlsList = new List<string>();
                if (!string.IsNullOrEmpty(siteSetting.containerListDiv))
                    urlsList = GetRedirectListUrls(webPageUrl, siteSetting);
                else
                    urlsList.Add(webPageUrl);

                //--- parse
                foreach (var url in urlsList)
                {
                    System.Threading.Thread.Sleep(DelayBetweenPagesParsing);

                    advExists = CrawlAdvertisementsFromPage(url, siteSetting, ref advertisments);
                }

                pageNum++;

                if (siteSetting.pagesCount > 0
                    && pageNum > siteSetting.pagesCount)
                    advExists = false;

            } while (advExists);
        }
        catch (Exception e)
        {
            WriteLog("Crawler inner error." +
                    "Error: " + e.Message + Environment.NewLine +
                    "Trace:" + e.StackTrace + Environment.NewLine +
                    "Return data. AdvCount " + advertisments.Count);
            if (e.InnerException != null)
                WriteLog("Inner Exception: " + e.InnerException.Message);
        }

        return advertisments.Count;
    }
    private List<string> GetRedirectListUrls(string listPageUrl, SiteSetting siteSetting)
    {
        var resultUrls = new List<string>();

        var request = (HttpWebRequest)WebRequest.Create(listPageUrl);
        Stream stream = request.GetResponse().GetResponseStream();
        var parsingPage = new HtmlDocument();

        parsingPage.Load(stream, Encoding.GetEncoding(siteSetting.encodingName));

        HtmlNodeCollection urlsContainers = parsingPage
                                            .DocumentNode
                                            .SelectNodes(siteSetting.containerListCurrentAdvUrl);
        if (urlsContainers != null)
            foreach (HtmlNode urlContainer in urlsContainers)
                if (urlContainer.Attributes["href"] != null)
                {
                    if (!string.IsNullOrEmpty(siteSetting.containerListCurrentAdvUrlHost))
                        resultUrls.Add(siteSetting.containerListCurrentAdvUrlHost + urlContainer.Attributes["href"].Value);
                    else
                        resultUrls.Add(urlContainer.Attributes["href"].Value);
                }

        return resultUrls;
    }
    private bool CrawlAdvertisementsFromPage(string pageUrl, SiteSetting siteSetting, ref List<Server.Entities.Advertisment> advertisments)
    {
        int advertismentsCrawledCount = 0;
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(System.Web.HttpUtility.HtmlDecode(pageUrl));
            request.Method = "GET";
            request.ContentType = "text/html";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            var parsingPage = new HtmlDocument();

            parsingPage.Load(stream, Encoding.GetEncoding(siteSetting.encodingName));

            HtmlNodeCollection advertismentContainerNodes = parsingPage
                                                        .DocumentNode
                                                        .SelectNodes(siteSetting.containerDiv);
            if (advertismentContainerNodes != null)
                foreach (HtmlNode advertismentContainerNode in advertismentContainerNodes)
                {
                    HtmlNode advertismentNode = advertismentContainerNode.SelectSingleNode("." + siteSetting.regexTemplate);
                    if (advertismentNode != null)
                    {
                        string advertismentText = Utils.StripTagsRegex(
                                                    siteSetting.excludeTags
                                                    ? Utils.StripAllTextBetweenTagsRegex(advertismentNode.InnerHtml)
                                                    : advertismentNode.InnerHtml);

                        var advertisment = new Server.Entities.Advertisment()
                        {
                            Text = advertismentText,
                            Phones = CrawlPhones(advertismentContainerNode, advertismentText, siteSetting),
                            PhotoUrls = CrawlPhotos(advertismentContainerNode, siteSetting),
                            Link = pageUrl,
                            SiteName = siteSetting.name
                        };

                        advertismentsCrawledCount++;
                        advertisments.Add(advertisment);
                    }
                }
        }
        catch (WebException ex)
        {
            WriteLog("Web exception captured.");
            if (ex.Response != null)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    WriteLog(reader.ReadToEnd());
                }
            }
            else
            {
                WriteLog("web exception error: " + ex.Message);
            }
        }

        return advertismentsCrawledCount > 0;
    }
    #region Crawl phones
    private List<string> CrawlPhones(HtmlNode advertismentContainerNode, string advertismentText, SiteSetting siteSetting)
    {
        var containerPhones = CrawlPhonesFromContainer(advertismentContainerNode, siteSetting);
        var textPhones = CrawlPhonesFromText(advertismentText);

        var resultPhones = new List<string>();
        if (containerPhones != null)
            resultPhones.AddRange(containerPhones);
        if (textPhones != null)
            resultPhones.AddRange(textPhones);

        return resultPhones.Distinct().ToList();
    }
    private List<string> CrawlPhonesFromContainer(HtmlNode advertismentsContainer, SiteSetting siteSetting)
    {
        //--- from 'another from text' container
        var containerPhones = new List<string>();
        if (!string.IsNullOrWhiteSpace(siteSetting.phonesRegexTemplate))
        {
            HtmlNodeCollection advertismentPhones = advertismentsContainer.SelectNodes("." + siteSetting.phonesRegexTemplate);
            if (advertismentPhones != null)
            {
                containerPhones.AddRange(
                    (from phone in advertismentPhones
                     select HttpUtility.HtmlDecode(Utils.StripTagsRegex(phone.InnerText)).Trim()).ToList());

                return containerPhones;
            }
        }
        //--- end  from 'another from text' container

        return null;
    }
    private List<string> CrawlPhonesFromText(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            string phonesRegexTemplate = Settings.getPhoneFormatsRegexTemplate();
            var parsingRegex = new Regex(phonesRegexTemplate);

            var matchCollection = parsingRegex.Matches(text);
            List<string> matchedPhones =
                (from Match match in matchCollection
                 select match.Value).ToList();

            return matchedPhones;
        }

        return null;
    }
    #endregion Crawl phones
    #region Crawl photos
    private List<string> CrawlPhotos(HtmlNode advertismentContainerNode, SiteSetting siteSetting)
    {
        var imageUrls = new List<string>();

        var imagesList = advertismentContainerNode.Descendants("img").Where(img => img.Attributes["src"] != null);
        foreach (var image in imagesList)
        {
            if (settingsPhotosUrlsForRemoving != null
                && (settingsPhotosUrlsForRemoving.Contains(image.Attributes["src"].Value)
                    || image.Attributes["src"].Value.Contains(".gif")))
                continue;

            string sourceImageUrl;
            if (!image.Attributes["src"].Value.Contains("http://")
                && !string.IsNullOrEmpty(siteSetting.containerListCurrentAdvUrlHost))
            {
                sourceImageUrl = siteSetting.containerListCurrentAdvUrlHost + image.Attributes["src"].Value;
            }
            else
            {
                sourceImageUrl = image.Attributes["src"].Value;
            }

            string siteImageUrl = sourceImageUrl;
            //SavePhotoToFTP(sourceImageUrl);
            image.Attributes["src"].Value = siteImageUrl;
            imageUrls.Add(siteImageUrl);
        }

        return imageUrls;
    }
    #endregion Crawl photos
    #endregion Get Advertisments

    private void PrepairAdvertisments(string sectionCode, Model.DataModel context, ref List<Server.Entities.Advertisment> advertisments)
    {
        Utils.PingServer();

        WriteLog("Sub sections division.");
        var subSectionsSeparator = new SubSectionsSeparationWorkflow(sectionCode, context);
        if (subSectionsSeparator.NeedToDivideIntoSubSections)
            subSectionsSeparator.DivideIntoSubSections(ref advertisments);
    }

    private int SaveAdvertisments(Model.SearchResult searchResult, IList<Server.Entities.Advertisment> advertisments)
    {
        Utils.PingServer();
        WriteLog("Saving advertisments in DB.");

        int savedAdvertismentsCount = 0;
        foreach (var advertisment in advertisments)
        {
            try
            {
                bool existsAdvertisment = context.Advertisments
                                            .Any(a => a.searchresult_id == searchResult.Id
                                                      && a.text == advertisment.Text);
                if (!existsAdvertisment)
                {
                    Model.AdvertismentSubSection subSectionObject = null;
                    if (advertisment.SubSectionID != null)
                    {
                        subSectionObject = context.AdvertismentSubSections
                                            .SingleOrDefault(s => s.Id == advertisment.SubSectionID.Value);
                        if (subSectionObject == null)
                            throw new Exception("Can not find Sub Section. ID: " + advertisment.SubSectionID.Value);
                    }

                    var advertismentEntity = new Model.Advertisment
                    {
                        createDate = Utils.GetUkranianDateTimeNow(),
                        modifyDate = Utils.GetUkranianDateTimeNow(),
                        text = advertisment.Text,
                        AdvertismentSection = searchResult.AdvertismentSection,
                        SearchResult = searchResult,
                        link = advertisment.Link,
                        siteName = advertisment.SiteName,
                        subpurchaseAdvertisment = true,
                        AdvertismentSubSection = subSectionObject
                    };
                    context.Advertisments.InsertOnSubmit(advertismentEntity);

                    try
                    {
                        context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        WriteLog("Saving advertisment error. " + Environment.NewLine
                            + "Text: " + advertisment.Text + Environment.NewLine
                            + "Link: " + advertisment.Link + Environment.NewLine
                            + "Site: " + advertisment.SiteName + Environment.NewLine
                            + "Error: " + e.Message + Environment.NewLine
                            + ". Trace:" + e.StackTrace);
                        throw;
                    }

                    //--- add phones
                    foreach (var phone in advertisment.Phones)
                    {
                        if (!string.IsNullOrWhiteSpace(phone))
                        {
                            //_log.WriteLog(phone.phone);
                            var advertismentPhoneEntity = new Model.AdvertismentPhone
                            {
                                phone = phone,
                                Advertisment = advertismentEntity
                            };
                            context.AdvertismentPhones.InsertOnSubmit(advertismentPhoneEntity);

                            advertismentEntity.AdvertismentPhones.Add(advertismentPhoneEntity);
                        }
                    }
                    //----

                    //--- add photos
                    foreach (var photoUrl in advertisment.PhotoUrls)
                    {
                        if (!string.IsNullOrWhiteSpace(photoUrl))
                        {
                            var advertismentPhotoEntity = new Model.AdvertismentsPhoto
                            {
                                filename = photoUrl,
                                createDate = Utils.GetUkranianDateTimeNow(),
                                Advertisment = advertismentEntity
                            };
                            context.AdvertismentsPhotos.InsertOnSubmit(advertismentPhotoEntity);

                            advertismentEntity.AdvertismentsPhotos.Add(advertismentPhotoEntity);
                        }
                    }
                    //---

                    try
                    {
                        context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        WriteLog("Saving advertisment's elements error. " + Environment.NewLine
                            + "Error: " + e.Message + Environment.NewLine
                            + ". Trace:" + e.StackTrace);
                        throw;
                    }

                    savedAdvertismentsCount++;
                }
            }
            catch (Exception e)
            {
                WriteLog("Saving advertisment error. " + Environment.NewLine
                            + "Error: " + e.Message + Environment.NewLine
                            + ". Trace:" + e.StackTrace);
            }
        }

        WriteLog("Finished. Saved advertisments in DB.");
        return savedAdvertismentsCount;
    }
    #endregion Private Methods
}