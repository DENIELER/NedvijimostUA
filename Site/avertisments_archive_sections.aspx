<%@ Page Language="C#" AutoEventWireup="true" CodeFile="avertisments_archive_sections.aspx.cs" Inherits="avertisments_archive" MasterPageFile="AdvertismentMaster.master" %>

<%@ Register tagPrefix="control" tagName="AdvertismentsViewControl" src="Controls/AdvertismentsViewControl.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Архив объявлений о недвижимости Харькова | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Полный каталог объявлений в Харькове БЕЗ посредников." />
    <meta name="keywords" content="Архив, объявлений, недвижимость, Харьков, квартиры в Харькове, Харьков, квартир, без посредников" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <div runat="server" id="pnlSites">
            <h2 class="block_header">Весь архив объявлений <hr/></h2>
            <p>
                Вашему вниманию представлен полный архив объявлений недвижимости по различным разделам в городе Харькове БЕЗ посредников. 
                Советуем вам выбирать наиболее свежие объявления (1-2 дня) в связи с динамичностью рынка недвижимости 
                в Харькове. Желаем вам найти свой уютный уголок наиболее выгодно и удобно.
            </p>

            <div style="margin: 20px 0;">
                <h4>Пожалуйста, выберите раздел для просмотра архива:</h4>
            </div>    

            <div style="margin-top: 20px; padding: 5px; box-shadow: 0 0 5px 2px rgba(0,0,0,.1);">
                <span>Жилая недвижимость</span>
                <div class="row-fluid" style="width: 90%; margin: 0 auto;">
                    <div class="span6">
                        <dl>
                            <dt>Аренда недвижимости</dt>
                            <dd><a href="/Arhiv-Objyavlenij/Sdam-kvartiru" title="Сдам квартиру, сегодня">Сдам квартиру</a></dd>
                            <dd><a href="/Arhiv-Objyavlenij/Snimu-kvartiru" title="Сниму квартиру, сегодня">Сниму квартиру</a></dd>
                        </dl>
                    </div>

                    <div class="span6">
                        <dl>
                            <dt>Продажа недвижимости</dt>
                            <dd><a href="/Arhiv-Objyavlenij/Prodam-kvartiru" title="Продам квартиру, сегодня">Продам квартиру</a></dd>
                            <dd><a href="/Arhiv-Objyavlenij/Kuplu-kvartiru" title="Куплю квартиру, сегодня">Куплю квартиру</a></dd>
                            <dd><a href="/Arhiv-Objyavlenij/Obyavleniya-Doma-Dachi" title="Дома, дачи, сегодня">Дома, дачи</a></dd>
                        </dl>
                    </div>
                </div>
                <span>Коммерческая недвижимость</span>
                    <div class="row-fluid" style="width: 90%; margin: 0 auto;">
                        <div class="span6">
                            <dl>
                                <dt>Аренда недвижимости</dt>
                                <dd><a href="/Arhiv-Objyavlenij/Arenda-ofisov" title="Аренда офисов, сегодня">Аренда офисов</a></dd>
                            </dl>
                        </div>

                        <div class="span6">
                            <dl>
                                <dt>Продажа недвижимости</dt>
                                <dd><a href="/Arhiv-Objyavlenij/Prodam-kommercheskuu-nedvijimost" title="Продам недвижимость, сегодня">Продам недвижимость</a></dd>
                            </dl>
                        </div>
                    </div>
                </div>
        </div>    
    </form>
</asp:Content>
