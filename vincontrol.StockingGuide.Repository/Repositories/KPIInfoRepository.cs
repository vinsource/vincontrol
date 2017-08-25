using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeData.Custom;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class KPIInfoRepository:SqlRepository<SGKPIInfo>, IKPIInfoRepository
    {
        public KPIInfoRepository(ObjectContext context) : base(context)
        {
        }
    }
}
