using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class MarketVehicleRepository : SqlRepository<year>, IMarketVehicleRepository
    {
        public MarketVehicleRepository(ObjectContext context)
            : base(context)
        {
        }

        public IQueryable<year> GetCarsWithin100MilesOnMarket(double longitude, double lattitude)
        {
            return GetCarsWithin100MilesOnMarketQuery(longitude, lattitude);
        }

        private IQueryable<year> GetCarsWithin100MilesOnMarketQuery(double longitude, double lattitude)
        {
            var minLongtitude = (longitude - 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var maxLongtitude = (longitude + 87.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var minLatitude = (lattitude - (100.00 / 69.00));
            var maxLatitude = (lattitude + (87.00 / 69.00));

            _objectSet.Context.CommandTimeout = 180;
           
            var currentYear = DateTime.Now.Year;

         
            var result =
                _objectSet.Where(i => i.Year1 > currentYear - 5 && !String.IsNullOrEmpty(i.Model) && i.Longitude.Value <= maxLongtitude &&
                        i.Longitude >= minLongtitude && i.Latitude >= minLatitude && i.Latitude <= maxLatitude);
            return result;
           
        }
    }
}
