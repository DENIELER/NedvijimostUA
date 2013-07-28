using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

public class FilterWorkflow : BaseContextWorkflow
{
    #region Variables
    private const int WebSearchSubPurchasesSaveIteration = 30;

    private string sectionCode;
    #endregion Variables

    #region Ctor
    public FilterWorkflow(string sectionCode, Model.DataModel context)
        : base(context)
	{
        this.sectionCode = sectionCode;
    }
    #endregion Ctor

    #region Public Methods
    public int FilterFromRealtors(IList<Model.Advertisment> adversitments)
    {
        WriteLog("Start filtering advertisments from subpurchases.");
        
        WriteLog("Database filtering.");
        WriteLog("Advertisments for filtering - " + adversitments.Count);
        List<Model.Advertisment> goodDatabaseAdvertisments = DatabaseFilter(adversitments);
        var badDatabaseAdvCount = adversitments.Count - goodDatabaseAdvertisments.Count;
        WriteLog("Database filtering has ended. Bad founded - " + badDatabaseAdvCount);

        Utils.PingServer();

        WriteLog("Web Search filtering.");
        WriteLog("Advertisments for filtering - " + goodDatabaseAdvertisments.Count);
        List<Model.Advertisment> goodWebSearchAdvertisments = WebSearchFilter(goodDatabaseAdvertisments);
        WriteLog("Web Search filtering has ended.");

        return goodWebSearchAdvertisments.Count;
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Filter subpurchases from Database
    /// </summary>
    /// <param name="adversitments"></param>
    /// <returns>Bad advertisments, with subpurchases phones</returns>
    private List<Model.Advertisment> DatabaseFilter(IList<Model.Advertisment> adversitments)
    {
        var goodAdvList = new List<Model.Advertisment>();

        int advertismentsCount = adversitments.Count;

        List<Tuple<int, string>> advertismentsPhonesToFilter = new List<Tuple<int, string>>();
        for (int i = 0; i < advertismentsCount; i++)
        {
            Model.Advertisment adversitment = adversitments[i];
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
                                SubPurchases.MakePhoneLikeExpression(advertismentPhone.phone)
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
                IQueryable<Model.SubPurchaseCheckResult> goodAdvertismentsList = context.CheckSubPurchases(xml);

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

                context.SubmitChanges();
            }

            if (i % 50 == 0)
            {
                WriteLog("Filtering..." + i.ToString() + " advertisments. " + Environment.NewLine +
                        "Good - " + goodAdvList.Count.ToString());
                Utils.PingServer();
            }
        }

        return goodAdvList;
    }

    private List<Model.Advertisment> WebSearchFilter(IList<Model.Advertisment> adversitments)
    {
        if (adversitments.Count <= 0)
        {
            WriteLog("Web Search filtering.. No data to find. Adv count is zero.");
            return null;
        }
        else
            WriteLog("Web Search filtering.. Adv count is ." + adversitments.Count);

        var goodAdvList = new List<Model.Advertisment>();
        var badAdvList = new List<Model.Advertisment>();

        int currentAdvIndex = 0;
        try
        {
            var stopWords = Settings.getStopSearchWords(sectionCode);
            
            var googleSearch = new GoogleSearch();
            var random = new Random();

            int webSearchRequestCount = 0;

            var subpurchasesWorkflow = new SubPurchases(context);

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

                                var filterAdvertisment = new Model.WebSearchFilterAdvertisment()
                                {
                                    title = webResult.title ?? "",
                                    text = webResult.content ?? "",
                                    createDate = Utils.GetUkranianDateTimeNow(),
                                    subPurchasePhone = phone.phone
                                };
                                
                                context.WebSearchFilterAdvertisments.InsertOnSubmit(filterAdvertisment);
                                context.SubmitChanges();
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

                            context.SubmitChanges();

                            goodAdvList.Add(advertisment);
                        }
                        else
                        {
                            //--- add subpurchase
                            var subpurchase = subpurchasesWorkflow.AddSubpurchasePhone(phone.phone, true);
                            advertisment.SubPurchase = subpurchase;
                            advertisment.subpurchaseAdvertisment = true;

                            context.SubmitChanges();

                            badAdvList.Add(advertisment);
                        }


                        if (webSearchRequestCount % WebSearchSubPurchasesSaveIteration == 0)
                        {
                            Utils.PingServer();

                            WriteLog("Web Search filtering... Requests count " + webSearchRequestCount.ToString());

                            //--- refresh Session
                            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                                HttpContext.Current.Session["webSearchRequestCount"] = webSearchRequestCount;
                        }
                    }
                }

                currentAdvIndex++;
            }
        }
        catch (Exception e)
        {
            WriteLog("Web Search filtering EXCEPTION." + Environment.NewLine +
                        "Error message: " + e.Message + Environment.NewLine +
                        "Trace: " + e.StackTrace);

            if (e.InnerException != null)
                WriteLog("Inner error: " + e.InnerException.Message);
        }

        return goodAdvList;
    }
    #endregion Private Methods
}