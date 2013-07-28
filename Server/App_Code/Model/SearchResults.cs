using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

/// <summary>
/// Summary description for SearchResultsWorkflow
/// </summary>
public class SearchResults
{
    private Model.DataModel _dbcontext;
    private Log _log;

    private string _sectionCode { get; set; }
    
	public SearchResults(string sectionCode)
	{
        this._sectionCode = sectionCode;

        _dbcontext = new Model.DataModel();
	}
    public SearchResults(string sectionCode, Model.DataModel context)
    {
        this._sectionCode = sectionCode;
        this._dbcontext = context;
    }

    public void SetLog(Log log)
    {
        _log = log;
    }

    public Model.SearchResult AddSearchResult(int parsedAdvertismentsCount = 0)
    {
        var advertSection = (from section in _dbcontext.AdvertismentSections
                             where section.code == _sectionCode
                             select section).SingleOrDefault();

        if (advertSection == null)
            throw new Exception("Section name with code '" + _sectionCode + "' was not founded");

        var searchResults = from lastSearchResultFromDB in _dbcontext.SearchResults
                            where lastSearchResultFromDB.AdvertismentSection.code == _sectionCode
                            orderby lastSearchResultFromDB.createDate descending
                            select lastSearchResultFromDB;

        Model.SearchResult searchResult;
        Model.SearchResult lastSearchResult = null;
        if (searchResults.Any())
            lastSearchResult = searchResults.Take(1).SingleOrDefault();

        if (lastSearchResult != null
            && lastSearchResult.createDate.Date == Utils.GetUkranianDateTimeNow().Date)
        {
            lastSearchResult.modifyDate = Utils.GetUkranianDateTimeNow();
            lastSearchResult.allParsedAdvertismentsCount = parsedAdvertismentsCount;
            searchResult = lastSearchResult;
        }
        else
        {
            searchResult = new Model.SearchResult
            {
                createDate = Utils.GetUkranianDateTimeNow(),
                modifyDate = Utils.GetUkranianDateTimeNow(),
                AdvertismentSection = advertSection,
                allParsedAdvertismentsCount = parsedAdvertismentsCount
            };

            _dbcontext.SearchResults.InsertOnSubmit(searchResult);
        }
        _dbcontext.SubmitChanges();

        return searchResult;
    }
}