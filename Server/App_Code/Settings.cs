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

    public class SubSectionDeterminationWordsSettings
    {
        private static SubSectionDeterminationWordsSettings _instance;
        public static SubSectionDeterminationWordsSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SubSectionDeterminationWordsSettings();

                return _instance;
            }
        }

        /// <summary>
        /// Get SubSections determination words
        /// </summary>
        /// <param name="sectionCode"></param>
        /// <param name="subSectionCodes"></param>
        /// <returns>Dictionary: Key - subSectionCode, Value - list of subSection determination words</returns>
        public Dictionary<string, List<string>> getSubSectionsDeterminationWords(string sectionCode, List<string> subSectionCodes)
        {
            if (subSectionCodes == null)
                throw new ArgumentNullException("SubSections parameter is empty");

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(settingsFileName));

            var result = new Dictionary<string, List<string>>();

            foreach (var subSectionCode in subSectionCodes)
            {
                var subSectionDeteminationWords = getSubSectionDeterminationWords(sectionCode, subSectionCode, xmlDocument);
                if (subSectionDeteminationWords != null && subSectionDeteminationWords.Any())
                    result.Add(subSectionCode, subSectionDeteminationWords);
            }

            return result;
        }
        private List<string> getSubSectionDeterminationWords(string sectionCode, string subSectionCode, XmlDocument xmlDocument = null)
        {
            var subSectionWords = new List<string>();

            if (xmlDocument == null)
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(settingsFileName));
            }

            var rootPath = xmlDocument.DocumentElement.SelectSingleNode("subSectionDeterminationWords");
            if (rootPath == null)
                throw new Exception("subSectionDeterminationWords section was not found in settings.xml file");

            var sectionPath = rootPath.SelectSingleNode(sectionCode + "_section");
            if (sectionPath == null)
                throw new Exception(sectionCode + "_section" + " section was not found in settings.xml file");

            var takeOffWords = sectionPath.SelectSingleNode(subSectionCode + "_subsection");
            if (takeOffWords == null)
                throw new Exception(subSectionCode + "_subsection" + " section was not found in settings.xml file");

            var words = takeOffWords.SelectNodes("word");
            if (words != null)
                foreach (XmlNode word in words)
                    subSectionWords.Add(word.InnerText);

            return subSectionWords;
        }
    }
}
