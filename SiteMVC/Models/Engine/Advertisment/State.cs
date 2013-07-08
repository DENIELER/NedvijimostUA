using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMVC.Models.Engine.Advertisment
{
    public enum State
    {
        JustParsed,
        Subpurchase,
        NotSubpurchase,
        SubpurchaseWithNotSubpurchase
    }
}