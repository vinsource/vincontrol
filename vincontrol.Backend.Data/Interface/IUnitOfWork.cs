using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Backend.Data.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        //whitmanenterprisewarehouseEntities  VinControlWareHouseContext { get; }
        //IBaseRepository<whitmanenterprisesetting> DealerSetting { get; }
        //IBaseRepository<whitmanenterprisedealergroup> DealerGroup { get; }
        void Commit();
    }
}
