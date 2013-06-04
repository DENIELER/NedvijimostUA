using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DTO = Nedvijimost.DataTransferObject;

/// <summary>
/// Summary description for SaleAdvertismentsList
/// </summary>
public class SaleAdvertismentsList
{
    public SaleAdvertismentsList()
    {
    }

    public List<DTO.Advertisment> SaleAdvertisments { get; set; }
    public List<DTO.Advertisment> RentAdvertisments { get; set; }
}