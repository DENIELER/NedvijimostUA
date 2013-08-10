using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.ModelInterlayerObjects
{
    public class User
    {
        public User(Models.User user)
        {
            if (user != null)
            {
                this.UserID = user.UserID;
                this.Login = user.Login;
                this.Email = user.Email;
            }
        }

        private string Login { get; set; }
        private string Email { get; set; }

        public int UserID { get; set; }
        
        public string DisplayLogin 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Login))
                    return Login;
                else
                {
                    if (this.Email != null)
                    {
                        int emailSeparatorIndex = this.Email.IndexOf("@");
                        if (emailSeparatorIndex > 0)
                            return this.Email.Substring(0, emailSeparatorIndex);
                    }
                    return "Аноним";
                }
            }
        }
    }
}