<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MakeAdvertismentSpecial.aspx.cs" Inherits="Payments_MakeAdvertismentSpecial" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <title>Как выделить Ваше объявление на сайте - Недвижимость-UA</title>

    <meta name="description" content="" />
    <meta name="keywords" content="объявление, выделить, на сайте, платные объявления" />
    
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <form runat="server">
        <h2>Как выделить Ваше объявление на сайте</h2>

        <p>
            Для того, чтобы Ваше объявление о недвижимости было наиболее заметно и приносило <strong>максимальную
            отдачу</strong> - Вы можете <strong>выделить</strong> Ваше объявление в списке объявлений в 
            соответствующем разделе нашего сайта. 
        </p>

        <div style="text-align: center;">
            <img src="../img/special_advertisment.png" width="400px" alt="Специальное объявление в списке" />
        </div>

        <p>
            Стоимость данной услуги составляет - <strong>5 грн.</strong>
        </p>
        <p>
            Срок выделения - <strong>1 неделя.</strong>
        </p>

        <p>
            Для выделения Вашего объявления введите свой номер телефон, выберите объявление или объявления, 
            которые Вы хотите выделить и нажмите кнопку "Заказать выделение", автоматически Вы 
            будете перенаправлены на сайт сервиса "Робокасса", где Вам будет предложено оплатить выделение
            одним из предложенных вариантов оплаты. Сразу после успешной оплаты Ваши выбранные 
            объявления станут <strong>выделенными</strong> в списке объявлений.
        </p>

        <asp:ScriptManager runat="server" ID="ScriptManager" />
        <asp:UpdatePanel runat="server" UpdateMode="Always" ID="UpdatePanel">
            <ContentTemplate>
                <div style="width: 500px; margin: 20px auto 0 auto;">
                    <fieldset>
                        <label><strong>Введите номер телефона:</strong></label>
                        <div class="form-inline">
                            <asp:TextBox runat="server" ID="tPhoneNumber" placeholder="Номер телефона" />
                            <asp:Button Text="Поиск" runat="server" ID="bFindByPhoneNumber" CssClass="btn" OnClick="bFindByPhoneNumber_Click" />
                        </div>
                    </fieldset>
                    <br />
                    <strong>Выбранные объявления:</strong>
                    <table class="table">
                    <asp:DataList runat="server" DataKeyField="Id" ID="dlAdvertismentsList" OnItemDataBound="dlAdvertismentsList_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 80px;">
                                    <asp:CheckBox runat="server" ID="chkAdvertisment" Checked="false" OnCheckedChanged="chkAdvertisment_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "text") %>
                                </td>
                                <td>
                                    <%# ((DateTime)DataBinder.Eval(Container.DataItem, "createDate")).ToShortDateString() %>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                </td>
                            </tr>
                        </SeparatorTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblEmpty" Text="Нет выбранных объявлений" runat="server" CssClass="muted" 
                                Visible='<%# bool.Parse((dlAdvertismentsList.Items.Count == 0).ToString()) %>'> 
                            </asp:Label> 
                        </FooterTemplate>
                    </asp:DataList>
                    </table>
                </div>

                <div style="margin-top: 20px;text-align: center;">
                    <a runat="server" id="lnkPay" class="button orange" style="font-size: 18px;" onclick="javascript:lnkPayClick()">
                        <i class="icon-shopping-cart icon-white" style="vertical-align: middle;"></i> Заказать выделение
                    </a>
                </div>

                <script type="text/javascript">
                    function lnkPayClick() {
                        var href = $("#<%=lnkPay.ClientID%>").attr('href');
                        if (href == '' || !href) {
                            $('#NoHrefModal').modal('show');
                        }
                    }
                </script>

                <!-- Modal -->
                <div id="NoHrefModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalLabel">Не выбраны объявления для выделения</h3>
                  </div>
                  <div class="modal-body">
                    <p>Пожалуйста, введите номер для поиска объявления, нажмите "Поиск" и из списка 
                        выберите объявления для выделения.</p>
                  </div>
                  <div class="modal-footer">
                    <button class="btn" data-dismiss="modal" aria-hidden="true">Закрыть</button>
                  </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
