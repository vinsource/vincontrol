using System.Data.Objects;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class MarketDetailWithTrimRepository 
        //: SqlRepository<SGMarketDetailWithTrim>, IMarketDetailWithTrimRepository
    {
        public MarketDetailWithTrimRepository(ObjectContext context) 
            //: base(context)
        {
        }
    }
}
