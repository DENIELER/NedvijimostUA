using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CottagesSiteSettingsWorkflow 
/// </summary>
public class RentSiteSettingsWorkflow : SiteSettingsWorkflow
{
    const string MainRentSection = "real_estate_rent_sites";

	public RentSiteSettingsWorkflow (string fileName)
        : base(fileName, MainRentSection)
	{
	}

    public RentSiteSettingsWorkflow(string fileName, int processorPart)
        : base(fileName, MainRentSection + "_" + processorPart.ToString())
	{
	}
}