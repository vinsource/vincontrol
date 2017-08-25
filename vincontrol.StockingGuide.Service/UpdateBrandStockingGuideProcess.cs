using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateBrandStockingGuideProcess : IUpdateBrandStockingGuideProcess
    {
        readonly IInventoryService _inventoryService;
        readonly IChromeService _chromeService;
        readonly IDealerBrandService _dealerBrandService;
        readonly IDealerService _dealerService;
        private readonly IInventorySegmentDetailService _inventorySegmentDetailService;
        private readonly ISoldMarketVehicleService _soldMarketVehicleService;
        private readonly ISoldMarketTruckService _soldMarketTruckService;
        private readonly ISettingService _settingService;
        private readonly IMarketVehicleService _marketVehicleService;
        private readonly IMarketTruckService _marketTruckService;
      


        public UpdateBrandStockingGuideProcess(IInventoryService inventoryService, IChromeService chromeService, IDealerBrandService dealerBrandService, IDealerService dealerService, ISoldMarketVehicleService soldMarketVehicleService, ISettingService settingService, IInventorySegmentDetailService inventorySegmentDetailService, IMarketVehicleService marketVehicleService, ISoldMarketTruckService soldMarketTruckService,IMarketTruckService marketTruckService)
        {
            _inventoryService = inventoryService;
            _chromeService = chromeService;
            _dealerBrandService = dealerBrandService;
            _dealerService = dealerService;
            _soldMarketVehicleService = soldMarketVehicleService;
            _settingService = settingService;
            _inventorySegmentDetailService = inventorySegmentDetailService;
            _marketVehicleService = marketVehicleService;
            _soldMarketTruckService = soldMarketTruckService;
            _marketTruckService = marketTruckService;
          
        }

        public void Run()
        {
            var dealerLocationList = _dealerService.GetDealerLocationListFromInventory();
            var brandDealerList = _settingService.GetBrandDealerList();

            foreach (var dealerLocation in dealerLocationList.Where(i => brandDealerList.Select(j => j.DealerId).Contains(i.DealerId)))
            {
                ServiceLog.Info(string.Format("UpdateBrandStockingGuideProcess: dealer {0}", dealerLocation.DealerId));
                var makeList =
             Parser.GetListBySeparatingCommas(
                 brandDealerList.Where(i => i.DealerId == dealerLocation.DealerId).Select(i => i.BrandName).FirstOrDefault());

                UpdateInsertNewData(makeList, dealerLocation);
                DeleteOlddata(makeList, dealerLocation.DealerId);

                ServiceLog.Info(string.Format("UpdateBrandStockingGuideProcess: end DeleteOlddata(brandDealerList, {0})", dealerLocation));
                SetWishListFieldFromOldSegments(dealerLocation.DealerId, makeList);
                ServiceLog.Info(string.Format("UpdateBrandStockingGuideProcess: end  SetWishListFieldFromOldSegments({0}, makeList)", dealerLocation));

            }
        }

        private void SetWishListFieldFromOldSegments(int dealerId, List<string> makeList)
        {
            var wishList = _inventorySegmentDetailService.GetInventorySegmentDetailForDealer(dealerId).Where(i => i.IsWishList.Value)
                 .ToList()
                 .Where(i => makeList.Contains(i.Make)).ToList();

            foreach (var item in wishList)
            {
                var dealerBrands = _dealerBrandService.GetDealerBrandByDealerID(dealerId)
                    .Where(i => i.Make == item.Make && i.Model == item.Model);
                foreach (var dealerBrand in dealerBrands)
                {
                    dealerBrand.IsWishList = true;
                    ServiceLog.Info(string.Format("UpdateBrandStockingGuideProcess: Update wishlist for {0} {1}", dealerBrand.Make, dealerBrand.Model));

                }
            }

            if (wishList.Any())
            {
                ServiceLog.Info(string.Format("UpdateBrandStockingGuideProcess: Delete wishlist list in segment with {0} items: {1}", wishList.Count, Parser.GetStringFromList(wishList.Select(i => i.Model))));

                _inventorySegmentDetailService.RemoveInventorySegmentDetails(wishList);
                _dealerBrandService.SaveChanges();
            }
        }

        private void DeleteOlddata(List<string> makeList, int dealerId)
        {
          
            var deletedMakeList = _dealerBrandService.GetDealerBrandByDealerID(dealerId)
                .Select(i => i.Make)
                .ToList()
                .Where(i => !makeList.Contains(i)).Distinct();
            var logMakeList = Parser.GetStringFromList(deletedMakeList);

            ServiceLog.Info("UpdateBrandStockingGuideProcess: start _dealerBrandService.DeleteDealerBrands(deletedMakeList);");
            ServiceLog.Info(string.Format("Deleted list: {0}", logMakeList));
            _dealerBrandService.DeleteDealerBrandsNotInWishlist(deletedMakeList);
            ServiceLog.Info("UpdateBrandStockingGuideProcess: end _dealerBrandService.DeleteDealerBrands(deletedMakeList);");

        }



        private void UpdateInsertNewData(IEnumerable<string> makeList, DealerLocation dealerLocation)
        {

            var inventory = _inventoryService.GetUsedInventoryForDealer(dealerLocation.DealerId).
                Select(
                    i =>
                        new MakeModelDateInStock
                        {
                            Make = i.Vehicle.Make,
                            Model = i.Vehicle.Model,
                            DateInStock = i.DateInStock.Value
                        }).ToList();
            var reconList = _inventoryService.GetReconList(dealerLocation.DealerId).ToList();

            var soldVehicles = _soldMarketVehicleService.GetHistoryList(DateTime.Now, dealerLocation.DealerId);
            var marketList = new List<MakeModelQuantityPrice>();

            if (dealerLocation.DealerId == Constants.DealerId.FreewayIsuzu)
            {
                marketList =
                    _marketVehicleService.GetCarOnMarketWithin100MilesByMakeModel((double) dealerLocation.Longitude,
                        (double) dealerLocation.Latitude);
                marketList.AddRange(_marketTruckService.GetCarOnMarketWithin100MilesByMakeModel((double)dealerLocation.Longitude, (double)dealerLocation.Latitude) );
            }
            else
            {
                marketList =
                    _marketTruckService.GetCarOnMarketWithin100MilesByMakeModel((double) dealerLocation.Longitude,
                        (double) dealerLocation.Latitude);
            }
          
           
            
            if (soldVehicles.Count != 3)
            {
                ServiceLog.Info("History should return 3 records");
            }

            ServiceLog.Info(
                string.Format("UpdateBrandStockingGuideProcess: Finish _soldMarketVehicleService.GetHistoryList({0}, {1})",
                    DateTime.Now, dealerLocation.DealerId));

            foreach (var make in makeList)
            {
                List<string> modelList = _chromeService.GetModels(make);
                var dealerBrands = new List<SGDealerBrand>();

                foreach (var model in modelList)
                {
                    UpsertDealerBrand(soldVehicles, make, model, inventory, dealerLocation, reconList, dealerBrands, marketList);
                }

                if (dealerBrands.Count > 0)
                {
                    ServiceLog.Info("Add dealerBrands and SaveChanges");
                    _dealerBrandService.AddDealerBrand(dealerBrands);
                }
                else
                {
                    ServiceLog.Info("SaveChanges for dealerBrands");
                    _dealerBrandService.SaveChanges();
                }
            }
        }

        private void UpsertDealerBrand(List<List<MakeHistory>> soldVehicles, string make, string model, IEnumerable<MakeModelDateInStock> inventory, DealerLocation dealerLocation, IEnumerable<Inventory> reconList, List<SGDealerBrand> dealerBrands, List<MakeModelQuantityPrice> marketList)
        {
            var history = (int)Math.Ceiling((GetHistory(soldVehicles[0], make, model) + GetHistory(soldVehicles[1], make, model) +
                           GetHistory(soldVehicles[2], make, model)) / 3);
            var dateInStockList = inventory.Where(i => i.Model.ToLower() == model.ToLower()).ToList();
            var stock = dateInStockList.Count();
            var dealerBrand = _dealerBrandService.GetDealerBrand(make, model, dealerLocation.DealerId);
            var marketItem = marketList.FirstOrDefault(i => i.Make == make && i.Model == model);
            if (dealerBrand == null)
            {
                dealerBrand = new SGDealerBrand
                {
                    Make = make,
                    Model = model,
                    History = history,
                    TurnOver = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock, history),
                    Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(stock, history),
                    Recon =
                        reconList.Count(
                            i => i.Vehicle.Make.ToLower() == make.ToLower() && i.Vehicle.Model.ToLower() == model.ToLower()),
                    Age =
                        dateInStockList.Any()
                            ? (int)Math.Ceiling(dateInStockList.Average(i => StockingGuideBusinessHelper.GetAgeFromNow(i.DateInStock)))
                            : 0,
                    DealerId = dealerLocation.DealerId,
                    Stock = stock,
                    CreatedDate = DateTime.Now,
                    MarketAveragePrice =marketItem==null?0:(decimal?)marketItem.AveragePrice,
                    MarketHighestPrice = marketItem==null?0:marketItem.MaxPrice,
                    MarketLowestPrice = marketItem==null?0:marketItem.MinPrice
                };
                dealerBrands.Add(dealerBrand);
                ServiceLog.Info(string.Format("Insert dealer Brand with {0} {1}",make, model));
            }
            else
            {
                dealerBrand.History = history;
                dealerBrand.TurnOver = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock,
                    history);
                dealerBrand.Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(stock, history);
                dealerBrand.Recon =
                    reconList.Count(
                        i => i.Vehicle.Make.ToLower() == make.ToLower() && i.Vehicle.Model.ToLower() == model.ToLower());
                dealerBrand.Age = dateInStockList.Any()
                    ? (int)Math.Ceiling(dateInStockList.Average(i => StockingGuideBusinessHelper.GetAgeFromNow(i.DateInStock)))
                    : 0;
                dealerBrand.Stock = stock;
                dealerBrand.UpdateDate = DateTime.Now;
                dealerBrand.MarketAveragePrice = marketItem == null ? 0 : (decimal?) marketItem.AveragePrice;
                dealerBrand.MarketHighestPrice = marketItem == null ? 0 : marketItem.MaxPrice;
                dealerBrand.MarketLowestPrice = marketItem == null ? 0 : marketItem.MinPrice;
                ServiceLog.Info(string.Format("Update dealer Brand with {0} {1}",make, model));
                //_dealerBrandService.SaveChanges();
            }
        }

        private static double GetHistory(IEnumerable<MakeHistory> soldVehicles, string make, string model)
        {
            return soldVehicles.Count(i => i.Make.ToLower() == make.ToLower() && i.Model.ToLower() == model.ToLower());
        }
    }
}
