using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.VehicleLogManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Manheim;
using vincontrol.Helper;

namespace Vincontrol.Web.Controllers
{
    public class ReportController : SecurityController
    {
        private const string PermissionCode = "REPORT";
        private const string AcceptedValues = "ALLACCESS";
        private IDealerManagementForm _dealerManagementForm;
        private IInventoryManagementForm _inventoryManagementForm;
        private IVehicleLogManagementForm _vehicleLogManagementForm;
        public ReportController()
        {
            _dealerManagementForm = new DealerManagementForm();
            _inventoryManagementForm=new InventoryManagementForm();
            _vehicleLogManagementForm=new VehicleLogManagementForm();
        }
        
        public ActionResult AppraisalPrintOption()
        {
            return View("AppraisalPrintOption");
        }

        public ActionResult PrintOption(int condition)
        {
            if (condition == Constanst.CarInfoType.Used)
                return View("PrintOption");
            else
            {
                return View("PrintOptionForNew");
            }
        }

        public ActionResult ViewPriceChangeReport()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/PriceTrackingReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewFullInventoryBuyerGuide()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string htmlToConvert = BuyerGuideHelper.BuildBuyerGuideInHTML(ControllerContext);

            byte[] byteStream = PDFHelper.GeneratePdfFromHtmlCode(htmlToConvert);

            return new FileStreamResult(new MemoryStream(byteStream), "application/pdf");


        }

        public ActionResult ViewFullInventoryBuyerGuideInSpanish()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string htmlToConvert = BuyerGuideHelper.BuildBuyerGuideInHTMLInSpanish(ControllerContext);

            byte[] byteStream = PDFHelper.GeneratePdfFromHtmlCode(htmlToConvert);

            return new FileStreamResult(new MemoryStream(byteStream), "application/pdf");


        }

        public ActionResult ViewTestSticker(string url)
        {
            var workStream = new MemoryStream();

            if (!String.IsNullOrEmpty(url))
            {
                var byteInfo = PDFHelper.GeneratePDFFromHtml(url);

                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

            }


            return new FileStreamResult(workStream, "application/pdf");
        }

        public ActionResult ManheimTransactionDetail(string year, string make, string model, string trim, string region = "NA", int pageIndex = 0, int pageSize = 0)
        {
            ViewData["ManheimYear"] = year;
            ViewData["ManheimMake"] = make;
            ViewData["ManheimModel"] = model;
            ViewData["ManheimTrim"] = trim;
            ViewData["ManheimRegion"] = region;

            ViewData["ManheimPageSize"] = pageSize;
            ViewData["ManheimPageIndex"] = pageIndex;
            try
            {
                var manheimService = new ManheimService();
                manheimService.Execute("US", year, make, model, trim, region, pageIndex, pageSize);
                ViewData["ManheimNumberOfTransactions"] = manheimService.NumberOfManheimTransactions;
                ViewData["HighPrice"] = manheimService.HighPrice;
                ViewData["LowPrice"] = manheimService.LowPrice;
                ViewData["AvgPrice"] = manheimService.AveragePrice;
                ViewData["AvgOdo"] = manheimService.AverageOdometer;

                var manheimReportViewModel = new ManheimReport()
                {
                    ManheimTransactions = manheimService.ManheimTransactions,
                    Region = region,
                    HighestPrice = manheimService.HighPrice,
                    LowestPrice = manheimService.LowPrice,
                    AveragePrice = manheimService.AveragePrice,
                    AverageOdometer = manheimService.AverageOdometer,
                    NumberOfTransactions = manheimService.NumberOfManheimTransactions
                };

                SessionHandler.ManheimTransactionsReport = manheimReportViewModel;
                return View("ManheimReportWindow", manheimReportViewModel);
            }
            catch (Exception)
            {
                return View("ManheimReportWindow", new ManheimReport());
            }

            
        }

        [HttpPost]
        public ActionResult ManheimReportDetail(string year, string make, string model, string trim, string region, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
               
                var manheimService = new ManheimService();
                manheimService.Execute("US", year, make, model, trim, region, pageIndex, pageSize);
                return View("ManheimReportDetail", manheimService.ManheimTransactions);
            }
            catch (Exception) { }

            return PartialView("ManheimReportDetail", new List<ManheimTransactionViewModel>());
        }

        public ActionResult ViewBuyerGuide(int ListingId)
        {
            var viewModel = new BuyerGuideViewModel()
                                {

                                    ListingId = ListingId,
                                    Languages = SelectListHelper.InitalLanguagesList(),
                                };

            return View("BuyerGuideWindowOptionNew", viewModel);
        }

        public ActionResult ViewBuyerGuideWithoutPopUp(int listingId, short inventoryStatus)
        {
            if (!SessionHandler.UserRight.Inventory.ViewProfile_BG)
            {
                return RedirectToAction("Unauthorized", "Security");
            }


            if (SessionHandler.Dealer != null)
            {
                var dealer = (DealershipViewModel)SessionHandler.Dealer;
                var context = new VincontrolEntities();

                var row = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                var carInfoModel = new CarInfoFormViewModel
                                    {
                                        Type = Constanst.CarInfoType.New,
                                        ButtonPermissions = SQLHelper.GetButtonList(dealer.DealershipId, Constanst.ProfileButton.BucketJumpTracking)
                                    };

                if (row == null && SessionHandler.CurrentView == CurrentViewEnum.SoldInventory.ToString())
                {
                    var soldOut = context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                    if (soldOut != null)
                    {
                        row = new Inventory(soldOut, Constanst.InventoryStatus.SoldOut);
                        carInfoModel.Type = Constanst.CarInfoType.Sold;
                    }
                }

                var viewModel = new BuyerGuideViewModel()
                                    {
                                        CarInfoFormViewModel = carInfoModel,
                                        ListingId = listingId,
                                        Year = row.Vehicle.Year.GetValueOrDefault(),
                                        Make = row.Vehicle.Make,
                                        Model = row.Vehicle.Model,
                                        Trim = row.Vehicle.Trim,
                                        Languages = SelectListHelper.InitalLanguagesList(),
                                        InventoryStatus = inventoryStatus,
                                        Type = carInfoModel.Type
                                        
                                    };

                ViewData["detailType"] = "BG_Tab";
                return View("BuyerGuideWindowOptionNewWithoutPopUp", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult CreateBuyerGuide(string type)
        {
            var viewModel = new AdminBuyerGuideViewModel();
            int typeNumber;
            Int32.TryParse(type, out typeNumber);

            viewModel.WarrantyType = typeNumber;
            viewModel.Message = "";

            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;


            var buyerGuideSetting = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                Constanst.LanguageVersion.English, typeNumber);

            if (buyerGuideSetting != null)
            {
                viewModel.Id = buyerGuideSetting.BuyerGuideId;
                viewModel.Vin = buyerGuideSetting.Vin;
                viewModel.Year = buyerGuideSetting.Year ?? 0;
                viewModel.Make = buyerGuideSetting.Make;
                viewModel.VehicleModel = buyerGuideSetting.Model;
                viewModel.StockNumber = buyerGuideSetting.Stock;
                viewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                viewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                viewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                viewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                viewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                viewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                viewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                viewModel.SystemCovered = buyerGuideSetting.SystemCovered;
                viewModel.Durations = buyerGuideSetting.Durations;
                viewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                viewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                viewModel.PriorRental = buyerGuideSetting.PriorRental;
                viewModel.SystemCoveredAndDurations = buyerGuideSetting.SystemCoveredAndDurations;
                viewModel.IsPriorRental = false;
                viewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                viewModel.SelectedLanguage = buyerGuideSetting.LanguageVersion.GetValueOrDefault();
                viewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                viewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                viewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
            }


            return View(viewModel);
        }

        public ActionResult CreateBuyerGuideSpanish(string type)
        {
            var viewModel = new AdminBuyerGuideViewModel();
            int typeNumber;
            Int32.TryParse(type, out typeNumber);

            viewModel.WarrantyType = typeNumber;
            viewModel.Message = "";

            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;

            var buyerGuideSetting = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                Constanst.LanguageVersion.Spanish, typeNumber);

            if (buyerGuideSetting != null)
            {
                viewModel.Id = buyerGuideSetting.BuyerGuideId;
                viewModel.Vin = buyerGuideSetting.Vin;
                viewModel.Year = buyerGuideSetting.Year ?? 0;
                viewModel.Make = buyerGuideSetting.Make;
                viewModel.VehicleModel = buyerGuideSetting.Model;
                viewModel.StockNumber = buyerGuideSetting.Stock;
                viewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                viewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                viewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                viewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                viewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                viewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                viewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                viewModel.SystemCovered = buyerGuideSetting.SystemCovered;
                viewModel.Durations = buyerGuideSetting.Durations;
                viewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                viewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                viewModel.PriorRental = buyerGuideSetting.PriorRental;
                viewModel.SystemCoveredAndDurations = buyerGuideSetting.SystemCoveredAndDurations;
                viewModel.IsPriorRental = false;
                viewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                viewModel.SelectedLanguage = buyerGuideSetting.LanguageVersion.GetValueOrDefault();
                viewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                viewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                viewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
            }


            return View(viewModel);
        }

        private AdminBuyerGuideViewModel BuildAdminBuyerGuideViewModel()
        {
            return new AdminBuyerGuideViewModel();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateBuyerGuide(AdminBuyerGuideViewModel viewModel)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;
            viewModel.Message = "";

            if (viewModel.IsPreview)
            {
                viewModel.SelectedLanguage = 1;
                viewModel.SystemCovered = ReplaceFontSizeForBuyerGuideReport(viewModel.SystemCovered);
                viewModel.Durations = ReplaceFontSizeForBuyerGuideReport(viewModel.Durations);
                viewModel.SystemCoveredAndDurations =
                    ReplaceFontSizeForBuyerGuideReport(viewModel.SystemCoveredAndDurations);
                Session["BuyerGuideSetting"] = viewModel;
                return RedirectToAction("PrintBuyerGuideSetting", "PDF");
            }
            try
            {
                var buyerGuide = new BuyerGuide()
                {
                    DealerId = dealer.DealershipId,
                    Vin = viewModel.Vin,
                    Year = viewModel.Year,
                    Make = viewModel.Make,
                    Model = viewModel.VehicleModel,
                    Stock = viewModel.StockNumber,
                    WarrantyType = viewModel.WarrantyType,
                    IsServiceContract = viewModel.IsServiceContract,
                    IsAsWarranty = viewModel.IsAsWarranty,
                    IsWarranty = viewModel.IsWarranty,
                    IsFullWarranty = viewModel.IsFullWarranty,
                    IsLimitedWarranty = viewModel.IsLimitedWarranty,
                    SystemCovered = viewModel.SystemCovered,
                    Durations = viewModel.Durations,
                    PercentageOfLabor = viewModel.PercentageOfLabor,
                    PercentageOfPart = viewModel.PercentageOfPart,
                    PriorRental = viewModel.PriorRental,
                    IsMixed = viewModel.IsMixed,
                    SystemCoveredAndDurations = viewModel.SystemCoveredAndDurations,
                    LanguageVersion = Constanst.LanguageVersion.English,
                    IsManufacturerWarranty = viewModel.IsManufacturerWarranty,
                    IsManufacturerUsedVehicleWarranty = viewModel.IsManufacturerUsedVehicleWarranty,
                    IsOtherWarranty = viewModel.IsOtherWarranty
                };
                _dealerManagementForm.UpdateBuyerGuide(buyerGuide);

                viewModel.Message = "Your settings have been saved successfully.";
            }
            catch (Exception ex)
            {
                viewModel.Message = "Failed to save your settings. \n " + ex.Message;
            }

            return View(viewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateBuyerGuideSpanish(AdminBuyerGuideViewModel viewModel)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;
            viewModel.Message = "";

            if (viewModel.IsPreview)
            {
                viewModel.SelectedLanguage = 2;
                viewModel.SystemCovered = ReplaceFontSizeForBuyerGuideReport(viewModel.SystemCovered);
                viewModel.Durations = ReplaceFontSizeForBuyerGuideReport(viewModel.Durations);
                viewModel.SystemCoveredAndDurations =
                    ReplaceFontSizeForBuyerGuideReport(viewModel.SystemCoveredAndDurations);
                Session["BuyerGuideSetting"] = viewModel;
                return RedirectToAction("PrintBuyerGuideSetting", "PDF");
            }
            try
            {
                var buyerGuide = new BuyerGuide()
                {
                    DealerId = dealer.DealershipId,
                    Vin = viewModel.Vin,
                    Year = viewModel.Year,
                    Make = viewModel.Make,
                    Model = viewModel.VehicleModel,
                    Stock = viewModel.StockNumber,
                    WarrantyType = viewModel.WarrantyType,
                    IsServiceContract = viewModel.IsServiceContract,
                    IsAsWarranty = viewModel.IsAsWarranty,
                    IsWarranty = viewModel.IsWarranty,
                    IsFullWarranty = viewModel.IsFullWarranty,
                    IsLimitedWarranty = viewModel.IsLimitedWarranty,
                    SystemCovered = viewModel.SystemCovered,
                    Durations = viewModel.Durations,
                    PercentageOfLabor = viewModel.PercentageOfLabor,
                    PercentageOfPart = viewModel.PercentageOfPart,
                    PriorRental = viewModel.PriorRental,
                    IsMixed = viewModel.IsMixed,
                    SystemCoveredAndDurations = viewModel.SystemCoveredAndDurations,
                    LanguageVersion = Constanst.LanguageVersion.Spanish,
                    IsManufacturerWarranty = viewModel.IsManufacturerWarranty,
                    IsManufacturerUsedVehicleWarranty = viewModel.IsManufacturerUsedVehicleWarranty,
                    IsOtherWarranty = viewModel.IsOtherWarranty
                };
                _dealerManagementForm.UpdateBuyerGuide(buyerGuide);

                viewModel.Message = "Your settings have been saved successfully.";
            }
            catch (Exception ex)
            {
                viewModel.Message = "Failed to save your settings. \n " + ex.Message;
            }

            return View(viewModel);
        }

        public ActionResult ViewBuyerGuideinHtml(BuyerGuideViewModel buyerGuide)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;

            var row = _inventoryManagementForm.GetInventory(buyerGuide.ListingId);

            var settingRow = _dealerManagementForm.GetDealerSettingById(dealer.DealershipId);

            var viewModel = new BuyerGuideViewModel
            {
                Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make,
                Year = row.Vehicle.Year.GetValueOrDefault(),
                Model = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model,
                StockNumber = String.IsNullOrEmpty(row.Stock)
                    ? "NA"
                    : row.Stock,
                Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin,
                PriorRental = row.PriorRental.GetValueOrDefault()
            };

            if (row.WarrantyInfo == null)
                viewModel.Warranty = 0;
            else
            {
                viewModel.Warranty = row.WarrantyInfo.GetValueOrDefault();

            }

            viewModel.AsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? "" : settingRow.AsIsWarranty;

            viewModel.AsWarrantyInSpanish = String.IsNullOrEmpty(settingRow.AsIsWarrantySpanish)
                                                ? ""
                                                : settingRow.AsIsWarrantySpanish;


            viewModel.ManufacturerWarranty = string.Empty;

            viewModel.ManufacturerWarrantyInSpanish = string.Empty;

            viewModel.ManufacturerWarrantyDuration = string.Empty;


            viewModel.DealerCertified = string.Empty;

            viewModel.DealerCertifiedInSpanish = string.Empty;


            viewModel.DealerCertifiedDuration = string.Empty;

            viewModel.ManufacturerCertified = string.Empty;

            viewModel.ManufacturerCertifiedInSpanish = string.Empty;

            viewModel.ManufacturerCertifiedDuration = string.Empty;

            viewModel.SelectedLanguage = buyerGuide.SelectedLanguage;

            var mileage = 0;

            if (row.Mileage.HasValue && viewModel.Year >= DateTime.Now.Year - 15 && mileage < 100000)
                viewModel.ServiceContract = true;



            return View("BuyerGuide", viewModel);
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewReport()
        {

            return View("Report");
        }

        public ActionResult ViewPreOwnedInventoryReport()
        {

            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;
                return Redirect("~/ReportTemplates/UsedInventoryReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult PriceRangeReport(string s)
        {
            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;
                return Redirect("~/ReportTemplates/PriceRangeReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult AgingReport(string s)
        {
            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;
                return Redirect("~/ReportTemplates/AgingReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewPreOwnedInventoryReportTemplate2()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;
                return Redirect("~/ReportTemplates/UsedInventoryReportTemplate2.aspx");
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewNewInventoryReport()
        {

            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/NewInventoryReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult KBBWholesale()
        {

            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/ProfitManagementReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult ViewReconInventoryReport()
        {
            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/ReconInventoryReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
           
        }

        public ActionResult ViewCertifiedInventoryReport()
        {
            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/CertifiednventoryReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewRebateReportByTrim(int RebateId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
               
                var dealer = SessionHandler.Dealer;

                var rebateInfo = _dealerManagementForm.GetRebates(dealer.DealershipId);

                var dealerInventory = _inventoryManagementForm.GetAllNewInventories(dealer.DealershipId);

                IQueryable<Inventory> avaiInventory =
                    from e in dealerInventory
                    from t in rebateInfo
                    where e.DealerId==t.DealerId && e.Vehicle.Year == t.Year 
                    && e.Vehicle.Make == t.Make && e.Vehicle.Model == t.Model
                    && e.Vehicle.Trim == t.Trim 
                    select e;


                var list = new List<CarInfoFormViewModel>();

                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.InventoryId,
                        ModelYear = tmp.Vehicle.Year.GetValueOrDefault(),
                        Stock = String.IsNullOrEmpty(tmp.Stock) ? "" : tmp.Stock,
                        Model = String.IsNullOrEmpty(tmp.Vehicle.Model) ? "" : tmp.Vehicle.Model,
                        Make = String.IsNullOrEmpty(tmp.Vehicle.Make) ? "" : tmp.Vehicle.Make,
                        Mileage = tmp.Mileage.GetValueOrDefault(),
                        Trim = String.IsNullOrEmpty(tmp.Vehicle.Trim) ? "" : tmp.Vehicle.Trim,
                        ChromeStyleId = tmp.Vehicle.ChromeStyleId,
                        ChromeModelId = tmp.Vehicle.ChromeModelId,
                        Vin = String.IsNullOrEmpty(tmp.Vehicle.Vin) ? "" : tmp.Vehicle.Vin,
                        SalePrice = tmp.SalePrice.GetValueOrDefault(),
                        Msrp = tmp.Vehicle.Msrp.GetValueOrDefault(),
                        ManufacturerRebate = tmp.ManufacturerRebate.GetValueOrDefault(),

                    };
                    list.Add(car);
                }

                return File(ReportHelper.ExportToCSVForRebate(list, dealer.DealershipId, "Rebate Report By Trim"), "application/vnd.ms-excel",
                            "RebateReportByTrim.xlsx");


            }
        }

        public ActionResult ViewPreOwnedMultipleInventoryReport()
        {
            if (SessionHandler.DealerGroup != null)
            {

                var dealerGroup = SessionHandler.DealerGroup;

                Session["dealerGroupId"] = dealerGroup.DealershipGroupId;

                return Redirect("~/ReportTemplates/UsedMultipleInventoryReport.aspx");
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ViewNewMultipleInventoryReport()
        {
            if (SessionHandler.DealerGroup != null)
            {

                var dealerGroup = SessionHandler.DealerGroup;

                Session["dealerGroupId"] = dealerGroup.DealershipGroupId;

                return Redirect("~/ReportTemplates/NewMultipleInventoryReport.aspx");
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ViewReconMultipleInventoryReport()
        {
            if (SessionHandler.DealerGroup == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = SessionHandler.Dealer;

                var dealerGroup = SessionHandler.DealerGroup;

                var context = new VincontrolEntities();

                IEnumerable<Int32> dealerList = from e in context.Dealers
                                                where e.DealerGroupId == dealerGroup.DealershipGroupId
                                                select e.DealerId;

                var avaiInventory = context.Inventories.Where(LogicHelper.BuildContainsExpression<Inventory, int>(e => e.DealerId, dealerList));

                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory.Where(x => x.InventoryStatusCodeId == Constanst.InventoryStatus.Recon))
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.InventoryId,
                        ModelYear = tmp.Vehicle.Year.GetValueOrDefault(),
                        Stock = String.IsNullOrEmpty(tmp.Stock) ? "" : tmp.Stock,
                        Model = String.IsNullOrEmpty(tmp.Vehicle.Model) ? "" : tmp.Vehicle.Model,
                        Make = String.IsNullOrEmpty(tmp.Vehicle.Make) ? "" : tmp.Vehicle.Make,
                        Mileage = tmp.Mileage.GetValueOrDefault(),
                        Trim = String.IsNullOrEmpty(tmp.Vehicle.Trim) ? "" : tmp.Vehicle.Trim,
                        ChromeStyleId = tmp.Vehicle.ChromeStyleId,
                        ChromeModelId = tmp.Vehicle.ChromeModelId,
                        Vin = String.IsNullOrEmpty(tmp.Vehicle.Vin) ? "" : tmp.Vehicle.Vin,
                        SalePrice = tmp.SalePrice.GetValueOrDefault(),
                        Msrp = tmp.Vehicle.Msrp.GetValueOrDefault(),
                        ManufacturerRebate = tmp.ManufacturerRebate.GetValueOrDefault(),
                    };
                    list.Add(car);
                }

                return File(ReportHelper.ExportToCSV(list, dealer.DealershipId, "Recon Report"), "application/vnd.ms-excel",
                            "ReconReport.xlsx");


            }
        }

        public void GetWarrantyInfo(ConsumerInformation[] consumerInformation, ref BuyerGuideViewModel viewModel)
        {
            foreach (ConsumerInformation consumer in consumerInformation)
            {
                if (consumer.type.Value.Equals("Warranty"))
                {
                    foreach (ConsumerInformationItem item in consumer.item)
                    {
                        switch (item.name)
                        {
                            case "Basic Years":
                                viewModel.WarrantyBumperYear = item.value + " Years";
                                break;
                            case "Basic Miles/km":
                                viewModel.WarrantyBumperMiles = item.value + " Miles";
                                break;
                            case "Drivetrain Years":
                                viewModel.WarrantyDrivetrainYear = item.value + " Years";
                                break;
                            case "Drivetrain Miles/km":
                                viewModel.WarrantyDrivetrainMiles = item.value + " Miles";
                                break;
                            case "Corrosion Years":
                                viewModel.WarrantyWearAndTearYear = item.value + " Years";
                                break;
                            case "Corrosion Miles/km":
                                viewModel.WarrantyWearAndTearMiles = item.value + " Miles";
                                break;
                            case "Roadside Assistance Years":
                                viewModel.WarrantyRoadsideYear = item.value + " Years";
                                break;
                            case "Roadside Assistance Miles/km":
                                viewModel.WarrantyRoadsideMiles = item.value + " Miles";
                                break;
                            default:
                                break;

                        }
                    }

                    break;
                }
            }


            viewModel.WarrantyBumperYear = String.IsNullOrEmpty(viewModel.WarrantyBumperYear) ? "Unavailable" : viewModel.WarrantyBumperYear;
            viewModel.WarrantyBumperMiles = String.IsNullOrEmpty(viewModel.WarrantyBumperMiles) ? "Unavailable" : viewModel.WarrantyBumperMiles;
            viewModel.WarrantyDrivetrainYear = String.IsNullOrEmpty(viewModel.WarrantyDrivetrainYear) ? "Unavailable" : viewModel.WarrantyDrivetrainYear;
            viewModel.WarrantyDrivetrainMiles = String.IsNullOrEmpty(viewModel.WarrantyDrivetrainMiles) ? "Unavailable" : viewModel.WarrantyDrivetrainMiles;
            viewModel.WarrantyWearAndTearYear = String.IsNullOrEmpty(viewModel.WarrantyWearAndTearYear) ? "Unavailable" : viewModel.WarrantyWearAndTearYear;
            viewModel.WarrantyWearAndTearMiles = String.IsNullOrEmpty(viewModel.WarrantyWearAndTearMiles) ? "Unavailable" : viewModel.WarrantyWearAndTearMiles;
            viewModel.WarrantyRoadsideYear = String.IsNullOrEmpty(viewModel.WarrantyRoadsideYear) ? "Unavailable" : viewModel.WarrantyRoadsideYear;
            viewModel.WarrantyRoadsideMiles = String.IsNullOrEmpty(viewModel.WarrantyRoadsideMiles) ? "Unavailable" : viewModel.WarrantyRoadsideMiles;
        }

        private string ReplaceFontSizeForBuyerGuideReport(string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            content = content.Replace("8pt", "28pt");
            content = content.Replace("8px", "28px");
            content = content.Replace("9pt", "29pt");
            content = content.Replace("9px", "29px");
            content = content.Replace("10pt", "30pt");
            content = content.Replace("10px", "30px");
            content = content.Replace("11pt", "31pt");
            content = content.Replace("11px", "31px");
            content = content.Replace("12pt", "32pt");
            content = content.Replace("12px", "32px");
            content = content.Replace("13pt", "33pt");
            content = content.Replace("13px", "33px");
            content = content.Replace("14pt", "34pt");
            content = content.Replace("14px", "34px");
            content = content.Replace("15pt", "35pt");
            content = content.Replace("15px", "35px");
            content = content.Replace("16pt", "36pt");
            content = content.Replace("16px", "36px");
            content = content.Replace("17pt", "37pt");
            content = content.Replace("17px", "37px");
            content = content.Replace("18pt", "38pt");
            content = content.Replace("18px", "38px");
            content = content.Replace("19pt", "39pt");
            content = content.Replace("19px", "39px");
            content = content.Replace("20pt", "40pt");
            content = content.Replace("20px", "40px");
            content = content.Replace("21pt", "41pt");
            content = content.Replace("21px", "41px");
            content = content.Replace("22pt", "42pt");
            content = content.Replace("22px", "42px");
            content = content.Replace("23pt", "43pt");
            content = content.Replace("23px", "43px");
            content = content.Replace("24pt", "44pt");
            content = content.Replace("24px", "44px");
            content = content.Replace("25pt", "45pt");
            content = content.Replace("25px", "45px");
            content = content.Replace("26pt", "46pt");
            content = content.Replace("26px", "46px");

            return content;
        }

        public ActionResult ViewBuyerGuideinPdf(BuyerGuideViewModel buyerGuide)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            
            var dealer = SessionHandler.Dealer;
            var adminBuyerGuideViewModel = new AdminBuyerGuideViewModel();
            
            var row = _inventoryManagementForm.GetInventory(buyerGuide.ListingId);
            var settingRow = _dealerManagementForm.GetDealerSettingById(dealer.DealershipId);


            var warrantyType = 0;
            warrantyType = row.WarrantyInfo == null ? 1 : row.WarrantyInfo.GetValueOrDefault();


            if (buyerGuide.SelectedLanguage < 3)
            {

                var buyerGuideSetting = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                    buyerGuide.SelectedLanguage, warrantyType);

                adminBuyerGuideViewModel.SelectedLanguage = buyerGuide.SelectedLanguage;
                if (buyerGuideSetting != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSetting.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations = ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }
            }
            else if (buyerGuide.SelectedLanguage == 3)
            {
                var buyerGuideSettingEnglish = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                    Constanst.LanguageVersion.English, warrantyType);

                var buyerGuideSettingSpanish = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                    Constanst.LanguageVersion.Spanish, warrantyType);


                adminBuyerGuideViewModel.SelectedLanguage = buyerGuide.SelectedLanguage;
                if (buyerGuideSettingEnglish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingEnglish.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSettingEnglish.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSettingEnglish.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSettingEnglish.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSettingEnglish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSettingEnglish.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSettingEnglish.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSettingEnglish.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSettingEnglish.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSettingEnglish.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSettingEnglish.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSettingEnglish.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSettingEnglish.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSettingEnglish.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    
                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }


                if (buyerGuideSettingSpanish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingSpanish.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyTypeSpanish = buyerGuideSettingSpanish.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContractSpanish = buyerGuideSettingSpanish.IsServiceContract ??
                                                                      false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = buyerGuideSettingSpanish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarrantySpanish = buyerGuideSettingSpanish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = buyerGuideSettingSpanish.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = buyerGuideSettingSpanish.IsLimitedWarranty ??
                                                                        false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = buyerGuideSettingSpanish.IsServiceContract ??
                                                                        false;
                    adminBuyerGuideViewModel.IsMixedSpanish = buyerGuideSettingSpanish.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.DurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.Durations);
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = buyerGuideSettingSpanish.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = buyerGuideSettingSpanish.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRentalSpanish = row.PriorRental.GetValueOrDefault()
                                                                      ? "PRIOR RENTAL"
                                                                      : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();

                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = !String.IsNullOrEmpty(settingRow.AsIsWarranty);
                    adminBuyerGuideViewModel.IsWarrantySpanish = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish = string.Empty;
                    adminBuyerGuideViewModel.DurationsSpanish = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = 100;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarrantySpanish && !adminBuyerGuideViewModel.IsFullWarrantySpanish &&
                        !adminBuyerGuideViewModel.IsLimitedWarrantySpanish)
                        adminBuyerGuideViewModel.IsWarrantySpanish = true;
                }
            }

            Session["BuyerGuideSetting"] = adminBuyerGuideViewModel;
            return RedirectToAction("PrintBuyerGuideSetting", "PDF");

        }

        public ActionResult ViewBuyerGuideinPdfNew(int selectedLanguage, int listingID)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
         
            var row = _inventoryManagementForm.GetInventory(listingID);

            if (row != null)
            {
                _vehicleLogManagementForm.AddVehicleLog(listingID, SessionHandler.CurrentUser.UserId,
                   Constanst.VehicleLogSentence.BuyerGuideCreatedByUser.Replace("USER",
                          SessionHandler.CurrentUser.FullName), null);
                BuildAdminBuyerGuideByInventory(selectedLanguage, listingID);

            }

            else
            {
                _vehicleLogManagementForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                  Constanst.VehicleLogSentence.BuyerGuideCreatedByUser.Replace("USER",
                         SessionHandler.CurrentUser.FullName), listingID);
              
                BuildAdminBuyerGuideBySold(selectedLanguage, listingID);
            }
            return Json(new { success = true, url = Url.Action("PrintBuyerGuideSetting", "PDF") });
            

        }

        private void BuildAdminBuyerGuideBySold(int selectedLanguage, int listingID)
        {
            var adminBuyerGuideViewModel = new AdminBuyerGuideViewModel();

            var dealer = SessionHandler.Dealer;

            var row = _inventoryManagementForm.GetSoldInventory(listingID);

            var settingRow = _dealerManagementForm.GetDealerSettingById(dealer.DealershipId);

            var warrantyType = 0;
            warrantyType = row.WarrantyInfo == null ? 1 : row.WarrantyInfo.GetValueOrDefault();


            if (selectedLanguage < 3)
            {
                var buyerGuideSetting = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                  selectedLanguage, warrantyType);
               

                adminBuyerGuideViewModel.SelectedLanguage = selectedLanguage;
                if (buyerGuideSetting != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSetting.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations = ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }
            }
            else if (selectedLanguage == 3)
            {

                var buyerGuideSettingEnglish = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                    Constanst.LanguageVersion.English, warrantyType);

                var buyerGuideSettingSpanish = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                    Constanst.LanguageVersion.Spanish, warrantyType);




                adminBuyerGuideViewModel.SelectedLanguage = selectedLanguage;
                if (buyerGuideSettingEnglish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingEnglish.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSettingEnglish.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSettingEnglish.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSettingEnglish.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSettingEnglish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSettingEnglish.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSettingEnglish.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSettingEnglish.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSettingEnglish.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSettingEnglish.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSettingEnglish.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSettingEnglish.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSettingEnglish.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSettingEnglish.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = String.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }


                if (buyerGuideSettingSpanish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingSpanish.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyTypeSpanish = buyerGuideSettingSpanish.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContractSpanish = buyerGuideSettingSpanish.IsServiceContract ??
                                                                      false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = buyerGuideSettingSpanish.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarrantySpanish = buyerGuideSettingSpanish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = buyerGuideSettingSpanish.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = buyerGuideSettingSpanish.IsLimitedWarranty ??
                                                                        false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = buyerGuideSettingSpanish.IsServiceContract ??
                                                                        false;
                    adminBuyerGuideViewModel.IsMixedSpanish = buyerGuideSettingSpanish.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.DurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.Durations);
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = buyerGuideSettingSpanish.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = buyerGuideSettingSpanish.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRentalSpanish = row.PriorRental.GetValueOrDefault()
                                                                      ? "PRIOR RENTAL"
                                                                      : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSettingSpanish.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSettingSpanish.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSettingSpanish.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarrantySpanish = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish = string.Empty;
                    adminBuyerGuideViewModel.DurationsSpanish = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = 100;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarrantySpanish && !adminBuyerGuideViewModel.IsFullWarrantySpanish &&
                        !adminBuyerGuideViewModel.IsLimitedWarrantySpanish)
                        adminBuyerGuideViewModel.IsWarrantySpanish = true;
                }
            }

            Session["BuyerGuideSetting"] = adminBuyerGuideViewModel;
        }

        private void BuildAdminBuyerGuideByInventory(int selectedLanguage, int listingID)
        {
            var adminBuyerGuideViewModel = new AdminBuyerGuideViewModel();

            var dealer = SessionHandler.Dealer;

            var context = new VincontrolEntities();

            var row = _inventoryManagementForm.GetInventory(listingID);

            var settingRow = _dealerManagementForm.GetDealerSettingById(dealer.DealershipId);


            var warrantyType = 0;
            if (row.WarrantyInfo == null)
                warrantyType = 1;
            else
            {
                warrantyType = row.WarrantyInfo.GetValueOrDefault();
            }


            if (selectedLanguage < 3)
            {

                var buyerGuideSetting = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                  selectedLanguage, warrantyType);
               

                adminBuyerGuideViewModel.SelectedLanguage = selectedLanguage;
                if (buyerGuideSetting != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSetting.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations = ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }
            }
            else if (selectedLanguage == 3)
            {
                var buyerGuideSettingEnglish = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                      Constanst.LanguageVersion.English, warrantyType);

                var buyerGuideSettingSpanish = _dealerManagementForm.GetBuyerGuide(dealer.DealershipId,
                    Constanst.LanguageVersion.Spanish, warrantyType);




                adminBuyerGuideViewModel.SelectedLanguage = selectedLanguage;
                if (buyerGuideSettingEnglish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingEnglish.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSettingEnglish.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSettingEnglish.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSettingEnglish.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSettingEnglish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSettingEnglish.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSettingEnglish.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSettingEnglish.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSettingEnglish.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSettingEnglish.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSettingEnglish.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSettingEnglish.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSettingEnglish.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSettingEnglish.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }


                if (buyerGuideSettingSpanish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingSpanish.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyTypeSpanish = buyerGuideSettingSpanish.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContractSpanish = buyerGuideSettingSpanish.IsServiceContract ??
                                                                      false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = buyerGuideSettingSpanish.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarrantySpanish = buyerGuideSettingSpanish.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = buyerGuideSettingSpanish.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = buyerGuideSettingSpanish.IsLimitedWarranty ??
                                                                        false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = buyerGuideSettingSpanish.IsServiceContract ??
                                                                        false;
                    adminBuyerGuideViewModel.IsMixedSpanish = buyerGuideSettingSpanish.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.DurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.Durations);
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = buyerGuideSettingSpanish.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = buyerGuideSettingSpanish.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRentalSpanish = row.PriorRental.GetValueOrDefault()
                                                                      ? "PRIOR RENTAL"
                                                                      : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();

                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year ?? 0;
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarrantySpanish = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish = string.Empty;
                    adminBuyerGuideViewModel.DurationsSpanish = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = 100;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarrantySpanish && !adminBuyerGuideViewModel.IsFullWarrantySpanish &&
                        !adminBuyerGuideViewModel.IsLimitedWarrantySpanish)
                        adminBuyerGuideViewModel.IsWarrantySpanish = true;
                }
            }

            Session["BuyerGuideSetting"] = adminBuyerGuideViewModel;
        }

        public ActionResult ViewTodayBucketJumpReport()
        {

            if (SessionHandler.Dealer != null)
            {

                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/TodayBucketJumpReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult ViewNext7DaysBucketJumpReport()
        {
            var dealer = SessionHandler.Dealer;

            Session["dealerId"] = dealer.DealershipId;

            return Redirect("~/ReportTemplates/Next7DaysBucketJumpReport.aspx");
        }

        public ActionResult ViewKarpowerReport()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/KarpowerInventoryReport.aspx");
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewManheimInventoryReport()
        {
            var dealer = SessionHandler.Dealer;

            Session["dealerId"] = dealer.DealershipId;

            return Redirect("~/ReportTemplates/ManheimInventoryReport.aspx");
        }

        public ActionResult ViewShareFlyersReport()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/ShareFlyersReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }
    }
}
