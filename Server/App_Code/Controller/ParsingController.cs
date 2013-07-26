using System;
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

    /// <summary>
    /// For long running operation execution
    /// </summary>
    private delegate void LongRun();
    #endregion variables

    public ParsingController(string sectionCode, string logServiceCode, string parsingProcessName, int? parsingProcessPart = null)
	{
        _advertismentSectionCode = sectionCode;

        _parsingProcessName = parsingProcessName;
        _parsingProcessPart = parsingProcessPart;

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
                //var Long = new LongRun(Parse);
                //var thread = new System.Threading.Thread(new System.Threading.ThreadStart(Long));

                //var jobHost = new JobHost(_log);
                //jobHost.DoWork(thread.Start);

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
            var siteSettingsWorkflow = 
                _parsingProcessPart != null
                ? new SiteSettingsWorkflow(Resources.Constants.SiteSettingsFile, _advertismentSectionCode, _parsingProcessPart.Value)
                : new SiteSettingsWorkflow(Resources.Constants.SiteSettingsFile, _advertismentSectionCode);
            System.Collections.Generic.IList<SiteSetting> siteSettings = siteSettingsWorkflow.getSiteSettings();

            var advertsProcessing = new AdvertsProcessing(_advertismentSectionCode);
            advertsProcessing.Log = _log;
            // capture advertismens from web sites
            advertsProcessing.CaptureAdvertisments(siteSettings);
            _log.WriteLog("Parsing finished.");
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