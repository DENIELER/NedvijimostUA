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
        Context = new NedvijimostDBEntities();
    }
	public AdvertismentsWorkflow(NedvijimostDBEntities context)
	{
        Context = context;
	}

    #endregion Ctor

    #region Variables

    public NedvijimostDBEntities Context { get; set; }

    #endregion Variables

    #region Methods

    public AdvertismentsView LoadAdvertisments(int sectionId, AdvertismentsState advertismentsFilter, int? subSectionId = null, int offset = 0, int limit = 100)
    {
        AdvertismentsView adversitmentsAndFullParsedCount = LoadTodayAdversitments(advertismentsFilter, sectionId, subSectionId, offset, limit);
        if (AdvertismentsNotLoaded(adversitmentsAndFullParsedCount))
            adversitmentsAndFullParsedCount = LoadYesterdayAdversitments(advertismentsFilter, sectionId, subSectionId, offset, limit);
        
        return adversitmentsAndFullParsedCount;
    }

    public AdvertismentsView LoadAdvertisments(int sectionId, AdvertismentsState advertismentsFilter, DateTime date, int? subSectionId = null, int offset = 0, int limit = 100)
    {
        return LoadAdversitments(
            advertismentsFilter,
            date.Date, 
            date.Date.AddDays(1),
            sectionId, 
            subSectionId, 
            offset, 
            limit);
    }

    public void MarkNotByTheme(int advertisment_id)
    {
        if (Context != null)
        {
            var adv = Context.Advertisments.FirstOrDefault(a => a.Id == advertisment_id);
            if (adv != null)
            {
                adv.not_realestate = true;
                Context.SaveChanges();
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
                Context.SaveChanges();
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
                Context.SaveChanges();
            }
        }
    }

    #region Advertisments loading
    private static bool AdvertismentsNotLoaded(AdvertismentsView adversitmentsAndFullParsedCount)
    {
        return adversitmentsAndFullParsedCount == null
               || (adversitmentsAndFullParsedCount != null
                   && adversitmentsAndFullParsedCount.Advertisments != null
                   && adversitmentsAndFullParsedCount.Advertisments.All(a => a.isSpecial));
    }

    private AdvertismentsView LoadTodayAdversitments(AdvertismentsState advertismentState, int sectionId, int? subSectionId = null, int offset = 0, int limit = 100)
    {
        return LoadAdversitments(
            advertismentState,
            Utils.GetUkranianDateTimeNow().Date,
            Utils.GetUkranianDateTimeNow().Date.AddDays(1),
            sectionId,
            subSectionId,
            offset,
            limit);
    }
    private AdvertismentsView LoadYesterdayAdversitments(AdvertismentsState advertismentState, int sectionId, int? subSectionId = null, int offset = 0, int limit = 100)
    {
        return LoadAdversitments(
            advertismentState,
            Utils.GetUkranianDateTimeNow().AddDays(-1).Date,
            Utils.GetUkranianDateTimeNow().Date,
            sectionId,
            subSectionId,
            offset,
            limit);
    }

    private AdvertismentsView LoadAdversitments(AdvertismentsState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo, int sectionId = 1, int? subSectionId = null, int offset = 0, int limit = 100)
    {
        IQueryable<Advertisment> advertisments = FilterAdversitments(dateTimeFrom, dateTimeTo);
        int advertismentsCount = 0;
        int advertismentsToShowCount = 0;
        if (advertisments != null)
        {
            advertisments = advertisments.Where(adv => adv.AdvertismentSection.Id == sectionId);

            if(subSectionId != null)
                advertisments = advertisments.Where(adv => adv.AdvertismentSubSection.Id == subSectionId.Value);

            advertismentsCount = advertisments.Count();

            switch (advertismentState)
            {
                case AdvertismentsState.JustParsed:
                    advertisments = advertisments.Where(
                        adv => adv.subpurchaseAdvertisment && adv.SubPurchase == null
                        );
                    break;
                case AdvertismentsState.NotSubpurchase:
                    advertisments = advertisments.Where(
                        adv => !adv.subpurchaseAdvertisment
                        );
                    break;
                case AdvertismentsState.Subpurchase:
                    advertisments = advertisments.Where(
                        adv => adv.subpurchaseAdvertisment && adv.SubPurchase != null
                        );
                    break;
                case AdvertismentsState.SubpurchaseWithNotSubpurchase:
                    advertisments = advertisments.Where(
                        adv => !adv.subpurchaseAdvertisment || adv.SubPurchase != null
                        );
                    break;
            }

            advertismentsToShowCount = advertisments.Count();
            advertisments = advertisments.Skip(offset).Take(limit);
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
                                 && adv.AdvertismentPhones.Any()
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
    private AdvertismentsView FormatResultAdversitments(IQueryable<Advertisment> advertisments, int fullAdvertismentsCountBeforeFilter, int advertismentsToShowCount)
    {
        //--- set last update time
        DateTime lastTimeUpdated;

        if (advertisments != null && advertisments.Any())
            lastTimeUpdated = advertisments.Max(adv => adv.createDate);
        else lastTimeUpdated = DateTime.Now;

        return new AdvertismentsView(advertisments, fullAdvertismentsCountBeforeFilter, advertismentsToShowCount, lastTimeUpdated);
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