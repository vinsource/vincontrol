using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class InventorySegmentDetailService: IInventorySegmentDetailService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;
        public InventorySegmentDetailService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }
        public void AddInventorySegmentDetails(List<SGInventoryDealerSegmentDetail> segmentDetailList)
        {
            foreach (var item in segmentDetailList)
            {
                _vincontrolUnitOfWork.InventorySegmentDetailRepository.Add(item);
            }
            _vincontrolUnitOfWork.Commit();
        }

        public IQueryable<SGInventoryDealerSegmentDetail> GetInventorySegmentDetailForDealer(int dealerId)
        {
           return _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(i =>i.DealerId  == dealerId);
        }

        public IQueryable<SGInventoryDealerSegmentDetail> GetInventorySegmentDetailForDealer(int dealerId, string make, string model)
        {
            return _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(i => i.DealerId == dealerId && i.Make == make && i.Model == model);
        }

        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }

        public void UpdateWishList(int id, bool isWishList)
        {
            var obj = _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(x => x.SGInventoryDealerSegmentDetailId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.IsWishList = isWishList;
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void RemoveFromWishList(int id)
        {
            var obj = _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(x => x.SGInventoryDealerSegmentDetailId == id).FirstOrDefault();
            if (obj != null)
            {
                _vincontrolUnitOfWork.InventorySegmentDetailRepository.Remove(obj);
                _vincontrolUnitOfWork.Commit();
            }
        }
        public List<int> GetDealerIdList()
        {
            return _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(i=>i.DealerId!=0).Select(i => i.DealerId).Distinct().ToList();
        }

        public void UpdateGuide(int id, int guide)
        {
            var obj = _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(x => x.SGInventoryDealerSegmentDetailId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Guide = guide;
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void RemoveInventorySegmentDetails(List<SGInventoryDealerSegmentDetail> deletedList)
        {
            foreach (var item in deletedList)
            {
               _vincontrolUnitOfWork.InventorySegmentDetailRepository.Remove(item);
            }
            _vincontrolUnitOfWork.Commit();
        }

        public List<MakeModel> GetMakeModelListByDealer(int dealerId)
        {
            return
                _vincontrolUnitOfWork.InventorySegmentDetailRepository.Find(i => i.DealerId == dealerId)
                    .Select(i => new MakeModel {Make = i.Make, Model = i.Model})
                    .Distinct()
                    .ToList();
        }
    }
}
