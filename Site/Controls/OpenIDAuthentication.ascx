<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenIDAuthentication.ascx.cs" Inherits="Controls_OpenIDAuthentication" %>

<script type="text/javascript">
    $(function () {
        $("#AuthorizedUser").hide();
        $("#NotAuthorizedUser").hide();

        VK.Auth.getLoginStatus(function (response) {
            if (response.session) {
                $("#AuthorizedUser").show();
            } else {
                $("#NotAuthorizedUser").show();
            }
        }, true);

        $("#UserLogout").click(function () {
            $.ajax({
                type: "POST",
                url: "/Auth/AuthorizeVkontakte.aspx/LogOff",
                data: "{}",
                contentType: "application/json;charset=urf-8",
                dataType: "json"
            });

            VK.Auth.logout(function (result) {
                document.location = "<%= Request.RawUrl %>";
            });
        });
    });
</script>

<div id="AuthorizedUser">
    <% if(Authorization.IsUserVkontakteAuthorized())
       { %>
        <div style="background-color: white; padding: 10px 0;">
            <div class="row-fluid">
                <div class="span3">
                    <a href="http://vk.com/<%= UserUid %>">
                        <img src="<%= Photo %>" alt="<%= UserUid %>" />
                    </a>
                </div>
                <div class="span9">
                    <div>
                        <span class="label label-info pull-right"><%= LoginName %></span>
                    </div>

                    <% if(Authorization.IsAdmin(Authorization.GetVkontakteUserUid())){ %>
                    <div>
                        <a href="../Admin/SubPurchases.aspx" style="font-size: 0.8em;">Добавить посредника(ов)</a>
                    </div>
                    <div>
                        <a href="../Admin/CheckSubPurchases.aspx" style="font-size: 0.8em;">Отмеченные посредники</a>
                    </div>
                    <% } %>

                    <div style="padding-top: 5px;">
                            <a href="#" id="UserLogout" class="pull-right">Выйти</a>
                    </div>
                </div>
            </div>
        </div>
    <% } %>
</div>

<div id="NotAuthorizedUser">
    <div id="vk_auth"></div>
    <script type="text/javascript">
        VK.Widgets.Auth("vk_auth", {
            width: "200px",
            authUrl: '/Auth/AuthorizeVkontakte.aspx?o=-1&amp;action=AuthorizationVkontakte'
        });
    </script>
</div>