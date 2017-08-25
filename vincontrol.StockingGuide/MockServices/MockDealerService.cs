using System.Collections.Generic;
using System.Configuration;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.MockServices
{
    public class MockDealerService : IDealerService
    {
        //DealerId = 3636, Latitude = (decimal)33.707390, Longitude = (decimal)-117.705377
        //DealerId = 37695, Latitude = (decimal)33.862585, Longitude = (decimal)-117.598903
        private int _dealerId;
        private decimal _latitude;
        private decimal _longitude;

        public MockDealerService()
        {
            _dealerId = int.Parse(ConfigurationManager.AppSettings["DealerId"]);
            _longitude = decimal.Parse(ConfigurationManager.AppSettings["Longitude"]);
            _latitude = decimal.Parse(ConfigurationManager.AppSettings["Latitude"]);
        }

        public IEnumerable<int> GetDealerIdList()
        {
            return new List<int> { _dealerId };
        }

        public List<DealerLocation> GetDealerLocationList()
        {
            return new List<DealerLocation> { new DealerLocation() { DealerId = _dealerId, Latitude = _latitude, Longitude = _longitude } };
        }

        public List<DealerLocation> GetDealerLocationListFromInventory()
        {
            return new List<DealerLocation> { new DealerLocation() { DealerId = _dealerId, Latitude = _latitude, Longitude = _longitude } };

        }

        public List<int> GetDealerIdListFromInventorySegmentDetail()
        {
            return new List<int> { _dealerId };

        }

        public List<int> GetDealerIdListFromInventory()
        {
            return new List<int> { _dealerId };

        }
    }
}
