using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;
using vincontrol.DataFeed.Helper;

namespace vincontrol.DataFeed.Model
{
    public class VehicleViewModel
    {
        public int ListingId { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string Vin { get; set; }

        public int DaysInInventory { get; set; }

        public string StockNumber { get; set; }

        public string SalePrice { get; set; }

        public string MSRP { get; set; }

        public string Mileage { get; set; }

        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string InteriorSurface { get; set; }

        public string BodyType { get; set; }

        public string EngineType { get; set; }

        public string DriveTrain { get; set; }

        public string Cylinders { get; set; }

        public string Liters { get; set; }

        public string FuelType { get; set; }

        public string Tranmission { get; set; }

        public string Doors { get; set; }

        public bool Certified { get; set; }

        public string CarsOptions { get; set; }

        public string Descriptions { get; set; }

        public string Disclaimer { get; set; }

        public string CarImageUrl { get; set; }

        public string ThumbnalImageurl { get; set; }

        public DateTime DateInStock { get; set; }

        public string DealershipName { get; set; }

        public string DealershipAddress { get; set; }

        public string DealershipCity { get; set; }

        public string DealershipState { get; set; }

        public string DealershipZipCode { get; set; }

        public string DealershipPhone { get; set; }

        public int DealerId { get; set; }

        public string DealerCost { get; set; }

        public string ACV { get; set; }

        public string DefaultImageUrl { get; set; }

        public string NewUsed { get; set; }

        public int Age { get; set; }

        public string AddToInventoryBy { get; set; }

        public string AppraisalId { get; set; }

        public string DefaultImageURL { get; set; }

        public string FuelEconomyCity { get; set; }

        public string FuelEconomyHighWay { get; set; }

        public string VehicleType { get; set; }
        
        public string TruckType { get; set; }
        
        public string TruckCategory { get; set; }
        
        public string TruckClass { get; set; }
        
        public bool IsTruck { get; set; }
        
        public bool Recon { get; set; }
        
        public int VehicleStatus { get; set; }

        public string StandardOptions { get; set; }

        public string RetailPrice { get; set; }

        public string DealerDiscount { get; set; }

        public string WindowStickerPrice { get; set; }

        public string ManufacturerRebate { get; set; }

        public int CarFaxOwner { get; set; }

        public int KBBTrimId { get; set; }

        public string KBBTrimOption { get; set; }

        public bool WholeSale { get; set; }

        public bool PriorRental { get; set; }

        public VehicleViewModel(){}

        public VehicleViewModel(whitmanenterprisedealershipinventory tmp)
        {
            ListingId = tmp.ListingID;
            Year = tmp.ModelYear.GetValueOrDefault();
            Make = tmp.Make;
            Model = tmp.Model;
            Trim = tmp.Trim;
            Vin = tmp.VINNumber;
            StockNumber = tmp.StockNumber;
            SalePrice = tmp.SalePrice;
            MSRP = tmp.MSRP;
            Mileage = tmp.Mileage;
            ExteriorColor = tmp.ExteriorColor;
            InteriorColor = tmp.InteriorColor;
            InteriorSurface = tmp.InteriorSurface;
            BodyType = tmp.BodyType;
            EngineType = tmp.EngineType;
            DriveTrain = tmp.DriveTrain;
            Cylinders = tmp.Cylinders;
            Liters = tmp.Liters;
            FuelType = tmp.FuelType;
            Tranmission = tmp.Tranmission;
            Doors = tmp.Doors;
            Certified = tmp.Certified.GetValueOrDefault();
            CarsOptions = tmp.CarsOptions;
            Descriptions = tmp.Descriptions;
            Disclaimer = tmp.Disclaimer;
            CarImageUrl = tmp.CarImageUrl;
            ThumbnalImageurl = tmp.ThumbnailImageURL;
            DateInStock = tmp.DateInStock.GetValueOrDefault();
            DealershipName = tmp.DealershipName;
            DealershipAddress = tmp.DealershipAddress;
            DealershipCity = tmp.DealershipCity;
            DealershipState = tmp.DealershipState;
            DealershipZipCode = tmp.DealershipZipCode;
            DealerId = tmp.DealershipId.GetValueOrDefault();
            DealerCost = tmp.DealerCost;
            ACV = tmp.ACV;
            DefaultImageUrl = tmp.DefaultImageUrl;
            NewUsed = tmp.NewUsed;
            VehicleType = tmp.VehicleType;
            FuelEconomyCity = tmp.FuelEconomyCity;
            FuelEconomyHighWay = tmp.FuelEconomyHighWay;
            KBBTrimOption = tmp.KBBOptionsId;
            KBBTrimId = tmp.KBBTrimId.GetValueOrDefault();
            WholeSale = tmp.PreWholeSale.GetValueOrDefault();
        }

        public VehicleViewModel(vincontrolwholesaleinventory tmp)
        {
            ListingId = tmp.ListingID;
            Year = tmp.ModelYear.GetValueOrDefault();
            Make = tmp.Make;
            Model = tmp.Model;
            Trim = tmp.Trim;
            Vin = tmp.VINNumber;
            StockNumber = tmp.StockNumber;
            SalePrice = tmp.SalePrice;
            MSRP = tmp.MSRP;
            Mileage = tmp.Mileage;
            ExteriorColor = tmp.ExteriorColor;
            InteriorColor = tmp.InteriorColor;
            InteriorSurface = tmp.InteriorSurface;
            BodyType = tmp.BodyType;
            EngineType = tmp.EngineType;
            DriveTrain = tmp.DriveTrain;
            Cylinders = tmp.Cylinders;
            Liters = tmp.Liters;
            FuelType = tmp.FuelType;
            Tranmission = tmp.Tranmission;
            Doors = tmp.Doors;
            Certified = tmp.Certified.GetValueOrDefault();
            CarsOptions = tmp.CarsOptions;
            Descriptions = tmp.Descriptions;
            CarImageUrl = tmp.CarImageUrl;
            ThumbnalImageurl = tmp.ThumbnailImageURL;
            DateInStock = tmp.DateInStock.GetValueOrDefault();
            DealershipName = tmp.DealershipName;
            DealershipAddress = tmp.DealershipAddress;
            DealershipCity = tmp.DealershipCity;
            DealershipState = tmp.DealershipState;
            DealershipZipCode = tmp.DealershipZipCode;
            DealerId = tmp.DealershipId.GetValueOrDefault();
            DealerCost = tmp.DealerCost;
            ACV = tmp.ACV;
            DefaultImageUrl = tmp.DefaultImageUrl;
            NewUsed = tmp.NewUsed;
            VehicleType = tmp.VehicleType;
            FuelEconomyCity = tmp.FuelEconomyCity;
            FuelEconomyHighWay = tmp.FuelEconomyHighWay;
            KBBTrimOption = tmp.KBBOptionsId;
            KBBTrimId = tmp.KBBTrimId.GetValueOrDefault();
            //WholeSale = tmp.PreWholeSale.GetValueOrDefault();
        }

        public VehicleViewModel(CommonHelper commonHelper, DataRow drRow, MappingViewModel model)
        {
            var xmlHelper = new XMLHelper();
            //if (model.HasHeader)
            //{
            //    StockNumber = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.StockNumber));
            //    Year = commonHelper.GetIntValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Year));
            //    Make = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Make)));
            //    Model = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Model)));
            //    Trim = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Trim)));
            //    Vin = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Vin));
            //    DaysInInventory = commonHelper.GetIntValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DaysInInventory));
            //    DateInStock = DateTime.Now.AddDays(DaysInInventory*-1);
            //    Mileage = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Mileage));
            //    SalePrice = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.SalePrice));
            //    NewUsed = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.NewUsed)));
            //    ExteriorColor = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.ExteriorColor)));
            //    InteriorColor = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.InteriorColor)));
            //    WindowStickerPrice = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.WindowStickerPrice));
            //    DealershipName = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipName));
            //    DealerCost = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealerCost));
            //    DealershipAddress = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipAddress));
            //    DealershipCity = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipCity));
            //    DealershipPhone = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipPhone));
            //    DealershipState = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipState));
            //    DealershipZipCode = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipZipCode));
            //    ACV = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.ACV));
            //    CarImageUrl = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.CarImageUrl));
            //    DriveTrain = commonHelper.GetStringValueFromMappingField(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DriveTrain));
            //}
            //else
            //{
                StockNumber = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.StockNumber));
                Year = commonHelper.GetIntValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Year));
                Make = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Make)));
                Model = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Model)));
                Trim = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Trim)));
                Vin = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Vin));
                DaysInInventory = commonHelper.GetIntValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DaysInInventory));
                DateInStock = DateTime.Now.AddDays(DaysInInventory * -1);
                Mileage = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.Mileage));
                SalePrice = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.SalePrice));
                NewUsed = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.NewUsed)));
                ExteriorColor = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.ExteriorColor)));
                InteriorColor = commonHelper.UppercaseWords(commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.InteriorColor)));
                WindowStickerPrice = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.WindowStickerPrice));
                DealershipName = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipName));
                DealerCost = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealerCost));
                DealershipAddress = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipAddress));
                DealershipCity = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipCity));
                DealershipPhone = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipPhone));
                DealershipState = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipState));
                DealershipZipCode = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DealershipZipCode));
                ACV = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.ACV));
                CarImageUrl = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.CarImageUrl));
                DriveTrain = commonHelper.GetStringValueFromMappingFieldNoHeader(drRow, xmlHelper.GetMappingField(model.Mappings, XMLHelper.DriveTrain));
            //}

            // Apply conditions
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.StockNumber);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.Make);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.Model);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.Trim);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.Vin);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.Mileage);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.SalePrice);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.NewUsed);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.ExteriorColor);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.InteriorColor);
            xmlHelper.GetStringValueAfterApplyCondition(this, model, XMLHelper.WindowStickerPrice);
            xmlHelper.GetIntValueAfterApplyCondition(this, model, XMLHelper.DaysInInventory);
            
        }

    }

}
