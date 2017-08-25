using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IMarketTruckService
    {
        List<MakeModelQuantityPrice> GetCarOnMarketWithin100MilesByMakeModel(double longitude, double latitude);
    }
}
