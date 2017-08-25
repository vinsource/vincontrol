using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.DatabaseModel;


namespace WhitmanEnterpriseMVC.Models
{
    public class AppraisalViewFormModel : ISelectedTrimItem
    {
        public AppraisalViewFormModel()
        {
            MechanicalList = new List<String>();
            ExteriorList = new List<String>();
            EntertainmentList = new List<String>();
            InteriorList = new List<String>();
            SafetyList = new List<String>();
            DriveTrainList = new List<SelectListItem>().AsEnumerable();
            TrimList = new List<SelectListItem>().AsEnumerable();
            BodyTypeList = new List<SelectListItem>().AsEnumerable();
        }

        public AppraisalViewFormModel(whitmanenterpriseappraisal row)
        {
            VinGenie = row.VinGenie;
            AppraisalID = row.idAppraisal;
            AppraisalGenerateId = row.idAppraisal.ToString();
            Notes = row.Note;
            ModelYear = row.ModelYear.GetValueOrDefault();
            Make = row.Make ?? string.Empty;
            AppraisalModel = row.Model ?? string.Empty;
            SelectedTrim = row.Trim ?? string.Empty;
            Trim = row.Trim ?? string.Empty;
            VinNumber = row.VINNumber ?? string.Empty;
            StockNumber = row.StockNumber ?? string.Empty;
            SalePrice = row.SalePrice ?? string.Empty;
            MSRP = row.MSRP ?? string.Empty;
            Mileage = row.Mileage ?? "0";
            Title = ModelYear + " " + Make + " " + AppraisalModel + (!String.IsNullOrEmpty(SelectedTrim) ? " " + SelectedTrim : "");
            SelectedCylinder = row.Cylinders ?? string.Empty;
            SelectedExteriorColorValue = row.ExteriorColor ?? string.Empty;
            SelectedExteriorColorCode = row.ColorCode ?? string.Empty;
            ExteriorColor = row.ExteriorColor ?? string.Empty;
            InteriorColor = row.InteriorColor ?? string.Empty;
            SelectedInteriorColor = row.InteriorColor ?? string.Empty;
            SelectedBodyType = row.BodyType ?? string.Empty;
            EngineType = row.EngineType ?? string.Empty;
            SelectedDriveTrain = row.DriveTrain ?? string.Empty;
            SelectedFuel = row.FuelType ?? string.Empty;
            SelectedTranmission = row.Tranmission ?? string.Empty;
            Door = row.Doors ?? string.Empty;
            SelectedLiters = row.Liters ?? string.Empty;
            SelectedFactoryOptions = row.CarsOptions ?? string.Empty;
            SelectedPackageOptions = row.CarsPackages ?? string.Empty;
            Descriptions = row.Descriptions ?? string.Empty;
            Notes = row.Note ?? string.Empty;
            DealerCost = row.DealerCost ?? string.Empty;
            DefaultImageUrl = row.DefaultImageUrl ?? string.Empty;
            PhotoUrl = row.PhotoUrl;
            DateOfAppraisal = row.AppraisalDate != null ? row.AppraisalDate.GetValueOrDefault() : DateTime.Now;
            CustomerFirstName = row.FirstName ?? string.Empty;
            CustomerLastName = row.LastName ?? string.Empty;
            CustomerAddress = row.Address ?? string.Empty;
            CustomerCity = row.City ?? string.Empty;
            CustomerState = row.State ?? string.Empty;
            CustomerZipCode = row.ZipCode ?? string.Empty;
            CustomerEmail = row.Email ?? string.Empty;
            AppraisalBy = row.AppraisalBy ?? string.Empty;
            AppraisalType = row.AppraisalType ?? string.Empty;
            ChromeModelId = row.ChromeModelId ?? string.Empty;
            ChromeStyleId = row.ChromeStyleId ?? string.Empty;
            StandardOptions = row.StandardOptions ?? string.Empty;
            IsCertified = row.Certified.GetValueOrDefault();
            VehicleType = row.VehicleType;
            TruckCategory = row.TruckCategory;
            TruckType = row.TruckType;
            TruckClass = row.TruckClass;
            ACV = row.ACV;

            if (row.KBBTrimId != null)
                KbbTrimId = row.KBBTrimId.Value;
            KbbOptionsId = row.KBBOptionsId;
        }
        
        public string VehicleType { get; set; }

        public string Title { get; set; }

        public int AppraisalID { get; set; }

        public int ModelYear { get; set; }

        public string SelectedModelYear { get; set; }
        
        public IEnumerable<SelectListItem> ModelYearList { get; set; }

        public string SelectedMake { get; set; }
        
        public IEnumerable<SelectListItem> MakeList { get; set; }

        public string Make { get; set; }

        public string SelectedModel { get; set; }
        
        public IEnumerable<SelectListItem> ModelList { get; set; }
        
        public string AppraisalModel { get; set; }

        public string Trim { get; set; }

        public string SelectedTrim { get; set; }

        public IEnumerable<SelectListItem> TrimList { get; set; }

        public string VinNumber { get; set; }

        public string SampleVin { get; set; }

        public string StockNumber { get; set; }

        public string SalePrice { get; set; }

        public string Mileage { get; set; }

        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string CusExteriorColor { get; set; }

        public string CusInteriorColor { get; set; }

        public string InteriorSurface { get; set; }

        public IEnumerable<SelectListItem> BodyTypeList { get; set; }

        public string SelectedBodyType { get; set; }

        public string EngineType { get; set; }

        public IEnumerable<SelectListItem> DriveTrainList { get; set; }
        
        public string SelectedDriveTrain { get; set; }
        
        public List<string> MechanicalList { get; set; }

        public List<string> ExteriorList { get; set; }

        public string FuelEconomyCity { get; set; }

        public string FuelEconomyHighWay { get; set; }
        
        public IEnumerable<SelectListItem> TranmissionList
        {
            get
            {
                return new List<SelectListItem>()
                       {
                           new SelectListItem(){Text = "Select...", Value = ""},
                           new SelectListItem(){Text = "Automatic", Value = "Automatic"},
                           new SelectListItem(){Text = "Manual", Value = "Manual"},
                           new SelectListItem(){Text = "Shiftable Automatic Transmission", Value = "Shiftable Automatic Transmission"}
                       };
            }
        }

        public string SelectedTranmission { get; set; }

        public IEnumerable<SelectListItem> FuelList { get; set; }

        public string SelectedFuel { get; set; }

        public string WheelDrive { get; set; }

        public string Engine { get; set; }

        public List<string> EntertainmentList { get; set; }

        public List<string> InteriorList { get; set; }

        public List<string> SafetyList { get; set; }

        //public List<ExtendedFactoryOptions> FactoryNonInstalledOptions { get; set; }

        //public List<ExtendedFactoryOptions> FactoryPackageOptions { get; set; }

        public string SelectedFactoryOptions { get; set; }

        public string SelectedPackageOptions { get; set; }

        public IEnumerable<SelectDetailListItem> FactoryNonInstalledOptions { get; set; }

        public IEnumerable<SelectDetailListItem> FactoryPackageOptions { get; set; }

        //public string SelectedExteriorColor { get; set; }
        
        public bool UseColorCodeId { get; set; }
        
        public string SelectedExteriorColorValue { get; set; }
        
        public string SelectedExteriorColorCode { get; set; }

        public string SelectedInteriorColor { get; set; }

        public IEnumerable<SelectListItem> ExteriorColorList { get; set; }

        public IEnumerable<SelectListItem> InteriorColorList { get; set; }

        public string Door { get; set; }

        public bool Certified { get; set; }

        public string CarsOptions { get; set; }

        public string StandardOptions { get; set; }

        public string Descriptions { get; set; }

        public string DefaultImageUrl { get; set; }

        public string CarImagesUrl { get; set; }

        public string PhotoUrl { get; set; }

        public string ThumbNailImagesUrl { get; set; }

        public DateTime DateInStock { get; set; }

        public string DealershipName { get; set; }

        public string DealershipAddress { get; set; }

        public string DealershipCity { get; set; }

        public string DealershipState { get; set; }

        public string DealershipPhone { get; set; }

        public string DealershipZipCode { get; set; }

        public bool VinDecodeSuccess { get; set; }
        
        public string CarFaxDealerId { get; set; }

        public int DealershipId { get; set; }

        public IEnumerable<SelectListItem> CylinderList { get; set; }

        public string SelectedCylinder { get; set; }

        public IEnumerable<SelectListItem> LitersList { get; set; }

        public string SelectedLiters { get; set; }

        public string MSRP { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerState { get; set; }

        public string CustomerZipCode { get; set; }

        public string CustomerEmail { get; set; }

        public string ACV { get; set; }

        public string DealerCost { get; set; }

        public string AppraisalDate { get; set; }

        public DateTime DateOfAppraisal { get; set; }

        public int CarsOnMarket { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal LowestPrice { get; set; }

        public decimal HighestPrice { get; set; }

        public string AppraisalBy { get; set; }

        public string AppraisalType { get; set; }

        public string Notes { get; set; }

        public string AddToInventoryBy { get; set; }

        public string AppraisalGenerateId { get; set; }

        public string ChromeModelId { get; set; }

        public string ChromeStyleId { get; set; }

        public CarFaxViewModel CarFax { get; set; }

        public KellyBlueBookViewModel KBB { get; set; }

        public BlackBookViewModel BB { get; set; }

        public bool IsTruck { get; set; }

        public bool SessionTimeOut { get; set; }

        public bool IsAppraisedByYear { get; set; }

        public string StandardInstalledOption { get; set; }

        public IEnumerable<SelectListItem> TruckTypeList { get; set; }

        public string SelectedTruckType { get; set; }

        public IEnumerable<SelectListItem> TruckCategoryList { get; set; }

        public string SelectedTruckCategory { get; set; }

        public IEnumerable<SelectListItem> TruckClassList { get; set; }

        public string SelectedTruckClass { get; set; }

        //EDIT

        public List<Color> ExteriorColorListForEdit { get; set; }

        public List<Color> InteriorColorListForEdit { get; set; }

        public List<string> TrimListEdit { get; set; }

        public List<ExtendedFactoryOptions> FactoryNonInstalledOptionsEdit { get; set; }
        
        public List<ExtendedFactoryOptions> FactoryPackageOptionsEdit { get; set; }

        public bool IsCertified { get; set; }

        public string OrginalName { get; set; }

        public IEnumerable<SelectListItem> VehicleTypeList { get; set; }
        
        public List<string> ExistOptions { get; set; }
        
        public List<string> ExistPackages { get; set; }
        
        public string SelectedVehicleType { get; set; }
        
        public int KbbTrimId { get; set; }

        public string KbbOptionsId { get; set; }
        
        public string TruckType { get; set; }

        public string TruckClass { get; set; }

        public string TruckCategory { get; set; }

        public String UserStamp { get; set; }
        
        public DateTime? DateStamp { get; set; }
        
        public bool? VinGenie { get; set; }

        public List<ManheimWholesaleViewModel> ManheimWholesales { get; set; }
        
        public string CusTrim { get; set; }
        
        public IEnumerable<SmallKarPowerViewModel> KarPowerData { get; set; }

        public bool IsPhotoFromVingenie { get; set; }

        public string SelectedPackagesDescription
        {
            get;
            set;
        }

        public string Location { get; set; }

        #region Implementation of ISelectedTrimItem

        public string SelectedTrimItem { get; set; }

        #endregion
    }
}
