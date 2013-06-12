using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Model;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var context = new Model.NedvijimostDBEntities();

            //AdvertismentsWithPhotosRotator.DBContext = context;
            SearchResultsChart.DBContext = context;
        }
    }

}