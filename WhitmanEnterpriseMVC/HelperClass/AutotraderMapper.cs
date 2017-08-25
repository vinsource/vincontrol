using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhitmanEnterpriseMVC.Interfaces;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class AutotraderMapper
    {
        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2013> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2012> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2011> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2010> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2009> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2008> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2007> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_year2006> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_from_year2004_to_year2005> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_from_year2001_to_year2003> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<autotrader_lessorequal_year2000> objectSet)
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
                Latitude = i.Latitude,
                Longitude = i.Longitude,
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
                Year = i.Year
            });
        }
       
    }
}
