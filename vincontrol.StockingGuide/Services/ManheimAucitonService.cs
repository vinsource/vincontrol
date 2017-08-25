using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class ManheimAuctionService: IManheimAuctionService
    {
        private readonly IVinsellUnitOfWork _vinsellUnitOfWork;

        public ManheimAuctionService()
        {
            _vinsellUnitOfWork = new VinsellUnitOfWork();
        }
        public List<KeyValueObject> GetRegionCodeMapping()
        {
            return _vinsellUnitOfWork.ManheimAuctionRepository.FindAll().Select(i => new KeyValueObject() { Key = i.Code, Value = i.Region }).ToList();
        }
    }
}
