using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IDealerService
    {
        IEnumerable<int> GetDealerIdList();
        List<DealerLocation> GetDealerLocationList();
        List<DealerLocation> GetDealerLocationListFromInventory();
        List<int> GetDealerIdListFromInventorySegmentDetail();
        List<int> GetDealerIdListFromInventory();

    }
}
