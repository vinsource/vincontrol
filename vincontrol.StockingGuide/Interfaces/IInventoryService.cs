using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IInventoryService
    {
        int GetCurrentMonthUsedStock(int dealerId);
        double GetCurrentMonthUsedStockForModel(string make, string model, int dealerId);
        IQueryable<Inventory> GetUsedInventoryWithModel(string make, string model, int dealerId);
        IQueryable<Inventory> GetUsedInventoryWithMake(string make, int dealerId);
        IQueryable<Inventory> GetUsedInventoryForDealer(int dealerId);
        IQueryable<Inventory> GetReconList(int dealerId);
        IQueryable<Inventory> GetUsedInventory();
        decimal? GetTotalSalesValue(int dealershipId);
        List<int> GetDealerIdList();
        List<DealerLocation> GetDealerLocationList();
    }
}