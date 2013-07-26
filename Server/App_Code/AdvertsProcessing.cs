﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Model;
using System.Threading;
using System.Net;
using System.Xml.Linq;

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
                    Log.WriteLog("Site parser parse error ThreadAbortException." + Environment.NewLine 
                        + "Site: " + siteSetting.name + Environment.NewLine
                        + e.Message + Environment.NewLine +
                        ". Trace:" + e.StackTrace);

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
                Utils.PingServer();
                var subSectionsSeparator = new SubSectionsSeparator(SectionCode, Context, Log);
                if (subSectionsSeparator.NeedToDivideIntoSubSections)
                {
                    subSectionsSeparator.DivideIntoSubSections(parsedAdvertisments);
                }

                Utils.PingServer();
                Log.WriteLog("Saving advertisments in DB.");
                searchResultsWorkflow.SaveAdvertismentsInSearchResult(searchResult, parsedAdvertisments);
                searchResult.allParsedAdvertismentsCount += parsedAdvertisments.Count();
                Context.SubmitChanges();
                Log.WriteLog("Finished. Saved advertisments in DB.");
                Utils.PingServer();
            }
            catch (Exception e)
            {
                Log.WriteLog("Site parser full error." + Environment.NewLine +
                    "Site: " + siteSetting.name + Environment.NewLine
                    + e.Message + Environment.NewLine 
                    + ". Trace:" + e.StackTrace);

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

        Utils.PingServer();

        Log.WriteLog("Web Search filtering.");
        Log.WriteLog("Advertisments for filtering - " + goodDatabaseAdvertisments.Count);
        List<Advertisment> goodWebSearchAdvertisments = WebSearchFilter(goodDatabaseAdvertisments, sectionCode);
        Log.WriteLog("Web Search filtering has ended.");

        Log.WriteLog("Founded good - " + goodWebSearchAdvertisments.Count);
        return goodDatabaseAdvertisments.Union(goodWebSearchAdvertisments).ToList();
    }

    /// <summary>
    /// Filter subpurchases from Database
    /// </summary>
    /// <param name="adversitments"></param>
    /// <returns>Bad advertisments, with subpurchases phones</returns>
    private List<Advertisment> DatabaseFilter(IList<Advertisment> adversitments)
    {
        var goodAdvList = new List<Advertisment>();

        int advertismentsCount = adversitments.Count;

        List<Tuple<int, string>> advertismentsPhonesToFilter = new List<Tuple<int, string>>();
        for(int i = 0; i < advertismentsCount; i++)
        {
            Advertisment adversitment = adversitments[i];
            if (i == 0 || i % 50 == 0)
                advertismentsPhonesToFilter.Clear();

            foreach (var advertismentPhone in adversitment.AdvertismentPhones
                .Where(p => !string.IsNullOrEmpty(p.phone)))
            {
                advertismentsPhonesToFilter
                    .Add(
                        new Tuple<int, string>
                            (
                                adversitment.Id, 
                                SubpurchasesWorkflow.MakePhoneLikeExpression(advertismentPhone.phone)
                            )
                    );
            }

            if ((i + 1) % 50 == 0 || (i + 1) == advertismentsCount)
            {
                // The XML
                XElement xml = new XElement("root",
                    from adv in advertismentsPhonesToFilter
                    select new XElement("a",
                        new XElement("Id", adv.Item1),
                        new XElement("p", adv.Item2)));
                IQueryable<SubPurchaseCheckResult> goodAdvertismentsList = Context.CheckSubPurchases(xml);

                foreach (var resultElement in goodAdvertismentsList)
                {
                    var currentAdvertisment = adversitments.SingleOrDefault(a => a.Id == resultElement.Id);
                    if (currentAdvertisment != null)
                    {
                        if (resultElement.SubPurchaseID == null)
                            goodAdvList.Add(adversitment);
                        else
                        {
                            adversitment.subpurchaseAdvertisment = true;
                            adversitment.SubPurchase_Id = resultElement.SubPurchaseID;
                        }
                    }
                }

                Context.SubmitChanges();
            }

            if (i % 50 == 0)
            {
                Log.WriteLog("Database filtering..." + i.ToString() + " advertisments. " + 
                        "Good have founded " + goodAdvList.Count.ToString() + " adv.");
                Utils.PingServer();
            }
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
                            Utils.PingServer();

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
            Log.WriteLog("Web Search filtering EXCEPTION." + Environment.NewLine +
                        "Error message: " + e.Message + Environment.NewLine +
                        "Trace: " + e.StackTrace);
            
            if(e.InnerException != null)
                Log.WriteLog("Inner error: " + e.InnerException.Message);

            return goodAdvList;
        }

        return goodAdvList;
    }
}
