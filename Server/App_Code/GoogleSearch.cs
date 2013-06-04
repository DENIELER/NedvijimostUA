using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for GoogleSearch
/// </summary>
public class GoogleSearch
{
    public GoogleSearch()
	{
		
	}

    public const int searchPagesCount = 10;

    public const int searchMinTimeout = 15;
    public const int searchMaxTimeout = 150;

    private const string searchUrl = "http://www.google.ru/search?q={0}";
    private string googleAdvContainerDiv = "//li[@class='g']";
    private string googleTitle = "h3[@class='r']//a";
    private string googleText = "div[@class='s']//span[@class='st']";
    private string googleWebsite = "div[@class='s']//div[@class='kv']//cite";


    private const string googleTitlesRegexTemplate = "<h3 class=\"r\"><a href=\"([^<>]*?)>(.*?)</a></h3>";
    private const string googleWebSiteRegexTemplate = "<div class=\"s\"><div class=\"f kv\"><cite>(.*?)</cite></div></div>";
    private const string googleTextRegexTemplate = "<div class=\"s\"><span class=\"st\">(.*?)</span></div>";
    private const string leftTemplate = "<h3 class=\"r\"><a href=\"([^<>]*?)>";
    private const string rightTemplate = "</a></h3>";

    public List<WebResult> Search(string keyword, int countResults)
    {
        List<WebResult> webResults = new List<WebResult>();

        var request = (HttpWebRequest)WebRequest.Create(string.Format(searchUrl, keyword));
        var stream = request.GetResponse().GetResponseStream();
        var parsingPage = new HtmlDocument();
        parsingPage.Load(stream, Encoding.GetEncoding("windows-1251"));

        HtmlNodeCollection searchResultContainers = parsingPage.DocumentNode.SelectNodes(googleAdvContainerDiv);
		if(searchResultContainers != null)
		{
			foreach (HtmlNode container in searchResultContainers)
			{
				string _title = container.SelectSingleNode(googleTitle) != null
					? container.SelectSingleNode(googleTitle).InnerText
					: string.Empty;
				string _text = container.SelectSingleNode(googleText) != null
					? container.SelectSingleNode(googleText).InnerText
					: string.Empty;
				string _website = container.SelectSingleNode(googleWebsite) != null
					? container.SelectSingleNode(googleWebsite).InnerText
					: string.Empty; 

				webResults.Add(new WebResult()
				{
					title = _title,
					content = _text,
					website = _website
				});
			}
		}

        return webResults;

        #region Old search code
        //List<WebResult> webResults = new List<WebResult>();
        
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(searchUrl, keyword));
        //request.Timeout = 50000;
        ////request.Accept = "text/html, application/xhtml+xml, */*";
        ////request.Headers["Accept-Language"] = "en-US";
        ////request.UserAgent = "Mozilla/5.0";
        ////request.Headers["Accept-Encoding"] = "gzip, deflate";
        ////request.KeepAlive = false;
        ////request.Host = "www.google.ru";

        //var response = request.GetResponse();
        //var responseStream = response.GetResponseStream();
        //StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251"));
        //string htmlFullText = responseStreamReader.ReadToEnd();
        
        //Regex parsingRegex = new Regex(googleTitlesRegexTemplate);
        //MatchCollection matchCollection = parsingRegex.Matches(htmlFullText);
        //if (matchCollection.Count > 0)
        //{
        //    int count = 1;
        //    foreach (Match match in matchCollection)
        //    {
        //        WebResult result = new WebResult();

        //        string leftRegexed = Regex.Replace(match.Value, leftTemplate, "");
        //        result.title = Regex.Replace(leftRegexed, rightTemplate, "");

        //        webResults.Add(result);

        //        count++;
        //        if (count > countResults) break;
        //    }
        //}

        //return webResults;
        #endregion Old search code
    }
}

public class WebResult
{
    public string title { get; set; }
    public string content { get; set; }
    public string website {get; set;}
}