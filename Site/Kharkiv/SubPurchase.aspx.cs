using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class Kharkiv_SubPurchase : System.Web.UI.Page
{
    public string SubPurchasePhone { get; set; }
    public string SubPurchaseFullName { get; set; }
    public string SubPurchaseCreateDate { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SubPurchasePhone = Request["phone"];

        var subpurchasesContext = new NedvijimostDBEntities();
        var currentSubPurchase = (from subPurchase in subpurchasesContext.SubPurchases
                                  join subPurchasePhone in subpurchasesContext.SubPurchasePhones 
                                    on subPurchase.Id equals subPurchasePhone.SubPurchaseId
                                  where subPurchasePhone.phone == SubPurchasePhone
                           select subPurchase).FirstOrDefault();

        if (currentSubPurchase != null)
        {
            SubPurchaseFullName = currentSubPurchase.name + " " + currentSubPurchase.surname;
            SubPurchaseCreateDate = currentSubPurchase.createDate.ToShortDateString();
        }
    }
}