using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

using DTO = Nedvijimost.DataTransferObject;

[DataContract]
public class AdvertismentsResponse
{
    public AdvertismentsResponse(List<DTO.Advertisment> advertisments, int fullCount, int advertismentsToShowCount, DateTime date)
    {
        _advertisments = advertisments;
        _fullCount = fullCount;
        _advCountToShow = advertismentsToShowCount;
        _date = date;
    }

    public AdvertismentsResponse(IEnumerable<Model.Advertisment> advertisments, int fullCount, int advertismentsToShowCount, DateTime date)
    {
        _advertisments = advertisments.Select(x => new DTO.Advertisment(x)).ToList();
        _fullCount = fullCount;
        _advCountToShow = advertismentsToShowCount;
        _date = date;
    }

    private List<DTO.Advertisment> _advertisments;
    private int _fullCount;
    private int _advCountToShow;
    private DateTime _date;

    [DataMember]
    public List<DTO.Advertisment> Advertisments 
    {
        get { return _advertisments; }
    }
    [DataMember]
    public int FullCount
    {
        get { return _fullCount; }
    }
    [DataMember]
    public int AdvCountToShow
    {
        get { return _advCountToShow; }
    }
    [DataMember]
    public DateTime Date
    {
        get { return _date; }
    }

    public void SetAdvertismets(List<DTO.Advertisment> list)
    {
        _advertisments = list;
    }
}