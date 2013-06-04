<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuthorizationVkontakteFailed.aspx.cs" Inherits="Auth_AuthorizationVkontakteFailed" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">

    <title>Недвижимость-UA - Ошибка авторизации Вконтакте</title>

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <div runat="server" id="pnlSites">
            <h2 class="block_header">Ошибка авторизации Вконтакте <hr/></h2>
            <p>
                Извините, произошла ошибка авторизации. Попробуйте еще раз или обратитесь к нам в тех. 
                поддержку - <a href="mailto:support@nedvijimost-ua.com">support@nedvijimost-ua.com</a>
            </p>
        </div>
    </form>
</asp:Content>