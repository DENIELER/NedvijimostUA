using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using System.Text;


namespace Authorization
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User
    {
        public User()
        {
        }

        public static bool RegisterUser(string email, string login, string password, string phone, bool isSubpurchase)
        {
            var dataModel = new NedvijimostDBEntities();
            var user = new Model.User();
            user.Email = email;
            user.Login = login;
            user.Password = Utils.CalculateMD5Hash(password);
            user.Phone = phone;
            user.IsSubPurchase = isSubpurchase;

            dataModel.AddToUsers(user);
            dataModel.SaveChanges();

            return true;
        }
    }
}