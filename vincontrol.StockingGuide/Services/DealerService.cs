using System.Collections.Generic;
using System.Linq;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class DealerService : IDealerService
    {
        IVincontrolUnitOfWork _vincontrolUnitOfWork = new VincontrolUnitOfWork();

        public IEnumerable<int> GetDealerIdList()
        {
            //TODO: Get List dealer
            return _vincontrolUnitOfWork.DealerRepository.Find(i => i.DealerId != 0).Select(i => i.DealerId).ToList();
        }

        public List<DealerLocation> GetDealerLocationList()
        {
            //TODO: Get List Dealer Location
            return _vincontrolUnitOfWork.DealerRepository.FindAll().Select(i => new DealerLocation() { DealerId = i.DealerId, Latitude = i.Lattitude ?? 0, Longitude = i.Longtitude ?? 0 }).ToList();
        }

        public List<DealerLocation> GetDealerLocationListFromInventory()
        {
            return _vincontrolUnitOfWork.ExportRepository.Find(i => i.DealerId > 0).Select(i => new DealerLocation() { DealerId = i.DealerId, Longitude = i.Dealer.Longtitude ?? 0, Latitude = i.Dealer.Lattitude ?? 0 }).Distinct().ToList();
        }

        public List<int> GetDealerIdListFromInventorySegmentDetail()
        {
            return _vincontrolUnitOfWork.ExportRepository.Find(i => i.DealerId > 0).Select(i => i.DealerId).Distinct().ToList();
        }

        public List<int> GetDealerIdListFromInventory()
        {
            return _vincontrolUnitOfWork.ExportRepository.Find(i => i.DealerId>0).Select(i => i.DealerId).Distinct().ToList();
        }
    }
}
