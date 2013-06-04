<%@ Page Language="C#" AutoEventWireup="true" CodeFile="articles.aspx.cs" Inherits="articles" MasterPageFile="AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Новости и статьи о недвижимости - Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Статьи по выбору недивижимости в Харькове, аренде, продаже. Помощь в аренде квартир без посредников." />
    <meta name="keywords" content="Недвижимость, аренда, снять квартиру, аренда квартир, Харьков" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    
    <div runat="server" id="pnlSites">
        <h1 class="block_header">Новости и статьи о недвижимости <hr/></h1>

        <asp:EntityDataSource runat="server" ID="dsArticles" ConnectionString="name=NedvijimostDBEntities" DefaultContainerName="NedvijimostDBEntities" EnableFlattening="False" EntitySetName="Articles" OrderBy="it.createDate DESC"></asp:EntityDataSource>
        <asp:ListView runat="server" ID="lvArticles" DataSourceID="dsArticles">
            <ItemTemplate>
                <a href="/News/<%# Eval("link") %>"><h2><%# Eval("header") %></h2></a>
                <p>
                    <%# Eval("description") %> <a href="/News/<%# Eval("link") %>">Читать дальше..</a>
                </p>
                <hr style="clear:both;"/>
            </ItemTemplate>
        </asp:ListView>

        <p style="text-align: center;">
            <asp:DataPager runat="server" ID="dpArticles" PagedControlID="lvArticles" PageSize="5" QueryStringField="page">
               <Fields>
                  <asp:NextPreviousPagerField FirstPageText="&lt;&lt;" ShowFirstPageButton="True" 
                         ShowNextPageButton="False" ShowPreviousPageButton="False" />
                  <asp:NumericPagerField />
                  <asp:NextPreviousPagerField LastPageText="&gt;&gt;" ShowLastPageButton="True" 
                         ShowNextPageButton="False" ShowPreviousPageButton="False" />
               </Fields>
            </asp:DataPager>
        </p>
    </div>

</asp:Content>
