using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class AppraisalRepository : SqlRepository<Appraisal>, IAppraisalRepository
    {
        public AppraisalRepository(ObjectContext context)
            : base(context)
        {
        }
    }
}
