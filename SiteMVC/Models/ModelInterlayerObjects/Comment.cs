using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.ModelInterlayerObjects
{
    public class Comment
    {
        public Comment(AdvertismentComment comment)
        {
            this.CommentID = comment.CommentID;
            this.Message = comment.Message;
            this.CreateDate = comment.CreateDate;
            this.AdvertismentID = comment.AdvertismentID;

            this.Login = comment.Login;
            this.User = new User(comment.User);
        }

        private string Login { get; set; }

        public Guid CommentID { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public int AdvertismentID { get; set; }
        public User User { get; set; }

        public string DisplayLogin
        {
            get
            {
                string resultLogin = "Аноним";
                if (!string.IsNullOrEmpty(Login))
                    resultLogin = Login;
                else if (User != null)
                    resultLogin = User.DisplayLogin;

                return resultLogin.Length > 16 
                    ? resultLogin.Substring(0, 15) + ".."
                    : resultLogin;
            }
        }

        public string CreateDateDisplay
        {
            get
            {
                return this.CreateDate.ToShortDateString() + " " + this.CreateDate.ToShortTimeString();
            }
        }
    }
}