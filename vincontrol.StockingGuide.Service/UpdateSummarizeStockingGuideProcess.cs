using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;
using vincontrol.StockingGuide.Services;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateSummarizeStockingGuideProcess : IUpdateSummarizeStockingGuideProcess
    {

        readonly IDealerService _dealerService;
        private readonly IDealerSegmentService _dealerSegmentService;
        private readonly IDealerBrandService _dealerBrandService;
        private readonly IKPIInfoService _kpiInfoService;


        public UpdateSummarizeStockingGuideProcess(IDealerService dealerService, IDealerSegmentService dealerSegmentService, IDealerBrandService dealerBrandService, IKPIInfoService kpiInfoService)
        {
            _dealerService = dealerService;
            _dealerSegmentService = dealerSegmentService;
            _dealerBrandService = dealerBrandService;
            _kpiInfoService = kpiInfoService;

        }

        public void Run()
        {
            var dealerIdList = _dealerService.GetDealerIdList();
            foreach (var dealerId in dealerIdList)
            {
                var list = _dealerSegmentService.GetAllDealerSegmentsForDealer(dealerId).ToList();
                var infoList = list.Select(segment => new SGKPIInfo
                {
                    Stock = segment.Stock, History = segment.History, Supply = segment.Supply, TurnOver = segment.Turnover,
                    DealerId = dealerId, Type = Constants.KPIInfoType.Segment, Name = segment.SGSegment.Name
                }).ToList();

                var brandList = _dealerBrandService.GetDealerBrandForDealer(dealerId).GroupBy(i=>i.Make).Select(i=>new{Make=i.Key,Stock=i.Sum(j=>j.Stock), History=i.Sum(j=>j.History)});
                foreach (var brand in brandList)
                {
                    infoList.Add(new SGKPIInfo
                    {
                        Stock = brand.Stock,
                        History = brand.History,
                        Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(brand.Stock, brand.History),
                        TurnOver = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(brand.Stock, brand.History),
                        DealerId = dealerId,
                        Type = Constants.KPIInfoType.Brand,
                        Name = brand.Make
                    }); 
                }
             

                _kpiInfoService.AddKPIInfos(infoList);
            }

        }


    }
}
