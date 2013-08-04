using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

public class GoogleMaps
{
    private const string geocodeUrl = @"http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false";

	public GoogleMaps()
	{
	}

    #region Methods
    public bool IsCorrectAddress(string cityName, string address)
    {
        Random r = new Random();
        var randomWait = r.Next(500);
        System.Threading.Thread.Sleep(randomWait);

        return IsCorrectAddressRequest(cityName, address);
    }
    #endregion Methods

    #region Private Methods
    private bool IsCorrectAddressRequest(string cityName, string address)
    {
        var client = new WebClient();
        client.Headers.Add("User-Agent", "Nobody");

        string addressGeocodeUrl = FormatUrl(cityName, address);
        var response = client.DownloadString(new Uri(addressGeocodeUrl));

        JObject googleParseResults = JObject.Parse(response);
        if((string)googleParseResults["status"] != "OK")
            return false;

        JArray results = (JArray)googleParseResults["results"];
        foreach (JToken result in results)
        {
            //--- if google return only partial match
            //--- then not detect it as good result
            if (result["partial_match"] == null)
                return true;
        }

        return false;
    }

    private string FormatUrl(string cityName, string address)
    {
        return string.Format(
                    geocodeUrl,
                    "г." + cityName + " " + address
                );
    }
    #endregion Private Methods
}
