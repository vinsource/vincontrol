using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class StockingGuideInfoRepository: SqlRepository<SGKPIInfo>, IStockingGuideInfoRepository
    {
        public StockingGuideInfoRepository(ObjectContext ctx): base(ctx)
        {
            
        }
    }
}
