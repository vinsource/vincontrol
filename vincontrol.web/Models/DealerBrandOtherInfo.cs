using System.Collections.Generic;

namespace Vincontrol.Web.Models
{
    public class DealerBrandOtherInfo
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
        public decimal GrossPerUnit { get; set; }

        public int Recon { get; set; }
        public bool IsWishList { get; set; }
        public int DealerId { get; set; }
        public int SGSegmentId { get; set; }
        public int SGDealerSegmentId { get; set; }
        public int SoldOneMonthBefore { get; set; }
        public int SoldTwoMonthBefore { get; set; }
        public int SoldThreeMonthBefore { get; set; }

        public double Balance { get; set; }
        public string BalancePercent { get; set; }

        
        public List<SGInventoryDealerSegmentDetailInfo> SGInventoryDealerSegmentDetails { get; set; }
        public List<SGMarketDealerSegmentDetailInfo> SGMarketDealerSegmentDetails { get; set; }
    }
}
