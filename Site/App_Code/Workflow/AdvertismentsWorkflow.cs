using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

/// <summary>
/// Summary description for AdvertismentsWorkflow
/// </summary>
public class AdvertismentsWorkflow
{
    #region Ctor

    public AdvertismentsWorkflow()
    {
        Context = new DataModel();
    }
    public AdvertismentsWorkflow(DataModel context)
	{
        Context = context;
	}

    #endregion Ctor

    #region Variables

    public DataModel Context { get; set; }

    #endregion Variables

    #region Methods

    public AdvertismentsResponse LoadAdvertisments(Nedvijimost.AdvertismentsRequest request)
    {
        if (request.Date == null)
        {
            AdvertismentsResponse adversitmentsAndFullParsedCount = LoadTodayAdversitments(request);
            if (AdvertismentsNotLoaded(adversitmentsAndFullParsedCount))
                adversitmentsAndFullParsedCount = LoadYesterdayAdversitments(request);

            return adversitmentsAndFullParsedCount;
        }
        else
            return LoadAdversitments(request);
    }

    public void MarkNotByTheme(int advertisment_id)
    {
        if (Context != null)
        {
            var adv = Context.Advertisments.FirstOrDefault(a => a.Id == advertisment_id);
            if (adv != null)
            {
                adv.not_realestate = true;
                Context.SubmitChanges();
            }
        }
    }
    public void MarkIsSpecial(int advertisment_id)
    {
        if (Context != null)
        {
            var adv = Context.Advertisments.FirstOrDefault(a => a.Id == advertisment_id);
            if (adv != null)
            {
                adv.isSpecial = true;
                adv.isSpecialDateTime = Utils.GetUkranianDateTimeNow();
                Context.SubmitChanges();
            }
        }
    }
    public List<Advertisment> GetAdvertismentsByPhone(string phoneNumber)
    {
        if (Context != null)
        {
            return (from adv in Context.Advertisments
                    join advPhone in Context.AdvertismentPhones on adv.Id equals advPhone.AdvertismentId
                    where advPhone.phone == phoneNumber
                    orderby adv.createDate descending
                    select adv).ToList();
        }

        return null;
    }
    public void HideAdvertisment(int advertisment_id)
    {
        if (Context != null)
        {
            var adv = Context.Advertisments.FirstOrDefault(a => a.Id == advertisment_id);
            if (adv != null)
            {
                adv.not_show_advertisment = true;
                Context.SubmitChanges();
            }
        }
    }

    #region Advertisments loading
    private static bool AdvertismentsNotLoaded(AdvertismentsResponse adversitmentsAndFullParsedCount)
    {
        return adversitmentsAndFullParsedCount == null
               || (adversitmentsAndFullParsedCount != null
                   && adversitmentsAndFullParsedCount.Advertisments != null
                   && adversitmentsAndFullParsedCount.Advertisments.All(a => a.isSpecial));
    }

    private AdvertismentsResponse LoadTodayAdversitments(Nedvijimost.AdvertismentsRequest request)
    {
        request.DateFrom = Utils.GetUkranianDateTimeNow().Date;
        request.DateTo = Utils.GetUkranianDateTimeNow().Date.AddDays(1);
        return LoadAdversitments(request);
    }
    private AdvertismentsResponse LoadYesterdayAdversitments(Nedvijimost.AdvertismentsRequest request)
    {
        request.DateFrom = Utils.GetUkranianDateTimeNow().AddDays(-1).Date;
        request.DateTo = Utils.GetUkranianDateTimeNow().Date;
        return LoadAdversitments(request);
    }

    //AdvertismentsState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo, int sectionId = 1, int? subSectionId = null, int offset = 0, int limit = 100
    private AdvertismentsResponse LoadAdversitments(Nedvijimost.AdvertismentsRequest requestParameters)
    {
        IQueryable<Advertisment> advertisments = FilterAdversitments(requestParameters.DateFrom.Value, requestParameters.DateTo.Value);
        int advertismentsCount = 0;
        int advertismentsToShowCount = 0;
        if (advertisments != null)
        {
            advertisments = advertisments.Where(adv => adv.AdvertismentSection_Id == requestParameters.SectionId);

            if (requestParameters.SubSectionId != null)
                advertisments = advertisments.Where(adv => adv.AdvertismentSubSection.Id == requestParameters.SubSectionId.Value);

            advertismentsCount = advertisments.Count();

            switch (requestParameters.State)
            {
                case AdvertismentsState.JustParsed:
                    advertisments = advertisments.Where(
                        adv => adv.subpurchaseAdvertisment && adv.SubPurchase_Id == null
                        );
                    break;
                case AdvertismentsState.NotSubpurchase:
                    advertisments = advertisments.Where(
                        adv => !adv.subpurchaseAdvertisment
                        );
                    break;
                case AdvertismentsState.Subpurchase:
                    advertisments = advertisments.Where(
                        adv => adv.subpurchaseAdvertisment && adv.SubPurchase_Id != null
                        );
                    break;
                case AdvertismentsState.SubpurchaseWithNotSubpurchase:
                    advertisments = advertisments.Where(
                        adv => !adv.subpurchaseAdvertisment || adv.SubPurchase_Id != null
                        );
                    break;
            }

            advertismentsToShowCount = advertisments.Count();

            //--- special filters
            if (requestParameters.OnlyWithPhotos)
                advertisments = advertisments.Where(adv => adv.AdvertismentsPhotos.Any());

            advertisments = advertisments.Skip(requestParameters.Offset).Take(requestParameters.Limit);
        }

        return FormatResultAdversitments(advertisments, advertismentsCount, advertismentsToShowCount);
    }

    #endregion Advertisments loading

    private IQueryable<Advertisment> FilterAdversitments(DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        //var searchResults = (from seachResult in Context.SearchResults
        //                     where seachResult.createDate >= dateTimeFrom.Date
        //                           && seachResult.createDate < dateTimeTo.Date
        //                     from adv in seachResult.Advertisments
        //                     where !adv.not_realestate
        //                     && !adv.not_show_advertisment
        //                     orderby adv.createDate descending
        //                     select adv).ToList();

        var specialFromDateTime = dateTimeFrom.Date.AddDays(-7);
        IQueryable<Advertisment> searchResults = from adv in Context.Advertisments
                             where 
                                ((!adv.isSpecial && adv.createDate >= dateTimeFrom.Date)
                                || (adv.isSpecial && adv.createDate >= specialFromDateTime.Date))
                                 && adv.createDate < dateTimeTo.Date
                                 && !adv.not_realestate
                                 && !adv.not_show_advertisment
                                 //&& adv.AdvertismentPhones.Any()
                             orderby adv.isSpecial descending, adv.createDate descending
                             select adv;

        //var manualResults = (from adv in Context.Advertisments
        //                     where !adv.searchresult_id.HasValue
        //                         && adv.createDate >= dateTimeFrom.Date
        //                         && adv.createDate < dateTimeTo.Date
        //                         && !adv.not_realestate
        //                         && !adv.not_show_advertisment
        //                     orderby adv.createDate descending
        //                     select adv).ToList();

        //var finalAdvertismentsList = searchResults;//specialResults.Union(searchResults);
        //int advertsCount = finalAdvertismentsList.Count();

        return searchResults;
    }
    private AdvertismentsResponse FormatResultAdversitments(IQueryable<Advertisment> advertisments, int fullAdvertismentsCountBeforeFilter, int advertismentsToShowCount)
    {
        //--- set last update time
        DateTime lastTimeUpdated;

        if (advertisments != null && advertisments.Any())
            lastTimeUpdated = advertisments.Max(adv => adv.createDate);
        else lastTimeUpdated = DateTime.Now;

        return new AdvertismentsResponse(advertisments, fullAdvertismentsCountBeforeFilter, advertismentsToShowCount, lastTimeUpdated);
    }
    public string GetAdvertismentText(int advertisment_id)
    {
        if (Context != null)
        {
            var adv = Context.Advertisments.FirstOrDefault(a => a.Id == advertisment_id);
            if (adv != null)
            {
                return adv.text;
            }
        }

        return string.Empty;
    }

    public bool ExistsAdvertisment(int advID, int? userID)
    {
        if (userID == null)
            return false;

        if (Context != null)
        {
            var adv = Context.Advertisments.FirstOrDefault(a => a.Id == advID && a.UserID == userID.Value);
            if (adv != null)
            {
                return true;
            }
        }

        return false;
    }

    #endregion Methods
}

public enum AdvertismentsState
{
    JustParsed,
    Subpurchase,
    NotSubpurchase,
    SubpurchaseWithNotSubpurchase
}