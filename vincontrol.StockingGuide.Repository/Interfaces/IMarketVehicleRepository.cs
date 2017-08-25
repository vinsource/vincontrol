using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface IMarketVehicleRepository:IRepository<year>
    {
        //List<GetVehiclesWithin100Miles_Result> GetCarsWithin100Miles(decimal longitude, decimal lattitude);
        //int GetNumberOfCarWithin100MilesOnMarket(double longitude, double lattitude, string make, string model);
        //MarketInfo GetNumberOfCarWithin100MilesOnMarket(double longitude, double lattitude, string make, string model, string trim);

        IQueryable<year> GetCarsWithin100MilesOnMarket(double longitude, double lattitude);
    }
}
