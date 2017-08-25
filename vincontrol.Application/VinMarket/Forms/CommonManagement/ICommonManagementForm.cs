using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.VinMarket.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application.VinMarket.Forms.CommonManagement
{
    public interface ICommonManagementForm
    {
        void AddNewCommercialTruck(CommercialTruckViewModel obj);
        void AddNewCommercialTruckDealer(CommercialTruckDealerViewModel obj);
        CommercialTruckDealerViewModel GetByCommercialTruckDealerName(string name);
        List<MarketCarInfo> GetNationwideMarketDataForTruck(int year, string make, string modelWord, string bodyStyle, bool ignoredTrim = false);
        List<MarketCarInfo> GetNationwideMarketDataForTruck(int yearFrom,int yearTo, string make, string modelWord, string bodyStyle, bool ignoredTrim = false);
        void MarkSoldCommercialTrucks();
        List<CarMaxStore> GetCarMaxStores();
        void AddNewCarMaxStore(CarMaxStoreViewModel obj);
        void AddNewCarMaxVehicle(CarMaxVehicleViewModel model);
        void UpdateCarMaxVehicleStore(long carId,int storeId);
        CarMaxStoreViewModel GetCarMaxStore(string carmaxStoreName);
        void MarkSoldCarMaxVehicles();
        List<CarMaxVehicleViewModel> GetCarMaxVehiclesMissingStoreId();
        List<CarMaxVehicleViewModel> GetCarMaxVehiclesMissingStoreId(string make);
    }
}
