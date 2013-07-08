using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SystemUtils
{
    public class Utils
    {
        public Utils()
        {
        }

        public static class Date
        {
            public static DateTime GetUkranianDateTimeNow()
            {
                var ukraineTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
                return TimeZoneInfo.ConvertTime(DateTime.Now, ukraineTimeZoneInfo);
            }

            public static DateTime GetUkranianDateTimeNow(DateTime datetime)
            {
                var ukraineTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
                return TimeZoneInfo.ConvertTime(datetime, ukraineTimeZoneInfo);
            }
        }

        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<[^>]*>", string.Empty).Replace("•", "").Replace(@"\r\n", "").Trim();
            //return Regex.Replace(source, "<.*?>", string.Empty).Replace("•", "").Replace(@"\r\n", "").Trim();
        }

        public static string StripAllTextBetweenTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>.*</.*?>", string.Empty).Replace("•", "").Replace(@"\r\n", "").Trim();
        }

        public static string CalculateMD5Hash(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            // step 1, calculate MD5 hash from input
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public static class Url
        {
            public static string InsertURLParam(string url, string key, string value)
            {
                var _url = url;
                if (_url.IndexOf(key + "=") >= 0)
                {
                    var prefix = _url.Substring(0, _url.IndexOf(key));
                    var suffixTemp = _url.Substring(_url.IndexOf(key));
                    var suffix = suffixTemp.Substring(suffixTemp.IndexOf("=") + 1);
                    suffix = (suffix.IndexOf("&") >= 0) ? suffix.Substring(suffix.IndexOf("&")) : "";
                    _url = prefix + key + "=" + value + suffix;
                }
                else
                {
                    if (_url.IndexOf("?") < 0)
                        _url += "?" + key + "=" + value;
                    else
                        _url += "&" + key + "=" + value;
                }
                return _url;
            }
        }
    }
}