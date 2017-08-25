using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.ChromeAutoService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using vincontrol.ChromeAutoService.AutomativeService;
using Vincontrol.Web.Security;
using ExtendedEquipmentOption = Vincontrol.Web.Models.ExtendedEquipmentOption;
using EncryptionHelper = vincontrol.Helper.EncryptionHelper;
using EmailHelper = Vincontrol.Web.HelperClass.EmailHelper;
using DataHelper = Vincontrol.Web.HelperClass.DataHelper;
using vincontrol.Helper;
using vincontrol.DomainObject;
using KarPowerService = vincontrol.KBB.KBBService;

namespace Vincontrol.Web.Controllers
{
    public class TradeInController : SecurityController
    {
        private KarPowerService _karPowerService;
        private ICarFaxService _carFaxService;

        public TradeInController()
        {
            _karPowerService = new KarPowerService();
            _carFaxService = new CarFaxService();
        }

  

        public ActionResult TradeInCustomer(TradeInVehicleModel vehicle)
        {
            var context = new VincontrolEntities();

            var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

            var comments = context.TradeInComments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

            vehicle.ReviewList = new List<CustomerReview>();

            foreach (var tmp in comments)
            {
                var customerReview = new CustomerReview()
                                         {
                                             City = tmp.City,
                                             Name = tmp.Name,
                                             ReviewContent = tmp.Content,
                                             State = tmp.State
                                         };

                vehicle.ReviewList.Add(customerReview);
            }

            return View("TradeInCustomer", vehicle);
        }

        public ActionResult TradeInCustomerByKarPower(TradeInVehicleModel vehicle)
        {
            var context = new VincontrolEntities();

            var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

            var comments = context.TradeInComments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

            vehicle.ReviewList = new List<CustomerReview>();

            foreach (var tmp in comments)
            {
                var customerReview = new CustomerReview()
                {
                    City = tmp.City,
                    Name = tmp.Name,
                    ReviewContent = tmp.Content,
                    State = tmp.State
                };

                vehicle.ReviewList.Add(customerReview);
            }

            return View("TradeInCustomerByKarPower", vehicle);
        }

        public ActionResult TradeInVehicle(string dealer)
        {
            Session["TradeInDealer"] = null;

            int dealerId = 0;

            dealer = dealer.Replace(" ", "+");
            dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(dealer));

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                IsValidDealer = true,
                ValidVin = true,
                Dealer = dealer,

            };
            Session["TradeInDealer"] = null;

            if (Session["TradeInDealer"] == null)
            {
                using (var context = new VincontrolEntities())
                {
                    var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                   
                    var dealerInfo = new DealershipViewModel(dealerDefault)
                        {
                            DealershipId = dealerId,
                            EncryptDealerId = dealer
                        };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new VincontrolEntities())
                    {
                        var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                     
                        var dealerInfo = new DealershipViewModel(dealerDefault)
                        {
                            DealershipId = dealerId,
                            EncryptDealerId = dealer
                        };

                        Session["TradeInDealer"] = dealerInfo;
                    }
                }
            }

            var httpCookie = Response.Cookies["Vindealer"];

            if (httpCookie != null)
            {
                httpCookie.Value = EncryptionHelper.EncryptString(dealerId.ToString());
                httpCookie.Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                httpCookie = new HttpCookie("Vindealer") { Value = EncryptionHelper.EncryptString(dealerId.ToString(CultureInfo.InvariantCulture)), Expires = DateTime.Now.AddDays(1) };

                Response.Cookies.Add(httpCookie);
            }


            try
            {
                _karPowerService.GetYears(DateTime.Now);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
                viewModel.YearsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch (Exception)
            {

            }

            return View("TradeVehicleByKarPower", viewModel);
        }

        public ActionResult TradeInVehicleByKarPower(string dealer)
        {
            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),

                ValidVin = true,
                Dealer = dealer
            };

            try
            {
                _karPowerService.GetYears(DateTime.Now);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
                viewModel.YearsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch (Exception)
            {

            }

            var httpCookie = Response.Cookies["Vindealer"];

            if (httpCookie != null)
            {
                httpCookie.Value = dealer;
                httpCookie.Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                httpCookie = new HttpCookie("Vindealer") { Value = dealer, Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Add(httpCookie);
            }

            return View("TradeVehicleByKarPower", viewModel);
        }

        public ActionResult IndexWithVin(string dealer, string vin)
        {
            // clear session
            Session["TradeInDealer"] = null;

            int dealerId = 0;

            if (!String.IsNullOrEmpty(dealer) && dealer.Contains(" "))
            {
                dealer = dealer.Replace(" ", String.Empty);
            }
            using (var context = new VincontrolEntities())
            {
                var existingDealerGroup = context.DealerGroups.FirstOrDefault(i => i.DealerGroupName.ToLower().Equals(dealer.ToLower()));
                if (existingDealerGroup != null)
                {
                    dealerId = existingDealerGroup.DefaultDealerId;
                }
                else
                {
                    var existingDealer = context.Dealers.FirstOrDefault(i => i.Name.Replace(" ", String.Empty).ToLower().Equals(dealer.ToLower()));
                    if (existingDealer != null)
                        dealerId = existingDealer.DealerId;
                }
            }

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),

                ValidVin = true,
                Dealer = dealer,

            };
            Session["TradeInDealer"] = null;
            if (Session["TradeInDealer"] == null)
            {
                using (var context = new VincontrolEntities())
                {
                    var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                 
                    var dealerInfo = new DealershipViewModel(dealerDefault)
                    {
                        DealershipId = dealerId,
                        EncryptDealerId = dealer
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new VincontrolEntities())
                    {
                        var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                        //var dealerDefaultSetting = context.Settings.First(x => x.DealerId == dealerId);

                        var dealerInfo = new DealershipViewModel(dealerDefault)
                        {
                            DealershipId = dealerId,
                            EncryptDealerId = dealer
                        };

                        Session["TradeInDealer"] = dealerInfo;
                    }
                }
            }

            var httpCookie = Response.Cookies["Vindealer"];

            if (httpCookie != null)
            {
                httpCookie.Value = EncryptionHelper.EncryptString(dealerId.ToString());
                httpCookie.Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                httpCookie = new HttpCookie("Vindealer") { Value = EncryptionHelper.EncryptString(dealerId.ToString(CultureInfo.InvariantCulture)), Expires = DateTime.Now.AddDays(1) };

                Response.Cookies.Add(httpCookie);
            }

            Session["CustomerLookUpVin"] = vin;

            try
            {
                _karPowerService.GetYears(DateTime.Now);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
                viewModel.YearsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch (Exception)
            {

            }

            return View("TradeVehicleByKarPower", viewModel);
        }

        public ActionResult TradeInVehicleWithVin(string dealer, string vin)
        {
            Session["TradeInDealer"] = null;

            int dealerId = 0;

            dealer = dealer.Replace(" ", "+");
            dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(dealer));

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                IsValidDealer = true,
                ValidVin = true,
                Dealer = dealer,

            };
            Session["TradeInDealer"] = null;

            if (Session["TradeInDealer"] == null)
            {
                using (var context = new VincontrolEntities())
                {
                    var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                 
                    var dealerInfo = new DealershipViewModel(dealerDefault)
                    {
                        DealershipId = dealerId,
                        EncryptDealerId = dealer
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new VincontrolEntities())
                    {
                        var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                      

                        var dealerInfo = new DealershipViewModel(dealerDefault)
                        {
                            DealershipId = dealerId,
                            EncryptDealerId = dealer
                        };

                        Session["TradeInDealer"] = dealerInfo;
                    }
                }
            }

            var httpCookie = Response.Cookies["Vindealer"];

            if (httpCookie != null)
            {
                httpCookie.Value = EncryptionHelper.EncryptString(dealerId.ToString());
                httpCookie.Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                httpCookie = new HttpCookie("Vindealer") { Value = EncryptionHelper.EncryptString(dealerId.ToString(CultureInfo.InvariantCulture)), Expires = DateTime.Now.AddDays(1) };

                Response.Cookies.Add(httpCookie);
            }

            Session["CustomerLookUpVin"] = vin;

            try
            {
                _karPowerService.GetYears(DateTime.Now);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
                viewModel.YearsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch (Exception)
            {

            }

            return View("TradeVehicleByKarPower", viewModel);
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

        public ActionResult PreviousTradeInVehicleWithVinByKarPower(TradeInVehicleModel vehicle)
        {
            vehicle.YearsList = (IEnumerable<ExtendedSelectListItem>)Session["KarPowerYears"] ?? new List<ExtendedSelectListItem>();
            vehicle.MakesList = (IEnumerable<ExtendedSelectListItem>)Session["KarPowerMakes"] ?? new List<ExtendedSelectListItem>();
            vehicle.ModelsList = (IEnumerable<ExtendedSelectListItem>)Session["KarPowerModels"] ?? new List<ExtendedSelectListItem>();
            vehicle.TrimsList = (IEnumerable<ExtendedSelectListItem>)Session["KarPowerTrims"] ?? new List<ExtendedSelectListItem>();
            vehicle.IsValidDealer = true;

            KeepValueForPreviousPage(vehicle);

       
            return RedirectToAction("Index", new { dealer = vehicle.DealerName.ToLower().Replace(" ", string.Empty) });
        }

        public ActionResult TradeInVehicleWithVinByKarPower(string dealer, string vin)
        {
            vin = string.Empty;
            dealer = dealer.Replace(" ", "+");
            int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(dealer));

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),

                ValidVin = true,
                Dealer = dealer,
                Vin = vin
            };
            Session["TradeInDealer"] = null;
            if (Session["TradeInDealer"] == null)
            {
                using (var context = new VincontrolEntities())
                {
                    var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                  
                    var dealerInfo = new DealershipViewModel(dealerDefault)
                    {
                        DealershipId = dealerId,
                        EncryptDealerId = dealer
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new VincontrolEntities())
                    {
                        var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                    
                        var dealerInfo = new DealershipViewModel(dealerDefault)
                        {
                            DealershipId = dealerId,
                            EncryptDealerId = dealer
                        };

                        Session["TradeInDealer"] = dealerInfo;
                    }
                }
            }

            var httpCookie = Response.Cookies["Vindealer"];

            if (httpCookie != null)
            {
                httpCookie.Value = dealer;
                httpCookie.Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                httpCookie = new HttpCookie("Vindealer") { Value = dealer, Expires = DateTime.Now.AddDays(1) };

                Response.Cookies.Add(httpCookie);
            }

            Session["CustomerLookUpVin"] = vin;

            try
            {
                _karPowerService.GetYears(DateTime.Now);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
                viewModel.YearsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch (Exception)
            {

            }

            return View("TradeVehicleByKarPower", viewModel);
        }

        public ActionResult Index(string dealer)
        {
            // clear session
            Session["TradeInDealer"] = null;

            int dealerId = 0;

            if (!String.IsNullOrEmpty(dealer) && dealer.Contains(" "))
            {
                dealer = dealer.Replace(" ", String.Empty);
            }

            bool isValidDealer = false;
            using (var context = new VincontrolEntities())
            {
                var existingDealerGroup = context.DealerGroups.FirstOrDefault(i => i.DealerGroupName.ToLower().Equals(dealer.ToLower()));
                if (existingDealerGroup != null)
                {
                    dealerId = existingDealerGroup.DefaultDealerId;
                    isValidDealer = true;
                }
                else
                {
                    var existingDealer = context.Dealers.FirstOrDefault(i => i.Name.Replace(" ", String.Empty).Replace("-", String.Empty).ToLower().Equals(dealer.ToLower()));
                    if (existingDealer != null)
                    {
                        dealerId = existingDealer.DealerId;
                        isValidDealer = true;
                    }
                }
            }

            var viewModel = new TradeInVehicleModel()
                                {
                                    YearsList = SelectListHelper.InitialYearListFromKBB(),
                                    MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),
                                    ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                                    TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),
                                    IsValidDealer = isValidDealer,
                                    ValidVin = true,
                                    Dealer = dealerId.ToString(CultureInfo.InvariantCulture),
                                    DealerName = dealer,
                                    Vin = Session["PreviousVin"] != null ? (string)Session["PreviousVin"] : string.Empty,
                                    Mileage = Session["PreviousMilage"] != null ? (string)Session["PreviousMilage"] : string.Empty,
                                    Condition = Session["PreviousCondition"] != null ? (string)Session["PreviousCondition"] : string.Empty
                                };

            try
            {
                _karPowerService.GetYears(DateTime.Now);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
                viewModel.YearsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch (Exception)
            {

            }

            var httpCookie = Response.Cookies["Vindealer"];

            if (httpCookie != null)
            {
                httpCookie.Value = EncryptionHelper.EncryptString(dealerId.ToString());
                httpCookie.Expires = DateTime.Now.AddDays(1);
            }
            else
            {
                httpCookie = new HttpCookie("Vindealer") { Value = EncryptionHelper.EncryptString(dealerId.ToString(CultureInfo.InvariantCulture)), Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Add(httpCookie);
            }

            return View("TradeVehicleByKarPower", viewModel);
        }

        [HttpPost]
        public ActionResult VinDecode(string vin)
        {
            var autoService = new ChromeAutoService();
            var vehicleInfo = autoService.GetVehicleInformationFromVin(vin);
            return Content(vehicleInfo == null ? "False" : "True");
        }

        public ActionResult TradeInVehicleInvalidVin()
        {
            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),

                MakesList = new List<ExtendedSelectListItem>().AsEnumerable(),

                ModelsList = new List<ExtendedSelectListItem>().AsEnumerable(),

                TrimsList = new List<ExtendedSelectListItem>().AsEnumerable(),

                ValidVin = false,

            };


            return View("TradeVehicle", viewModel);

        }
        
        private IEnumerable<OptionContract> GetOptionalEquipment(VehicleDescription vehicleInfo)
        {
            var autoService = new ChromeAutoService();
            if (vehicleInfo.style != null && vehicleInfo.style.Any())
            {
                var styleInfo = autoService.GetStyleInformationFromStyleId(vehicleInfo.style.First().id);
                return GetOptionList(styleInfo);
            }
            return new List<OptionContract>();
        }

        private static IEnumerable<OptionContract> GetOptionalEquipment(string selectedYear, string selectedMake, string selectedModel)
        {
            var autoService = new ChromeAutoService();

            int resultYear;
            var optionList = new List<OptionContract>();
            if (int.TryParse(selectedYear, out resultYear))
            {
                var chromeMakeId = GetChromeMake(autoService, resultYear, selectedMake);
                var chromeModelId = GetChromeModel(autoService, resultYear, selectedModel, chromeMakeId);
                var chromeStyleId = GetChromeTrim(autoService, chromeModelId);
                var styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);
                optionList = GetOptionList(styleInfo);
            }

            return optionList;
        }



        private static List<OptionContract> GetOptionList(VehicleDescription styleInfo)
        {
            var optionList = new List<OptionContract>();
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            var hash = new Hashtable();

            if (styleInfo != null && styleInfo.factoryOption != null)
            {
                optionList.AddRange(from fo in styleInfo.factoryOption
                                    let name = CommonHelper.TrimString(fo.description.First(), 40)
                                    let newString = regex.Replace(name, m => m.Value.ToLowerInvariant())
                                    where !hash.Contains(name) && fo.price.msrpMax > 0 && !name.Contains("PKG") && !name.Contains("PACKAGE") && fo.description.Any()
                                    select new OptionContract
                                    {
                                        __type = "KBB.Karpower.WebServices.LightOption",
                                        Id = fo.header.ToString(),
                                        DisplayName = fo.description[0]
                                    });
            }
            return optionList;
        }

        private static int GetChromeMake(ChromeAutoService autoService, int selectedYear, string selectedMake)
        {
            var chromeMake = autoService.GetDivisions(Convert.ToInt32(selectedYear)).FirstOrDefault(i => i.Value.ToLower() == selectedMake.ToLower());
            return chromeMake != null ? chromeMake.id : 0;
        }

        private static int GetChromeModel(ChromeAutoService autoService, int selectedYear, string selectedModel, int chromeMakeId)
        {
            if (chromeMakeId == 0) return 0;
            var chromeModel = autoService.GetModelsByDivision(selectedYear, chromeMakeId).FirstOrDefault(i => i.Value.ToLower() == selectedModel.ToLower());
            return chromeModel != null ? chromeModel.id : 0;
        }

        private static int GetChromeTrim(ChromeAutoService autoService, int chromeModelId)
        {
            if (chromeModelId == 0) return 0;
            var chromeStyle = autoService.GetStyles(chromeModelId).First();
            return chromeStyle != null ? chromeStyle.id : 0;
        }

        //public ActionResult TradeInOptions(TradeInVehicleModel vehicle)
        //{
        //    if (!String.IsNullOrEmpty(vehicle.SelectedTrim) && !vehicle.SelectedTrim.Contains("Trim..."))
        //    {

        //        vehicle.VehicleId = Convert.ToInt32(vehicle.SelectedTrim);

        //        string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

        //        int mileageNumber = 0; Int32.TryParse(mileageString, out mileageNumber);

        //        if (Session["TradeInDealer"] != null)
        //        {

        //            var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

        //            var returnVehicle = KellyBlueBookHelper.GetKBBVehicleFromVehicleId(vehicle.VehicleId, mileageNumber, dealerSessionInfo.ZipCode);

        //            returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.SelectedTrim);

        //            if (returnVehicle.OptionalEquipment.Any())
        //            {


        //                return View("TradeInOptions", returnVehicle);
        //            }
        //            else
        //            {
        //                var context = new VincontrolEntities();


        //                var comments =
        //                    context.TradeInComments.Where(
        //                        x => x.DealerId == dealerSessionInfo.DealershipId);

        //                returnVehicle.ReviewList = new List<CustomerReview>();

        //                foreach (var tmp in comments)
        //                {
        //                    var customerReview = new CustomerReview()
        //                    {
        //                        City = tmp.City,
        //                        Name = tmp.Name,
        //                        ReviewContent = tmp.Content,
        //                        State = tmp.State
        //                    };

        //                    returnVehicle.ReviewList.Add(customerReview);
        //                }
        //                return View("TradeInCustomer", returnVehicle);
        //            }



        //        }
        //        else
        //        {
        //            var httpCookie = Request.Cookies["Vindealer"];

        //            int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

        //            var context = new VincontrolEntities();

        //            var dealerDefault =
        //                context.Dealers.Include("Setting").First(
        //                    x => x.DealerId == dealerId);

        //            //var dealerDefaultSetting =
        //            //    context.Settings.First(x => x.DealerId == dealerId);

        //            var dealerInfo = new DealershipViewModel(dealerDefault)
        //            {
        //                DealershipId = dealerId
        //            };
        //            Session["TradeInDealer"] = dealerInfo;

        //            var returnVehicle = KellyBlueBookHelper.GetKBBVehicleFromVehicleId(vehicle.VehicleId, mileageNumber, dealerInfo.ZipCode);

        //            returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.SelectedTrim);


        //            if (returnVehicle.OptionalEquipment.Any())
        //            {
        //                return View("TradeInOptions", returnVehicle);
        //            }
        //            else
        //            {

        //                var comments =
        //                    context.TradeInComments.Where(
        //                        x => x.DealerId == dealerInfo.DealershipId);

        //                returnVehicle.ReviewList = new List<CustomerReview>();

        //                foreach (var tmp in comments)
        //                {
        //                    var customerReview = new CustomerReview()
        //                    {
        //                        City = tmp.City,
        //                        Name = tmp.Name,
        //                        ReviewContent = tmp.Content,
        //                        State = tmp.State
        //                    };

        //                    returnVehicle.ReviewList.Add(customerReview);
        //                }
        //                return View("TradeInCustomer", returnVehicle);
        //            }


        //        }


        //    }

        //    else
        //    {
        //        if (!String.IsNullOrEmpty(vehicle.Vin.Trim()))
        //        {
        //            string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

        //            int mileageNumber = 0; Int32.TryParse(mileageString, out mileageNumber);

        //            TradeInVehicleModel returnVehicle = null;

        //            if (Session["TradeInDealer"] != null)
        //            {

        //                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

        //                returnVehicle = KellyBlueBookHelper.GetKBBTrimsOrOptionsFromVin(vehicle.Vin.Trim(), mileageNumber, dealerSessionInfo.ZipCode);


        //            }
        //            else
        //            {
        //                var httpCookie = Request.Cookies["Vindealer"];

        //                int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

        //                var context = new VincontrolEntities();

        //                var dealerDefault =
        //                    context.Dealers.Include("Setting").First(
        //                        x => x.DealerId == dealerId);

        //                //var dealerDefaultSetting =
        //                //    context.Settings.First(x => x.DealerId == dealerId);


        //                var dealerInfo = new DealershipViewModel(dealerDefault)
        //                {
        //                    DealershipId = dealerId,
        //                };
        //                Session["TradeInDealer"] = dealerInfo;

        //                returnVehicle = KellyBlueBookHelper.GetKBBTrimsOrOptionsFromVin(vehicle.Vin.Trim(), mileageNumber, dealerInfo.ZipCode);

        //            }

        //            returnVehicle.Condition = vehicle.Condition;

        //            if (returnVehicle.ValidVin)
        //            {

        //                if (returnVehicle.SpecificKBBTrimList.Count > 1)
        //                {
        //                    foreach (var tmp in returnVehicle.SpecificKBBTrimList)
        //                    {
        //                        tmp.VIN = EncryptionHelper.EncryptString(tmp.Id.ToString(CultureInfo.InvariantCulture));

        //                    }

        //                    returnVehicle.Dealer = vehicle.Dealer;

        //                    return View("TradeInMultipleTrims", returnVehicle);
        //                }
        //                else
        //                {
        //                    returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(returnVehicle.SpecificKBBTrimList.First().Id.ToString(CultureInfo.InvariantCulture));

        //                    if (returnVehicle.OptionalEquipment.Any())

        //                        return View("TradeInOptions", returnVehicle);

        //                    return View("TradeInCustomer", returnVehicle);
        //                }
        //            }
        //            else
        //            {
        //                return RedirectToAction("TradeInVehicleInvalidVin");


        //            }
        //        }
        //        else
        //        {

        //            return RedirectToAction("TradeInVehicleInvalidVin");

        //        }
        //    }


        //}
        
        private string GetManheimModel(string selectedModelValue)
        {
            return selectedModelValue;
        }

        private string GetManheimMake(string selectedMakeValue)
        {
            selectedMakeValue = selectedMakeValue.ToLower().Replace("bmw", "b m w");
            selectedMakeValue = selectedMakeValue.ToLower().Replace("american motors (amc)", "american (amc)");
            return selectedMakeValue;
        }

        public ActionResult GetTradeInValueByKarPower(TradeInVehicleModel vehicle)
        {
            if (!ValidateCustomerSubmitForm(vehicle))
            {
                using (var context = new VincontrolEntities())
                {
                    var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                    var comments = context.TradeInComments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

                    vehicle.ReviewList = new List<CustomerReview>();

                    foreach (var tmp in comments)
                    {
                        var customerReview = new CustomerReview()
                        {
                            City = tmp.City,
                            Name = tmp.Name,
                            ReviewContent = tmp.Content,
                            State = tmp.State
                        };

                        vehicle.ReviewList.Add(customerReview);
                    }

                    return View("TradeInCustomerByKarPower", vehicle);
                }
            }

            try
            {
                string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

                int mileageNumber = 0; Int32.TryParse(mileageString, out mileageNumber);

                vehicle.EncryptVehicleId = vehicle.EncryptVehicleId.Replace(" ", "+");

                int id = Convert.ToInt32(EncryptionHelper.DecryptString(vehicle.EncryptVehicleId));

                var returnVehicle = new TradeInVehicleModel();

                DealershipViewModel dealerSessionInfo;
                var context = new VincontrolEntities();
                if (Session["TradeInDealer"] != null)
                {
                    dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                    //var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId);

                    //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);

                    //returnVehicle.DealerId = dealerSessionInfo.DealershipId;
                    //returnVehicle.DealerName = dealerSessionInfo.DealershipName;
                }
                else
                {
                    var httpCookie = Request.Cookies["Vindealer"];
                    int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));
                    var dealerDefault = context.Dealers.Include("Setting").First(x => x.DealerId == dealerId);
                    //var dealerDefaultSetting = context.Settings.First(x => x.DealerId == dealerId);

                    dealerSessionInfo = new DealershipViewModel(dealerDefault)
                     {
                         DealershipId = dealerId
                     };

                    Session["TradeInDealer"] = dealerSessionInfo;

                    //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);

                    //returnVehicle.DealerId = dealerSessionInfo.DealershipId;
                    //returnVehicle.DealerName = dealerSessionInfo.DealershipName;
                }

                var makes = (List<ExtendedSelectListItem>)Session["KarPowerMakes"];
                var models = (List<ExtendedSelectListItem>)Session["KarPowerModels"];
                var trims = (List<ExtendedSelectListItem>)Session["KarPowerTrims"];
                _karPowerService.ExecuteGetDefaultOptionalEquipmentWithUser(Convert.ToInt32(vehicle.VehicleId), DateTime.Now);
                if (!String.IsNullOrEmpty(_karPowerService.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.GetDefaultOptionalEquipmentWithUserResult);
                    var optionalEquipmentMarkupList = _karPowerService.CreateOptionalEquipmentList(_karPowerService.ConvertToString((JValue)(jsonObj["d"])));
                    if (optionalEquipmentMarkupList.Count > 0 && !String.IsNullOrWhiteSpace(vehicle.SelectedOptions))
                    {
                        foreach (var optionContract in optionalEquipmentMarkupList)
                        {
                            optionContract.IsSelected = vehicle.SelectedOptions.Contains(optionContract.Id);
                        }
                    }

                    int receivedCondition = vehicle.Condition.ToLower().Equals("great") ? 2 : vehicle.Condition.ToLower().Equals("fair") ? 3 : 4;
                    _karPowerService.ExecuteGetValuation(Convert.ToInt32(vehicle.VehicleId), DateTime.Now, mileageNumber, receivedCondition, 0, new OptionContract[] { }, optionalEquipmentMarkupList.ToArray());
                    decimal baseWholesale = 0;
                    decimal mileageAdjustment = 0;
                    decimal wholesale = 0;
                    if (!String.IsNullOrEmpty(_karPowerService.GetValuationResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.GetValuationResult);

                        decimal.TryParse(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value), out baseWholesale);
                        decimal.TryParse(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value), out mileageAdjustment);
                           decimal.TryParse(Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value),out wholesale);
                    }

                    var matchingMake = makes.FirstOrDefault(i => i.Value == vehicle.SelectedMake);
                    var matchingModel = models.FirstOrDefault(i => i.Value == vehicle.SelectedModel);
                    var matchingTrim = trims.FirstOrDefault(i => i.Value == vehicle.SelectedTrim);
                    returnVehicle = new TradeInVehicleModel()
                                        {
                                            EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.VehicleId.ToString()),
                                            SelectedYear = vehicle.SelectedYear,
                                            SelectedMake = matchingMake != null ? matchingMake.Text : vehicle.SelectedMake,
                                            SelectedModel = matchingModel != null ? matchingModel.Text : vehicle.SelectedModel,
                                            SelectedTrim = matchingTrim != null ? matchingTrim.Text : vehicle.SelectedTrim,
                                            VehicleId = vehicle.VehicleId,
                                            OptionalEquipment =
                                                optionalEquipmentMarkupList.Select(i => new ExtendedEquipmentOption()
                                                                                            {
                                                                                                VehicleOptionId =
                                                                                                    Convert.ToInt32(i.Id),
                                                                                                DisplayName =
                                                                                                    i.DisplayName,
                                                                                                DisplayNameAdditionalData
                                                                                                    = i.DisplayName,
                                                                                                IsSelected =
                                                                                                    i.IsSelected
                                                                                            }).ToList(),
                                            TradeInFairPrice = baseWholesale,
                                            TradeInGoodPrice = wholesale,
                                            Mileage = mileageString,
                                        };
                }

                returnVehicle.Vin = vehicle.Vin;

                returnVehicle.Condition = CommonHelper.UppercaseWords(vehicle.Condition);

                returnVehicle.VehicleId = id;

                returnVehicle.MileageNumber = mileageNumber;

                returnVehicle.CustomerEmail = vehicle.CustomerEmail;

                returnVehicle.CustomerPhone = vehicle.CustomerPhone;

                returnVehicle.CustomerFirstName = CommonHelper.UppercaseWords(vehicle.CustomerFirstName.ToLower());

                returnVehicle.CustomerLastName = CommonHelper.UppercaseWords(vehicle.CustomerLastName.ToLower());

                returnVehicle.Dealer = EncryptionHelper.EncryptString(dealerSessionInfo.DealershipId.ToString(CultureInfo.InvariantCulture));
                returnVehicle.SelectedOptions = vehicle.SelectedOptions;

                var tradeinEmail = SendEmail(vehicle, returnVehicle, dealerSessionInfo);
                returnVehicle.EmailTextContent = tradeinEmail.TextContent;
                returnVehicle.EmailADFContent = tradeinEmail.AdfContent;
                returnVehicle.Receivers = tradeinEmail.Receivers;



                if (String.IsNullOrEmpty(vehicle.Vin))
                {
                    returnVehicle.CarFax = new CarFaxViewModel()
                    {
                        Success = false,
                    };
                }
                else
                {
                    returnVehicle.CarFax = new CarFaxViewModel();

                    var carFaxModel = _carFaxService.ConvertXmlToCarFaxModelAndSave(vehicle.Vin, dealerSessionInfo.DealerSetting.CarFax, dealerSessionInfo.DealerSetting.CarFaxPassword);

                    returnVehicle.CarFax = carFaxModel;
                }

                return View("TradeInValueByKarPower", returnVehicle);
            }
            catch (Exception)
            {
                //throw;
                var httpCookie = Request.Cookies["dealer"];
                if (httpCookie != null)
                    return RedirectToAction("TradeInVehicleByKarPower", new { dealer = httpCookie.Value });

                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                return RedirectToAction("TradeInVehicleByKarPower", new { dealer = dealerSessionInfo.EncryptDealerId });
            }
        }

        private TradeinEmail SendEmail(TradeInVehicleModel vehicle, TradeInVehicleModel returnVehicle, DealershipViewModel dealerSessionInfo)
        {
            var tradeinEmail = new TradeinEmail()
            {
                Receivers = String.IsNullOrEmpty(dealerSessionInfo.Email) ? new List<string>() : dealerSessionInfo.Email.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList()
            };

            var bodyEmail = EmailHelper.CreateBannerBodyHtmlEmail(returnVehicle, dealerSessionInfo);
            var bodyPdf = EmailHelper.CreateBannerBodyForPdf(returnVehicle, dealerSessionInfo);

            EmailHelper.SendEmailForTradeInBanner(new MailAddress(vehicle.CustomerEmail),
                                               "Your Trade In Value For " + returnVehicle.SelectedYear + " " +
                                               returnVehicle.SelectedMakeValue + " " + returnVehicle.SelectedModelValue + " " +
                                               returnVehicle.SelectedTrimValue, bodyEmail, bodyPdf);

            switch (dealerSessionInfo.DealerSetting.EmailFormat)
            {
                //case 0:
                //    tradeinEmail.TextContent = SendTextContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                //    break;
                //case 1:
                //    tradeinEmail.AdfContent = SendAdfContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                //    break;
                //case 2:
                //    tradeinEmail.TextContent = SendTextContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                //    tradeinEmail.AdfContent = SendAdfContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                //    break;
                default:
                    break;
            }

            return tradeinEmail;

        }
        
        public ActionResult TradeInCustomerEmail(int tradeInId)
        {
            var context = new VincontrolEntities();

            var tradeIn =
                context.TradeInCustomers.First(x => x.TradeInCustomerId == tradeInId);

            var returnVehicle = new TradeInVehicleModel()
                                    {
                                        SelectedYear = tradeIn.Year.ToString(),
                                        SelectedMake = tradeIn.Make,
                                        SelectedModel = tradeIn.Model,
                                        SelectedTrim = tradeIn.Trim,
                                        Mileage = tradeIn.Mileage.ToString(),
                                        Condition = tradeIn.Condition,
                                        TradeInFairPrice = tradeIn.TradeInFairValue,
                                        TradeInGoodPrice = tradeIn.TradeInMaxValue

                                    };


            return View("TradeInHtmlEmail", returnVehicle);
        }

        //public JsonResult YearAjaxPost(int yearId)
        //{
        //    return new DataContractJsonResult(/*KellyBlueBookHelper.GetKBBMakesByYear(yearId)*/);
        //}

        //public JsonResult MakeAjaxPost(int yearId, int makeId)
        //{
        //    return new DataContractJsonResult(/*KellyBlueBookHelper.GetKBBModelByMakeId(yearId, makeId)*/);
        //}

        //public JsonResult ModelAjaxPost(int yearId, int modelId)
        //{
        //    return new DataContractJsonResult(/*KellyBlueBookHelper.GetKBBTrimByModelId(yearId, modelId)*/);
        //}

        public ActionResult GetYears()
        {
            var viewModel = new TradeInVehicleModel() { SelectedYear = "0", YearsList = new List<ExtendedSelectListItem>() };
            try
            {
                viewModel.YearsList = GetYearData(viewModel);
                Session["KarPowerYears"] = viewModel.YearsList;
            }
            catch
            {

            }
            return PartialView("Years", viewModel);
        }

        private static IEnumerable<ExtendedSelectListItem> GetYearData(TradeInVehicleModel viewModel)
        {
            var autoService = new ChromeAutoService();
            var years = autoService.GetModelYears();

            var result = new List<ExtendedSelectListItem> { SelectListHelper.CreateSelectListItem("Year...", "Year...", true) };
            if (years != null)
            {
                result.AddRange(years.Select(item => new ExtendedSelectListItem
                                                         {
                                                             Text = item.ToString(CultureInfo.InvariantCulture),
                                                             Value = item.ToString(CultureInfo.InvariantCulture)
                                                         }));
            }

            return result;
        }


        public ActionResult GetMakes(int yearId)
        {
            var viewModel = new TradeInVehicleModel { SelectedMake = "0", MakesList = new List<ExtendedSelectListItem>() };
            try
            {
                var result = new List<ExtendedSelectListItem> { SelectListHelper.CreateSelectListItem("Make...", "Make...", true) };
                var context = new VincontrolEntities();
                var makeList =
                    context.YearMakes.Where(ym => ym.Year == yearId).Select(ym => new { ym.Make.Value, ym.MakeId }).ToList();

                result.AddRange(
                    makeList.Select(
                        ym =>
                        new ExtendedSelectListItem { Text = ym.Value, Value = ym.MakeId.ToString(CultureInfo.InvariantCulture) })
                        .ToList());
                viewModel.MakesList = result;
                Session["KarPowerMakes"] = viewModel.MakesList;
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
                var result = new List<ExtendedSelectListItem> { SelectListHelper.CreateSelectListItem("Model...", "Model...", true) };
                var context = new VincontrolEntities();
                var modelList = context.Models.Where(m => m.YearMake.MakeId == makeId && m.YearMake.Year == yearId).Select(m => new { m.Value, m.ModelId }).ToList();
                if (!modelList.Any())
                {
                    var yearmakeItem =
                        context.YearMakes.Where(m => m.MakeId == makeId)
                               .Select(i => new { i.MakeId, i.Make.ChromeId, i.YearMakeId })
                               .FirstOrDefault();
                    var autoService = new ChromeAutoService();
                    if (yearmakeItem != null)
                    {
                        var models = autoService.GetModelsByDivision(yearId, yearmakeItem.ChromeId.Value);
                        if (models != null)
                        {
                            var list = models.Select(model => new Model
                                {
                                    YearMakeId = yearmakeItem.YearMakeId,
                                    Value = model.Value,
                                    ChromeId = model.id
                                });
                            foreach (var newModel in list)
                            {
                                context.AddToModels(newModel);
                                context.SaveChanges();
                                result.Add(new ExtendedSelectListItem { Text = newModel.Value, Value = newModel.ModelId.ToString(CultureInfo.InvariantCulture) });
                            }
                        }
                    }
                }
                else
                {
                    result.AddRange(modelList.Select(ym => new ExtendedSelectListItem { Text = ym.Value, Value = ym.ModelId.ToString(CultureInfo.InvariantCulture) }).ToList());
                }

                viewModel.ModelsList = result;
                Session["KarPowerModels"] = viewModel.ModelsList;
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
                var result = new List<ExtendedSelectListItem> { SelectListHelper.CreateSelectListItem("Trim...", "Trim...", true) };
                var context = new VincontrolEntities();
                var modelList = context.Trims.Where(t => t.ModelId == modelId).Select(m => new { m.TrimName, m.ModelId }).ToList();
                result.AddRange(modelList.Select(ym => new ExtendedSelectListItem { Text = ym.TrimName, Value = ym.ModelId.ToString(CultureInfo.InvariantCulture) }).ToList());
                viewModel.TrimsList = result;
                Session["KarPowerTrims"] = viewModel.TrimsList;
            }
            catch (Exception)
            {

            }
            return PartialView("Trims", viewModel);
        }
        
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
            //else
            //{
            //    string clientEmail = customer.CustomerEmail.Trim();

            //    if(!IsEmailSyntaxValid(clientEmail))
            //        ModelState.AddModelError("CustomerEmail", "Valid Email is Required");
            //}



            return ModelState.IsValid;
        }

        private bool IsEmailSyntaxValid(string emailToValidate)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(emailToValidate,
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        private string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s = s.ToLower();
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        #region Admin Section

        public ActionResult InputComment()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                return View("TradeInCommentAdmin", GetComments());
            }
        }

        private IQueryable<TradeinCommentViewModel> GetComments()
        {

            var context = new VincontrolEntities();
            var dealerSessionInfo = SessionHandler.Dealer;

            var comments =
                context.TradeInComments.Where(
                    x => x.DealerId == dealerSessionInfo.DealershipId).Select(e => new TradeinCommentViewModel()
                    {
                        City = e.City,
                        Content = e.Content,
                        State = e.State,
                        ID = e.TradeInCommentId,
                        Name = e.Name
                    });
            return comments;
        }

        public ActionResult AddComment(string city, string state, string comment, string name)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();

                var dealerSessionInfo = SessionHandler.Dealer;

                context.AddToTradeInComments(new TradeInComment()
                {
                    DealerId = dealerSessionInfo.DealershipId,
                    City = city,
                    State = state,
                    Content = comment,
                    CommentDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Name = name
                });

                context.SaveChanges();
                return PartialView("TradeInComment", GetComments());
            }
        }

        public ActionResult SaveComment(int id, string city, string state, string comment, string name)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();
                var dealerSessionInfo = SessionHandler.Dealer;
                var customer = context.TradeInComments.FirstOrDefault(e => e.DealerId == dealerSessionInfo.DealershipId && e.TradeInCommentId == id);
                customer.City = city;
                customer.State = state;
                customer.Content = comment;
                customer.Name = name;
                context.SaveChanges();
                return PartialView("TradeInComment", GetComments());
            }
        }

        public ActionResult DeleteComment(int id)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();
                var dealerSessionInfo = SessionHandler.Dealer;
                var customer = context.TradeInComments.FirstOrDefault(e => e.DealerId == dealerSessionInfo.DealershipId && e.TradeInCommentId == id);
                if (customer != null)
                {
                    context.DeleteObject(customer);
                    context.SaveChanges();
                }
                return PartialView("TradeInComment", GetComments());
            }
        }

        private ActionResult Create()
        {
            return View("TradeInCommentAdminCreate");
        }

        public ActionResult ReportOptions()
        {
            return View("ReportOptions");
        }

        public ActionResult InputVariance(int value)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();
                var dealerSessionInfo = SessionHandler.Dealer;

                var customer = context.TradeInCustomers.FirstOrDefault(e => e.DealerId == dealerSessionInfo.DealershipId);
                customer.VarianceValue = 15;

                return PartialView("Report", customer.VarianceValue);
            }
        }

        [VinControlAuthorization(PermissionCode = "APPRAISAL", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult Report(int? type)
        {
            if (!SessionHandler.UserRight.Appraisals.Advisor)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                return View("Report");
            }
        }

        public ActionResult LoadReportList(int? type, string fromDate, string toDate)
        {
            var customers = GetTradeInCustomers("Date", "des", type, fromDate, toDate);
            var result = new AdvisorListViewModel();
            result.AdvisorList = customers.ToList();

            return PartialView("~/Views/Appraisal/AdvisorListData.ascx", result);
        }

        public ActionResult GetPartialTradeinList(string sort, string sortOrder)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var customers = GetTradeInCustomers(sort, sortOrder, null);

                return PartialView("AppraisalList", customers);
            }
        }

        private IOrderedEnumerable<TradeinCustomerViewModel> SortCustomer(IEnumerable<TradeinCustomerViewModel> list, string sortedField, string sortOrder)
        {
            switch (sortedField)
            {
                case "Year":
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.Year) : list.OrderByDescending(e => e.Year);
                case "Make":
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.Make) : list.OrderByDescending(e => e.Make);
                case "Model":
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.Model) : list.OrderByDescending(e => e.Model);
                case "Condition":
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.Condition) : list.OrderByDescending(e => e.Condition);
                case "Status":
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.TradeInStatus) : list.OrderByDescending(e => e.TradeInStatus);
                case "Price":
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.TradeInFairValue) : list.OrderByDescending(e => e.TradeInFairValue);
                case "Date":
                default:
                    return sortOrder.Equals("asc") ? list.OrderBy(e => e.CreatedDate) : list.OrderByDescending(e => e.CreatedDate);
            }
        }

        private DateTime ParseDateTimeFromString(string szDateTime, bool bIsFromDate)
        {
            DateTime result = DateTime.Today;

            if (!string.IsNullOrEmpty(szDateTime))
            {
                string[] arrDate = szDateTime.Split('/');
                int nDay = 1;
                int nMonth = 1;
                int nYear = 2000;

                if (arrDate.Count() > 2)
                {
                    nMonth = Convert.ToInt32(arrDate[0]);
                    nDay = Convert.ToInt32(arrDate[1]);
                    nYear = Convert.ToInt32(arrDate[2]);
                }

                if (bIsFromDate)
                {
                    result = new DateTime(nYear, nMonth, nDay, 0, 0, 0);
                }
                else
                {
                    result = new DateTime(nYear, nMonth, nDay, 23, 59, 59);
                }
            }

            return result;
        }

        private IOrderedEnumerable<TradeinCustomerViewModel> GetTradeInCustomers(string sortField, string sortOrder, int? type, string fromDate = "", string toDate = "")
        {
            var context = new VincontrolEntities();
            var dealerSessionInfo = SessionHandler.Dealer;

            DateTime dtFromDate = ParseDateTimeFromString(fromDate, true);
            DateTime dtToDate = ParseDateTimeFromString(toDate, false);

            //DateTime dateToCompare;
            if (type == null)
            {
                dtFromDate = DateTime.Now.AddDays(-120); 
                dtToDate =  DateTime.Now;
            }

            var customers = InventoryQueryHelper.GetSingleOrGroupTradein(context).Where(
                e =>
                e.DateStamp.HasValue && 
                //e.DealerId == dealerSessionInfo.DealershipId &&
                e.DateStamp.Value >= dtFromDate && e.DateStamp.Value <= dtToDate &&
                ((e.TradeInStatus == null) || !e.TradeInStatus.Equals("Deleted"))).ToList()
                .Select(e => new TradeinCustomerViewModel()
                                 {
                                     Condition = e.Condition,
                                     Date =
                                         e.DateStamp.HasValue ? e.DateStamp.Value.ToShortDateString() : String.Empty,
                                     CreatedDate = e.DateStamp,
                                     Email = e.Email,
                                     FirstName = e.FirstName,
                                     LastName = e.LastName,
                                     Make = e.Make,
                                     ShortMake = !String.IsNullOrEmpty(e.Make) && e.Make.Length > 10
                                             ? e.Make.Substring(0, 9)
                                             : e.Make,
                                     MileageAdjustment = e.Mileage.HasValue ? e.Mileage.Value.ToString() : string.Empty,
                                     Model = e.Model,
                                     Phone = e.Phone,
                                     TradeInStatus = e.TradeInStatus,
                                     Year = e.Year.HasValue ? e.Year.Value.ToString() : String.Empty,
                                     ID = e.TradeInCustomerId,
                                     TradeInFairValue = e.TradeInFairValue ?? 0,
                                     //EmailContent = e.EmailContent,
                                     SortVin =
                                         !String.IsNullOrEmpty(e.Vin) && e.Vin.Length > 6
                                             ? e.Vin.Substring(0, 7)
                                             : e.Vin,
                                     StrTrim =
                                         !String.IsNullOrEmpty(e.Trim) && e.Trim.Length > 7
                                             ? e.Trim.Substring(0, 8)
                                             : e.Trim,
                                     StrExteriorColor = string.Empty,

                                     StrCarFaxOwner = "Unknow",
                                     StrClientName = string.Format("{0} {1}", e.FirstName, e.LastName),
                                     ClientContact = e.Email,
                                     Age = e.DateStamp.HasValue ? DateTime.Now.Subtract(e.DateStamp.Value).Days : 0
                                 });
            if (type == 4)
                customers = customers.Where(x => x.TradeInStatus == Constanst.TradeInStatus.Done);
            return SortCustomer(customers, sortField, sortOrder);
        }

        public ActionResult SaveTradeInStatus(string status, int id)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();
                var dealerSessionInfo = SessionHandler.Dealer;

                var customer = context.TradeInCustomers.FirstOrDefault(e => e.DealerId == dealerSessionInfo.DealershipId && e.TradeInCustomerId == id);
                if (customer != null)
                {
                    customer.TradeInStatus = status;
                    context.SaveChanges();
                }
                return Json(new {success = true});
            }
        }

        //public ActionResult GetVarianceCost()
        //{
        //    if (SessionHandler.Dealer == null)
        //    {
        //        return RedirectToAction("LogOff", "Account");
        //    }
        //    else
        //    {
        //        var context = new VincontrolEntities();
        //        var dealerSessionInfo = SessionHandler.Dealer;
        //        var dealer = context.Settings.FirstOrDefault(e => e.DealerId == dealerSessionInfo.DealershipId);
        //        return View("VarianceCost", new VariantCodeViewModel() { Variance = (dealer.PriceVariance.HasValue ? dealer.PriceVariance.Value : 0) });
        //    }
        //}

        //public ActionResult SaveVarianceCost(string cost)
        //{
        //    if (SessionHandler.Dealer == null)
        //    {
        //        return RedirectToAction("LogOff", "Account");
        //    }
        //    else
        //    {
        //        var context = new VincontrolEntities();
        //        var dealerSessionInfo = SessionHandler.Dealer;

        //        var dealer = context.Settings.FirstOrDefault(e => e.DealerId == dealerSessionInfo.DealershipId);
        //        if (dealer != null)
        //        {
        //            decimal result;
        //            dealer.PriceVariance = decimal.TryParse(cost, out result) ? result : 0;
        //            context.SaveChanges();
        //        }

        //        return View("VarianceCost", new VariantCodeViewModel() { Variance = (dealer.PriceVariance.HasValue ? dealer.PriceVariance.Value : 0) });

        //    }
        //}

        public ActionResult GetEmailContent(int id)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();
                var dealerSessionInfo = SessionHandler.Dealer;

                var customer = context.TradeInCustomers.FirstOrDefault(e => e.TradeInCustomerId == id);
                if (customer != null)
                {
                    //return View("EmailContent", new EmailContentViewModel()
                    //    {
                    //        //TextContent = customer.EmailContent
                    //        //ADFContent = customer.ADFEmailContent,
                    //        //Receivers = customer.Receivers
                    //    });
                    return View("EmailContent", new EmailContentViewModel());
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult InputVIN(int id)
        {
            return View("VINInput", new CustomerModel() { ID = id });
        }

        public ActionResult SuccesfulMessage(string content)
        {
            return View("SuccesfulMessage", new SuccesfulMessageViewModel() { UserEmail = content });
        }

        public ActionResult ResendEmailContent(int id,string acv)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new VincontrolEntities();
                var dealerSessionInfo = SessionHandler.Dealer;

                var customer = context.TradeInCustomers.FirstOrDefault(e => e.TradeInCustomerId == id);
                if (customer != null)
                {
                    var returnVehicle = DataHelper.GetTradeinVehicle(id);
                    var bodyEmail = EmailHelper.CreateBannerBodyHtmlEmail(returnVehicle, dealerSessionInfo);
                    var bodyPDF = EmailHelper.CreateBannerBodyForPdfNew(returnVehicle, dealerSessionInfo, acv);
                    EmailHelper.SendEmailForTradeInBanner(new MailAddress(returnVehicle.CustomerEmail),
                                              "Your Trade In Value For " + returnVehicle.SelectedYear + " " +
                                              returnVehicle.SelectedMake + " " + returnVehicle.SelectedModel + " " +
                                              returnVehicle.SelectedTrim, bodyEmail, bodyPDF);
                    return Content(returnVehicle.CustomerEmail);
                }

                return View();

            }
        }

        public ActionResult SaveCost(int id, decimal cost)
        {
            var context = new VincontrolEntities();

            var customer = context.TradeInCustomers.FirstOrDefault(e => e.TradeInCustomerId == id);
            if (customer != null)
            {
                customer.TradeInFairValue = cost;
                context.SaveChanges();
            }
            return Json("UPDATED");
        }

        public ActionResult SaveMileage(int id, string mileage)
        {
            var context = new VincontrolEntities();

            var customer = context.TradeInCustomers.FirstOrDefault(e => e.TradeInCustomerId == id);
            if (customer != null)
            {
                customer.Mileage = CommonHelper.RemoveSpecialCharactersAndReturnNumber(mileage);
                context.SaveChanges();
            }
            return Json("UPDATED");
        }

        #endregion
    }
}
