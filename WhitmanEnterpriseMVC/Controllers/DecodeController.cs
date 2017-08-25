using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class DecodeController : SecurityController
    {
        public ActionResult VinDecode(string vin)
        {
            ClearChromeSession();
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                JavaScriptModel model = DataHelper.GetJavaScripModel(vin.Trim().ToUpper(), dealer);
                return Json(model);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult SearchStock(string stock)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var dealer = (DealershipViewModel)Session["Dealership"];

            stock = CommonHelper.RemoveSpecialCharactersForSearchStock(stock);

            int numberofResultFromSearchStock;

            var sessionSingle = SessionHandler.Single;

            if (sessionSingle)
                numberofResultFromSearchStock = SQLHelper.CheckStockExist(stock, dealer);
            else
            {
                var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];
                numberofResultFromSearchStock = SQLHelper.CheckStockExistInGroup(stock, dealerGroup);
            }

            if (numberofResultFromSearchStock > 0)
            {
                if (numberofResultFromSearchStock == 1)
                {
                    var context = new whitmanenterprisewarehouseEntities();

                    var row = InventoryQueryHelper.GetSingleOrGroupInventory(context).First(x => x.StockNumber.ToLower().Contains(stock.ToLower()));

                    var model = new JavaScriptModel()
                                    {
                                        ListingId = row.ListingID.ToString(CultureInfo.InvariantCulture),
                                        Status = "Inventory"
                                    };

                    return Json(model);
                }
                else
                {
                    var model = new JavaScriptModel()
                                    {
                                        Status = "MutilpleInventoryResultFromStock",
                                    };

                    return Json(model);
                }
            }

            int numberofResultFromSearchVin;

            if (sessionSingle)
                numberofResultFromSearchVin = SQLHelper.CheckSimilarVinExist(stock, dealer);
            else
            {
                var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];
                numberofResultFromSearchVin = SQLHelper.CheckVinExistInGroup(stock, dealerGroup);
            }

            if (numberofResultFromSearchVin > 0)
            {
                if (numberofResultFromSearchVin == 1)
                {
                    var context = new whitmanenterprisewarehouseEntities();

                    var row = InventoryQueryHelper.GetSingleOrGroupInventory(context).First(x => x.VINNumber != null && x.VINNumber.ToLower() == stock.ToLower());

                    var model = new JavaScriptModel()
                                    {
                                        ListingId = row.ListingID.ToString(CultureInfo.InvariantCulture),
                                        Status = "Inventory"
                                    };

                    return Json(model);
                }
                else
                {
                    var model = new JavaScriptModel() { Status = "MutilpleInventoryResultFromVin" };

                    return Json(model);
                }
            }
            else
            {
                var model = new JavaScriptModel()
                                {
                                    Stock = stock,
                                    Status = "StockNotExisted"
                                };

                return Json(model);
            }
        }

        public ActionResult InvalidVinAlert(string vin)
        {
            ViewData["Vin"] = vin;

            return View("InvalidVinAlert");
        }

        public ActionResult InvalidCustomerVinAlert(string vin)
        {
            ViewData["Vin"] = vin;

            return View("InvalidCustomerVinAlert");
        }

        public ActionResult DecodeProcessingByVin(string vin)
        {
            if (SessionHandler.Dealership != null)
            {
                if (!SessionHandler.Single)
                    return RedirectToAction("ViewInventoryForAllStores", "Inventory");

                var viewModel = new AppraisalViewFormModel();

                var autoService = new ChromeAutoService();

                var dealer = SessionHandler.Dealership;

                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    if (Session["DealerGroup"] == null)
                    {
                        if (
                            context.whitmanenterpriseappraisals.Any(
                                i =>
                                i.DealershipId == dealer.DealershipId && i.VINNumber == vin))
                        {
                            var existingAppraisals =
                                context.whitmanenterpriseappraisals.First(
                                    i =>
                                    i.DealershipId == dealer.DealershipId && i.VINNumber == vin);

                            if (existingAppraisals != null)
                            {
                                return RedirectToAction("ViewProfileForAppraisal", "Appraisal",
                                                        new { appraisalId = existingAppraisals.idAppraisal });
                            }
                        }


                    }
                    else
                    {
                        if (context.whitmanenterpriseappraisals.Any(i => i.DealershipId == dealer.DealershipId && i.VINNumber == vin))
                        {

                            var existingAppraisals =
                                context.whitmanenterpriseappraisals.First(
                                    i =>
                                    i.DealershipId == dealer.DealershipId && i.VINNumber == vin);

                            if (existingAppraisals != null)
                            {
                                return RedirectToAction("ViewProfileForAppraisal", "Appraisal",
                                                        new { appraisalId = existingAppraisals.idAppraisal });
                            }

                        }
                        else
                        {
                            var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];
                            int matchingAppraisalId = SQLHelper.CheckVinExistInGroupForAppraisal(vin, dealerGroup);

                            if (matchingAppraisalId > 0)
                                return RedirectToAction("ViewProfileForAppraisal", "Appraisal",
                                                        new { appraisalId = matchingAppraisalId });
                        }


                    }

                }

                var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);

                if (vehicleInfo != null && (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful || vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.ConditionallySuccessful))
                {
                    if (vehicleInfo.style != null && vehicleInfo.style.Any())
                    {
                        var firstStyle = vehicleInfo.style.FirstOrDefault();
                        if (firstStyle != null)
                        {
                            bool existed;

                            //EXCEPTION FOR 

                            if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                            {
                                viewModel.TrimList = SelectListHelper.InitalTrimListForMercedesBenz(viewModel, firstStyle.mfrModelCode, vehicleInfo.style, firstStyle.id, out existed);
                                SessionHandler.ChromeTrimList = viewModel.TrimList;
                                if(viewModel.TrimList.Count()>1)
                                    vehicleInfo = autoService.GetVehicleInformationFromVin(vin, firstStyle.id);
                                var styleInfo = autoService.GetStyleInformationFromStyleId(firstStyle.id);

                                viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);
                            }
                            else
                            {
                                viewModel.TrimList = SelectListHelper.InitalTrimList(viewModel, firstStyle.trim, vehicleInfo.style, firstStyle.id, out existed);
                                SessionHandler.ChromeTrimList = viewModel.TrimList;
                                if (viewModel.TrimList.Count() > 1)
                                    vehicleInfo = autoService.GetVehicleInformationFromVin(vin, firstStyle.id);
                                var styleInfo = autoService.GetStyleInformationFromStyleId(firstStyle.id);

                                viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);
                            }


                        }
                    }
                }
                else
                {
                    return DecodeProcessingManual();
                }

                if (viewModel.IsTruck)
                {
                    viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                    viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                    viewModel.TruckClassList = SelectListHelper.InitalTruckClassList();

                    return View("NewAppraisalByTruck", viewModel);
                }

                return View("NewAppraisal", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult DuplicateAppraisal(int appraisalId, string location)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var dealer = (DealershipViewModel)Session["Dealership"];

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
                    var viewModel = new AppraisalViewFormModel { AppraisalGenerateId = row.AppraisalID.ToString() };
                    viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, location, false);

                    return View("DuplicateAppraisal", viewModel);
                }
            }
            else
            {

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

                    appraisal.AppraisalGenerateId = row.AppraisalID.ToString();

                    appraisal.ChromeModelId = chromeStyleId.ToString();

                    appraisal.ChromeStyleId = chromeModelId.ToString();

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
        }

        public ActionResult DuplicateAppraisalForTruck(int appraisalId, string location)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var row = ConvertHelper.GetAppraisalModelFromAppriaslId(appraisalId);

                var autoService = new ChromeAutoService();

                if (!String.IsNullOrEmpty(row.VinNumber))
                {
                    VehicleDescription vehicleInfo = autoService.GetVehicleInformationFromVin(row.VinNumber);

                    VehicleDescription styleInfo = null;
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

                        viewModel.SelectedTruckType = row.TruckType;

                        viewModel.SelectedTruckClass = row.TruckClass;

                        viewModel.SelectedTruckCategory = row.TruckCategory;

                        viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                        viewModel.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                        viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        return View("DuplicateAppraisalForTruck", viewModel);
                    }
                    else
                    {
                        var viewModel = new AppraisalViewFormModel();

                        viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, location, false);

                        viewModel.SelectedTruckType = row.TruckType;

                        viewModel.SelectedTruckClass = row.TruckClass;

                        viewModel.SelectedTruckCategory = row.TruckCategory;

                        viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                        viewModel.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                        viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        return View("DuplicateAppraisalForTruck", viewModel);
                    }
                }
                else
                {
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

                        appraisal.ChromeModelId = chromeModelId.ToString();

                        appraisal.ChromeStyleId = chromeStyleId.ToString();

                        appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                        if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                            appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                        appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                        appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                        appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        return View("DuplicateAppraisalForTruck", appraisal);
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

                        return View("DuplicateAppraisalForTruck", appraisal);
                    }
                }
            }
        }

        public ActionResult DecodeProcessingByYear(string year)
        {
            if (Session["Dealership"] != null)
            {
                if (!SessionHandler.Single)
                    return RedirectToAction("ViewInventoryForAllStores", "Inventory");

                var autoService = new ChromeAutoService();

                int modelYear = Convert.ToInt16(year);

                var divisionList = autoService.GetDivisions(modelYear);

                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        MakeList = SelectListHelper.InitialMakeList(divisionList),
                        ModelList = new List<SelectListItem>().AsEnumerable(),
                        TrimList = new List<SelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        FuelList = new List<SelectListItem>().AsEnumerable(),
                        CylinderList = new List<SelectListItem>().AsEnumerable(),
                        LitersList = new List<SelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<SelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<SelectListItem>().AsEnumerable(),
                        ModelYear = modelYear,
                        AppraisalDate = DateTime.Now.ToShortDateString()
                    };

                return View("NewAppraisalByYear", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult DecodeProcessingByYearForTruck(string year)
        {
            if (Session["Dealership"] != null)
            {
                if (!SessionHandler.Single)
                    return RedirectToAction("ViewInventoryForAllStores", "Inventory");

                var autoService = new ChromeAutoService();

                int modelYear = Convert.ToInt16(year);

                var divisionList = autoService.GetDivisions(modelYear);

                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        MakeList = SelectListHelper.InitialMakeList(divisionList),
                        ModelList = new List<SelectListItem>().AsEnumerable(),
                        TrimList = new List<SelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        FuelList = new List<SelectListItem>().AsEnumerable(),
                        CylinderList = new List<SelectListItem>().AsEnumerable(),
                        LitersList = new List<SelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<SelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<SelectListItem>().AsEnumerable(),
                        ModelYear = modelYear,
                        AppraisalDate = DateTime.Now.ToShortDateString(),
                        TruckTypeList = SelectListHelper.InitalTruckTypeList()
                    };

                viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                viewModel.TruckClassList = SelectListHelper.InitalTruckClassList();

                viewModel.IsTruck = true;

                return View("NewAppraisalByYearForTruck", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult DecodeProcessingManual()
        {
            if (Session["Dealership"] != null)
            {
                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        AppraisalDate = DateTime.Now.ToShortDateString()
                    };

                return View("ManualAppraisal", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult DecodeProcessingTruckManual()
        {
            if (Session["Dealership"] != null)
            {
                var viewModel = new AppraisalViewFormModel
                    {
                        VinDecodeSuccess = false,
                        AppraisalDate = DateTime.Now.ToShortDateString(),
                        TruckTypeList = SelectListHelper.InitalTruckTypeList()
                    };

                viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                viewModel.TruckClassList = SelectListHelper.InitalTruckClassList();

                viewModel.IsTruck = true;

                return View("ManualAppraisalForTruck", viewModel);
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult BlankAppraisal()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var viewModel = new AppraisalViewFormModel
                    {
                        ModelYearList = SelectListHelper.InitialYearList(),
                        VinDecodeSuccess = false,
                        MakeList = new List<SelectListItem>().AsEnumerable(),
                        ModelList = new List<SelectListItem>().AsEnumerable(),
                        TrimList = new List<SelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        FuelList = new List<SelectListItem>().AsEnumerable(),
                        CylinderList = new List<SelectListItem>().AsEnumerable(),
                        LitersList = new List<SelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<SelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<SelectListItem>().AsEnumerable(),
                        //AppraisalGenerateId = SQLHelper.GenerateAppraisalIdByDealerId(dealer.DealershipId),
                        AppraisalDate = DateTime.Now.ToShortDateString()
                    };


                return View("BlankAppraisal", viewModel);
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult RestoredBlankAppraisal()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var viewModel = new AppraisalViewFormModel
                    {
                        ModelYearList = SelectListHelper.InitialYearList(),
                        VinDecodeSuccess = false,
                        MakeList = new List<SelectListItem>().AsEnumerable(),
                        ModelList = new List<SelectListItem>().AsEnumerable(),
                        TrimList = new List<SelectListItem>().AsEnumerable(),
                        ExteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        InteriorColorList = new List<SelectListItem>().AsEnumerable(),
                        FuelList = new List<SelectListItem>().AsEnumerable(),
                        CylinderList = new List<SelectListItem>().AsEnumerable(),
                        LitersList = new List<SelectListItem>().AsEnumerable(),
                        BodyTypeList = new List<SelectListItem>().AsEnumerable(),
                        DriveTrainList = new List<SelectListItem>().AsEnumerable(),
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
                    appraisal.Trim = selectedTrim.Text;
                    appraisal.SelectedTrimItem = selectedTrim.Value;
                }
            }

            if (!isTruck)
            {

            }
            else
            {
                //appraisal.SelectedTruckType = row.TruckType;

                //appraisal.SelectedTruckClass = row.TruckClass;

                //appraisal.SelectedTruckCategory = row.TruckCategory;

                appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                //appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();
            }

            return PartialView("DetailAppraisal", appraisal);
        }

        private void ClearChromeSession()
        {
            SessionHandler.ChromeTrimList = null;
        }
    }
}
