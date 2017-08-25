using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class InventoryService : IInventoryService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public InventoryService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public int GetCurrentMonthUsedStock(int dealerId)
        {
            return _vincontrolUnitOfWork.InventoryRepository.Find(i => i.DealerId == dealerId && i.Condition == Constants.ConditionStatus.Used).Count();
      
        }

        public double GetCurrentMonthUsedStockForModel(string make, string model, int dealerId)
        {
            return GetUsedInventoryWithModel(make, model, dealerId).Count();
        }

        public IQueryable<Inventory> GetUsedInventoryWithModel(string make, string model, int dealerId)
        {
            return GetUsedInventoryWithMake(make,dealerId).Where(i=> i.Vehicle.Model == model);

        }

        public IQueryable<Inventory> GetUsedInventoryWithMake(string make, int dealerId)
        {
            return GetUsedInventoryForDealer(dealerId).Where(i=> i.Vehicle.Make == make);
        }

        public IQueryable<Inventory> GetUsedInventoryForDealer(int dealerId)
        {
            
            return
            _vincontrolUnitOfWork.InventoryRepository.Find(
                i =>
                    i.DealerId == dealerId && i.Condition == Constants.ConditionStatus.Used);
        }

        public IQueryable<Inventory> GetUsedInventory()
        {
            return _vincontrolUnitOfWork.InventoryRepository.Find(i => i.Condition == Constants.ConditionStatus.Used);
        }

        public decimal? GetTotalSalesValue(int dealershipId)
        {
            return _vincontrolUnitOfWork.InventoryRepository.Find(i => i.DealerId == dealershipId).Sum(i=>i.SalePrice);
        }

        public IQueryable<Inventory>  GetReconList(int dealerId)
         {
             return _vincontrolUnitOfWork.InventoryRepository.Find(i => i.DealerId == dealerId && i.InventoryStatusCodeId == Constants.InventoryStatus.Recon);
         }

        public List<int> GetDealerIdList()
        {
            return GetUsedInventory().Where(i => i.DealerId != 0).Select(i => i.DealerId).Distinct().ToList();
        }

        public List<DealerLocation> GetDealerLocationList()
        {
            return GetUsedInventory().Where(i => i.DealerId != 0).Select(i =>new DealerLocation() {DealerId = i.DealerId, Longitude = i.Dealer.Longtitude??0, Latitude = i.Dealer.Lattitude??0}).Distinct().ToList();
        }

    }
}
