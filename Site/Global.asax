<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        RegisterRoutes(RouteTable.Routes);
    }

    public static void RegisterRoutes(RouteCollection routeCollection)
    {
        routeCollection.MapPageRoute("Route_Rent_RentSubSection", "Obyavleniya-Arenda/Sdam-kvartiru/{SubpurchaseMode}", "~/ViewPages/Rent/RentAdvertisments.aspx", true, 
            new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" }});
        routeCollection.MapPageRoute("Route_Rent_TakeOffSubSection", "Obyavleniya-Arenda/Snimu-kvartiru/{SubpurchaseMode}", "~/ViewPages/Rent/TakeOffAdvertisments.aspx", true,
            new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" }});

        routeCollection.MapPageRoute("Route_Sale_SaleSubSection", "Obyavleniya-Prodaja/Prodam-kvartiru/{SubpurchaseMode}", "~/ViewPages/Sale/SaleAdvertisments.aspx", true, 
            new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" }});
        routeCollection.MapPageRoute("Route_Sale_BuySubSection", "Obyavleniya-Prodaja/Kuplu-kvartiru/{SubpurchaseMode}", "~/ViewPages/Sale/BuyAdvertisments.aspx", true,
            new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" } });

        routeCollection.MapPageRoute("Route_RentCommercialSection", "Obyavleniya-Arenda-Kommercheskaya/Arenda-ofisov/{SubpurchaseMode}", "~/ViewPages/Commercial/RentCommercialAdvertisments.aspx", true, 
            new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" }});
        routeCollection.MapPageRoute("Route_SaleCommercialSection", "Obyavleniya-Prodaja-Kommercheskaya/Prodam/{SubpurchaseMode}", "~/ViewPages/Commercial/SaleCommercialAdvertisments.aspx", true, 
            new RouteValueDictionary() { { "SubpurchaseMode", "Bez-posrednikov" }});

        routeCollection.MapPageRoute("Route_CottagesHousesSection", "Obyavleniya-Doma-Dachi/{SubpurchaseMode}", "~/ViewPages/Cottages/CottagesAdvertisments.aspx", true,
            new RouteValueDictionary() { 
                { "SubpurchaseMode", "Bez-posrednikov" }
            });
        //routeCollection.MapPageRoute("Route_DefaultSectionRent", "Advertisments/{section}/{subsection}", "~/default.aspx");

        routeCollection.MapPageRoute("Route_AdvertismentsArchiveSections", "Arhiv-obyavlenij", "~/avertisments_archive_sections.aspx");
        routeCollection.MapPageRoute("Route_AdvertismentsArchive", "Arhiv-Objyavlenij/{Section}/{SubpurchaseMode}", "~/avertisments_archive.aspx", true,
            new RouteValueDictionary() { 
                { "Section", "Sdam-kvartiru" },
                { "SubpurchaseMode", "Bez-posrednikov"}
            });
        
        routeCollection.MapPageRoute("Route_NewsArticles", "Novosti-nedvijimosti", "~/articles.aspx");
        routeCollection.MapPageRoute("Route_Contacts", "Kontakti", "~/contacts.aspx");

        routeCollection.MapPageRoute("Route_PaymentResult", "Payment/{payment_result}", "~/Payments/PaymentResult.aspx");
        routeCollection.MapPageRoute("Route_HowToMarkSpecial", "Kak-videlit-obyavlenie", "~/Payments/MakeAdvertismentSpecial.aspx");

        routeCollection.MapPageRoute("Route_Article", "News/{article_link}", "~/News/Article.aspx");

        routeCollection.MapPageRoute("Route_Authorization", "authorization", "~/AuthorizationPage.aspx");
        routeCollection.MapPageRoute("Route_AuthorizationFailed", "failed-authorization", "~/FailedAuthorization.aspx");
        routeCollection.MapPageRoute("Route_Register", "register", "~/Registration.aspx");
        routeCollection.MapPageRoute("Route_RegisterSuccess", "success-registration", "~/SuccessRegistration.aspx");
        routeCollection.MapPageRoute("Route_RegisterFailed", "failed-registration", "~/FailedRegistration.aspx");
        routeCollection.MapPageRoute("Route_UserOptions", "user-options", "~/AuthorizedUserOptions.aspx");

        routeCollection.MapPageRoute("Route_MyAdvertisments", "my-advertisments", "~/MyAdvertisments.aspx");
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
