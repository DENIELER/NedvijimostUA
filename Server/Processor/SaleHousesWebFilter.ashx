<%@ WebHandler Language="C#" Class="SaleHousesWebFilter" %>

using System;
using System.Web;

public class SaleHousesWebFilter : IHttpHandler {

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
            var filter = new FilteringController("sale", "SaleHousesWebFilter", "SaleHousesFilter");
            filter.StartFiltering();
        }    
    }
}