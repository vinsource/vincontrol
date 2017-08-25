using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using HiQPdf;
using System.Web;
using System.IO;
using WhitmanEnterpriseMVC.Interfaces;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class ChartController : SecurityController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewCarfax(string Url)
        {
            var vinNumber = new VINViewModel() { VINNumber = CarFaxHelper.GetVinNumberFromDetailUrl(Url) };
            return View("ViewCARFAX", vinNumber);
        }

        public ActionResult CARFAXDetail(string vin)
        {
            var dealer = SessionHandler.Dealership;
            var item = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(vin, dealer.CarFax, dealer.CarFaxPassword);
            var carFax = new CarFaxDetailViewModel() { CarFax = item, CarFaxDealerId = dealer.CarFax, Vin = vin };
            return PartialView(carFax);
        }

        public ActionResult ViewFullChart(int listingId, string filterOptions)
        {
            return GetChartData(listingId, "ViewGraph", filterOptions);
        }

        private ActionResult GetChartData(int listingId, string viewName, string filterOptions)
        {
            SessionHandler.CanViewBucketJumpReport = null;
            if (SessionHandler.Dealership == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var contextVinControl = new whitmanenterprisewarehouseEntities();
            var model = GetChartItems(listingId, filterOptions, contextVinControl);
            GetSavedSelections(listingId, model, contextVinControl, filterOptions, Constanst.Inventory);

            return View(viewName, model);
        }

        private ChartSelectionViewModel GetChartItems(int listingId, string filterOptions, whitmanenterprisewarehouseEntities contextVinControl)
        {
            var dealer = SessionHandler.Dealership;
            SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

            // Create a Session to identify what the chart screen is
            SessionHandler.ChartScreen = Constanst.Inventory;

            var model = new ChartSelectionViewModel() { FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null };
            model.CarsCom = new ChartSelection();


            whitmanenterprisedealershipinventory targetCar;

            targetCar =
                contextVinControl.whitmanenterprisedealershipinventories.FirstOrDefault((x => x.ListingID == listingId));
            if (targetCar == null)
            {
                targetCar = new whitmanenterprisedealershipinventory();
                // check to see if the car is in wholesale
                var wholesalevehicle =
                    contextVinControl.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == listingId);
                if (wholesalevehicle != null)
                {
                    targetCar.VINNumber = wholesalevehicle.VINNumber;
                    targetCar.Trim = wholesalevehicle.Trim;
                    targetCar.ModelYear = wholesalevehicle.ModelYear;
                    targetCar.Make = wholesalevehicle.Make;
                    targetCar.Model = wholesalevehicle.Model;
                    targetCar.Certified = wholesalevehicle.Certified;
                    targetCar.DealershipName = wholesalevehicle.DealershipName;
                    targetCar.DealershipId = wholesalevehicle.DealershipId;
                    targetCar.DealershipZipCode = wholesalevehicle.DealershipZipCode;
                    targetCar.DealershipState = wholesalevehicle.DealershipState;
                    targetCar.DealershipPhone = wholesalevehicle.DealershipPhone;
                    targetCar.DealershipCity = wholesalevehicle.DealershipCity;
                    targetCar.DealershipAddress = wholesalevehicle.DealershipAddress;
                }
            }

            ViewData[Constanst.ListingId] = listingId;

            ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                   targetCar.Trim + " - Mileage : " + targetCar.Mileage + " / Price : " +
                                   targetCar.SalePrice;
            return model;
        }

        private static void GetSavedSelections(int listingId, ChartSelectionViewModel model, whitmanenterprisewarehouseEntities contextVinControl, string filterOptions, string screenType)
        {
            //Get seleciton form POSTDATA
            if (!String.IsNullOrEmpty(filterOptions))
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(filterOptions)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(SelectionData));
                    var selectionData = (SelectionData)serializer.ReadObject(ms);

                    model.IsCarsCom = selectionData.Source == "carscom";
                    model.IsAll = selectionData.DealerType == "all";
                    model.IsCertified = selectionData.IsCertified;
                    model.IsFranchise = selectionData.DealerType == "franchise";
                    model.IsIndependant = selectionData.DealerType == "independant";
                    model.Options = selectionData.Options;
                    model.Trims = String.Join(",", selectionData.Trims);

                    if (selectionData.Source == "carscom")
                    {
                        model.CarsCom.IsCarsCom = model.IsCarsCom = true;
                        model.CarsCom.IsAll = selectionData.DealerType == "all";
                        model.CarsCom.IsCertified = selectionData.IsCertified;
                        model.CarsCom.IsFranchise = selectionData.DealerType == "franchise";
                        model.CarsCom.IsIndependant = selectionData.DealerType == "independant";
                        model.CarsCom.Options = selectionData.Options;
                        model.CarsCom.Trims = String.Join(",", selectionData.Trims);
                    }
                }
            }
            else
            {
                var existingChartSelection =
                    contextVinControl.vincontrolchartselections.Where(
                        s => s.listingId == listingId && s.screen == screenType).ToList();

                if (existingChartSelection.Count > 0)
                {
                    foreach (var vincontrolchartselection in existingChartSelection)
                    {
                        if (vincontrolchartselection.sourceType.Equals(Constanst.AutoTrader))
                        {
                            model.IsAll = vincontrolchartselection.isAll != null &&
                                          Convert.ToBoolean(vincontrolchartselection.isAll);
                            model.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                            model.IsCertified = vincontrolchartselection.isCertified != null &&
                                                Convert.ToBoolean(vincontrolchartselection.isCertified);
                            model.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                                Convert.ToBoolean(vincontrolchartselection.isFranchise);
                            model.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                                  Convert.ToBoolean(vincontrolchartselection.isIndependant);
                            model.Options = vincontrolchartselection.options != null
                                                ? vincontrolchartselection.options.ToLower()
                                                : "";
                            model.Trims = vincontrolchartselection.trims != null
                                              ? vincontrolchartselection.trims.ToLower()
                                              : "";
                        }

                        if (vincontrolchartselection.sourceType.Equals(Constanst.CarsCom))
                        {
                            model.CarsCom.IsAll = vincontrolchartselection.isAll != null &&
                                                  Convert.ToBoolean(vincontrolchartselection.isAll);
                            model.CarsCom.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                                      Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                            model.CarsCom.IsCertified = vincontrolchartselection.isCertified != null &&
                                                        Convert.ToBoolean(vincontrolchartselection.isCertified);
                            model.CarsCom.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                                        Convert.ToBoolean(vincontrolchartselection.isFranchise);
                            model.CarsCom.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                                          Convert.ToBoolean(vincontrolchartselection.isIndependant);
                            model.CarsCom.Options = vincontrolchartselection.options != null
                                                        ? vincontrolchartselection.options.ToLower()
                                                        : "";
                            model.CarsCom.Trims = vincontrolchartselection.trims != null
                                                      ? vincontrolchartselection.trims.ToLower()
                                                      : "";
                        }
                    }
                }
            }

        }


        public ActionResult ViewGoogleGraph(int listingId, string filterOptions)
        {
            return GetChartData(listingId, "ViewGoogleGraph", filterOptions);
        }

        public ActionResult ViewFullChartInAppraisal(int appraisalId)
        {
            SessionHandler.CanViewBucketJumpReport = null;
            if (SessionHandler.Dealership == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealership;
            SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

            // Create a Session to identify what the chart screen is
            SessionHandler.ChartScreen = Constanst.Appraisal;

            var model = new ChartSelectionViewModel();
            model.CarsCom = new ChartSelection();
            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var targetCar =
                contextVinControl.whitmanenterpriseappraisals.FirstOrDefault((x => x.idAppraisal == appraisalId));

            ViewData[Constanst.ListingId] = targetCar.idAppraisal.ToString(CultureInfo.InvariantCulture);

            ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                   targetCar.Trim + " - Mileage : " + targetCar.Mileage;
            var existingChartSelection =
                contextVinControl.vincontrolchartselections.Where(
                    s => s.listingId == appraisalId && s.screen == Constanst.Appraisal).ToList();
            if (existingChartSelection.Count > 0)
            {
                foreach (var vincontrolchartselection in existingChartSelection)
                {
                    if (vincontrolchartselection.sourceType.Equals(Constanst.AutoTrader))
                    {
                        model.IsAll = vincontrolchartselection.isAll != null &&
                                      Convert.ToBoolean(vincontrolchartselection.isAll);
                        model.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                          Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                        model.IsCertified = vincontrolchartselection.isCertified != null &&
                                            Convert.ToBoolean(vincontrolchartselection.isCertified);
                        model.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                            Convert.ToBoolean(vincontrolchartselection.isFranchise);
                        model.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isIndependant);
                        model.Options = vincontrolchartselection.options != null
                                            ? vincontrolchartselection.options.ToLower()
                                            : "";
                        model.Trims = vincontrolchartselection.trims != null
                                          ? vincontrolchartselection.trims.ToLower()
                                          : "";
                    }

                    if (vincontrolchartselection.sourceType.Equals(Constanst.CarsCom))
                    {
                        model.CarsCom.IsAll = vincontrolchartselection.isAll != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isAll);
                        model.CarsCom.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                                  Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                        model.CarsCom.IsCertified = vincontrolchartselection.isCertified != null &&
                                                    Convert.ToBoolean(vincontrolchartselection.isCertified);
                        model.CarsCom.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                                    Convert.ToBoolean(vincontrolchartselection.isFranchise);
                        model.CarsCom.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                                      Convert.ToBoolean(vincontrolchartselection.isIndependant);
                        model.CarsCom.Options = vincontrolchartselection.options != null
                                                    ? vincontrolchartselection.options.ToLower()
                                                    : "";
                        model.CarsCom.Trims = vincontrolchartselection.trims != null
                                                  ? vincontrolchartselection.trims.ToLower()
                                                  : "";
                    }
                }
            }

            return View("ViewGraphInAppraisal", model);
        }

        private ActionResult InventoryChart(int dealerId, int listingId)
        {
            var model = new ChartSelectionViewModel();
            model.CarsCom = new ChartSelection();
            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var existingDealer = contextVinControl.whitmanenterprisedealerships.FirstOrDefault(i => i.idWhitmanenterpriseDealership == dealerId);
            if (existingDealer != null)
            {
                var dealerViewModel = new DealershipViewModel()
                {
                    DealershipId = existingDealer.idWhitmanenterpriseDealership,
                    DealerGroupId = existingDealer.DealerGroupID,
                    Address = existingDealer.Address,
                    City = existingDealer.City,
                    ZipCode = existingDealer.ZipCode,
                    State = existingDealer.State ?? "CA",
                    Latitude = existingDealer.Lattitude,
                    Longtitude = existingDealer.Longtitude
                };
                SessionHandler.Dealership = dealerViewModel;
                SessionHandler.CanViewBucketJumpReport = null;
                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(existingDealer.idWhitmanenterpriseDealership);

                // Create a Session to identify what the chart screen is
                SessionHandler.ChartScreen = Constanst.Inventory;
            }

            whitmanenterprisedealershipinventory targetCar;
            targetCar = contextVinControl.whitmanenterprisedealershipinventories.FirstOrDefault((x => x.ListingID == listingId));
            if (targetCar == null)
            {
                // check to see if the car is in wholesale
                var wholesalevehicle = contextVinControl.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == listingId);
                if (wholesalevehicle != null)
                {
                    targetCar = new whitmanenterprisedealershipinventory
                                    {
                                        VINNumber = wholesalevehicle.VINNumber,
                                        Trim = wholesalevehicle.Trim,
                                        ModelYear = wholesalevehicle.ModelYear,
                                        Make = wholesalevehicle.Make,
                                        Model = wholesalevehicle.Model,
                                        Certified = wholesalevehicle.Certified,
                                        DealershipName = wholesalevehicle.DealershipName,
                                        DealershipId = wholesalevehicle.DealershipId,
                                        DealershipZipCode = wholesalevehicle.DealershipZipCode,
                                        DealershipState = wholesalevehicle.DealershipState,
                                        DealershipPhone = wholesalevehicle.DealershipPhone,
                                        DealershipCity = wholesalevehicle.DealershipCity,
                                        DealershipAddress = wholesalevehicle.DealershipAddress
                                    };
                }
            }

            if (targetCar == null) return View("ViewGraphOnMobile", model);

            ViewData[Constanst.ListingId] = listingId;

            ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                   targetCar.Trim + " - Mileage : " + targetCar.Mileage + " / Price : " +
                                   targetCar.SalePrice;

            var existingChartSelection = contextVinControl.vincontrolchartselections.Where(s => s.listingId == listingId && s.screen == "Inventory").ToList();
            if (existingChartSelection.Count > 0)
            {
                foreach (var vincontrolchartselection in existingChartSelection)
                {
                    if (vincontrolchartselection.sourceType.Equals(Constanst.AutoTrader))
                    {
                        model.IsAll = vincontrolchartselection.isAll != null &&
                                      Convert.ToBoolean(vincontrolchartselection.isAll);
                        model.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                          Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                        model.IsCertified = vincontrolchartselection.isCertified != null &&
                                            Convert.ToBoolean(vincontrolchartselection.isCertified);
                        model.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                            Convert.ToBoolean(vincontrolchartselection.isFranchise);
                        model.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isIndependant);
                        model.Options = vincontrolchartselection.options != null
                                            ? vincontrolchartselection.options.ToLower()
                                            : "";
                        model.Trims = vincontrolchartselection.trims != null
                                          ? vincontrolchartselection.trims.ToLower()
                                          : "";
                    }

                    if (vincontrolchartselection.sourceType.Equals(Constanst.CarsCom))
                    {
                        model.CarsCom.IsAll = vincontrolchartselection.isAll != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isAll);
                        model.CarsCom.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                                  Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                        model.CarsCom.IsCertified = vincontrolchartselection.isCertified != null &&
                                                    Convert.ToBoolean(vincontrolchartselection.isCertified);
                        model.CarsCom.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                                    Convert.ToBoolean(vincontrolchartselection.isFranchise);
                        model.CarsCom.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                                      Convert.ToBoolean(vincontrolchartselection.isIndependant);
                        model.CarsCom.Options = vincontrolchartselection.options != null
                                                    ? vincontrolchartselection.options.ToLower()
                                                    : "";
                        model.CarsCom.Trims = vincontrolchartselection.trims != null
                                                  ? vincontrolchartselection.trims.ToLower()
                                                  : "";
                    }
                }
            }

            return View("ViewGraphOnMobile", model);
        }

        private ActionResult AppraisalChart(int dealerId, int listingId)
        {
            var model = new ChartSelectionViewModel { CarsCom = new ChartSelection() };
            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var existingDealer = contextVinControl.whitmanenterprisedealerships.FirstOrDefault(i => i.idWhitmanenterpriseDealership == dealerId);
            if (existingDealer != null)
            {
                var dealerViewModel = new DealershipViewModel()
                {
                    DealershipId = existingDealer.idWhitmanenterpriseDealership,
                    DealerGroupId = existingDealer.DealerGroupID,
                    Address = existingDealer.Address,
                    City = existingDealer.City,
                    ZipCode = existingDealer.ZipCode,
                    State = existingDealer.State ?? "CA",
                    Latitude = existingDealer.Lattitude,
                    Longtitude = existingDealer.Longtitude
                };
                SessionHandler.Dealership = dealerViewModel;
                SessionHandler.CanViewBucketJumpReport = null;
                SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(existingDealer.idWhitmanenterpriseDealership);

                // Create a Session to identify what the chart screen is
                SessionHandler.ChartScreen = Constanst.Appraisal;
            }

            var targetCar = contextVinControl.whitmanenterpriseappraisals.FirstOrDefault((x => x.idAppraisal == listingId));

            if (targetCar == null) return View("ViewGraphInAppraisalOnMobile", model);

            ViewData[Constanst.ListingId] = targetCar.idAppraisal.ToString(CultureInfo.InvariantCulture);

            ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " + targetCar.Trim + " - Mileage : " + targetCar.Mileage;

            var existingChartSelection = contextVinControl.vincontrolchartselections.Where(s => s.listingId == listingId && s.screen == Constanst.Appraisal).ToList();
            if (existingChartSelection.Count > 0)
            {
                foreach (var vincontrolchartselection in existingChartSelection)
                {
                    if (vincontrolchartselection.sourceType.Equals(Constanst.AutoTrader))
                    {
                        model.IsAll = vincontrolchartselection.isAll != null &&
                                      Convert.ToBoolean(vincontrolchartselection.isAll);
                        model.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                          Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                        model.IsCertified = vincontrolchartselection.isCertified != null &&
                                            Convert.ToBoolean(vincontrolchartselection.isCertified);
                        model.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                            Convert.ToBoolean(vincontrolchartselection.isFranchise);
                        model.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isIndependant);
                        model.Options = vincontrolchartselection.options != null
                                            ? vincontrolchartselection.options.ToLower()
                                            : "";
                        model.Trims = vincontrolchartselection.trims != null
                                          ? vincontrolchartselection.trims.ToLower()
                                          : "";
                    }

                    if (vincontrolchartselection.sourceType.Equals(Constanst.CarsCom))
                    {
                        model.CarsCom.IsAll = vincontrolchartselection.isAll != null &&
                                              Convert.ToBoolean(vincontrolchartselection.isAll);
                        model.CarsCom.IsCarsCom = vincontrolchartselection.isCarsCom != null &&
                                                  Convert.ToBoolean(vincontrolchartselection.isCarsCom);
                        model.CarsCom.IsCertified = vincontrolchartselection.isCertified != null &&
                                                    Convert.ToBoolean(vincontrolchartselection.isCertified);
                        model.CarsCom.IsFranchise = vincontrolchartselection.isFranchise != null &&
                                                    Convert.ToBoolean(vincontrolchartselection.isFranchise);
                        model.CarsCom.IsIndependant = vincontrolchartselection.isIndependant != null &&
                                                      Convert.ToBoolean(vincontrolchartselection.isIndependant);
                        model.CarsCom.Options = vincontrolchartselection.options != null
                                                    ? vincontrolchartselection.options.ToLower()
                                                    : "";
                        model.CarsCom.Trims = vincontrolchartselection.trims != null
                                                  ? vincontrolchartselection.trims.ToLower()
                                                  : "";
                    }
                }
            }

            return View("ViewGraphInAppraisalOnMobile", model);
        }

        public ActionResult ViewFullChartOnMobile(string token, int listingId, string type)
        {
            int dealerId = 2299;
            SessionHandler.AutoTrader = null;
            try
            {
                dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(token.Replace(" ", "+")).Split('|')[0]);
            }
            catch (Exception) { }

            if (type.ToLower().Equals(Constanst.Appraisal.ToLower()))
            {
                return AppraisalChart(dealerId, listingId);
            }
            else
            {
                return InventoryChart(dealerId, listingId);
            }
        }

        public ActionResult NavigateToLocalMarket(int listingId, bool isCarsCom, string filterOptions)
        {
            return GetLocalData(listingId, isCarsCom,false,filterOptions);
        }

        public ActionResult NavigateToGoogleLocalMarket(int listingId, bool isCarsCom, string filterOptions)
        {
            return GetLocalData(listingId, isCarsCom, true,filterOptions);
        }

        private ActionResult GetLocalData(int listingId, bool isCarsCom, bool isGoogleMap, string filterOptions)
        {
            SessionHandler.CanViewBucketJumpReport = null;
            if (SessionHandler.Dealership == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealership;
            SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

            var model = new ChartSelectionViewModel(){FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null};
            model.CarsCom = new ChartSelection();

            ViewData[Constanst.ListingId] = listingId;
            var contextVinControl = new whitmanenterprisewarehouseEntities();
            var screenType = Constanst.Inventory;
            var actionType = Constanst.ViewGraph;
            if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
            {
                screenType = Constanst.Appraisal;
                actionType = Constanst.ViewGraphInAppraisal;

                var targetCar =
                    contextVinControl.whitmanenterpriseappraisals.FirstOrDefault((x => x.idAppraisal == listingId));
                ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                       targetCar.Trim + " - Mileage : " + targetCar.Mileage + " / Price : " +
                                       targetCar.SalePrice;
            }
            else
            {
                var targetCar =
                    contextVinControl.whitmanenterprisedealershipinventories.FirstOrDefault(
                        (x => x.ListingID == listingId));
                ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                       targetCar.Trim + " - Mileage : " + targetCar.Mileage + " / Price : " +
                                       targetCar.SalePrice;
            }

            GetSavedSelections(listingId, model, contextVinControl, filterOptions, screenType);

            model.IsCarsCom = isCarsCom;
            if(isGoogleMap)
            {
                return View(Constanst.ViewGoogleGraph, model);
            }
            else
            {
                 return View(actionType, model);
            }
           
        }

        public ActionResult NavigateToNationwide(int listingId, bool isCarsCom, string filterOptions)
        {
            return GetNationwideData(listingId, isCarsCom, "ViewGraphNationwide", filterOptions);
        }

        public ActionResult NavigateToGoogleNationwide(int listingId, bool isCarsCom, string filterOptions)
        {
            return GetNationwideData(listingId, isCarsCom, "ViewGoogleGraphNationwide", filterOptions);
        }

        private ActionResult GetNationwideData(int listingId, bool isCarsCom, string viewName, string filterOptions)
        {
            SessionHandler.CanViewBucketJumpReport = null;
            if (SessionHandler.Dealership == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealership;
            SessionHandler.CanViewBucketJumpReport = LinqHelper.CanViewBucketJumpReport(dealer.DealershipId);

            var model = new ChartSelectionViewModel() { FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null };
            model.CarsCom = new ChartSelection();

            ViewData[Constanst.ListingId] = listingId;
            var contextVinControl = new whitmanenterprisewarehouseEntities();
            var screenType = Constanst.Inventory;
            if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
            {
                screenType = Constanst.Appraisal;

                var targetCar =
                    contextVinControl.whitmanenterpriseappraisals.FirstOrDefault((x => x.idAppraisal == listingId));
                ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                       targetCar.Trim + " - Mileage : " + targetCar.Mileage + " / Price : " +
                                       targetCar.SalePrice;
            }
            else
            {
                var targetCar =
                    contextVinControl.whitmanenterprisedealershipinventories.FirstOrDefault(
                        (x => x.ListingID == listingId));
                ViewData[Constanst.CarTitle] = targetCar.ModelYear + " " + targetCar.Make + " " + targetCar.Model + " " +
                                       targetCar.Trim + " - Mileage : " + targetCar.Mileage + " / Price : " +
                                       targetCar.SalePrice;
            }

           
            GetSavedSelections(listingId, model, contextVinControl, filterOptions, screenType);

            model.IsCarsCom = isCarsCom;
            return View(viewName, model);
        }

        public string KeepPDFContentForMarketPriceRangeChange(string content)
        {
            SessionHandler.PDFContentForMarketPriceRangeChange = content;
            return content;
        }

        [ValidateInput(false)]
        [HttpPost]
        public void KeepPDFContentForMarketPriceRangeChange(ChartSelectionViewModel model)
        {
            SessionHandler.PDFContentForMarketPriceRangeChange = model.PdfContent;
        }

        [ValidateInput(false)]
        [HttpPost]
        public string UpdateCarRanking(int listingId, int carRanking, int numberOfCars, int oldCarRanking,
                                       int oldNumberOfCars, bool isCarscom, int smallestPrice, int averagePrice,
                                       int largestPrice)
        {
            var content = String.Empty;
            var car = new MarketCarInfo();
            if (SessionHandler.PDFContentForMarketPriceRangeChange != null)
            {
                content = SessionHandler.PDFContentForMarketPriceRangeChange;
                SessionHandler.PDFContentForMarketPriceRangeChange = null;
            }

            var readyToSendMail = false;
            var dealer = SessionHandler.Dealership;
            try
            {
                // Currently, we only update ranking for AutoTrader
                if (!isCarscom)
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
                        {
                            var existingAppraisal =
                                context.whitmanenterpriseappraisals.FirstOrDefault(i => i.idAppraisal == listingId);
                            if (existingAppraisal != null)
                            {
                                int priceNumber = 0; Int32.TryParse(existingAppraisal.SalePrice, out priceNumber);



                                oldCarRanking = existingAppraisal.CarRanking ?? oldCarRanking;
                                oldNumberOfCars = existingAppraisal.NumberOfCar ?? oldNumberOfCars;

                                existingAppraisal.CarRanking = carRanking;
                                existingAppraisal.NumberOfCar = numberOfCars;
                                existingAppraisal.MarketLowestPrice = smallestPrice;
                                existingAppraisal.MarketAveragePrice = averagePrice;
                                existingAppraisal.MarketHighestPrice = largestPrice;
                                context.SaveChanges();

                                car.Year = existingAppraisal.ModelYear;
                                car.Make = existingAppraisal.Make;
                                car.Model = existingAppraisal.Model;
                                car.Trim = existingAppraisal.Trim;
                                car.Vin = existingAppraisal.VINNumber;

                                car.CurrentPrice = priceNumber;
                                car.AutoTraderStockNo = existingAppraisal.StockNumber;
                                readyToSendMail = true;
                            }
                        }
                        else
                        {
                            var existingInventory =
                                context.whitmanenterprisedealershipinventories.FirstOrDefault(
                                    i => i.ListingID == listingId);
                            if (existingInventory != null && existingInventory.CarRanking != carRanking)
                            {
                                int priceNumber = 0; Int32.TryParse(existingInventory.SalePrice, out priceNumber);


                                oldCarRanking = existingInventory.CarRanking ?? oldCarRanking;
                                oldNumberOfCars = existingInventory.NumberOfCar ?? oldNumberOfCars;

                                existingInventory.CarRanking = carRanking;
                                existingInventory.NumberOfCar = numberOfCars;
                                existingInventory.MarketLowestPrice = smallestPrice;
                                existingInventory.MarketAveragePrice = averagePrice;
                                existingInventory.MarketHighestPrice = largestPrice;
                                context.SaveChanges();

                                car.Year = existingInventory.ModelYear;
                                car.Make = existingInventory.Make;
                                car.Model = existingInventory.Model;
                                car.Trim = existingInventory.Trim;
                                car.Vin = existingInventory.VINNumber;
                                car.CurrentPrice = priceNumber;
                                car.AutoTraderStockNo = existingInventory.StockNumber;
                                readyToSendMail = true;
                            }
                            else
                            {
                                var existingWholesale =
                                    context.vincontrolwholesaleinventories.FirstOrDefault(i => i.ListingID == listingId);
                                if (existingWholesale != null && existingWholesale.CarRanking != carRanking)
                                {
                                    int priceNumber = 0; Int32.TryParse(existingWholesale.SalePrice, out priceNumber);

                                    oldCarRanking = existingWholesale.CarRanking ?? oldCarRanking;
                                    oldNumberOfCars = existingWholesale.NumberOfCar ?? oldNumberOfCars;

                                    existingWholesale.CarRanking = carRanking;
                                    existingWholesale.NumberOfCar = numberOfCars;
                                    existingWholesale.MarketLowestPrice = smallestPrice;
                                    existingWholesale.MarketAveragePrice = averagePrice;
                                    existingWholesale.MarketHighestPrice = largestPrice;
                                    context.SaveChanges();

                                    car.Year = existingWholesale.ModelYear;
                                    car.Make = existingWholesale.Make;
                                    car.Model = existingWholesale.Model;
                                    car.Trim = existingWholesale.Trim;
                                    car.Vin = existingWholesale.VINNumber;
                                    car.CurrentPrice = priceNumber;
                                    car.AutoTraderStockNo = existingWholesale.StockNumber;
                                    readyToSendMail = true;
                                }
                            }
                        }

                        if (readyToSendMail)
                        {
                            var result =
                                from e in context.whitmanenterpriseusersnotifications
                                from et in context.whitmanenterpriseusers
                                where
                                    e.DealershipId == dealer.DealershipId && e.PriceChangeNotification.Value &&
                                    et.Active.Value && e.UserName == et.UserName

                                select new
                                    {
                                        et.Name,
                                        et.UserName,
                                        et.Password,
                                        et.Email,
                                        et.Cellphone,
                                        et.RoleName,
                                        e.PriceChangeNotification
                                    };

                            var contentViewModel = new ContentViewModel
                                {
                                    Text = string.Format("{0}", HttpUtility.HtmlDecode(content)),
                                    DealshipName = dealer.DealershipName
                                };
                            try
                            {
                                string htmlToConvert = PDFHelper.RenderViewAsString("GraphInfo", contentViewModel,
                                                                                    ControllerContext);

                                //// instantiate the HiQPdf HTML to PDF converter
                                var htmlToPdfConverter = new HtmlToPdf();
                                PDFHelper.ConfigureConverter(htmlToPdfConverter);
                                var pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                                PDFController.FormatHeader(pdfDocument, dealer.DealershipName);
                                var file = new MemoryStream(pdfDocument.WriteToMemory());
                                EmailHelper.SendEmail(result.Select(x => x.Email).AsEnumerable(),
                                                      "Market Price Range Change",
                                                      EmailHelper.CreateBodyEmailForUpdateMarketPriceRange(
                                                          dealer.DealershipId,
                                                          listingId, carRanking, numberOfCars,
                                                          oldCarRanking, oldNumberOfCars,
                                                          User.Identity.Name, car), file);
                            }
                            catch (Exception)
                            {
                                // send notification email without attached file
                                EmailHelper.SendEmail(result.Select(x => x.Email).AsEnumerable(),
                                                      "Market Price Range Change",
                                                      EmailHelper.CreateBodyEmailForUpdateMarketPriceRange(
                                                          dealer.DealershipId,
                                                          listingId, carRanking, numberOfCars,
                                                          oldCarRanking, oldNumberOfCars,
                                                          User.Identity.Name, car));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Successfully";
        }

        [HttpPost]
        public string SaveSelections(string listingId, string isCarsCom, string options, string trims,
                                     string isCertified, string isAll, string isFranchise, string isIndependant,
                                     string screen)
        {
            if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
                screen = Constanst.Appraisal;
            try
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var receivedListingId = Convert.ToInt32(listingId);
                    var sourceType = Convert.ToBoolean(isCarsCom) ? Constanst.CarsCom : Constanst.AutoTrader;
                    var existingChartSelection =
                        context.vincontrolchartselections.Where(
                            s => s.listingId == receivedListingId && s.screen == screen && s.sourceType == sourceType)
                               .FirstOrDefault();
                    if (existingChartSelection != null)
                    {
                        existingChartSelection.isAll = Convert.ToBoolean(isAll);
                        existingChartSelection.isCarsCom = Convert.ToBoolean(isCarsCom);
                        existingChartSelection.isCertified = Convert.ToBoolean(isCertified);
                        existingChartSelection.isFranchise = Convert.ToBoolean(isFranchise);
                        existingChartSelection.isIndependant = Convert.ToBoolean(isIndependant);
                        existingChartSelection.options = options.IndexOf(',') > 0
                                                             ? (options.Split(',')[0].Equals("0") ? "0" : options)
                                                             : options.ToLower();
                        existingChartSelection.trims = trims.IndexOf(',') > 0
                                                           ? (trims.Split(',')[0].Equals("0") ? "0" : trims)
                                                           : trims.ToLower();

                        context.SaveChanges();
                    }
                    else
                    {
                        var newSelection = new vincontrolchartselection()
                            {
                                listingId = Convert.ToInt32(listingId),
                                isAll = Convert.ToBoolean(isAll),
                                isCarsCom = Convert.ToBoolean(isCarsCom),
                                isCertified = Convert.ToBoolean(isCertified),
                                isFranchise = Convert.ToBoolean(isFranchise),
                                isIndependant = Convert.ToBoolean(isIndependant),
                                options =
                                    options.IndexOf(',') > 0
                                        ? (options.Split(',')[0].Equals("0") ? "0" : options)
                                        : options,
                                trims = trims.IndexOf(',') > 0 ? (trims.Split(',')[0].Equals("0") ? "0" : trims) : trims,
                                screen = screen,
                                sourceType = sourceType
                            };
                        context.AddTovincontrolchartselections(newSelection);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Your selections have been saved successfully";
        }

        private whitmanenterprisedealershipinventory GetTargetCar(int listingId)
        {
            whitmanenterprisedealershipinventory targetCar;
            using (var contextVinControl = new whitmanenterprisewarehouseEntities())
            {
                targetCar =
                    contextVinControl.whitmanenterprisedealershipinventories.FirstOrDefault(
                        (x => x.ListingID == listingId));

                if (targetCar == null)
                {
                    targetCar = new whitmanenterprisedealershipinventory();
                    // check to see if the car is in wholesale
                    var wholesalevehicle =
                        contextVinControl.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == listingId);
                    if (wholesalevehicle != null)
                    {
                        targetCar.VINNumber = wholesalevehicle.VINNumber;
                        targetCar.Trim = wholesalevehicle.Trim;
                        targetCar.ModelYear = wholesalevehicle.ModelYear;
                        targetCar.Make = wholesalevehicle.Make;
                        targetCar.Model = wholesalevehicle.Model;
                        targetCar.Certified = wholesalevehicle.Certified;
                        targetCar.DealershipName = wholesalevehicle.DealershipName;
                        targetCar.DealershipId = wholesalevehicle.DealershipId;
                        targetCar.DealershipZipCode = wholesalevehicle.DealershipZipCode;
                        targetCar.DealershipState = wholesalevehicle.DealershipState;
                        targetCar.DealershipPhone = wholesalevehicle.DealershipPhone;
                        targetCar.DealershipCity = wholesalevehicle.DealershipCity;
                        targetCar.DealershipAddress = wholesalevehicle.DealershipAddress;

                    }
                    else
                    {
                        var soldOutVehicle =
                            contextVinControl.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(
                                x => x.ListingID == listingId);
                        if (soldOutVehicle != null)
                        {
                            targetCar.VINNumber = soldOutVehicle.VINNumber;
                            targetCar.Trim = soldOutVehicle.Trim;
                            targetCar.ModelYear = soldOutVehicle.ModelYear;
                            targetCar.Make = soldOutVehicle.Make;
                            targetCar.Model = soldOutVehicle.Model;
                            targetCar.Certified = soldOutVehicle.Certified;
                            targetCar.DealershipName = soldOutVehicle.DealershipName;
                            targetCar.DealershipId = soldOutVehicle.DealershipId;
                            targetCar.DealershipZipCode = soldOutVehicle.DealershipZipCode;
                            targetCar.DealershipState = soldOutVehicle.DealershipState;
                            targetCar.DealershipPhone = soldOutVehicle.DealershipPhone;
                            targetCar.DealershipCity = soldOutVehicle.DealershipCity;
                            targetCar.DealershipAddress = soldOutVehicle.DealershipAddress;

                        }
                    }
                }

                return targetCar;
            }
        }

        public JsonResult GetMarketDataByListingWithin100MilesRadius(int listingId)
        {
            var dealer = SessionHandler.Dealership;
            if (dealer == null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var temp = context.whitmanenterprisedealerships.FirstOrDefault(i => i.idWhitmanenterpriseDealership == SessionHandler.DealerId);
                    dealer = new DealershipViewModel() { Latitude = temp.Lattitude, Longtitude = temp.Longtitude, City = temp.City, State = temp.State, Address = temp.Address, ZipCode = temp.ZipCode };
                }
            }

            var chartGraph = LinqHelper.GetMarketCarsOnAutoTraderWithin100MilesRadius(listingId, dealer);

            return new DataContractJsonResult(chartGraph);
        }

        public JsonResult GetMarketDataByListingWholesaleWithin100MilesRadius(int listingId)
        {
            var dealer = SessionHandler.Dealership;

            var chartGraph = LinqHelper.GetMarketCarsOnAutoTraderWithin100MilesRadiusForWholeSaleChart(listingId, dealer);

            return new DataContractJsonResult(chartGraph);
        }

        public JsonResult GetMarketDataWithin100MilesRadius(int appraisalId)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            var chartGraph = LinqHelper.GetMarketCarsOnAutoTraderWithin100MilesRadiusForAppraisalChart(appraisalId,
                                                                                                       dealer);

            return new DataContractJsonResult(chartGraph);

        }

        [HttpPost]
        public void LoadMarketDataByListingFromCarsComIntoSession(int listingId)
        {
            if (SessionHandler.CarsCom == null)
            {
                try
                {
                    //Session["CarsCom"] = CalculateMarketDataByListingFromCarsComWithHttpPost(ListingId);
                    var dealer = SessionHandler.Dealership;
                    SessionHandler.CarsCom = LinqHelper.GetMarketCarsOnCarsComForLocalMarket(listingId, dealer);
                }
                catch (Exception)
                {
                    SessionHandler.CarsCom = null;
                }
            }
        }

        [HttpPost]
        public void LoadMarketDataByAppraisalFromCarsComIntoSession(int listingId)
        {
            if (SessionHandler.CarsCom == null)
            {
                try
                {
                    //Session["CarsCom"] = CalculateMarketDataByListingFromCarsComWithHttpPost(ListingId);
                    var dealer = SessionHandler.Dealership;
                    SessionHandler.CarsCom = LinqHelper.GetMarketCarsOnCarsComForLocalMarketForAppraisal(listingId, dealer);
                }
                catch (Exception)
                {
                    SessionHandler.CarsCom = null;
                }
            }
        }



        [HttpPost]
        public JsonResult GetMarketDataByListingFromCarsComWithHttpPost(int listingId)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealership == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            // load data from Session if have
            if (SessionHandler.CarsCom != null && SessionHandler.CarsCom.ChartModels != null)
            {
                chartGraph = SessionHandler.CarsCom;
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                //chartGraph = CalculateMarketDataByListingFromCarsComWithHttpPost(ListingId);
                var dealer = SessionHandler.Dealership;
                chartGraph = LinqHelper.GetMarketCarsOnCarsComForLocalMarket(listingId, dealer);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }
            if (SessionHandler.CarsCom == null)
                SessionHandler.CarsCom = chartGraph;
            return new DataContractJsonResult(chartGraph);
        }

        [HttpPost]
        public JsonResult GetMarketDataByAppraisalFromCarsComWithHttpPost(int listingId)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealership == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            // load data from Session if have
            if (SessionHandler.CarsCom != null)
            {
                chartGraph = SessionHandler.CarsCom;
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                //chartGraph = CalculateMarketDataByListingFromCarsComWithHttpPost(ListingId);
                var dealer = SessionHandler.Dealership;
                chartGraph = LinqHelper.GetMarketCarsOnCarsComForLocalMarketForAppraisal(listingId, dealer);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }
            if (SessionHandler.CarsCom == null)
                SessionHandler.CarsCom = chartGraph;
            return new DataContractJsonResult(chartGraph);
        }


        [HttpPost]
        public void LoadMarketDataByListingFromAutoTraderIntoSession(int listingId)
        {
            if (SessionHandler.AutoTrader == null)
            {
                try
                {
                    //Session["AutoTrader"] = CalculateMarketDataByListingFromAutoTraderWithHttpPost(ListingId);
                    var dealer = SessionHandler.Dealership;
                    SessionHandler.CarsCom = LinqHelper.GetMarketCarsOnAutoTraderForLocalMarket(listingId, dealer);
                }
                catch (Exception)
                {
                    SessionHandler.AutoTrader = null;
                }
            }
        }

        [HttpPost]
        public void LoadMarketDataByAppraisalFromAutoTraderIntoSession(int listingId)
        {
            if (SessionHandler.AutoTrader == null)
            {
                try
                {
                    //Session["AutoTrader"] = CalculateMarketDataByListingFromAutoTraderWithHttpPost(ListingId);
                    var dealer = SessionHandler.Dealership;
                    SessionHandler.CarsCom = LinqHelper.GetMarketCarsOnAutoTraderForLocalMarketForAppraisal(listingId,
                                                                                                        dealer);
                }
                catch (Exception)
                {
                    SessionHandler.AutoTrader = null;
                }
            }
        }


        [HttpPost]
        public JsonResult GetMarketDataByListingFromAutoTraderWithHttpPost(int listingId)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealership == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            // load data from Session if have
            if (SessionHandler.AutoTrader != null)
            {
                chartGraph = SessionHandler.AutoTrader;
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                //chartGraph = CalculateMarketDataByListingFromAutoTraderWithHttpPost(ListingId);
                var dealer = SessionHandler.Dealership;
                chartGraph = LinqHelper.GetMarketCarsOnAutoTraderForLocalMarket(listingId, dealer);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            // keep chart graph with Session
            if (SessionHandler.AutoTrader == null)
                SessionHandler.AutoTrader = chartGraph;

            return new DataContractJsonResult(chartGraph);
        }

        [HttpPost]
        public JsonResult GetMarketDataByAppraisalFromAutoTraderWithHttpPost(int listingId)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealership == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            // load data from Session if have
            if (SessionHandler.AutoTrader != null)
            {
                chartGraph = SessionHandler.AutoTrader;
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                //chartGraph = CalculateMarketDataByListingFromAutoTraderWithHttpPost(ListingId);
                var dealer = SessionHandler.Dealership;
                chartGraph = LinqHelper.GetMarketCarsOnAutoTraderForLocalMarketForAppraisal(listingId, dealer);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            // keep chart graph with Session
            if (SessionHandler.AutoTrader == null)
                SessionHandler.AutoTrader = chartGraph;

            return new DataContractJsonResult(chartGraph);
        }

        [HttpPost]
        public void LoadMarketDataByListingNationwideFromAutoTraderIntoSession(int listingId)
        {
            if (SessionHandler.AutoTraderNationwide == null)
            {
                try
                {
                    var dealer = SessionHandler.Dealership;
                    if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
                        SessionHandler.AutoTraderNationwide = LinqHelper.GetMarketCarsOnAutoTraderForNationwideMarketForAppraisal(listingId, dealer);
                    else
                        SessionHandler.AutoTraderNationwide = LinqHelper.GetMarketCarsOnAutoTraderForNationwideMarket(listingId, dealer);
                }
                catch (Exception)
                {
                    SessionHandler.AutoTraderNationwide = null;
                }
            }
        }

        [HttpPost]
        public JsonResult GetMarketDataByListingNationwideFromAutoTraderWithHttpPost(int listingId)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealership == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            if (SessionHandler.AutoTraderNationwide != null)
            {
                chartGraph = SessionHandler.AutoTraderNationwide;
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                var dealer = SessionHandler.Dealership;
                if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
                    chartGraph = LinqHelper.GetMarketCarsOnAutoTraderForNationwideMarketForAppraisal(listingId, dealer);
                else
                    chartGraph = LinqHelper.GetMarketCarsOnAutoTraderForNationwideMarket(listingId, dealer);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            if (SessionHandler.AutoTraderNationwide == null)
                SessionHandler.AutoTraderNationwide = chartGraph;

            return new DataContractJsonResult(chartGraph);
        }

        [HttpPost]
        public void LoadMarketDataByListingNationwideFromCarsComIntoSession(int listingId)
        {
            if (SessionHandler.CarsComNationwide == null)
            {
                try
                {
                    var dealer = SessionHandler.Dealership;
                    if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
                        SessionHandler.CarsComNationwide = LinqHelper.GetMarketCarsOnCarsComForNationwideMarketForAppraisal(listingId, dealer);
                    else
                        SessionHandler.CarsComNationwide = LinqHelper.GetMarketCarsOnCarsComForNationwideMarket(listingId, dealer);
                }
                catch (Exception)
                {
                    SessionHandler.CarsComNationwide = null;
                }
            }
        }

        [HttpPost]
        public JsonResult GetMarketDataByListingNationwideFromCarsComWithHttpPost(int listingId)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealership == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            if (SessionHandler.CarsComNationwide != null)
            {
                chartGraph = SessionHandler.CarsComNationwide;
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                var dealer = SessionHandler.Dealership;
                if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
                    chartGraph = LinqHelper.GetMarketCarsOnCarsComForNationwideMarketForAppraisal(listingId, dealer);
                else
                    chartGraph = LinqHelper.GetMarketCarsOnCarsComForNationwideMarket(listingId, dealer);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            if (SessionHandler.CarsComNationwide == null)
                SessionHandler.CarsComNationwide = chartGraph;

            return new DataContractJsonResult(chartGraph);
        }

        public JsonResult GetMarketDataBanner(string tradeId)
        {
            var dealer = SessionHandler.Dealership;

            tradeId = tradeId.Replace(" ", "+");

            int tradeAutoId = Convert.ToInt32(EncryptionHelper.DecryptString(tradeId));

            var list = LinqHelper.GetMarketCarsOnAutoTraderRadiusForBanner(tradeAutoId);

            var chartGraph = new ChartGraph();

            if (list.Any())
            {
                if (list.Count() > 1)
                {

                    chartGraph.Market = new ChartGraph.MarketInfo
                        {
                            MinimumPrice = list.Select(x => x.Price).Min().ToString("c0"),
                            AveragePrice =
                                Math.Round(list.Select(x => x.Price).Average()).ToString("c0"),
                            MaximumPrice = list.Select(x => x.Price).Max().ToString("c0"),
                            MinimumColor =
                                list.First(x => x.Price == list.Select(y => y.Price).Min()).
                                     Color.Exterior,
                            MaximumColor =
                                list.First(x => x.Price == list.Select(y => y.Price).Max()).
                                     Color.Exterior,
                            MinimumMileage =
                                CommonHelper.FormatNumberInThousand(
                                    list.First(x => x.Price == list.Select(y => y.Price).Min()).
                                         Miles.ToString(CultureInfo.InvariantCulture)),
                            AverageMileage =
                                CommonHelper.FormatNumberInThousand(
                                    (Math.Round(list.Select(x => x.Miles).Average())).ToString(
                                        CultureInfo.InvariantCulture)),
                            MaximumMileage =
                                CommonHelper.FormatNumberInThousand(
                                    list.First(x => x.Price == list.Select(y => y.Price).Max()).
                                         Miles.ToString(CultureInfo.InvariantCulture))

                        };
                }
                else
                {
                    chartGraph.Market = new ChartGraph.MarketInfo
                        {
                            MinimumPrice = "NA",
                            AveragePrice = list.FirstOrDefault().Price.ToString("c0"),
                            MaximumPrice = "NA",
                            MinimumColor = "NA",
                            MaximumColor =
                                "NA",
                            MinimumMileage =
                                "NA",
                            AverageMileage =
                                CommonHelper.FormatNumberInThousand(
                                    list.FirstOrDefault().Miles.ToString(CultureInfo.InvariantCulture)),
                            MaximumMileage =
                                "NA",

                        };
                }
            }

            else
            {

                chartGraph.Market = new ChartGraph.MarketInfo
                    {
                        MinimumPrice = "NA",
                        AveragePrice = "NA",
                        MaximumPrice = "NA",
                        MinimumColor = "NA",
                        MaximumColor = "NA",
                        MinimumMileage = "NA",
                        AverageMileage = "NA",
                        MaximumMileage = "NA",

                    };

            }


            return new DataContractJsonResult(chartGraph);

        }




    }
}
