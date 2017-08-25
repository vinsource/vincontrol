using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
//using vincontrol.CarFax;
using vincontrol.Application.ViewModels.InventoryManagement;
using vincontrol.Constant;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;
using ChartSelection = vincontrol.Data.Model.ChartSelection;

namespace vincontrol.Application.Forms.InventoryManagement
{
    public class InventoryManagementForm : BaseForm, IInventoryManagementForm
    {
        #region Constructors
        public InventoryManagementForm() : this(new SqlUnitOfWork()) { /*_carfaxService = new CarFaxService();*/ }

        public InventoryManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IInventoryManagementForm Members

        public IList<CarInfoViewModel> GetUsedSoldInventories(int dealerId)
        {
            var list = UnitOfWork.Inventory.GetUsedSoldInventories(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public SoldoutInventory GetSoldInventory(int listingId)
        {
            return UnitOfWork.Inventory.GetSoldoutInventory(listingId);
        }

        public Inventory GetInventory(int listingId)
        {
            return UnitOfWork.Inventory.GetInventory(listingId);
        }

        public ChartSelection GetChartSelection(int listingId)
        {
            return UnitOfWork.Inventory.GetChartSelection(listingId);
        }

        public IList<CarInfoViewModel> GetUsedInventories(int dealerId)
        {
            var list = UnitOfWork.Inventory.GetUsedInventories(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetUsedInventories(int dealerId, int pageIndex, int pageSize)
        {
            var list = UnitOfWork.Inventory.GetUsedInventories(dealerId, pageIndex, pageSize);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetUsedInventories(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock)
        {
            var list = UnitOfWork.Inventory.GetUsedInventories(dealerId, pageIndex, pageSize, year, make, model, stock);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetUsedInventoriesIncludeSoldOut(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock)
        {
            var list = UnitOfWork.Inventory.GetUsedInventoriesIncludeSoldOut(dealerId, pageIndex, pageSize, year, make, model, stock);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetSimilarUsedInventories(int dealerId, string vin, string make, string model)
        {
            var list = UnitOfWork.Inventory.GetSimilarUsedInventories(dealerId, vin, make, model);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetSimilarUsedInventories(string groupId, int excludedListingId, int year, string make, string model, string trim)
        {
            var list = UnitOfWork.Inventory.GetSimilarUsedInventories(groupId, excludedListingId, year, make, model, trim);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetSimilarBucketJumpInventories(string groupId, int excludedListingId, int year, string make, string model, string trim, int firstTimeRangeBucketJump, int secondTimeRangeBucketJump, int intervalBucketJump)
        {
            var list = UnitOfWork.Inventory.GetSimilarUsedInventories(groupId, excludedListingId, year, make, model, trim);
            var result = new List<CarInfoViewModel>();
            foreach (var tmp in list)
            {
                var dateatMidnight = new DateTime(tmp.DateInStock.GetValueOrDefault().Year,
                    tmp.DateInStock.GetValueOrDefault().Month,
                    tmp.DateInStock.GetValueOrDefault().Day);

                var daysInInventory = DateTime.Now.Subtract(dateatMidnight).Days;
                var bucketDay = tmp.BucketJumpCompleteDay.GetValueOrDefault();
                var nextBucketDay = 0;
                if (bucketDay == 0 || bucketDay < firstTimeRangeBucketJump)
                    nextBucketDay = firstTimeRangeBucketJump;
                else if (bucketDay < secondTimeRangeBucketJump)
                    nextBucketDay = secondTimeRangeBucketJump;
                else if (bucketDay >= secondTimeRangeBucketJump)
                    nextBucketDay = bucketDay + intervalBucketJump;

                var flag = /*bucketDay == 0 || */ nextBucketDay <= daysInInventory;

                if (!flag) continue;
                var car = new CarInfoViewModel(tmp)
                {
                    NotDoneBucketJump = true,
                    Type = Constanst.CarInfoType.Used
                };
                result.Add(car);
            }
            return result;
        }

        public IList<CarInfoViewModel> GetSimilarMake(int dealerId, string vin, string make)
        {

            var list = UnitOfWork.Inventory.GetSimilarMake(dealerId, vin, make,4);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetSimilarModel(int dealerId, string vin, string model)
        {

            var list = UnitOfWork.Inventory.GetSimilarModel(dealerId, vin, model, 4);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetSimilarBodyType(int dealerId, string vin, string body)
        {
            var list = UnitOfWork.Inventory.GetSimilarBodyType(dealerId, vin, body, 4);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarInfoViewModel> GetNewInventories(int dealerId)
        {
            var list = UnitOfWork.Inventory.GetNewInventories(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new CarInfoViewModel(i)).ToList() : new List<CarInfoViewModel>();
        }

        public IList<CarShortViewModel> GetAllInventories(int dealerId)
        {
            var list = UnitOfWork.Inventory.GetAllInventories(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new CarShortViewModel(i)).ToList() : new List<CarShortViewModel>();
        }

        public IList<CarShortViewModel> GetFeaturedInventories(int dealerId)
        {
            var list = UnitOfWork.Inventory.GetFeaturedInventories(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new CarShortViewModel(i)).ToList() : new List<CarShortViewModel>();
        }

        public CarInfoViewModel GetFullInventoryDetail(int dealerId, string vin)
        {
            var existingCar = UnitOfWork.Inventory.GetInventory(vin, dealerId);
            var viewModel = existingCar != null ? new CarInfoViewModel(existingCar) : new CarInfoViewModel();

           

            return viewModel;
        }

        public CarInfoViewModel GetFullInventoryDetail(int dealerId, int listingId)
        {
            var existingCar = UnitOfWork.Inventory.GetInventory(listingId, dealerId);
            var viewModel = existingCar != null ? new CarInfoViewModel(existingCar) : new CarInfoViewModel();
            
            //try
            //{
            //    var dealerSetting = UnitOfWork.Admin.GetSetting(dealerId);
            //    viewModel.CarFax = _carfaxService.XmlSerializeCarFax(viewModel.Vin, dealerSetting.CarFax, dealerSetting.CarFaxPassword);
            //}
            //catch (Exception) { viewModel.CarFax = new CarFax.CarFaxViewModel(); }

            return viewModel;
        }

        public CarInfoViewModel GetFullSoldOutInventoryDetail(int dealerId, int listingId)
        {
            var existingCar = UnitOfWork.Inventory.GetSoldoutInventory(listingId, dealerId);
            var viewModel = existingCar != null ? new CarInfoViewModel(existingCar) : new CarInfoViewModel();

            //try
            //{
            //    var dealerSetting = UnitOfWork.Admin.GetSetting(dealerId);
            //    viewModel.CarFax = _carfaxService.XmlSerializeCarFax(viewModel.Vin, dealerSetting.CarFax, dealerSetting.CarFaxPassword);
            //}
            //catch (Exception) { viewModel.CarFax = new CarFax.CarFaxViewModel(); }

            return viewModel;
        }

        public CarShortViewModel GetCarInfo(int listingId)
        {
            var existingCar = UnitOfWork.Inventory.GetInventory(listingId);
            return existingCar != null ? new CarShortViewModel(existingCar) : new CarShortViewModel();
        }
        public CarShortViewModel GetSoldCarInfo(int listingId)
        {
            var existingCar = UnitOfWork.Inventory.GetSoldoutInventory(listingId);
            return existingCar != null ? new CarShortViewModel(existingCar) : new CarShortViewModel();
        }
        
        public CarShortViewModel GetCarInfo(int dealerId, string vin)
        {
            var existingCar = UnitOfWork.Inventory.GetInventory(vin, dealerId);
            return existingCar != null ? new CarShortViewModel(existingCar) : new CarShortViewModel();
        }

        public CarShortViewModel GetCarInfo(int dealerId, int listingId)
        {
            var existingCar = UnitOfWork.Inventory.GetInventory(listingId, dealerId);
            return existingCar != null ? new CarShortViewModel(existingCar) : new CarShortViewModel();
        }

        public List<SelectListItem> GetUsedYears(int dealerId)
        {
            return UnitOfWork.Inventory.GetUsedYears(dealerId);
        }

        public List<SelectListItem> GetUsedMakes(int dealerId)
        {
            return UnitOfWork.Inventory.GetUsedMakes(dealerId);
        }

        public List<SelectListItem> GetUsedModels(int dealerId)
        {
            return UnitOfWork.Inventory.GetUsedModels(dealerId);
        }

        public IList<VehicleLogTracking> GetVehicleLogs(int inventoryId)
        {
            var list = UnitOfWork.VehicleLog.GetVehicleLogs(inventoryId);
            return list.Any() ? list.AsEnumerable().Select(i => new VehicleLogTracking(i)).ToList() : new List<VehicleLogTracking>();
        }
        
        public IList<VehicleLogTracking> GetSoldVehicleLogs(int inventoryId)
        {
            var list = UnitOfWork.VehicleLog.GetSoldVehicleLogs(inventoryId);
            return list.Any() ? list.AsEnumerable().Select(i => new VehicleLogTracking(i)).ToList() : new List<VehicleLogTracking>();
        }

        public void AddPriceChangeHistory(PriceChangeHistory priceChangeHistory)
        {
            UnitOfWork.Inventory.AddPriceChangeHistory(priceChangeHistory);
            UnitOfWork.CommitVincontrolModel();
        }

        public void ResetKbbTrim(int listingId, int type)
        {
            UnitOfWork.Inventory.ResetKbbTrim(listingId,type);
            UnitOfWork.CommitVincontrolModel();
        }

        public void ResetManheimTrim(int listingId, int type)
        {
            UnitOfWork.Inventory.ResetManheimTrim(listingId, type);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateIsFeatured(int listingId, bool isFeatured)
        {
            UnitOfWork.Inventory.UpdateIsFeatured(listingId,isFeatured);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateIsFeaturedForSoldCar(int listingId, bool isFeatured)
        {
            UnitOfWork.Inventory.UpdateIsFeaturedForSoldCar(listingId, isFeatured);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateWarrantyInfo(int warrantyInfo, int listingId)
        {
            UnitOfWork.Inventory.UpdateWarrantyInfo(warrantyInfo, listingId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdatePriorRental(bool priorRental, int listingId)
        {
            UnitOfWork.Inventory.UpdatePriorRental(priorRental, listingId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateDealerDemo(bool dealerDemo, int listingId)
        {
            UnitOfWork.Inventory.UpdateDealerDemo(dealerDemo, listingId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateUnwind(bool unwind, int listingId)
        {
            UnitOfWork.Inventory.UpdateUnwind(unwind, listingId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateDescription(int listingId, string description)
        {
            UnitOfWork.Inventory.UpdateDescription(listingId, description);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateStatus(int listingId, short status)
        {
            UnitOfWork.Inventory.UpdateStatus(listingId, status);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateMileage(int listingId, long? mileage)
        {
            UnitOfWork.Inventory.UpdateMileage(listingId, mileage);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSoldMileage(int listingId, long? mileage)
        {
            UnitOfWork.Inventory.UpdateSoldMileage(listingId, mileage);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateDealerCost(int listingId, decimal dealerCost)
        {
            UnitOfWork.Inventory.UpdateDealerCost(listingId, dealerCost);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSoldDealerCost(int listingId, decimal dealerCost)
        {
            UnitOfWork.Inventory.UpdateSoldDealerCost(listingId, dealerCost);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSoldDealerCost(SoldoutInventory obj, decimal dealerCost)
        {
            obj.DealerCost = dealerCost;
            obj.LastUpdated = DateTime.Now;
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateAcv(int listingId, decimal acv)
        {
              UnitOfWork.Inventory.UpdateAcv(listingId, acv);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSoldAcv(int listingId, decimal acv)
        {
            UnitOfWork.Inventory.UpdateSoldAcv(listingId, acv);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSalePrice(int listingId, decimal saleprice)
        {

            UnitOfWork.Inventory.UpdateSalePrice(listingId, saleprice);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSoldSalePrice(int listingId, decimal saleprice)
        {
            UnitOfWork.Inventory.UpdateSoldSalePrice(listingId, saleprice);
            UnitOfWork.CommitVincontrolModel();
        }

        public SilentSalesmanViewModel GetSilentSalesman(int listingId, int dealerId)
        {
            var existingItem = UnitOfWork.Inventory.GetSilentSalesman(listingId, dealerId);
            return existingItem != null ? new SilentSalesmanViewModel(existingItem) : null;
        }

        public void NewSilentSalesman(int listingId, int dealerId, string title, string engine, string additionalOptions, string otherOptions, int userId)
        {
            var existingItem = UnitOfWork.Inventory.GetSilentSalesman(listingId, dealerId);
            if (existingItem == null)
            {
                var newItem = new SilentSalesman()
                {
                    InventoryId = listingId,
                    DealerId = dealerId,
                    Title = title,
                    Engine = engine,
                    AdditionalOptions = additionalOptions,
                    OtherOptions = otherOptions,
                    UserStamp = userId,
                    DateStamp = DateUtilities.Now()
                };
                UnitOfWork.Inventory.NewSilentSalesman(newItem);
                
            }
            else
            {
                existingItem.Title = title;
                existingItem.Engine = engine;
                existingItem.AdditionalOptions = additionalOptions;
                existingItem.OtherOptions = otherOptions;
                existingItem.DateStamp = DateUtilities.Now();
                existingItem.UserStamp = userId;
            }
            UnitOfWork.CommitVincontrolModel();
        }

        public string GetStausCodeName(int statusCode)
        {
            return UnitOfWork.Inventory.GetStausCodeName(statusCode);
        }

        public void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims, bool? isCertified, string isAll,
            string isFranchise, string isIndependant)
        {
            UnitOfWork.Inventory.UpdateChartSelection(listingId,isCarsCom,options,trims,isCertified,isAll,isFranchise,isIndependant);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateSmallChartSelection(int listingId, string trims)
        {
            UnitOfWork.Inventory.UpdateSmallChartSelection(listingId,  trims);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateCustomerInfoForSold(int listingId, string firstName, string lastName, string address,
            string street,
            string city, string state, string zipcode)
        {
            UnitOfWork.Inventory.UpdateCustomerInfoForSold(listingId, firstName, lastName, address, street,
                city, state, zipcode);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateMarketRange(int listingId, int marketRange)
        {
            UnitOfWork.Inventory.UpdateMarketRange(listingId,marketRange);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateAutoDescriptionStatus(int listingId, bool status)
        {
            UnitOfWork.Inventory.UpdateAutoDescriptionStatus(listingId, status);
            UnitOfWork.CommitVincontrolModel();
        }

        public IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(int dealerId, bool? condition, DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.Inventory.GetSoldInventoriesInDateRange(dealerId, condition, startDate, endDate);
        }

        public IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(IEnumerable<int> dealerList, bool? condition, DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.Inventory.GetSoldInventoriesInDateRange(dealerList, condition, startDate, endDate);
        }

        public IQueryable<Inventory> GetAllUsedInventories(int dealerId)
        {
            return UnitOfWork.Inventory.GetAllUsedInventories(dealerId);
        }

        public IQueryable<Inventory> GetAllUsedInventories(IEnumerable<int> dealerList)
        {
            return UnitOfWork.Inventory.GetAllUsedInventories(dealerList);
        }

        public IQueryable<Inventory> GetAllNewInventories(int dealerId)
        {
            return UnitOfWork.Inventory.GetNewInventories(dealerId);
        }

        public IQueryable<Inventory> GetAllNewInventories(IEnumerable<int> dealerList)
        {
            return UnitOfWork.Inventory.GetNewInventories(dealerList);
        }

        public IEnumerable<Inventory> GetTodayBucketJumpInventories(int dealerId)
        {
            return UnitOfWork.Inventory.GetTodayBucketJumpInventories(dealerId);
        }

        public IEnumerable<Inventory> FilterTodayBucketJumpInventories(IQueryable<Inventory> usedInventories, int dealerId)
        {
            var todayBucketJumpList = new List<Inventory>();
            var dealerSetting = UnitOfWork.Dealer.GetDealerSettingById(dealerId);
            foreach (var tmp in usedInventories)
            {
                var dateatMidnight = new DateTime(tmp.DateInStock.GetValueOrDefault().Year,
                    tmp.DateInStock.GetValueOrDefault().Month,
                    tmp.DateInStock.GetValueOrDefault().Day);

                int daysInInventory = DateTime.Now.Subtract(dateatMidnight).Days;
                int bucketDay = tmp.BucketJumpCompleteDay.GetValueOrDefault();
                int nextBucketDay = 0;
                if (bucketDay == 0 || bucketDay < dealerSetting.FirstTimeRangeBucketJump.GetValueOrDefault())
                    nextBucketDay = dealerSetting.FirstTimeRangeBucketJump.GetValueOrDefault();
                else if (bucketDay < dealerSetting.SecondTimeRangeBucketJump.GetValueOrDefault())
                    nextBucketDay = dealerSetting.SecondTimeRangeBucketJump.GetValueOrDefault();
                else if (bucketDay >= dealerSetting.SecondTimeRangeBucketJump.GetValueOrDefault())
                    nextBucketDay = bucketDay + dealerSetting.IntervalBucketJump.GetValueOrDefault();

                bool flag = nextBucketDay <= daysInInventory;

                if (flag)
                {
                    todayBucketJumpList.Add(tmp);

                }

            }
            return todayBucketJumpList;
        }

        public BucketJumpHistory GetLatestBucketJumpHistory(int listingId, short vehicleStatusCode)
        {
            return UnitOfWork.Inventory.GetLatestBucketJumpHistory(listingId, vehicleStatusCode);
        }

        public IQueryable<BucketJumpHistory> GetBucketJumpHistory(int listingId, short vehicleStatusCode)
        {
            return UnitOfWork.Inventory.GetBucketJumpHistory(listingId, vehicleStatusCode);
        }

        public List<BucketJumpHistoryViewModel> GetDailyBucketJumpHistoryBySingleStore(int dealerId, short vehicleStatusCode = 0)
        {
            var list = UnitOfWork.Inventory.GetDailyBucketJumpHistoryBySingleStore(dealerId, vehicleStatusCode);
            return list.Any()
                ? list.AsEnumerable().Select(i => new BucketJumpHistoryViewModel(i)).ToList()
                : new List<BucketJumpHistoryViewModel>();
        }

        public List<BucketJumpHistoryViewModel> GetDailyBucketJumpHistoryByAllStore(string groupId, short vehicleStatusCode = 0)
        {
            var list = UnitOfWork.Inventory.GetDailyBucketJumpHistoryByAllStore(groupId, vehicleStatusCode);
            return list.Any()
                ? list.AsEnumerable().Select(i => new BucketJumpHistoryViewModel(i)).ToList()
                : new List<BucketJumpHistoryViewModel>();
        }

        public EmailWaitingList GetEmailWaitingList(int emailNotificationId)
        {
            return UnitOfWork.Inventory.GetEmailWaitingList(emailNotificationId);
        }

        public void UpdateRebateInfo(int year, string make, string model, string trim, int dealerId, int rebateAmount,
            string disclaimer, DateTime expirationDate)
        {
            UnitOfWork.Inventory.UpdateRebateInfo(year, make, model, trim, dealerId, rebateAmount, disclaimer, expirationDate);
            UnitOfWork.CommitVincontrolModel();
        }

        public MassBucketJump GetMassBucketJumpByInventoryId(int inventoryId)
        {
            return UnitOfWork.Inventory.GetMassBucketJump(inventoryId);
        }

        public bool CheckMassBucketJumpExisting(int inventoryId)
        {
            return UnitOfWork.Inventory.GetMassBucketJump(inventoryId) != null;
        }

        public void UpdateMassBucketJumpCertified(int inventoryId, bool certified, decimal amount)
        {
            var existingRecord = UnitOfWork.Inventory.GetMassBucketJump(inventoryId);
            if (existingRecord != null)
            {
                existingRecord.Certified = certified;
                existingRecord.CertifiedAmount = certified ? amount : (decimal?) null;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateCertifiedAmount(int inventoryId, decimal amount)
        {
            var existingRecord = UnitOfWork.Inventory.GetInventory(inventoryId);
            if (existingRecord != null)
            {
                existingRecord.CertifiedAmount = amount;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateMassBucketJumpACar(int inventoryId, bool acar, decimal amount)
        {
            var existingRecord = UnitOfWork.Inventory.GetMassBucketJump(inventoryId);
            if (existingRecord != null)
            {
                existingRecord.ACar = acar;
                existingRecord.ACarAmount = acar ? amount : (decimal?)null;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateMileageAdjustment(int inventoryId, decimal amount)
        {
            var existingRecord = UnitOfWork.Inventory.GetInventory(inventoryId);
            if (existingRecord != null)
            {
                existingRecord.MileageAdjustment = amount;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateNote(int inventoryId, string note)
        {
            var existingRecord = UnitOfWork.Inventory.GetInventory(inventoryId);
            if (existingRecord != null)
            {
                existingRecord.Note = note;
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdatePhoto(int inventoryId, string normalUrl, string thumbnailUrl)
        {
            var row = GetInventory(inventoryId);
            if (row != null)
            {
                if (String.IsNullOrEmpty(row.PhotoUrl))
                    row.PhotoUrl = normalUrl;
                else
                {
                    string[] carImagesList = row.PhotoUrl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (carImagesList.First().Contains(ConfigurationManagement.ConfigurationHandler.WebServerUrl))
                    {
                        if (carImagesList.First().Contains("DefaultStockImage"))
                            row.PhotoUrl = normalUrl;
                        else
                            row.PhotoUrl = row.PhotoUrl + "," + normalUrl;
                    }
                    else
                    {
                        row.PhotoUrl = normalUrl;
                    }
                }

                if (String.IsNullOrEmpty(row.ThumbnailUrl))
                {
                    row.ThumbnailUrl = thumbnailUrl;
                }
                else
                {
                    string[] carImagesList = row.ThumbnailUrl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (carImagesList.First().Contains(ConfigurationManagement.ConfigurationHandler.WebServerUrl))
                    {
                        if (carImagesList.First().Contains("DefaultStockImage"))
                            row.ThumbnailUrl = thumbnailUrl;
                        else
                            row.ThumbnailUrl = row.ThumbnailUrl + "," + thumbnailUrl;
                    }
                    else
                    {
                        row.ThumbnailUrl = thumbnailUrl;
                    }

                }

                row.LastUpdated = DateTime.Now;

                UnitOfWork.CommitVincontrolModel();
            }
        }

        public MassBucketJumpAmounts AddMassBucketJump(int inventoryId, string dealer, string image, string vin, int year, string make, string model, string color,
            decimal price, decimal odometer, decimal plusPrice, decimal wholesaleWithOptions,
            decimal wholesaleWithoutOptions, string option = "")
        {
            var existingRecord = UnitOfWork.Inventory.GetMassBucketJump(inventoryId);
            if (existingRecord == null)
            {
                UnitOfWork.Inventory.AddMassBucketJump(inventoryId, dealer, image, vin, year, make, model, color, price,
                    odometer,
                    plusPrice, wholesaleWithOptions, wholesaleWithoutOptions, option);
                UnitOfWork.CommitVincontrolModel();
                return new MassBucketJumpAmounts();
            }
            else
            {
                existingRecord.MarketDealer = dealer;
                existingRecord.MarketDealerImage = image;
                existingRecord.MarketVIN = vin;
                existingRecord.MarketYear = year;
                existingRecord.MarketMake = make;
                existingRecord.MarketModel = model;
                existingRecord.MarketPrice = price;
                existingRecord.MarketPlusPrice = plusPrice;
                existingRecord.MarketOdometer = odometer;
                existingRecord.MarketColor = color;
                existingRecord.WholesaleWithOptions = wholesaleWithOptions;
                existingRecord.WholesaleWithoutOptions = wholesaleWithoutOptions;
                existingRecord.MarketOption = option;
                existingRecord.ModifiedDate = DateTime.Now;
                UnitOfWork.CommitVincontrolModel();

                return new MassBucketJumpAmounts()
                {
                    Certified =
                        existingRecord.Certified.GetValueOrDefault()
                            ? existingRecord.CertifiedAmount.GetValueOrDefault()
                            : 0,
                    ACar = existingRecord.ACar.GetValueOrDefault() ? existingRecord.ACarAmount.GetValueOrDefault() : 0
                };
            }
        }

        #endregion
    }

    public class MassBucketJumpAmounts
    {
        public decimal Certified { get; set; }
        public decimal ACar { get; set; }
    }
}
