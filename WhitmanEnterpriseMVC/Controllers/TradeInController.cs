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
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using WhitmanEnterpriseMVC.Security;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class TradeInController : Controller
    {
        private KarPowerService _karPowerService;

        public TradeInController()
        {
            _karPowerService = new KarPowerService();
        }

        public ActionResult TradeInByTrim(string vId, string mileage, string condition)
        {
            if (!String.IsNullOrEmpty(vId) && !String.IsNullOrEmpty(condition) && !String.IsNullOrEmpty(mileage))
            {
                try
                {

                    vId = vId.Replace(" ", "+");

                    string id = EncryptionHelper.DecryptString(vId);

                    if (Session["TradeInDealer"] != null)
                    {

                        var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];


                        var returnVehicle = KellyBlueBookHelper.GetKBBVehicleFromVehicleId(Convert.ToInt32(id),
                                                                                           Convert.ToInt32(mileage),
                                                                                           dealerSessionInfo.ZipCode);

                        returnVehicle.Condition = condition;

                        returnVehicle.EncryptVehicleId = vId;

                        return View("TradeInOptions", returnVehicle);
                    }
                    else
                    {
                        var httpCookie = Request.Cookies["Vindealer"];

                        int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

                        var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];


                        var context = new whitmanenterprisewarehouseEntities();

                        var dealerDefault =
                            context.whitmanenterprisedealerships.First(
                                x => x.idWhitmanenterpriseDealership == dealerId);

                        var dealerDefaultSetting =
                            context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);


                        var dealerInfo = new DealershipViewModel()
                                             {
                                                 DealershipId = dealerId,
                                                 DealershipName = dealerDefault.DealershipName,
                                                 DealershipAddress = dealerDefault.DealershipAddress,
                                                 Address = dealerDefault.Address,
                                                 City = dealerDefault.City,
                                                 State = dealerDefault.State,
                                                 ZipCode = dealerDefault.ZipCode,
                                                 Phone = dealerDefault.Phone,
                                                 Email = dealerDefault.Email,
                                                 CarFax = dealerDefaultSetting.CarFax,
                                                 CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                                                 EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                                                 PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault(),
                                                 KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                                                 KellyPassword = dealerDefaultSetting.KellyPassword,
                                                 Manheim = dealerDefaultSetting.Manheim,
                                                 ManheimPassword = dealerDefaultSetting.ManheimPassword

                                             };
                        Session["TradeInDealer"] = dealerInfo;

                        var returnVehicle = KellyBlueBookHelper.GetKBBVehicleFromVehicleId(Convert.ToInt32(id),
                                                                                           Convert.ToInt32(mileage),
                                                                                           dealerSessionInfo.ZipCode);

                        returnVehicle.Condition = condition;

                        returnVehicle.EncryptVehicleId = vId;

                        return View("TradeInOptions", returnVehicle);

                    }
                }
                catch (Exception)
                {
                    var httpCookie = Request.Cookies["Vindealer"];
                    if (httpCookie != null)
                        return RedirectToAction("TradeInVehicle", new { dealer = httpCookie.Value });
                    var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                    return Redirect(dealerSessionInfo.WebSiteUrl);
                }
            }
            else
            {
                var httpCookie = Request.Cookies["Vindealer"];

                if (httpCookie != null)
                    return RedirectToAction("TradeInVehicle", new { dealer = httpCookie.Value });
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                return RedirectToAction("TradeInVehicle", new { dealer = dealerSessionInfo.EncryptDealerId });
            }
        }

        public ActionResult TradeInCustomer(TradeInVehicleModel vehicle)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

            var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

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
            var context = new whitmanenterprisewarehouseEntities();

            var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

            var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

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
                MakesList = new List<SelectListItem>().AsEnumerable(),
                ModelsList = new List<SelectListItem>().AsEnumerable(),
                TrimsList = new List<SelectListItem>().AsEnumerable(),
                IsValidDealer = true,
                ValidVin = true,
                Dealer = dealer,

            };
            Session["TradeInDealer"] = null;

            if (Session["TradeInDealer"] == null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                    var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                    var dealerInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                        EncryptDealerId = dealer,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                        var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                        var dealerInfo = new DealershipViewModel()
                        {
                            DealershipId = dealerId,
                            DealershipName = dealerDefault.DealershipName,
                            DealershipAddress = dealerDefault.DealershipAddress,
                            Address = dealerDefault.Address,
                            City = dealerDefault.City,
                            State = dealerDefault.State,
                            ZipCode = dealerDefault.ZipCode,
                            Phone = dealerDefault.Phone,
                            Email = dealerDefault.Email,
                            CarFax = dealerDefaultSetting.CarFax,
                            CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                            WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                            EncryptDealerId = dealer,
                            EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                            KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                            KellyPassword = dealerDefaultSetting.KellyPassword,
                            Manheim = dealerDefaultSetting.Manheim,
                            ManheimPassword = dealerDefaultSetting.ManheimPassword,
                            PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
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
                MakesList = new List<SelectListItem>().AsEnumerable(),
                ModelsList = new List<SelectListItem>().AsEnumerable(),
                TrimsList = new List<SelectListItem>().AsEnumerable(),

                ValidVin = true,
                Dealer = dealer
            };

            //dealer = dealer.Replace(" ", "+");
            //int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(dealer));
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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var existingDealerGroup = context.whitmanenterprisedealergroups.FirstOrDefault(i => i.DealerGroupName.ToLower().Equals(dealer.ToLower()));
                if (existingDealerGroup != null)
                {
                    dealerId = existingDealerGroup.DefaultDealerID.GetValueOrDefault();
                }
                else
                {
                    var existingDealer = context.whitmanenterprisedealerships.FirstOrDefault(i => i.DealershipName.Replace(" ", String.Empty).ToLower().Equals(dealer.ToLower()));
                    if (existingDealer != null)
                        dealerId = existingDealer.idWhitmanenterpriseDealership;
                }
            }

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<SelectListItem>().AsEnumerable(),
                ModelsList = new List<SelectListItem>().AsEnumerable(),
                TrimsList = new List<SelectListItem>().AsEnumerable(),

                ValidVin = true,
                Dealer = dealer,
              
            };
            Session["TradeInDealer"] = null;
            if (Session["TradeInDealer"] == null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                    var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                    var dealerInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                        EncryptDealerId = dealer,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                        var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                        var dealerInfo = new DealershipViewModel()
                        {
                            DealershipId = dealerId,
                            DealershipName = dealerDefault.DealershipName,
                            DealershipAddress = dealerDefault.DealershipAddress,
                            Address = dealerDefault.Address,
                            City = dealerDefault.City,
                            State = dealerDefault.State,
                            ZipCode = dealerDefault.ZipCode,
                            Phone = dealerDefault.Phone,
                            Email = dealerDefault.Email,
                            CarFax = dealerDefaultSetting.CarFax,
                            CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                            WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                            EncryptDealerId = dealer,
                            EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                            KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                            KellyPassword = dealerDefaultSetting.KellyPassword,
                            Manheim = dealerDefaultSetting.Manheim,
                            ManheimPassword = dealerDefaultSetting.ManheimPassword,
                            PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
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
                MakesList = new List<SelectListItem>().AsEnumerable(),
                ModelsList = new List<SelectListItem>().AsEnumerable(),
                TrimsList = new List<SelectListItem>().AsEnumerable(),
                IsValidDealer = true,
                ValidVin = true,
                Dealer = dealer,
               
            };
            Session["TradeInDealer"] = null;
            
            if (Session["TradeInDealer"] == null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                    var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                    var dealerInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                        EncryptDealerId = dealer,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                        var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                        var dealerInfo = new DealershipViewModel()
                        {
                            DealershipId = dealerId,
                            DealershipName = dealerDefault.DealershipName,
                            DealershipAddress = dealerDefault.DealershipAddress,
                            Address = dealerDefault.Address,
                            City = dealerDefault.City,
                            State = dealerDefault.State,
                            ZipCode = dealerDefault.ZipCode,
                            Phone = dealerDefault.Phone,
                            Email = dealerDefault.Email,
                            CarFax = dealerDefaultSetting.CarFax,
                            CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                            WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                            EncryptDealerId = dealer,
                            EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                            KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                            KellyPassword = dealerDefaultSetting.KellyPassword,
                            Manheim = dealerDefaultSetting.Manheim,
                            ManheimPassword = dealerDefaultSetting.ManheimPassword,
                            PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
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
            vehicle.YearsList = (IEnumerable<SelectListItem>)Session["KarPowerYears"] ?? new List<SelectListItem>();
            vehicle.MakesList = (IEnumerable<SelectListItem>)Session["KarPowerMakes"] ?? new List<SelectListItem>();
            vehicle.ModelsList = (IEnumerable<SelectListItem>)Session["KarPowerModels"] ?? new List<SelectListItem>();
            vehicle.TrimsList = (IEnumerable<SelectListItem>)Session["KarPowerTrims"] ?? new List<SelectListItem>();
            vehicle.IsValidDealer = true;

            KeepValueForPreviousPage(vehicle);

            //return View("TradeVehicleByKarPower", vehicle);
            return RedirectToAction("Index", new {dealer = vehicle.DealerName.ToLower().Replace(" ", string.Empty)});
        }

        public ActionResult TradeInVehicleWithVinByKarPower(string dealer, string vin)
        {
            vin = string.Empty;
            dealer = dealer.Replace(" ", "+");
            int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(dealer));

            var viewModel = new TradeInVehicleModel()
            {
                YearsList = SelectListHelper.InitialYearListFromKBB(),
                MakesList = new List<SelectListItem>().AsEnumerable(),
                ModelsList = new List<SelectListItem>().AsEnumerable(),
                TrimsList = new List<SelectListItem>().AsEnumerable(),

                ValidVin = true,
                Dealer = dealer,
                Vin = vin
            };
            Session["TradeInDealer"] = null;
            if (Session["TradeInDealer"] == null)
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                    var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                    var dealerInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                        EncryptDealerId = dealer,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                    };

                    Session["TradeInDealer"] = dealerInfo;
                }
            }
            else
            {
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                if (dealerSessionInfo.DealershipId != dealerId)
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                        var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                        var dealerInfo = new DealershipViewModel()
                        {
                            DealershipId = dealerId,
                            DealershipName = dealerDefault.DealershipName,
                            DealershipAddress = dealerDefault.DealershipAddress,
                            Address = dealerDefault.Address,
                            City = dealerDefault.City,
                            State = dealerDefault.State,
                            ZipCode = dealerDefault.ZipCode,
                            Phone = dealerDefault.Phone,
                            Email = dealerDefault.Email,
                            CarFax = dealerDefaultSetting.CarFax,
                            CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                            WebSiteUrl = dealerDefaultSetting.WebSiteURL,
                            EncryptDealerId = dealer,
                            EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                            KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                            KellyPassword = dealerDefaultSetting.KellyPassword,
                            Manheim = dealerDefaultSetting.Manheim,
                            ManheimPassword = dealerDefaultSetting.ManheimPassword,
                            PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var existingDealerGroup = context.whitmanenterprisedealergroups.FirstOrDefault(i => i.DealerGroupName.ToLower().Equals(dealer.ToLower()));
                if (existingDealerGroup != null)
                {
                    dealerId = existingDealerGroup.DefaultDealerID.GetValueOrDefault();
                    isValidDealer = true;
                }
                else
                { 
                    var existingDealer = context.whitmanenterprisedealerships.FirstOrDefault(i => i.DealershipName.Replace(" ", String.Empty).Replace("-", String.Empty).ToLower().Equals(dealer.ToLower()));
                    if (existingDealer != null)
                    { 
                        dealerId = existingDealer.idWhitmanenterpriseDealership;
                        isValidDealer = true;
                    }
                }
            }

            var viewModel = new TradeInVehicleModel()
                                {
                                    YearsList = SelectListHelper.InitialYearListFromKBB(),
                                    MakesList = new List<SelectListItem>().AsEnumerable(),
                                    ModelsList = new List<SelectListItem>().AsEnumerable(),
                                    TrimsList = new List<SelectListItem>().AsEnumerable(),
                                    IsValidDealer = isValidDealer,
                                    ValidVin = true,
                                    Dealer = dealerId.ToString(CultureInfo.InvariantCulture),
                                    DealerName = dealer,
                                    Vin = Session["PreviousVin"] != null ? (string) Session["PreviousVin"] : string.Empty,
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

                MakesList = new List<SelectListItem>().AsEnumerable(),

                ModelsList = new List<SelectListItem>().AsEnumerable(),

                TrimsList = new List<SelectListItem>().AsEnumerable(),

                ValidVin = false,

            };


            return View("TradeVehicle", viewModel);

        }

        //NOTE: 02/22/2013 - David changed this medtho to use value from Mainheim
        public ActionResult TradeInWithOptions(TradeInVehicleModel vehicle)
        {
            var httpCookie = Request.Cookies["Vindealer"];
            int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));
            string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);
            int mileageNumber;Int32.TryParse(mileageString, out mileageNumber);

            var context = new whitmanenterprisewarehouseEntities();

            DealershipViewModel dealerInfo;
            if (Session["TradeInDealer"] != null)
            {
                dealerInfo = (DealershipViewModel)Session["TradeInDealer"];
            }
            else
            {
                var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                dealerInfo = new DealershipViewModel()
                {
                    DealershipId = dealerId,
                    DealershipName = dealerDefault.DealershipName,
                    DealershipAddress = dealerDefault.DealershipAddress,
                    Address = dealerDefault.Address,
                    City = dealerDefault.City,
                    State = dealerDefault.State,
                    ZipCode = dealerDefault.ZipCode,
                    Phone = dealerDefault.Phone,
                    Email = dealerDefault.Email,
                    CarFax = dealerDefaultSetting.CarFax,
                    CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                    EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                    KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                    KellyPassword = dealerDefaultSetting.KellyPassword,
                    Manheim = dealerDefaultSetting.Manheim,
                    ManheimPassword = dealerDefaultSetting.ManheimPassword,
                    PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                };

                Session["TradeInDealer"] = dealerInfo;
            }


            var returnVehicle = new TradeInVehicleModel()
            {
                EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.VehicleId.ToString()),
                Condition = vehicle.Condition,
                SelectedYear = vehicle.SelectedYear,
                SelectedMake = vehicle.SelectedMake,
                SelectedModel = vehicle.SelectedModel,
                SelectedTrim = vehicle.SelectedTrim,
                SelectedMakeValue = vehicle.SelectedMakeValue,
                SelectedModelValue = vehicle.SelectedModelValue,
                SelectedTrimValue = vehicle.SelectedTrimValue,
                VehicleId = vehicle.VehicleId,
                OptionalEquipment = new List<ExtendedEquipmentOption>(),
                Mileage = mileageString,
                SelectedOptions = vehicle.SelectedOptions,
                DealerName = vehicle.DealerName
            };

            //TODO: Get options from chrome
            try
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                if (!String.IsNullOrEmpty(vehicle.Vin))
                {
                    var autoService = new ChromeAutoService();
                    var vehicleInfo = autoService.GetVehicleInformationFromVin(vehicle.Vin);
                    returnVehicle.OptionalEquipment = GetOptionalEquipment(vehicleInfo).Select(i => new ExtendedEquipmentOption()
                                                                                           {
                                                                                               VehicleOptionId =
                                                                                                   Convert.ToInt32(i.Id),
                                                                                               DisplayName = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant())
                                                                                                  ,
                                                                                               DisplayNameAdditionalData
                                                                                                   = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
                                                                                               IsSelected = i.IsSelected
                                                                                           }).ToList();
                    returnVehicle.SelectedYear = vehicleInfo.modelYear.ToString();
                    returnVehicle.SelectedMakeValue = vehicleInfo.bestMakeName;
                    returnVehicle.SelectedModelValue = vehicleInfo.bestModelName;
                    returnVehicle.SelectedTrimValue = vehicleInfo.bestTrimName;
                }
                else
                {
                    int result;
                    if (int.TryParse(returnVehicle.SelectedTrim, out result))
                    {
                        var vinContext = new vincontrolwarehouseEntities();
                        //var id = (int)vinContext.trims.Where(t => t.Id == result).Select(t=>t.ChromeId).FirstOrDefault();
                        //var autoService = new AutoService();
                        //var styleInfo = autoService.getStyleInformationFromStyleId(new[] { id },
                        //                                                           null, null, null, null);
                        returnVehicle.OptionalEquipment =
                             vinContext.trimoptions.Where(o => o.TrimId == result).Select(i => new { i.option.Id, i.option.Value }).
                             ToList().Select(i => new OptionContract
                             {
                                 __type = "KBB.Karpower.WebServices.LightOption",
                                 Id = i.Id.ToString(),
                                 DisplayName = i.Value
                             }).Select(i => new ExtendedEquipmentOption()
                                                                     {
                                                                         VehicleOptionId =
                                                                             Convert.ToInt32(i.Id),
                                                                         DisplayName =
                                                                            regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
                                                                         DisplayNameAdditionalData
                                                                             = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
                                                                         IsSelected = i.IsSelected
                                                                     }).ToList();
                    }
                }
            }
            catch (Exception e)
            {

            }

            if (returnVehicle.OptionalEquipment.Any())
            {
              
                return View("TradeInOptions", returnVehicle);
            }
            else
            {
                returnVehicle.SkipStepTwo = true;
                var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerInfo.DealershipId);

                returnVehicle.ReviewList = new List<CustomerReview>();

                foreach (var customerReview in comments.Select(tmp => new CustomerReview() { City = tmp.City, Name = tmp.Name, ReviewContent = tmp.Content, State = tmp.State }))
                {
                    returnVehicle.ReviewList.Add(customerReview);
                }

                return View("TradeInCustomer", returnVehicle);
            }
        }
        
        //public ActionResult TradeInOptionsByKarPower(TradeInVehicleModel vehicle)
        //{
        //    var httpCookie = Request.Cookies["Vindealer"];
        //    int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));
        //    string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);
        //    int mileageNumber;
        //    Int32.TryParse(mileageString, out mileageNumber);

        //    var dealerInfo = new DealershipViewModel();
        //    if (Session["TradeInDealer"] != null)
        //    {

        //        dealerInfo = (DealershipViewModel)Session["TradeInDealer"];
        //    }
        //    else
        //    {
        //        using (var context = new whitmanenterprisewarehouseEntities())
        //        {
        //            var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
        //            var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

        //            dealerInfo = new DealershipViewModel()
        //            {
        //                DealershipId = dealerId,
        //                DealershipName = dealerDefault.DealershipName,
        //                DealershipAddress = dealerDefault.DealershipAddress,
        //                Address = dealerDefault.Address,
        //                City = dealerDefault.City,
        //                State = dealerDefault.State,
        //                ZipCode = dealerDefault.ZipCode,
        //                Phone = dealerDefault.Phone,
        //                Email = dealerDefault.Email,
        //                CarFax = dealerDefaultSetting.CarFax,
        //                CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
        //                EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
        //                KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
        //                KellyPassword = dealerDefaultSetting.KellyPassword,
        //                Manheim = dealerDefaultSetting.Manheim,
        //                ManheimPassword = dealerDefaultSetting.ManheimPassword,
        //                PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
        //            };

        //            Session["TradeInDealer"] = dealerInfo;
        //        }
        //    }

        //    if (!String.IsNullOrEmpty(vehicle.Vin))
        //    {
        //        using (var context = new whitmanenterprisewarehouseEntities())
        //        {
        //            _karPowerService.ExecuteGetVehicleByVin(vehicle.Vin, DateTime.Now, dealerInfo.KellyBlueBook, dealerInfo.KellyPassword);
        //            if (!String.IsNullOrEmpty(_karPowerService.GetVehicleByVinResult))
        //            {
        //                var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.GetVehicleByVinResult);

        //                var selectedYearId = _karPowerService.ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
        //                var selectedMakeId = _karPowerService.ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
        //                var selectedModelId = _karPowerService.ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
        //                var selectedTrimId = _karPowerService.ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));

        //                // get makes
        //                var makes = _karPowerService.CreateDataList(jsonObj["d"]["makes"], selectedMakeId);
        //                Session["KarPowerMakes"] = makes;

        //                // get models
        //                var models = _karPowerService.CreateDataList(jsonObj["d"]["models"], selectedModelId);
        //                Session["KarPowerModels"] = models;

        //                // get trims
        //                var trims = _karPowerService.CreateDataList(jsonObj["d"]["trims"], selectedTrimId);
        //                Session["KarPowerTrims"] = trims;

        //                var result = _karPowerService.ExecuteWithoutTrim(vehicle.Vin, mileageString, DateTime.Now);
        //                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
        //                var returnVehicle = new TradeInVehicleModel()
        //                {
        //                    EncryptVehicleId = EncryptionHelper.EncryptString(selectedTrimId.ToString()),
        //                    Condition = vehicle.Condition,
        //                    SelectedYear = selectedYearId.ToString(),
        //                    SelectedMake = makes.FirstOrDefault(i => i.Value == selectedMakeId.ToString()).Text,
        //                    SelectedModel = models.FirstOrDefault(i => i.Value == selectedModelId.ToString()).Text,
        //                    SelectedTrim = trims.FirstOrDefault(i => i.Value == selectedTrimId.ToString()).Text,
        //                    VehicleId = selectedTrimId,
        //                    OptionalEquipment = result.OptionalEquipmentMarkupList.Select(i => new ExtendedEquipmentOption()
        //                    {
        //                        VehicleOptionId = Convert.ToInt32(i.Id),
        //                        DisplayName = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
        //                        DisplayNameAdditionalData = regex.Replace(i.DisplayName, m => m.Value.ToLowerInvariant()),
        //                        IsSelected = i.IsSelected
        //                    }).ToList(),
        //                    Mileage = mileageString,
        //                    SelectedOptions = vehicle.SelectedOptions

        //                };

        //                if (returnVehicle.OptionalEquipment.Any())
        //                {
        //                    return View("TradeInOptionsByKarPower", returnVehicle);
        //                }
        //                else
        //                {
        //                    var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerInfo.DealershipId);

        //                    returnVehicle.ReviewList = new List<CustomerReview>();

        //                    foreach (var customerReview in comments.Select(tmp => new CustomerReview() { City = tmp.City, Name = tmp.Name, ReviewContent = tmp.Content, State = tmp.State }))
        //                    {
        //                        returnVehicle.ReviewList.Add(customerReview);
        //                    }

        //                    return View("TradeInCustomer", returnVehicle);
        //                }
        //            }
        //            else
        //            {
        //                return RedirectToAction("TradeInVehicleInvalidVin");
        //            }
        //        }
        //    }
        //    else if (!String.IsNullOrEmpty(vehicle.SelectedTrim) && !vehicle.SelectedTrim.Equals("0"))
        //    {
        //        using (var context = new whitmanenterprisewarehouseEntities())
        //        {
        //            var makes = (List<SelectListItem>)Session["KarPowerMakes"];
        //            var models = (List<SelectListItem>)Session["KarPowerModels"];
        //            var trims = (List<SelectListItem>)Session["KarPowerTrims"];
        //            var selectedMake = makes.FirstOrDefault(i => i.Value == vehicle.SelectedMake);
        //            var selectedModel = models.FirstOrDefault(i => i.Value == vehicle.SelectedModel);
        //            var selectedTrim = trims.FirstOrDefault(i => i.Value == vehicle.SelectedTrim);
        //            if (selectedMake != null && selectedModel != null && selectedTrim != null)
        //            {
        //                var optionalEquipmentMarkupList = GetOptionalEquipment(vehicle.SelectedYear, selectedMake.Text, selectedModel.Text);
        //                var returnVehicle = new TradeInVehicleModel
        //                                         {
        //                                             Condition = vehicle.Condition,
        //                                             EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.SelectedTrim),
        //                                             SelectedYear = vehicle.SelectedYear,
        //                                             SelectedMake = selectedMake.Text,
        //                                             SelectedModel = selectedModel.Text,
        //                                             SelectedTrim = selectedTrim.Text,
        //                                             VehicleId = Convert.ToInt32(vehicle.SelectedTrim),
        //                                             OptionalEquipment = optionalEquipmentMarkupList.Select(i => new ExtendedEquipmentOption()
        //                                             {
        //                                                 VehicleOptionId = Convert.ToInt32(i.Id),
        //                                                 DisplayName = i.DisplayName,
        //                                                 DisplayNameAdditionalData = i.DisplayName,
        //                                                 IsSelected = i.IsSelected
        //                                             }).ToList(),
        //                                             Mileage = mileageString,
        //                                             SelectedOptions = vehicle.SelectedOptions
        //                                         };

        //                if (returnVehicle.OptionalEquipment != null && returnVehicle.OptionalEquipment.Any())
        //                {
        //                    return View("TradeInOptionsByKarPower", returnVehicle);
        //                }

        //                var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerInfo.DealershipId);

        //                returnVehicle.ReviewList = new List<CustomerReview>();

        //                foreach (var customerReview in comments.Select(tmp => new CustomerReview() { City = tmp.City, Name = tmp.Name, ReviewContent = tmp.Content, State = tmp.State }))
        //                {
        //                    returnVehicle.ReviewList.Add(customerReview);
        //                }

        //                return View("TradeInCustomer", returnVehicle);

        //            }
        //            //}
        //        }

        //    }

        //    return RedirectToAction("TradeInVehicleInvalidVin");
        //}

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
                var styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId );
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
            var chromeStyle =autoService.GetStyles(chromeModelId).First();
            return chromeStyle != null ? chromeStyle.id : 0;
        }

        public ActionResult TradeInOptions(TradeInVehicleModel vehicle)
        {
            if (!String.IsNullOrEmpty(vehicle.SelectedTrim) && !vehicle.SelectedTrim.Contains("Trim..."))
            {

                vehicle.VehicleId = Convert.ToInt32(vehicle.SelectedTrim);

                string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

                int mileageNumber = 0; Int32.TryParse(mileageString, out mileageNumber);

                if (Session["TradeInDealer"] != null)
                {

                    var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                    var returnVehicle = KellyBlueBookHelper.GetKBBVehicleFromVehicleId(vehicle.VehicleId, mileageNumber, dealerSessionInfo.ZipCode);

                    returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.SelectedTrim);

                    if (returnVehicle.OptionalEquipment.Any())
                    {


                        return View("TradeInOptions", returnVehicle);
                    }
                    else
                    {
                        var context = new whitmanenterprisewarehouseEntities();


                        var comments =
                            context.vincontroltradeinbannercomments.Where(
                                x => x.DealerId == dealerSessionInfo.DealershipId);

                        returnVehicle.ReviewList = new List<CustomerReview>();

                        foreach (var tmp in comments)
                        {
                            var customerReview = new CustomerReview()
                            {
                                City = tmp.City,
                                Name = tmp.Name,
                                ReviewContent = tmp.Content,
                                State = tmp.State
                            };

                            returnVehicle.ReviewList.Add(customerReview);
                        }
                        return View("TradeInCustomer", returnVehicle);
                    }



                }
                else
                {
                    var httpCookie = Request.Cookies["Vindealer"];

                    int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

                    var context = new whitmanenterprisewarehouseEntities();

                    var dealerDefault =
                        context.whitmanenterprisedealerships.First(
                            x => x.idWhitmanenterpriseDealership == dealerId);

                    var dealerDefaultSetting =
                        context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);


                    var dealerInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()

                    };
                    Session["TradeInDealer"] = dealerInfo;

                    var returnVehicle = KellyBlueBookHelper.GetKBBVehicleFromVehicleId(vehicle.VehicleId, mileageNumber, dealerInfo.ZipCode);

                    returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.SelectedTrim);


                    if (returnVehicle.OptionalEquipment.Any())
                    {
                        return View("TradeInOptions", returnVehicle);
                    }
                    else
                    {

                        var comments =
                            context.vincontroltradeinbannercomments.Where(
                                x => x.DealerId == dealerInfo.DealershipId);

                        returnVehicle.ReviewList = new List<CustomerReview>();

                        foreach (var tmp in comments)
                        {
                            var customerReview = new CustomerReview()
                            {
                                City = tmp.City,
                                Name = tmp.Name,
                                ReviewContent = tmp.Content,
                                State = tmp.State
                            };

                            returnVehicle.ReviewList.Add(customerReview);
                        }
                        return View("TradeInCustomer", returnVehicle);
                    }


                }


            }

            else
            {
                if (!String.IsNullOrEmpty(vehicle.Vin.Trim()))
                {
                    string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

                    int mileageNumber = 0; Int32.TryParse(mileageString, out mileageNumber);

                    TradeInVehicleModel returnVehicle = null;

                    if (Session["TradeInDealer"] != null)
                    {

                        var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                        returnVehicle = KellyBlueBookHelper.GetKBBTrimsOrOptionsFromVin(vehicle.Vin.Trim(), mileageNumber, dealerSessionInfo.ZipCode);


                    }
                    else
                    {
                        var httpCookie = Request.Cookies["Vindealer"];

                        int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

                        var context = new whitmanenterprisewarehouseEntities();

                        var dealerDefault =
                            context.whitmanenterprisedealerships.First(
                                x => x.idWhitmanenterpriseDealership == dealerId);

                        var dealerDefaultSetting =
                            context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);


                        var dealerInfo = new DealershipViewModel()
                        {
                            DealershipId = dealerId,
                            DealershipName = dealerDefault.DealershipName,
                            DealershipAddress = dealerDefault.DealershipAddress,
                            Address = dealerDefault.Address,
                            City = dealerDefault.City,
                            State = dealerDefault.State,
                            ZipCode = dealerDefault.ZipCode,
                            Phone = dealerDefault.Phone,
                            Email = dealerDefault.Email,
                            CarFax = dealerDefaultSetting.CarFax,
                            CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                            EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                            Manheim = dealerDefaultSetting.Manheim,
                            ManheimPassword = dealerDefaultSetting.ManheimPassword,
                            KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                            KellyPassword = dealerDefaultSetting.KellyPassword,
                            PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()

                        };
                        Session["TradeInDealer"] = dealerInfo;

                        returnVehicle = KellyBlueBookHelper.GetKBBTrimsOrOptionsFromVin(vehicle.Vin.Trim(), mileageNumber, dealerInfo.ZipCode);

                    }

                    returnVehicle.Condition = vehicle.Condition;

                    if (returnVehicle.ValidVin)
                    {

                        if (returnVehicle.SpecificKBBTrimList.Count > 1)
                        {
                            foreach (var tmp in returnVehicle.SpecificKBBTrimList)
                            {
                                tmp.VIN = EncryptionHelper.EncryptString(tmp.Id.ToString(CultureInfo.InvariantCulture));

                            }

                            returnVehicle.Dealer = vehicle.Dealer;

                            return View("TradeInMultipleTrims", returnVehicle);
                        }
                        else
                        {
                            returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(returnVehicle.SpecificKBBTrimList.First().Id.ToString(CultureInfo.InvariantCulture));

                            if (returnVehicle.OptionalEquipment.Any())

                                return View("TradeInOptions", returnVehicle);

                            return View("TradeInCustomer", returnVehicle);
                        }
                    }
                    else
                    {
                        return RedirectToAction("TradeInVehicleInvalidVin");


                    }
                }
                else
                {

                    return RedirectToAction("TradeInVehicleInvalidVin");

                }
            }


        }

        //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
        public ActionResult GetTradeInValue(TradeInVehicleModel vehicle)
        {
            if (!ValidateCustomerSubmitForm(vehicle))
            {
                var context = new whitmanenterprisewarehouseEntities();

                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

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

            try
            {
                string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

                int mileageNumber = 0;

                Int32.TryParse(mileageString, out mileageNumber);

                vehicle.EncryptVehicleId = vehicle.EncryptVehicleId.Replace(" ", "+");

                int id = Convert.ToInt32(EncryptionHelper.DecryptString(vehicle.EncryptVehicleId));

                var returnVehicle = new TradeInVehicleModel();

                var manheimUserName = string.Empty;
                var manheimPassword = string.Empty;

                DealershipViewModel dealerSessionInfo = null;
                var context = new whitmanenterprisewarehouseEntities();
                if (Session["TradeInDealer"] != null)
                {
                    dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                    //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
                    //var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId);
                    //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);
                    manheimUserName = dealerSessionInfo.Manheim;
                    manheimPassword = dealerSessionInfo.ManheimPassword;

                    returnVehicle.DealerId = dealerSessionInfo.DealershipId;

                    returnVehicle.DealerName = dealerSessionInfo.DealershipName;
                }
                else
                {
                    var httpCookie = Request.Cookies["Vindealer"];

                    int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

                    var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);

                    var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                    dealerSessionInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                    };
                    Session["TradeInDealer"] = dealerSessionInfo;

                    //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
                    //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);
                    manheimUserName = dealerSessionInfo.Manheim;
                    manheimPassword = dealerSessionInfo.ManheimPassword;

                    returnVehicle.DealerId = dealerSessionInfo.DealershipId;

                    returnVehicle.DealerName = dealerSessionInfo.DealershipName;
                }

                //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
                returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.VehicleId.ToString());
                returnVehicle.SelectedYear = String.IsNullOrEmpty(vehicle.SelectedYear) || vehicle.SelectedYear == "Year..." ? "0" : vehicle.SelectedYear;
                returnVehicle.SelectedMake = vehicle.SelectedMake;
                returnVehicle.SelectedModel = vehicle.SelectedModel;
                returnVehicle.SelectedTrim = vehicle.SelectedTrim;
                returnVehicle.SelectedMakeValue = vehicle.SelectedMakeValue;
                returnVehicle.SelectedModelValue = vehicle.SelectedModelValue;
                returnVehicle.SelectedTrimValue = vehicle.SelectedTrimValue;
                returnVehicle.VehicleId = vehicle.VehicleId;
                returnVehicle.Mileage = mileageString;
                returnVehicle.SelectedOptions = vehicle.SelectedOptions;
                returnVehicle.SelectedOptionList = vehicle.SelectedOptionList;
                returnVehicle.Vin = vehicle.Vin;
                returnVehicle.Condition = CommonHelper.UppercaseWords(vehicle.Condition);
                returnVehicle.VehicleId = id;
                returnVehicle.MileageNumber = mileageNumber;
                returnVehicle.CustomerEmail = vehicle.CustomerEmail;
                returnVehicle.CustomerPhone = vehicle.CustomerPhone;
                returnVehicle.CustomerFirstName = CommonHelper.UppercaseWords(vehicle.CustomerFirstName.ToLower());
                returnVehicle.CustomerLastName = CommonHelper.UppercaseWords(vehicle.CustomerLastName.ToLower());
                returnVehicle.Dealer = EncryptionHelper.EncryptString(returnVehicle.DealerId.ToString(CultureInfo.InvariantCulture));


                try
                {
                    var year = int.Parse(returnVehicle.SelectedYear);

                    var manheimResult = LinqHelper.ManheimReportForTradeIn(vehicle.Vin, year, returnVehicle.SelectedMakeValue,
                                                                           returnVehicle.SelectedModelValue,
                                                                           returnVehicle.SelectedTrimValue, manheimUserName,
                                                                           manheimPassword);
                    returnVehicle.TradeInFairPrice =
                        (Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(manheimResult.LowestPrice)) -
                         dealerSessionInfo.PriceVariance).ToString("c0");
                    returnVehicle.TradeInGoodPrice =
                        (Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(manheimResult.AveragePrice)) -
                         dealerSessionInfo.PriceVariance).ToString("c0");

                }
                catch (Exception e)
                {

                }


                var tradeinEmail = SendEmail(vehicle, returnVehicle, dealerSessionInfo);
                returnVehicle.EmailTextContent = tradeinEmail.TextContent;
                returnVehicle.EmailADFContent = tradeinEmail.AdfContent;
                returnVehicle.Receivers = tradeinEmail.Receivers;

                int tradeinAutoId = SQLHelper.AddTradeInCustomerVehicle(returnVehicle);

                returnVehicle.TradeInCustomerId = EncryptionHelper.EncryptString(tradeinAutoId.ToString(CultureInfo.InvariantCulture));

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

                    var carFaxModel = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(vehicle.Vin, dealerSessionInfo.CarFax, dealerSessionInfo.CarFaxPassword);

                    returnVehicle.CarFax = carFaxModel;

                }

                return View("TradeInValue", returnVehicle);
            }
            catch (Exception)
            {
                //throw;
                var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
                var httpCookie = Request.Cookies["dealer"];
                if (httpCookie != null)
                    //return RedirectToAction("TradeInVehicle", new { dealer = httpCookie.Value });
                    return RedirectToAction("Index", new { dealer = dealerSessionInfo.DealershipName });

                //return RedirectToAction("TradeInVehicle", new { dealer = dealerSessionInfo.EncryptDealerId });
                return RedirectToAction("Index", new { dealer = dealerSessionInfo.DealershipName });
            }


        }


        //public ActionResult GetTradeInValue(TradeInVehicleModel vehicle)
        //{
        //    if (!ValidateCustomerSubmitForm(vehicle))
        //    {
        //        var context = new whitmanenterprisewarehouseEntities();

        //        var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

        //        var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

        //        vehicle.ReviewList = new List<CustomerReview>();

        //        foreach (var tmp in comments)
        //        {
        //            var customerReview = new CustomerReview()
        //            {
        //                City = tmp.City,
        //                Name = tmp.Name,
        //                ReviewContent = tmp.Content,
        //                State = tmp.State
        //            };

        //            vehicle.ReviewList.Add(customerReview);
        //        }

        //        return View("TradeInCustomer", vehicle);
        //    }

        //    try
        //    {
        //        string mileageString = CommonHelper.RemoveSpecialCharactersForMsrp(vehicle.Mileage);

        //        int mileageNumber = 0;

        //        Int32.TryParse(mileageString, out mileageNumber);

        //        vehicle.EncryptVehicleId = vehicle.EncryptVehicleId.Replace(" ", "+");

        //        int id = Convert.ToInt32(EncryptionHelper.DecryptString(vehicle.EncryptVehicleId));

        //        var returnVehicle = new TradeInVehicleModel();

        //        var manheimUserName = string.Empty;
        //        var manheimPassword = string.Empty;

        //        DealershipViewModel dealerSessionInfo = null;
        //        var context = new whitmanenterprisewarehouseEntities();
        //        if (Session["TradeInDealer"] != null)
        //        {
        //            dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

        //            //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
        //            //var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId);
        //            //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);
        //            manheimUserName = dealerSessionInfo.Manheim;
        //            manheimPassword = dealerSessionInfo.ManheimPassword;

        //            returnVehicle.DealerId = dealerSessionInfo.DealershipId;

        //            returnVehicle.DealerName = dealerSessionInfo.DealershipName;
        //        }
        //        else
        //        {
        //            var httpCookie = Request.Cookies["Vindealer"];

        //            int dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(httpCookie.Value));

        //            var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);

        //            var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

        //            dealerSessionInfo = new DealershipViewModel()
        //            {
        //                DealershipId = dealerId,
        //                DealershipName = dealerDefault.DealershipName,
        //                DealershipAddress = dealerDefault.DealershipAddress,
        //                Address = dealerDefault.Address,
        //                City = dealerDefault.City,
        //                State = dealerDefault.State,
        //                ZipCode = dealerDefault.ZipCode,
        //                Phone = dealerDefault.Phone,
        //                Email = dealerDefault.Email,
        //                CarFax = dealerDefaultSetting.CarFax,
        //                CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
        //                EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
        //                KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
        //                KellyPassword = dealerDefaultSetting.KellyPassword,
        //                Manheim = dealerDefaultSetting.Manheim,
        //                ManheimPassword = dealerDefaultSetting.ManheimPassword,
        //                PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
        //            };
        //            Session["TradeInDealer"] = dealerSessionInfo;

        //            //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
        //            //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);
        //            manheimUserName = dealerSessionInfo.Manheim;
        //            manheimPassword = dealerSessionInfo.ManheimPassword;

        //            returnVehicle.DealerId = dealerSessionInfo.DealershipId;

        //            returnVehicle.DealerName = dealerSessionInfo.DealershipName;
        //        }

        //        //NOTE: 02/22/2013 - David changed this method to use value from Mainheim
        //        returnVehicle.EncryptVehicleId = EncryptionHelper.EncryptString(vehicle.VehicleId.ToString());
        //        returnVehicle.SelectedYear = String.IsNullOrEmpty(vehicle.SelectedYear) || vehicle.SelectedYear == "Year..." ? "0" : vehicle.SelectedYear;
        //        returnVehicle.SelectedMake = vehicle.SelectedMake;
        //        returnVehicle.SelectedModel = vehicle.SelectedModel;
        //        returnVehicle.SelectedTrim = vehicle.SelectedTrim;
        //        returnVehicle.SelectedMakeValue = vehicle.SelectedMakeValue;
        //        returnVehicle.SelectedModelValue = vehicle.SelectedModelValue;
        //        returnVehicle.SelectedTrimValue = vehicle.SelectedTrimValue;
        //        returnVehicle.VehicleId = vehicle.VehicleId;
        //        returnVehicle.Mileage = mileageString;
        //        returnVehicle.SelectedOptions = vehicle.SelectedOptions;
        //        returnVehicle.SelectedOptionList = vehicle.SelectedOptionList;
        //        try
        //        {
        //            var year = int.Parse(returnVehicle.SelectedYear);
        //            var matchedManheimMake = GetManheimMake(returnVehicle.SelectedMakeValue).ToLower();
        //            var matchedManheimModel = GetManheimModel(returnVehicle.SelectedModelValue).ToLower();
        //            var manheimmakeItem = context.manheimmakes.FirstOrDefault(i => i.name.ToLower() == matchedManheimMake);
        //            var manheimmodelItem = context.manheimmakemodels.Where(i => i.manheimmodel.name.ToLower() == matchedManheimModel && i.manheimyearmake.makeId == manheimmakeItem.id && i.manheimyearmake.year.Value == year).Select(i => i.manheimmodel).FirstOrDefault();
        //            if (manheimmodelItem == null)
        //            {
        //                manheimmodelItem = context.manheimmakemodels.Where(i => i.manheimmodel.name.ToLower().Contains(matchedManheimModel.ToLower()) && i.manheimyearmake.makeId == manheimmakeItem.id && i.manheimyearmake.year == year).Select(i => i.manheimmodel).FirstOrDefault();
        //            }

        //            var manheimtrimItem = context.manheimmodeltrims.Where(t => t.manheimmakemodel.modelId == manheimmodelItem.id && t.manheimmakemodel.manheimyearmake.year == year && t.manheimmakemodel.manheimyearmake.makeId == manheimmakeItem.id).Select(i => i.manheimtrim).FirstOrDefault();

        //            var manheimResult = new List<ManheimWholesaleViewModel>();
        //            if (!String.IsNullOrEmpty(vehicle.Vin))
        //            {
        //                manheimResult = LinqHelper.ManheimReportForTradeIn(vehicle.Vin, 0, 0, 0, 0, manheimUserName, manheimPassword);
        //            }
        //            else if (manheimmakeItem != null && manheimmodelItem != null && manheimmakeItem.serviceId.HasValue && manheimmodelItem.serviceId.HasValue)
        //            {
        //                if (manheimtrimItem != null && manheimtrimItem.serviceId.HasValue)
        //                {
        //                    manheimResult = LinqHelper.ManheimReportForTradeIn(vehicle.Vin, Convert.ToInt32(returnVehicle.SelectedYear), manheimmakeItem.serviceId.Value, manheimmodelItem.serviceId.Value, manheimtrimItem.serviceId.Value, manheimUserName, manheimPassword);
        //                }
        //                else
        //                {
        //                    var manheimService = new ManheimService();
        //                    var trimItem = manheimService.GetTrimItem(returnVehicle.SelectedYear, manheimmakeItem.serviceId.Value.ToString(), manheimmodelItem.serviceId.Value.ToString());
        //                    manheimResult = LinqHelper.ManheimReportForTradeIn(vehicle.Vin, Convert.ToInt32(returnVehicle.SelectedYear), manheimmakeItem.serviceId.Value, manheimmodelItem.serviceId.Value, trimItem.ServiceId, manheimUserName, manheimPassword);
        //                }

        //            }

        //            if (manheimResult.Count > 0)
        //            {
        //                var firstResult = manheimResult.FirstOrDefault();

        //                returnVehicle.TradeInFairPrice = (Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(firstResult.LowestPrice)) - dealerSessionInfo.PriceVariance).ToString("c0");
        //                returnVehicle.TradeInGoodPrice = (Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForPurePrice(firstResult.AveragePrice)) - dealerSessionInfo.PriceVariance).ToString("c0");
        //            }
        //        }
        //        catch (Exception e)
        //        {

        //        }

        //        returnVehicle.Vin = vehicle.Vin;

        //        returnVehicle.Condition = CommonHelper.UppercaseWords(vehicle.Condition);

        //        returnVehicle.VehicleId = id;

        //        returnVehicle.MileageNumber = mileageNumber;

        //        returnVehicle.CustomerEmail = vehicle.CustomerEmail;

        //        returnVehicle.CustomerPhone = vehicle.CustomerPhone;

        //        returnVehicle.CustomerFirstName = CommonHelper.UppercaseWords(vehicle.CustomerFirstName.ToLower());

        //        returnVehicle.CustomerLastName = CommonHelper.UppercaseWords(vehicle.CustomerLastName.ToLower());

        //        returnVehicle.Dealer = EncryptionHelper.EncryptString(returnVehicle.DealerId.ToString(CultureInfo.InvariantCulture));

        //        var tradeinEmail = SendEmail(vehicle, returnVehicle, dealerSessionInfo);
        //        returnVehicle.EmailTextContent = tradeinEmail.TextContent;
        //        returnVehicle.EmailADFContent = tradeinEmail.AdfContent;
        //        returnVehicle.Receivers = tradeinEmail.Receivers;

        //        int tradeinAutoId = SQLHelper.AddTradeInCustomerVehicle(returnVehicle);

        //        returnVehicle.TradeInCustomerId = EncryptionHelper.EncryptString(tradeinAutoId.ToString(CultureInfo.InvariantCulture));

        //        if (String.IsNullOrEmpty(vehicle.Vin))
        //        {
        //            returnVehicle.CarFax = new CarFaxViewModel()
        //            {
        //                Success = false,
        //            };
        //        }
        //        else
        //        {
        //            returnVehicle.CarFax = new CarFaxViewModel();

        //            var carFaxModel = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(vehicle.Vin, dealerSessionInfo.CarFax, dealerSessionInfo.CarFaxPassword);

        //            returnVehicle.CarFax = carFaxModel;

        //        }

        //        return View("TradeInValue", returnVehicle);
        //    }
        //    catch (Exception)
        //    {
        //        //throw;
        //        var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];
        //        var httpCookie = Request.Cookies["dealer"];
        //        if (httpCookie != null)
        //            //return RedirectToAction("TradeInVehicle", new { dealer = httpCookie.Value });
        //            return RedirectToAction("Index", new { dealer = dealerSessionInfo.DealershipName });

        //        //return RedirectToAction("TradeInVehicle", new { dealer = dealerSessionInfo.EncryptDealerId });
        //        return RedirectToAction("Index", new { dealer = dealerSessionInfo.DealershipName });
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
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var dealerSessionInfo = (DealershipViewModel)Session["TradeInDealer"];

                    var comments = context.vincontroltradeinbannercomments.Where(x => x.DealerId == dealerSessionInfo.DealershipId);

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
                var context = new whitmanenterprisewarehouseEntities();
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
                    var dealerDefault = context.whitmanenterprisedealerships.First(x => x.idWhitmanenterpriseDealership == dealerId);
                    var dealerDefaultSetting = context.whitmanenterprisesettings.First(x => x.DealershipId == dealerId);

                    dealerSessionInfo = new DealershipViewModel()
                    {
                        DealershipId = dealerId,
                        DealershipName = dealerDefault.DealershipName,
                        DealershipAddress = dealerDefault.DealershipAddress,
                        Address = dealerDefault.Address,
                        City = dealerDefault.City,
                        State = dealerDefault.State,
                        ZipCode = dealerDefault.ZipCode,
                        Phone = dealerDefault.Phone,
                        Email = dealerDefault.Email,
                        CarFax = dealerDefaultSetting.CarFax,
                        CarFaxPassword = dealerDefaultSetting.CarFaxPassword,
                        EmailFormat = dealerDefault.EmailFormat.GetValueOrDefault(),
                        KellyBlueBook = dealerDefaultSetting.KellyBlueBook,
                        KellyPassword = dealerDefaultSetting.KellyPassword,
                        Manheim = dealerDefaultSetting.Manheim,
                        ManheimPassword = dealerDefaultSetting.ManheimPassword,
                        PriceVariance = dealerDefault.PriceVariance.GetValueOrDefault()
                    };

                    Session["TradeInDealer"] = dealerSessionInfo;

                    //returnVehicle = KellyBlueBookHelper.GetKBBTradeInValue(id, mileageNumber, vehicle.SelectedOptions, dealerSessionInfo.ZipCode, true, dealerDefault.PriceVariance.HasValue ? dealerDefault.PriceVariance.Value : 0);

                    //returnVehicle.DealerId = dealerSessionInfo.DealershipId;
                    //returnVehicle.DealerName = dealerSessionInfo.DealershipName;
                }

                var makes = (List<SelectListItem>)Session["KarPowerMakes"];
                var models = (List<SelectListItem>)Session["KarPowerModels"];
                var trims = (List<SelectListItem>)Session["KarPowerTrims"];
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
                    string baseWholesale = string.Empty;
                    string mileageAdjustment = string.Empty;
                    string wholesale = string.Empty;
                    if (!String.IsNullOrEmpty(_karPowerService.GetValuationResult))
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.GetValuationResult);
                        baseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                        mileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                        wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
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

                //int tradeinAutoId = SQLHelper.AddTradeInCustomerVehicle(returnVehicle);
                //returnVehicle.TradeInCustomerId = EncryptionHelper.EncryptString(tradeinAutoId.ToString(CultureInfo.InvariantCulture));

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

                    var carFaxModel = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(vehicle.Vin, dealerSessionInfo.CarFax, dealerSessionInfo.CarFaxPassword);

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

            switch (dealerSessionInfo.EmailFormat)
            {
                case 0:
                    tradeinEmail.TextContent = SendTextContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                    break;
                case 1:
                    tradeinEmail.AdfContent = SendAdfContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                    break;
                case 2:
                    tradeinEmail.TextContent = SendTextContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                    tradeinEmail.AdfContent = SendAdfContent(tradeinEmail.Receivers, returnVehicle, dealerSessionInfo, bodyPdf);
                    break;
                default:
                    break;
            }

            return tradeinEmail;

        }

        private string SendAdfContent(List<string> receivers, TradeInVehicleModel returnVehicle, DealershipViewModel dealerSessionInfo, string bodyPDF)
        {
            string emailContent = string.Empty;
            if (Session["CustomerLookUpVin"] == null)
            {
                emailContent = EmailHelper.CreateBodyEmailForSendClientRequestForAdf(
                        returnVehicle, dealerSessionInfo);
            }
            else
            {
                emailContent = EmailHelper.CreateBodyEmailForSendClientRequestForAdf(Session["CustomerLookUpVin"].ToString(),
                                                                          returnVehicle, dealerSessionInfo);
            }

            EmailHelper.SendEmailForTradeInBannerToDealer(
                    receivers,
                    "Customer Request Trade In Value For " + returnVehicle.SelectedYear + " " +
                    returnVehicle.SelectedMakeValue + " " + returnVehicle.SelectedModelValue +
                    " " +
                    returnVehicle.SelectedTrimValue, emailContent
                    , bodyPDF);
            return emailContent;
        }

        private string SendTextContent(List<string> receivers, TradeInVehicleModel returnVehicle, DealershipViewModel dealerSessionInfo, string bodyPDF)
        {
            string emailContent = string.Empty;
            if (Session["CustomerLookUpVin"] == null)
            {
                emailContent = EmailHelper.CreateBodyEmailForSendClientRequestForText(
                        returnVehicle, dealerSessionInfo);
            }
            else
            {
                emailContent = EmailHelper.CreateBodyEmailForSendClientRequestForText(Session["CustomerLookUpVin"].ToString(),
                                                                        returnVehicle, dealerSessionInfo);
            }
            EmailHelper.SendEmailForTradeInBannerToDealer(
                  receivers,
                  "Customer Request Trade In Value For " + returnVehicle.SelectedYear + " " +
                  returnVehicle.SelectedMakeValue + " " + returnVehicle.SelectedModelValue +
                  " " +
                  returnVehicle.SelectedTrimValue, emailContent
                 , bodyPDF);
            return emailContent;
        }

        public ActionResult TradeInCustomerEmail(int tradeInId)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var tradeIn =
                context.vincontrolbannercustomers.First(x => x.TradeInCustomerId == tradeInId);

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

        public JsonResult YearAjaxPost(int yearId)
        {
            return new DataContractJsonResult(KellyBlueBookHelper.GetKBBMakesByYear(yearId));
        }

        public JsonResult MakeAjaxPost(int yearId, int makeId)
        {
            return new DataContractJsonResult(KellyBlueBookHelper.GetKBBModelByMakeId(yearId, makeId));
        }

        public JsonResult ModelAjaxPost(int yearId, int modelId)
        {
            return new DataContractJsonResult(KellyBlueBookHelper.GetKBBTrimByModelId(yearId, modelId));
        }

        public ActionResult GetYears()
        {
            var viewModel = new TradeInVehicleModel() { SelectedYear = "0", YearsList = new List<SelectListItem>() };
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

        private static IEnumerable<SelectListItem> GetYearData(TradeInVehicleModel viewModel)
        {
            var autoService = new ChromeAutoService();
            var years = autoService.GetModelYears();

            var result = new List<SelectListItem> { SelectListHelper.CreateSelectListItem("Year...", "Year...", true) };
            if (years != null)
            {
                result.AddRange(years.Select(item => new SelectListItem
                                                         {
                                                             Text = item.ToString(CultureInfo.InvariantCulture),
                                                             Value = item.ToString(CultureInfo.InvariantCulture)
                                                         }));
            }

            return result;
        }

       
        public ActionResult GetMakes(int yearId)
        {
            var viewModel = new TradeInVehicleModel { SelectedMake = "0", MakesList = new List<SelectListItem>() };
            try
            {
                var result = new List<SelectListItem> { SelectListHelper.CreateSelectListItem("Make...", "Make...", true) };
                var context = new vincontrolwarehouseEntities();
                var makeList =
                    context.yearmakes.Where(ym => ym.Year == yearId).Select(ym => new { ym.make.Value, ym.MakeId }).ToList();

                result.AddRange(
                    makeList.Select(
                        ym =>
                        new SelectListItem { Text = ym.Value, Value = ym.MakeId.ToString(CultureInfo.InvariantCulture) })
                        .ToList());
                viewModel.MakesList = result;
                Session["KarPowerMakes"] = viewModel.MakesList;
            }
            catch (Exception e)
            {

            }
            return PartialView("Makes", viewModel);
        }


        //private string ReplaceMakeName(string name)
        //{
        //    name = name.Replace("b m w", "BMW");
        //    name = name.Replace("american (amc)", "American Motors (AMC)");
        //    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        //}

        //private string ReplaceModelName(string name)
        //{
        //    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        //}

        //private string ReplaceTrimName(string name)
        //{
        //    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        //}

        ////KarPower GetModels
        //public ActionResult GetModels(int yearId, int makeId)
        //{
        //    var viewModel = new TradeInVehicleModel() { SelectedModel = "0", ModelsList = new List<SelectListItem>() };
        //    try
        //    {
        //        _karPowerService.GetModels(yearId, makeId, DateTime.Now);
        //        var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
        //        viewModel.ModelsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
        //        Session["KarPowerModels"] = viewModel.ModelsList;
        //        if (viewModel.ModelsList.Count() > 0)
        //        {
        //            viewModel.ModelsList.FirstOrDefault().Text = "Model...";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return PartialView("Models", viewModel);
        //}

        //public ActionResult GetModels(int yearId, int makeId)
        //{
        //    var viewModel = new TradeInVehicleModel() { SelectedModel = "0", ModelsList = new List<SelectListItem>() };
        //    try
        //    {
        //        var context = new vincontrolwarehouseEntities();
        //        var modelList = context.models.Where(m => m.yearmake.MakeId == makeId).Select(m => new { m.Value, m.Id }).ToList();
        //        viewModel.ModelsList = modelList.Select(ym => new SelectListItem { Text = ReplaceModelName(ym.Value), Value = ym.Id.ToString(CultureInfo.InvariantCulture) }).ToList();
        //        Session["KarPowerModels"] = viewModel.ModelsList;
        //        if (viewModel.ModelsList.Count() > 0)
        //        {
        //            viewModel.ModelsList.FirstOrDefault().Text = "Model...";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return PartialView("Models", viewModel);
        //}

        public ActionResult GetModels(int yearId, int makeId)
        {
            var viewModel = new TradeInVehicleModel { SelectedModel = "0", ModelsList = new List<SelectListItem>() };
            try
            {
                var result = new List<SelectListItem> { SelectListHelper.CreateSelectListItem("Model...", "Model...", true) };
                var context = new vincontrolwarehouseEntities();
                var modelList = context.models.Where(m => m.yearmake.MakeId == makeId && m.yearmake.Year == yearId).Select(m => new { m.Value, m.Id }).ToList();
                if (!modelList.Any())
                {
                    var yearmakeItem =
                        context.yearmakes.Where(m => m.MakeId == makeId)
                               .Select(i => new { i.MakeId, i.make.ChromeId, i.Id })
                               .FirstOrDefault();
                    var autoService = new ChromeAutoService();
                    if (yearmakeItem != null)
                    {
                        var models = autoService.GetModelsByDivision(yearId, yearmakeItem.ChromeId.Value);
                        if (models != null)
                        {
                            var list = models.Select(model => new model
                                {
                                    YearMakeId = yearmakeItem.Id,
                                    Value = model.Value,
                                    ChromeId = model.id
                                });
                            foreach (var newModel in list)
                            {
                                context.AddTomodels(newModel);
                                context.SaveChanges();
                                result.Add(new SelectListItem { Text = newModel.Value, Value = newModel.Id.ToString(CultureInfo.InvariantCulture) });
                            }
                        }
                    }
                }
                else
                {
                    result.AddRange(modelList.Select(ym => new SelectListItem { Text = ym.Value, Value = ym.Id.ToString(CultureInfo.InvariantCulture) }).ToList());
                }

                viewModel.ModelsList = result;
                Session["KarPowerModels"] = viewModel.ModelsList;
            }
            catch (Exception)
            {

            }
            return PartialView("Models", viewModel);
        }

        //public ActionResult GetManheimModels(int yearId, int makeId)
        //{
        //    var viewModel = new TradeInVehicleModel() { SelectedModel = "0", ModelsList = new List<SelectListItem>() };
        //    try
        //    {
        //        var context = new vincontrolwarehouseEntities();

        //        var context = new whitmanenterprisewarehouseEntities();
        //        var modelList = context.manheimmakemodels.Where(m => m.manheimyearmake.year == yearId && m.manheimyearmake.makeId == makeId).Select(m => new { m.manheimmodel.name, m.modelId }).ToList();
        //        viewModel.ModelsList = modelList.Select(ym => new SelectListItem { Text = ReplaceModelName(ym.name), Value = ym.modelId.Value.ToString(CultureInfo.InvariantCulture) }).ToList();
        //        Session["KarPowerModels"] = viewModel.ModelsList;
        //        if (viewModel.ModelsList.Count() > 0)
        //        {
        //            viewModel.ModelsList.FirstOrDefault().Text = "Model...";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return PartialView("Models", viewModel);
        //}

        ////KarPower GetTrims
        //public ActionResult GetTrims(int yearId, int modelId)
        //{
        //    var viewModel = new TradeInVehicleModel() { SelectedTrim = "0", TrimsList = new List<SelectListItem>() };
        //    try
        //    {
        //        _karPowerService.GetTrims(yearId, modelId, DateTime.Now);
        //        var jsonObj = (JObject)JsonConvert.DeserializeObject(_karPowerService.Result);
        //        viewModel.TrimsList = _karPowerService.CreateDataList(jsonObj["d"], 0);
        //        Session["KarPowerTrims"] = viewModel.TrimsList;
        //        if (viewModel.TrimsList.Count() > 0)
        //        {
        //            viewModel.TrimsList.FirstOrDefault().Text = "Trim...";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return PartialView("Trims", viewModel);
        //}

        public ActionResult GetTrims(int yearId, int makeId, int modelId)
        {
            var viewModel = new TradeInVehicleModel() { SelectedTrim = "0", TrimsList = new List<SelectListItem>() };
            try
            {
                var result = new List<SelectListItem> { SelectListHelper.CreateSelectListItem("Trim...", "Trim...", true) };
                var context = new vincontrolwarehouseEntities();
                var modelList = context.trims.Where(t => t.ModelId == modelId).Select(m => new { m.Value, m.Id }).ToList();
                result.AddRange(modelList.Select(ym => new SelectListItem { Text = ym.Value, Value = ym.Id.ToString(CultureInfo.InvariantCulture) }).ToList());
                viewModel.TrimsList = result;
                Session["KarPowerTrims"] = viewModel.TrimsList;
            }
            catch (Exception)
            {

            }
            return PartialView("Trims", viewModel);
        }

        //public ActionResult GetTrims(int yearId, int makeId, int modelId)
        //{
        //    var viewModel = new TradeInVehicleModel() { SelectedTrim = "0", TrimsList = new List<SelectListItem>() };
        //    try
        //    {
        //        var context = new whitmanenterprisewarehouseEntities();
        //        var modelList = context.manheimmodeltrims.Where(t => t.manheimmakemodel.modelId == modelId && t.manheimmakemodel.manheimyearmake.year == yearId && t.manheimmakemodel.manheimyearmake.makeId == makeId
        //            ).Select(m => new { m.manheimtrim.name, m.trimId }).ToList();
        //        viewModel.TrimsList = modelList.Select(ym => new SelectListItem { Text = ReplaceTrimName(ym.name), Value = ym.trimId.Value.ToString(CultureInfo.InvariantCulture) }).ToList();
        //        Session["KarPowerTrims"] = viewModel.TrimsList;
        //        if (viewModel.TrimsList.Count() > 0)
        //        {
        //            viewModel.TrimsList.FirstOrDefault().Text = "Trim...";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return PartialView("Trims", viewModel);
        //}

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
            if (Session["Dealership"] == null)
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

            var context = new whitmanenterprisewarehouseEntities();
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

            var comments =
                context.vincontroltradeinbannercomments.Where(
                    x => x.DealerId == dealerSessionInfo.DealershipId).Select(e => new TradeinCommentViewModel()
                    {
                        City = e.City,
                        Content = e.Content,
                        State = e.State,
                        ID = e.SmartCommentId,
                        Name = e.Name
                    });
            return comments;
        }

        public ActionResult AddComment(string city, string state, string comment, string name)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();

                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                context.AddTovincontroltradeinbannercomments(new vincontroltradeinbannercomment()
                {
                    DealerId = dealerSessionInfo.DealershipId,
                    City = city,
                    State = state,
                    Content = comment,
                    DateComment = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Name = name
                });

                context.SaveChanges();
                return PartialView("TradeInComment", GetComments());
            }
        }

        public ActionResult SaveComment(int id, string city, string state, string comment, string name)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
                var customer = context.vincontroltradeinbannercomments.Where(e => e.DealerId == dealerSessionInfo.DealershipId && e.SmartCommentId == id).FirstOrDefault();
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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
                var customer = context.vincontroltradeinbannercomments.Where(e => e.DealerId == dealerSessionInfo.DealershipId && e.SmartCommentId == id).FirstOrDefault();
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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                var customer = context.vincontrolbannercustomers.Where(e => e.DealerId == dealerSessionInfo.DealershipId).FirstOrDefault();
                customer.VarianceValue = "15";

                return PartialView("Report", customer.VarianceValue);
            }
        }

        public ActionResult Report()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var customers = GetTradeInCustomers("Date", "des");

                return View("Report", customers);
            }
        }

        public ActionResult GetPartialTradeinList(string sort, string sortOrder)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var customers = GetTradeInCustomers(sort, sortOrder);

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

        private IOrderedEnumerable<TradeinCustomerViewModel> GetTradeInCustomers(string sortField, string sortOrder)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
            var dateToCompare = DateTime.Now.AddDays(-120);
            decimal result = 0;
            var customers = InventoryQueryHelper.GetSingleOrGroupTradein(context).Where(e => e.CreatedDate.HasValue && e.DealerId == dealerSessionInfo.DealershipId && e.CreatedDate.Value.CompareTo(dateToCompare) >= 0 && ((e.TradeInStatus == null) || !e.TradeInStatus.Equals("Deleted"))).ToList()
                .Select(e => new TradeinCustomerViewModel()
                {
                    Condition = e.Condition,
                    Date = e.CreatedDate.HasValue ? e.CreatedDate.Value.ToShortDateString() : String.Empty,
                    CreatedDate = e.CreatedDate,
                    Email = e.Email,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Make = e.Make,
                    MileageAdjustment = e.Mileage.HasValue ? e.Mileage.Value.ToString() : string.Empty,
                    Model = e.Model,
                    Phone = e.Phone,
                    TradeInStatus = e.TradeInStatus,
                    Year = e.Year.HasValue ? e.Year.Value.ToString() : String.Empty,
                    ID = e.TradeInCustomerId,
                    TradeInFairValue = decimal.TryParse(e.TradeInFairValue, out result) ? result : 0,
                    EmailContent = e.EmailContent
                });
            return SortCustomer(customers, sortField, sortOrder);
        }

        public ActionResult SaveTradeInStatus(string status, int id)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                var customer = context.vincontrolbannercustomers.Where(e => e.DealerId == dealerSessionInfo.DealershipId && e.TradeInCustomerId == id).FirstOrDefault();
                if (customer != null)
                {
                    customer.TradeInStatus = status;
                    context.SaveChanges();
                }
                return View();
            }
        }

        public ActionResult GetVarianceCost()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];
                var dealer = context.whitmanenterprisedealerships.Where(e => e.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId).FirstOrDefault();
                return View("VarianceCost", new VariantCodeViewModel() { Variance = (dealer.PriceVariance.HasValue ? dealer.PriceVariance.Value : 0) });
            }
        }

        public ActionResult SaveVarianceCost(string cost)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                var dealer = context.whitmanenterprisedealerships.Where(e => e.idWhitmanenterpriseDealership == dealerSessionInfo.DealershipId).FirstOrDefault();
                if (dealer != null)
                {
                    decimal result;
                    dealer.PriceVariance = decimal.TryParse(cost, out result) ? result : 0;
                    context.SaveChanges();
                }

                return View("VarianceCost", new VariantCodeViewModel() { Variance = (dealer.PriceVariance.HasValue ? dealer.PriceVariance.Value : 0) });

            }
        }

        public ActionResult GetEmailContent(int id)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                var customer = context.vincontrolbannercustomers.Where(e => e.TradeInCustomerId == id).FirstOrDefault();
                if (customer != null)
                {
                    return View("EmailContent", new EmailContentViewModel()
                        {
                            TextContent = customer.EmailContent,
                            ADFContent = customer.ADFEmailContent,
                            Receivers = customer.Receivers
                        });

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

        public ActionResult ResendEmailContent(int id)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var context = new whitmanenterprisewarehouseEntities();
                var dealerSessionInfo = (DealershipViewModel)Session["Dealership"];

                var customer = context.vincontrolbannercustomers.Where(e => e.TradeInCustomerId == id).FirstOrDefault();
                if (customer != null)
                {
                    var returnVehicle = DataHelper.GetTradeinVehicle(id);
                    var bodyEmail = EmailHelper.CreateBannerBodyHtmlEmail(returnVehicle, dealerSessionInfo);
                    var bodyPDF = EmailHelper.CreateBannerBodyForPdf(returnVehicle, dealerSessionInfo);
                    EmailHelper.SendEmailForTradeInBanner(new MailAddress(returnVehicle.CustomerEmail),
                                              "Your Trade In Value For " + returnVehicle.SelectedYear + " " +
                                              returnVehicle.SelectedMake + " " + returnVehicle.SelectedModel + " " +
                                              returnVehicle.SelectedTrim, bodyEmail, bodyPDF);
                    return Content(returnVehicle.CustomerEmail);
                }

                return View();

            }
        }

        public ActionResult SaveCost(int id, string cost)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var customer = context.vincontrolbannercustomers.Where(e => e.TradeInCustomerId == id).FirstOrDefault();
            if (customer != null)
            {
                customer.TradeInFairValue = cost;
                context.SaveChanges();
            }
            return View("");
        }

        #endregion
    }
}
