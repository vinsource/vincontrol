using System.Collections.Generic;
using System.Linq;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;
using vincontrol.StockingGuide.Service;

namespace vincontrol.StockingGuide.Services
{
    public class SettingService : ISettingService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public SettingService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public List<string> GetBrandListForDealer(int dealerId)
        {
            var brandNames = _vincontrolUnitOfWork.SettingRepository.Find(i => i.DealerId == dealerId).Select(i => i.BrandName).FirstOrDefault();
            return Parser.GetListBySeparatingCommas(brandNames);
        }

        public List<BrandDealer> GetBrandDealerList()
        {
           return _vincontrolUnitOfWork.SettingRepository.Find(i=>i.BrandName!=null).Select(i => new BrandDealer() {BrandName = i.BrandName, DealerId = i.DealerId}).ToList();
        }

    }
}
