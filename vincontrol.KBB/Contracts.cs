using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.KBB
{
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

    public class OptionContract
    {
        public string __type { get; set; } // KBB.Karpower.WebServices.LightOption
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
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
