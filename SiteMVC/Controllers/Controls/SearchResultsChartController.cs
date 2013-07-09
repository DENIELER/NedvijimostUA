using SiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers.Controls
{
    public class SearchResultsChartController : Controller
    {
        public ActionResult SearchResultsChartControl()
        {
            var searchResultsChart = new List<ViewModels.Controls.SearchResult>();

            var dataModel = new DataModel();
            searchResultsChart.AddRange(dataModel.ExecuteQuery<ViewModels.Controls.SearchResult>(@"
                                    select 
	                                    [date] = createDate, 
	                                    [fullCount] = searchresult_fullcount, 
	                                    [withoutSubPurchaseCount] = searchresult_count 
                                    from
                                    SearchResults sr
                                    JOIN (select COUNT(*) as searchresult_count, searchresult_id from Advertisments adv
                                    where searchresult_id in 
	                                    (select top 8 Id 
	                                    from SearchResults 
	                                    where AdvertismentSection_Id = 1
	                                    order by createDate desc)
	                                    and adv.subpurchaseAdvertisment = 0
	                                    and adv.SubPurchase_Id is NULL
                                    group by searchresult_id) srcount ON sr.Id = srcount.searchresult_id
                                    JOIN (select COUNT(*) as searchresult_fullcount, searchresult_id from Advertisments adv
                                    where searchresult_id in 
	                                    (select top 8 Id 
	                                    from SearchResults 
	                                    where AdvertismentSection_Id = 1
	                                    order by createDate desc)
                                    group by searchresult_id) srfullcount ON sr.Id = srfullcount.searchresult_id
                                    order by createDate;")
                                .ToList());

            return PartialView("~/Views/Controls/SearchResultsChart.cshtml", searchResultsChart);
        }
    }
}
