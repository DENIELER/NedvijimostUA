using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UndergroundStationDetectionWorkflow : BaseContextWorkflow
{
    private List<UndergroundStation> stations;

	public UndergroundStationDetectionWorkflow()
	{
		
	}

    public void GetUndergroundStations(ref List<Server.Entities.Advertisment> advertisments)
    {
        foreach (var advertisment in advertisments)
        {
            advertisment.UndergroundStationID = GetUndergroundStationID(advertisment.Text);
        }
    }

    public int? GetUndergroundStationID(string text)
    {
        if(stations == null)
            using(DataModel dataModel = new DataModel())
            {
                stations = dataModel.UndergroundStations.ToList();
            }

        foreach (UndergroundStation station in stations)
        {
            string[] filterStationNames = station.Filter.Split('|');
            foreach (string filterStationName in filterStationNames)
            {
                if (IsUndergroundStation(text, filterStationName))
                    return station.StationID;
            }
        }
        
        return null;
    }

    private bool IsUndergroundStation(string advertismentText, string undergroundStation)
    {
        if (string.IsNullOrEmpty(advertismentText) || string.IsNullOrEmpty(undergroundStation))
            return false;

        var jaroWinklerAlgorithm = new SimMetricsMetricUtilities.JaroWinkler();
        int startIndex = 0, index = 0;
        do
        {
            index = advertismentText.IndexOf(undergroundStation[0], startIndex);
            if (index != -1)
            {
                string checkSubString = 
                    index + undergroundStation.Length < advertismentText.Length 
                    ? advertismentText.Substring(index, undergroundStation.Length)
                    : advertismentText.Substring(index);
                double rate = jaroWinklerAlgorithm.GetSimilarity(checkSubString, undergroundStation);
                if (rate > 0.84)
                    return true;

                startIndex = index + 1;
            }

        } while (index != -1);

        return false;
    }
}