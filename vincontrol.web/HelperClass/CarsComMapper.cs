using System.Linq;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
//using Vincontrol.Web.DatabaseModelScrapping;
//using Vincontrol.Web.Interfaces;


namespace Vincontrol.Web.HelperClass
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
                Latitude = i.Latitude??0,
                Longitude = i.Longitude??0,
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

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2016> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2015> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2014> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2013> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2012> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2011> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2010> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2009> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2008> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2007> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2006> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

        //public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2004_to_year2005> objectSet)
        //{
        //    return objectSet.Select(i => new MarketCarInfo()
        //    {
        //        Address = i.Address,
        //        AutoTrader = i.AutoTrader,
        //        AutoTraderListingURL = i.AutoTraderListingURL,
        //        AutoTraderStockNo = i.AutoTraderStockNo,
        //        AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
        //        BodyStyle = i.BodyStyle,
        //        CarsCom = i.CarsCom,
        //        CarsComListingURL = i.CarsComListingURL,
        //        CarsComStockNo = i.CarsComStockNo,
        //        Certified = i.Certified,
        //        CurrentPrice = i.CurrentPrice.Value,
        //        DateAdded = i.DateAdded,
        //        Dealershipname = i.Dealershipname,
        //        Doors = i.Doors,
        //        DriveType = i.DriveType,
        //        Engine = i.Engine,
        //        ExteriorColor = i.ExteriorColor,
        //        Franchise = i.Franchise,
        //        FuelType = i.FuelType,
        //        InteriorColor = i.InteriorColor,
        //        Latitude = i.Latitude,
        //        Longitude = i.Longitude,
        //        Make = i.Make,
        //        Mileage = i.Mileage.Value,
        //        Model = i.Model,
        //        MoonRoof = i.MoonRoof,
        //        MSRP = i.MSRP,
        //        RegionalListingId = i.RegionalListingId,
        //        StartingPrice = i.StartingPrice.Value,
        //        State = i.State,
        //        SunRoof = i.SunRoof,
        //        Tranmission = i.Tranmission,
        //        Trim = i.Trim,
        //        Vin = i.Vin,
        //        Year = i.Year,
        //        AutoTraderListingId = i.AutoTraderListingId
        //    });
        //}

    //    public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<year2001_to_year2003> objectSet)
    //    {
    //        return objectSet.Select(i => new MarketCarInfo()
    //        {
    //            Address = i.Address,
    //            AutoTrader = i.AutoTrader,
    //            AutoTraderListingURL = i.AutoTraderListingURL,
    //            AutoTraderStockNo = i.AutoTraderStockNo,
    //            AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
    //            BodyStyle = i.BodyStyle,
    //            CarsCom = i.CarsCom,
    //            CarsComListingURL = i.CarsComListingURL,
    //            CarsComStockNo = i.CarsComStockNo,
    //            Certified = i.Certified,
    //            CurrentPrice = i.CurrentPrice.Value,
    //            DateAdded = i.DateAdded,
    //            Dealershipname = i.Dealershipname,
    //            Doors = i.Doors,
    //            DriveType = i.DriveType,
    //            Engine = i.Engine,
    //            ExteriorColor = i.ExteriorColor,
    //            Franchise = i.Franchise,
    //            FuelType = i.FuelType,
    //            InteriorColor = i.InteriorColor,
    //            Latitude = i.Latitude,
    //            Longitude = i.Longitude,
    //            Make = i.Make,
    //            Mileage = i.Mileage.Value,
    //            Model = i.Model,
    //            MoonRoof = i.MoonRoof,
    //            MSRP = i.MSRP,
    //            RegionalListingId = i.RegionalListingId,
    //            StartingPrice = i.StartingPrice.Value,
    //            State = i.State,
    //            SunRoof = i.SunRoof,
    //            Tranmission = i.Tranmission,
    //            Trim = i.Trim,
    //            Vin = i.Vin,
    //            Year = i.Year,
    //            AutoTraderListingId = i.AutoTraderListingId
    //        });
    //    }

    //    public static IQueryable<MarketCarInfo> GetMarketCarInfoQuery(System.Data.Objects.ObjectQuery<yearlessorequal2000> objectSet)
    //    {
    //        return objectSet.Select(i => new MarketCarInfo()
    //        {
    //            Address = i.Address,
    //            AutoTrader = i.AutoTrader,
    //            AutoTraderListingURL = i.AutoTraderListingURL,
    //            AutoTraderStockNo = i.AutoTraderStockNo,
    //            AutoTraderThumbnailURL = i.AutoTraderThumbnailURL,
    //            BodyStyle = i.BodyStyle,
    //            CarsCom = i.CarsCom,
    //            CarsComListingURL = i.CarsComListingURL,
    //            CarsComStockNo = i.CarsComStockNo,
    //            Certified = i.Certified,
    //            CurrentPrice = i.CurrentPrice.Value,
    //            DateAdded = i.DateAdded,
    //            Dealershipname = i.Dealershipname,
    //            Doors = i.Doors,
    //            DriveType = i.DriveType,
    //            Engine = i.Engine,
    //            ExteriorColor = i.ExteriorColor,
    //            Franchise = i.Franchise,
    //            FuelType = i.FuelType,
    //            InteriorColor = i.InteriorColor,
    //            Latitude = i.Latitude,
    //            Longitude = i.Longitude,
    //            Make = i.Make,
    //            Mileage = i.Mileage.Value,
    //            Model = i.Model,
    //            MoonRoof = i.MoonRoof,
    //            MSRP = i.MSRP,
    //            RegionalListingId = i.RegionalListingId,
    //            StartingPrice = i.StartingPrice.Value,
    //            State = i.State,
    //            SunRoof = i.SunRoof,
    //            Tranmission = i.Tranmission,
    //            Trim = i.Trim,
    //            Vin = i.Vin,
    //            Year = i.Year,
    //            AutoTraderListingId = i.AutoTraderListingId
    //        });
    //    }
    }
}
