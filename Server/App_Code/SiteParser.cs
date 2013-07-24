using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Model;
using System.Web;

public class SiteParser
{
    private const int timeDelay = 700;
    private const int betweenPagesTimeDelay = 300;

    public SiteSetting CurrentSiteSetting { get; set; }
    public Log Log { get; set; }

	public SiteParser(SiteSetting siteSetting)
	{
        this.CurrentSiteSetting = siteSetting;
	}

    private List<string> photosUrlsToRemove;
    //Return pair: first - dictionary of the advertisments with key - order number, the second - phones list with key - order advert number
    public IList<ParsedAdvertisment> GetAdvertisements()
    {
        var resultAdvertisments = new List<ParsedAdvertisment>();

        bool advExists = false;
        int pageNum = CurrentSiteSetting.startPageIndex;

        //--- load photo settings
        var photoSettings = new PhotoSettings();
        photosUrlsToRemove = photoSettings.getPhotoUrlsToRemove();
        //--- end load photo settings

        try
        {
            do
            {
                System.Threading.Thread.Sleep(timeDelay);

                string webPageUrl = pageNum == CurrentSiteSetting.startPageIndex
                    ? CurrentSiteSetting.url
                    : string.Format(CurrentSiteSetting.nextPageUrl, pageNum.ToString());

                //--- decide parse list or current page for advertisments
                var urlsList = new List<string>();
                if (!string.IsNullOrEmpty(CurrentSiteSetting.containerListDiv))
                {
                    urlsList = GetRedirectToAdvertismentUrls(webPageUrl);
                }
                else
                    urlsList.Add(webPageUrl);
                
                //--- parse
                foreach (var url in urlsList)
                {
                    System.Threading.Thread.Sleep(betweenPagesTimeDelay);

                    List<ParsedAdvertisment> pageParsedAdvertisments = ParseAdvertismentPage(url);
                    if (pageParsedAdvertisments != null
                        && pageParsedAdvertisments.Any())
                    {
                        resultAdvertisments.AddRange(pageParsedAdvertisments);
                        advExists = true;
                    }
                    else
                        advExists = false;
                }
                
                pageNum++;
                Utils.PingServer();

                if (CurrentSiteSetting.pagesCount > 0 
                    && pageNum > CurrentSiteSetting.pagesCount) 
                    advExists = false;

            } while (advExists);

            return resultAdvertisments;
        }
        catch(Exception e)
        {
            Log.WriteLog("Error! Site parsing. " + e.Message + Environment.NewLine 
                        + "Trace:" + e.StackTrace + Environment.NewLine 
                        + "Return data. AdvCount " + resultAdvertisments.Count);

            if (e.InnerException != null)
            {
                Log.WriteLog("Inner Exception: " + e.InnerException.Message + Environment.NewLine
                        + "Trace:" + e.InnerException.StackTrace);
            }

            return resultAdvertisments;
        }finally
        {
            Log.WriteLog("Parsing of the site ended. " + Environment.NewLine 
                + "Page num: " + pageNum + Environment.NewLine 
                + "Adv count: " + resultAdvertisments.Count);

        }
    }

    private List<string> GetRedirectToAdvertismentUrls(string listPageUrl)
    {
        List<string> resultUrls = new List<string>();

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(listPageUrl);
        Stream stream = request.GetResponse().GetResponseStream();
        HtmlDocument parsingPage = new HtmlDocument();

        parsingPage.Load(stream, Encoding.GetEncoding(CurrentSiteSetting.encodingName));

        HtmlNodeCollection urlsContainers = parsingPage.DocumentNode.SelectNodes(CurrentSiteSetting.containerListCurrentAdvUrl);
        if (urlsContainers != null)
        {
            foreach (HtmlNode urlContainer in urlsContainers)
            {
                if (urlContainer.Attributes["href"] != null)
                {
                    if (!string.IsNullOrEmpty(CurrentSiteSetting.containerListCurrentAdvUrlHost))
                    {
                        resultUrls.Add(CurrentSiteSetting.containerListCurrentAdvUrlHost + urlContainer.Attributes["href"].Value);
                    }
                    else
                    {
                        resultUrls.Add(urlContainer.Attributes["href"].Value);
                    }
                }
            }
        }

        return resultUrls;
    }

    private List<ParsedAdvertisment> ParseAdvertismentPage(string pageUrl)
    {
        List<ParsedAdvertisment> resultAdvertisments = new List<ParsedAdvertisment>();

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(System.Web.HttpUtility.HtmlDecode(pageUrl));
            request.Method = "GET";
            request.ContentType = "text/html";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            HtmlDocument parsingPage = new HtmlDocument();

            parsingPage.Load(stream, Encoding.GetEncoding(CurrentSiteSetting.encodingName));

            HtmlNodeCollection advertismentsContainers = parsingPage.DocumentNode.SelectNodes(CurrentSiteSetting.containerDiv);
            if (advertismentsContainers != null)
            {
                foreach (HtmlNode container in advertismentsContainers)
                {
                    HtmlNode advertisment = container.SelectSingleNode("." + CurrentSiteSetting.regexTemplate);
                    if (advertisment != null)
                    {
                        var parsedAdvertisment = new ParsedAdvertisment();

                        parsedAdvertisment.PhotoUrls = SeparatePhotos(container, advertisment.InnerHtml);

                        //--- parse Text
                        var advText = CurrentSiteSetting.excludeTags
                                      ? Utils.StripAllTextBetweenTagsRegex(advertisment.InnerHtml)
                                      : advertisment.InnerHtml;
                        parsedAdvertisment.Text = Utils.StripTagsRegex(advText);
                        //--- end parse Text

                        //--- parse Phone
                        parsedAdvertisment.Phones = SeparatePhones(container, parsedAdvertisment.Text);
                        //--- end parse Phone

                        //--- parse Link and SiteName
                        parsedAdvertisment.Link = pageUrl;
                        parsedAdvertisment.SiteName = CurrentSiteSetting.name;
                        //--- end parse Link and SiteName

                        resultAdvertisments.Add(parsedAdvertisment);
                    }
                }
            }
        }
        catch (WebException ex)
        {
            Log.WriteLog("Web exception captured.");
            if (ex.Response != null)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Log.WriteLog(reader.ReadToEnd());
                }
            }
            else
            {
                Log.WriteLog("web exception error: " + ex.Message);
            }
        }

        return resultAdvertisments;
    }

    private List<string> SeparatePhotos(HtmlNode container, string advertismentInnerHtml)
    {
        var imageUrls = new List<string>();

        var imagesList = container.Descendants("img").Where(img => img.Attributes["src"] != null);
        foreach (var image in imagesList)
        {
            if (photosUrlsToRemove != null
                && (photosUrlsToRemove.Contains(image.Attributes["src"].Value) 
                    || image.Attributes["src"].Value.Contains(".gif")))
                continue;

            string sourceImageUrl;
            if (!image.Attributes["src"].Value.Contains("http://")
                && !string.IsNullOrEmpty(CurrentSiteSetting.containerListCurrentAdvUrlHost))
            {
                sourceImageUrl = CurrentSiteSetting.containerListCurrentAdvUrlHost + image.Attributes["src"].Value;
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

    //--- it is a lot of problems with FTP downloading files
    private string SavePhotoToFTP(string imageUrl)
    {
        imageUrl = imageUrl.Replace("&amp;", "&");

        string ftpUrl = "lilac.arvixe.com/nedvijimost-ua.com/wwwroot";
        string filePath = "/files";

        string ftpusername = "denieler";
        string ftppassword = "gtycbz1990";

        string fileExtenstion;
        fileExtenstion = GetFileExtension(imageUrl);
        string filename = "photo_" + Guid.NewGuid() + fileExtenstion;
        byte[] photoBytes;
        using (WebClient clientRequest = new WebClient())
        {
            photoBytes = clientRequest.DownloadData(imageUrl);
        }

        FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpUrl + filePath + "/" + filename));
        ftpClient.Credentials = new System.Net.NetworkCredential(ftpusername, ftppassword);
        ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
        ftpClient.UseBinary = true;
        ftpClient.KeepAlive = false;
        ftpClient.Method = WebRequestMethods.Ftp.UploadFile;
        ftpClient.ContentLength = photoBytes.Length;

        using (System.IO.Stream rs = ftpClient.GetRequestStream())
        {
            rs.Write(photoBytes, 0, photoBytes.Length);
            rs.Close();
        }

        using (FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse())
        {
            //var value = uploadResponse.StatusDescription;
            uploadResponse.Close();
        }
        
        return filePath + "/" + filename;
    }

    private string GetFileExtension(string imageUrl)
    {
        if (imageUrl.Contains(".jpg"))
            return ".jpg";
        else if (imageUrl.Contains(".png"))
            return ".png";
        else if (imageUrl.Contains(".gif"))
            return ".gif";
        else return ".jpg";
    }

    private List<string> SeparatePhones(HtmlNode container, string advText)
    {
        var containerPhones = SeparatePhonesFromContainer(container);
        var textPhones = SeparatePhonesFromText(advText);

        var resultPhones = new List<string>();
        if (containerPhones != null)
            resultPhones.AddRange(containerPhones);
        if (textPhones != null)
            resultPhones.AddRange(textPhones);

        return resultPhones.Distinct().ToList();
    }
    private List<string> SeparatePhonesFromContainer(HtmlNode advertismentsContainer)
    {
        //--- from 'another from text' container
        var containerPhones = new List<string>();
        if (!string.IsNullOrWhiteSpace(CurrentSiteSetting.phonesRegexTemplate))
        {
            HtmlNodeCollection advertismentPhones = advertismentsContainer.SelectNodes("." + CurrentSiteSetting.phonesRegexTemplate);
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
    private List<string> SeparatePhonesFromText(string text)
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

    private IList<Advertisment> ConvertToAdvertismentList(List<ParsedAdvertisment> parsedAdvertisments)
    {
        var advertisments = new List<Advertisment>();

        foreach (var parsedAdvertisment in parsedAdvertisments)
        {
            var advertisment = new Advertisment();

            //--- phones
            if (parsedAdvertisment.Phones != null
                && parsedAdvertisment.Phones.Any())
            {
                parsedAdvertisment.Phones.ForEach(
                    parsedPhone => advertisment.AdvertismentPhones.Add(
                        new AdvertismentPhone()
                        {
                            phone = parsedPhone
                        }));
            }

            //--- photos
            if (parsedAdvertisment.PhotoUrls != null
                && parsedAdvertisment.PhotoUrls.Any())
            {
                parsedAdvertisment.PhotoUrls.ForEach(
                    parsedPhoto => advertisment.AdvertismentsPhotos.Add(
                        new AdvertismentsPhoto()
                        {
                            filename = parsedPhoto
                        }));
            }

            advertisment.text = parsedAdvertisment.Text;

            advertisment.link = parsedAdvertisment.Link;
            advertisment.siteName = parsedAdvertisment.SiteName;

            advertisments.Add(advertisment);
        }

        return advertisments;
    }
}

public class ParsedAdvertisment
{
    public string Text {get;set;}
    public List<string> Phones {get;set;}
    public List<string> PhotoUrls { get; set; }

    public string Link { get; set; }
    public string SiteName { get; set; }

    public int? SectionID { get; set; }
    public int? SubSectionID { get; set; }
}