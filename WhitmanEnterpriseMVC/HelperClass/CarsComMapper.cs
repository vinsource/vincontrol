using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhitmanEnterpriseMVC.Interfaces;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class CarsComMapper
    {
        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_lessorequal_year2000> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL=i.CarsComThumbnailURL,
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

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_from_year2001_to_year2003> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_from_year2004_to_year2005> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2006> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2007> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        internal static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2008> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        internal static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2009> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        internal static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2010> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        internal static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2011> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        internal static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2012> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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

        internal static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<carscom_year2013> objectQuery)
        {
            return objectQuery.Select(i => new MarketCarInfo()
            {
                Address = i.Address,
                AutoTrader = i.AutoTrader,
                AutoTraderListingURL = i.AutoTraderListingURL,
                AutoTraderStockNo = i.AutoTraderStockNo,
                AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
                CarsComThumbnailURL = i.CarsComThumbnailURL,
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
