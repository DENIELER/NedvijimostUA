<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AuthorizationControl.ascx.cs" Inherits="Controls_AuthorizationControl" %>

<%
    if(!Authorization.Authorization.IsUserAuthorized())
    {
%>
<div id="authorization" style="background-color: white; height: 110px; padding: 5px 0;">
    <div style="clear:both;">Вход: <a href="/register" style="float:right;">Регистрация</a></div>
    <form id="authorizationForm" class="form-inline" action="/authorization" method="post">
      <input name="login" type="text" class="input" placeholder="Email/Логин" />
      <input name="password" type="password" class="input" placeholder="Пароль" />
      <label class="checkbox">
        <input name="rememberMe" type="checkbox" /> Запомнить меня
      </label>
      <button type="submit" class="btn" style="float:right;">Войти</button>
    </form>
    <div style="clear:both;"></div>
</div>
<%
    }else{
%>
<div id="user_info" style="background-color: white; height: 130px; padding: 5px 0;">
    <div>
        Здравствуйте, <strong><%= Authorization.Authorization.CurrentUser_Login() %></strong>!
    </div>
    <div style="margin: 20px 0;">Ваш телефон: <%= Authorization.Authorization.CurrentUser_Phone() ?? "еще не указан" %></div>
    <div style="clear:both;">
        <a href="/user-options">Настройки</a>
        <a href="/authorization" style="float:right;">Выйти</a>
    </div>
</div>
<%
    }
%>