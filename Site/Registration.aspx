<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" MasterPageFile="~/AdvertismentMaster.master"%>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Регистрация нового пользователя | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <h2>Регистрация нового пользователя</h2>
    
    <form runat="server" class="form-horizontal">
      <div class="control-group">
        <label class="control-label">Email*</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="txtEmail" placeholder="Email"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <label class="control-label">Логин</label>
        <div class="controls">
            <asp:TextBox runat="server" ID="txtLogin" placeholder="Логин"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <div class="controls" style="line-height: 20px;">
            <label class="checkbox">
                <asp:CheckBox runat="server" ID="chkSubpurchase" />  
                <small>
                    Являетесь ли Вы представителем агенства недвижимости или частным агентом?
                </small>
            </label>
            <br />
            <small class="muted">
                Для агенств недвижимости и частных агентов предоставляются специализированные сервисы и услуги.
            </small>
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
            <asp:TextBox runat="server" ID="txtPhone" placeholder="XXX-XXX-XX-XX"></asp:TextBox>
        </div>
      </div>
      <div class="control-group">
        <div class="controls" style="line-height: 42px;">
            <asp:RequiredFieldValidator CssClass="alert alert-error" runat="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="Введите Email пользователя"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator CssClass="alert alert-error" runat="server" Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ErrorMessage="Email имеет не правильный формат"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator CssClass="alert alert-error" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Введите пароль пользователя"></asp:RequiredFieldValidator>
            <asp:CompareValidator CssClass="alert alert-error" runat="server" ControlToValidate="txtPassword" ControlToCompare="txtRepeatPassword" Display="None" ErrorMessage="Введенные пароли не совпадают"></asp:CompareValidator>

            <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="alert alert-error"/>
        </div>
      </div>
      <div class="control-group">
        <div class="controls">
            <asp:Button runat="server" ID="btnRegister" Text="Зарегистрировать" CssClass="btn" OnClick="btnRegister_Click" />
        </div>
      </div>
    </form>
</asp:Content>