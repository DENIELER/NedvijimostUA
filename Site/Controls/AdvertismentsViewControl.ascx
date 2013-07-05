<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdvertismentsViewControl.ascx.cs" Inherits="AdvertismentsViewControl" %>

<div runat="server" id="pnlAdvRentRecords">

    <noindex>
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
    </noindex>

    <div id="advertisments_header" style="margin-top: 25px;" runat="server"></div>

    <div class="row-fluid">
        <div class="span12">
            <div class="row-fluid">
                <div class="span8" style="text-align: center;"><strong>Текст объявления</strong></div>
                <div class="span4" style="text-align: center;"><strong>Телефоны</strong></div>
            </div>
            <hr class="advSeparator"/>
        </div>
    </div>
    
    <div id="advertisments" runat="server">
        <div class="loading_container">
            <div style="margin-top: 20px;">
                Идет загрузка объявлений.. 
            </div>
            <div class="loading"></div>

            <div class="muted">
                <small>
                    Если объявления не были загружены - пожалуйста попробуйте перезагрузить страницу 
                    или обратиться к нам в тех. поддержку 
                    <a href="mailto:support@nedvijimost-ua.com">support@nedvijimost-ua.com</a>.
                </small>
            </div>
        </div>
    </div>

    <script src="/js/jquery.masonry.min.js"></script>
    
    <noindex>
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
    </noindex>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        //getAdvertisments('<%= Settings.SectionId %>', '<%= Settings.SubSectionId != null ? Settings.SubSectionId.ToString() : "null" %>', '<%= Convert.ToInt32(Settings.Filter) %>', '<%= Settings.Offset %>', '<%= Settings.Limit %>', '<%= Settings.Date != null ? Settings.Date.Value.ToShortDateString() : "" %>');

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
    });

    function getAdvertisments(sectionId, subSectionId, filter, offset, limit, date)
    {
        //var getAdvertismentsUrl = "/WebServices/AdvertismentsService.asmx/GetAdvertismentsHtml";
        //var filterAdvertismentsData = 
        //    "{ "+
        //    "\"sectionId\": " + sectionId + "," +
        //    "\"subSectionId\": " + subSectionId + "," +
        //    "\"filter\": " + filter + "," +
        //    "\"offset\": " + offset + "," +
        //    "\"limit\": " + limit + "," +
        //    "\"date\": '" + date + "'" +
        //    " }";
        //$.ajax({
        //    url: getAdvertismentsUrl,
        //    type: "POST",
        //    data: filterAdvertismentsData,
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (data) {
        //        getAdvertismentsFromJson(data);
        //    },
        //    error: function (data) {
        //        showAdvertismentsLoadingError(data);
        //    }
        //});
    }
    function getAdvertismentsFromJson(response)
    {
        //var advertismentsView = response.d;

        //$('<%= "#" + advertisments_header.ClientID %>').append(advertismentsView.Header);

        //$('<%= "#" + advertisments.ClientID %>').empty();
        //$('<%= "#" + advertisments.ClientID %>').append(advertismentsView.Advertisments);
        //$('<%= "#" + advertisments.ClientID %>').append(advertismentsView.Pagging);

        ////--- format photos
        //var $photosContainer = $('.gallery');
        //$photosContainer.imagesLoaded(function () {
        //    $photosContainer.masonry({
        //        itemSelector: '.gallery-item',
        //        //columnWidth: 300,
        //        //gutterWidth: 20
        //    });
        //});

        //var bootstrapLoadImage = 'http://blueimp.github.com/JavaScript-Load-Image/load-image.min.js';
        //var bootstrapImageGallery = '/js/bootstrap-image-gallery.min.js';
        //$.getScript(bootstrapLoadImage);
        //$.getScript(bootstrapImageGallery);
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
