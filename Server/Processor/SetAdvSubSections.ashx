﻿<%@ WebHandler Language="C#" Class="SetAdvSubSections" %>

using System;
using System.Web;

public class SetAdvSubSections : IHttpHandler
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
            var parser = new SetAdvSubSectionController("SubSectionsSetter");
            parser.StartSettingSubSections();
        }
    }
}