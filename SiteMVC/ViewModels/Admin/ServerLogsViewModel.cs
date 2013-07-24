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
            LogMessages = new Dictionary<string, IEnumerable<Tuple<string, DateTime>>>();
        }

        public IEnumerable<string> ServiceCodes { get; set; }
        public Dictionary<string, IEnumerable<Tuple<string, DateTime>>> LogMessages { get; set; }
    }
}