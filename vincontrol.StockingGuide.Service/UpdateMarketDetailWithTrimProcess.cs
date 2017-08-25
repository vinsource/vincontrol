using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateMarketDetailWithTrimProcess : IUpdateMarketDetailWithTrimProcess
    {
        private IMarketDetailWithTrimService _marketDetailWithTrimService;
        private IMarketVehicleService _marketVehicleService;
        private ISoldMarketVehicleService _soldMarketVehicleService;
        private IInventoryService _inventoryService;
        private IDealerService _dealerService;
        private readonly IInventorySegmentDetailService _inventorySegmentDetailService;
        private readonly IDealerBrandService _dealerBrandService;
        private readonly IChromeService _chromeService;

        public UpdateMarketDetailWithTrimProcess(IMarketDetailWithTrimService marketDetailWithTrimService)
        {
            marketDetailWithTrimService = _marketDetailWithTrimService;
        }
        public void Run()
        {
            List<DealerLocation> dealerLocationList = _dealerService.GetDealerLocationList();

            var dealerIdList = _inventoryService.GetDealerIdList();

            foreach (var dealerId in dealerIdList)
            {
               // var dealerLocation = dealerLocationList.FirstOrDefault(i => i.DealerId == dealerId);

               //var historyList = _soldMarketVehicleService.GetHistoryListByTrim(DateTime.Now);
               //var marketDetails = _marketDetailWithTrimService.GetMarketDetailsForDealer(dealerId);

               //List<MakeModelTrim> chromeMakeModelTrimList = _chromeService.GetMakeModelTrimList();

               // var brandMakeModelList =_dealerBrandService.GetMakeModelListByDealer(dealerId);
               // UpsertMarketDetailWithTrim(brandMakeModelList, dealerLocation, historyList, marketDetails, chromeMakeModelTrimList);

               // var segmentMakeModelList = _inventorySegmentDetailService.GetMakeModelListByDealer(dealerId);
               // UpsertMarketDetailWithTrim(segmentMakeModelList, dealerLocation, historyList, marketDetails, chromeMakeModelTrimList);

            }
        }

        //private void UpsertMarketDetailWithTrim(List<MakeModel> makeModelList, DealerLocation dealerLocation, List<List<MakeModelTrimHistory>> historyList, List<SGMarketDetailWithTrim> marketDetails, List<MakeModelTrim> chromeMakeModelTrimList)
        //{
        //    foreach (var item in makeModelList)
        //    {
        //        var newMarketDetails = new List<SGMarketDetailWithTrim>();
        //        var trimName =
        //            chromeMakeModelTrimList.FirstOrDefault(i => i.Make == item.Make && i.Model == item.Model);
        //        int? onMarket = null;
        //        int? highestPrice = null;
        //        int? lowestPrice = null;
        //        int? averagePrice = null;
        //        //var inAppraisal = ;
        //        //var atAuction =;
        //        //var turnOver =;

        //        if (trimName != null)
        //        {
        //            var marketInfo =
        //                _marketVehicleService.GetNumberOfCarOnMarketWithin100Miles((double) dealerLocation.Longitude,
        //                    (double)dealerLocation.Latitude, item.Make, item.Model, trimName.Trim);
        //            onMarket = marketInfo.NumberOfCars;
        //            highestPrice = marketInfo.HighestPrice;
        //            lowestPrice = marketInfo.LowestPrice;
        //            averagePrice = marketInfo.AveragePrice;
        //        }

        //        var marketDetail =
        //            marketDetails.Where(i => i.Make == item.Make && i.Model == item.Model)
        //                .Where(CompareTrimPredicate(trimName))
        //                .FirstOrDefault();
        //        if (marketDetail == null)
        //        {
        //            //newMarketDetails.Add(new SGMarketDetailWithTrim(){OnMarket = onMarket,LowestPrice = lowestPrice,HighestPrice = highestPrice,AveragePrice = averagePrice,DealerId = dealerLocation.DealerId,Make=item.Make,Model = item.Model, InAppraisal = inAppraisal,AtAuction = atAuction,TurnOver = ,Trim=item.});
        //        }
        //        else
        //        {
        //            //marketDetail.OnMarket =onMarket;
        //            //marketDetail.AtAuction =;
        //            //marketDetail.InAppraisal =;
        //        }

        //        if (newMarketDetails.Any())
        //        {
        //            _marketDetailWithTrimService.AddMarketDetailWithTrims(newMarketDetails);
        //        }
        //        else
        //        {
        //            _marketDetailWithTrimService.SaveChanges();
        //        }

        //    }
           
        //}

        //private static Func<SGMarketDetailWithTrim, bool> CompareTrimPredicate(MakeModelTrim trimName)
        //{
        //    if(trimName!=null)
        //    return i => i.Trim == trimName.Trim;
        //    return i=>true;
        //}
    }
}
