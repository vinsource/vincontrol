using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Data.Model;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Models;
using Vincontrol.Web.DatabaseModel;
using vincontrol.Helper;

namespace Vincontrol.Web.HelperClass
{
    public class KarPowerServiceBAK
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

        public KarPowerServiceBAK()
        {
            UserName = DefaultUserName;
            Password = DefaultPassword;
            CookieContainer = new CookieContainer();
            CookieCollection = new CookieCollection();
        }

        #region Public Methods

        //public void Execute()
        //{
        //    WebRequestGet();
        //    WebRequestPost();
            
        //    GetVehicleByVin("1C3CCBAB4CN101389", DateTime.Now);
        //}

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

        public void ExecuteGetValuation(int vrsVehicleId, DateTime valuationDate, int mileage, int vehicleCondition, int inventoryEntryId, OptionContract[] optionHistory, OptionContract[] currentOptionsList)
        {
            try
            {
                GetValuation(vrsVehicleId, valuationDate, mileage, vehicleCondition, inventoryEntryId, optionHistory, currentOptionsList);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetValuation(vrsVehicleId, valuationDate, mileage, vehicleCondition, inventoryEntryId, optionHistory, currentOptionsList);
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

        public void ExecuteGetEngines(int vehicleId, DateTime valuationDate)
        {
            try
            {
                GetEngines(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                
            }
            
        }

        public void ExecuteGetTransmissions(int vehicleId, DateTime valuationDate)
        {
            try
            {
                GetTransmissions(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                
            }
            
        }

        public void ExecuteGetDriveTrains(int vehicleId, DateTime valuationDate)
        {
            try
            {
                GetDriveTrains(vehicleId, valuationDate);
            }
            catch (Exception)
            {

            }
            
        }

        public void ExecuteGetDefaultOptionalEquipmentWithUser(int vehicleId, DateTime valuationDate)
        {
            try
            {
                GetDefaultOptionalEquipmentWithUser(vehicleId, valuationDate);
            }
            catch (Exception)
            {
                // log into the karpower again
                WebRequestGet();
                WebRequestPost();

                // try to execute this method again
                GetDefaultOptionalEquipmentWithUser(vehicleId, valuationDate);
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
                        var firstTmp = context.KellyBlueBooks.FirstOrDefault(o => o.Vin.Equals(vin));
                        var dt = DateTime.Parse(firstTmp.Expiration.ToString());

                        if (dt.Date > DateTime.Now.Date)
                        {
                            var list = context.KellyBlueBooks.Where(x => x.Vin == vin);

                            foreach (var kbb in list)
                            {
                                var karpowerValue = new SmallKarPowerViewModel()
                                {
                                    BaseWholesale = kbb.BaseWholeSale.GetValueOrDefault(),
                                    MileageAdjustment = kbb.MileageAdjustment.GetValueOrDefault(),
                                    Wholesale = kbb.WholeSale.GetValueOrDefault(),
                                    SelectedTrimName = kbb.Trim,
                                    SelectedTrimId = kbb.TrimId.GetValueOrDefault()
                                };

                                result.Add(karpowerValue);
                            }
                        }
                        else
                        {
                            var searchResult = context.KellyBlueBooks.Where(x => x.Vin == vin).ToList();

                            foreach (var tmp in searchResult)
                            {
                                context.Attach(tmp);
                                context.DeleteObject(tmp);
                            }

                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            
            return result;
        }

        public List<SmallKarPowerViewModel> Execute(string vin, string mileage, DateTime valuationDate, string userName, string password, short type)
        {
            var result = new List<SmallKarPowerViewModel>();
            //var result = GetKarPowerFromDatabase(vin);

            //if(result.Any())
            //{
            //    return result;
            //}

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

                    //var selectedYearId = ConvertToInt32((JValue) (jsonObj["d"]["yearId"]));
                    //var selectedMakeId = ConvertToInt32(((JValue) (jsonObj["d"]["makeId"])));
                    //var selectedModelId = ConvertToInt32(((JValue) (jsonObj["d"]["modelId"])));
                    var selectedTrimId = ConvertToInt32(((JValue) (jsonObj["d"]["trimId"])));
                    //var selectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    //var selectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    //var selectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    //var valuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    //var optionalEquipmentMarkup = ConvertToString((JValue) (jsonObj["d"]["optionalEquipmentMarkup"]));

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
                    foreach (var trim in trims)
                    {
                        if (trim.Value == "0" || trim.Value == "Year...") continue;

                        // get Engines list
                        ExecuteGetEngines(Convert.ToInt32(trim.Value), valuationDate);
                        if (!String.IsNullOrEmpty(GetEnginesResult))
                        {
                            jsonObj = (JObject) JsonConvert.DeserializeObject(GetEnginesResult);
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
                            jsonObj = (JObject) JsonConvert.DeserializeObject(GetTransmissionsResult);
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
                            jsonObj = (JObject) JsonConvert.DeserializeObject(GetDriveTrainsResult);
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
                            jsonObj = (JObject) JsonConvert.DeserializeObject(GetDefaultOptionalEquipmentWithUserResult);
                            var optionalEquipmentMarkupList =
                                CreateOptionalEquipmentList(ConvertToString((JValue) (jsonObj["d"])));

                            ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(mileage) ? 0 : Convert.ToInt32(mileage), 2,
                                                0, new OptionContract[] {}, optionalEquipmentMarkupList.ToArray());

                            if (!String.IsNullOrEmpty(GetValuationResult))
                            {
                                jsonObj = (JObject) JsonConvert.DeserializeObject(GetValuationResult);
                                result.Add(new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = ParseDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                        MileageAdjustment = ParseDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value))),
                                        Wholesale = ParseDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                        SelectedTrimId = Convert.ToInt32(trim.Value),
                                        SelectedTrimName = trim.Text,
                                        IsSelected = true
                                    });
                            }
                        }

                    }

                    SQLHelper.AddSimpleKbbReportFromKarPower(result, vin, type);
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }

        private static decimal ParseDecimal(string jsonObj)
        {
            decimal result = 0;
            return decimal.TryParse(jsonObj,out result)?result:0 ;
        }

        public List<SmallKarPowerViewModel> Execute(string vin, string mileage, int saveTrimId, string saveOptionIds, DateTime valuationDate, string userName, string password, short type)
        {
            var result = new List<SmallKarPowerViewModel>();
            //var result = GetKarPowerFromDatabase(vin);

            //if (result.Any())
            //{
            //    return result;
            //}

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
                    //var selectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    //var selectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
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
                    foreach (var trim in trims)
                    {
                        if (trim.Value == "0" || trim.Value == "Year...") continue;

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
                            var optionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                            // loading save options in database
                            if (trim.Value == saveTrimId.ToString())
                            {
                                foreach (var optionContract in optionalEquipmentMarkupList)
                                {
                                    optionContract.IsSelected = saveOptionIds.Contains(optionContract.Id);
                                }
                            }

                            ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(mileage) ? 0 : Convert.ToInt32(mileage), 2,
                                                0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                            if (!String.IsNullOrEmpty(GetValuationResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                                result.Add(new SmallKarPowerViewModel()
                                {
                                    BaseWholesale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),
                                    MileageAdjustment = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value))),
                                    Wholesale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                    SelectedTrimId = Convert.ToInt32(trim.Value),
                                    SelectedTrimName = trim.Text,
                                    IsSelected = saveTrimId == Convert.ToInt32(trim.Value)
                                });
                            }
                        }

                    }

                    SQLHelper.AddSimpleKbbReportFromKarPower(result, vin, type);
                }
            }
            catch (Exception)
            {

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
                    foreach (var trim in trims)
                    {
                        if (trim.Value == "0" || trim.Value == "Year...") continue;

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

                                var newSmallKarPowerViewModel = new SmallKarPowerViewModel()
                                {

                                    BaseWholesale =
                                        Convert.ToDecimal(
                                            CommonHelper.RemoveSpecialCharactersForMsrp(
                                                Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),

                                    Wholesale =
                                        Convert.ToDecimal(
                                            CommonHelper.RemoveSpecialCharactersForMsrp(
                                                Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                    SelectedTrimId = Convert.ToInt32(trim.Value),
                                    SelectedTrimName = trim.Text
                                };
                                var mileageAdjValue = Convert.ToString(
                                    ((JValue) (jsonObj["d"]["wholesaleMileageAdjusted"])).Value);

                                if (mileageAdjValue.Contains("(") && mileageAdjValue.Contains(")"))
                                {
                                    newSmallKarPowerViewModel.MileageAdjustment =(-1) *Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue) (jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                }
                                else
                                {
                                    newSmallKarPowerViewModel.MileageAdjustment = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                }

                                result.Add(newSmallKarPowerViewModel);
                            }
                        }

                        break;
                    }

                    foreach (var trim in trims)
                    {
                        if (trim.Value == "0" || trim.Value == "Year...") continue;

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

                            ExecuteGetValuation(Convert.ToInt32(trim.Value), valuationDate, String.IsNullOrEmpty(marketCarMileage) ? 0 : Convert.ToInt32(marketCarMileage), 2,
                                                0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());

                            if (!String.IsNullOrEmpty(GetValuationResult))
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(GetValuationResult);
                                var newSmallKarPowerViewModel = new SmallKarPowerViewModel()
                                {

                                    BaseWholesale =
                                        Convert.ToDecimal(
                                            CommonHelper.RemoveSpecialCharactersForMsrp(
                                                Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value))),

                                    Wholesale =
                                        Convert.ToDecimal(
                                            CommonHelper.RemoveSpecialCharactersForMsrp(
                                                Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value))),
                                    SelectedTrimId = Convert.ToInt32(trim.Value),
                                    SelectedTrimName = trim.Text
                                };
                                var mileageAdjValue = Convert.ToString(
                                    ((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);

                                if (mileageAdjValue.Contains("(") && mileageAdjValue.Contains(")"))
                                {
                                    newSmallKarPowerViewModel.MileageAdjustment = (-1) * Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                }
                                else
                                {
                                    newSmallKarPowerViewModel.MileageAdjustment = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value)));
                                }

                                result.Add(newSmallKarPowerViewModel);
                            }
                        }
                        break;

                    }

                 
                    

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

        public void GetTrims(int yearId, int modelId, DateTime valuationDate)
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
            Result = streamReader.ReadToEnd();
            streamReader.Close();
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

        public List<SelectListItem> CreateDataList(JToken obj, int selectedId)
        {
            var result = new List<SelectListItem>();
            result.Add(SelectListHelper.CreateSelectListItem("Year...", "Year...", false));
            if (obj != null)
            {
                result.AddRange(obj.Children().Select(item => new SelectListItem()
                {
                    Text = item.Value<string>("DisplayName"),
                    Value = item.Value<string>("Id"),
                    Selected = item.Value<int>("Id") == selectedId
                }));
            }

            return result;
        }

        public List<OptionContract> CreateOptionalEquipmentList(string data)
        {
            var result = new List<OptionContract>();
            try
            {
                //var doc = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
                //doc.LoadXml(model.OptionalEquipmentMarkup);
                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                var tdOptions = doc.DocumentNode.SelectNodes("//table/tr/td");
                foreach (var tdOption in tdOptions)
                {
                    var span = tdOption.SelectSingleNode("span");
                    var input = tdOption.SelectSingleNode("span/input");
                    var label = tdOption.SelectSingleNode("span/label");
                    if (span == null || input == null || label == null) continue;

                    //if (model.OptionalEquipmentMarkupList != null && !model.OptionalEquipmentMarkupList.Any(i => i.Id == Convert.ToString(span.Attributes["recordnumber"])))
                    //{
                    result.Add(new OptionContract()
                    {
                        __type = "KBB.Karpower.WebServices.LightOption",
                        Id = Convert.ToString(span.Attributes["recordnumber"].Value),
                        IsSelected = (input.Attributes["checked"] != null && Convert.ToString(input.Attributes["checked"].Value) == "checked") ? true : false,
                        DisplayName = label.InnerText
                    });
                    //}
                }
            }
            catch (Exception)
            {
                
            }

            return result;
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
            PostData = String.Format("Login1$hfCloseExistingSessions=close&Login1$TextBoxUserName={0}&Login1$TextBoxPassword={1}&Login1$ButtonLogin={2}&__VIEWSTATE={3}&Login1$MachineName=LV-IDC-KPOWEB1&__EVENTVALIDATION={4}", UserName, Password, "Login", ViewState, EventValidation);

            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create(SubmitLogInUrl);
            request.Method = "POST";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = UserAgent;
            request.ContentLength = PostData.Length;
            request.ContentType = ContentType;
            request.Referer = RefererUrl;

            //var credentialCache = new CredentialCache
            //                          {
            //                              {
            //                                  new Uri("http://www.karpower.com"), "Basic",
            //                                  new NetworkCredential("paul@newportcoastauto.com", "vagina")
            //                                  }
            //                          };
            //Request.Credentials = credentialCache;

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Have some cookies.
            CookieCollection = response.Cookies;
            CookieContainer.SetCookies(new Uri("http://www.karpower.com"), response.Headers["Set-Cookie"]);

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var result = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private void GetVehicleByVin(string vin, DateTime valuationDate)
        {
            // Setup the http request.
            //var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx?wsdl");
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetVehicleByVin");
            //var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetModels");
            //request.Headers.Add("SOAPAction", "http://kpo.kbb.com/GetModels");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";//"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetVehicleByVin(new GetVehicleByVinContract()
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

            request.ContentLength = PostData.Length;
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
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            GetVehicleByVinResult = streamReader.ReadToEnd();
            streamReader.Close();
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

        private void GetValuation(int vrsVehicleId, DateTime valuationDate, int mileage, int vehicleCondition, int inventoryEntryId, OptionContract[] optionHistory, OptionContract[] currentOptionsList)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetValuation");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetValuation(new GetValuationContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vrsVehicleId = vrsVehicleId.ToString(),
                mileage = mileage.ToString(),
                vehicleCondition = vehicleCondition.ToString(),
                inventoryEntryId = inventoryEntryId == 0 ? "" : inventoryEntryId.ToString(),
                optionHistory = optionHistory,
                currentOptionsList = currentOptionsList
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
            GetValuationResult = streamReader.ReadToEnd();
            streamReader.Close();
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

        private void GetEngines(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetEngines");
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
            GetEnginesResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private void GetTransmissions(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetTransmissions");
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
            GetTransmissionsResult = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private void GetDriveTrains(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetDriveTrains");
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
            GetDriveTrainsResult = streamReader.ReadToEnd();
            streamReader.Close();
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

        private void GetDefaultOptionalEquipmentWithUser(int vehicleId, DateTime valuationDate)
        {
            // Setup the http request.
            var request = (HttpWebRequest)WebRequest.Create("http://www.karpower.com/WebServices/VehicleWebService.asmx/GetDefaultOptionalEquipmentWithUser");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;
            PostData = CreateDataToGetDefaultOptionalEquipmentWithUser(new GetDefaultOptionalEquipmentWithUserContract()
            {
                valuationDate = valuationDate.ToString("M/d/yyyy"),
                vrsVehicleId = vehicleId.ToString()
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
            GetDefaultOptionalEquipmentWithUserResult = streamReader.ReadToEnd();
            streamReader.Close();
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

        #endregion
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