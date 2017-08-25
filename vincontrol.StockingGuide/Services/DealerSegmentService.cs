using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class DealerSegmentService : IDealerSegmentService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;
        public DealerSegmentService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }
        public void AddDealerSegments(List<SGDealerSegment> dealerSegmentList)
        {
            foreach (var item in dealerSegmentList)
            {
                _vincontrolUnitOfWork.DealerSegmentRepository.Add(item);
            }

            _vincontrolUnitOfWork.Commit();
        }

        public IQueryable<SGDealerSegment> GetAllDealerSegmentsForDealer(int dealerId, string make = "", string model = "")
        {
            return _vincontrolUnitOfWork.DealerSegmentRepository.GetAllDealerSegmentsForDealer(dealerId, make, model);
        }

        public void UpdateGuide(int id, int guide)
        {
            var obj = _vincontrolUnitOfWork.DealerSegmentRepository.Find(x => x.SGDealerSegmentId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Guide = guide;
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void UpdateWishList(int id, bool isWishList)
        {
            var obj = _vincontrolUnitOfWork.DealerSegmentRepository.Find(x => x.SGDealerSegmentId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.IsWishList = isWishList;
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void UpdateGrossPerUnit(int id, int grossPerUnit)
        {
            var obj = _vincontrolUnitOfWork.DealerSegmentRepository.Find(x => x.SGDealerSegmentId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.GrossPerUnit = grossPerUnit;
                _vincontrolUnitOfWork.Commit();
            }
        }

        public SGDealerSegment GetDealerSegment(int dealerId, short sgSegmentId)
        {
            return _vincontrolUnitOfWork.DealerSegmentRepository.Find(
                x => x.DealerId == dealerId && x.SGSegmentId == sgSegmentId).FirstOrDefault();
        }

        public List<SGDealerSegment> GetDealerSegments(int dealerId)
        {
            return _vincontrolUnitOfWork.DealerSegmentRepository.Find(
                x => x.DealerId == dealerId).ToList();
        }

        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }

        public List<DealerSegmentMapping> GetDealerSegmentMapping(int dealerId)
        {
            return _vincontrolUnitOfWork.DealerSegmentRepository.Find(x => x.DealerId == dealerId).Select(i=>new DealerSegmentMapping{DealerSegmentId = i.SGDealerSegmentId, SegmentId = i.SGSegmentId}).ToList();
        }
    }
}
