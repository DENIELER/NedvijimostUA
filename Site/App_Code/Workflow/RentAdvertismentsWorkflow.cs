using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

using DTO = Nedvijimost.DataTransferObject;

/// <summary>
/// Summary description for RentAdvertismentsWorkflow
/// </summary>
public class RentAdvertismentsWorkflow : AdvertismentsWorkflow
{
	public RentAdvertismentsWorkflow()
	{
		
	}

    public RentAdvertismentsList SeparateTakeOffAndRentAdvertisments(List<DTO.Advertisment> adversitments)
    {
        var rentAdvList = new RentAdvertismentsList();

        if (adversitments != null)
        {
            var rentAdvertismentWordOptions = Settings.getRentOptions();
            if (rentAdvertismentWordOptions != null)
            {
                //--- TAKE OFF 
                if (rentAdvertismentWordOptions.TakeOffWords != null && rentAdvertismentWordOptions.TakeOffWords.Count > 0)
                {
                    rentAdvList.TakeOffAdvertisments = new List<DTO.Advertisment>();
                    foreach (var adversitment in adversitments)
                    {
                        foreach (var takeOffWord in rentAdvertismentWordOptions.TakeOffWords)
                        {
                            if (adversitment.text.ToLower().Contains(takeOffWord))
                            {
                                rentAdvList.TakeOffAdvertisments.Add(adversitment);
                                break;
                            }
                        }
                    }
                }

                //--- put all others that not contains in TakeOff
                rentAdvList.RentAdvertisments = new List<DTO.Advertisment>();
                if(rentAdvList.TakeOffAdvertisments != null)
                {
                    foreach (var adversitment in adversitments)
                    {
                        if (!rentAdvList.TakeOffAdvertisments.Contains(adversitment)) 
                            rentAdvList.RentAdvertisments.Add(adversitment);
                    }
                }else
                {
                    foreach (var adversitment in adversitments)
                        rentAdvList.RentAdvertisments.Add(adversitment);
                }
            }
        }

        return rentAdvList;
    }

    public SaleAdvertismentsList SeparateSaleAndRentAdvertisments(List<DTO.Advertisment> adversitments)
    {
        var rentAdvList = new SaleAdvertismentsList();

        if (adversitments != null)
        {
            var rentAdvertismentWordOptions = Settings.getRentOptions();
            if (rentAdvertismentWordOptions != null)
            {
                //--- TAKE OFF 
                if (rentAdvertismentWordOptions.TakeOffWords != null
                    && rentAdvertismentWordOptions.TakeOffWords.Count > 0
                    && rentAdvertismentWordOptions.RentWords != null
                    && rentAdvertismentWordOptions.RentWords.Count > 0)
                {
                    rentAdvList.RentAdvertisments = new List<DTO.Advertisment>();
                    foreach (var adversitment in adversitments)
                    {
                        foreach (var takeOffWord in rentAdvertismentWordOptions.TakeOffWords)
                        {
                            if (adversitment.text.ToLower().Contains(takeOffWord))
                            {
                                rentAdvList.RentAdvertisments.Add(adversitment);
                                break;
                            }
                        }

                        foreach (var rentWord in rentAdvertismentWordOptions.RentWords)
                        {
                            if (adversitment.text.ToLower().Contains(rentWord))
                            {
                                rentAdvList.RentAdvertisments.Add(adversitment);
                                break;
                            }
                        }
                    }

                    //--- put all others that not contains in TakeOff
                    rentAdvList.SaleAdvertisments = new List<DTO.Advertisment>();
                    if (rentAdvList.RentAdvertisments != null)
                    {
                        foreach (var adversitment in adversitments)
                        {
                            if (!rentAdvList.RentAdvertisments.Contains(adversitment))
                                rentAdvList.SaleAdvertisments.Add(adversitment);
                        }
                    }
                    else
                    {
                        foreach (var adversitment in adversitments)
                            rentAdvList.SaleAdvertisments.Add(adversitment);
                    }
                }
            }
        }

        return rentAdvList;
    }
}