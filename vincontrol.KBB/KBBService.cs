using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using vincontrol.DomainObject;
using Newtonsoft.Json.Linq;
using vincontrol.Data.Model;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using vincontrol.Application.ViewModels.CommonManagement;

namespace vincontrol.KBB
{
    public class KBBService : IKBBService
    {
        #region Const
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; " +
                                         "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                         ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                         "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string AuthenticityTokenPattern = "<input name=\"authenticity_token\" type=\"hidden\" value=\"([^\\\"]*)\" />";
        private const string LogInUrl = "http://www.karpower.com/Marketing/index.aspx";
        private const string SubmitLogInUrl = "http://www.karpower.com/Marketing/index.aspx";//"http://www.karpower.com/Vehicle/VehicleInfo/VehicleInfo.aspx";
        private const string RefererUrl = "http://www.karpower.com/Marketing/index.aspx";
        private const string DefaultUserName = "phillip";
        private const string DefaultPassword = "kaitlyn1";
        private string[] InvalidValues = {"Year...", "[Choose One]", "0"};
        #endregion
        // Vehicle Conditions
        // 0: Exellent
        // 1: Very Good
        // 2: Good
        // 3: Fair
        // 4: Poor

        // Category
        // Appraisal: ca090fcb-597d-482d-b5aa-f528dd7bba21
        // Pre-Owned Retail: ff95ccb7-ce93-40f7-a060-70ae0ab0368f
        // Wholesale: 49633639-c404-4719-b28d-432c554dbe6d
        // Sold: 3836789f-8180-43ac-a378-f7e53f485bb8
        // Pending: a9de214c-9990-4151-a1cc-dadec3da3392
        // Other: 750da92c-8d07-4da3-8dab-41ef91a366fc

        #region Properties
        public CookieContainer CookieContainer { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public string Result { get; set; }
        public string AuthenticityToken { get; set; }
        public string PostData { get; set; }
        public string Sid { get; set; }
        public string EntryId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ViewState { get; set; }
        public string EventValidation { get; set; }
        public string ReportParameters { get; set; }
        public string GetVehicleByVinResult { get; set; }
        public string GetPartiallyDecodedTrainsWithUserResult { get; set; }
        public string GetPartiallyDecodedTransmissionsWithUserResult { get; set; }
        public string GetValuationResult { get; set; }
        public string GetOptionalEquipmentOptionChangedWithUserResult { get; set; }
        public string GetEnginesResult { get; set; }
        public string GetTransmissionsResult { get; set; }
        public string GetDriveTrainsResult { get; set; }
        public string GetDefaultOptionalEquipmentWithUserResult { get; set; }
        public string GetListCustomerReportsResult { get; set; }
        public string SaveVrsVehicleResult { get; set; }
        public string GetReportParametersResult { get; set; }
        public string GetPreOwnedVehicleReportResult { get; set; }

        #endregion

        public KBBService()
        {
            UserName = DefaultUserName;
            Password = DefaultPassword;
            CookieContainer = new CookieContainer();
            CookieCollection = new CookieCollection();
        }

        #region Public Methods

        public void ExecuteGetVehicleByVin(string vin, DateTime valuationDate)
        {
            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
            }
            catch (Exception)
            {
                
            }
            
        }

        public void ExecuteGetVehicleByVin(string vin, DateTime valuationDate, string userName, string password)
        {
            try
            {
                UserName = userName;
                Password = password;

                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
            }
            catch (Exception)
            {
                
            }
            
        }

        public void ExecuteGetPartiallyDecodedTrainsWithUser(string vin, DateTime valuationDate, int vehicleId, int engineId, int transId)
        {
            try
            {
                GetPartiallyDecodedTrainsWithUser(vin, valuationDate, vehicleId, engineId, transId);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetPartiallyDecodedTrainsWithUser(vin, valuationDate, vehicleId, engineId, transId);
            }
        }

        public void ExecuteGetPartiallyDecodedTransmissionsWithUser(string vin, DateTime valuationDate, int vehicleId, int engineId)
        {
            try
            {
                GetPartiallyDecodedTransmissionsWithUser(vin, valuationDate, vehicleId, engineId);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetPartiallyDecodedTransmissionsWithUser(vin, valuationDate, vehicleId, engineId);
            }
        }

        public string ExecuteGetValuation(int vrsVehicleId, DateTime valuationDate, int mileage, int vehicleCondition, int inventoryEntryId, OptionContract[] optionHistory, OptionContract[] currentOptionsList)
        {
            try
            {
                return GetValuation(vrsVehicleId, valuationDate, mileage, vehicleCondition, inventoryEntryId, optionHistory, currentOptionsList);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                return GetValuation(vrsVehicleId, valuationDate, mileage, vehicleCondition, inventoryEntryId, optionHistory, currentOptionsList);
            }
        }

        public void ExecuteGetOptionalEquipmentOptionChangedWithUser(int vrsVehicleId, DateTime valuationDate, OptionContract changedOption, OptionContract[] currentOptionsList)
        {
            try
            {
                GetOptionalEquipmentOptionChangedWithUser(vrsVehicleId, valuationDate, changedOption, currentOptionsList);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetOptionalEquipmentOptionChangedWithUser(vrsVehicleId, valuationDate, changedOption, currentOptionsList);
            }
        }

        public string ExecuteGetTrims(int year, int modelId, DateTime valuationDate)
        {
            try
            {
                return GetTrims(year, modelId, valuationDate);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string ExecuteGetEngines(int vehicleId, DateTime valuationDate)
        {
            try
            {
                return GetEngines(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }

        public string ExecuteGetTransmissions(int vehicleId, DateTime valuationDate)
        {
            try
            {
                return GetTransmissions(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }

        public string ExecuteGetDriveTrains(int vehicleId, DateTime valuationDate)
        {
            try
            {
                return GetDriveTrains(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }

        public string ExecuteGetDefaultOptionalEquipmentWithUser(int vehicleId, DateTime valuationDate)
        {
            try
            {
                return GetDefaultOptionalEquipmentWithUser(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                return GetDefaultOptionalEquipmentWithUser(vehicleId, valuationDate);
            }
        }

        public void ExecuteGetListCustomerReports(int vehicleId, DateTime valuationDate)
        {
            try
            {
                GetListCustomerReports(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetListCustomerReports(vehicleId, valuationDate);
            }

        }

        public void ExecuteSaveVrsVehicle(SaveVrsVehicleContract contract)
        {
            try
            {
                SaveVrsVehicle(contract);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                SaveVrsVehicle(contract);
            }
        }

        public void ExecuteGetReportParameters(string inventoryEntryId, string reportId, string reportName, string sessionStockNum, string sessionVin)
        {
            try
            {
                GetReportParameters(inventoryEntryId, reportId, reportName, sessionStockNum, sessionVin);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetReportParameters(inventoryEntryId, reportId, reportName, sessionStockNum, sessionVin);
            }
        }

        public Stream ExecuteGetPreOwnedVehicleReport(string hfReportParams)
        {
            try
            {
                return GetPreOwnedVehicleReport(hfReportParams);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                return GetPreOwnedVehicleReport(hfReportParams);
            }
        }

        public void Execute(string userName, string password)
        {
            UserName = userName;
            Password = password;

            WebRequestPost();
        }

        public void ExecuteSaveVehicleWithVin(string vin, long mileage, string exteriorColor, string interiorColor, string userName, string password)
        {
            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, DateTime.Now);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject) JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    var selectedYearId = ConvertToInt32((JValue) (jsonObj["d"]["yearId"]));
                    var selectedMakeId = ConvertToInt32(((JValue) (jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue) (jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue) (jsonObj["d"]["trimId"])));
                    var selectedEngineId = ConvertToInt32(((JValue) (jsonObj["d"]["engineId"])));
                    var selectedTransmissionId = ConvertToInt32(((JValue) (jsonObj["d"]["transmissionId"])));
                    var selectedDriveTrainId = ConvertToInt32(((JValue) (jsonObj["d"]["drivetrainId"])));
                    var valuationDate = ConvertToDateTime(((JValue) (jsonObj["d"]["valuationDate"])));
                    var optionalEquipmentMarkup = ConvertToString((JValue) (jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get makes
                    var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get engines
                    var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    // get drive trains
                    var driveTrains = CreateDataList(jsonObj["d"]["drivetrains"], selectedDriveTrainId);

                    // multiple engines
                    if (selectedEngineId == 0)
                    {
                        var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                        if (firstEngine != null)
                        {
                            firstEngine.Selected = true;
                            selectedEngineId = Convert.ToInt32(firstEngine.Value);
                        }

                        ExecuteGetPartiallyDecodedTransmissionsWithUser(vin, valuationDate, selectedTrimId, selectedEngineId);
                        if (!String.IsNullOrEmpty(GetPartiallyDecodedTransmissionsWithUserResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(GetPartiallyDecodedTransmissionsWithUserResult);
                            selectedTransmissionId = ConvertToInt32((JValue)(jsonObj["d"]["transmissionId"]));
                            selectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));

                            transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);
                            driveTrains = CreateDataList(jsonObj["d"]["drivetrains"], selectedDriveTrainId);
                            optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));
                        }
                    }

                    // multiple transmissions
                    if (selectedTransmissionId == 0)
                    {
                        try
                        {
                            var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                            if (firstTransmission != null)
                            {
                                firstTransmission.Selected = true;
                                selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                            }

                            ExecuteGetPartiallyDecodedTrainsWithUser(vin, valuationDate, selectedTrimId, selectedEngineId, selectedTransmissionId);
                            if (!String.IsNullOrEmpty(GetPartiallyDecodedTrainsWithUserResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetPartiallyDecodedTrainsWithUserResult);
                                selectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));
                                optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                                // get drive trains
                                driveTrains = CreateDataList(jsonObj["d"]["drivetrains"], selectedDriveTrainId);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    var optionalEquipmentMarkupList = CreateOptionalEquipmentList(optionalEquipmentMarkup);
                    var selectedMake = makes.FirstOrDefault(i => i.Value == selectedMakeId.ToString());
                    var selectedModel = models.FirstOrDefault(i => i.Value == selectedModelId.ToString());
                    var selectedTrim = trims.FirstOrDefault(i => i.Value == selectedTrimId.ToString());
                    var selectedEngine = engines.FirstOrDefault(i => i.Value == selectedEngineId.ToString());
                    var selectedTransmission = transmissions.FirstOrDefault(i => i.Value == selectedTransmissionId.ToString());
                    var selectedDriveTrain = driveTrains.FirstOrDefault(i => i.Value == selectedDriveTrainId.ToString());
                    
                    var dataToSave = new SaveVrsVehicleContract()
                    {
                        category = "ca090fcb-597d-482d-b5aa-f528dd7bba21", // Appraisal
                        certified = false,
                        drivetrain = selectedDriveTrain != null ? selectedDriveTrain.Text : string.Empty,
                        drivetrainId = selectedDriveTrainId.ToString(),
                        engine = selectedEngine != null ? selectedEngine.Text : string.Empty,
                        engineId = selectedEngineId.ToString(),
                        exteriorColor = String.IsNullOrEmpty(exteriorColor) ? "Select Color or Enter Manually" : exteriorColor,
                        interiorColor = String.IsNullOrEmpty(interiorColor) ? "Select Color or Enter Manually" : interiorColor,
                        initialDate = valuationDate.ToString("M/d/yyyy"),
                        inventoryEntryId = "",
                        isPreOwnedSessionVehicle = false,
                        make = selectedMake != null ? selectedMake.Text : string.Empty,
                        makeId = selectedMakeId.ToString(),
                        mileage = mileage,
                        model = selectedModel != null ? selectedModel.Text : string.Empty,
                        modelId = selectedModelId.ToString(),
                        optionHistory = new List<OptionContract>().ToArray(),
                        options = optionalEquipmentMarkupList.ToArray(),
                        sellPrice = "",
                        stockNumber = "",
                        transmission = selectedTransmission != null ? selectedTransmission.Text : string.Empty,
                        transmissionId = selectedTransmissionId.ToString(),
                        trim = selectedTrim != null ? selectedTrim.Text : string.Empty,
                        trimId = selectedTrimId.ToString(),
                        valuationDate = valuationDate.ToString("M/d/yyyy"),
                        vehicleCondition = "2",
                        vin = vin,
                        webSellPrice = "",
                        year = selectedYearId.ToString(),
                        yearId = selectedYearId.ToString()
                    };

                    ExecuteSaveVrsVehicle(dataToSave);
                }
            }
            catch(Exception)
            {
                
            }
        }

        public void ExecuteSaveVehicleWithVin(string vin, string entryId, int trimId, string savedOptionIds, long mileage, string exteriorColor, string interiorColor, string userName, string password)
        {
            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, DateTime.Now);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    var selectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    var selectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = trimId; //ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    var selectedEngineId = 0; //ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    var selectedTransmissionId = 0; //ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    var selectedDriveTrainId = 0; // ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    var optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get makes
                    var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get makes
                    var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    // get drive trains
                    var driveTrains = CreateDataList(jsonObj["d"]["drivetrains"], selectedDriveTrainId);

                    // multiple engines
                    if (selectedEngineId == 0)
                    {
                        var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                        if (firstEngine != null)
                        {
                            firstEngine.Selected = true;
                            selectedEngineId = Convert.ToInt32(firstEngine.Value);
                        }

                        ExecuteGetPartiallyDecodedTransmissionsWithUser(vin, valuationDate, selectedTrimId, selectedEngineId);
                        if (!String.IsNullOrEmpty(GetPartiallyDecodedTransmissionsWithUserResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(GetPartiallyDecodedTransmissionsWithUserResult);
                            selectedTransmissionId = ConvertToInt32((JValue)(jsonObj["d"]["transmissionId"]));
                            selectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));

                            transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);
                            driveTrains = CreateDataList(jsonObj["d"]["drivetrains"], selectedDriveTrainId);
                            optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));
                        }
                    }

                    // multiple transmissions
                    if (selectedTransmissionId == 0)
                    {
                        try
                        {
                            var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                            if (firstTransmission != null)
                            {
                                firstTransmission.Selected = true;
                                selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                            }

                            ExecuteGetPartiallyDecodedTrainsWithUser(vin, valuationDate, selectedTrimId, selectedEngineId, selectedTransmissionId);
                            if (!String.IsNullOrEmpty(GetPartiallyDecodedTrainsWithUserResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetPartiallyDecodedTrainsWithUserResult);
                                selectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));
                                optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                                // get drive trains
                                driveTrains = CreateDataList(jsonObj["d"]["drivetrains"], selectedDriveTrainId);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    var optionalEquipmentMarkupList = CreateOptionalEquipmentList(optionalEquipmentMarkup);
                    foreach (var optionContract in optionalEquipmentMarkupList)
                    {
                        optionContract.IsSelected = savedOptionIds.Contains(optionContract.Id);
                    }
                    var selectedMake = makes.FirstOrDefault(i => i.Value == selectedMakeId.ToString());
                    var selectedModel = models.FirstOrDefault(i => i.Value == selectedModelId.ToString());
                    var selectedTrim = trims.FirstOrDefault(i => i.Value == selectedTrimId.ToString());
                    var selectedEngine = engines.FirstOrDefault(i => i.Value == selectedEngineId.ToString());
                    var selectedTransmission = transmissions.FirstOrDefault(i => i.Value == selectedTransmissionId.ToString());
                    var selectedDriveTrain = driveTrains.FirstOrDefault(i => i.Value == selectedDriveTrainId.ToString());

                    var dataToSave = new SaveVrsVehicleContract()
                    {
                        category = "ca090fcb-597d-482d-b5aa-f528dd7bba21", // Appraisal
                        certified = false,
                        drivetrain = selectedDriveTrain != null ? selectedDriveTrain.Text : string.Empty,
                        drivetrainId = selectedDriveTrainId.ToString(),
                        engine = selectedEngine != null ? selectedEngine.Text : string.Empty,
                        engineId = selectedEngineId.ToString(),
                        exteriorColor = String.IsNullOrEmpty(exteriorColor) ? "Select Color or Enter Manually" : exteriorColor,
                        interiorColor = String.IsNullOrEmpty(interiorColor) ? "Select Color or Enter Manually" : interiorColor,
                        initialDate = valuationDate.ToString("M/d/yyyy"),
                        inventoryEntryId = entryId,
                        isPreOwnedSessionVehicle = false,
                        make = selectedMake != null ? selectedMake.Text : string.Empty,
                        makeId = selectedMakeId.ToString(),
                        mileage = mileage,
                        model = selectedModel != null ? selectedModel.Text : string.Empty,
                        modelId = selectedModelId.ToString(),
                        optionHistory = new List<OptionContract>().ToArray(),
                        options = optionalEquipmentMarkupList.ToArray(),
                        sellPrice = "",
                        stockNumber = "",
                        transmission = selectedTransmission != null ? selectedTransmission.Text : string.Empty,
                        transmissionId = selectedTransmissionId.ToString(),
                        trim = selectedTrim != null ? selectedTrim.Text : string.Empty,
                        trimId = selectedTrimId.ToString(),
                        valuationDate = valuationDate.ToString("M/d/yyyy"),
                        vehicleCondition = "2",
                        vin = vin,
                        webSellPrice = "",
                        year = selectedYearId.ToString(),
                        yearId = selectedYearId.ToString()
                    };

                    ExecuteSaveVrsVehicle(dataToSave);
                    if (!String.IsNullOrEmpty(SaveVrsVehicleResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(SaveVrsVehicleResult);
                        EntryId = ConvertToString((JValue)(jsonObj["d"]["id"]));
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public List<SmallKarPowerViewModel> GetKarPowerFromDatabase(string vin)
        {
            var result = new List<SmallKarPowerViewModel>();
            try
            {
                using (var context = new VincontrolEntities())
                {

                    if (context.KellyBlueBooks.Any(x => x.Vin == vin))
                    {
                        var searchResult = context.KellyBlueBooks.Where(x => x.Vin == vin).ToList();

                        {
                            result = searchResult.Select(tmp => new SmallKarPowerViewModel
                            {
                                BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                                MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                                SelectedModelId = tmp.ModelId.GetValueOrDefault(),
                                SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                SelectedTrimName = tmp.Trim,
                                Wholesale = tmp.WholeSale.GetValueOrDefault(),
                                AddsDeducts =
                                    tmp.WholeSale.GetValueOrDefault() - tmp.BaseWholeSale.GetValueOrDefault() -
                                    tmp.MileageAdjustment.GetValueOrDefault(),
                            }).ToList();

                            if (result.Count > 0 && !result.Any(i => i.IsSelected))
                            {
                                foreach (var item in result)
                                {
                                    item.IsSelected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public List<SmallKarPowerViewModel> GetKarPowerFromDatabase(int vehicleId)
        {
            var result = new List<SmallKarPowerViewModel>();
            try
            {
                using (var context = new VincontrolEntities())
                {

                    if (context.KellyBlueBooks.Any(x => x.VehicleId == vehicleId))
                    {
                        var searchResult = context.KellyBlueBooks.Where(x => x.VehicleId == vehicleId).ToList();

                        {
                            result = searchResult.Select(tmp => new SmallKarPowerViewModel
                            {
                                BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                                MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                                SelectedModelId = tmp.ModelId.GetValueOrDefault(),
                                SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                SelectedTrimName = tmp.Trim,
                                Wholesale = tmp.WholeSale.GetValueOrDefault(),
                                AddsDeducts =
                                    tmp.WholeSale.GetValueOrDefault() - tmp.BaseWholeSale.GetValueOrDefault() -
                                    tmp.MileageAdjustment.GetValueOrDefault(),
                            }).ToList();

                            if (result.Count > 0 && !result.Any(i => i.IsSelected))
                            {
                                foreach (var item in result)
                                {
                                    item.IsSelected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            
            return result;
        }

        public List<SmallKarPowerViewModel> Execute(int vehicleId,string vin, string mileage, DateTime valuationDate, string userName, string password, short type)
        {
            var result = new List<SmallKarPowerViewModel>();

            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject) JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    var selectedYearId = ConvertToInt32((JValue) (jsonObj["d"]["yearId"]));
                    //var selectedMakeId = ConvertToInt32(((JValue) (jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue) (jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue) (jsonObj["d"]["trimId"])));
                    //var selectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    //var selectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    //var selectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    //var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    //var optionalEquipmentMarkup = ConvertToString((JValue) (jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    //var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    //var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get transmissions
                    //var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get engines
                    //var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    if (selectedModelId.Equals(0))
                    {
                        result.Add(new SmallKarPowerViewModel()
                        {
                            BaseWholesale = 0,
                            MileageAdjustment = 0,
                            Wholesale = 0,
                            SelectedModelId = 0,
                            SelectedTrimId = 0,
                            SelectedTrimName = "This VIN required selected model",
                            IsSelected = true
                        });

                        return result;
                    }

                    //In case, we have multiple model matching
                    //if (trims.All(i => InvalidValues.Contains(i.Value)))
                    foreach (var model in models.Where(i => !InvalidValues.Contains(i.Value) && i.Value.Equals(selectedModelId.ToString())))
                    {
                        var content = GetTrims(selectedYearId, Convert.ToInt32(model.Value), valuationDate);
                        jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                        var tempTrims = CreateDataList(jsonObj["d"], selectedTrimId);
                        //trims.AddRange(tempTrims);
                        var selectedEngineId = 0;
                        var selectedTransmissionId = 0;
                        var selectedDriveTrainId = 0;
                        foreach (var trim in tempTrims.Where(i => !InvalidValues.Contains(i.Value)))
                        {
                            // get Engines list
                            ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetEnginesResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                                var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                                if (selectedEngineId == 0)
                                {
                                    var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedEngineId = Convert.ToInt32(firstEngine.Value);
                                }
                            }

                            // get Transmissions list
                            ExecuteGetTransmissions(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetTransmissionsResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                                var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                                if (selectedTransmissionId == 0)
                                {
                                    var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                                }
                            }

                            // get Drive Trains list
                            ExecuteGetDriveTrains(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                                var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                                if (selectedDriveTrainId == 0)
                                {
                                    var firstDriveTrain = driveTrains.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                                }
                            }

                            ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                                var optionalEquipmentMarkupList =
                                    CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                                ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(mileage) ? 0 : Convert.ToInt32(mileage), 2,
                                                    0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                                if (!String.IsNullOrEmpty(GetValuationResult))
                                {
                                    jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);

                                    var newSmallKarPowerViewModel = new SmallKarPowerViewModel()
                                    {

                                        BaseWholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                        Wholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                        SelectedModelId = selectedModelId,
                                        SelectedTrimId = Convert.ToInt32(trim.Value),
                                        SelectedTrimName = trim.Text,
                                        IsSelected = true
                                    };
                                    var mileageAdjValue = Convert.ToString(
                                        ((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);

                                    if (mileageAdjValue.Contains("(") && mileageAdjValue.Contains(")"))
                                    {
                                        newSmallKarPowerViewModel.MileageAdjustment = (-1) * Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                    }
                                    else
                                    {
                                        newSmallKarPowerViewModel.MileageAdjustment = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                    }

                                    newSmallKarPowerViewModel.AddsDeducts = newSmallKarPowerViewModel.Wholesale -
                                                                            newSmallKarPowerViewModel.BaseWholesale -
                                                                            newSmallKarPowerViewModel.MileageAdjustment;

                                    result.Add(newSmallKarPowerViewModel);
                                }
                            }

                        }
                    }

                    AddSimpleKbbReportFromKarPower(vehicleId, result, vin, type);
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public List<SmallKarPowerViewModel> ExecuteVinsell(int vehicleId, string vin, string mileage, DateTime valuationDate, string userName, string password)
        {
            var result = new List<SmallKarPowerViewModel>();

            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    var selectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                 
                    // get makes
                    //var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    //var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get transmissions
                    //var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get engines
                    //var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    if (selectedModelId.Equals(0))
                    {
                        result.Add(new SmallKarPowerViewModel()
                        {
                            BaseWholesale = 0,
                            MileageAdjustment = 0,
                            Wholesale = 0,
                            SelectedModelId = 0,
                            SelectedTrimId = 0,
                            SelectedTrimName = "This VIN required selected model",
                            IsSelected = true
                        });

                        return result;
                    }

                    //In case, we have multiple model matching
                    //if (trims.All(i => InvalidValues.Contains(i.Value)))
                    foreach (var model in models.Where(i => !InvalidValues.Contains(i.Value) && i.Value.Equals(selectedModelId.ToString())))
                    {
                        var content = GetTrims(selectedYearId, Convert.ToInt32(model.Value), valuationDate);
                        jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                        var tempTrims = CreateDataList(jsonObj["d"], selectedTrimId);
                        //trims.AddRange(tempTrims);
                        var selectedEngineId = 0;
                        var selectedTransmissionId = 0;
                        var selectedDriveTrainId = 0;
                        foreach (var trim in tempTrims.Where(i => !InvalidValues.Contains(i.Value)))
                        {
                            // get Engines list
                            ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetEnginesResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                                var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                                if (selectedEngineId == 0)
                                {
                                    var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedEngineId = Convert.ToInt32(firstEngine.Value);
                                }
                            }

                            // get Transmissions list
                            ExecuteGetTransmissions(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetTransmissionsResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                                var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                                if (selectedTransmissionId == 0)
                                {
                                    var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                                }
                            }

                            // get Drive Trains list
                            ExecuteGetDriveTrains(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                                var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                                if (selectedDriveTrainId == 0)
                                {
                                    var firstDriveTrain = driveTrains.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                                }
                            }

                            ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                                var optionalEquipmentMarkupList =
                                    CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                                ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(mileage) ? 0 : Convert.ToInt32(mileage), 2,
                                                    0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                                if (!String.IsNullOrEmpty(GetValuationResult))
                                {
                                    jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);

                                    var newSmallKarPowerViewModel = new SmallKarPowerViewModel()
                                    {

                                        BaseWholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                        Wholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                        SelectedModelId = selectedModelId,
                                        SelectedTrimId = Convert.ToInt32(trim.Value),
                                        SelectedTrimName = trim.Text,
                                        IsSelected = true
                                    };
                                    var mileageAdjValue = Convert.ToString(
                                        ((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);

                                    if (mileageAdjValue.Contains("(") && mileageAdjValue.Contains(")"))
                                    {
                                        newSmallKarPowerViewModel.MileageAdjustment = (-1) * Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                    }
                                    else
                                    {
                                        newSmallKarPowerViewModel.MileageAdjustment = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                    }

                                    newSmallKarPowerViewModel.AddsDeducts = newSmallKarPowerViewModel.Wholesale -
                                                                            newSmallKarPowerViewModel.BaseWholesale -
                                                                            newSmallKarPowerViewModel.MileageAdjustment;

                                    result.Add(newSmallKarPowerViewModel);
                                }
                            }

                        }
                    }

                    AddSimpleKbbReportFromKarPowerToVinsell(vehicleId, result, vin);
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public List<SmallKarPowerViewModel> Execute(string vin, string mileage, DateTime valuationDate, string userName, string password, short type)
        {
            var result = new List<SmallKarPowerViewModel>();

            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    var selectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    //var selectedMakeId = ConvertToInt32(((JValue) (jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    //var selectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    //var selectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    //var selectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    //var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    //var optionalEquipmentMarkup = ConvertToString((JValue) (jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    //var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    //var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get transmissions
                    //var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get engines
                    //var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    if (selectedModelId.Equals(0))
                    {
                        result.Add(new SmallKarPowerViewModel()
                        {
                            BaseWholesale = 0,
                            MileageAdjustment = 0,
                            Wholesale = 0,
                            SelectedModelId = 0,
                            SelectedTrimId = 0,
                            SelectedTrimName = "This VIN required selected model",
                            IsSelected = true
                        });

                        return result;
                    }

                    //In case, we have multiple model matching
                    //if (trims.All(i => InvalidValues.Contains(i.Value)))
                    foreach (var model in models.Where(i => !InvalidValues.Contains(i.Value) && i.Value.Equals(selectedModelId.ToString())))
                    {
                        var content = GetTrims(selectedYearId, Convert.ToInt32(model.Value), valuationDate);
                        jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                        var tempTrims = CreateDataList(jsonObj["d"], selectedTrimId);
                        //trims.AddRange(tempTrims);
                        var selectedEngineId = 0;
                        var selectedTransmissionId = 0;
                        var selectedDriveTrainId = 0;
                        foreach (var trim in tempTrims.Where(i => !InvalidValues.Contains(i.Value)))
                        {
                            // get Engines list
                            ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetEnginesResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                                var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                                if (selectedEngineId == 0)
                                {
                                    var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedEngineId = Convert.ToInt32(firstEngine.Value);
                                }
                            }

                            // get Transmissions list
                            ExecuteGetTransmissions(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetTransmissionsResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                                var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                                if (selectedTransmissionId == 0)
                                {
                                    var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                                }
                            }

                            // get Drive Trains list
                            ExecuteGetDriveTrains(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                                var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                                if (selectedDriveTrainId == 0)
                                {
                                    var firstDriveTrain = driveTrains.FirstOrDefault(i => IsValidItem(i.Value));
                                    selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                                }
                            }

                            ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(trim.Value), valuationDate);
                            if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                                var optionalEquipmentMarkupList =
                                    CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                                ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(mileage) ? 0 : Convert.ToInt32(mileage), 2,
                                                    0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                                if (!String.IsNullOrEmpty(GetValuationResult))
                                {
                                    jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                                    result.Add(new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = ParseDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                        MileageAdjustment = ParseDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value))),
                                        Wholesale = ParseDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                        SelectedModelId = Convert.ToInt32(model.Value),
                                        SelectedTrimId = Convert.ToInt32(trim.Value),
                                        SelectedTrimName = trim.Text,
                                        IsSelected = true
                                    });
                                }
                            }

                        }
                    }

                    AddSimpleKbbReportFromKarPower(result, vin, type);
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }
        
        public List<SmallKarPowerViewModel> Execute(int vehicleId, string vin, string mileage, int saveTrimId, string saveOptionIds, DateTime valuationDate, string userName, string password, short type)
        {
            var result = new List<SmallKarPowerViewModel>();
         
            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                var getVehicleByVinResult = GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(getVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(getVehicleByVinResult);
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    
                    // get trims
                    var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    var selectedEngineId = 0;
                    var selectedTransmissionId = 0;
                    var selectedDriveTrainId = 0;
                    foreach (var trim in trims.Where(i => !InvalidValues.Contains(i.Value)))
                    {
                        // get Engines list
                        var getEnginesResult = ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(getEnginesResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(getEnginesResult);
                            var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                            if (selectedEngineId == 0)
                            {
                                var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                                selectedEngineId = Convert.ToInt32(firstEngine.Value);
                            }
                        }

                        // get Transmissions list
                        var getTransmissionsResult = ExecuteGetTransmissions(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(getTransmissionsResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(getTransmissionsResult);
                            var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                            if (selectedTransmissionId == 0)
                            {
                                var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                                selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                            }
                        }

                        // get Drive Trains list
                        var getDriveTrainsResult = ExecuteGetDriveTrains(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(getDriveTrainsResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(getDriveTrainsResult);
                            var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                            if (selectedDriveTrainId == 0)
                            {
                                var firstDriveTrain = driveTrains.FirstOrDefault(i => IsValidItem(i.Value));
                                selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                            }
                        }

                        var getDefaultOptionalEquipmentWithUserResult = ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(getDefaultOptionalEquipmentWithUserResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(getDefaultOptionalEquipmentWithUserResult);
                            var optionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                            // loading save options in database
                            if (trim.Value == saveTrimId.ToString())
                            {
                                foreach (var optionContract in optionalEquipmentMarkupList)
                                {
                                    optionContract.IsSelected = saveOptionIds.Contains(optionContract.Id);
                                }
                            }

                            var getValuationResult = ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(mileage) ? 0 : Convert.ToInt32(mileage), 2,
                                                0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                            if (!String.IsNullOrEmpty(getValuationResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(getValuationResult);

                                var newSmallKarPowerViewModel = new SmallKarPowerViewModel()
                                {

                                    BaseWholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                    Wholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                    SelectedModelId = selectedModelId,
                                    SelectedTrimId = Convert.ToInt32(trim.Value),
                                    SelectedTrimName = trim.Text
                                };
                                var mileageAdjValue = Convert.ToString(
                                    ((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);

                                if (mileageAdjValue.Contains("(") && mileageAdjValue.Contains(")"))
                                {
                                    newSmallKarPowerViewModel.MileageAdjustment = (-1) * Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                }
                                else
                                {
                                    newSmallKarPowerViewModel.MileageAdjustment = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                }

                                newSmallKarPowerViewModel.AddsDeducts = newSmallKarPowerViewModel.Wholesale -
                                                                        newSmallKarPowerViewModel.BaseWholesale -
                                                                        newSmallKarPowerViewModel.MileageAdjustment;

                                result.Add(newSmallKarPowerViewModel);
                            }
                        }

                    }

                    AddSimpleKbbReportFromKarPower(vehicleId, result, vin, type);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("ERROR: " + vin + " (" + ex.Message + ")");
            }

            return result;
        }

        public SimpleKarPowerContract Execute(string vin, string mileage, DateTime valuationDate, int trimId, string userName, string password)
        {
            var result = new SimpleKarPowerContract();
            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    //var selectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    var selectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    //var selectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    //var selectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    //var selectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    //var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    //var optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    //var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    //var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    //var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get makes
                    //var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get makes
                    //var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    var selectedEngineId = 0;
                    var selectedTransmissionId = 0;
                    var selectedDriveTrainId = 0;
                    // get Engines list
                    ExecuteGetEngines(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetEnginesResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                        var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                        if (selectedEngineId == 0)
                        {
                            var firstEngine = engines.FirstOrDefault(i => i.Value != "0");
                            //selectedEngineId = Convert.ToInt32(firstEngine.Value);
                        }
                    }

                    // get Transmissions list
                    ExecuteGetTransmissions(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetTransmissionsResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                        var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                        if (selectedTransmissionId == 0)
                        {
                            var firstTransmission = transmissions.FirstOrDefault(i => i.Value != "0");
                            //selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                        }
                    }

                    // get Drive Trains list
                    ExecuteGetDriveTrains(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                        var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                        if (selectedDriveTrainId == 0)
                        {
                            var firstDriveTrain = driveTrains.FirstOrDefault(i => i.Value != "0");
                            //selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                        }
                    }

                    ExecuteGetDefaultOptionalEquipmentWithUser(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                        var optionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                        ExecuteGetValuation(trimId, valuationDate, Convert.ToInt32(mileage), 2, 0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                        if (!String.IsNullOrEmpty(GetValuationResult))
                        {
                            //jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                            result = new SimpleKarPowerContract()
                            {
                                OptionalEquipmentMarkupList = optionalEquipmentMarkupList
                            };
                        }
                    }


                }
            }
            catch (Exception)
            {

            }

            return result;
        }
                
        public SimpleKarPowerContract ExecuteWithoutTrim(string vin, string mileage, DateTime valuationDate)
        {
            var result = new SimpleKarPowerContract();
            //UserName = userName;
            //Password = password;

            try
            {
                //WebRequestGet();
                //WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    //var selectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    var selectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    //var selectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    //var selectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    //var selectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    //var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    //var optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    //var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    //var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get makes
                    //var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get makes
                    //var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    var selectedEngineId = 0;
                    var selectedTransmissionId = 0;
                    var selectedDriveTrainId = 0;
                    var firstTrim = trims.FirstOrDefault(i => i.Value != "0");
                    var trimId = Convert.ToInt32(firstTrim.Value);
                    
                    // get Engines list
                    ExecuteGetEngines(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetEnginesResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                        var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                        if (selectedEngineId == 0)
                        {
                            var firstEngine = engines.FirstOrDefault(i => i.Value != "0");
                            //selectedEngineId = Convert.ToInt32(firstEngine.Value);
                        }
                    }

                    // get Transmissions list
                    ExecuteGetTransmissions(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetTransmissionsResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                        var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                        if (selectedTransmissionId == 0)
                        {
                            var firstTransmission = transmissions.FirstOrDefault(i => i.Value != "0");
                            //selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                        }
                    }

                    // get Drive Trains list
                    ExecuteGetDriveTrains(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                        var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                        if (selectedDriveTrainId == 0)
                        {
                            var firstDriveTrain = driveTrains.FirstOrDefault(i => i.Value != "0");
                            //selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                        }
                    }

                    ExecuteGetDefaultOptionalEquipmentWithUser(trimId, valuationDate);
                    if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                        var optionalEquipmentMarkupList =
                            CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                        ExecuteGetValuation(trimId, valuationDate, Convert.ToInt32(mileage), 2,
                                            0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                        if (!String.IsNullOrEmpty(GetValuationResult))
                        {
                            //jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                            result = new SimpleKarPowerContract()
                            {
                                OptionalEquipmentMarkupList = optionalEquipmentMarkupList
                            };
                        }
                    }


                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public int GetMileageAdjustment(string vin, string dealerCarMileage, string marketCarMileage, DateTime valuationDate, string userName, string password)
        {
            var result = new List<SmallKarPowerViewModel>();

            UserName = userName;
            Password = password;

            try
            {
                WebRequestGet();
                WebRequestPost();

                GetVehicleByVin(vin, valuationDate);
                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    var selectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    var selectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    //var selectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    //var selectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    //var selectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    //var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    var optionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    var makes = CreateDataList(jsonObj["d"]["makes"], selectedMakeId);

                    // get models
                    var models = CreateDataList(jsonObj["d"]["models"], selectedModelId);

                    // get trims
                    var trims = CreateDataList(jsonObj["d"]["trims"], selectedTrimId);

                    // get makes
                    //var transmissions = CreateDataList(jsonObj["d"]["transmissions"], selectedTransmissionId);

                    // get makes
                    //var engines = CreateDataList(jsonObj["d"]["engines"], selectedEngineId);

                    var selectedEngineId = 0;
                    var selectedTransmissionId = 0;
                    var selectedDriveTrainId = 0;
                    var filterTrims = trims.Where(i => !InvalidValues.Contains(i.Value)).ToList();
                    if (!filterTrims.Any())
                    {
                        if (selectedModelId.Equals(0))
                            selectedModelId = Convert.ToInt32(models.Where(i => !InvalidValues.Contains(i.Value)).First().Value);
                        var content = GetTrims(selectedYearId, selectedModelId, valuationDate);
                        jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                        trims = CreateDataList(jsonObj["d"], selectedTrimId);
                        filterTrims = trims.Where(i => !InvalidValues.Contains(i.Value)).ToList();
                    }

                    foreach (var trim in filterTrims)
                    {
                        // get Engines list
                        ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(GetEnginesResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                            var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                            if (selectedEngineId == 0)
                            {
                                var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                                selectedEngineId = Convert.ToInt32(firstEngine.Value);
                            }
                        }

                        // get Transmissions list
                        ExecuteGetTransmissions(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(GetTransmissionsResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                            var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                            if (selectedTransmissionId == 0)
                            {
                                var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                                selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                            }
                        }

                        // get Drive Trains list
                        ExecuteGetDriveTrains(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                            var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                            if (selectedDriveTrainId == 0)
                            {
                                var firstDriveTrain = driveTrains.FirstOrDefault(i => IsValidItem(i.Value));
                                selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                            }
                        }

                        ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                        {
                            jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                            var optionalEquipmentMarkupList =
                                CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                            ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(dealerCarMileage) ? 0 : Convert.ToInt32(dealerCarMileage), 2,
                                                0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                            if (!String.IsNullOrEmpty(GetValuationResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                                var strMileageAdjustment = Convert.ToString(((JValue) (jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                                result.Add(new SmallKarPowerViewModel()
                                {
                                    BaseWholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                    MileageAdjustment = strMileageAdjustment.Contains("(") ? Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(strMileageAdjustment)) * -1 : Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(strMileageAdjustment)),
                                    Wholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                    SelectedTrimId = Convert.ToInt32(trim.Value),
                                    SelectedTrimName = trim.Text
                                });
                            }

                            ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(marketCarMileage) ? 0 : Convert.ToInt32(marketCarMileage), 2,
                                                0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                            if (!String.IsNullOrEmpty(GetValuationResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                                var strMileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                                result.Add(new SmallKarPowerViewModel()
                                {
                                    BaseWholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                    MileageAdjustment = strMileageAdjustment.Contains("(") ? Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(strMileageAdjustment)) * -1 : Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(strMileageAdjustment)),
                                    Wholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                    SelectedTrimId = Convert.ToInt32(trim.Value),
                                    SelectedTrimName = trim.Text
                                });
                            }
                        }

                        if (filterTrims.Count().Equals(1) || !result.Count(i => i.MileageAdjustment.Equals(0)).Equals(2)) break;
                        result.Clear();
                    }

                    //foreach (var trim in trims.Where(i => !InvalidValues.Contains(i.Value)))
                    //{
                    //    // get Engines list
                    //    ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                    //    if (!String.IsNullOrEmpty(GetEnginesResult))
                    //    {
                    //        jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                    //        var engines = CreateDataList(jsonObj["d"], selectedEngineId);
                    //        if (selectedEngineId == 0)
                    //        {
                    //            var firstEngine = engines.FirstOrDefault(i => IsValidItem(i.Value));
                    //            selectedEngineId = Convert.ToInt32(firstEngine.Value);
                    //        }
                    //    }

                    //    // get Transmissions list
                    //    ExecuteGetTransmissions(Convert.ToInt32(trim.Value), valuationDate);
                    //    if (!String.IsNullOrEmpty(GetTransmissionsResult))
                    //    {
                    //        jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                    //        var transmissions = CreateDataList(jsonObj["d"], selectedTransmissionId);
                    //        if (selectedTransmissionId == 0)
                    //        {
                    //            var firstTransmission = transmissions.FirstOrDefault(i => IsValidItem(i.Value));
                    //            selectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    //        }
                    //    }

                    //    // get Drive Trains list
                    //    ExecuteGetDriveTrains(Convert.ToInt32(trim.Value), valuationDate);
                    //    if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                    //    {
                    //        jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                    //        var driveTrains = CreateDataList(jsonObj["d"], selectedDriveTrainId);
                    //        if (selectedDriveTrainId == 0)
                    //        {
                    //            var firstDriveTrain = driveTrains.FirstOrDefault(i => IsValidItem(i.Value));
                    //            selectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    //        }
                    //    }

                    //    ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(trim.Value), valuationDate);
                    //    if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                    //    {
                    //        jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                    //        var optionalEquipmentMarkupList =
                    //            CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));

                    //        ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(marketCarMileage) ? 0 : Convert.ToInt32(marketCarMileage), 2,
                    //                            0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                    //        if (!String.IsNullOrEmpty(GetValuationResult))
                    //        {
                    //            jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                    //            var strMileageAdjustment = Convert.ToString(((JValue) (jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    //            result.Add(new SmallKarPowerViewModel()
                    //            {
                    //                BaseWholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                    //                MileageAdjustment = strMileageAdjustment.Contains("(") ? Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(strMileageAdjustment)) * -1 : Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(strMileageAdjustment)),
                    //                Wholesale = Convert.ToDecimal(RemoveSpecialCharactersForKBBWholesale(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                    //                SelectedTrimId = Convert.ToInt32(trim.Value),
                    //                SelectedTrimName = trim.Text
                    //            });
                    //        }
                    //    }
                    //    break;

                    //}

                }
            }
            catch (Exception)
            {

            }

            if (result.Any() && result.Count == 2)
            {
                decimal mileageAdjustmentForDealer = result.First().MileageAdjustment;
                decimal mileageAdjustmentForMarket = result.Last().MileageAdjustment;
                
                if (mileageAdjustmentForDealer > 0 && mileageAdjustmentForMarket < 0)
                    return Convert.ToInt32(mileageAdjustmentForDealer + mileageAdjustmentForMarket * (-1));
                else if (mileageAdjustmentForDealer < 0 && mileageAdjustmentForMarket > 0)
                    return Convert.ToInt32(mileageAdjustmentForDealer * (-1) + mileageAdjustmentForMarket);
                else
                {
                    return Convert.ToInt32(Math.Abs(mileageAdjustmentForDealer - mileageAdjustmentForMarket));
                }
            }

            return 0;
        }

        public void GetYears(DateTime vrsVersionDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetYears");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetYears(new GetYearsContract()
            {
                vrsVersionDate = vrsVersionDate.ToString("M/d/yyyy")
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            //request.CookieContainer = CookieContainer;
            //request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            Result = streamReader.ReadToEnd();
            streamReader.Close();
        }

        public void GetMakes(int yearId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetMakes");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetMakes(new GetMakesContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                yearId = yearId.ToString()
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            //request.CookieContainer = CookieContainer;
            //request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            Result = streamReader.ReadToEnd();
            streamReader.Close();
        }

        public void GetModels(int yearId, int makeId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetModels");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetModels(new GetModelsContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                yearId = yearId.ToString(),
                makeId = makeId.ToString()
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            //request.CookieContainer = CookieContainer;
            //request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            Result = streamReader.ReadToEnd();
            streamReader.Close();
        }

        public string GetTrims(int yearId, int modelId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetTrims");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetTrims(new GetTrimsContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                yearId = yearId.ToString(),
                modelId = modelId.ToString()
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            //request.CookieContainer = CookieContainer;
            //request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }

        public int ConvertToInt32(JValue obj)
        {
            return obj != null ? Convert.ToInt32(obj.Value) : 0;
        }

        public string ConvertToString(JValue obj)
        {
            return obj != null ? Convert.ToString(obj.Value) : string.Empty;
        }

        public DateTime ConvertToDateTime(JValue obj)
        {
            return obj != null ? Convert.ToDateTime(obj.Value) : DateTime.Now;
        }

        public List<ExtendedSelectListItem> CreateDataList(JToken obj, int selectedId)
        {
            var result = new List<ExtendedSelectListItem>();
            result.Add(CreateSelectListItem("Year...", "Year...", false));
            if (obj != null)
            {
                result.AddRange(obj.Children().Select(item => new ExtendedSelectListItem()
                {
                    Text = item.Value<string>("DisplayName"),
                    Value = item.Value<string>("Id"),
                    Selected = item.Value<int>("Id") == selectedId
                }));
            }

            return result;
        }
        
        public KarPowerViewModel CreateViewModelForKarPowerResult(string kbbUserName, string kbbPassword, string vin, string mileage, string trimId, string type, DealerUser dealerUser)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            
            const int savedTrimId = 0;
            var savedOptionIds = string.Empty;

            try
            {
                if (String.IsNullOrEmpty(kbbUserName) || String.IsNullOrEmpty(kbbPassword))
                    return model;

                ExecuteGetVehicleByVin(vin, valuationDate, kbbUserName, kbbPassword);
                dealerUser.KbbCookieContainer = CookieContainer;
                dealerUser.KbbCookieCollection = CookieCollection;

                if (!String.IsNullOrEmpty(GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetVehicleByVinResult);

                    model.SelectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    model.SelectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    model.SelectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    model.SelectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    model.SelectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    model.SelectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    model.SelectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    model.ValuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    model.OptionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    model.Makes = CreateDataList(jsonObj["d"]["makes"], model.SelectedMakeId);

                    // get models
                    model.Models = CreateDataList(jsonObj["d"]["models"], model.SelectedModelId);

                    // get trims
                    model.Trims = CreateDataList(jsonObj["d"]["trims"], model.SelectedTrimId);

                    // get transmissions
                    model.Transmissions = CreateDataList(jsonObj["d"]["transmissions"], model.SelectedTransmissionId);

                    // get engines
                    model.Engines = CreateDataList(jsonObj["d"]["engines"], model.SelectedEngineId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            model.SelectedTrimId = Convert.ToInt32(trimId);
            model.SelectedEngineId = 0;
            model.SelectedTransmissionId = 0;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

            try
            {
                // get Engines list
                ExecuteGetEngines(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                    model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }

                // get Transmissions list
                ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                    model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
                }

                // get Drive Trains list
                ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                    model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }

                IList<OptionContract> tempOptionalEquipmentMarkupList = new List<OptionContract>();
                ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    tempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                // loading save options in database
                if (model.SelectedTrimId == savedTrimId)
                {
                    foreach (var optionContract in model.OptionalEquipmentMarkupList)
                    {
                        optionContract.IsSelected = savedOptionIds.Contains(optionContract.Id);
                    }
                }

                ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), tempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }

                ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new ExtendedSelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }
            }
            catch (Exception)
            {

            }

            return model;
        }

        public List<OptionContract> CreateOptionalEquipmentList(string data)
        {
            if (String.IsNullOrEmpty(data)) return new List<OptionContract>();

            var result = new List<OptionContract>();
            var doc = new HtmlDocument();
            doc.LoadHtml(data);
            var tdOptions = doc.DocumentNode.SelectNodes("//table/tr/td");
            foreach (var tdOption in tdOptions)
            {
                var span = tdOption.SelectSingleNode("span");
                var input = tdOption.SelectSingleNode("span/input");
                var label = tdOption.SelectSingleNode("span/label");
                if (span == null || input == null || label == null) continue;

                result.Add(new OptionContract()
                {
                    __type = "KBB.Karpower.WebServices.LightOption",
                    Id = Convert.ToString(span.Attributes["recordnumber"].Value),
                    IsSelected = (input.Attributes["checked"] != null && Convert.ToString(input.Attributes["checked"].Value) == "checked") ? true : false,
                    DisplayName = label.InnerText
                });
            }

            return result;
        }
        
        public KarPowerViewModel CreateViewModelForUpdateValuationByOptionalEquipment(string listingId, bool isChecked, KarPowerViewModel karPowerViewModel, CookieContainer container, CookieCollection collection)
        {
            var model = new KarPowerViewModel();
            if (karPowerViewModel != null)
                model = karPowerViewModel;

            try
            {
                CookieContainer = container;
                CookieCollection = collection;
                
                var changedOption = model.OptionalEquipmentMarkupList.FirstOrDefault(i => i.Id == listingId);
                if (changedOption != null)
                {
                    changedOption.IsSelected = isChecked;
                    var existingOptionHistory = model.OptionalEquipmentHistoryList.FirstOrDefault(i => i.Id == changedOption.Id);
                    if (existingOptionHistory != null)
                        existingOptionHistory.IsSelected = Convert.ToBoolean(isChecked);
                    else
                        model.OptionalEquipmentHistoryList.Add(changedOption);
                }

                // get kar power values without options
                ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, new OptionContract[] { }, new OptionContract[] { });

                if (!String.IsNullOrEmpty(GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                    model.BaseWholesaleWithoutOptions = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                }

                ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), model.OptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }
            }
            catch (Exception)
            {

            }

            return model;
        }

        public KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId, int transmissionId, CookieContainer container, CookieCollection collection, KarPowerViewModel model)
        {
            //var model = new KarPowerViewModel();

            model.SelectedTrimId = trimId;
            model.SelectedEngineId = 0;
            model.SelectedTransmissionId = transmissionId;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

            try
            {
                CookieContainer = container;
                CookieCollection = collection;

                // get Engines list
                ExecuteGetEngines(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetEnginesResult);
                    model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }

                // get Transmissions list
                ExecuteGetTransmissions(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetTransmissionsResult);
                    model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
                }

                // get Drive Trains list
                ExecuteGetDriveTrains(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetDriveTrainsResult);
                    model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                ExecuteGetDefaultOptionalEquipmentWithUser(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }


                ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new ExtendedSelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }

            }
            catch (Exception)
            {

            }

            return model;
        }

        public Stream PrintReport(KarPowerViewModel model, string kbbUsername, string kbbPassword, CookieContainer cookieContainer, CookieCollection cookieCollection, int dealerId)
        {
            try
            {
                CookieContainer = cookieContainer;
                CookieCollection = cookieCollection;
                UserName = kbbUsername;
                Password = kbbPassword;

                var selectedMake = model.Makes.FirstOrDefault(i => i.Value == model.SelectedMakeId.ToString());
                var selectedModel = model.Models.FirstOrDefault(i => i.Value == model.SelectedModelId.ToString());
                var selectedTrim = model.Trims.FirstOrDefault(i => i.Value == model.SelectedTrimId.ToString());
                var selectedEngine = model.Engines.FirstOrDefault(i => i.Value == model.SelectedEngineId.ToString());
                var selectedTransmission = model.Transmissions.FirstOrDefault(i => i.Value == model.SelectedTransmissionId.ToString());
                var selectedDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value == model.SelectedDriveTrainId.ToString());
                var selectedReport = model.Reports.FirstOrDefault(i => i.Value == model.SelectedReport);
                var dataToSave = new SaveVrsVehicleContract()
                {
                    category = "ca090fcb-597d-482d-b5aa-f528dd7bba21",
                    certified = false,
                    drivetrain = selectedDriveTrain != null ? selectedDriveTrain.Text : String.Empty,
                    drivetrainId = selectedDriveTrain != null ? selectedDriveTrain.Value : String.Empty,
                    engine = selectedEngine != null ? selectedEngine.Text : String.Empty,
                    engineId = selectedEngine != null ? selectedEngine.Value : String.Empty,
                    exteriorColor = "Select Color or Enter Manually",
                    interiorColor = "Select Color or Enter Manually",
                    initialDate = model.ValuationDate.ToString("M/d/yyyy"),
                    inventoryEntryId = "",
                    isPreOwnedSessionVehicle = true,
                    make = selectedMake != null ? selectedMake.Text : String.Empty,
                    makeId = selectedMake != null ? selectedMake.Value : String.Empty,
                    mileage = model.SelectedMileage,
                    model = selectedModel != null ? selectedModel.Text : String.Empty,
                    modelId = selectedModel != null ? selectedModel.Value : string.Empty,
                    optionHistory = model.OptionalEquipmentHistoryList.ToArray(),
                    options = model.OptionalEquipmentMarkupList.ToArray(),
                    sellPrice = "",
                    stockNumber = "",
                    transmission = selectedTransmission != null ? selectedTransmission.Text : String.Empty,
                    transmissionId = selectedTransmission != null ? selectedTransmission.Value : String.Empty,
                    trim = selectedTrim != null ? selectedTrim.Text : string.Empty,
                    trimId = selectedTrim != null ? selectedTrim.Value : string.Empty,
                    valuationDate = model.ValuationDate.ToString("M/d/yyyy"),
                    vehicleCondition = "2",
                    vin = model.Vin,
                    webSellPrice = "",
                    year = model.SelectedYearId.ToString(),
                    yearId = model.SelectedYearId.ToString()
                };

                ExecuteSaveVrsVehicle(dataToSave);
                if (!String.IsNullOrEmpty(SaveVrsVehicleResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(SaveVrsVehicleResult);
                    model.EncodedInventoryId = ConvertToString((JValue)(jsonObj["d"]["id"]));
                    model.EncodedVin = ConvertToString((JValue)(jsonObj["d"]["vin"]));
                    model.EncodedStockNumber = ConvertToString((JValue)(jsonObj["d"]["stockNumber"]));
                    model.EncodedInventoryCategoryId = ConvertToString((JValue)(jsonObj["d"]["inventoryCategoryId"]));
                    model.CategoryName = ConvertToString((JValue)(jsonObj["d"]["categoryName"]));
                }

                ExecuteGetReportParameters(model.EncodedInventoryId, selectedReport.Value, selectedReport.Text, "", model.Vin);
                if (!String.IsNullOrEmpty(GetReportParametersResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(GetReportParametersResult);
                    var hfReportParams = ConvertToString((JValue)(jsonObj["d"]));
                    return ExecuteGetPreOwnedVehicleReport(hfReportParams);
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public bool IsAuthorized(string userName, string password)
        {
            var postData = String.Format("Login1$hfCloseExistingSessions=close&Login1$TextBoxUserName={0}&Login1$TextBoxPassword={1}&Login1$ButtonLogin={2}&__VIEWSTATE={3}&Login1$MachineName=LV-IDC-KPOWEB1&__EVENTVALIDATION={4}", userName, password, "Login", ViewState, EventValidation);

            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create(SubmitLogInUrl);
            request.Method = "POST";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = UserAgent;
            request.ContentLength = postData.Length;
            request.ContentType = ContentType;
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Have some cookies.
            return response.Headers["Set-Cookie"] != null && response.Headers["Set-Cookie"].Contains("KBBASPNETAUTH");
        }

        #endregion

        #region Private Methods

        private void WebRequestGet()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(LogInUrl);
                request.UserAgent = UserAgent;
                request.CookieContainer = CookieContainer;
                request.CookieContainer.Add(CookieCollection);
                //Get the response from the server and save the cookies from the first request..
                var response = (HttpWebResponse)request.GetResponse();
                CookieCollection = response.Cookies;
                CookieContainer.SetCookies(new Uri("http://www.karpower.com"), response.Headers["Set-Cookie"]);

                var streamReader = new StreamReader(response.GetResponseStream());
                var result = streamReader.ReadToEnd();
                streamReader.Close();
                ViewState = ExtractViewState(result);
                //NOTE: 08/23/2013 Karpower does not need event validation anymore
                //EventValidation = ExtractEventValidation(result);
            }
            catch (Exception)
            {
                
            }
        }

        private void WebRequestPost()
        {
            //PostData = String.Format("Login1$hfCloseExistingSessions=&__LASTFOCUS=&__EVENTTARGET=&__EVENTARGUMENT=&Login1$task=None&Login1$TextBoxUserName={0}&Login1$TextBoxPassword={1}&Login1$ButtonLogin={2}&Login1$MachineName=LV-IDC-KPOWEB1&__VIEWSTATE=/wEPDwUKMTU2Nzc2MDYwMg9kFgICAw9kFgICAQ9kFgICAw8PZBYCHgV2YWx1ZWVkZJif0c+VnIvQNBFt67oFpUaVB3cD&__EVENTVALIDATION=/wEWCAKd2rK4BgKBr7j0DgLbzaLdCQLywPaTDALRsvvVCgKE9u+FDALr/fGIDAL6lLnxD4185FBom6IjjyzzvS+QRxMGg6J+", UserName, Password, "Login");
            var postData = String.Format("Login1$hfCloseExistingSessions=close&Login1$TextBoxUserName={0}&Login1$TextBoxPassword={1}&Login1$ButtonLogin={2}&__VIEWSTATE={3}&Login1$MachineName=LV-IDC-KPOWEB1&__EVENTVALIDATION={4}", UserName, Password, "Login", ViewState, EventValidation);

            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create(SubmitLogInUrl);
            request.Method = "POST";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = UserAgent;
            request.ContentLength = postData.Length;
            request.ContentType = ContentType;
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Have some cookies.
            CookieCollection = response.Cookies;
            CookieContainer.SetCookies(new Uri("http://www.karpower.com"), response.Headers["Set-Cookie"]);

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            //var result = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private string GetVehicleByVin(string vin, DateTime valuationDate)
        {
            // Setup the http request.
            //var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx?wsdl");
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetVehicleByVin");
            //var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetModels");
            //request.Headers.Add("SOAPAction", "http://kpo.kbb.com/GetModels");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";//"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = UserAgent;
            var postData = CreateDataToGetVehicleByVin(new GetVehicleByVinContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vin = vin
            });
            //PostData = CreateDataToGetYears(new GetYearsContract()
            //                                    {
            //                                        vrsVersionDate = date.ToString("MM/dd/yyyy")
            //                                    });
            //PostData = CreateDataToGetModels(new GetModelsContract()
            //                                     {
            //                                         valuationDate = date.ToString("M/d/yyyy"),
            //                                         yearId = "2012",
            //                                         makeId = "5"
            //                                     });

            request.ContentLength = postData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;
            //var credentialCache = new CredentialCache
            //                          {
            //                              {
            //                                  new Uri("http://www.karpower.com"), "Basic",
            //                                  new NetworkCredential("paul@newportcoastauto.com", "vagina")
            //                                  }
            //                          };
            //request.Credentials = credentialCache;
            //new NetworkCredential("paul@newportcoastauto.com", "vagina");

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var getVehicleByVinResult = streamReader.ReadToEnd();
            GetVehicleByVinResult = getVehicleByVinResult;
            streamReader.Close();
            return getVehicleByVinResult;
        }

        private void GetPartiallyDecodedTrainsWithUser(string vin, DateTime valuationDate, int vehicleId, int engineId, int transId)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetPartiallyDecodedTrainsWithUser");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetPartiallyDecodedTrainsWithUser(new GetPartiallyDecodedTrainsWithUserContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vin = vin,
                vehicleId = vehicleId.ToString(),
                engineId = engineId.ToString(),
                transId = transId.ToString()
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            GetPartiallyDecodedTrainsWithUserResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private void GetPartiallyDecodedTransmissionsWithUser(string vin, DateTime valuationDate, int vehicleId, int engineId)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetPartiallyDecodedTransmissionsWithUser");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetPartiallyDecodedTrainsWithUser(new GetPartiallyDecodedTrainsWithUserContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vin = vin,
                vehicleId = vehicleId.ToString(),
                engineId = engineId.ToString(),
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            GetPartiallyDecodedTransmissionsWithUserResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private string GetValuation(int vrsVehicleId, DateTime valuationDate, int mileage, int vehicleCondition, int inventoryEntryId, OptionContract[] optionHistory, OptionContract[] currentOptionsList)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetValuation");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            var postData = CreateDataToGetValuation(new GetValuationContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vrsVehicleId = vrsVehicleId.ToString(),
                mileage = mileage.ToString(),
                vehicleCondition = vehicleCondition.ToString(),
                inventoryEntryId = inventoryEntryId == 0 ? "" : inventoryEntryId.ToString(),
                optionHistory = optionHistory,
                currentOptionsList = currentOptionsList
            });

            request.ContentLength = postData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var getValuationResult = streamReader.ReadToEnd();
            GetValuationResult = getValuationResult;
            streamReader.Close();
            return getValuationResult;
        }

        private void GetOptionalEquipmentOptionChangedWithUser(int vrsVehicleId, DateTime valuationDate, OptionContract changedOption, OptionContract[] currentOptionsList)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/OptionalEquipmentOptionChangedWithUser");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetOptionalEquipmentOptionChangedWithUser(new GetOptionalEquipmentOptionChangedWithUserContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vrsVehicleId = vrsVehicleId.ToString(),
                changedOption = changedOption,
                currentOptions = currentOptionsList
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            GetOptionalEquipmentOptionChangedWithUserResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private IList<OptionContract> AddEngineTransmissionDriveTrainAsOptions(IList<OptionContract> list, int engineId, int transmissionId, int driveTrainId)
        {
            var result = list;
            result.Add(new OptionContract()
            {
                __type = "KBB.Karpower.WebServices.LightOption",
                Id = engineId.ToString(),
                IsSelected = true,
                DisplayName = string.Empty
            });

            result.Add(new OptionContract()
            {
                __type = "KBB.Karpower.WebServices.LightOption",
                Id = transmissionId.ToString(),
                IsSelected = true,
                DisplayName = string.Empty
            });

            result.Add(new OptionContract()
            {
                __type = "KBB.Karpower.WebServices.LightOption",
                Id = driveTrainId.ToString(),
                IsSelected = true,
                DisplayName = string.Empty
            });

            return result;
        }


        private string GetEngines(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetEngines");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            var postData = CreateDataToGetEngines(new GetEnginesContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vehicleId = vehicleId.ToString()
            });

            request.ContentLength = postData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var getEnginesResult = streamReader.ReadToEnd();
            GetEnginesResult = getEnginesResult;
            streamReader.Close();
            return getEnginesResult;
        }

        private string GetTransmissions(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetTransmissions");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            var postData = CreateDataToGetEngines(new GetEnginesContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vehicleId = vehicleId.ToString()
            });

            request.ContentLength = postData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var getTransmissionsResult = streamReader.ReadToEnd();
            GetTransmissionsResult = getTransmissionsResult;
            streamReader.Close();
            return getTransmissionsResult;
        }

        private string GetDriveTrains(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetDriveTrains");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            var postData = CreateDataToGetEngines(new GetEnginesContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vehicleId = vehicleId.ToString()
            });

            request.ContentLength = postData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var getDriveTrainsResult = streamReader.ReadToEnd();
            GetDriveTrainsResult = getDriveTrainsResult;
            streamReader.Close();
            return getDriveTrainsResult;
        }

        private void GetListCustomerReports(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/ListCustomerReports");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetEngines(new GetEnginesContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vehicleId = vehicleId.ToString()
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            GetListCustomerReportsResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private string GetDefaultOptionalEquipmentWithUser(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetDefaultOptionalEquipmentWithUser");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            var postData = CreateDataToGetDefaultOptionalEquipmentWithUser(new GetDefaultOptionalEquipmentWithUserContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vrsVehicleId = vehicleId.ToString()
            });

            request.ContentLength = postData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(postData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var getDefaultOptionalEquipmentWithUserResult = streamReader.ReadToEnd();
            GetDefaultOptionalEquipmentWithUserResult = getDefaultOptionalEquipmentWithUserResult;
            streamReader.Close();
            return getDefaultOptionalEquipmentWithUserResult;
        }

        private void SaveVrsVehicle(SaveVrsVehicleContract contract)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/SaveVrsVehicle");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToSaveVrsVehicle(contract);

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = "	http://www.karpower.com/Inventory/Inventory.aspx"; //RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            SaveVrsVehicleResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private void GetReportParameters(string inventoryEntryId, string reportId, string reportName, string sessionStockNum, string sessionVin)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetReportParameters");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetReportParameters(new GetReportParametersContract()
            {
                inventoryEntryID = inventoryEntryId,
                reportID = reportId,
                reportName = reportName,
                sessionStockNum = sessionStockNum,
                sessionVIN = sessionVin
            });

            request.ContentLength = PostData.Length;
            //Request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            GetReportParametersResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private Stream GetPreOwnedVehicleReport(string hfReportParams)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/Reports/Vehicle/PreOwnedVehicleReport.aspx");
            request.Method = "POST";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = UserAgent;
            PostData = String.Format("hfReportParams={0}", hfReportParams);

            request.ContentLength = PostData.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json; charset=utf-8";
            request.Referer = RefererUrl;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();
            response.Headers.Set("content-disposition", "attachment");
            // Read the response
            //var streamReader = new StreamReader(response.GetResponseStream());
            //GetPreOwnedVehicleReportResult = streamReader.ReadToEnd();
            //streamReader.Close();
            return response.GetResponseStream();
        }

        private string ExtractViewState(string s)
        {
            string viewStateNameDelimiter = "__VIEWSTATE";
            string valueDelimiter = "value=\"";

            int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
            int viewStateValuePosition = s.IndexOf(valueDelimiter, viewStateNamePosition);

            int viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
            int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

            return HttpUtility.UrlEncodeUnicode(s.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition));
        }

        private string ExtractEventValidation(string s)
        {
            string eventValidationNameDelimiter = "__EVENTVALIDATION";
            string valueDelimiter = "value=\"";

            int eventValidationNamePosition = s.IndexOf(eventValidationNameDelimiter);
            int eventValidationValuePosition = s.IndexOf(valueDelimiter, eventValidationNamePosition);

            int eventValidationStartPosition = eventValidationValuePosition + valueDelimiter.Length;
            int eventValidationEndPosition = s.IndexOf("\"", eventValidationStartPosition);

            return HttpUtility.UrlEncodeUnicode(s.Substring(eventValidationStartPosition, eventValidationEndPosition - eventValidationStartPosition));
        }

        private string CreateDataToGetVehicleByVin(GetVehicleByVinContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetYears(GetYearsContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetMakes(GetMakesContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetModels(GetModelsContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetTrims(GetTrimsContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetEngines(GetEnginesContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetPartiallyDecodedTrainsWithUser(GetPartiallyDecodedTrainsWithUserContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetValuation(GetValuationContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetDefaultOptionalEquipmentWithUser(GetDefaultOptionalEquipmentWithUserContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetOptionalEquipmentOptionChangedWithUser(GetOptionalEquipmentOptionChangedWithUserContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToSaveVrsVehicle(SaveVrsVehicleContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private string CreateDataToGetReportParameters(GetReportParametersContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }

        private bool IsValidItem(string itemValue)
        {
            return (itemValue != "0" && itemValue != "Year...");
        }

        private static decimal ParseDecimal(string jsonObj)
        {
            decimal result = 0;
            return decimal.TryParse(jsonObj, out result) ? result : 0;
        }

        private void AddSimpleKbbReportFromKarPower(int vehicleId, List<SmallKarPowerViewModel> karpowerResult, string vin, short type)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (var tmp in karpowerResult)
                {

                    var e = new KellyBlueBook()
                    {
                        Trim = tmp.SelectedTrimName,
                        BaseWholeSale = tmp.BaseWholesale,
                        MileageAdjustment = tmp.MileageAdjustment,
                        WholeSale = tmp.Wholesale,
                        ModelId = tmp.SelectedModelId,
                        TrimId = tmp.SelectedTrimId,
                        DateStamp = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Expiration = GetNextFriday(),
                        Vin = vin,
                        VehicleStatusCodeId = type,
                        
                    };
                    if (vehicleId > 0)
                        e.VehicleId = vehicleId;
                    //Add to memory
                    context.AddToKellyBlueBooks(e);
                }
                
                if (karpowerResult.Count == 1)
                {
                    var searchVehicleResult = context.Vehicles.FirstOrDefault(x => x.VehicleId == vehicleId);

                    if (searchVehicleResult != null)
                    {
                        searchVehicleResult.KBBTrimId = karpowerResult.First().SelectedTrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        private void AddSimpleKbbReportFromKarPower(List<SmallKarPowerViewModel> karpowerResult, string vin, short type)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (var tmp in karpowerResult)
                {

                    var e = new KellyBlueBook()
                    {
                        Trim = tmp.SelectedTrimName,
                        BaseWholeSale = tmp.BaseWholesale,
                        MileageAdjustment = tmp.MileageAdjustment,
                        WholeSale = tmp.Wholesale,
                        ModelId = tmp.SelectedModelId,
                        TrimId = tmp.SelectedTrimId,
                        DateStamp = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Expiration = GetNextFriday(),
                        Vin = vin,
                        VehicleStatusCodeId = type,
                        VehicleId = (int?)null
                    };

                    //Add to memory
                    context.AddToKellyBlueBooks(e);
                }
                
                if (karpowerResult.Count == 1)
                {
                    var searchVehicleResult = context.Vehicles.FirstOrDefault(x => x.Vin.Equals(vin));

                    if (searchVehicleResult != null)
                    {
                        searchVehicleResult.KBBTrimId = karpowerResult.First().SelectedTrimId;
                    }
                }

                context.SaveChanges();
            }
        }

        private void AddSimpleKbbReportFromKarPowerToVinsell(int vehicleId, List<SmallKarPowerViewModel> karpowerResult, string vin)
        {
            using (var context = new VinsellEntities())
            {
                foreach (var tmp in karpowerResult)
                {

                    var e = new manheim_KellyBlueBook()
                    {
                        Trim = tmp.SelectedTrimName,
                        BaseWholeSale = tmp.BaseWholesale,
                        MileageAdjustment = tmp.MileageAdjustment,
                        WholeSale = tmp.Wholesale,
                        ModelId = tmp.SelectedModelId,
                        TrimId = tmp.SelectedTrimId,
                        DateStamp = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Expiration = GetNextFriday(),
                        Vin = vin,
                       
                    };
                    if (vehicleId > 0)
                        e.VehicleId = vehicleId;
                    //Add to memory
                    context.AddTomanheim_KellyBlueBook(e);
                }
                context.SaveChanges();
            
            }
        }

        private ExtendedSelectListItem CreateSelectListItem(string text, string value, bool selected)
        {
            var item = new ExtendedSelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selected
            };

            return item;
        }

        private string RemoveSpecialCharactersForKBBWholesale(string input)
        {
            input = input.Replace("&mdash;", "");
            input = input.Replace("N/A", "");
            if (!String.IsNullOrEmpty(input))
            {
                if (input.Contains(".")) input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
            } return "0";
        }

        private DateTime GetNextFriday()
        {
            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            for (int i = 1; i < 8; i++)
            {
                DateTime dt = dtNow.AddDays(i);
                if (dt.DayOfWeek.Equals(DayOfWeek.Friday))
                    return dt;
            }
            return dtNow;

        }
        #endregion
    }
}
