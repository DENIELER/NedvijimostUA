<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentResult.aspx.cs" Inherits="Payments_PaymentResult" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <title>Результат оплаты - Недвижимость-UA</title>

    <meta name="description" content="Результат оплаты заказа на выделение объявления" />
    <meta name="keywords" content="оплата, заказ, результат оплаты, успешно, не удачно" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <h2>Результат оплаты выделения объявления</h2>

    <% if(ResultSuccess){ %>
    <h3><img src="../img/success_payment.png" alt="Успешно" /> Платеж прошел успешно.</h3>
    <p>
       Заказ на выделение Вашего объявления выполнен. Спасибо! 
    </p>
    <% }else{ %>
    <h3><img src="../img/failed_payment.png" alt="Не успешно" /> Платеж не прошел.</h3>
    <p>
       Извините, платеж не был удачным. <br />
       При возникновении любых вопросов - пожалуйста, обращайтесь к нам в отдел технической поддержки 
       <a href="mailto:support@nedvijimost-ua.com">support@nedvijimost-ua.com</a>. Спасибо!
    </p>
    <% } %>
</asp:Content>