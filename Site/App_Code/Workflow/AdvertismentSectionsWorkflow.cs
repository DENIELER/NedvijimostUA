﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

/// <summary>
/// Summary description for AdvertismentSectionsWorkflow
/// </summary>
public class AdvertismentSectionsWorkflow
{
	public AdvertismentSectionsWorkflow(Model.NedvijimostDBEntities context)
	{
        Context = context;
	}

    public Model.NedvijimostDBEntities Context { get; set; }

    public List<AdvertismentSection> LoadSections()
    {
        var sections = (from section in Context.AdvertismentSections
                                select section).ToList();
        
        return sections;
    }
}