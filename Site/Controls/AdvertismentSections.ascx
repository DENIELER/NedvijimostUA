<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdvertismentSections.ascx.cs" Inherits="Controls_AdvertismentSections" %>

<div class="row-fluid">
    <ul class="nav nav-pills">
        <%
            foreach (var section in AdvertismentSections)
            {
                string paramSeparatorUrl = string.Empty;
                if(string.IsNullOrEmpty(Request.Url.Query))
                    paramSeparatorUrl = "section=" + section.Id;
                else
                {
                    NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                    if (!string.IsNullOrEmpty(queryString["section"]))
                    {
                        queryString["section"] = section.Id.ToString();
                    }
                    else
                        queryString.Add("section", section.Id.ToString());

                    paramSeparatorUrl = queryString.ToString();
                }                            
                            
                if (section.Id == CurrentSection)
                {
                %><li class="active"><a href="<%= Request.Url.AbsolutePath + "?" + paramSeparatorUrl %>"><%= section.displayName %></a></li><%   
                }else
                {
                %><li><a href="<%= Request.Url.AbsolutePath + "?" + paramSeparatorUrl %>"><%= section.displayName %></a></li><%
                }
            }
        %>
    </ul>
</div>