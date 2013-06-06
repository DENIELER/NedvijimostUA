<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserTopPanel.ascx.cs" Inherits="Controls_UserTopPanel" %>

<% if(Authorization.Authorization.IsUserAuthorized())
   { %>
<script type="text/javascript">
    $(window).scroll(function (e) {
        $el = $('#userTopPanel');
        if ($(this).scrollTop() > 0 && $el.css('position') != 'fixed') {
            $('#userTopPanel').css({ 'position': 'fixed'});
        }

        if ($(this).scrollTop() <= 0 && $el.css('position') != 'inherit') {
            $('#userTopPanel').css({ 'position': 'inherit' });
        }
    });
</script>

<%--Flat for Linux icons--%>
<div id="userTopPanel">
    <div class="innerUserPanelBlock">
        <div class="userMenuItem">
            <a href="/my-advertisments">
                <img src="../img/1370563209_27-Edit Text.png" alt="Мои объявления" />
                Мои объявления
            </a>
        </div>
    </div>
</div>
<% } %>