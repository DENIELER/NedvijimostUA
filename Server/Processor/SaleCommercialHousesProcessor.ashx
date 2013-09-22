<%@ WebHandler Language="C#" Class="SaleCommercialHousesProcessor" %>

using System;
using System.Web;

public class SaleCommercialHousesProcessor : IHttpHandler {

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
            
            var parser = new ParsingController("sale_commercial", "SaleCommercialHousesCrawler", "SaleCommercialHouses", null, city);
            parser.StartParsing();
        }
    }
}