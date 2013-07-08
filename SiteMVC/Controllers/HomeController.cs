using SiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult Articles()
        {
            var articles = new List<Models.UI.Article>();

            var dataModel = new DataModel();
            articles.AddRange(dataModel.Articles
                .OrderByDescending(a => a.createDate)
                .Select(a => new Models.UI.Article(a)));

            return View(articles);
        }

        public ActionResult Article(string article_link)
        {
            var dataModel = new DataModel();
            Models.UI.Article article = dataModel.Articles
                                       .Where(a => a.link == article_link.Replace(".aspx", ""))
                                       .Select(a => new Models.UI.Article(a))
                                       .FirstOrDefault();
            return View(article);
        }
        
        #region User Controls
        public ActionResult AuthorizationControl()
        {
            var dataModel = new DataModel();
            var authorization = new Models.UI.Controls.Authorization();
            authorization.IsAuthorized = SystemUtils.Authorization.IsAuthorized;
            if(authorization.IsAuthorized)
            {
                authorization.Login = SystemUtils.Authorization.Login;
                authorization.Phone = SystemUtils.Authorization.Phone;

                authorization.UserID = SystemUtils.Authorization.UserID;
                if (authorization.UserID != null)
                    authorization.AdvertismentsCount = dataModel.Advertisments
                                                       .Count(a => a.UserID == authorization.UserID);

                authorization.IsAdmin = SystemUtils.Authorization.IsAdmin;
            }

            return PartialView("~/Views/Controls/AuthorizationControl.cshtml", authorization);
        }

        public ActionResult LogIn(string Login, string Password, bool RememberMe)
        {
            if (SystemUtils.Authorization.LogIn(Login, Password, RememberMe))
            {
                string returnUrl = Request["ReturnUrl"];

                if (returnUrl == null) returnUrl = "~/";
                return Redirect(returnUrl);
            }
            
            return Redirect("~/registration/failed-authorization");
        }
        public ActionResult LogOut()
        {
            SystemUtils.Authorization.LogOut();
            return Redirect("~/");
        }

        public ActionResult SearchResultsChartControl()
        {
            var searchResultsChart = new List<Models.UI.Controls.SearchResult>();

            var dataModel = new DataModel();
            searchResultsChart.AddRange(dataModel.ExecuteQuery<Models.UI.Controls.SearchResult>(@"
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
        #endregion User Controls
    }
}
