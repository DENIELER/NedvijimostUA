using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Model;

namespace Authorization
{
    public class Authorization
    {
        public Authorization()
        {

        }

        public static bool Login(string loginOrEmail, string password, bool rememberMe)
        {
            string passwordMD5 = Utils.CalculateMD5Hash(password);

            var dataModel = new NedvijimostDBEntities();
            var currentUser = dataModel.Users
                .FirstOrDefault(u => (u.Login == loginOrEmail || u.Email == loginOrEmail)
                    && u.Password == passwordMD5);

            if (currentUser != null)
            {
                string userData = currentUser.IsAdmin + "|" + currentUser.Phone + "|" + currentUser.UserId + "|";

                // Create forms authentication ticket
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, // Ticket version
                loginOrEmail, // Username to be associated with this ticket
                DateTime.Now, // Date/time ticket was issued
                DateTime.Now.AddMinutes(50), // Date and time the cookie will expire
                rememberMe,
                userData,
                FormsAuthentication.FormsCookiePath);

                // To give more security it is suggested to hash it
                string hashCookies = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies); // Hashed ticket

                // Add the cookie to the response, user browser
                HttpContext.Current.Response.Cookies.Add(cookie);// Get the requested page from the url
                string returnUrl = HttpContext.Current.Request.QueryString["ReturnUrl"];

                // check if it exists, if not then redirect to default page
                if (returnUrl == null) returnUrl = "~/";
                HttpContext.Current.Response.Redirect(returnUrl);

                return true;
            }

            HttpContext.Current.Response.Redirect("~/failed-authorization");
            return false;
        }

        public static bool Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);

            HttpContext.Current.Response.Redirect("~/");
            return true;
        }

        public static bool IsUserAuthorized()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                return true;
            return false;
        }

        public static string CurrentUser_Login()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                return null;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            return ticket.Name;
        }

        public static bool CurrentUser_IsAdmin()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                return false;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string userData = ticket.UserData;
            string[] data = userData.Split('|');
            if (data == null || !data.Any())
                return false;

            bool isAdmin;
            if (bool.TryParse(data[0], out isAdmin) && isAdmin)
                return true;
            return false;
        }

        public static string CurrentUser_Phone()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                return null;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string userData = ticket.UserData;
            string[] data = userData.Split('|');
            if (data == null || data.Count() < 2)
                return null;

            return data[1];
        }

        public static int? CurrentUser_UserID()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                return null;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string userData = ticket.UserData;
            string[] data = userData.Split('|');
            if (data == null || data.Count() < 3)
                return null;

            int userID;
            if (int.TryParse(data[2], out userID))
                return userID;
            return null;
        }

        #region Vkontakte

        public static string Vk_uid_SessionKey = "VK_Uid";
        public static string Vk_FirstName_SessionKey = "Vk_FirstName";
        public static string Vk_LastName_SessionKey = "Vk_LastName";
        public static string Vk_Photo_SessionKey = "Vk_Photo";
        public static string Vk_PhotoRec_SessionKey = "Vk_PhotoRec";
        public static string Vk_Hash_SessionKey = "Vk_Hash";

        public bool AuthorizeVkontakteUser(string uid, string first_name, string last_name, string photo, string photo_small, string hash)
        {
            HttpContext.Current.Session[Vk_uid_SessionKey] = uid;
            HttpContext.Current.Session[Vk_FirstName_SessionKey] = first_name;
            HttpContext.Current.Session[Vk_LastName_SessionKey] = last_name;
            HttpContext.Current.Session[Vk_Photo_SessionKey] = photo;
            HttpContext.Current.Session[Vk_PhotoRec_SessionKey] = photo_small;
            HttpContext.Current.Session[Vk_Hash_SessionKey] = hash;

            string cookiestr;
            HttpCookie ck;
            var tkt = new FormsAuthenticationTicket(1, uid, DateTime.Now,
            DateTime.Now.AddHours(7), true, first_name + "|" + last_name + "|" + photo + "|" + photo_small);
            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            ck.Expires = tkt.Expiration;
            ck.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Add(ck);

            return true;
        }
        public bool LogoffVkontakteUser()
        {
            HttpContext.Current.Session.Remove(Vk_uid_SessionKey);
            HttpContext.Current.Session.Remove(Vk_FirstName_SessionKey);
            HttpContext.Current.Session.Remove(Vk_LastName_SessionKey);
            HttpContext.Current.Session.Remove(Vk_Photo_SessionKey);
            HttpContext.Current.Session.Remove(Vk_PhotoRec_SessionKey);
            HttpContext.Current.Session.Remove(Vk_Hash_SessionKey);
            FormsAuthentication.SignOut();

            return true;
        }

        public static bool IsUserVkontakteAuthorized()
        {
            string user_uid = HttpContext.Current.Session[Vk_uid_SessionKey] as string;
            string user_hash = HttpContext.Current.Session[Vk_Hash_SessionKey] as string;

            return !string.IsNullOrEmpty(user_uid)
                && !string.IsNullOrEmpty(user_hash)
               && user_hash == Utils.CalculateMD5Hash("3111027" + user_uid + "FSApuxGl1jU3CpgG1Fra");
        }
        public static bool IsAdmin(string uid)
        {
            return uid == "106154673"; // DENIELER account
        }

        public static string GetVkontakteUserName()
        {
            if (IsUserVkontakteAuthorized())
            {
                string user_firstname = HttpContext.Current.Session[Vk_FirstName_SessionKey] as string;
                string user_lastname = HttpContext.Current.Session[Vk_LastName_SessionKey] as string;

                return user_firstname + " " + user_lastname;
            }
            else return null;
        }
        public static string GetVkontakteUserSmallPhoto()
        {
            if (IsUserVkontakteAuthorized())
            {
                string user_photo = HttpContext.Current.Session[Vk_PhotoRec_SessionKey] as string;

                return user_photo;
            }
            else return null;
        }
        public static string GetVkontakteUserUid()
        {
            if (IsUserVkontakteAuthorized())
            {
                string user_uid = HttpContext.Current.Session[Vk_uid_SessionKey] as string;

                return user_uid;
            }
            else return null;
        }

        #endregion Vkontakte
        
    }
}