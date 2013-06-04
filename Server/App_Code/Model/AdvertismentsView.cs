using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AdvertismentsView
{
    public AdvertismentsView(List<Advertisment> advertisments, int fullCount, DateTime date)
    {
        Advertisments = advertisments;
        FullCount = fullCount;
        Date = date;
    }

    public List<Advertisment> Advertisments { get; set; }
    public int FullCount { get; set; }
    public DateTime Date { get; set; }
}