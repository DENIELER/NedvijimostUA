using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

/// <summary>
/// Summary description for JobHost
/// </summary>
public class JobHost : IRegisteredObject
{
    private readonly object _lock = new object();
    private bool _shuttingDown;
    private Log _log;

    public JobHost()
    {
        HostingEnvironment.RegisterObject(this);
    }

    public JobHost(Log log)
    {
        _log = log;
        HostingEnvironment.RegisterObject(this);
    }

    public void Stop(bool immediate)
    {
        if(_log != null)
            _log.WriteLog("Fast closing!!! Immediate: " + immediate.ToString());

        lock (_lock)
        {
            _shuttingDown = true;
        }
        HostingEnvironment.UnregisterObject(this);
    }

    public void DoWork(Action work)
    {
        lock (_lock)
        {
            if (_shuttingDown)
            {
                return;
            }
            work();
        }
    }
}