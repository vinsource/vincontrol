using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using ChartSelection = vincontrol.DomainObject.ChartSelection;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class CarInfoViewModel
    {
        public CarInfoViewModel()
        {

        }

        public CarInfoViewModel(SoldoutInventory row)
        {
            DealerDiscount = row.DealerDiscount.GetValueOrDefault().ToString("c0");
            ListingId = row.SoldoutInventoryId;
            CarsOptions = row.Vehicle.StandardOptions;
            AdditonalOptions = row.AdditionalOptions != null ? row.AdditionalOptions.Replace("\"", "").Replace("\'", "") : string.Empty;
            AdditonalPackages = row.AdditionalPackages != null ? row.AdditionalPackages.Replace("\"", "").Replace("\'", "") : string.Empty;
            Door = row.Vehicle.Doors ?? 0;
            Vin = row.Vehicle.Vin;
            Make = row.Vehicle.Make ?? string.Empty;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Model = row.Vehicle.Model ?? string.Empty;
            Litters = row.Vehicle.Litter.GetValueOrDefault().ToString();
            MSRP = row.DealerMsrp.GetValueOrDefault().ToString();
            Fuel = row.Vehicle.FuelType ?? string.Empty;
            WheelDrive = row.Vehicle.DriveTrain ?? string.Empty;
            Engine = row.Vehicle.EngineType ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            ChromeStyleId = row.Vehicle.ChromeStyleId;
            ChromeModelId = row.Vehicle.ChromeModelId;
            ExteriorColor = row.ExteriorColor ?? string.Empty;
            InteriorColor = row.Vehicle.InteriorColor ?? string.Empty;
            Cylinder = row.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Mileage = row.Mileage.GetValueOrDefault().ToString("#,##0");
            BodyType = row.Vehicle.BodyType ?? string.Empty;
            Tranmission = row.Vehicle.Tranmission ?? string.Empty;
            ExistPackages = new List<string>();
            ExistOptions = new List<string>();
            StandardPackages = new List<string>();
            StandardOptions = new List<string>();
            DefaultImageUrl = row.Vehicle.DefaultStockImage;
            Description = row.Descriptions ?? string.Empty;
            BrandedTitle = row.BrandedTitle.GetValueOrDefault();
            ACar = row.ACar.GetValueOrDefault();
            SalePrice = row.SalePrice.GetValueOrDefault().ToString("c0");
            DealerCost = row.DealerCost.GetValueOrDefault().ToString("#,##0");
            ACV = row.ACV.GetValueOrDefault().ToString("#,##0");
            SinglePhoto = String.IsNullOrEmpty(row.ThumbnailUrl) ? row.Vehicle.DefaultStockImage : row.ThumbnailUrl.Split(',')[0];

            OrginalName = ModelYear + " " + Make + " " + Model;
            if (!String.IsNullOrEmpty(Trim) && !Trim.Equals("NA"))
                OrginalName += " " + Trim;

            Condition = row.Condition ? "Used" : "New";
            CarImageUrl = row.PhotoUrl ?? string.Empty;
            CarThumbnailImageUrl = row.ThumbnailUrl ?? string.Empty;

            CustomerFirstName = row.FirstName;
            CustomerLastName = row.LastName;
            CustomerCity = row.City;
            CustomerState = row.State;

            InventoryStatus = Constanst.VehicleStatus.SoldOut;
            IsCertified = row.Certified.GetValueOrDefault();
            if (row.Vehicle.Carfaxes.Any())
            {
                CarFax = new CarFaxViewModel(row.Vehicle.Carfaxes.FirstOrDefault());
            }
        }
        
        public CarInfoViewModel(Inventory row)
        {
            DealerDiscount = row.DealerDiscount.GetValueOrDefault().ToString("c0");
            ListingId = row.InventoryId;
            Vin = row.Vehicle.Vin;
            CarsOptions = row.Vehicle.StandardOptions;
            AdditonalOptions = row.AdditionalOptions != null ? row.AdditionalOptions.Replace("\"","").Replace("\'","") : string.Empty;
            AdditonalPackages = row.AdditionalPackages != null ? row.AdditionalPackages.Replace("\"", "").Replace("\'", "") : string.Empty;
            Make = row.Vehicle.Make ?? string.Empty;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Model = row.Vehicle.Model ?? string.Empty;
            Litters = row.Vehicle.Litter.GetValueOrDefault().ToString();
            MSRP = row.DealerMsrp.GetValueOrDefault().ToString("c0");
            Fuel = row.Vehicle.FuelType ?? string.Empty;
            WheelDrive = row.Vehicle.DriveTrain ?? string.Empty;
            Engine = row.Vehicle.EngineType ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            ChromeStyleId = row.Vehicle.ChromeStyleId;
            ChromeModelId = row.Vehicle.ChromeModelId;
            ExteriorColor = row.ExteriorColor ?? string.Empty;
            InteriorColor = row.Vehicle.InteriorColor ?? string.Empty;
            Cylinder = row.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Mileage = row.Mileage.GetValueOrDefault().ToString("#,##0");
            BodyType = row.Vehicle.BodyType ?? string.Empty;
            Tranmission = row.Vehicle.Tranmission ?? string.Empty;
            ExistPackages = new List<string>();
            ExistOptions = new List<string>();
            StandardPackages = new List<string>();
            Door = row.Vehicle.Doors ?? 0;
            DefaultImageUrl = row.Vehicle.DefaultStockImage;
            Description = row.Descriptions ?? string.Empty;
            BrandedTitle = row.BrandedTitle.GetValueOrDefault();
            ACar = row.ACar.GetValueOrDefault();
            SalePrice = row.SalePrice.GetValueOrDefault().ToString("c0");
            WindowStickerPrice = row.SalePrice >= row.WindowStickerPrice
                ? "0"
                : row.WindowStickerPrice.GetValueOrDefault().ToString("c0");
            DealerCost = row.DealerCost.GetValueOrDefault().ToString("#,##0");
            ACV = row.ACV.GetValueOrDefault().ToString("#,##0");
            SinglePhoto = String.IsNullOrEmpty(row.ThumbnailUrl) ? row.Vehicle.DefaultStockImage : row.ThumbnailUrl.Split(',')[0];

            OrginalName = ModelYear + " " + Make + " " + Model;
            if (!String.IsNullOrEmpty(Trim) && !Trim.Equals("NA"))
                OrginalName += " " + Trim;

            Condition = row.Condition ? "Used" : "New";
            CarImageUrl = row.PhotoUrl ?? string.Empty;
            CarThumbnailImageUrl = row.ThumbnailUrl ?? string.Empty;

            InventoryStatus = row.InventoryStatusCodeId;
            if (row.Vehicle.Carfaxes.Any())
            {
                CarFax = new CarFaxViewModel(row.Vehicle.Carfaxes.FirstOrDefault());
            }
            IsCertified = row.Certified.GetValueOrDefault();
            DealershipName = row.Dealer.Name;
        }

        public CarInfoViewModel(Inventory row, int dealershipId)
        {
            DealerDiscount = row.DealerDiscount.GetValueOrDefault().ToString("c0");
            ListingId = row.InventoryId;
            CarsOptions = row.Vehicle.StandardOptions;
            AdditonalOptions = row.AdditionalOptions != null ? row.AdditionalOptions.Replace("\"", "").Replace("\'", "") : string.Empty;
            AdditonalPackages = row.AdditionalPackages != null ? row.AdditionalPackages.Replace("\"", "").Replace("\'", "") : string.Empty;
            Vin = row.Vehicle.Vin;
            Make = row.Vehicle.Make ?? string.Empty;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Model = row.Vehicle.Model ?? string.Empty;
            Litters = row.Vehicle.Litter.GetValueOrDefault().ToString();
            MSRP = row.DealerMsrp.GetValueOrDefault().ToString();
            Fuel = row.Vehicle.FuelType ?? string.Empty;
            WheelDrive = row.Vehicle.DriveTrain ?? string.Empty;
            Engine = row.Vehicle.EngineType ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            ChromeStyleId = row.Vehicle.ChromeStyleId;
            ChromeModelId = row.Vehicle.ChromeModelId;
            ExteriorColor = row.ExteriorColor ?? string.Empty;
            InteriorColor = row.Vehicle.InteriorColor ?? string.Empty;
            Cylinder = row.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Mileage = row.Mileage.GetValueOrDefault().ToString("#,##0");
            Door = row.Vehicle.Doors??0;
            BodyType = row.Vehicle.BodyType ?? string.Empty;
            Tranmission = row.Vehicle.Tranmission ?? string.Empty;
            DealershipId = dealershipId;
            ExistPackages = new List<string>();
            ExistOptions = new List<string>();
            StandardPackages = new List<string>();
            StandardOptions = new List<string>();
            
            DefaultImageUrl = row.Vehicle.DefaultStockImage;
            Description = row.Descriptions ?? string.Empty;
            BrandedTitle = row.BrandedTitle.GetValueOrDefault();
            ACar = row.ACar.GetValueOrDefault();
            SalePrice = row.SalePrice.GetValueOrDefault().ToString("c0");
            DealerCost = row.DealerCost.GetValueOrDefault().ToString("#,##0");
            ACV = row.ACV.GetValueOrDefault().ToString("#,##0");
            SinglePhoto = row.Vehicle.DefaultStockImage;
            
            OrginalName = ModelYear + " " + Make + " " + Model;
            if (!String.IsNullOrEmpty(Trim) && !Trim.Equals("NA"))
                OrginalName += " " + Trim;

            Condition = row.Condition ? "Used" : "New";
            CarImageUrl = row.PhotoUrl ?? string.Empty;
            CarThumbnailImageUrl = row.ThumbnailUrl ?? string.Empty;

            InventoryStatus = row.InventoryStatusCodeId;
            if (row.Vehicle.Carfaxes.Any())
            {
                CarFax = new CarFaxViewModel(row.Vehicle.Carfaxes.FirstOrDefault());
            }

            IsCertified = row.Certified.GetValueOrDefault();
        }

        #region ALL CarsAtributes
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
        
        public IEnumerable<ExtendedSelectListItem> ModelYears { get; set; }

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

        public string CarsOptions { get; set; }

        public string AdditonalOptions { get; set; }

        public string AdditonalPackages { get; set; }
             
        public string CarImageUrl { get; set; }

        public string CarThumbnailImageUrl { get; set; }

        public int Door { get; set; }

        public string Fuel { get; set; }

        public string Litters { get; set; }

        public string Tranmission { get; set; }

        public string WheelDrive { get; set; }

        public string Engine { get; set; }

        public ExtendedSelectListItem UserOrNew { get; set; }

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

        public IEnumerable<ExtendedSelectListItem> TransferDealerGroup { get; set; }

        public string SelectedDealerTransfer { get; set; }

        public string NewStockNumber { get; set; }

        public DealerGroupViewModel DealerGroup { get; set; }

        public string SelectedDealership { get; set; }

        public IEnumerable<ExtendedSelectListItem> DealerList { get; set; }

        public bool IsTruck { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckTypeList { get; set; }

        public string SelectedTruckType { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckCategoryList { get; set; }

        public string SelectedTruckCategory { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckClassList { get; set; }

        public string SelectedTruckClass { get; set; }

        public int WarrantyInfo { get; set; }

        public int CarFaxOwner { get; set; }

        public bool Reconstatus { get; set; }

        public int InventoryStatus { get; set; }

        public IEnumerable<ExtendedSelectListItem> VehicleTypeList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeExteriorColorList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeInteriorColorList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeTranmissionList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeDriveTrainList { get; set; }

        public string SelectedVehicleType { get; set; }

        public string SelectedExteriorColorCode { get; set; }

        public string SelectedInteriorColor { get; set; }

        public string SelectedTranmission { get; set; }

        public string SelectedDriveTrain { get; set; }

        public IEnumerable<ExtendedSelectDetailListItem> ChromeFactoryNonInstalledOptions { get; set; }

        public IEnumerable<ExtendedSelectDetailListItem> ChromeFactoryPackageOptions { get; set; }

        public IEnumerable<ExtendedSelectListItem> EditTrimList { get; set; }
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

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerState { get; set; }
        #endregion
    }

    public class SelectedOption
    {
        public SelectedOption()
        {
         
        }

        public string SelectedOptionName { get; set; }

        public string PriceInfo { get; set; }
    }

    public class CarShortViewModel
    {
        public CarShortViewModel()
        {
            AdditonalOptions = string.Empty;
        }

        public CarShortViewModel(Inventory row)
        {
            ListingId = row.InventoryId;
            Price = row.SalePrice.GetValueOrDefault();
            SalePrice = row.SalePrice.GetValueOrDefault().ToString("c0");
            CertifiedAmount = row.CertifiedAmount.GetValueOrDefault();
            MileageAdjustment = row.MileageAdjustment.GetValueOrDefault();
            ExteriorColor = row.ExteriorColor;
            InteriorColor = row.Vehicle.InteriorColor;
            CarsOptions = row.Vehicle.StandardOptions;
            AdditonalOptions = String.IsNullOrEmpty(row.AdditionalOptions) ? string.Empty : row.AdditionalOptions;
            AdditionalPackages = String.IsNullOrEmpty(row.AdditionalPackages) ? string.Empty : row.AdditionalPackages;
            Door = row.Vehicle.Doors ?? 0;
            Vin = row.Vehicle.Vin;
            Make = row.Vehicle.Make ?? string.Empty;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Model = row.Vehicle.Model ?? string.Empty;
            Litters = row.Vehicle.Litter.GetValueOrDefault().ToString();
            BodyType = row.Vehicle.BodyType ?? string.Empty;
            Fuel = row.Vehicle.FuelType ?? string.Empty;
            WheelDrive = row.Vehicle.DriveTrain ?? string.Empty;
            Engine = row.Vehicle.EngineType ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            Description = row.Descriptions;
            Condition = row.Condition ? "Used" : "New";
            Mileage = row.Mileage.GetValueOrDefault().ToString("#,##0");
            Odometer = row.Mileage.GetValueOrDefault();
            CarImageUrl = row.PhotoUrl ?? string.Empty;
            CarThumbnailImageUrl = row.ThumbnailUrl ?? string.Empty;
            Tranmission = row.Vehicle.Tranmission ?? string.Empty;
            OrginalName = ModelYear + " " + Make + " " + Model+" "+Trim;
            Cylinder = row.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Certified = row.Certified.GetValueOrDefault();
            DealerId = row.DealerId;
            DealerName = row.Dealer.Name;
            DefaultImageUrl = row.Vehicle.DefaultStockImage;
            Note=row.Note;
            DaysAged = DateTime.Now.Subtract(row.DateInStock.GetValueOrDefault()).Days;
        }

        public CarShortViewModel(SoldoutInventory row)
        {
            ListingId = row.SoldoutInventoryId;
            Price = row.SalePrice.GetValueOrDefault();
            SalePrice = row.SalePrice.GetValueOrDefault().ToString("c0");
            CertifiedAmount = row.CertifiedAmount.GetValueOrDefault();
            MileageAdjustment = row.MileageAdjustment.GetValueOrDefault();
            ExteriorColor = row.ExteriorColor;
            InteriorColor = row.Vehicle.InteriorColor;
            CarsOptions = row.Vehicle.StandardOptions;
            AdditonalOptions = String.IsNullOrEmpty(row.AdditionalOptions) ? string.Empty : row.AdditionalOptions;
            AdditionalPackages = String.IsNullOrEmpty(row.AdditionalPackages) ? string.Empty : row.AdditionalPackages;
            Door = row.Vehicle.Doors ?? 0;
            Vin = row.Vehicle.Vin;
            Make = row.Vehicle.Make ?? string.Empty;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Model = row.Vehicle.Model ?? string.Empty;
            Litters = row.Vehicle.Litter.GetValueOrDefault().ToString();
            BodyType = row.Vehicle.BodyType ?? string.Empty;
            Fuel = row.Vehicle.FuelType ?? string.Empty;
            WheelDrive = row.Vehicle.DriveTrain ?? string.Empty;
            Engine = row.Vehicle.EngineType ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            Description = row.Descriptions;
            Condition = row.Condition ? "Used" : "New";
            Mileage = row.Mileage.GetValueOrDefault().ToString("#,##0");
            Odometer = row.Mileage.GetValueOrDefault();
            CarImageUrl = row.PhotoUrl ?? string.Empty;
            CarThumbnailImageUrl = row.ThumbnailUrl ?? string.Empty;
            Tranmission = row.Vehicle.Tranmission ?? string.Empty;
            OrginalName = ModelYear + " " + Make + " " + Model + " " + Trim;
            Cylinder = row.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Certified = row.Certified.GetValueOrDefault();
            DealerId = row.DealerId;
            DealerName = row.Dealer.Name;
            DefaultImageUrl = row.Vehicle.DefaultStockImage;
            DaysAged = DateTime.Now.Subtract(row.DateInStock.GetValueOrDefault()).Days;
        }

        public CarShortViewModel(Appraisal row)
        {
            ListingId = row.AppraisalId;
            Price = row.SalePrice.GetValueOrDefault();
            SalePrice = row.SalePrice.GetValueOrDefault().ToString("c0");
            CertifiedAmount = row.CertifiedAmount.GetValueOrDefault();
            MileageAdjustment = row.MileageAdjustment.GetValueOrDefault();
            ExteriorColor = row.ExteriorColor;
            InteriorColor = row.Vehicle.InteriorColor;
            CarsOptions = row.Vehicle.StandardOptions;
            AdditonalOptions = String.IsNullOrEmpty(row.AdditionalOptions) ? string.Empty : row.AdditionalOptions;
            AdditionalPackages = String.IsNullOrEmpty(row.AdditionalPackages) ? string.Empty : row.AdditionalPackages;
            Door = row.Vehicle.Doors ?? 0;
            Vin = row.Vehicle.Vin;
            Make = row.Vehicle.Make ?? string.Empty;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Model = row.Vehicle.Model ?? string.Empty;
            Litters = row.Vehicle.Litter.GetValueOrDefault().ToString();
            BodyType = row.Vehicle.BodyType ?? string.Empty;
            Fuel = row.Vehicle.FuelType ?? string.Empty;
            WheelDrive = row.Vehicle.DriveTrain ?? string.Empty;
            Engine = row.Vehicle.EngineType ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            Description = row.Descriptions;
            Mileage = row.Mileage.GetValueOrDefault().ToString("#,##0");
            Odometer = row.Mileage.GetValueOrDefault();
            CarImageUrl = row.PhotoUrl ?? string.Empty;
            CarThumbnailImageUrl = row.ThumbnailUrl ?? string.Empty;
            Tranmission = row.Vehicle.Tranmission ?? string.Empty;
            OrginalName = ModelYear + " " + Make + " " + Model + " " + Trim;
            Cylinder = row.Vehicle.Cylinders.GetValueOrDefault().ToString();
            Certified = row.Certified.GetValueOrDefault();
            DealerId = row.DealerId;
            DealerName = row.Dealer.Name;
            DefaultImageUrl = row.Vehicle.DefaultStockImage;
            Note = row.Note;
            DaysAged = DateTime.Now.Subtract(row.AppraisalDate.GetValueOrDefault()).Days;
        }
        
        #region Attributes for short vehicle
        public int ListingId { get; set; }

        public string Title { get; set; }

        public string Condition { get; set; }
        
        public string StockNumber { get; set; }
        
        public string SalePrice { get; set; }
        
        public decimal Price { get; set; }

        public decimal CertifiedAmount { get; set; }

        public decimal MileageAdjustment { get; set; }
        
        public long Odometer { get; set; }
        
        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string CarsOptions { get; set; }

        public string AdditonalOptions { get; set; }

        public string AdditionalPackages { get; set; }

        public string OtherOptions { get; set; }

        public string SelectedOptions { get; set; }

        public string CarImageUrl { get; set; }

        public string CarThumbnailImageUrl { get; set; }

        public int Door { get; set; }

        public string Fuel { get; set; }

        public string Litters { get; set; }

        public string Tranmission { get; set; }

        public string WheelDrive { get; set; }

        public string Engine { get; set; }

        public string Vin { get; set; }

        public int CarFaxOwner { get; set; }

        public string OrginalName { get; set; }        

        public int ModelYear { get; set; }       

        public string Make { get; set; }

        public string Model { get; set; }        

        public string Trim { get; set; }

        public string Cylinder { get; set; }        

        public string BodyType { get; set; }

        public string Mileage { get; set; }

        public string Description { get; set; }

        public bool Certified { get; set; }

        public int DealerId { get; set; }

        public string DealerName { get; set; }

        public string DefaultImageUrl { get; set; }

        public string Note { get; set; }

        public int DaysAged { get; set; }

        #endregion
    }
}
