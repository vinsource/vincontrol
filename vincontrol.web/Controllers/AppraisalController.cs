using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.EC;
using vincontrol.Application.Forms.AppraisalManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.ChromeAutoService;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;
using MapperFactory = vincontrol.Helper.MapperFactory;
using EncryptionHelper = vincontrol.Helper.EncryptionHelper;
using DataHelper = Vincontrol.Web.HelperClass.DataHelper;
using IdentifiedString = vincontrol.ChromeAutoService.AutomativeService.IdentifiedString;
using SelectListItem = vincontrol.DomainObject.ExtendedSelectListItem;
using KarPowerService = vincontrol.KBB.KBBService;
using vincontrol.Application.Forms.CommonManagement;

namespace Vincontrol.Web.Controllers
{
    public class AppraisalController : SecurityController
    {
        private ICarFaxService _carFaxService;
        private ICommonManagementForm _commonManagementForm;
        private IAppraisalManagementForm _appraisalManagementForm;
        private IDealerManagementForm _dealerManagementForm;
        private const string PermissionCode = "APPRAISAL";
        private const string AcceptedValues = "READONLY, ALLACCESS";

        public AppraisalController()
        {
            _carFaxService = new CarFaxService();
            _commonManagementForm = new CommonManagementForm();
            _appraisalManagementForm=new AppraisalManagementForm();
            _dealerManagementForm=new DealerManagementForm();
        }

        private void ResetSessionValue()
        {
            SessionHandler.AutoTrader = null;
            SessionHandler.CarsCom = null;
            SessionHandler.AutoTraderNationwide = null;
            SessionHandler.CarsComNationwide = null;
            SessionHandler.ChromeTrimList = null;
        }

        public ActionResult ViewProfileByVin(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                appraisal.SelectedTrim = appraisal.SelectedTrimItem;

                appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, dealer,
                                                                      HttpContext.User.Identity.Name,
                                                                      SessionHandler.CurrentUser.UserId);

                SessionHandler.IsNewAppraisal = true;

                if (appraisal.ModelYear > 0)
                {
                    var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                    return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.AppraisalId });
                }
                else
                {
                    return RedirectToAction("ViewInventory", "Inventory");
                }
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewProfileByVinForTruck(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = SessionHandler.Dealer;

                appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, dealer,
                                                                      HttpContext.User.Identity.Name,
                                                                      SessionHandler.CurrentUser.UserId);

                appraisal.IsTruck = true;

                SessionHandler.IsNewAppraisal = true;

                if (appraisal.ModelYear > 0)
                {
                    var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                    return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.AppraisalId });
                }
                else
                {
                    return RedirectToAction("ViewInventory", "Inventory");
                }
            }
        }

        public ActionResult ViewProfileByYear(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, dealer, HttpContext.User.Identity.Name, SessionHandler.CurrentUser.UserId);

                appraisal.IsAppraisedByYear = true;
                appraisal.IsManualDecode = true;

                if (!String.IsNullOrEmpty(appraisal.SelectedMake))
                {
                    if (appraisal.SelectedMake.Contains("|"))
                        appraisal.Make = appraisal.SelectedMake.Substring(appraisal.SelectedMake.IndexOf("|", System.StringComparison.Ordinal) + 1);
                }
                if (!String.IsNullOrEmpty(appraisal.SelectedModel))
                {
                    if (appraisal.SelectedModel.Contains("|"))
                        appraisal.AppraisalModel = appraisal.SelectedModel.Substring(appraisal.SelectedModel.IndexOf("|", System.StringComparison.Ordinal) + 1);
                }

                if (appraisal.ModelYear > 0)
                {
                    var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                    return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.AppraisalId });
                }
                else
                {
                    return RedirectToAction("ViewInventory", "Inventory");
                }
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }


        public ActionResult ViewProfileByManualYear(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                appraisal.IsAppraisedByYear = true;
                appraisal.IsManualDecode = true;
                appraisal.Mileage = Convert.ToInt64(CommonHelper.RemoveSpecialCharactersForMsrp(form["Mileage"]));
                if (appraisal.ModelYear > 0)
                {
                    var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                    return RedirectToAction("ViewProfileForAppraisal", new {appraisalId = insertedappraisal.AppraisalId});
                }
                else
                {
                    return RedirectToAction("ViewInventory", "Inventory");
                }
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewProfileForAppraisal(int appraisalId)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
              
                var dealer = SessionHandler.Dealer;
                var row = _appraisalManagementForm.GetAppraisal(appraisalId);

                if (SessionHelper.AllowToAccessAppraisal(row) == false)
                {
                    return RedirectToAction("Unauthorized", "Security");
                }

                if (row.AppraisalStatusCodeId != Constanst.AppraisalStatus.Approved && SessionHandler.CurrentUser.RoleId != Constanst.RoleType.Employee)
                {
                    _appraisalManagementForm.UpdateApprovedStatus(row.AppraisalId);
                    
                }

                var appraisal = new AppraisalViewFormModel(row)
                {
                    Location = row.Dealer.Name,
                    DealershipId = dealer.DealershipId
                };

              
                ViewData["AverageCost"] = SessionHandler.Dealer.DealerSetting.AverageCost;
                ViewData["AverageProfit"] = SessionHandler.Dealer.DealerSetting.AverageProfit;
                ViewData["AverageProfitPercentage"] = SessionHandler.Dealer.DealerSetting.AverageProfitPercentage;
                ViewData["AverageProfitUsage"] = SessionHandler.Dealer.DealerSetting.AverageProfitUsage;

                appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;
              
                return View("SavedProfile", appraisal);

              
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult GetImageLinks(int appraisalId)
        {
            var result = GetImageResultForGallery(appraisalId);
            ViewData["result"] = result;
            return View("ImageLink");
        }

        private string GetImageResultForGallery(int appraisalId)
        {
            var result = String.Empty;


            var appraisal = _appraisalManagementForm.GetAppraisal(appraisalId);

            if (appraisal != null && !String.IsNullOrEmpty(appraisal.PhotoUrl))
            {
                var list = CommonHelper.GetArrayFromString(appraisal.PhotoUrl);
                result = list.Aggregate(result,
                    (current, tmp) =>
                        current +
                        ("<img src=\"" +
                         tmp.Replace("ThumbnailSizeImages", "NormalSizeImages") +
                         "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                         Environment.NewLine));

            }
            else if (appraisal != null)
            {
                result = "<img src=\"" +
                         (String.IsNullOrEmpty(appraisal.Vehicle.DefaultStockImage)
                             ? "http://vincontrol.com/alpha/no-images.jpg"
                             : appraisal.Vehicle.DefaultStockImage) + "\"width=\"40\" height=\"40\" value=\"Upload\" />" +
                         Environment.NewLine;
            }




            return result;
        }

        private ActionResult GetServiceInfo(Appraisal insertedAppraisal, DealershipViewModel dealer, AppraisalViewFormModel appraisal)
        {
            appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt64(appraisal.Mileage), dealer.State);

            appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;
            
            return View("SavedProfile", appraisal);
        }

        public ActionResult ViewAppraisalOnMobile(string token, int appraisalId)
        {
            ResetSessionValue();
            SessionHandler.Single = true;
            SessionHandler.IsEmployee = false;
            SessionHandler.CurrentUser = new UserRoleViewModel { Role = "Admin" };
            int dealerId = -1;
            try
            {
                dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(token).Split('|')[0]);
            }
            catch (Exception) { }

            SessionHandler.MobileDealerId = dealerId;
            var dealerViewModel = _dealerManagementForm.GetDealerById(dealerId);
            if (dealerViewModel != null)
            {
               SessionHandler.Dealer = dealerViewModel;
            }

            var dealer = SessionHandler.Dealer;
            var row = _appraisalManagementForm.GetAppraisal(appraisalId);
            if (row == null)
                return RedirectToAction("Index", "Home");

            if (row.AppraisalStatusCodeId != Constanst.AppraisalStatus.Approved && SessionHandler.CurrentUser.RoleId != Constanst.RoleType.Employee)
            {
                _appraisalManagementForm.UpdateApprovedStatus(row.AppraisalId);
            }

            var appraisal = ConvertHelper.GetAppraisalModel(row);

            appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.AppraisalModel;

            appraisal.AppraisalGenerateId = appraisalId.ToString();

            appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;
            appraisal.Location = row.Location;
            return GetServiceInfoOnMobile(row, dealer, appraisal);
        }

     
        public ActionResult ManheimDataOnMobile(string listingId)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new VincontrolEntities())
                {
                    int convertedListingId = Convert.ToInt32(listingId);
                    var row = context.Appraisals.FirstOrDefault(x => x.AppraisalId == convertedListingId);
                    var manheimCredential = VincontrolLinqHelper.GetManheimCredential(SessionHandler.MobileDealerId);
                    if (manheimCredential != null)
                        result = VincontrolLinqHelper.ManheimReportForAppraisal(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim()).GroupBy(p => p.TrimName).Select(g => g.First()).ToList();
                    else
                        result = new List<ManheimWholesaleViewModel>();
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return PartialView("ManheimData", result);
        }

        public ActionResult KarPowerDataOnMobile(string listingId)
        {
            ViewData["LISTINGID"] = listingId;

            var kbbStatus = string.Empty;
            try
            {
                kbbStatus = System.Configuration.ConfigurationManager.AppSettings["KBBStatus"];
            }
            catch (Exception)
            {
                // in case we forgot include KBBStatus in web.config
            }

            using (var context = new VincontrolEntities())
            {
                int convertedListingId = Convert.ToInt32(listingId);
                //var dealer = SessionHandler.Dealer;
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == SessionHandler.MobileDealerId);
                var row = context.Appraisals.FirstOrDefault(x => x.AppraisalId == convertedListingId);

                // save this appraisal on KarPower
                if (SessionHandler.IsNewAppraisal)
                {
                    var karpowerService = new KarPowerService();
                    var kbbUserName = string.Empty;
                    var kbbPassword = string.Empty;
                    if (setting != null)
                    {
                        kbbUserName = setting.KellyBlueBook;
                        kbbPassword = setting.KellyPassword;
                    }

                    if (!String.IsNullOrEmpty(row.Vehicle.Vin) && !String.IsNullOrEmpty(kbbUserName) &&
                        !String.IsNullOrEmpty(kbbPassword))
                    {
                        karpowerService.ExecuteSaveVehicleWithVin(row.Vehicle.Vin, row.Mileage.GetValueOrDefault(), row.ExteriorColor,
                                                                  row.Vehicle.InteriorColor, kbbUserName, kbbPassword);
                        if (!String.IsNullOrEmpty(karpowerService.SaveVrsVehicleResult))
                        {
                            var jsonObj = (JObject)JsonConvert.DeserializeObject(karpowerService.SaveVrsVehicleResult);
                            row.KarPowerEntryId = karpowerService.ConvertToString((JValue)(jsonObj["d"]["id"]));
                            context.SaveChanges();
                        }
                    }

                    SessionHandler.IsNewAppraisal = false;
                }

                if (String.IsNullOrEmpty(row.Vehicle.Vin))
                {
                    using (var scrappingContext = new VinMarketEntities())
                    {
                        if (String.IsNullOrEmpty(row.Vehicle.Vin))
                        {

                            var query = MapperFactory.GetMarketCarQuery(scrappingContext, row.Vehicle.Year, false);

                            var sampleCar = vincontrol.Helper.DataHelper.GetNationwideMarketData(query, row.Vehicle.Make, row.Vehicle.Model, row.Vehicle.Trim, false).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));

                            if (sampleCar != null)

                                row.Vehicle.Vin = sampleCar.Vin;
                        }
                    }

                }

                {
                    List<SmallKarPowerViewModel> result;
                    if (setting != null)
                    {
                        try
                        {
                            if (context.KellyBlueBooks.Any(x => x.Vin == row.Vehicle.Vin && x.Expiration > DateTime.Now))
                            {
                                var searchResult = context.KellyBlueBooks.Where(x => x.Vin == row.Vehicle.Vin);

                                result = new List<SmallKarPowerViewModel>();

                                foreach (var tmp in searchResult)
                                {
                                    var kbbModel = new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                                        MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                                        SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                        SelectedTrimName = tmp.Trim,
                                        Wholesale = tmp.WholeSale.GetValueOrDefault(),
                                        IsSelected = row.Vehicle.KBBTrimId.GetValueOrDefault() == 0 ? true : (tmp.TrimId == row.Vehicle.KBBTrimId)
                                    };

                                    result.Add(kbbModel);
                                }

                                if (result.Count > 0 && !result.Any(i => i.IsSelected))
                                {
                                    foreach (var item in result)
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                            else
                            {
                                var karpowerService = new KarPowerService();
                                if (row.Vehicle.KBBTrimId == null)
                                    result = karpowerService.Execute(row.VehicleId, row.Vehicle.Vin, row.Mileage.GetValueOrDefault().ToString(), DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, Constanst.VehicleStatus.Appraisal);
                                else
                                {
                                    result = karpowerService.Execute(row.VehicleId, row.Vehicle.Vin, row.Mileage.GetValueOrDefault().ToString(), row.Vehicle.KBBTrimId.GetValueOrDefault(), row.Vehicle.KBBOptionsId, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, Constanst.VehicleStatus.Appraisal);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            result = new List<SmallKarPowerViewModel>();
                        }

                    }
                    else
                        result = new List<SmallKarPowerViewModel>();

                    return PartialView("KarPowerData", result);
                }

            }
        }

        public ActionResult ViewProfileByYearForTruck(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var _carFaxService = new CarFaxService();

                var dealer = SessionHandler.Dealer;

                string additionalPackages = form["txtFactoryPackageOption"];

                string additionalOptions = form["txtNonInstalledOption"];


                if (!String.IsNullOrEmpty(additionalPackages))
                {
                    string[] filterOptions = additionalPackages.Split(new string[] { ",", "$", "." },
                                                                      StringSplitOptions.RemoveEmptyEntries);
                    additionalPackages = "";
                    if (filterOptions.Any())
                    {
                        foreach (string tmp in filterOptions)
                        {
                            int number = 0;
                            bool flag = int.TryParse(tmp, out number);
                            if (!flag)
                                additionalPackages += tmp.Trim() + ",";
                        }
                    }
                    if (!String.IsNullOrEmpty(additionalPackages))
                    {
                        if (additionalPackages.Substring(additionalPackages.Length - 1).Equals(","))
                            additionalPackages = additionalPackages.Substring(0, additionalPackages.Length - 1).Trim();
                    }
                }

                if (!String.IsNullOrEmpty(additionalOptions))
                {
                    string[] filterOptions = additionalOptions.Split(new string[] { ",", "$", "." },
                                                                     StringSplitOptions.RemoveEmptyEntries);
                    additionalOptions = "";
                    if (filterOptions.Any())
                    {
                        foreach (string tmp in filterOptions)
                        {
                            int number = 0;
                            bool flag = int.TryParse(tmp, out number);
                            if (!flag)
                                additionalOptions += tmp.Trim() + ",";
                        }
                    }
                    if (!String.IsNullOrEmpty(additionalOptions))
                    {
                        if (additionalOptions.Substring(additionalOptions.Length - 1).Equals(","))
                            additionalOptions = additionalOptions.Substring(0, additionalOptions.Length - 1).Trim();
                    }
                }

                if (!String.IsNullOrEmpty(appraisal.SelectedTrim) && appraisal.SelectedTrim.Contains("|"))
                {
                    var result = appraisal.SelectedTrim.Split('|');
                    if (result.Count() > 1)
                    {
                        appraisal.ChromeStyleId = result[0];
                        appraisal.SelectedTrim = result[1];
                    }
                }
                else if (!String.IsNullOrEmpty(appraisal.CusTrim))
                {
                    appraisal.SelectedTrim = appraisal.CusTrim;
                    appraisal.ChromeStyleId = null;
                }

                appraisal.SelectedPackageOptions = additionalPackages;

                appraisal.SelectedFactoryOptions = additionalOptions;


                appraisal.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VehicleId, appraisal.VinNumber,
                                  dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);

              
                appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt64(appraisal.Mileage), dealer.State);


                appraisal.IsTruck = true;

                if (!String.IsNullOrEmpty(appraisal.SelectedMake))
                {
                    if (appraisal.SelectedMake.Contains("|"))
                        appraisal.Make =
                            appraisal.SelectedMake.Substring(
                                appraisal.SelectedMake.IndexOf("|", System.StringComparison.Ordinal) + 1);
                }
                if (!String.IsNullOrEmpty(appraisal.SelectedModel))
                {
                    if (appraisal.SelectedModel.Contains("|"))
                        appraisal.AppraisalModel =
                            appraisal.SelectedModel.Substring(
                                appraisal.SelectedModel.IndexOf("|", System.StringComparison.Ordinal) + 1);
                }


                var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);
                return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.AppraisalId });


            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewProfileByYearBlank(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                string additionalPackages = form["txtFactoryPackageOption"];

                string additionalOptions = form["txtNonInstalledOption"];


                if (!String.IsNullOrEmpty(additionalPackages))
                {
                    string[] filterOptions = additionalPackages.Split(new string[] { ",", "$", "." },
                                                                      StringSplitOptions.RemoveEmptyEntries);
                    additionalPackages = "";
                    if (filterOptions.Any())
                    {
                        foreach (string tmp in filterOptions)
                        {
                            int number = 0;
                            bool flag = int.TryParse(tmp, out number);
                            if (!flag)
                                additionalPackages += tmp.Trim() + ",";
                        }
                    }
                    if (!String.IsNullOrEmpty(additionalPackages))
                    {
                        if (additionalPackages.Substring(additionalPackages.Length - 1).Equals(","))
                            additionalPackages = additionalPackages.Substring(0, additionalPackages.Length - 1).Trim();
                    }
                }

                if (!String.IsNullOrEmpty(additionalOptions))
                {
                    string[] filterOptions = additionalOptions.Split(new string[] { ",", "$", "." },
                                                                     StringSplitOptions.RemoveEmptyEntries);
                    additionalOptions = "";
                    if (filterOptions.Any())
                    {
                        foreach (string tmp in filterOptions)
                        {
                            int number = 0;
                            bool flag = int.TryParse(tmp, out number);
                            if (!flag)
                                additionalOptions += tmp.Trim() + ",";
                        }
                    }
                    if (!String.IsNullOrEmpty(additionalOptions))
                    {
                        if (additionalOptions.Substring(additionalOptions.Length - 1).Equals(","))
                            additionalOptions = additionalOptions.Substring(0, additionalOptions.Length - 1).Trim();
                    }
                }

                if (!String.IsNullOrEmpty(appraisal.SelectedTrim) && appraisal.SelectedTrim.Contains("|"))
                {
                    var result = appraisal.SelectedTrim.Split('|');
                    if (result.Count() > 1)
                    {
                        appraisal.ChromeStyleId = result[0];
                        appraisal.SelectedTrim = result[1];
                    }
                }
                else if (!String.IsNullOrEmpty(appraisal.CusTrim))
                {
                    appraisal.SelectedTrim = appraisal.CusTrim;
                    appraisal.ChromeStyleId = null;
                }

                appraisal.SelectedPackageOptions = additionalPackages;

                appraisal.SelectedFactoryOptions = additionalOptions;

                if (!String.IsNullOrEmpty(appraisal.SelectedMake) && appraisal.SelectedMake.Contains("|"))
                {
                    int index = appraisal.SelectedMake.IndexOf("|", System.StringComparison.Ordinal);
                    appraisal.Make = appraisal.SelectedMake.Substring(index + 1);

                }


                appraisal.ModelYear = Convert.ToInt32(appraisal.SelectedModelYear);

                appraisal.AppraisalModel = appraisal.SelectedModel;

                appraisal.TrimList = new List<SelectListItem>().AsEnumerable();

                appraisal.ExteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.InteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.DefaultImageUrl = dealer.DealerSetting.DefaultStockImageUrl;

                appraisal.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VehicleId, appraisal.VinNumber,
                     dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);

                //appraisal.KBB = KellyBlueBookHelper.GetFullReport(appraisal.VinNumber, dealer.ZipCode, appraisal.Mileage);

                appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt64(appraisal.Mileage), dealer.State);

                appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;

                var insertedAppraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                appraisal.AppraisalGenerateId = insertedAppraisal.AppraisalId.ToString();

                // include Manheim Wholesales values
                try
                {

                    var manheimCredential = VincontrolLinqHelper.GetManheimCredential(dealer.DealershipId);
                    if (manheimCredential != null)
                        appraisal.ManheimWholesales = VincontrolLinqHelper.ManheimReportForAppraisal(insertedAppraisal,
                                                                                           manheimCredential.Manheim
                                                                                                            .Trim(),
                                                                                           manheimCredential
                                                                                               .ManheimPassword.Trim());
                    else
                        appraisal.ManheimWholesales = new List<ManheimWholesaleViewModel>();


                }
                catch (Exception)
                {
                    appraisal.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                }

                return View("Profile", appraisal);
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewProfileByManual(AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                appraisal.ModelYear = Convert.ToInt32(appraisal.SelectedModelYear);

                appraisal.Make = appraisal.SelectedMake;

                appraisal.AppraisalModel = appraisal.SelectedModel;

                appraisal.TrimList = new List<SelectListItem>().AsEnumerable();

                appraisal.ExteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.InteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.DefaultImageUrl = dealer.DealerSetting.DefaultStockImageUrl;

                if (!String.IsNullOrEmpty(appraisal.VinNumber))
                {

                    appraisal.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VehicleId, appraisal.VinNumber,
                      dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);

                    //appraisal.KBB = KellyBlueBookHelper.GetFullReport(appraisal.VinNumber, dealer.ZipCode, appraisal.Mileage);

                    appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt32(appraisal.Mileage), dealer.State);
                }

                var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.AppraisalModel + " " +
                               appraisal.Trim;

                appraisal.AppraisalGenerateId = insertedappraisal.AppraisalId.ToString(CultureInfo.InvariantCulture);
                appraisal.Location = insertedappraisal.Location;
                return GetServiceInfo(insertedappraisal, dealer, appraisal);

            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewProfileByManualForTruck(AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                appraisal.ModelYear = Convert.ToInt32(appraisal.SelectedModelYear);

                appraisal.Make = appraisal.SelectedMake;

                appraisal.AppraisalModel = appraisal.SelectedModel;

                appraisal.TrimList = new List<SelectListItem>().AsEnumerable();

                appraisal.ExteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.InteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.DefaultImageUrl = dealer.DealerSetting.DefaultStockImageUrl;


                appraisal.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VehicleId, appraisal.VinNumber,
                    dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);

               

                appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt32(appraisal.Mileage), dealer.State);

                appraisal.IsTruck = true;
                appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;
                return View("TruckProfile", appraisal);
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddToInventory(AppraisalViewFormModel appraisal)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                return SaveToInventory(appraisal, dealer, appraisal.InventoryStatusCodeId);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult AddToInventoryNew(int appraisalID, string stock, short inventoryStatusCodeId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var appraisal = _appraisalManagementForm.GetAppraisal(appraisalID);

                var newListingId = SQLHelper.InsertToInvetory(appraisal.AppraisalId, stock, inventoryStatusCodeId,
                    dealer);

                return Json(new {success = true, listingid = newListingId});

            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult CheckAppaisalInInventory(string vin, int appraisalId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                using (var context = new VincontrolEntities())
                {
                    if (!String.IsNullOrEmpty(vin) && context.Inventories.Any(x => x.DealerId == dealer.DealershipId && x.Vehicle.Vin == vin))
                    {
                        int listingId =
                            context.Inventories.First(
                                x => x.DealerId == dealer.DealershipId && x.Vehicle.Vin == vin).
                                InventoryId;
                        return Json(new { sussess = true, isExist = true, url = Url.Action("ViewIProfile", "Inventory", new { ListingID = listingId }) });
                    }
                    else if (context.Inventories.Any(
                            x => x.DealerId == dealer.DealershipId && x.AppraisalId == appraisalId))
                    {
                        int listingId =
                            context.Inventories.First(
                                x => x.DealerId == dealer.DealershipId && x.AppraisalId == appraisalId).InventoryId;
                        return Json(new { sussess = true, isExist = true, url = Url.Action("ViewIProfile", "Inventory", new { ListingID = listingId }) });
                    }
                    else
                    {
                        return Json(new { sussess = true, isExist = false });
                    }
                }
            }
            else
            {
                return Json(new { sussess = false });
            }
        }

        public ActionResult CheckAppaisalInSoldOutInventory(string vin, int appraisalId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
                using (var context = new VincontrolEntities())
                {
                    if (!String.IsNullOrEmpty(vin) && context.SoldoutInventories.Any(x => x.DealerId == dealer.DealershipId && x.Vehicle.Vin == vin))
                    {
                        int listingId =
                            context.SoldoutInventories.First(
                                x => x.DealerId == dealer.DealershipId && x.Vehicle.Vin == vin).
                                SoldoutInventoryId;
                        return Json(new { sussess = true, isExist = true, url = Url.Action("ViewISoldProfile", "Inventory", new { ListingID = listingId }) });
                    }
                    else if (context.SoldoutInventories.Any(
                            x => x.DealerId == dealer.DealershipId && x.AppraisalId == appraisalId))
                    {
                        int listingId =
                            context.SoldoutInventories.First(
                                x => x.DealerId == dealer.DealershipId && x.AppraisalId == appraisalId).SoldoutInventoryId;
                        return Json(new { sussess = true, isExist = true, url = Url.Action("ViewISoldProfile", "Inventory", new { ListingID = listingId }) });
                    }
                    else
                    {
                        return Json(new { sussess = true, isExist = false });
                    }
                }
            }
            else
            {
                return Json(new { sussess = false });
            }
        }

        public ActionResult PopUpStock()
        {
            return View();
        }

        public ActionResult PopUpStockList(int appraisalID)
        {
            ViewData["appraisalID"] = appraisalID;
            return View("PopUpStock");
        }

        public ActionResult PopUpViewDetail(string url, string message)
        {
            ViewData["Url"] = url;
            if (message.Equals("Sold Out"))
                ViewData["Message"] = "as a sold car";
            else
            {
                ViewData["Message"] = "in Inventory";
            }
            return View();
        }

        private ActionResult SaveToInventory(AppraisalViewFormModel appraisal, DealershipViewModel dealer, short inventoryStatusCodeId)
        {
            using (var context = new VincontrolEntities())
            {
                if (!String.IsNullOrEmpty(appraisal.VinNumber) && context.Inventories.Any(x => x.DealerId == dealer.DealershipId && x.Vehicle.Vin == appraisal.VinNumber))
                {
                    int listingId =
                        context.Inventories.First(
                            x => x.DealerId == dealer.DealershipId && x.Vehicle.Vin == appraisal.VinNumber).
                            InventoryId;
                    return RedirectToAction("ViewIProfile", "Inventory", new { ListingID = listingId });
                }
                else
                {
                    var id = Convert.ToInt32(appraisal.AppraisalGenerateId);

                    var newListingId= SQLHelper.InsertToInvetory(id, appraisal.StockNumber, inventoryStatusCodeId, dealer);

                    return RedirectToAction("ViewIProfile", "Inventory", new { ListingID = newListingId });

                }
            }
        }

       
        public ActionResult ViewProfileForCustomerAppraisal(int customerId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var appraisal = ConvertHelper.ConvertDataRowToCustomerApppraisal(customerId, dealer);

                appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " +
                                           appraisal.AppraisalModel;

                appraisal.AppraisalGenerateId = customerId.ToString(CultureInfo.InvariantCulture);

                appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;

                return View("SavedCustomerProfile", appraisal);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

       

        private ActionResult GetServiceInfoOnMobile(Appraisal insertedAppraisal, DealershipViewModel dealer, AppraisalViewFormModel appraisal)
        {
            var context = new VinMarketEntities();

            if (String.IsNullOrEmpty(insertedAppraisal.Vehicle.Vin))
            {
                var query = MapperFactory.GetMarketCarQuery(context, insertedAppraisal.Vehicle.Year, false);

                var sampleCar = DataHelper.GetNationwideMarketData(query, insertedAppraisal.Vehicle.Make, insertedAppraisal.Vehicle.Model, insertedAppraisal.Vehicle.Trim, false).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));

                if (sampleCar != null)
                    appraisal.SampleVin = sampleCar.Vin;
            }

            try
            {
                appraisal.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.VehicleId, appraisal.VinNumber,
                   dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);
            }
            catch (Exception)
            {

            }

            appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, Convert.ToInt64(appraisal.Mileage), dealer.State);

            appraisal.CarFaxDealerId = dealer.DealerSetting.CarFax;

     

            return View("SavedProfileOnMobile", appraisal);
        }

 

        [CompressFilter(Order = 1)]
        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewAppraisal(int? type)
        {
            if (!SessionHandler.UserRight.Appraisals.Recent)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

            if (SessionHandler.Dealer != null)
            {
                var viewModel = new AppraisalListViewModel();
                return View("ViewAppraisal", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }
        
     

        public string SortAppraisalListJson(int condition, bool isUp, string fromDate, string toDate, int pageIndex = 1, int pageSize = 50)
        {
            var viewModel = new AppraisalListViewModel()
            {
                UnlimitedAppraisals = new List<AppraisalViewFormModel>()
            };

          
            DateTime dtFromDate = DataHelper.ParseDateTimeFromString(fromDate, true);
            DateTime dtToDate = DataHelper.ParseDateTimeFromString(toDate, false);

            IQueryable<Appraisal> appraisalList;
            if (SessionHandler.IsEmployee)
            {
                appraisalList =
                   _appraisalManagementForm.GetActiveAppraisalsByUserInDateRange(SessionHandler.CurrentUser.UserId,
                       dtFromDate, dtToDate);
            }
            else
            {
                if (!SessionHandler.Single)
                {
                    if (SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
                    {
                        appraisalList = _appraisalManagementForm.GetActiveAppraisalsInDateRange(
                            SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId),
                            dtFromDate, dtToDate);
                    }
                    else
                    {
                        appraisalList =
                       _appraisalManagementForm.GetActiveAppraisalsInDateRange(SessionHandler.Dealer.DealershipId,
                           dtFromDate, dtToDate);
                    }
                }
                else
                {
                    appraisalList =
                        _appraisalManagementForm.GetActiveAppraisalsInDateRange(SessionHandler.Dealer.DealershipId,
                            dtFromDate, dtToDate);
                }


            }


            viewModel.NumberOfRecords = appraisalList.Count();
            SortInternalAppraisalList(ref appraisalList, condition, isUp);

            foreach (var row in appraisalList.Skip((pageIndex - 1) * pageSize).Take(pageSize))
            {
                var appraisalTmp = new AppraisalViewFormModel(row);
                appraisalTmp.ShortAppraisalBy = appraisalTmp.AppraisalBy.Length > 30
                                            ? string.Format("{0}...", appraisalTmp.AppraisalBy.Substring(0, 29))
                                            : appraisalTmp.AppraisalBy;
                viewModel.UnlimitedAppraisals.Add(appraisalTmp);
                
            }

            var js = new JavaScriptSerializer();
            string str = js.Serialize(viewModel);
            return (str);
        }

      

        public string SortPendingAppraisalListJson(int condition, bool isUp, int pageIndex = 1, int pageSize = 50)
        {
            var viewModel = new AppraisalListViewModel()
            {
                UnlimitedAppraisals = new List<AppraisalViewFormModel>()
            };

           
            IQueryable<Appraisal> appraisalList;

            if (SessionHandler.IsEmployee)
            {
                appraisalList =
                    _appraisalManagementForm.GetPendingAppraisalsByUser(SessionHandler.CurrentUser.UserId);
            }
            else
            {
                if (!SessionHandler.Single)
                {
                    if (SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
                    {
                        appraisalList = _appraisalManagementForm.GetPendingAppraisals(
                            SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId));
                    }
                    else
                    {
                        appraisalList = _appraisalManagementForm.GetPendingAppraisals(SessionHandler.Dealer.DealershipId);
                    }
                }
                else
                {
                    appraisalList = _appraisalManagementForm.GetPendingAppraisals(SessionHandler.Dealer.DealershipId);
                }


            }

            viewModel.NumberOfRecords = appraisalList.Count();
            SortInternalAppraisalList(ref appraisalList, condition, isUp);

            foreach (var row in appraisalList.Skip((pageIndex - 1) * pageSize).Take(pageSize))
            {
                var appraisalTmp = new AppraisalViewFormModel(row);
                appraisalTmp.ShortAppraisalBy = appraisalTmp.AppraisalBy.Length > 30
                                            ? string.Format("{0}...", appraisalTmp.AppraisalBy.Substring(0, 29))
                                            : appraisalTmp.AppraisalBy;
                viewModel.UnlimitedAppraisals.Add(appraisalTmp);

            }

            var js = new JavaScriptSerializer();
            string str = js.Serialize(viewModel);
            return (str);
        }

        private void SortInternalAppraisalList(ref IQueryable<Appraisal> appraisalList, int condition, bool isUp)
        {
            switch (condition)
            {
                case Constanst.SortOption.AppraisalDate:
                    appraisalList = appraisalList.OrderByDescending(x => x.AppraisalDate.Value).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Year:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Make:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Model:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Trim:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.Vehicle.Trim).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.Vehicle.Trim).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model);
                    break;
                case Constanst.SortOption.Vin:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.Vehicle.Vin).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.Vehicle.Vin).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Color:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.ExteriorColor).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.ExteriorColor).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Owner:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.CARFAXOwner).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.CARFAXOwner).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Mileage:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.Mileage).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.Mileage).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
                case Constanst.SortOption.Price:
                    if (isUp)
                        appraisalList = appraisalList.OrderBy(x => x.ACV).ThenBy(x => x.Vehicle.Year).ThenBy(x => x.Vehicle.Make).ThenBy(x => x.Vehicle.Model).ThenBy(x => x.Vehicle.Trim);
                    else
                        appraisalList = appraisalList.OrderByDescending(x => x.ACV).ThenByDescending(x => x.Vehicle.Year).ThenByDescending(x => x.Vehicle.Make).ThenByDescending(x => x.Vehicle.Model).ThenByDescending(x => x.Vehicle.Trim);
                    break;
            }
        }

        public ActionResult UpdateAcv(int appraisalId, string acv)
        {
            acv = CommonHelper.RemoveSpecialCharactersForPurePrice(acv);

            decimal? acvNumber = null;
            if (!string.IsNullOrEmpty(acv))
                acvNumber = Convert.ToDecimal(acv);

            _appraisalManagementForm.UpdateAcv(appraisalId,acvNumber.GetValueOrDefault());
            

            if (Request.IsAjaxRequest())
            {
                return Json(acv);

            }
            return Json(appraisalId + " NOT UPDATED " + acv);

        }

        public ActionResult UpdateMileage(int appraisalId, string mileage)
        {
            var mileageNumber = CommonHelper.RemoveSpecialCharactersAndReturnNumber(mileage);


            _appraisalManagementForm.UpdateMileage(appraisalId, mileageNumber);

            if (Request.IsAjaxRequest())
            {
                return Json(mileage);

            }
            return Json(appraisalId + " NOT UPDATED " + mileage);

        }

      
        public ActionResult EditAppraisal(int appraisalId)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var dealer = SessionHandler.Dealer;

           
            var appraisal = _appraisalManagementForm.GetAppraisal(appraisalId);

            Session["AllowSave"] = true;

            if (appraisal != null && (SessionHandler.IsEmployee && appraisal.AppraisalById != SessionHandler.CurrentUser.UserId))
            {
                Session["AllowSave"] = false;
            }

            if (SessionHelper.AllowToAccessAppraisal(appraisal) == false)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

            var row = new AppraisalViewFormModel(appraisal);
            var autoService = new ChromeAutoService();

            return !String.IsNullOrEmpty(row.VinNumber) && !row.IsManualDecode ? ReDecodeAppraisalWithVin(dealer, row, autoService) : ReDecodeAppraisalWithStyle(dealer, row);
        }

        private ActionResult ReDecodeAppraisalWithStyle(DealershipViewModel dealer, AppraisalViewFormModel row)
        {
            var autoService = new ChromeAutoService();
            var vehicleInfo = new VehicleDescription();
            IdentifiedString[] styleArray;
            row.DealershipId = dealer.DealershipId;

            try
            {


                if (!String.IsNullOrEmpty(row.ChromeStyleId))
                {
                    vehicleInfo = autoService.GetStyleInformationFromStyleId(Convert.ToInt32(row.ChromeStyleId));

                    styleArray = autoService.GetTrims(row.ModelYear, row.Make, row.Model);
                }
                else
                {
                    int chromeModelId;
                    Int32.TryParse(row.ChromeModelId, out chromeModelId);

                    styleArray = autoService.GetTrims(row.ModelYear, row.Make, row.Model);

                    if (row.SelectedTrim != null && row.SelectedTrim.Equals(string.Empty))
                    {
                        if (styleArray != null)
                            vehicleInfo = autoService.GetStyleInformationFromStyleId(styleArray.First().id);
                    }
                }

                row.TrimList = autoService.InitalTrimList(styleArray, row.Trim);
                if (row.TrimList!=null)
                {
                    row.SelectedTrim = row.TrimList.Any(i => i.Selected)? row.TrimList.First(i => i.Selected).Value
                        : string.Empty;
                }


                if (row.SelectedTrim.Contains("Base/Other Trims"))
                    row.CusTrim = row.Trim;
                if (!String.IsNullOrEmpty(row.Trim) && String.IsNullOrEmpty(row.ChromeStyleId))
                {
                    if (!row.TrimList.Any(i => i.Text.Equals(row.Trim))) row.CusTrim = row.Trim;
                }
                if (vehicleInfo != null)
                {
                    row.ExteriorColorList = autoService.InitalExteriorColorList(vehicleInfo.exteriorColor);
                    row.InteriorColorList = autoService.InitalInteriorColorList(vehicleInfo.interiorColor);
                    var listPackageOptions = autoService.GetPackageOptions(vehicleInfo);
                    var listNonInstalledOptions = autoService.GetNonInstalledOptions(vehicleInfo);
                    row.FactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(listPackageOptions);
                    row.FactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(listNonInstalledOptions);
                    if (vehicleInfo.engine != null)
                    {
                        row.FuelList = autoService.InitialFuelList(vehicleInfo.engine);
                        row.CylinderList = autoService.InitialCylinderList(vehicleInfo.engine);
                        row.LitersList = autoService.InitialLitterList(vehicleInfo.engine);

                        var firstEngine = vehicleInfo.engine.FirstOrDefault();
                        if (firstEngine != null && firstEngine.fuelEconomy != null)
                        {
                            row.FuelEconomyCity = firstEngine.fuelEconomy.city.low.ToString();
                            row.FuelEconomyHighWay = firstEngine.fuelEconomy.hwy.low.ToString();
                        }
                    }

                    var builderOption = new StringBuilder();

                    if ( vehicleInfo.genericEquipment != null)
                    {

                        foreach (
                            var sd in
                                vehicleInfo.genericEquipment.Where(x => x.installed != null))
                        {
                            var category = (CategoryDefinition)sd.Item;
                            builderOption.Append(category.category.Value + ",");

                        }

                        if (String.IsNullOrEmpty(builderOption.ToString()))
                            row.StandardOptions = "";
                        else
                        {
                            builderOption.Remove(builderOption.Length - 1, 1);
                            row.StandardOptions = builderOption.ToString().Replace("\'", "\\'");
                        }

                    }

                }
                else
                {
                    row.ExteriorColorList = autoService.InitalExteriorColorList(null);
                    row.InteriorColorList = autoService.InitalInteriorColorList(null);
                    row.FactoryPackageOptions = SelectListHelper.InitalFactoryPackagesOrOption(null);
                    row.FactoryNonInstalledOptions = SelectListHelper.InitalFactoryPackagesOrOption(null);
                }


                // Other exterior color
                row.CusExteriorColor = String.IsNullOrEmpty(row.SelectedExteriorColorCode) ||
                                       row.SelectedExteriorColorCode.Equals("Other Colors")
                    ? row.SelectedExteriorColorValue
                    : String.Empty;
                row.SelectedExteriorColorValue = !String.IsNullOrEmpty(row.CusExteriorColor)
                    ? "Other Colors"
                    : row.SelectedExteriorColorValue;
                // Other interior color
                var list = row.InteriorColorList.Where(t => t.Text.Equals(row.SelectedInteriorColor));
                if (!list.Any())
                {
                    row.CusInteriorColor = row.InteriorColor ?? string.Empty;
                }
                else row.CusInteriorColor = string.Empty;
               
                row.SelectedInteriorColor = !String.IsNullOrEmpty(row.CusInteriorColor)
                    ? "Other Colors"
                    : row.SelectedInteriorColor;


                row.BodyTypeList = autoService.InitialBodyTypeList(row.SelectedBodyType);

                row.DriveTrainList = SelectListHelper.InitalEditDriveTrainList(row.WheelDrive);

                row.ExistOptions = String.IsNullOrEmpty(row.SelectedFactoryOptions)
                    ? null
                    : (from data in CommonHelper.GetArrayFromString(row.SelectedFactoryOptions) select data).ToList();

                row.ExistPackages = String.IsNullOrEmpty(row.SelectedPackageOptions)
                    ? null
                    : (from data in CommonHelper.GetArrayFromString(row.SelectedPackageOptions) select data).ToList();

             
            }
            catch (Exception ex)
            {
              
            }

            row.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

            row.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

            row.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

            row.VehicleTypeList = SelectListHelper.InitalVehicleTypeList(row.VehicleType);

            row.VinDecodeSuccess = false;

            return View("EditAppraisal", row);
        }

        private ActionResult ReDecodeAppraisalWithVin(DealershipViewModel dealer, AppraisalViewFormModel row, ChromeAutoService autoService)
        {
            var vehicleInfo = autoService.GetVehicleInformationFromVin(row.VinNumber);

            VehicleDescription styleInfo = null;

            if (!String.IsNullOrEmpty(row.ChromeStyleId))
            {
                int chromeStyleId;

                Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);
            }
            else
            {
                if (vehicleInfo.style != null)
                {
                    var element = vehicleInfo.style.FirstOrDefault(x => x.trim == row.Trim);
                    styleInfo = autoService.GetStyleInformationFromStyleId(element != null ? element.id : vehicleInfo.style.First().id);
                }
            }

            if (vehicleInfo != null && vehicleInfo.style != null)
            {

                var viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);

                viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, true);
                
                viewModel.TruckType = row.TruckType;

                viewModel.SelectedTruckType = row.SelectedTruckType;

                viewModel.TruckClass = row.TruckClass;

                viewModel.SelectedTruckClassId = row.SelectedTruckClassId;

                viewModel.TruckCategory = row.TruckCategory;

                viewModel.SelectedTruckCategoryId = row.SelectedTruckCategoryId;

                viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

                viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

                viewModel.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

                viewModel.VinDecodeSuccess = true;

                return View("EditAppraisal", viewModel);
            }
            else
            {
                var viewModel = new AppraisalViewFormModel { AppraisalGenerateId = row.AppraisalID.ToString(CultureInfo.InvariantCulture) };

                viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, false);

                viewModel.TruckType = row.TruckType;

                viewModel.SelectedTruckType = row.SelectedTruckType;

                viewModel.TruckClass = row.TruckClass;

                viewModel.SelectedTruckClassId = row.SelectedTruckClassId;

                viewModel.TruckCategory = row.TruckCategory;

                viewModel.SelectedTruckCategoryId = row.SelectedTruckCategoryId;

                viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

                viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

                viewModel.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

                viewModel.VinDecodeSuccess = false;

                return View("EditAppraisal", viewModel);
            }
        }

        public ActionResult EditAppraisalForTruck(int appraisalId)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = SessionHandler.Dealer;

                var row = ConvertHelper.GetAppraisalModelFromAppriaslId(appraisalId);

                var autoService = new ChromeAutoService();

                if (!String.IsNullOrEmpty(row.VinNumber))
                {
                    var vehicleInfo = autoService.GetVehicleInformationFromVin(row.VinNumber);
                    VehicleDescription styleInfo = null;

                    if (!String.IsNullOrEmpty(row.ChromeStyleId))
                    {
                        int chromeStyleId;

                        Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                        styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);
                    }
                    else
                    {
                        var element = vehicleInfo.style.Where(x => x.trim == row.Trim).FirstOrDefault();
                        styleInfo = autoService.GetStyleInformationFromStyleId(element != null ? element.id : vehicleInfo.style.First().id);
                    }

                    if (vehicleInfo != null)
                    {
                        var viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);

                        viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, true);

                        viewModel.TruckType = row.TruckType;

                        viewModel.SelectedTruckType = row.SelectedTruckType;

                        viewModel.TruckClass = row.TruckClass;

                        viewModel.SelectedTruckClassId = row.SelectedTruckClassId;

                        viewModel.TruckCategory = row.TruckCategory;

                        viewModel.SelectedTruckCategoryId = row.SelectedTruckCategoryId;

                        viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

                        viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

                        viewModel.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

                        viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        viewModel.IsTruck = true;

                        return View("EditAppraisalForTruck", viewModel);
                    }
                    else
                    {
                        var viewModel = new AppraisalViewFormModel();

                        viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, false);

                        viewModel.TruckType = row.TruckType;

                        viewModel.SelectedTruckType = row.SelectedTruckType;

                        viewModel.TruckClass = row.TruckClass;

                        viewModel.SelectedTruckClassId = row.SelectedTruckClassId;

                        viewModel.TruckCategory = row.TruckCategory;

                        viewModel.SelectedTruckCategoryId = row.SelectedTruckCategoryId;

                        viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.SelectedTruckType);

                        viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.SelectedTruckType);

                        viewModel.TruckClassList = _commonManagementForm.GetTruckClasses(row.SelectedTruckClassId);

                        viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        viewModel.IsTruck = true;

                        return View("EditAppraisalForTruck", viewModel);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(row.ChromeStyleId))
                    {
                        int chromeModelId; Int32.TryParse(row.ChromeModelId, out chromeModelId);

                        int chromeStyleId; Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                        var styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);

                        var styleArray = autoService.GetStyles(Convert.ToInt32(chromeModelId));

                        var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                        appraisal = ConvertHelper.UpdateSuccessfulAppraisalModelWithoutVin(appraisal, row, styleInfo, dealer.DealershipId, row.Location, false);

                        appraisal.ChromeModelId = chromeModelId.ToString();

                        appraisal.ChromeStyleId = chromeStyleId.ToString();

                        appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                        if (!String.IsNullOrEmpty(appraisal.SelectedTrim))
                        {
                            var selectedTrim = appraisal.TrimList.Where(x => x.Value.Contains(appraisal.SelectedTrim));

                            if (selectedTrim != null)
                            {
                                var extractTrimList = appraisal.TrimList.Where(x => !x.Value.Contains(appraisal.SelectedTrim));

                                var listItem = new List<ExtendedSelectListItem> { selectedTrim.First() };

                                listItem.AddRange(extractTrimList);

                                appraisal.TrimList = listItem.AsEnumerable();
                            }
                        }

                        if (appraisal.ExteriorColorList.Any())
                        {
                            var selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Value.Equals(appraisal.SelectedExteriorColorValue.Trim()));

                            ExtendedSelectListItem selectedFirstOrDefault;

                            if (selectedExteriorColor != null && selectedExteriorColor.Any())
                            {
                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Value.Equals(appraisal.SelectedExteriorColorValue.Trim()));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractExteriorColorList);

                                appraisal.ExteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractExteriorColorList);

                                appraisal.ExteriorColorList = listItem.AsEnumerable();

                                appraisal.CusExteriorColor = appraisal.SelectedExteriorColorValue.Trim();
                            }
                        }

                        if (appraisal.InteriorColorList.Any())
                        {
                            var selectedInteriorColor = appraisal.InteriorColorList.Where(x => x.Value.Equals(appraisal.SelectedInteriorColor));

                            SelectListItem selectedFirstOrDefault;

                            if (selectedInteriorColor != null && selectedInteriorColor.Any())
                            {
                                selectedFirstOrDefault = selectedInteriorColor.First();

                                var extractInteriorColorList = appraisal.InteriorColorList.Where(x => !x.Value.Equals(appraisal.SelectedInteriorColor));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedInteriorColor = appraisal.InteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedInteriorColor.First();

                                var extractInteriorColorList = appraisal.InteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();

                                appraisal.CusInteriorColor = appraisal.SelectedInteriorColor;
                            }
                        }

                        appraisal.SalePrice = appraisal.MSRP;

                        if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                            appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                        appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                        appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                        appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        appraisal.IsTruck = true;

                        return View("EditAppraisalForTruck", appraisal);
                    }
                    else
                    {
                        int chromeModelId;
                        Int32.TryParse(row.ChromeModelId, out chromeModelId);

                        var styleArray = autoService.GetStyles(chromeModelId);

                        var styleInfo = autoService.GetStyleInformationFromStyleId(styleArray.First().id);

                        var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                        appraisal = ConvertHelper.UpdateSuccessfulAppraisalModelWithoutVin(appraisal, row, styleInfo, dealer.DealershipId, row.Location, false);

                        appraisal.ChromeModelId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                        appraisal.ChromeStyleId = styleArray.First().id.ToString(CultureInfo.InvariantCulture);

                        appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                        if (!String.IsNullOrEmpty(appraisal.SelectedTrim) && appraisal.TrimList.Any())
                        {
                            var selectedTrim = appraisal.TrimList.First(x => x.Value.Contains(appraisal.SelectedTrim));

                            var extractTrimList = appraisal.TrimList.Where(x => !x.Value.Contains(appraisal.SelectedTrim));

                            var listItem = new List<SelectListItem> { selectedTrim };

                            listItem.AddRange(extractTrimList);

                            appraisal.TrimList = listItem.AsEnumerable();
                        }

                        if (appraisal.ExteriorColorList.Any())
                        {
                            var selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Text.Equals(appraisal.SelectedExteriorColorValue.Trim()));

                            SelectListItem selectedFirstOrDefault;

                            if (selectedExteriorColor != null && selectedExteriorColor.Any())
                            {
                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Text.Equals(appraisal.SelectedExteriorColorValue.Trim()));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractExteriorColorList);

                                appraisal.ExteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractExteriorColorList);

                                appraisal.ExteriorColorList = listItem.AsEnumerable();

                                appraisal.CusExteriorColor = appraisal.SelectedExteriorColorValue.Trim();
                            }
                        }

                        if (appraisal.InteriorColorList.Any())
                        {
                            var selectedInteriorColor = appraisal.InteriorColorList.Where(x => x.Value.Equals(appraisal.SelectedInteriorColor));

                            SelectListItem selectedFirstOrDefault;

                            if (selectedInteriorColor != null && selectedInteriorColor.Any())
                            {
                                selectedFirstOrDefault = selectedInteriorColor.First();

                                var extractInteriorColorList = appraisal.InteriorColorList.Where(x => !x.Value.Equals(appraisal.SelectedInteriorColor));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedInteriorColor = appraisal.InteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedInteriorColor.First();

                                var extractInteriorColorList = appraisal.InteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> { selectedFirstOrDefault };

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();

                                appraisal.CusInteriorColor = appraisal.SelectedInteriorColor;
                            }
                        }

                        appraisal.SalePrice = appraisal.MSRP;

                        if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                            appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                        appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                        appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                        appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        appraisal.IsTruck = true;

                        return View("EditAppraisalForTruck", appraisal);
                    }
                }
            }
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveEditAppraisal(FormCollection form, AppraisalViewFormModel appraisal)
        {
            appraisal.SelectedTrim = appraisal.SelectedTrimItem;
            return SaveAppraisalInfo(form, appraisal, appraisal.SelectedVehicleType.Equals("Truck"));
        }

        private ActionResult SaveAppraisalInfo(FormCollection form, AppraisalViewFormModel appraisal, bool isTruck)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, SessionHandler.Dealer, HttpContext.User.Identity.Name, SessionHandler.CurrentUser.UserId);

            appraisal.DealershipZipCode = SessionHandler.Dealer.ZipCode;

            if (!isTruck)
            {
                SQLHelper.UpdateAppraisal(appraisal);
            }
            else
            {
                SQLHelper.UpdateTruckAppraisal(appraisal);
            }

            return RedirectToAction("ViewProfileForAppraisal", new { AppraisalId = appraisal.AppraisalGenerateId });
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveEditTruckAppraisal(FormCollection form, AppraisalViewFormModel appraisal)
        {
            appraisal.SelectedTrim = appraisal.SelectedTrimItem;
            return SaveAppraisalInfo(form, appraisal, true);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CancelAppraisal(AppraisalViewFormModel appraisal)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            
            return RedirectToAction("ViewProfileForAppraisal", new { AppraisalId = appraisal.AppraisalGenerateId });

        }

        public ActionResult SaveKBBOptions(int appraisalId, string OptionSelect, int TrimId, string BaseWholeSale, string WholeSale, string MileageAdjustment)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json("SessionTimeOut");

            }
            else
            {
                var dealer = SessionHandler.Dealer;

                SQLHelper.UpdateKBBOptionsForAppraisal(appraisalId, OptionSelect, TrimId, Convert.ToDecimal(BaseWholeSale), Convert.ToDecimal(WholeSale), Convert.ToDecimal(MileageAdjustment));

                if (Request.IsAjaxRequest())
                {
                    return Json("Success");

                }

            }
            return Json(appraisalId + " NOT UPDATED ");



        }

        public ActionResult CreateAppraisal(int CustomerID, string ActionName, string ID)
        {
            if (SessionHandler.Dealer != null)
            {

              
                switch (ActionName)
                {
                    case "ViewProfileForAppraisal":
                        DeleteCustomer(CustomerID);
                        return RedirectToAction("ViewProfileForAppraisal", "Appraisal", new { AppraisalId = ID });
                    case "ViewIProfile":
                        DeleteCustomer(CustomerID);
                        return RedirectToAction("ViewIProfile", "Inventory", new { ListingID = ID });
                    case "SoldOutAlert":
                        return RedirectToAction("SoldOutAlert", "Inventory", new { ListingID = ID });
                    case "InvalidVinAlert":
                        return RedirectToAction("InvalidVinAlert", "Decode", new { Vin = ID });
                    case "InvalidCustomerVinAlert":
                        return RedirectToAction("InvalidCustomerVinAlert", "Decode", new { Vin = ID });
                    default:
                    case "DecodeProcessingByVin":
                        if (!String.IsNullOrEmpty(ID))
                        {
                            return CreateAppraisalWithVIN(CustomerID, ID);
                        }
                        else
                        {
                            return RedirectToAction("InvalidVinAlert", "Decode", new { Vin = ID });
                        }
                    //return RedirectToAction("DecodeProcessingByVin", "Decode", new { Vin = ID });                     

                }

            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult RenderPhoto(string appraisalId)
        {
            using (var context = new VincontrolEntities())
            {
                int id = Convert.ToInt32(appraisalId);
                //byte[] photo = context.Appraisals.Where(a => a.AppraisalId == id).FirstOrDefault().Photo;
                //return photo != null ? File(photo, "image/jpeg") : File(new byte[] { }, "image/jpeg");
                return File(new byte[] { }, "image/jpeg");
            }
        }

        public ActionResult CreateAppraisalWithVIN(int CustomerID, string ID)
        {
            var dealer = SessionHandler.Dealer;
            AppraisalViewFormModel appraisal = DataHelper.GetAppraisalViewModel(ID);
            var context = new VincontrolEntities();
            var customer = context.TradeInCustomers.FirstOrDefault(e => e.TradeInCustomerId == CustomerID);
            if (customer != null)
            {
                appraisal.CustomerFirstName = customer.FirstName;
                appraisal.CustomerLastName = customer.LastName;

            }
            var insertedAppraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

            DeleteCustomer(CustomerID);

            return RedirectToAction("EditAppraisal", "Appraisal", new { AppraisalId = insertedAppraisal.AppraisalId });
        }

        private static void DeleteCustomer(int CustomerID)
        {
            var context = new VincontrolEntities();
            var customer = context.TradeInCustomers.FirstOrDefault(e => e.TradeInCustomerId == CustomerID);


            if (customer != null)
            {
                customer.TradeInStatus = Constanst.TradeInStatus.Deleted;
                context.SaveChanges();
            }

        }

        public ActionResult GetVehicleInformationFromStyleId(string appraisalId, string vin, string styleId, bool isTruck, string styleName, string cusStyle)
        {
            var autoService = new ChromeAutoService();
            var appraisal = new AppraisalViewFormModel() { AppraisalGenerateId = appraisalId, Trim = styleName, CusTrim = cusStyle };
            var dealer = SessionHandler.Dealer;

            var row = ConvertHelper.GetAppraisalModelFromAppriaslId(Convert.ToInt32(appraisalId));

            if (!String.IsNullOrEmpty(vin))
            {
                var vehicleInfo = autoService.GetVehicleInformationFromVin(vin, Convert.ToInt32(styleId));
                var styleInfo = autoService.GetStyleInformationFromStyleId(Convert.ToInt32(styleId));
                appraisal = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);
                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModel(appraisal, row, vehicleInfo, dealer.DealershipId, row.Location, true);
            }
            else
            {
                var styleInfo = autoService.GetStyleInformationFromStyleId(Convert.ToInt32(styleId));
                appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);
                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModelWithoutVin(appraisal, row, styleInfo, dealer.DealershipId, row.Location, false);
            }

            // VIN is null or empty
            if (string.IsNullOrEmpty(vin))
            {
                var styleArray = autoService.GetTrims(row.ModelYear, row.Make, row.AppraisalModel);
                //appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray, appraisal.Trim);
                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray, styleName);
            }
            if (!String.IsNullOrEmpty(styleName))
            {
                foreach (var item in appraisal.TrimList)
                {
                    item.Selected = item.Text.Equals(styleName);
                }

                var selectedTrim = appraisal.TrimList.FirstOrDefault(i => i.Selected);
                if (selectedTrim != null)
                {
                    appraisal.Trim = selectedTrim.Text;
                    appraisal.SelectedTrimItem = selectedTrim.Value;
                    appraisal.SelectedTrim = selectedTrim.Value;
                }
                else
                {
                    selectedTrim = appraisal.TrimList.FirstOrDefault();
                    appraisal.Trim = selectedTrim.Text;
                    appraisal.SelectedTrimItem = selectedTrim.Value;
                    appraisal.SelectedTrim = selectedTrim.Value;
                }
            }

            return PartialView("DetailAppraisal", appraisal);
        }

        #region Pending Appraisal

        public ActionResult GetPendingAppraisalNumber()
        {
            if (SessionHandler.Dealer != null)
            {
                var pendingAppraisal = _appraisalManagementForm.GetPendingAppraisals(SessionHandler.Dealer.DealershipId);
                return Content(pendingAppraisal.Count().ToString(CultureInfo.InvariantCulture));
            }
            return Content("-1");
        }

        [HttpPost]
        public ActionResult ListOfPendingAppraisals()
        {
          
            if (SessionHandler.Dealer != null)
            {
                return ShowPending(null);
            }

            return RedirectToAction("LogOff", "Account");
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewPendingAppraisal(int? type)
        {
            if (!SessionHandler.UserRight.Appraisals.Pending)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

           
            if (SessionHandler.Dealer != null)
            {
                return ShowPending(type);
            }

            return RedirectToAction("LogOff", "Account");
        }

        private ActionResult ShowPending(int? type)
        {
            var viewModel = new AppraisalListViewModel();

            return View("ViewPendingAppraisal", viewModel);
        }

     
        #endregion

        public ActionResult ViewCustomerInfo(int appraisalId)
        {
           
            var row = _appraisalManagementForm.GetAppraisal(appraisalId);
            var viewModel = new CustomeInfoModel() { AppraisalId = appraisalId, States = SelectListHelper.InitialStateList() };
            if (row.AppraisalCustomer != null)
            {
                viewModel = new CustomeInfoModel()
                                    {
                                        AppraisalId = appraisalId,
                                        FirstName = row.AppraisalCustomer.FirstName,
                                        LastName = row.AppraisalCustomer.LastName,
                                        Address = row.AppraisalCustomer.Address,
                                        ZipCode = row.AppraisalCustomer.ZipCode,
                                        City = row.AppraisalCustomer.City,
                                        State = row.AppraisalCustomer.State,
                                        States = SelectListHelper.InitialStateList(),
                                        Email = row.AppraisalCustomer.Email,
                                        Street = row.AppraisalCustomer.Street
                                    };
            }
            return View("CustomerInfo", viewModel);
          
        }

        public ActionResult ViewInSpectionFormInfo(int appraisalId)
        {
            var context = new VincontrolEntities();
            var row = _appraisalManagementForm.GetInspectionFormCost(appraisalId);
            InspectionFormCostModel viewModel;
            if (row != null)
            {
                viewModel = new InspectionFormCostModel()
                {
                    AppraisalId = appraisalId,
                    Mechanical = row.Mechanical!=null?row.Mechanical.Value:0,
                    FrontBumper = row.FrontBumper != null ? row.FrontBumper.Value : 0,
                    RearBumper = row.RearBumper != null ? row.RearBumper.Value : 0,
                    Glass = row.Glass != null ? row.Glass.Value : 0,
                    Tires = row.Tires != null ? row.Tires.Value : 0,
                    FrontEnd = row.FrontEnd != null ? row.FrontEnd.Value : 0,
                    RearEnd = row.RearEnd != null ? row.RearEnd.Value : 0,
                    DriverSide = row.DriverSide != null ? row.DriverSide.Value : 0,
                    PassengerSide = row.PassengerSide != null ? row.PassengerSide.Value : 0,
                    Interior = row.Interior != null ? row.Interior.Value : 0,
                    LightsBulbs = row.LightsBulbs != null ? row.LightsBulbs.Value : 0,
                    Other = row.Other != null ? row.Other.Value : 0,
                    LMA = row.LMA != null ? row.LMA.Value : 0
                };
            }
            else
            {
                var result = DataHelper.GetRetailForAppraisal(context, appraisalId);
                viewModel = new InspectionFormCostModel()
                {
                    AppraisalId = appraisalId,
                    Mechanical = result.MechanicalSubTotal,
                    FrontBumper = result.FrontBumperSubTotal,
                    RearBumper = result.RearBumperSubTotal,
                    Glass = result.GlassSubTotal,
                    Tires = result.TireSubTotal,
                    FrontEnd = result.FrontEndSubTotal,
                    RearEnd = result.RearEndSubTotal,
                    DriverSide = result.DriverSideSubTotal,
                    PassengerSide = result.PassengerSideSubTotal,
                    Interior = result.InteriorSubTotal,
                    LightsBulbs = result.LightBulbSubTotal,
                    Other = result.OtherSubTotal,
                    LMA = result.LMASubTotal,
                };
            }
            return View("InspectionFormInfo", viewModel);
          
        }

        public ActionResult UpdateInSpectionFormInfo(InspectionFormCostModel inspectionFormInfo)
        {
            var inspectionCost = new InspectionFormCost()
            {
                AppraisalID = inspectionFormInfo.AppraisalId,
                Mechanical = inspectionFormInfo.Mechanical,
                FrontBumper = inspectionFormInfo.FrontBumper,
                RearBumper = inspectionFormInfo.RearBumper,
                Glass = inspectionFormInfo.Glass,
                Tires = inspectionFormInfo.Tires,
                FrontEnd = inspectionFormInfo.FrontEnd,
                RearEnd = inspectionFormInfo.RearEnd,
                DriverSide = inspectionFormInfo.DriverSide,
                PassengerSide = inspectionFormInfo.PassengerSide,
                Interior = inspectionFormInfo.Interior,
                LightsBulbs = inspectionFormInfo.LightsBulbs,
                Other = inspectionFormInfo.Other,
                LMA = inspectionFormInfo.LMA,
                UpdatedDate = DateTime.Now,
                UpdatedUser = SessionHandler.CurrentUser.UserId,
            };
            _appraisalManagementForm.UpdateInspection(inspectionFormInfo.AppraisalId, SessionHandler.CurrentUser.UserId, inspectionCost);

          
            return Json("Success");
        }

        public ActionResult UpdateCustomerInfo(CustomeInfoModel customeInfo)
        {
            var customer = new AppraisalCustomer
            {
                FirstName = customeInfo.FirstName,
                LastName = customeInfo.LastName,
                Address = customeInfo.Address,
                ZipCode = customeInfo.ZipCode,
                City = customeInfo.City,
                State = customeInfo.State,
                Email = customeInfo.Email,
                Street = customeInfo.Street
            };
            _appraisalManagementForm.UpdateCustomerInfo(customeInfo.AppraisalId, customer);

          
            return Json("Success");
        }

        public ActionResult ViewCustomerInfoTradeIn(int tradeInCustomerId)
        {
            var context = new VincontrolEntities();
            var row = context.TradeInCustomers.FirstOrDefault(x => x.TradeInCustomerId == tradeInCustomerId);

            var viewModel = new CustomeInfoModel()
            {
                TradeInCustomerId = tradeInCustomerId,
                FirstName = row.FirstName,
                LastName = row.LastName,
                Address = string.Empty,
                ZipCode = string.Empty,
                City = string.Empty,
                States = SelectListHelper.InitialStateList(),
                Email = row.Email,
                Street = string.Empty
            };
            return View("CustomerInfoTradeIn", viewModel);
           
        }

        public ActionResult UpdateCustomerInfoTradeIn(CustomeInfoModel customeInfo)
        {
            var context = new VincontrolEntities();
            var row = context.TradeInCustomers.FirstOrDefault(x => x.TradeInCustomerId == customeInfo.TradeInCustomerId);
            if (row != null)
            {
                row.FirstName = customeInfo.FirstName;
                row.LastName = customeInfo.LastName;
                row.Email = customeInfo.Email;

                context.SaveChanges();
            }
         
            return Json("Success");
        }

        public ActionResult OpenStatus(int appraisalID)
        {
            if (SessionHandler.Dealer != null)
            {
                var title = string.Empty;
                const string currentStatus = "Appraisal";
                var rowAppraisal = _appraisalManagementForm.GetAppraisal(appraisalID);
                if (rowAppraisal != null)
                {
                    title =
                        string.Format("{0} {1} {2} {3}", rowAppraisal.Vehicle.Year, rowAppraisal.Vehicle.Make,
                            rowAppraisal.Vehicle.Model, rowAppraisal.Vehicle.Trim);
                }
                var viewModel = new StatusInfoModel()
                {
                    AppraisalID = appraisalID,
                    Title = title,
                    CurrentStatus = currentStatus,
                    ListStatus = SelectListHelper.InitialStatusList(0),
                    Vin = rowAppraisal.Vehicle.Vin,
                    ListingID =
                        rowAppraisal.Vehicle.Inventories.FirstOrDefault() != null
                            ? rowAppraisal.Vehicle.Inventories.FirstOrDefault().InventoryId
                            : 0
                };

                return View(viewModel);
            }
            return Json("SessionTimeOut");
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult ViewMarkSold(string appraisalID)
        {
            if (SessionHandler.Dealer != null)
            {

                var viewModel = new CustomeInfoModel
                {
                    ListingId =Convert.ToInt32( appraisalID),
                    States = SelectListHelper.InitialStateList(),
                    Countries = SelectListHelper.InitialCountryList()
                };
                return View("MarkSold", viewModel);
            }
            return Json("SessionTimeOut");
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkSold(CustomeInfoModel customer)
        {
            var dealer = SessionHandler.Dealer;

            int soldInventoryId= SQLHelper.MarkSoldAppraisal(Convert.ToInt32(customer.ListingId),  customer);

            if (Request.IsAjaxRequest())
            {
                return Json(soldInventoryId);
            }

            return Json(0);
        }

        public ActionResult OpenSilverlightUploadWindow(int appraisalId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var row = _appraisalManagementForm.GetAppraisal(appraisalId);
                if (row != null)
                {
                    var viewModel = new SilverlightImageViewModel
                    {
                        AppraisalId = appraisalId,
                        Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "" : row.Vehicle.Vin,
                        DealerId = dealer.DealershipId,
                        InventoryStatus = Constanst.VehicleStatus.Appraisal,
                        ImageServiceURL =
                            (System.Web.HttpContext.Current.Request.Url.Port == 80)
                                ? String.Format("{0}://{1}/ImageHandlers/SilverlightHandler.ashx",
                                    System.Web.HttpContext.Current.Request.Url.Scheme,
                                    System.Web.HttpContext.Current.Request.Url.Host)
                                : String.Format("{0}://{1}:{2}/ImageHandlers/SilverlightHandler.ashx",
                                    System.Web.HttpContext.Current.Request.Url.Scheme,
                                    System.Web.HttpContext.Current.Request.Url.Host,
                                    System.Web.HttpContext.Current.Request.Url.Port)
                    };

                    return View("imageSilverlightSortFrame", viewModel);
                }

            }
            else
            {
                var viewModel = new SilverlightImageViewModel {SessionTimeOut = true};
                return View("imageSilverlightSortFrame", viewModel);
            }
            return null;
        }

        #region Leo Text
        public string GetRecentAppraisalListJson(string fromDate, string toDate, int condition = 0, bool isUp = false, int pageIndex = 1, int pageSize = 50)
        {
            var viewModel = new AppraisalListViewModel()
            {
                UnlimitedAppraisals = new List<AppraisalViewFormModel>()
            };

            DateTime dtFromDate = DataHelper.ParseDateTimeFromString(fromDate, true);
            DateTime dtToDate = DataHelper.ParseDateTimeFromString(toDate, false);

            IQueryable<Appraisal> appraisalList;

            if (SessionHandler.IsEmployee)
            {
                appraisalList =
                   _appraisalManagementForm.GetActiveAppraisalsByUserInDateRange(SessionHandler.CurrentUser.UserId,
                       dtFromDate, dtToDate);
            }
            else
            {
                if (SessionHandler.AllStore)
                {
                    if (SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
                    {
                        appraisalList = _appraisalManagementForm.GetActiveAppraisalsInDateRange(
                            SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId),
                            dtFromDate, dtToDate);
                    }
                    else
                    {
                        appraisalList =
                       _appraisalManagementForm.GetActiveAppraisalsInDateRange(SessionHandler.Dealer.DealershipId,
                           dtFromDate, dtToDate);
                    }
                }
                else
                {
                    appraisalList =
                        _appraisalManagementForm.GetActiveAppraisalsInDateRange(SessionHandler.Dealer.DealershipId,
                            dtFromDate, dtToDate);
                }


            }

            viewModel.NumberOfRecords = appraisalList.Count();
            SortInternalAppraisalList(ref appraisalList, condition, isUp);

            foreach (var row in appraisalList.Skip((pageIndex - 1) * pageSize).Take(pageSize))
            {
              
                var appraisalTmp = new AppraisalViewFormModel(row);
                appraisalTmp.ShortAppraisalBy = appraisalTmp.AppraisalBy.Length > 30
                                            ? string.Format("{0}...", appraisalTmp.AppraisalBy.Substring(0, 29))
                                            : appraisalTmp.AppraisalBy;
                viewModel.UnlimitedAppraisals.Add(appraisalTmp);
            }

            var js = new JavaScriptSerializer();
            string str = js.Serialize(viewModel);
            return (str);
        }

        public string GetPendingAppraisalListJson(int condition = 0, bool isUp = false, int pageIndex = 1, int pageSize = 50)
        {
            var viewModel = new AppraisalListViewModel()
            {
                UnlimitedAppraisals = new List<AppraisalViewFormModel>()
            };
            
            IQueryable<Appraisal> appraisalList;

            if (SessionHandler.IsEmployee)
            {
                appraisalList =
                    _appraisalManagementForm.GetPendingAppraisalsByUser(SessionHandler.CurrentUser.UserId);
            }
            else
            {
                if (!SessionHandler.Single)
                {
                    if (SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
                    {
                        appraisalList = _appraisalManagementForm.GetPendingAppraisals(
                            SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId));
                    }
                    else
                    {
                        appraisalList = _appraisalManagementForm.GetPendingAppraisals(SessionHandler.Dealer.DealershipId);
                    }
                }
                else
                {
                    appraisalList = _appraisalManagementForm.GetPendingAppraisals(SessionHandler.Dealer.DealershipId);
                }


            }

            viewModel.NumberOfRecords = appraisalList.Count();
            SortInternalAppraisalList(ref appraisalList, condition, isUp);

            foreach (var row in appraisalList.Skip((pageIndex - 1) * pageSize).Take(pageSize))
            {
                var appraisalTmp = new AppraisalViewFormModel(row);
                appraisalTmp.ShortAppraisalBy = appraisalTmp.AppraisalBy.Length > 30
                                            ? string.Format("{0}...", appraisalTmp.AppraisalBy.Substring(0, 29))
                                            : appraisalTmp.AppraisalBy;
                viewModel.UnlimitedAppraisals.Add(appraisalTmp);
                
            }

            var js = new JavaScriptSerializer();
            string str = js.Serialize(viewModel);
            return (str);
        }
        #endregion
    }
}
