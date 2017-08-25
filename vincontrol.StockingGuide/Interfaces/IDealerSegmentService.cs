using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IDealerSegmentService
    {
        void AddDealerSegments(List<SGDealerSegment> dealerSegmentList);
        IQueryable<SGDealerSegment> GetAllDealerSegmentsForDealer(int dealerId, string make = "", string model = "");

        void UpdateWishList(int id, bool isWishList);
        void UpdateGuide(int id, int guide);

        void UpdateGrossPerUnit(int id, int grossPerUnit);
        SGDealerSegment GetDealerSegment(int dealerId, short sgSegmentId);
        void SaveChanges();
        List<DealerSegmentMapping> GetDealerSegmentMapping(int dealerId);
        List<SGDealerSegment> GetDealerSegments(int dealerId);

    }
}
