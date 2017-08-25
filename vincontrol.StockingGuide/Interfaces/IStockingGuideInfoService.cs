using System;
using System.Linq;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IStockingGuideInfoService
    {
        //void AddStockingGuideInfo(StockingGuideInfo stockingGuideInfo);
        void AddStockingGuideInfo(SGKPIInfo stockingGuideInfo);

        IQueryable<SGKPIInfo> GetStockingGuideInfoByDealerID(int dealerID);
    }
}