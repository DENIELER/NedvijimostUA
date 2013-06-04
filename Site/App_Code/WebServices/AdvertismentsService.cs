using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for AdvertismentsService
/// </summary>
[WebService]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AdvertismentsService : System.Web.Services.WebService {

    public AdvertismentsService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public bool MarkAsNotByTheme(int advertisment_id)
    {
        try
        {
            if (Authorization.IsAdmin(Authorization.GetVkontakteUserUid()))
            {
                var advertismentsWorkflow = new AdvertismentsWorkflow();
                advertismentsWorkflow.MarkNotByTheme(advertisment_id);
            }
            else
            {
                var advertismentsWorkflow = new AdvertismentsWorkflow();
                var text = advertismentsWorkflow.GetAdvertismentText(advertisment_id);

                var emailWorkflow = new Email();
                emailWorkflow.SendMail("danielostapenko@gmail.com", "Отмечено новое объявление",
                    string.Format(
@"Отмечено объявление меткой ""Не по теме"".
Текст объявления: {0},
Номер объявления: {1}
Линк подтверждения: {2}", 
                        text, 
                        advertisment_id,
                        "http://nedvijimost-ua.com/WebServices/AdvertismentsService.asmx/ForceMarkAsNotByTheme?password=gtycbz1990&advertisment_id=" + advertisment_id));
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public void ForceMarkAsNotByTheme(int advertisment_id, string password)
    {
        if (password == "gtycbz1990")
        {
            var advertismentsWorkflow = new AdvertismentsWorkflow();
            advertismentsWorkflow.MarkNotByTheme(advertisment_id);
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public bool HideAdvertisment(int advertisment_id)
    {
        try
        {
            if (Authorization.IsAdmin(Authorization.GetVkontakteUserUid()))
            {
                var advertismentsWorkflow = new AdvertismentsWorkflow();
                advertismentsWorkflow.HideAdvertisment(advertisment_id);
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public AdvertismentsView GetAdvertisments(int sectionId, int? subSectionId, AdvertismentsState filter, int offset, int limit, string date)
    {
        var context = new NedvijimostDBEntities();

        DateTime _date;
        if (!DateTime.TryParse(date, out _date))
            _date = DateTime.Now;

        //--- load advertisments
        var advertismentsWorkflow = new AdvertismentsWorkflow(context);
        AdvertismentsView view = null;
        if(string.IsNullOrEmpty(date))
            view = advertismentsWorkflow.LoadAdvertisments(sectionId, filter, subSectionId, offset, limit);
        else
            view = advertismentsWorkflow.LoadAdvertisments(sectionId, filter, _date, subSectionId, offset, limit);

        return view;
    }
}
