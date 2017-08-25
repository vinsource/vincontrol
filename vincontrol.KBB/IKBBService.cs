using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;

namespace vincontrol.KBB
{
    public interface IKBBService
    {
        bool IsAuthorized(string userName, string password);
        void ExecuteGetVehicleByVin(string vin, DateTime valuationDate);
        void ExecuteGetVehicleByVin(string vin, DateTime valuationDate, string userName, string password);
        void ExecuteGetPartiallyDecodedTrainsWithUser(string vin, DateTime valuationDate, int vehicleId, int engineId, int transId);
        void ExecuteGetPartiallyDecodedTransmissionsWithUser(string vin, DateTime valuationDate, int vehicleId, int engineId);
        string ExecuteGetValuation(int vrsVehicleId, DateTime valuationDate, int mileage, int vehicleCondition, int inventoryEntryId, OptionContract[] optionHistory, OptionContract[] currentOptionsList);
        void ExecuteGetOptionalEquipmentOptionChangedWithUser(int vrsVehicleId, DateTime valuationDate, OptionContract changedOption, OptionContract[] currentOptionsList);
        string ExecuteGetTrims(int year, int modelId, DateTime valuationDate);
        string ExecuteGetEngines(int vehicleId, DateTime valuationDate);
        string ExecuteGetTransmissions(int vehicleId, DateTime valuationDate);
        string ExecuteGetDriveTrains(int vehicleId, DateTime valuationDate);
        string ExecuteGetDefaultOptionalEquipmentWithUser(int vehicleId, DateTime valuationDate);
        void ExecuteGetListCustomerReports(int vehicleId, DateTime valuationDate);
        void ExecuteSaveVrsVehicle(SaveVrsVehicleContract contract);
        void ExecuteGetReportParameters(string inventoryEntryId, string reportId, string reportName, string sessionStockNum, string sessionVin);
        Stream ExecuteGetPreOwnedVehicleReport(string hfReportParams);
        void Execute(string userName, string password);
        void ExecuteSaveVehicleWithVin(string vin, long mileage, string exteriorColor, string interiorColor, string userName, string password);
        void ExecuteSaveVehicleWithVin(string vin, string entryId, int trimId, string savedOptionIds, long mileage, string exteriorColor, string interiorColor, string userName, string password);
        List<SmallKarPowerViewModel> GetKarPowerFromDatabase(string vin);
        List<SmallKarPowerViewModel> GetKarPowerFromDatabase(int vehicleId);
        List<SmallKarPowerViewModel> Execute(int vehicleId, string vin, string mileage, DateTime valuationDate, string userName, string password, short type);
        List<SmallKarPowerViewModel> Execute(string vin, string mileage, DateTime valuationDate, string userName, string password, short type);
        List<SmallKarPowerViewModel> Execute(int vehicleId, string vin, string mileage, int saveTrimId, string saveOptionIds, DateTime valuationDate, string userName, string password, short type);
        SimpleKarPowerContract Execute(string vin, string mileage, DateTime valuationDate, int trimId, string userName, string password);
        SimpleKarPowerContract ExecuteWithoutTrim(string vin, string mileage, DateTime valuationDate);
        int GetMileageAdjustment(string vin, string dealerCarMileage, string marketCarMileage, DateTime valuationDate, string userName, string password);
        void GetYears(DateTime vrsVersionDate);
        void GetMakes(int yearId, DateTime valuationDate);
        void GetModels(int yearId, int makeId, DateTime valuationDate);
        string GetTrims(int yearId, int modelId, DateTime valuationDate);
        int ConvertToInt32(JValue obj);
        string ConvertToString(JValue obj);
        DateTime ConvertToDateTime(JValue obj);
        List<ExtendedSelectListItem> CreateDataList(JToken obj, int selectedId);
        List<OptionContract> CreateOptionalEquipmentList(string data);
        KarPowerViewModel CreateViewModelForKarPowerResult(string kbbUserName, string kbbPassword, string vin, string mileage, string trimId, string type, DealerUser dealerUser);
        KarPowerViewModel CreateViewModelForUpdateValuationByOptionalEquipment(string listingId, bool isChecked, KarPowerViewModel karPowerViewModel, CookieContainer container, CookieCollection collection);
        KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId, int transmissionId, CookieContainer container, CookieCollection collection, KarPowerViewModel model);
        Stream PrintReport(KarPowerViewModel model, string kbbUsername, string kbbPassword, CookieContainer cookieContainer, CookieCollection cookieCollection, int dealerId);
    }
}
