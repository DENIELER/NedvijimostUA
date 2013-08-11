using SiteMVC.Models;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.App_Code
{
    public class AdvertismentsLoader
    {
        #region Ctor
        public AdvertismentsLoader()
        {
        }
        #endregion Ctor

        #region Methods
        public AdvertismentsList LoadAdversitments(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            IQueryable<viewAdvertisment> advertisments = LoadAdvertismentsByDate(request.DateFrom.Value, request.DateTo.Value);
            int fullCount = 0;
            int countToShow = 0;
            int countToShowAfterFilter = 0;
            if (advertisments != null)
            {
                advertisments = advertisments.Where(adv => adv.AdvertismentSection_Id == request.SectionId);

                if (request.SubSectionId != null)
                    advertisments = advertisments.Where(adv => adv.AdvertismentSubSection_Id == request.SubSectionId.Value);

                fullCount = advertisments.Count();

                switch (request.State)
                {
                    case State.JustParsed:
                        advertisments = advertisments.Where(
                            adv => adv.subpurchaseAdvertisment && adv.SubPurchase_Id == null
                            );
                        break;
                    case State.NotSubpurchase:
                        advertisments = advertisments.Where(
                            adv => !adv.subpurchaseAdvertisment
                            );
                        break;
                    case State.Subpurchase:
                        advertisments = advertisments.Where(
                            adv => adv.subpurchaseAdvertisment && adv.SubPurchase_Id != null
                            );
                        break;
                    case State.SubpurchaseWithNotSubpurchase:
                        advertisments = advertisments.Where(
                            adv => !adv.subpurchaseAdvertisment || adv.SubPurchase_Id != null
                            );
                        break;
                }

                countToShow = advertisments.Count();

                //--- special filters
                if (request.Filter != null)
                    ApplyFilters(request.Filter, ref advertisments);

                countToShowAfterFilter = advertisments.Count();
                advertisments = advertisments.Skip(request.Offset).Take(request.Limit);
            }

            DateTime lastTimeUpdated;
            if (advertisments != null && advertisments.Any())
                lastTimeUpdated = advertisments.Max(adv => adv.modifyDate);
            else lastTimeUpdated = DateTime.Now;

            return new AdvertismentsList(advertisments, fullCount, countToShow, countToShowAfterFilter, lastTimeUpdated);
        }
        
        public bool IsLoaded(AdvertismentsList response)
        {
            return response != null
                   && response.Advertisments != null
                   && response.Advertisments.Count > 0
                   && !response.Advertisments.All(a => a.IsSpecial);
        }

        public void SetTodayDate(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            request.DateFrom = SystemUtils.Utils.Date.GetUkranianDateTimeNow().Date;
            request.DateTo = SystemUtils.Utils.Date.GetUkranianDateTimeNow().Date.AddDays(1);
        }

        public void SetYesterdayDate(SiteMVC.Models.Engine.AdvertismentsRequest request)
        {
            request.DateFrom = SystemUtils.Utils.Date.GetUkranianDateTimeNow().AddDays(-1).Date;
            request.DateTo = SystemUtils.Utils.Date.GetUkranianDateTimeNow().Date;
        }
        #endregion Methods

        #region Private Methods
        private void ApplyFilters(Models.Engine.AdvertismentsFilter filter, ref IQueryable<viewAdvertisment> advertisments)
        {
            if (filter == null)
                throw new Exception("Advertisments filter is null");

            if (filter.OnlyWithPhotos)
                advertisments = advertisments.Where(adv => adv.AdvertismentsPhotos.Any());

            if (!string.IsNullOrWhiteSpace(filter.Text))
                advertisments = advertisments.Where(adv => adv.text.Contains(filter.Text));

            if (filter.OnlyNew)
                advertisments = advertisments.Where(adv => adv.CountByTextColumn == 1);

            if (filter.NearUndeground)
                advertisments = advertisments.Where(adv => adv.UndergroundStationID != null);
        }

        private IQueryable<viewAdvertisment> LoadAdvertismentsByDate(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var dataModel = new Models.DataModel();

            var specialFromDateTime = dateTimeFrom.Date.AddDays(-7);

            IQueryable<viewAdvertisment> searchResults =
                dataModel.viewAdvertisments
                .Where(a =>
                    ((!a.isSpecial && a.modifyDate >= dateTimeFrom.Date) || (a.isSpecial && a.modifyDate >= specialFromDateTime.Date))
                    && a.modifyDate < dateTimeTo.Date
                    && !a.not_realestate
                    && !a.not_show_advertisment)
                .OrderByDescending(a => a.isSpecial)
                .OrderByDescending(a => a.modifyDate);
            return searchResults;
        }
        #endregion Private Methods
    }
}