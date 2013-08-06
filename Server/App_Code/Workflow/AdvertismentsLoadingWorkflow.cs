using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

/// <summary>
/// Summary description for AdvertismentsWorkflow
/// </summary>
public class AdvertismentsLoadingWorkflow : BaseContextWorkflow
{
    #region Ctor
    public AdvertismentsLoadingWorkflow(DataModel context)
        : base(context)
	{
	}
    #endregion Ctor

    #region Public Methods
    public IList<Model.Advertisment> LoadTodayAdversitments(AdvertismentState advertismentState, string sectionCode)
    {
        return LoadAdversitments(
            advertismentState,
            Utils.GetUkranianDateTimeNow().Date,
            Utils.GetUkranianDateTimeNow().Date.AddDays(1),
            sectionCode);
    }
    public IList<Model.Advertisment> LoadAdversitments(AdvertismentState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo, string sectionCode)
    {
        IQueryable<Advertisment> advertisments = LoadAdvertismentsByDate(advertismentState, dateTimeFrom, dateTimeTo);
        int advertismentsCount = 0;
        if (advertisments != null)
        {
            advertisments = advertisments.Where(adv => adv.AdvertismentSection.code == sectionCode);
            advertismentsCount = advertisments.Count();

            switch (advertismentState)
            {
                case AdvertismentState.JustParsed:
                    advertisments = advertisments.Where(
                        adv => adv.subpurchaseAdvertisment && adv.SubPurchase == null
                        );
                    break;
                case AdvertismentState.NotSubpurchase:
                    advertisments = advertisments.Where(
                        adv => !adv.subpurchaseAdvertisment
                        );
                    break;
                case AdvertismentState.Subpurchase:
                    advertisments = advertisments.Where(
                        adv => adv.subpurchaseAdvertisment && adv.SubPurchase != null
                        );
                    break;
            }
        }

        advertisments = advertisments.Where(a => a.AdvertismentPhones.Any());

        return advertisments.ToList();
    }
    #endregion Public Methods

    #region Private Methods
    private IQueryable<Advertisment> LoadAdvertismentsByDate(AdvertismentState advertismentState, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        IQueryable<Advertisment> searchResults = from adv in context.Advertisments
                             where 
                                !adv.isSpecial
                                && adv.createDate >= dateTimeFrom.Date
                                && adv.createDate < dateTimeTo.Date
                                && !adv.not_realestate
                             select adv;

        return searchResults;
    }
    #endregion Private Methods
}
