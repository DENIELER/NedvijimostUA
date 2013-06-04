<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentCommercialAdvertisments.aspx.cs" Inherits="RentCommercialAdvertisments" MasterPageFile="~/AdvertismentMaster.master"%>

<%@ Register tagPrefix="control" tagName="AdvertismentsViewControl" src="~/Controls/AdvertismentsViewControl.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Аренда офисов без посредников в Харькове (сниму, сдам) | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Объявления по аренде коммерческой недвижимости, офисов в Харькове без посредников." />
    <meta name="keywords" content="аренда, сниму, сдам, квартиру, сдам в аренду, офис, офисов" />
    
    <link rel="stylesheet" href="/css/bootstrap-image-gallery.min.css" />
</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <ul class="breadcrumb">
          <li>Объявления <%= Utils.GetUkranianDateTimeNow().ToString("dd/MM/yyyy") %> <span class="divider">/</span></li>
          <li class="active"><%= SubSectionName %></li>
        </ul>

        <div style="float:right; line-height: 50px;">
            <a href="#" runat="server" id="lnkWithSubpurchases">Объявления включая посредников</a>
        </div>

        <h2 class="page_header"><%= SubSectionName %></h2>
        <p>
            <% if(AdvertismentsMode == AdvertismentsState.NotSubpurchase) { %>
            Объявления по аренде коммерческой недвижимости, офисов в Харькове без посредников. 
            Объявления отфильтрованы от посредников и на данной странице представлены объявления 
            только от хозяев за сегодняшний день.
            <% }else{ %>
            Объявления по аренде коммерческой недвижимости, офисов в Харькове. На данной странице представлены 
            все объявления, включая посредников за сегодняшний день.
            <% } %>
        </p>
        
        <div class="advertisments">
            <control:AdvertismentsViewControl runat="server" ID="AdvertismentsViewControl" />
        </div>
    </form>
</asp:Content>