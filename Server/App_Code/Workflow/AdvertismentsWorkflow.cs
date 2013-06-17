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
    public AdvertismentsWorkflow()
    {
        Context = new DataModel();
    }

	public AdvertismentsWorkflow(DataModel context)
	{
        Context = context;
	}

    public DataModel Context { get; set; }

    public AdvertismentsView LoadTodayAdversitments(AdvertismentsState advertismentState, int sectionId)
    {
        return LoadAdversitments(
            advertismentState,
            Utils.GetUkranianDateTimeNow().Date,
            Utils.GetUkranianDateTimeNow().Date.AddDays(1),
            sectionId);
    }
    public AdvertismentsView LoadYesterdayAdversitments(AdvertismentsState advertismentState, int sectionId)
    {
        return LoadAdversitments(
            advertismentState,
            Utils.GetUkranianDateTimeNow().AddDays(-1).Date,
            Utils.GetUkranianDateTimeNow().Date,
            sectionId);
    }
    public AdvertismentsView LoadTodayAdversitments(AdvertismentsState advertismentState, string sectionCode)
    {
        return LoadAdversitments(
            advertismentState,
            Utils.GetUkranianDateTimeNow().Date,
            Utils.GetUkranianDateTimeNow().Date.AddDays(1),
            sectionCode);
    }
    
    public AdvertismentsView LoadAdversitments(AdvertismentsState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo, int sectionId = 1)
    {
        IQueryable<Advertisment> advertisments = FilterAdversitments(advertismentState, dateTimeFrom, dateTimeTo);
        int advertismentsCount = 0;
        if (advertisments != null)
        {
            advertisments = advertisments.Where(adv => adv.AdvertismentSection.Id == sectionId);
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
        }

        return FormatResultAdversitments(advertisments, advertismentsCount);
    }
    public AdvertismentsView LoadAdversitments(AdvertismentsState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo, string sectionCode)
    {
        IQueryable<Advertisment> advertisments = FilterAdversitments(advertismentState, dateTimeFrom, dateTimeTo);
        int advertismentsCount = 0;
        if (advertisments != null)
        {
            advertisments = advertisments.Where(adv => adv.AdvertismentSection.code == sectionCode);
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
            }
        }

        return FormatResultAdversitments(advertisments, advertismentsCount);
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

    private IQueryable<Advertisment> FilterAdversitments(AdvertismentsState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo)
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
    private AdvertismentsView FormatResultAdversitments(IQueryable<Advertisment> advertisments, int fullAdvertismentsCountBeforeFilter)
    {
        //--- set last update time
        DateTime lastTimeUpdated;

        if (advertisments != null && advertisments.Any())
            lastTimeUpdated = advertisments.Max(adv => adv.createDate);
        else lastTimeUpdated = DateTime.Now;

        return new AdvertismentsView(advertisments.ToList(), fullAdvertismentsCountBeforeFilter, lastTimeUpdated);
    }
    internal string GetAdvertismentText(int advertisment_id)
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
}

public enum AdvertismentsState
{
    JustParsed,
    Subpurchase,
    NotSubpurchase,
    SubpurchaseWithNotSubpurchase
}