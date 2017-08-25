using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;
using vincontrol.Data.Model;

namespace vincontrol.Helper
{
    public class CarsComMapper
    {
        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year> objectSet)
        {
            return objectSet.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                BodyStyle = i.BodyStyle,
                CarsCom = i.CarsCom,
                CarsComListingURL = i.CarsComListingURL,
                CarsComStockNo = i.CarsComStockNo,
                Certified = i.Certified,
                CurrentPrice = i.CurrentPrice.Value,
                DateAdded = i.DateAdded,
                Dealershipname = i.Dealershipname,
                Doors = i.Doors,
                DriveType = i.DriveType,
                Engine = i.Engine,
                ExteriorColor = i.ExteriorColor,
                Franchise = i.Franchise,
                FuelType = i.FuelType,
                InteriorColor = i.InteriorColor,
                Latitude = i.Latitude.GetValueOrDefault(),
                Longitude = i.Longitude.GetValueOrDefault(),
                Make = i.Make,
                Mileage = i.Mileage.Value,
                Model = i.Model,
                MoonRoof = i.MoonRoof,
                MSRP = i.MSRP,
                RegionalListingId = i.RegionalListingId,
                StartingPrice = i.StartingPrice.Value,
                State = i.State,
                SunRoof = i.SunRoof,
                Tranmission = i.Tranmission,
                Trim = i.Trim,
                Vin = i.Vin,
                Year = i.Year1,
                AutoTraderListingId = i.AutoTraderListingId
            });
        }

    }
}
