using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    /// <summary>
    /// Leave server alive
    /// </summary>
    public static void PingServer()
    {
        try
        {
            WebClient http = new WebClient();
            string Result = http.DownloadString(Resources.Constants.PingServerUrl);
        }
        catch
        { }
    }

    public static byte[] CalculateMD5Hash(string input)
    {
        var md5Hasher = System.Security.Cryptography.MD5.Create();
        List<byte> bytes = new List<byte>();
        string leaveString = input;
        
        int length = leaveString.Length;
        while (length >= 8000)
        {
            string substring = leaveString.Substring(0, 8000);
            bytes.AddRange(
                md5Hasher.ComputeHash(
                    System.Text.Encoding.Default.GetBytes(substring)
                )
            );

            leaveString = leaveString.Remove(0, 8000);
            length = leaveString.Length;
        }

        bytes.AddRange(
                md5Hasher.ComputeHash(
                    System.Text.Encoding.Default.GetBytes(leaveString)
                )
            );
        return bytes.ToArray();
    }

    public static string HashToHex(byte[] bytes, bool upperCase)
    {
        System.Text.StringBuilder result = new System.Text.StringBuilder(bytes.Length * 2);

        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

        return result.ToString();
    }
}