using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class SettingRepository: SqlRepository<Setting>, ISettingRepository
    {
        public SettingRepository(ObjectContext context) : base(context)
        {
        }
    }
}
