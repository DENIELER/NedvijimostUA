<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CottagesAdvertisments.aspx.cs" Inherits="RentAdvertisments" MasterPageFile="~/AdvertismentMaster.master"%>

<%@ Register tagPrefix="control" tagName="AdvertismentsViewControl" src="~/Controls/AdvertismentsViewControl.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Продам дом, дачу, участок в Харькове и области | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Объявления о продаже и покупке домов, дач, участков в Харькове и харьковской области без посредников" />
    <meta name="keywords" content="продам, куплю, дом, дача, участок, область, харьковская, харьков" />
    
    <link rel="stylesheet" href="/css/blueimp-gallery.min.css">
</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <ul class="breadcrumb">
          <li>Объявления <%= Utils.GetUkranianDateTimeNow().ToString("dd/MM/yyyy") %> <span class="divider">/</span></li>
          <li class="active">Продам, куплю дом, дачу, участок</li>
        </ul>

        <div style="float:right; line-height: 50px;">
            <a href="#" runat="server" id="lnkWithSubpurchases">Объявления включая посредников</a>
        </div>

        <h2 class="page_header">Продам, куплю дом, дачу, участок</h2>
        <p>
            <% if(AdvertismentsMode == AdvertismentsState.NotSubpurchase) { %>
            Объявления по прожаже и покупке домов, дачных участков, дач в Харькове и харьковской области без посредников. 
            Объявления отфильтрованы от посредников и на данной странице представлены 
            только от хозяев квартир за сегодняшний день. Если вы хотите <strong>продать или купить
            дом, дачу или участок в Харькове или области</strong> без посредников, то данная страница 
            будет вам очень полезна.
            <% }else{ %>
            Объявления по прожаже и покупке домов, дачных участков, дач в Харькове и харьковской области без посредников.  На данной странице 
            представлены все объявления, включая посредников за сегодняшний день. Если вы 
            не против иметь отношения с посредниками и хотите <strong>продать или купить
            дом, дачу или участок в Харькове или области</strong> без посредников или с их участием, то данная страница вам в помощь.
            <% } %>
        </p>
        
        <div class="advertisments">
            <control:AdvertismentsViewControl runat="server" ID="AdvertismentsViewControl" />
        </div>
    </form>
</asp:Content>