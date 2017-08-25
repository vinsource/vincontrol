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
    public class UpdateSegmentMarketDetailProcess : IUpdateSegmentMarketDetailProcess
    {
        readonly IMarketVehicleService _marketVehicleService;
        readonly IDealerService _dealerService;
        readonly IInventoryService _inventoryService;
        readonly IInventorySegmentDetailService _inventorySegmentDetailService;
        readonly IMarketSegmentDetailService _marketSegmentDetailService;
        readonly ISoldMarketVehicleService _soldMarketVehicleService;
        readonly ISettingService _settingService;
 
        readonly IChromeService _chromeService;
        readonly IDealerSegmentService _dealerSegmentService;


        public UpdateSegmentMarketDetailProcess(IMarketVehicleService marketVehicleService, IDealerService dealerService, IInventoryService inventoryService, IInventorySegmentDetailService inventorySegmentDetailService, IMarketSegmentDetailService marketSegmentDetailService, ISettingService settingService, ISoldMarketVehicleService soldMarketVehicleService, IChromeService chromeService, IDealerSegmentService dealerSegmentService)
        {
            _marketVehicleService = marketVehicleService;
            _dealerService = dealerService;
            _inventorySegmentDetailService = inventorySegmentDetailService;
            _inventoryService = inventoryService;
            _marketSegmentDetailService = marketSegmentDetailService;
            _soldMarketVehicleService = soldMarketVehicleService;
            _settingService = settingService;
            _chromeService = chromeService;
            _dealerSegmentService = dealerSegmentService;
        
        }

        public void Run()
        {
            List<DealerLocation> dealerLocationList = _dealerService.GetDealerLocationList();
            var dealerIdList = _dealerService.GetDealerIdListFromInventory();
            
            foreach (var dealerId in dealerIdList)
            {
                ServiceLog.Info(string.Format("UpdateSegmentMarketDetailProcess: dealer {0}", dealerId));
                var dealerLocation = dealerLocationList.FirstOrDefault(i => i.DealerId == dealerId);
                List<string> makeList = _settingService.GetBrandListForDealer(dealerId);

                var inventorySegmentDetailList = _inventorySegmentDetailService.GetInventorySegmentDetailForDealer(dealerId).ToList();

                var inventories = _inventoryService.GetUsedInventoryForDealer(dealerId).Where(i => !makeList.Contains(i.Vehicle.Make)).Select(i => new { i.Vehicle.Year, i.Vehicle.Make, i.Vehicle.Model, DateInStock = i.DateInStock.Value }).ToList().GroupBy(i => new { i.Make, i.Model }).Select(i => new MakeModelAgeStock { Make = i.Key.Make, Model = i.Key.Model, Stock = i.Count(), Age = i.Average(j => StockingGuideBusinessHelper.GetAgeFromNow(j.DateInStock)) }).ToList();

                ServiceLog.Info(string.Format("Delete OLd Data: dealer {0}", dealerId));
                DeleteOldData(dealerId);

                ServiceLog.Info(string.Format("Insert New Data: dealer {0}", dealerId));
                UpdateInsertNewData(inventorySegmentDetailList, dealerLocation, inventories, dealerId);
                
                ServiceLog.Info(string.Format("End UpdateSegmentMarketDetailProcess: dealer {0}", dealerId));

              

            }
        }

        private void DeleteOldData(int dealerId)
        {
            var currentList = _marketSegmentDetailService.GetMarketSegmentDetailForDealer(dealerId);
         
            var deletedList = currentList.Where(item => item.IsWishList ==null || item.IsWishList==false).ToList();

            if (deletedList.Any())
            {
                ServiceLog.Info(string.Format("Start _marketSegmentDetailService.RemoveInventorySegmentDetails(deletedList): {0} items",deletedList.Count) );

                _marketSegmentDetailService.RemoveInventorySegmentDetails(deletedList);

                ServiceLog.Info("End _marketSegmentDetailService.RemoveInventorySegmentDetails(deletedList)");

            }
        }

        private void UpdateInsertNewData(List<SGInventoryDealerSegmentDetail> inventorySegmentDetailList,
            DealerLocation dealerLocation, List<MakeModelAgeStock> inventories,
            int dealerId)
        {
            try
            {
            var marketDealerSegmentDetails = new List<SGMarketDealerSegmentDetail>();

            var history = _soldMarketVehicleService.GetHistoryList(DateTime.Now, (double) dealerLocation.Longitude,
                (double) dealerLocation.Latitude);

            var marketList =
                _marketVehicleService.GetCarOnMarketWithin100MilesByMakeModel((double) dealerLocation.Longitude,
                    (double) dealerLocation.Latitude);

            List<MakeModelSegmentId> makeModelSegmentList = _chromeService.GetMakeModelSegmentList();
            List<DealerSegmentMapping> segmentDealerSegmentList = _dealerSegmentService.GetDealerSegmentMapping(dealerId);
            var brandList = _settingService.GetBrandListForDealer(dealerId);

           
                foreach (var marketItem in marketList)
                {
                    if (!brandList.Contains(marketItem.Make))
                    {
                        int number = marketItem.Count;

                        var inventory =
                            inventories.FirstOrDefault(
                                i => i.Make == marketItem.Make && i.Model == marketItem.Model);

                        var inventorySegmentDetail =
                            inventorySegmentDetailList.FirstOrDefault(
                                i => i.Make == marketItem.Make && i.Model == marketItem.Model);

                        var marketSegmentDetail = _marketSegmentDetailService.GetMarketSegmentDetailForDealer(dealerId,
                            marketItem.Make, marketItem.Model);



                        if (marketSegmentDetail == null)
                        {
                            var makeModelSegmentId =
                                makeModelSegmentList.FirstOrDefault(
                                    i => i.Make == marketItem.Make && i.Model == marketItem.Model);

                            int? segmentId = (makeModelSegmentId == null) ? null : makeModelSegmentId.SegmentId;

                            var dealerSegmentMapping =
                                segmentDealerSegmentList.FirstOrDefault(i => i.SegmentId == segmentId);
                            int? dealerSegmentId = (dealerSegmentMapping == null)
                                ? null
                                : (int?) dealerSegmentMapping.DealerSegmentId;
                            var marketHistory = (int)
                                Math.Ceiling((GetMarketHistory(history[0], marketItem) +
                                              GetMarketHistory(history[1], marketItem) +
                                              GetMarketHistory(history[2], marketItem))/3);

                            if (dealerSegmentId.GetValueOrDefault() > 0)
                            {
                                var item = new SGMarketDealerSegmentDetail
                                {
                                    MarketHistory = marketHistory

                                    ,
                                    OU = (inventory == null || inventorySegmentDetail == null)
                                        ? 0
                                        : (inventory.Stock - inventorySegmentDetail.Guide),
                                    DealerId = dealerId,
                                    SGDealerSegmentId = dealerSegmentId,
                                    SGSegmentId = (short?) segmentId,
                                    TurnOver =
                                        Math.Ceiling(StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(number,
                                            marketHistory)),
                                    Age = (int) Math.Ceiling(marketItem.Age ?? 0),
                                    Supply =
                                        StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(number, marketHistory),
                                    History = marketHistory,
                                    MarketStock = number,
                                    YourStock = inventory == null ? 0 : inventory.Stock,
                                    Model = marketItem.Model,
                                    Make = marketItem.Make,
                                    CreatedDate = DateTime.Now,
                                    MarketHighestPrice = marketItem.MaxPrice,
                                    MarketLowestPrice = marketItem.MinPrice,
                                    MarketAveragePrice = (decimal?) marketItem.AveragePrice

                                };
                                marketDealerSegmentDetails.Add(item);
                            }

                        }
                        else
                        {
                            marketSegmentDetail.Make = marketItem.Make;
                            marketSegmentDetail.Model = marketItem.Model;
                            marketSegmentDetail.YourStock = inventory == null ? 0 : inventory.Stock;
                            marketSegmentDetail.MarketStock = number;
                            marketSegmentDetail.History = inventorySegmentDetail == null
                                ? 0
                                : inventorySegmentDetail.History;
                            marketSegmentDetail.Supply = inventorySegmentDetail == null
                                ? 0
                                : inventorySegmentDetail.Supply;
                            marketSegmentDetail.Age = (int) Math.Ceiling(marketItem.Age ?? 0);
                            marketSegmentDetail.TurnOver = inventorySegmentDetail == null
                                ? 0
                                : inventorySegmentDetail.TurnOver;
                            marketSegmentDetail.OU = (inventory == null || inventorySegmentDetail == null)
                                ? 0
                                : (inventory.Stock - inventorySegmentDetail.Guide);
                            marketSegmentDetail.MarketHistory =
                                (int)
                                    Math.Ceiling((GetMarketHistory(history[0], marketItem) +
                                                  GetMarketHistory(history[1], marketItem) +
                                                  GetMarketHistory(history[2], marketItem))/3);
                            marketSegmentDetail.UpdateDate = DateTime.Now;

                            marketSegmentDetail.MarketHighestPrice = marketItem.MaxPrice;
                            marketSegmentDetail.MarketLowestPrice = marketItem.MinPrice;
                            marketSegmentDetail.MarketAveragePrice = (decimal?) marketItem.AveragePrice;
                            marketSegmentDetail.UpdateDate = DateTime.Now;


                        }
                    }
                }


                if (marketDealerSegmentDetails.Any())
                {
                    _marketSegmentDetailService.AddMarketSegmentDetails(marketDealerSegmentDetails);
                }
                else
                {
                    _marketSegmentDetailService.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ServiceLog.Info(dealerId.ToString());
                ServiceLog.Info("Error when saving data");
                ServiceLog.Info(e.StackTrace);
                ServiceLog.Info(e.Message);
                ServiceLog.Info(e.Source);
                ServiceLog.Info(e.InnerException.StackTrace);
                ServiceLog.Info(e.InnerException.Message);
                ServiceLog.Info(e.InnerException.Source);
            }

        }

        private static double GetMarketHistory(List<MakeHistory> history, MakeModelQuantityPrice inventory)
        {
            return history.Where(i => i.Make == inventory.Make && i.Model == inventory.Model).Select(i => i.History).FirstOrDefault();
        }

      
    }
}
