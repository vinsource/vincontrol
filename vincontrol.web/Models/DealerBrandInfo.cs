using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class DealerBrandInfo
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string StrTurnOver { get; set; }
        public string StrGrossPerUnit { get; set; }

        public int History { get; set; }
        public int Stock { get; set; }
        public int Guide { get; set; }

        public int Supply { get; set; }
        public int Age { get; set; }
        public double TurnOver { get; set; }
        public int GrossPerUnit { get; set; }

        public int Recon { get; set; }
        public bool IsWishList { get; set; }
        public int DealerId { get; set; }
        public int SGDealerBrandId { get; set; }
        public int SoldOneMonthBefore { get; set; }
        public int SoldTwoMonthBefore { get; set; }
        public int SoldThreeMonthBefore { get; set; }

        public double Balance { get; set; }
        public string BalancePercent { get; set; }

        public string URLDetail { get; set; }
    }
}
