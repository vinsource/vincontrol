using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class DealerBrandRepository : SqlRepository<SGDealerBrand>, IDealerBrandRepository
    {
        public DealerBrandRepository(ObjectContext context) : base(context)
        {
            
        }
    }
}
