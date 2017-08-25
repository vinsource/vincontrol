using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.StockingGuide.Entity.EntityModel.Vincontrol;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class KPIInfoService:IKPIInfoService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public KPIInfoService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public void AddKPIInfos(List<SGKPIInfo> infoList)
        {
            foreach (var item in infoList)
            {
                _vincontrolUnitOfWork.KpiInfoRepository.Add(item);
            }
            _vincontrolUnitOfWork.Commit();
        }
    }
}
