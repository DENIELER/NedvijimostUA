<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyAdvertisments.aspx.cs" Inherits="MyAdvertisments" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Мои объявления | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
        <h2>Управление моими объявлениями</h2>
        <p>
            Вы можете управлять своими объявлениями: удалять, редактировать, добавлять когда и где вам необходимо.
            Желаем удачных сделок в сфере недвижимости! :)
        </p>

        <% if(editAdvertismentID == null) { %>
        <form id="Form1" runat="server">
            <asp:LinqDataSource ID="ldsAdvertisments" runat="server" ContextTypeName="Model.NedvijimostDBEntities" EntityTypeName="" OrderBy="createDate desc" Select="new (Id, text, createDate, modifyDate, AdvertismentsPhotos, AdvertismentSection, AdvertismentPhones)" TableName="Advertisments" Where="UserID == @UserID &amp;&amp; not_show_advertisment != @not_show_advertisment">
                <WhereParameters>
                    <asp:Parameter DefaultValue="-1" Name="UserID" Type="Int32" />
                    <asp:Parameter DefaultValue="True" Name="not_show_advertisment" Type="Boolean" />
                </WhereParameters>
            </asp:LinqDataSource>
            <div style="margin-top: 40px;">
                <asp:Repeater ID="rptAdvertisments" runat="server" DataSourceID="ldsAdvertisments">
                    <ItemTemplate>
                        <div class="advertisment_block row-fluid">
                            <div class="span8">
                                <strong>Текст:</strong>
                                <div style="padding: 10px;">
                                    <%# Eval("text") %>
                                </div>
                            </div>
                            <div class="span2" style="text-align: center;">
                                <strong>Редактировать</strong>
                                <div style="margin-top: 15px;">
                                    <a href="/my-advertisments?edit=<%# Eval("Id") %>">
                                        <img src="img/1370565845_018.png" />
                                    </a>
                                </div>
                            </div>
                            <div class="span2" style="text-align: center;">
                                <strong>Удалить</strong>
                                <div style="margin-top: 15px;">
                                    <asp:LinkButton runat="server" ID="lnkDelete" CommandArgument='<%# Eval("Id") %>' OnCommand="lnkDelete_Command">
                                        <img src="img/1370565849_101.png" />
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </form>
        <% }else{ %>
            <form class="form-horizontal" runat="server">
                <div class="control-group">
                    <label class="control-label">Текст</label>
                    <div class="controls">
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtText"/>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="inputPassword">Password</label>
                    <div class="controls">
                        <input type="password" id="inputPassword" placeholder="Password">
                    </div>
                </div>
                <div class="control-group">
                    <div class="controls">
                        <label class="checkbox">
                            <input type="checkbox"> Remember me
                        </label>
                        <button type="submit" class="btn">Sign in</button>
                    </div>
                </div>
            </form>
        <% } %>
</asp:Content>