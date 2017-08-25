using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Models;
using System.Web.Mvc;
using vincontrol.Helper;

namespace Vincontrol.Web.HelperClass
{
    public class BuyerGuideHelper
    {
        public static string BuildBuyerGuideInHTML(ControllerContext controllerContext)
        {
            var context = new VincontrolEntities();

            var rows = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(x =>  x.Condition == Constanst.ConditionStatus.Used);

            var settingRow = InventoryQueryHelper.GetSingleOrGroupSetting(context);

            var buyerGuideList = InventoryQueryHelper.GetSingleOrGroupBuyerGuide(context).Where(bg => bg.LanguageVersion == 1).ToList();

            var returnList = new List<AdminBuyerGuideViewModel>();

            foreach (var row in rows)
            {
                var adminBuyerGuideViewModel = new AdminBuyerGuideViewModel();
                var warrantyType = 0;
                if (row.WarrantyInfo == null)
                    warrantyType = 1;
                else
                {
                    warrantyType = row.WarrantyInfo.GetValueOrDefault();
                }

                var buyerGuideSetting = buyerGuideList.FirstOrDefault(bg => bg.WarrantyType == warrantyType &&bg.DealerId==row.DealerId);

                adminBuyerGuideViewModel.SelectedLanguage = 1;

                if (buyerGuideSetting != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSetting.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year.GetValueOrDefault();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations = ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year.GetValueOrDefault();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.FirstOrDefault(i => row.DealerId == i.DealerId).AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }
                returnList.Add(adminBuyerGuideViewModel);

            }


            string htmlToConvert = PDFRender.RenderViewAsString("BuyerGuideBody", returnList, controllerContext);


                return htmlToConvert;
            

      

        }
        public static string BuildBuyerGuideInHTMLInSpanish( ControllerContext controllerContext)
        {
            var context = new VincontrolEntities();

            var rows = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(x => x.Condition == Constanst.ConditionStatus.Used );

            var settingRow = InventoryQueryHelper.GetSingleOrGroupSetting(context);

            var buyerGuideList = InventoryQueryHelper.GetSingleOrGroupBuyerGuide(context).Where(bg => bg.LanguageVersion == 2).ToList();

            var returnList = new List<AdminBuyerGuideViewModel>();

            foreach (var row in rows)
            {
                var adminBuyerGuideViewModel = new AdminBuyerGuideViewModel();
                var warrantyType = 0;
                if (row.WarrantyInfo == null)
                    warrantyType = 1;
                else
                {
                    warrantyType = row.WarrantyInfo.GetValueOrDefault();
                }

                var buyerGuideSetting = buyerGuideList.FirstOrDefault(bg => bg.WarrantyType == warrantyType && bg.DealerId == row.DealerId);

                adminBuyerGuideViewModel.SelectedLanguage = 2;

                if (buyerGuideSetting != null)
                {
                    adminBuyerGuideViewModel.Id = buyerGuideSetting.BuyerGuideId;
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year.GetValueOrDefault();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.WarrantyType = buyerGuideSetting.WarrantyType;
                    adminBuyerGuideViewModel.ServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsAsWarranty = buyerGuideSetting.IsAsWarranty ?? false;
                    adminBuyerGuideViewModel.IsWarranty = buyerGuideSetting.IsWarranty ?? false;
                    adminBuyerGuideViewModel.IsFullWarranty = buyerGuideSetting.IsFullWarranty ?? false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = buyerGuideSetting.IsLimitedWarranty ?? false;
                    adminBuyerGuideViewModel.IsServiceContract = buyerGuideSetting.IsServiceContract ?? false;
                    adminBuyerGuideViewModel.IsMixed = buyerGuideSetting.IsMixed ?? false;
                    adminBuyerGuideViewModel.SystemCovered =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCovered);
                    adminBuyerGuideViewModel.SystemCoveredAndDurations =
                        ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.SystemCoveredAndDurations);
                    adminBuyerGuideViewModel.Durations = ReplaceFontSizeForBuyerGuideReport(buyerGuideSetting.Durations);
                    adminBuyerGuideViewModel.PercentageOfLabor = buyerGuideSetting.PercentageOfLabor ?? 0;
                    adminBuyerGuideViewModel.PercentageOfPart = buyerGuideSetting.PercentageOfPart ?? 0;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();
                    adminBuyerGuideViewModel.IsManufacturerWarranty = buyerGuideSetting.IsManufacturerWarranty ?? false;
                    adminBuyerGuideViewModel.IsManufacturerUsedVehicleWarranty = buyerGuideSetting.IsManufacturerUsedVehicleWarranty ?? false;
                    adminBuyerGuideViewModel.IsOtherWarranty = buyerGuideSetting.IsOtherWarranty ?? false;
                }
                else
                {
                    adminBuyerGuideViewModel.Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                    adminBuyerGuideViewModel.Year = row.Vehicle.Year.GetValueOrDefault();
                    adminBuyerGuideViewModel.Make = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;
                    adminBuyerGuideViewModel.VehicleModel = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;
                    adminBuyerGuideViewModel.StockNumber = String.IsNullOrEmpty(row.Stock)
                                                               ? "NA"
                                                               : row.Stock;
                    adminBuyerGuideViewModel.ServiceContract = false;
                    adminBuyerGuideViewModel.IsAsWarranty = String.IsNullOrEmpty(settingRow.FirstOrDefault(i => row.DealerId == i.DealerId).AsIsWarranty) ? false : true;
                    adminBuyerGuideViewModel.IsWarranty = warrantyType != 1 ? true : false;
                    adminBuyerGuideViewModel.IsFullWarranty = false;
                    adminBuyerGuideViewModel.IsLimitedWarranty = false;
                    adminBuyerGuideViewModel.IsServiceContract = false;
                    adminBuyerGuideViewModel.SystemCovered = string.Empty;
                    adminBuyerGuideViewModel.Durations = string.Empty;
                    adminBuyerGuideViewModel.PercentageOfLabor = 100;
                    adminBuyerGuideViewModel.PercentageOfPart = 100;
                    adminBuyerGuideViewModel.PriorRental = row.PriorRental.GetValueOrDefault() ? "PRIOR RENTAL" : "";
                    adminBuyerGuideViewModel.IsPriorRental = row.PriorRental.GetValueOrDefault();

                    if (!adminBuyerGuideViewModel.IsWarranty && !adminBuyerGuideViewModel.IsFullWarranty &&
                        !adminBuyerGuideViewModel.IsLimitedWarranty)
                        adminBuyerGuideViewModel.IsAsWarranty = true;
                }
                returnList.Add(adminBuyerGuideViewModel);

            }


            string htmlToConvert = PDFRender.RenderViewAsString("BuyerGuideBodyInSpanish", returnList, controllerContext);


            return htmlToConvert;




        }





        private static string ReplaceFontSizeForBuyerGuideReport(string content)
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
    }
}
