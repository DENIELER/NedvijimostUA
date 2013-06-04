using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for Settings
/// </summary>
public class Settings
{
    private const string settingsFileName = "~/xmlSettings/settings.xml";

	public static string getPhoneFormatsRegexTemplate()
	{
	    string regexTemplate = "";

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(settingsFileName));

        XmlNodeList phoneFormats = xmlDocument.GetElementsByTagName("phoneFormat");
        foreach (XmlNode phoneFormat in phoneFormats)
        {
            regexTemplate += phoneFormat.InnerText + "|";
        }

	    return regexTemplate.Remove(regexTemplate.Length - 1);
	}

    public static List<string> getStopSearchWords(string sectionCode)
    {
        List<string> stopWordsList = new List<string>();
        
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(settingsFileName));

        XmlNode sectionStopWords = xmlDocument.SelectSingleNode("//" + sectionCode + "StopSearchWords");
        XmlNodeList stopWords = sectionStopWords.SelectNodes("stopWord");
        foreach (XmlNode word in stopWords)
            stopWordsList.Add(word.InnerText);
        
        return stopWordsList;
    }

    public static Dictionary<string, string> getResearchPhoneFormatsRegexTemplates()
    {
        Dictionary<string, string> researchPhoneFormatsResult = new Dictionary<string, string>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(settingsFileName));

        XmlNodeList researchPhoneFormats = xmlDocument.GetElementsByTagName("researchPhoneFormat");
        foreach (XmlNode phoneFormat in researchPhoneFormats)
        {
            researchPhoneFormatsResult.Add(phoneFormat.InnerText, phoneFormat.Attributes["researchFormat"].Value);
        }

        return researchPhoneFormatsResult;
    }

    public static RentOptions getRentOptions()
    {
        var rentOptions = new RentOptions();
        
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(settingsFileName));

        var rentPath = xmlDocument.DocumentElement.SelectSingleNode("rent");
        if(rentPath != null)
        {
            var takeOffWords = rentPath.SelectSingleNode("take_off_words");
            if(takeOffWords != null)
            {
                var words = takeOffWords.SelectNodes("word");
                if(words != null)
                {
                    rentOptions.TakeOffWords = new List<string>();
                    foreach (XmlNode word in words)
                        rentOptions.TakeOffWords.Add(word.InnerText);
                }
            }

            var rentWords = rentPath.SelectSingleNode("rent_words");
            if(rentWords != null)
            {
                var words = rentWords.SelectNodes("word");
                if(words != null)
                {
                    rentOptions.RentWords = new List<string>();
                    foreach (XmlNode word in words)
                        rentOptions.RentWords.Add(word.InnerText);
                }
            }
        }
        
        return rentOptions;
    }
}

public class RentOptions
{
    public List<string> TakeOffWords { get; set; }
    public List<string> RentWords { get; set; }
}