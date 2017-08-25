using System;

namespace Vincontrol.Web.Models
{
    public class SGMarketDealerSegmentDetailInfo
    {
        public int DealerId { get; set; }
        public Nullable<short> SGSegmentId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int YourStock { get; set; }
        public double BalanceYourStock { get; set; }
        public int MarketStock { get; set; }
        public int BalanceMarketStock { get; set; }
        public int MarketHistory { get; set; }
        public int History { get; set; }
        public int Supply { get; set; }
        public int Age { get; set; }
        public double TurnOver { get; set; }
        public Nullable<bool> IsWishList { get; set; }
        public int SGMarketDealerSegmentDetailId { get; set; }
        public Nullable<int> SGDealerSegmentId { get; set; }
        public string URLDetail { get; set; }

        public string StrTurnOver { get; set; }
    }
}
