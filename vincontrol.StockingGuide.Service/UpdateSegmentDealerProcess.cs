using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateSegmentDealerProcess : IUpdateSegmentDealerProcess
    {
        private readonly ISegmentService _segmentService;
        private readonly IInventorySegmentDetailService _inventorySegmentDetailService;
        private readonly IDealerSegmentService _dealerSegmentService;
        private readonly IDealerService _dealerService;
        




        public UpdateSegmentDealerProcess(ISegmentService segmentService, IInventorySegmentDetailService inventorySegmentDetailService, IDealerSegmentService dealerSegmentService, IDealerService dealerService)
        {
            _segmentService = segmentService;
            _inventorySegmentDetailService = inventorySegmentDetailService;
            _dealerSegmentService = dealerSegmentService;
            _dealerService = dealerService;
       
        }

      
        public void Run()
        {
            var dealerIdList = _dealerService.GetDealerLocationListFromInventory();
                
            foreach (var dealer in dealerIdList)
            {
                ServiceLog.Info(string.Format("UpdateSegmentDealerProcess: dealer {0}", dealer.DealerId));

                List<SGSegment> segments = _segmentService.GetAllSegments();
                var inventorySegmentDetailList = _inventorySegmentDetailService.GetInventorySegmentDetailForDealer(dealer.DealerId).ToList();
                var dealerSegmentList = new List<SGDealerSegment>();
                var oldItem = new List<SGDealerSegment>();
                var dealerSegmentDbList = _dealerSegmentService.GetDealerSegments(dealer.DealerId);

                foreach (var dealerSegment in inventorySegmentDetailList)
                {
                    var sgDealerSegment = dealerSegmentDbList.FirstOrDefault(i => dealerSegment.SGSegmentId == i.SGSegmentId);
                    if (sgDealerSegment != null)
                        dealerSegment.SGDealerSegmentId = sgDealerSegment.SGDealerSegmentId;
                }

                _inventorySegmentDetailService.SaveChanges();

                foreach (var segment in segments)
                {
                    var subsegment = inventorySegmentDetailList.Where(i => i.SGSegmentId == segment.SGSegmentId).ToList();
                    var history = subsegment.Any() ? subsegment.Sum(i => i.History) : 0;
                    var stock = subsegment.Any() ? subsegment.Sum(i => i.InStock) : 0;

                    var dealerSegment = dealerSegmentDbList.FirstOrDefault(i => i.SGSegmentId == segment.SGSegmentId);
                    if (dealerSegment == null)
                    {
                        dealerSegmentList.Add(new SGDealerSegment
                        {
                            DealerId = dealer.DealerId,
                            SGSegmentId = segment.SGSegmentId,
                            History = history,
                            Stock = stock,
                            Turnover = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock, history),
                            Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(stock, history),
                            Age = subsegment.Any() ? (int)Math.Ceiling(subsegment.Average(i => i.Age)) : 0,
                            Recon = subsegment.Any() ? subsegment.Sum(i => i.Recon) : 0,
                            CreatedDate = DateTime.Now

                        }
                            );
                        ServiceLog.Info(string.Format("Insert dealerSegment with {0}", segment.Name));

                    }
                    else
                    {

                        dealerSegment.History = history;
                        dealerSegment.Stock = stock;
                        dealerSegment.Turnover = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock, history);
                        dealerSegment.Supply = StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(stock, history);
                        dealerSegment.Age = subsegment.Any() ? (int)Math.Ceiling(subsegment.Average(i => i.Age)) : 0;
                        dealerSegment.Recon = subsegment.Any() ? subsegment.Sum(i => i.Recon) : 0;
                        dealerSegment.UpdateDate = DateTime.Now;
                        ServiceLog.Info(string.Format("Update dealerSegment with {0}", segment.Name));
                        oldItem.Add(dealerSegment);

                    }
                }

                if (dealerSegmentList.Count > 0)
                {
                    _dealerSegmentService.AddDealerSegments(dealerSegmentList);
                }
                else
                {
                    _dealerSegmentService.SaveChanges();
                }
            }
        }
    }
}
