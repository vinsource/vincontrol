using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Models
{
    public class BuyerGuideViewModel
    {
        public BuyerGuideViewModel()
        {
            Make = string.Empty;
            Model = " ";
            Vin = string.Empty;
            StockNumber = string.Empty;
            SystemCovered = string.Empty;
            Durations = string.Empty;
            IsAsWarranty = false;
            IsDealerCertified = false;
            IsFullWarranty = false;
            IsLimitedWarranty = false;
            IsManufacturerCertified = false;
            IsManufacturerWarranty = false;
            IsWarranty = false;
            PriorRentalText = "prior rental";
            IsManufacturerUsedVehicleWarranty = false;
            IsOtherWarranty = false;
        }

        public int Type { get; set; }

        public CarInfoFormViewModel CarInfoFormViewModel { get; set; }

        public int ListingId { get; set; }

        public string Vin { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string StockNumber { get; set; }

        public int Warranty { get; set; }

        public string SelectedWarranty { get; set; }

        public IEnumerable<ExtendedSelectListItem> Warranties { get; set; }

        public string PartsPercentage { get; set; }

        public string LaborPercentage { get; set; }

        public string Bumper { get; set; }

        public string Drivetrain { get; set; }

        public string Wear  { get; set; }

        public string Roadside  { get; set; }

        public string SelectedDurationForBumper { get; set; }

        public string SelectedDurationForDriveTrain { get; set; }

        public string SelectedDurationForWear { get; set; }

        public string SelectedDurationForRoadSide { get; set; }

        public IEnumerable<ExtendedSelectListItem> Duration { get; set; }

        public IEnumerable<ExtendedSelectListItem> Languages { get; set; }

        public int SelectedLanguage { get; set; }

        public string WarrantyBumperYear { get; set; }

        public string WarrantyBumperMiles { get; set; }

        public string WarrantyDrivetrainYear { get; set; }

        public string WarrantyDrivetrainMiles { get; set; }

        public string WarrantyWearAndTearYear { get; set; }

        public string WarrantyWearAndTearMiles { get; set; }

        public string WarrantyRoadsideYear { get; set; }

        public string WarrantyRoadsideMiles { get; set; }

        public string AsWarranty { get; set; }

        public string AsWarrantyInSpanish { get; set; }


        public string ManufacturerWarranty { get; set; }

        public string ManufacturerWarrantyInSpanish { get; set; }


        public string DealerCertified { get; set; }

        public string DealerCertifiedInSpanish { get; set; }

        public string ManufacturerCertified { get; set; }

        public string ManufacturerCertifiedInSpanish { get; set; }

        public string ManufacturerWarrantyDuration { get; set; }

        public string DealerCertifiedDuration { get; set; }

        public string ManufacturerCertifiedDuration { get; set; }
        
        public bool PriorRental { get; set; }

        public bool ServiceContract { get; set; }

        public bool IsAsWarranty { get; set; }
        public bool IsManufacturerWarranty { get; set; }
        public bool IsManufacturerUsedVehicleWarranty { get; set; }
        public bool IsOtherWarranty { get; set; }
        public bool IsDealerCertified { get; set; }
        public bool IsManufacturerCertified { get; set; }
        public bool IsWarranty { get; set; }
        public bool IsFullWarranty { get; set; }
        public bool IsLimitedWarranty { get; set; }
        public bool IsServiceContract { get; set; }
        public string SystemCovered { get; set; }
        public string Durations { get; set; }
        public int PercentageOfLabor { get; set; }
        public int PercentageOfPart { get; set; }
        public string PriorRentalText { get; set; }

        public short InventoryStatus { get; set; }
   }

    public class AdminBuyerGuideViewModel
    {
        public AdminBuyerGuideViewModel()
        {
            Id = 0;
            Make = string.Empty;
            VehicleModel = string.Empty;
            Vin = string.Empty;
            StockNumber = string.Empty;
            SystemCovered = string.Empty;
            Durations = string.Empty;
            IsAsWarranty = false;
            IsDealerCertified = false;
            IsFullWarranty = false;
            IsLimitedWarranty = false;
            IsManufacturerCertified = false;
            IsManufacturerWarranty = false;
            IsWarranty = false;
            PriorRental = "";
        }

        public int Id { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string VehicleModel { get; set; }
        public string StockNumber { get; set; }
        public int WarrantyType { get; set; }
        public bool ServiceContract { get; set; }
        public bool IsAsWarranty { get; set; }
        public bool IsManufacturerWarranty { get; set; }
        public bool IsManufacturerUsedVehicleWarranty { get; set; }
        public bool IsDealerCertified { get; set; }
        public bool IsManufacturerCertified { get; set; }
        public bool IsWarranty { get; set; }
        public bool IsFullWarranty { get; set; }
        public bool IsLimitedWarranty { get; set; }
        public bool IsOtherWarranty { get; set; }
        public bool IsServiceContract { get; set; }
        public bool IsPriorRental { get; set; }
        public bool IsBrandedTitle { get; set; }
        public string SystemCovered { get; set; }
        public string Durations { get; set; }
        public string SystemCoveredAndDurations { get; set; }
        public double PercentageOfLabor { get; set; }
        public double PercentageOfPart { get; set; }
        public string PriorRental { get; set; }
        public string Message { get; set; }
        public bool IsPreview { get; set; }
        public bool IsMixed { get; set; }


        public int WarrantyTypeSpanish { get; set; }
        public bool ServiceContractSpanish { get; set; }
        public bool IsAsWarrantySpanish { get; set; }
        public bool IsManufacturerWarrantySpanish { get; set; }
        public bool IsDealerCertifiedSpanish { get; set; }
        public bool IsManufacturerCertifiedSpanish { get; set; }
        public bool IsWarrantySpanish { get; set; }
        public bool IsFullWarrantySpanish { get; set; }
        public bool IsLimitedWarrantySpanish { get; set; }
        public bool IsServiceContractSpanish { get; set; }
        public bool IsPriorRentalSpanish { get; set; }
        public bool IsBrandedTitlelSpanish { get; set; }
        public string SystemCoveredSpanish { get; set; }
        public string DurationsSpanish { get; set; }
        public string SystemCoveredAndDurationsSpanish { get; set; }
        public double PercentageOfLaborSpanish { get; set; }
        public double PercentageOfPartSpanish { get; set; }
        public string PriorRentalSpanish { get; set; }
        public string MessageSpanish { get; set; }
        public bool IsPreviewSpanish { get; set; }
        public bool IsMixedSpanish { get; set; }
        public int SelectedLanguage { get; set; }
    }
}
