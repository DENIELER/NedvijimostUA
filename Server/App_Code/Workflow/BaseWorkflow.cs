using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BaseWorkflow
/// </summary>
public class BaseWorkflow
{
    private Log log;

	public BaseWorkflow()
	{
		
	}

    public void SetLog(Log log)
    {
        this.log = log;
    }
    protected void WriteLog(string message)
    {
        if (log != null)
            log.WriteLog(message);
    }
}