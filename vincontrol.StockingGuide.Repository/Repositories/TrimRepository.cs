using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class TrimRepository: SqlRepository<Trim>, ITrimRepository
    {
        public TrimRepository(ObjectContext context) : base(context)
        {
        }
    }
}
