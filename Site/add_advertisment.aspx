﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="add_advertisment.aspx.cs" Inherits="add_advertisment" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Title">
    Разместить объявление недвижимости | Недвижимость-UA
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <meta name="description" content="Размещение нового объявления недвижимости на сервисе Недвижимость-UA, г.Харьков." />
    <meta name="keywords" content="Объявление, недвижимость, размещение объявления, продам, куплю, сдам, сниму, Харьков" />
    
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/themes/base/jquery-ui.css" id="theme">
    <link rel="stylesheet" href="/css/jquery.fileupload-ui.css">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    
    <form id="MainForm" runat="server" class="form-horizontal">
        <% if (!string.IsNullOrWhiteSpace(Request["success"]))
           {%>
            <h2 class="block_header">Объявление добавлено <hr/></h2>
            <p>Спасибо! Ваше объявление было успешно добавлено и размещено в списке объявлений.</p>
        <% }
           else
           { %>
        <div runat="server" id="pnlSites">
            <h2 class="block_header">Добавление нового объявления <hr/></h2>
            
            <p style="margin-top: 20px;">
                Добавьте свое объявление в нашу базу объявлений недвижимости города Харькова. При добавлении
                объявления правильно укажите раздел куда вы хотите добавить свое объявление.
            </p>

            <p style="margin-top: 20px;">
                Пожалуйста, указывайте номер телефона и адрес. Фотографии квартиры, объекта недвижимости 
                в объявлении будут несомненным плюсом. Чем больше информации Вы укажите при создании объявления, 
                тем меньше времени Вы потратите на ответы на нецелевые для Вас звонки.
            </p>

            <% if(!Authorization.Authorization.IsUserAuthorized()) { %>
            <p style="margin-top: 20px;">
                Внимание! Если Вы не <a href="/register">зарегистрируетесь</a>, то не сможете редактировать и управлять своим добавленным объявлением.
            </p>
            <% } %>

            <asp:ScriptManager runat="server" ID="ScriptManager"></asp:ScriptManager>
            <div style="width: 650px;margin: 0 auto;">
                <asp:UpdatePanel runat="server" ID="UpdatePanelAdvSection">
                    <ContentTemplate>
                        <div class="control-group">
                            <label class="control-label" for="inputType">Раздел объявлений:</label>
                            <div class="controls">
                                <asp:LinqDataSource runat="server" ID="ldsAdvertismentSections" ContextTypeName="Model.DataModel" EntityTypeName="" Select="new (Id, displayName, code)" TableName="AdvertismentSections"/>
                                <asp:DropDownList runat="server" ID="ddlAdvSection" AutoPostBack="true" DataSourceID="ldsAdvertismentSections" DataValueField="Id" DataTextField="displayName" OnSelectedIndexChanged="ddlAdvType_SelectedIndexChanged" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True" Value="-1">(Выберите раздел)</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="pnlAdvSubSections" CssClass="control-group" Visible="false">
                            <label class="control-label" for="inputType">Подраздел:</label>
                            <div class="controls">
                                <asp:DropDownList runat="server" ID="ddlAdvSubSection" DataValueField="Id" DataTextField="displayName">
                                </asp:DropDownList>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="control-group">
                    <label class="control-label" for="inputAddress">Адрес квартиры:</label>
                    <div class="controls">
                        <input type="text" id="inputAddress" placeholder="Адрес" runat="server" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="inputPhone">Телефон(-ы):</label>
                    <div class="controls">
                        <input type="text" id="inputPhone" placeholder="Телефон" runat="server" />
                    </div>
                </div>
            
                <div class="control-group">
                    <label class="control-label" for="inputAdvText">Текст объявления:</label>
                    <div class="controls">
                        <textarea type="text" id="inputAdvText" placeholder="Текст" runat="server" rows="10"/>
                    </div>
                </div>
                
                <div class="control-group">
                    <label class="control-label" for="inputPhotos">Фото:</label>
                    <div class="controls">
                        <p>При загрузке фото, пожалуйста, не забывайте загружать фото с помощью кнопки "Загрузить все фото". 
                        Если Вы не нажмете данную кнопку, то Ваши фото не будут загружены на сервер. Спасибо!</p>
                        <div id="fileupload">
                            <form method="POST" enctype="multipart/form-data">
                                <div class="fileupload-buttonbar ui-widget-header ui-corner-top" style="padding: 0;">
                                    <label class="fileinput-button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-icon-primary" style="margin-bottom: 0;">
                                        <span>Добавить фото...</span>
                                        <input type="file" name="files[]" multiple>
                                    </label>
                                    <button type="submit" class="start">Загрузить все фото</button>
                                    <%--<button type="reset" class="cancel">Отменить загрузку</button>
                                                            <button type="button" class="delete">Удалить фото</button>--%>
                                </div>
                            </form>
                            <div class="fileupload-content">
                                <table class="files" runat="server" id="FilesTable"></table>
                                <div class="fileupload-progressbar"></div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="control-group">
                    <div class="controls" style="line-height: 42px;">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="inputAdvText" Display="Dynamic" ErrorMessage="Текст объявления не может быть пустым.<br/>" CssClass="alert alert-error"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlAdvSection" Display="Dynamic" ErrorMessage="Не выбран раздел объявления.<br/>" InitialValue="-1" CssClass="alert alert-error"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="control-group">
                    <div class="controls">
                        <asp:Button ID="btnAdvAdd" CssClass="btn" runat="server" Text="Добавить объявление" OnClick="AddNewAdvertisment"/>
                    </div>
                </div>
            </div>
        </div>
        
        <script id="template-upload" type="text/x-jquery-tmpl">
            <tr class="template-upload{{if error}} ui-state-error{{/if}}">
                <td class="preview"></td>
                <td class="name">{{if name}}${name}{{else}}Untitled{{/if}}</td>
                <td class="size">${sizef}</td>
                {{if error}}
                    <td class="error" colspan="2"><b>Ошибка</b> :
                        {{if error === 'maxFileSize'}}Слишком большой размер файла (максимальный размер - 10 Mb)
                        {{else error === 'minFileSize'}}Слишком маленький размер файла
                        {{else error === 'acceptFileTypes'}}Извините, данный тип файлов не поддерживается
                        {{else error === 'maxNumberOfFiles'}}Загружено максимальное количество файлов
                        {{else}}${error}
                        {{/if}}
                    </td>
                {{else}}
                    <td class="start"><button>Загрузить</button></td>
                {{/if}}
                <td class="cancel"><button>Удалить</button></td>
            </tr>
        </script>
        <script id="template-download" type="text/x-jquery-tmpl">
            <tr class="template-download{{if error}} ui-state-error{{/if}}">
                {{if error}}
                    <td></td>
                    <td class="name">${name}</td>
                    <td class="size">${sizef}</td>
                    <td class="error" colspan="2"><b>Ошибка</b> :
                        {{if error === 1}}File exceeds upload_max_filesize (php.ini directive)
                        {{else error === 2}}File exceeds MAX_FILE_SIZE (HTML form directive)
                        {{else error === 3}}File was only partially uploaded
                        {{else error === 4}}No File was uploaded
                        {{else error === 5}}Missing a temporary folder
                        {{else error === 6}}Failed to write file to disk
                        {{else error === 7}}File upload stopped by extension
                        {{else error === 'maxFileSize'}}File is too big
                        {{else error === 'minFileSize'}}File is too small
                        {{else error === 'acceptFileTypes'}}Filetype not allowed
                        {{else error === 'maxNumberOfFiles'}}Max number of files exceeded
                        {{else error === 'uploadedBytes'}}Uploaded bytes exceed file size
                        {{else error === 'emptyResult'}}Empty file upload result
                        {{else}}Непредвиденная ошибка, пожалуйста попробуйте еще раз или обратитесь в тех. поддержку сайта
                        {{/if}}
                    </td>
                {{else}}
                    <td class="preview">
                        {{if thumbnail_url}}
                            <a href="${url}" target="_blank"><img src="${thumbnail_url}"></a>
                        {{/if}}
                    </td>
                    <td class="name">
                        <a href="${url}"{{if thumbnail_url}} target="_blank"{{/if}}>${name}</a>
                    </td>
                    <td class="size">${sizef}</td>
                    <td colspan="2"></td>
                {{/if}}
                <td class="delete">
                    <button data-type="${delete_type}" data-url="${delete_url}">Sil</button>
                </td>
            </tr>
        </script>
        <%--<%strUserAgent = UCase(CStr(Request.ServerVariables("HTTP_USER_AGENT")))
            if (InStr(strUserAgent, "OPERA") or InStr(strUserAgent, "MSIE")) then
            response.write("<small><font color=#777777>Bu tarayıcıda, dosyaları sürükle & bırak tekniğiyle yükleyemezsiniz.</font></small>")
            else
            response.write("<small><font color=#777777>Dosyalarınızı buraya sürükleyerek bile yükleme yapabilirsiniz.</font></small>")
            end if%>--%>

        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js"></script>
        <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.min.js"></script>
        <script src="http://ajax.aspnetcdn.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>
        <script src="/js/jquery.iframe-transport.js"></script>
        <script src="/js/jquery.fileupload.js"></script>
        <script src="/js/jquery.fileupload-ui.js"></script>
        <script src="/js/application.js"></script>
        <% } %>
    </form>

</asp:Content>
