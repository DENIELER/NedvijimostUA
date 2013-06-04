using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RentSiteSettingsWorkflow
/// </summary>
public class SaleCommercialSiteSettingsWorkflow : SiteSettingsWorkflow
{
    const string MainSaleSection = "realestate_sale_commercial_sites";

    public SaleCommercialSiteSettingsWorkflow(string fileName)
        : base(fileName, MainSaleSection)
    {
    }

    public SaleCommercialSiteSettingsWorkflow(string fileName, int processorPart)
        : base(fileName, MainSaleSection + "_" + processorPart.ToString())
	{
	}

}