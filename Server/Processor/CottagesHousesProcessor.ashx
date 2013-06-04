<%@ WebHandler Language="C#" Class="CottagesHousesProcessor" %>

using System;
using System.Web;

public class CottagesHousesProcessor : IHttpHandler {

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        if (!string.IsNullOrWhiteSpace(context.Request["password"])
            && context.Request["password"] == "gtycbz")
        {
            var parser = new ParsingController("cottages", @"~/Logs/cottagesparse.log", "Cottages&Houses");
            parser.StartParsing();
        }
    }
}