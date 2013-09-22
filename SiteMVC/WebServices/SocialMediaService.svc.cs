using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SiteMVC.WebServices
{
    public class SocialMediaService : ISocialMediaService
    {
        public void PostVkGroup(string message)
        {
            string appId = "3111027";
            string appSecret = "FSApuxGl1jU3CpgG1Fra";
            string groupId = "43123714";

            var vk = new SiteMVC.App_Code.Vkontakte(appId, appSecret);
            vk.Authorize();

            vk.PostWall(groupId, message);
        }
    }
}
