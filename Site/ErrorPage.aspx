﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html;charset=UTF-8" />

    <link type="text/css" rel="stylesheet" href="/css/main.css" />

    <link href="/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="/js/bootstrap.js" type="text/javascript"></script>
    
    <!-- Google Analitycs -->
	<script type="text/javascript">
	    var _gaq = _gaq || [];
	    _gaq.push(['_setAccount', 'UA-26373227-1']);
	    _gaq.push(['_trackPageview']);
	    (function () {
	        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
	        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
	        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
	    })();
	</script>
    <!-- end Google Analitycs -->
    
    <!-- Social network buttons --->
    <script type="text/javascript" src="http://userapi.com/js/api/openapi.js?52"></script>
    
    <script type="text/javascript">
        VK.init({ apiId: 3111027, onlyWidgets: true });
    </script>

    <script type="text/javascript" src="https://apis.google.com/js/plusone.js">
        { lang: 'ru' }
    </script>
    <!-- Social network buttons --->
</head>
<body>
    <div class="container" style="width: 1170px;">
            
            <div class="container" style="width: 1060px;">
                <div style="width: 1060px;">
                    <div style="float:left; width: 385px; height: 223px;">
                        <img style="display: block;" src="/images/site/header_image_top.png" alt="Харьков. Аренда квартиры."/>
                        <img style="display: block;" src="/images/site/header_image_bottom.png" alt="Харьков. Аренда квартиры."/>
                    </div>

                    <div style="float:left; width: 675px;height: 223px;">
                        <div class="row-fluid" style="height: 223px;">
                            <div class="span8">
                                <div style="height: 120px;">
                                    <h1 style="color: #b8b8b8;position: relative; top: 20px; left: 10px;">Недвижимость</h1>
                                    <h2 style="color: #dbdbdb;position: relative; top: 20px; left: 130px; font-size: 16px; width: 330px;">Каталог без посредников</h2>
                                </div>

                                <div style="height: 65px; position:relative; top:28px;">
                                    <h3 style="color: #b8b8b8; position: relative; left: 10px; top: 5px; z-index: 2; display: block;">Харьков</h3>
                                    <img style="height: 66px; max-width: 222px; display: block; top: -61px; position: relative; z-index: 1;" src="/images/site/header_squares.png" alt="Squares"/> 
                                </div>
                            </div>

                            <div class="span4" style="height: 223px;">
                                <div style="width: 200px; background-color: #dbdbdb; height: 223px;">
                                    <div style="padding-top: 10px;">
                                        &nbsp;
                                    </div>

                                    <div>
                                        <div style="margin-top: 10px;">
                                            <g:plusone></g:plusone>
                                        </div>
                                        <div style="margin-top: 10px;">
                                            <div id="vk_like"></div>
                                            <script type="text/javascript">
                                                VK.Widgets.Like("vk_like", { type: "button", pageUrl: "http://nedvijimost-ua.com" }, 190);
                                            </script>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="container" style="height: 43px; width: 1170px;">
                <div style="float:left; width: 55px; height: 43px;">
                    <div style="height: 36px; background: url(/images/site/content_corner_left.png) no-repeat right;">&nbsp;</div>
                    <div style="height: 7px; background: url(/images/site/content_left_border.png) repeat-y right;">&nbsp;</div>
                </div>
                
                <div style="float:left;height: 36px; width: 1060px;">
                    <div style="float:left; width: 378px; background: url(/images/site/menu_border.png) repeat-x;">
                        &nbsp;
                    </div>

                    <div style="float:left; width: 682px; height: 43px;">
                        <div id="mainMenu">
                            <a href="<%= ResolveUrl("/") %>" class="first">Главная</a>
                            <a href="<%= ResolveUrl("/Arhiv-obyavlenij") %>">Архив объявлений</a>
                            <a href="<%= ResolveUrl("/Novosti-nedvijimosti") %>">Новости и статьи</a>
                            <a href="<%= ResolveUrl("/Kontakti") %>" class="last">Контакты</a>
                        </div>
                    </div>
                </div>
                
                <div style="float:left; width: 55px; height: 43px;">
                    <div style="height: 36px; background: url(/images/site/content_corner_right.png) no-repeat left;">&nbsp;</div>
                    <div style="height: 7px; background: url(/images/site/content_right_border.png) repeat-y left;">&nbsp;</div>
                </div>
            </div>

    </div>
    
    <div class="container" style="display: table; width: 1170px;">
        <div style="display: table-row;">

            <div style="display: table-cell; width: 55px; vertical-align: top;background: url(/images/site/content_left_border.png) repeat-y right;">
                &nbsp;
            </div>
            
            <div class="row-fluid">
                <div style="margin: 0 10px;">
                    <h1>Ошибка загрузки страницы</h1>
                    <p>
                        Приносим свои извинения, но произошла ошибка при загрузке запрашиваемой Вами страницы.
                        Пожалуйста, через некоторое время попробуйте перезагрузить страницу в браузере. 
                    </p>

                    <p>
                        Если проблема не была решена - пожалуйста, напишите нам в поддержку - <a href="mailto:support@nedvijimost-ua.com">support@nedvijimost-ua.com</a>
                    </p>
                </div>
            </div>
    
            <div style="display: table-cell; width: 55px; vertical-align: top;background: url(/images/site/content_right_border.png) repeat-y left;">
                &nbsp;
            </div>
        </div>
    </div>
    
    <div class="container" style="width: 1170px;">
        <div style="width: 1060px; margin: 0 auto;">
            <img src="/img/footer_line.gif" style="width: 1060px; display: block;" alt="Недвижимость-UA footer line" />
            <div style="margin: 20px 0;">
                <div style="width: 100px; float:left;">
                    <!-- Yandex.Metrika counter -->
                    <script type="text/javascript">
                        (function (d, w, c) {
                            (w[c] = w[c] || []).push(function () {
                                try {
                                    w.yaCounter9602347 = new Ya.Metrika({
                                        id: 9602347,
                                        webvisor: true,
                                        clickmap: true,
                                        trackLinks: true,
                                        accurateTrackBounce: true
                                    });
                                } catch (e) { }
                            });

                            var n = d.getElementsByTagName("script")[0],
                                s = d.createElement("script"),
                                f = function () { n.parentNode.insertBefore(s, n); };
                            s.type = "text/javascript";
                            s.async = true;
                            s.src = (d.location.protocol == "https:" ? "https:" : "http:") + "//mc.yandex.ru/metrika/watch.js";

                            if (w.opera == "[object Opera]") {
                                d.addEventListener("DOMContentLoaded", f, false);
                            } else { f(); }
                        })(document, window, "yandex_metrika_callbacks");
                    </script>
                    <noscript><div><img src="//mc.yandex.ru/watch/9602347" style="position:absolute; left:-9999px;" alt="" /></div></noscript>
                    <!-- /Yandex.Metrika counter -->
	
                    <!-- begin of Top100 code -->
                    <script id="top100Counter" type="text/javascript" src="http://counter.rambler.ru/top100.jcn?2845097"></script>
                    <noscript>
                    <a href="http://top100.rambler.ru/navi/2845097/">
                    <img src="http://counter.rambler.ru/top100.cnt?2845097" alt="Rambler's Top100" border="0" />
                    </a>
                    </noscript>
                    <!-- end of Top100 code -->
                </div>
                <div style="padding-left: 15px;">
                    Недвижимость-UA, 2012 - 2013 г.<br />
                    <small class="muted">Продажа недвижимости, аренда квартир в Украине</small>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
