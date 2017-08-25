using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.DomainObject;
using vincontrol.VinSell.Handlers;
using vincontrol.Helper;
using vincontrol.CarFax;
using vincontrol.KBB;
using vincontrol.Manheim;

namespace vincontrol.VinSell.Controllers
{
    public class AuctionController : BaseController
    {
        private IAuctionManagement _manheimAuctionManagement;
        private ICarFaxService _carfaxService;
        private IAccountManagementForm _accountManagementForm;
        private IDealerManagementForm _dealerManagementForm;

        public AuctionController()
        {
            _manheimAuctionManagement = new AuctionManagement();
            _carfaxService = new CarFaxService();
            _accountManagementForm = new AccountManagementForm();
            _dealerManagementForm = new DealerManagementForm();
        }

        public ActionResult Index()
        {
            var model = new RegionActionSummarizeListViewModel(_manheimAuctionManagement.GetAllRegionsWithAuctionStatistics());
            SessionHandler.RegionAuctionSummarizeList = model.RegionAuctionSummarizeList;
            SessionHandler.ManheimYearMakeModelList = new AuctionListViewModel(LinqHelper.GetManheimYearMakeModelForAdvancedSearch());
            
            return View(model);
        }

        public ActionResult DetailVehicle(int id)
        {
            var model = _manheimAuctionManagement.GetVehicle(id);
            model.IsFavorite = _manheimAuctionManagement.IsFavorite(model.Id, SessionHandler.User.DealerId, SessionHandler.User.Id);
            model.Note = _manheimAuctionManagement.GetNote(model.Id, SessionHandler.User.DealerId, SessionHandler.User.Id);
            model.DealerId = SessionHandler.User.DealerId;
            model.Url = HttpUtility.UrlEncode(model.Url);
            return View(model);
        }

        public ActionResult DetailVehicleFromVincontrol(int userId, int id)
        {
            if (SessionHandler.User==null||SessionHandler.Dealer == null)
            {
                var existingUser = _accountManagementForm.GetUser(userId);

                if (existingUser != null)
                {
                    SessionHandler.User = existingUser;
                    SessionHandler.Dealer = _dealerManagementForm.GetDealerById(existingUser.DealerId);
                }
            }

            return RedirectToAction("DetailVehicle", new {id});

          
        }

        public ActionResult ViewVehicleOnMobile(string token, int id)
        {
            int dealerId = 3738;
            string userName = "vincontrol";
            try
            {
                var decryptString = EncryptionHelper.DecryptString(token.Replace(" ", "+"));
                dealerId = Convert.ToInt32(decryptString.Split('|')[0]);
                userName = decryptString.Split('|')[1];
            }
            catch (Exception) { }

            SessionHandler.User = new UserViewModel() { DealerId = dealerId, UserName = userName };
            SessionHandler.Dealer = _dealerManagementForm.GetDealerById(dealerId);

            return RedirectToAction("DetailVehicle", new { id });
        }

        public ActionResult ViewVehicleOnWindow(string token, string vin, string manheimTrimId, string kbbTrimId)
        {
            int dealerId = 3738;
            string userName = "vincontrol";
            int convertedManheimTrimId = 0;
            int convertedKbbTrimId = 0;
            try
            {
                var decryptString = EncryptionHelper.DecryptString(token.Replace(" ", "+"));
                dealerId = Convert.ToInt32(decryptString.Split('|')[0]);
                userName = decryptString.Split('|')[1];
                convertedManheimTrimId = Convert.ToInt32(manheimTrimId);
                convertedKbbTrimId = Convert.ToInt32(kbbTrimId);
            }
            catch (Exception) { }

            SessionHandler.User = _accountManagementForm.InitializeWithDealerId(dealerId);
            SessionHandler.User.UserName = userName;

            var model = _manheimAuctionManagement.GetVehicle(vin);
            model.IsFavorite = _manheimAuctionManagement.IsFavorite(model.Id, SessionHandler.User.DealerId, SessionHandler.User.Id);
            model.Note = _manheimAuctionManagement.GetNote(model.Id, SessionHandler.User.DealerId, SessionHandler.User.Id);
            model.ManheimWholesale = new List<ManheimWholesaleViewModel>();
            model.KarpowerWholesale = new List<SmallKarPowerViewModel>();
            model.MarketValue = new ChartGraph();

            try
            {
                model.CarFax = _carfaxService.XmlSerializeCarFax(model.Vin, SessionHandler.User.Setting.CarFax, SessionHandler.User.Setting.CarFaxPassword);
            }
            catch (Exception)
            {
                model.CarFax = new CarFaxViewModel() { Success = false };
            }

            try
            {
                if (!string.IsNullOrEmpty(vin))
                {
                    if (model.Mmr > 0 && model.MmrAbove > 0 && model.MmrBelow > 0)
                    {
                        var newRecord = new ManheimWholesaleViewModel()
                        {
                            LowestPrice = (model.MmrBelow),
                            AveragePrice = (model.Mmr),
                            HighestPrice = (model.MmrAbove),
                            Year = model.Year,
                            TrimName = model.Trim
                        };

                        model.ManheimWholesale.Add(newRecord);
                    }
                    else
                    {
                        var manheimService = new ManheimService();
                        model.ManheimWholesale = manheimService.ManheimReport(model, SessionHandler.User.Setting.Manheim, SessionHandler.User.Setting.ManheimPassword);
                    }
                }

            }
            catch (Exception)
            {
                
            }

            try
            {
                model.KarpowerWholesale = LinqHelper.GetKbbReport(vin);
                if (model.KarpowerWholesale == null)
                {
                    var karpowerService = new KBBService();
                    model.KarpowerWholesale = karpowerService.Execute(vin, model.Mileage.ToString(), DateTime.Now, SessionHandler.User.Setting.KellyBlueBook, SessionHandler.User.Setting.KellyPassword, Constant.Constanst.InventoryStatus.Inventory);
                }

                var savedKbbTrim = LinqHelper.GetSavedKbbTrim(vin, SessionHandler.User.DealerId);
                if (savedKbbTrim != null)
                    convertedKbbTrimId = savedKbbTrim.TrimId;
                {
                    foreach (var smallKarPowerViewModel in model.KarpowerWholesale)
                    {
                        smallKarPowerViewModel.IsSelected = smallKarPowerViewModel.SelectedTrimId == convertedKbbTrimId;
                    }
                }
            }
            catch (Exception)
            {
                
            }

            try
            {
                model.MarketValue = MarketDataHelper.GetMarketCarsOnAutoTraderVersion2(_manheimAuctionManagement.GetVehicle(model.Id), SessionHandler.User);
            }
            catch (Exception)
            {
                
            }

            return View("PrintVehicleOnWinApp", model);
        }

        public ActionResult PrintVehicle(int id)
        {
            var model = _manheimAuctionManagement.GetVehicle(id);
            model.IsFavorite = _manheimAuctionManagement.IsFavorite(model.Id, SessionHandler.User.DealerId, SessionHandler.User.Id);
            model.Note = _manheimAuctionManagement.GetNote(model.Id, SessionHandler.User.DealerId, SessionHandler.User.Id);
            try
            {
                model.CarFax = _carfaxService.XmlSerializeCarFax(model.Vin, SessionHandler.User.Setting.CarFax, SessionHandler.User.Setting.CarFaxPassword);
            }
            catch (Exception)
            {
                model.CarFax = new CarFaxViewModel() { Success = false };
            }
            return View(model);
        }

        public ActionResult ManheimData(string vin)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();
                if (!string.IsNullOrEmpty(vin))
                {
                    var model = _manheimAuctionManagement.GetVehicle(vin);

                    if (model.Mmr > 0 && model.MmrAbove > 0 && model.MmrBelow > 0)
                    {
                        var newRecord = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = (model.MmrBelow),
                                AveragePrice = (model.Mmr),
                                HighestPrice = (model.MmrAbove),
                                Year = model.Year,
                                TrimName = model.Trim
                            };
                        result.Add(newRecord);
                    }
                    else
                    {
                        
                        result = manheimService.ManheimReport(model, SessionHandler.User.Setting.Manheim, SessionHandler.User.Setting.ManheimPassword);    
                    }                    
                }
                else
                {
                    result = new List<ManheimWholesaleViewModel>();
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return PartialView("ManheimData", result);
        }

        public ActionResult KarpowerData(string vin, string mileage)
        {
            List<SmallKarPowerViewModel> result;
            try
            {
                result = LinqHelper.GetKbbReport(vin);
                if (result == null)
                {
                    var karpowerService = new KBBService();
                    result = karpowerService.Execute(vin, mileage, DateTime.Now, SessionHandler.User.Setting.KellyBlueBook, SessionHandler.User.Setting.KellyPassword, Constant.Constanst.InventoryStatus.Inventory);
                }

                var savedKbbTrim = LinqHelper.GetSavedKbbTrim(vin, SessionHandler.User.DealerId);
                if (savedKbbTrim != null)
                {
                    foreach (var smallKarPowerViewModel in result)
                    {
                        smallKarPowerViewModel.IsSelected = smallKarPowerViewModel.SelectedTrimId == savedKbbTrim.TrimId;
                    }
                }
            }
            catch (Exception)
            {
                result = new List<SmallKarPowerViewModel>();
            }

            return PartialView("KarpowerData", result);
        }

        public ActionResult MarketData(string id)
        {
            try
            {
                var convertedId = Convert.ToInt32(id);
                return PartialView("MarketData", MarketDataHelper.GetMarketCarsOnAutoTraderVersion2(_manheimAuctionManagement.GetVehicle(convertedId), SessionHandler.User));
            }
            catch (Exception)
            {
                return PartialView("MarketData", null);
            }
        }

        public ActionResult GetAuctions(string state)
        {
            return View("Auctions", SessionHandler.RegionAuctionSummarizeList.FirstOrDefault(i => i.Name.ToLower().Equals(state.ToLower())));
        }

        public ActionResult GetVehicles(string auctionCode, string auctionName, string state)
        {
            var auctions = _manheimAuctionManagement.GetVehicles(auctionCode, SessionHandler.User.DealerId, SessionHandler.User.Id);
            SessionHandler.VehiclesInAuctions = auctions;

            var listOfDate =
                SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Subtract(DateTime.Now).Days >= 0).OrderBy(i => i.AuctionDate)
                    .Select(i => i.AuctionDate)
                    .Distinct()
                    .Skip(0)
                    .Take(7)
                    .ToList();

            SessionHandler.VehiclesInAuctionsByDate =
                SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Equals(listOfDate.FirstOrDefault()))
                              .ToList();
            var model = new AuctionListViewModel(SessionHandler.VehiclesInAuctionsByDate) {Date = listOfDate, State = state, Auction = auctionName, AuctionCode = auctionCode};

            return View("Vehicles", model);
        }

        public ActionResult GetVehiclesByLane(string auctionCode, string auctionName, string state, int lane)
        {
            if (SessionHandler.VehiclesInAuctions != null && SessionHandler.VehiclesInAuctions.Any())
            {
                var listOfDate =
                  SessionHandler.VehiclesInAuctions.OrderBy(i => i.AuctionDate)
                                .Select(i => i.AuctionDate)
                                .Distinct()
                                .Skip(0)
                                .Take(7)
                                .ToList();

                SessionHandler.VehiclesInAuctionsByDate =
                    SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Equals(listOfDate.FirstOrDefault()) /*&& i.Lane == lane*/)
                                  .ToList();
                var model = new AuctionListViewModel(SessionHandler.VehiclesInAuctionsByDate) { Date = listOfDate, SelectedLane = lane, State = state, Auction = auctionName, AuctionCode = auctionCode };

                return View("Vehicles", model);
            }
            else
            {

                var auctions = _manheimAuctionManagement.GetVehicles(auctionCode, SessionHandler.User.DealerId,
                                                                     SessionHandler.User.Id);
                SessionHandler.VehiclesInAuctions = auctions;

                var listOfDate =
                    SessionHandler.VehiclesInAuctions.OrderBy(i => i.AuctionDate)
                                  .Select(i => i.AuctionDate)
                                  .Distinct()
                                  .Skip(0)
                                  .Take(7)
                                  .ToList();

                SessionHandler.VehiclesInAuctionsByDate =
                   SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Equals(listOfDate.FirstOrDefault()) /*&& i.Lane == lane*/)
                                 .ToList();
                var model = new AuctionListViewModel(SessionHandler.VehiclesInAuctionsByDate) { Date = listOfDate, SelectedLane = lane, State = state, Auction = auctionName, AuctionCode = auctionCode };

                return View("Vehicles", model);
            }
        }

        public ActionResult GetVehiclesByRun(string auctionCode, string auctionName, string state, int lane, int run)
        {
            if (SessionHandler.VehiclesInAuctions != null && SessionHandler.VehiclesInAuctions.Any())
            {
                var listOfDate =
                  SessionHandler.VehiclesInAuctions.OrderBy(i => i.AuctionDate)
                                .Select(i => i.AuctionDate)
                                .Distinct()
                                .Skip(0)
                                .Take(7)
                                .ToList();

                SessionHandler.VehiclesInAuctionsByDate =
                    SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Equals(listOfDate.FirstOrDefault()) /*&& i.Lane == lane && i.Run==run*/)
                                  .ToList();
                var model = new AuctionListViewModel(SessionHandler.VehiclesInAuctionsByDate) { Date = listOfDate, SelectedLane = lane, SelectedRun = run, State = state, Auction = auctionName, AuctionCode = auctionCode };

                return View("Vehicles", model);
            }
            else
            {

                var auctions = _manheimAuctionManagement.GetVehicles(auctionCode, SessionHandler.User.DealerId,
                                                                     SessionHandler.User.Id);
                SessionHandler.VehiclesInAuctions = auctions;

                var listOfDate =
                    SessionHandler.VehiclesInAuctions.OrderBy(i => i.AuctionDate)
                                  .Select(i => i.AuctionDate)
                                  .Distinct()
                                  .Skip(0)
                                  .Take(7)
                                  .ToList();

                SessionHandler.VehiclesInAuctionsByDate =
           SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Equals(listOfDate.FirstOrDefault()) /*&& i.Lane == lane && i.Run == run*/)
                         .ToList();
                var model = new AuctionListViewModel(SessionHandler.VehiclesInAuctionsByDate) { Date = listOfDate, SelectedLane = lane, SelectedRun = run, State = state, Auction = auctionName, AuctionCode = auctionCode };

                return View("Vehicles", model);
            }
        }

        public ActionResult GetVehiclesInAuction(string date, string auctionCode)
        {
            var convertedDate = Convert.ToDateTime(date);
            if (SessionHandler.VehiclesInAuctions == null)
                SessionHandler.VehiclesInAuctions = _manheimAuctionManagement.GetVehicles(auctionCode, SessionHandler.User.DealerId, SessionHandler.User.Id);

            SessionHandler.VehiclesInAuctionsByDate = SessionHandler.VehiclesInAuctions.Where(i => i.AuctionDate.Equals(convertedDate)).ToList();
            var model = new AuctionListViewModel(SessionHandler.VehiclesInAuctionsByDate) { };
            return PartialView("VehiclesInAuction", model);
        }

        public ActionResult FilterVehiclesInAuction(string year, string make, string model, string trim, string lane, string run, string seller, string category)
        {
            var convertedYear = String.IsNullOrEmpty(year) ? 0 : Convert.ToInt32(year);
            var convertedLane = String.IsNullOrEmpty(lane) ? 0 : Convert.ToInt32(lane);
            var convertedRun = String.IsNullOrEmpty(run) ? 0 : Convert.ToInt32(run);

            var temp = SessionHandler.VehiclesInAuctionsByDate;
            var viewModel =
                new AuctionListViewModel(temp.Where(
                        i => (convertedYear.Equals(0) ? true : i.Year.Equals(convertedYear))
                        && (convertedLane.Equals(0) ? true : i.Lane.Equals(convertedLane))
                        && (convertedRun.Equals(0) ? true : i.Run.Equals(convertedRun))
                        && (String.IsNullOrEmpty(make) ? true : i.Make.Equals(make))
                        && (String.IsNullOrEmpty(model) ? true : i.Model.Equals(model))
                        && (String.IsNullOrEmpty(trim) ? true : i.Trim.Equals(trim))
                        && (String.IsNullOrEmpty(seller) ? true : i.Seller.Equals(seller))
                        && (String.IsNullOrEmpty(category) || i.Category == null ? true : i.Category.Equals(category)))
                        .ToList()) {};
            return PartialView("Data", viewModel);
        }

        public ActionResult Favorites()
        {
            var records = _manheimAuctionManagement.GetFavoriteVehicles(SessionHandler.User.DealerId, SessionHandler.User.Id);
            SessionHandler.FavoriteAuctions = records;
            return View(new AuctionListViewModel(SessionHandler.FavoriteAuctions));
        }

        public ActionResult FilterFavoriteVehicles(string year, string make, string model, string trim, string lane, string run, string seller, string category)
        {
            var convertedYear = String.IsNullOrEmpty(year) ? 0 : Convert.ToInt32(year);
            var convertedLane = String.IsNullOrEmpty(lane) ? 0 : Convert.ToInt32(lane);
            var convertedRun = String.IsNullOrEmpty(run) ? 0 : Convert.ToInt32(run);

            if (SessionHandler.FavoriteAuctions == null)
                SessionHandler.FavoriteAuctions = _manheimAuctionManagement.GetFavoriteVehicles(SessionHandler.User.DealerId, SessionHandler.User.Id);

            var temp = SessionHandler.FavoriteAuctions;
            var viewModel =
                new AuctionListViewModel(temp.Where(
                        i => (convertedYear.Equals(0) ? true : i.Year.Equals(convertedYear))
                        && (convertedLane.Equals(0) ? true : i.Lane.Equals(convertedLane))
                        && (convertedRun.Equals(0) ? true : i.Run.Equals(convertedRun))
                        && (String.IsNullOrEmpty(make) ? true : i.Make.Equals(make))
                        && (String.IsNullOrEmpty(model) ? true : i.Model.Equals(model))
                        && (String.IsNullOrEmpty(trim) ? true : i.Trim.Equals(trim))
                        && (String.IsNullOrEmpty(seller) ? true : i.Seller.Equals(seller))
                        && (String.IsNullOrEmpty(category) || i.Category == null ? true : i.Category.Equals(category)))
                        .ToList()) { };
            return PartialView("FavoriteData", viewModel);
        }

        public ActionResult Notes()
        {
            var records = _manheimAuctionManagement.GetNotedVehicles(SessionHandler.User.DealerId, SessionHandler.User.Id);
            SessionHandler.NotedAuctions = records;
            return View(new AuctionListViewModel(records));
        }

        public ActionResult FilterNotedVehicles(string year, string make, string model, string trim, string lane, string run, string seller, string category)
        {
            var convertedYear = String.IsNullOrEmpty(year) ? 0 : Convert.ToInt32(year);
            var convertedLane = String.IsNullOrEmpty(lane) ? 0 : Convert.ToInt32(lane);
            var convertedRun = String.IsNullOrEmpty(run) ? 0 : Convert.ToInt32(run);

            if (SessionHandler.NotedAuctions == null)
                SessionHandler.NotedAuctions = _manheimAuctionManagement.GetNotedVehicles(SessionHandler.User.DealerId, SessionHandler.User.Id);

            var temp = SessionHandler.NotedAuctions;
            var viewModel =
                new AuctionListViewModel(temp.Where(
                        i => (convertedYear.Equals(0) ? true : i.Year.Equals(convertedYear))
                        && (convertedLane.Equals(0) ? true : i.Lane.Equals(convertedLane))
                        && (convertedRun.Equals(0) ? true : i.Run.Equals(convertedRun))
                        && (String.IsNullOrEmpty(make) ? true : i.Make.Equals(make))
                        && (String.IsNullOrEmpty(model) ? true : i.Model.Equals(model))
                        && (String.IsNullOrEmpty(trim) ? true : i.Trim.Equals(trim))
                        && (String.IsNullOrEmpty(seller) ? true : i.Seller.Equals(seller))
                        && (String.IsNullOrEmpty(category) || i.Category == null ? true : i.Category.Equals(category)))
                        .ToList()) { };
            return PartialView("NotedData", viewModel);
        }

        public ActionResult AdvancedSearch()
        {
            return View(_manheimAuctionManagement.IntializeAdvancedSearchForm());
        }

        [HttpPost]
        public ActionResult AdvancedSearch(AdvancedSearchViewModel model)
        {
            SessionHandler.AdvancedSearchViewModel = model;
            return RedirectToAction("Search", "Search");
        }
        
        public void MarkFavorite(int vehicleId)
        {
            try
            {
                _manheimAuctionManagement.MarkFavorite(vehicleId, SessionHandler.User.DealerId, SessionHandler.User.Id);
                SessionHandler.VehiclesInAuctions = null;
                SessionHandler.FavoriteAuctions = null;
            }
            catch (Exception)
            {
                
            }
        }

        [ValidateInput(false)]
        public void MarkNote(int vehicleId, string note)
        {
            _manheimAuctionManagement.MarkNote(vehicleId, note, SessionHandler.User.DealerId, SessionHandler.User.Id);
            SessionHandler.NotedAuctions = null;
        }

        public void MarkScrollTop(int position)
        {
            SessionHandler.ScrollTop = position;
        }


        public ActionResult ManheimTransaction(int listingId, short vehicleStatus, short auctionRegion, int pageIndex = 1, int pageSize = 10)
        {
            var manheimTransactions = MarketHelper.GetManheimTransactionForVinsell(listingId, vehicleStatus, auctionRegion, SessionHandler.Dealer);
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

    }
}
