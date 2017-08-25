using System;

namespace Vincontrol.Web.Models
{
    public class SGInventoryDealerSegmentDetailInfo
    {
        public int SGInventoryDealerSegmentDetailId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int InStock { get; set; }
        public int OU { get; set; }
        public int Guide { get; set; }
        public int History { get; set; }
        public int Recon { get; set; }
        public double TurnOver { get; set; }
        public double Supply { get; set; }
        public Nullable<short> SGSegmentId { get; set; }
        public int DealerId { get; set; }
        public double Age { get; set; }
        public Nullable<int> SGDealerSegmentId { get; set; }
        public bool IsWishList { get; set; }

        public string URLDetail { get; set; }

        public string StrTurnOver { get; set; }
    }
}
