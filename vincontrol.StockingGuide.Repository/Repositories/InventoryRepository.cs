using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class InventoryRepository : SqlRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(ObjectContext context) : base(context)
        {
        }
    }
}
