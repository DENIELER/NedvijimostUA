using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Loggin information
/// </summary>
public class Log
{
    private string _logFile = @"~/Logs/renthouse_log.log";

    public Log()
	{}

    public Log(string filename)
    {
        _logFile = filename;
    }

    public void WriteLog(string message)
    {
        var sw = new StreamWriter(GetFilePath(_logFile), true);
        try
        {
            var ukraineTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");

            sw.WriteLine("[" + TimeZoneInfo.ConvertTime(DateTime.Now, ukraineTimeZoneInfo).ToString() + "] " + message);
        }
        finally
        {
            sw.Close();
        }
    }

    private string GetFilePath(string filename)
    {
        return System.Web.Hosting.HostingEnvironment.MapPath(filename);
    }
}