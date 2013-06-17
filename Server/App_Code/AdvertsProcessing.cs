using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Model;
using System.Threading;
using System.Net;

public class AdvertsProcessing
{
    public AdvertsProcessing(string sectionCode)
    {
        SectionCode = sectionCode;

        Context = new Model.DataModel();
    }

    public AdvertsProcessing(Model.DataModel context)
    {
        Context = context;
    }

    public Model.DataModel Context { get; set; }

    public Log Log { get; set; }

    public string SectionCode { get; set; }

    public void CaptureAdvertisments(IList<SiteSetting> siteSettings)
    {
        var searchResultsWorkflow = new SearchResultsWorkflow(SectionCode, Context, Log);
        //-- create new SearchResult
        var searchResult = searchResultsWorkflow.AddSearchResult();
        //-- save advertisments into database

        foreach (var siteSetting in siteSettings)
        {
            try
            {
                Log.WriteLog("Start getting advertisments from " + Environment.NewLine + siteSetting.name);

                var siteParser = new SiteParser(siteSetting);
                siteParser.Log = Log;

                IList<ParsedAdvertisment> parsedAdvertisments = null;
                try
                {
                    // --------
                    parsedAdvertisments = siteParser.GetAdvertisements();
                    Log.WriteLog("Captured all advertisments.");
                    // --------
                }
                catch (ThreadAbortException e)
                {
                    Log.WriteLog("Site parser parse error ThreadAbortException. Site: " + siteSetting.name + Environment.NewLine
                        + e.Message + ". Trace:" + e.StackTrace);

                    if (e.InnerException != null)
                        Log.WriteLog("Inner exception: " + e.InnerException.Message);

                    //Thread.ResetAbort();
                }
                catch (Exception e)
                {
                    Log.WriteLog("Site parser parse error. Site: " + siteSetting.name + Environment.NewLine
                        + e.Message + ". Trace:" + e.StackTrace);

                    if (e.InnerException != null)
                        Log.WriteLog("Inner exception: " + e.InnerException.Message);
                }

                Log.WriteLog("Check whether need to divide advertisments into sub sections.");
                var subSectionsSeparator = new SubSectionsSeparator(SectionCode, Context, Log);
                if (subSectionsSeparator.NeedToDivideIntoSubSections)
                {
                    subSectionsSeparator.DivideIntoSubSections(parsedAdvertisments);
                }

                PingServer();
                Log.WriteLog("Saving advertisments in DB.");
                searchResultsWorkflow.SaveAdvertismentsInSearchResult(searchResult, parsedAdvertisments);
                searchResult.allParsedAdvertismentsCount += parsedAdvertisments.Count();
                Context.SubmitChanges();
                Log.WriteLog("Finished. Saving advertisments in DB.");
            }
            catch (Exception e)
            {
                Log.WriteLog("Site parser full error. Site: " + siteSetting.name + Environment.NewLine
                        + e.Message + ". Trace:" + e.StackTrace);

                if (e.InnerException != null)
                    Log.WriteLog("Inner exception: " + e.InnerException.Message);
            }
        }
    }

    public IList<Advertisment> FilterSubpurchasers(IList<Advertisment> adversitments, string sectionCode)
    {
        var listAdvertisments = adversitments as List<Advertisment>;

        Log.WriteLog("Start filtering advertisments from subpurchases.");

        Log.WriteLog("Database filtering.");
        Log.WriteLog("Advertisments for filtering - " + adversitments.Count);
        List<Advertisment> goodDatabaseAdvertisments = DatabaseFilter(listAdvertisments);
        var badDatabaseAdvCount = adversitments.Count - goodDatabaseAdvertisments.Count;
        Log.WriteLog("Database filtering has ended. Bad founded - " + badDatabaseAdvCount);

        Log.WriteLog("Web Search filtering.");
        Log.WriteLog("Advertisments for filtering - " + goodDatabaseAdvertisments.Count);
        List<Advertisment> goodWebSearchAdvertisments = WebSearchFilter(goodDatabaseAdvertisments, sectionCode);
        Log.WriteLog("Web Search filtering has ended.");

        Log.WriteLog("End filtering advertisments from subpurchases.");
        return goodDatabaseAdvertisments.Union(goodWebSearchAdvertisments).ToList();

        //var goodAdvertismentsSet = adversitments;
        ////--- if WebSearch ended less than in the end.. in some index - indexEndSearch
        //if (indexEndSearch >= 0 && adversitments.Count > 0)
        //{
        //    Log.WriteLog("Web Search ended by Exception on index - " + indexEndSearch);
        //    goodAdvertismentsSet = adversitments.GetRange(0, indexEndSearch);
        //}

        //IList<Advertisment> goodAdvertismentsTemp = goodAdvertismentsSet;
        //if (badDatabaseAdvertisments != null && badDatabaseAdvertisments.Count > 0)
        //{
        //    goodAdvertismentsTemp =
        //        goodAdvertismentsSet.Where(adversitment => !badDatabaseAdvertisments.Contains(adversitment));
        //}

        //if (badWebSearchAdvertisments != null && badWebSearchAdvertisments.Count > 0)
        //{
        //    goodAdvertismentsTemp =
        //        goodAdvertismentsTemp.Where(adversitment => !badWebSearchAdvertisments.Contains(adversitment));
        //}

        //goodAdversitmentsWithPhoneses = goodAdvertismentsTemp.ToList();
    }

    /// <summary>
    /// Filter subpurchases from Database
    /// </summary>
    /// <param name="adversitments"></param>
    /// <returns>Bad advertisments, with subpurchases phones</returns>
    private List<Advertisment> DatabaseFilter(IList<Advertisment> adversitments)
    {
        var goodAdvList = new List<Advertisment>();

        var allDatabaseSubpurchases = (from subPurchaseObject in Context.SubPurchases
                                       join subPurchasePhoneObject in Context.SubPurchasePhones
                                          on subPurchaseObject.id equals subPurchasePhoneObject.SubPurchaseId
                                       select new { SubPurchase = subPurchaseObject, SubPurchasePhone = subPurchasePhoneObject }).ToList();

        if (!allDatabaseSubpurchases.Any())
        {
            Log.WriteLog("Database filtering...No advertisments was loaded from databae.");
            return null;
        }
        else
            Log.WriteLog("Database filtering... Loaded " + allDatabaseSubpurchases.Count() + " advertisments.");
        
        int tempCounter = 0;
        foreach (var adversitment in adversitments)
        {
            SubPurchase subpurchase = null;
            foreach (var advPhone in adversitment.AdvertismentPhones)
            {
                if (!string.IsNullOrEmpty(advPhone.phone))
                {
                    var advPhoneExpression = SubpurchasesWorkflow.MakePhoneLikeExpression(advPhone.phone);
                    Regex regex = new Regex(string.Format("^{0}$", advPhoneExpression.Replace("%", ".*")), RegexOptions.IgnoreCase);
                    var subpurchasePhonePair = allDatabaseSubpurchases.FirstOrDefault(
                        sp =>
                            {
                                return regex.IsMatch(sp.SubPurchasePhone.phone ?? string.Empty);
                            });
                    
                    if (subpurchasePhonePair != null && subpurchasePhonePair.SubPurchase != null)
                    {
                        subpurchase = subpurchasePhonePair.SubPurchase;
                        break;
                    }
                }
            }
            if (subpurchase == null)
            {
                //adversitment.subpurchaseAdvertisment = false;
                //adversitment.SubPurchase = null;

                //Context.SaveChanges();

                goodAdvList.Add(adversitment);
            }
            else
            {
                adversitment.subpurchaseAdvertisment = true;
                adversitment.SubPurchase = subpurchase;

                Context.SubmitChanges();
            }

            tempCounter++;
            if (tempCounter % 50 == 0)
                Log.WriteLog("Database filtering..." + tempCounter.ToString() + " advertisments. Good have founded " +
                             goodAdvList.Count.ToString() + " adv.");
        }

        return goodAdvList;
    }

    private const int webSearchSubPurchasesSaveIteration = 30;
    public List<Advertisment> WebSearchFilter(IList<Advertisment> adversitments, string sectionCode)
    {
        if (adversitments.Count <= 0)
        {
            Log.WriteLog("Web Search filtering.. No data to find. Adv count is zero.");
            return null;
        }else
            Log.WriteLog("Web Search filtering.. Adv count is ." + adversitments.Count);
        
        var goodAdvList = new List<Advertisment>();
        var badAdvList = new List<Advertisment>();

        int currentAdvIndex = 0;
        try
        {
            var stopWords = Settings.getStopSearchWords(sectionCode);
            //var researchFormats = Settings.getResearchPhoneFormatsRegexTemplates();

            var googleSearch = new GoogleSearch();
            var random = new Random();

            int webSearchRequestCount = 0;

            var subpurchasesWorkflow = new SubpurchasesWorkflow(Context);

            foreach (var advertisment in adversitments)
            {
                if (advertisment.AdvertismentPhones != null && advertisment.AdvertismentPhones.Count > 0)
                {
                    foreach (var phone in advertisment.AdvertismentPhones)
                    {
                        if (phone != null && string.IsNullOrEmpty(phone.phone))
                            continue;

                        System.Threading.Thread.Sleep(10 * random.Next(GoogleSearch.searchMinTimeout,
                                                                     GoogleSearch.searchMaxTimeout));
                        var results = googleSearch.Search(phone.phone, GoogleSearch.searchPagesCount);
                        webSearchRequestCount++;

                        bool goodAdv = true;
                        int badWebResultsCount = 0;
                        foreach (var webResult in results.Where(r => !r.website.Contains("nedvijimost-ua.com")))
                        {
                            bool isBadResult = false;
                            foreach (var stopWord in stopWords)
                            {
                                if (webResult.title.ToLower().Contains(stopWord))
                                    isBadResult = true;
                            }

                            if (isBadResult)
                            {
                                badWebResultsCount++;

                                Log.WriteLog("SubPurchase founded. Phone - " + phone.phone);

                                var filterAdvertisment = new Model.WebSearchFilterAdvertisment();
                                filterAdvertisment.title = webResult.title ?? "";
                                filterAdvertisment.text = webResult.content ?? "";
                                filterAdvertisment.createDate = Utils.GetUkranianDateTimeNow();
                                filterAdvertisment.subPurchasePhone = phone.phone;
                                Context.WebSearchFilterAdvertisments.InsertOnSubmit(filterAdvertisment);
                                Context.SubmitChanges();
                            }

                            if (badWebResultsCount == 3)
                            {
                                goodAdv = false;
                                break;
                            }
                        }

                        if (goodAdv)
                        {
                            advertisment.SubPurchase = null;
                            advertisment.subpurchaseAdvertisment = false;

                            Context.SubmitChanges();

                            goodAdvList.Add(advertisment);
                        }
                        else
                        {
                            //--- add subpurchase
                            var subpurchase = subpurchasesWorkflow.AddSubpurchasePhone(phone.phone, true);
                            advertisment.SubPurchase = subpurchase;
                            advertisment.subpurchaseAdvertisment = true;

                            Context.SubmitChanges();

                            badAdvList.Add(advertisment);
                        }

                        
                        if (webSearchRequestCount % webSearchSubPurchasesSaveIteration == 0)
                        {
                            //--- add all bad web search advertisments into db
                            //AddAllPhonesAsSubPuchases(badAdvList);
                            //---

                            Log.WriteLog("Web Search filtering... Requests count " + webSearchRequestCount.ToString());

                            //--- refresh Session
                            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                                HttpContext.Current.Session["webSearchRequestCount"] = webSearchRequestCount;
                        }
                    }
                }
                //else
                //{
                //    Log.WriteLog("Adv. without phone - " + advertisment.text);
                //}

                currentAdvIndex++;
            }
        }
        catch (Exception e)
        {
            Log.WriteLog("Web Search filtering EXCEPTION.");
            Log.WriteLog("Error message: " + e.Message);
            
            if(e.InnerException != null)
                Log.WriteLog("Inner error: " + e.InnerException.Message);
            Log.WriteLog("Trace: " + e.StackTrace);

            return goodAdvList;
        }

        return goodAdvList;
    }

    //private void AddAllPhonesAsSubPuchases(List<Advertisment> adversitments)
    //{
    //    var subPurchasesList = (from subPurchaseObject in Context.SubPurchases
    //                            join subPurchasePhoneObject in Context.SubPurchasePhones
    //                               on subPurchaseObject.Id equals subPurchasePhoneObject.SubPurchaseId
    //                            select new {SubPruchase = subPurchaseObject, SubPurchasePhone = subPurchasePhoneObject}).ToList();

    //    foreach (var adversitment in adversitments)
    //    {
    //        foreach (var phone in adversitment.AdvertismentPhones)
    //        {
    //            if (subPurchasesList.Any(subPurchObject => subPurchObject.SubPurchasePhone.phone == phone.phone)) 
    //                continue;

    //            var subPurchaseByPhone = (from subPurchaseObject in subPurchasesList
    //                                      where subPurchaseObject.SubPurchasePhone.phone == phone.phone
    //                                      select subPurchaseObject.SubPruchase).SingleOrDefault();

    //            Model.SubPurchase subPurchase;
    //            if (subPurchaseByPhone == null)
    //            {
    //                subPurchase = new Model.SubPurchase()
    //                {
    //                    Id = Guid.NewGuid(),
    //                    not_checked = false,
    //                    createDate = Utils.GetUkranianDateTimeNow()
    //                };
    //                Context.AddToSubPurchases(subPurchase);
    //                Context.SaveChanges();

    //                //--- maybe need to replace it some right code
    //                subPurchasesList = (from subPurchaseObject in Context.SubPurchases
    //                                    join subPurchasePhoneObject in Context.SubPurchasePhones
    //                                       on subPurchaseObject.Id equals subPurchasePhoneObject.SubPurchaseId
    //                                    select new { SubPruchase = subPurchaseObject, SubPurchasePhone = subPurchasePhoneObject }).ToList();
    //            }
    //            else
    //                subPurchase = subPurchaseByPhone;

    //            var newSubPurchasePhone = new Model.SubPurchasePhone()
    //            {
    //                Id = Guid.NewGuid(),
    //                phone = phone.phone,
    //                createDate = Utils.GetUkranianDateTimeNow(),
    //                SubPurchase = subPurchase
    //            };
    //            Context.AddToSubPurchasePhones(newSubPurchasePhone);
    //            Context.SaveChanges();
    //        }            
    //    }
    //}

    /// <summary>
    /// Leave server alive
    /// </summary>
    public void PingServer()
    {
        try
        {
            WebClient http = new WebClient();
            string Result = http.DownloadString(Resources.Constants.PingServerUrl);
        }
        catch (Exception ex)
        {
            Log.WriteLog("Ping server error: " + ex.Message);
        }
    }
}
