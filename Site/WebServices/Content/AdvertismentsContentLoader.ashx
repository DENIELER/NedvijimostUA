<%@ WebHandler Language="C#" Class="AdvertismentsContentLoader" %>

using System;
using System.Web;
using System.Collections.Generic;
using Model;
using DTO = Nedvijimost.DataTransferObject;

public class AdvertismentsContentLoader : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        //--- common parameters
        int sectionId = context.Request["sectionId"] != null ? TryGetIntParameter(context.Request["sectionId"]) : 0;
        int? subSectionId = context.Request["subSectionId"] != null ? TryGetIntParameter(context.Request["subSectionId"]) : (int?)null;
        AdvertismentsState filter = context.Request["filter"] != null ? (AdvertismentsState)TryGetIntParameter(context.Request["filter"]) : AdvertismentsState.NotSubpurchase;
        int offset = context.Request["offset"] != null ? TryGetIntParameter(context.Request["offset"]) : 0;
        int limit = context.Request["limit"] != null ? TryGetIntParameter(context.Request["limit"]) : 1;
        string date = context.Request["date"];
        string url = context.Request["url"];
        
        //--- special filters
        bool showOnlyWithPhotos = context.Request["onlyPhoto"] != null ? Convert.ToBoolean(context.Request["onlyPhoto"]) : false;

        var requestParameters = new Nedvijimost.AdvertismentsRequest()
        {
            SectionId = sectionId,
            SubSectionId = subSectionId,
            State = filter,
            Offset = offset,
            Limit = limit,
            Url = url,

            Date = date,
            
            OnlyWithPhotos = showOnlyWithPhotos
        };
        
        Nedvijimost.DataTransferObject.Html.AdvertismentsList html = GetAdvertismentsHtml(requestParameters);
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(html));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private int TryGetIntParameter(string parameter)
    {
        int result = 0;
        int.TryParse(parameter, out result);
        return result;
    }
    
    private Nedvijimost.DataTransferObject.Html.AdvertismentsList GetAdvertismentsHtml(Nedvijimost.AdvertismentsRequest requestParameters)
    {
        var html = new Nedvijimost.DataTransferObject.Html.AdvertismentsList();

        AdvertismentsResponse view = GetAdvertisments(requestParameters);

        html.Header = PrepairHeaderHtml(view.FullCount, view.AdvCountToShow);
        html.Advertisments = PrepairAdvertismentsHtml(view.Advertisments);

        html.Pagging = PrepairPaggingHtml(view.AdvCountToShow, requestParameters.Offset, requestParameters.Limit, requestParameters.Url);

        return html;
    }

    private AdvertismentsResponse GetAdvertisments(Nedvijimost.AdvertismentsRequest requestParameters)
    {
        var context = new DataModel();

        //--- load advertisments
        var advertismentsWorkflow = new AdvertismentsWorkflow(context);
        AdvertismentsResponse view = advertismentsWorkflow.LoadAdvertisments(requestParameters);
        
        return view;
    }

    #region Prepair Advertisments Html for Ajax

    private string PrepairHeaderHtml(int fullAdvCount, int toShowAdvCount)
    {
        return "<pre>" +
                "Количество объявлений: " + toShowAdvCount + " из " + fullAdvCount +
               "</pre>";
    }

    private string PrepairPhotosHtml(List<DTO.Photo> photos)
    {
        if (photos.Count <= 0)
            return "";

        var block_height = "";
        if (photos.Count <= 3)
            block_height = " style=\"min-height: 80px;\"";

        var photosDiv = "<div class=\"row-fluid\"" + block_height + ">" +
                            "<div class=\"span8\">" +
                                "<strong>Фото:</strong>" +
                                "<div class=\"gallery\" data-toggle=\"modal-gallery\" data-target=\"#modal-gallery\" data-selector=\"div.gallery-item\">";

        int photo_index = 0;
        foreach (var photo in photos)
        {
            photosDiv += "<div class=\"gallery-item\" data-href=\"" + photo.filenameFormated + "\" data-index=\"" + photo_index + "\">" +
                            "<img src=\"" + photo.filename + "\"/>" +
                         "</div>";
            photo_index++;
        }
        photosDiv += "</div>" +
                            "</div>" +
                        "</div>";

        return photosDiv;
    }
    private string PrepairPhonesHtml(List<DTO.Phone> phones, int advOrderIndex)
    {
        var phonesDiv = "<ul class=\"unstyled phones\" id=\"phones_" + advOrderIndex + "\">";
        foreach (var phone in phones)
        {
            phonesDiv += "<li><strong>" + phone.phone + "</strong></li>";
        }
        phonesDiv += "</ul>";

        return phonesDiv;
    }
    private string PrepairAdvertismentsHtml(List<DTO.Advertisment> advertisments)
    {
        var advertismentDivs = "";

        bool isAuthorizedUserAsAdmin = Authorization.Authorization.CurrentUser_IsAdmin();

        for (var i = 0; i < advertisments.Count; i++)
        {
            DTO.Advertisment adv = advertisments[i];

            var advertismentBlockStyle = "";
            if (adv.isSpecial)
                advertismentBlockStyle = "special_advertisment";

            var phonesDiv = PrepairPhonesHtml(adv.Phones, i);
            var photosDiv = PrepairPhotosHtml(adv.Photos);

            var removeAdvertismentButton = "";
            if (isAuthorizedUserAsAdmin)
                removeAdvertismentButton = "<a class=\"btn btn-mini\" onclick=\"AdminRemoveAdvertisment(" + i + ", this, " + adv.Id + ");\"><i class=\"icon-trash\"></i> Удалить объявление</a>&nbsp;";

            var advertismentDiv =
                "<div style=\"margin-top: 10px;\" id=\"advertisment_" + i + "\">" +
                    "<div class=\"" + advertismentBlockStyle + " advertisment_block\">" +
                            "<div class=\"row-fluid\">" +
                                "<div class=\"row-fluid\">" +
                                    "<div class=\"span9\">" + adv.text + "</div>" +
                                    "<div class=\"span3\">" + phonesDiv + "</div>" +
                                "</div>" +
                                photosDiv +
                            "</div>" +
                            "<div class=\"clear:both\">&nbsp;</div>" +
                            "<div class=\"advertisment_info_footer row-fluid\">" +
                                "<div class=\"span5\">" +
                                    "<span>Дата обновления:" + adv.createDateFormated + "</span>&emsp;" +
                                "</div>" +
                                "<div class=\"span7\" style=\"text-align: right;\">" +
                                    removeAdvertismentButton +
                                    "<div class=\"btn-group\">" +
                                        "<button class=\"btn dropdown-toggle btn-mini\" data-toggle=\"dropdown\"><i class=\"icon-exclamation-sign\"></i> Сообщить <span class=\"caret\"></span></button>" +
                                            "<ul class=\"dropdown-menu\">" +
                                                "<li><a onclick=\"MarkAsSubpurchase(" + i + ", this);\">Объявление посредника</a></li>" +
                                                "<li><a onclick=\"MarkAsNotByTheme(" + i + ", this, " + adv.Id + ");\">Объявление не по теме</a></li>" +
                                            "</ul>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                    "</div>" +
                "</div>";
            advertismentDivs += advertismentDiv;
        };

        return advertismentDivs;
    }

    private string PrepairPaggingHtml(int toShowAdvCount, int offset, int limit, string currentUrl)
    {
        var paggingBlock = "";

        var advCount = toShowAdvCount;
        var currentPage = Math.Round((decimal)offset / (decimal)limit) + 1;
        var pageAdvCount = (decimal)limit;
        var lastPage = advCount % pageAdvCount == 0
                       ? Math.Floor(advCount / pageAdvCount)
                       : Math.Floor(advCount / pageAdvCount) + 1;

        if (pageAdvCount > advCount)
            return "";

        string previousDisable = "", nextDisable = "", prevPageUrl = "", nextPageUrl = "";
        var prevPage = currentPage - 1;
        var nextPage = currentPage + 1;

        if (currentPage <= 1)
        {
            previousDisable = " disabled";
            prevPageUrl = "#";
        }
        else
            prevPageUrl = "href=\"" + InsertURLParam(currentUrl, "page", prevPage.ToString()) + "\"";
        if (currentPage >= lastPage)
        {
            nextDisable = " disabled";
            nextPageUrl = "";
        }
        else nextPageUrl = "href=\"" + InsertURLParam(currentUrl, "page", nextPage.ToString()) + "\"";


        paggingBlock =
            "<div>" +
                "<ul class=\"pager\">" +
                    "<li class=\"previous" + previousDisable + "\">" +
                        "<a " + prevPageUrl + ">&larr; Previous</a>" +
                    "</li>" +
                    "<li class=\"next" + nextDisable + "\">" +
                        "<a " + nextPageUrl + ">Next &rarr;</a>" +
                    "</li>" +
                "</ul>" +
            "</div>";

        return paggingBlock;
    }

    private string InsertURLParam(string url, string key, string value)
    {
        var _url = url;
        if (_url.IndexOf(key + "=") >= 0)
        {
            var prefix = _url.Substring(0, _url.IndexOf(key));
            var suffixTemp = _url.Substring(_url.IndexOf(key));
            var suffix = suffixTemp.Substring(suffixTemp.IndexOf("=") + 1);
            suffix = (suffix.IndexOf("&") >= 0) ? suffix.Substring(suffix.IndexOf("&")) : "";
            _url = prefix + key + "=" + value + suffix;
        }
        else
        {
            if (_url.IndexOf("?") < 0)
                _url += "?" + key + "=" + value;
            else
                _url += "&" + key + "=" + value;
        }
        return _url;
    }

    #endregion Prepair Advertisments Html for Ajax

}