using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for Utils
/// </summary>
public class Utils
{
	public Utils()
	{
	}

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
}