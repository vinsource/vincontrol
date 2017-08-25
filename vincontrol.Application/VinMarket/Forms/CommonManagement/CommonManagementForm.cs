using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Application.VinMarket.ViewModels.CommonManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Model.Truck;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;

namespace vincontrol.Application.VinMarket.Forms.CommonManagement
{
    public class CommonManagementForm : BaseForm, ICommonManagementForm
    {
        #region Constructors
        public CommonManagementForm() : this(new SqlUnitOfWork()) { }

        public CommonManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ICommonManagementForm' Members

        #region CommercialTruck
        public void AddNewCommercialTruck(CommercialTruckViewModel obj)
        {
            var existingTruck = UnitOfWork.VinmarketCommon.GetTruckByCommercialTruckId(obj.CommercialTruckId);
            if (existingTruck == null)
                UnitOfWork.VinmarketCommon.AddNewCommercialTruck(MappingHandler.ToEntity(obj));
            else
            {
                existingTruck.BodyStyle = obj.BodyStyle;
                existingTruck.Category = obj.Category;
                existingTruck.IsNew = obj.IsNew;
                existingTruck.Price = obj.Price;
                existingTruck.Mileage = obj.Mileage;
                existingTruck.Images = obj.Images;
                existingTruck.Description = obj.Description;
                existingTruck.Url = obj.Url;
                existingTruck.Updated = DateTime.Now;
            }
            UnitOfWork.CommitVinCommercialTruckModel();
        }

        public void AddNewCommercialTruckDealer(CommercialTruckDealerViewModel obj)
        {
            UnitOfWork.VinmarketCommon.AddNewCommercialTruckDealer(MappingHandler.ToEntity(obj));
            UnitOfWork.CommitVinCommercialTruckModel();
        }

        public CommercialTruckDealerViewModel GetByCommercialTruckDealerName(string name) 
        {
            var dealer = UnitOfWork.VinmarketCommon.GetByCommercialTruckDealerName(name);
            return dealer != null ? new CommercialTruckDealerViewModel(dealer) : new CommercialTruckDealerViewModel();
        }

        public List<MarketCarInfo> GetNationwideMarketDataForTruck(int year, string make, string modelWord, string bodyStyle, bool ignoredTrim = false)
        {
            var list = UnitOfWork.VinmarketCommon.GetNationwideMarketDataForTruck(year, make, modelWord, bodyStyle, ignoredTrim);
            return list.Any() ? MappingHandler.ToEntities(list.ToList()) : new List<MarketCarInfo>();
        }

        public List<MarketCarInfo> GetNationwideMarketDataForTruck(int yearFrom,int yearTo, string make, string modelWord, string bodyStyle, bool ignoredTrim = false)
        {
            var list = UnitOfWork.VinmarketCommon.GetNationwideMarketDataForTruck(yearFrom,yearTo, make, modelWord, bodyStyle, ignoredTrim);
            return list.Any() ? MappingHandler.ToEntities(list.ToList()) : new List<MarketCarInfo>();
        }
        
        public void MarkSoldCommercialTrucks()
        {
            try
            {
                var list = UnitOfWork.VinmarketCommon.GetTrucksToMarkSold().ToList();
                Console.WriteLine("TOTAL: {0}", list.Count);
                foreach (var item in list)
                {
                    try
                    {
                        UnitOfWork.VinmarketCommon.AddSoldOutCommercialTruck(new CommercialTruckSoldOut(item));
                        UnitOfWork.VinmarketCommon.DeleteCommercialTruck(item);
                        UnitOfWork.CommitVinCommercialTruckModel();
                        Console.WriteLine("DELETED: {0}", item.CommercialTruckId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: {0} {1}", item.CommercialTruckId, ex.Message + " " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Cannot get commercial trucks: {0}", ex.Message + " " + ex.StackTrace);
            }
            
        }

        public List<CarMaxStore> GetCarMaxStores()
        {
            return UnitOfWork.VinmarketCommon.GetAllCarMaxStore();
        }

        #endregion

        #region CarMax
        public void AddNewCarMaxStore(CarMaxStoreViewModel model)
        {
            var store = UnitOfWork.VinmarketCommon.GetCarMaxStore(model.CarMaxStoreId);
            var zipcode = UnitOfWork.VinmarketCommon.GetSpecificZipCode(model.ZipCode);
            if (store == null)
            {
                model.State = zipcode.StateAbbr;
                model.Latitude = zipcode.Latitude.GetValueOrDefault();
                model.Longitude = zipcode.Longitude.GetValueOrDefault();
                UnitOfWork.VinmarketCommon.AddNewCarMaxStore(MappingHandler.ToEntity(model));
            }
            else
            {
                store.CarMaxStoreId = model.CarMaxStoreId;
                store.Name = model.Name;
                store.FullName = model.FullName;
                store.Url = model.Url;
                store.Address = model.Address;
                store.City = model.City;
                store.State = zipcode.StateAbbr;
                store.Latitude = zipcode.Latitude.GetValueOrDefault();
                store.Longitude = zipcode.Longitude.GetValueOrDefault();
                store.ZipCode = model.ZipCode;
                store.Phone = model.Phone;
                store.UpdatedDate = DateTime.Now;
            }
            UnitOfWork.CommitVinmarketModel();
        }

        public CarMaxStoreViewModel GetCarMaxStore(int carmaxStoreId)
        {
            var store = UnitOfWork.VinmarketCommon.GetCarMaxStore(carmaxStoreId);
            return store != null ? new CarMaxStoreViewModel(store) : new CarMaxStoreViewModel();
        }

        public void UpdateCarMaxVehicleStore(long carId, int storeId)
        {
            var updateFlag=UnitOfWork.VinmarketCommon.UpdateCarMaxVehicleStore(carId,storeId);
            if(updateFlag)
                UnitOfWork.CommitVinmarketModel();
        }

        public CarMaxStoreViewModel GetCarMaxStore(string carmaxStoreName)
        {
            var store = UnitOfWork.VinmarketCommon.GetCarMaxStore(carmaxStoreName);
            return store != null ? new CarMaxStoreViewModel(store) : new CarMaxStoreViewModel();
        }

        public void AddNewCarMaxVehicle(CarMaxVehicleViewModel model)
        {
            var vehicle = UnitOfWork.VinmarketCommon.GetCarMaxVehicle(model.CarMaxVehicleId);
            if (vehicle == null)
                UnitOfWork.VinmarketCommon.AddNewCarMaxVehicle(MappingHandler.ToEntity(model));
            else
            {
                vehicle.CarMaxVehicleId = model.CarMaxVehicleId;
                vehicle.StoreId = model.StoreId.Equals(0) ? (int?)null : model.StoreId;
                vehicle.Year = model.Year;
                vehicle.Make = model.Make;
                vehicle.Model = model.Model;
                vehicle.Trim = model.Trim;
                vehicle.Price = model.Price;
                vehicle.Miles = model.Miles;
                vehicle.Vin = model.Vin;
                vehicle.Stock = model.Stock;
                vehicle.DriveTrain = model.DriveTrain;
                vehicle.Transmission = model.Transmission;
                vehicle.ExteriorColor = model.ExteriorColor;
                vehicle.InteriorColor = model.InteriorColor;
                vehicle.MPGHighway = model.MPGHighway;
                vehicle.MPGCity = model.MPGCity;
                vehicle.Rating = model.Rating;
                vehicle.Features = model.Features;
                vehicle.ThumbnailPhotos = model.ThumbnailPhotos;
                vehicle.FullPhotos = model.FullPhotos;
                vehicle.Certified = model.Certified;
                vehicle.Used = model.Used;
                vehicle.Url = model.Url;
                vehicle.UpdatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            }
            UnitOfWork.CommitVinmarketModel();
        }

        public CarMaxVehicleViewModel GetCarMaxVehicle(int carmaxVehicleId)
        {
            var vehicle = UnitOfWork.VinmarketCommon.GetCarMaxVehicle(carmaxVehicleId);
            return vehicle != null ? new CarMaxVehicleViewModel(vehicle) : new CarMaxVehicleViewModel();
        }

        public void MarkSoldCarMaxVehicles()
        {
            try
            {
                var list = UnitOfWork.VinmarketCommon.GetCarMaxVehiclesToMarkSold().ToList();
                Console.WriteLine("TOTAL: {0}", list.Count);
                int count = 0;
                foreach (var item in list)
                {
                    try
                    {
                        var carmaxSoldOut = new CarMaxVehicleSoldOut()
                        {

                            CarMaxVehicleId = item.CarMaxVehicleId,
                            Certified = item.Certified,
                            DateStamp = item.CreatedDate,
                            DriveTrain = item.DriveTrain,
                            ExteriorColor = item.ExteriorColor,
                            Features = item.Features,
                            FullPhotos = item.FullPhotos,
                            InteriorColor = item.InteriorColor,
                            MPGCity = item.MPGCity,
                            MPGHighway = item.MPGHighway,
                            Make = item.Make,
                            Miles = item.Miles,
                            Model = item.Model,
                            Price = item.Price,
                            Rating = item.Rating,
                            Stock = item.Stock,
                            StoreId = item.StoreId,
                            ThumbnailPhotos = item.ThumbnailPhotos,
                            Transmission = item.Transmission,
                            Trim = item.Trim,
                            Url = item.Url,
                            Used = item.Used,
                            VehicleId = item.VehicleId,
                            Vin = item.Vin,
                            Year = item.Year,

                        };

                        UnitOfWork.VinmarketCommon.AddSoldOutCarMaxVehicle(carmaxSoldOut);
                        UnitOfWork.VinmarketCommon.DeleteCarMaxVehicle(item);
                        UnitOfWork.CommitVinmarketModel();
                        count++;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Console.WriteLine("MARK SOLD: {0}", count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR when getting carmax vehicles to mark sold: {0}", ex);
            }
        }

        public List<CarMaxVehicleViewModel> GetCarMaxVehiclesMissingStoreId()
        {
            var list = UnitOfWork.VinmarketCommon.GetCarMaxVehiclesMissingStoreId();
            return list.Any() ? list.AsEnumerable().Select(i => new CarMaxVehicleViewModel(i)).ToList() : new List<CarMaxVehicleViewModel>();
        }

        public List<CarMaxVehicleViewModel> GetCarMaxVehiclesMissingStoreId(string make)
        {
            var list = UnitOfWork.VinmarketCommon.GetCarMaxVehiclesMissingStoreId(make);
            return list.Any() ? list.AsEnumerable().Select(i => new CarMaxVehicleViewModel(i)).ToList() : new List<CarMaxVehicleViewModel>();
        }
        #endregion

        #endregion
    }
}
