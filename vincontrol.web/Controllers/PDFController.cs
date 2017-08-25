using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using HiQPdf;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using vincontrol.Application.Forms.AppraisalManagement;
using vincontrol.Application.Forms.BucketJumpManagementForm;
using vincontrol.Application.Forms.EmailWaitingListManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.VehicleLogManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.InventoryManagement;
using vincontrol.CarFax;
using vincontrol.ConfigurationManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.Manheim;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;
using VINCONTROL.Image;
using Chart = System.Web.UI.DataVisualization.Charting.Chart;
using ChartSelection = vincontrol.DomainObject.ChartSelection;
using Color = System.Drawing.Color;
using CommonHelper = vincontrol.Helper.CommonHelper;
using DataHelper = Vincontrol.Web.HelperClass.DataHelper;
using Font = System.Drawing.Font;
using Inventory = vincontrol.Data.Model.Inventory;
using KarPowerService = vincontrol.KBB.KBBService;
using PdfDocument = HiQPdf.PdfDocument;
using PdfFont = HiQPdf.PdfFont;
using PdfImage = HiQPdf.PdfImage;
using PdfPage = HiQPdf.PdfPage;
using PriceChangeItem = Vincontrol.Web.HelperClass.PriceChangeItem;

namespace Vincontrol.Web.Controllers
{
    public class ReportItem
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }

    public class PDFController : SecurityController
    {
        private ICarFaxService _carFaxService;
        private IInventoryManagementForm _inventoryManagementForm;
        private IEmailWaitingListManagementForm _emailWaitingForm;
        private IVehicleLogManagementForm _vehicleLogManagementForm;
        private IBucketJumpWaitingListManagementForm _bucketJumpWaitingListManagementForm;
        private IAppraisalManagementForm _appraisalManagementForm;

        public PDFController()
        {
            _carFaxService = new CarFaxService();
            _inventoryManagementForm = new InventoryManagementForm();
            _emailWaitingForm = new EmaiLWaitingListManagementForm();
            _vehicleLogManagementForm=new VehicleLogManagementForm();
            _appraisalManagementForm=new AppraisalManagementForm();
            _bucketJumpWaitingListManagementForm=new BucketJumpWaitingListManagementForm();
        }

        #region Actions

        public ActionResult PrintCustomerEmail(int id)
        {
            var dealerSessionInfo = SessionHandler.Dealer;
            MemoryStream workStream = DataHelper.GetCustomerInfoStream(id, dealerSessionInfo);
            HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Report.pdf");
            return new FileStreamResult(workStream, "application/excel");

        }

        public ActionResult PrintFourSquare(int inventoryId, int type)
        {

            var inventoryViewModel = GetFourSquareData(inventoryId, type);

            return GetPDFStreamCustomName(inventoryViewModel, "FourSquare", null, "4Square");

        }


        private CarInfoFormViewModel GetFourSquareData(int inventoryId, int type)
        {
            CarInfoFormViewModel inventoryViewModel;

            if (type == Constanst.CarInfoType.Sold)
            {
                var inventory = _inventoryManagementForm.GetSoldInventory(inventoryId);

                inventoryViewModel = inventory == null
                    ? new CarInfoFormViewModel()
                    : new CarInfoFormViewModel(inventory);
            }
            else
            {
                var inventory =
                     _inventoryManagementForm.GetInventory(inventoryId);
                inventoryViewModel = inventory == null
                    ? new CarInfoFormViewModel()
                    : new CarInfoFormViewModel(inventory);
            }

            inventoryViewModel.RetailPrice = inventoryViewModel.SalePrice * (decimal)1.1;

            inventoryViewModel.Monthsof60Payment =
                Math.Round(AutoLoanHelper.MonthlyPayment((double) inventoryViewModel.SalePrice, 10, 10, 0,
                    (double) inventoryViewModel.SalePrice*0.2, 60), 2);

            inventoryViewModel.Monthsof48Payment =
                Math.Round(AutoLoanHelper.MonthlyPayment((double) inventoryViewModel.SalePrice, 10, 10, 0,
                    (double) inventoryViewModel.SalePrice*0.2, 48), 2);

            inventoryViewModel.Monthsof36Payment =
                Math.Round(AutoLoanHelper.MonthlyPayment((double) inventoryViewModel.SalePrice, 10, 10, 0,
                    (double) inventoryViewModel.SalePrice*0.2, 36), 2);

            return inventoryViewModel;
        }

        public ActionResult PrintExcelCarInfo()
        {
            var result = GetExcelCarData();
            return GetPDFStream(result, "CarExcelInfo", result.DealshipName);
        }

        public ActionResult RenderWalkaroundPhoto(int id)
        {
            var list = GetWalkaroundList(id);
            var walkaroundImage = new WalkaroundImage(Server.MapPath("~/images/car.jpg"), list);
            return File(walkaroundImage.CreateImage(), "image/jpeg");
        }

        public ActionResult PrintPriceTracking(string itemId, ChartTimeType type, int inventoryStatus)
        {
            var dealerSessionInfo = SessionHandler.Dealer;
            return GetPdfStreamForPriceTracking(new PriceChangeViewModel { PriceChangeHistory = DataHelper.GetPriceChangeList(itemId, type, inventoryStatus), Id = itemId }, "PriceTracking", dealerSessionInfo.DealershipName, type, DataHelper.GetCreatedDate(itemId, inventoryStatus).Value, inventoryStatus);
        }

        public ActionResult PrintGraphInfo(string content)
        {
            var result = GetGraphData(content);
            return GetPDFStream(result, "GraphInfo", result.DealshipName);
            //return View("GraphInfo", result);
        }

        public ActionResult PrintAppraisal(int id)
        {
            var result = DataHelper.GetPendingAppraisal(id);
            //return View("InspectionForm", result);
            return GetPDFStreamForAppraisal(result, "InspectionForm");
        }

        public ActionResult ShowAppraisal(int id)
        {
            var result = DataHelper.GetPendingAppraisal(id);
            return View("InspectionForm", result);
        
        }

        public ActionResult PrintAppraisalCarInfo(int? numberOfDay)
        {
            if (numberOfDay == null)
            {
                numberOfDay = 60;
            }

            var result = GetAppraisalData((int)numberOfDay);
            return GetPDFStream(result, "AppraisalInfo", result.DealshipName);
        }

        public ActionResult PrintDetailedAppraisalCarInfo()
        {
            var dealerSessionInfo = SessionHandler.Dealer;
            var result = GetDetailedData();
            return GetPDFStream(result, "ReportDetail", dealerSessionInfo.DealershipName);
        }

        public ActionResult PrintSummaryAppraisalCarInfo()
        {
            var dealerSessionInfo = SessionHandler.Dealer;
            var result = GetSummaryData();
            return GetPDFStream(result, "ReportSummary", dealerSessionInfo.DealershipName);
        }



        public ActionResult ViewSticker(int listingId)
        {

            var result = WindowStickerHelper.BuildWindowStickerInHtml(listingId, SessionHandler.Dealer.DealershipId);
            return View("WindowSticker", result);

        }

        public ActionResult PrintSticker(int listingId)
        {

            var result = WindowStickerHelper.BuildWindowStickerInHtml(listingId, SessionHandler.Dealer.DealershipId);

            _vehicleLogManagementForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                Constanst.VehicleLogSentence.WindowStickerCreatedByUser.Replace("USER",
                    SessionHandler.CurrentUser.FullName), null);


            return GetPDFStream(result, "WindowSticker", null);

        }

        public ActionResult PrintStickerWithTemplate(int listingId, string templateUrl)
        {

            var result = WindowStickerHelper.BuildWindowStickerInHtml(listingId, SessionHandler.Dealer.DealershipId);
            result.TemplateUrl = templateUrl;
            _vehicleLogManagementForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                Constanst.VehicleLogSentence.WindowStickerCreatedByUser.Replace("USER",
                    SessionHandler.CurrentUser.FullName), null);

            return GetPDFStreamForStickerWithTemplate(result, "TemplateWindowSticker", null);
        }

        public ActionResult ViewStickerWithTemplate(int listingId, string templateUrl)
        {

            var result = WindowStickerHelper.BuildWindowStickerInHtml(listingId, SessionHandler.Dealer.DealershipId);
            result.TemplateUrl = templateUrl;
            return View("TemplateWindowSticker", result);

        }

        public ActionResult PrintTradeInCustomer(string period)
        {
            var context = new VincontrolEntities();
            var dealerSessionInfo = SessionHandler.Dealer;
            System.Linq.Expressions.Expression<Func<TradeInCustomer, bool>> query = null;
            DateTime currentStartDate = GetStartDateOfTheWeek(DateTime.Now);
            DateTime currentEndDate = currentStartDate.AddDays(7);
            switch (period)
            {
                case "week":
                    query = e => e.DateStamp.HasValue && e.DealerId == dealerSessionInfo.DealershipId && currentStartDate.CompareTo(e.DateStamp.Value) <= 0 && currentEndDate.CompareTo(e.DateStamp.Value) >= 0;

                    break;
                case "month":
                    query = e => e.DateStamp.HasValue && e.DealerId == dealerSessionInfo.DealershipId && e.DateStamp.Value.Month == DateTime.Now.Month;

                    break;
            }
            var customers = InventoryQueryHelper.GetSingleOrGroupTradein(context).Where(query).ToList().
                Select(e => new TradeinCustomerViewModel
                    {
                        Condition = e.Condition,
                        Date = e.DateStamp.HasValue ? e.DateStamp.Value.ToShortDateString() : String.Empty,
                        Email = e.Email,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Make = e.Make,
                        MileageAdjustment = e.Mileage.HasValue ? e.Mileage.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
                        Model = e.Model,
                        Phone = e.Phone,
                        TradeInStatus = e.TradeInStatus,
                        Year = e.Year.HasValue ? e.Year.Value.ToString(CultureInfo.InvariantCulture) : String.Empty,
                        ID = e.TradeInCustomerId
                    });
            return GetPDFStream(customers, "TradeInCustomer", dealerSessionInfo.DealershipName);
        }

        public ActionResult PrintBuyerGuideSetting(string mode)
        {
            var viewModel = new AdminBuyerGuideViewModel();
            if (Session["BuyerGuideSetting"] != null)
            {
                viewModel = (AdminBuyerGuideViewModel)Session["BuyerGuideSetting"];
            }

            if (mode != null && mode.ToLower() == "debug")
                return View("PrintBuyerGuide", viewModel);
            else
                return GetPdfStreamForBuyerGuide(viewModel, "PrintBuyerGuide", "");
        }

        public ActionResult PrintBuyerGuide()
        {
            var viewModel = new BuyerGuideViewModel();
            if (Session["BuyerGuide"] != null)
                viewModel = (BuyerGuideViewModel)Session["BuyerGuide"];


            return GetPdfStreamForBuyerGuide(viewModel, "PrintBuyerGuide", "");
          
        }
                
        [HttpPost]
        public void StoreKarPowerOptions(/*List<string> options*/string[] options)
        {
            Session["StoreKarPowerOptions"] = options.Aggregate((first, second) => first + ", " + second);//String.IsNullOrEmpty(options) ? null : options.Substring(0, options.Length - 2);//
        }
        
        public ActionResult PrintSilentSalesman(string title, string engine, string additionalOptions, string otherOptions)
        {
            var viewModel = new CarShortViewModel() { Title = title, Engine = engine, AdditonalOptions = String.IsNullOrEmpty(additionalOptions) ? string.Empty : HttpUtility.HtmlDecode(additionalOptions), OtherOptions = String.IsNullOrEmpty(otherOptions) ? string.Empty : HttpUtility.HtmlDecode(otherOptions) };
            string htmlToConvert = PDFRender.RenderViewAsString("SilentSalesman", viewModel, ControllerContext);
            
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            var bytes = pdfDocument.WriteToMemory();
            var workStream = new MemoryStream();
            workStream.Write(bytes, 0, bytes.Length);
            workStream.Position = 0;
            HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + String.Format("{0}.pdf", viewModel.Title));
            return new FileStreamResult(workStream, "application/pdf");
        }

        public ActionResult PrintBucketJumpWithKarPowerOptions(string listingId, string dealer, string price,
            string year, string make, string model, string color, string miles, string plusPrice, bool certified,
            string independentAdd, string certifiedAdd, string wholesaleWithoutOptions, string wholesaleWithOptions,
            string[] options, int chartCarType, string trims, bool isAll, bool isFranchise, bool isIndependant,
            int inventoryType, int ranges, string selectedVin, string image, string marketId, string marketType)
        {
            var workStream = new MemoryStream();

            var listingIdArray = listingId.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            using (var zipFile = new ZipFile())
            {
                foreach (var idString in listingIdArray)
                {
                    var salePrice = Convert.ToDecimal(price);
                    var receivedListingId = Convert.ToInt32(idString);
                    var marketListingId = CommonHelper.RemoveSpecialCharactersAndReturnNumber(marketId);
                    var existingVehicle = inventoryType.Equals(Constanst.VehicleStatus.Appraisal)
                        ? _appraisalManagementForm.GetCarInfo(receivedListingId)
                        : _inventoryManagementForm.GetCarInfo(receivedListingId);
                    var bucketJumpModel = ProcessBucketJumpWithKarPowerOptions(idString, existingVehicle.DealerName,
                        price, year, make, model,
                        color, miles, plusPrice, certified, independentAdd, certifiedAdd, wholesaleWithoutOptions,
                        wholesaleWithOptions, options, chartCarType, trims, isAll, isFranchise, isIndependant,
                        inventoryType,
                        ranges, selectedVin, 0,0, image);
                    bucketJumpModel.CarOnMarket.Dealer = dealer;

                    var chartGraph = new ChartGraph();

                    try
                    {
                        var cs = new ChartSelection
                        {
                            IsAll = isAll,
                            IsFranchise = isFranchise,
                            IsIndependant = isIndependant,
                            Trims = trims
                        };

                        chartGraph = inventoryType.Equals(Constanst.VehicleStatus.Inventory)
                            ? MarketHelper.GetMarketCarsForNationwideMarket(receivedListingId, SessionHandler.Dealer, cs,
                                ranges, chartCarType)
                            : MarketHelper.GetMarketCarsForNationwideMarketForAppraisal(receivedListingId,
                                SessionHandler.Dealer, cs, ranges, chartCarType, false);
                    }
                    catch (Exception ex)
                    {
                        chartGraph.Error = ex.Message;
                    }

                    if (chartGraph != null && chartGraph.ChartModels != null && chartGraph.ChartModels.Count > 0)
                    {
                        if (chartGraph.Target != null)
                        {
                            chartGraph.ChartModels.Add(MarketHelper.ConvertToCarsArrayListItem(chartGraph.Target));
                            salePrice = chartGraph.Target.SalePrice;
                        }

                        bucketJumpModel.ChartGraph = new ChartGraph
                        {
                            ChartModels = chartGraph.ChartModels.OrderBy(i => i[MarketCarFields.Price]).ToList()
                        };
                        //try
                        //{
                        //    var matchingMarketCar = bucketJumpModel.ChartGraph.ChartModels.FirstOrDefault(
                        //            i =>
                        //            i[MarketCarFields.Price].Equals(bucketJumpModel.CarOnMarket.Price) &&
                        //            i[MarketCarFields.Miles].Equals(bucketJumpModel.CarOnMarket.Miles));
                        //}
                        //catch (Exception) { }
                    }

                    var bytes = GetPdfStreamForBucketJump(bucketJumpModel);

                    // store pdf file on hard drive and save history
                    try
                    {
                        VincontrolLinqHelper.SaveBucketJumpHistory(receivedListingId, existingVehicle.DealerId,
                            existingVehicle.DealerName, existingVehicle.Vin, existingVehicle.StockNumber,
                            salePrice, bucketJumpModel.CarOfDealer.SuggestedRetailPrice, existingVehicle.CertifiedAmount,
                            existingVehicle.MileageAdjustment, existingVehicle.Note, ((short) inventoryType), bytes,
                            HttpContext.User.Identity.Name, SessionHandler.CurrentUser.FullName, marketListingId, existingVehicle.DaysAged);
                    }
                    catch (Exception)
                    {
                    }

                    if (listingIdArray.Count().Equals(1))
                    {
                        workStream.Write(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        {
                            var ms = new MemoryStream(bytes);
                            ms.Seek(0, SeekOrigin.Begin);
                            zipFile.AddEntry(
                                String.Format("{0} {1} {2} {3} {4}.pdf", existingVehicle.ModelYear, existingVehicle.Make,
                                    existingVehicle.Model, existingVehicle.Trim.Replace("/", " "),
                                    DateTime.Now.ToString("MMddyyhhmmss")), ms);

                        }
                    }
                }
                zipFile.Save(workStream);
            }

            workStream.Position = 0;

            if (listingIdArray.Count().Equals(1))
            {
                HttpContext.Response.AddHeader("Content-disposition",
                    "attachment; filename=" + String.Format("Bucket Jump Report {0} {1} {2}.pdf", year, make, model));
                return new FileStreamResult(workStream, "application/pdf");
            }

            HttpContext.Response.AddHeader("Content-disposition",
                "attachment; filename=" + String.Format("Bucket Jump Report {0}.zip", DateTime.Now.ToString("MMddyyyy")));
            return new FileStreamResult(workStream, System.Net.Mime.MediaTypeNames.Application.Zip);
        }

        public string PrintExpressBucketJumpWithKarPowerOptions(string listingId, string dealer, string price,
            string year, string make, string model, string color, string miles, string plusPrice, bool certified,
            string independentAdd, string certifiedAdd, string wholesaleWithoutOptions, string wholesaleWithOptions,
            string[] options, int chartCarType, string trims, bool isAll, bool isFranchise, bool isIndependant,
            int inventoryType, int ranges, string selectedVin, string image, string suggestedretailprice, string marketId, string marketType, string mileageadjustmentprice)
        {
            try
            {
                var workStream = new MemoryStream();

                var salePrice = Convert.ToDecimal(price);
                var receivedListingId = Convert.ToInt32(listingId);
                var existingVehicle = inventoryType.Equals(Constanst.VehicleStatus.Appraisal)
                        ? _appraisalManagementForm.GetCarInfo(receivedListingId)
                        : _inventoryManagementForm.GetCarInfo(receivedListingId);
                var suggestedPrice = CommonHelper.RemoveSpecialCharactersAndReturnNumber(suggestedretailprice);
                var mileageAdjustment = CommonHelper.RemoveSpecialCharactersAndReturnNumber(mileageadjustmentprice);
                var marketListingId = CommonHelper.RemoveSpecialCharactersAndReturnNumber(marketId);
                var bucketJumpModel = ProcessBucketJumpWithKarPowerOptions(listingId, existingVehicle.DealerName,
                    price, year, make, model,
                    color, miles, plusPrice, certified, independentAdd, certifiedAdd, wholesaleWithoutOptions,
                    wholesaleWithOptions, options, chartCarType, trims, isAll, isFranchise, isIndependant,
                    inventoryType, ranges, selectedVin, suggestedPrice, mileageAdjustment, image);
                bucketJumpModel.CarOnMarket.Dealer = dealer;


                var chartGraph = new ChartGraph();

                try
                {
                    var cs = new ChartSelection
                    {
                        IsAll = isAll,
                        IsFranchise = isFranchise,
                        IsIndependant = isIndependant,
                        Trims = trims
                    };

                    chartGraph = inventoryType.Equals(Constanst.VehicleStatus.Inventory)
                        ? MarketHelper.GetMarketCarsForNationwideMarket(receivedListingId, SessionHandler.Dealer, cs,
                            ranges, chartCarType)
                        : MarketHelper.GetMarketCarsForNationwideMarketForAppraisal(receivedListingId,
                            SessionHandler.Dealer, cs, ranges, chartCarType, false);
                }
                catch (Exception ex)
                {
                    chartGraph.Error = ex.Message;
                }

                if (chartGraph != null && chartGraph.ChartModels != null && chartGraph.ChartModels.Count > 0)
                {
                    if (chartGraph.Target != null)
                    {
                        chartGraph.ChartModels.Add(MarketHelper.ConvertToCarsArrayListItem(chartGraph.Target));
                        salePrice = chartGraph.Target.SalePrice;
                    }

                    bucketJumpModel.ChartGraph = new ChartGraph
                    {
                        ChartModels = chartGraph.ChartModels.OrderBy(i => i[MarketCarFields.Price]).ToList()
                    };

                }
                var bytes = GetPdfStreamForBucketJump(bucketJumpModel);

                try
                {
                    VincontrolLinqHelper.SaveBucketJumpHistory(receivedListingId, existingVehicle.DealerId,
                            existingVehicle.DealerName, existingVehicle.Vin, existingVehicle.StockNumber,
                            salePrice, bucketJumpModel.CarOfDealer.SuggestedRetailPrice, existingVehicle.CertifiedAmount,
                            existingVehicle.MileageAdjustment,existingVehicle.Note, ((short)inventoryType), bytes,
                            HttpContext.User.Identity.Name, SessionHandler.CurrentUser.FullName, marketListingId, existingVehicle.DaysAged);
                }
                catch (Exception)
                {
                }
                workStream.Write(bytes, 0, bytes.Length);

                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }

        public ActionResult PrintDailyBucketJump(string dealerId, string dealerName, string type)
        {
            try
            {
                var model = new DailyBuckẹtumpHistoryViewModel() { Store = dealerName };
                var list = dealerId.Equals(ConfigurationHandler.Pendragon)
                    ? _inventoryManagementForm.GetDailyBucketJumpHistoryByAllStore((dealerId))
                    : _inventoryManagementForm.GetDailyBucketJumpHistoryBySingleStore(Convert.ToInt32(dealerId));
                model.List = list;
                if (Convert.ToInt32(type).Equals(Constanst.ReportType.Pdf))
                {
                    if (list.Any())
                    {
                        var workStream = new MemoryStream();
                        {
                            //using (var zipFile = new ZipFile())
                            //{
                                byte[] mergedPdf = null;
                                var tempStream = new MemoryStream();
                                {
                                    using (var document = new Document())
                                    {
                                        using (var copy = new PdfCopy(document, tempStream))
                                        {
                                            document.Open();
                                            foreach (var item in list)
                                            {
                                                string filePath =
                                                    System.Web.HttpContext.Current.Server.MapPath("\\BucketJumpReports") +
                                                    "\\" +
                                                    item.DealerId + "\\" +
                                                    (item.VehicleStatusId == Constanst.VehicleStatus.Appraisal
                                                        ? "Appraisal"
                                                        : "Inventory") +
                                                    "\\" + item.ListingId + "\\" + item.AttachFile;


                                                if (System.IO.File.Exists(filePath))
                                                {
                                                    //var bytes = System.IO.File.ReadAllBytes(filePath);
                                                    //var ms = new MemoryStream(bytes);
                                                    //ms.Seek(0, SeekOrigin.Begin);
                                                    //zipFile.AddEntry(item.AttachFile, ms);

                                                    {
                                                        var reader = new PdfReader(filePath);
                                                        // loop over the pages in that document
                                                        int n = reader.NumberOfPages;
                                                        for (int page = 0; page < n;)
                                                        {
                                                            copy.AddPage(copy.GetImportedPage(reader, ++page));
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    //tempStream.Position = 0;
                                    mergedPdf = tempStream.ToArray();
                                    workStream = new MemoryStream(mergedPdf);
                                    workStream.Seek(0, SeekOrigin.Begin);
                                    workStream.Position = 0;
                                    HttpContext.Response.AddHeader("Content-disposition",
                                        "attachment; filename=" +
                                        String.Format("Daily Bucket Jump Report {0}.pdf", DateTime.Now.ToString("MMddyyyy")));
                                    return new FileStreamResult(workStream, "application/pdf");
                                    //zipFile.AddEntry(
                                    //    String.Format("Daily Bucket Jump Report {0}.pdf",
                                    //        DateTime.Now.ToString("MMddyyyy")), ms);
                                    //zipFile.Save(workStream);
                                //}
                            }

                          
                        }
                    }
                    else
                    {
                        var workStream = new MemoryStream {Position = 0};
                        HttpContext.Response.AddHeader("Content-disposition",
                            "attachment; filename=" +
                            String.Format("Daily Bucket Jump Report {0}.pdf", DateTime.Now.ToString("MMddyyyy")));
                        return new FileStreamResult(workStream, "application/pdf");
                    }
                   
                }
                else
                {
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    worksheet.FirstRow().Style.Font.Bold = true;
                    worksheet.Cell("A1").Value = new[]
                    {
                        new
                        {
                            Col1 = "VIN",
                            Col2 = "Stock#",
                            Col3 = "Store",
                            Col4 = "Certified",
                            Col5 = "Misc",
                            Col6 = "Notes",
                            Col7 = "Before Price",
                            Col8 = "After Price",
                            Col9 = "User",
                            Col10 = "Date Stamp",
                            Col11 = "Days Aged",
                            //Col12 = "Vehicle Status"
                        }
                    };

                    if (list.Any())
                    {
                        worksheet.Cell("A2").Value = list
                            .Select(i => new
                            {
                                Col1 = i.VIN,
                                Col2 = i.Stock,
                                Col3 = i.Store,
                                Col4 = i.CertifiedAmount.ToString("c0"),
                                Col5 = i.MileageAdjustment.ToString("c0"),
                                Col6 = i.Note,
                                Col7 = i.SalePrice.ToString("c0"),
                                Col8 = i.RetailPrice.ToString("c0"),
                                Col9 = i.UserFullName,
                                Col10 = i.DateStamp.ToString("MM/dd/yyyy HH:mm:ss"),
                                Col11 = i.DaysAged,
                                //Col12 = i.VehicleStatusName
                            }).OrderByDescending(i => i.Col10);
                    }
                    return new ExcelResult(workbook, String.Format("Daily Bucket Jump Report {0}", DateTime.Now.ToString("ddMMyyHHmmss")));
                }
            }
            catch (Exception)
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
            
        }

        public ActionResult PrintManheimReportDetail(string year, string make, string model, string trim, string regionValue, string regionName, int reportType)
        {
            try
            {
                var manheimReportViewModel = new ManheimReport() { Region = regionName };
                if (SessionHandler.ManheimTransactionsReport != null)
                {
                    manheimReportViewModel.HighestPrice = SessionHandler.ManheimTransactionsReport.HighestPrice;
                    manheimReportViewModel.LowestPrice = SessionHandler.ManheimTransactionsReport.LowestPrice;
                    manheimReportViewModel.AveragePrice = SessionHandler.ManheimTransactionsReport.AveragePrice;
                    manheimReportViewModel.AverageOdometer = SessionHandler.ManheimTransactionsReport.AverageOdometer;
                    manheimReportViewModel.NumberOfTransactions = SessionHandler.ManheimTransactionsReport.NumberOfTransactions;
                    manheimReportViewModel.ManheimTransactions = SessionHandler.ManheimTransactionsReport.ManheimTransactions.OrderByDescending(i => i.SaleDate).ToList();
                }
                else
                {
                    var manheimService = new ManheimService();
                    manheimService.Execute("US", year, make, model, trim, regionValue, 0, 0);
                    manheimReportViewModel.HighestPrice = manheimService.HighPrice;
                    manheimReportViewModel.LowestPrice = manheimService.LowPrice;
                    manheimReportViewModel.AveragePrice = manheimService.AveragePrice;
                    manheimReportViewModel.AverageOdometer = manheimService.AverageOdometer;
                    manheimReportViewModel.NumberOfTransactions = manheimService.NumberOfManheimTransactions;
                    manheimReportViewModel.ManheimTransactions = manheimService.ManheimTransactions.OrderByDescending(i => i.SaleDate).ToList();
                }

                if (reportType==Constanst.ReportType.Pdf)
                {
                    string htmlToConvert = PDFRender.RenderViewAsString("ManheimTransaction", manheimReportViewModel, ControllerContext);

                    //// instantiate the HiQPdf HTML to PDF converter
                    var htmlToPdfConverter = new HtmlToPdf();
                    PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
                    PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                    //AddDateTimeHeader(pdfDocument);
                    var bytes = pdfDocument.WriteToMemory();
                    var workStream = new MemoryStream();
                    workStream.Write(bytes, 0, bytes.Length);
                    workStream.Position = 0;
                    HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + String.Format("Manheim Transaction Report {0}.pdf", DateTime.Now.ToString("ddMMyyHHmmss")));
                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {                                        
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    worksheet.FirstRow().Style.Font.Bold = true;
                    worksheet.Cell("A1").Value = new[] { new { Col1 = "Sale date", Col2 = "Auction", Col3 = "Odometer", Col4 = "Price", Col5 = "Engine", Col6 = "Cond", Col7 = "Color", Col8 = "In Sample"} };
                    worksheet.Cell("A2").Value = manheimReportViewModel.ManheimTransactions.Where(i => i.Price != "0" && i.Odometer != "0")
                        .Select(i => new {
                            Col1 = i.SaleDate,
                            Col2 = i.Auction,
                            Col3 = i.Odometer,
                            Col4 = i.Price,
                            Col5 = i.Engine,
                            Col6 = i.Cond,
                            Col7 = i.Color,
                            Col8 = i.Sample
                        }).OrderByDescending(i => i.Col1);
                    return new ExcelResult(workbook, String.Format("Manheim Transaction Report {0}.xls", DateTime.Now.ToString("ddMMyyHHmmss")));
                }
            }
            catch (Exception) 
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
        }

        public ActionResult PrintManheimPastAuctionReport(int listingId, short vehicleStatus, short auctionRegion, int reportType)
        {
            try
            {
                var manheimReportViewModel = new ManheimReport() { IsAuction = true };
                if (SessionHandler.ManheimPastAuctionsReport != null)
                {
                    manheimReportViewModel.ManheimTransactions = SessionHandler.ManheimPastAuctionsReport.ManheimTransactions;
                }
                else
                {
                    manheimReportViewModel.ManheimTransactions = MarketHelper.GetManheimTransaction(listingId,vehicleStatus, auctionRegion, SessionHandler.Dealer);
                }

                if (reportType==Constanst.ReportType.Pdf)
                {
                    string htmlToConvert = PDFRender.RenderViewAsString("ManheimPastTransaction", manheimReportViewModel, ControllerContext);

                    //// instantiate the HiQPdf HTML to PDF converter
                    var htmlToPdfConverter = new HtmlToPdf();
                    PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
                    PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                    //AddDateTimeHeader(pdfDocument);
                    var bytes = pdfDocument.WriteToMemory();
                    var workStream = new MemoryStream();
                    workStream.Write(bytes, 0, bytes.Length);
                    workStream.Position = 0;
                    HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + String.Format("Manheim Past Auction Report {0}.pdf", DateTime.Now.ToString("ddMMyyHHmmss")));
                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    worksheet.FirstRow().Style.Font.Bold = true;
                    worksheet.Cell("A1").Value = new[] { new { Col1 = "Sale date", Col2 = "Price", Col3 = "Odometer", Col4 = "Vin", Col5 = "Engine", Col6 = "Color", Col7 = "Auction", Col8 = "Region" } };
                    worksheet.Cell("A2").Value = manheimReportViewModel.ManheimTransactions.Select(i => new
                        {
                            Col1 = i.SaleDate,
                            Col2 = i.Price,
                            Col3 = i.Odometer,
                            Col4 = i.Vin,
                            Col5 = i.Engine,
                            Col6 = i.Color,
                            Col7 = i.Auction,
                            Col8 = i.Region
                        }).OrderByDescending(i => i.Col1);
                    return new ExcelResult(workbook, String.Format("Manheim Past Auction Report {0}.xls", DateTime.Now.ToString("ddMMyyHHmmss")));
                }
            }
            catch (Exception)
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
        }

        public ActionResult PrintVehicleLogs(int listingId, int type)
        {
            try
            {
                var vehicleLogs = type == Constanst.CarInfoType.Sold ? _inventoryManagementForm.GetSoldVehicleLogs(listingId).ToList() : _inventoryManagementForm.GetVehicleLogs(listingId).ToList();
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Sheet 1");
                worksheet.FirstRow().Style.Font.Bold = true;
                worksheet.Cell("A1").Value = new[]
                {
                    new
                    {
                        Col1 = "Date Stamp",
                        Col2 = "Description",
                
                    }
                };
                worksheet.Cell("A2").Value = vehicleLogs.Select(i => new
                {
                    Col1 = i.LogDate.ToShortDateString() + " " +i.LogDate.ToShortTimeString(),
                    Col2 = i.Description,
                
                }).OrderBy(i => i.Col1);
                return new ExcelResult(workbook,
                    String.Format("Vehicle Logs {0}.xls", DateTime.Now.ToString("ddMMyyHHmmss")));
            }

            catch (Exception)
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
        }

        public ActionResult PrintStockingGuideAuctionsReport(string make, string model, int reportType)
        {
            try
            {
                var auctions = new List<ManheimRegionVehicle>();
                if (SessionHandler.StockingGuideAuctionsReport != null)
                {
                    auctions = SessionHandler.StockingGuideAuctionsReport.OrderByDescending(i => i.ManheimVehicle.SaleDate).ToList();                    
                }
                
                if (reportType == Constanst.ReportType.Pdf)
                {
                    string htmlToConvert = PDFRender.RenderViewAsString("StockingGuideAuction", auctions, ControllerContext);

                    //// instantiate the HiQPdf HTML to PDF converter
                    var htmlToPdfConverter = new HtmlToPdf();
                    PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
                    PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                    //AddDateTimeHeader(pdfDocument);
                    var bytes = pdfDocument.WriteToMemory();
                    var workStream = new MemoryStream();
                    workStream.Write(bytes, 0, bytes.Length);
                    workStream.Position = 0;
                    HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + String.Format("Stocking Guide Auction Report {0}.pdf", DateTime.Now.ToString("ddMMyyHHmmss")));
                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    worksheet.FirstRow().Style.Font.Bold = true;
                    worksheet.Cell("A1").Value = new[] { new { Col1 = "Year", Col2 = "Make", Col3 = "Model", Col4 = "Trim", Col5 = "Body", Col6 = "Vin", Col7 = "Region", Col8 = "Seller", Col9 = "Miles", Col10 = "MMR", Col11 = "Age", Col12 = "Auction Date" } };
                    worksheet.Cell("A2").Value = auctions.Select(i => new
                    {
                        Col1 = i.ManheimVehicle.Year,
                        Col2 = i.ManheimVehicle.Make,
                        Col3 = i.ManheimVehicle.Model,
                        Col4 = i.ManheimVehicle.Trim,
                        Col5 = i.ManheimVehicle.BodyStyle,
                        Col6 = i.ManheimVehicle.Vin,
                        Col7 = i.Region,
                        Col8 = i.ManheimVehicle.Seller,
                        Col9 = i.ManheimVehicle.Mileage.HasValue ? i.ManheimVehicle.Mileage.Value.ToString("N0") : "0",
                        Col10 = i.ManheimVehicle.Mmr,
                        Col11 = i.ManheimVehicle.DateStamp == null ? "0" : DateTime.Now.Subtract(i.ManheimVehicle.DateStamp.Value).Days.ToString("N0"),
                        Col12 = i.ManheimVehicle.SaleDate == null ? DateTime.Now : i.ManheimVehicle.SaleDate.Value
                    }).OrderByDescending(i => i.Col12);
                    return new ExcelResult(workbook, String.Format("Stocking Guide Auction Report {0}.xls", DateTime.Now.ToString("ddMMyyHHmmss")));
                }
            }
            catch (Exception)
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
        }

        public ActionResult PrintStockingGuideCarsReport(string make, string model, int reportType)
        {
            try
            {
                var cars = new List<Appraisal>();
                if (SessionHandler.StockingGuideCarsReport != null)
                {
                    cars = SessionHandler.StockingGuideCarsReport.OrderByDescending(i => i.AppraisalDate).ToList();
                }

                if (reportType == Constanst.ReportType.Pdf)
                {
                    string htmlToConvert = PDFRender.RenderViewAsString("StockingGuideCar", cars, ControllerContext);

                    //// instantiate the HiQPdf HTML to PDF converter
                    var htmlToPdfConverter = new HtmlToPdf();
                    PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
                    PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                    //AddDateTimeHeader(pdfDocument);
                    var bytes = pdfDocument.WriteToMemory();
                    var workStream = new MemoryStream();
                    workStream.Write(bytes, 0, bytes.Length);
                    workStream.Position = 0;
                    HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + String.Format("Stocking Guide Car Report {0}.pdf", DateTime.Now.ToString("ddMMyyHHmmss")));
                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    worksheet.FirstRow().Style.Font.Bold = true;
                    worksheet.Cell("A1").Value = new[] { new { Col1 = "Year", Col2 = "Make", Col3 = "Model", Col4 = "Trim", Col5 = "Miles", Col6 = "ACV", Col7 = "Age", Col8 = "Color", Col9 = "Owner", Col10 = "Vin", Col11 = "Appraiser", Col12 = "Appraisal Date" } };
                    worksheet.Cell("A2").Value = cars.Select(i => new
                    {
                        Col1 = i.Vehicle.Year,
                        Col2 = i.Vehicle.Make,
                        Col3 = i.Vehicle.Model,
                        Col4 = i.Vehicle.Trim,
                        Col5 = i.Mileage.HasValue ? i.Mileage.Value.ToString("N0"):"0",
                        Col6 = i.ACV.HasValue ? i.ACV.Value.ToString("C0") : "$0",
                        Col7 = i.AppraisalDate == null ? "0" : DateTime.Now.Subtract(i.AppraisalDate.Value).Days.ToString("N0"),
                        Col8 = i.Vehicle.InteriorColor,
                        Col9 = i.Vehicle.Carfaxes == null || i.Vehicle.Carfaxes.FirstOrDefault() == null ? "" : i.Vehicle.Carfaxes.FirstOrDefault().Owner.ToString(),
                        Col10 = i.Vehicle.Vin,
                        Col11 = i.User != null ? i.User.Name : "",
                        Col12 = i.AppraisalDate.Value
                    }).OrderByDescending(i => i.Col12);
                    return new ExcelResult(workbook, String.Format("Stocking Guide Car Report {0}.xls", DateTime.Now.ToString("ddMMyyHHmmss")));
                }
            }
            catch (Exception)
            {
                HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Error.pdf");
                return new FileStreamResult(null, "application/pdf");
            }
        }

        public ActionResult ExpressBucketJumpWithKarPowerOptions(string listingId, string dealer, string price,
            string year, string make, string model, string color, string miles, string plusPrice, bool certified,
            string independentAdd, string certifiedAdd, string wholesaleWithoutOptions, string wholesaleWithOptions,
            string[] options, int chartCarType, string trims, bool isAll, bool isFranchise, bool isIndependant,
            int inventoryType, int ranges, string selectedVin)
        {
            int convertedPlusPrice; Int32.TryParse(SessionHandler.Dealer.IsPendragon ? plusPrice : independentAdd, out convertedPlusPrice);
            int convertedCertifiedAdd; Int32.TryParse(SessionHandler.Dealer.IsPendragon ? "0" : certifiedAdd, out convertedCertifiedAdd);
            
          
            var bucketJumpModel = new BucketJumpViewModel
            {
                CarOfDealer = new DealerCar(),
                CarOnMarket = new DealerCar
                {
                    Dealer = dealer,
                    Year = Convert.ToInt32(year),
                    Make = make,
                    Model = model,
                    Color = color,
                    Price = Convert.ToInt32(price),                    
                    Miles = Convert.ToInt32(miles),
                    Vin = selectedVin == null || selectedVin.Trim() == "null" ? "N/A" : selectedVin
                },
                PlusPrice = convertedPlusPrice
            };

            var receivedListingId = Convert.ToInt32(listingId);

            var appraisal = _appraisalManagementForm.GetAppraisal(receivedListingId);
            if (appraisal != null)
            {
                bucketJumpModel.CarOfDealer = new DealerCar
                {
                    ListingId = appraisal.AppraisalId,
                    Vin = appraisal.Vehicle.Vin,
                    StockNumber = appraisal.Stock,
                    Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                    Make = appraisal.Vehicle.Make,
                    Model = appraisal.Vehicle.Model,
                    Color = appraisal.ExteriorColor,
                    Miles = Convert.ToInt32(appraisal.Mileage),
                    KBBTrimId = appraisal.Vehicle.KBBTrimId.GetValueOrDefault(),
                    DaysInInventory = DateTime.Now.Subtract(appraisal.AppraisalDate.GetValueOrDefault()).Days,
                    Price = appraisal.SalePrice.GetValueOrDefault(),
                    Image = appraisal.Vehicle.DefaultStockImage,
                    ExpandedMileageAdjustment = appraisal.MileageAdjustment.GetValueOrDefault(),
                    CertifiedAmount = appraisal.CertifiedAmount.GetValueOrDefault()
                };
            }
            else
            {
                var inventory = _inventoryManagementForm.GetInventory(receivedListingId);

                if (inventory != null)
                {
                    bucketJumpModel.CarOfDealer = new DealerCar
                    {
                        ListingId = inventory.InventoryId,
                        Vin = inventory.Vehicle.Vin,
                        StockNumber = inventory.Stock,
                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                        Make = inventory.Vehicle.Make,
                        Model = inventory.Vehicle.Model,
                        Color = inventory.ExteriorColor,
                        Miles = Convert.ToInt32(inventory.Mileage),
                        KBBTrimId = inventory.Vehicle.KBBTrimId.GetValueOrDefault(),
                        DaysInInventory = DateTime.Now.Subtract(inventory.DateInStock.GetValueOrDefault()).Days,
                        Price = inventory.SalePrice.GetValueOrDefault(),
                        Image = inventory.Vehicle.DefaultStockImage,
                        ExpandedMileageAdjustment = inventory.MileageAdjustment.GetValueOrDefault(),
                        CertifiedAmount = inventory.CertifiedAmount.GetValueOrDefault()
                    };
                }
            }

            var setting = SessionHandler.Dealer.DealerSetting;
            var karpowerService = new KarPowerService();

            if (setting != null)
            {
                var positiveValue = karpowerService.GetMileageAdjustment(bucketJumpModel.CarOfDealer.Vin, bucketJumpModel.CarOfDealer.Miles.ToString(CultureInfo.InvariantCulture), bucketJumpModel.CarOnMarket.Miles.ToString(CultureInfo.InvariantCulture), DateTime.Now, setting.KellyBlueBook, setting.KellyPassword);

                if (bucketJumpModel.CarOfDealer.Miles < bucketJumpModel.CarOnMarket.Miles)
                {
                    bucketJumpModel.MileageAdjustmentDiff = positiveValue;

                    bucketJumpModel.CarOfDealer.SuggestedRetailPrice = bucketJumpModel.CarOnMarket.Price + positiveValue;
                }
                else if (bucketJumpModel.CarOfDealer.Miles > bucketJumpModel.CarOnMarket.Miles)
                {
                    bucketJumpModel.MileageAdjustmentDiff = positiveValue * (-1);

                    bucketJumpModel.CarOfDealer.SuggestedRetailPrice = bucketJumpModel.CarOnMarket.Price - positiveValue;
                }
                else
                {
                    bucketJumpModel.CarOfDealer.SuggestedRetailPrice = Math.Min(bucketJumpModel.CarOnMarket.Price, bucketJumpModel.CarOfDealer.Price);
                }
                //if (bucketJumpModel.CarOfDealer.CertifiedAmount > 0)
                //{
                //    bucketJumpModel.CarOfDealer.SuggestedRetailPrice += bucketJumpModel.CarOfDealer.CertifiedAmount;
                    
                //}
                //if (bucketJumpModel.CarOfDealer.ExpandedMileageAdjustment > 0)
                //{
                //    bucketJumpModel.CarOfDealer.SuggestedRetailPrice += bucketJumpModel.CarOfDealer.ExpandedMileageAdjustment;
                    
                //}
            }

            return Json(new { sellingPrice = bucketJumpModel.CarOnMarket.Price.ToString("c0"), mileageAdjustment = bucketJumpModel.MileageAdjustmentDiff.ToString("c0"), suggestedPrice = bucketJumpModel.CarOfDealer.SuggestedRetailPrice.ToString("c0") }, JsonRequestBehavior.AllowGet);
        }

        public BucketJumpViewModel ProcessBucketJumpWithKarPowerOptions(string listingId, string dealer, string price, string year, string make, string model, string color, string miles, string plusPrice, bool certified, string independentAdd, string certifiedAdd, string wholesaleWithoutOptions, string wholesaleWithOptions, string[] options, int chartCarType, string trims, bool isAll, bool isFranchise, bool isIndependant, int inventoryType, int ranges, string selectedVin, int suggestedRetailPrice,int mileageadjustmentprice,string image = "")
        {
            int convertedPlusPrice; Int32.TryParse(SessionHandler.Dealer.IsPendragon ? plusPrice : independentAdd, out convertedPlusPrice);
            int convertedCertifiedAdd; Int32.TryParse(SessionHandler.Dealer.IsPendragon ? "0" : certifiedAdd, out convertedCertifiedAdd);
            
            //var context = new VincontrolEntities();
            var bucketJumpModel = new BucketJumpViewModel
            {
                CarOfDealer = new DealerCar(),
                
                CarOnMarket = new DealerCar
                {
                    Dealer = dealer,
                    Year = Convert.ToInt32(year),
                    Make = make,
                    Model = model,
                    Color = color,
                    Price = Convert.ToInt32(price),
                    Miles = Convert.ToInt32(miles),
                    Vin = selectedVin == null || selectedVin.Trim() == "null" ? "N/A" : selectedVin,
                    Image = image
                },
                PlusPrice = convertedPlusPrice
            };

            var receivedListingId = Convert.ToInt32(listingId);

            if (inventoryType.Equals(Constanst.VehicleStatus.Appraisal))//Constanst.CarInfoType.Appraisal
            {
                var appraisal =_appraisalManagementForm.GetAppraisal(receivedListingId);
                if (appraisal != null)
                {
                    bucketJumpModel.CarOfDealer = new DealerCar
                    {
                        ListingId = appraisal.AppraisalId,
                        Vin = appraisal.Vehicle.Vin,
                        StockNumber = appraisal.Stock,
                        Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                        Make = appraisal.Vehicle.Make,
                        Model = appraisal.Vehicle.Model,
                        Color = appraisal.ExteriorColor,
                        Miles = Convert.ToInt32(appraisal.Mileage),
                        KBBTrimId = appraisal.Vehicle.KBBTrimId.GetValueOrDefault(),
                        DaysInInventory = DateTime.Now.Subtract(appraisal.AppraisalDate.GetValueOrDefault()).Days,
                        Price = appraisal.SalePrice.GetValueOrDefault(),
                        Image = appraisal.Vehicle.DefaultStockImage,
                        CertifiedAmount = appraisal.CertifiedAmount.GetValueOrDefault(),
                        ExpandedMileageAdjustment = appraisal.MileageAdjustment.GetValueOrDefault(),
                        ExplainedNote=appraisal.Note
                    };
                }
            }
            else
            {
                var inventory = _inventoryManagementForm.GetInventory(receivedListingId);

                if (inventory != null)
                {
                    bucketJumpModel.CarOfDealer = new DealerCar
                    {
                        ListingId = inventory.InventoryId,
                        Vin = inventory.Vehicle.Vin,
                        StockNumber = inventory.Stock,
                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                        Make = inventory.Vehicle.Make,
                        Model = inventory.Vehicle.Model,
                        Color = inventory.ExteriorColor,
                        Miles = Convert.ToInt32(inventory.Mileage),
                        KBBTrimId = inventory.Vehicle.KBBTrimId.GetValueOrDefault(),
                        DaysInInventory = DateTime.Now.Subtract(inventory.DateInStock.GetValueOrDefault()).Days,
                        Price = inventory.SalePrice.GetValueOrDefault(),
                        Image = inventory.Vehicle.DefaultStockImage,
                        CertifiedAmount = inventory.CertifiedAmount.GetValueOrDefault(),
                        ExpandedMileageAdjustment = inventory.MileageAdjustment.GetValueOrDefault(),
                        ExplainedNote=inventory.Note
                    };
                }
            }

            bucketJumpModel.DealerName = dealer;

            int firstTimeRange = 0;
            int secondTimeRange = 0;
            int interval = 0;
            var setting = SessionHandler.Dealer.DealerSetting;
            if (SessionHandler.Dealer.DealerSetting != null)
            {
                interval = SessionHandler.Dealer.DealerSetting.IntervalBucketJump;
                if (setting.FirstTimeRangeBucketJump == 0 && setting.SecondTimeRangeBucketJump == 0)
                {
                    firstTimeRange = interval;
                    secondTimeRange = interval * 2;
                }
                else if (setting.FirstTimeRangeBucketJump > 0 && setting.SecondTimeRangeBucketJump > 0)
                {
                    firstTimeRange = setting.FirstTimeRangeBucketJump;
                    secondTimeRange = setting.SecondTimeRangeBucketJump;
                }
                else if (setting.FirstTimeRangeBucketJump > 0)
                {
                    firstTimeRange = setting.FirstTimeRangeBucketJump;
                    secondTimeRange = interval + firstTimeRange;
                }
                else if (setting.SecondTimeRangeBucketJump > 0)
                {
                    secondTimeRange = setting.SecondTimeRangeBucketJump;
                    firstTimeRange = secondTimeRange > interval ? secondTimeRange - interval : 0;
                }
            }

            bucketJumpModel = UpdateAvailableDaysBucketJump(bucketJumpModel, firstTimeRange, secondTimeRange, interval);

            var karpowerService = new KarPowerService();

            if (setting != null)
            {
                if (suggestedRetailPrice > 0)
                {
                    bucketJumpModel.CarOfDealer.SuggestedRetailPrice = suggestedRetailPrice;
                    if (bucketJumpModel.CarOfDealer.Miles < bucketJumpModel.CarOnMarket.Miles)
                    {
                        bucketJumpModel.MileageAdjustmentDiff = mileageadjustmentprice;

                    }
                    if (bucketJumpModel.CarOfDealer.Miles > bucketJumpModel.CarOnMarket.Miles)
                    {
                        bucketJumpModel.MileageAdjustmentDiff = mileageadjustmentprice * (-1);

                    }
                   
                }
                else
                {
                    var positiveValue = karpowerService.GetMileageAdjustment(bucketJumpModel.CarOfDealer.Vin, bucketJumpModel.CarOfDealer.Miles.ToString(CultureInfo.InvariantCulture), bucketJumpModel.CarOnMarket.Miles.ToString(CultureInfo.InvariantCulture), DateTime.Now, setting.KellyBlueBook, setting.KellyPassword);

                    if (bucketJumpModel.CarOfDealer.Miles < bucketJumpModel.CarOnMarket.Miles)
                    {
                        bucketJumpModel.MileageAdjustmentDiff = positiveValue;

                        bucketJumpModel.CarOfDealer.SuggestedRetailPrice = bucketJumpModel.CarOnMarket.Price + positiveValue;
                    }
                    else if (bucketJumpModel.CarOfDealer.Miles > bucketJumpModel.CarOnMarket.Miles)
                    {
                        bucketJumpModel.MileageAdjustmentDiff = positiveValue * (-1);

                        bucketJumpModel.CarOfDealer.SuggestedRetailPrice = bucketJumpModel.CarOnMarket.Price - positiveValue;
                    }
                    else
                    {
                        bucketJumpModel.CarOfDealer.SuggestedRetailPrice = Math.Min(bucketJumpModel.CarOnMarket.Price, bucketJumpModel.CarOfDealer.Price);
                    }
                }

             
            }
            
            if (SessionHandler.Dealer.IsPendragon)
            {
                //bucketJumpModel.CarOfDealer.SuggestedRetailPrice += bucketJumpModel.CarOfDealer.CertifiedAmount;
                //bucketJumpModel.CarOfDealer.SuggestedRetailPrice += bucketJumpModel.CarOfDealer.ExpandedMileageAdjustment;

                if(SessionHandler.AllStore){
                var amounts = _inventoryManagementForm.AddMassBucketJump(Convert.ToInt32(listingId), dealer, image, selectedVin,
                    Convert.ToInt32(year), make, model, color,
                    CommonHelper.RemoveSpecialCharactersAndReturnNumber(price),
                    CommonHelper.RemoveSpecialCharactersAndReturnNumber(miles),
                    CommonHelper.RemoveSpecialCharactersAndReturnNumber(plusPrice),
                    CommonHelper.RemoveSpecialCharactersAndReturnNumber(wholesaleWithOptions),
                    CommonHelper.RemoveSpecialCharactersAndReturnNumber(wholesaleWithoutOptions), options != null && options.Any() ? options.First() : string.Empty);
               
                }
            }

            //if (certified && SessionHandler.Dealer.IsPendragon)
            //{
            //    if (!SessionHandler.Dealer.IsPendragonWholesale) bucketJumpModel.CarOfDealer.SuggestedRetailPrice += 2250;
            //}
            //else if (convertedCertifiedAdd > 0)
            //{
            //    bucketJumpModel.CertifiedAdd = convertedCertifiedAdd;
            //    bucketJumpModel.CarOfDealer.SuggestedRetailPrice += convertedCertifiedAdd;
            //}

            try
            {
                bucketJumpModel.IndependentAdd = Convert.ToInt32((convertedPlusPrice) * bucketJumpModel.CarOfDealer.SuggestedRetailPrice) / 100;
                bucketJumpModel.CarOfDealer.SuggestedRetailPrice += bucketJumpModel.IndependentAdd;


                if (!String.IsNullOrEmpty(wholesaleWithOptions) && !String.IsNullOrEmpty(wholesaleWithoutOptions))
                {
                    var wholeSalWithoutOptionsNumber = 0;
                    Int32.TryParse(CommonHelper.RemoveSpecialCharactersForPurePrice(wholesaleWithoutOptions),
                        out wholeSalWithoutOptionsNumber);

                    var wholeSalWithOptionsNumber = 0;
                    Int32.TryParse(CommonHelper.RemoveSpecialCharactersForPurePrice(wholesaleWithOptions),
                        out wholeSalWithOptionsNumber);

                    if (wholeSalWithOptionsNumber > 0)
                        bucketJumpModel.WholeSaleWithOptions = wholeSalWithOptionsNumber;

                    if (wholeSalWithOptionsNumber > 0 && wholeSalWithoutOptionsNumber > 0 &&
                        wholeSalWithOptionsNumber >= wholeSalWithoutOptionsNumber)
                    {

                        bucketJumpModel.OptionsPrice = wholeSalWithOptionsNumber - wholeSalWithoutOptionsNumber;
                        bucketJumpModel.CarOfDealer.SuggestedRetailPrice =
                            bucketJumpModel.CarOfDealer.SuggestedRetailPrice + bucketJumpModel.OptionsPrice;
                    }


                }



            }
            catch (Exception)
            {

            }

            int dayToShow = bucketJumpModel.HighlightedDaysInInventory.LastOrDefault() + interval;
            bucketJumpModel.CarOfDealer.Note = "Less than " + dayToShow + " days";
            if (plusPrice != "0")
                bucketJumpModel.CarOfDealer.Note += ", plus " + plusPrice + "%";
            bucketJumpModel.CarOfDealer.Note += " compare to the price of an independent dealership.";

            if (Session != null && Session["StoreKarPowerOptions"] != null)
            {
                bucketJumpModel.CarOfDealer.Note += "<br/>Additional Options: " + (String)Session["StoreKarPowerOptions"];
                bucketJumpModel.CarOfDealer.Options = String.IsNullOrEmpty((String)Session["StoreKarPowerOptions"]) ? (options != null && options.Any() ? options.First().Split(',').ToArray() : new string[]{}) :  ((String)Session["StoreKarPowerOptions"]).Split(',').ToArray();
                Session["StoreKarPowerOptions"] = null;
            }

            //if (bucketJumpModel.CarOfDealer.CertifiedAmount > 0)
            //{
            //    bucketJumpModel.CarOfDealer.Note += "<br/>Addtional amount due to a certified vehicle: " + bucketJumpModel.CarOfDealer.CertifiedAmount.ToString("C");
            //}

            //if (bucketJumpModel.CarOfDealer.ExpandedMileageAdjustment > 0)
            //{
            //    bucketJumpModel.CarOfDealer.Note += "<br/>Miscellaneous value : " + bucketJumpModel.CarOfDealer.ExpandedMileageAdjustment.ToString("C");
            //}

            if (!String.IsNullOrEmpty(bucketJumpModel.CarOfDealer.ExplainedNote))
            {
                bucketJumpModel.CarOfDealer.Note += "<br/>User note: " + bucketJumpModel.CarOfDealer.ExplainedNote;
            }

            return bucketJumpModel;
        }

        public BucketJumpViewModel UpdateAvailableDaysBucketJump(BucketJumpViewModel bucketJumpModel, int firstTimeRange, int secondTimeRange, int interval)
        {
            bucketJumpModel.AvailableDaysInInventory = new[] { firstTimeRange, secondTimeRange, secondTimeRange + interval, secondTimeRange + interval * 2, secondTimeRange + interval * 3, secondTimeRange + interval * 4, secondTimeRange + interval * 5, secondTimeRange + interval * 6, secondTimeRange + interval * 7, secondTimeRange + interval * 8 };

            if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[0] &
                bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[1])
                bucketJumpModel.HighlightedDaysInInventory = new[] { bucketJumpModel.AvailableDaysInInventory[0] };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[1] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[2])
                bucketJumpModel.HighlightedDaysInInventory = new[] { bucketJumpModel.AvailableDaysInInventory[0], bucketJumpModel.AvailableDaysInInventory[1] };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[2] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[3])
                bucketJumpModel.HighlightedDaysInInventory = new[] { bucketJumpModel.AvailableDaysInInventory[0], bucketJumpModel.AvailableDaysInInventory[1], bucketJumpModel.AvailableDaysInInventory[2] };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[3] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[4])
                bucketJumpModel.HighlightedDaysInInventory = new[] { bucketJumpModel.AvailableDaysInInventory[0], bucketJumpModel.AvailableDaysInInventory[1], bucketJumpModel.AvailableDaysInInventory[2], bucketJumpModel.AvailableDaysInInventory[3] };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[4] & bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[5])
                bucketJumpModel.HighlightedDaysInInventory = new[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2],
                                                                     bucketJumpModel.AvailableDaysInInventory[3],
                                                                     bucketJumpModel.AvailableDaysInInventory[4]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[5] & bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[6])
                bucketJumpModel.HighlightedDaysInInventory = new[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2],
                                                                     bucketJumpModel.AvailableDaysInInventory[3],
                                                                     bucketJumpModel.AvailableDaysInInventory[4],
                                                                     bucketJumpModel.AvailableDaysInInventory[5]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[6] & bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[7])
                bucketJumpModel.HighlightedDaysInInventory = new[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2],
                                                                     bucketJumpModel.AvailableDaysInInventory[3],
                                                                     bucketJumpModel.AvailableDaysInInventory[4],
                                                                     bucketJumpModel.AvailableDaysInInventory[5],
                                                                     bucketJumpModel.AvailableDaysInInventory[6]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[7] & bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[8])
                bucketJumpModel.HighlightedDaysInInventory = new[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2],
                                                                     bucketJumpModel.AvailableDaysInInventory[3],
                                                                     bucketJumpModel.AvailableDaysInInventory[4],
                                                                     bucketJumpModel.AvailableDaysInInventory[5],
                                                                     bucketJumpModel.AvailableDaysInInventory[6],
                                                                     bucketJumpModel.AvailableDaysInInventory[7]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[8] & bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[9])
                bucketJumpModel.HighlightedDaysInInventory = new[] { bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2],
                                                                     bucketJumpModel.AvailableDaysInInventory[3],
                                                                     bucketJumpModel.AvailableDaysInInventory[4],
                                                                     bucketJumpModel.AvailableDaysInInventory[5],
                                                                     bucketJumpModel.AvailableDaysInInventory[6],
                                                                     bucketJumpModel.AvailableDaysInInventory[7],
                                                                     bucketJumpModel.AvailableDaysInInventory[8]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[9])
                bucketJumpModel.HighlightedDaysInInventory = new[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2],
                                                                     bucketJumpModel.AvailableDaysInInventory[3],
                                                                     bucketJumpModel.AvailableDaysInInventory[4],
                                                                     bucketJumpModel.AvailableDaysInInventory[5],
                                                                     bucketJumpModel.AvailableDaysInInventory[6],
                                                                     bucketJumpModel.AvailableDaysInInventory[7],
                                                                     bucketJumpModel.AvailableDaysInInventory[8],
                                                                     bucketJumpModel.AvailableDaysInInventory[9]
                                                                 };
            else
                bucketJumpModel.HighlightedDaysInInventory = new int[] { };

            return bucketJumpModel;
        }

        public ActionResult Flyer()
        {
            return View();
        }

        public ActionResult SendFlyerString(int emailNotificationId)
        {

            var emailNotification = _inventoryManagementForm.GetEmailWaitingList(emailNotificationId);
            if (emailNotification != null)
            {
                var inventoryId = emailNotification.ListingId.GetValueOrDefault();
                if (inventoryId > 0)
                {
                    var existingInventory = _inventoryManagementForm.GetInventory(inventoryId);
                    var content = GenerateFlyerStringContent(existingInventory, emailNotification);

                    return Content(content);
                }

            }
            return Content(string.Empty);

        }

        [HttpPost]
        public void SendFlyer(int inventoryId, string emails, string names)
        {
            
            var emailWaitingList = new EmailWaitingList()
            {
                ListingId = inventoryId,
                DateStamp = DateTime.Now,
                Expire = false,
                NotificationTypeCodeId = Constanst.NotificationType.ShareFyler,
                UserId = SessionHandler.CurrentUser.UserId,
                CustomerEmails = emails


            };
            
            emailWaitingList.CustomerEmails = emails;

            _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);


        }

        [HttpPost]
        public ActionResult SendBrochure(int inventoryId, string emails, string names)
        {

            var existingInventory = _inventoryManagementForm.GetInventory(inventoryId);

            if (existingInventory != null)
            {

                var emailWaitingList = new EmailWaitingList()
                {
                    ListingId = inventoryId,
                    DateStamp = DateTime.Now,
                    Expire = false,
                    NotificationTypeCodeId = Constanst.NotificationType.SendBrochure,
                    UserId = SessionHandler.CurrentUser.UserId,
                    Year = existingInventory.Vehicle.Year,
                    Make = existingInventory.Vehicle.Make,
                    Model = existingInventory.Vehicle.Model,
                    CustomerEmails = emails

                };
                
                _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);

                return Json(new {isExisted = true, message = "success"});
            }
            return Json(new {isExisted = false, message = "fail"});

        }

        public ActionResult Flyer(int inventoryId)
        {

            var inventory = _inventoryManagementForm.GetInventory(inventoryId);
            var inventoryViewModel = inventory == null
                ? new CarInfoFormViewModel()
                : new CarInfoFormViewModel(inventory);

            if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
            {
                try
                {
                    inventoryViewModel.CarFax =
                        _carFaxService.ConvertXmlToCarFaxModelAndSave(inventory.Vehicle.VehicleId,
                            inventory.Vehicle.Vin,
                            SessionHandler.Dealer.DealerSetting.CarFax,
                            SessionHandler.Dealer.DealerSetting.CarFaxPassword);

                }
                catch (Exception)
                {
                }
            }
            else
            {
                var carfax = new CarFaxViewModel {ReportList = new List<CarFaxWindowSticker>(), Success = false};


                inventoryViewModel.CarFax = carfax;

            }

            return View(inventoryViewModel);

        }

        public ActionResult PrintFlyer(int inventoryId)
        {
            return GetStream(GenerateFlyerContent(inventoryId));
        }

        public ActionResult GenerateFlyerView(int inventoryId)
        {
            var existingInventory = _inventoryManagementForm.GetInventory(inventoryId);
            var inventoryViewModel = existingInventory == null
                ? new CarInfoFormViewModel()
                : new CarInfoFormViewModel(existingInventory);

            if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
            {
                try
                {
                    inventoryViewModel.CarFax =
                        _carFaxService.ConvertXmlToCarFaxModelAndSave(existingInventory.Vehicle.VehicleId,
                            existingInventory.Vehicle.Vin,
                            SessionHandler.Dealer.DealerSetting.CarFax,
                            SessionHandler.Dealer.DealerSetting.CarFaxPassword);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                var carfax = new CarFaxViewModel {ReportList = new List<CarFaxWindowSticker>(), Success = false};
                inventoryViewModel.CarFax = carfax;

            }

            return View("Flyer", inventoryViewModel);

        }

        public byte[] GenerateFlyerContent(int inventoryId)
        {
            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();

            try
            {

                var existingInventory = _inventoryManagementForm.GetInventory(inventoryId);
                var inventoryViewModel = existingInventory == null
                    ? new CarInfoFormViewModel()
                    : new CarInfoFormViewModel(existingInventory);

                if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
                {
                    try
                    {
                        inventoryViewModel.CarFax =
                            _carFaxService.ConvertXmlToCarFaxModelAndSave(existingInventory.Vehicle.VehicleId,
                                existingInventory.Vehicle.Vin,
                                SessionHandler.Dealer.DealerSetting.CarFax,
                                SessionHandler.Dealer.DealerSetting.CarFaxPassword);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    var carfax = new CarFaxViewModel {ReportList = new List<CarFaxWindowSticker>(), Success = false};

                    inventoryViewModel.CarFax = carfax;
                  
                }

                string htmlToConvert = PDFRender.RenderViewAsString("Flyer", inventoryViewModel, ControllerContext);

                PDFHelper.ConfigureConverter(htmlToPdfConverter);
                PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
                FormatHeader(pdfDocument, SessionHandler.Dealer.DealershipName);
                return (pdfDocument.WriteToMemory());
            }

            catch (Exception)
            {
                PDFHelper.ConfigureConverter(htmlToPdfConverter);
                PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(string.Empty, null);
                FormatHeader(pdfDocument, SessionHandler.Dealer.DealershipName);
                return (pdfDocument.WriteToMemory());
            }
        }

        public byte[] GenerateFlyerByteContent(string htmlToConvert)
        {
            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();

            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, SessionHandler.Dealer.DealershipName);
            return (pdfDocument.WriteToMemory());
        }

        public string GenerateFlyerStringContent(Inventory inventory)
        {
            try
            {
                {
                    var inventoryViewModel = inventory == null
                                                 ? new CarInfoFormViewModel()
                                                 : new CarInfoFormViewModel(inventory);

                    if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
                    {
                        try
                        {
                            inventoryViewModel.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(inventory.Vehicle.VehicleId, inventory.Vehicle.Vin,
                inventory.Dealer.Setting.CarFax, inventory.Dealer.Setting.CarFaxPassword);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), Success = false };

                        inventoryViewModel.CarFax = carfax;
                  
                    }

                    string htmlToConvert = PDFRender.RenderViewAsString("Flyer", inventoryViewModel, ControllerContext);
                    return htmlToConvert;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GenerateFlyerStringContent(Inventory inventory, EmailWaitingList notificationEmail)
        {
            try
            {
                {
                    var inventoryViewModel = inventory == null
                                                 ? new CarInfoFormViewModel()
                                                 : new CarInfoFormViewModel(inventory);

                    if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
                    {
                        try
                        {
                            inventoryViewModel.CarFax = _carFaxService.GetCarFaxReportInDatabase(inventory.Vehicle.VehicleId);
                            if (!inventoryViewModel.CarFax.Success)
                                inventoryViewModel.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(inventory.Vehicle.VehicleId, inventory.Vehicle.Vin, inventory.Dealer.Setting.CarFax, inventory.Dealer.Setting.CarFaxPassword);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), Success = false };

                        inventoryViewModel.CarFax = carfax;


                    }
                    var searchUser = SQLHelper.CheckUserExistWithStatus(notificationEmail.UserId.GetValueOrDefault());
                    SessionHandler.UserId = searchUser.UserId;
                    SessionHandler.UserRight = new UserRightSetting(searchUser.RoleId);

                    if (searchUser.RoleId == Constanst.RoleType.Master)
                    {
                        SessionHandler.IsMaster = true;

                        var result = SQLHelper.MasterLogin(searchUser);

                        SessionHandler.CurrentUser = searchUser;

                        SessionHandler.Dealer = result.Dealer;

                        SessionHandler.DealerGroup = result.DealerGroup;

                    }

                    else if (searchUser.MultipleDealerLogin)
                    {
                        var result = SQLHelper.LoginMultipleStore(searchUser);

                        SessionHandler.CurrentUser = searchUser;

                        SessionHandler.Dealer = result.Dealer;

                        SessionHandler.DealerGroup = result.DealerGroup;

                        if (searchUser.CanSeeAllStores || searchUser.DefaultLogin == 0)
                        {
                            SessionHandler.Single = false;
                        }

                     
                        SessionHandler.UserRight.UpdateAllSettingsFromDatabase();

                    }
                    else
                    {
                        var dealerLoginResult = SQLHelper.LoginSingleStore(searchUser);

                        SessionHandler.CurrentUser = searchUser;

                        SessionHandler.Dealer = dealerLoginResult.Dealer;
                    }



                    string htmlToConvert = PDFRender.RenderViewAsString("Flyer", inventoryViewModel, ControllerContext);
                    return htmlToConvert;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GenerateBrochureStringContent(Inventory inventory)
        {
            try
            {
                {
                    var inventoryViewModel = inventory == null
                                                 ? new CarInfoFormViewModel()
                                                 : new CarInfoFormViewModel(inventory);

                    if (inventoryViewModel.Condition == Constanst.ConditionStatus.Used)
                    {
                        try
                        {
                            inventoryViewModel.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(inventory.Vehicle.VehicleId, inventory.Vehicle.Vin, SessionHandler.Dealer.DealerSetting.CarFax, SessionHandler.Dealer.DealerSetting.CarFaxPassword);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), Success = false };

                        inventoryViewModel.CarFax = carfax;
                      
                    }

                    string htmlToConvert = PDFRender.RenderViewAsString("brochure", inventoryViewModel, ControllerContext);
                    return htmlToConvert;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public ActionResult ViewFullSticker()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;
            var windowStickerList = new List<WindowStickerViewModel>();

            using (var context = new VincontrolEntities())
            {
                var setting = InventoryQueryHelper.GetSingleOrGroupSetting(context);
                if (setting != null)
                {

                    var rows =
                        InventoryQueryHelper.GetSingleOrGroupInventory(context, true, true, true)
                            .Where(
                                x =>
                                    x.Condition == Constanst.ConditionStatus.Used &&
                                    (x.InventoryStatusCodeId != Constanst.InventoryStatus.Recon))
                            .ToList();
                    windowStickerList.AddRange(
                        rows.Select(
                            inventory => WindowStickerHelper.BuildWsByInventory(inventory, setting.FirstOrDefault())));

                    return GetPDFStream(windowStickerList, "WindowStickerList", null);
                }


            }
            return null;

        }

#endregion

#region Get Data

        private static List<short?> GetMarkedParts(int id)
        {
            using (var context = new VincontrolEntities())
            {
                return
                    context.Walkarounds.Where(i => i.Appraisal.AppraisalId == id)
                        .OrderBy(i => i.Order)
                        .Select(w => w.CarPartId)
                        .ToList();
            }
        }

        private static List<Walkaround> GetWalkarounds(int appraisalID)
        {
            using (var context = new VincontrolEntities())
            {
                return
                    context.Walkarounds.Where(i => i.Appraisal.AppraisalId == appraisalID)
                        .OrderBy(i => i.CarPartId)
                        .ToList();
            }
        }

        private static List<WalkaroundPoint> GetWalkaroundList(int id)
        {
            var context = new VincontrolEntities();
            var walkarounds = context.Walkarounds.Where(i => i.Appraisal.AppraisalId == id).OrderBy(i => i.Order).ToList();
            var list = new List<WalkaroundPoint>();
            if (walkarounds.Count > 0)
            {
                list = walkarounds.Select(w => new WalkaroundPoint
                    {
                        RecTangle = new RectangleF
                            {
                                X = (float)(w.X ?? 0) - 20,
                                Y = (float)(w.Y ?? 0) - 20,
                                Width = 40,
                                Height = 40
                            },
                        Value = w.Order ?? 0
                    }).ToList();
            }

            return list;
        }


        private IEnumerable<ReportSummaryViewModel> GetSummaryData()
        {
            var context = new VincontrolEntities();
            var dealer = SessionHandler.Dealer;
            var appraisalList = GetSummarizedAppraisal(context);
            var inventoryList = GetSummarizedInventory(context);

            var names = inventoryList.Select(i => i.Name)
              .Union(appraisalList.Select(a => a.Name))
              .Distinct();

            var result =
              from name in names
              join i in inventoryList on name equals i.Name into gi
              from iresult in gi.DefaultIfEmpty()
              join a in appraisalList on name equals a.Name into ga
              from aresult in ga.DefaultIfEmpty()
              //where iresult == null ^ aresult == null
              where iresult != null || aresult != null
              select new ReportSummaryViewModel
              {
                  Name = (iresult != null) ? iresult.Name : aresult.Name,
                  NumOfInventory = (iresult != null) ? iresult.Number : 0,
                  NumOfAppraisal = (aresult != null) ? aresult.Number : 0
              };
            return result.ToList().OrderBy(e => e.Name);
        }

        private ReportDetailedViewModel GetDetailedData()
        {
            var context = new VincontrolEntities();

            return new ReportDetailedViewModel
                {
                    Appraisals = GetDetailedAppraisal(context),
                    Inventories = GetDetailedInventory(context)
                };
        }

        private static IEnumerable<InventoryReportItemViewModel> GetDetailedInventory(VincontrolEntities context)
        {
            return InventoryQueryHelper.GetSingleOrGroupInventory(context)
                 .Where(e => e.AddToInventoryById.HasValue)
                 .OrderBy(e => e.AddToInventoryById)
                 .Select(i => new InventoryReportItemViewModel
                 {
                     Name = i.User.Name,
                     Make = i.Vehicle.Make,
                     Model = i.Vehicle.Model,
                     Year = i.Vehicle.Year ?? 0
                 }).ToList();
         
        }

        private static IEnumerable<AppraisalReportItemViewModel> GetDetailedAppraisal(VincontrolEntities context)
        {
            return InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
                                                 .Where(
                                                     e =>
                                                     e.AppraisalById.HasValue &&
                                                     (!e.AppraisalStatusCodeId.HasValue ||
                                                      e.AppraisalStatusCodeId.Value != Constanst.AppraisalStatus.Pending))
                                                 .OrderBy(e => e.AppraisalById)
                                                 .ThenBy(e => e.AppraisalDate.Value)
                                                 .Select(
                                                     i => new AppraisalReportItemViewModel
                                                         {
                                                             Name = i.VinGenieUserId != null ? (i.User == null ? String.Empty : i.User.Name) : (i.User1 == null ? String.Empty : i.User1.Name),
                                                             FullDate = i.AppraisalDate.Value,
                                                             Make = i.Vehicle.Make,
                                                             Model = i.Vehicle.Model,
                                                             Year = i.Vehicle.Year ?? 0
                                                         }
                 ).ToList();
        
        }

        private static List<ReportItem> GetSummarizedInventory(VincontrolEntities context)
        {
            var inventory = InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                                .Where(e => e.AddToInventoryById.HasValue)
                                                .GroupBy(g => g.AddToInventoryById);
            var combineInventory = inventory.Join(context.Users, a => a.Key, b => b.UserId, (a, b) => new ReportItem { Name = b.Name, Number = a.Count() })
                .ToList();
          
            return combineInventory;
        }

        private static List<ReportItem> GetSummarizedAppraisal(VincontrolEntities context)
        {
            var appraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
                .Where(
                    e =>
                    e.AppraisalById.HasValue && (!e.AppraisalStatusCodeId.HasValue || e.AppraisalStatusCodeId.Value != Constanst.AppraisalStatus.Pending))
                .GroupBy(g => g.AppraisalById);


            var combineAppraisal = appraisal.Join(context.Users, a => a.Key, b => b.UserId, (a, b) => new ReportItem { Name = b.Name, Number = a.Count() })
                .ToList();

         
            return combineAppraisal;
        }

        private CarExcelInfoPrintViewModel GetExcelCarData()
        {
            var viewModel = SessionHandler.KpiConditionInventoryList ?? SessionHandler.KpiInventoryList.CarsList;
            var dealer = SessionHandler.Dealer;
            var context = new VincontrolEntities();
            var dtDealerSetting = context.Settings.FirstOrDefault(x => x.DealerId == dealer.DealershipId);

            return new CarExcelInfoPrintViewModel
                {
                    CarInfoList = viewModel.Select(x => dtDealerSetting != null ? x.ConvertToCarExcelViewInfo(dtDealerSetting.DefaultStockImageUrl) : null),
                    DealshipName = dealer.DealershipName
                };
        }

        private AppraisalInfoPrintViewModel GetAppraisalData(int numberOfDay)
        {
            var dealer = SessionHandler.Dealer;
            var context = new VincontrolEntities();
            var dtCompare = DateTime.Now.AddDays(-numberOfDay);
            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            var result =
                InventoryQueryHelper.GetSingleOrGroupAppraisal(context).Where(
                    x => (!x.AppraisalStatusCodeId.HasValue || x.AppraisalStatusCodeId.Value != Constanst.AppraisalStatus.Pending) && x.AppraisalDate.Value > dtCompare).
                    OrderByDescending(x => x.AppraisalDate.Value).ThenBy(x => x.Vehicle.Make).ToList();
            return new AppraisalInfoPrintViewModel
                {
                    CarInfoList = result.Select(x =>
                        {
                            var firstOrDefault = dtDealerSetting.FirstOrDefault(i => x.DealerId == i.DealerId);
                            return firstOrDefault != null ? x.ConvertToAppraisalViewModel(firstOrDefault.DefaultStockImageUrl) : null;
                        }).OrderBy(x => x.Make),
                    DealshipName = dealer.DealershipName,
                    NoOfDays = numberOfDay
                };

        }

        private ContentViewModel GetGraphData(string content)
        {
            var dealer = SessionHandler.Dealer;
            return new ContentViewModel { Text = string.Format("{0}", HttpUtility.HtmlDecode(content)), DealshipName = dealer.DealershipName };
        }

#endregion

#region Common function

      

        private ActionResult GetPDFStream(object result, string viewName, string dealershipName)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            if (!String.IsNullOrEmpty(dealershipName))
                FormatHeader(pdfDocument, dealershipName);

            string szFileName = string.Empty;

            if (result is WindowStickerViewModel)
            {
                szFileName = ((WindowStickerViewModel)result).Title;
            }

            return GetStream(pdfDocument.WriteToMemory(), szFileName);
        }

        private ActionResult GetPDFStreamCustomName(object result, string viewName, string dealershipName, string fileName)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            if (!String.IsNullOrEmpty(dealershipName))
                FormatHeader(pdfDocument, dealershipName);

            return GetStream(pdfDocument.WriteToMemory(), fileName);
        }

        private ActionResult GetPDFStreamForStickerWithTemplate(object result, string viewName, string dealershipName)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureStickerWithTemplateConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            if (!String.IsNullOrEmpty(dealershipName))
                FormatHeader(pdfDocument, dealershipName);

            string szFileName =  ((WindowStickerViewModel)result).Title;

            return GetStream(pdfDocument.WriteToMemory(), szFileName);
        }

        private ActionResult GetPDFStreamForAppraisal(InspectionAppraisalViewModel result, string viewName)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureAppraisalConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            //FormatAppraisalHeader(pdfDocument, dealershipName, false);
            AddCarImage(pdfDocument, result.AppraisalInfo.AppraisalId);
            return GetStream(pdfDocument.WriteToMemory());
        }

        private byte[] GetPdfStreamForBucketJump(BucketJumpViewModel result)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(!SessionHandler.Dealer.IsPendragon ? "NewBucketJump" : "BucketJump", result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            if (SessionHandler.Dealer.IsPendragon) AddLogoDragonImage(pdfDocument);
            AddDateTimeHeader(pdfDocument);
            return pdfDocument.WriteToMemory();
        }

        private byte[] GetPdfStreamForDailyBucketJump(DailyBuckẹtumpHistoryViewModel result)
        {
            string htmlToConvert = PDFRender.RenderViewAsString("DailyBucketJump", result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureBucketJumpConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            if (SessionHandler.Dealer.IsPendragon) AddLogoDragonImage(pdfDocument);
            AddDateTimeHeader(pdfDocument);
            return pdfDocument.WriteToMemory();
        }

        private ActionResult GetPdfStreamForPriceTracking(PriceChangeViewModel result, string viewName, string dealershipName, ChartTimeType chartTimeType, DateTime createdDate, int inventoryStatus)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealershipName);

            var chart = CreateChart(DataHelper.GetPriceChangeListForChart(result.Id, chartTimeType, createdDate, inventoryStatus), RenderType.BinaryStreaming, 1600, 600, chartTimeType, createdDate);
            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                var bitmap = new Bitmap(ms);
                //return File(ms.ToArray(), "image/png", "mychart.png");
                const float carYPos = 0;
                const float carXPos = 50;

                if (pdfDocument.Pages.Count > 0)
                {
                    PdfPage page1 = pdfDocument.Pages[0];
                    //var bitmap = new Bitmap(Server.MapPath("~/images/logo_gragon.jpg"));
                    // layout a PNG image with alpha transparency

                    var transparentPdfImage = new PdfImage(new RectangleF { X = carXPos, Y = carYPos, Width = 500, }, bitmap) { AlphaBlending = true };

                    page1.Layout(transparentPdfImage);
                }
            }



            return GetStream(pdfDocument.WriteToMemory());
        }

        public static DateTime FirstDayOfMonthFromDateTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static Chart CreateChart(IEnumerable<PriceChangeItem> filterResult, RenderType renderType, int width, int height, ChartTimeType chartTimeType, DateTime createdDate)
        {
            var priceHistory = filterResult.GroupBy(i => i.ChangedDate.Date).Select(i => new { i.Key, Item = i }).ToList();
            var chart = new Chart
            {
                Width = width,
                Height = height,
                RenderType = renderType,
                Palette = ChartColorPalette.BrightPastel
            };


            var t = new Title("Price Tracking Change Chart", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(t);

            chart.ChartAreas.Add("Series 1");
            chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.IsMarginVisible = true;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            if (priceHistory.Any())
            {
                switch (chartTimeType)
                {
                    case ChartTimeType.LastMonth:
                        chart.ChartAreas[0].AxisX.Maximum = LastDayOfMonthFromDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).ToOADate();
                        chart.ChartAreas[0].AxisX.Minimum = FirstDayOfMonthFromDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).ToOADate();
                        break;
                    case ChartTimeType.ThisMonth:
                        chart.ChartAreas[0].AxisX.Maximum = LastDayOfMonthFromDateTime(DateTime.Now).ToOADate();
                        chart.ChartAreas[0].AxisX.Minimum = FirstDayOfMonthFromDateTime(DateTime.Now).ToOADate();
                        break;
                    case ChartTimeType.Last7Days:
                        chart.ChartAreas[0].AxisX.Maximum = DateTime.Now.Date.ToOADate();
                        chart.ChartAreas[0].AxisX.Minimum = DateTime.Now.Date.AddDays(-6).ToOADate();
                        break;
                    case ChartTimeType.FromBeginning:
                        chart.ChartAreas[0].AxisX.Interval = (priceHistory.Count / 40) + 1;
                        chart.ChartAreas[0].AxisX.Maximum = priceHistory.Max(i => i.Key).ToOADate();
                        chart.ChartAreas[0].AxisX.Minimum = priceHistory.Min(i => i.Key).ToOADate();
                        break;
                }
                //chart.ChartAreas[0].AxisX.Maximum = priceHistory.Max(i => i.Key).ToOADate();
                //chart.ChartAreas[0].AxisX.Minimum = priceHistory.Min(i => i.Key).ToOADate();
            }


            chart.Series.Add("Series 1");
            chart.Series["Series 1"].IsVisibleInLegend = false;
            //chart.Series["Series 1"].IsValueShownAsLabel = true;

            //if (chartTimeType.Equals(ChartTimeType.ThisMonth) || chartTimeType.Equals(ChartTimeType.LastMonth))
            //{

            //    chart.Series["Series 1"].IsValueShownAsLabel = false;
            //}
            chart.Series["Series 1"].IsValueShownAsLabel = false;
            if (chartTimeType.Equals(ChartTimeType.ThisMonth))
            {
                chart.ChartAreas[0].AxisX.Maximum = LastDayOfMonthFromDateTime(DateTime.Now).ToOADate();
            }
            chart.Series["Series 1"].XValueType = ChartValueType.Date;

            var result =
                priceHistory.Select(value =>
                {
                    var priceChangeItem = value.Item.OrderByDescending(i => i.ChangedDate).FirstOrDefault();
                    return priceChangeItem != null ? new
                    {
                        X = value.Key.ToOADate(),
                        Y = (double)priceChangeItem.ChangedPrice
                    } : null;
                }).ToList();
            foreach (var value in result.OrderBy(i => i.X))
            {
                chart.Series["Series 1"].Points.Add(new DataPoint(value.X, value.Y));
            }


            if (result.Any())
            {
                var max = result.Max(i => i.Y);
                var min = result.Min(i => i.Y);
                chart.ChartAreas[0].AxisY.Maximum = (max == min) ? max + 2 : max;
                chart.ChartAreas[0].AxisY.Minimum = (max == min) ? min - 3 : min;
            }

            chart.Series["Series 1"].ChartType = SeriesChartType.Line;
            chart.Series["Series 1"].Color = Color.Blue;
            chart.Series["Series 1"].BorderWidth = 3;
            chart.Series["Series 1"].Font = new Font("Verdana", 9f, FontStyle.Regular);
            chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart.Legends.Add("Legend1");
            return chart;
        }

       

        private void AddPDFImage(PdfDocument pdfDocument, string name)
        {
            PdfPage page1 = pdfDocument.Pages[0];
            const float carYPos = 45;
            const float carXPos = 495;
            var otherWalkaroundImage =
                new PDFImage(string.Format(Server.MapPath("~/Content/VinControl/InspectionForm/{0}.png"), name));

            // layout a PNG image with alpha transparency
            var otherTransparentPdfImage = new PdfImage(carXPos,
                                                        carYPos, 95, otherWalkaroundImage.CreateBitmapImage())
                                               {
                                                   AlphaBlending
                                                       =
                                                       true
                                               };
            page1.Layout(otherTransparentPdfImage);
        }

        private void AddCarImage(PdfDocument pdfDocument, int id)
        {
            if (pdfDocument.Pages.Count > 0)
            {
                var list = GetWalkarounds(id);
                AddPDFImage(pdfDocument, "img_car");
                List<WalkaroundExt> listExt = new List<WalkaroundExt>();
                foreach (var item in list)
                {
                    WalkaroundExt ext = new WalkaroundExt();
                    ext.Note = item.Note;
                    ext.Order = item.Order != null ? item.Order.Value : 0;
                    ext.CarPartID = item.CarPartId != null ? item.CarPartId.Value : (short)0;
                    switch (item.CarPartId)
                    {
                        case 2: ext.Index = 0;
                            break;
                        case 5: ext.Index = 1;
                            break;
                        case 7: ext.Index = 2;
                            break;
                        case 8: ext.Index = 3;
                            break;
                        case 6: ext.Index = 4;
                            break;
                        case 3: ext.Index = 5;
                            break;
                        case 1: ext.Index = 6;
                            break;
                        case 9: ext.Index = 7;
                            break;
                        case 4: ext.Index = 8;
                            break;
                        default:
                            ext.Index = 0;break;
                    }
                    listExt.Add(ext);
                }
                List<int> listIDs = new List<int>();
                foreach (var item in listExt.OrderBy(x=>x.Index))
                {
                    string name = string.Empty;
                    switch (item.CarPartID)
                    {
                        case 1:
                            if (item.Note != "New" && item.Note != "75%")
                            {
                                name = string.Format("wa_t{0}_on", item.Order);
                                AddPDFImage(pdfDocument, name);
                            }
                            break;
                        case 2: name = "wa_fb_on";
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 3: name = "wa_rb_on";
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 4: name = string.Format("wa_g{0}_on", item.Order);
                            listIDs.Add(item.Order);
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 5: name = "wa_fe_on";
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 6: name = "wa_re_on";
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 7: name = "wa_ds_on";
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 8: name = "wa_ps_on";
                            AddPDFImage(pdfDocument, name);
                            break;
                        case 9: name = string.Format("wa_l{0}_on", item.Order);
                            AddPDFImage(pdfDocument, name);
                            break;
                    }
                }

                for (int i = 1; i < 9; i++)
                {
                    foreach (var item in listIDs)
                    {
                        if (item != i && !listIDs.Contains(i))
                        {
                            listIDs.Add(i);
                            AddPDFImage(pdfDocument, string.Format("wa_g{0}", i));
                            break;
                        }
                    }
                }
               

            }
        }

        private void AddLogoDragonImage(PdfDocument pdfDocument)
        {
            const float carYPos = 45;
            const float carXPos = 20;

            if (pdfDocument.Pages.Count > 0)
            {
                PdfPage page1 = pdfDocument.Pages[0];
                var bitmap = new Bitmap(Server.MapPath("~/images/logo_gragon.jpg"));
                // layout a PNG image with alpha transparency
                var transparentPdfImage = new PdfImage(new RectangleF { X = carXPos, Y = carYPos, Width = 50, Height = 48 }, bitmap)
                    {
                        AlphaBlending = true
                    };

                page1.Layout(transparentPdfImage);
            }
        }

        private ActionResult GetPdfStreamForBuyerGuide(object result, string viewName, string dealershipName)
        {
            string htmlToConvert = PDFRender.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverterForBuyerGuide(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealershipName);
            return GetStream(pdfDocument.WriteToMemory());
        }

        public static void FormatHeader(PdfDocument pdfDocument, string dealershipName)
        {
            FormatHeader(pdfDocument, dealershipName, true);
        }

     
        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName, bool showDateTime)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, 10);
            var sysFont = new Font("Times New Roman", 10, GraphicsUnit.Point);
            //pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);

            if (showDateTime)
            {
                pdfDocument.Header.Layout(new PdfText { Text = DateTime.Now.ToShortDateString(), TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Right });
            }
            pdfDocument.Header.Layout(new PdfText { Text = dealershipName, TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Center });
        }

        private static void AddDateTimeHeader(PdfDocument pdfDocument)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, pdfDocument.Pages[0].DrawableRectangle.Height);
            var sysFont = new Font("Times New Roman", 8, GraphicsUnit.Point);
            //pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);
            pdfDocument.Header.Layout(new PdfText { Text = string.Format("Date: {0}", DateTime.Now.ToShortDateString()), TextFont = pdfFontEmbed, DestX = 500, DestY = 40 });
        }

        private ActionResult GetStream(byte[] pdfBuffer, string szFileName = "", bool isZip = false)
        {
            var workStream = new MemoryStream();
            workStream.Write(pdfBuffer, 0, pdfBuffer.Length);
            workStream.Position = 0;
            
            if (string.IsNullOrEmpty(szFileName)) szFileName = "VINReport.pdf";            

            HttpContext.Response.AddHeader("Content-disposition", "inline; filename=" + szFileName);
            return new FileStreamResult(workStream, "application/pdf");
        }

        private MemoryStream GetStreamFromByte(byte[] pdfBuffer)
        {
            var workStream = new MemoryStream();
            workStream.Write(pdfBuffer, 0, pdfBuffer.Length);
            workStream.Position = 0;
            return workStream;
        }

        private ActionResult GetPDFStream(string result)
        {
            return GetStream(PDFHelper.GeneratePdfFromHtmlCode(result));
        }

        private DateTime GetStartDateOfTheWeek(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return date.AddDays(-1);
                case DayOfWeek.Tuesday:
                    return date.AddDays(-2);
                case DayOfWeek.Wednesday:
                    return date.AddDays(-3);
                case DayOfWeek.Thursday:
                    return date.AddDays(-4);
                case DayOfWeek.Friday:
                    return date.AddDays(-5);
                case DayOfWeek.Saturday:
                    return date.AddDays(-6);
                default:
                    return date;
            }
        }

#endregion
    }

    public enum ChartTimeType
    {
        Last7Days, ThisMonth, LastMonth, FromBeginning
    }

    public class WalkaroundExt
    {
        public short CarPartID { get; set; }
        public int Index { get; set; }
        public string Note { get; set; }
        public int Order { get; set; }
    }
}
