using SiteMVC.Models;
using SiteMVC.Models.Engine.Advertisment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SiteMVC.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExternalService" in both code and config file together.
    [ServiceContract]
    public interface IExternalService
    {
        [OperationContract]
        [WebGet(UriTemplate = "KharkovCapital/Advertisments/Rent")]
        IEnumerable<ExternalAdvertisment> KharkovCapital_RentAdvertisments();
    }
}
