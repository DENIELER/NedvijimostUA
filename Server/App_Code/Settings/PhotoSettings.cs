using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for PhotoSettings
/// </summary>
public class PhotoSettings
{
    public string FileName = Resources.Constants.SettingsFile;

	public PhotoSettings()
	{
		
	}

    public List<string> getPhotoUrlsToRemove()
    {
        if (string.IsNullOrEmpty(FileName))
        {
            throw new Exception("Not right configured Photo Settings class.");
        }

        List<string> removePhotosUrls = new List<string>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath(FileName));

        XmlNodeList photoUrlPaths = xmlDocument.DocumentElement.SelectSingleNode("remove_images").SelectNodes("remove_image");
        foreach (XmlNode urlPath in photoUrlPaths)
            removePhotosUrls.Add(urlPath.InnerText);

        return removePhotosUrls;
    }
}
