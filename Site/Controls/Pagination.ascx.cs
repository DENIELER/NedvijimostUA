using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Pagination : System.Web.UI.UserControl
{
    public int ElementsCount { get; set; }
    public int ElementsPerPage { get; set; }
    public int CurrentPageNum { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected string GetCurrentUrlWithPageNum(int pageNum)
    {
        string rawUrl = Request.RawUrl;

        var parameters = new RouteValueDictionary(Request.Url.Query);
        parameters["page_num"] = pageNum;

        return RouteTable.Routes.GetVirtualPath(null, "Advertisments", parameters).VirtualPath;
    }
}