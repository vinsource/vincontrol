using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateSegmentInventoryDetailProcess : IUpdateSegmentInventoryDetailProcess
    {
        readonly IInventoryService _inventoryService;
        readonly ISettingService _settingService;
        readonly ISoldMarketVehicleService _soldMarketVehicleService;
        readonly IChromeService _chromeService;
        readonly IInventorySegmentDetailService _inventorySegmentDetailService;
     
        readonly IDealerBrandService _dealerBrandService;
        readonly IMarketVehicleService _marketVehicleService;
        readonly IDealerService _dealerService;
        readonly IDealerSegmentService _dealerSegmentService;
        public UpdateSegmentInventoryDetailProcess(IInventoryService inventoryService, ISettingService settingService, ISoldMarketVehicleService soldMarketVehicleService, IChromeService chromeService, IInventorySegmentDetailService inventorySegmentDetailService, IDealerBrandService dealerBrandService, IMarketVehicleService marketVehicleService, IDealerService dealerService, IDealerSegmentService dealerSegmentService)
        {
            _inventoryService = inventoryService;
            _settingService = settingService;
            _soldMarketVehicleService = soldMarketVehicleService;
            _chromeService = chromeService;
            _inventorySegmentDetailService = inventorySegmentDetailService;
            _dealerBrandService = dealerBrandService;
        
            _marketVehicleService = marketVehicleService;
            _dealerService = dealerService;
            _dealerSegmentService = dealerSegmentService;
        }

        public void Run()
        {


            var brandDealerList = _settingService.GetBrandDealerList();

            var dealerLocationList = _dealerService.GetDealerLocationListFromInventory();

            foreach (var dealerLocation in dealerLocationList)
            {
                try
                {

                    var makeList = Parser.GetListBySeparatingCommas(brandDealerList.Where(i => i.DealerId == dealerLocation.DealerId).Select(i => i.BrandName).FirstOrDefault());

                    var reconList = _inventoryService.GetReconList(dealerLocation.DealerId).ToList();

                    var inventory = _inventoryService.GetUsedInventoryForDealer(dealerLocation.DealerId).Where(i => !makeList.Contains(i.Vehicle.Make)).Select(i => new { i.Vehicle.Year, i.Vehicle.Make, i.Vehicle.Model, DateInStock = i.DateInStock.Value }).ToList().GroupBy(i => new { i.Make, i.Model }).Select(i => new MakeModelAgeStock { Make = i.Key.Make, Model = i.Key.Model, Stock = i.Count(), Age = i.Average(j => StockingGuideBusinessHelper.GetAgeFromNow(j.DateInStock)) }).ToList();

                    var soldVehicles = _soldMarketVehicleService.GetHistoryList(DateTime.Now, dealerLocation.DealerId);

                    if (soldVehicles.Count != 3) throw new Exception("History should return 3 records");

                    var segmentDetailList = new List<SGInventoryDealerSegmentDetail>();

                    var marketList = _marketVehicleService.GetCarOnMarketWithin100MilesByMakeModel((double)dealerLocation.Longitude, (double)dealerLocation.Latitude);

                    DeleteOldData(dealerLocation.DealerId, inventory);
                    UpdateInsertNewData(inventory, soldVehicles, segmentDetailList, dealerLocation, reconList, marketList);

                    SetWishListFieldFromOldDealerBrands(dealerLocation.DealerId, makeList);
                   
                }
                catch (Exception e)
                {
                    ServiceLog.Info(e.StackTrace);
                    ServiceLog.Info(e.Message);
                    ServiceLog.Info(e.Source);
                    ServiceLog.Info(e.InnerException.StackTrace);
                    ServiceLog.Info(e.InnerException.Message);
                    ServiceLog.Info(e.InnerException.Source);
                }



            }
        }

      
        private void SetWishListFieldFromOldDealerBrands(int dealerId, List<string> makeList)
        {
            var wishList = _dealerBrandService.GetDealerBrandForDealer(dealerId).Where(i => i.IsWishList.Value)
                 .ToList()
                 .Where(i => !makeList.Contains(i.Make)).ToList();

            foreach (var item in wishList)
            {
                var _inventorySegmentDetails = _inventorySegmentDetailService.GetInventorySegmentDetailForDealer(dealerId)
                    .Where(i => i.Make == item.Make && i.Model == item.Model);
                foreach (var inventorySegment in _inventorySegmentDetails)
                {
                    inventorySegment.IsWishList = true;
                    ServiceLog.Info(string.Format("UpdateSegmentInventoryDetailProcess: Update wishlist for {0} {1}", inventorySegment.Make, inventorySegment.Model));

                }
            }

            if (wishList.Any())
            {
                ServiceLog.Info(string.Format("UpdateSegmentInventoryDetailProcess: Delete wishlist list in dealer brand with {0} items: {1}", wishList.Count, Parser.GetStringFromList(wishList.Select(i => i.Model))));

                _dealerBrandService.RemoveDealerBrands(wishList);
                _dealerBrandService.SaveChanges();
            }
        }


        private void DeleteOldData(int dealerId, List<MakeModelAgeStock> inventory)
        {
            var list = _inventorySegmentDetailService.GetInventorySegmentDetailForDealer(dealerId);
            var deletedList = new List<SGInventoryDealerSegmentDetail>();
            foreach (var item in list)
            {
                if (inventory.FirstOrDefault(i => i.Make == item.Make && i.Model == item.Model) == null)
                {
                    if (!(item.IsWishList ?? false))
                    {
                        deletedList.Add(item);
                    }
                    else
                    {
                        item.InStock = 0;
                        item.History = 0;
                        item.TurnOver = 0;
                        item.Supply = 0;
                        item.Recon = 0;
                        item.Age = 0;
                    }
                }
            }

            if (deletedList.Any())
            {
                _inventorySegmentDetailService.RemoveInventorySegmentDetails(deletedList);
            }
            else
            {
                _inventorySegmentDetailService.SaveChanges();

            }
        }

        private void UpdateInsertNewData(List<MakeModelAgeStock> inventory, List<List<MakeHistory>> soldVehicles, List<SGInventoryDealerSegmentDetail> segmentDetailList, DealerLocation dealerLocation, List<Inventory> reconList, List<MakeModelQuantityPrice> marketList)
        {
            List<DealerSegmentMapping> segmentDealerSegmentList = _dealerSegmentService.GetDealerSegmentMapping(dealerLocation.DealerId);
            List<MakeModelSegmentId> makeModelSegmentList = _chromeService.GetMakeModelSegmentList();
            foreach (var item in inventory)
            {
                if (soldVehicles == null || soldVehicles[0] == null || soldVehicles[1] == null || soldVehicles[2] == null || !soldVehicles[0].Any() || !soldVehicles[1].Any() || !soldVehicles[2].Any()) continue;

                var history =
               (int)
                   Math.Ceiling((GetHistory(soldVehicles[0], item.Make, item.Model) +
                                 GetHistory(soldVehicles[1], item.Make, item.Model) +
                                 GetHistory(soldVehicles[2], item.Make, item.Model)) / 3);
                var dateInStockList = inventory.Where(i => i.Make == item.Make && i.Model == item.Model).ToList();
                var stock = dateInStockList.Sum(i => i.Stock);
                var segmentDetail = _inventorySegmentDetailService.GetInventorySegmentDetailForDealer(dealerLocation.DealerId, item.Make, item.Model).FirstOrDefault();
                var marketItem = marketList.FirstOrDefault(i => i.Make == item.Make && i.Model == item.Model);
                
                var makeModelSegmentId =
                              makeModelSegmentList.FirstOrDefault(
                                  i => i.Make == item.Make && i.Model == item.Model);

                int? segmentId = (makeModelSegmentId == null) ? null : makeModelSegmentId.SegmentId;
                var dealerSegmentMapping = segmentDealerSegmentList.FirstOrDefault(i => i.SegmentId == segmentId);
                int? dealerSegmentId = (dealerSegmentMapping == null) ? null : (int?)dealerSegmentMapping.DealerSegmentId;
                if (segmentDetail != null)
                {
                    if (dealerSegmentId.GetValueOrDefault() > 0)
                    {
                        segmentDetail.History = history;
                        segmentDetail.InStock = stock;
                        segmentDetail.TurnOver = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock,
                            history);
                        segmentDetail.Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(stock, history);
                        segmentDetail.Recon =
                            reconList.Count(i => i.Vehicle.Make == item.Make && i.Vehicle.Model == item.Model);
                        segmentDetail.Age = (int) Math.Ceiling(dateInStockList.Average(i => i.Age));
                        segmentDetail.UpdateDate = DateTime.Now;
                        segmentDetail.MarketAveragePrice = marketItem == null ? 0 : (decimal?) marketItem.AveragePrice;
                        segmentDetail.MarketHighestPrice = marketItem == null ? 0 : marketItem.MaxPrice;
                        segmentDetail.MarketLowestPrice = marketItem == null ? 0 : marketItem.MinPrice;
                        segmentDetail.SGDealerSegmentId = dealerSegmentId;
                        ServiceLog.Info(string.Format("Update inventory segment detail with {0} {1}", item.Make,
                            item.Model));
                    }

                }
                else
                {
                    if (dealerSegmentId.GetValueOrDefault() > 0)
                    {
                        segmentDetailList.Add(new SGInventoryDealerSegmentDetail
                        {
                            DealerId = dealerLocation.DealerId,
                            History = history,
                            Make = item.Make,
                            Model = item.Model ?? String.Empty,
                            InStock = stock,
                            TurnOver = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock, history),
                            Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(stock, history),
                            Recon = reconList.Count(i => i.Vehicle.Make == item.Make && i.Vehicle.Model == item.Model),
                            SGSegmentId = _chromeService.GetSegmentId(item.Make, item.Model),
                            Age = (int)Math.Ceiling(dateInStockList.Average(i => i.Age)),
                            CreatedDate = DateTime.Now,
                            MarketAveragePrice = marketItem == null ? 0 : (decimal?)marketItem.AveragePrice,
                            MarketHighestPrice = marketItem == null ? 0 : marketItem.MaxPrice,
                            MarketLowestPrice = marketItem == null ? 0 : marketItem.MinPrice,
                            SGDealerSegmentId = dealerSegmentId,

                        });
                    }

               
                    ServiceLog.Info(string.Format("Insert inventory segment detail with {0} {1}", item.Make, item.Model));

                }
                //UpsertSegmentDetail(soldVehicles, item, inventory, segmentDetailList, dealerLocation, reconList, marketList);

            }

            try
            {
                if (segmentDetailList.Any())
                {
                    ServiceLog.Info("Add inventorySegmentDetail and SaveChanges");
                    _inventorySegmentDetailService.AddInventorySegmentDetails(segmentDetailList);
                }
                else
                {
                    ServiceLog.Info("SaveChanges");
                    _inventorySegmentDetailService.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                ServiceLog.Info("Error:");
                ServiceLog.Info(exception.InnerException.Message);
                ServiceLog.Info(exception.StackTrace);
                ServiceLog.Info(exception.Message);
                ServiceLog.Info(exception.Source);

            }
        }

       
        private static double GetHistory(IEnumerable<MakeHistory> soldVehicles, string make, string model)
        {
            return soldVehicles.Count(i => i.Make == make && i.Model == model);
        }
    }
}
