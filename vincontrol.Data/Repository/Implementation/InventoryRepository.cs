using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;
using vincontrol.DomainObject;
using ChartSelection = vincontrol.Data.Model.ChartSelection;

namespace vincontrol.Data.Repository.Implementation
{
    public class InventoryRepository : IInventoryRepository
    {
        private VincontrolEntities _context;
        private IAdminRepository _adminRepository;
        private IDealerRepository _dealerRepository;

        public InventoryRepository(VincontrolEntities context)
        {
            _context = context;
            _adminRepository = new AdminRepository(context);
            _dealerRepository=new DealerRepository(context);
        }

        #region IInventoryRepository's Members

        public SoldoutInventory GetSoldoutInventory(int listingId, int dealerId)
        {
            return _context.SoldoutInventories.FirstOrDefault(i => i.SoldoutInventoryId == listingId && i.DealerId == dealerId);
        }

        public SoldoutInventory GetSoldoutInventory(int listingId)
        {
            return _context.SoldoutInventories.FirstOrDefault(i => i.SoldoutInventoryId == listingId );
        }
        public Inventory GetInventory(int listingId)
        {
            return _context.Inventories.FirstOrDefault(i => i.InventoryId == listingId);
        }

        public Inventory GetInventory(int listingId, int dealerId)
        {
            return _context.Inventories.FirstOrDefault(i => i.InventoryId == listingId && i.DealerId == dealerId);
        }

        public Inventory GetInventory(string vin, int dealerId)
        {
            return _context.Inventories.FirstOrDefault(i => i.Vehicle.Vin.ToLower().Equals(vin.ToLower()) && i.DealerId == dealerId);
        }

        public Inventory GetInventory(int year, string make, string model, string trim, int dealerId)
        {
            return
                _context.Inventories.FirstOrDefault(
                    i =>
                    i.Vehicle.Year == year && i.Vehicle.Make.ToLower().Equals(make.ToLower()) &&
                    i.Vehicle.Model.ToLower().Equals(model.ToLower()) && i.Vehicle.Trim.ToLower().Equals(trim.ToLower()) &&
                    i.DealerId == dealerId);
        }

        public ChartSelection GetChartSelection(int listingId)
        {
            return _context.ChartSelections.FirstOrDefault(i => i.ListingId == listingId && i.VehicleStatusCodeId==Constanst.VehicleStatus.Inventory);
        }

        public void UpdateChartSelection(int listingId, string isCarsCom, string options, string trims,
            bool? isCertified, string isAll,
            string isFranchise, string isIndependant)
        {
            var existingChartSelection =
                _context.ChartSelections.FirstOrDefault(
                    s => s.ListingId == listingId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory);
            if (existingChartSelection != null)
            {
                existingChartSelection.IsAll = Convert.ToBoolean(isAll);
                existingChartSelection.IsCarsCom = Convert.ToBoolean(isCarsCom);
                existingChartSelection.IsCertified = isCertified;
                existingChartSelection.IsFranchise = Convert.ToBoolean(isFranchise);
                existingChartSelection.IsIndependant = Convert.ToBoolean(isIndependant);
                existingChartSelection.Options = options.IndexOf(',') > 0
                    ? (options.Split(',')[0].Equals("0") ? "0" : options)
                    : options.ToLower();
                existingChartSelection.Trims = trims.IndexOf(',') > 0
                    ? (trims.Split(',')[0].Equals("0") ? null : trims)
                    : trims.Equals("null") ? null : trims.ToLower();


            }
            else
            {
                var newSelection = new ChartSelection
                {
                    ListingId = Convert.ToInt32(listingId),
                    IsAll = Convert.ToBoolean(isAll),
                    IsCarsCom = Convert.ToBoolean(isCarsCom),
                    IsCertified = isCertified,
                    IsFranchise = Convert.ToBoolean(isFranchise),
                    IsIndependant = Convert.ToBoolean(isIndependant),
                    Options =
                        options.IndexOf(',') > 0
                            ? (options.Split(',')[0].Equals("0") ? "0" : options)
                            : options,
                    Trims =
                        trims.IndexOf(',') > 0
                            ? (trims.Split(',')[0].Equals("0") ? null : trims)
                            : trims.Equals("null") ? null : trims.ToLower(),
                    VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,

                };
                _context.AddToChartSelections(newSelection);

            }
        }

        public void UpdateSmallChartSelection(int listingId, string trims)
        {
            var existingChartSelection = _context.ChartSelections.FirstOrDefault(s => s.ListingId == listingId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory);
            if (existingChartSelection != null)
            {
                existingChartSelection.Trims = trims.IndexOf(',') > 0
                                                   ? (trims.Split(',')[0].Equals("0") ? "0" : trims)
                                                   : trims.Equals("null") ? null : trims.ToLower();

                _context.SaveChanges();
            }
            else
            {
                var newSelection = new ChartSelection
                {
                    ListingId = Convert.ToInt32(listingId),
                    Trims =
                        trims.IndexOf(',') > 0
                            ? (trims.Split(',')[0].Equals("0") ? "0" : trims)
                            : trims.Equals("null") ? null : trims.ToLower(),
                    VehicleStatusCodeId = Constanst.VehicleStatus.Inventory
                };
                _context.AddToChartSelections(newSelection);
               
            }
        }


        public IQueryable<Inventory> GetAllInventories(int dealerId)
        {
            return InventoryQuery(dealerId);
        }

        public IQueryable<Inventory> GetInventories(int vehicleId)
        {
            return _context.Inventories.Where(i => i.VehicleId == vehicleId);
        }

        public IQueryable<Inventory> GetNewInventories(int dealerId)
        {
            return _context.Inventories.Where(i => i.DealerId == dealerId && i.Condition == Constanst.ConditionStatus.New);
        }

        public IQueryable<Inventory> GetNewInventories(IEnumerable<int> dealerList)
        {
            return
                _context.Inventories.Where(
                    LinqExtendedHelper.BuildContainsExpression<Inventory, int>(e => e.DealerId, dealerList));

        }

        public IQueryable<Inventory> GetUsedInventories(int dealerId)
        {
            return InventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.Used).OrderByDescending(i => i.SalePrice).ThenBy(i => i.Vehicle.Year);
        }

        public IQueryable<Inventory> GetAllUsedInventories(int dealerId)
        {
            return _context.Inventories.Where(i => i.DealerId == dealerId && i.Condition == Constanst.ConditionStatus.Used);
        }

        public IQueryable<Inventory> GetUsedInventories(int dealerId, int pageIndex, int pageSize)
        {
            return InventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.Used).OrderByDescending(i => i.SalePrice).ThenBy(i => i.Vehicle.Year).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Inventory> GetUsedInventories(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock)
        {
            return InventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.Used 
                                                       && (year.Equals(0) || (i.Vehicle.Year != null && i.Vehicle.Year.Value.Equals(year))) 
                                                       && (string.IsNullOrEmpty(make) || i.Vehicle.Make.Equals(make))
                                                       && (string.IsNullOrEmpty(model) || i.Vehicle.Model.Equals(model))
                                                       && (string.IsNullOrEmpty(stock) || i.Stock.Contains(stock)))
                .OrderByDescending(i => i.SalePrice)
                .ThenBy(i => i.Vehicle.Year)
                .Skip((pageIndex - 1)*pageSize)
                .Take(pageSize);
        }

        public IQueryable<Inventory> GetUsedInventoriesIncludeSoldOut(int dealerId, int pageIndex, int pageSize, int year, string make, string model, string stock)
        {
            var usedInventories = GetUsedInventories(dealerId).AsEnumerable().Select(i => new Inventory(i));
            var dealerSetting = _adminRepository.GetSetting(dealerId);
            var usedSoldOutInventories = GetUsedSoldInventories(dealerId, dealerSetting.SoldOut).AsEnumerable().Select(i => new Inventory(i, Constanst.VehicleStatus.SoldOut));
            var list = usedInventories.Concat(usedSoldOutInventories);
            return list.Where(i => (year.Equals(0) || (i.Vehicle.Year != null && i.Vehicle.Year.Value.Equals(year)))
                                   && (string.IsNullOrEmpty(make) || i.Vehicle.Make.Equals(make))
                                   && (string.IsNullOrEmpty(model) || i.Vehicle.Model.Equals(model))
                                   && (string.IsNullOrEmpty(stock) || i.Stock.Contains(stock)))
                .OrderByDescending(i => i.SalePrice)
                .ThenBy(i => i.Vehicle.Year)
                .Skip((pageIndex - 1)*pageSize)
                .Take(pageSize).AsQueryable();
        }

        public IQueryable<Inventory> GetSimilarUsedInventories(int dealerId, string vin, string make, string model)
        {
            return InventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.Used && !i.Vehicle.Vin.Equals(vin) && i.Vehicle.Make.Equals(make) && i.Vehicle.Model.Equals(model)).OrderByDescending(i => i.SalePrice).ThenBy(i => i.Vehicle.Year).Take(4);
        }

        public IQueryable<Inventory> GetSimilarUsedInventories(string groupId, int excluedListingId, int year, string make, string model, string trim)
        {
            var query = _context.Inventories.Where(i => i.Dealer.DealerGroupId.Equals(groupId) && (i.InventoryStatusCodeId != Constanst.InventoryStatus.Recon && i.InventoryStatusCodeId != Constanst.InventoryStatus.Wholesale));
            query = query.Where(i => i.Condition == Constanst.ConditionStatus.Used && !i.InventoryId.Equals(excluedListingId) && i.Vehicle.Year.Value.Equals(year) && i.Vehicle.Make.Equals(make) && i.Vehicle.Model.Equals(model)).OrderByDescending(i => i.Mileage);
            return query;
        }

        public IQueryable<Inventory> GetSimilarMake(int dealerId, string vin, string make, int number)
        {
            
            var query =
                InventoryQuery(dealerId)
                    .Where(
                        i =>
                            i.Condition == Constanst.ConditionStatus.Used && !i.Vehicle.Vin.Equals(vin) &&
                            i.Vehicle.Make.Equals(make))
                    .OrderByDescending(i => i.SalePrice)
                    .ThenBy(i => i.Vehicle.Year);
            var rownum = 0;
            if (query.Count() > 1)
            {
                rownum = new Random().Next(query.Count() - 1);
            }
            
            return query.Skip(rownum).Take(number);
        }

        public IQueryable<Inventory> GetSimilarModel(int dealerId, string vin, string model, int number)
        {

            var query =
                InventoryQuery(dealerId)
                    .Where(
                        i =>
                            i.Condition == Constanst.ConditionStatus.Used && !i.Vehicle.Vin.Equals(vin) &&
                            i.Vehicle.Model.Equals(model))
                    .OrderByDescending(i => i.SalePrice)
                    .ThenBy(i => i.Vehicle.Year);

            var rownum = 0;
            if (query.Count() > 1)
            {
                rownum = new Random().Next(query.Count() - 1);
            }
            return query.Skip(rownum).Take(number);
        }

        public IQueryable<Inventory> GetSimilarBodyType(int dealerId, string vin, string body, int number)
        {

            var query =
                InventoryQuery(dealerId)
                    .Where(
                        i =>
                            i.Condition == Constanst.ConditionStatus.Used && !i.Vehicle.Vin.Equals(vin) &&
                            i.Vehicle.BodyType.Equals(body))
                    .OrderByDescending(i => i.SalePrice)
                    .ThenBy(i => i.Vehicle.Year);

            var rownum = 0;
            if (query.Count() > 1)
            {
                rownum = new Random().Next(query.Count() - 1);
            }
            return query.Skip(rownum).Take(number);
        }

        public IQueryable<Inventory> GetReconInventories(int dealerId)
        {
            return InventoryQuery(dealerId, true);
        }

        public IQueryable<Inventory> GetFeaturedInventories(int dealerId)
        {
            return
                InventoryQuery(dealerId)
                    .Where(i => i.IsFeatured.HasValue && i.IsFeatured.Value)
                    .OrderByDescending(i => i.SalePrice)
                    .ThenBy(i => i.Vehicle.Year); //.Take(8);
        }

        public IQueryable<SoldoutInventory> GetAllSoldInventories(int dealerId)
        {
            return SoldInventoryQuery(dealerId);
        }

        public IQueryable<SoldoutInventory> GetNewSoldInventories(int dealerId)
        {
            return SoldInventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.New);
        }

        public IQueryable<SoldoutInventory> GetUsedSoldInventories(int dealerId)
        {
            return SoldInventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.Used);
        }

        public IQueryable<SoldoutInventory> GetUsedSoldInventories(int dealerId, string soldoutAction)
        {
            var dateToCompare = DateTime.Now.AddDays(-30);
            switch (soldoutAction)
            {
                case Constanst.SoldOutAction._3Days: dateToCompare = DateTime.Now.AddDays(-3); break;
                case Constanst.SoldOutAction._5Days: dateToCompare = DateTime.Now.AddDays(-5); break;
                case Constanst.SoldOutAction._7Days: dateToCompare = DateTime.Now.AddDays(-7); break;
            }
            return SoldInventoryQuery(dealerId).Where(i => i.Condition == Constanst.ConditionStatus.Used && (soldoutAction.Equals(Constanst.SoldOutAction._Delete) || i.DateRemoved >= dateToCompare));
        }

        public List<SelectListItem> GetUsedYears(int dealerId)
        {
            return GetUsedInventories(dealerId).OrderByDescending(i => i.Vehicle.Year).Select(i => SqlFunctions.StringConvert((double)i.Vehicle.Year)).Distinct().Select(i => new SelectListItem() { Text = i, Value = i }).ToList();
        }

        public List<SelectListItem> GetUsedMakes(int dealerId)
        {
            return GetUsedInventories(dealerId).OrderBy(i => i.Vehicle.Make).Select(i => i.Vehicle.Make).Distinct().Select(i => new SelectListItem() { Text = i, Value = i }).ToList();
        }

        public List<SelectListItem> GetUsedModels(int dealerId)
        {
            return GetUsedInventories(dealerId).OrderBy(i => i.Vehicle.Model).Select(i => i.Vehicle.Model).Distinct().Select(i => new SelectListItem() { Text = i, Value = i }).ToList();
        }

        public SilentSalesman GetSilentSalesman(int listingId, int dealerId)
        {
            return _context.SilentSalesmen.FirstOrDefault(i => i.InventoryId.Equals(listingId) && i.DealerId.Equals(dealerId));
        }

        public void AddPriceChangeHistory(PriceChangeHistory priceChangeHistory)
        {
            _context.PriceChangeHistories.AddObject(priceChangeHistory);
        }

        public void UpdateIsFeatured(int listingId, bool isFeatured)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.IsFeatured = isFeatured;
                row.LastUpdated = DateTime.Now;
            }
        }

        public void UpdateIsFeaturedForSoldCar(int listingId, bool isFeatured)
        {

            var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

            if (row != null)
            {
                row.IsFeatured = isFeatured;
                row.LastUpdated = DateTime.Now;
            }
        }

        public void UpdateWarrantyInfo(int warrantyInfo, int listingId)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.WarrantyInfo = warrantyInfo;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdatePriorRental(bool priorRental, int listingId)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.PriorRental = priorRental;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateDealerDemo(bool dealerDemo, int listingId)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.DealerDemo = dealerDemo;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateUnwind(bool unwind, int listingId)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.Unwind = unwind;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateDescription(int listingId, string description)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.Descriptions = description;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateMileage(int listingId, long? mileage)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.Mileage = mileage;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateSoldMileage(int listingId, long? mileage)
        {
            var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

            if (row != null)
            {
                row.Mileage = mileage;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateDealerCost(int listingId, decimal dealerCost)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.DealerCost = dealerCost;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateSoldDealerCost(int listingId, decimal dealerCost)
        {
            var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

            if (row != null)
            {
                row.DealerCost = dealerCost;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateAcv(int listingId, decimal acv)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.ACV = acv;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateSoldAcv(int listingId, decimal acv)
        {

            var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

            if (row != null)
            {
                row.ACV = acv;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateSalePrice(int listingId, decimal saleprice)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.SalePrice = saleprice;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateSoldSalePrice(int listingId, decimal saleprice)
        {
            var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

            if (row != null)
            {
                row.SalePrice = saleprice;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateStatus(int listingId, short status)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.InventoryStatusCodeId = status;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void NewSilentSalesman(SilentSalesman obj)
        {
            _context.SilentSalesmen.AddObject(obj);
        }

        public void ResetKbbTrim(int listingId, int type)
        {
            if (type == Constanst.VehicleStatus.Inventory)
            {
                var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

                if (row != null)
                {
                    row.Vehicle.KBBTrimId = null;

                }
            }
            if (type == Constanst.VehicleStatus.Appraisal)
            {
                var row = _context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);

                if (row != null)
                {
                    row.Vehicle.KBBTrimId = null;

                }
            }
            if (type == Constanst.VehicleStatus.SoldOut)
            {
                var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                if (row != null)
                {
                    row.Vehicle.KBBTrimId = null;

                }
            }
        }

        public void ResetManheimTrim(int listingId, int type)
        {
            if (type == Constanst.VehicleStatus.Inventory)
            {
                var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

                if (row != null)
                {
                    row.Vehicle.ManheimTrimId = null;

                }
            }
            if (type == Constanst.VehicleStatus.Appraisal)
            {
                var row = _context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);

                if (row != null)
                {
                    row.Vehicle.ManheimTrimId = null;

                }
            }
            if (type == Constanst.VehicleStatus.SoldOut)
            {
                var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                if (row != null)
                {
                    row.Vehicle.ManheimTrimId = null;

                }
            }
        }

        public void UpdateMarketRange(int listingId, int marketRange)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
             
            if (row != null)
            {
                row.MarketRange = marketRange;
                row.LastUpdated = DateTime.Now;

            }
        }

        public void UpdateAutoDescriptionStatus(int listingId, bool status)
        {
            var row = _context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);

            if (row != null)
            {
                row.EnableAutoDescription = status;
                row.LastUpdated = DateTime.Now;

            }
        }

        public string GetStausCodeName(int statusCode)
        {
            var searchRecord = _context.InventoryStatusCodes.FirstOrDefault(x => x.InventoryStatusCodeId == statusCode);
            if (searchRecord != null)
                return searchRecord.Value;
            return String.Empty;
        }

        public void UpdateCustomerInfoForSold(int listingId, string firstName, string lastName, string address, string street,
            string city, string state, string zipcode)
        {
            var row = _context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

            if (row != null)
            {
                row.FirstName = firstName;
                row.LastName = lastName;
                row.Address = address;
                row.Street = street;
                row.City = city;
                row.State = state;
                row.ZipCode = zipcode;
                row.LastUpdated = DateTime.Now;

            }
        }

        public IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(int dealerId, bool? condition, DateTime startDate, DateTime endDate)
        {
            if (condition == null)
            {
                return
          _context.SoldoutInventories.Where(
              x =>
                  x.DealerId == dealerId &&x.DateRemoved.Value >= startDate && x.DateRemoved.Value <= endDate);
            }
            return
                _context.SoldoutInventories.Where(
                    x =>
                        x.DealerId == dealerId &&
                        x.Condition == condition &&
                        x.DateRemoved.Value >= startDate && x.DateRemoved.Value <= endDate);
        }

        public IQueryable<SoldoutInventory> GetSoldInventoriesInDateRange(IEnumerable<int> dealerList, bool? condition, DateTime startDate, DateTime endDate)
        {
            if (condition == null)
            {
                return
                    _context.SoldoutInventories.Where(
                        LinqExtendedHelper.BuildContainsExpression<SoldoutInventory, int>(e => e.DealerId, dealerList))
                        .Where(
                            x => x.DateRemoved.Value >= startDate && x.DateRemoved.Value <= endDate);
            }
            return
                _context.SoldoutInventories.Where(
                    LinqExtendedHelper.BuildContainsExpression<SoldoutInventory, int>(e => e.DealerId, dealerList))
                    .Where(
                        x => x.Condition == condition && x.DateRemoved.Value >= startDate && x.DateRemoved.Value <= endDate);
          
        }

        public IEnumerable<Inventory> GetTodayBucketJumpInventories(int dealerId)
        {
            var todayBucketJumpList = new List<Inventory>();
            var usedInventories = GetAllUsedInventories(dealerId);
            var dealerSetting = _dealerRepository.GetDealerSettingById(dealerId);
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

        public IQueryable<BucketJumpHistory> GetBucketJumpHistory(int listingId, short vehicleStatusCode)
        {
            return
                _context.BucketJumpHistories.Where(
                    i =>
                        i.ListingId == listingId &&
                        i.VehicleStatusCodeId == vehicleStatusCode)
                    .OrderByDescending(i => i.DateStamp);
            
        }

        public IQueryable<BucketJumpHistory> GetDailyBucketJumpHistoryBySingleStore(int dealerId, short vehicleStatusCode = 0)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var inventoryIds = _context.Inventories.Where(i => i.DealerId.Equals(dealerId)).Select(i => i.InventoryId);

            var bucketJumpHistory =
                _context.BucketJumpHistories.Where(
                    i => EntityFunctions.TruncateTime(i.DateStamp) == currentDate && i.DealerId == dealerId && i.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory)
                     .OrderByDescending(i => i.DateStamp.HasValue ? i.DateStamp.Value : new DateTime());
            
            return bucketJumpHistory;
        }

        public IQueryable<BucketJumpHistory> GetDailyBucketJumpHistoryByAllStore(string groupId, short vehicleStatusCode = 0)
        {
            var dealerIds = _context.Dealers.Where(i => i.DealerGroupId.Equals(groupId)).Select(i => i.DealerId);
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var inventoryIds = _context.Inventories.Where(i => dealerIds.Any(ii => ii == i.DealerId)).Select(i => i.InventoryId);

            var bucketJumpHistory =

                _context.BucketJumpHistories.Where(LinqExtendedHelper.BuildContainsExpression<BucketJumpHistory, int>(e => e.DealerId.Value, dealerIds))
                .Where(i => EntityFunctions.TruncateTime(i.DateStamp) == currentDate && i.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory)
                    .OrderByDescending(i => i.DateStamp.HasValue ? i.DateStamp.Value : new DateTime());
               
            return bucketJumpHistory;
        }

        public EmailWaitingList GetEmailWaitingList(int emailNotificationId)
        {
            return _context.EmailWaitingLists.FirstOrDefault(i => i.NotificationEmailId == emailNotificationId);
        }

        public IQueryable<Inventory> GetAllUsedInventories(IEnumerable<int> dealerList)
        {
            return
                _context.Inventories.Where(
                    LinqExtendedHelper.BuildContainsExpression<Inventory, int>(e => e.DealerId, dealerList));
                
        }

        public void UpdateRebateInfo(int year, string make, string model, string trim, int dealerId, int rebateAmount,
            string disclaimer, DateTime expirationDate)
        {
           var inventoryList= _context.Inventories.Where(i => i.DealerId == dealerId && i.Condition == Constanst.ConditionStatus.New
               &&i.Vehicle.Year==year && i.Vehicle.Make==make && i.Vehicle.Model==model && i.Vehicle.Trim==trim);
           foreach (var tmp in inventoryList)
           {
               tmp.ManufacturerRebate = rebateAmount;
               tmp.Disclaimer = disclaimer;
               tmp.WindowStickerPrice = tmp.RetailPrice - tmp.DealerDiscount - tmp.ManufacturerRebate;
               if (rebateAmount > 0)
               {
                   var vehiclelog = new VehicleLog()
                   {
                       DateStamp = DateTime.Now,
                       Description = Constanst.VehicleLogSentence.RebateApplied
                           .Replace("[rebate amount]", rebateAmount.ToString(CultureInfo.InvariantCulture))
                           .Replace("[sales price]", tmp.SalePrice.GetValueOrDefault().ToString(CultureInfo.InvariantCulture))
                           .Replace("[rebate expiration date]", expirationDate.ToShortDateString()),
                       InventoryId = tmp.InventoryId,
                       UserId = null
                   };
                   _context.AddToVehicleLogs(vehiclelog);
               }
          
           }
        }

        public BucketJumpHistory GetLatestBucketJumpHistory(int listingId, short vehicleStatusCode)
        {
            return
                _context.BucketJumpHistories.OrderByDescending(x=>x.DateStamp).FirstOrDefault(
                    i =>
                        i.ListingId == listingId &&
                        i.VehicleStatusCodeId == vehicleStatusCode);

        }

        public MassBucketJump GetMassBucketJump(int inventoryId)
        {
            return _context.MassBucketJumps.FirstOrDefault(i => i.InventoryId.Equals(inventoryId));
        }

        public void AddMassBucketJump(int inventoryId, string dealer, string image, string vin, int year, string make, string model, string color,
            decimal price, decimal odometer, decimal plusPrice, decimal wholesaleWithOptions,
            decimal wholesaleWithoutOptions, string option = "")
        {
            _context.MassBucketJumps.AddObject(new MassBucketJump()
            {
                InventoryId = inventoryId,
                MarketDealer = dealer,
                MarketDealerImage = image,
                MarketVIN = vin,
                MarketYear = year,
                MarketMake = make,
                MarketModel = model,
                MarketColor = color,
                MarketPrice = price,
                MarketOdometer = odometer,
                MarketPlusPrice = plusPrice,
                WholesaleWithOptions = wholesaleWithOptions,
                WholesaleWithoutOptions = wholesaleWithoutOptions,
                MarketOption = option,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });
        }

        #endregion

        #region Private Methods

        private IQueryable<Inventory> InventoryQuery(int dealerId)
        {
            return _context.Inventories.Where(i => i.DealerId == dealerId && (i.InventoryStatusCodeId != Constanst.InventoryStatus.Recon && i.InventoryStatusCodeId != Constanst.InventoryStatus.Wholesale));
        }

        private IQueryable<Inventory> InventoryQuery(int dealerId, bool isRecon)
        {
            return
                _context.Inventories.Where(
                    i =>
                    i.DealerId == dealerId &&
                    (!isRecon ? (i.InventoryStatusCodeId != Constanst.InventoryStatus.Recon) : (i.InventoryStatusCodeId == Constanst.InventoryStatus.Recon)));
        }

        private IQueryable<SoldoutInventory> SoldInventoryQuery(int dealerId)
        {
            return _context.SoldoutInventories.Where(i => i.DealerId == dealerId);
        }

        #endregion
    }
}
