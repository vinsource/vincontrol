using System;
using System.Collections.Generic;
using System.Web.Mvc;
using vincontrol.Application.ViewModels.CommonManagement;
using Vincontrol.Web.HelperClass;


namespace Vincontrol.Web.Models
{
    public class KarPowerViewModel
    {
        public KarPowerViewModel()
        {
            Makes = new List<SelectListItem>();
            Models = new List<SelectListItem>();
            Trims = new List<SelectListItem>();
            Transmissions = new List<SelectListItem>();
            DriveTrains = new List<SelectListItem>();
            Engines = new List<SelectListItem>();
            Reports = new List<SelectListItem>();
            OptionalEquipmentMarkupList = new List<OptionContract>();
            OptionalEquipmentHistoryList = new List<OptionContract>();
            IsMultipleTrims = true;
            HasVin = true;
        }

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
        public IList<SelectListItem> Makes { get; set; }
        public IList<SelectListItem> Models { get; set; }
        public IList<SelectListItem> Trims { get; set; }
        public IList<SelectListItem> Transmissions { get; set; }
        public IList<SelectListItem> DriveTrains { get; set; }
        public IList<SelectListItem> Engines { get; set; }
        public List<SelectListItem> Reports { get; set; }
        public string BaseWholesaleWithoutOptions { get; set; }
        public string BaseWholesale { get; set; }
        public string MileageAdjustment { get; set; }
        public string Wholesale { get; set; }
        public string OptionalEquipmentMarkup { get; set; }
        public IList<OptionContract> OptionalEquipmentMarkupList { get; set; }
        public IList<OptionContract> OptionalEquipmentHistoryList { get; set; }
        public string SelectedOptionIds { get; set; }
        public bool IsMultipleTrims { get; set; }
        public string DownloadTokenValueId { get; set; }
        public bool HasVin { get; set; }
        public string Type { get; set; }
    }

    public class SmallKarPowerViewModel
    {
        public int SelectedTrimId { get; set; }
        public string SelectedTrimName { get; set; }
        public decimal BaseWholesale { get; set; }
        public decimal MileageAdjustment { get; set; }
        public decimal Wholesale { get; set; }
        public bool IsSelected { get; set; }
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
}