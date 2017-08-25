using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class InventorySegmentDetailRepository: SqlRepository<SGInventoryDealerSegmentDetail>, IInventorySegmentDetailRepository
    {
        public InventorySegmentDetailRepository(ObjectContext context) : base(context)
        {
        }
    }
}
