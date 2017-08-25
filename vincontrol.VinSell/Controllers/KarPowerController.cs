using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.KBB;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    public class KarPowerController : BaseController
    {
        private IKBBService _kbbService;

        public KarPowerController()
        {
            
            _kbbService = new KBBService();
            
        }

        public ActionResult GetSingleKarPowerSummary(int listingId, string trimId, string modelId)
        {
            if (SessionHandler.User == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new VinsellEntities())
            {
                var hasVin = true;
                int receivedListingId = listingId;
                var row = context.manheim_vehicles.FirstOrDefault(x => x.Id == receivedListingId);

                ViewData["VIN"] = row.Vin;
                ViewData["MILEAGE"] = row.Mileage.GetValueOrDefault();
                ViewData["MODELID"] = modelId;
                ViewData["TRIMID"] = trimId;
                ViewData["TYPE"] = "Inventory";
                ViewData["HASVIN"] = hasVin;
      
            }

            return View("SingleKarPowerSummary");
        }

        public ActionResult UpdateValuationByOptionalEquipmentInSingleMode(string listingId, string isChecked, string trimId)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;

            Session["KarPowerViewModel"] = model;
            return PartialView("SingleKarPowerSummaryDetail", model);
        }

        public ActionResult UpdateValuationByChangingTrimAndTransmission(int trimId, int transmissionId)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(Convert.ToInt32(trimId), Convert.ToInt32(transmissionId));
            model.IsMultipleTrims = false;

            Session["KarPowerViewModel"] = model;
            return PartialView("SingleKarPowerSummaryDetail", model);
        }

        [HttpPost]
        public void SaveKarPowerOptions(string vin, string trimId, string selectedOptionIds, string transmissionId, string driveTrainId, string type)
        {
            LinqHelper.SaveKarPowerOptions(vin, trimId, selectedOptionIds, transmissionId, driveTrainId, type, SessionHandler.User.DealerId);
        }


        [HttpPost]
        public ActionResult PrintReport(KarPowerViewModel submittedModel)
        {
            var model = new KarPowerViewModel();
            if (SessionHandler.KarPowerViewModel != null)
                model = SessionHandler.KarPowerViewModel;
            try
            {
                var dealer = SessionHandler.User;
                var karPowerServiceWrapper = new KBBService()
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"],
                    UserName = dealer.Setting.KellyBlueBook,
                    Password = dealer.Setting.KellyPassword
                };

                var selectedMake = model.Makes.FirstOrDefault(i => i.Value == model.SelectedMakeId.ToString());
                var selectedModel = model.Models.FirstOrDefault(i => i.Value == model.SelectedModelId.ToString());
                var selectedTrim = model.Trims.FirstOrDefault(i => i.Value == model.SelectedTrimId.ToString());
                var selectedEngine = model.Engines.FirstOrDefault(i => i.Value == model.SelectedEngineId.ToString());
                var selectedTransmission = model.Transmissions.FirstOrDefault(i => i.Value == model.SelectedTransmissionId.ToString());
                var selectedDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value == model.SelectedDriveTrainId.ToString());
                var selectedReport = model.Reports.FirstOrDefault(i => i.Value == submittedModel.SelectedReport);
                var dataToSave = new SaveVrsVehicleContract()
                {
                    category = "ca090fcb-597d-482d-b5aa-f528dd7bba21", // Appraisal
                    certified = false,
                    drivetrain = selectedDriveTrain != null ? selectedDriveTrain.Text : "",
                    drivetrainId = selectedDriveTrain != null ? selectedDriveTrain.Value : "",
                    engine = selectedEngine != null ? selectedEngine.Text : "",
                    engineId = selectedEngine != null ? selectedEngine.Value : "",
                    exteriorColor = "Select Color or Enter Manually",
                    interiorColor = "Select Color or Enter Manually",
                    initialDate = model.ValuationDate.ToString("M/d/yyyy"),
                    inventoryEntryId = "",
                    isPreOwnedSessionVehicle = true,
                    make = selectedMake != null ? selectedMake.Text : "",
                    makeId = selectedMake != null ? selectedMake.Value : "",
                    mileage = model.SelectedMileage,
                    model = selectedModel != null ? selectedModel.Text : "",
                    modelId = selectedModel != null ? selectedModel.Value : "",
                    optionHistory = model.OptionalEquipmentHistoryList.ToArray(),
                    options = model.OptionalEquipmentMarkupList.ToArray(),
                    sellPrice = "",
                    stockNumber = "",
                    transmission = selectedTransmission != null ? selectedTransmission.Text : "",
                    transmissionId = selectedTransmission != null ? selectedTransmission.Value : "",
                    trim = selectedTrim != null ? selectedTrim.Text : "",
                    trimId = selectedTrim != null ? selectedTrim.Value : "",
                    valuationDate = model.ValuationDate.ToString("M/d/yyyy"),
                    vehicleCondition = "2",
                    vin = model.Vin,
                    webSellPrice = "",
                    year = model.SelectedYearId.ToString(),
                    yearId = model.SelectedYearId.ToString()
                };

                karPowerServiceWrapper.ExecuteSaveVrsVehicle(dataToSave);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.SaveVrsVehicleResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.SaveVrsVehicleResult);
                    model.EncodedInventoryId = ConvertToString((JValue)(jsonObj["d"]["id"]));
                    model.EncodedVin = ConvertToString((JValue)(jsonObj["d"]["vin"]));
                    model.EncodedStockNumber = ConvertToString((JValue)(jsonObj["d"]["stockNumber"]));
                    model.EncodedInventoryCategoryId = ConvertToString((JValue)(jsonObj["d"]["inventoryCategoryId"]));
                    model.CategoryName = ConvertToString((JValue)(jsonObj["d"]["categoryName"]));
                }

                // Save trim & options on karpower
                try
                {
                    SaveKarPowerOptions(model.Vin, selectedTrim.Value, submittedModel.SelectedOptionIds, selectedTransmission.Value, selectedDriveTrain.Value, submittedModel.Type);
                }
                catch (Exception)
                {

                }

                karPowerServiceWrapper.ExecuteGetReportParameters(model.EncodedInventoryId, selectedReport.Value, selectedReport.Text, "", model.Vin);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetReportParametersResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetReportParametersResult);
                    var hfReportParams = ConvertToString((JValue)(jsonObj["d"]));

                    return File(karPowerServiceWrapper.ExecuteGetPreOwnedVehicleReport(hfReportParams), "application/pdf", selectedReport.Text + " " + DateTime.Now.ToString("MM/dd/yyy") + ".pdf");
                }
            }
            catch (Exception)
            {

            }

            return File(new byte[] { }, "application/pdf", "Error.pdf");
        }
        public bool HasKBBAuthorization()
        {
            try
            {
                return _kbbService.IsAuthorized(SessionHandler.User.Setting.KellyBlueBook, SessionHandler.User.Setting.KellyPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult KarPowerResultInSingleMode(string vin, string mileage, string modelId, string trimId, string type, bool hasVin)
        {
            var model = CreateViewModelForKarPowerResult(vin, mileage, modelId, trimId, type);
            //model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;
            model.HasVin = hasVin;

            SessionHandler.KarPowerViewModel = model;
            return PartialView("KarPowerResult", model);
        }

        #region Private Methods

        private KarPowerViewModel CreateViewModelForKarPowerResult(string vin, string mileage, string modelId, string trimId, string type)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            var karPowerServiceWrapper = new KBBService();

            var savedModelId = 0;
            var savedTrimId = 0;
            var savedOptionIds = string.Empty;
            var savedEngineId = 0;
            var savedTransmissionId = 0;
            var savedDriveTrainId = 0;

            try
            {
                var dealer = SessionHandler.User;
                var kbbUserName = dealer.Setting.KellyBlueBook;
                var kbbPassword = dealer.Setting.KellyPassword;

              
                if (String.IsNullOrEmpty(kbbUserName) || String.IsNullOrEmpty(kbbPassword))
                    return model;

                karPowerServiceWrapper.ExecuteGetVehicleByVin(vin, valuationDate, kbbUserName, kbbPassword);
                Session["CookieContainer"] = karPowerServiceWrapper.CookieContainer;
                Session["CookieCollection"] = karPowerServiceWrapper.CookieCollection;

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetVehicleByVinResult);

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
            model.SelectedEngineId = savedTrimId == model.SelectedTrimId ? savedEngineId : 0;
            model.SelectedTransmissionId = savedTrimId == model.SelectedTrimId ? savedTransmissionId : 0;
            model.SelectedDriveTrainId = savedTrimId == model.SelectedTrimId ? savedDriveTrainId : 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

            try
            {
                // get Trims list
                //NOTE: more than 1 model
                if (model.SelectedModelId.Equals(0))
                {
                    model.IsMultipleModels = true;

                    model.SelectedModelId = modelId.Equals("0") ? Convert.ToInt32(model.Models.First().Value) : Convert.ToInt32(modelId);
                    var trimsResult = karPowerServiceWrapper.ExecuteGetTrims(model.SelectedYearId, model.SelectedModelId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(trimsResult))
                    {
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(trimsResult);
                        model.Trims = CreateDataList(jsonObj["d"], model.SelectedTrimId);
                        model.SelectedTrimId = modelId.Equals("0")
                            ? Convert.ToInt32(model.Trims.First().Value)
                            : model.SelectedTrimId;
                    }
                }

                // get Engines list
                karPowerServiceWrapper.ExecuteGetEngines(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetEnginesResult);
                    model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }

                // get Transmissions list
                karPowerServiceWrapper.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetTransmissionsResult);
                    model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
                }

                // get Drive Trains list
                karPowerServiceWrapper.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDriveTrainsResult);
                    model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                // loading save options in database
                if (model.SelectedTrimId == savedTrimId)
                {
                    foreach (var optionContract in model.OptionalEquipmentMarkupList)
                    {
                        optionContract.IsSelected = savedOptionIds.Contains(optionContract.Id);
                    }
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

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

        private KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId, int transmissionId)
        {
            var model = new KarPowerViewModel();
            if (Session["KarPowerViewModel"] != null)
                model = (KarPowerViewModel)Session["KarPowerViewModel"];

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
                var karPowerServiceWrapper = new KBBService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"]
                };

                // get Engines list
                karPowerServiceWrapper.ExecuteGetEngines(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetEnginesResult);
                    model.Engines = CommonHelper.CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }

                // get Transmissions list
                karPowerServiceWrapper.ExecuteGetTransmissions(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetTransmissionsResult);
                    model.Transmissions = CommonHelper.CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
                }

                // get Drive Trains list
                karPowerServiceWrapper.ExecuteGetDriveTrains(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDriveTrainsResult);
                    model.DriveTrains = CommonHelper.CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(CommonHelper.ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

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

        private KarPowerViewModel CreateViewModelForUpdateValuationByOptionalEquipment(string listingId, string isChecked)
        {
            var model = new KarPowerViewModel();
            if (Session["KarPowerViewModel"] != null)
                model = (KarPowerViewModel)Session["KarPowerViewModel"];

            try
            {
                var karPowerServiceWrapper = new KBBService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"]
                };
                var changedOption = model.OptionalEquipmentMarkupList.FirstOrDefault(i => i.Id == listingId);
                if (changedOption != null)
                {
                    changedOption.IsSelected = Convert.ToBoolean(isChecked);
                    var existingOptionHistory = model.OptionalEquipmentHistoryList.FirstOrDefault(i => i.Id == changedOption.Id);
                    if (existingOptionHistory != null)
                        existingOptionHistory.IsSelected = Convert.ToBoolean(isChecked);
                    else
                        model.OptionalEquipmentHistoryList.Add(changedOption);
                }

                // get kar power values without options
                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, new OptionContract[] { }, new OptionContract[] { });

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesaleWithoutOptions = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), model.OptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
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

        private int ConvertToInt32(JValue obj)
        {
            return obj != null ? Convert.ToInt32(obj.Value) : 0;
        }
        private string ConvertToString(JValue obj)
        {
            return obj != null ? Convert.ToString(obj.Value) : string.Empty;
        }

        private DateTime ConvertToDateTime(JValue obj)
        {
            return obj != null ? Convert.ToDateTime(obj.Value) : DateTime.Now;
        }
        private List<ExtendedSelectListItem> CreateDataList(JToken obj, int selectedId)
        {
            var result = new List<ExtendedSelectListItem>();
            if (obj != null)
            {
                result.AddRange(obj.Children().Where(item => !item.Value<string>("DisplayName").Equals("[Choose One]")).Select(item => new ExtendedSelectListItem()
                {
                    Text = item.Value<string>("DisplayName"),
                    Value = item.Value<string>("Id"),
                    Selected = item.Value<int>("Id") == selectedId
                }));
            }

            return result;
        }

        private List<OptionContract> CreateOptionalEquipmentList(string data)
        {
            if (String.IsNullOrEmpty(data)) return new List<OptionContract>();

            var result = new List<OptionContract>();
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

            return result;
        }

        #endregion
    }
}
