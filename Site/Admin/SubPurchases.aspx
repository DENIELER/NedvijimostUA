<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubPurchases.aspx.cs" Inherits="Admin_SubPurchases" MasterPageFile="~/Site.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <div runat="server" id="pnlSites">
            <h2 class="block_header">Администрирование посредников <hr/></h2>
            
            <% if(Request["success"] == "1" ){%>
            <div>
                <h4>Новый посредник успешно добавлен. Спасибо!</h4>
                <p>
                    <a href="SubPurchases.aspx">Добавить еще посредника</a>
                </p>
            </div>
            <%}else{ %>
            <div>
                <h4>Добавить нового посредника</h4>
                
                <div class="control-group">
                    <label class="control-label" for="inputSubPurchPhone">Телефон посредника:</label>
                    <div class="controls">
                        <textarea id="inputSubPurchPhone" placeholder="Телефон" runat="server" cols="20" rows="10" />
                    </div>
                </div>

                <div class="control-group">
                    <label class="control-label" for="inputSubPurchName">Имя посрденика \ Название компании:</label>
                    <div class="controls">
                        <input type="text" id="inputSubPurchName" placeholder="Имя" runat="server" />
                    </div>
                </div>

                <div class="control-group">
                    <div class="controls">
                        <asp:Button ID="btnAdvAdd" CssClass="btn" runat="server" Text="Добавить посредника" OnClick="AddNewSubPurchase"/>
                    </div>
                </div>
            </div>
            <%} %>
        </div>
    </form>
</asp:Content>