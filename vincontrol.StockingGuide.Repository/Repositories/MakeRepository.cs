using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class MakeRepository:SqlRepository<Make>, IMakeRepository
    {
        public MakeRepository(ObjectContext context) : base(context)
        {
        }
    }
}
