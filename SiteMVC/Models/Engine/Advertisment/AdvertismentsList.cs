using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SiteMVC.Models.Engine.Advertisment
{
    [DataContract]
    public class AdvertismentsList
    {
        public AdvertismentsList(List<Models.ModelInterlayerObjects.Advertisment> advertisments, int fullCount, int countToShow, int countToShowAfterFilters, DateTime date)
        {
            _advertisments = advertisments;
            _fullCount = fullCount;
            _countToShow = countToShow;
            _countToShowAfterFilter = countToShowAfterFilters;
            _date = date;
        }

        public AdvertismentsList(IEnumerable<viewAdvertisment> advertisments, int fullCount, int countToShow, int countToShowAfterFilters, DateTime date)
        {
            _advertisments = advertisments.Select(x => new Models.ModelInterlayerObjects.Advertisment(x)).ToList();
            _fullCount = fullCount;
            _countToShow = countToShow;
            _countToShowAfterFilter = countToShowAfterFilters;
            _date = date;
        }

        private List<Models.ModelInterlayerObjects.Advertisment> _advertisments;
        private int _fullCount;
        private int _countToShow;
        private int _countToShowAfterFilter;
        private DateTime _date;

        [DataMember]
        public List<Models.ModelInterlayerObjects.Advertisment> Advertisments
        {
            get { return _advertisments; }
        }
        [DataMember]
        public int FullCount
        {
            get { return _fullCount; }
        }
        [DataMember]
        public int CountToShow
        {
            get { return _countToShow; }
        }
        [DataMember]
        public int CountToShowAfterFilter
        {
            get { return _countToShowAfterFilter; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return _date; }
        }

        public int Offset { get; set; }
        public int Limit { get; set; }

        public void SetAdvertismets(List<Models.ModelInterlayerObjects.Advertisment> list)
        {
            _advertisments = list;
        }
    }

    [DataContract]
    public class ExternalAdvertisment
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
    }
}