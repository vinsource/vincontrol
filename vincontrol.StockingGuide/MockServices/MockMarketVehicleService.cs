using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;

namespace vincontrol.StockingGuide.MockServices
{
    public class MockMarketVehicleService : IMarketVehicleService
    {
        public List<MakeModelQuantityPrice> GetCarOnMarketWithin100MilesByMakeModel(double longitude, double latitude)
        {
            return new List<MakeModelQuantityPrice>();
        }
    }
}
