using System;
using System.Collections.Generic;
using System.Linq;
using AjaxControlToolkit;
using vincontrol.Application.ViewModels.CommonManagement;
//using Vincontrol.Web.com.chromedata.services.Description7a;
using vincontrol.Constant;
using vincontrol.Data;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.DatabaseModel;
using Color = vincontrol.ChromeAutoService.AutomativeService.Color;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Models
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
            DriveTrainList = new List<ExtendedSelectListItem>().AsEnumerable();
            TrimList = new List<ExtendedSelectListItem>().AsEnumerable();
            BodyTypeList = new List<ExtendedSelectListItem>().AsEnumerable();
        }

        public AppraisalViewFormModel(Appraisal row)
        {
            InventoryId = row.Vehicle.Inventories.FirstOrDefault()!= null ? row.Vehicle.Inventories.FirstOrDefault().InventoryId:0;
            VinGenie = row.VinGenie;
            VehicleId = row.Vehicle.VehicleId;
            AppraisalID = row.AppraisalId;
            AppraisalGenerateId = row.AppraisalId.ToString();
            Notes = row.Note;
            ModelYear = row.Vehicle.Year.GetValueOrDefault();
            Make = row.Vehicle.Make ?? string.Empty;
            AppraisalModel = row.Vehicle.Model ?? string.Empty;
            SelectedModel = row.Vehicle.Model ?? string.Empty;
            SelectedTrim = row.Vehicle.Trim ?? string.Empty;
            Trim = row.Vehicle.Trim ?? string.Empty;
            VinNumber = row.Vehicle.Vin ?? string.Empty;
            StockNumber = row.Stock ?? string.Empty;
            SalePrice = row.SalePrice.GetValueOrDefault();
            MSRP = row.Vehicle.Msrp.GetValueOrDefault();
            Mileage = row.Mileage.GetValueOrDefault();
            Title = ModelYear + " " + Make + " " + AppraisalModel + (!String.IsNullOrEmpty(SelectedTrim) ? " " + SelectedTrim : "");
            SelectedCylinder = row.Vehicle.Cylinders.GetValueOrDefault();
            SelectedExteriorColorValue = row.ExteriorColor ?? string.Empty;
            SelectedExteriorColorCode = row.Vehicle.ColorCode ?? string.Empty;
            ExteriorColor = row.ExteriorColor ?? string.Empty;
            InteriorColor = row.Vehicle.InteriorColor ?? string.Empty;
            SelectedInteriorColor = row.Vehicle.InteriorColor ?? string.Empty;
            SelectedBodyType = row.Vehicle.BodyType ?? string.Empty;
            EngineType = row.Vehicle.EngineType ?? string.Empty;
            SelectedDriveTrain = row.Vehicle.DriveTrain ?? string.Empty;
            SelectedFuel = row.Vehicle.FuelType ?? string.Empty;
            SelectedTranmission = row.Vehicle.Tranmission ?? string.Empty;
            Door = row.Vehicle.Doors.GetValueOrDefault();
            SelectedLiters = row.Vehicle.Litter.GetValueOrDefault();
            SelectedFactoryOptions = row.AdditionalOptions ?? string.Empty;
            SelectedPackageOptions = row.AdditionalPackages ?? string.Empty;
            Descriptions = row.Descriptions ?? string.Empty;
            Notes = row.Note ?? string.Empty;
            DealerCost = row.DealerCost.GetValueOrDefault();

            Model = row.Vehicle.Model ?? string.Empty;

            PhotoUrl = row.PhotoUrl;
            DateOfAppraisal = row.AppraisalDate != null ? row.AppraisalDate.GetValueOrDefault() : DateTime.Now;
            IsManualDecode = row.IsManualDecode;

            if (row.AppraisalCustomer != null)
            {
                CustomerFirstName = row.AppraisalCustomer.FirstName ?? string.Empty;
                CustomerLastName = row.AppraisalCustomer.LastName ?? string.Empty;
                CustomerAddress = row.AppraisalCustomer.Address ?? string.Empty;
                CustomerCity = row.AppraisalCustomer.City ?? string.Empty;
                CustomerState = row.AppraisalCustomer.State ?? string.Empty;
                CustomerZipCode = row.AppraisalCustomer.ZipCode ?? string.Empty;
                CustomerEmail = row.AppraisalCustomer.Email ?? string.Empty;
            }

            AppraisalBy = row.VinGenieUserId != null ? (row.User == null ? String.Empty : row.User.Name) : (row.User1 == null ? String.Empty : row.User1.Name);
            
            AppraisalType = row.AppraisalType ?? string.Empty;
            ChromeModelId = row.Vehicle.ChromeModelId ?? string.Empty;
            ChromeStyleId = row.Vehicle.ChromeStyleId ?? string.Empty;
            StandardOptions = row.Vehicle.StandardOptions ?? string.Empty;
            IsCertified = row.Certified.GetValueOrDefault();
            VehicleType = row.Vehicle.VehicleType.GetValueOrDefault();
            ACV = row.ACV;

            if (row.Vehicle.Msrp != null && row.Vehicle.Msrp.Value > 0)
                MSRP = row.Vehicle.Msrp.Value;

            if (row.Vehicle.KBBTrimId != null)
                KbbTrimId = row.Vehicle.KBBTrimId.Value;
            KbbOptionsId = row.Vehicle.KBBOptionsId;

            DefaultImageUrl = row.VinGenie.HasValue && !String.IsNullOrEmpty(row.PhotoUrl) ? row.PhotoUrl : (row.Vehicle.DefaultStockImage ?? string.Empty);
            FirstPhoto = row.VinGenie.HasValue && !String.IsNullOrEmpty(row.PhotoUrl) ? row.PhotoUrl : (row.Vehicle.DefaultStockImage ?? string.Empty);

            if (row.CARFAXOwner.GetValueOrDefault() <= 0)
                StrCarFaxOwner = "NA";
            else if (row.CARFAXOwner == 1)
                StrCarFaxOwner = row.CARFAXOwner + " Owner";
            else
                StrCarFaxOwner = row.CARFAXOwner + " Owners";

            StrClientName = string.Format("{0} {1}", CustomerFirstName, CustomerLastName);

            if(!String.IsNullOrEmpty(row.ThumbnailUrl))
                FirstPhoto = row.ThumbnailUrl.Split(new string[] { ",", "|" },StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            else
            {
                if (row.VinGenie.GetValueOrDefault())
                {
                    FirstPhoto = !String.IsNullOrEmpty(row.PhotoUrl) ? row.PhotoUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() : row.Vehicle.DefaultStockImage;
                  
                }
                else
                {
                    FirstPhoto = row.Vehicle.DefaultStockImage;
                }                
            }

            IsTruck = row.Vehicle.VehicleType.Equals(Constanst.VehicleType.Truck);
            SelectedVehicleType = row.Vehicle.VehicleType.Equals(Constanst.VehicleType.Truck) ? "Truck" : "Car";
            TruckType = row.Vehicle.TruckType;
            SelectedTruckType = row.Vehicle.TruckType;
            SelectedTruckCategoryId = row.Vehicle.TruckCategoryId.GetValueOrDefault();
            TruckCategory = row.Vehicle.TruckCategory != null ? row.Vehicle.TruckCategory.CategoryName : string.Empty;
            SelectedTruckClassId = row.Vehicle.TruckClassId.GetValueOrDefault();
            TruckClass = row.Vehicle.TruckClass != null ? row.Vehicle.TruckClass.ClassName : string.Empty;
        }

        public short VehicleType { get; set; }

        public int InventoryId { get; set; }

        public string Title { get; set; }

        public string Model { get; set; }

        public int VehicleId { get; set; }

        public int AppraisalID { get; set; }

        public int ModelYear { get; set; }

        public string SelectedModelYear { get; set; }

        public IEnumerable<ExtendedSelectListItem> ModelYearList { get; set; }

        public string SelectedMake { get; set; }

        public IEnumerable<ExtendedSelectListItem> MakeList { get; set; }

        public string Make { get; set; }

        public string SelectedModel { get; set; }

        public IEnumerable<ExtendedSelectListItem> ModelList { get; set; }
        
        public string AppraisalModel { get; set; }

        public string Trim { get; set; }

        public string SelectedTrim { get; set; }

        public IEnumerable<ExtendedSelectListItem> TrimList { get; set; }

        public string VinNumber { get; set; }

        public string SampleVin { get; set; }

        public string StockNumber { get; set; }

        public decimal SalePrice { get; set; }

        public long Mileage { get; set; }

        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string CusExteriorColor { get; set; }

        public string CusInteriorColor { get; set; }

        public string InteriorSurface { get; set; }

        public IEnumerable<ExtendedSelectListItem> BodyTypeList { get; set; }

        public string SelectedBodyType { get; set; }

        public string EngineType { get; set; }

        public IEnumerable<ExtendedSelectListItem> DriveTrainList { get; set; }
        
        public string SelectedDriveTrain { get; set; }
        
        public List<string> MechanicalList { get; set; }

        public List<string> ExteriorList { get; set; }

        public string FuelEconomyCity { get; set; }

        public string FuelEconomyHighWay { get; set; }

        public short InventoryStatusCodeId { get; set; }

        public IEnumerable<ExtendedSelectListItem> TranmissionList
        {
            get
            {
                return new List<ExtendedSelectListItem>()
                       {
                           new ExtendedSelectListItem(){Text = "Select...", Value = ""},
                           new ExtendedSelectListItem(){Text = "Automatic", Value = "Automatic"},
                           new ExtendedSelectListItem(){Text = "Manual", Value = "Manual"},
                           new ExtendedSelectListItem(){Text = "Shiftable Automatic Transmission", Value = "Shiftable Automatic Transmission"}
                       };
            }
        }

        public string SelectedTranmission { get; set; }

        public IEnumerable<ExtendedSelectListItem> FuelList { get; set; }

        public string SelectedFuel { get; set; }

        public string WheelDrive { get; set; }

        public string Engine { get; set; }

        public List<string> EntertainmentList { get; set; }

        public List<string> InteriorList { get; set; }

        public List<string> SafetyList { get; set; }

        public string SelectedFactoryOptions { get; set; }

        public string SelectedPackageOptions { get; set; }

        public IEnumerable<SelectDetailListItem> FactoryNonInstalledOptions { get; set; }

        public IEnumerable<SelectDetailListItem> FactoryPackageOptions { get; set; }

        public string AfterSelectedPackage { get; set; }

        public string AfterSelectedOptions { get; set; }

        public string AfterSelectedOptionCodes { get; set; }
        
        public bool UseColorCodeId { get; set; }
        
        public string SelectedExteriorColorValue { get; set; }
        
        public string SelectedExteriorColorCode { get; set; }

        public string SelectedInteriorColor { get; set; }

        public IEnumerable<ExtendedSelectListItem> ExteriorColorList { get; set; }

        public IEnumerable<ExtendedSelectListItem> InteriorColorList { get; set; }

        public int Door { get; set; }

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

        public IEnumerable<ExtendedSelectListItem> CylinderList { get; set; }

        public int SelectedCylinder { get; set; }

        public IEnumerable<ExtendedSelectListItem> LitersList { get; set; }

        public double SelectedLiters { get; set; }

        public decimal MSRP { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerState { get; set; }

        public string CustomerZipCode { get; set; }

        public string CustomerEmail { get; set; }

        public decimal? ACV { get; set; }

        public decimal DealerCost { get; set; }

        public string AppraisalDate { get; set; }

        public DateTime DateOfAppraisal { get; set; }

        public int CarsOnMarket { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal LowestPrice { get; set; }

        public decimal HighestPrice { get; set; }

        public int? AppraisalById { get; set; }

        public string AppraisalBy { get; set; }

        public string ShortAppraisalBy { get; set; }

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

        public bool IsManualDecode { get; set; }

        public string StandardInstalledOption { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckTypeList { get; set; }

        public string TruckType { get; set; }

        public string SelectedTruckType { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckCategoryList { get; set; }

        public string TruckCategory { get; set; }

        public int SelectedTruckCategoryId { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckClassList { get; set; }

        public string TruckClass { get; set; }

        public int SelectedTruckClassId { get; set; }
        
        //EDIT

        public List<Color> ExteriorColorListForEdit { get; set; }

        public List<Color> InteriorColorListForEdit { get; set; }

        public List<string> TrimListEdit { get; set; }

        public List<ExtendedFactoryOptions> FactoryNonInstalledOptionsEdit { get; set; }
        
        public List<ExtendedFactoryOptions> FactoryPackageOptionsEdit { get; set; }

        public bool IsCertified { get; set; }

        public string OrginalName { get; set; }

        public IEnumerable<ExtendedSelectListItem> VehicleTypeList { get; set; }
        
        public List<string> ExistOptions { get; set; }
        
        public List<string> ExistPackages { get; set; }
        
        public string SelectedVehicleType { get; set; }
        
        public int KbbTrimId { get; set; }

        public string KbbOptionsId { get; set; }
        
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

        public string URLDetail { get; set; }

        public string SortVin { get; set; }

        public string ShortMake { get; set; }

        public string StrTrim { get; set; }

        public string StrExteriorColor { get; set; }

        public string StrCarFaxOwner { get; set; }

        public string StrClientName { get; set; }

        public string StrMileage { get; set; }

        public string StrACV { get; set; }

        public string URLImage { get; set; }

        public string AppraisalModelSort { get; set; }

        public string FirstPhoto { get; set; }

        #region Implementation of ISelectedTrimItem

        public string SelectedTrimItem { get; set; }

        #endregion
    }
}
