using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BaseParsingController
/// </summary>
public class SetAdvSubSectionController
{
    #region variables
    protected Log _log;
    private const string TaskExecutedName = "SetSubSections_TaskExecuted";

    /// <summary>
    /// For long running operation execution
    /// </summary>
    private delegate void LongRun();
    #endregion variables

    public SetAdvSubSectionController(string logFileName)
    {
        _log = new Log(logFileName);
    }

    public void StartSettingSubSections()
    {
        var context = HttpContext.Current;
        var taskAppExecutingFlag = context.Application[TaskExecutedName];
        if (context != null
            && (taskAppExecutingFlag == null
               ||
               (taskAppExecutingFlag is bool && (bool)taskAppExecutingFlag == false))
            )
            try
            {
                HttpContext.Current.Application[TaskExecutedName] = true;

                _log.WriteLog("---------------------------------------------------------------------------" +
                                Environment.NewLine +
                                "Start " + TaskExecutedName + " setting processing.");
                var Long = new LongRun(Parse);
                var thread = new System.Threading.Thread(new System.Threading.ThreadStart(Long));

                var jobHost = new JobHost(_log);
                jobHost.DoWork(thread.Start);
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
            const int advertismentsProcessingCount = 4000;

            var context = new DataModel();
            var sections = context.AdvertismentSections
                            .Select(x => x)
                            .OrderBy(x => x.Id)
                            .ToList();

            foreach (var section in sections)
            {
                var advertisments = context.Advertisments
                            .Where(a => a.AdvertismentSection.code == section.code && a.AdvertismentSubSection == null)
                            .OrderByDescending(a => a.createDate)
                            .Take(advertismentsProcessingCount).ToList();

                if (advertisments.Any())
                {
                    if (context.AdvertismentSubSections.Where(s => s.AdvertismentSection.code == section.code).Any())
                    {
                        List<Model.AdvertismentSubSection> subSections = context.AdvertismentSubSections
                                .Where(s => s.AdvertismentSection.code == section.code)
                                .ToList();

                        List<string> subSectionCodes = subSections.Select(s => s.code).ToList();

                        Dictionary<string, List<string>> subSectionDeterminationWords =
                            Settings.SubSectionDeterminationWordsSettings.Instance
                            .getSubSectionsDeterminationWords(section.code, subSectionCodes);

                        foreach (var advertisment in advertisments)
                        {
                            foreach (var subSectionWords in subSectionDeterminationWords)
                            {
                                foreach (string subSectionWord in subSectionWords.Value)
                                {
                                    if (advertisment.text.ToLower().Contains(subSectionWord))
                                    {
                                        advertisment.AdvertismentSubSection = subSections.FirstOrDefault(s => s.code == subSectionWords.Key);
                                        break;
                                    }
                                }

                                if (advertisment.AdvertismentSubSection != null)
                                    break;
                            }

                            if (advertisment.AdvertismentSubSection == null)
                                advertisment.AdvertismentSubSection = subSections.FirstOrDefault();

                            context.SubmitChanges();
                        }

                        return;
                    }
                }
            }
        }
        catch (Exception e)
        {
            _log.WriteLog("Parsing inner error! Error message: " + e.Message + ". Trace:" + e.StackTrace);
        }
        finally
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Application[TaskExecutedName] = false;
        }
    }
}