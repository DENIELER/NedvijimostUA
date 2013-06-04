using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Payments_MakeAdvertismentSpecial : System.Web.UI.Page
{
    private List<int> specialAdvertisments = new List<int>();
    private string SessionSpecialAdvertismentsKey = "SpecialAdvertisments";

    // your registration data
    string sMrchLogin = "DENIELER";
    string sMrchPass1 = "danielostapenko1990";
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Session[SessionSpecialAdvertismentsKey] != null)
            {
                specialAdvertisments = (List<int>)Session[SessionSpecialAdvertismentsKey];
                MakePayLinkForRobokassa(specialAdvertisments);
            }
            else
            {
                MakePayLinkForRobokassa(null);
            }
        }
        else
        {
            MakePayLinkForRobokassa(null);
            if (Session[SessionSpecialAdvertismentsKey] != null)
                Session.Remove(SessionSpecialAdvertismentsKey);
        }
    }

    private void MakePayLinkForRobokassa(List<int> advertismentsList)
    {
        if (advertismentsList == null
            || advertismentsList.Count() == 0)
        {
            lnkPay.HRef= string.Empty;
            return;
        }

        // order properties
        decimal nOutSum = 5M * advertismentsList.Count();
        int nInvId = 0;
        string sDesc = "Оплата заказа на выделение объявления сроком на 1 неделю - НедвижимостьUA";
        string sAdvertismentIdParamName = "shpAdvertismentId";
        //--- set advertisments Index parameters
        string sAdvertismentIds = "";
        foreach (var adv in advertismentsList)
            sAdvertismentIds += 
                (string.IsNullOrWhiteSpace(sAdvertismentIds) 
                ? ""
                : "|") + adv;

        string sOutSum = nOutSum.ToString("0.00", CultureInfo.InvariantCulture);
        string sCrcBase = string.Format("{0}:{1}:{2}:{3}:{4}={5}",
                                         sMrchLogin, sOutSum, nInvId, sMrchPass1, sAdvertismentIdParamName, sAdvertismentIds);
        // build CRC value
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));

        StringBuilder sbSignature = new StringBuilder();
        foreach (byte b in bSignature)
            sbSignature.AppendFormat("{0:x2}", b);

        string sCrc = sbSignature.ToString();

        // build URL
        lnkPay.HRef = //"https://merchant.roboxchange.com/Index.aspx?" +
            "http://test.robokassa.ru/Index.aspx?" +
                                            "MrchLogin=" + sMrchLogin +
                                            "&OutSum=" + sOutSum +
                                            "&InvId=" + nInvId +
                                            "&Desc=" + sDesc +
                                            "&SignatureValue=" + sCrc +
                                            "&" + sAdvertismentIdParamName + "=" + sAdvertismentIds;
    }

    protected void bFindByPhoneNumber_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(tPhoneNumber.Text))
        {
            var advertismentsWorkflow = new AdvertismentsWorkflow();
            dlAdvertismentsList.DataSource = advertismentsWorkflow.GetAdvertismentsByPhone(tPhoneNumber.Text);
            dlAdvertismentsList.DataBind();

            if (Session[SessionSpecialAdvertismentsKey] != null)
            {
                Session.Remove(SessionSpecialAdvertismentsKey);
                MakePayLinkForRobokassa(null);
            }
        }
    }

    protected void chkAdvertisment_CheckedChanged(object sender, EventArgs e)
    {
        foreach (DataListItem item in dlAdvertismentsList.Items)
        {
            if (item.ItemType == ListItemType.Item
                || item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox checkBox = item.FindControl("chkAdvertisment") as CheckBox;
                if (checkBox != null && checkBox == sender as CheckBox)
                {
                    int advertismentID = (int)dlAdvertismentsList.DataKeys[item.ItemIndex];

                    if (checkBox.Checked && !specialAdvertisments.Contains(advertismentID))
                    { specialAdvertisments.Add(advertismentID); }
                    else if(!checkBox.Checked && specialAdvertisments.Contains(advertismentID))
                    { specialAdvertisments.Remove(advertismentID); }

                    Session[SessionSpecialAdvertismentsKey] = specialAdvertisments;
                    MakePayLinkForRobokassa(specialAdvertisments);
                }
            }
        }

        UpdateScriptManagerPostBackControls();
    }
    protected void dlAdvertismentsList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item
            || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var checkBox = e.Item.FindControl("chkAdvertisment") as CheckBox;
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(checkBox);
        }
    }
    private void UpdateScriptManagerPostBackControls()
    {
        foreach (DataListItem item in dlAdvertismentsList.Items)
        {
            if (item.ItemType == ListItemType.Item
                || item.ItemType == ListItemType.AlternatingItem)
            {
                var checkBox = item.FindControl("chkAdvertisment") as CheckBox;
                ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(checkBox);
            }
        }
    }
}