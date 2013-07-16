using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SiteMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Sections Pages
            routes.MapRoute(
                name: "Route_AdvertismentsBySectionAndSubSection_RentSection",
                url: "{sectionUrl}/{subSectionUrl}/{subpurchaseMode}",
                defaults: new
                {
                    controller = "Advertisments",
                    action = "Index",

                    subSectionUrl = "",
                    subpurchaseMode = UrlParameter.Optional
                },
                constraints: new { sectionUrl = "Obyavleniya-Arenda|Obyavleniya-Prodaja|Obyavleniya-Arenda-Kommercheskaya|Obyavleniya-Prodaja-Kommercheskaya|Obyavleniya-Doma-Dachi" }
            );

            #endregion Sections Pages

            #region Common Pages
            routes.MapRoute(
                name: "Route_NewsArticles",
                url: "Novosti-nedvijimosti",
                defaults: new { controller = "Home", action = "Articles" });
            routes.MapRoute(
                name: "Route_Article", 
                url: "News/{article_link}",
                defaults: new { controller = "Home", action = "Article", article_link = "" });

            routes.MapRoute(
                name: "Route_AdvertismentsArchiveSections",
                url: "Arhiv-obyavlenij",
                defaults: new { controller = "Archive", action = "Sections" });
            routes.MapRoute(
                name: "Route_AdvertismentsArchive", 
                url: "Arhiv-Objyavlenij/{section}/{subpurchaseMode}", 
                defaults: new { controller = "Archive", action = "Section", section = "Sdam-kvartiru", subpurchaseMode = "Bez-posrednikov" });
            
            routes.MapRoute(
                name: "Route_Contacts",
                url: "Kontakti",
                defaults: new { controller = "Home", action = "Contacts" }
            );
            #endregion Common Pages

            #region Authentication
            routes.MapRoute(
                name: "Route_Register",
                url: "Registration",
                defaults: new { controller = "Authentication", action = "Registration" }
            );
            #endregion Authentication

            routes.MapRoute(
                name: "Route_AddAdvertisment",
                url: "add-advertisment",
                defaults: new { controller = "AddAdvertisments", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapPageRoute("Route_Rent_RentSubSection", "Obyavleniya-Arenda/Sdam-kvartiru/{SubpurchaseMode}", "~/ViewPages/Rent/RentAdvertisments.aspx", true,
            //new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });
            //routes.MapPageRoute("Route_Rent_TakeOffSubSection", "Obyavleniya-Arenda/Snimu-kvartiru/{SubpurchaseMode}", "~/ViewPages/Rent/TakeOffAdvertisments.aspx", true,
            //    new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });

            //routes.MapPageRoute("Route_Sale_SaleSubSection", "Obyavleniya-Prodaja/Prodam-kvartiru/{SubpurchaseMode}", "~/ViewPages/Sale/SaleAdvertisments.aspx", true,
            //    new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });
            //routes.MapPageRoute("Route_Sale_BuySubSection", "Obyavleniya-Prodaja/Kuplu-kvartiru/{SubpurchaseMode}", "~/ViewPages/Sale/BuyAdvertisments.aspx", true,
            //    new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });

            //routes.MapPageRoute("Route_RentCommercialSection", "Obyavleniya-Arenda-Kommercheskaya/Arenda-ofisov/{SubpurchaseMode}", "~/ViewPages/Commercial/RentCommercialAdvertisments.aspx", true,
            //    new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });
            //routes.MapPageRoute("Route_SaleCommercialSection", "Obyavleniya-Prodaja-Kommercheskaya/Prodam/{SubpurchaseMode}", "~/ViewPages/Commercial/SaleCommercialAdvertisments.aspx", true,
            //    new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });

            //routes.MapPageRoute("Route_CottagesHousesSection", "Obyavleniya-Doma-Dachi/{SubpurchaseMode}", "~/ViewPages/Cottages/CottagesAdvertisments.aspx", true,
            //    new RouteValueDictionary() { 
            //    { "SubpurchaseMode", "Bez-posrednikov" }
            //});
            ////routeCollection.MapPageRoute("Route_DefaultSectionRent", "Advertisments/{section}/{subsection}", "~/default.aspx");

            //routes.MapPageRoute("Route_PaymentResult", "Payment/{payment_result}", "~/Payments/PaymentResult.aspx");
            //routes.MapPageRoute("Route_HowToMarkSpecial", "Kak-videlit-obyavlenie", "~/Payments/MakeAdvertismentSpecial.aspx");

            //routes.MapPageRoute("Route_Authorization", "authorization", "~/AuthorizationPage.aspx");
            //routes.MapPageRoute("Route_AuthorizationFailed", "failed-authorization", "~/FailedAuthorization.aspx");
            //routes.MapPageRoute("Route_Register", "register", "~/Registration.aspx");
            //routes.MapPageRoute("Route_RegisterSuccess", "success-registration", "~/SuccessRegistration.aspx");
            //routes.MapPageRoute("Route_RegisterFailed", "failed-registration", "~/FailedRegistration.aspx");
            //routes.MapPageRoute("Route_UserOptions", "user-options", "~/AuthorizedUserOptions.aspx");

            //routes.MapPageRoute("Route_MyAdvertisments", "my-advertisments", "~/MyAdvertisments.aspx");
        }
    }
}