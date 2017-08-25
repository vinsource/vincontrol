using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.InventoryManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using ChartSelection = vincontrol.Data.Model.ChartSelection;

namespace vincontrol.Application.Forms.InventoryManagement
{
    public interface IInventoryManagementForm
    {
        SoldoutInventory GetSoldInventory(int listingId);
        Inventory GetInventory(int listingId);
        ChartSelection GetChartSelection(int listingId);
        IList<CarInfoViewModel> GetUsedInventories(int dealerId);
        IList<CarInfoViewModel> GetUsedInventories(int dealerId, int pageIndex, int pageSize);
        IList<CarInfoViewModel> GetUsedInventories(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock);
        IList<CarInfoViewModel> GetUsedInventoriesIncludeSoldOut(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock);
        IList<CarInfoViewModel> GetSimilarUsedInventories(int dealerId, string vin, string make, string model);
        IList<CarInfoViewModel> GetSimilarUsedInventories(string groupId, int excludedListingId, int year, string make, string model, string trim);

        IList<CarInfoViewModel> GetSimilarBucketJumpInventories(string groupId, int excludedListingId, int year,
            string make, string model, string trim, int firstTimeRangeBucketJump, int secondTimeRangeBucketJump,
            int intervalBucketJump);

        IList<CarInfoViewModel> GetSimilarMake(int dealerId, string vin, string make);
        IList<CarInfoViewModel> GetSimilarModel(int dealerId, string vin, string model);
        IList<CarInfoViewModel> GetSimilarBodyType(int dealerId, string vin, string body);

        IList<CarInfoViewModel> GetNewInventories(int dealerId);
        IList<CarInfoViewModel> GetUsedSoldInventories(int dealerId);
        IList<CarShortViewModel> GetAllInventories(int dealerId);
        IList<CarShortViewModel> GetFeaturedInventories(int dealerId);
        CarInfoViewModel GetFullInventoryDetail(int dealerId, string vin);
        CarInfoViewModel GetFullInventoryDetail(int dealerId, int listingId);
        CarInfoViewModel GetFullSoldOutInventoryDetail(int dealerId, int listingId);
        CarShortViewModel GetCarInfo(int listingId);
        CarShortViewModel GetCarInfo(int dealerId, string vin);
        CarShortViewModel GetCarInfo(int dealerId, int listingId);
        CarShortViewModel GetSoldCarInfo(int listingId);
        List<SelectListItem> GetUsedYears(int dealerId);
        List<SelectListItem> GetUsedMakes(int dealerId);
        List<SelectListItem> GetUsedModels(int dealerId);
        IList<VehicleLogTracking> GetVehicleLogs(int listingId);
        IList<VehicleLogTracking> GetSoldVehicleLogs(int listingId);
        void AddPriceChangeHistory(PriceChangeHistory priceChangeHistory);
        void ResetKbbTrim(int listingId, int type);
        void ResetManheimTrim(int listingId, int type);
        void UpdateIsFeatured(int listingId, bool isFeatured);
        void UpdateIsFeaturedForSoldCar(int listingId, bool isFeatured);
        void UpdateWarrantyInfo(int warrantyInfo, int listingId);
        void UpdatePriorRental(bool priorRental, int listingId);
        void UpdateDealerDemo(bool dealerDemo, int listingId);
        void UpdateUnwind(bool unwind, int listingId);
        void UpdateDescription(int listingId, string description);
        void UpdateStatus(int listingId, short status);
        void UpdateMileage(int listingId, long? mileage);
        void UpdateSoldMileage(int listingId, long? mileage);
        void UpdateDealerCost(int listingId, decimal dealerCost);
        void UpdateSoldDealerCost(SoldoutInventory obj, decimal dealerCost);
        void UpdateSoldDealerCost(int listingId, decimal dealerCost);
        void UpdateAcv(int listingId, decimal acv);
        void UpdateSoldAcv(int listingId, decimal acv);
        void UpdateSalePrice(int listingId, decimal saleprice);
        void UpdateSoldSalePrice(int listingId, decimal saleprice);
        void UpdateCertifiedAmount(int inventoryId, decimal amount);
        void UpdateMileageAdjustment(int inventoryId, decimal amount);
        void UpdateNote(int inventoryId, string note);
        SilentSalesmanViewModel GetSilentSalesman(int listingId, int dealerId);
        void NewSilentSalesman(int listingId, int dealerId, string title, string engine, string additionalOptions, string otherOptions, int userId);
        string GetStausCodeName(int statusCode);
        void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims,
                                   bool? isCertified, string isAll, string isFranchise, string isIndependant);
        void UpdateSmallChartSelection(int listingId, string trims);

        void UpdateCustomerInfoForSold(int listingId, string firstName, string lastName, string address, string street,
            string city, string state, string zipcode);
        void UpdateMarketRange(int listingId, int marketRange);
        void UpdateAutoDescriptionStatus(int listingId, bool status);
        IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(int dealerId, bool? condition, DateTime startDate,
           DateTime endDate);

        IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(IEnumerable<int> dealerList, bool? condition, DateTime startDate,
           DateTime endDate);
        IQueryable<Inventory> GetAllUsedInventories(int dealerId);
        IQueryable<Inventory> GetAllUsedInventories(IEnumerable<int> dealerList);
        IQueryable<Inventory> GetAllNewInventories(int dealerId);
        IQueryable<Inventory> GetAllNewInventories(IEnumerable<int> dealerList);
        IEnumerable<Inventory> GetTodayBucketJumpInventories(int dealerId);
        IEnumerable<Inventory> FilterTodayBucketJumpInventories(IQueryable<Inventory> usedInventories, int dealerId);
        BucketJumpHistory GetLatestBucketJumpHistory(int listingId, short vehicleStatusCode);
        IQueryable<BucketJumpHistory> GetBucketJumpHistory(int listingId, short vehicleStatusCode);
        List<BucketJumpHistoryViewModel> GetDailyBucketJumpHistoryBySingleStore(int dealerId, short vehicleStatusCode = 0);
        List<BucketJumpHistoryViewModel> GetDailyBucketJumpHistoryByAllStore(string groupId, short vehicleStatusCode = 0);
        EmailWaitingList GetEmailWaitingList(int emailNotificationId);

        void UpdateRebateInfo(int year, string make, string model, string trim, int dealerId, int rebateAmount,
            string disclaimer, DateTime expirationDate);

        MassBucketJump GetMassBucketJumpByInventoryId(int inventoryId);
        bool CheckMassBucketJumpExisting(int inventoryId);
        void UpdateMassBucketJumpCertified(int inventoryId, bool certified, decimal amount);
        void UpdateMassBucketJumpACar(int inventoryId, bool acar, decimal amount);
        void UpdatePhoto(int inventoryId, string normalUrl, string thumbnailUrl);
        MassBucketJumpAmounts AddMassBucketJump(int inventoryId, string dealer, string image, string vin, int year, string make, string model, string color,
            decimal price, decimal odometer, decimal plusPrice, decimal wholesaleWithOptions,
            decimal wholesaleWithoutOptions, string option = "");
    }
}
