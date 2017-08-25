using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Security;


namespace WhitmanEnterpriseMVC.Controllers
{
    public class ReportController : SecurityController
    {
        private const string PermissionCode = "REPORT";
        private const string AcceptedValues = "ALLACCESS";

        //
        // GET: /Report/
    
        public ActionResult AppraisalPrintOption()
        {
            return View("AppraisalPrintOption");
        }

        public ActionResult PrintOption(string condition)
        {
            if(condition.Equals("Used"))
                return View("PrintOption");
            else
            {
                return View("PrintOptionForNew");
            }
        }

        public ActionResult ViewSticker(string ListingId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            string fillinContext = WindowStickerHelper.BuildWindowStickerInHtml(ListingId, dealer.DealershipId);

            var workStream = new MemoryStream();

            if (!String.IsNullOrEmpty(fillinContext))
            {
                var reader = new StringReader(fillinContext);
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, workStream).CloseStream = false;
                var worker = new HTMLWorker(document);
                document.Open();
                worker.StartDocument();
                worker.Parse(reader);
                worker.EndDocument();
                document.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;


            }


            return new FileStreamResult(workStream, "application/pdf");
        }

        public ActionResult ViewPriceChangeReport()
        {
            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/PriceTrackingReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewFullSticker()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            string fillinContext = WindowStickerHelper.BuildWindowStickerInHtml();

            var workStream = new MemoryStream();

            if (!String.IsNullOrEmpty(fillinContext))
            {
                var reader = new StringReader(fillinContext);
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, workStream).CloseStream = false;
                var worker = new HTMLWorker(document);
                document.Open();
                worker.StartDocument();
                worker.Parse(reader);
                worker.EndDocument();
                document.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
            }

            return new FileStreamResult(workStream, "application/pdf");
        }

        public ActionResult ViewFullInventoryBuyerGuide()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            string htmlToConvert = BuyerGuideHelper.BuildBuyerGuideInHTML(ControllerContext);

            byte[] byteStream = PDFHelper.GeneratePDFFromHtmlCode(htmlToConvert);
            
            return new FileStreamResult(new MemoryStream(byteStream), "application/pdf");

            
        }

        public ActionResult ViewFullInventoryBuyerGuideInSpanish()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            string htmlToConvert = BuyerGuideHelper.BuildBuyerGuideInHTMLInSpanish(ControllerContext);

            byte[] byteStream = PDFHelper.GeneratePDFFromHtmlCode(htmlToConvert);

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

        public ActionResult ManheimTransactionDetail(string year, string make, string model, string trim)
        {
            var manheimTransactions = new List<ManheimTransactionViewModel>();
            ViewData["ManheimYear"] = year;
            ViewData["ManheimMake"] = make;
            ViewData["ManheimModel"] = model;
            ViewData["ManheimTrim"] = trim;
            try
            {
                manheimTransactions = LinqHelper.ManheimTransactions(year, make, model, trim, "NA");
            }
            catch (Exception)
            {

            }

            return View("ManheimReportWindow", manheimTransactions);
        }

        [HttpPost]
        public ActionResult ManheimReportDetail(string year, string make, string model, string trim, string region)
        {
            var manheimTransactions = new List<ManheimTransactionViewModel>();
            try
            {
                manheimTransactions = LinqHelper.ManheimTransactions(year, make, model, trim, region);
            }
            catch (Exception)
            {

            }

            return PartialView("ManheimReportDetail", manheimTransactions);
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
  
    
        public ActionResult CreateBuyerGuide(string type)
        {
            var viewModel = new AdminBuyerGuideViewModel();
            int typeNumber;
            Int32.TryParse(type, out typeNumber);

            viewModel.WarrantyType = typeNumber;
            viewModel.Message = "";

            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var buyerGuideSetting =
                    context.vincontrolbuyerguides.FirstOrDefault(bg => bg.dealershipId == dealer.DealershipId && bg.LanguageVersion==1 &&  bg.warrantyType==typeNumber);
                if(buyerGuideSetting != null)
                {
                    viewModel.Id = buyerGuideSetting.buyerguideid;
                    viewModel.Vin = buyerGuideSetting.vinNumber;
                    viewModel.Year = buyerGuideSetting.year;
                    viewModel.Make = buyerGuideSetting.make;
                    viewModel.VehicleModel = buyerGuideSetting.model;
                    viewModel.StockNumber = buyerGuideSetting.stockNumber;
                    viewModel.WarrantyType = buyerGuideSetting.warrantyType.GetValueOrDefault();
                    viewModel.ServiceContract = buyerGuideSetting.isServiceContract ?? false;
                    viewModel.IsAsWarranty = buyerGuideSetting.isAsWarranty ?? false;
                    viewModel.IsWarranty = buyerGuideSetting.isWarranty ?? false;
                    viewModel.IsFullWarranty = buyerGuideSetting.isFullWarranty ?? false;
                    viewModel.IsLimitedWarranty = buyerGuideSetting.isLimitedWarranty ?? false;
                    viewModel.IsServiceContract = buyerGuideSetting.isServiceContract ?? false;
                    viewModel.SystemCovered = buyerGuideSetting.systemCovered;
                    viewModel.Durations = buyerGuideSetting.durations;
                    viewModel.PercentageOfLabor = buyerGuideSetting.percentageOfLabor ?? 0;
                    viewModel.PercentageOfPart = buyerGuideSetting.percentageOfPart ?? 0;
                    viewModel.PriorRental = buyerGuideSetting.priorRental;
                    viewModel.SystemCoveredAndDurations = buyerGuideSetting.systemCoveredAndDurations;
                    viewModel.IsPriorRental = false;
                    viewModel.IsMixed = buyerGuideSetting.isMixed ?? false;
                    viewModel.SelectedLanguage = buyerGuideSetting.LanguageVersion.GetValueOrDefault();
                }
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

            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var buyerGuideSetting =
                    context.vincontrolbuyerguides.FirstOrDefault(bg => bg.dealershipId == dealer.DealershipId && bg.LanguageVersion == 2 && bg.warrantyType==typeNumber);
                if (buyerGuideSetting != null)
                {
                    viewModel.Id = buyerGuideSetting.buyerguideid;
                    viewModel.Vin = buyerGuideSetting.vinNumber;
                    viewModel.Year = buyerGuideSetting.year;
                    viewModel.Make = buyerGuideSetting.make;
                    viewModel.VehicleModel = buyerGuideSetting.model;
                    viewModel.StockNumber = buyerGuideSetting.stockNumber;
                    viewModel.WarrantyType = buyerGuideSetting.warrantyType.GetValueOrDefault();
                    viewModel.ServiceContract = buyerGuideSetting.isServiceContract ?? false;
                    viewModel.IsAsWarranty = buyerGuideSetting.isAsWarranty ?? false;
                    viewModel.IsWarranty = buyerGuideSetting.isWarranty ?? false;
                    viewModel.IsFullWarranty = buyerGuideSetting.isFullWarranty ?? false;
                    viewModel.IsLimitedWarranty = buyerGuideSetting.isLimitedWarranty ?? false;
                    viewModel.IsServiceContract = buyerGuideSetting.isServiceContract ?? false;
                    viewModel.SystemCovered = buyerGuideSetting.systemCovered;
                    viewModel.Durations = buyerGuideSetting.durations;
                    viewModel.PercentageOfLabor = buyerGuideSetting.percentageOfLabor ?? 0;
                    viewModel.PercentageOfPart = buyerGuideSetting.percentageOfPart ?? 0;
                    viewModel.PriorRental = buyerGuideSetting.priorRental;
                    viewModel.SystemCoveredAndDurations = buyerGuideSetting.systemCoveredAndDurations;
                    viewModel.IsPriorRental = false;
                    viewModel.IsMixed = buyerGuideSetting.isMixed ?? false;
                    viewModel.SelectedLanguage = buyerGuideSetting.LanguageVersion.GetValueOrDefault();
                }
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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];
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
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var buyerGuideSetting =
                        context.vincontrolbuyerguides.FirstOrDefault(
                            bg =>
                            bg.dealershipId == dealer.DealershipId && bg.LanguageVersion==1 &&
                            bg.warrantyType==viewModel.WarrantyType);
                    if (buyerGuideSetting != null)
                    {
                        buyerGuideSetting.vinNumber = viewModel.Vin;
                        buyerGuideSetting.year = viewModel.Year;
                        buyerGuideSetting.make = viewModel.Make;
                        buyerGuideSetting.model = viewModel.VehicleModel;
                        buyerGuideSetting.stockNumber = viewModel.StockNumber;
                        buyerGuideSetting.warrantyType = viewModel.WarrantyType;
                        buyerGuideSetting.isServiceContract = viewModel.IsServiceContract;
                        buyerGuideSetting.isAsWarranty = viewModel.IsAsWarranty;
                        buyerGuideSetting.isWarranty = viewModel.IsWarranty;
                        buyerGuideSetting.isFullWarranty = viewModel.IsFullWarranty;
                        buyerGuideSetting.isLimitedWarranty = viewModel.IsLimitedWarranty;
                        buyerGuideSetting.systemCovered = viewModel.SystemCovered;
                        buyerGuideSetting.durations = viewModel.Durations;
                        buyerGuideSetting.percentageOfLabor = viewModel.PercentageOfLabor;
                        buyerGuideSetting.percentageOfPart = viewModel.PercentageOfPart;
                        buyerGuideSetting.priorRental = viewModel.PriorRental;
                        buyerGuideSetting.isMixed = viewModel.IsMixed;
                        buyerGuideSetting.systemCoveredAndDurations = viewModel.SystemCoveredAndDurations;

                        context.SaveChanges();
                    }
                    else
                    {
                        var buyerGuideToInsert = new vincontrolbuyerguide()
                        {
                            dealershipId = dealer.DealershipId,
                            vinNumber = viewModel.Vin,
                            year = viewModel.Year,
                            make = viewModel.Make,
                            model = viewModel.VehicleModel,
                            stockNumber = viewModel.StockNumber,
                            warrantyType = viewModel.WarrantyType,
                            isServiceContract = viewModel.IsServiceContract,
                            isAsWarranty = viewModel.IsAsWarranty,
                            isWarranty = viewModel.IsWarranty,
                            isFullWarranty = viewModel.IsFullWarranty,
                            isLimitedWarranty = viewModel.IsLimitedWarranty,
                            systemCovered = viewModel.SystemCovered,
                            durations = viewModel.Durations,
                            percentageOfLabor = viewModel.PercentageOfLabor,
                            percentageOfPart = viewModel.PercentageOfPart,
                            priorRental = viewModel.PriorRental,
                            isMixed = viewModel.IsMixed,
                            systemCoveredAndDurations = viewModel.SystemCoveredAndDurations,
                            LanguageVersion = 1,
                        };

                        context.AddTovincontrolbuyerguides(buyerGuideToInsert);
                        context.SaveChanges();
                    }
                }

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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];
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
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var buyerGuideSetting =
                        context.vincontrolbuyerguides.FirstOrDefault(
                            bg =>
                            bg.dealershipId == dealer.DealershipId && bg.LanguageVersion==2&&
                            bg.warrantyType==viewModel.WarrantyType);
                    if (buyerGuideSetting != null)
                    {
                        buyerGuideSetting.vinNumber = viewModel.Vin;
                        buyerGuideSetting.year = viewModel.Year;
                        buyerGuideSetting.make = viewModel.Make;
                        buyerGuideSetting.model = viewModel.VehicleModel;
                        buyerGuideSetting.stockNumber = viewModel.StockNumber;
                        buyerGuideSetting.warrantyType = viewModel.WarrantyType;
                        buyerGuideSetting.isServiceContract = viewModel.IsServiceContract;
                        buyerGuideSetting.isAsWarranty = viewModel.IsAsWarranty;
                        buyerGuideSetting.isWarranty = viewModel.IsWarranty;
                        buyerGuideSetting.isFullWarranty = viewModel.IsFullWarranty;
                        buyerGuideSetting.isLimitedWarranty = viewModel.IsLimitedWarranty;
                        buyerGuideSetting.systemCovered = viewModel.SystemCovered;
                        buyerGuideSetting.durations = viewModel.Durations;
                        buyerGuideSetting.percentageOfLabor = viewModel.PercentageOfLabor;
                        buyerGuideSetting.percentageOfPart = viewModel.PercentageOfPart;
                        buyerGuideSetting.priorRental = viewModel.PriorRental;
                        buyerGuideSetting.isMixed = viewModel.IsMixed;
                        buyerGuideSetting.systemCoveredAndDurations = viewModel.SystemCoveredAndDurations;

                        context.SaveChanges();
                    }
                    else
                    {
                        var buyerGuideToInsert = new vincontrolbuyerguide()
                        {
                            dealershipId = dealer.DealershipId,
                            vinNumber = viewModel.Vin,
                            year = viewModel.Year,
                            make = viewModel.Make,
                            model = viewModel.VehicleModel,
                            stockNumber = viewModel.StockNumber,
                            warrantyType = viewModel.WarrantyType,
                            isServiceContract = viewModel.IsServiceContract,
                            isAsWarranty = viewModel.IsAsWarranty,
                            isWarranty = viewModel.IsWarranty,
                            isFullWarranty = viewModel.IsFullWarranty,
                            isLimitedWarranty = viewModel.IsLimitedWarranty,
                            systemCovered = viewModel.SystemCovered,
                            durations = viewModel.Durations,
                            percentageOfLabor = viewModel.PercentageOfLabor,
                            percentageOfPart = viewModel.PercentageOfPart,
                            priorRental = viewModel.PriorRental,
                            isMixed = viewModel.IsMixed,
                            systemCoveredAndDurations = viewModel.SystemCoveredAndDurations,
                            LanguageVersion = 2,
                        };

                        context.AddTovincontrolbuyerguides(buyerGuideToInsert);
                        context.SaveChanges();
                    }
                }

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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            var context = new whitmanenterprisewarehouseEntities();

            var row =
                context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == buyerGuide.ListingId);

            var settingRow = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

            var viewModel = new BuyerGuideViewModel();

            viewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;

            viewModel.ModelYear = row.ModelYear.GetValueOrDefault();

            viewModel.Model = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;

            viewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                        ? "NA"
                                        : row.StockNumber;

            viewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;

            viewModel.PriorRental = row.PriorRental.GetValueOrDefault();

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


            viewModel.ManufacturerWarranty = String.IsNullOrEmpty(settingRow.ManufacturerWarranty)
                                                 ? ""
                                                 : settingRow.ManufacturerWarranty;

            viewModel.ManufacturerWarrantyInSpanish = String.IsNullOrEmpty(settingRow.ManufacturerWarrantySpanish)
                                                          ? ""
                                                          : settingRow.ManufacturerWarrantySpanish;

            viewModel.ManufacturerWarrantyDuration = String.IsNullOrEmpty(settingRow.ManufacturerWarrantyDuration)
                                                         ? ""
                                                         : settingRow.ManufacturerWarrantyDuration;


            viewModel.DealerCertified = String.IsNullOrEmpty(settingRow.DealerCertified)
                                            ? ""
                                            : settingRow.DealerCertified;

            viewModel.DealerCertifiedInSpanish = String.IsNullOrEmpty(settingRow.DealerCertifiedSpanish)
                                                     ? ""
                                                     : settingRow.DealerCertifiedSpanish;


            viewModel.DealerCertifiedDuration = String.IsNullOrEmpty(settingRow.DealerCertifiedDuration)
                                                    ? ""
                                                    : settingRow.DealerCertifiedDuration;

            viewModel.ManufacturerCertified = String.IsNullOrEmpty(settingRow.ManufacturerCertified)
                                                  ? ""
                                                  : settingRow.ManufacturerCertified;

            viewModel.ManufacturerCertifiedInSpanish = String.IsNullOrEmpty(settingRow.ManufacturerCertifiedSpanish)
                                                           ? ""
                                                           : settingRow.ManufacturerCertifiedSpanish;

            viewModel.ManufacturerCertifiedDuration = String.IsNullOrEmpty(settingRow.ManufacturerCertifiedDuration)
                                                          ? ""
                                                          : settingRow.ManufacturerCertifiedDuration;

            viewModel.SelectedLanguage = buyerGuide.SelectedLanguage;

            int mileage = 0;

            bool flag = Int32.TryParse(row.Mileage, out mileage);

            if (flag && viewModel.ModelYear >= DateTime.Now.Year - 15 && mileage < 100000)
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

            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

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
            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

                Session["dealerId"] = dealer.DealershipId;
                return Redirect("~/ReportTemplates/PriceRangeReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewPreOwnedInventoryReportTemplate2()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

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

            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

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

            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

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
            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

                Session["dealerId"] = dealer.DealershipId;

                return Redirect("~/ReportTemplates/ReconInventoryReport.aspx");
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
            //if (Session["Dealership"] == null)
            //{
            //    return RedirectToAction("LogOff", "Account");
            //}
            //else
            //{
            //    var context = new whitmanenterprisewarehouseEntities();

            //    var dealer = (DealershipViewModel)Session["Dealership"];

            //    IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
            //        from e in context.whitmanenterprisedealershipinventories
            //        where e.DealershipId == dealer.DealershipId && e.Recon.Value
            //        select e;



            //    var list = new List<CarInfoFormViewModel>();


            //    foreach (var tmp in avaiInventory)
            //    {
            //        var car = new CarInfoFormViewModel()
            //        {
            //            ListingId = tmp.ListingID,
            //            ModelYear = tmp.ModelYear.GetValueOrDefault(),
            //            StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
            //            Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
            //            Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
            //            Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
            //            Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
            //            ChromeStyleId = tmp.ChromeStyleId,
            //            ChromeModelId = tmp.ChromeModelId,
            //            Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
            //            ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
            //            DateInStock = tmp.DateInStock.GetValueOrDefault(),
            //            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days,
            //            SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
            //            Reconstatus = tmp.Recon.GetValueOrDefault()
            //        };
            //        list.Add(car);
            //    }

            //    return File(ReportHelper.ExportToCSV(list, dealer.DealershipId, "Recon Report"), "application/vnd.ms-excel",
            //                "ReconReport.xlsx");


            //}
        }


        public ActionResult ViewCertifiedInventoryReport()
        {
            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();

                var dealer = (DealershipViewModel)Session["Dealership"];

                var rebateInfo = context.vincontrolrebates.FirstOrDefault(x => x.vincontrolrebateid == RebateId);

                IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                    from e in context.whitmanenterprisedealershipinventories
                    where e.DealershipId == dealer.DealershipId && e.ModelYear == rebateInfo.Year && e.Make == rebateInfo.Make && e.Model == rebateInfo.Model && e.Trim == rebateInfo.Trim && e.NewUsed == "New"
                    select e;



                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory)
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.Value,
                        StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                        Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                        Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                        Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                        Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                        SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                        MSRP = String.IsNullOrEmpty(tmp.MSRP) ? "" : tmp.MSRP,
                        ManufacturerRebate = String.IsNullOrEmpty(tmp.ManufacturerRebate) ? "" : tmp.ManufacturerRebate,

                    };
                    list.Add(car);
                }

                return File(ReportHelper.ExportToCSVForRebate(list, dealer.DealershipId, "Rebate Report By Trim"), "application/vnd.ms-excel",
                            "RebateReportByTrim.xlsx");


            }
        }

        public ActionResult ViewPreOwnedMultipleInventoryReport()
        {
            if (Session["DealerGroup"] != null)
            {

                var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                Session["dealerGroupId"] = dealerGroup.DealershipGroupId;

                return Redirect("~/ReportTemplates/UsedMultipleInventoryReport.aspx");
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ViewNewMultipleInventoryReport()
        {
            if (Session["DealerGroup"] != null)
            {

                var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                Session["dealerGroupId"] = dealerGroup.DealershipGroupId;

                return Redirect("~/ReportTemplates/NewMultipleInventoryReport.aspx");
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ViewReconMultipleInventoryReport()
        {
            if (Session["DealerGroup"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var dealerGroup = (DealerGroupViewModel)Session["DealerGroup"];

                var context = new whitmanenterprisewarehouseEntities();

                IEnumerable<Int32> dealerList = from e in context.whitmanenterprisedealerships
                                                where e.DealerGroupID == dealerGroup.DealershipGroupId
                                                select e.idWhitmanenterpriseDealership;

                var avaiInventory = context.whitmanenterprisedealershipinventories.Where(LogicHelper.BuildContainsExpression<whitmanenterprisedealershipinventory, int>(e => e.DealershipId.Value, dealerList));

                var list = new List<CarInfoFormViewModel>();


                foreach (var tmp in avaiInventory.Where(x => x.Recon == true))
                {
                    var car = new CarInfoFormViewModel()
                    {
                        ListingId = tmp.ListingID,
                        ModelYear = tmp.ModelYear.Value,
                        StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                        Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                        Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                        Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                        Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                        ChromeStyleId = tmp.ChromeStyleId,
                        ChromeModelId = tmp.ChromeModelId,
                        Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        DateInStock = tmp.DateInStock.Value,
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                        Reconstatus = tmp.Recon.GetValueOrDefault()
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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            //string warrantyType = "";
            var dealer = (DealershipViewModel) Session["Dealership"];
            var adminBuyerGuideViewModel = new AdminBuyerGuideViewModel();
            var context = new whitmanenterprisewarehouseEntities();

            var row =
                context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == buyerGuide.ListingId);
            var settingRow = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

          
            var warrantyType=0;
            if (row.WarrantyInfo == null)
                warrantyType = 1;
            else
            {
                warrantyType = row.WarrantyInfo.GetValueOrDefault();   
            }
             
          
            if (buyerGuide.SelectedLanguage < 3)
            {

                var buyerGuideSetting = context.vincontrolbuyerguides.FirstOrDefault(bg =>
                                                                                     bg.dealershipId ==
                                                                                     dealer.DealershipId &&
                                                                                     bg.LanguageVersion ==
                                                                                     buyerGuide.SelectedLanguage &&
                                                                                     bg.warrantyType == warrantyType);

                adminBuyerGuideViewModel.SelectedLanguage = buyerGuide.SelectedLanguage;
                if (buyerGuideSetting != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSetting.buyerguideid;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;
                    adminBuyerGuideViewModel.Year = row.ModelYear.GetValueOrDefault().ToString();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                                               ? "NA"
                                                               : row.StockNumber;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSetting.warrantyType.GetValueOrDefault();
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSetting.isServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSetting.isAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSetting.isWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSetting.isFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSetting.isLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSetting.isServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSetting.isMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.systemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.systemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations = ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSetting.percentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSetting.percentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;
                    adminBuyerGuideViewModel.Year = row.ModelYear.GetValueOrDefault().ToString();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                                               ? "NA"
                                                               : row.StockNumber;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = String.IsNullOrEmpty(settingRow.ManufacturerWarrantyDuration)
                                                             ? ""
                                                             : settingRow.ManufacturerWarrantyDuration;
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
            else if (buyerGuide.SelectedLanguage == 3)
            {
                var buyerGuideSettingEnglish = context.vincontrolbuyerguides.FirstOrDefault(bg =>
                                                                                            bg.dealershipId ==
                                                                                            dealer.DealershipId &&
                                                                                            bg.LanguageVersion ==
                                                                                            1 &&
                                                                                            bg.warrantyType == warrantyType);

                var buyerGuideSettingSpanish = context.vincontrolbuyerguides.FirstOrDefault(bg =>
                                                                                            bg.dealershipId ==
                                                                                            dealer.DealershipId &&
                                                                                            bg.LanguageVersion ==
                                                                                            2 &&
                                                                                           bg.warrantyType == warrantyType);


                adminBuyerGuideViewModel.SelectedLanguage = buyerGuide.SelectedLanguage;
                if (buyerGuideSettingEnglish != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSettingEnglish.buyerguideid;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;
                    adminBuyerGuideViewModel.Year = row.ModelYear.GetValueOrDefault().ToString();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                                               ? "NA"
                                                               : row.StockNumber;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSettingEnglish.warrantyType.GetValueOrDefault();
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSettingEnglish.isServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSettingEnglish.isAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSettingEnglish.isWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSettingEnglish.isFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSettingEnglish.isLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSettingEnglish.isServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSettingEnglish.isMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.systemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.systemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingEnglish.durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSettingEnglish.percentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSettingEnglish.percentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitle = row.BrandedTitle.GetValueOrDefault();
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;
                    adminBuyerGuideViewModel.Year = row.ModelYear.GetValueOrDefault().ToString();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                                               ? "NA"
                                                               : row.StockNumber;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType !=1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = String.IsNullOrEmpty(settingRow.ManufacturerWarrantyDuration)
                                                             ? ""
                                                             : settingRow.ManufacturerWarrantyDuration;
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
                    adminBuyerGuideViewModel.Id = buyerGuideSettingSpanish.buyerguideid;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;
                    adminBuyerGuideViewModel.Year = row.ModelYear.GetValueOrDefault().ToString();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                                               ? "NA"
                                                               : row.StockNumber;
                    adminBuyerGuideViewModel.WarrantyTypeSpanish = buyerGuideSettingSpanish.warrantyType.GetValueOrDefault();
                    adminBuyerGuideViewModel.ServiceContractSpanish = buyerGuideSettingSpanish.isServiceContract ??
                                                                      false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = buyerGuideSettingSpanish.isAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarrantySpanish = buyerGuideSettingSpanish.isWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = buyerGuideSettingSpanish.isFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = buyerGuideSettingSpanish.isLimitedWarranty ??
                                                                        false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = buyerGuideSettingSpanish.isServiceContract ??
                                                                        false;
                    adminBuyerGuideViewModel.IsMixedSpanish = buyerGuideSettingSpanish.isMixed ?? false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.systemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.systemCoveredAndDurations);
                    adminBuyerGuideViewModel.DurationsSpanish =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSettingSpanish.durations);
                    adminBuyerGuideViewModel.PercentageOfLaborSpanish = buyerGuideSettingSpanish.percentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPartSpanish = buyerGuideSettingSpanish.percentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRentalSpanish = row.PriorRental.GetValueOrDefault()
                                                                      ? "PRIOR RENTAL"
                                                                      : "";
                    adminBuyerGuideViewModel.IsPriorRentalSpanish = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsBrandedTitlelSpanish = row.BrandedTitle.GetValueOrDefault();

                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.VINNumber) ? "NA" : row.VINNumber;
                    adminBuyerGuideViewModel.Year = row.ModelYear.GetValueOrDefault().ToString();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Make) ? "NA" : row.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Model) ? "NA" : row.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.StockNumber)
                                                               ? "NA"
                                                               : row.StockNumber;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarrantySpanish = String.IsNullOrEmpty(settingRow.AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarrantySpanish = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsLimitedWarrantySpanish = false;
                    adminBuyerGuideViewModel.IsServiceContractSpanish = false;
                    adminBuyerGuideViewModel.SystemCoveredSpanish = string.Empty;
                    adminBuyerGuideViewModel.DurationsSpanish = String.IsNullOrEmpty(settingRow.ManufacturerWarrantyDuration)
                                                             ? ""
                                                             : settingRow.ManufacturerWarrantyDuration;
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

        public ActionResult ViewTodayBucketJumpReport()
        {

            if (Session["Dealership"] != null)
            {

                var dealer = (DealershipViewModel)Session["Dealership"];

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
            var dealer = (DealershipViewModel)Session["Dealership"];

            Session["dealerId"] = dealer.DealershipId;

            return Redirect("~/ReportTemplates/Next7DaysBucketJumpReport.aspx");
        }

        public ActionResult ViewKarpowerReport()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

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
            var dealer = (DealershipViewModel)Session["Dealership"];

            Session["dealerId"] = dealer.DealershipId;

            return Redirect("~/ReportTemplates/ManheimInventoryReport.aspx");
        }

     
    }
}
