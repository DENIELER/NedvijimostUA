using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.Engine
{
    public class AdvertismentsRequest
    {
        public AdvertismentsRequest()
        {
        }

        #region common parameters
        public int SectionId { get; set; }
        public int? SubSectionId { get; set; }

        private State _state = State.NotSubpurchase;
        public State State 
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        private int _offset = 0;
        public int Offset 
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }

        private int _limit = 10;
        public int Limit 
        {
            get
            {
                return _limit;
            }
            set
            {
                _limit = value;
            }
        }
        public string Url { get; set; }
        #endregion common parameters

        /// <summary>
        /// Using to show section name in archive
        /// </summary>
        private string _sectionName;
        public string SectionName 
        { 
            get
            {
                return _sectionName ?? string.Empty;
            }
            set
            {
                _sectionName = value;
            }
        }

        /// <summary>
        /// Can be set title of page
        /// </summary>
        public string SectionTitle { get; set; }
        /// <summary>
        /// Page desciption
        /// </summary>
        public string SectionDescription { get; set; }
        /// <summary>
        /// Page Keywords
        /// </summary>
        public string SectionKeywords { get; set; }
        /// <summary>
        /// Page Header text
        /// </summary>
        public string SectionHeader { get; set; }

        private string _date;
        public string Date 
        {
            get 
            { 
                return _date; 
            }
            set
            {
                _date = value;

                DateTime _tempDate;
                if (DateTime.TryParse(_date, out _tempDate))
                    _dateFrom = _tempDate;
                else 
                    _date = null;
            }
        }
        private DateTime? _dateFrom;
        public DateTime? DateFrom 
        {
            get
            {
                return _dateFrom;
            }

            set
            {
                _dateFrom = value;
            }
        }
        private DateTime? _dateTo;
        public DateTime? DateTo 
        {
            get
            {
                if (_dateTo != null && _dateFrom != null)
                {
                    return _dateTo;
                }
                else if (_dateTo == null && _dateFrom != null)
                {
                    return _dateFrom.Value.Date.AddDays(1);
                }
                else return null;
            }
            set
            {
                _dateTo = value;
            }
        }

        public bool OnlyWithPhotos { get; set; }
    }
}