using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Payments_PaymentResult : System.Web.UI.Page
{
    private string sMrchPass1 = "danielostapenko1990";
    private string sMrchPass2 = "gtycbz1990";

    protected bool ResultSuccess = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.RouteData.Values["payment_result"] != null)
        {
            if (Page.RouteData.Values["payment_result"].ToString() == "success")
            {
                ResultSuccess = true;

                var result = DetectSuccessPayment(sMrchPass1);
                ResultSuccess = result.Item1;
                List<int> advertisment_Ids = result.Item2;

                // after check payment signature
                if (ResultSuccess)
                {
                    var advertismentsWorkflow = new AdvertismentsWorkflow();
                    foreach (var advertisment_Id in advertisment_Ids)
                    {
                        advertismentsWorkflow.MarkIsSpecial(advertisment_Id);
                    }
                }
            }
            else
                ResultSuccess = false;
        }
    }

    private Tuple<bool, List<int>> DetectSuccessPayment(string password)
    {
        // HTTP parameters
        string sOutSum = GetPrm("OutSum");
        string sInvId = GetPrm("InvId");
        string sCrc = GetPrm("SignatureValue");
        string AdvertismentIdParameter = GetPrm("shpAdvertismentId");

        string sCrcBase = string.Format("{0}:{1}:{2}:shpAdvertismentId={3}",
                                         sOutSum, sInvId, password, AdvertismentIdParameter);

        // build own CRC
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));

        StringBuilder sbSignature = new StringBuilder();
        foreach (byte b in bSignature)
            sbSignature.AppendFormat("{0:x2}", b);

        string sMyCrc = sbSignature.ToString();

        if (sMyCrc.ToUpper() != sCrc.ToUpper())
        {
            return new Tuple<bool, List<int>>(false, null);
        }

        List<int> advertismentIds = GetAdvertismentIdsFromQuery(AdvertismentIdParameter);

        return new Tuple<bool, List<int>>(true, advertismentIds);
    }

    private List<int> GetAdvertismentIdsFromQuery(string AdvertismentIdParameter)
    {
        string[] advIds = AdvertismentIdParameter.Split('|');
        var resultIds = new List<int>();

        foreach (string advId in advIds)
        {
            try
            {
                resultIds.Add(Convert.ToInt32(advId));
            }
            catch (Exception e)
            {
                Log l = new Log("CommonErrors.log");
                l.WriteLog("Convert To Int wrong in payment processing. Message:" + e.Message);
                l.WriteLog("AdvertismentID:" + advId);
            }
        }

        return resultIds;
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