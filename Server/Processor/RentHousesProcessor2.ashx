<%@ WebHandler Language="C#" Class="RentHousesProcessor2" %>

using System;
using System.Web;

public class RentHousesProcessor2 : IHttpHandler {

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
            var parser = new ParsingController("rent", "RentHousesCrawler_2", "RentHouses", 2, "Dnepropetrovsk");
            parser.StartParsing();
        }
    }
}