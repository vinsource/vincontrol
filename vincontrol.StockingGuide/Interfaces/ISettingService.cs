using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Service
{
    public interface ISettingService
    {
        List<string> GetBrandListForDealer(int dealerId);
        List<BrandDealer> GetBrandDealerList();
    }
}