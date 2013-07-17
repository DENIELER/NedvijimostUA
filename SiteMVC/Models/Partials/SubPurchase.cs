using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SiteMVC.Models
{
    public partial class SubPurchase
    {
        public List<string> GetPhoneFormatsList(string phone)
        {
            List<string> phones = new List<string>();

            string temp = phone.Replace("+", "").Replace(" ", "").Replace("-", "").Trim();
            if (temp.StartsWith("8"))
                temp = temp.Substring(1);
            else if (temp.StartsWith("38"))
                temp = temp.Substring(2);

            if (temp.Length == 7)
            {
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{2})(\d{2})(\d{3})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{1})(\d{3})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{1})(\d{3})(\d{3})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{2})(\d{3})(\d{2})", @"$1-$2-$3"));
            }
            else if (temp.Length == 10)
            {
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{2})(\d{2})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{7})", @"$1-$2"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{5})(\d{2})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{4})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{4})(\d{3})", @"$1-$2-$3"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{1})(\d{3})(\d{3})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{3})(\d{1})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{1})(\d{3})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})(\d{3})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{4})(\d{1})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{1})(\d{2})(\d{2})(\d{2})", @"$1-$2-$3-$4-$5"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{3})(\d{2})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{1})(\d{3})", @"$1 $2 $3 $4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{2})(\d{2})", @"$1 $2 $3 $4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})(\d{3})", @"$1 $2 $3 $4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{4})(\d{1})", @"$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{4})", @"$1 $2 $3"));
                phones.Add(Regex.Replace(temp, @"(\d{10})", @"$1"));
                phones.Add(Regex.Replace(temp, @"(\d{10})", @"8$1"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})(\d{3})", @"8-$1-$2-$3-$4"));
                phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{2})(\d{2})", @"8-$1-$2-$3-$4"));
            }
            else
            {
                phones.Add(phone);
            }

            phones.ForEach(p =>
            {
                if (string.IsNullOrEmpty(p))
                    phones.Remove(p);
            });
            return phones;
        }
    }
}