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
            var articles = new List<ViewModels.Article>();

            var dataModel = new DataModel();
            articles.AddRange(dataModel.Articles
                .OrderByDescending(a => a.createDate)
                .Select(a => new ViewModels.Article(a)));

            return View(articles);
        }

        public ActionResult Article(string article_link)
        {
            var dataModel = new DataModel();
            ViewModels.Article article = dataModel.Articles
                                       .Where(a => a.link == article_link.Replace(".aspx", ""))
                                       .Select(a => new ViewModels.Article(a))
                                       .FirstOrDefault();
            return View(article);
        }
    }
}
