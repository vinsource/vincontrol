using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IManheimAuctionService
    {
        List<KeyValueObject> GetRegionCodeMapping();
    }
}
