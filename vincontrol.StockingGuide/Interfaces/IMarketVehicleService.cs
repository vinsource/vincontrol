using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IMarketVehicleService
    {
        //List<GetVehiclesWithin100Miles_Result> GetCarsWithin100Miles(decimal longitude, decimal latitude);
        //int GetNumberOfCarOnMarketWithin100Miles(double longitude, double lattitude, string make, string model);
        //MarketInfo GetNumberOfCarOnMarketWithin100Miles(double longitude, double lattitude, string make, string model, string trim);
        List<MakeModelQuantityPrice> GetCarOnMarketWithin100MilesByMakeModel(double longitude, double latitude);
    }
}
