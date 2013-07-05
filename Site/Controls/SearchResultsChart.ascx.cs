using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class Controls_SearchResultsChart : System.Web.UI.UserControl
{
    public DataModel DBContext
    {
        get;
        set;
    }

    protected List<SearchResultCountEntity> SearchResults
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (DBContext != null)
        {
            SearchResults = DBContext.ExecuteQuery<SearchResultCountEntity>(@"
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
order by createDate;").ToList();
        }
    }

    public class SearchResultCountEntity
    {
        public DateTime date { get; set; }
        public int fullCount { get; set; }
        public int withoutSubPurchaseCount { get; set; }
    }
}