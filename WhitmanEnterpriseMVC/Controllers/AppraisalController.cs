using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Security;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using Newtonsoft.Json;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class AppraisalController : SecurityController
    {
        private const string PermissionCode = "APPRAISAL";
        private const string AcceptedValues = "READONLY, ALLACCESS";
        
        private void ResetSessionValue()
        {
            Session["AutoTrader"] = null;
            Session["CarsCom"] = null;
            Session["AutoTraderNationwide"] = null;
            Session["CarsComNationwide"] = null;
            SessionHandler.ChromeTrimList = null;
        }

        //[VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewProfileByVin(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (SessionHandler.Dealership != null)
            {
                var dealer = SessionHandler.Dealership;

                appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, dealer, HttpContext.User.Identity.Name);

                SessionHandler.IsNewAppraisal = true;

                if (appraisal.ModelYear > 0)
                {
                    var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                    return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.idAppraisal });
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
            if (SessionHandler.Dealership == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = SessionHandler.Dealership;

                appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, dealer, HttpContext.User.Identity.Name);

                appraisal.IsTruck = true;

                SessionHandler.IsNewAppraisal = true;
                
                if (appraisal.ModelYear > 0)
                {
                    var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                    return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.idAppraisal });
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
            if (SessionHandler.Dealership != null)
            {
                var dealer = SessionHandler.Dealership;

                appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, dealer, HttpContext.User.Identity.Name);

                appraisal.IsAppraisedByYear = true;

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

                    return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.idAppraisal });
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

        public ActionResult ViewProfileForAppraisal(int appraisalId)
        {
            ResetSessionValue();
            if (SessionHandler.Dealership != null)
            {
                var dealer = SessionHandler.Dealership;

                var context = new whitmanenterprisewarehouseEntities();

                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == appraisalId);

                if (row.VinGenie.HasValue && row.VinGenie.Value && row.Status != "Approved")
                {
                    row.Status = "Approved";
                    row.AppraisalDate = DateTime.Now;
                    row.AppraisalBy = GetAppraisalUser(dealer.DealershipId);
                    row.CarImageUrl = row.PhotoUrl;//"/Appraisal/RenderPhoto?appraisalId=" + row.AppraisalID;
                    row.DefaultImageUrl = row.CarImageUrl;
                    context.SaveChanges();
                }

                var appraisal = ConvertHelper.GetAppraisalModel(row);

                appraisal.Location = row.DealershipName;
                
           
                return GetServiceInfo(row, dealer, appraisal);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult ViewAppraisalOnMobile(string token, int appraisalId)
        {
            ResetSessionValue();
            SessionHandler.Single = true;
            Session["IsEmployee"] = false;
            int dealerId = 2299;
            try
            {
                dealerId = Convert.ToInt32(EncryptionHelper.DecryptString(token.Replace(" ", "+")).Split('|')[0]);
            }
            catch (Exception) { }

            SessionHandler.DealerId = dealerId;
            var contextVinControl = new whitmanenterprisewarehouseEntities();
            var existingDealer = contextVinControl.whitmanenterprisedealerships.FirstOrDefault(i => i.idWhitmanenterpriseDealership == dealerId);
            if (existingDealer != null)
            {
                var dealerViewModel = new DealershipViewModel()
                {
                    DealershipId = existingDealer.idWhitmanenterpriseDealership,
                    DealerGroupId = existingDealer.DealerGroupID,
                    Address = existingDealer.Address,
                    City = existingDealer.City,
                    ZipCode = existingDealer.ZipCode,
                    State = existingDealer.State ?? "CA",
                    Latitude = existingDealer.Lattitude,
                    Longtitude = existingDealer.Longtitude
                };

                Session["DealershipName"] = existingDealer.DealershipName;
                Session["Dealership"] = dealerViewModel;
            }

            var dealer = (DealershipViewModel)Session["Dealership"];
            var row = contextVinControl.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == appraisalId);

            if (row.VinGenie.HasValue && row.VinGenie.Value && row.Status != "Approved")
            {
                row.Status = "Approved";
                row.AppraisalDate = DateTime.Now;
                row.AppraisalBy = GetAppraisalUser(dealer.DealershipId);
                row.CarImageUrl = row.PhotoUrl;//"/Appraisal/RenderPhoto?appraisalId=" + row.AppraisalID;
                row.DefaultImageUrl = row.CarImageUrl;
                contextVinControl.SaveChanges();
            }

            var appraisal = ConvertHelper.GetAppraisalModel(row);

            appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " +
                                       appraisal.AppraisalModel;

            appraisal.AppraisalGenerateId = appraisalId.ToString(CultureInfo.InvariantCulture);

            appraisal.CarFaxDealerId = dealer.CarFax;
            appraisal.Location = row.Location;
            return GetServiceInfoOnMobile(row, dealer, appraisal);
        }

        public ActionResult ManheimData(string listingId)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    int convertedListingId = Convert.ToInt32(listingId);
                    var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == convertedListingId);
                    var dealer = (DealershipViewModel)Session["Dealership"];
                    var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                    if (manheimCredential != null)
                        result = LinqHelper.ManheimReportForAppraisal(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
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

        public ActionResult KarPowerData(string listingId)
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

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int convertedListingId = Convert.ToInt32(listingId);
                var dealer = (DealershipViewModel)Session["Dealership"];
                var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealer.DealershipId);
                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == convertedListingId);

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

                    if (!String.IsNullOrEmpty(row.VINNumber) && !String.IsNullOrEmpty(kbbUserName) &&
                        !String.IsNullOrEmpty(kbbPassword))
                    {
                        karpowerService.ExecuteSaveVehicleWithVin(row.VINNumber, row.Mileage, row.ExteriorColor,
                                                                  row.InteriorColor, kbbUserName, kbbPassword);
                        if (!String.IsNullOrEmpty(karpowerService.SaveVrsVehicleResult))
                        {
                            var jsonObj = (JObject)JsonConvert.DeserializeObject(karpowerService.SaveVrsVehicleResult);
                            row.KarPowerEntryId = karpowerService.ConvertToString((JValue)(jsonObj["d"]["id"]));
                            context.SaveChanges();
                        }
                    }

                    SessionHandler.IsNewAppraisal = false;
                }

                if (String.IsNullOrEmpty(row.VINNumber))
                {
                    using (var scrappingContext = new vincontrolscrappingEntities())
                    {
                        if (String.IsNullOrEmpty(row.VINNumber))
                        {

                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            var sampleCar = DataHelper.GetNationwideMarketData(query, row.Make, row.Model, row.Trim).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));

                            if (sampleCar != null)

                                row.VINNumber = sampleCar.Vin;
                        }
                    }

                }

                if (!kbbStatus.ToLower().Equals("inactive") && (setting.KellyBlueBookAccessRight != null && setting.KellyBlueBookAccessRight.Value))
                {
                    var result = new KellyBlueBookViewModel();

                    try
                    {
                        result = row.KBBTrimId == null ? KellyBlueBookHelper.GetFullReport(row.VINNumber, dealer.ZipCode, String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage) : KellyBlueBookHelper.GetFullReport(row.VINNumber, dealer.ZipCode, row.Mileage, row.KBBTrimId.Value, row.KBBOptionsId);
                        result.Success = true;
                    }
                    catch (Exception)
                    {

                    }

                    return PartialView("KBBData", result);
                }
                else
                {
                    List<SmallKarPowerViewModel> result;
                    if (setting != null)
                    {
                        try
                        {
                            if (context.whitmanenterprisekbbs.Any(x => x.Vin == row.VINNumber && x.ExpiredDate > DateTime.Now))
                            {
                                var searchResult = context.whitmanenterprisekbbs.Where(x => x.Vin == row.VINNumber);

                                result = new List<SmallKarPowerViewModel>();

                                foreach (var tmp in searchResult)
                                {
                                    var kbbModel = new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = tmp.BaseWholeSale,
                                        MileageAdjustment = tmp.MileageAdjustment,
                                        SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                        SelectedTrimName = tmp.Trim,
                                        Wholesale = tmp.WholeSale,
                                        IsSelected = row.KBBTrimId.GetValueOrDefault() == 0 ? true : (tmp.TrimId == row.KBBTrimId)
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
                                if (row.KBBTrimId == null || String.IsNullOrEmpty(row.KBBOptionsId))
                                    result = karpowerService.Execute(row.VINNumber, String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, Constanst.VehicleTable.Appraisal);
                                else
                                {
                                    result = karpowerService.Execute(row.VINNumber, String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage, row.KBBTrimId.GetValueOrDefault(), row.KBBOptionsId, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, Constanst.VehicleTable.Appraisal);
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

        public ActionResult ManheimDataOnMobile(string listingId)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    int convertedListingId = Convert.ToInt32(listingId);
                    var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == convertedListingId);
                    //var dealer = (DealershipViewModel)Session["Dealership"];
                    var manheimCredential = LinqHelper.GetManheimCredential(SessionHandler.DealerId);
                    if (manheimCredential != null)
                        result = LinqHelper.ManheimReportForAppraisal(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
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

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int convertedListingId = Convert.ToInt32(listingId);
                //var dealer = (DealershipViewModel)Session["Dealership"];
                var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == SessionHandler.DealerId);
                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == convertedListingId);

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

                    if (!String.IsNullOrEmpty(row.VINNumber) && !String.IsNullOrEmpty(kbbUserName) &&
                        !String.IsNullOrEmpty(kbbPassword))
                    {
                        karpowerService.ExecuteSaveVehicleWithVin(row.VINNumber, row.Mileage, row.ExteriorColor,
                                                                  row.InteriorColor, kbbUserName, kbbPassword);
                        if (!String.IsNullOrEmpty(karpowerService.SaveVrsVehicleResult))
                        {
                            var jsonObj = (JObject)JsonConvert.DeserializeObject(karpowerService.SaveVrsVehicleResult);
                            row.KarPowerEntryId = karpowerService.ConvertToString((JValue)(jsonObj["d"]["id"]));
                            context.SaveChanges();
                        }
                    }

                    SessionHandler.IsNewAppraisal = false;
                }

                if (String.IsNullOrEmpty(row.VINNumber))
                {
                    using (var scrappingContext = new vincontrolscrappingEntities())
                    {
                        if (String.IsNullOrEmpty(row.VINNumber))
                        {

                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            var sampleCar = DataHelper.GetNationwideMarketData(query, row.Make, row.Model, row.Trim).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));

                            if (sampleCar != null)

                                row.VINNumber = sampleCar.Vin;
                        }
                    }

                }

                if (!kbbStatus.ToLower().Equals("inactive") && (setting.KellyBlueBookAccessRight != null && setting.KellyBlueBookAccessRight.Value))
                {
                    var result = new KellyBlueBookViewModel();

                    try
                    {
                        result = row.KBBTrimId == null ? KellyBlueBookHelper.GetFullReport(row.VINNumber, "dealer.ZipCode", String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage) : KellyBlueBookHelper.GetFullReport(row.VINNumber, "dealer.ZipCode", row.Mileage, row.KBBTrimId.Value, row.KBBOptionsId);
                        result.Success = true;
                    }
                    catch (Exception)
                    {

                    }

                    return PartialView("KBBData", result);
                }
                else
                {
                    List<SmallKarPowerViewModel> result;
                    if (setting != null)
                    {
                        try
                        {
                            if (context.whitmanenterprisekbbs.Any(x => x.Vin == row.VINNumber && x.ExpiredDate > DateTime.Now))
                            {
                                var searchResult = context.whitmanenterprisekbbs.Where(x => x.Vin == row.VINNumber);

                                result = new List<SmallKarPowerViewModel>();

                                foreach (var tmp in searchResult)
                                {
                                    var kbbModel = new SmallKarPowerViewModel()
                                    {
                                        BaseWholesale = tmp.BaseWholeSale,
                                        MileageAdjustment = tmp.MileageAdjustment,
                                        SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                        SelectedTrimName = tmp.Trim,
                                        Wholesale = tmp.WholeSale,
                                        IsSelected = row.KBBTrimId.GetValueOrDefault() == 0 ? true : (tmp.TrimId == row.KBBTrimId)
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
                                if (row.KBBTrimId == null || String.IsNullOrEmpty(row.KBBOptionsId))
                                    result = karpowerService.Execute(row.VINNumber, String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, Constanst.VehicleTable.Appraisal);
                                else
                                {
                                    result = karpowerService.Execute(row.VINNumber, String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage, row.KBBTrimId.GetValueOrDefault(), row.KBBOptionsId, DateTime.Now, setting.KellyBlueBook, setting.KellyPassword, Constanst.VehicleTable.Appraisal);
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
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

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


                appraisal.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.CarFax,
                                                                               dealer.CarFaxPassword);

                appraisal.KBB = KellyBlueBookHelper.GetFullReport(appraisal.VinNumber, dealer.ZipCode,
                                                                  appraisal.Mileage);

                appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, appraisal.Mileage, dealer.State);


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
                return RedirectToAction("ViewProfileForAppraisal", new { appraisalId = insertedappraisal.idAppraisal });


            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult ViewProfileByYearBlank(FormCollection form, AppraisalViewFormModel appraisal)
        {
            ResetSessionValue();
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

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

                appraisal.DefaultImageUrl = dealer.DefaultStockImageUrl;

                appraisal.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.CarFax,
                                                                                 dealer.CarFaxPassword);

                appraisal.KBB = KellyBlueBookHelper.GetFullReport(appraisal.VinNumber, dealer.ZipCode, appraisal.Mileage);

                appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, appraisal.Mileage, dealer.State);

                appraisal.CarFaxDealerId = dealer.CarFax;

                var insertedAppraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                appraisal.AppraisalGenerateId = insertedAppraisal.idAppraisal.ToString(CultureInfo.InvariantCulture);

                // include Manheim Wholesales values
                try
                {

                    var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                    if (manheimCredential != null)
                        appraisal.ManheimWholesales = LinqHelper.ManheimReportForAppraisal(insertedAppraisal,
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
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                appraisal.ModelYear = Convert.ToInt32(appraisal.SelectedModelYear);

                appraisal.Make = appraisal.SelectedMake;

                appraisal.AppraisalModel = appraisal.SelectedModel;

                appraisal.TrimList = new List<SelectListItem>().AsEnumerable();

                appraisal.ExteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.InteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.DefaultImageUrl = dealer.DefaultStockImageUrl;

                if (!String.IsNullOrEmpty(appraisal.VinNumber))
                {

                    appraisal.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.CarFax,
                                                                                   dealer.CarFaxPassword);

                    appraisal.KBB = KellyBlueBookHelper.GetFullReport(appraisal.VinNumber, dealer.ZipCode,
                                                                      appraisal.Mileage);

                    appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, appraisal.Mileage, dealer.State);
                }

                var insertedappraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

                appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " + appraisal.AppraisalModel + " " +
                               appraisal.Trim;

                appraisal.AppraisalGenerateId = insertedappraisal.idAppraisal.ToString(CultureInfo.InvariantCulture);
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
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                appraisal.ModelYear = Convert.ToInt32(appraisal.SelectedModelYear);

                appraisal.Make = appraisal.SelectedMake;

                appraisal.AppraisalModel = appraisal.SelectedModel;

                appraisal.TrimList = new List<SelectListItem>().AsEnumerable();

                appraisal.ExteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.InteriorColorList = new List<SelectListItem>().AsEnumerable();

                appraisal.DefaultImageUrl = dealer.DefaultStockImageUrl;


                appraisal.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.CarFax,
                                                                               dealer.CarFaxPassword);

                appraisal.KBB = KellyBlueBookHelper.GetFullReport(appraisal.VinNumber, dealer.ZipCode, appraisal.Mileage);

                appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, appraisal.Mileage, dealer.State);

                appraisal.IsTruck = true;
                appraisal.CarFaxDealerId = dealer.CarFax;
                return View("TruckProfile", appraisal);
            }

            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveExistAppraisal(AppraisalViewFormModel appraisal)
        {
            if (Session["Dealership"] != null)
            {
                var viewModel = new AppraisalListViewModel();

                var dealer = (DealershipViewModel)Session["Dealership"];

                appraisal.DealershipId = dealer.DealershipId;


                SQLHelper.SaveExistAppraisalToDatabase(appraisal);


                viewModel.UnlimitedAppraisals = SQLHelper.GetListOfAppraisal(dealer.DealershipId);

                viewModel.RecentAppraisals = viewModel.UnlimitedAppraisals.Take(5).ToList();

                viewModel.SortSetList = SelectListHelper.InitalSortSetList();

                return View("ViewAppraisal", viewModel);
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
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                return SaveToInventory(appraisal, dealer, false);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        private ActionResult SaveToInventory(AppraisalViewFormModel appraisal, DealershipViewModel dealer, bool isRecon)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (!String.IsNullOrEmpty(appraisal.VinNumber) && context.whitmanenterprisedealershipinventories.Any(x => x.DealershipId == dealer.DealershipId && x.VINNumber == appraisal.VinNumber))
                {
                    int listingId =
                        context.whitmanenterprisedealershipinventories.First(
                            x => x.DealershipId == dealer.DealershipId && x.VINNumber == appraisal.VinNumber).
                            ListingID;
                    return RedirectToAction("ViewIProfile", "Inventory", new { ListingID = listingId });
                }
                else
                {
                    int newListingId = SQLHelper.InsertToInvetory(Convert.ToInt32(appraisal.AppraisalGenerateId), dealer, isRecon);

                    var emailList = EmailQueryHelpers.GetEmailsForInventoryNotification(dealer.DealershipId);
                    
                    EmailHelper.SendEmail(emailList, "Add New Car to Inventory", EmailHelper.CreateBodyEmailForAddToInventory(dealer, newListingId, appraisal.AppraisalBy));

                    // Calling AutoDescription
                    //var autoDescription = new AutoDescription();
                    //autoDescription.GenerateAutoDescription(newListingId, dealer);

                    return RedirectToAction("ViewIProfile", "Inventory", new { ListingID = newListingId });

                }
            }
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddToRecon(AppraisalViewFormModel appraisal)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                return SaveToInventory(appraisal, dealer, true);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }



        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult AddToWholeSale(AppraisalViewFormModel appraisal)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    if (!String.IsNullOrEmpty(appraisal.VinNumber) && context.vincontrolwholesaleinventories.Any(x => x.DealershipId == dealer.DealershipId && x.VINNumber == appraisal.VinNumber))
                    {
                        int ListingId =
                            context.whitmanenterprisedealershipinventories.First(
                                x => x.DealershipId == dealer.DealershipId && x.VINNumber == appraisal.VinNumber).
                                ListingID;
                        return RedirectToAction("ViewIWholesaleProfile", "Inventory", new { ListingID = ListingId });
                    }
                    else
                    {
                        int newListingId = SQLHelper.InsertToWholeSale(Convert.ToInt32(appraisal.AppraisalGenerateId), dealer);

                        var result =
                            from e in context.whitmanenterpriseusersnotifications
                            from et in context.whitmanenterpriseusers
                            where
                                e.DealershipId == dealer.DealershipId && e.WholeNotification.Value &&
                                e.UserName == et.UserName && et.Active.Value

                            select new
                            {
                                et.Name,
                                et.UserName,
                                et.Password,
                                et.Email,
                                et.Cellphone,
                                et.RoleName,
                                e.PriceChangeNotification
                            };


                        EmailHelper.SendEmail(result.Select(x => x.Email).AsEnumerable(), "Add New Car to WholeSale",
                                              EmailHelper.CreateBodyEmailForAddToWholeSale(dealer, newListingId, appraisal.AppraisalBy));
                        return RedirectToAction("ViewIWholesaleProfile", "Inventory", new { ListingID = newListingId });
                    }



                }



            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        public ActionResult ViewProfileForCustomerAppraisal(int customerId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var appraisal = ConvertHelper.ConvertDataRowToCustomerApppraisal(customerId);

                appraisal.Title = appraisal.ModelYear + " " + appraisal.Make + " " +
                                           appraisal.AppraisalModel;

                appraisal.AppraisalGenerateId = customerId.ToString(CultureInfo.InvariantCulture);

                appraisal.CarFaxDealerId = dealer.CarFax;

                // include Manheim Wholesales values
                try
                {
                    using (var context = new whitmanenterprisewarehouseEntities())
                    {
                        var insertedAppraisal = context.whitmanenterpriseappraisals.FirstOrDefault(i => i.idAppraisal == customerId);
                        if (insertedAppraisal != null)
                        {
                            var manheimCredential = LinqHelper.GetManheimCredential(dealer.DealershipId);
                            if (manheimCredential != null)
                                appraisal.ManheimWholesales = LinqHelper.ManheimReportForAppraisal(insertedAppraisal, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim());
                            else
                                appraisal.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                        }
                    }
                }
                catch (Exception)
                {
                    appraisal.ManheimWholesales = new List<ManheimWholesaleViewModel>();
                }

                //return GetServiceInfo(customerId, dealer, appraisal);
                return View("SavedCustomerProfile", appraisal);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }

        }

        private ActionResult GetServiceInfo(whitmanenterpriseappraisal insertedAppraisal, DealershipViewModel dealer, AppraisalViewFormModel appraisal)
        {
            var context = new vincontrolscrappingEntities();

            if (String.IsNullOrEmpty(insertedAppraisal.VINNumber))
            {
                var query = MapperFactory.GetCarsComMarketCarQuery(context, insertedAppraisal.ModelYear);

                var sampleCar = DataHelper.GetNationwideMarketData(query, insertedAppraisal.Make, insertedAppraisal.Model, insertedAppraisal.Trim).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));

                if (sampleCar != null)
                    appraisal.SampleVin = sampleCar.Vin;
            }

            try
            {
                appraisal.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.CarFax, dealer.CarFaxPassword);
            }
            catch (Exception)
            {
                
            }

            appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, appraisal.Mileage, dealer.State);

            appraisal.CarFaxDealerId = dealer.CarFax;

            if (!String.IsNullOrEmpty(appraisal.VehicleType) && appraisal.VehicleType.Equals("Truck"))
            {
                appraisal.SelectedTruckType = appraisal.TruckType;

                appraisal.SelectedTruckClass = appraisal.TruckClass;

                appraisal.SelectedTruckCategory = appraisal.TruckCategory;

                return View("SavedTruckProfile", appraisal);
            }

            return View("SavedProfile", appraisal);
        }

        private ActionResult GetServiceInfoOnMobile(whitmanenterpriseappraisal insertedAppraisal, DealershipViewModel dealer, AppraisalViewFormModel appraisal)
        {
            var context = new vincontrolscrappingEntities();

            if (String.IsNullOrEmpty(insertedAppraisal.VINNumber))
            {
                var query = MapperFactory.GetCarsComMarketCarQuery(context, insertedAppraisal.ModelYear);

                var sampleCar = DataHelper.GetNationwideMarketData(query, insertedAppraisal.Make, insertedAppraisal.Model, insertedAppraisal.Trim).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));

                if (sampleCar != null)
                    appraisal.SampleVin = sampleCar.Vin;
            }

            try
            {
                appraisal.CarFax = CarFaxHelper.ConvertXmlToCarFaxModelAndSave(appraisal.VinNumber, dealer.CarFax, dealer.CarFaxPassword);
            }
            catch (Exception)
            {

            }

            appraisal.BB = BlackBookService.GetFullReport(appraisal.VinNumber, appraisal.Mileage, dealer.State);

            appraisal.CarFaxDealerId = dealer.CarFax;

            if (!String.IsNullOrEmpty(appraisal.VehicleType) && appraisal.VehicleType.Equals("Truck"))
            {
                appraisal.SelectedTruckType = appraisal.TruckType;

                appraisal.SelectedTruckClass = appraisal.TruckClass;

                appraisal.SelectedTruckCategory = appraisal.TruckCategory;

                return View("SavedTruckProfileOnMobile", appraisal);
            }

            return View("SavedProfileOnMobile", appraisal);
        }

        private string GetAppraisalUser(int id)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var dealerGroup = context.whitmanenterprisedealergroups.Where(o => o.DefaultDealerID == id).FirstOrDefault();
            if (dealerGroup != null)
                return dealerGroup.MasterUserName;

            var user = context.whitmanenterpriseusers.Where(o => o.DealershipID == id).FirstOrDefault();
            return user != null ? user.UserName : String.Empty;

        }

        //[VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewAppraisal()
        {
            if (Session["Dealership"] != null)
            {
                var viewModel = new AppraisalListViewModel();

                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var dtCompare = DateTime.Now.AddDays(-60);

                //var appraisalList =
                //    InventoryQueryHelper.GetSingleOrGroupAppraisal(context).Where(
                //    x => (x.Status == null || x.Status != "Pending") && x.AppraisalDate.Value > dtCompare).
                //        OrderByDescending(x => x.AppraisalDate);

                var appraisalList =
                   context.whitmanenterpriseappraisals.Where(
                    x => x.DealershipId==dealer.DealershipId   && (x.Status == null || x.Status != "Pending") && x.AppraisalDate.Value > dtCompare).
                        OrderByDescending(x => x.AppraisalDate);

                var recentList = new List<AppraisalViewFormModel>();

                var fullList = new List<AppraisalViewFormModel>();

                foreach (var row in appraisalList.Take(11))
                {
                    var appraisalTmp = new AppraisalViewFormModel
                                           {

                                               AppraisalID = row.idAppraisal,
                                               Make = row.Make,
                                               ModelYear = row.ModelYear.GetValueOrDefault(),
                                               AppraisalModel = row.Model,
                                               Trim = row.Trim,
                                               VinNumber = row.VINNumber,
                                               StockNumber = row.StockNumber,
                                               ACV = row.ACV,
                                               CarImagesUrl = row.DefaultImageUrl,
                                               DefaultImageUrl = row.DefaultImageUrl,
                                               ExteriorColor = row.ExteriorColor,
                                               AppraisalDate = row.AppraisalDate.GetValueOrDefault().ToShortDateString(),
                                               AppraisalGenerateId = row.AppraisalID,
                                               DealershipName = row.DealershipName,
                                               AppraisalBy = !String.IsNullOrEmpty(row.UserStamp) ? GetUserNameByUserName(row.UserStamp) : GetUserNameByUserName(row.AppraisalBy)
                                           };



                    recentList.Add(appraisalTmp);
                }

                foreach (whitmanenterpriseappraisal row in appraisalList)
                {
                    var appraisalTmp = new AppraisalViewFormModel
                                           {
                                               AppraisalID = row.idAppraisal,
                                               Make = row.Make,
                                               ModelYear = row.ModelYear.GetValueOrDefault(),
                                               AppraisalModel = row.Model,
                                               Trim = row.Trim,
                                               VinNumber = row.VINNumber,
                                               StockNumber = row.StockNumber,
                                               ACV = row.ACV,
                                               CarImagesUrl = row.DefaultImageUrl,
                                               DefaultImageUrl = row.DefaultImageUrl,
                                               ExteriorColor = row.ExteriorColor,
                                               AppraisalDate = row.AppraisalDate.GetValueOrDefault().ToShortDateString(),
                                               AppraisalGenerateId = row.AppraisalID,
                                               DealershipName = row.DealershipName
                                           };



                    fullList.Add(appraisalTmp);
                }

                viewModel.RecentAppraisals = recentList;

                viewModel.UnlimitedAppraisals = fullList;

                viewModel.SortSetList = SelectListHelper.InitalSortSetList();

                return View("ViewAppraisal", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        private string GetUserNameByUserName(string userName)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                if (context.whitmanenterpriseusers.Any(i => i.UserName.ToLower().Equals(userName.ToLower())))
                {
                    var firstOrDefault = context.whitmanenterpriseusers.FirstOrDefault(i => i.UserName.ToLower().Equals(userName.ToLower()));
                    if (firstOrDefault != null)
                        return firstOrDefault.Name;
                }
            }

            return string.Empty;
        }


        public ActionResult UpdateAcv(string appraisalId, string acv)
        {
            SQLHelper.UpdateAcvForAppraisal(appraisalId, acv);

            if (Request.IsAjaxRequest())
            {
                return Json(acv);

            }
            return Json(appraisalId + " NOT UPDATED " + acv);
           
        }

        public ActionResult UpdateCustomerFirstName(string appraisalId, string customerFirstName)
        {
            SQLHelper.UpdateCustomerFirstNameForAppraisal(appraisalId, customerFirstName);

            if (Request.IsAjaxRequest())
            {
                return Json(customerFirstName);

            }
            return Json(appraisalId + " NOT UPDATED " + customerFirstName);

        }

        public ActionResult UpdateCustomerLastName(string appraisalId, string customerLastName)
        {
            SQLHelper.UpdateCustomerLastNameForAppraisal(appraisalId, customerLastName);

            if (Request.IsAjaxRequest())
            {
                return Json(customerLastName);

            }
            return Json(appraisalId + " NOT UPDATED " + customerLastName);

        }

        public ActionResult UpdateCustomerAddress(string appraisalId, string customerAddress)
        {
            SQLHelper.UpdateCustomerAddressForAppraisal(appraisalId, customerAddress);

            if (Request.IsAjaxRequest())
            {
                return Json(customerAddress);

            }
            return Json(appraisalId + " NOT UPDATED " + customerAddress);

        }

        public ActionResult UpdateCustomerCity(string appraisalId, string customerCity)
        {
            SQLHelper.UpdateCustomerCityForAppraisal(appraisalId, customerCity);

            if (Request.IsAjaxRequest())
            {
                return Json(customerCity);

            }
            return Json(appraisalId + " NOT UPDATED " + customerCity);

        }

        public ActionResult UpdateCustomerState(string appraisalId, string customerState)
        {
            SQLHelper.UpdateCustomerStateForAppraisal(appraisalId, customerState);

            if (Request.IsAjaxRequest())
            {
                return Json(customerState);

            }
            return Json(appraisalId + " NOT UPDATED " + customerState);

        }

        public ActionResult UpdateCustomerZipCode(string appraisalId, string customerZipCode)
        {
            SQLHelper.UpdateCustomerZipCodeForAppraisal(appraisalId, customerZipCode);

            if (Request.IsAjaxRequest())
            {
                return Json(customerZipCode);

            }
            return Json(appraisalId + " NOT UPDATED " + customerZipCode);

        }

        public ActionResult UpdateCustomerEmail(string appraisalId, string customerEmail)
        {
            SQLHelper.UpdateCustomerEmailForAppraisal(appraisalId, customerEmail);

            if (Request.IsAjaxRequest())
            {
                return Json(customerEmail);

            }
            return Json(appraisalId + " NOT UPDATED " + customerEmail);

        }

     

        public ActionResult SortBy(AppraisalListViewModel appraisal)
        {
            if (Session["Dealership"] != null)
            {
                var viewModel = new AppraisalListViewModel();

                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var dtCompare = DateTime.Now.AddDays(-60);

                var appraisalList =
                    context.whitmanenterpriseappraisals.Where(
                        x => x.DealershipId == dealer.DealershipId && (x.Status == null || x.Status != "Pending") && x.AppraisalDate.Value > dtCompare).
                        OrderByDescending(x => x.AppraisalDate);


                var recentList = new List<AppraisalViewFormModel>();

                var fullList = new List<AppraisalViewFormModel>();

                foreach (var row in appraisalList.Take(5))
                {
                    var appraisalTmp = new AppraisalViewFormModel
                                           {

                                               AppraisalID = row.idAppraisal,
                                               Make = row.Make,
                                               ModelYear = row.ModelYear.GetValueOrDefault(),
                                               AppraisalModel = row.Model,
                                               Trim = row.Trim,
                                               VinNumber = row.VINNumber,
                                               StockNumber = row.StockNumber,
                                               ACV = row.ACV,
                                               CarImagesUrl = row.DefaultImageUrl,
                                               DefaultImageUrl = row.DefaultImageUrl,
                                               ExteriorColor = row.ExteriorColor,
                                               AppraisalDate = row.AppraisalDate.Value.ToShortDateString(),
                                               AppraisalGenerateId = row.AppraisalID
                                           };



                    recentList.Add(appraisalTmp);
                }

                foreach (var row in appraisalList)
                {
                    var appraisalTmp = new AppraisalViewFormModel
                                           {
                                               AppraisalID = row.idAppraisal,
                                               Make = row.Make,
                                               ModelYear = row.ModelYear.GetValueOrDefault(),
                                               AppraisalModel = row.Model,
                                               Trim = row.Trim,
                                               VinNumber = row.VINNumber,
                                               StockNumber = row.StockNumber,
                                               ACV = row.ACV,
                                               CarImagesUrl = row.DefaultImageUrl,
                                               DefaultImageUrl = row.DefaultImageUrl,
                                               ExteriorColor = row.ExteriorColor,
                                               AppraisalDate = row.AppraisalDate.Value.ToShortDateString(),
                                               AppraisalGenerateId = row.AppraisalID
                                           };



                    fullList.Add(appraisalTmp);
                }
                viewModel.RecentAppraisals = recentList;
                viewModel.UnlimitedAppraisals = fullList;

                switch (appraisal.SelectedSortSet)
                {
                    case "Year":
                        viewModel.UnlimitedAppraisals = fullList.OrderBy(x => x.ModelYear).ToList();
                        break;
                    case "Make":
                        viewModel.UnlimitedAppraisals = fullList.OrderBy(x => x.Make).ToList();
                        break;
                    case "Model":
                        viewModel.UnlimitedAppraisals = fullList.OrderBy(x => x.AppraisalModel).ToList();
                        break;
                    case "Price":
                        viewModel.UnlimitedAppraisals = fullList.OrderBy(x => x.SalePrice).ToList();
                        break;
                    case "Age":
                        viewModel.UnlimitedAppraisals = fullList.OrderBy(x => x.AppraisalDate).ToList();
                        break;
                    default:
                        break;

                }
                viewModel.SortSetList = SelectListHelper.InitalSortSetList();
                return View("ViewAppraisal", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }


        public ActionResult EditAppraisal(int appraisalId)
        {
            ResetSessionValue();
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            
            var dealer = SessionHandler.Dealership;

            var row = ConvertHelper.GetAppraisalModelFromAppriaslId(appraisalId);

            var autoService = new ChromeAutoService();

            if (!String.IsNullOrEmpty(row.VinNumber))
            {
                return ReDecodeAppraisalWithVin(dealer, row, autoService);
            }
            else
            {
                return ReDecodeAppraisalWithStyle(dealer, row, autoService);
            }
        }

        private ActionResult ReDecodeAppraisalWithStyle(DealershipViewModel dealer, AppraisalViewFormModel row, ChromeAutoService autoService)
        {
            if (!String.IsNullOrEmpty(row.ChromeStyleId))
            {
                int chromeModelId; Int32.TryParse(row.ChromeModelId, out chromeModelId);

                int chromeStyleId; Int32.TryParse(row.ChromeStyleId, out chromeStyleId);

                var styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);

                var styleArray = autoService.GetStyles(Convert.ToInt32(chromeModelId));

                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModelWithoutVin(appraisal, row, styleInfo, dealer.DealershipId, row.Location, false);

                appraisal.ChromeModelId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeStyleId = chromeStyleId.ToString(CultureInfo.InvariantCulture);

                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                    appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                return View("EditAppraisal", appraisal);
            }
            else
            {
                int chromeModelId; Int32.TryParse(row.ChromeModelId, out chromeModelId);

                var styleArray = autoService.GetStyles(chromeModelId);

                VehicleDescription styleInfo = null;

                if (row.SelectedTrim != null && row.SelectedTrim.Equals(string.Empty))
                {
                    styleInfo = autoService.GetStyleInformationFromStyleId(styleArray.First().id);
                }

                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);

                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModelWithoutVin(appraisal, row, styleInfo, dealer.DealershipId, row.Location, false);

                appraisal.AppraisalGenerateId = row.AppraisalID.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeModelId = chromeModelId.ToString(CultureInfo.InvariantCulture);

                appraisal.ChromeStyleId = styleArray.First().id.ToString();

                appraisal.TrimList = SelectListHelper.InitalTrimList(styleArray);

                return View("EditAppraisal", appraisal);
            }
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
                var element = vehicleInfo.style.Where(x => x.trim == row.Trim).FirstOrDefault();
                styleInfo = autoService.GetStyleInformationFromStyleId(element != null ? element.id : vehicleInfo.style.First().id);
            }

            if (vehicleInfo != null)
            {

                var viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);

                viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, true);

                return View("EditAppraisal", viewModel);
            }
            else
            {
                var viewModel = new AppraisalViewFormModel { AppraisalGenerateId = row.AppraisalID.ToString(CultureInfo.InvariantCulture) };

                viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, false);

                return View("EditAppraisal", viewModel);
            }
        }

        public ActionResult EditAppraisalForTruck(int appraisalId)
        {
            ResetSessionValue();
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

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

                        viewModel.SelectedTruckType = row.TruckType;

                        viewModel.SelectedTruckClass = row.TruckClass;

                        viewModel.SelectedTruckCategory = row.TruckCategory;

                        viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                        viewModel.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);
                        
                        viewModel.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                        viewModel.IsTruck = true;
                        
                        return View("EditAppraisalForTruck", viewModel);
                    }
                    else
                    {
                        var viewModel = new AppraisalViewFormModel();

                        viewModel = ConvertHelper.UpdateSuccessfulAppraisalModel(viewModel, row, vehicleInfo, dealer.DealershipId, row.Location, false);

                        viewModel.SelectedTruckType = row.TruckType;

                        viewModel.SelectedTruckClass = row.TruckClass;

                        viewModel.SelectedTruckCategory = row.TruckCategory;

                        viewModel.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                        viewModel.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(viewModel.TruckTypeList.First().Value));

                        viewModel.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);
                        
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

                                var listItem = new List<SelectListItem> {selectedTrim.First()};

                                listItem.AddRange(extractTrimList);

                                appraisal.TrimList = listItem.AsEnumerable();
                            }
                        }

                        if (appraisal.ExteriorColorList.Any())
                        {
                            var selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Value.Equals(appraisal.SelectedExteriorColorValue.Trim()));

                            SelectListItem selectedFirstOrDefault;

                            if (selectedExteriorColor != null && selectedExteriorColor.Any())
                            {
                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Value.Equals(appraisal.SelectedExteriorColorValue.Trim()));

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

                                listItem.AddRange(extractExteriorColorList);

                                appraisal.ExteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

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

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedInteriorColor = appraisal.InteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedInteriorColor.First();

                                var extractInteriorColorList = appraisal.InteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

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

                            var listItem = new List<SelectListItem> {selectedTrim};

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

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

                                listItem.AddRange(extractExteriorColorList);

                                appraisal.ExteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedExteriorColor = appraisal.ExteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedExteriorColor.First();

                                var extractExteriorColorList = appraisal.ExteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

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

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();
                            }
                            else
                            {
                                selectedInteriorColor = appraisal.InteriorColorList.Where(x => x.Value.Equals("Other Colors"));

                                selectedFirstOrDefault = selectedInteriorColor.First();

                                var extractInteriorColorList = appraisal.InteriorColorList.Where(x => !x.Value.Equals("Other Colors"));

                                var listItem = new List<SelectListItem> {selectedFirstOrDefault};

                                listItem.AddRange(extractInteriorColorList);

                                appraisal.InteriorColorList = listItem.AsEnumerable();

                                appraisal.CusInteriorColor = appraisal.SelectedInteriorColor;
                            }
                        }
                        
                        appraisal.SalePrice = appraisal.MSRP;

                        if (styleInfo.style != null && styleInfo.style.First().stockImage!=null)
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
            return SaveAppraisalInfo(form, appraisal, false);
        }

        private ActionResult SaveAppraisalInfo(FormCollection form, AppraisalViewFormModel appraisal, bool isTruck)
        {
            if (SessionHandler.Dealership == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            appraisal = ConvertHelper.UpdateAppraisalBeforeSaving(form, appraisal, SessionHandler.Dealership, HttpContext.User.Identity.Name);

            appraisal.DealershipZipCode = SessionHandler.Dealership.ZipCode;

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
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }


            return RedirectToAction("ViewProfileForAppraisal", new { AppraisalId = appraisal.AppraisalGenerateId });

        }

        public ActionResult SaveKBBOptions(int appraisalId, string OptionSelect, int TrimId, string BaseWholeSale, string WholeSale, string MileageAdjustment)
        {
            if (Session["Dealership"] == null)
            {
                return Json("SessionTimeOut");

            }
            else
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                SQLHelper.UpdateKBBOptionsForAppraisal(appraisalId, OptionSelect, TrimId, BaseWholeSale, WholeSale, MileageAdjustment);

                if (Request.IsAjaxRequest())
                {
                    return Json("Success");

                }

            }
            return Json(appraisalId + " NOT UPDATED ");



        }

        public ActionResult CreateAppraisal(int CustomerID, string ActionName, string ID)
        {
            if (Session["Dealership"] != null)
            {

                //DecodeController 
                //AppraisalViewFormModel appraisal = DataHelper.GetAppraisalViewModel(VIN);
                //JavaScriptModel model = DataHelper.GetJavaScripModel(Vin, dealer);
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
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int id = Convert.ToInt32(appraisalId);
                byte[] photo = context.whitmanenterpriseappraisals.Where(a => a.idAppraisal == id).FirstOrDefault().Photo;
                return photo != null ? File(photo, "image/jpeg") : File(new byte[] { }, "image/jpeg");
            }
        }

        public ActionResult CreateAppraisalWithVIN(int CustomerID, string ID)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];
            AppraisalViewFormModel appraisal = DataHelper.GetAppraisalViewModel(ID);
            var context = new whitmanenterprisewarehouseEntities();
            var customer = context.vincontrolbannercustomers.Where(e => e.TradeInCustomerId == CustomerID).FirstOrDefault();
            if (customer != null)
            {
                appraisal.CustomerFirstName = customer.FirstName;
                appraisal.CustomerLastName = customer.LastName;

            }
            var insertedAppraisal = SQLHelper.InsertAppraisalToDatabase(appraisal, dealer);

            DeleteCustomer(CustomerID);

            return RedirectToAction("EditAppraisal", "Appraisal", new { AppraisalId = insertedAppraisal.idAppraisal });
        }

        private static void DeleteCustomer(int CustomerID)
        {
            var context = new whitmanenterprisewarehouseEntities();
            var customer = context.vincontrolbannercustomers.Where(e => e.TradeInCustomerId == CustomerID).FirstOrDefault();


            if (customer != null)
            {
                customer.TradeInStatus = "Deleted";
                context.SaveChanges();
            }

        }

        public ActionResult GetVehicleInformationFromStyleId(string appraisalId, string vin, string styleId, bool isTruck, string styleName, string cusStyle)
        {
            var autoService = new ChromeAutoService();

            var dealer = (DealershipViewModel)Session["Dealership"];

            var row = ConvertHelper.GetAppraisalModelFromAppriaslId(Convert.ToInt32(appraisalId));

            if (!String.IsNullOrEmpty(vin))
            {
                var vehicleInfo = autoService.GetVehicleInformationFromVin(vin, Convert.ToInt32(styleId));
                var styleInfo = autoService.GetStyleInformationFromStyleId(Convert.ToInt32(styleId));
                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyle(vehicleInfo, styleInfo);
                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModel(appraisal, row, vehicleInfo, dealer.DealershipId, row.Location, true);
                appraisal.AppraisalGenerateId = appraisalId;
                appraisal.CusTrim = cusStyle;
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
                    }
                    else
                    {
                        selectedTrim = appraisal.TrimList.FirstOrDefault();
                        appraisal.Trim = selectedTrim.Text;
                        appraisal.SelectedTrimItem = selectedTrim.Value;
                    }
                }

                if (!isTruck)
                {
                    appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeList();
                }
                else
                {
                    appraisal.SelectedTruckType = row.TruckType;

                    appraisal.SelectedTruckClass = row.TruckClass;

                    appraisal.SelectedTruckCategory = row.TruckCategory;

                    appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                    appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                    appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                    appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                    appraisal.IsTruck = true;
                }

                return PartialView("DetailAppraisal", appraisal);
            }
            else
            {
                var styleInfo = autoService.GetStyleInformationFromStyleId(Convert.ToInt32(styleId));
                var appraisal = ConvertHelper.GetVehicleInfoFromChromeDecode(styleInfo);
                appraisal = ConvertHelper.UpdateSuccessfulAppraisalModelWithoutVin(appraisal, row, styleInfo, dealer.DealershipId, row.Location, false);
                appraisal.AppraisalGenerateId = appraisalId;
                appraisal.CusTrim = cusStyle;
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
                    }
                    else
                    {
                        selectedTrim = appraisal.TrimList.FirstOrDefault();
                        appraisal.Trim = selectedTrim.Text;
                        appraisal.SelectedTrimItem = selectedTrim.Value;
                    }
                }

                if (!isTruck)
                {
                    appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeList();
                    
                    appraisal.TrimList = SelectListHelper.InitalTrimList(styleInfo.style);

                    if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                        appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;
                }
                else
                {
                    appraisal.TrimList = SelectListHelper.InitalTrimList(styleInfo.style);

                    if (styleInfo.style != null && styleInfo.style.First().stockImage != null)
                        appraisal.DefaultImageUrl = styleInfo.style.First().stockImage.url;

                    appraisal.SelectedTruckType = row.TruckType;

                    appraisal.SelectedTruckClass = row.TruckClass;

                    appraisal.SelectedTruckCategory = row.TruckCategory;

                    appraisal.TruckTypeList = SelectListHelper.InitalTruckTypeList();

                    appraisal.TruckCategoryList = SelectListHelper.InitalTruckCategoryList(SQLHelper.GetListOfTruckCategoryByTruckType(appraisal.TruckTypeList.First().Value));

                    appraisal.TruckClassList = SelectListHelper.InitalTruckClassList(row.TruckClass);

                    appraisal.VehicleTypeList = SelectListHelper.InitalVehicleTypeListForTruck();

                    appraisal.IsTruck = true;
                }

                return PartialView("DetailAppraisal", appraisal);
            }
        }

        #region Pending Appraisal

        public ActionResult GetPendingAppraisalNumber()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                var context = new whitmanenterprisewarehouseEntities();
                var pendingAppraisal = context.whitmanenterpriseappraisals.Where(e => e.Status == "Pending" && e.DealershipId == dealer.DealershipId && e.VinGenie.HasValue && e.VinGenie.Value);
                return Content(pendingAppraisal.Count().ToString());
            }
            return Content("-1");
        }

        [HttpPost]
        public ActionResult ListOfPendingAppraisals()
        {
            var fullList = new List<AppraisalViewFormModel>();
            if (Session["Dealership"] != null)
            {
                return ShowPending();
            }

            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ViewPendingAppraisal()
        {
            //var fullList = new List<AppraisalViewFormModel>();
            if (Session["Dealership"] != null)
            {
                return ShowPending();
            }

            return RedirectToAction("LogOff", "Account");
        }

        private ActionResult ShowPending()
        {
            var viewModel = new AppraisalListViewModel();

            var context = new whitmanenterprisewarehouseEntities();

            var pendingAppraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
                                      .Where(e => e.Status == "Pending" && e.VinGenie.HasValue && e.VinGenie.Value).OrderByDescending(x => x.DateStamp).ToList();

            var userPendingAppraisal = from a in pendingAppraisal
                                       from u in context.whitmanenterpriseusers
                                       where a.UserStamp.ToLower().Equals(u.UserName.ToLower())
                                       select new AppraisalViewFormModel()
                                           {
                                               AppraisalID = a.idAppraisal,
                                               Make = a.Make,
                                               ModelYear = a.ModelYear ?? 2012,
                                               AppraisalModel = a.Model,
                                               Trim = a.Trim,
                                               VinNumber = a.VINNumber,
                                               StockNumber = a.StockNumber,
                                               ACV = a.ACV,
                                               CarImagesUrl = a.DefaultImageUrl,
                                               DefaultImageUrl = a.DefaultImageUrl,
                                               ExteriorColor = a.ExteriorColor,
                                               AppraisalGenerateId = a.AppraisalID,
                                               UserStamp = u.Name,
                                               DateStamp = a.DateStamp,
                                               VinGenie = a.VinGenie,
                                               IsPhotoFromVingenie=a.Photo!=null
                                           };

            var masterPendingAppraisal = from a in pendingAppraisal
                                         from u in context.whitmanenterprisedealergroups
                                       where a.UserStamp.ToLower().Equals(u.MasterUserName.ToLower())
                                       select new AppraisalViewFormModel()
                                       {
                                           AppraisalID = a.idAppraisal,
                                           Make = a.Make,
                                           ModelYear = a.ModelYear ?? 2012,
                                           AppraisalModel = a.Model,
                                           Trim = a.Trim,
                                           VinNumber = a.VINNumber,
                                           StockNumber = a.StockNumber,
                                           ACV = a.ACV,
                                           CarImagesUrl = a.DefaultImageUrl,
                                           DefaultImageUrl = a.DefaultImageUrl,
                                           ExteriorColor = a.ExteriorColor,
                                           AppraisalGenerateId = a.AppraisalID,
                                           UserStamp = u.MasterUserName,
                                           DateStamp = a.DateStamp,
                                           VinGenie = a.VinGenie,
                                           IsPhotoFromVingenie = a.Photo != null
                                       };

            var temporaryist = userPendingAppraisal.Union(masterPendingAppraisal).ToList();

            var hashSet = new HashSet<string>();

            var recentList = new List<AppraisalViewFormModel>();

            foreach (var tmp in temporaryist)
            {
                var uniqueString = tmp.UserStamp + tmp.VinNumber;
                if (!hashSet.Contains(uniqueString))
                    recentList.Add(tmp);

                hashSet.Add(uniqueString);
            }

            viewModel.RecentAppraisals = recentList;

            viewModel.UnlimitedAppraisals = recentList;

            viewModel.SortSetList = SelectListHelper.InitalSortSetList();

            return View("ViewPendingAppraisal", viewModel);
        }

        #endregion
    }
}
