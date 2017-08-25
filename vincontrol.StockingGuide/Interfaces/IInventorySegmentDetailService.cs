using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IInventorySegmentDetailService
    {
        void AddInventorySegmentDetails(List<SGInventoryDealerSegmentDetail> segmentDetailList);
        IQueryable<SGInventoryDealerSegmentDetail> GetInventorySegmentDetailForDealer(int dealerId);
        IQueryable<SGInventoryDealerSegmentDetail> GetInventorySegmentDetailForDealer(int dealerId,string make, string model);
        void SaveChanges();
        void UpdateWishList(int id, bool isWishList);
        void RemoveFromWishList(int id);
        List<int> GetDealerIdList();
        void UpdateGuide(int id, int guide);
        void RemoveInventorySegmentDetails(List<SGInventoryDealerSegmentDetail> deletedList);
        List<MakeModel> GetMakeModelListByDealer(int dealerId);
    }
}
