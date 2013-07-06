using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nedvijimost
{
    public class AdvertismentsRequest
    {
        public AdvertismentsRequest()
        {
        }

        #region common parameters
        public int SectionId { get; set; }
        public int? SubSectionId { get; set; }

        private AdvertismentsState _state = AdvertismentsState.NotSubpurchase;
        public AdvertismentsState State 
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