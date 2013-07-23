using SiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SiteMVC.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AdminService.svc or AdminService.svc.cs at the Solution Explorer and start debugging.
    public class AdminService : IAdminService
    {
        public bool RemoveAdvertisment(string adverisment_id, string password)
        {
            if (password == "gtycbz")
            {
                int _adverisment_id;
                if(!int.TryParse(adverisment_id, out _adverisment_id))
                    throw new FaultException("Cannot parse adverisment_id parameter");

                var dataModel = new DataModel();

                var advertisment = dataModel.Advertisments
                    .SingleOrDefault(a => a.Id == _adverisment_id);
                if (advertisment == null)
                    throw new FaultException("Advertisment not found");

                advertisment.not_show_advertisment = true;

                dataModel.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}
