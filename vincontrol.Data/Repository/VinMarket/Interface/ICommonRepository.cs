using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.VinMarket.Interface
{
    public interface ICommonRepository
    {
        void AddNewCommercialTruck(Model.Truck.CommercialTruck obj);
        void AddSoldOutCommercialTruck(Model.Truck.CommercialTruckSoldOut obj);
        void AddNewCommercialTruckDealer(Model.Truck.CommercialTruckDealer obj);
        void DeleteCommercialTruck(int commercialTruckId);
        void DeleteCommercialTruck(Model.Truck.CommercialTruck commercialTruck);
        Model.Truck.CommercialTruckDealer GetByCommercialTruckDealerId(int commercialTruckDealerId);
        Model.Truck.CommercialTruckDealer GetByCommercialTruckDealerName(string name);
        Model.Truck.CommercialTruck GetTruckByCommercialTruckId(int commercialTruckId);
        IQueryable<Model.Truck.CommercialTruck> GetNationwideMarketDataForTruck(int year, string make, string modelWord, string bodyStyle, bool ignoredTrim = false);
        IQueryable<Model.Truck.CommercialTruck> GetNationwideMarketDataForTruck(int yearFrom,int yearTo, string make, string modelWord, string bodyStyle, bool ignoredTrim = false);
        IQueryable<Model.Truck.CommercialTruck> GetTrucksToMarkSold();
        
        void AddNewCarMaxStore(CarMaxStore obj);
        void AddSoldOutCarMaxVehicle(CarMaxVehicleSoldOut obj);
        usazipcode GetSpecificZipCode(int zipcode);

        List<CarMaxStore> GetAllCarMaxStore();
        CarMaxStore GetCarMaxStore(long carmaxStoreId);
        CarMaxStore GetCarMaxStore(string carmaxStoreName);
        void AddNewCarMaxVehicle(CarMaxVehicle obj);
        bool UpdateCarMaxVehicleStore(long carId, int storeId);
        void DeleteCarMaxVehicle(CarMaxVehicle obj);
        CarMaxVehicle GetCarMaxVehicle(long carmaxVehicleId);
        IQueryable<CarMaxVehicle> GetCarMaxVehiclesMissingStoreId();
        IQueryable<CarMaxVehicle> GetCarMaxVehiclesMissingStoreId(string make);
        IQueryable<CarMaxVehicle> GetCarMaxVehiclesToMarkSold();
    }
}
