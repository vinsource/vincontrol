using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using ChartSelection = vincontrol.Data.Model.ChartSelection;

namespace vincontrol.Data.Repository.Interface
{
    public interface IInventoryRepository
    {
        SoldoutInventory GetSoldoutInventory(int listingId, int dealerId);
        SoldoutInventory GetSoldoutInventory(int listingId);
        Inventory GetInventory(int listingId);
        Inventory GetInventory(int listingId, int dealerId);
        Inventory GetInventory(string vin, int dealerId);
        Inventory GetInventory(int year, string make, string model, string trim, int dealerId);
        ChartSelection GetChartSelection(int listingId);
        void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims,
                                     bool? isCertified, string isAll, string isFranchise, string isIndependant);
        void UpdateSmallChartSelection(int listingId, string trims);
        IQueryable<Inventory> GetAllInventories(int dealerId);

        IQueryable<Inventory> GetInventories(int vehicleId);
        IQueryable<Inventory> GetNewInventories(int dealerId);
        IQueryable<Inventory> GetNewInventories(IEnumerable<int> dealerList);
        IQueryable<Inventory> GetUsedInventories(int dealerId);
        IQueryable<Inventory> GetAllUsedInventories(int dealerId);
        IQueryable<Inventory> GetUsedInventories(int dealerId, int pageIndex, int pageSize);
        IQueryable<Inventory> GetUsedInventories(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock);
        IQueryable<Inventory> GetUsedInventoriesIncludeSoldOut(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock);
        IQueryable<Inventory> GetReconInventories(int dealerId);
        IQueryable<Inventory> GetFeaturedInventories(int dealerId);
        IQueryable<Inventory> GetSimilarUsedInventories(int dealerId, string vin, string make, string model);
        IQueryable<Inventory> GetSimilarUsedInventories(string groupId, int excluedListingId, int year, string make, string model, string trim);

        IQueryable<Inventory> GetSimilarMake(int dealerId, string vin, string make, int number);
        IQueryable<Inventory> GetSimilarModel(int dealerId, string vin, string model, int number);
        IQueryable<Inventory> GetSimilarBodyType(int dealerId, string vin, string body, int number);

        IQueryable<SoldoutInventory> GetAllSoldInventories(int dealerId);
        IQueryable<SoldoutInventory> GetNewSoldInventories(int dealerId);
        IQueryable<SoldoutInventory> GetUsedSoldInventories(int dealerId);
        IQueryable<SoldoutInventory> GetUsedSoldInventories(int dealerId, string soldoutAction);

        List<SelectListItem> GetUsedYears(int dealerId);
        List<SelectListItem> GetUsedMakes(int dealerId);
        List<SelectListItem> GetUsedModels(int dealerId);

        SilentSalesman GetSilentSalesman(int listingId, int dealerId);
        void AddPriceChangeHistory(PriceChangeHistory priceChangeHistory);
        void UpdateIsFeatured(int listingId, bool isFeatured);
        void UpdateIsFeaturedForSoldCar(int listingId, bool isFeatured);
        void UpdateWarrantyInfo(int warrantyInfo, int listingId);
        void UpdatePriorRental(bool priorRental, int listingId);
        void UpdateDealerDemo(bool dealerDemo, int listingId);
        void UpdateUnwind(bool unwind, int listingId);
        void UpdateDescription(int listingId, string description);
        void UpdateMileage(int listingId, long? mileage);
        void UpdateSoldMileage(int listingId, long? mileage);
        void UpdateDealerCost(int listingId, decimal dealerCost);
        void UpdateSoldDealerCost(int listingId, decimal dealerCost);
        void UpdateAcv(int listingId, decimal acv);
        void UpdateSoldAcv(int listingId, decimal acv);
        void UpdateSalePrice(int listingId, decimal saleprice);
        void UpdateSoldSalePrice(int listingId, decimal saleprice);
        void UpdateStatus(int listingId, short status);
        void NewSilentSalesman(SilentSalesman obj);
        void ResetKbbTrim(int listingId, int type);
        void ResetManheimTrim(int listingId, int type);
        void UpdateMarketRange(int listingId, int marketRange);
        void UpdateAutoDescriptionStatus(int listingId, bool status);
        string GetStausCodeName(int statusCode);
        void UpdateCustomerInfoForSold(int listingId, string firstName, string lastName, string address, string street,
          string city, string state, string zipcode);

        IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(int dealerId, bool? condition, DateTime startDate,
            DateTime endDate);

        IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(IEnumerable<int> dealerList, bool? condition, DateTime startDate,
           DateTime endDate);
        IEnumerable<Inventory> GetTodayBucketJumpInventories(int dealerId);
        IQueryable<BucketJumpHistory> GetBucketJumpHistory(int listingId,short vehicleStatusCode = 0);
        IQueryable<BucketJumpHistory> GetDailyBucketJumpHistoryBySingleStore(int dealerId, short vehicleStatusCode = 0);
        IQueryable<BucketJumpHistory> GetDailyBucketJumpHistoryByAllStore(string groupId, short vehicleStatusCode = 0);
        EmailWaitingList GetEmailWaitingList(int emailNotificationId);
        IQueryable<Inventory> GetAllUsedInventories(IEnumerable<int> dealerList);
        void UpdateRebateInfo(int year, string make, string model, string trim, int dealerId, int rebateAmount, string disclaimer, DateTime expirationDate);
        BucketJumpHistory GetLatestBucketJumpHistory(int listingId, short vehicleStatusCode);
        MassBucketJump GetMassBucketJump(int inventoryId);
        void AddMassBucketJump(int inventoryId, string dealer, string image, string vin, int year, string make, string model, string color,
            decimal price, decimal odometer, decimal plusPrice, decimal wholesaleWithOptions,
            decimal wholesaleWithoutOptions, string option = "");
    }
}
