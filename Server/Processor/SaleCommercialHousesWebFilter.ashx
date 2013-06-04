<%@ WebHandler Language="C#" Class="SaleCommercialHousesWebFilter" %>

using System;
using System.Web;

public class SaleCommercialHousesWebFilter : IHttpHandler
{

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
            var filter = new FilteringController("sale_commercial", @"~/Logs/salecommercialhousefilter.log", "SaleCommercialHousesFilter");
            filter.StartFiltering();
        }    
    }
}