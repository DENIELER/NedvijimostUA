using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class BaseContextWorkflow : BaseWorkflow
{
    protected Model.DataModel context { get; private set; }
    
	public BaseContextWorkflow()
	{
        context = new Model.DataModel();
	}

    public BaseContextWorkflow(Model.DataModel context)
    {
        this.context = context;
    }
}