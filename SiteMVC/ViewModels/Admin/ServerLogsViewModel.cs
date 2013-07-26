using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels.Admin
{
    public class ServerLogsViewModel
    {
        public ServerLogsViewModel()
        {
            LogMessages = new Dictionary<SeviceDescription, IEnumerable<LogLine>>();
        }

        public IEnumerable<SeviceDescription> ServiceCodes { get; set; }

        public Dictionary<SeviceDescription, IEnumerable<LogLine>> LogMessages { get; set; }

        public class SeviceDescription
        {
            public string ServiceCode { get; set; }
            public string Url { get; set; }
        }

        public class LogLine
        {
            public string Message { get; set; }
            public DateTime Date { get; set; }
        }
    }
}