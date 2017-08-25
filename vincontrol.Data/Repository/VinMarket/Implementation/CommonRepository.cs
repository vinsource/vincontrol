using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.Data.Model.Truck;
using vincontrol.Data.Repository.VinMarket.Interface;

namespace vincontrol.Data.Repository.VinMarket.Implementation
{
    public class CommonRepository : ICommonRepository
    {
        private VinMarketEntities _context;
        private VinCommercialTruckEntities _truckContext;
     
        public CommonRepository(VinMarketEntities context)
        {
            _context = context;
        }

        public CommonRepository(VinCommercialTruckEntities truckContext)
        {
            _truckContext = truckContext;
        }

        public CommonRepository(VinMarketEntities context, VinCommercialTruckEntities truckContext)
        {
            _context = context;
            _truckContext = truckContext;
          
        }

        #region ICommonRepository' Members

        #region CommercialTruck
        public void AddNewCommercialTruck(Model.Truck.CommercialTruck obj)
        {
            _truckContext.CommercialTrucks.AddObject(obj);
        }

        public void AddSoldOutCommercialTruck(Model.Truck.CommercialTruckSoldOut obj)
        {
            _truckContext.CommercialTruckSoldOuts.AddObject(obj);
        }

        public Model.Truck.CommercialTruck GetTruckByCommercialTruckId(int commercialTruckId)
        {
            return _truckContext.CommercialTrucks.FirstOrDefault(i => i.CommercialTruckId.Equals(commercialTruckId));
        }

        public void AddNewCommercialTruckDealer(Model.Truck.CommercialTruckDealer obj)
        {
            if (GetByCommercialTruckDealerId(obj.CommercialTruckDealerId) == null)
                _truckContext.CommercialTruckDealers.AddObject(obj);
        }

        public void DeleteCommercialTruck(int commercialTruckId)
        {
            _truckContext.CommercialTrucks.DeleteObject(GetTruckByCommercialTruckId(commercialTruckId));
        }

        public void DeleteCommercialTruck(Model.Truck.CommercialTruck commercialTruck)
        {
            _truckContext.CommercialTrucks.DeleteObject(commercialTruck);
        }

        public Model.Truck.CommercialTruckDealer GetByCommercialTruckDealerId(int commercialTruckDealerId)
        {
            return _truckContext.CommercialTruckDealers.FirstOrDefault(i => i.CommercialTruckDealerId.Equals(commercialTruckDealerId));
        }

        public Model.Truck.CommercialTruckDealer GetByCommercialTruckDealerName(string name)
        {
            return _truckContext.CommercialTruckDealers.FirstOrDefault(i => i.Name.Equals(name));
        }

        public static string FilterTruckModelForMarket(string modelWord)
        {
            if (!String.IsNullOrEmpty(modelWord))
            {
                modelWord = modelWord.Replace("commercial cutaway", "");

                modelWord = modelWord.Replace("super duty", "");

                modelWord = modelWord.Replace("reg cab", "");

                //modelWord = modelWord.Replace("econoline cargo", "");
                modelWord = modelWord.Replace("drw", "");

                modelWord = modelWord.Replace("dsl", "");

                modelWord = modelWord.Replace("reg", "");

                modelWord = modelWord.Replace("recreational", "");

                modelWord = modelWord.Replace("white", "");

                modelWord = modelWord.Replace("-", "");

                 modelWord = modelWord.Replace("savana cargo van", "cargo van");
                 

                return modelWord.Trim();

            }

            else
                return String.Empty;
        }

        public IQueryable<Model.Truck.CommercialTruck> GetNationwideMarketDataForTruck(int year,string make, string modelWord, string bodyStyle, bool ignoredTrim = false)
        {
            var query = _truckContext.CommercialTrucks.Include("CommercialTruckDealer").Where(i => i.CommercialTruckDealer != null);

            modelWord = FilterTruckModelForMarket(modelWord.ToLower());

            if (make.ToLower().Equals("chevrolet"))
            {
                if (modelWord.Contains("silverado"))
                {

                    var finalModelWord = modelWord.Replace(" ", "");

                    var result =
                  query.Where(
                      i =>
                          i.Year == year && i.Make == make
                          &&
                          (i.Model.ToLower().Replace("-", "").Replace(" ", "") == finalModelWord) && i.Price > 0 && i.Mileage > 0);

                    return result;
                }
                if (modelWord.Contains("express"))
                {

                    var finalModelWord = modelWord.Replace(" ", "");

                    var result =
                  query.Where(
                      i =>
                          i.Year == year && i.Make == make
                          &&
                          (i.Model.ToLower().Replace("-", "").Replace(" ", "") == finalModelWord) && i.Price > 0 && i.Mileage > 0);

                    return result;
                }
                else
                {
                    var finalModelWord = modelWord.Replace(" ", "");

                    var result =
                        query.Where(
                            i =>
                                i.Year == year && i.Make == make
                                &&
                                (i.Model.ToLower().Replace("-", "").Replace(" ", "").Contains(finalModelWord)) && i.Price > 0 && i.Mileage > 0);

                    return result;
                }
            }
           
            else
            {
                var finalModelWord = modelWord.Replace(" ", "");

                var result =
                    query.Where(
                        i =>
                            i.Year == year && i.Make == make
                            &&
                            (i.Model.ToLower().Replace("-", "").Replace(" ", "").Contains(finalModelWord)) && i.Price > 0 && i.Mileage > 0);

                return result;
            }

            
        }

        public IQueryable<Model.Truck.CommercialTruck> GetNationwideMarketDataForTruck(int yearFrom,int yearTo, string make, string modelWord, string bodyStyle, bool ignoredTrim = false)
        {
            var query = _truckContext.CommercialTrucks.Include("CommercialTruckDealer").Where(i => i.CommercialTruckDealer != null);

            modelWord = FilterTruckModelForMarket(modelWord.ToLower());

            if (make.ToLower().Equals("chevrolet"))
            {
                if (modelWord.Contains("silverado"))
                {

                    var finalModelWord = modelWord.Replace(" ", "");

                    var result =
                  query.Where(
                      i =>
                          i.Year >=yearFrom && i.Year <=yearTo && i.Make == make
                          &&
                          (i.Model.ToLower().Replace("-", "").Replace(" ", "") == finalModelWord) && i.Price > 0 && i.Mileage > 0);

                    return result;
                }
                if (modelWord.Contains("express"))
                {

                    var finalModelWord = modelWord.Replace(" ", "");

                    var result =
                  query.Where(
                      i =>
                          i.Year >= yearFrom && i.Year <= yearTo && i.Make == make
                          &&
                          (i.Model.ToLower().Replace("-", "").Replace(" ", "") == finalModelWord) && i.Price > 0 && i.Mileage > 0);

                    return result;
                }
                else
                {
                    var finalModelWord = modelWord.Replace(" ", "");

                    var result =
                        query.Where(
                            i =>
                                i.Year >= yearFrom && i.Year <= yearTo && i.Make == make
                                &&
                                (i.Model.ToLower().Replace("-", "").Replace(" ", "").Contains(finalModelWord)) && i.Price > 0 && i.Mileage > 0);

                    return result;
                }
            }

            else
            {
                var finalModelWord = modelWord.Replace(" ", "");

                var result =
                    query.Where(
                        i =>
                            i.Year >= yearFrom && i.Year <= yearTo && i.Make == make
                            &&
                            (i.Model.ToLower().Replace("-", "").Replace(" ", "").Contains(finalModelWord)) && i.Price > 0 && i.Mileage > 0);

                return result;
            }


        }

        public IQueryable<Model.Truck.CommercialTruck> GetTrucksToMarkSold()
        {
            var dateToCompare = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddDays(-1);
            var query = _truckContext.CommercialTrucks.Include("CommercialTruckDealer").Where(i => i.Updated < dateToCompare).AsQueryable();
            return query;
        }
        #endregion

        #region CarMax
        public void AddNewCarMaxStore(CarMaxStore obj)
        {
            _context.CarMaxStores.AddObject(obj);
        }

        public usazipcode GetSpecificZipCode(int zipcode)
        {
            return _context.usazipcodes.FirstOrDefault(x => x.ZIPCode == zipcode);
        }

        public List<CarMaxStore> GetAllCarMaxStore()
        {
            return _context.CarMaxStores.ToList();
        }

        public CarMaxStore GetCarMaxStore(long carmaxStoreId)
        {
            return _context.CarMaxStores.FirstOrDefault(i => i.CarMaxStoreId.Equals(carmaxStoreId));
        }

        public CarMaxStore GetCarMaxStore(string carmaxStoreName)
        {
            return _context.CarMaxStores.FirstOrDefault(i => i.FullName.Equals(carmaxStoreName));
        }

        public void AddNewCarMaxVehicle(CarMaxVehicle obj)
        {
            _context.CarMaxVehicles.AddObject(obj);
        }

        public bool UpdateCarMaxVehicleStore(long carId, int storeId)
        {
            var lookupVehilce = _context.CarMaxVehicles.FirstOrDefault(i => i.CarMaxVehicleId == carId);

            if (lookupVehilce != null)
            {
                lookupVehilce.StoreId = storeId;
                return true;
            }
            return false;
        }

        public void AddSoldOutCarMaxVehicle(CarMaxVehicleSoldOut obj)
        {
            _context.CarMaxVehicleSoldOuts.AddObject(obj);
        }

        public void DeleteCarMaxVehicle(CarMaxVehicle obj)
        {
            _context.CarMaxVehicles.DeleteObject(obj);
        }

        public CarMaxVehicle GetCarMaxVehicle(long carmaxVehicleId)
        {
            return _context.CarMaxVehicles.FirstOrDefault(i => i.CarMaxVehicleId.Equals(carmaxVehicleId));
        }

        public IQueryable<CarMaxVehicle> GetCarMaxVehiclesMissingStoreId()
        {
            return _context.CarMaxVehicles.Where(i => i.StoreId == null);
        }

        public IQueryable<CarMaxVehicle> GetCarMaxVehiclesMissingStoreId(string make)
        {
            return _context.CarMaxVehicles.Where(i => i.StoreId == null && i.Make.Equals(make));
        }

        public IQueryable<CarMaxVehicle> GetCarMaxVehiclesToMarkSold()
        {
            var dateToCompare = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddDays(-1);
            var query = _context.CarMaxVehicles.Include("CarMaxStore").Where(i => i.UpdatedDate < dateToCompare).AsQueryable();
            return query;
        }
        #endregion

        #endregion

        #region Private Methods

        #endregion
    }
}
