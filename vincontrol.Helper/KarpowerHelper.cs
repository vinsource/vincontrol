using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using KarPowerService = vincontrol.KBB.KBBService;

namespace vincontrol.Helper
{
    public class KarpowerHelper
    {
        public static KarPowerViewModel CreateViewModelForKarPowerResult(string kbbUserName, string kbbPassword, string vin, string mileage, string trimId, string type, DealerUser dealerUser)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            var karPowerServiceWrapper = new KarPowerService();

            const int savedTrimId = 0;
            var savedOptionIds = string.Empty;

            try
            {
                if (String.IsNullOrEmpty(kbbUserName) || String.IsNullOrEmpty(kbbPassword))
                    return model;

                karPowerServiceWrapper.ExecuteGetVehicleByVin(vin, valuationDate, kbbUserName, kbbPassword);
                dealerUser.KbbCookieContainer = karPowerServiceWrapper.CookieContainer;
                dealerUser.KbbCookieCollection = karPowerServiceWrapper.CookieCollection;

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetVehicleByVinResult);

                    model.SelectedYearId = CommonHelper.ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    model.SelectedMakeId = CommonHelper.ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    model.SelectedModelId = CommonHelper.ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    model.SelectedTrimId = CommonHelper.ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    model.SelectedEngineId = CommonHelper.ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    model.SelectedTransmissionId = CommonHelper.ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    model.SelectedDriveTrainId = CommonHelper.ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    model.ValuationDate = CommonHelper.ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    model.OptionalEquipmentMarkup = CommonHelper.ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    model.Makes = CommonHelper.CreateDataList(jsonObj["d"]["makes"], model.SelectedMakeId);

                    // get models
                    model.Models = CommonHelper.CreateDataList(jsonObj["d"]["models"], model.SelectedModelId);

                    // get trims
                    model.Trims = CommonHelper.CreateDataList(jsonObj["d"]["trims"], model.SelectedTrimId);

                    // get transmissions
                    model.Transmissions = CommonHelper.CreateDataList(jsonObj["d"]["transmissions"], model.SelectedTransmissionId);

                    // get engines
                    model.Engines = CommonHelper.CreateDataList(jsonObj["d"]["engines"], model.SelectedEngineId);
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
                karPowerServiceWrapper.ExecuteGetEngines(model.SelectedTrimId, model.ValuationDate);
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
                karPowerServiceWrapper.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
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
                karPowerServiceWrapper.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
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

                IList<OptionContract> tempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(CommonHelper.ConvertToString((JValue)(jsonObj["d"])));
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

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), tempOptionalEquipmentMarkupList.ToArray());

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

        private static List<OptionContract> CreateOptionalEquipmentList(string data)
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

        private static IList<OptionContract> AddEngineTransmissionDriveTrainAsOptions(IList<OptionContract> list, int engineId, int transmissionId, int driveTrainId)
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

        public static KarPowerViewModel CreateViewModelForUpdateValuationByOptionalEquipment(string listingId, bool isChecked, KarPowerViewModel karPowerViewModel, CookieContainer container, CookieCollection collection)
        {
            var model = new KarPowerViewModel();
            if (karPowerViewModel != null)
                model = karPowerViewModel;

            try
            {
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = container,
                    CookieCollection = collection
                };
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

        public static KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId, int transmissionId, CookieContainer container, CookieCollection collection, KarPowerViewModel model)
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
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = container,
                    CookieCollection = collection
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

        public static Stream PrintReport(KarPowerViewModel model, string kbbUsername, string kbbPassword, CookieContainer cookieContainer, CookieCollection cookieCollection, int dealerId)
        {
            try
            {
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = cookieContainer,
                    CookieCollection = cookieCollection,
                    UserName = kbbUsername,
                    Password = kbbPassword
                };

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
                    drivetrain = selectedDriveTrain!=null? selectedDriveTrain.Text:String.Empty,
                    drivetrainId = selectedDriveTrain!=null?selectedDriveTrain.Value:String.Empty,
                    engine = selectedEngine!=null? selectedEngine.Text:String.Empty,
                    engineId = selectedEngine!=null?selectedEngine.Value:String.Empty,
                    exteriorColor = "Select Color or Enter Manually",
                    interiorColor = "Select Color or Enter Manually",
                    initialDate = model.ValuationDate.ToString("M/d/yyyy"),
                    inventoryEntryId = "",
                    isPreOwnedSessionVehicle = true,
                    make =selectedMake!=null? selectedMake.Text:String.Empty,
                    makeId = selectedMake!=null? selectedMake.Value:String.Empty,
                    mileage = model.SelectedMileage,
                    model = selectedModel!=null? selectedModel.Text:String.Empty,
                    modelId =selectedModel!=null? selectedModel.Value:string.Empty,
                    optionHistory = model.OptionalEquipmentHistoryList.ToArray(),
                    options = model.OptionalEquipmentMarkupList.ToArray(),
                    sellPrice = "",
                    stockNumber = "",
                    transmission =selectedTransmission!=null? selectedTransmission.Text:String.Empty,
                    transmissionId = selectedTransmission!=null? selectedTransmission.Value:String.Empty,
                    trim = selectedTrim!=null? selectedTrim.Text:string.Empty,
                    trimId =selectedTrim!=null? selectedTrim.Value:string.Empty,
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
                    model.EncodedInventoryId = CommonHelper.ConvertToString((JValue)(jsonObj["d"]["id"]));
                    model.EncodedVin = CommonHelper.ConvertToString((JValue)(jsonObj["d"]["vin"]));
                    model.EncodedStockNumber = CommonHelper.ConvertToString((JValue)(jsonObj["d"]["stockNumber"]));
                    model.EncodedInventoryCategoryId = CommonHelper.ConvertToString((JValue)(jsonObj["d"]["inventoryCategoryId"]));
                    model.CategoryName = CommonHelper.ConvertToString((JValue)(jsonObj["d"]["categoryName"]));
                }

                karPowerServiceWrapper.ExecuteGetReportParameters(model.EncodedInventoryId, selectedReport.Value, selectedReport.Text, "", model.Vin);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetReportParametersResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetReportParametersResult);
                    var hfReportParams = CommonHelper.ConvertToString((JValue)(jsonObj["d"]));
                    return karPowerServiceWrapper.ExecuteGetPreOwnedVehicleReport(hfReportParams);
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }
    }
}
