using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Model;

/// <summary>
/// Summary description for MarkAsSubpurchase
/// </summary>
[WebService]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SubpurchaseService : System.Web.Services.WebService {

    public SubpurchaseService ()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool MarkAsSubpurchase(List<string> phonesList)
    {
        try
        {   
            var subpurchasesWorkflow = new SubpurchasesWorkflow();
            foreach (var phone in phonesList)
            {
                subpurchasesWorkflow.AddSubpurchasePhone(phone, false);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

}
