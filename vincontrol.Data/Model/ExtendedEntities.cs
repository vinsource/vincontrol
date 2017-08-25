using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Constant;

namespace vincontrol.Data.Model
{
    #region Class
    public class setting
    {
        public setting() { }

        public setting(Setting obj)
        {
            CarFax = obj.CarFax;
            CarFaxPassword = obj.CarFaxPassword;

            Manheim = obj.Manheim;
            ManheimPassword = obj.ManheimPassword;

            KellyBlueBook = obj.KellyBlueBook;
            KellyPassword = obj.KellyPassword;

            BlackBook = obj.BlackBook;
            BlackBookPassword = obj.BlackBookPassword;
        }

        public string CarFax { get; set; }
        public string CarFaxPassword { get; set; }

        public string Manheim { get; set; }
        public string ManheimPassword { get; set; }

        public string KellyBlueBook { get; set; }
        public string KellyPassword { get; set; }

        public string BlackBook { get; set; }
        public string BlackBookPassword { get; set; }
    }

    public class manheim_regions_auctions_summarize
    {
        public string State { get; set; }
        //Means Number Of Auctions
        public int NumberOfRegions { get; set; }
        public IEnumerable<auctions_summarize> Auctions { get; set; }
    }

    public class auctions_summarize
    {
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public int NumberofVehicles { get; set; }
    }

    public class ManheimVehicle
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Mileage { get; set; }
        public double MMR { get; set; }
        public int Id { get; set; }
        public string Images { get; set; }
        public bool AsIs { get; set; }
        public string Seller { get; set; }
        public string Vin { get; set; }
    }
    #endregion

    #region Partial Class
    public partial class manheim_vehicles
    {
        public string AuctionName { get; set; }
        public bool IsFavorite { get; set; }
        public bool HasNote { get; set; }

        public manheim_vehicles() { }

        public manheim_vehicles(manheim_vehicles vehicle)
        {
            Id = vehicle.Id;
            Vin = vehicle.Vin;
            Year = vehicle.Year;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Trim = vehicle.Trim;
            Mileage = vehicle.Mileage;
            FuelType = vehicle.FuelType;
            Engine = vehicle.Engine;
            Litters = vehicle.Litters;
            Doors = vehicle.Doors;
            BodyStyle = vehicle.BodyStyle;
            VehicleType = vehicle.VehicleType;
            DriveTrain = vehicle.DriveTrain;
            Transmission = vehicle.Transmission;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            AsIs = vehicle.AsIs;
            Cr = vehicle.Cr;
            Mmr = vehicle.Mmr;
            Lane = vehicle.Lane;
            Run = vehicle.Run;
            SaleDate = vehicle.SaleDate;
            Status = vehicle.Status;
            Auction = vehicle.Auction;
            Url = vehicle.Url;
            DateStamp = vehicle.DateStamp;
            Images = vehicle.Images;
            Seller = vehicle.Seller;
            Equipment = vehicle.Equipment;
            MmrAbove = vehicle.MmrAbove;
            MmrBelow = vehicle.MmrBelow;
            Stereo = vehicle.Stereo;
            Airbags = vehicle.Airbags;
            InteriorType = vehicle.InteriorType;
            Category = vehicle.Category;
            LaneDescription = vehicle.LaneDescription;
            CrUrl = vehicle.CrUrl;
        }

        public manheim_vehicles(manheim_vehicles vehicle,IEnumerable<manheim_auctions> auctionList )
        {
            Id = vehicle.Id;
            Vin = vehicle.Vin;
            Year = vehicle.Year;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Trim = vehicle.Trim;
            Mileage = vehicle.Mileage;
            FuelType = vehicle.FuelType;
            Engine = vehicle.Engine;
            Litters = vehicle.Litters;
            Doors = vehicle.Doors;
            BodyStyle = vehicle.BodyStyle;
            VehicleType = vehicle.VehicleType;
            DriveTrain = vehicle.DriveTrain;
            Transmission = vehicle.Transmission;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            AsIs = vehicle.AsIs;
            Cr = vehicle.Cr;
            Mmr = vehicle.Mmr;
            Lane = vehicle.Lane;
            Run = vehicle.Run;
            SaleDate = vehicle.SaleDate;
            Status = vehicle.Status;
            Auction = vehicle.Auction;
            Url = vehicle.Url;
            DateStamp = vehicle.DateStamp;
            Images = vehicle.Images;
            Seller = vehicle.Seller;
            Equipment = vehicle.Equipment;
            MmrAbove = vehicle.MmrAbove;
            MmrBelow = vehicle.MmrBelow;
            Stereo = vehicle.Stereo;
            Airbags = vehicle.Airbags;
            InteriorType = vehicle.InteriorType;
            Category = vehicle.Category;
            LaneDescription = vehicle.LaneDescription;
            CrUrl = vehicle.CrUrl;
            AuctionName = auctionList.FirstOrDefault(x => x.Code == vehicle.Auction).Name;
        }
    }

    public partial class manheim_vehicles_sold
    {
        public string AuctionName { get; set; }
        public bool IsFavorite { get; set; }
        public bool HasNote { get; set; }

        public manheim_vehicles_sold() { }

        public manheim_vehicles_sold(manheim_vehicles_sold vehicle)
        {
            Id = vehicle.Id;
            Vin = vehicle.Vin;
            Year = vehicle.Year;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Trim = vehicle.Trim;
            Mileage = vehicle.Mileage;
            FuelType = vehicle.FuelType;
            Engine = vehicle.Engine;
            Litters = vehicle.Litters;
            Doors = vehicle.Doors;
            BodyStyle = vehicle.BodyStyle;
            VehicleType = vehicle.VehicleType;
            DriveTrain = vehicle.DriveTrain;
            Transmission = vehicle.Transmission;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            AsIs = vehicle.AsIs;
            Cr = vehicle.Cr;
            Mmr = vehicle.Mmr;
            Lane = vehicle.Lane;
            Run = vehicle.Run;
            SaleDate = vehicle.SaleDate;
            Status = vehicle.Status;
            Auction = vehicle.Auction;
            Url = vehicle.Url;
            DateStamp = vehicle.DateStamp;
            Images = vehicle.Images;
            Seller = vehicle.Seller;
            Equipment = vehicle.Equipment;
            MmrAbove = vehicle.MmrAbove;
            MmrBelow = vehicle.MmrBelow;
            Stereo = vehicle.Stereo;
            Airbags = vehicle.Airbags;
            InteriorType = vehicle.InteriorType;
            DateStampSold = vehicle.DateStampSold;
            VehicleId = vehicle.VehicleId;
            Category = vehicle.Category;
            LaneDescription = vehicle.LaneDescription;
            CrUrl = vehicle.CrUrl;
        }

        public manheim_vehicles_sold(manheim_vehicles_sold vehicle,IEnumerable<manheim_auctions> auctionList )
        {
            Id = vehicle.Id;
            Vin = vehicle.Vin;
            Year = vehicle.Year;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Trim = vehicle.Trim;
            Mileage = vehicle.Mileage;
            FuelType = vehicle.FuelType;
            Engine = vehicle.Engine;
            Litters = vehicle.Litters;
            Doors = vehicle.Doors;
            BodyStyle = vehicle.BodyStyle;
            VehicleType = vehicle.VehicleType;
            DriveTrain = vehicle.DriveTrain;
            Transmission = vehicle.Transmission;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            AsIs = vehicle.AsIs;
            Cr = vehicle.Cr;
            Mmr = vehicle.Mmr;
            Lane = vehicle.Lane;
            Run = vehicle.Run;
            SaleDate = vehicle.SaleDate;
            Status = vehicle.Status;
            Auction = vehicle.Auction;
            Url = vehicle.Url;
            DateStamp = vehicle.DateStamp;
            Images = vehicle.Images;
            Seller = vehicle.Seller;
            Equipment = vehicle.Equipment;
            MmrAbove = vehicle.MmrAbove;
            MmrBelow = vehicle.MmrBelow;
            Stereo = vehicle.Stereo;
            Airbags = vehicle.Airbags;
            InteriorType = vehicle.InteriorType;
            DateStampSold = vehicle.DateStampSold;
            VehicleId = vehicle.VehicleId;
            Category = vehicle.Category;
            LaneDescription = vehicle.LaneDescription;
            CrUrl = vehicle.CrUrl;
            AuctionName = auctionList.FirstOrDefault(x => x.Code == vehicle.Auction).Name;
        }
    }

    public partial class User
    {
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string DealerCity { get; set; }
        public string DealerState { get; set; }
        public string DealerZipCode { get; set; }
        public string DealerLatitude { get; set; }
        public string DealerLongtitude { get; set; }
        public Setting Setting { get; set; }
    }
    #endregion
    
    #region Partial Class
    public partial class Appraisal
    {
        public Appraisal() { }
    }

    public partial class Vehicle
    {
        public Vehicle() { }


        public Vehicle(VinControlVehicle vehicle)
        {
            Year = vehicle.Year;
            Make = vehicle.Make ?? String.Empty;
            Model = vehicle.Model ?? String.Empty;
            Trim = vehicle.Trim;
            Vin = String.IsNullOrEmpty(vehicle.VIN)?null:vehicle.VIN.Trim();
            Msrp = vehicle.MSRP;
            InteriorColor = vehicle.InteriorColor ?? String.Empty;
            BodyType = vehicle.BodyType ?? String.Empty;
            Cylinders = vehicle.Cylinders;
            Litter = vehicle.Litter;
            EngineType = vehicle.EngineType ?? String.Empty;
            DriveTrain = vehicle.DriveTrain ?? String.Empty;
            FuelType = vehicle.FuelType ?? String.Empty;
            Tranmission = vehicle.Tranmission ?? String.Empty;
            Doors = vehicle.Doors;
            DefaultStockImage = vehicle.DefaultStockImage ?? String.Empty;
            StandardOptions = vehicle.StandardOptions;
            FuelEconomyCity = vehicle.FuelEconomyCity ?? String.Empty;
            FuelEconomyHighWay = vehicle.FuelEconomyHighWay ?? String.Empty;
            VehicleType = Constanst.VehicleType.Car;
            ModelNumber = String.IsNullOrEmpty(vehicle.ModelNumber) ? null : vehicle.ModelNumber;
            TruckClassId = vehicle.TruckClassId == 0 ? (int?)null : vehicle.TruckClassId;
            
        }
    }

    public partial class AppraisalCustomer
    {
        public AppraisalCustomer() { }
    }
    
    public partial class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(Inventory inventory)
        {
            InventoryId = inventory.InventoryId;
            ACV = inventory.ACV;
            Mileage = inventory.Mileage;
            Stock = inventory.Stock;
            CarfaxOwner = inventory.CarfaxOwner;
            ThumbnailUrl = inventory.ThumbnailUrl;
            DealerId = inventory.DealerId;
            PhotoUrl = inventory.PhotoUrl;
            VehicleId = inventory.VehicleId;
            Unwind = inventory.Unwind;
            RetailPrice = inventory.RetailPrice;
            SalePrice = inventory.SalePrice;
            PriorRental = inventory.PriorRental;
            LastUpdated = inventory.LastUpdated;
            WarrantyInfo = inventory.WarrantyInfo;
            WindowStickerPrice = inventory.WindowStickerPrice;
            ACar = inventory.ACar;
            MarketTrim = inventory.MarketTrim;
            AdditionalOptions = inventory.AdditionalOptions;
            AdditionalPackages = inventory.AdditionalPackages;
            AdditionalTitle = inventory.AdditionalTitle;
            AppraisalId = inventory.AppraisalId;
            BrandedTitle = inventory.BrandedTitle;
            BucketJumpCompleteDay = inventory.BucketJumpCompleteDay;
            CarRanking = inventory.CarRanking;
            Certified = inventory.Certified;
            Condition = inventory.Condition;
            DateInStock = inventory.DateInStock;
            DealerCost = inventory.DealerCost;
            DealerDemo = inventory.DealerDemo;
            DealerDiscount = inventory.DealerDiscount;
            DealerMsrp = inventory.DealerMsrp;
            Descriptions = inventory.Descriptions;
            Disclaimer = inventory.Disclaimer;
            ExteriorColor = inventory.ExteriorColor;
            IsFeatured = inventory.IsFeatured;
            Dealer = inventory.Dealer;
            Vehicle = inventory.Vehicle;
            InventoryStatusCodeId = Constanst.VehicleStatus.Inventory;
        }

        public Inventory(SoldoutInventory soldoutInventory, short inventoryStatus)
        {
            InventoryId = soldoutInventory.SoldoutInventoryId;
            ACV = soldoutInventory.ACV;
            Mileage = soldoutInventory.Mileage;
            Stock = soldoutInventory.Stock;
            CarfaxOwner = soldoutInventory.CarFaxOwner;
            ThumbnailUrl = soldoutInventory.ThumbnailUrl;
            DealerId = soldoutInventory.DealerId;
            PhotoUrl = soldoutInventory.PhotoUrl;
            VehicleId = soldoutInventory.VehicleId;
            Unwind = soldoutInventory.Unwind;
            RetailPrice = soldoutInventory.RetailPrice;
            SalePrice = soldoutInventory.SalePrice;
            PriorRental = soldoutInventory.PriorRental;
            LastUpdated = soldoutInventory.LastUpdated;
            WarrantyInfo = soldoutInventory.WarrantyInfo;
            WindowStickerPrice = soldoutInventory.WindowStickerPrice;
            ACar = soldoutInventory.ACar;
            MarketTrim = soldoutInventory.MarketTrim;
            AdditionalOptions = soldoutInventory.AdditionalOptions;
            AdditionalPackages = soldoutInventory.AdditionalPackages;
            AdditionalTitle = soldoutInventory.AdditionalTitle;
            AppraisalId = soldoutInventory.AppraisalId;
            BrandedTitle = soldoutInventory.BrandedTitle;
            BucketJumpCompleteDay = soldoutInventory.BucketJumpCompleteDay;
            CarRanking = soldoutInventory.CarRanking;
            Certified = soldoutInventory.Certified;
            Condition = soldoutInventory.Condition;
            DateInStock = soldoutInventory.DateInStock;
            DealerCost = soldoutInventory.DealerCost;
            DealerDemo = soldoutInventory.DealerDemo;
            DealerDiscount = soldoutInventory.DealerDiscount;
            DealerMsrp = soldoutInventory.DealerMsrp;
            Descriptions = soldoutInventory.Descriptions;
            Disclaimer = soldoutInventory.Disclaimer;
            ExteriorColor = soldoutInventory.ExteriorColor;
            IsFeatured = soldoutInventory.IsFeatured;
            Dealer = soldoutInventory.Dealer;
            Vehicle = soldoutInventory.Vehicle;
            InventoryStatusCodeId = inventoryStatus;
        }

        public Inventory(SoldoutInventory soldoutInventory, VinControlVehicle vehicle)
        {
            InventoryId = soldoutInventory.SoldoutInventoryId;
            ACV = soldoutInventory.ACV;
            Mileage = vehicle.Mileage;
            Stock = vehicle.StockNumber;
            CarfaxOwner = null;
            ThumbnailUrl = soldoutInventory.ThumbnailUrl;
            DealerId = soldoutInventory.DealerId;
            PhotoUrl = soldoutInventory.PhotoUrl;
            VehicleId = soldoutInventory.VehicleId;
            RetailPrice = soldoutInventory.RetailPrice;
            SalePrice = vehicle.SalePrice;
            PriorRental = soldoutInventory.PriorRental;
            LastUpdated = soldoutInventory.LastUpdated;
            ACar = soldoutInventory.ACar;
            MarketTrim = soldoutInventory.MarketTrim;
            AdditionalOptions = soldoutInventory.AdditionalOptions;
            AdditionalPackages = soldoutInventory.AdditionalPackages;
            OptionCodes = soldoutInventory.OptionCodes;
            AdditionalTitle = soldoutInventory.AdditionalTitle;
            AppraisalId = soldoutInventory.AppraisalId;
            BrandedTitle = soldoutInventory.BrandedTitle;
            DateInStock = vehicle.DateInStock;
            Descriptions = soldoutInventory.Descriptions;
            ExteriorColor = soldoutInventory.ExteriorColor;
            Dealer = soldoutInventory.Dealer;
            Vehicle = soldoutInventory.Vehicle;
            InventoryStatusCodeId = vehicle.InventoryStatus;
            ExternalPhotoUrl = soldoutInventory.ExternalPhotoUrl;
            Certified = vehicle.Certified;
        }

        public Inventory(VinControlVehicle vehicle, int vehicleId, VinControlDealer dealer, short inventoryStatus)
        {
            InventoryId = vehicle.ListingId;
            ACV = vehicle.ACV;
            Mileage = vehicle.Mileage;
            Stock = vehicle.StockNumber;
            CarfaxOwner = vehicle.CarFaxOwner;
            ThumbnailUrl = vehicle.ThumbnailUrl;
            DealerId = dealer.DealerId;
            PhotoUrl = vehicle.PhotoUrl;
            VehicleId = vehicleId;
            Unwind = vehicle.Unwind;
            SalePrice = vehicle.SalePrice;
            PriorRental = vehicle.PriorRental;
            LastUpdated = DateTime.Now;
            WindowStickerPrice = vehicle.WindowStickerPrice;
            AdditionalOptions = vehicle.AdditionalOptions;
            AdditionalPackages = vehicle.AdditionalPackages;
            Certified = vehicle.Certified;
            Condition = vehicle.Condition;
            DateInStock = vehicle.DateInStock;
            DealerCost = vehicle.DealerCost;
            DealerDemo = vehicle.DealerDemo;
            DealerDiscount = vehicle.DealerDiscount;
            DealerMsrp = vehicle.DealerMSRP;
            Descriptions = vehicle.Descriptions;
            Disclaimer = vehicle.Disclaimer;
            ExteriorColor = vehicle.ExteriorColor;
            DealerId = vehicle.DealerId;
            InventoryStatusCodeId = inventoryStatus;
            Invoice = vehicle.Invoice;
            WindowStickerPrice = vehicle.WindowStickerPrice;
            DealerMsrp = vehicle.DealerMSRP;
            ExternalPhotoUrl = vehicle.ExternalPhotoUrl;

        }

        public Inventory(VinControlVehicle vehicle, Vehicle databasevehicle, VinControlDealer dealer, short inventoryStatus)
        {
            InventoryId = vehicle.ListingId;
            ACV = vehicle.ACV;
            Mileage = vehicle.Mileage;
            Stock = vehicle.StockNumber;
            CarfaxOwner = vehicle.CarFaxOwner;
            ThumbnailUrl = vehicle.ThumbnailUrl;
            DealerId = dealer.DealerId;
            PhotoUrl = vehicle.PhotoUrl;
            VehicleId = databasevehicle.VehicleId;
            Unwind = vehicle.Unwind;
            SalePrice = vehicle.SalePrice;
            PriorRental = vehicle.PriorRental;
            LastUpdated = DateTime.Now;
            WindowStickerPrice = vehicle.WindowStickerPrice;
            AdditionalOptions = vehicle.AdditionalOptions;
            AdditionalPackages = vehicle.AdditionalPackages;
            Certified = vehicle.Certified;
            Condition = vehicle.Condition;
            DateInStock = vehicle.DateInStock;
            DealerCost = vehicle.DealerCost;
            DealerDemo = vehicle.DealerDemo;
            DealerDiscount = vehicle.DealerDiscount;
            DealerMsrp = vehicle.DealerMSRP;
            Descriptions = vehicle.Descriptions;
            Disclaimer = vehicle.Disclaimer;
            ExteriorColor = vehicle.ExteriorColor;
            Vehicle = databasevehicle;
            DealerId = vehicle.DealerId;
            InventoryStatusCodeId = inventoryStatus;

        }
        
        public Inventory(Appraisal appraisal, VinControlVehicle vehicle, short inventoryStatus)
        {
            InventoryId = appraisal.AppraisalId;
            AppraisalId = appraisal.AppraisalId;
            ACV = vehicle.ACV;
            Mileage = vehicle.Mileage;
            Stock = vehicle.StockNumber;
            Mileage = vehicle.Mileage;
            Stock = vehicle.StockNumber;
            CarfaxOwner = vehicle.CarFaxOwner;
            ThumbnailUrl = vehicle.ThumbnailUrl;
            DealerId = vehicle.DealerId;
            PhotoUrl = vehicle.PhotoUrl;
            VehicleId = appraisal.VehicleId;
            Unwind = vehicle.Unwind;
            SalePrice = vehicle.SalePrice;
            PriorRental = vehicle.PriorRental;
            LastUpdated = DateTime.Now;
            WindowStickerPrice = vehicle.WindowStickerPrice;
            AdditionalOptions = vehicle.AdditionalOptions;
            AdditionalPackages = vehicle.AdditionalPackages;
            Certified = vehicle.Certified;
            Condition = vehicle.Condition;
            DateInStock = vehicle.DateInStock;
            DealerCost = vehicle.DealerCost;
            DealerDemo = vehicle.DealerDemo;
            DealerDiscount = vehicle.DealerDiscount;
            DealerMsrp = vehicle.DealerMSRP;
            Descriptions = vehicle.Descriptions;
            Disclaimer = vehicle.Disclaimer;
            ExteriorColor = vehicle.ExteriorColor;
            DealerId = vehicle.DealerId;
            InventoryStatusCodeId = inventoryStatus;

        }
    }
    
    public partial class SoldoutInventory
    {
        public SoldoutInventory()
        {
        }

        public SoldoutInventory(Inventory inventory, string userName)
        {

            ACV = inventory.ACV;
            Mileage = inventory.Mileage.GetValueOrDefault();
            Stock = inventory.Stock;
            CarFaxOwner = inventory.CarfaxOwner;
            InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale;
            ThumbnailUrl = inventory.ThumbnailUrl;
            DealerId = inventory.DealerId;
            PhotoUrl = inventory.PhotoUrl;
            VehicleId = inventory.VehicleId;
            Unwind = inventory.Unwind;
            RetailPrice = inventory.RetailPrice;
            SalePrice = inventory.SalePrice;
            PriorRental = inventory.PriorRental;
            LastUpdated = inventory.LastUpdated;
            WarrantyInfo = inventory.WarrantyInfo;
            WindowStickerPrice = inventory.WindowStickerPrice;
            ACar = inventory.ACar;
            MarketTrim = inventory.MarketTrim;
            AdditionalOptions = inventory.AdditionalOptions;
            AdditionalPackages = inventory.AdditionalPackages;
            AdditionalTitle = inventory.AdditionalTitle;
            AppraisalId = inventory.AppraisalId;
            BrandedTitle = inventory.BrandedTitle;
            BucketJumpCompleteDay = inventory.BucketJumpCompleteDay;
            CarRanking = inventory.CarRanking;
            Certified = inventory.Certified;
            Condition = inventory.Condition;
            DateInStock = inventory.DateInStock;
            DealerCost = inventory.DealerCost;
            DealerDemo = inventory.DealerDemo;
            DealerDiscount = inventory.DealerDiscount;
            DealerMsrp = inventory.DealerMsrp;
            Descriptions = inventory.Descriptions;
            Disclaimer = inventory.Disclaimer;
            ExteriorColor = inventory.ExteriorColor;
            ManufacturerRebate = inventory.ManufacturerRebate;
            IsFeatured = inventory.IsFeatured;
            RemoveBy = userName;
            DateRemoved = DateTime.Now;
            TempOldListingId = inventory.InventoryId;
            EnableAutoDescription = inventory.EnableAutoDescription;
            ExternalPhotoUrl = inventory.ExternalPhotoUrl;





        }

        public SoldoutInventory(Appraisal appraisal, string userName)
        {
            AppraisalId = appraisal.AppraisalId;
            ACV = appraisal.ACV;
            Mileage = appraisal.Mileage.GetValueOrDefault();
            Stock = appraisal.Stock;
            CarFaxOwner = appraisal.CARFAXOwner;
            InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale;
            ThumbnailUrl = appraisal.ThumbnailUrl;
            DealerId = appraisal.DealerId;
            PhotoUrl = appraisal.PhotoUrl;
            VehicleId = appraisal.VehicleId;
            //Unwind = inventory.Unwind;
            //RetailPrice = inventory.RetailPrice;
            SalePrice = appraisal.SalePrice;
            //PriorRental = inventory.PriorRental;
            LastUpdated = appraisal.LastUpdated;
            //WarrantyInfo = inventory.WarrantyInfo;
            //WindowStickerPrice = inventory.WindowStickerPrice;
            //ACar = inventory.ACar;
            MarketTrim = appraisal.MarketTrim;
            //AddToInventoryBy = inventory.AddToInventoryBy;
            AdditionalOptions = appraisal.AdditionalOptions;
            AdditionalPackages = appraisal.AdditionalPackages;
            //AdditionalTitle = inventory.AdditionalTitle;
            AppraisalId = appraisal.AppraisalId;
            //BrandedTitle = inventory.BrandedTitle;
            //BucketJumpCompleteDay = inventory.BucketJumpCompleteDay;
            CarRanking = appraisal.CarRanking;
            Certified = appraisal.Certified;
            //Condition = inventory.Condition;
            //DateInStock = inventory.DateInStock;
            DealerCost = appraisal.DealerCost;
            //DealerDemo = inventory.DealerDemo;
            //DealerDiscount = inventory.DealerDiscount;
            //DealerMsrp = inventory.DealerMsrp;
            Descriptions = appraisal.Descriptions;
            //Disclaimer = inventory.Disclaimer;
            ExteriorColor = appraisal.ExteriorColor;
            //ManufacturerRebate = inventory.ManufacturerRebate;
            //IsFeatured = inventory.IsFeatured;
            RemoveBy = userName;
            DateRemoved = DateTime.Now;



        }

    }

    public partial class yearsold
    {
        public yearsold()
        {

        }

        public yearsold(year vehicle)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year1;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault();
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault();
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }

        public yearsold(region1_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }


        public yearsold(region2_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region3_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region4_p1_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region4_p2_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region5_p1_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region5_p2_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region6_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region7_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region8_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region9_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region10_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }


        public yearsold(region1_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region2_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region3_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region4_p1_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region4_p2_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region5_p1_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region5_p2_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region6_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region7_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region8_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region9_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public yearsold(region10_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }


    }

    public partial class marketdata
    {
        public marketdata() { }

        public marketdata(VehicleStandardModel vehicle)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.AutoTraderDealerName;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude;
            Longitude = vehicle.Longitude;
        }

        public marketdata(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public marketdata(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
        
        public bool? AutoTrader { get; set; }
        public bool? CarsCom { get; set; }
        public int? AutoTraderDealerId { get; set; }
        public string AutoTraderDescription { get; set; }
        public string AutoTraderInstalledFeatures { get; set; }
        public int? AutoTraderListingId { get; set; }
        public string AutoTraderListingName { get; set; }
        public string AutoTraderListingURL { get; set; }
        public string AutoTraderStockNo { get; set; }
        public string AutoTraderThumbnailURL { get; set; }
        public int? CarsComDealerId { get; set; }
        public string CarsComDescription { get; set; }
        public string CarsComInstalledFeatures { get; set; }
        public string CarsComListingId { get; set; }
        public string CarsComListingName { get; set; }
        public string CarsComListingURL { get; set; }
        public string CarsComStockNo { get; set; }
        public string CarsComThumbnailURL { get; set; }
        public int? VinControlDealerId { get; set; }
        public string Vin { get; set; }
        public string BodyStyle { get; set; }
        public int? CarFaxType { get; set; }
        public string CarFaxURL { get; set; }
        public string AutoCheckURL { get; set; }
        public bool? Certified { get; set; }
        public int? StartingPrice { get; set; }
        public int? CurrentPrice { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? LastUpdatedPrice { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Doors { get; set; }
        public string DriveType { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public bool? Navigation { get; set; }
        public bool? SunRoof { get; set; }
        public bool? MoonRoof { get; set; }
        public string Tranmission { get; set; }
        public bool? UsedNew { get; set; }
        public int? Mileage { get; set; }
        public string Engine { get; set; }
        public string FuelType { get; set; }
        public string ZipCode { get; set; }
        public string Dealershipname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CountyName { get; set; }
        public string State { get; set; }
        public bool? Franchise { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }

    public partial class region1_autotrader
    {
        public region1_autotrader()
        {

        }

        public region1_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region1_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region2_autotrader
    {
        public region2_autotrader()
        {

        }

        public region2_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region2_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region3_autotrader
    {
        public region3_autotrader()
        {

        }

        public region3_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
        
        public region3_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region4_p1_autotrader
    {
        public region4_p1_autotrader()
        {

        }

        public region4_p1_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region4_p1_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region4_p2_autotrader
    {
        public region4_p2_autotrader()
        {

        }

        public region4_p2_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region4_p2_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region5_p1_autotrader
    {
        public region5_p1_autotrader()
        {

        }

        public region5_p1_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region5_p1_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region5_p2_autotrader
    {
        public region5_p2_autotrader()
        {

        }

        public region5_p2_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region5_p2_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region6_autotrader
    {
        public region6_autotrader()
        {

        }

        public region6_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region6_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region7_autotrader
    {
        public region7_autotrader()
        {

        }

        public region7_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region7_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region8_autotrader
    {
        public region8_autotrader()
        {

        }

        public region8_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region8_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region9_autotrader
    {
        public region9_autotrader()
        {

        }

        public region9_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region9_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region9_new_autotrader
    {
        public region9_new_autotrader()
        {

        }

        public region9_new_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region10_autotrader
    {
        public region10_autotrader()
        {

        }

        public region10_autotrader(VehicleStandardModel vehicle, auotraderdealerview dealer)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId =
                vehicle.AutoTraderDealerId;
            AutoTraderDescription =
                vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures =
                vehicle.
                    AutoTraderInstalledFeatures;
            AutoTraderListingId =
                vehicle.AutoTraderListingId;
            AutoTraderListingName =
                vehicle.AutoTraderListingName;
            AutoTraderListingURL =
                vehicle.AutoTraderListingURL;
            AutoTraderStockNo =
                vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL =
                vehicle.AutoTraderThumbnailURL;
            CarsComDealerId =
                dealer.CarsComId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = DataCommonHelper.RemoveSpecialCharactersWithSpace(
                vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = DataCommonHelper.RemoveSpecialCharactersWithSpace(vehicle.InteriorColor);
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.AutoTraderDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }

        public region10_autotrader(yearsold vehicle, int listingId)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = listingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = vehicle.LastUpdated;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = false;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = false;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }
    }

    public partial class region1_carscom
    {
        public region1_carscom()
        {

        }

        public region1_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region2_carscom
    {
        public region2_carscom()
        {

        }

        public region2_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region3_carscom
    {
        public region3_carscom()
        {

        }

        public region3_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region4_p1_carscom
    {
        public region4_p1_carscom()
        {

        }

        public region4_p1_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region4_p2_carscom
    {
        public region4_p2_carscom()
        {

        }

        public region4_p2_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region5_p1_carscom
    {
        public region5_p1_carscom()
        {

        }

        public region5_p1_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region5_p2_carscom
    {
        public region5_p2_carscom()
        {

        }

        public region5_p2_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region6_carscom
    {
        public region6_carscom()
        {

        }

        public region6_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region7_carscom
    {
        public region7_carscom()
        {

        }

        public region7_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region8_carscom
    {
        public region8_carscom()
        {

        }

        public region8_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region9_carscom
    {
        public region9_carscom()
        {

        }

        public region9_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class region10_carscom
    {
        public region10_carscom()
        {

        }

        public region10_carscom(VehicleStandardModel vehicle, carscomdealer dealer)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = dealer.AutoTraderId;
            VinControlDealerId = vehicle.VinControlDealerId;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.Trim);
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor =
                DataCommonHelper.
                    RemoveSpecialCharactersWithSpace(
                        vehicle.ExteriorColor);
            InteriorColor = "";
            Navigation = vehicle.Navigation;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = dealer.ZipCode.ToString();
            Dealershipname =
                dealer.CarsComDealerName;
            Address = dealer.Address;
            City = dealer.City;
            CountyName = dealer.CountyName;
            State = dealer.State;
            Franchise = dealer.Franchise;
            Latitude = dealer.Latitude;
            Longitude = dealer.Longitude;
        }
    }

    public partial class year
    {
        public year()
        {

        }

        public year(CarMaxVehicle vehicle)
        {
            AutoTrader = false;
            CarsCom = false;
            Carmax = true;
            CarsComDealerId = null;
            CarsComDescription =null;
            CarsComInstalledFeatures =null;
            CarsComListingId = null;
            CarsComListingName = null;
            CarsComListingURL = null;
            CarsComStockNo = null;
            CarsComThumbnailURL = null;
            AutoTraderDealerId = null;
            VinControlDealerId = null;
            Vin = vehicle.Vin;
            BodyStyle = null;
            CarFaxType = null;
            CarFaxURL = null;
            AutoCheckURL = null;
            Certified = null;
            StartingPrice = vehicle.Price;
            CurrentPrice = vehicle.Price;
            DateAdded = vehicle.CreatedDate;
            LastUpdated = vehicle.UpdatedDate;
            LastUpdatedPrice = vehicle.UpdatedDate;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year1 = vehicle.Year;
            Trim =vehicle.Trim;
            Doors = null;
            DriveType = vehicle.DriveTrain;
            ExteriorColor =vehicle.ExteriorColor;
            InteriorColor = "";
            Navigation = null;
            SunRoof = null;
            MoonRoof = null;
            Tranmission = null;
            UsedNew = 0;
            Mileage = vehicle.Miles;
            Engine = null;
            FuelType = null;
            ZipCode = vehicle.CarMaxStore.ZipCode.ToString(CultureInfo.InvariantCulture);
            Dealershipname = "Carmax " + vehicle.CarMaxStore.FullName;
            Address = vehicle.CarMaxStore.Address;
            City = vehicle.CarMaxStore.City;
            State = vehicle.CarMaxStore.State;
            Franchise = false;
            Latitude = vehicle.CarMaxStore.Latitude;
            Longitude = vehicle.CarMaxStore.Longitude;
            CarmaxStoreId = vehicle.CarMaxStore.StoreId;
            CarMaxVehicleId = vehicle.CarMaxVehicleId;
            CarmaxInstalledFeature = vehicle.Features;
            CarmaxListingUrl = vehicle.Url;
            CarmaxThumbnailUrl = string.Empty;
        }

        public year(yearsold vehicle)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DateTime.Now;
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year1 = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault();
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault();
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }

      
    }

    public partial class yearsold
    {
       
        public yearsold(CarMaxVehicleSoldOut vehicle)
        {
            AutoTrader = false;
            CarsCom = false;
            CarsComDealerId = null;
            CarsComDescription = null;
            CarsComInstalledFeatures = null;
            CarsComListingId = null;
            CarsComListingName = null;
            CarsComListingURL = null;
            CarsComStockNo = null;
            CarsComThumbnailURL = null;
            AutoTraderDealerId = null;
            VinControlDealerId = null;
            Vin = vehicle.Vin;
            BodyStyle = null;
            CarFaxType = null;
            CarFaxURL = null;
            AutoCheckURL = null;
            Certified = null;
            StartingPrice = vehicle.Price;
            CurrentPrice = vehicle.Price;
            DateAdded = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = null;
            DriveType = vehicle.DriveTrain;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = "";
            Navigation = null;
            SunRoof = null;
            MoonRoof = null;
            Tranmission = null;
            UsedNew = 0;
            Mileage = vehicle.Miles;
            Engine = null;
            FuelType = null;
            ZipCode = vehicle.CarMaxStore.ZipCode.ToString(CultureInfo.InvariantCulture);
            Dealershipname = "Carmax " + vehicle.CarMaxStore.FullName;
            Address = vehicle.CarMaxStore.Address;
            City = vehicle.CarMaxStore.City;
            State = vehicle.CarMaxStore.State;
            Franchise = false;
            Latitude = vehicle.CarMaxStore.Latitude;
            Longitude = vehicle.CarMaxStore.Longitude;
          
        }
    }

     public partial class region_dailysold
    {
        public region_dailysold()
        {

        }

        public region_dailysold(year vehicle)
        {
            AutoTrader = vehicle.AutoTrader;
            CarsCom = vehicle.CarsCom;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId;
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year1;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault();
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault();
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
        }

        public region_dailysold(region1_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region2_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region3_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region4_p1_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region4_p2_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region5_p1_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region5_p2_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region6_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region7_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region8_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region9_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region10_autotrader vehicle)
        {
            AutoTrader = true;
            CarsCom = false;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            AutoTraderDescription = vehicle.AutoTraderDescription;
            AutoTraderInstalledFeatures = vehicle.AutoTraderInstalledFeatures;
            AutoTraderListingId = vehicle.AutoTraderListingId.ToString();
            AutoTraderListingName = vehicle.AutoTraderListingName;
            AutoTraderListingURL = vehicle.AutoTraderListingURL;
            AutoTraderStockNo = vehicle.AutoTraderStockNo;
            AutoTraderThumbnailURL = vehicle.AutoTraderThumbnailURL;
            CarsComDealerId = vehicle.CarsComDealerId.GetValueOrDefault();
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region1_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region2_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region3_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region4_p1_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region4_p2_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region5_p1_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region5_p2_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region6_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region7_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region8_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region9_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }

        public region_dailysold(region10_carscom vehicle)
        {
            AutoTrader = false;
            CarsCom = true;
            CarsComDealerId = vehicle.CarsComDealerId;
            CarsComDescription =
                vehicle.CarsComDescription;
            CarsComInstalledFeatures =
                vehicle.CarsComInstalledFeatures;
            CarsComListingId = vehicle.CarsComListingId;
            CarsComListingName =
                vehicle.CarsComListingName;
            CarsComListingURL = vehicle.CarsComListingURL;
            CarsComStockNo = vehicle.CarsComStockNo;
            CarsComThumbnailURL =
                vehicle.CarsComThumbnailURL;
            AutoTraderDealerId = vehicle.AutoTraderDealerId;
            VinControlDealerId = vehicle.VinControlDealerId.GetValueOrDefault();
            Vin = vehicle.Vin;
            BodyStyle = vehicle.BodyStyle;
            CarFaxType = vehicle.CarFaxType;
            CarFaxURL = vehicle.CarFaxURL;
            AutoCheckURL = vehicle.AutoCheckURL;
            Certified = vehicle.Certified;
            StartingPrice = vehicle.StartingPrice;
            CurrentPrice = vehicle.CurrentPrice;
            DateAdded = vehicle.DateAdded;
            LastUpdated = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            LastUpdatedPrice = vehicle.LastUpdatedPrice;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Trim = vehicle.Trim;
            Doors = vehicle.Doors;
            Doors = vehicle.Doors;
            DriveType = vehicle.DriveType;
            ExteriorColor = vehicle.ExteriorColor;
            InteriorColor = vehicle.InteriorColor;
            Navigation = vehicle.Navigation.GetValueOrDefault() ? (short)1 : (short)0;
            SunRoof = vehicle.SunRoof;
            MoonRoof = vehicle.MoonRoof;
            Tranmission = vehicle.Tranmission;
            UsedNew = vehicle.UsedNew.GetValueOrDefault() ? (short)1 : (short)0;
            Mileage = vehicle.Mileage;
            Engine = vehicle.Engine;
            FuelType = vehicle.FuelType;
            ZipCode = vehicle.ZipCode;
            Dealershipname =
                vehicle.Dealershipname;
            Address = vehicle.Address;
            City = vehicle.City;
            CountyName = vehicle.CountyName;
            State = vehicle.State;
            Franchise = vehicle.Franchise;
            Latitude = vehicle.Latitude.GetValueOrDefault();
            Longitude = vehicle.Longitude.GetValueOrDefault();
        }


    }

    #endregion
}
