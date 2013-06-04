using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

using DTO = Nedvijimost.DataTransferObject;

/// <summary>
/// Summary description for RentAdvertismentsList
/// </summary>
public class RentAdvertismentsList
{
	public RentAdvertismentsList()
	{
	}

    public List<DTO.Advertisment> RentAdvertisments { get; set; }
    public List<DTO.Advertisment> TakeOffAdvertisments { get; set; }
}