using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.ChromeAutoService;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;

namespace Vincontrol.Web.Controllers
{
    public class DecodeController : SecurityController
    {
        private ICommonManagementForm _commonManagementForm;
    

        public DecodeController()
        {
            _commonManagementForm = new CommonManagementForm();
        }

        public ActionResult VinDecode(string vin)
        {
            ClearChromeSession();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                var model= HelperClass.DataHelper.GetJavaScripModel(vin.Trim().ToUpper(), dealer);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ViewOrCreateNew(string vin,string appraisalDate)
        {
            ViewData["AppraisalDate"] = appraisalDate;
            return View();
        }

        public ActionResult FullTextSearch(string searchTerm)
        {
            using (var context = new VincontrolEntities())
            {
                var vehicleQuery = GetVehicleQuery(searchTerm, context, QueryBuilderHelper.GetFullTextTopVehicleQuery);

                var query =
                    string.Format("{0} UNION ALL {1} UNION ALL {2}",
                        vehicleQuery.AppraisalQuery, vehicleQuery.InventoryQuery, vehicleQuery.SoldoutQuery);

                return Json(BuildFullTextSearchResult(query, context), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FullTextSearchWithFullResult(string searchTerm)
        {
            using (var context = new VincontrolEntities())
            {
                var vehicleQuery = GetVehicleQuery(searchTerm, context, QueryBuilderHelper.GetFullTextVehicleQuery);

                var query =
                    string.Format("SELECT * FROM ( {0} UNION ALL {1} UNION ALL {2}) RESULT ORDER BY DealerId, Year DESC, Make, Model, Trim",
                        vehicleQuery.AppraisalQuery, vehicleQuery.InventoryQuery, vehicleQuery.SoldoutQuery);

                return new LargeJsonResult
                {
                    Data = BuildFullTextSearchResult(query, context)
                };
            }
        }

        private static VehicleQuery GetVehicleQuery(string searchTerm, VincontrolEntities context, Func<string,List<string>, IEnumerable<int>, string> getQueryFunc )
        {
            var dealerIdList = InventoryQueryHelper.GetDealerList(context).ToList();
            var termList =
                searchTerm.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList();

            return new VehicleQuery()
            {
                AppraisalQuery = getQueryFunc("AppraisalCar", termList, dealerIdList),
                InventoryQuery = getQueryFunc("InventoryCar", termList, dealerIdList),
                SoldoutQuery = getQueryFunc("SoldoutInventoryCar", termList, dealerIdList)
            };
        }

        private List<CarResult> BuildFullTextSearchResult(string query, VincontrolEntities context)
        {
            var result = context.ExecuteStoreQuery<CarResult>(query).ToList();

            foreach (var item in result)
            {
                if (item.ThumbnailUrl != null)
                {
                    item.ThumbnailUrl =
                        item.ThumbnailUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                            .FirstOrDefault();
                    item.PhotoUrl = item.ThumbnailUrl;
                }
                else if (item.PhotoUrl != null && item.VehicleStatus != Constanst.VehicleStatus.SoldOut)
                {
                    item.PhotoUrl =
                        item.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                            .FirstOrDefault();
                }
                else
                {
                    item.PhotoUrl = item.DefaultStockImage;
                }
            }

            return result;

        }

        public ActionResult InvalidVinAlert(string vin)
        {
            ViewData["Vin"] = vin;

            return View("InvalidVinAlert");
        }

        public ActionResult YearOutOfRangeAlert(string vin)
        {
            ViewData["Vin"] = vin;

            return View("YearOutOfRangeAlert");
        }

        public ActionResult InvalidCustomerVinAlert(string vin)
        {
            ViewData["Vin"] = vin;

            return View("InvalidCustomerVinAlert");
        }

        public ActionResult CheckVin(string vin)
        {
            if (SessionHandler.Dealer != null && !string.IsNullOrEmpty(vin))
            {
                vin = vin.Trim();
                var dealer = SessionHandler.Dealer;
                using (var context = new VincontrolEntities())
                {
                    var existingInventory =
                            context.Inventories.FirstOrDefault(
                                i =>
                                i.DealerId == dealer.DealershipId && i.Vehicle.Vin == vin);
                    if (existingInventory != null && existingInventory.InventoryId > 0)
                        return
                            Json(
                                new
                                    {
                                        success = true,
                                        isExisted = true,
                                        id = existingInventory.InventoryId,
                                        isAppraisal = false,
                                        vin = vin
                                    });
                    else
                    {
                        var existingAppraisals =
                            context.Appraisals.FirstOrDefault(
                                i =>
                                i.DealerId == dealer.DealershipId && i.Vehicle.Vin == vin);
                        if (existingAppraisals != null && existingAppraisals.AppraisalId > 0)
                            return
                                Json(
                                    new
                                        {
                                            success = true,
                                            isExisted = true,
                                            id = existingAppraisals.AppraisalId,
                                            isAppraisal = true,
                                            vin = vin
                                        });
                        else
                            return Json(new { success = true, isExisted = false, vin = vin });
                    }
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult DecodeProcessingByVin(string vin)
        {
            if (SessionHandler.Dealer != null)
            {
                if (!SessionHandler.Single)
                    return RedirectToAction("ViewInventory", "Inventory");

                var viewModel = new AppraisalViewFormModel();

                var autoService = new ChromeAutoService();

                var vehicleInfo = SessionHandler.GetVehicleDescriptionData(vin) ??
                                  autoService.GetVehicleInformationFromVin(vin);

                if (vehicleInfo != null &&
                    (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                     vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.ConditionallySuccessful))
                {
                    if (vehicleInfo.style != null && vehicleInfo.style.Any())
                    {
                        var firstStyle = vehicleInfo.style.FirstOrDefault();

                        if (firstStyle != null)
                        {
                            bool existed;

                            if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                            {
                                viewModel.TrimList = SelectListHelper.InitalTrimListForMercedesBenz(viewModel,
                                    firstStyle.mfrModelCode, vehicleInfo.style, firstStyle.id, out existed);
                            }
                            else
                            {
                                viewModel.TrimList = SelectListHelper.InitalTrimList(viewModel, firstStyle.trim,
                                    vehicleInfo.style, firstStyle.id, out existed);
                            }

                            SessionHandler.ChromeTrimList = viewModel.TrimList;

                            var styleInfo = autoService.GetStyleInformationFromStyleId(firstStyle.id);

                            viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);

                        }
                    }
                    else
                    {
                        viewModel.VinNumber = vehicleInfo.vinDescription.vin;
                        viewModel.ModelYear = vehicleInfo.vinDescription.modelYear;
                        viewModel.Make = vehicleInfo.vinDescription.division;
                        viewModel.SelectedModel = vehicleInfo.vinDescription.modelName;
                    }
                }
                else
                {
                    return DecodeProcessingManual();
                }
                viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes();

                viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategories();

                viewModel.TruckClassList = _commonManagementForm.GetTruckClasses();

                viewModel.SelectedTrim = viewModel.SelectedTrimItem;
                viewModel.SelectedVehicleType = viewModel.IsTruck ? "Truck" : "Car";
                viewModel.VehicleTypeList =
                    SelectListHelper.InitalVehicleTypeList(viewModel.IsTruck
                        ? Constanst.VehicleType.Truck
                        : Constanst.VehicleType.Car);
                return View("NewAppraisal", viewModel);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult DuplicateAppraisal(int appraisalId, string location)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var dealer = SessionHandler.Dealer;

            var row = ConvertHelper.GetAppraisalModelFromAppriaslId(appraisalId);

            var autoService = new ChromeAutoService();

            if (!String.IsNullOrEmpty(row.VinNumber))
            {
                VehicleDescription vehicleInfo = autoService.GetVehicleInformationFromVin(row.VinNumber);
                VehicleDescription styleInfo;

                if (!String.IsNullOrEmpty(row.ChromeStyleId))
                {
                    int styleId;
                    Int32.TryParse(row.ChromeStyleId, out styleId);
                    styleInfo = autoService.GetStyleInformationFromStyleId(styleId);
                }
                else
                {
                    if (vehicleInfo.style.Any(x => x.trim == row.Trim))
                    {
                        var element = vehicleInfo.style.First(x => x.trim == row.Trim);
                        styleInfo = autoService.GetStyleInformationFromStyleId(element.id);
                    }
                    else
                    {
                        styleInfo = autoService.GetStyleInformationFromStyleId(vehicleInfo.style.First().id);
                    }
                }

                if (vehicleInfo != null)
                {
                    var viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);
                    viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, location, true);

                    return View("DuplicateAppraisal", viewModel);
                }
                else
                {
                    var viewModel = new AppraisalViewFormModel { AppraisalGenerateId = row.AppraisalID.ToString(CultureInfo.InvariantCulture) };
                    viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, null, dealer.DealershipId, location, false);

                    return View("DuplicateAppraisal", viewModel);
                }
            }
            if (!String.IsNullOrEmpty(row.ChromeStyleId))
            {
                int chromeStyleId;
                Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                int chromeModelId;
                Int32.TryParse(row.ChromeModelId, out chromeModelId);

                var styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);

                var styleArray = autoService.GetStyles(chromeModelId);

                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModel(appraisal, row, styleInfo, dealer.DealershipId, location, false);

                appraisal.AppraisalGenerateId = row.AppraisalID.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeModelId = chromeStyleId.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeStyleId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                    appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                return View("DuplicateAppraisal", appraisal);
            }
            else
            {
                int chromeModelId;
                Int32.TryParse(row.ChromeModelId, out chromeModelId);

                var styleArray = autoService.GetStyles(chromeModelId);
                VehicleDescription styleInfo = null;

                if (row.SelectedTrim != null && row.SelectedTrim.Equals(string.Empty))
                {
                    styleInfo = autoService.GetStyleInformationFromStyleId(styleArray.First().id);
                }

                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModel(appraisal, row, styleInfo, dealer.DealershipId, location, false);

                appraisal.AppraisalGenerateId = row.AppraisalID.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeModelId = styleArray.First().id.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeStyleId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                return View("DuplicateAppraisal", appraisal);
            }
        }

        public ActionResult DuplicateAppraisalForTruck(int appraisalId, string location)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;

            var row = ConvertHelper.GetAppraisalModelFromAppriaslId(appraisalId);

            var autoService = new ChromeAutoService();

            if (!String.IsNullOrEmpty(row.VinNumber))
            {
                VehicleDescription vehicleInfo = autoService.GetVehicleInformationFromVin(row.VinNumber);

                VehicleDescription styleInfo;
                if (!String.IsNullOrEmpty(row.ChromeStyleId))
                {
                    int styleId;
                    Int32.TryParse(row.ChromeStyleId, out styleId);

                    styleInfo = autoService.GetStyleInformationFromStyleId(styleId);
                }
                else
                {
                    if (vehicleInfo.style.Any(x => x.trim == row.Trim))
                    {
                        var element = vehicleInfo.style.First(x => x.trim == row.Trim);
                        styleInfo = autoService.GetStyleInformationFromStyleId(element.id);
                    }
                    else
                    {
                        styleInfo = autoService.GetStyleInformationFromStyleId(vehicleInfo.style.First().id);
                    }
                }

                if (vehicleInfo != null)
                {
                    var viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);

                    viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, location, true);

                    viewModel.TruckType = row.TruckType;

                    viewModel.SelectedTruckType = row.SelectedTruckType;

                    viewModel.TruckClass = row.TruckClass;

                    viewModel.SelectedTruckClassId = row.SelectedTruckClassId;

                    viewModel.TruckCategory = row.TruckCategory;

                    viewModel.SelectedTruckCategoryId = row.SelectedTruckCategoryId;

                    viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

                    viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

                    viewModel.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

                    viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                    return View("DuplicateAppraisal", viewModel);
                }
                else
                {
                    var viewModel = new AppraisalViewFormModel();

                    viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, null, dealer.DealershipId, location, false);

                    viewModel.TruckType = row.TruckType;

                    viewModel.SelectedTruckType = row.SelectedTruckType;

                    viewModel.TruckClass = row.TruckClass;

                    viewModel.SelectedTruckClassId = row.SelectedTruckClassId;

                    viewModel.TruckCategory = row.TruckCategory;

                    viewModel.SelectedTruckCategoryId = row.SelectedTruckCategoryId;

                    viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

                    viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

                    viewModel.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

                    viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                    return View("DuplicateAppraisal", viewModel);
                }
            }
            if (!String.IsNullOrEmpty(row.ChromeStyleId))
            {
                int chromeStyleId;
                Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                int chromeModelId;
                Int32.TryParse(row.ChromeModelId, out chromeModelId);

                var styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);

                var styleArray = autoService.GetStyles(chromeModelId);

                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModel(appraisal, row, styleInfo, dealer.DealershipId, location, false);

                appraisal.ChromeModelId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeStyleId = chromeStyleId.ToString(CultureInfo.InvariantCulture);

                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                    appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                return View("DuplicateAppraisal", appraisal);
            }
            else
            {
                int chromeModelId;
                Int32.TryParse(row.ChromeModelId, out chromeModelId);

                var styleArray = autoService.GetStyles(chromeModelId);
                VehicleDescription styleInfo = null;

                if (row.SelectedTrim != null && row.SelectedTrim.Equals(string.Empty))
                {
                    styleInfo = autoService.GetStyleInformationFromStyleId(styleArray.First().id);
                }

                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModel(appraisal, row, styleInfo, dealer.DealershipId, location, false);

                appraisal.ChromeModelId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeStyleId = styleArray.First().id.ToString(CultureInfo.InvariantCulture);

                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                if (styleInfo != null && styleInfo.style != null && styleInfo.style.First().stockImage != null)
                    appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                return View("DuplicateAppraisal", appraisal);
            }
        }

        public ActionResult DecodeProcessingByYear(string year)
        {
            if (SessionHandler.Dealer != null)
            {
                if (!SessionHandler.Single)
                    return RedirectToAction("ViewInventory", "Inventory");

                var autoService = new ChromeAutoService();

                int modelYear = Convert.ToInt16(year);

                var divisionList = autoService.GetDivisions(modelYear);

                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        SelectedVehicleType = "Car",
                        MakeList = SelectListHelper.InitialMakeListDefault(divisionList),
                        ModelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        TrimList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        FuelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        CylinderList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        LitersList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ModelYear = modelYear,
                        AppraisalDate = DateTime.Now.ToShortDateString(),
                        TruckTypeList = _commonManagementForm.GetTruckTypes(),
                        TruckCategoryList = _commonManagementForm.GetTruckCategories(),
                        TruckClassList = _commonManagementForm.GetTruckClasses(),
                        VehicleTypeList = SelectListHelper.InitalVehicleTypeList(Constanst.VehicleType.Car)
                    };

                return View("NewAppraisalByYear", viewModel);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult DecodeProcessingByYearForTruck(string year)
        {
            if (SessionHandler.Dealer != null)
            {
                if (!SessionHandler.Single)
                    return RedirectToAction("ViewInventory", "Inventory");

                var autoService = new ChromeAutoService();

                int modelYear = Convert.ToInt16(year);

                var divisionList = autoService.GetDivisions(modelYear);

                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        MakeList = SelectListHelper.InitialMakeList(divisionList),
                        ModelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        TrimList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        FuelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        CylinderList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        LitersList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ModelYear = modelYear,
                        AppraisalDate = DateTime.Now.ToShortDateString(),
                        TruckTypeList = _commonManagementForm.GetTruckTypes(),
                        TruckCategoryList = _commonManagementForm.GetTruckCategories(),
                        TruckClassList = _commonManagementForm.GetTruckClasses()
                    };

                viewModel.IsTruck = true;

                return View("NewAppraisalByYear", viewModel);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult DecodeProcessingManual(int year = 0)
        {
            if (SessionHandler.Dealer != null)
            {
                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        AppraisalDate = DateTime.Now.ToShortDateString(),
                        ModelYear = year,
                        IsTruck = false,
                        SelectedVehicleType = "Car",
                        TruckTypeList = _commonManagementForm.GetTruckTypes(),
                        TruckCategoryList = _commonManagementForm.GetTruckCategories(),
                        TruckClassList = _commonManagementForm.GetTruckClasses(),
                        VehicleTypeList = SelectListHelper.InitalVehicleTypeList(Constanst.VehicleType.Car),
                        DriveTrainList = SelectListHelper.InitalEditDriveTrainList(null)
                    };

                return View("ManualAppraisal", viewModel);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult DecodeProcessingTruckManual()
        {
            if (SessionHandler.Dealer != null)
            {
                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        AppraisalDate = DateTime.Now.ToShortDateString(),
                        TruckTypeList = _commonManagementForm.GetTruckTypes(),
                        TruckCategoryList = _commonManagementForm.GetTruckCategories(),
                        TruckClassList = _commonManagementForm.GetTruckClasses(),
                        IsTruck = true,
                        SelectedVehicleType = "Truck",
                        VehicleTypeList = SelectListHelper.InitalVehicleTypeList(Constanst.VehicleType.Truck)
                    };

                return View("ManualAppraisal", viewModel);
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult BlankAppraisal()
        {
            if (SessionHandler.Dealer != null)
            {
                var viewModel = new AppraisalViewFormModel
                    {
                        ModelYearList = SelectListHelper.InitialYearList(),
                        VinDecodeSuccess = false,
                        MakeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ModelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        TrimList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        FuelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        CylinderList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        LitersList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        //AppraisalGenerateId = SQLHelper.GenerateAppraisalIdByDealerId(dealer.DealershipId),
                        AppraisalDate = DateTime.Now.ToShortDateString()
                    };


                return View("BlankAppraisal", viewModel);
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult RestoredBlankAppraisal()
        {
            if (SessionHandler.Dealer != null)
            {
                var viewModel = new AppraisalViewFormModel
                    {
                        ModelYearList = SelectListHelper.InitialYearList(),
                        VinDecodeSuccess = false,
                        MakeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ModelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        TrimList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        FuelList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        CylinderList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        LitersList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<ExtendedSelectListItem>().AsEnumerable(),
                        //AppraisalGenerateId = SQLHelper.GenerateAppraisalIdByDealerId(dealer.DealershipId),
                        AppraisalDate = DateTime.Now.ToShortDateString()
                    };


                return View("BlankAppraisal", viewModel);
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult GetVehicleInformationFromStyleId(string vin, string styleId, bool isTruck, string styleName, string cusStyle)
        {
            var autoService = new ChromeAutoService();

            var vehicleInfo = autoService.GetVehicleInformationFromVin(vin, Convert.ToInt32(styleId));
            var styleInfo = autoService.GetStyleInformationFromStyleId(Convert.ToInt32(styleId));
            var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);
            appraisal.CusTrim = cusStyle;
            if (!String.IsNullOrEmpty(styleName))
            {
                foreach (var item in appraisal.TrimList)
                {
                    item.Selected = item.Text.Equals(styleName);
                }

                var selectedTrim = appraisal.TrimList.FirstOrDefault(i => i.Selected);
                if (selectedTrim != null)
                {
                    appraisal.Trim = selectedTrim.Text;
                    appraisal.SelectedTrimItem = selectedTrim.Value;
                }
                else
                {
                    selectedTrim = appraisal.TrimList.FirstOrDefault();
                    if (selectedTrim != null)
                    {
                        appraisal.Trim = selectedTrim.Text;
                        appraisal.SelectedTrimItem = selectedTrim.Value;
                    }
                }
            }
                        
            {
                appraisal.TruckTypeList = _commonManagementForm.GetTruckTypes();
                appraisal.TruckCategoryList = _commonManagementForm.GetTruckCategories();
                appraisal.TruckClassList = _commonManagementForm.GetTruckClasses();
                appraisal.SelectedVehicleType = appraisal.IsTruck ? "Truck" : "Car";
                appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeList(appraisal.IsTruck ? Constanst.VehicleType.Truck : Constanst.VehicleType.Car);
            }

            //return PartialView("DetailAppraisal", appraisal);
            return PartialView("../Appraisal/DetailAppraisal", appraisal);
        }

        private void ClearChromeSession()
        {
            SessionHandler.ChromeTrimList = null;
        }
    }
}
