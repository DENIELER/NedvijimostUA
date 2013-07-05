using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using Model;

/// <summary>
/// Summary description for SearchResultsWorkflow
/// </summary>
public class SearchResultsWorkflow
{
    private Model.DataModel _dbcontext;

    public string SectionCode { get; set; }
    
	public SearchResultsWorkflow(string sectionCode)
	{
        this.SectionCode = sectionCode;

        _dbcontext = new Model.DataModel();
	}

    public SearchResultsWorkflow(Model.DataModel context)
    {
        _dbcontext = context;
    }

    public SearchResultsWorkflow(string sectionCode, Model.DataModel context)
    {
        this.SectionCode = sectionCode;

        _dbcontext = context;
    }

    private Log _log;
    public SearchResultsWorkflow(string sectionCode, Model.DataModel context, Log log)
    {
        this.SectionCode = sectionCode;

        _dbcontext = context;
        _log = log;
    }

    public DateTime? GetDateFromRequest(string requestParameter)
    {
        string dateString;
        DateTime resultDate;

        if (!string.IsNullOrWhiteSpace(requestParameter))
            dateString = requestParameter.Replace('_', '.');
        else
            dateString = Utils.GetUkranianDateTimeNow().ToString("dd.MM.yyyy");

        var culture = System.Globalization.CultureInfo.CreateSpecificCulture("uk-UA");
        var styles = System.Globalization.DateTimeStyles.None;

        if (DateTime.TryParse(dateString, culture, styles, out resultDate))
            return resultDate;
        else return null;
    }
    public List<DateTime> GetSearchResultsDates(int sectionId)
    {
        var allExistingDates = from searchResult in _dbcontext.SearchResults
                               where searchResult.AdvertismentSection.Id == sectionId
                               orderby searchResult.createDate descending
                               select searchResult.createDate;

        //--- all dates that we have search results
        var existingDateList = (from existingDate in allExistingDates
                                select existingDate).ToList();

        var scrollDatesList = new List<DateTime>();
        foreach (var existingDate in existingDateList)
            scrollDatesList.Add(existingDate);

        return scrollDatesList;
    }
}