using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.HelperClass;
using HiQPdf;
using System.IO;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.DatabaseModel;
using System.Drawing;
using System.Globalization;
using VINCONTROL.Image;
using System.Drawing.Imaging;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class ReportItem
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }

    public class PDFController : SecurityController
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintCustomerEmail(int id)
        {
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            MemoryStream workStream = DataHelper.GetCustomerInfoStream(id, dealerSessionInfo);
            HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=Report.pdf");
            return new FileStreamResult(workStream, "application/excel");

        }

        public ActionResult PrintExcelCarInfo()
        {
            var result = GetExcelCarData();
            return GetPDFStream(result, "CarExcelInfo", result.DealshipName);
        }

        public ActionResult RenderWalkaroundPhoto(int id)
        {
            var list = GetWalkaroundList(id);
            WalkaroundImage walkaroundImage = new WalkaroundImage(Server.MapPath("~/images/car.jpg"), list);
            return File(walkaroundImage.CreateImage(), "image/jpeg");
        }

        public ActionResult RenderSignature(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                byte[] photo = context.whitmanenterpriseappraisals.Where(a => a.idAppraisal == id).FirstOrDefault().Signature;
                return File(photo, "image/jpeg");
            }
        }

        public ActionResult PrintPriceTracking(string itemId, ChartTimeType type, int inventoryStatus)
        {
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            return GetPDFStreamForPriceTracking(new PriceChangeViewModel() { PriceChangeHistory = DataHelper.GetPriceChangeList(itemId, type, inventoryStatus), Id = itemId }, "PriceTracking", dealerSessionInfo.DealershipName, type, DataHelper.GetCreatedDate(itemId, inventoryStatus).Value, inventoryStatus);
        }

        public ActionResult PrintGraphInfo(string content)
        {
            var result = GetGraphData(content);
            return GetPDFStream(result, "GraphInfo", result.DealshipName);
            //return View("GraphInfo", result);
        }

        public ActionResult PrintAppraisal(int id)
        {
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            var result = DataHelper.GetPendingAppraisal(id);
           
            return GetPDFStreamForAppraisal(result, "InspectionForm", dealerSessionInfo == null ? String.Empty : dealerSessionInfo.DealershipName);
        }

        public ActionResult ShowAppraisal(int id)
        {
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            var result = DataHelper.GetPendingAppraisal(id);
            return View("InspectionForm", result);
            //return GetPDFStreamForAppraisal(result, "InspectionForm", dealerSessionInfo == null ? String.Empty : dealerSessionInfo.DealershipName);
        }

        public ActionResult PrintAppraisalCarInfo(int? NumberOfDay)
        {
            if (NumberOfDay == null)
            {
                NumberOfDay = 60;
            }

            var result = GetAppraisalData((int)NumberOfDay);
            return GetPDFStream(result, "AppraisalInfo", result.DealshipName);
        }

        public ActionResult PrintDetailedAppraisalCarInfo()
        {
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            var result = GetDetailedData();
            return GetPDFStream(result, "ReportDetail", dealerSessionInfo.DealershipName);
        }

        public ActionResult PrintSummaryAppraisalCarInfo()
        {
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            var result = GetSummaryData();
            //return View("ReportSummary", result);
            return GetPDFStream(result, "ReportSummary", dealerSessionInfo.DealershipName);
        }

        public ActionResult PrintTradeInCustomer(string period)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            System.Linq.Expressions.Expression<Func<vincontrolbannercustomer, bool>> query = null;
            DateTime currentStartDate = GetStartDateOfTheWeek(DateTime.Now);
            DateTime currentEndDate = currentStartDate.AddDays(7);
            switch (period)
            {
                case "week":
                    query = e => e.CreatedDate.HasValue && e.DealerId == dealerSessionInfo.DealershipId && currentStartDate.CompareTo(e.CreatedDate.Value) <= 0 && currentEndDate.CompareTo(e.CreatedDate.Value) >= 0;

                    break;
                case "month":
                    query = e => e.CreatedDate.HasValue && e.DealerId == dealerSessionInfo.DealershipId && e.CreatedDate.Value.Month == DateTime.Now.Month;

                    break;
            }
            var customers = InventoryQueryHelper.GetSingleOrGroupTradein(context).Where(query).ToList().
                Select(e => new TradeinCustomerViewModel()
                {
                    Condition = e.Condition,
                    Date = e.CreatedDate.HasValue ? e.CreatedDate.Value.ToShortDateString() : String.Empty,
                    Email = e.Email,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Make = e.Make,
                    MileageAdjustment = e.Mileage.HasValue ? e.Mileage.Value.ToString() : string.Empty,
                    Model = e.Model,
                    Phone = e.Phone,
                    TradeInStatus = e.TradeInStatus,
                    Year = e.Year.HasValue ? e.Year.Value.ToString() : String.Empty,
                    ID = e.TradeInCustomerId
                });
            return GetPDFStream(customers, "TradeInCustomer", dealerSessionInfo.DealershipName);
        }



        public ActionResult PrintBuyerGuideSetting()
        {
            var viewModel = new AdminBuyerGuideViewModel();
            if (Session["BuyerGuideSetting"] != null)
            {
                viewModel = (AdminBuyerGuideViewModel)Session["BuyerGuideSetting"];
            }

            return GetPDFStreamForBuyerGuide(viewModel, "PrintBuyerGuide", "");
            //return View(viewModel);
        }

        public ActionResult PrintBuyerGuide()
        {
            var viewModel = new BuyerGuideViewModel();
            if (Session["BuyerGuide"] != null)
                viewModel = (BuyerGuideViewModel)Session["BuyerGuide"];

            return GetPDFStreamForBuyerGuide(viewModel, "PrintBuyerGuide", "");
            //return View(viewModel);
        }

        //[HttpPost]
        public ActionResult PrintBucketJump(string listingId, string dealer, string price, string year, string make, string model, string color, string miles)
        {
            var type = "Inventory";
            var salePrice = Convert.ToInt32(price);

            var context = new whitmanenterprisewarehouseEntities();
            var bucketJumpModel = new BucketJumpViewModel();
            bucketJumpModel.CarOfDealer = new DealerCar();
            bucketJumpModel.CarOnMarket = new DealerCar()
            {
                Dealer = dealer,
                Year = Convert.ToInt32(year),
                Make = make,
                Model = model,
                Color = color,
                Price = Convert.ToInt32(price),
                Miles = Convert.ToInt32(miles)
            };

            var receivedListingId = Convert.ToInt32(listingId);
            var inventory = context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == receivedListingId).FirstOrDefault();

            var dealerSetting = context.whitmanenterprisesettings.Where(i => i.DealershipId == inventory.DealershipId).First();
            if (inventory != null)
            {
                bucketJumpModel.CarOfDealer = new DealerCar()
                {
                    ListingId = inventory.ListingID,
                    Vin = inventory.VINNumber,
                    StockNumber = inventory.StockNumber,
                    Year = inventory.ModelYear ?? 2012,
                    Make = inventory.Make,
                    Model = inventory.Model,
                    Color = inventory.ExteriorColor,
                    Miles = Convert.ToInt32(inventory.Mileage),
                    KBBTrimId = inventory.KBBTrimId.GetValueOrDefault(),
                    DaysInInventory = DateTime.Now.Subtract(inventory.DateInStock.GetValueOrDefault()).Days
                };
                int dealerPrice = 0;
                Int32.TryParse(String.IsNullOrEmpty(inventory.SalePrice) ? "0" : inventory.SalePrice, out dealerPrice);
                bucketJumpModel.CarOfDealer.Price = dealerPrice;
            }
            else
            {
                var wholesale = context.vincontrolwholesaleinventories.Where(i => i.ListingID == receivedListingId).FirstOrDefault();
                if (wholesale != null)
                {
                    bucketJumpModel.CarOfDealer = new DealerCar()
                    {
                        ListingId = wholesale.ListingID,
                        Vin = inventory.VINNumber,
                        StockNumber = wholesale.StockNumber,
                        Year = wholesale.ModelYear ?? 2012,
                        Make = wholesale.Make,
                        Model = wholesale.Model,
                        Color = wholesale.ExteriorColor,
                        Miles = Convert.ToInt32(wholesale.Mileage),
                        KBBTrimId = inventory.KBBTrimId.GetValueOrDefault(),
                        DaysInInventory = DateTime.Now.Subtract(wholesale.DateInStock.GetValueOrDefault()).Days
                    };

                    int dealerPrice = 0;
                    Int32.TryParse(String.IsNullOrEmpty(inventory.SalePrice) ? "0" : inventory.SalePrice, out dealerPrice);
                    bucketJumpModel.CarOfDealer.Price = dealerPrice;

                    type = "Wholesale";
                }
                else
                {
                    var appraisal = context.whitmanenterpriseappraisals.Where(i => i.idAppraisal == receivedListingId).FirstOrDefault();
                    if (appraisal != null)
                    {
                        bucketJumpModel.CarOfDealer = new DealerCar()
                        {
                            ListingId = appraisal.idAppraisal,
                            Vin = inventory.VINNumber,
                            StockNumber = appraisal.StockNumber,
                            Year = appraisal.ModelYear ?? 2012,
                            Make = appraisal.Make,
                            Model = appraisal.Model,
                            Color = appraisal.ExteriorColor,
                            Miles = Convert.ToInt32(appraisal.Mileage),
                            KBBTrimId = inventory.KBBTrimId.GetValueOrDefault(),
                            DaysInInventory = DateTime.Now.Subtract(appraisal.AppraisalDate.GetValueOrDefault()).Days
                        };

                        int dealerPrice = 0;
                        Int32.TryParse(String.IsNullOrEmpty(inventory.SalePrice) ? "0" : inventory.SalePrice, out dealerPrice);
                        bucketJumpModel.CarOfDealer.Price = dealerPrice;

                        type = "Appraisal";
                    }
                }
            }

            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            bucketJumpModel.DealerName = dealerSessionInfo.DealershipName;

            int firstTimeRange = 0;
            int secondTimeRange = 0;
            int interval = 0;
            var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealerSessionInfo.DealershipId);
            if (setting != null)
            {
                interval = setting.IntervalBucketJump ?? 10;
                if (setting.FirstTimeRangeBucketJump == null && setting.SecondTimeRangeBucketJump == null)
                {
                    firstTimeRange = interval;
                    secondTimeRange = interval * 2;
                }
                else if (setting.FirstTimeRangeBucketJump != null && setting.SecondTimeRangeBucketJump != null)
                {
                    firstTimeRange = setting.FirstTimeRangeBucketJump.Value;
                    secondTimeRange = setting.SecondTimeRangeBucketJump.Value;
                }
                else if (setting.FirstTimeRangeBucketJump != null)
                {
                    firstTimeRange = setting.FirstTimeRangeBucketJump.Value;
                    secondTimeRange = interval + firstTimeRange;
                }
                else if (setting.SecondTimeRangeBucketJump != null)
                {
                    secondTimeRange = setting.SecondTimeRangeBucketJump.Value;
                    firstTimeRange = secondTimeRange > interval ? secondTimeRange - interval : 0;
                }
            }
            bucketJumpModel.AvailableDaysInInventory = new int[]
                                                           {
                                                               firstTimeRange,
                                                               secondTimeRange,
                                                               secondTimeRange + interval,
                                                               secondTimeRange + interval*2,
                                                               secondTimeRange + interval*3,
                                                               secondTimeRange + interval*4,
                                                               secondTimeRange + interval*5,
                                                               secondTimeRange + interval*6,
                                                               secondTimeRange + interval*7,
                                                               secondTimeRange + interval*8
                                                           };


            if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[0] &
                bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[1])
                bucketJumpModel.HighlightedDaysInInventory = new int[] { bucketJumpModel.AvailableDaysInInventory[0] };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[1] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[2])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[2] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[3])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[3] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[4])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[3]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[4] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[5])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[4]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[5] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[6])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[0]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[1]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[2]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[3]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[4]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[5]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[6] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[7])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [6]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[7] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[8])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [6],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [7]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[8] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[9])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [6],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [7],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [8]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[9])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [6],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [7],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [8],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [9]
                                                                 };
            else
                bucketJumpModel.HighlightedDaysInInventory = new int[] { };

            var karpowerService = new KarPowerService();

            var positiveValue = karpowerService.GetMileageAdjustment(bucketJumpModel.CarOfDealer.Vin, bucketJumpModel.CarOfDealer.Miles.ToString(
                                                       CultureInfo.InvariantCulture), bucketJumpModel.CarOnMarket.Miles.ToString(
                                                       CultureInfo.InvariantCulture), DateTime.Now,
                                                   dealerSetting.KellyBlueBook, dealerSetting.KellyPassword);

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
                bucketJumpModel.CarOfDealer.SuggestedRetailPrice = Math.Min(bucketJumpModel.CarOnMarket.Price,
                                                                            bucketJumpModel.CarOfDealer.Price);
            }

            //return View("BucketJump", bucketJumpModel);
            //return GetPDFStreamForBucketJump(bucketJumpModel, "BucketJump");

            string htmlToConvert = PDFHelper.RenderViewAsString("BucketJump", bucketJumpModel, ControllerContext);

            // instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureBucketJumplConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            AddLogoDragonImage(pdfDocument);
            AddDateTimeHeader(pdfDocument);
            var bytes = pdfDocument.WriteToMemory();

            // store pdf file on hard drive and save history
            try
            {
                LinqHelper.SaveBucketJumpHistory(Convert.ToInt32(listingId), SessionHandler.Dealership.DealershipId, salePrice, bucketJumpModel.CarOfDealer.SuggestedRetailPrice, type, bytes, HttpContext.User.Identity.Name);
            }
            catch (Exception)
            {

            }

            return GetStream(bytes);
        }

        [HttpPost]
        public void StoreKarPowerOptions(/*List<string> options*/string[] options)
        {
            Session["StoreKarPowerOptions"] = options.Aggregate<string>((first, second) => first + ", " + second);//String.IsNullOrEmpty(options) ? null : options.Substring(0, options.Length - 2);//
        }

        public ActionResult PrintBucketJumpWithKarPowerOptions(string listingId, string dealer, string price, string year, string make, string model, string color, string miles, string plusPrice, bool certified, string wholesaleWithoutOptions, string wholesaleWithOptions, string[] options)
        {
            var type = "Inventory";
            var salePrice = Convert.ToInt32(price);

            var context = new whitmanenterprisewarehouseEntities();
            var bucketJumpModel = new BucketJumpViewModel();
            bucketJumpModel.CarOfDealer = new DealerCar();
            bucketJumpModel.CarOnMarket = new DealerCar()
            {
                Dealer = dealer,
                Year = Convert.ToInt32(year),
                Make = make,
                Model = model,
                Color = color,
                Price = Convert.ToInt32(price),
                Miles = Convert.ToInt32(miles)
            };

            var receivedListingId = Convert.ToInt32(listingId);
            var inventory = context.whitmanenterprisedealershipinventories.Where(i => i.ListingID == receivedListingId).FirstOrDefault();

            //var dealerSetting = context.whitmanenterprisesettings.Where(i => i.DealershipId == inventory.DealershipId).First();
            if (inventory != null)
            {
                bucketJumpModel.CarOfDealer = new DealerCar()
                {
                    ListingId = inventory.ListingID,
                    Vin = inventory.VINNumber,
                    StockNumber = inventory.StockNumber,
                    Year = inventory.ModelYear ?? 2012,
                    Make = inventory.Make,
                    Model = inventory.Model,
                    Color = inventory.ExteriorColor,
                    Miles = Convert.ToInt32(inventory.Mileage),
                    KBBTrimId = inventory.KBBTrimId.GetValueOrDefault(),
                    DaysInInventory = DateTime.Now.Subtract(inventory.DateInStock.GetValueOrDefault()).Days
                };
                int dealerPrice = 0;
                Int32.TryParse(String.IsNullOrEmpty(inventory.SalePrice) ? "0" : inventory.SalePrice, out dealerPrice);
                bucketJumpModel.CarOfDealer.Price = dealerPrice;

            }
            else
            {
                var wholesale = context.vincontrolwholesaleinventories.Where(i => i.ListingID == receivedListingId).FirstOrDefault();
                if (wholesale != null)
                {
                    bucketJumpModel.CarOfDealer = new DealerCar()
                    {
                        ListingId = wholesale.ListingID,
                        Vin = wholesale.VINNumber,
                        StockNumber = wholesale.StockNumber,
                        Year = wholesale.ModelYear ?? 2012,
                        Make = wholesale.Make,
                        Model = wholesale.Model,
                        Color = wholesale.ExteriorColor,
                        Miles = Convert.ToInt32(wholesale.Mileage),
                        KBBTrimId = wholesale.KBBTrimId.GetValueOrDefault(),
                        DaysInInventory = DateTime.Now.Subtract(wholesale.DateInStock.GetValueOrDefault()).Days
                    };

                    int dealerPrice = 0;
                    Int32.TryParse(String.IsNullOrEmpty(wholesale.SalePrice) ? "0" : wholesale.SalePrice, out dealerPrice);
                    bucketJumpModel.CarOfDealer.Price = dealerPrice;

                    type = "Wholesale";
                }
                else
                {
                    var appraisal = context.whitmanenterpriseappraisals.Where(i => i.idAppraisal == receivedListingId).FirstOrDefault();
                    if (appraisal != null)
                    {
                        bucketJumpModel.CarOfDealer = new DealerCar()
                        {
                            ListingId = appraisal.idAppraisal,
                            Vin = appraisal.VINNumber,
                            StockNumber = appraisal.StockNumber,
                            Year = appraisal.ModelYear ?? 2012,
                            Make = appraisal.Make,
                            Model = appraisal.Model,
                            Color = appraisal.ExteriorColor,
                            Miles = Convert.ToInt32(appraisal.Mileage),
                            KBBTrimId = appraisal.KBBTrimId.GetValueOrDefault(),
                            DaysInInventory = DateTime.Now.Subtract(appraisal.AppraisalDate.GetValueOrDefault()).Days
                        };

                        int dealerPrice = 0;
                        Int32.TryParse(String.IsNullOrEmpty(appraisal.SalePrice) ? "0" : appraisal.SalePrice, out dealerPrice);
                        bucketJumpModel.CarOfDealer.Price = dealerPrice;

                        type = "Appraisal";
                    }
                }
            }

            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            bucketJumpModel.DealerName = dealerSessionInfo.DealershipName;

            int firstTimeRange = 0;
            int secondTimeRange = 0;
            int interval = 0;
            var setting = context.whitmanenterprisesettings.FirstOrDefault(
                i => i.DealershipId == dealerSessionInfo.DealershipId);
            if (setting != null)
            {
                interval = setting.IntervalBucketJump ?? 10;
                if (setting.FirstTimeRangeBucketJump == null && setting.SecondTimeRangeBucketJump == null)
                {
                    firstTimeRange = interval;
                    secondTimeRange = interval * 2;
                }
                else if (setting.FirstTimeRangeBucketJump != null && setting.SecondTimeRangeBucketJump != null)
                {
                    firstTimeRange = setting.FirstTimeRangeBucketJump.Value;
                    secondTimeRange = setting.SecondTimeRangeBucketJump.Value;
                }
                else if (setting.FirstTimeRangeBucketJump != null)
                {
                    firstTimeRange = setting.FirstTimeRangeBucketJump.Value;
                    secondTimeRange = interval + firstTimeRange;
                }
                else if (setting.SecondTimeRangeBucketJump != null)
                {
                    secondTimeRange = setting.SecondTimeRangeBucketJump.Value;
                    firstTimeRange = secondTimeRange > interval ? secondTimeRange - interval : 0;
                }
            }
            bucketJumpModel.AvailableDaysInInventory = new int[]
                                                           {
                                                               firstTimeRange,
                                                               secondTimeRange,
                                                               secondTimeRange + interval,
                                                               secondTimeRange + interval*2,
                                                               secondTimeRange + interval*3,
                                                               secondTimeRange + interval*4,
                                                               secondTimeRange + interval*5,
                                                               secondTimeRange + interval*6,
                                                               secondTimeRange + interval*7,
                                                               secondTimeRange + interval*8
                                                           };


            if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[0] &
                bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[1])
                bucketJumpModel.HighlightedDaysInInventory = new int[] { bucketJumpModel.AvailableDaysInInventory[0] };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[1] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[2])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[2] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[3])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.AvailableDaysInInventory[0],
                                                                     bucketJumpModel.AvailableDaysInInventory[1],
                                                                     bucketJumpModel.AvailableDaysInInventory[2]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory > bucketJumpModel.AvailableDaysInInventory[3] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <= bucketJumpModel.AvailableDaysInInventory[4])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[3]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[4] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[5])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[4]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[5] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[6])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[0]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[1]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[2]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[3]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[4]
                                                                     ,
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory[5]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[6] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[7])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [6]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[7] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[8])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [6],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [7]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[8] &
                     bucketJumpModel.CarOfDealer.DaysInInventory <=
                     bucketJumpModel.AvailableDaysInInventory[9])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [6],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [7],
                                                                     bucketJumpModel.
                                                                         AvailableDaysInInventory
                                                                         [8]
                                                                 };
            else if (bucketJumpModel.CarOfDealer.DaysInInventory >
                     bucketJumpModel.AvailableDaysInInventory[9])
                bucketJumpModel.HighlightedDaysInInventory = new int[]
                                                                 {
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [0],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [1],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [2],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [3],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [4],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [5],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [6],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [7],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [8],
                                                                     bucketJumpModel
                                                                         .
                                                                         AvailableDaysInInventory
                                                                         [9]
                                                                 };
            else
                bucketJumpModel.HighlightedDaysInInventory = new int[] { };

            var karpowerService = new KarPowerService();

            var positiveValue = karpowerService.GetMileageAdjustment(bucketJumpModel.CarOfDealer.Vin, bucketJumpModel.CarOfDealer.Miles.ToString(
                                                       CultureInfo.InvariantCulture), bucketJumpModel.CarOnMarket.Miles.ToString(
                                                       CultureInfo.InvariantCulture), DateTime.Now,
                                                   setting.KellyBlueBook, setting.KellyPassword);

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
                bucketJumpModel.CarOfDealer.SuggestedRetailPrice = Math.Min(bucketJumpModel.CarOnMarket.Price,
                                                                            bucketJumpModel.CarOfDealer.Price);
            }

            if (certified)
                bucketJumpModel.CarOfDealer.SuggestedRetailPrice += 2250;

            try
            {
                bucketJumpModel.CarOfDealer.SuggestedRetailPrice = bucketJumpModel.CarOfDealer.SuggestedRetailPrice + bucketJumpModel.CarOfDealer.SuggestedRetailPrice * Convert.ToInt32(plusPrice) / 100;
                bucketJumpModel.CarOfDealer.SuggestedRetailPrice = bucketJumpModel.CarOfDealer.SuggestedRetailPrice +
                    (Convert.ToInt32(CommonHelper.RemoveSpecialCharactersForPurePrice(wholesaleWithOptions)) -
                    Convert.ToInt32(CommonHelper.RemoveSpecialCharactersForPurePrice(wholesaleWithoutOptions)));
            }
            catch (Exception)
            {

            }

            int dayToShow = bucketJumpModel.HighlightedDaysInInventory.LastOrDefault() + interval;
            bucketJumpModel.CarOfDealer.Note = "Less than " + dayToShow + " days";
            if (plusPrice != "0")
                bucketJumpModel.CarOfDealer.Note += ", plus " + plusPrice + "%";
            bucketJumpModel.CarOfDealer.Note += " compare to the price of an independent dealership.";

            if (Session["StoreKarPowerOptions"] != null)
            {
                bucketJumpModel.CarOfDealer.Note += "<br/>Additional Options: " + (String)Session["StoreKarPowerOptions"];
                Session["StoreKarPowerOptions"] = null;
            }

            // load data from Session if have
            var chartGraph = new ChartGraph();
            if (Session["AutoTrader"] != null)
            {
                chartGraph = (ChartGraph)Session["AutoTrader"];
            }

            try
            {
                chartGraph = LinqHelper.GetMarketCarsOnAutoTraderForLocalMarket(Convert.ToInt32(listingId), (DealershipViewModel)Session["Dealership"]);
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            // keep chart graph with Session
            if (Session["AutoTrader"] == null)
                Session["AutoTrader"] = chartGraph;

            if (chartGraph != null && chartGraph.ChartModels.Count > 0)
            {
                if (chartGraph.Target != null)
                {
                    chartGraph.ChartModels.Add(new ChartModel()
                    {
                        Trims = new List<string>() { chartGraph.Target.Trim },
                        Distance = chartGraph.Target.Distance,
                        Certified = chartGraph.Target.Certified,
                        Seller = chartGraph.Target.Seller,
                        Miles = chartGraph.Target.Mileage,
                        Price = chartGraph.Target.SalePrice,
                        CarsCom = chartGraph.Target.CarsCom,
                        AutoTrader = chartGraph.Target.AutoTrader,
                        IsTargetCar = true
                    });

                    salePrice = chartGraph.Target.SalePrice;
                }

                bucketJumpModel.ChartGraph = new ChartGraph()
                {
                    ChartModels = chartGraph.ChartModels.Where(i => i.Distance <= 100).OrderBy(i => i.Price).ToList()
                };
            }

            //return View("BucketJump", bucketJumpModel);
            //return GetPDFStreamForBucketJump(bucketJumpModel, "BucketJump");

            string htmlToConvert = PDFHelper.RenderViewAsString("BucketJump", bucketJumpModel, ControllerContext);

            // instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureBucketJumplConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            AddLogoDragonImage(pdfDocument);
            AddDateTimeHeader(pdfDocument);
            var bytes = pdfDocument.WriteToMemory();

            // store pdf file on hard drive and save history
            try
            {
                LinqHelper.SaveBucketJumpHistory(Convert.ToInt32(listingId), SessionHandler.Dealership.DealershipId, salePrice, bucketJumpModel.CarOfDealer.SuggestedRetailPrice, type, bytes, HttpContext.User.Identity.Name);
            }
            catch (Exception)
            {

            }

            return GetStream(bytes);
        }

        #endregion

        #region Get Data

        private static List<WalkaroundPoint> GetWalkaroundList(int id)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var walkarounds = context.vincontrolwalkarounds.Where(i => i.whitmanenterpriseappraisal.idAppraisal == id).OrderBy(i => i.order).ToList();
            var list = new List<WalkaroundPoint>();
            if (walkarounds.Count > 0)
            {
                list = walkarounds.Select(w => new WalkaroundPoint()
                {
                    RecTangle = new RectangleF()
                    {
                        X = (float)(w.x ?? 0) - 20,
                        Y = (float)(w.y ?? 0) - 20,
                        Width = 40,
                        Height = 40
                    },
                    Value = w.order ?? 0
                }).ToList();
            }

            return list;
        }


        private IEnumerable<ReportSummaryViewModel> GetSummaryData()
        {
            var context = new whitmanenterprisewarehouseEntities();
            var dealer = (DealershipViewModel)Session["Dealership"];
            var appraisalList = GetSummarizedAppraisal(context, dealer);
            var inventoryList = GetSummarizedInventory(context, dealer);

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
            var context = new whitmanenterprisewarehouseEntities();
            var dealer = (DealershipViewModel)Session["Dealership"];

            return new ReportDetailedViewModel()
            {
                Appraisals = GetDetailedAppraisal(context, dealer),
                Inventories = GetDetailedInventory(context, dealer)
            };
        }

        private static IEnumerable<InventoryReportItemViewModel> GetDetailedInventory(whitmanenterprisewarehouseEntities context, DealershipViewModel dealer)
        {
            var inventory = InventoryQueryHelper.GetSingleOrGroupInventory(context)
                .Where(e => !String.IsNullOrEmpty(e.AddToInventoryBy))
                .OrderBy(e => e.AddToInventoryBy);
            var combinedInventory = inventory.Join(context.whitmanenterpriseusers, a => a.AddToInventoryBy, b => b.UserName, (a, b) => new InventoryReportItemViewModel()
                {
                    Name = b.Name,
                    Make = a.Make,
                    Model = a.Model,
                    Year = a.ModelYear.HasValue ? a.ModelYear.Value : 0
                }).ToList();
            var masterCombinedInventory = inventory.Join(context.whitmanenterprisedealergroups, a => a.AddToInventoryBy, b => b.MasterUserName, (a, b) => new InventoryReportItemViewModel()
            {
                Name = b.DealerGroupName,
                Make = a.Make,
                Model = a.Model,
                Year = a.ModelYear.HasValue ? a.ModelYear.Value : 0
            }).ToList();
            combinedInventory.AddRange(masterCombinedInventory);
            return combinedInventory;
        }

        private static IEnumerable<AppraisalReportItemViewModel> GetDetailedAppraisal(whitmanenterprisewarehouseEntities context, DealershipViewModel dealer)
        {
            var appraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
                .Where(e => !String.IsNullOrEmpty(e.AppraisalBy) && (String.IsNullOrEmpty(e.Status) || e.Status != "Pending"))
                .OrderBy(e => e.AppraisalBy)
                .ThenBy(e => e.AppraisalDate);
            var combinedAppraisal =
                appraisal.Join(context.whitmanenterpriseusers, a => a.AppraisalBy, b => b.UserName, (a, b) => new AppraisalReportItemViewModel()
                {
                    Name = b.Name,
                    FullDate = a.AppraisalDate.Value,
                    Make = a.Make,
                    Model = a.Model,
                    Year = a.ModelYear.HasValue ? a.ModelYear.Value : 0
                }).ToList();
            var mastercombinedAppraisal =
                 appraisal.Join(context.whitmanenterprisedealergroups, a => a.AppraisalBy, b => b.MasterUserName, (a, b) => new AppraisalReportItemViewModel()
                 {
                     Name = b.DealerGroupName,
                     FullDate = a.AppraisalDate.Value,
                     Make = a.Make,
                     Model = a.Model,
                     Year = a.ModelYear.HasValue ? a.ModelYear.Value : 0
                 }).ToList();
            combinedAppraisal.AddRange(mastercombinedAppraisal);
            return combinedAppraisal;
        }

        private static List<ReportItem> GetSummarizedInventory(whitmanenterprisewarehouseEntities context, DealershipViewModel dealer)
        {
            var inventory = InventoryQueryHelper.GetSingleOrGroupInventory(context)
                .Where(e => !String.IsNullOrEmpty(e.AddToInventoryBy))
                .GroupBy(g => g.AddToInventoryBy);
            var combineInventory = inventory.Join(context.whitmanenterpriseusers, a => a.Key, b => b.UserName, (a, b) => new ReportItem() { Name = b.Name, Number = a.Count() })
                .ToList();
            var masterCombineInventory = inventory.Join(context.whitmanenterprisedealergroups, a => a.Key, b => b.MasterUserName, (a, b) => new ReportItem() { Name = b.DealerGroupName, Number = a.Count() })
                .ToList();
            combineInventory.AddRange(masterCombineInventory);
            return combineInventory;
        }

        private static List<ReportItem> GetSummarizedAppraisal(whitmanenterprisewarehouseEntities context, DealershipViewModel dealer)
        {
            var appraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
                .Where(
                    e =>
                    !String.IsNullOrEmpty(e.AppraisalBy) && (String.IsNullOrEmpty(e.Status) || e.Status != "Pending"))
                .GroupBy(g => g.AppraisalBy);


            var combineAppraisal = appraisal.Join(context.whitmanenterpriseusers, a => a.Key, b => b.UserName, (a, b) => new ReportItem() { Name = b.Name, Number = a.Count() })
                .ToList();

            var masterCombinedAppraisal = appraisal.Join(context.whitmanenterprisedealergroups, a => a.Key, b => b.MasterUserName, (a, b) => new ReportItem() { Name = b.DealerGroupName, Number = a.Count() })
                .ToList();

            combineAppraisal.AddRange(masterCombinedAppraisal);
            return combineAppraisal;
        }

        private CarExcelInfoPrintViewModel GetExcelCarData()
        {
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];
            var dealer = (DealershipViewModel)Session["Dealership"];
            var context = new whitmanenterprisewarehouseEntities();
            var dtDealerSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealer.DealershipId);

            return new CarExcelInfoPrintViewModel()
            {
                CarInfoList = viewModel.SubSetList.Select(x => x.ConvertToCarExcelViewInfo(dtDealerSetting.DefaultStockImageUrl)).Cast<CarExcelInfoViewModel>(),
                DealshipName = dealer.DealershipName
            };
        }

        private AppraisalInfoPrintViewModel GetAppraisalData(int numberOfDay)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];
            var context = new whitmanenterprisewarehouseEntities();
            var dtCompare = DateTime.Now.AddDays(-numberOfDay);
            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            var result =
                InventoryQueryHelper.GetSingleOrGroupAppraisal(context).Where(
                    x => (String.IsNullOrEmpty(x.Status) || x.Status != "Pending") && x.AppraisalDate.Value > dtCompare).
                    OrderByDescending(x => x.AppraisalDate).OrderBy(x => x.Make).ToList();
            return new AppraisalInfoPrintViewModel()
            {
                CarInfoList = result.Cast<whitmanenterpriseappraisal>().Select(x => x.ConvertToAppraisalViewModel(dtDealerSetting.FirstOrDefault(i => x.DealershipId == i.DealershipId).DefaultStockImageUrl)).OrderBy(x => x.Make),
                DealshipName = dealer.DealershipName,
                NoOfDays = numberOfDay
            };

        }

        private ContentViewModel GetGraphData(string content)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];
            return new ContentViewModel { Text = string.Format("{0}", HttpUtility.HtmlDecode(content)), DealshipName = dealer.DealershipName };
        }

        #endregion

        #region Common function

        private ActionResult GetPDFStream(object result, string viewName, string dealershipName)
        {
            string htmlToConvert = PDFHelper.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealershipName);
            return GetStream(pdfDocument.WriteToMemory());
        }

        private ActionResult GetPDFStreamForAppraisal(InspectionAppraisalViewModel result, string viewName, string dealershipName)
        {
            string htmlToConvert = PDFHelper.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureAppraisalConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            //FormatAppraisalHeader(pdfDocument, dealershipName, false);
            AddCarImage(pdfDocument, result.AppraisalInfo.AppraisalId);
            return GetStream(pdfDocument.WriteToMemory());
        }

        private ActionResult GetPDFStreamForBucketJump(BucketJumpViewModel result, string viewName)
        {
            string htmlToConvert = PDFHelper.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureBucketJumplConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            AddLogoDragonImage(pdfDocument);
            AddDateTimeHeader(pdfDocument);
            return GetStream(pdfDocument.WriteToMemory());
        }

        private ActionResult GetPDFStreamForPriceTracking(PriceChangeViewModel result, string viewName, string dealershipName, ChartTimeType chartTimeType, DateTime createdDate, int inventoryStatus)
        {
            string htmlToConvert = PDFHelper.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealershipName);

            var chart = CreateChart(DataHelper.GetPriceChangeListForChart(result.Id.ToString(), chartTimeType,createdDate, inventoryStatus), RenderType.BinaryStreaming, 1600, 600, chartTimeType, createdDate);
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
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
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


            var t = new Title("Price Tracking Change Chart", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
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
                        chart.ChartAreas[0].AxisX.Maximum = PDFController.LastDayOfMonthFromDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).ToOADate();
                        chart.ChartAreas[0].AxisX.Minimum = PDFController.FirstDayOfMonthFromDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).ToOADate();
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
            chart.Series["Series 1"].Color = System.Drawing.Color.Blue;
            chart.Series["Series 1"].BorderWidth = 3;
            chart.Series["Series 1"].Font = new Font("Verdana", 9f, FontStyle.Regular);
            chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.Legends.Add("Legend1");
            return chart;
        }

        private static IEnumerable<PriceChangeHistory> GetFilter(object result, ChartTimeType chartTimeType)
        {
            switch (chartTimeType)
            {
                case ChartTimeType.Last7Days:
                    return GetLast7Days(result);
                case ChartTimeType.LastMonth:
                    return GetLastMonth(result);
                default:
                    return GetThisMonth(result);
            }
        }

        private static IEnumerable<PriceChangeHistory> GetLast7Days(object result)
        {
            return ((PriceChangeViewModel)result).PriceChangeHistory.Where(i => i.DateStamp.Date >= DateTime.Now.Date.AddDays(-6));
        }

        private static IEnumerable<PriceChangeHistory> GetThisMonth(object result)
        {
            return ((PriceChangeViewModel)result).PriceChangeHistory.Where(i => i.DateStamp.Month == DateTime.Now.Month && i.DateStamp.Year == DateTime.Now.Year);
        }

        private static IEnumerable<PriceChangeHistory> GetLastMonth(object result)
        {
            return ((PriceChangeViewModel)result).PriceChangeHistory.Where(i => i.DateStamp.Month == DateTime.Now.AddMonths(-1).Month && i.DateStamp.Year == DateTime.Now.AddMonths(-1).Year);
        }

        private void AddCarImage(PdfDocument pdfDocument, int id)
        {
            float carYPos = 45;
            float carXPos = 495;

            if (pdfDocument.Pages.Count > 0)
            {
                PdfPage page1 = pdfDocument.Pages[0];

                var list = GetWalkaroundList(id);
                WalkaroundImage walkaroundImage = new WalkaroundImage(Server.MapPath("~/images/car.jpg"), list);

                // layout a PNG image with alpha transparency
                PdfImage transparentPdfImage = new PdfImage(carXPos,
                        carYPos, 95, walkaroundImage.CreateBitmapImage());
                transparentPdfImage.AlphaBlending = true;

                var context = new whitmanenterprisewarehouseEntities();
                byte[] photo = context.whitmanenterpriseappraisals.Where(a => a.idAppraisal == id).FirstOrDefault().Signature;
                if (photo != null)
                {
                    var ic = new ImageConverter();
                    var img = (System.Drawing.Image)ic.ConvertFrom(photo);

                    var signaturePdfImage = new PdfImage(new RectangleF() { X = 300, Y = 715, Width = 150, Height = 48 }
                       , new Bitmap(img));
                    signaturePdfImage.AlphaBlending = true;
                    page1.Layout(signaturePdfImage);
                }

                page1.Layout(transparentPdfImage);

            }
        }

        private void AddLogoDragonImage(PdfDocument pdfDocument)
        {
            float carYPos = 45;
            float carXPos = 20;

            if (pdfDocument.Pages.Count > 0)
            {
                PdfPage page1 = pdfDocument.Pages[0];
                var bitmap = new Bitmap(Server.MapPath("~/images/logo_gragon.jpg"));
                // layout a PNG image with alpha transparency
                PdfImage transparentPdfImage = new PdfImage(new RectangleF() { X = carXPos, Y = carYPos, Width = 50, Height = 48 }, bitmap);
                transparentPdfImage.AlphaBlending = true;

                page1.Layout(transparentPdfImage);
            }
        }

        private ActionResult GetPDFStreamForBuyerGuide(object result, string viewName, string dealershipName)
        {
            string htmlToConvert = PDFHelper.RenderViewAsString(viewName, result, ControllerContext);

            //// instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            PDFHelper.ConfigureConverterForBuyerGuide(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealershipName);
            return GetStream(pdfDocument.WriteToMemory());
        }
        public static void FormatHeader(PdfDocument pdfDocument, string dealershipName)
        {
            FormatHeader(pdfDocument, dealershipName, true);
        }

        //private static void FormatAppraisalHeader(PdfDocument pdfDocument, string dealershipName, bool showDateTime)
        //{
        //    pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, 10);
        //    Font sysFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
        //    PdfFont pdfFont = pdfDocument.CreateFont(sysFont);
        //    PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);
        //    if (pdfDocument.Pages.Count > 0)
        //    {
        //        //pdfDocument.Pages[0].Layout(new PdfText() { DestY = 2, Text = dealershipName, TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Center });
        //    }
        //}

        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName, bool showDateTime)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, 10);
            Font sysFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            PdfFont pdfFont = pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);

            if (showDateTime)
            {
                pdfDocument.Header.Layout(new PdfText() { Text = DateTime.Now.ToShortDateString(), TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Right });
            }
            pdfDocument.Header.Layout(new PdfText() { Text = dealershipName, TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Center });
        }

        private static void AddDateTimeHeader(PdfDocument pdfDocument)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, pdfDocument.Pages[0].DrawableRectangle.Height);
            Font sysFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            PdfFont pdfFont = pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);
            pdfDocument.Header.Layout(new PdfText() { Text = string.Format("Date: {0}", DateTime.Now.ToShortDateString()), TextFont = pdfFontEmbed, DestX = 500, DestY = 40 });
        }

        private ActionResult GetStream(byte[] pdfBuffer)
        {
            var workStream = new MemoryStream();
            workStream.Write(pdfBuffer, 0, pdfBuffer.Length);
            workStream.Position = 0;
            string fileName = "Report-" + DateTime.Now.Millisecond + ".pdf";
            HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
            return new FileStreamResult(workStream, "application/pdf");
        }

        private ActionResult GetPDFStream(string result)
        {
            return GetStream(PDFHelper.GeneratePDFFromHtmlCode(result));
        }

        private DateTime GetStartDateOfTheWeek(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return date.AddDays(-1);
                    break;
                case DayOfWeek.Tuesday:
                    return date.AddDays(-2);
                    break;
                case DayOfWeek.Wednesday:
                    return date.AddDays(-3);
                    break;
                case DayOfWeek.Thursday:
                    return date.AddDays(-4);
                    break;
                case DayOfWeek.Friday:
                    return date.AddDays(-5);
                    break;
                case DayOfWeek.Saturday:
                    return date.AddDays(-6);
                    break;
                default:
                case DayOfWeek.Sunday:
                    return date;
                    break;
            }
        }

        #endregion
    }

    public enum ChartTimeType
    {
        Last7Days, ThisMonth, LastMonth, FromBeginning
    }
}
