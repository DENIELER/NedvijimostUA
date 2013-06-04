using System;
using System.Collections.Generic;
using System.Linq;
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
}