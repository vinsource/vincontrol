using System.Collections.Generic;

namespace vincontrol.DomainObject
{
    public class SoldInfo
    {
        public int Last30Days { get; set; }
        public int Last30To60Days { get; set; }
        public int Last60To90Days { get; set; }
    }

    public class RecommendationSoldInfo
    {
        public List<MarketCarInfo> Last30Days { get; set; }
        public List<MarketCarInfo> Last30To60Days { get; set; }
        public List<MarketCarInfo> Last60To90Days { get; set; }
    }
}