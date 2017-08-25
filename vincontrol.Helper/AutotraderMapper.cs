using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;
using vincontrol.Data.Model;

namespace vincontrol.Helper
{
    public class AutotraderMapper
    {

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year> objectSet)
        {
            return objectSet.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsCom = i.CarsCom,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
                Certified = i.Certified,
                CurrentPrice = i.CurrentPrice.Value,
                DateAdded = i.DateAdded,
                Dealershipname = i.Dealershipname,
                ExteriorColor = i.ExteriorColor,
                Franchise = i.Franchise,
                FuelType = i.FuelType,
                InteriorColor = i.InteriorColor,
                Latitude = i.Latitude??0,
                Longitude = i.Longitude??0,
                Make = i.Make,
                Mileage = i.Mileage.Value,
                Model = i.Model,
                RegionalListingId = i.RegionalListingId,
                StartingPrice = i.StartingPrice.Value,
                State = i.State,
                Trim = i.Trim,
                Vin = i.Vin,
                Year = i.Year1,
                AutoTraderListingId = i.AutoTraderListingId,
                CarscomListingId = i.CarsComListingId,
                AutoTraderDealerId = i.AutoTraderDealerId,
                CarMax=i.Carmax,
                CarMaxListingId=i.CarMaxVehicleId,
                CarsMaxThumbnailURL = i.CarmaxThumbnailUrl,
                BodyStyle = i.BodyStyle,
                Highlighted = i.Highlighted

            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(IQueryable<yearsold> objectSet)
        {
            return objectSet.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsCom = i.CarsCom,
                Certified = i.Certified,
                CurrentPrice = i.CurrentPrice.Value,
                DateAdded = i.DateAdded,
                Dealershipname = i.Dealershipname,
                ExteriorColor = i.ExteriorColor,
                Franchise = i.Franchise,
                FuelType = i.FuelType,
                InteriorColor = i.InteriorColor,
                Latitude = i.Latitude??0,
                Longitude = i.Longitude??0,
                Make = i.Make,
                Mileage = i.Mileage.Value,
                Model = i.Model,
                RegionalListingId = i.RegionalListingId,
                StartingPrice = i.StartingPrice.Value,
                State = i.State,
                Trim = i.Trim,
                Vin = i.Vin,
                Year = i.Year,
                AutoTraderListingId = i.AutoTraderListingId,
                CarscomListingId = i.CarsComListingId,
                AutoTraderDealerId = i.AutoTraderDealerId,
                LastUpdatedDate = i.LastUpdated.Value,
                BodyStyle = i.BodyStyle,
                Highlighted=false
            });
        }

       
    }
}
