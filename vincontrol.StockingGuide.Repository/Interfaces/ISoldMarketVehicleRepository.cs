using System;
using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface ISoldMarketVehicleRepository : IRepository<yearsold>
    {
        IQueryable<yearsold> GetSoldVehicleForMonth(DateTime dateTime, int dealerId);
        IQueryable<yearsold> GetSoldVehicleForMonthWithin100Miles(DateTime dateTime, double longitude, double lattitude);
       
    }
}