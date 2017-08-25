using System;
using System.Data.Objects;
using System.Linq;
using EmployeeData.Custom;
using vincontrol.Data.Model.Truck;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class SoldMarketTruckRepository : SqlRepository<CommercialTruckSoldOut>, ISoldMarketTruckRepository
    {
        private ObjectContext _context;

        public SoldMarketTruckRepository(ObjectContext context) : base(context)
        {
        }

        public IQueryable<CommercialTruckSoldOut> GetSoldVehicleForMonth(DateTime dateTime, int dealerId)
        {
            var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var endDateOfTheMonth = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).AddDays(1);
            return _objectSet.Where(i => i.DateStamp >= startDateOfTheMonth && i.DateStamp < endDateOfTheMonth && i.DealerId == dealerId);
        }

        public IQueryable<CommercialTruckSoldOut> GetSoldVehicleForMonthWithin100Miles(DateTime dateTime, double longitude, double lattitude)
        {
            var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var endDateOfTheMonth = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).AddDays(1);
            return GetCarsWithin100MilesOnMarketQuery(longitude, lattitude).Where(i => i.DateStamp >= startDateOfTheMonth && i.DateStamp < endDateOfTheMonth);
        }

        //public IQueryable<yearsold> GetSoldVehicleForMonthWithMakeModel(DateTime dateTime, int dealerId, string make, string model)
        //{
        //    var startDateOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        //    var endDateOfTheMonth = (new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))).AddDays(1);
        //    return _objectSet.Where(i => i.LastUpdated >= startDateOfTheMonth && i.LastUpdated < endDateOfTheMonth && i.VinControlDealerId == dealerId && i.Make ==make && i.Model == model);
        //}


        private IQueryable<CommercialTruckSoldOut> GetCarsWithin100MilesOnMarketQuery(double longitude, double lattitude)
        {
            var minLongtitude = (longitude - 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var maxLongtitude = (longitude + 100.00 / Math.Abs(Math.Cos(LocationHelper.DegreeToRadian(lattitude)) * 69.00));
            var minLatitude = (lattitude - (100.00 / 69.00));
            var maxLatitude = (lattitude + (100.00 / 69.00));

            _objectSet.Context.CommandTimeout = 180;
            var result =
                _objectSet.Where(
                    i => i.CommercialTruckDealer.Longitude <= maxLongtitude &&
                        i.CommercialTruckDealer.Longitude >= minLongtitude && i.CommercialTruckDealer.Latitude >= minLatitude && i.CommercialTruckDealer.Latitude <= maxLatitude);
            return result;
        }

    }
}
