using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class News_Article : System.Web.UI.Page
{
    protected bool Article_Detected;

    protected string Article_Title;
    protected string Article_Description;
    protected string Article_Keywords;

    protected string Article_Header;
    protected string Article_Text;

    protected DateTime Article_CreateDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        string article_link;
        if (Page.RouteData.Values["article_link"] != null)
        {
            article_link = Page.RouteData.Values["article_link"].ToString();

            var _context = new NedvijimostDBEntities();
            var articles = from article in _context.Articles
                          where article.link == article_link.Replace(".aspx", "")
                          select article;
            if (articles != null)
            {
                var article = articles.FirstOrDefault();
                if (article != null)
                {
                    Article_Detected = true;

                    Article_CreateDate = article.createDate;
                    Article_Description = article.description;
                    Article_Keywords = article.keywords;
                    Article_Header = article.header;
                    Article_Text = article.text;
                    Article_Title = article.title;
                }
            }
        }
    }
}