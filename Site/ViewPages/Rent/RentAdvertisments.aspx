<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentAdvertisments.aspx.cs" Inherits="RentAdvertisments" MasterPageFile="~/AdvertismentMaster.master"%>

<%@ Register tagPrefix="control" tagName="AdvertismentsViewControl" src="~/Controls/AdvertismentsViewControl.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Снять квартиру, гостинку Харьков | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Сдам квартиру в Харькове без посредников. Объявления об аренде квартир в Харькове без посредников" />
    <meta name="keywords" content="сдам, аренда, квартиру, сдам в аренду, однокомнатная, гостинка, жилье" />
    
    <link rel="stylesheet" href="/css/blueimp-gallery.min.css">
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
            Объявления по аренде квартир, гостинок, комнат в Харькове без посредников. 
            Объявления отфильтрованы от посредников и на данной странице представлены 
            только от хозяев квартир за сегодняшний день. Если вы хотите <strong>снять
            квартиру или гостинку в Харькове</strong> без посредников, то данная страница 
            будет вам очень полезна.
            <% }else{ %>
            Объявления по аредне квартир, гостинок, комнат в Харькове. На данной странице 
            представлены все объявления, включая посредников за сегодняшний день. Если вы 
            не против иметь отношения с посредниками и хотите <strong>снять квартиру или 
            гостинку в Харькове</strong> без посредников, то данная страница вам в помощь.
            <% } %>
        </p>
        
        <div class="advertisments">
            <control:AdvertismentsViewControl runat="server" ID="AdvertismentsViewControl" />
        </div>
    </form>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="RightColumn">
    <div class="well" style="margin-top: 20px;">
        <h4>Сдам квартиру</h4>

        <div style="margin-top: 20px;">
            <a href="/Arhiv-Objyavlenij/Sdam-kvartiru?date=<%= DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") %>">Объявления вчерашнего дня</a>
        </div>
    </div>

    <div class="well" style="margin-top: 20px;">
        <h4>Фильтр</h4>

        <div style="margin-top: 20px;">
            <label class="checkbox">
              <input type="checkbox"> Только объявления с фото
            </label>
        </div>
    </div>
</asp:Content>