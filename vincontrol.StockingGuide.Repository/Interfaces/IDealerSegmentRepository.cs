using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface IDealerSegmentRepository: IRepository<SGDealerSegment>
    {
        IQueryable<SGDealerSegment> GetAllDealerSegmentsForDealer(int dealerId, string make = "", string model = "");
    }
}
