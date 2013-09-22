using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace SiteMVC.App_Code
{
    public class Vkontakte : SocialMedia
    {
        public Vkontakte(string appId, string appSecret)
        {
            AppID = appId;
            AppSecret = appSecret;
        }

        public void Authorize()
        {
            this.AccessToken = GetAccessToken();
        }

        public bool PostWall(string authorId, string message)
        {
            if (string.IsNullOrEmpty(this.AccessToken))
                throw new Exception("Access token parameter is missing. You should authorize using OAuth protocol before making transactions.");

            string request = string.Format(
                "https://api.vkontakte.ru/method/wall.post?owner_id={0}=&access_token={1}&message={2}",
                authorId, this.AccessToken, message);

            WebClient webClient = new WebClient();
            var response = webClient.DownloadString(request);

            var result = JsonConvert.DeserializeObject<dynamic>(response);
            if (result.response.post_id)
                return true;
            else throw new Exception(result.error.error_msg);
        }

        protected override string GetAccessToken()
        {
            if (string.IsNullOrEmpty(this.AppID))
                throw new Exception("AppID is missing.");
            if (string.IsNullOrEmpty(this.AppSecret))
                throw new Exception("AppSecret is missing.");

            string authUrl = string.Format("https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials",
                    HttpUtility.UrlEncode(AppID),
                    HttpUtility.UrlEncode(AppSecret));

            WebClient webClient = new WebClient();
            string response = webClient.DownloadString(authUrl);

            var result = JsonConvert.DeserializeObject<dynamic>(response);
            return result.access_token;
        }

        private Dictionary<int, string> errorMessages = new Dictionary<int, string>()
            {
                {1, "Unknown error occurred."},
                {2, "Application is disabled. Enable your application or use test mode."},
                {4, "Incorrect signature."},
                {5, "User authorization failed."},
                {6, "Too many requests per second."},
                {7, "Permission to perform this action is denied by user."},
                {14, "Captcha is needed."},
                {100, "One of the parameters specified was missing or invalid."},
                {214, "Access to adding post denied."},
                {10005, "Too frequently."},
                {10007, "Operation denied by user."}
            };
    }
}