<%@ WebHandler Language="C#" Class="CottagesHousesWebFilter" %>

using System;
using System.Web;

public class CottagesHousesWebFilter : IHttpHandler {

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
            var filter = new FilteringController("cottages", @"~/Logs/cottagesfilter.log", "Cottages&HousesFilter");
            filter.StartFiltering();
        }    
    }
}