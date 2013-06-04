﻿<%@ WebHandler Language="C#" Class="RentHousesProcessor" %>

using System;
using System.Web;

public class RentHousesProcessor : IHttpHandler {

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
            var parser = new ParsingController("rent", @"~/Logs/renthouseparse.log", "RentHouses");
            parser.StartParsing();
        }
    }
}