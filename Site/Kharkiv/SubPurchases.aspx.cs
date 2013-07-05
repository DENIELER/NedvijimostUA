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

public partial class Kharkiv_SubPurchases : System.Web.UI.Page
{
    public int SubPurchasesCount { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    
    protected void dpPhoneNumbers_PreRender(object sender, EventArgs e)
    {
        LoadSubPuchasesPhones();
    }

    private void LoadSubPuchasesPhones()
    {
        var subpurchasesContext = new DataModel();
        var subPurchases = from subPurchase in subpurchasesContext.SubPurchases
                           join subPurchasePhone in subpurchasesContext.SubPurchasePhones
                                    on subPurchase.id equals subPurchasePhone.SubPurchaseId
                           where !subPurchase.not_checked.HasValue || !subPurchase.not_checked.Value
                           orderby subPurchasePhone.phone
                           select new { subpurchase = subPurchase, phone = subPurchasePhone };

        SubPurchasesCount = subPurchases.Count();

        lvPhoneNumbers.DataSource = subPurchases;
        lvPhoneNumbers.DataBind();
    }
}