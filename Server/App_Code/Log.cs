using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Loggin information
/// </summary>
public class Log
{
    private string _serviceCode;
    private string _sectionCode;
    private int? _sectionId;

    private DataModel _dataModel;

    public Log(string serviceCode, string sectionCode, int sectionId)
    {
        _serviceCode = serviceCode;
        _sectionCode = sectionCode;
        _sectionId = sectionId;
    }

    public Log(string serviceCode, string sectionCode)
    {
        _serviceCode = serviceCode;
        _sectionCode = sectionCode;

        if (_dataModel == null)
            _dataModel = new DataModel();

        var section = _dataModel.AdvertismentSections
                .SingleOrDefault(s => s.code == _sectionCode);
        if (section == null)
            throw new Exception("Can not found section by sectionCode");

        _sectionId = section.Id;
    }

    public Log(string serviceCode)
    {
        _serviceCode = serviceCode;
    }

    public void WriteLog(string message)
    {
        if (_dataModel == null)
            _dataModel = new DataModel();

        var serverLogMessage = new ServerLog()
        {
            Id = Guid.NewGuid(),
            createDate = Utils.GetUkranianDateTimeNow(),
            message = message,
            sectionCode = _sectionCode,
            sectionId = _sectionId,
            serviceCode = _serviceCode
        };

        _dataModel.ServerLogs.InsertOnSubmit(serverLogMessage);
        _dataModel.SubmitChanges();
    }
}