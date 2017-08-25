using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Interfaces
{
    public class MarketSegmentDetailService:IMarketSegmentDetailService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public MarketSegmentDetailService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public void AddMarketSegmentDetails(List<SGMarketDealerSegmentDetail> marketDealerSegmentDetails)
        {
            foreach (var item in marketDealerSegmentDetails)
            {
                _vincontrolUnitOfWork.MarketSegmentDetailRepository.Add(item);
            }
           _vincontrolUnitOfWork.Commit();
        }

        public void UpdateWishList(int id, bool isWishList)
        {
            var obj = _vincontrolUnitOfWork.MarketSegmentDetailRepository.Find(x => x.SGMarketDealerSegmentDetailId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.IsWishList = isWishList;
                _vincontrolUnitOfWork.Commit();
            }
        }
        public void RemoveFromWishList(int id)
        {
            var obj = _vincontrolUnitOfWork.MarketSegmentDetailRepository.Find(x => x.SGMarketDealerSegmentDetailId == id).FirstOrDefault();
            if (obj != null)
            {
                _vincontrolUnitOfWork.MarketSegmentDetailRepository.Remove(obj);
                _vincontrolUnitOfWork.Commit();
            }
        }

        public SGMarketDealerSegmentDetail GetMarketSegmentDetailForDealer(int dealerId, string make, string model)
        {
           return _vincontrolUnitOfWork.MarketSegmentDetailRepository.Find(x => x.DealerId == dealerId && x.Make==make && x.Model == model).FirstOrDefault();
        }

        public IQueryable<SGMarketDealerSegmentDetail> GetMarketSegmentDetailForDealer(int dealerId)
        {
            return _vincontrolUnitOfWork.MarketSegmentDetailRepository.Find(x => x.DealerId == dealerId);
        }

        //public IQueryable<SGMarketDealerSegmentDetail> GetEmptyMarketSegmentDetailsForDealer(int dealerId)
        //{
        //    return _vincontrolUnitOfWork.MarketSegmentDetailRepository.Find(x => x.DealerId == dealerId && );
        //}

        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }

        public void RemoveInventorySegmentDetails(List<SGMarketDealerSegmentDetail> deletedList)
        {
            foreach (var item in deletedList)
            {
                _vincontrolUnitOfWork.MarketSegmentDetailRepository.Remove(item);
            }
             _vincontrolUnitOfWork.Commit();
        }

        public void AddMarketSegmentDetail(SGMarketDealerSegmentDetail marketDealerSegmentDetail)
        {
            _vincontrolUnitOfWork.MarketSegmentDetailRepository.Add(marketDealerSegmentDetail);
            _vincontrolUnitOfWork.Commit();
        }
    }
}
