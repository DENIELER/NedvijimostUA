using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SiteMVC.WebServices
{
    [ServiceContract]
    public interface ISocialMediaService
    {
        [OperationContract]
        [WebGet(UriTemplate = "SocialMedia/PostVkGroup?message={message}")]
        void PostVkGroup(string message);
    }
}
