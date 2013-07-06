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

<script src="/js/blueimp-gallery.min.js"></script>
<script src="/js/jquery.masonry.min.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        //--- format photos
        var $photosContainer = $('.gallery');
        $photosContainer.imagesLoaded(function () {
            $photosContainer.masonry({
                itemSelector: '.gallery-item',
                //columnWidth: 300,
                //gutterWidth: 20
            });
        });

        $('.gallery').each(function (index) {
            var linksContainer = $(this).on('click', 'div', function (event) {
                // Show the Gallery as lightbox when selecting a link,
                // starting with the selected image:
                if (blueimp.Gallery(linksContainer.find('div'), {
                    index: $(this).data('index')
                })) {
                    // Prevent the default link action on
                    // successful Gallery initialization:
                    event.preventDefault();
                }
            });
        });
    });

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

<!-- The Gallery as lightbox dialog, should be a child element of the document body -->
<div id="blueimp-gallery" class="blueimp-gallery">
    <div class="slides"></div>
    <h3 class="title"></h3>
    <a class="prev">‹</a>
    <a class="next">›</a>
    <a class="close">×</a>
    <a class="play-pause"></a>
    <ol class="indicator"></ol>
</div>