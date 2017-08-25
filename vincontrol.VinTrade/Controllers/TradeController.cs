using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Vinsocial.Forms.ReviewManagement;
using vincontrol.Application.Forms.TradeInManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Application.ViewModels.TradeInManagement;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.VinTrade.Handlers;
using Vincontrol.Market;
using vincontrol.VinTrade.Helpers;
using ChromeService = vincontrol.ChromeAutoService.ChromeAutoService;
using ExtendedEquipmentOption = vincontrol.DomainObject.TradeIn.ExtendedEquipmentOption;

namespace vincontrol.VinTrade.Controllers
{
    public class TradeController : Controller
    {
        private IDealerManagementForm _dealerManagement;
        private ICommonManagementForm _commonManagement;
        private ITradeInManagementForm _tradeInManagement;
        private IReviewManagementForm _reviewManagement;
        private IInventoryManagementForm _inventoryManagement;

        public TradeController()
        {
            _dealerManagement = new DealerManagementForm();
            _commonManagement = new CommonManagementForm();
            _tradeInManagement = new TradeInManagementForm();
            _reviewManagement = new ReviewManagementForm();
            _inventoryManagement = new InventoryManagementForm();
        }

        public ActionResult AppraiseAnotherVehicle()
        {
            return RedirectToRoute("TradeStepOne", new { dealer =SessionHandler.TradeInDealer.DealershipName });
        }

        public ActionResult Index(string dealer)
        {
            return RedirectToRoute("TradeStepOne", new { dealer = "newportcoastauto" });
        }

        [HttpPost]
        public ActionResult VinDecode(string vin)
        {
            var autoService = new ChromeService();
            var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);
            SessionHandler.VehicleInfo = vehicleInfo;
            return Content(vehicleInfo == null ? "False" : "True");
        }
     
        public ActionResult StepOneDecode(string dealer)
        {
            // clear session
            SessionHandler.TradeInDealer = null;

            var dealerInfo = _dealerManagement.GetDealerByName(dealer);
            if (dealerInfo == null) return View("TradeInStep1", new TradeInVehicleModel());

            SessionHandler.TradeInDealer = dealerInfo;
            
            var viewModel = new TradeInVehicleModel()
            {
                YearsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                IsValidDealer = true,
                ValidVin = true,
                Dealer = dealerInfo.DealershipId.ToString(),
                DealerName = dealer,
                Vin = SessionHandler.PreviousVin,
                Mileage = SessionHandler.PreviousMilage,
                Condition = SessionHandler.PreviousCondition
            };

            try
            {
                viewModel.YearsList = _commonManagement.GetChromeYear();
                SessionHandler.ChromeYears = viewModel.YearsList;
            }
            catch (Exception)
            {
            }
            
            return View("TradeInStep1", viewModel);
        }

        public ActionResult StepOneDecodeInterested(string dealer, string interestedVehicle)
        {
            // clear session
            SessionHandler.TradeInDealer = null;
            SessionHandler.InterestedVehicle = interestedVehicle;

            var dealerInfo = _dealerManagement.GetDealerByName(dealer);
            if (dealerInfo == null) return View("TradeInStep1", new TradeInVehicleModel());

            SessionHandler.TradeInDealer = dealerInfo;

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                IsValidDealer = true,
                ValidVin = true,
                Dealer = dealerInfo.DealershipId.ToString(),
                DealerName = dealer,
                Vin = SessionHandler.PreviousVin,
                Mileage = SessionHandler.PreviousMilage,
                Condition = SessionHandler.PreviousCondition
            };

            try
            {
                viewModel.YearsList = _commonManagement.GetChromeYear();
                SessionHandler.ChromeYears = viewModel.YearsList;
            }
            catch (Exception)
            {
            }
            
            return View("TradeInStep1", viewModel);
        }

        public ActionResult TradeInWithOptions(TradeInVehicleModel vehicle)
        {
            DealershipViewModel dealerInfo = null;
            if (SessionHandler.TradeInDealer != null)
            {
                dealerInfo = SessionHandler.TradeInDealer;
            }
            else
            {
                return RedirectToRoute("StepOneDecode", new { dealer=vehicle.DealerName });
            }

            string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);
            int mileageNumber; Int32.TryParse(mileageString, out mileageNumber);
            
            vehicle.OptionalEquipment = new List<ExtendedEquipmentOption>();
            vehicle.MileageNumber = mileageNumber;
            vehicle.InterestedVehicle = SessionHandler.InterestedVehicle;
            vehicle.DealerId = dealerInfo.DealershipId;
            //TODO: Get options from chrome
            try
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                if (!String.IsNullOrEmpty(vehicle.Vin))
                {
                    var vehicleInfo = SessionHandler.VehicleInfo;
                    vehicle.OptionalEquipment = GetOptionalEquipment(vehicleInfo).Select(i => new ExtendedEquipmentOption()
                    {
                        VehicleOptionId = Convert.ToInt32(i.Id),
                        DisplayName = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
                        DisplayNameAdditionalData = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
                        IsSelected = i.IsSelected
                    }).ToList();

                   
                    vehicle.SelectedYear = vehicleInfo.modelYear;
                    vehicle.SelectedMakeValue = vehicleInfo.bestMakeName;
                    vehicle.SelectedModelValue = vehicleInfo.bestModelName;
                    vehicle.SelectedTrimValue = vehicleInfo.bestTrimName;
                    vehicle.ImageUrl = vehicleInfo.style.FirstOrDefault() == null ? "" : vehicleInfo.style.FirstOrDefault().stockImage.url;
                    
                }
                else
                {
                    int result;
                    if (int.TryParse(vehicle.SelectedTrim, out result))
                    {
                        var trimOptionsList = _tradeInManagement.GetOptionalEquipments(result);
                        if (trimOptionsList.Any())
                        {
                            vehicle.OptionalEquipment = trimOptionsList.Count <= 21
                                                            ? trimOptionsList
                                                            : trimOptionsList.GetDistinctRandom(21);
                        }
                        else
                        {
                            vehicle.OptionalEquipment = new List<ExtendedEquipmentOption>();
                        }
                    }
                }
            }
            catch (Exception e) { }

            if (vehicle.OptionalEquipment.Any())
            {
                SessionHandler.TradeInVehicle = vehicle;
                return RedirectToRoute("TradeStepTwo", new { dealer = vehicle.DealerName });
            }
            else
            {
                vehicle.SkipStepTwo = true;
                SessionHandler.TradeInVehicle = vehicle;
                return RedirectToRoute("TradeStepThree", new { dealer = vehicle.DealerName });
            }
        }

        public ActionResult StepTwoOptions(string dealer)
        {
            // clear session
            if (SessionHandler.TradeInVehicle == null) return RedirectToRoute("StepOneDecode", new { dealer });
            return View("TradeInStep2", SessionHandler.TradeInVehicle);
        }

        public ActionResult TradeInCustomer(TradeInVehicleModel vehicle)
        {
            if (SessionHandler.TradeInVehicle == null) return RedirectToRoute("StepOneDecode", new { dealer= vehicle.DealerName});

            SessionHandler.TradeInVehicle.SelectedOptions = vehicle.SelectedOptions;
            
            return RedirectToRoute("TradeStepThree", new { dealer = vehicle.DealerName });
        }

        public ActionResult StepThreeContact(string dealer)
        {
            // clear session
            if (SessionHandler.TradeInVehicle == null) return RedirectToRoute("TradeStepOne", new {dealer});

            var trimId = Convert.ToInt32(SessionHandler.TradeInVehicle.SelectedTrim);

             var imageUrl= _tradeInManagement.GetStockImage(trimId);

             if (!String.IsNullOrEmpty(imageUrl))
                 SessionHandler.TradeInVehicle.ImageUrl = imageUrl;
            

            return View("TradeInStep3", SessionHandler.TradeInVehicle);
        }

        public ActionResult GetTradeInValue(TradeInVehicleModel vehicle)
        {
            if (SessionHandler.TradeInVehicle == null)
                return RedirectToRoute("TradeStepOne", new {dealer = vehicle.DealerName});

            if (!ValidateCustomerSubmitForm(vehicle))
            {
                return RedirectToRoute("TradeStepThree", new {dealer = vehicle.DealerName});
            }
            else
            {


                try
                {
                    var dealer = SessionHandler.TradeInDealer;

                    SessionHandler.TradeInVehicle.CustomerFirstName = vehicle.CustomerFirstName;

                    SessionHandler.TradeInVehicle.CustomerLastName = vehicle.CustomerLastName;

                    SessionHandler.TradeInVehicle.CustomerEmail = vehicle.CustomerEmail;

                    SessionHandler.TradeInVehicle.CustomerPhone =
                        CommonHelper.RemoveSpecialCharacters(vehicle.CustomerPhone);

                
                   
                        var selectedYear = Convert.ToInt32(SessionHandler.TradeInVehicle.SelectedYear);

                        var context = new VinMarketEntities();

                        var targetCar = new Inventory()
                        {
                            Vehicle = new Vehicle()
                            {
                                Year = selectedYear,
                                Make = SessionHandler.TradeInVehicle.SelectedMakeValue,
                                Model = SessionHandler.TradeInVehicle.SelectedModelValue,
                                Trim = SessionHandler.TradeInVehicle.SelectedTrimValue,
                            }
                        };



                        var chartGraph = new ChartGraph()
                        {
                            Target = new ChartGraph.TargetCar()
                            {
                                ListingId = 0,
                                Mileage = 0,
                                SalePrice = 0,
                                Distance = 0,
                                Title =
                                    new TitleInfo
                                    {
                                        Make = targetCar.Vehicle.Make,
                                        Model = targetCar.Vehicle.Model.TrimEnd(),
                                        Trim =
                                            string.IsNullOrEmpty(targetCar.Vehicle.Trim)
                                                ? "other"
                                                : targetCar.Vehicle.Trim,
                                        Year = targetCar.Vehicle.Year ?? 2013
                                    },
                                Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim) ? "other" : targetCar.Vehicle.Trim,
                                Seller =
                                    new SellerInfo
                                    {
                                        SellerName = dealer.DealershipName,
                                        SellerAddress =
                                            dealer.Address + " " +
                                            dealer.City +
                                            "," + dealer.State +
                                            " " +
                                            dealer.ZipCode
                                    },


                            }
                        };

                        var unionList = MarketHelper.GetMarketCarList(SessionHandler.TradeInDealer, context,
                            targetCar.Vehicle, null,
                            null, Constanst.ChartCarType.All, chartGraph, true, false, false);

                    if (unionList != null && unionList.Any())
                    {
                        var limitMarketList = unionList.Where(x => x.Distance < 250).ToList();

                        if (limitMarketList.Any())
                        {
                            MarketHelper.SetValuesOnMarketToCarList(chartGraph, limitMarketList);

                            SessionHandler.TradeInVehicle.MarketLowestPrice = chartGraph.Market.MinimumPrice;
                            SessionHandler.TradeInVehicle.MarketAveragePrice = chartGraph.Market.AveragePrice;
                            SessionHandler.TradeInVehicle.MarketHighestPrice = chartGraph.Market.MaximumPrice;

                            SessionHandler.TradeInVehicle.MarketLowestMileage =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MinimumPrice).Miles;
                            SessionHandler.TradeInVehicle.MarketAverageMileage =
                                (long) Math.Round(chartGraph.TypedChartModels.Average(x => x.Miles));
                            SessionHandler.TradeInVehicle.MarketHighestMileage =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MaximumPrice).Miles;

                            if (dealer.DealerSetting.AverageProfitUsage == 1)
                            {
                                SessionHandler.TradeInVehicle.TradeInFairPrice = chartGraph.Market.MinimumPrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 dealer.DealerSetting.AverageProfit;

                                SessionHandler.TradeInVehicle.TradeInGoodPrice = chartGraph.Market.AveragePrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 dealer.DealerSetting.AverageProfit;
                                SessionHandler.TradeInVehicle.TradeInVeryGoodPrice = chartGraph.Market.MaximumPrice -
                                                                                     dealer.DealerSetting.AverageCost -
                                                                                     dealer.DealerSetting.AverageProfit;
                            }
                            else
                            {
                                SessionHandler.TradeInVehicle.TradeInFairPrice = chartGraph.Market.MinimumPrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 (chartGraph.Market.MinimumPrice*
                                                                                  dealer.DealerSetting
                                                                                      .AverageProfitPercentage)/100;
                                SessionHandler.TradeInVehicle.TradeInGoodPrice = chartGraph.Market.AveragePrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 (chartGraph.Market.MinimumPrice*
                                                                                  dealer.DealerSetting
                                                                                      .AverageProfitPercentage)/100;
                                SessionHandler.TradeInVehicle.TradeInVeryGoodPrice = chartGraph.Market.MaximumPrice -
                                                                                     dealer.DealerSetting.AverageCost -
                                                                                     (chartGraph.Market.MinimumPrice*
                                                                                      dealer.DealerSetting
                                                                                          .AverageProfitPercentage)/100;
                            }


                            SessionHandler.TradeInVehicle.AboveThumnailUrl =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MaximumPrice)
                                    .ThumbnailURL;
                            SessionHandler.TradeInVehicle.BelowThumbnailUrl =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MinimumPrice)
                                    .ThumbnailURL;
                        }
                        else
                        {
                            MarketHelper.SetValuesOnMarketToCarList(chartGraph, unionList);

                            SessionHandler.TradeInVehicle.MarketLowestPrice = chartGraph.Market.MinimumPrice;
                            SessionHandler.TradeInVehicle.MarketAveragePrice = chartGraph.Market.AveragePrice;
                            SessionHandler.TradeInVehicle.MarketHighestPrice = chartGraph.Market.MaximumPrice;

                            SessionHandler.TradeInVehicle.MarketLowestMileage =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MinimumPrice).Miles;
                            SessionHandler.TradeInVehicle.MarketAverageMileage =
                                (long) Math.Round(chartGraph.TypedChartModels.Average(x => x.Miles));
                            SessionHandler.TradeInVehicle.MarketHighestMileage =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MaximumPrice).Miles;

                            if (dealer.DealerSetting.AverageProfitUsage == 1)
                            {
                                SessionHandler.TradeInVehicle.TradeInFairPrice = chartGraph.Market.MinimumPrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 dealer.DealerSetting.AverageProfit;

                                SessionHandler.TradeInVehicle.TradeInGoodPrice = chartGraph.Market.AveragePrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 dealer.DealerSetting.AverageProfit;
                                SessionHandler.TradeInVehicle.TradeInVeryGoodPrice = chartGraph.Market.MaximumPrice -
                                                                                     dealer.DealerSetting.AverageCost -
                                                                                     dealer.DealerSetting.AverageProfit;
                            }
                            else
                            {
                                SessionHandler.TradeInVehicle.TradeInFairPrice = chartGraph.Market.MinimumPrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 (chartGraph.Market.MinimumPrice*
                                                                                  dealer.DealerSetting
                                                                                      .AverageProfitPercentage)/100;
                                SessionHandler.TradeInVehicle.TradeInGoodPrice = chartGraph.Market.AveragePrice -
                                                                                 dealer.DealerSetting.AverageCost -
                                                                                 (chartGraph.Market.MinimumPrice*
                                                                                  dealer.DealerSetting
                                                                                      .AverageProfitPercentage)/100;
                                SessionHandler.TradeInVehicle.TradeInVeryGoodPrice = chartGraph.Market.MaximumPrice -
                                                                                     dealer.DealerSetting.AverageCost -
                                                                                     (chartGraph.Market.MinimumPrice*
                                                                                      dealer.DealerSetting
                                                                                          .AverageProfitPercentage)/100;
                            }


                            SessionHandler.TradeInVehicle.AboveThumnailUrl =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MaximumPrice)
                                    .ThumbnailURL;
                            SessionHandler.TradeInVehicle.BelowThumbnailUrl =
                                chartGraph.TypedChartModels.First(x => x.Price == chartGraph.Market.MinimumPrice)
                                    .ThumbnailURL;
                        }


                    }

                    if (SessionHandler.TradeInVehicle.TradeInGoodPrice <= 5000)
                    {
                        SessionHandler.TradeInVehicle.TradeInFairPrice = 0;

                        SessionHandler.TradeInVehicle.TradeInGoodPrice = 0;
                        SessionHandler.TradeInVehicle.TradeInVeryGoodPrice = 0;
                    }

                    if (dealer.DealershipId == 12056 || dealer.DealershipId == 35421)
                    {
                        SessionHandler.TradeInVehicle.TradeInFairPrice = 0;

                        SessionHandler.TradeInVehicle.TradeInGoodPrice = 0;
                        SessionHandler.TradeInVehicle.TradeInVeryGoodPrice = 0;
                    }

                    






                }
                catch (Exception e)
                {

                }

                return RedirectToRoute("TradeVehicleValue", new {dealer = vehicle.DealerName});
            }
        }

        public ActionResult ReviewsData(int dealerId)
        {
            try
            {
                _tradeInManagement.SaveTradeInCustomer(SessionHandler.TradeInVehicle);

                TradeInEmailHelper.SendEmail(SessionHandler.TradeInVehicle, SessionHandler.TradeInDealer);

                DealerReviewViewModel dealerRview;

                var userReviews = _reviewManagement.GetGoodUserReviews(dealerId);

                if (userReviews.Any())
                {
                    dealerRview = userReviews.Count <= 20 ? new DealerReviewViewModel {UserReviews = userReviews} : new DealerReviewViewModel {UserReviews = userReviews.GetDistinctRandom(20)};
                }
                else
                {
                    return PartialView("ReviewsData", new DealerReviewViewModel());
                }


                return PartialView("ReviewsData", dealerRview);

            }
            catch (Exception)
            {
                return PartialView("ReviewsData", null);
            }
        }

        public ActionResult VehicleValue(string dealer)
        {
            // clear session
            if (SessionHandler.TradeInVehicle == null) return RedirectToRoute("StepOneDecode", new { dealer });
            return View("VehicleValue", SessionHandler.TradeInVehicle);
        }

        public ActionResult GetMakes(int yearId)
        {
            var viewModel = new TradeInVehicleModel { SelectedMake = "0", MakesList = new List<ExtendedSelectListItem>() };
            try
            {
                viewModel.MakesList = ExceptionHelper.FilterMakes(_commonManagement.GetChromeMake(yearId));
           
            }
            catch (Exception e)
            {

            }
            return PartialView("Makes", viewModel);
        }

        public ActionResult GetModels(int yearId, int makeId)
        {
            var viewModel = new TradeInVehicleModel { SelectedModel = "0", ModelsList = new List<ExtendedSelectListItem>() };
            try
            {
                viewModel.ModelsList = _commonManagement.GetChromeModel(yearId, makeId);
              
            }
            catch (Exception)
            {

            }
            return PartialView("Models", viewModel);
        }

        public ActionResult GetTrims(int yearId, int makeId, int modelId)
        {
            var viewModel = new TradeInVehicleModel() { SelectedTrim = "0", TrimsList = new List<ExtendedSelectListItem>() };
            try
            {
              
                viewModel.TrimsList = ExceptionHelper.FilterTrims(_commonManagement.GetChromeTrim(yearId, makeId, modelId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return PartialView("Trims", viewModel);
        }
        
        public ActionResult Reviews()
        {
            var soldCars = _inventoryManagement.GetUsedSoldInventories(SessionHandler.TradeInDealer.DealershipId);
            return View(soldCars);
        }

        #region Private Methods

        private bool ValidateCustomerSubmitForm(TradeInVehicleModel customer)
        {
            if (String.IsNullOrEmpty(customer.CustomerFirstName))
            {
                ModelState.AddModelError("CustomerFirstName", "First Name is Required");
            }
            else if (String.IsNullOrEmpty(customer.CustomerFirstName.Trim()))
            {
                ModelState.AddModelError("CustomerFirstName", "First Name is Required");
            }

            if (String.IsNullOrEmpty(customer.CustomerLastName))
            {
                ModelState.AddModelError("CustomerLastName", "Last Name is Required");

            }
            else if (String.IsNullOrEmpty(customer.CustomerLastName.Trim()))
            {
                ModelState.AddModelError("CustomerLastName", "Last Name is Required");
            }

            if (String.IsNullOrEmpty(customer.CustomerEmail))
            {
                ModelState.AddModelError("CustomerEmail", "Email is Required");

            }
            else if (String.IsNullOrEmpty(customer.CustomerEmail.Trim()))
            {
                ModelState.AddModelError("CustomerEmail", "Email is Required");
            }

            return ModelState.IsValid;
        }

        private IEnumerable<OptionContract> GetOptionalEquipment(VehicleDescription vehicleInfo)
        {
            var autoService = new ChromeService();
            if (vehicleInfo.style != null && vehicleInfo.style.Any())
            {
                var styleInfo = autoService.GetStyleInformationFromStyleId(vehicleInfo.style.First().id);
                return GetOptionList(styleInfo);
            }
            return new List<OptionContract>();
        }

        private static IEnumerable<OptionContract> GetOptionList(VehicleDescription styleInfo)
        {
            var optionList = new List<OptionContract>();
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            var hash = new Hashtable();

            if (styleInfo != null && styleInfo.factoryOption != null)
            {
                optionList.AddRange(from fo in styleInfo.factoryOption
                                    let name = CommonHelper.TrimString(fo.description.First(), 40)
                                    let newString = regex.Replace(name, m => m.Value.ToLowerInvariant())
                                    where !hash.Contains(name) && fo.price.msrpMax > 0 
                                    select new OptionContract
                                    {
                                        __type = "KBB.Karpower.WebServices.LightOption",
                                        Id = fo.header.id.ToString(),
                                        DisplayName = fo.description[0]
                                    });
            }
            return optionList;
        }

        private void KeepValueForPreviousPage(TradeInVehicleModel vehicle)
        {
            Session["PreviousVin"] = vehicle.Vin;
            Session["PreviousMilage"] = vehicle.Mileage;
            Session["PreviousCondition"] = vehicle.Condition;
            Session["PreviousSelectedYear"] = vehicle.SelectedYear;
            Session["PreviousSelectedMake"] = vehicle.SelectedMake;
            Session["PreviousSelectedModel"] = vehicle.SelectedModel;
            Session["PreviousSelectedTrim"] = vehicle.SelectedTrim;
        }

        
        #endregion
    }
}
