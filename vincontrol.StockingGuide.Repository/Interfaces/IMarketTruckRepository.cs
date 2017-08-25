using System.Linq;
using vincontrol.Data.Model.Truck;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface IMarketTruckRepository
    {
        IQueryable<CommercialTruck> GetCarsWithin100MilesOnMarket(double longitude, double lattitude);
    }
}
