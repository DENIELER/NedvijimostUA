<%@ Page Language="C#" AutoEventWireup="true" CodeFile="avertisments_archive.aspx.cs" Inherits="avertisments_archive" MasterPageFile="AdvertismentMaster.master" %>

<%@ Register tagPrefix="control" tagName="AdvertismentsViewControl" src="Controls/AdvertismentsViewControl.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    <%= AdvSectionName != null ? AdvSectionName + ". " : "" %>Архив объявлений о недвижимости Харькова | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Архив объявлений раздела <%= AdvSectionName != null ? AdvSectionName.ToLower() : "архив объявлений" %>. Полный каталог объявлений в Харькове БЕЗ посредников." />
    <meta name="keywords" content="<%= AdvSectionName != null ? AdvSectionName : "Архив объявлений" %>, архив, объявлений, недвижимость, Харьков, квартиры в Харькове, Харьков, квартир, без посредников" />
    
    <link href="/css/bootstrap.calendar.css" type="text/css" rel="Stylesheet"/>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <div runat="server" id="pnlSites">
            <h2 class="block_header">Архив объявлений <%= AdvSectionName != null ? AdvSectionName.ToLower() : "" %> <hr/></h2>
            <p>
                Архив объявлений недвижимости <%= AdvSectionName %> в городе Харькове БЕЗ посредников. 
                Советуем вам выбирать наиболее свежие объявления (1-2 дня) в связи с динамичностью рынка 
                недвижимости в Харькове. Желаем вам найти свой уютный уголок наиболее выгодно и удобно.
            </p>

            <h2><%= Date.ToString("dd/MM/yyyy") %></h2>

            <control:AdvertismentsViewControl runat="server" ID="AdvertismentsViewControl"/>
        </div>
    </form>
    
    <script src="/js/bootstrap.calendar.js"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="RightColumn">
    <script type="text/javascript">
        $(document).ready(function () {

            $('#calendar').Calendar({ 'weekStart': 1, 'url': '<%= new Uri(Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Path) %>' })
            //.on('changeDay', function (event) { alert(event.day.valueOf() + '-' + event.month.valueOf() + '-' + event.year.valueOf()); })
            //.on('onEvent', function (event) { alert(event.day.valueOf() + '-' + event.month.valueOf() + '-' + event.year.valueOf()); })
            //.on('onNext', function (event) { alert("Next"); })
            //.on('onPrev', function (event) { alert("Prev"); })
            //.on('onCurrent', function (event) { alert("Current"); });
        });
    </script>

    <div id="calendar" style="margin-top: 10px;"></div>
</asp:Content>