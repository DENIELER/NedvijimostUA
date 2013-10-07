<%@ WebHandler Language="C#" Class="RentHousesProcessor3" %>

using System;
using System.Web;

public class RentHousesProcessor3 : IHttpHandler {

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
            var parser = new ParsingController("rent", "RentHousesCrawler_3", "RentHouses", 3, "Kiev");
            parser.StartParsing();
        }
    }
}