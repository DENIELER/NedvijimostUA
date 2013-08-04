using SiteMVC.App_Code;
using SiteMVC.Models;
using SiteMVC.Models.Engine;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SiteMVC.WebServices
{
    public class ExternalService : IExternalService
    {
        public IEnumerable<ExternalAdvertisment> KharkovCapital_RentAdvertisments()
        {
            var advertismentsLoader = new AdvertismentsLoader();

            var request = new AdvertismentsRequest()
            {
                State = State.NotSubpurchase,
                SectionId = 1, //--- rent appartments
                SubSectionId = 1, //--- only rent
                Offset = 0,
                Limit = 500,
                Filter = new AdvertismentsFilter()
                         {
                             OnlyNew = true
                         }
            };
            advertismentsLoader.SetTodayDate(request);
            
            AdvertismentsList advertismentsList = advertismentsLoader.LoadAdversitments(request);
            if (advertismentsList == null || advertismentsList.Advertisments == null)
                return null;

            return advertismentsList.Advertisments
                   .Select(a =>
                    {
                        return new ExternalAdvertisment()
                        {
                            Text = a.Text,
                            Price = a.Price
                        };
                    });
        }
    }
}
