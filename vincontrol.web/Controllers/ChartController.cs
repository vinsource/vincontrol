using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Drawing.Charts;
using vincontrol.Application.Forms.AppraisalManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.ViewModels.Chart;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using System.IO;
using Vincontrol.Web.Security;
using vincontrol.DomainObject;
using vincontrol.CarFax;
using System.Collections.Generic;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Services;
using Inventory = vincontrol.Data.Model.Inventory;
using MarketHelper = vincontrol.Helper.MarketHelper;
using SoldoutInventory = vincontrol.Data.Model.SoldoutInventory;
using Vehicle = vincontrol.Data.Model.Vehicle;
using CarFaxDetailViewModel = vincontrol.Application.ViewModels.CommonManagement.CarFaxDetailViewModel;
using ChartSelection = vincontrol.Application.ViewModels.Chart.ChartSelection;
using VINViewModel = Vincontrol.Web.Models.VINViewModel;

namespace Vincontrol.Web.Controllers
{
    public class ChartController : SecurityController
    {
        
        private ICarFaxService _carFaxService;
        private IInventoryManagementForm _inventoryManagementForm;
        private IAppraisalManagementForm _appraisalManagementForm;
        private IDealerManagementForm _dealerManagementForm;
        readonly IManheimVehicleService _manheimVehicleService;

        public ChartController()
        {
            _manheimVehicleService = new ManheimVehicleService();
            _carFaxService = new CarFaxService();
           _inventoryManagementForm=new InventoryManagementForm();
            _appraisalManagementForm=new AppraisalManagementForm();
            _dealerManagementForm=new DealerManagementForm();
        }

        [VinControlAuthorization(PermissionCode = "MARKETSEARCH", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult MarketSearch()
        {
            var marketFilterSource = new MarketFilterSource()
            {
                Year = ChromeHelper.GetChromeYear()
            };
            
            return View("Newmarket", marketFilterSource);
        }

        [VinControlAuthorization(PermissionCode = "MARKETSEARCH", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult MarketSearchContent()
        {
            ViewData["Year"] = ChromeHelper.GetChromeYear();
            return PartialView("MarketSearch");
        }

        [HttpPost]
        public ActionResult GetMakesFromChrome(int yearFrom, int yearTo)
        {
            var listMake = GetMakeList(yearFrom, yearTo);
            return PartialView(listMake);
        }

        private static List<ExtendedSelectListItem> GetMakeList(int yearFrom, int yearTo)
        {
            List<ExtendedSelectListItem> listMake = VincontrolLinqHelper.GetChromeMake(yearFrom, yearTo);
            listMake.Insert(0, new ExtendedSelectListItem()
            {
                Value = "-1",
                Text = "Select ..."
            });
            return listMake;
        }

        [HttpPost]
        public ActionResult GetModelsFromChrome(int yearFrom, int yearTo, int makeID)
        {
            var listModel = GetModelList(yearFrom, yearTo, makeID);
            return PartialView(listModel);
        }

        private static List<ExtendedSelectListItem> GetModelList(int yearFrom, int yearTo, int makeID)
        {
            List<ExtendedSelectListItem> listModel = VincontrolLinqHelper.GetChromeModel(yearFrom,
                yearTo, makeID);
            listModel.Insert(0, new ExtendedSelectListItem()
            {
                Value = "-1",
                Text = "Select ..."
            });
            return listModel;
        }

        [HttpPost]
        public ActionResult GetTrimsFromChrome(int yearFrom, int yearTo, int makeID, int modelID)
        {
            List<ExtendedSelectListItem> listTrim = VincontrolLinqHelper.GetTrimModel(yearFrom, yearTo, makeID, modelID);

            if (listTrim.Count > 0)
            {
                listTrim.Insert(0, new ExtendedSelectListItem()
                {
                    Value = "-1",
                    Text = "Select ..."
                });
            }

            return PartialView(listTrim);
        }

        public ActionResult ViewCarfax(string url)
        {
            var vinNumber = new VINViewModel { VINNumber = _carFaxService.GetVinNumberFromDetailUrl(url) };
            return View("ViewCARFAX", vinNumber);
        }

        public ActionResult CARFAXDetail(string vin)
        {
            var dealer = SessionHandler.Dealer;
            var item = _carFaxService.ConvertXmlToCarFaxModelAndSave(vin, dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);
            var carFax = new CarFaxDetailViewModel { CarFax = item, CarFaxDealerId = dealer.DealerSetting.CarFax, Vin = vin };
            return PartialView(carFax);
        }

        public ActionResult CarFaxReportFromAutoTrader(string vin)
        {
            var carfaxService = new CarFaxService();
            ViewData["CarFaxReport"] = carfaxService.MakeApiCall(vin, SessionHandler.Dealer.DealerSetting.CarFax, SessionHandler.Dealer.DealerSetting.CarFaxPassword);
            if (ViewData["CarFaxReport"]!=null)
                return Json(SessionHandler.Dealer.DealerSetting.CarFax);
            else
            {
                return Json("Error");
            }
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewFullChart(int listingId, string filterOptions, int currentScreen)
        {
            return GetChartData(listingId, "ViewGraphNationwide", filterOptions, currentScreen);
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewFullChartOnMobile(string token, int listingId, string type)
        {
            int dealerId = 2299;
            SessionHandler.AutoTrader = null;
            try
            {
                dealerId = Convert.ToInt32(vincontrol.Helper.EncryptionHelper.DecryptString(token).Split('|')[0]);
            }
            catch{ }

            return type.ToLower().Equals(Constanst.Appraisal.ToLower())
                ? AppraisalChart(dealerId, listingId)
                : InventoryChart(dealerId, listingId);
                //GetChartData(listingId, "ViewGraphOnMobile", null, Constanst.ScreenType.InventoryScreen);
        }

        public ActionResult MarketStockingGuide(string make, string model)
        {
            ViewData["Year"] = ChromeHelper.GetChromeYear();
            ViewData["Make"] = make;
            ViewData["Model"] = model;
            return View();
        }

        public ActionResult StockingGuideMarketSearch(string make, string model)
        {
            var years = ChromeHelper.GetChromeYear();
            ViewData["Make"] = make;
            ViewData["Model"] = model;
            var makeList = GetMakeList(DateTime.Now.Year - 4, DateTime.Now.Year);
            ViewData["Makes"] = makeList;
            var extendedSelectListItem = makeList.FirstOrDefault(i => i.Text==make);
            var modelList = new List<ExtendedSelectListItem>();
            if (extendedSelectListItem != null)
                modelList = GetModelList(DateTime.Now.Year - 4, DateTime.Now.Year, int.Parse(extendedSelectListItem.Value));

            var marketFilterSource = new MarketFilterSource()
            {
                Year = years,
                Makes = makeList,
                Models = modelList,
                SeletedYearFrom = (DateTime.Now.Year - 4).ToString(CultureInfo.InvariantCulture),
                SeletedYearTo = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
                //SelectedMakeId = extendedSelectListItem.Value,
                //SeletedModelId = singleOrDefault.Value
            };

            if (extendedSelectListItem != null)
                marketFilterSource.SelectedMakeId = extendedSelectListItem.Value;

            var singleOrDefault = modelList.SingleOrDefault(i => i.Text == model);
            if (singleOrDefault != null)
            {
                marketFilterSource.SeletedModelId = singleOrDefault.Value;
            }
            return View(marketFilterSource);
        }

        private ActionResult GetChartData(int listingId, string viewName, string filterOptions, int currentScreen)
        {
            SessionHandler.CanViewBucketJumpReport = null;
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
    
            ChartSelectionViewModel model = currentScreen == Constanst.ScreenType.SoldoutScreen ? GetChartSoldItems(listingId, filterOptions) : GetChartItems(listingId, filterOptions);
            GetSavedSelections(listingId, model, filterOptions, Constanst.VehicleStatus.Inventory);
            if (currentScreen == Constanst.ScreenType.InventoryScreen)
            {
                model.Type = Constanst.VehicleStatus.Inventory; //Constanst.CarInfoType.Used;
                model.InventoryStatus = Constanst.InventoryStatus.Inventory;
            }
            else if (currentScreen == Constanst.ScreenType.SoldoutScreen)
            {
                model.Type = Constanst.VehicleStatus.Inventory;//Constanst.CarInfoType.Sold;
                model.InventoryStatus = Constanst.InventoryStatus.SoldOut;
            }
            else if (currentScreen == Constanst.ScreenType.WholesaleScreen)
            {
                model.Type = Constanst.VehicleStatus.Inventory; //Constanst.CarInfoType.Wholesale;
                model.InventoryStatus = Constanst.InventoryStatus.Wholesale;
            }
            return View(viewName, model);
        }

        private ChartSelectionViewModel GetChartItems(int listingId, string filterOptions)
        {
            SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

         
            var model = new ChartSelectionViewModel
                {
                    FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null,
                    CarsCom = new ChartSelection()
                };

            var targetCar =_inventoryManagementForm.GetInventory(listingId);

            ViewData[Constanst.ListingId] = listingId;

            if (targetCar != null)
            {
                if (targetCar.DateInStock != null)
                {
                    ViewData[Constanst.CarTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                                   targetCar.Vehicle.Model + " " +
                                                   targetCar.Vehicle.Trim + " (Age : " +
                                                   DateTime.Now.Subtract(targetCar.DateInStock.Value).Days +
                                                   ")";
                    ViewData[Constanst.PageChartTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                                         targetCar.Vehicle.Model;
                }
                ViewData[Constanst.CarMileAndPrice] = string.Format("{0} Miles | {1}", targetCar.Mileage != null ? targetCar.Mileage.Value.ToString("#,###") : "0",
                                                                    targetCar.SalePrice != null ? targetCar.SalePrice.Value.ToString("C0") : "0");
                model.CurrentCar = new Car()
                {
                    Year = targetCar.Vehicle.Year,
                    Make = targetCar.Vehicle.Make,
                    Model = targetCar.Vehicle.Model,
                    Trim = targetCar.Vehicle.Trim,
                    IsCertified = targetCar.Certified.GetValueOrDefault(),
                    CertifiedAmount = targetCar.CertifiedAmount.GetValueOrDefault(),
                    ACar = targetCar.ACar.GetValueOrDefault(),
                    MileageAdjustment = targetCar.MileageAdjustment.GetValueOrDefault(),
                    Note = targetCar.Note
                };
            }
            return model;
        }

        private ChartSelectionViewModel GetChartSoldItems(int listingId, string filterOptions)
        {
            SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

      

            var model = new ChartSelectionViewModel
                {
                    FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null,
                    CarsCom = new ChartSelection()
                };


            var targetCar =_inventoryManagementForm.GetSoldInventory(listingId);
            if (targetCar == null)
            {
                targetCar = new SoldoutInventory();
                // check to see if the car is in wholesale
                var wholesalevehicle = _inventoryManagementForm.GetInventory(listingId);
                if (wholesalevehicle != null)
                {
                    targetCar.Vehicle.Vin = wholesalevehicle.Vehicle.Vin;
                    targetCar.Vehicle.Trim = wholesalevehicle.Vehicle.Trim;
                    targetCar.Vehicle.Year = wholesalevehicle.Vehicle.Year;
                    targetCar.Vehicle.Make = wholesalevehicle.Vehicle.Make;
                    targetCar.Vehicle.Model = wholesalevehicle.Vehicle.Model;
                    targetCar.Certified = wholesalevehicle.Certified;
                  
                }
            }

            ViewData[Constanst.ListingId] = listingId;

       
            ViewData[Constanst.CarTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " + targetCar.Vehicle.Model + " " +
                                   targetCar.Vehicle.Trim;
            ViewData[Constanst.PageChartTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                                         targetCar.Vehicle.Model;
            ViewData[Constanst.CarMileAndPrice] = string.Format("{0} Miles | {1}", targetCar.Mileage != null ? targetCar.Mileage.Value.ToString("#,###") : "0",
                                                                targetCar.SalePrice != null ? targetCar.SalePrice.Value.ToString("C0") : "0");
            model.CurrentCar = new Car()
            {
                Year = targetCar.Vehicle.Year,
                Make = targetCar.Vehicle.Make,
                Model = targetCar.Vehicle.Model,
                Trim = targetCar.Vehicle.Trim,
                IsCertified = targetCar.Certified.GetValueOrDefault(),
                CertifiedAmount = targetCar.CertifiedAmount.GetValueOrDefault(),
                ACar = targetCar.ACar.GetValueOrDefault(),
                MileageAdjustment = targetCar.MileageAdjustment.GetValueOrDefault(),
                Note = targetCar.Note
            };
            return model;
        }

        private void GetSavedSelections(int listingId, ChartSelectionViewModel model, string filterOptions,
            short screenType)
        {
            model.Id = listingId;
            if (!String.IsNullOrEmpty(filterOptions))
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(filterOptions)))
                {
                    var serializer = new DataContractJsonSerializer(typeof (SelectionData));
                    var selectionData = (SelectionData) serializer.ReadObject(ms);

                    model.IsCarsCom = selectionData.Source == "carscom";
                    model.IsAll = selectionData.DealerType == "all";
                    model.IsCertified = selectionData.IsCertified;
                    model.IsFranchise = selectionData.DealerType == "franchise";
                    model.IsIndependant = selectionData.DealerType == "independant";
                    model.Options = selectionData.Options;
                    model.Trims = String.Join(",", selectionData.Trims);

                }
            }
            else
            {
                vincontrol.Data.Model.ChartSelection vincontrolchartselection = null;
                
                if(screenType==Constanst.VehicleStatus.Inventory)
                   vincontrolchartselection= _inventoryManagementForm.GetChartSelection(listingId);
                if (screenType == Constanst.VehicleStatus.Appraisal)
                    vincontrolchartselection = _appraisalManagementForm.GetChartSelection(listingId);
                
                if (vincontrolchartselection != null)
                {

                    model.IsAll = vincontrolchartselection.IsAll != null &&
                                  Convert.ToBoolean(vincontrolchartselection.IsAll);
                    model.IsCarsCom = vincontrolchartselection.IsCarsCom != null &&
                                      Convert.ToBoolean(vincontrolchartselection.IsCarsCom);
                    model.IsCertified = vincontrolchartselection.IsCertified;
                    model.IsFranchise = vincontrolchartselection.IsFranchise != null &&
                                        Convert.ToBoolean(vincontrolchartselection.IsFranchise);
                    model.IsIndependant = vincontrolchartselection.IsIndependant != null &&
                                          Convert.ToBoolean(vincontrolchartselection.IsIndependant);
                    model.Options = vincontrolchartselection.Options != null
                        ? vincontrolchartselection.Options.ToLower()
                        : "";
                    model.Trims = vincontrolchartselection.Trims != null
                        ? vincontrolchartselection.Trims.ToLower()
                        : "";


                }
            }

        }



        private ActionResult InventoryChart(int dealerId, int listingId)
        {
            var model = new ChartSelectionViewModel {CarsCom = new ChartSelection()};
            var contextVinControl = new VincontrolEntities();

            var dealerViewModel = _dealerManagementForm.GetDealerById(dealerId);
            if (dealerViewModel != null)
            {

                SessionHandler.Dealer = dealerViewModel;
                SessionHandler.CanViewBucketJumpReport = null;
                SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

            }

            var targetCar = _inventoryManagementForm.GetInventory(listingId);
            if (targetCar == null)
            {
                // check to see if the car is in wholesale
                var wholesalevehicle =
                    contextVinControl.Inventories.FirstOrDefault(
                        x =>
                            x.InventoryId == listingId && x.InventoryStatusCodeId == Constanst.InventoryStatus.Wholesale);
                if (wholesalevehicle != null)
                {
                    targetCar = new Inventory
                    {
                        Vehicle = new Vehicle
                        {
                            Vin = wholesalevehicle.Vehicle.Vin,
                            Trim = wholesalevehicle.Vehicle.Trim,
                            Year = wholesalevehicle.Vehicle.Year,
                            Make = wholesalevehicle.Vehicle.Make,
                            Model = wholesalevehicle.Vehicle.Model,
                        },

                        Certified = wholesalevehicle.Certified,

                    };
                }
            }

            if (targetCar == null) return View("ViewGraphOnMobile", model);

            model.CurrentCar = new Car()
            {
                Year = targetCar.Vehicle.Year,
                Make = targetCar.Vehicle.Make,
                Model = targetCar.Vehicle.Model,
                Trim = targetCar.Vehicle.Trim
            };
            ViewData[Constanst.ListingId] = listingId;


            if (targetCar.DateInStock != null)
            {
                ViewData[Constanst.CarTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                               targetCar.Vehicle.Model + " " +
                                               targetCar.Vehicle.Trim + " (Age : " +
                                               DateTime.Now.Subtract(targetCar.DateInStock.Value).Days +
                                               ")";
                ViewData[Constanst.PageChartTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                                     targetCar.Vehicle.Model;
            }
            ViewData[Constanst.CarMileAndPrice] = string.Format("{0} Miles | {1}",
                targetCar.Mileage != null ? targetCar.Mileage.Value.ToString("#,###") : "0",
                targetCar.SalePrice != null ? targetCar.SalePrice.Value.ToString("C0") : "0");

            var vincontrolchartselection = _inventoryManagementForm.GetChartSelection(listingId);
            if (vincontrolchartselection != null)
            {

                model.IsAll = vincontrolchartselection.IsAll != null &&
                              Convert.ToBoolean(vincontrolchartselection.IsAll);
                model.IsCarsCom = vincontrolchartselection.IsCarsCom != null &&
                                  Convert.ToBoolean(vincontrolchartselection.IsCarsCom);
                model.IsCertified = vincontrolchartselection.IsCertified != null &&
                                    Convert.ToBoolean(vincontrolchartselection.IsCertified);
                model.IsFranchise = vincontrolchartselection.IsFranchise != null &&
                                    Convert.ToBoolean(vincontrolchartselection.IsFranchise);
                model.IsIndependant = vincontrolchartselection.IsIndependant != null &&
                                      Convert.ToBoolean(vincontrolchartselection.IsIndependant);
                model.Options = vincontrolchartselection.Options != null
                    ? vincontrolchartselection.Options.ToLower()
                    : "";
                model.Trims = vincontrolchartselection.Trims != null
                    ? vincontrolchartselection.Trims.ToLower()
                    : "";


            }

            return View("ViewGraphOnMobile", model);
        }

        private ActionResult AppraisalChart(int dealerId, int listingId)
        {
            var model = new ChartSelectionViewModel { CarsCom = new ChartSelection() };
            var contextVinControl = new VincontrolEntities();

            var dealerViewModel = _dealerManagementForm.GetDealerById(dealerId);
            if (dealerViewModel != null)
            {
             
                SessionHandler.Dealer = dealerViewModel;
                SessionHandler.CanViewBucketJumpReport = null;
                SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

            }

            var targetCar = _appraisalManagementForm.GetAppraisal(listingId);

            if (targetCar == null) return View("ViewGraphOnMobile", model);

            model.CurrentCar = new Car()
            {
                Year = targetCar.Vehicle.Year,
                Make = targetCar.Vehicle.Make,
                Model = targetCar.Vehicle.Model,
                Trim =  targetCar.Vehicle.Trim
            };


            ViewData[Constanst.ListingId] = targetCar.AppraisalId.ToString(CultureInfo.InvariantCulture);

            ViewData[Constanst.CarTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " + targetCar.Vehicle.Model + " " +
                                   targetCar.Vehicle.Trim;
            ViewData[Constanst.PageChartTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                                         targetCar.Vehicle.Model;
            ViewData[Constanst.CarMileAndPrice] = string.Format("{0} Miles | {1}", targetCar.Mileage != null ? targetCar.Mileage.Value.ToString("#,###") : "0",
                                                                targetCar.SalePrice != null ? targetCar.SalePrice.Value.ToString("C0") : "0");

            var vincontrolchartselection = _appraisalManagementForm.GetChartSelection(listingId);
            if (vincontrolchartselection != null)
            {

                model.IsAll = vincontrolchartselection.IsAll != null &&
                              Convert.ToBoolean(vincontrolchartselection.IsAll);
                model.IsCarsCom = vincontrolchartselection.IsCarsCom != null &&
                                  Convert.ToBoolean(vincontrolchartselection.IsCarsCom);
                model.IsCertified = vincontrolchartselection.IsCertified != null &&
                                    Convert.ToBoolean(vincontrolchartselection.IsCertified);
                model.IsFranchise = vincontrolchartselection.IsFranchise != null &&
                                    Convert.ToBoolean(vincontrolchartselection.IsFranchise);
                model.IsIndependant = vincontrolchartselection.IsIndependant != null &&
                                      Convert.ToBoolean(vincontrolchartselection.IsIndependant);
                model.Options = vincontrolchartselection.Options != null
                    ? vincontrolchartselection.Options.ToLower()
                    : "";
                model.Trims = vincontrolchartselection.Trims != null
                    ? vincontrolchartselection.Trims.ToLower()
                    : "";

            }

            return View("ViewGraphOnMobile", model);
        }

        public ActionResult NavigateToNationwide(int listingId, string filterOptions)
        {
            var appraisal = _appraisalManagementForm.GetAppraisal(listingId);

            if (SessionHelper.AllowToAccessAppraisal(appraisal) == false)
            {
                return RedirectToAction("Unauthorized", "Security");
            }
            return GetNationwideData(listingId, "ViewGraphNationwide", filterOptions);
        }



        private ActionResult GetNationwideData(int listingId, string viewName, string filterOptions)
        {
            SessionHandler.CanViewBucketJumpReport = null;
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

            var model = new ChartSelectionViewModel
                {
                    FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null,
                    CarsCom = new ChartSelection()
                };

            ViewData[Constanst.ListingId] = listingId;


            const short screenType = Constanst.VehicleStatus.Appraisal;

            var targetCar = _appraisalManagementForm.GetAppraisal(listingId);

            if (targetCar != null)
            {
              
                ViewData[Constanst.PageChartTitle] = targetCar.Vehicle.Year + " " + targetCar.Vehicle.Make + " " +
                                                     targetCar.Vehicle.Model;
                ViewData[Constanst.CarMileAndPrice] = string.Format("{0} Miles | {1}", targetCar.Mileage != null ? targetCar.Mileage.Value.ToString("#,###") : "0",
                                                                    targetCar.ACV != null ? targetCar.ACV.Value.ToString("C0") : "0");

                model.CurrentCar = new Car()
                {
                    Make = targetCar.Vehicle.Make,
                    Model = targetCar.Vehicle.Model,
                    Year = targetCar.Vehicle.Year,
                    IsCertified = targetCar.Certified.GetValueOrDefault(),
                    CertifiedAmount = targetCar.CertifiedAmount.GetValueOrDefault(),
                    ACar = targetCar.ACar.GetValueOrDefault(),
                    MileageAdjustment = targetCar.MileageAdjustment.GetValueOrDefault(),
                    Note = targetCar.Note
                };
            }


            GetSavedSelections(listingId, model, filterOptions, screenType);
            model.Type = Constanst.VehicleStatus.Appraisal;//Constanst.CarInfoType.Appraisal;

            return View(viewName, model);
        }


        [ValidateInput(false)]
        [HttpPost]
        public string UpdateCarRanking(int listingId, int carRanking, int numberOfCars, int oldCarRanking,
            int oldNumberOfCars,  int smallestPrice, int averagePrice,int largestPrice, short screen)
        {



            try
            {

                using (var context = new VincontrolEntities())
                {
                    if (screen == Constanst.VehicleStatus.Appraisal)
                    {
                        var existingAppraisal =
                            context.Appraisals.FirstOrDefault(i => i.AppraisalId == listingId);
                        if (existingAppraisal != null)
                        {

                            existingAppraisal.CarRanking = carRanking;
                            existingAppraisal.NumberOfCar = numberOfCars;
                            existingAppraisal.Vehicle.MarketLowestPrice = smallestPrice;
                            existingAppraisal.Vehicle.MarketAveragePrice = averagePrice;
                            existingAppraisal.Vehicle.MarketHighestPrice = largestPrice;
                            context.SaveChanges();


                        }
                    }
                    else
                    {
                        var existingInventory = context.Inventories.FirstOrDefault(i => i.InventoryId == listingId);
                        if (existingInventory != null && existingInventory.CarRanking != carRanking)
                        {
                            existingInventory.CarRanking = carRanking;
                            existingInventory.NumberOfCar = numberOfCars;
                            existingInventory.Vehicle.MarketLowestPrice = smallestPrice;
                            existingInventory.Vehicle.MarketAveragePrice = averagePrice;
                            existingInventory.Vehicle.MarketHighestPrice = largestPrice;
                            context.SaveChanges();
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
                                     bool? isCertified, string isAll, string isFranchise, string isIndependant,
                                     short screen)
        {

            try
            {
                var receivedListingId = Convert.ToInt32(listingId);
                if (screen == Constanst.VehicleStatus.Inventory)
                {
                   
                    _inventoryManagementForm.UpdateChartSelection(receivedListingId, isCarsCom, options, trims,
                        isCertified, isAll, isFranchise, isIndependant);
                }
                if (screen == Constanst.VehicleStatus.Appraisal)
                {
                    
                    _appraisalManagementForm.UpdateChartSelection(receivedListingId, isCarsCom, options, trims,
                        isCertified, isAll, isFranchise, isIndependant);
                }

                SessionHandler.Nationwide = null;
                SessionHandler.SoldNationwide = null;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Your selections have been saved successfully";
        }

        [HttpPost]
        public string SaveSmallSelections(string listingId, string trims, short screen)
        {
         
            try
            {
                var receivedListingId = Convert.ToInt32(listingId);
                if (screen == Constanst.VehicleStatus.Inventory)
                {
                 
                    _inventoryManagementForm.UpdateSmallChartSelection(receivedListingId,trims);
                }
                if (screen == Constanst.VehicleStatus.Appraisal)
                {

                    _appraisalManagementForm.UpdateSmallChartSelection(receivedListingId, trims);
                }


                SessionHandler.Nationwide = null;
                SessionHandler.SoldNationwide = null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Your selections have been saved successfully";
        }



        public string GetSelectedTrims(int listingId)
        {

            var existingChartSelection = _inventoryManagementForm.GetChartSelection(listingId);

            if (existingChartSelection != null)
                return existingChartSelection.Trims ?? String.Empty;
            return String.Empty;

        }

        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public JsonResult GetMarketDataByListingNationwideWithHttpPost(int listingId, short screen)
        {
            if (screen == Constanst.VehicleStatus.Appraisal)
            {
                var chartGraph = MarketDataByListingNationwideWithHttpPost(listingId, SessionHandler.AppraisalNationwide, screen, false);
                if (SessionHandler.AppraisalNationwide == null)
                    SessionHandler.AppraisalNationwide = chartGraph;
                return new DataContractJsonResult(chartGraph);
            }
            else
            {
                var chartGraph = MarketDataByListingNationwideWithHttpPost(listingId, SessionHandler.Nationwide, screen, false);
                if (SessionHandler.Nationwide == null)
                    SessionHandler.Nationwide = chartGraph;
                return new DataContractJsonResult(chartGraph);
            }
        }


        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public JsonResult GetMarketDataByListingNationwideWithHttpPostByYear(int yearFrom, int yearTo, string make, string model, string trim)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealer == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                var dealer = SessionHandler.Dealer;

                chartGraph = MarketHelper.GetChartDataWithYearRange(yearFrom, yearTo, make, model, trim, dealer, false);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            return new DataContractJsonResult(chartGraph);
        }

        private static ChartGraph MarketDataByListingNationwideWithHttpPost(int listingId, ChartGraph data, short screen, bool isSold)
        {
            var chartGraph = new ChartGraph();
            var dealer = SessionHandler.Dealer;
            if (SessionHandler.Dealer == null) return chartGraph;
            
            try
            {
                chartGraph = Constanst.VehicleStatus.Appraisal == screen
                    ? MarketHelper.GetMarketCarsForNationwideMarketForAppraisalOnChart(listingId, dealer, isSold)
                    : MarketHelper.GetMarketCarsForNationwideMarket(listingId, dealer, isSold);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            return chartGraph;
        }

        public JsonResult GetSoldInfoJsonResult(int? stateDistance, int year, string make, string model, string trim, string trimList)
        {
            return new DataContractJsonResult(
             MarketHelper.GetSoldCarWithin90DaysPeriod(stateDistance, year, make, model, trim, SessionHandler.Dealer,
                 new vincontrol.DomainObject.ChartSelection() { Trims = trimList })
             );

        }

        [HttpPost]
        public JsonResult GetMarketDataByListingNationwideWithHttpPostBySold(int listingId, short screen)
        {
            if (screen == Constanst.VehicleStatus.Appraisal)
            {
                var chartGraph = MarketDataByListingNationwideWithHttpPost(listingId, SessionHandler.AppraisalSoldNationwide, screen, true);
                if (SessionHandler.AppraisalSoldNationwide == null)
                    SessionHandler.AppraisalSoldNationwide = chartGraph;

                return new DataContractJsonResult(chartGraph);
            }
            else
            {
                var chartGraph = MarketDataByListingNationwideWithHttpPost(listingId, SessionHandler.SoldNationwide, screen, true);
                if (SessionHandler.SoldNationwide == null)
                    SessionHandler.SoldNationwide = chartGraph;

                return new DataContractJsonResult(chartGraph);

            }
        }



        [HttpPost]
        public JsonResult GetMarketDataByListingNationwideWithHttpPostByYear100Mies(int yearFrom, int yearTo, string make, string model, bool isFirstTime)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealer == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                var dealer = SessionHandler.Dealer;

                if (isFirstTime)
                {
                    if (SessionHandler.StockingGuideChart != null)
                        chartGraph = SessionHandler.StockingGuideChart;
                    else
                        chartGraph = MarketHelper.GetChartDataWithYearRange(yearFrom, yearTo, make, model,string.Empty,
                                                                                         dealer, false);
                }
                else
                {
                    chartGraph = MarketHelper.GetChartDataWithYearRange(yearFrom, yearTo, make, model,string.Empty,
                                                                                         dealer, false);
                }
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            return new DataContractJsonResult(chartGraph);
        }

        [HttpPost]
        public JsonResult GetWishListMarketDataByListingNationwideWithHttpPostByYear100Mies(int yearFrom, int yearTo, string make, string model)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealer == null)
            {
                return new DataContractJsonResult(chartGraph);
            }

            try
            {
                var dealer = SessionHandler.Dealer;
                chartGraph = MarketHelper.GetChartDataWithYearRange(yearFrom, yearTo, make, model,string.Empty,
                                                                                 dealer, false);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            var wishListDetailInfo = new WishListMarketDetailInfo();
            var carInfos = new List<WishlistMarketCarInfo>();
            if (chartGraph.ChartModels != null && chartGraph.ChartModels.Count > 0)
            {
                foreach (var item in chartGraph.ChartModels)
                {
                    var carInfo = new WishlistMarketCarInfo
                    {
                        Year = item[2].ToString(),
                        Make = item[3].ToString(),
                        Model = item[4].ToString(),
                        Trim = item[5].ToString(),
                        SellerAddress = item[18].ToString(),
                        Seller = item[19].ToString(),
                        Distance = item[24].ToString(),
                        Price = item[9].ToString(),
                        Mileage = item[8].ToString(),
                        Age = string.Format("{0} day(s)", item[23].ToString()),
                        Vin = item[1].ToString().Substring(8,8),
                        ExteriorColor = item[7].ToString()
                    };

                    carInfos.Add(carInfo);
                }
                carInfos = carInfos.OrderByDescending(x => x.Year).ToList();
            }

            wishListDetailInfo.ListCarInfo = carInfos;
            if (carInfos.Count > 0)
            {
                wishListDetailInfo.Highest = carInfos.Max(x => Convert.ToDecimal(x.Price)).ToString();
                wishListDetailInfo.Lowest = carInfos.Min(x => Convert.ToDecimal(x.Price)).ToString();
                wishListDetailInfo.Average = carInfos.Average(x => Convert.ToDecimal(x.Price)).ToString();
            }
            else
            {
                wishListDetailInfo.Highest = "0";
                wishListDetailInfo.Lowest = "0";
                wishListDetailInfo.Average = "0";
            }

            return new DataContractJsonResult(wishListDetailInfo);
        }

        [HttpPost]
        public JsonResult GetWishListAuctionDataByListingNationwideWithHttpPostByYear100Mies(int yearFrom, int yearTo, string make, string model)
        {
            var chartGraph = new ChartGraph();

            if (SessionHandler.Dealer == null)
            {
                return new DataContractJsonResult(chartGraph);
            }


            var dealer = SessionHandler.Dealer;
            var manheims =
                _manheimVehicleService.GetManheimWithModel(make, model)
                                      .Where(x => x.Year >= yearFrom && x.Year <= yearTo)
                                      .ToList();

            WishListDetailInfo wishListDetailInfo = new WishListDetailInfo();
            List<CarInfo> CarInfos = new List<CarInfo>();
            if (manheims != null && manheims.Count > 0)
            {
                foreach (var item in manheims)
                {
                    CarInfo carInfo = new CarInfo();
                    carInfo.Year = item.Year.ToString();
                    carInfo.Make = item.Make;
                    carInfo.Model = item.Model;
                    carInfo.Trim = item.Trim;
                    carInfo.Seller = item.Seller;
                    carInfo.Distance = string.Empty;
                    carInfo.Price = string.Empty;
                    carInfo.Mileage = item.Mileage.ToString();
                    carInfo.Age = string.Empty;

                    CarInfos.Add(carInfo);
                }
                CarInfos = CarInfos.OrderByDescending(x => x.Year).ToList();
            }
            wishListDetailInfo.ListCarInfo = CarInfos;
          
            return new DataContractJsonResult(wishListDetailInfo);
        }

        public ActionResult ManheimTransaction(int listingId, short vehicleStatus, short auctionRegion, int pageIndex = 1, int pageSize = 10)
        {
            var manheimTransactions = MarketHelper.GetManheimTransaction(listingId,vehicleStatus, auctionRegion, SessionHandler.Dealer);
            var manheimReportViewModel = new ManheimReport()
            {
                IsAuction = true,
                ManheimTransactions = manheimTransactions,
                Region = string.Empty,                
                NumberOfTransactions = manheimTransactions.Count
            };

            ViewData["LISTINGID"] = listingId;
            ViewData["REGION"] = auctionRegion;
            ViewData["VEHICLESTATUS"] = vehicleStatus;
            SessionHandler.ManheimPastAuctionsReport = manheimReportViewModel;
            return View("ManheimReportWindow", manheimReportViewModel);
        }

        public ActionResult MoreDistance()
        {
            return View();
        }

        public JsonResult GetJsonData()
        {
            var array = new ArrayList();
            array.Add("test");
            array.Add(1);
            array.Add(true);
            return Json(array, JsonRequestBehavior.AllowGet);
        }

    }

    public class CarInfo
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Seller { get; set; }
        public string SellerAddress { get; set; }
        public string Distance { get; set; }
        public string Age { get; set; }
        public string Price { get; set; }
        public string Mileage { get; set; }
    }

    public class WishlistMarketCarInfo : CarInfo
    {
        public string Vin { get; set; }
        //public string Stock { get; set; }
        public string ExteriorColor { get; set; }
    }

    public class WishListMarketDetailInfo
    {
        public List<WishlistMarketCarInfo> ListCarInfo { get; set; }
        public string Highest { get; set; }
        public string Average { get; set; }
        public string Lowest { get; set; }
    }

    public class WishListDetailInfo
    {
        public List<CarInfo> ListCarInfo { get; set; }
        public string Highest { get; set; }
        public string Average { get; set; }
        public string Lowest { get; set; }
    }
}
