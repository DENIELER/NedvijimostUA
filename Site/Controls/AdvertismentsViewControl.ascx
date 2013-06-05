<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdvertismentsViewControl.ascx.cs" Inherits="AdvertismentsViewControl" %>

<div runat="server" id="pnlAdvRentRecords">

    <div style="height: 280px;">
        <div class="span6">
            <script type="text/javascript"><!--
                google_ad_client = "ca-pub-5891602113354577";
                /* Баннер на WithMe квадрат */
                google_ad_slot = "1289867380";
                google_ad_width = 336;
                google_ad_height = 280;
                //-->
            </script>
            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>
        </div>
        <div class="span6">
            <script type="text/javascript"><!--
                google_ad_client = "ca-pub-5891602113354577";
                /* Баннер на WithMe квадрат */
                google_ad_slot = "1289867380";
                google_ad_width = 336;
                google_ad_height = 280;
                //-->
            </script>
            <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
            </script>
        </div>
    </div>

    <div id="advertisments_header" style="margin-top: 25px;"></div>

    <div class="row-fluid">
        <div class="span12">
            <div class="row-fluid">
                <div class="span8" style="text-align: center;"><strong>Текст объявления</strong></div>
                <div class="span4" style="text-align: center;"><strong>Телефоны</strong></div>
            </div>
            <hr class="advSeparator"/>
        </div>
    </div>
    
    <div id="advertisments">
        <div class="loading_container">
            <div class="loading"></div>
        </div>
    </div>

    <script src="/js/jquery.masonry.min.js"></script>
    
    <%--<asp:ListView runat="server" ID="lvAdvertisments" OnItemDataBound="lvAdvertisments_ItemDataBound">
        <ItemTemplate>
            <div style="margin-top: 10px;" id="advertisment_<%# Container.DataItemIndex %>">
                <div class="row-fluid<%# (bool)DataBinder.Eval(Container.DataItem, "isSpecial") ? " special_advertisment" : "" %> advertisment_block">
                    <div class="span12">
                        <div class="row-fluid">
                            <div class="span9">
                                <%# Utils.StripTagsRegex((string)DataBinder.Eval(Container.DataItem, "text")) %>
                            </div>

                            <div class="span3">
                                <ul class="unstyled" id="phones_<%# Container.DataItemIndex %>">
                                    <asp:ListView runat="server" ID="lvAdvertismentPhones" DataSource='<%# DataBinder.Eval(Container.DataItem, "AdvertismentPhones") %>' ItemPlaceholderID="AdvertismentsPhones">
                                        <LayoutTemplate>
                                            <asp:PlaceHolder runat="server" ID="AdvertismentsPhones"></asp:PlaceHolder>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <li><strong><%# DataBinder.Eval(Container.DataItem, "phone") %></strong></li>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ul>
                                <% if(!NotShowSubpurchaseCheckbox) {%>
						        <div runat="server" id="divSubpurchaseCheckBox">
                                    <input type="checkbox" onchange="checkboxIsSubpurchase(<%# Container.DataItemIndex %>, this);" />
                                    <span style="font-size: 0.7em;margin-left: 5px;">Думаю, это посредник</span>
						        </div>
                                <% } %>
                                <div runat="server" id="divNotThemeAdvertismentCheckBox">
                                    <input type="checkbox" onchange="notThemeAdvertisment(<%# Container.DataItemIndex %>, this, <%# DataBinder.Eval(Container.DataItem, "Id") %>);" />
                                    <span style="font-size: 0.7em;margin-left: 5px;">Объявление не по теме</span>
						        </div>
                                <% if(Authorization.IsAdmin(Authorization.GetVkontakteUserUid())) { %>
                                <div runat="server" id="divHideAdvertisment">
                                    <input type="checkbox" onchange="hideAdvertisment(<%# Container.DataItemIndex %>, this, <%# DataBinder.Eval(Container.DataItem, "Id") %>);" />
                                    <span style="font-size: 0.7em;margin-left: 5px;">Удалить объявление</span>
						        </div>
                                <% } %>
                            </div>
                        </div>
                        
                        <asp:ListView runat="server" ID="lvAdvertismentPhotos" DataSource='<%# DataBinder.Eval(Container.DataItem, "AdvertismentsPhotos") %>' ItemPlaceholderID="AdvertismentsPhotos">
                            <LayoutTemplate>
                                <div class="row-fluid">
                                    <div class="span8">
                                        <strong>Фото:</strong>
                                        <div id="gallery" data-toggle="modal-gallery" data-target="#modal-gallery" data-selector="div.gallery-item">
                                            <asp:PlaceHolder runat="server" ID="AdvertismentsPhotos"></asp:PlaceHolder>
                                        </div>
                                    </div>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="gallery-item" data-href="<%# FormatGalleryPhoto(DataBinder.Eval(Container.DataItem, "filename").ToString()) %>"><img src="<%# DataBinder.Eval(Container.DataItem, "filename") %>" style="height: 80px;"/></div>
                            </ItemTemplate>
                        </asp:ListView>

                        <div class="advertisment_info_footer">
                            <span>Дата обновления: <%# ((DateTime)DataBinder.Eval(Container.DataItem, "createDate")).ToString("g") %></span>
                            &emsp;
                            <% if(Authorization.IsAdmin(Authorization.GetVkontakteUserUid())){ %>
                            Источник: <noindex><a rel="noindex, nofollow" href='<%# DataBinder.Eval(Container.DataItem, "link") %>'><%# CutToFirstSpace((string)DataBinder.Eval(Container.DataItem, "siteName")) %></a></noindex>
                            <% } %>
                        </div>
                    </div>

                    <a runat="server" visible="false" id="lnkHowToMakeSpecial" href="~/Kak-videlit-obyavlenie" style="float:right; font-size: 0.7em; color: #DA7C0C; line-height: 0.7em; opacity:0.5;">Как попасть сюда</a>
                </div>
            </div>

            <asp:Panel style="margin-top: 10px;" runat="server" Visible="<%# Container.DataItemIndex == 2 %>">
                <div class="row-fluid advertisment_block">
                    <div class="span6">
                        <script type="text/javascript"><!--
                            google_ad_client = "ca-pub-5891602113354577";
                            /* Баннер на WithMe квадрат */
                            google_ad_slot = "1289867380";
                            google_ad_width = 336;
                            google_ad_height = 280;
                            //-->
                        </script>
                        <script type="text/javascript"
                        src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                        </script>
                    </div>
                    <div class="span6">
                        <script type="text/javascript"><!--
                            google_ad_client = "ca-pub-5891602113354577";
                            /* Баннер на WithMe квадрат */
                            google_ad_slot = "1289867380";
                            google_ad_width = 336;
                            google_ad_height = 280;
                            //-->
                        </script>
                        <script type="text/javascript"
                        src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                        </script>
                    </div>
                </div>
            </asp:Panel>
        </ItemTemplate>
        <EmptyItemTemplate>
            <pre style="text-align: center;">
                Извините, в этом разделе нет объявлений
            </pre>
        </EmptyItemTemplate>
    </asp:ListView>--%>
    
    <%--<div style="text-align: center;">
        <asp:DataPager runat="server" ID="dpAdvertisments" PagedControlID="lvAdvertisments" PageSize="100" QueryStringField="page">
            <Fields>
                <asp:NextPreviousPagerField 
                    ButtonType="Link" 
                    ShowPreviousPageButton="true"
                    ShowNextPageButton="false"
                    PreviousPageText="Предыдущая" RenderDisabledButtonsAsLabels="true" />

                <asp:NumericPagerField 
                    PreviousPageText="&lt; Предыдущие 100"
                    NextPageText="Следующие 100 &gt;"
                    ButtonCount="100" />

                <asp:NextPreviousPagerField
                    ButtonType ="Link"
                    ShowNextPageButton="true"
                    ShowPreviousPageButton="false" 
                    NextPageText="Следующая" RenderDisabledButtonsAsLabels="true"/>
            </Fields>
        </asp:DataPager>
    </div>--%>

    <div style="margin-top: 10px; text-align:center;">
        <script type="text/javascript"><!--
            google_ad_client = "ca-pub-5891602113354577";
            /* NedvijimostUA между объяв */
            google_ad_slot = "2757750864";
            google_ad_width = 468;
            google_ad_height = 60;
            //-->
        </script>
        <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
        </script>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        getAdvertisments('<%= Settings.SectionId %>', '<%= Settings.SubSectionId != null ? Settings.SubSectionId.ToString() : "null" %>', '<%= Convert.ToInt32(Settings.Filter) %>', '<%= Settings.Offset %>', '<%= Settings.Limit %>', '<%= Settings.Date != null ? Settings.Date.Value.ToShortDateString() : "" %>');
    });

    var isAuthorizedUserAsAdmin = false;
    function getAdvertisments(sectionId, subSectionId, filter, offset, limit, date)
    {
        //var getAdvertismentsUrl = "=  //Page.RouteData.RouteHandler != null 
                //? ResolveUrl(((System.Web.Routing.PageRouteHandler)Page.RouteData.RouteHandler).VirtualPath + "/GetAdvertisments") 
                //: ResolveUrl(Request.RawUrl + "/GetAdvertisments") ";

        var getAdvertismentsUrl = "/WebServices/AdvertismentsService.asmx/GetAdvertisments";
        var filterAdvertismentsData = 
            "{ "+
            "\"sectionId\": " + sectionId + "," +
            "\"subSectionId\": " + subSectionId + "," +
            "\"filter\": " + filter + "," +
            "\"offset\": " + offset + "," +
            "\"limit\": " + limit + "," +
            "\"date\": '" + date + "'" +
            " }";
        $.ajax({
            url: getAdvertismentsUrl,
            type: "POST",
            data: filterAdvertismentsData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                getAdvertismentsFromJson(data);
            },
            error: function (data) {
                showAdvertismentsLoadingError(data);
            }
        });
    }
    function getAdvertismentsFromJson(response)
    {
        var advertismentsView = response.d;

        $('#advertisments_header').append(prepairHeader(advertismentsView));

        $('#advertisments').empty();
        $('#advertisments').append(prepairAdvertisments(advertismentsView));
        $('#advertisments').append(prepairPagging(advertismentsView));

        //--- format photos
        var $photosContainer = $('.gallery');
        $photosContainer.imagesLoaded(function () {
            $photosContainer.masonry({
                itemSelector: '.gallery-item',
                //columnWidth: 300,
                //gutterWidth: 20
            });
        });

        var bootstrapLoadImage = 'http://blueimp.github.com/JavaScript-Load-Image/load-image.min.js';
        var bootstrapImageGallery = '/js/bootstrap-image-gallery.min.js';
        $.getScript(bootstrapLoadImage);
        $.getScript(bootstrapImageGallery);
    }
    function prepairHeader(view) {
        return '<pre>' +
                'Количество объявлений: ' + view.AdvCountToShow + ' из ' + view.FullCount +
               '</pre>';
    }
    function prepairAdvertisments(view) {
        var advertismentDivs = '';

        isAuthorizedUserAsAdmin = '<%= Authorization.Authorization.CurrentUser_IsAdmin() %>';
        
        var advertisments = view.Advertisments;
        
        for (var i = 0; i < advertisments.length; i++) {
            adv = advertisments[i];

            var advertismentBlockStyle = '';
            if (adv.isSpecial)
                advertismentBlockStyle = 'special_advertisment';

            var phonesDiv = prepairPhones(adv.Phones, i);
            var photosDiv = prepairPhotos(adv.Photos, i);

            var removeAdvertismentButton = '';
            if(isAuthorizedUserAsAdmin)
                removeAdvertismentButton = '<a class="btn btn-mini" onclick="AdminRemoveAdvertisment(' + i + ', this, ' + adv.Id + ');"><i class="icon-trash"></i> Удалить объявление</a>&nbsp;'

            var advertismentDiv =
                '<div style="margin-top: 10px;" id="advertisment_' + i + '">' +
                    '<div class="' + advertismentBlockStyle + ' advertisment_block">' +
                            '<div class="row-fluid">' + 
                                '<div class="row-fluid">' +
                                    '<div class="span9">' + adv.text + '</div>' +
                                    '<div class="span3">' + phonesDiv + '</div>' +
                                '</div>' +
                                photosDiv +
                            '</div>' +
                            '<div class="clear:both">&nbsp;</div>' + 
                            '<div class="advertisment_info_footer row-fluid">' +
                                '<div class="span5">' + 
                                    '<span>Дата обновления:' + adv.createDateFormated + '</span>&emsp;' +
                                '</div>' +
                                '<div class="span7" style="text-align: right;">' +
                                    removeAdvertismentButton +
                                    '<div class="btn-group">' + 
                                        '<button class="btn dropdown-toggle btn-mini" data-toggle="dropdown"><i class="icon-exclamation-sign"></i> Сообщить <span class="caret"></span></button>' +
                                            '<ul class="dropdown-menu">' +
                                                '<li><a onclick="MarkAsSubpurchase(' + i + ', this);">Объявление посредника</a></li>' +
                                                '<li><a onclick="MarkAsNotByTheme(' + i + ', this, ' + adv.Id + ');">Объявление не по теме</a></li>' +
                                            '</ul>' +
                                    '</div>' +
                                '</div>' + 
                            '</div>' +
                    '</div>' +
                '</div>';
            advertismentDivs += advertismentDiv;
        };

        return advertismentDivs;
    }
        function prepairPhones(phones, index) {
            var phonesDiv = '<ul class="unstyled phones" id="phones_' + index + '">';
            $.each(phones, function () {
                phonesDiv += '<li><strong>' + this.phone + '</strong></li>';
            });
            phonesDiv += '</ul>';

            return phonesDiv;
        }
        function prepairPhotos(photos, index) {
            if (photos.length <= 0)
                return '';

            var block_height = '';
            if (photos.length <= 3)
                block_height = ' style="min-height: 80px;"';

            var photosDiv = '<div class="row-fluid"' + block_height + '>' +
                                '<div class="span8">' +
                                    '<strong>Фото:</strong>' +
                                    '<div class="gallery" data-toggle="modal-gallery" data-target="#modal-gallery" data-selector="div.gallery-item">';
            
            $.each(photos, function () {
                photosDiv += '<div class="gallery-item" data-href="' + this.filenameFormated + '">' +
                                '<img src="' + this.filename + '"/>' +
                             '</div>';
            });
            photosDiv +=            '</div>' +
                                '</div>' +
                            '</div>';

            return photosDiv;
        }
    function prepairPagging(view) {
        var paggingBlock = '';

        var advCount = view.AdvCountToShow;
        var currentPage = parseInt('<%= Math.Round((decimal)Settings.Offset / (decimal)Settings.Limit) + 1 %>');
        var pageAdvCount = parseInt('<%= Settings.Limit %>');
        var lastPage = Math.round(advCount / pageAdvCount);

        if (pageAdvCount > advCount)
            return '';

        var previousDisable = '', nextDisable = '', prevPageUrl = '', nextPageUrl = '';
        var prevPage = currentPage - 1;
        var nextPage = currentPage + 1;

        if(currentPage <= 1)
        {
            previousDisable = ' disabled';
            prevPageUrl = '#';
        }
        else
        prevPageUrl = 'href="' + insertURLParam('<%= Request.RawUrl %>', 'page', prevPage) + '"';
        if (currentPage >= lastPage)
        {
            nextDisable = ' disabled';
            nextPageUrl = '';
        }
        else nextPageUrl = 'href="' + insertURLParam('<%= Request.RawUrl %>', 'page', nextPage) + '"';

        
        paggingBlock =
            '<div>' +
                '<ul class="pager">' +
                    '<li class="previous' + previousDisable + '">' +
                        '<a ' + prevPageUrl + '>&larr; Previous</a>' +
                    '</li>' +
                    '<li class="next' + nextDisable + '">' +
                        '<a ' + nextPageUrl + '>Next &rarr;</a>' +
                    '</li>' +
                '</ul>' + 
            '</div>';
                
        return paggingBlock;
    }
    function showAdvertismentsLoadingError(response) {
        $('#advertisments').empty();
        $('#advertisments').html(
            '<div style="text-align: center;">' +
                'Извините, но произошла ошибка и объявления не загружены. Попробуйте перезагрузить страницу.' + 
            '</div>'
            );
    }

    function MarkAsSubpurchase(subpurchaseIndex, val) {
        if (confirm("Вы уверены, что данное объявление является объявлением посредника?")) {
            val.disabled = true;

            var phonesList = new Array();
            $("#phones_" + subpurchaseIndex + " li").each(function () {
                phonesList.push($(this).text());
            });

            var jsonText = JSON.stringify({ phonesList: phonesList });

            $.ajax({
                type: "POST",
                url: "/WebServices/SubpurchaseService.asmx/MarkAsSubpurchase",
                data: jsonText,
                contentType: "application/json;charset=urf-8",
                dataType: "json",
                success: function (result) {
                    $("#advertisment_" + subpurchaseIndex).fadeOut(900);
                }
            });
        }
    }
    function AdminRemoveAdvertisment(subpurchaseIndex, val, advertisment_id) {
        if (confirm("Вы уверены, что хотите удалить данное объявление?")) {
            val.disabled = true;
            var phonesList = new Array();

            $.ajax({
                type: "POST",
                url: "/WebServices/AdvertismentsService.asmx/HideAdvertisment",
                data: "{ advertisment_id: " + advertisment_id + "}",
                contentType: "application/json;charset=urf-8",
                dataType: "json",
                success: function (result) {
                    $("#advertisment_" + subpurchaseIndex).fadeOut(500);
                }
            });
        }
    }
    function MarkAsNotByTheme(subpurchaseIndex, val, advertisment_id) {
        if (confirm("Вы уверены, что данное объявление не является объявлением о недвижимости?")) {
            val.disabled = true;
            var phonesList = new Array();

            $.ajax({
                type: "POST",
                url: "/WebServices/AdvertismentsService.asmx/MarkAsNotByTheme",
                data: "{ advertisment_id: " + advertisment_id + "}",
                contentType: "application/json;charset=urf-8",
                dataType: "json",
                success: function (result) {
                    $("#advertisment_" + subpurchaseIndex).fadeOut(800);
                }
            });
        }
    }

    function insertURLParam(url, key, value) {
        var _url = url;
        if (_url.indexOf(key + "=") >= 0) {
            var prefix = _url.substring(0, _url.indexOf(key));
            var suffix = _url.substring(_url.indexOf(key)).substring(_url.indexOf("=") + 1);
            suffix = (suffix.indexOf("&") >= 0) ? suffix.substring(suffix.indexOf("&")) : "";
            _url = prefix + key + "=" + value + suffix;
        }
        else {
            if (_url.indexOf("?") < 0)
                _url += "?" + key + "=" + value;
            else
                _url += "&" + key + "=" + value;
        }
        return _url;
    }
</script>

<!-- modal-gallery is the modal dialog used for the image gallery -->
<div id="modal-gallery" class="modal modal-gallery hide fade" style="width:auto;">
    <div class="modal-header">
        <a class="close" data-dismiss="modal">&times;</a>
        <h3 class="modal-title"></h3>
    </div>
    <div class="modal-body"><div class="modal-image"></div></div>
    <div class="modal-footer">
        <a class="btn btn-info modal-prev"><i class="icon-arrow-left icon-white"></i> Назад</a>
        <a class="btn btn-primary modal-next">Дальше <i class="icon-arrow-right icon-white"></i></a>
    </div>
</div>
