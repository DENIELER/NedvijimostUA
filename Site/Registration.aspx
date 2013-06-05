<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" MasterPageFile="~/AdvertismentMaster.master"%>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Регистрация нового пользователя | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <h2>Регистрация нового пользователя</h2>
    
    <form runat="server" class="form-horizontal">
      <div class="control-group">
        <label class="control-label">Email/Логин*</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="txtEmailLogin" placeholder="Email/Логин"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <label class="control-label">Пароль*</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="txtPassword" placeholder="Пароль" TextMode="Password"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <label class="control-label">Повторите пароль*</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="txtRepeatPassword" placeholder="Повторите пароль" TextMode="Password"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <label class="control-label">Телефон</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="txtPhone" placeholder="Телефон"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <div class="controls" style="line-height: 42px;">
            <asp:RequiredFieldValidator CssClass="alert alert-error" runat="server" ControlToValidate="txtEmailLogin" Display="Dynamic" ErrorMessage="Введите Email или Логин пользователя<br/>"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator CssClass="alert alert-error" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="Введите пароль пользователя<br/>"></asp:RequiredFieldValidator>
            <asp:CompareValidator CssClass="alert alert-error" runat="server" ControlToValidate="txtPassword" ControlToCompare="txtRepeatPassword" Display="Dynamic" ErrorMessage="Введенные пароли не совпадают<br/>"></asp:CompareValidator>
        </div>
      </div>
      <div class="control-group">
        <div class="controls">
            <asp:Button runat="server" ID="btnRegister" Text="Зарегистрировать" CssClass="btn" OnClick="btnRegister_Click" />
        </div>
      </div>
    </form>
</asp:Content>