using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Data.Model;
using Vincontrol.Web.Models;

namespace Vincontrol.Web.HelperClass
{
    public class WindowStickerHelper
    {
       
        public static WindowStickerViewModel BuildWindowStickerInHtml(int listingId, int dealerId)
        {
            var inventoryManagementForm = new InventoryManagementForm();

            var dealerManagementForm = new DealerManagementForm();
            var row = inventoryManagementForm.GetInventory(listingId);
            var setting = dealerManagementForm.GetDealerSettingById(dealerId);
            return row != null ? BuildWsByInventory(row, setting) : BuildWsBySoldInventory(listingId, setting);
           
        }

        public static WindowStickerViewModel BuildWsByInventory(Inventory inventory, Setting setting)
        {
                return GetVehicleInfo(inventory, setting);
        }

        private static WindowStickerViewModel GetVehicleInfo(Inventory row, Setting setting)
        {
            int result = 0;
            var vehicleInfoViewModel = new WindowStickerViewModel()
                {
                    BarCodeUrl =
                    "http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" +
                    row.Vehicle.Vin +
                    "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false",
                    Title = row.Vehicle.Year.GetValueOrDefault() + " " + row.Vehicle.Make + " " +
                            row.Vehicle.Model,
                    Trim = row.Vehicle.Trim,
                    Cylinders = row.Vehicle.Cylinders,
                    FuelType = row.Vehicle.FuelType,
                    Mileage = (int)row.Mileage.GetValueOrDefault(),
                    Stock = row.Stock,
                    ExteriorColor = row.ExteriorColor,
                    InteriorColor = row.Vehicle.InteriorColor,
                    Engine = row.Vehicle.EngineType,
                    RetailPrice = (setting.RetailPrice.HasValue&& setting.RetailPrice.Value)? (decimal?)row.RetailPrice.GetValueOrDefault():null,
                    DealerDiscount = (setting.DealerDiscount.HasValue && setting.DealerDiscount.Value) ? (decimal?)row.DealerDiscount.GetValueOrDefault() : null,
                    ManufacturerRebate = (setting.ManufacturerReabte.HasValue && setting.ManufacturerReabte.Value) ? (decimal?)row.ManufacturerRebate.GetValueOrDefault() : null,
                    RetailPriceText = String.IsNullOrEmpty(setting.RetailPriceText) ? "Retail Price" : setting.RetailPriceText,
                    DealerDiscountText = String.IsNullOrEmpty(setting.DealerDiscountText) ? "Dealer Discount" : setting.DealerDiscountText,
                    ManufacturerRebateText = String.IsNullOrEmpty(setting.ManufactureReabateText) ? "Manufacturer Rebate" : setting.ManufactureReabateText,
                    SalePriceText = String.IsNullOrEmpty(setting.SalePriceText)?"Sale Price" : setting.SalePriceText,
                    FuelEconomyCity = int.TryParse(row.Vehicle.FuelEconomyCity, out result) ? result : 0,
                    FuelEconomyHighWay = int.TryParse(row.Vehicle.FuelEconomyHighWay, out result) ? result : 0,
                    Transmission =
                        !String.IsNullOrEmpty(row.Vehicle.Tranmission)
                            ? (row.Vehicle.Tranmission.ToLower().Contains("auto") ? "Automatic" : "Manual")
                            : string.Empty,
                    VIN = row.Vehicle.Vin,
                    StandardOptions = String.IsNullOrEmpty(row.Vehicle.StandardOptions)?new List<string>() : row.Vehicle.StandardOptions.Split(',').ToList()
                };

            vehicleInfoViewModel.SalePrice = (setting.SalePrice.HasValue && setting.SalePrice.Value) ? (decimal?)(row.RetailPrice.GetValueOrDefault() - row.DealerDiscount.GetValueOrDefault() - row.ManufacturerRebate.GetValueOrDefault()) : null;


            //CAR OPTIONS
            vehicleInfoViewModel.PackageAndOptions = !String.IsNullOrEmpty(row.AdditionalOptions) || !String.IsNullOrEmpty(row.AdditionalPackages)
                                                         ? GetFinalPackageAndOptions(row.AdditionalOptions,row.AdditionalPackages)
                                                         : new List<string>();


            return vehicleInfoViewModel;
        }

        private static WindowStickerViewModel GetVehicleInfo(SoldoutInventory row, Setting setting)
        {

            int result = 0;
            var vehicleInfoViewModel = new WindowStickerViewModel()
            {
                BarCodeUrl =
                "http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" +
                row.Vehicle.Vin +
                "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false",
                Title = row.Vehicle.Year.GetValueOrDefault() + " " + row.Vehicle.Make + " " +
                        row.Vehicle.Model,
                Trim = row.Vehicle.Trim,
                Cylinders = row.Vehicle.Cylinders,
                FuelType = row.Vehicle.FuelType,
                Mileage = (int)row.Mileage.GetValueOrDefault(),
                Stock = row.Stock,
                ExteriorColor = row.ExteriorColor,
                InteriorColor = row.Vehicle.InteriorColor,
                Engine = row.Vehicle.EngineType,
                RetailPrice = (setting.RetailPrice.HasValue && setting.RetailPrice.Value) ? (decimal?)row.RetailPrice.GetValueOrDefault() : null,
                DealerDiscount = (setting.DealerDiscount.HasValue && setting.DealerDiscount.Value) ? (decimal?)row.DealerDiscount.GetValueOrDefault() : null,
                ManufacturerRebate = (setting.ManufacturerReabte.HasValue && setting.ManufacturerReabte.Value) ? (decimal?)row.ManufacturerRebate.GetValueOrDefault() : null,
                RetailPriceText = String.IsNullOrEmpty(setting.RetailPriceText) ? "Retail Price" : setting.RetailPriceText,
                DealerDiscountText = String.IsNullOrEmpty(setting.DealerDiscountText) ? "Dealer Discount" : setting.DealerDiscountText,
                ManufacturerRebateText = String.IsNullOrEmpty(setting.ManufactureReabateText) ? "Manufacturer Rebate" : setting.ManufactureReabateText,
                SalePriceText = String.IsNullOrEmpty(setting.SalePriceText) ? "Sale Price" : setting.SalePriceText,
                FuelEconomyCity = int.TryParse(row.Vehicle.FuelEconomyCity, out result) ? result : 0,
                FuelEconomyHighWay = int.TryParse(row.Vehicle.FuelEconomyHighWay, out result) ? result : 0,
                Transmission =
                    !String.IsNullOrEmpty(row.Vehicle.Tranmission)
                        ? (row.Vehicle.Tranmission.ToLower().Contains("auto") ? "Automatic" : "Manual")
                        : string.Empty,
                VIN = row.Vehicle.Vin,
                StandardOptions = String.IsNullOrEmpty(row.Vehicle.StandardOptions) ? new List<string>() : row.Vehicle.StandardOptions.Split(',').ToList()
            };

            vehicleInfoViewModel.SalePrice = vehicleInfoViewModel.RetailPrice - vehicleInfoViewModel.DealerDiscount ?? 0 - vehicleInfoViewModel.ManufacturerRebate ?? 0;


            //CAR OPTIONS
            vehicleInfoViewModel.PackageAndOptions = !String.IsNullOrEmpty(row.AdditionalOptions) || !String.IsNullOrEmpty(row.AdditionalPackages)
                                                         ? GetFinalPackageAndOptions(row.AdditionalOptions, row.AdditionalPackages)
                                                         : new List<string>();


            return vehicleInfoViewModel;
        }

        private static List<string> GetFinalPackageAndOptions(string additionalOptions, string additionalPackages)
        {
            var finalPackageAndOptions = new List<string>();

            if (!String.IsNullOrEmpty(additionalPackages))
            {
                var addtionalPackagesList =
                    additionalPackages.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                finalPackageAndOptions.AddRange(addtionalPackagesList.AsEnumerable());
            }

            if (!String.IsNullOrEmpty(additionalOptions))
            {
                var addtionalOptionList =
                   additionalOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                finalPackageAndOptions.AddRange(addtionalOptionList.AsEnumerable());
            }
            return finalPackageAndOptions;
        }

        private static WindowStickerViewModel BuildWsBySoldInventory(int listingId, Setting setting)
        {
            var inventoryManagementForm = new InventoryManagementForm();
            var row = inventoryManagementForm.GetSoldInventory(listingId); 
            return row != null ? GetVehicleInfo(row, setting) : new WindowStickerViewModel();
        }

      
    }
}
