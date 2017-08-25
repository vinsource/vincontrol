using System;
using System.Collections.Generic;
using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class KarPowerViewModel
    {
        public KarPowerViewModel()
        {
            Makes = new List<ExtendedSelectListItem>();
            Models = new List<ExtendedSelectListItem>();
            Trims = new List<ExtendedSelectListItem>();
            Transmissions = new List<ExtendedSelectListItem>();
            DriveTrains = new List<ExtendedSelectListItem>();
            Engines = new List<ExtendedSelectListItem>();
            Reports = new List<ExtendedSelectListItem>();
            OptionalEquipmentMarkupList = new List<OptionContract>();
            OptionalEquipmentHistoryList = new List<OptionContract>();
            IsMultipleModels = false;
            IsMultipleTrims = true;
            HasVin = true;
        }

        public int InventoryId { get; set; }
        public string Vin { get; set; }
        public string EncodedVin { get; set; }
        public string EncodedStockNumber { get; set; }
        public string EncodedInventoryId { get; set; }
        public string EncodedInventoryCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SelectedYearId { get; set; }
        public int SelectedMakeId { get; set; }
        public int SelectedModelId { get; set; }
        public int SelectedTrimId { get; set; }
        public int SelectedEngineId { get; set; }
        public int SelectedTransmissionId { get; set; }
        public int SelectedDriveTrainId { get; set; }
        public int SelectedMileage { get; set; }
        public string SelectedReport { get; set; }
        public DateTime ValuationDate { get; set; }
        public IList<ExtendedSelectListItem> Makes { get; set; }
        public IList<ExtendedSelectListItem> Models { get; set; }
        public IList<ExtendedSelectListItem> Trims { get; set; }
        public IList<ExtendedSelectListItem> Transmissions { get; set; }
        public IList<ExtendedSelectListItem> DriveTrains { get; set; }
        public IList<ExtendedSelectListItem> Engines { get; set; }
        public List<ExtendedSelectListItem> Reports { get; set; }
        public string BaseWholesaleWithoutOptions { get; set; }
        public string BaseWholesale { get; set; }
        public string MileageAdjustment { get; set; }
        public string Wholesale { get; set; }
        public string OptionalEquipmentMarkup { get; set; }
        public IList<OptionContract> OptionalEquipmentMarkupList { get; set; }
        public IList<OptionContract> OptionalEquipmentHistoryList { get; set; }
        public string SelectedOptionIds { get; set; }
        public bool IsMultipleModels { get; set; }
        public bool IsMultipleTrims { get; set; }
        public string DownloadTokenValueId { get; set; }
        public bool HasVin { get; set; }
        public string Type { get; set; }
    }

    public class SmallKarPowerViewModel
    {
        public int SelectedModelId { get; set; }
        public int SelectedTrimId { get; set; }
        public string SelectedTrimName { get; set; }
        public decimal BaseWholesale { get; set; }
        public decimal MileageAdjustment { get; set; }
        public decimal Wholesale { get; set; }
        public decimal AddsDeducts { get; set; }
        public bool IsSelected { get; set; }
        public int TotalCount { get; set; }
    }

    public class SimpleKarPowerContract
    {
        public SimpleKarPowerContract()
        {
            OptionalEquipmentMarkupList = new List<OptionContract>();
        }

        public int SelectedTrimId { get; set; }
        public string SelectedTrimName { get; set; }
        public string BaseWholesale { get; set; }
        public string MileageAdjustment { get; set; }
        public string Wholesale { get; set; }
        public IList<OptionContract> OptionalEquipmentMarkupList { get; set; }
    }

    public class OptionContract
    {
        public string __type { get; set; } // KBB.Karpower.WebServices.LightOption
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
        public long Price { get; set; }
    }

    public class GetVehicleByVinContract
    {
        public string vin { get; set; }
        public string valuationDate { get; set; }
    }

    public class GetYearsContract
    {
        public string vrsVersionDate { get; set; }
    }

    public class GetMakesContract
    {
        public string yearId { get; set; }
        public string valuationDate { get; set; }
    }

    public class GetModelsContract
    {
        public string yearId { get; set; }
        public string makeId { get; set; }
        public string valuationDate { get; set; }
    }

    public class GetTrimsContract
    {
        public string yearId { get; set; }
        public string modelId { get; set; }
        public string valuationDate { get; set; }
    }

    public class GetPartiallyDecodedTrainsWithUserContract
    {
        public string vin { get; set; }
        public string valuationDate { get; set; }
        public string vehicleId { get; set; }
        public string engineId { get; set; }
        public string transId { get; set; }
    }

    public class GetOptionalEquipmentOptionChangedWithUserContract
    {
        public OptionContract changedOption { get; set; }
        public OptionContract[] currentOptions { get; set; }
        public string valuationDate { get; set; }
        public string vrsVehicleId { get; set; }
    }

    public class GetValuationContract
    {
        public string inventoryEntryId { get; set; }
        public string mileage { get; set; }
        public OptionContract[] optionHistory { get; set; }
        public OptionContract[] currentOptionsList { get; set; }
        public string valuationDate { get; set; }
        public string vehicleCondition { get; set; }
        public string vrsVehicleId { get; set; }
    }

    public class GetEnginesContract
    {
        public string vehicleId { get; set; }
        public string valuationDate { get; set; }
    }

    public class GetDefaultOptionalEquipmentWithUserContract
    {
        public string vrsVehicleId { get; set; }
        public string valuationDate { get; set; }
    }

    public class SaveVrsVehicleContract
    {
        public string category { get; set; }
        public bool certified { get; set; }
        public string drivetrain { get; set; }
        public string drivetrainId { get; set; }
        public string engine { get; set; }
        public string engineId { get; set; }
        public string exteriorColor { get; set; }
        public string initialDate { get; set; }
        public string interiorColor { get; set; }
        public string inventoryEntryId { get; set; }
        public bool isPreOwnedSessionVehicle { get; set; }
        public string make { get; set; }
        public string makeId { get; set; }
        public long mileage { get; set; }
        public string model { get; set; }
        public string modelId { get; set; }
        public OptionContract[] optionHistory { get; set; }
        public OptionContract[] options { get; set; }
        public string sellPrice { get; set; }
        public string stockNumber { get; set; }
        public string transmission { get; set; }
        public string transmissionId { get; set; }
        public string trim { get; set; }
        public string trimId { get; set; }
        public string valuationDate { get; set; }
        public string vehicleCondition { get; set; }
        public string vin { get; set; }
        public string webSellPrice { get; set; }
        public string year { get; set; }
        public string yearId { get; set; }
    }

    public class GetReportParametersContract
    {
        public string inventoryEntryID { get; set; }
        public string reportID { get; set; }
        public string reportName { get; set; }
        public string sessionStockNum { get; set; }
        public string sessionVIN { get; set; }
    }
}