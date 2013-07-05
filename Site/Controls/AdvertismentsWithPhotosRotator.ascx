<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdvertismentsWithPhotosRotator.ascx.cs" Inherits="Controls_AdvertismentsWithPhotosRotator" %>

<div id="divAdvertismentsWithPhotosRotator" style="position:relative; overflow:hidden; width: 100%; height:150px;">
    <div class="scroll_items" style="position:absolute; width:20000em;">
        <asp:Repeater runat="server" ID="rptAdvertisments">
            <ItemTemplate>
                <div class="scroll_item" style="float:left; margin: 0 5px;">
                    <a href="<%= "/Obyavleniya-Arenda/Sdam-kvartiru" %>" style="display: block; text-align: center;">
                        <img style="height: 150px;" src="<%# FormatPhotoFileName(((Model.viewAdvertismentPhoto)Container.DataItem).PhotoFileName)  %>" alt="<%# ((Model.viewAdvertismentPhoto)Container.DataItem).siteName %>"/>
                        <%--<div>
                            <%# ((Model.viewAdvertismentPhotos)Container.DataItem).SectionDisplayName %>
                        </div>--%>
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $.easing.custom = function (x, t, b, c, d) {
            var s = 1.70158;
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (0)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
        };

        $("#divAdvertismentsWithPhotosRotator").scrollable({ easing: 'custom', speed: 800, circular: true }).autoscroll({ autoplay: true });

    });
</script>