using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IKPIInfoService
    {
        void AddKPIInfos(List<SGKPIInfo> infoList);
    }
}
