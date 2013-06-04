using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Payments_PaymentAnswer : System.Web.UI.Page
{
    private string sMrchPass2 = "gtycbz1990";

    protected void Page_Load(object sender, EventArgs e)
    {
        // HTTP parameters
        string sOutSum = GetPrm("OutSum");
        string sInvId = GetPrm("InvId");
        string sCrc = GetPrm("SignatureValue");
        string AdvertismentId = GetPrm("shpAdvertismentId");

        string sCrcBase = string.Format("{0}:{1}:{2}:shpAdvertismentId={3}",
                                         sOutSum, sInvId, sMrchPass2, AdvertismentId);

        // build own CRC
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));

        StringBuilder sbSignature = new StringBuilder();
        foreach (byte b in bSignature)
            sbSignature.AppendFormat("{0:x2}", b);

        string sMyCrc = sbSignature.ToString();

        if (sMyCrc.ToUpper() != sCrc.ToUpper())
        {
            Response.Write("bad sign");
            return;
        }

        Response.Write(string.Format("OK{0}", sInvId));
        // perform some action (change order state to paid)    
    }

    private string GetPrm(string sName)
    {
        string sValue;
        sValue = HttpContext.Current.Request.Form[sName] as string;

        if (string.IsNullOrEmpty(sValue))
            sValue = HttpContext.Current.Request.QueryString[sName] as string;

        if (string.IsNullOrEmpty(sValue))
            sValue = String.Empty;

        return sValue;
    }
}