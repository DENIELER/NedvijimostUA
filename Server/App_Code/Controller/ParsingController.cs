﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Summary description for BaseParsingController
/// </summary>
public class ParsingController
{
    #region variables
    protected string _advertismentSectionCode;
    protected Log _log;

    protected string _parsingProcessName;
    protected int? _parsingProcessPart;

    private string _city;

    /// <summary>
    /// For long running operation execution
    /// </summary>
    private delegate void LongRun();
    #endregion variables

    public ParsingController(string sectionCode, string logServiceCode, string parsingProcessName, int? parsingProcessPart, string city)
	{
        _advertismentSectionCode = sectionCode;

        _parsingProcessName = parsingProcessName;
        _parsingProcessPart = parsingProcessPart;

        _city = city;

        _log = new Log(logServiceCode, sectionCode);
	}

    public void StartParsing()
    {
        var context = HttpContext.Current;
        var taskAppExecutingFlag = context.Application[_parsingProcessName + _parsingProcessPart ?? string.Empty + "_TaskExecuted"];
        if (context != null
            && (taskAppExecutingFlag == null
               || 
               (taskAppExecutingFlag is bool && (bool)taskAppExecutingFlag == false))
            )
            try
            {
                HttpContext.Current.Application[_parsingProcessName + _parsingProcessPart ?? string.Empty + "_TaskExecuted"] = true;

                _log.WriteLog("------------------" +
                                Environment.NewLine +
                                "Start " + _parsingProcessName + " parse processing.");
                
                Task.Factory.StartNew(Parse);
            }
            catch (Exception exc)
            {
                _log.WriteLog("Parsing error! Error message: " + exc.Message);
            }
    }
    private void Parse()
    {
        try
        {
            //-- get sites settings
            var siteSettingsWorkflow = new SiteSettingsWorkflow(Resources.Constants.SiteSettingsFile, _advertismentSectionCode, _parsingProcessPart, _city);
            System.Collections.Generic.IList<SiteSetting> siteSettings = siteSettingsWorkflow.getSiteSettings();

            var advertsProcessing = new CrawlWorkflow(_advertismentSectionCode);
            advertsProcessing.SetLog(_log);
            // capture advertismens from web sites
            var crawledCount = advertsProcessing.Crawl(siteSettings);
            _log.WriteLog("Parsing finished. Crawled - " + crawledCount);
        }
        catch (Exception e)
        {
            _log.WriteLog("Parsing inner error!" + Environment.NewLine +
                "Error message: " + e.Message + Environment.NewLine + 
                ". Trace:" + e.StackTrace);
        }
        finally
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Application[_parsingProcessName + _parsingProcessPart ?? string.Empty + "_TaskExecuted"] = false;
        }
    }
}