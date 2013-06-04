using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RentSiteSettingsWorkflow
/// </summary>
public class CottagesSiteSettingsWorkflow : SiteSettingsWorkflow
{
    const string MainCottagesSection = "cottages_houses";

	public CottagesSiteSettingsWorkflow(string fileName)
        : base(fileName, MainCottagesSection)
	{
	}

    public CottagesSiteSettingsWorkflow(string fileName, int processorPart)
        : base(fileName, MainCottagesSection + "_" + processorPart.ToString())
	{
	}
}