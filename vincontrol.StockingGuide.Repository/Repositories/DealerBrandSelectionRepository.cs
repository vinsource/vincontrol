using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class DealerBrandSelectionRepository : SqlRepository<SGDealerBrandSelection>, IDealerBrandSelectionRepository
    {
        public DealerBrandSelectionRepository(ObjectContext context)
            : base(context)
        {
            
        }
    }
}
