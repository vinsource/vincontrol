using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class DataDealerExportRepository: SqlRepository<DataFeedDealer>, IDataDealerExportRepository
    {
        public DataDealerExportRepository(ObjectContext context)
            : base(context)
        {
        }
    }
}
