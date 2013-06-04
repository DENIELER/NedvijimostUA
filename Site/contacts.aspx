<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contacts.aspx.cs" Inherits="contacts" MasterPageFile="~/AdvertismentMaster.master"%>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Контакты | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Контакты для связи с нашей командой по недвижимости в Украине" />
    <meta name="keywords" content="контакты, телефон, email, связь" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <h2>Контакты</h2>

    <p>
        По любым вопросам об объявлениях, о размещении объявлений на сайте, сотрудничестве или 
        размещении рекламы на сайте, пожалуйста, пишите нам на наш email:
    </p>

    <address>
        <strong>Email</strong><br />
        <a href="mailto:support@nedvijimost-ua.com">support@nedvijimost-ua.com</a>
    </address>

    <p>
        Также, если Вам удобно, можете связываться с нами в группе ВКонтакте:
    </p>

    <address>
        <strong>Группа ВКонтакте</strong><br />
        <a href="http://vk.com/nedvijimost_ua">vk.com/nedvijimost_ua</a>
    </address>

    <p>
        Всегда рады живому общению с Вами!
    </p>
</asp:Content>