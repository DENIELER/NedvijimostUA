using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SiteMVC.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdminService" in both code and config file together.
    [ServiceContract]
    public interface IAdminService
    {
        [OperationContract]
        [WebGet(UriTemplate = "RemoveAdvertisment/{adverisment_id}/{password}")]
        bool RemoveAdvertisment(string adverisment_id, string password);
    }
}
