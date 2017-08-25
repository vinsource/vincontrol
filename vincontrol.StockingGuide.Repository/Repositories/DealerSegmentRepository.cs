using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;
using System.Linq;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class DealerSegmentRepository : SqlRepository<SGDealerSegment>, IDealerSegmentRepository
    {
        public DealerSegmentRepository(ObjectContext context) : base(context)
        {
           
        }

        public IQueryable<SGDealerSegment> GetAllDealerSegmentsForDealer(int dealerId, string make = "", string model = "")
        {
            var data = _objectSet.Include("SGInventoryDealerSegmentDetails").Include("SGMarketDealerSegmentDetails")
                .Where(i => i.DealerId == dealerId
                        //&& 
                        //(make == "" || (i.SGInventoryDealerSegmentDetails.Any() && i.SGMarketDealerSegmentDetails.Any()))
                        //(make == "" || (i.SGInventoryDealerSegmentDetails.Any(xx => xx.Make.Equals(make))) || i.SGMarketDealerSegmentDetails.Any(xx => xx.Make.Equals(make))) 
                        //&& 
                        //(model == "" || (i.SGInventoryDealerSegmentDetails.Any() && i.SGMarketDealerSegmentDetails.Any()))
                        //(model == "" || (i.SGInventoryDealerSegmentDetails.Any(xx => xx.Model.Equals(model)) || i.SGMarketDealerSegmentDetails.Any(xx => xx.Model.Equals(model))))
                        );
            
            return data;
        }
    }
}
