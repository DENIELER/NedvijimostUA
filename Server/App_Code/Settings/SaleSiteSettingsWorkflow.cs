using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RentSiteSettingsWorkflow
/// </summary>
public class SaleSiteSettingsWorkflow : SiteSettingsWorkflow
{
    const string MainSaleSection = "realestate_sale_sites";

    public SaleSiteSettingsWorkflow(string fileName)
        : base(fileName, MainSaleSection)
    {
    }

    public SaleSiteSettingsWorkflow(string fileName, int processorPart)
        : base(fileName, MainSaleSection + "_" + processorPart.ToString())
	{
	}

}