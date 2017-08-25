using System;
using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Repository.Interfaces;
using System.Linq;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class SoldMarketVehicleRepository : SqlRepository<yearsold>, ISoldMarketVehicleRepository
    {
        private ObjectContext _context;
        public SoldMarketVehicleRepository(ObjectContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<yearsold> GetSoldVehicleForMonth(DateTime dateTime, int dealerId)
        {
            var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var endDateOfTheMonth = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).AddDays(1);
            return _objectSet.Where(i => i.LastUpdated >= startDateOfTheMonth && i.LastUpdated < endDateOfTheMonth && i.VinControlDealerId == dealerId);
        }

        public IQueryable<yearsold> GetSoldVehicleForMonthWithin100Miles(DateTime dateTime, double longitude, double lattitude)
        {
            var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var endDateOfTheMonth = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).AddDays(1);
            return GetCarsWithin100MilesOnMarketQuery(longitude, lattitude).Where(i => i.LastUpdated >= startDateOfTheMonth && i.LastUpdated < endDateOfTheMonth);
        }
        
        private IQueryable<yearsold> GetCarsWithin100MilesOnMarketQuery(double longitude, double lattitude)
        {
            var minLongtitude = (longitude - 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var maxLongtitude = (longitude + 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var minLatitude = (lattitude - (100.00 / 69.00));
            var maxLatitude = (lattitude + (100.00 / 69.00));

            _objectSet.Context.CommandTimeout = 180;
            var result =
                _objectSet.Where(
                    i => i.Longitude.Value <= maxLongtitude &&
                        i.Longitude >= minLongtitude && i.Latitude >= minLatitude && i.Latitude <= maxLatitude);
            return result;
        }
    }
}
