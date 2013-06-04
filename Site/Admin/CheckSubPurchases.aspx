<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckSubPurchases.aspx.cs" Inherits="Admin_CheckSubPurchases" MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head"></asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Body">
    <form id="MainForm" runat="server">
        <div runat="server" id="pnlSites">
            <h2 class="block_header">Отмеченные пользователями посредники <hr/></h2>

            <asp:DataList ID="dlCheckSubPurchases" runat="server" CellPadding="4" ForeColor="#333333" OnEditCommand="dlCheckSubPurchases_EditCommand" OnUpdateCommand="dlCheckSubPurchases_UpdateCommand" DataSourceID="ldsSubPurchases" OnCancelCommand="dlCheckSubPurchases_CancelCommand" OnDeleteCommand="dlCheckSubPurchases_DeleteCommand">
                <AlternatingItemStyle BackColor="White" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="#EFF3FB" />
                <HeaderTemplate>
                    <table>
                        <tr>
                            <td style="width: 200px;">Телефон посредника</td>
                            <td style="width: 200px;">Имя\Название компании</td>
                            <td style="width: 200px;">Добавлено</td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="subpurchaseId" runat="server" Text='<%# Bind("SubPurchase.Id") %>' Visible="false"></asp:Label>
                    <table>
                        <tr>
                            <td style="width: 200px;"><asp:Label ID="phoneLabel" runat="server" Text='<%# Eval("SubPurchasePhone.phone") %>' /></td>
                            <td style="width: 200px;"><asp:Label ID="nameLabel" runat="server" Text='<%# Eval("SubPurchase.name") %>' />&nbsp;<asp:Label ID="surnameLabel" runat="server" Text='<%# Eval("SubPurchase.surname") %>' /></td>
                            <td style="width: 200px;"><asp:Label ID="createDateLabel" runat="server" Text='<%# Eval("SubPurchasePhone.createDate") %>' /></td>
                            <td><asp:Button runat="server" ID="btnEdit"  CommandName="edit" Text="Редактировать" /></td>
                            <td><asp:Button runat="server" ID="btnDelete"  CommandName="delete" Text="Удалить" /></td>
                            <td><asp:Button runat="server" ID="btnAdvertisment" Text="Объявление" CommandArgument='<%# Eval("SubPurchasePhone.phone") %>' OnCommand="btnAdvertisment_Command"/></td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="subpurchaseId" runat="server" Text='<%# Bind("SubPurchase.Id") %>' Visible="false"></asp:Label>
                    <table>
                        <tr>
                            <td style="width: 200px;">Не проверено: </td>
                            <td style="width: 200px;"><asp:CheckBox runat="server" ID="chkCheckedSubPurchase" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "SubPurchase.not_checked")) %>' /></td>
                        </tr>
                        <tr>
                            <td style="width: 200px;"><asp:Button runat="server" ID="btnUpdate" CommandName="update" Text="Применить" />&nbsp;<asp:Button runat="server" ID="btnCancel" CommandName="cancel" Text="Отмена" /></td>
                            <td style="width: 200px;">&nbsp;</td>
                        </tr>
                    </table>
                </EditItemTemplate>
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:DataList>
            <asp:LinqDataSource ID="ldsSubPurchases" runat="server" 
                ContextTypeName="Model.NedvijimostDBEntities" onselecting="ldsSubPurchases_Selecting">
                <WhereParameters>
                    <asp:Parameter DefaultValue="True" Name="not_checked" Type="Boolean" />
                </WhereParameters>
            </asp:LinqDataSource>

            <asp:Label runat="server" ID="lAdvText" />
        </div>
    </form>
</asp:Content>