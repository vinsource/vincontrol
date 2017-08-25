using System;
using System.Linq;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class StockingGuideInfoService : IStockingGuideInfoService
    {
         private readonly IVincontrolUnitOfWork _vincontrolUnitOfWork;
         public StockingGuideInfoService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        //public void AddStockingGuideInfo(StockingGuideInfo stockingGuideInfo)
        //{
        //    _vincontrolUnitOfWork.StockingGuideInfoRepository.Add(new SGKPIInfo(stockingGuideInfo));
        //    //_vincontrolUnitOfWork.WeeklyTurnOverRepository.Add(new SGWeeklyTurnOver(stockingGuideInfo) { });
        //    _vincontrolUnitOfWork.Commit();

        //}

        public void AddStockingGuideInfo(SGKPIInfo stockingGuideInfo)
        {
            _vincontrolUnitOfWork.StockingGuideInfoRepository.Add( stockingGuideInfo);
            //_vincontrolUnitOfWork.WeeklyTurnOverRepository.Add(new SGWeeklyTurnOver(stockingGuideInfo) { });
            _vincontrolUnitOfWork.Commit();
        }

        public IQueryable<SGKPIInfo> GetStockingGuideInfoByDealerID(int dealerID)
        {
            return _vincontrolUnitOfWork.StockingGuideInfoRepository.Find(x => x.DealerId == dealerID);
        }

    }
}
