<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/Site.master" %>

<%@ Register tagPrefix="control" tagName="AdvertismentsWithPhotosViewControl" src="~/Controls/AdvertismentsWithPhotosRotator.ascx" %>
<%@ Register tagPrefix="control" tagName="SearchResultsChart" src="~/Controls/SearchResultsChart.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Недвижимость Харькова без посредников | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Недвижимость Харькова без посредников. Коллектор и доска объявлений недвижимости Харькова без посредников. Квартиры, новостройки, продажа и аренда." />
    <meta name="keywords" content="Недвижимость Харькова, без посредников, квартиры, новостройки, продажа, аренда, Харьков, коллектор объявлений" />

    <link rel="stylesheet" href="Scripts/tufte-graph/tufte-graph.css" type="text/css" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server" action="#main">
        <div class="row-fluid">
            <%-- Left column  ---%>
            <div class="span6">
                <h2>Недвижимость Харькова без посредников</h2>
                <p>
                    Привет! Наш сервис, Недвижимость-UA, является коллектором объявлений о недвижимости из ведущих печатных и 
                    интернет источников Харькова. Мы специализируемся на объявлениях об аренде, продаже, покупке <strong>недвижимости 
                    без посредников</strong> (от хозяев), для этого мы фильтруем объявления и отбираем только те 
                    объявления, которые добавил непосредственно хозяин объекта (квартиры, новостройки).
                </p>
                <p>
                    Наш сервис предлагает большую базу как по продаже, так и по аренде недвижимости, квартир, новостроек в Харькове.
                    Мы постарались собрать в одном месте наиболее интересные предложения по недвижимости в городе Харькове. Содержимое
                    нашей базы обновляется ежечасно и вы в течение дня можете следить за обновлениями интересующего вас раздела, будь то
                    продажа квартир или аренда недвижимости.
                </p>
                <p>
                    Посредники при операциях с недвижимостью в Харькове последнее время превращаются в настоящую панацею и мы постарались
                    сделать сервис, который позволяет хоть частично, но избавиться от посредников при покупке или аренде недвижимости.
                    Также непосредственно на сайте вы сами можете внести свой вклад в наше дело и отмечать телефоны и объявления посредников.
                </p>
        
                <div style="margin-top: 20px; text-align: center;">
                    <a href="/add_advertisment.aspx" class="add_advertisment_mainpage_button button orange">Добавить объявление</a>
                </div>

                <div id="Advertisment_Links" style="margin-top: 10px;">
                    <script type="text/javascript"><!--
                        google_ad_client = "ca-pub-5891602113354577";
                        /* Links Nedvijimost Main Page */
                        google_ad_slot = "4672892179";
                        google_ad_width = 468;
                        google_ad_height = 15;
                        //-->
                    </script>
                    <script type="text/javascript"
                    src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                    </script>
                </div>

                <div id="SearchResult_Charts">
                    <h3>Статистика объявлений аренды</h3>
                    <control:SearchResultsChart runat="server" ID="SearchResultsChart" />
                </div>
            </div>
            
            <%-- Right column  ---%>
            <div class="span6">
                                <div style="margin-top: 20px; padding: 5px; box-shadow: 0 0 5px 2px rgba(0,0,0,.1);">
                <h3>Разделы</h3>
                <h4>Жилая недвижимость</h4>
                    <div class="row-fluid" style="width: 90%; margin: 0 auto;">
                        <div class="span6">
                            <dl>
                                <dt>Аренда недвижимости</dt>
                                <dd><a href="Obyavleniya-Arenda/Sdam-kvartiru" title="Сдам квартиру, сегодня">Сдам квартиру</a></dd>
                                <dd><a href="Obyavleniya-Arenda/Snimu-kvartiru" title="Сниму квартиру, сегодня">Сниму квартиру</a></dd>
                            </dl>
                        </div>

                        <div class="span6">
                            <dl>
                                <dt>Продажа недвижимости</dt>
                                <dd><a href="Obyavleniya-Prodaja/Prodam-kvartiru" title="Продам квартиру, сегодня">Продам квартиру</a></dd>
                                <dd><a href="Obyavleniya-Prodaja/Kuplu-kvartiru" title="Куплю квартиру, сегодня">Куплю квартиру</a></dd>
                                <dd><a href="Obyavleniya-Doma-Dachi" title="Дома, дачи, сегодня">Дома, дачи</a></dd>
                            </dl>
                        </div>
                    </div>
                <h4>Коммерческая недвижимость</h4>
                    <div class="row-fluid" style="width: 90%; margin: 0 auto;">
                        <div class="span6">
                            <dl>
                                <dt>Аренда недвижимости</dt>
                                <dd><a href="Obyavleniya-Arenda-Kommercheskaya/Arenda-ofisov" title="Аренда офисов, сегодня">Аренда офисов</a></dd>
                            </dl>
                        </div>

                        <div class="span6">
                            <dl>
                                <dt>Продажа недвижимости</dt>
                                <dd><a href="Obyavleniya-Prodaja-Kommercheskaya/Prodam" title="Продам недвижимость, сегодня">Продам недвижимость</a></dd>
                            </dl>
                        </div>
                    </div>
                </div>

                <%--<div id="Advertisments_withPhonesRotator" style="margin-top: 10px;">
                    <h3>Недавние объявления</h3>
                    <control:AdvertismentsWithPhotosViewControl runat="server" ID="AdvertismentsWithPhotosRotator" />
                </div>--%>

                <div id="Vkontakte_Social_Group" style="margin-top: 10px;">
                    <h3>Сообщество</h3>

                    <!-- VK Widget -->
                    <div id="vk_groups"></div>
                    <script type="text/javascript">
                        VK.Widgets.Group("vk_groups", { mode: 2, width: "507", height: "300" }, 43123714);
                    </script>
                </div>

                <div id="News" style="margin-top: 10px;">
                    <h3>Новости недвижимости</h3>

                    <p>
                        <a href="News/arendator-i-arendodatel-problemi-i-vzaimootnosheniya.aspx" title="Арендатор и арендодатель - проблемы и взаимоотношения при аренде квартиры">
                            <img src="img/uridicheskie-aspekti-arendi-kvartir.jpg" alt="Арендатор и арендодатель - проблемы и взаимоотношения при аренде квартиры" style="float:left; margin: 10px; height: 100px;"/>
                        </a>
                        В данной статье мы рассмотрели некоторые проблемы, с которыми сталкивается 
                        арендодатель при сдаче квартиры в аренду и человек, снимающий квартиру. 
                        Затронули некоторые юридические аспекты аренды квартиры в Украине. 
                        <a href="News/arendator-i-arendodatel-problemi-i-vzaimootnosheniya.aspx" title="Арендатор и арендодатель - проблемы и взаимоотношения при аренде квартиры">Читать дальше..</a>
                    </p>
                </div>
            </div>
        </div>
    </form>
</asp:Content>