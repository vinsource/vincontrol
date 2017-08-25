using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class ManheimVehicleRepository: SqlRepository<manheim_vehicles>, IManheimVehicleRepository
    {
        public ManheimVehicleRepository(ObjectContext context) : base(context)
        {
        }
    }
}
