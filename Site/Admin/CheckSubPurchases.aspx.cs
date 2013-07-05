using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CheckSubPurchases : System.Web.UI.Page
{
    public int SubPurchasesCount { get; set; }
    private Model.DataModel _dbcontext;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Authorization.Authorization.CurrentUser_IsAdmin())
            Response.Redirect("/", true);

        _dbcontext = new Model.DataModel();
    }

    protected void dlCheckSubPurchases_EditCommand(object source, DataListCommandEventArgs e)
    {
        dlCheckSubPurchases.EditItemIndex = e.Item.ItemIndex;
        dlCheckSubPurchases.DataBind();
    }
    protected void dlCheckSubPurchases_UpdateCommand(object source, DataListCommandEventArgs e)
    {
        CheckBox chkChecked;
        chkChecked = (CheckBox)(e.Item.FindControl("chkCheckedSubPurchase"));
        bool checkedState = chkChecked.Checked;

        Label subpurchaseIdLabel;
        subpurchaseIdLabel = (Label)(e.Item.FindControl("subpurchaseId"));

        //--- save into DB
        var editItemKey = Guid.Parse(subpurchaseIdLabel.Text);
        var subPurchase = (from subPurch in _dbcontext.SubPurchases
                                where subPurch.id == editItemKey
                                select subPurch).SingleOrDefault();

        _dbcontext.SubPurchases.Attach(subPurchase);
        subPurchase.not_checked = checkedState;

        _dbcontext.SubmitChanges();
        //--- end save into DB

        if (!checkedState)
        {
            if (subPurchase != null)
            {
                //--- remove Advertisments with current SubPurchase
                var subPurchaseAdvertisments = from advertisment in _dbcontext.Advertisments
                                               join advertismentPhone in _dbcontext.AdvertismentPhones on advertisment.Id equals advertismentPhone.AdvertismentId
                                               join subpruchasePhone in _dbcontext.SubPurchasePhones on advertismentPhone.phone equals subpruchasePhone.phone
                                               where subpruchasePhone.SubPurchaseId == editItemKey
                                               select advertisment;
                                                
                if (subPurchaseAdvertisments.Count() > 0)
                {
                    foreach (var adv in subPurchaseAdvertisments)
                    {
                        adv.not_show_advertisment = true;
                    }
                    _dbcontext.SubmitChanges();
                }
            }
        }

        dlCheckSubPurchases.EditItemIndex = -1;
        dlCheckSubPurchases.DataBind();
    }
    protected void dlCheckSubPurchases_CancelCommand(object source, DataListCommandEventArgs e)
    {
        dlCheckSubPurchases.EditItemIndex = -1;
        dlCheckSubPurchases.DataBind();
    }
    protected void dlCheckSubPurchases_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        //--- save into DB
        var subpurchasesContext = new Model.DataModel();

        Label subpurchaseIdLabel;
        subpurchaseIdLabel = (Label)(e.Item.FindControl("subpurchaseId"));

        var editItemKey = Guid.Parse(subpurchaseIdLabel.Text);
        var subPurchase = new Model.SubPurchase()
        {
            id = editItemKey
        };
        subpurchasesContext.SubPurchases.Attach(subPurchase);

        subpurchasesContext.SubPurchases.DeleteOnSubmit(subPurchase);
        subpurchasesContext.SubmitChanges();
        //--- end save into DB

        dlCheckSubPurchases.EditItemIndex = -1;
        dlCheckSubPurchases.DataBind();
    }
    
    protected void ldsSubPurchases_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
        var result = from subPurchaseObject in _dbcontext.SubPurchases
                     join subPurchasePhoneObject in _dbcontext.SubPurchasePhones
                        on subPurchaseObject.id equals subPurchasePhoneObject.SubPurchaseId
                     where subPurchaseObject.not_checked.HasValue && subPurchaseObject.not_checked.Value
                     orderby subPurchaseObject.createDate descending
                     select new { SubPurchase = subPurchaseObject, SubPurchasePhone = subPurchasePhoneObject };

        e.Result = result;
    }

    protected void btnAdvertisment_Command(object sender, CommandEventArgs e)
    {
        string phone = (string)e.CommandArgument;

        string result = (from adv in _dbcontext.Advertisments
                     join advPhone in _dbcontext.AdvertismentPhones
                        on adv.Id equals advPhone.AdvertismentId
                     where advPhone.phone == phone
                     orderby adv.createDate descending
                     select adv.text).FirstOrDefault();

        lAdvText.Text = result;
    }
}