using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class ModelRepository : SqlRepository<Model>, IModelRepository
    {
        public ModelRepository(ObjectContext context) : base(context)
        {
        }
    }
}
