<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubPurchases.aspx.cs" Inherits="Kharkiv_SubPurchases" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <title>Недвижимость-UA - Телефоны, номера посредников и агенств недвижимости. Харьков</title>

    <meta name="description" content="Телефонные номера агенств недвижимости и посредников Харькова. Единый список посредников города Харькова при продаже или аренде недвижимости, квартир." />
    <meta name="keywords" content="Телефон, телефоны, номер, номера, посредник, агенство, агенство недвижимости, Харьков, город, город Харьков" />

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">

    <form id="MainForm" runat="server">
        <h2>Каталог посредников. Харьков.</h2>
        <pre>
            Количество телефонов посредников: <strong><%= SubPurchasesCount %></strong>
        </pre>
        <table style="width:100%;">
            <thead>
                <tr style="line-height: 25px;">
                    <td style="width: 50%;"><strong>Номер телефона</strong></td>
                    <td style="width: 50%;"><strong>Имя посредника\Название организации</strong></td>
                </tr>    
            </thead>
            <tbody>
                <asp:ListView ID="lvPhoneNumbers" runat="server">
                    <ItemTemplate>
                        <tr style="line-height: 25px;">
                            <td style="width: 50%;">
                                <a href="<%= ResolveUrl("/Kharkiv/SubPurchase.aspx?phone=") %><%# Eval("phone.phone") %>"><%# Eval("phone.phone")%></a>
                            </td>
                            <td style="width: 50%;">
                                <a href="<%= ResolveUrl("/Kharkiv/SubPurchase.aspx?phone=") %><%# Eval("phone.phone") %>"><%# Eval("subpurchase.name") + " " + Eval("subpurchase.surname")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </tbody>
        </table>
        <div style="line-height: 25px; margin-top: 30px; text-align: center;">
            <asp:DataPager ID="dpPhoneNumbers" runat="server" PagedControlID="lvPhoneNumbers" PageSize="20" OnPreRender="dpPhoneNumbers_PreRender" QueryStringField="pageIndex">
                <Fields>
                    <asp:NextPreviousPagerField ShowFirstPageButton="true" ShowNextPageButton="false" ShowPreviousPageButton="false" FirstPageText="Начало" />
                    <asp:NumericPagerField />
                    <asp:NextPreviousPagerField ShowLastPageButton="true" ShowNextPageButton="false" ShowPreviousPageButton="false" LastPageText="Конец"/>
                </Fields>
            </asp:DataPager>
        </div>
    </form>
    
</asp:Content>