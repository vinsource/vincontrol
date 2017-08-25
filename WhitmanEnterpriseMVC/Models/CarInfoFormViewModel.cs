using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.HelperClass;

namespace WhitmanEnterpriseMVC.Models
{
    public class SelectDetailListItem : SelectListItem
    {
        public string Description { get; set; }
    }

    public interface ISelectedTrimItem
    {
        string SelectedTrimItem { get; set; }
    }

    public class CarInfoFormViewModel : ISelectedTrimItem
    {
        public CarInfoFormViewModel()
        {
       
        }

        public CarInfoFormViewModel(whitmanenterprisedealershipinventory row, int dealershipId)
        {
            ListingId = row.ListingID;
            Vin = row.VINNumber ?? string.Empty;
            Make = row.Make ?? string.Empty;
            ModelYear = row.ModelYear.GetValueOrDefault();
            Model = row.Model ?? string.Empty;
            Litters = row.Liters ?? string.Empty;
            MSRP = row.MSRP ?? string.Empty;
            Fuel = row.FuelType ?? string.Empty;
            WheelDrive = row.DriveTrain ?? string.Empty;
            Engine = row.EngineType ?? string.Empty;
            StockNumber = row.StockNumber ?? string.Empty;
            Trim = row.Trim ?? string.Empty;
            ChromeStyleId = row.ChromeStyleId;
            ChromeModelId = row.ChromeModelId;
            ExteriorColor = row.ExteriorColor ?? string.Empty;
            InteriorColor = row.InteriorColor ?? string.Empty;
            Cylinder = row.Cylinders ?? string.Empty;
            Mileage = row.Mileage ?? string.Empty;
            BodyType = row.BodyType ?? string.Empty;
            Tranmission = row.Tranmission ?? string.Empty;
            DealershipId = dealershipId;
            ExistPackages = new List<string>();
            ExistOptions = new List<string>();
            StandardPackages = new List<string>();
            StandardOptions = new List<string>();
            DefaultImageUrl = row.DefaultImageUrl;
            Description = row.Descriptions ?? string.Empty;
            BrandedTitle = row.BrandedTitle.GetValueOrDefault();
            ACar = row.ACar.GetValueOrDefault();

            if (String.IsNullOrEmpty(row.SalePrice))
                SalePrice = "NA";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.SalePrice, out priceFormat);
                if (flag)
                    SalePrice = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.DealerCost))
                DealerCost = "NA";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.DealerCost, out priceFormat);
                if (flag)
                    DealerCost = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.ACV))
                ACV = "NA";
            else
            {
                double priceFormat;
                bool flag = Double.TryParse(row.ACV, out priceFormat);
                if (flag)
                    ACV = priceFormat.ToString("#,##0");
            }

            if (String.IsNullOrEmpty(row.CarImageUrl))
                SinglePhoto = row.DefaultImageUrl;
            else
            {
                string[] totalImages = CommonHelper.GetArrayFromString(row.CarImageUrl);
                SinglePhoto = totalImages[0];
            }

            OrginalName = ModelYear + " " + Make + " " + Model;

            if (!String.IsNullOrEmpty(Trim) && !Trim.Equals("NA"))
                OrginalName += " " + Trim;

            if (String.IsNullOrEmpty(row.NewUsed))
                Condition = "Used";
            else
            {
                Condition = row.NewUsed.Trim().ToUpperInvariant().Equals("USED") ? "Used" : "New";
            }

            ExistOptions = String.IsNullOrEmpty(row.CarsOptions) ? new List<string>()  : (from data in CommonHelper.GetArrayFromString(row.CarsOptions) select data).ToList();

            ExistPackages = String.IsNullOrEmpty(row.CarsPackages) ? new List<string>() : (from data in CommonHelper.GetArrayFromString(row.CarsPackages) select data).ToList();

            UploadPhotosURL = String.IsNullOrEmpty(row.CarImageUrl) ? new List<string>() : (from data in CommonHelper.GetArrayFromString(row.CarImageUrl) select data).ToList();

            CarImageUrl = row.CarImageUrl ?? string.Empty;

            if (!String.IsNullOrEmpty(row.StandardOptions))
            {
                StandardOptions = CommonHelper.GetArrayFromString(row.StandardOptions).ToList();
            }
        }

        [Required]
        public string Vin { get; set; }

        public int ListingId { get; set; }

        public bool VinDecodeSuccess { get; set; }
        
        public string CarName { get; set; }
        
        public string StockNumber { get; set; }
        
        public string MSRP { get; set; }
        
        public string TotalMSRP { get; set; }
        
        public string OrginalName { get; set; }
        
        public string Title { get; set; }
        
        public int ModelYear { get; set; }
        
        public IEnumerable<SelectListItem> ModelYears { get; set; }
        
        public string Make { get; set; }
        
        public string Model { get; set; }
        
        public string VehicleModel { get; set; }
        
        public string Trim { get; set; }

        public string ChromeStyleId { get; set; }
        
        public string ChromeModelId { get; set; }
        
        public string ChromeModel { get; set; }
        
        public string ACV { get; set; }
        
        public string DealerCost { get; set; }
        
        public string Style { get; set; }
        
        public string Cylinder { get; set; }
        
        public IdentifiedString[] MakeNameList { get; set; }
        
        public string BodyType { get; set; }
        
        public int DealershipId { get; set; }
        
        public string DealershipName { get; set; }
        
        public string DealershipAddress { get; set; }
        
        public string DealerPhoneNumber { get; set; }
        
        public string RetailPrice { get; set; }
        
        public string WindowStickerPrice { get; set; }
        
        public string DealerDiscount { get; set; }
        
        public string ManufacturerRebate { get; set; }
        
        public string SalePrice { get; set; }
        
        public string ZipCode { get; set; }
        
        public string ExteriorColor { get; set; }
        
        public string InteriorColor { get; set; }
        
        public string CusExteriorColor { get; set; }
        
        public string CusInteriorColor { get; set; }
        
        public List<Color> ExteriorColorList { get; set; }
        
        public List<Color> InteriorColorList { get; set; }
        
        public List<IdentifiedString> ModelList { get; set; }
        
        public List<Style> StyleList { get; set; }

        public string Mileage { get; set; }
        
        public string Description { get; set; }
        
        public List<ExtendedFactoryOptions> FactoryNonInstalledOptions { get; set; }
        
        public List<ExtendedFactoryOptions> FactoryPackageOptions { get; set; }
        
        public List<string> ExistOptions { get; set; }
        
        public List<string> ExistPackages { get; set; }
        
        public List<string> StandardPackages { get; set; }
        
        public List<string> StandardOptions { get; set; }
        
        public string CarImageUrl { get; set; }
        
        public int Door { get; set; }
        
        public string Fuel { get; set; }
        
        public string Litters { get; set; }

        public string Tranmission { get; set; }

        public string WheelDrive { get; set; }

        public string Engine { get; set; }

        public SelectList UserOrNew { get; set; }

        public List<SelectedOption> SelectedOptions { get; set; }

        public bool Success { get; set; }

        public List<string> MutiplePhotos { get; set; }

        public string SinglePhoto { get; set; }

        public string DefaultImageUrl { get; set; }

        public string CurrentDistance { get; set; }

        public int DistanceFromDealerShip { get; set; }

        public decimal AveragePriceOnMarket { get; set; }

        public decimal MilageDecimal { get; set; }

        public int MarketRange { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateInStock { get; set; }

        public int DaysInInvenotry { get; set; }

        public int NumberofCarOnAutoTrader { get; set; }

        public int NumberofCarOnCarsCom { get; set; }

        public int NumberofCarOnEbay { get; set; }

        public decimal Price { get; set; }

        public Hashtable CarsOnMarket { get; set; }

        public List<string> UploadPhotosURL { get; set; }

        public decimal LowestPrice { get; set; }

        public decimal HighestPrice { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public List<string> TrimList { get; set; }

        public string CarsOptions { get; set; }

        public string CarPackages { get; set; }

        public bool IsManual { get; set; }
        
        public string AppraisalId { get; set; }

        public List<string> MechanicalList { get; set; }

        public List<string> ExteriorList { get; set; }
        
        public List<string> EntertainmentList { get; set; }

        public List<string> InteriorList { get; set; }
        
        public List<string> SafetyList { get; set; }

        public string FuelEconomyCity { get; set; }

        public string FuelEconomyHighWay { get; set; }
        
        public int HealthLevel { get; set; }

        public bool IsElectric { get; set; }

        public bool HasImage { get; set; }

        public bool HasDescription { get; set; }
        
        public bool HasSalePrice { get; set; }

        public bool IsSold { get; set; }

        public bool IsCertified { get; set; }

        public bool PriorRental { get; set; }

        public bool DealerDemo { get; set; }

        public bool Unwind { get; set; }

        public string AppraisalType { get; set; }

        public string AppraisalGenerateId { get; set; }

        public int SoldOutDaysLeft { get; set; }

        public string LogoUrl { get; set; }

        public string Condition { get; set; }

        public string EbayHtmlSource { get; set; }

        public string CarFaxDealerId { get; set; }

        public CarFaxViewModel CarFax { get; set; }

        public KellyBlueBookViewModel KBB { get; set; }

        public BlackBookViewModel BB { get; set; }

        public bool SessionTimeOut { get; set; }

        public bool MultipleDealers { get; set; }
        
        public IEnumerable<SelectListItem> TransferDealerGroup { get; set; }

        public string SelectedDealerTransfer { get; set; }

        public string NewStockNumber { get; set; }

        public DealerGroupViewModel DealerGroup { get; set; }

        public string SelectedDealership { get; set; }

        public IEnumerable<SelectListItem> DealerList { get; set; }
        
        public bool IsTruck { get; set; }

        public IEnumerable<SelectListItem> TruckTypeList { get; set; }

        public string SelectedTruckType { get; set; }

        public IEnumerable<SelectListItem> TruckCategoryList { get; set; }

        public string SelectedTruckCategory { get; set; }

        public IEnumerable<SelectListItem> TruckClassList { get; set; }

        public string SelectedTruckClass { get; set; }

        public int WarrantyInfo { get; set; }

        public int CarFaxOwner { get; set; }

        public bool Reconstatus { get; set; }

        public int InventoryStatus { get; set; }

        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }

        public IEnumerable<SelectListItem> ChromeExteriorColorList { get; set; }

        public IEnumerable<SelectListItem> ChromeInteriorColorList { get; set; }

        public IEnumerable<SelectListItem> ChromeTranmissionList { get; set; }

        public IEnumerable<SelectListItem> ChromeDriveTrainList { get; set; }

        public string SelectedVehicleType { get; set; }

        //public string SelectedExteriorColor { get; set; }

        public string SelectedExteriorColorCode { get; set; }

        public string SelectedInteriorColor { get; set; }

        public string SelectedTranmission { get; set; }

        public string SelectedDriveTrain { get; set; }

        public IEnumerable<SelectDetailListItem> ChromeFactoryNonInstalledOptions { get; set; }

        public IEnumerable<SelectDetailListItem> ChromeFactoryPackageOptions { get; set; }

        public IEnumerable<SelectListItem> EditTrimList { get; set; }
        // 0: New
        // 1: Used
        // 2: Wholesale
        // 3: Appraisal
        // 4: Sold
        public int Type { get; set; }
        
        public int PreviousListingId { get; set; }
        
        public int NextListingId { get; set; }
        
        public bool? IsFeatured { get; set; }
        
        public ChartSelection SavedSelections { get; set; }
        
        public int CarRanking { get; set; }
        
        public int NumberOfCar { get; set; }
        
        public string MarketLowestPrice { get; set; }
        
        public string MarketAveragePrice { get; set; }
        
        public string MarketHighestPrice { get; set; }
        
        public List<ManheimWholesaleViewModel> ManheimWholesales { get; set; }

        public bool UseColorCodeId { get; set; }
        
        public string SelectedExteriorColorValue { get; set; }
        
        public string CusTrim { get; set; }
        
        public IEnumerable<SmallKarPowerViewModel> KarPowerData { get; set; }

        public bool Loaner { get; set; }

        public bool Auction { get; set; }
        
        public string SelectedPackagesDescription { get; set; }

        public bool EnableAutoDescriptionSetting { get; set; }

        public string NewUsed { get; set; }
        
        public int? KBBTrimId { get; set; }

        public string KBBOptionsId { get; set; }

        public bool NotDoneBucketJump { get; set; }

        public bool ACar { get; set; }

        public bool BrandedTitle { get; set; }

        public IEnumerable<WarrantyTypeViewModel> WarrantyTypes { get; set; }

        public string SelectedTrimItem { get; set; }

        public IList<ButtonPermissionViewModel> ButtonPermissions { get; set; }

        public string AfterSelectedPackage { get; set; }

        public string AfterSelectedOptions { get; set; }
    }
    
    public class SelectedOption
    {
        public SelectedOption()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string SelectedOptionName { get; set; }

        public string PriceInfo { get; set; }
    }
}
