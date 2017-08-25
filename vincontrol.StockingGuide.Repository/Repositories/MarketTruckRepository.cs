using System;
using System.Data.Objects;
using System.Linq;
using EmployeeData.Custom;
using vincontrol.Data.Model.Truck;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class MarketTruckRepository : SqlRepository<CommercialTruck>, IMarketTruckRepository
    {
        public MarketTruckRepository(ObjectContext context) : base(context)
        {
        }

        public IQueryable<CommercialTruck> GetCarsWithin100MilesOnMarket(double longitude, double lattitude)
        {
            return GetCarsWithin100MilesOnMarketQuery(longitude, lattitude);
        }

        private IQueryable<CommercialTruck> GetCarsWithin100MilesOnMarketQuery(double longitude, double lattitude)
        {
            var minLongtitude = (longitude - 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var maxLongtitude = (longitude + 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var minLatitude = (lattitude - (100.00 / 69.00));
            var maxLatitude = (lattitude + (100.00 / 69.00));

            _objectSet.Context.CommandTimeout = 180;
            var result =
                _objectSet.Where(i => i.CommercialTruckDealer.Longitude <= maxLongtitude &&
                   i.CommercialTruckDealer.Longitude >= minLongtitude && i.CommercialTruckDealer.Latitude >= minLatitude && i.CommercialTruckDealer.Latitude <= maxLatitude);
            return result;
        }
    }
}
