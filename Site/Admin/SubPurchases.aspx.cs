using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class Admin_SubPurchases : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Authorization.Authorization.CurrentUser_IsAdmin())
            Response.Redirect("/", true);
    }

    protected void AddNewSubPurchase(object sender, EventArgs e)
    {
        string[] subpurchasePhoneNumbers = inputSubPurchPhone.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        string subpurchaseName = inputSubPurchName.Value;

        if (!subpurchasePhoneNumbers.Any())
        {
            throw new Exception("Добавьте номер посредника, который хотите разместить в базе.");
        }

        var context = new DataModel();
        var subpurchasesWorkflow = new SubpurchasesWorkflow();
        foreach(var subpurchasePhoneNumber in subpurchasePhoneNumbers)
        {
            if (!string.IsNullOrEmpty(subpurchasePhoneNumber))
            {
                subpurchasesWorkflow.AddSubpurchasePhone(subpurchasePhoneNumber, subpurchaseName, string.Empty, true);
                
                //--- remove Advertisments with current SubPurchase
                var subPurchaseAdvertisments = from advertisment in context.Advertisments
                                               where advertisment.AdvertismentPhones.Any(advPhone => advPhone.phone == subpurchasePhoneNumber)
                                               select advertisment;
                if (subPurchaseAdvertisments.Count() > 0)
                {
                    foreach (var adv in subPurchaseAdvertisments)
                    {
                        context.Advertisments.DeleteOnSubmit(adv);
                    }
                    context.SubmitChanges();
                }
            }
            Response.Redirect(Request.RawUrl + "?success=1");
        }
        
    }
}