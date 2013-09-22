<%@ WebHandler Language="C#" Class="RentHousesProcessor1" %>

using System;
using System.Web;

public class RentHousesProcessor1 : IHttpHandler {

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
            string city = context.Request["city"];
            
            var parser = new ParsingController("rent", "RentHousesCrawler_1", "RentHouses", 1, city);
            parser.StartParsing();
        }
    }
}