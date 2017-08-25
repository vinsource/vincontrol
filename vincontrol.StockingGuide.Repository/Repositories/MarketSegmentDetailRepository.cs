using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class MarketSegmentDetailRepository:SqlRepository<SGMarketDealerSegmentDetail>, IMarketSegmentDetailRepository
    {
        public MarketSegmentDetailRepository(ObjectContext context) : base(context)
        {
        }
    }
}
