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
    private Model.NedvijimostDBEntities _dbcontext;

    public string SectionCode { get; set; }
    
	public SearchResultsWorkflow(string sectionCode)
	{
        this.SectionCode = sectionCode;

        _dbcontext = new Model.NedvijimostDBEntities();
	}

    public SearchResultsWorkflow(Model.NedvijimostDBEntities context)
    {
        _dbcontext = context;
    }

    public SearchResultsWorkflow(string sectionCode, Model.NedvijimostDBEntities context)
    {
        this.SectionCode = sectionCode;

        _dbcontext = context;
    }

    private Log _log;
    public SearchResultsWorkflow(string sectionCode, Model.NedvijimostDBEntities context, Log log)
    {
        this.SectionCode = sectionCode;

        _dbcontext = context;
        _log = log;
    }

    public Model.SearchResult AddSearchResult(int parsedAdvertismentsCount = 0)
    {
        var advertSection = (from section in _dbcontext.AdvertismentSections
                             where section.code == SectionCode
                             select section).SingleOrDefault();

        if (advertSection == null)
            throw new Exception("Section name with code '" + SectionCode + "' was not founded");

        var searchResults = from lastSearchResultFromDB in _dbcontext.SearchResults
                            where lastSearchResultFromDB.AdvertismentSection.code == SectionCode
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

            _dbcontext.AddToSearchResults(searchResult);
        }
        _dbcontext.SaveChanges();

        return searchResult;
    }

    public void SaveAdvertismentsInSearchResult(Model.SearchResult searchResult, IList<Advertisment> advertisments)
    {
        //var advertismentsFromDB = (from adv in _dbcontext.Advertisments
        //                           select adv).ToList();
        
        foreach (var advertisment in advertisments)
        {
            try
            {
                //_log.WriteLog("Search advertisment");
                //var existsAdvertisment = (from adv in advertismentsFromDB
                //                         where adv.searchresult_id == searchResult.Id
                //                         && adv.text == advertisment.text
                //                         select adv).ToList();

                var existsAdvertisment = (from adv in _dbcontext.Advertisments
                                          where adv.searchresult_id == searchResult.Id
                                          && adv.text == advertisment.text
                                          select adv).ToList();

                if (existsAdvertisment.Count <= 0)
                {
                    //_log.WriteLog("Add advertisment:");
                    //_log.WriteLog(advertisment.text);
                    //_log.WriteLog(advertisment.link);
                    //_log.WriteLog(advertisment.siteName);
                    //_log.WriteLog(Utils.GetUkranianDateTimeNow().ToString());

                    var advertismentEntity = new Model.Advertisment
                    {
                        createDate = Utils.GetUkranianDateTimeNow(),
                        modifyDate = Utils.GetUkranianDateTimeNow(),
                        text = advertisment.text,
                        AdvertismentSection = searchResult.AdvertismentSection,
                        SearchResult = searchResult,
                        link = advertisment.link,
                        siteName = advertisment.siteName,
                        subpurchaseAdvertisment = true
                    };
                    _dbcontext.AddToAdvertisments(advertismentEntity);
                    _dbcontext.SaveChanges();

                    //--- add phones
                    foreach (var phone in advertisment.AdvertismentPhones)
                    {
                        if (!string.IsNullOrWhiteSpace(phone.phone))
                        {
                            //_log.WriteLog(phone.phone);
                            var advertismentPhoneEntity = new Model.AdvertismentPhone
                            {
                                phone = phone.phone,
                                Advertisment = advertismentEntity
                            };
                            _dbcontext.AddToAdvertismentPhones(advertismentPhoneEntity);
                            _dbcontext.SaveChanges();

                            advertismentEntity.AdvertismentPhones.Add(advertismentPhoneEntity);
                        }
                    }
                    //----

                    //--- add photos
                    foreach (var photo in advertisment.AdvertismentsPhotos)
                    {
                        if (!string.IsNullOrWhiteSpace(photo.filename))
                        {
                            //_log.WriteLog(photo.filename);
                            var advertismentPhotoEntity = new Model.AdvertismentsPhoto
                            {
                                filename = photo.filename,
                                createDate = Utils.GetUkranianDateTimeNow(),
                                Advertisment = advertismentEntity
                            };
                            _dbcontext.AddToAdvertismentsPhotoes(advertismentPhotoEntity);
                            _dbcontext.SaveChanges();

                            advertismentEntity.AdvertismentsPhotos.Add(advertismentPhotoEntity);
                        }
                    }
                    //---

                    //_log.WriteLog("End");

                    searchResult.Advertisments.Add(advertismentEntity);
                    _dbcontext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _log.WriteLog("Save advertisment error. Adv text: " + advertisment.text + Environment.NewLine
                    + "link: " + advertisment.link + Environment.NewLine
                    + "site: " + advertisment.siteName + Environment.NewLine
                    + "Error: " + e.Message + ". Trace:" + e.StackTrace);
                foreach (var phone in advertisment.AdvertismentPhones)
                    _log.WriteLog("Phone: " + phone.phone + Environment.NewLine);
                
                if (e.InnerException != null)
                    _log.WriteLog("Inner exception: " + e.InnerException.Message);
            }
        }
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