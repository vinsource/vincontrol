using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IMarketSegmentDetailService
    {
        void AddMarketSegmentDetails(List<SGMarketDealerSegmentDetail> marketDealerSegmentDetails);
        void RemoveFromWishList(int id);
        void UpdateWishList(int id, bool isWishList);
        SGMarketDealerSegmentDetail GetMarketSegmentDetailForDealer(int dealerId, string make, string model);
        IQueryable<SGMarketDealerSegmentDetail> GetMarketSegmentDetailForDealer(int dealerId);
        void SaveChanges();
        void RemoveInventorySegmentDetails(List<SGMarketDealerSegmentDetail> deletedList);
        void AddMarketSegmentDetail(SGMarketDealerSegmentDetail marketDealerSegmentDetail);
    }
}
