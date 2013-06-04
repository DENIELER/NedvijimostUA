<%@ WebHandler Language="C#" Class="RentHousesWebFilter" %>

using System;
using System.Web;

public class RentHousesWebFilter : IHttpHandler {

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
            var filter = new FilteringController("rent", @"~/Logs/renthousefilter.log", "RentHousesFilter");
            filter.StartFiltering();
        }    
    }
}