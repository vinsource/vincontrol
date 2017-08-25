using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.Data.Model.Truck;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface ISoldMarketTruckRepository
    {
        IQueryable<CommercialTruckSoldOut> GetSoldVehicleForMonth(DateTime dateTime, int dealerId);

        IQueryable<CommercialTruckSoldOut> GetSoldVehicleForMonthWithin100Miles(DateTime dateTime,
            double longitude, double lattitude);

    }
}
