using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace SystemUtils
{
    public class Authorization
    {
        public static bool LogIn(string loginOrEmail, string password, bool rememberMe)
        {
            string passwordMD5 = Utils.CalculateMD5Hash(password);

            var dataModel = new SiteMVC.Models.DataModel();
            var currentUser = dataModel.Users
                .FirstOrDefault(u => (u.Login == loginOrEmail || u.Email == loginOrEmail)
                    && u.Password == passwordMD5);

            if (currentUser != null)
            {
                string userData = currentUser.IsAdmin + "|" + currentUser.Phone + "|" + currentUser.UserID + "|";

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
        public static bool LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);

            HttpContext.Current.Response.Redirect("~/");
            return true;
        }

        public static bool IsAuthorized
        {
            get
            {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                    return true;
                return false;
            }
        }
        public static bool IsAdmin
        {
            get
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
        }

        public static string Login
        {
            get
            {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                    return null;

                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                return ticket.Name;
            }
        }
        public static string Phone
        {
            get
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
        }
        public static int? UserID
        {
            get
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
        }
    }
}