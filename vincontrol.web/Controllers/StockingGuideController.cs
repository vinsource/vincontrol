using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.StockingGuide.Interfaces;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using vincontrol.StockingGuide.Services;
using System.Web.Script.Serialization;
using Vincontrol.Web.Security;


namespace Vincontrol.Web.Controllers
{
    public class LargeBrandJsonResult : JsonResult
    {
        private const string JsonRequestGetNotAllowed =
            "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.";

        public LargeBrandJsonResult()
        {
            MaxJsonLength = Int32.MaxValue;
            RecursionLimit = 10000;
        }

        public int MaxJsonLength { get; set; }
        public int RecursionLimit { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(JsonRequestGetNotAllowed);

            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = MaxJsonLength,
                    RecursionLimit = RecursionLimit
                };
                response.Write(serializer.Serialize(Data));
            }
        }
    }

    public class StockingGuideController : SecurityController
    {
        private const string PermissionCode = "STOCKINGGUIDE";
        private const string AcceptedValues = "ALLACCESS";

        //
        // GET: /StockingGuide/

        readonly IDealerBrandService _dealerBrandService;
        readonly IDealerBrandSelectionService _dealerBrandSelectionService;
        readonly IDealerSegmentService _dealerSegmentService;
        readonly IMarketSegmentDetailService _marketSegmentDetailService;
        readonly IInventorySegmentDetailService _inventorySegmentDetailService;
        readonly IWeeklyTurnOverService _weeklyTurnOverService;

        //readonly IStockingGuideInfoService _stockingGuideInfoService;

        readonly IInventoryService _inventoryService;
        readonly IAppraisalService _appraisalService;
        readonly IManheimVehicleService _manheimVehicleService;
        private readonly IManheimAuctionService _manheimAuctionService;
        private readonly ISoldMarketVehicleService _soldMarketVehicleService;
        private readonly IStateService _stateService;
        private readonly IInventoryStatisticsService _inventoryStatisticsService;

        public StockingGuideController()
        {
            _dealerBrandService = new DealerBrandService();
            _dealerBrandSelectionService = new DealerBrandSelectionService();
            _dealerSegmentService = new DealerSegmentService();
            _marketSegmentDetailService = new MarketSegmentDetailService();
            //_stockingGuideInfoService = new StockingGuideInfoService();
            _inventorySegmentDetailService = new InventorySegmentDetailService();
            _inventoryService = new InventoryService();
            _weeklyTurnOverService = new WeeklyTurnOverService();

            _appraisalService = new AppraisalService();
            _manheimVehicleService = new ManheimVehicleService();
            _manheimAuctionService = new ManheimAuctionService();
            _inventoryStatisticsService = new InventoryStatisticsService();
            _soldMarketVehicleService = new SoldMarketVehicleService();
            _stateService = new StateService();
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult InventoryStatisticCalculator()
        {
            return View();

        }

        public ActionResult GetInventoryStatistics()
        {
            var inventoryStatistic = _inventoryStatisticsService.GetInventoryStatistics(SessionHandler.Dealer.DealershipId);
            var totalSalesValue = _inventoryService.GetTotalSalesValue(SessionHandler.Dealer.DealershipId);
            if (inventoryStatistic == null)
            {
                inventoryStatistic = new SGInventoryStatistic();
                inventoryStatistic.Units = _inventoryService.GetCurrentMonthUsedStock(SessionHandler.Dealer.DealershipId);
                inventoryStatistic.SalesPerMonth = _soldMarketVehicleService.GetHistory(DateTime.Now,
                    SessionHandler.Dealer.DealershipId);
                inventoryStatistic.TotalSalesValue = totalSalesValue == null ? 0 : (int)totalSalesValue.Value;
                inventoryStatistic.AverageCost = 80;
                inventoryStatistic.GrossProfitPerUnit = 12.5;
                inventoryStatistic.TurnRate = 12;
            }
            return Json(new
            {
                funits = inventoryStatistic.Units,
                fsalesPerMonth = inventoryStatistic.SalesPerMonth,
                ftotalSalesValue = inventoryStatistic.TotalSalesValue,
                faverageCost = inventoryStatistic.AverageCost,
                fgrossProfitPerUnit = inventoryStatistic.GrossProfitPerUnit,
                fturnRate = inventoryStatistic.TurnRate

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveInventoryStatisticsValues(SGInventoryStatistic item)
        {
            if (item != null)
            {
                var databaseItem = _inventoryStatisticsService.GetInventoryStatistics(SessionHandler.Dealer.DealershipId);
                if (databaseItem != null)
                {
                    databaseItem.Units = item.Units;
                    databaseItem.SalesPerMonth = item.SalesPerMonth;
                    databaseItem.TotalSalesValue = item.TotalSalesValue;
                    databaseItem.AverageCost = item.AverageCost;
                    databaseItem.GrossProfitPerUnit = item.GrossProfitPerUnit;
                    databaseItem.TurnRate = item.TurnRate;
                    _inventoryStatisticsService.SaveChanges();
                }
                else
                {
                    item.DealerId = SessionHandler.Dealer.DealershipId;
                    _inventoryStatisticsService.AddInventoryStatistics(item);
                }
            }

            return Content("Successful");
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult Index()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            // Reset KPI Condition
            SessionHandler.KpiConditon = 0;

            var context = new VincontrolEntities();

            var dtDealerShip =
                InventoryQueryHelper.GetSingleOrGroupInventory(context)
                                    .Where(
                                        x =>
                                        x.Condition == Constanst.ConditionStatus.Used &&
                                        x.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory);

            var viewModel = new InventoryFormViewModel { IsCompactView = false };

            var list = new List<CarInfoFormViewModel>();

            foreach (var tmp in dtDealerShip)
            {
                var carInfo = new CarInfoFormViewModel(tmp)
                {
                    IsUsed = true
                };

                list.Add(carInfo);

            }

            SessionHandler.KPIViewInfo = new ViewInfo { SortFieldName = "make", IsUp = true };
            viewModel.CarsList = HelperClass.DataHelper.SortList(list, SessionHandler.KPIViewInfo.SortFieldName, SessionHandler.KPIViewInfo.IsUp);

            viewModel.SortSetList = SelectListHelper.InitalSortSetList();

            SessionHandler.KpiInventoryList = viewModel;

            var brandList =
                _dealerBrandService.GetDealerBrandForDealer(SessionHandler.Dealer.DealershipId).Where(x => x.Stock != 0 || x.History != 0 || x.Guide != 0)
                                   .GroupBy(i => i.Make)
                                   .Select(
                                       i =>
                                       new
                                           {
                                               Name = i.Key,
                                               Stock = i.Sum(j => j.Stock),
                                               History = i.Sum(j => j.History),
                                               Profit = i.Sum(j => j.GrossPerUnit)
                                           });
            var segmentList =
                _dealerSegmentService.GetAllDealerSegmentsForDealer(SessionHandler.Dealer.DealershipId).Where(x => x.Stock != 0 || x.History != 0 || x.Guide != 0)
                                   .GroupBy(i => i.SGSegment.Name)
                                   .Select(
                                       i =>
                                       new
                                       {
                                           Name = i.Key,
                                           Stock = i.Sum(j => j.Stock),
                                           History = i.Sum(j => j.History),
                                           Profit = i.Sum(j => j.GrossPerUnit)
                                       });

            var segmentInfos = new List<SegmentInfo>();
            foreach (var info in brandList)
            {
                var segmentInfo = new SegmentInfo();
                segmentInfo.Name = info.Name;
                segmentInfo.History = info.History;
                segmentInfo.Stock = info.Stock;
                segmentInfo.Supply = (info.History == 0 || info.Stock == 0)
                                         ? 0
                                         : (int)Math.Ceiling(Convert.ToDecimal(((double)info.Stock / (double)info.History) * 30));
                segmentInfo.Turn = (info.History == 0 || info.Stock == 0)
                                       ? 0
                                       : Math.Ceiling(Convert.ToDecimal(((double)info.History / (double)info.Stock) * 12));
                segmentInfo.StrTurn = segmentInfo.Turn.ToString("0.00");
                segmentInfo.Profit = info.Profit;
                segmentInfo.Type = 1;

                segmentInfos.Add(segmentInfo);
            }

            foreach (var info in segmentList)
            {
                var segmentInfo = new SegmentInfo();
                segmentInfo.Name = info.Name;
                segmentInfo.History = info.History;
                segmentInfo.Stock = info.Stock;
                segmentInfo.Supply = (info.History == 0 || info.Stock == 0)
                                         ? 0
                                         : (int)Math.Ceiling(Convert.ToDecimal(((double)info.Stock / (double)info.History) * 30));
                segmentInfo.Turn = (info.History == 0 || info.Stock == 0)
                                       ? 0
                                       : Math.Ceiling(Convert.ToDecimal(((double)info.History / (double)info.Stock) * 12));
                segmentInfo.StrTurn = segmentInfo.Turn.ToString("0.00");
                segmentInfo.Profit = info.Profit;
                segmentInfo.Type = 2;

                segmentInfos.Add(segmentInfo);
            }

            ViewData["Segments"] = segmentInfos.OrderBy(x => x.Type).ThenBy(x => x.Name).ToList();

            return View(viewModel);
        }

        public ActionResult GetChartData()
        {
            if (SessionHandler.Dealer != null)
            {
                var chartData = _weeklyTurnOverService.GetTurnOverListByMonth(DateTime.Now,
                                                                              SessionHandler.Dealer.DealershipId)
                                                      .OrderBy(x => x.Year)
                                                      .ThenBy(x => x.Month)
                                                      .ThenBy(x => x.Week);
                var chartTurnInfos = new List<ChartTurnInfo>();
                int weekCount = 1;
                foreach (var item in chartData)
                {
                    var data = new ChartTurnInfo
                    {
                        Week = weekCount++,
                        WeekText = item.Week == 1 ? GetMonth(item.Month) : string.Empty,
                        Month = item.Month,
                        Year = item.Year,
                        Turn = item.Turnover != null ? item.Turnover.Value : 0,
                        Y = item.Turnover != null ? item.Turnover.Value.ToString("0.00") : "0.00"
                    };
                    chartTurnInfos.Add(data);
                }
                return Json(new { data = chartTurnInfos, success = true });
            }
            return Json(new { success = false });
        }

        private string GetMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Agu";
                case 9:
                    return "Sep";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
                default:
                    return "";
            }
        }

        public ActionResult UpdateWishList(int id, bool isWishList)
        {
            _dealerBrandService.UpdateWishList(id, isWishList);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateGrossPerUnit(int id, int grossPerUnit)
        {
            _dealerBrandService.UpdateGrossPerUnit(id, grossPerUnit);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateGuide(int id, int guide)
        {
            _dealerBrandService.UpdateGuide(id, guide);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateWishListOther(int id, bool isWishList)
        {
            _dealerSegmentService.UpdateWishList(id, isWishList);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateWishListOtherInventory(int id, bool isWishList)
        {
            _inventorySegmentDetailService.UpdateWishList(id, isWishList);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateWishListOtherMarket(int id, bool isWishList)
        {
            _marketSegmentDetailService.UpdateWishList(id, isWishList);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateGuideOther(int id, int guide)
        {
            _dealerSegmentService.UpdateGuide(id, guide);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateSubGuideOther(int id, int guide)
        {
            _inventorySegmentDetailService.UpdateGuide(id, guide);

            return Json(new { success = "true" });
        }

        public ActionResult UpdateGrossPerUnitOther(int id, int grossPerUnit)
        {
            _dealerSegmentService.UpdateGrossPerUnit(id, grossPerUnit);

            return Json(new { success = "true" });
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult StockingGuideBrand()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }
            return View();
        }

        public ActionResult StockingGuideOther()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }
            return View();
        }

        public ActionResult StockingGuideRecommendation()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }
            return View();
        }

        public ActionResult StockingGuideBrandJson()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }

            return GetBrandJson(SessionHandler.Dealer.DealershipId, string.Empty);
        }

        public ActionResult StockingGuideBrandJsonByIDs(string ids)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }

            return GetBrandJson(SessionHandler.Dealer.DealershipId, ids);
        }

        public ActionResult StockingGuideOtherBrandJson(string make = "", string model = "")
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }

            return GetBrandOtherJson(SessionHandler.Dealer.DealershipId, make, model);
        }

        public ActionResult StockingGuideRecommendationBrandJson(string make = "", string model = "")
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }

            return GetBrandOtherJson(SessionHandler.Dealer.DealershipId, make, model, true);
        }

        public ActionResult StockingGuideRecommendationSoldOut()
        {
            if (SessionHandler.Dealer == null)
            {
                return PartialView("_SoldOutRecommendation", new List<MarketCarInfo>());
            }

            return PartialView("_SoldOutRecommendation", MarketHelper.GetRecommendationSoldCarWithin90DaysPeriod(SessionHandler.Dealer.Latitude, SessionHandler.Dealer.Longtitude));
        }

        public ActionResult StockingGuideAdminBrandJson()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }
            List<vincontrol.Data.Model.Make> listBrand = SQLHelper.GetListBrand();
            string selectedBrands = string.Empty;

            if (!string.IsNullOrEmpty(SessionHandler.Dealer.DealerSetting.BrandName))
            {
                selectedBrands = SessionHandler.Dealer.DealerSetting.BrandName;
            }

            List<Brand> brands = new List<Brand>();
            foreach (var item in listBrand)
            {
                Brand brand = new Brand();
                brand.Make = item.Value;
                brand.MakeId = item.MakeId;
                brands.Add(brand);
            }

            var arr = SQLHelper.GetListBrandByBrandNames(selectedBrands).Select(x => x.MakeId).ToArray();


            return Json(new { listBrand = brands.OrderBy(x => x.Make).ToList(), selectedBrands = arr });
        }

        private ActionResult GetBrandJson(int dealerId, string ids)
        {
            List<SGDealerBrand> dealerBrandList;
            if (string.IsNullOrEmpty(ids))
                dealerBrandList = _dealerBrandService.GetDealerBrandByDealerID(dealerId).ToList();
            else
            {
                var strId = ids.Split(',').ToList();
                var listId = new List<int>();
                foreach (var i in strId)
                {
                    listId.Add(Convert.ToInt32(i));
                }
                dealerBrandList = _dealerBrandService.GetDealerBrandByDealerID(dealerId)
                                       .Where(x => listId.Contains(x.SGDealerBrandId))
                                       .ToList();
            }
            
            var dealerBrandInfos = new List<ListInfo>();

            foreach (var brand in dealerBrandList.GroupBy(i => i.Make))
            {
                var dealerBrandInfo = new List<DealerBrandInfo>();
                var info = new ListInfo { Make = brand.Key, StockingGuideBrandData = dealerBrandInfo };
                dealerBrandInfos.Add(info);

                foreach (var item in brand)
                {
                    var data = new DealerBrandInfo
                    {
                        Age = (int)Math.Ceiling(Convert.ToDecimal(item.Age)),
                        DealerId = item.DealerId,
                        SGDealerBrandId = item.SGDealerBrandId,
                        StrGrossPerUnit = Convert.ToDecimal(item.GrossPerUnit).ToString("C0"),
                        StrTurnOver = Convert.ToDecimal(item.TurnOver).ToString("0.00"),
                        GrossPerUnit = item.GrossPerUnit,
                        Guide = item.Guide,
                        History = (int)Math.Ceiling(Convert.ToDecimal(item.History)),
                        Stock = item.Stock,
                        Supply = (int)Math.Ceiling(Convert.ToDecimal(item.Supply)),
                        TurnOver = item.TurnOver,
                        IsWishList = item.IsWishList != null && item.IsWishList.Value,
                        Make = item.Make,
                        Model = item.Model,
                        Recon = item.Recon
                    };

                    data.SGDealerBrandId = item.SGDealerBrandId;
                    data.Balance = item.Stock - item.Guide;
                    data.BalancePercent = Math.Abs(data.Balance) > 10 ? "100%" : GetPercentageString(Math.Abs(data.Balance) / 10);
                    data.URLDetail = Url.Action("ViewCars", "StockingGuide", new { make = item.Make, model = item.Model });
                    dealerBrandInfo.Add(data);
                }
            }

            foreach (var data in dealerBrandInfos)
            {
                var headerInfo = new DealerBrandInfo();
                if (data.StockingGuideBrandData.Count > 0)
                {
                    headerInfo.Make = data.StockingGuideBrandData[0].Make;
                    headerInfo.History = data.StockingGuideBrandData.Sum(x => x.History);
                    headerInfo.Stock = data.StockingGuideBrandData.Sum(x => x.Stock);
                    headerInfo.Guide = data.StockingGuideBrandData.Sum(x => x.Guide);
                    headerInfo.TurnOver = headerInfo.Stock == 0 ? 0 : ((double)headerInfo.History / (double)headerInfo.Stock) * 12;
                    headerInfo.Supply = headerInfo.History == 0
                                            ? 0
                                            : (int)Math.Ceiling(Convert.ToDecimal(((double)headerInfo.Stock / (double)headerInfo.History) * 30));
                    headerInfo.Age = (int)Math.Ceiling(Convert.ToDecimal(data.StockingGuideBrandData.Average(x => x.Age)));
                    headerInfo.GrossPerUnit = data.StockingGuideBrandData.Sum(x => x.GrossPerUnit);
                    headerInfo.Recon = data.StockingGuideBrandData.Sum(x => x.Recon);

                    headerInfo.StrTurnOver = Convert.ToDecimal(headerInfo.TurnOver).ToString("0.00");
                    headerInfo.StrGrossPerUnit = Convert.ToDecimal(headerInfo.GrossPerUnit).ToString("C0");
                    headerInfo.Recon = data.StockingGuideBrandData.Sum(x => x.Recon);

                    headerInfo.Balance = headerInfo.Stock - headerInfo.Guide;
                    headerInfo.BalancePercent = Math.Abs(headerInfo.Balance) > 10 ? "100%" : GetPercentageString(Math.Abs(headerInfo.Balance) / 10);
                }
                data.HeaderInfo = headerInfo;
            }
            var brandSelection = _dealerBrandSelectionService.GetDealerBrandSelection(SessionHandler.Dealer.DealershipId);

            var brandInfo = new BrandInfo();
            brandInfo.ListInfo = dealerBrandInfos;
            brandInfo.BrandSelection = brandSelection != null ? brandSelection.ModelSelections : string.Empty;
            return Json(brandInfo);
        }

        private ActionResult GetBrandOtherJson(int dealerId, string make = "", string model = "", bool isRecommendation = false)
        {
            List<SGDealerSegment> dealerBrandList = _dealerSegmentService.GetAllDealerSegmentsForDealer(dealerId, make, model).ToList();
            var dealerBrandInfo = new List<DealerBrandOtherInfo>();
            foreach (var item in dealerBrandList)
            {
                var stock = item.SGInventoryDealerSegmentDetails.Where(i => (make == "" || (i.Make.Equals(make))) && (model == "" || (i.Model.Equals(model)))).Sum(i => i.InStock);
                var guide = item.SGInventoryDealerSegmentDetails.Where(i => (make == "" || (i.Make.Equals(make))) && (model == "" || (i.Model.Equals(model)))).Sum(i => i.Guide);
                var history = item.SGInventoryDealerSegmentDetails.Where(i => (make == "" || (i.Make.Equals(make))) && (model == "" || (i.Model.Equals(model)))).Sum(i => i.History);
                
                var data = new DealerBrandOtherInfo();
                data.Age = (int)Math.Ceiling(Convert.ToDecimal(item.Age));
                data.DealerId = item.DealerId;
                data.StrGrossPerUnit = Convert.ToDecimal(item.GrossPerUnit).ToString("C0");
                data.StrTurnOver = Convert.ToDecimal(item.Turnover).ToString("0.00");
                data.GrossPerUnit = item.GrossPerUnit;
                data.Guide = guide;//item.Guide;
                data.History = (int)Math.Ceiling(Convert.ToDecimal(history));
                data.Stock = stock;//item.Stock;
                data.Supply = (int)Math.Ceiling(Convert.ToDecimal(item.Supply));
                data.TurnOver = item.Turnover;

                data.IsWishList = item.IsWishList != null && item.IsWishList.Value;
                data.SGSegmentId = item.SGSegmentId;
                data.SGDealerSegmentId = item.SGDealerSegmentId;
                //data.Make = "Used Other";
                data.Model = item.SGSegment.Name;
                data.Recon = item.Recon;

                data.Balance = stock - guide;
                data.BalancePercent = Math.Abs(data.Balance) > 10
                                          ? "100%"
                                          : GetPercentageString(Math.Abs(data.Balance) / 10);
                var inventoryBrands = new List<SGInventoryDealerSegmentDetailInfo>();
                foreach (var inventoryBrand in item.SGInventoryDealerSegmentDetails
                    .Where(i => (make == "" || (i.Make.Equals(make))) 
                       && (model == "" || (i.Model.Equals(model)))))
                {
                    var inventoryInfo = new SGInventoryDealerSegmentDetailInfo();
                    inventoryInfo.SGInventoryDealerSegmentDetailId = inventoryBrand.SGInventoryDealerSegmentDetailId;
                    inventoryInfo.SGDealerSegmentId = inventoryBrand.SGDealerSegmentId;
                    inventoryInfo.Make = inventoryBrand.Make;
                    inventoryInfo.Model = inventoryBrand.Model;
                    inventoryInfo.InStock = inventoryBrand.InStock;
                    inventoryInfo.Guide = inventoryBrand.Guide;
                    inventoryInfo.OU = inventoryInfo.Guide - inventoryInfo.InStock;
                    inventoryInfo.History = (int)Math.Ceiling(Convert.ToDecimal(inventoryBrand.History));
                    inventoryInfo.Recon = inventoryBrand.Recon;
                    inventoryInfo.TurnOver = inventoryBrand.TurnOver;
                    inventoryInfo.Supply = inventoryBrand.Supply;
                    inventoryInfo.IsWishList = inventoryBrand.IsWishList != null && inventoryBrand.IsWishList.Value;
                    inventoryInfo.URLDetail = Url.Action("ViewCars", "StockingGuide", new { make = inventoryBrand.Make, model = inventoryBrand.Model });
                    inventoryInfo.StrTurnOver = Convert.ToDecimal(inventoryInfo.TurnOver).ToString("0.00");
                    inventoryBrands.Add(inventoryInfo);
                }

                var marketBrands = new List<SGMarketDealerSegmentDetailInfo>();
                foreach (var marketBrand in item.SGMarketDealerSegmentDetails
                    .Where(i => (make == "" || (i.Make.Equals(make))) 
                       && (model == "" || (i.Model.Equals(model)))
                       && (isRecommendation == false || (i.MarketStock - i.MarketHistory < -30))
                       ))
                {
                    var marketInfo = new SGMarketDealerSegmentDetailInfo();
                    marketInfo.SGMarketDealerSegmentDetailId = marketBrand.SGMarketDealerSegmentDetailId;
                    marketInfo.Make = marketBrand.Make;
                    marketInfo.Model = marketBrand.Model;
                    marketInfo.YourStock = marketBrand.YourStock;
                    marketInfo.MarketStock = marketBrand.MarketStock;
                    marketInfo.MarketHistory = (int)Math.Ceiling(Convert.ToDecimal(marketBrand.MarketHistory));
                    marketInfo.History = (int)Math.Ceiling(Convert.ToDecimal(marketBrand.History));
                    marketInfo.Supply = (int)Math.Ceiling(Convert.ToDecimal(marketBrand.Supply));
                    marketInfo.Age = (int)Math.Ceiling(Convert.ToDecimal(marketBrand.Age));
                    marketInfo.TurnOver = marketBrand.TurnOver;
                    marketInfo.IsWishList = marketBrand.IsWishList;
                    marketInfo.BalanceYourStock = marketBrand.OU != null ? marketBrand.OU.Value : 0;
                    marketInfo.BalanceMarketStock = marketInfo.MarketStock - marketInfo.MarketHistory;

                    marketInfo.URLDetail = Url.Action("ViewCars", "StockingGuide", new { make = marketBrand.Make, model = marketBrand.Model });

                    marketInfo.StrTurnOver = Convert.ToDecimal(marketInfo.TurnOver).ToString("0.00");
                    marketBrands.Add(marketInfo);
                }

                data.SGInventoryDealerSegmentDetails = inventoryBrands.OrderByDescending(x => x.InStock).ToList();
                data.SGMarketDealerSegmentDetails = marketBrands.OrderByDescending(x => x.YourStock).ThenBy(x => x.BalanceMarketStock).ToList();
                dealerBrandInfo.Add(data);
            }

            //_marketSegmentDetailService.GetMarketSegmentDetailForDealer()
            var headerInfo = new DealerBrandOtherInfo
            {
                History = dealerBrandInfo.Sum(x => x.History),
                Stock = dealerBrandInfo.Sum(x => x.Stock),
                Guide = dealerBrandInfo.Sum(x => x.Guide)
            };
            
            headerInfo.TurnOver = (headerInfo.Stock == 0) ? 0 : ((double)headerInfo.History / (double)headerInfo.Stock) * 12;
            headerInfo.Supply = headerInfo.History == 0 ? 0 : (int)Math.Ceiling(Convert.ToDecimal(((double)headerInfo.Stock / (double)headerInfo.History) * 30));
            headerInfo.Age = (int)Math.Ceiling(Convert.ToDecimal(dealerBrandInfo.Count > 0 && dealerBrandInfo.Any(x => x.Age > 0)
                                          ? dealerBrandInfo.Where(x => x.Age > 0).Average(x => x.Age)
                                          : 0));

            headerInfo.GrossPerUnit = dealerBrandInfo.Sum(x => x.GrossPerUnit);
            headerInfo.Recon = dealerBrandInfo.Sum(x => x.Recon);

            headerInfo.StrTurnOver = Convert.ToDecimal(headerInfo.TurnOver).ToString("0.00");
            headerInfo.StrGrossPerUnit = Convert.ToDecimal(headerInfo.GrossPerUnit).ToString("C0");

            headerInfo.Balance = headerInfo.Stock - headerInfo.Guide;
            headerInfo.BalancePercent = Math.Abs(headerInfo.Balance) > 10
                                            ? "100%"
                                            : GetPercentageString(Math.Abs(headerInfo.Balance) / 10);

            var info = new OtherInfo
            {
                HeaderInfo = headerInfo,
                StockingGuideBrandOtherData =
                    dealerBrandInfo.OrderByDescending(x => x.History)
                        .ThenByDescending(x => x.Stock)
                        .ThenByDescending(x => x.Guide)
                        .ThenBy(x => x.Make)
                        .ToList()
            };

            return Json(info);
        }

        public ActionResult ViewListCars(string make, string model)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            ViewData["Make"] = make;
            ViewData["Model"] = model;
            var appraisals = _appraisalService.GetAppraisalWithModel(make, model, SessionHandler.Dealer.DealershipId).ToList();
            SessionHandler.StockingGuideCarsReport = appraisals;
            return View(appraisals);
        }

        public ActionResult ViewListAuctions(string make, string model)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            ViewData["Make"] = make;
            ViewData["Model"] = model;

            var manheims = _manheimVehicleService.GetManheimWithModel(make, model).ToList();
            var regionCodeMapping = _manheimAuctionService.GetRegionCodeMapping();
            var auctions = manheims.Select(i =>
            {
                var singleOrDefault = regionCodeMapping.SingleOrDefault(j => i.Auction == j.Key);
                return singleOrDefault != null ? new ManheimRegionVehicle(i, singleOrDefault.Value, SessionHandler.CurrentUser.UserId) : new ManheimRegionVehicle(i, null, SessionHandler.CurrentUser.UserId);
                
            }).OrderByDescending(i => i.ManheimVehicle.SaleDate).ToList();
            SessionHandler.StockingGuideAuctionsReport = auctions;
            return View(auctions);
        }

        public ActionResult ViewCars(string make, string model)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            ViewData["Make"] = make;
            ViewData["Model"] = model;
            return View();
        }

        public ActionResult GetAuctionCount(string make, string model)
        {
            return Content(_manheimVehicleService.GetManheimWithModel(make, model).Count().ToString(CultureInfo.InvariantCulture));
        }

        public ActionResult GetMarketCount(string make, string model)
        {
            int marketCount = 0;
            ChartGraph chart = MarketHelper.GetChartDataWithYearRange(DateTime.Now.Year - 4, DateTime.Now.Year,
                                                                          make, model, string.Empty, SessionHandler.Dealer, false);
            if (chart != null && chart.ChartModels != null)
            {
                marketCount = chart.TypedChartModels.Count(x => x.Distance < 100);
                SessionHandler.StockingGuideChart = chart;
            }

            return Content(marketCount.ToString(CultureInfo.InvariantCulture));
        }

        public ActionResult GetAppraisalCount(string make, string model)
        {
            return Content(_appraisalService.GetAppraisalWithModel(make, model, SessionHandler.Dealer.DealershipId).ToList().Count.ToString());
        }

        public ActionResult SaveBrandSelection(string brandSelection)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }
            var dealerBrandSelection = new SGDealerBrandSelection
            {
                DealerId = SessionHandler.Dealer.DealershipId,
                ModelSelections = brandSelection
            };
            _dealerBrandSelectionService.AddDealerBrandSelection(dealerBrandSelection);

            return Json(new { success = true });
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult WishList()
        {
            return View();
        }

        public JsonResult LoadWishList()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false });
            }
            var wishLists =
                _dealerBrandService.GetDealerBrandByDealerID(SessionHandler.Dealer.DealershipId)
                                   .Where(x => x.IsWishList == true).ToList();

            var wishListOthers =
                _dealerSegmentService.GetAllDealerSegmentsForDealer(SessionHandler.Dealer.DealershipId)
                                     .Where(
                                         x =>
                                         x.SGInventoryDealerSegmentDetails.Where(y => y.IsWishList == true).Count() > 0 ||
                                         x.SGMarketDealerSegmentDetails.Where(z => z.IsWishList == true).Count() > 0)
                                     .ToList();

            var wishListInfos = new List<WishListInfo>();
            foreach (var item in wishLists)
            {
                var data = new WishListInfo
                {
                    Make = item.Make,
                    Model = item.Model,
                    URLDetail = Url.Action("WishListDetail", new { make = item.Make, model = item.Model }),
                    IsBrand = true,
                    Age = item.Age,
                    AddedDate = item.CreatedDate.ToShortDateString(),
                    Highest = item.MarketHighestPrice != null
                        ? item.MarketHighestPrice.Value.ToString("C0")
                        : "$0",
                    Average = item.MarketAveragePrice != null
                        ? item.MarketAveragePrice.Value.ToString("C0")
                        : "$0",
                    Lowest = item.MarketLowestPrice != null
                        ? item.MarketLowestPrice.Value.ToString("C0")
                        : "$0",
                    Turn = item.TurnOver.ToString(),
                    History = item.History.ToString(),
                    Id = item.SGDealerBrandId,
                    Source = Constanst.StockingGuideSource.Brand
                };
                wishListInfos.Add(data);
            }

            foreach (var itemOther in wishListOthers)
            {
                foreach (var inventoryInfo in itemOther.SGInventoryDealerSegmentDetails)
                {
                    if (inventoryInfo.IsWishList != null && inventoryInfo.IsWishList.Value == true)
                    {
                        var data = new WishListInfo
                        {
                            Make = inventoryInfo.Make,
                            Model = inventoryInfo.Model,
                            Segment = itemOther.SGSegment.Name,
                            URLDetail = Url.Action("WishListDetail",
                                new { make = inventoryInfo.Make, model = inventoryInfo.Model }),
                            IsBrand = false,
                            Age = inventoryInfo.Age,
                            AddedDate = inventoryInfo.CreatedDate.ToShortDateString(),
                            Highest = inventoryInfo.MarketHighestPrice != null
                                ? inventoryInfo.MarketHighestPrice.Value.ToString("C0")
                                : "$0",
                            Average = inventoryInfo.MarketAveragePrice != null
                                ? inventoryInfo.MarketAveragePrice.Value.ToString("C0")
                                : "$0",
                            Lowest = inventoryInfo.MarketLowestPrice != null
                                ? inventoryInfo.MarketLowestPrice.Value.ToString("C0")
                                : "$0",
                            Turn = inventoryInfo.TurnOver.ToString(),
                            History = inventoryInfo.History.ToString(),
                            Id = inventoryInfo.SGInventoryDealerSegmentDetailId,
                            Source = Constanst.StockingGuideSource.OtherBrand


                        };

                        wishListInfos.Add(data);
                    }
                }

                foreach (var marketInfo in itemOther.SGMarketDealerSegmentDetails)
                {
                    if (marketInfo.IsWishList != null && marketInfo.IsWishList.Value == true)
                    {
                        var data = new WishListInfo
                        {
                            Make = marketInfo.Make,
                            Model = marketInfo.Model,
                            Segment = itemOther.SGSegment.Name,
                            URLDetail = Url.Action("WishListDetail",
                                new { make = marketInfo.Make, model = marketInfo.Model }),
                            IsBrand = false,
                            Age = marketInfo.Age,
                            AddedDate = marketInfo.CreatedDate.ToShortDateString(),
                            Highest = marketInfo.MarketHighestPrice != null
                                ? marketInfo.MarketHighestPrice.Value.ToString("C0")
                                : "$0",
                            Average = marketInfo.MarketAveragePrice != null
                                ? marketInfo.MarketAveragePrice.Value.ToString("C0")
                                : "$0",
                            Lowest = marketInfo.MarketLowestPrice != null
                                ? marketInfo.MarketLowestPrice.Value.ToString("C0")
                                : "$0",
                            Turn = marketInfo.TurnOver.ToString(),
                            History = marketInfo.History.ToString(),
                            Id = marketInfo.SGMarketDealerSegmentDetailId,
                            Source = Constanst.StockingGuideSource.Market

                        };

                        wishListInfos.Add(data);
                    }
                }
            }
            return new DataContractJsonResult(wishListInfos);
        }

        public ActionResult RemoveFromWishList(int id, int source)
        {
            try
            {
                switch (source)
                {
                    case Constanst.StockingGuideSource.Brand:
                        _dealerBrandService.RemoveFromWishList(id);
                        break;

                    case Constanst.StockingGuideSource.OtherBrand:
                        _inventorySegmentDetailService.RemoveFromWishList(id);
                        break;

                    case Constanst.StockingGuideSource.Market:
                        _marketSegmentDetailService.RemoveFromWishList(id);
                        break;
                }
            }
            catch (Exception e)
            {
                return Json(e.InnerException);
            }
            return Json("Successful");
        }

        public ActionResult WishListDetail(string make, string model)
        {
            ViewData["Make"] = make;
            ViewData["Model"] = model;
            return View();
        }

        private static string GetPercentageString(double ratio)
        {
            return ratio.ToString("0%");
        }

    }

    public class ListInfo
    {
        public string Make { get; set; }
        public DealerBrandInfo HeaderInfo { get; set; }
        public List<DealerBrandInfo> StockingGuideBrandData { get; set; }
    }

    public class BrandInfo
    {
        public string BrandSelection { get; set; }
        public List<ListInfo> ListInfo { get; set; }
    }

    public class OtherInfo
    {
        public DealerBrandOtherInfo HeaderInfo { get; set; }
        public List<DealerBrandOtherInfo> StockingGuideBrandOtherData { get; set; }
    }

    public class ChartTurnInfo
    {
        public int Week { get; set; }
        public string WeekText { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Turn { get; set; }
        public string Y { get; set; }
    }

    public class SegmentInfo
    {
        public string Name { get; set; }
        public int History { get; set; }
        public int Stock { get; set; }
        public int Supply { get; set; }
        public decimal Turn { get; set; }
        public string StrTurn { get; set; }
        public int Profit { get; set; }
        public int Type { get; set; }
    }

    public class WishListInfo
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Segment { get; set; }
        public string URLDetail { get; set; }
        public bool IsBrand { get; set; }
        public string AddedDate { get; set; }
        public int Age { get; set; }
        public string Highest { get; set; }
        public string Average { get; set; }
        public string Lowest { get; set; }
        public string Turn { get; set; }
        public string History { get; set; }
        public int Id { get; set; }
        public short Source { get; set; }
    }

    public class Brand
    {
        public string Make { get; set; }
        public int MakeId { get; set; }
    }
}
