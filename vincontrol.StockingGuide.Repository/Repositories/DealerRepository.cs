using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class DealerRepository: SqlRepository<Dealer>, IDealerRepository
    {
        public DealerRepository(ObjectContext context) : base(context)
        {
        }
    }
}
