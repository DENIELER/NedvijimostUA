<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleAdvertisments.aspx.cs" Inherits="SaleAdvertisments" MasterPageFile="~/AdvertismentMaster.master"%>

<%@ Register tagPrefix="control" tagName="AdvertismentsViewControl" src="~/Controls/AdvertismentsViewControl.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Продам квартиру Харьков | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Объявления по продаже квартир, гостинок, новостроев в Харькове без посредников. Объявления отфильтрованы без посредников, от хозяев квартир." />
    <meta name="keywords" content="продам, продажа, квартиру, однокомнатную, гостинку, жилье" />
    
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
            Объявления по продаже квартир, гостинок, новостроев в Харькове без посредников. 
            Объявления отфильтрованы от посредников и на данной странице представлены только от 
            хозяев квартир за сегодняшний день. Данная страница будет интересна вам, если вы 
            хотите <strong>купить квартиру</strong> в Харькове или <strong>купить гостинку или 
            квартиру в новострое</strong>.
            <% }else{ %>
            Объявления по продаже квартир, гостинок, новостроев в Харькове. На данной странице 
            представлены все объявления, включая посредников за сегодняшний день. Данная страница 
            будет интересна вам, если вы хотите <strong>купить квартиру</strong> в Харькове 
            или <strong>купить гостинку или квартиру в новострое</strong> и не против иметь отношения 
            с посредниками недвижимости.
            <% } %>
        </p>
        
        <div class="advertisments">
            <control:AdvertismentsViewControl runat="server" ID="AdvertismentsViewControl" />
        </div>
    </form>
</asp:Content>