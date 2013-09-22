<%@ WebHandler Language="C#" Class="SaleHousesProcessor1" %>

using System;
using System.Web;

public class SaleHousesProcessor1 : IHttpHandler {

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
            
            var parser = new ParsingController("sale", "SaleHousesCrawler_1", "SaleHouses", 1, city);
            parser.StartParsing();
        }
    }
}