using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Helper;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;

namespace Vincontrol.Web.Handlers
{
    public static class MappingHanlder
    {
        public static Appraisal ConvertToAppraisal(AppraisalViewFormModel appraisal)
        {
            return new Appraisal()
            {
                Stock = appraisal.StockNumber ?? String.Empty,
                Mileage = appraisal.Mileage,
                ExteriorColor = appraisal.SelectedExteriorColorValue ?? String.Empty,
                Certified = false,
                AdditionalOptions = appraisal.SelectedFactoryOptions,
                AdditionalPackages = appraisal.SelectedPackageOptions,
                OptionCodes = appraisal.AfterSelectedOptionCodes,
                Descriptions = appraisal.Descriptions,
                AppraisalDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                AppraisalId = String.IsNullOrEmpty(appraisal.AppraisalGenerateId)
                    ? 0
                    : Convert.ToInt32(appraisal.AppraisalGenerateId),
                ACV = appraisal.ACV,
                DealerCost = Convert.ToDecimal(appraisal.DealerCost),
                AppraisalById = appraisal.AppraisalById,
                AppraisalType = appraisal.AppraisalType ?? String.Empty,
                CARFAXOwner = (appraisal.CarFax == null || String.IsNullOrEmpty(appraisal.CarFax.NumberofOwners))
                    ? -1
                    : Convert.ToInt32(appraisal.CarFax.NumberofOwners),
                PackageDescription = appraisal.SelectedPackagesDescription,
                Location = appraisal.Location,
                Note = appraisal.Notes,
                //Descriptions = appraisal.Notes,
                IsManualDecode = appraisal.IsManualDecode,
            };
        }

        public static Vehicle ConvertToVehicle(AppraisalViewFormModel appraisal)
        {
            return new Vehicle()
            {
                Year = appraisal.ModelYear,
                Make = appraisal.Make ?? String.Empty,
                Model = appraisal.AppraisalModel ?? String.Empty,
                Trim = appraisal.SelectedTrim,
                Vin = appraisal.VinNumber ?? String.Empty,
                Msrp = appraisal.MSRP,
                ColorCode = appraisal.SelectedExteriorColorCode ?? String.Empty,
                InteriorColor = appraisal.SelectedInteriorColor ?? String.Empty,
                BodyType = appraisal.SelectedBodyType ?? String.Empty,
                Cylinders = appraisal.SelectedCylinder,
                Litter = appraisal.SelectedLiters,
                EngineType = appraisal.EngineType ?? String.Empty,
                DriveTrain = appraisal.SelectedDriveTrain ?? String.Empty,
                FuelType = appraisal.SelectedFuel ?? String.Empty,
                Tranmission = appraisal.SelectedTranmission ?? String.Empty,
                Doors = appraisal.Door,
                DefaultStockImage = appraisal.DefaultImageUrl ?? String.Empty,
                StandardOptions = appraisal.StandardInstalledOption,
                ChromeModelId = appraisal.ChromeModelId ?? String.Empty,
                ChromeStyleId = appraisal.ChromeStyleId ?? String.Empty,
                FuelEconomyCity = appraisal.FuelEconomyCity ?? String.Empty,
                FuelEconomyHighWay = appraisal.FuelEconomyHighWay ?? String.Empty,
                TruckType = appraisal.SelectedTruckType,
                TruckCategoryId = appraisal.SelectedTruckCategoryId==0?(int?)null:appraisal.SelectedTruckCategoryId,
                TruckClassId = appraisal.SelectedTruckClassId==0?(int?)null:appraisal.SelectedTruckClassId,
                VehicleType =(appraisal.SelectedVehicleType==null || !appraisal.SelectedVehicleType.Equals("Truck")) ? Constanst.VehicleType.Car : Constanst.VehicleType.Truck,
            };


        }

        public static AppraisalCustomer ConvertToAppraisalCustomer(AppraisalViewFormModel appraisal)
        {
            return new AppraisalCustomer()
            {
                FirstName = appraisal.CustomerFirstName ?? String.Empty,
                LastName = appraisal.CustomerLastName ?? String.Empty,
                Address = appraisal.CustomerAddress ?? String.Empty,
                City = appraisal.CustomerCity ?? String.Empty,
                State = appraisal.CustomerState ?? String.Empty,
                ZipCode = appraisal.CustomerZipCode ?? String.Empty,
            };
        }

        public static Inventory ConvertToInventory(Appraisal appraisal, DealershipViewModel dealer)
        {
            return new Inventory()
            {
                AppraisalId = appraisal.AppraisalId,
                ACV = appraisal.ACV,
                Mileage = appraisal.Mileage,
                Stock = appraisal.Stock,
                CarfaxOwner = appraisal.CARFAXOwner,
                Condition = Constanst.ConditionStatus.Used,
                DealerId = appraisal.DealerId,
                VehicleId = appraisal.VehicleId,
                Unwind = false,
                RetailPrice = 0,
                SalePrice = 0,
                PriorRental = false,
                LastUpdated = DateTime.Now,
                WarrantyInfo = 0,
                WindowStickerPrice = 0,
                ACar = false,
                MarketTrim = appraisal.MarketTrim,
                AdditionalOptions = appraisal.AdditionalOptions,
                AdditionalPackages = appraisal.AdditionalPackages,
                OptionCodes = appraisal.OptionCodes,
                AdditionalTitle = string.Empty,
                DateInStock = DateTime.Now,
                DealerCost = appraisal.DealerCost,
                DealerDemo = false,
                DealerDiscount = 0,
                DealerMsrp = 0,
                Descriptions = appraisal.Descriptions,
                Disclaimer = string.Empty,
                ExteriorColor = appraisal.ExteriorColor,
                IsFeatured = false,
                Dealer = appraisal.Dealer,
                Vehicle = appraisal.Vehicle,
                IsManualDecode = appraisal.IsManualDecode,
                PhotoUrl =
                    dealer.DealerSetting.OverideStockImage
                        ? (String.IsNullOrEmpty(dealer.DealerSetting.DefaultStockImageUrl)
                            ? ""
                            : dealer.DealerSetting.DefaultStockImageUrl)
                        : "",

                ThumbnailUrl =
                    dealer.DealerSetting.OverideStockImage
                        ? (String.IsNullOrEmpty(dealer.DealerSetting.DefaultStockImageUrl)
                            ? ""
                            : dealer.DealerSetting.DefaultStockImageUrl)
                        : ""

            };
        }

        public static Inventory ConvertToInventory(SoldoutInventory soldoutInventory, short inventoryStatus)
        {
            return new Inventory()
            {
                ACV = soldoutInventory.ACV,
                Mileage = soldoutInventory.Mileage,
                Stock = soldoutInventory.Stock,
                CarfaxOwner = soldoutInventory.CarFaxOwner,
                ThumbnailUrl = soldoutInventory.ThumbnailUrl,
                DealerId = soldoutInventory.DealerId,
                PhotoUrl = soldoutInventory.PhotoUrl,
                VehicleId = soldoutInventory.VehicleId,
                Unwind = soldoutInventory.Unwind,
                RetailPrice = soldoutInventory.RetailPrice,
                SalePrice = soldoutInventory.SalePrice,
                PriorRental = soldoutInventory.PriorRental,
                LastUpdated = soldoutInventory.LastUpdated,
                WarrantyInfo = soldoutInventory.WarrantyInfo,
                WindowStickerPrice = soldoutInventory.WindowStickerPrice,
                ACar = soldoutInventory.ACar,
                MarketTrim = soldoutInventory.MarketTrim,
                AddToInventoryById = soldoutInventory.AddToInventoryById,
                AdditionalOptions = soldoutInventory.AdditionalOptions,
                AdditionalPackages = soldoutInventory.AdditionalPackages,
                OptionCodes = soldoutInventory.OptionCodes,
                AdditionalTitle = soldoutInventory.AdditionalTitle,
                AppraisalId = soldoutInventory.AppraisalId,
                BrandedTitle = soldoutInventory.BrandedTitle,
                BucketJumpCompleteDay = soldoutInventory.BucketJumpCompleteDay,
                CarRanking = soldoutInventory.CarRanking,
                Certified = soldoutInventory.Certified,
                Condition = soldoutInventory.Condition,
                DateInStock = soldoutInventory.DateInStock,
                DealerCost = soldoutInventory.DealerCost,
                DealerDemo = soldoutInventory.DealerDemo,
                DealerDiscount = soldoutInventory.DealerDiscount,
                DealerMsrp = soldoutInventory.DealerMsrp,
                Descriptions = soldoutInventory.Descriptions,
                Disclaimer = soldoutInventory.Disclaimer,
                ExteriorColor = soldoutInventory.ExteriorColor,
                IsFeatured = soldoutInventory.IsFeatured,
                Dealer = soldoutInventory.Dealer,
                Vehicle = soldoutInventory.Vehicle,
                InventoryStatusCodeId = inventoryStatus
            };
        }

        public static SoldoutInventory ConvertToSoldoutInventory(Inventory inventory, string userName, CustomeInfoModel customer)
        {
            return new SoldoutInventory()
            {
                ACV = inventory.ACV,
                Mileage = inventory.Mileage.GetValueOrDefault(),
                Stock = inventory.Stock,
                CarFaxOwner = inventory.CarfaxOwner,
                InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale,
                ThumbnailUrl = inventory.ThumbnailUrl,
                DealerId = inventory.DealerId,
                PhotoUrl = inventory.PhotoUrl,
                VehicleId = inventory.VehicleId,
                Unwind = inventory.Unwind,
                RetailPrice = inventory.RetailPrice,
                SalePrice = inventory.SalePrice,
                PriorRental = inventory.PriorRental,
                LastUpdated = inventory.LastUpdated,
                WarrantyInfo = inventory.WarrantyInfo,
                WindowStickerPrice = inventory.WindowStickerPrice,
                ACar = inventory.ACar,
                MarketTrim = inventory.MarketTrim,
                AddToInventoryById = inventory.AddToInventoryById,
                AdditionalOptions = inventory.AdditionalOptions,
                AdditionalPackages = inventory.AdditionalPackages,
                AdditionalTitle = inventory.AdditionalTitle,
                AppraisalId = inventory.AppraisalId,
                BrandedTitle = inventory.BrandedTitle,
                BucketJumpCompleteDay = inventory.BucketJumpCompleteDay,
                CarRanking = inventory.CarRanking,
                Certified = inventory.Certified,
                Condition = inventory.Condition,
                DateInStock = inventory.DateInStock,
                DealerCost = inventory.DealerCost,
                DealerDemo = inventory.DealerDemo,
                DealerDiscount = inventory.DealerDiscount,
                DealerMsrp = inventory.DealerMsrp,
                Descriptions = inventory.Descriptions,
                Disclaimer = inventory.Disclaimer,
                ExteriorColor = inventory.ExteriorColor,
                IsFeatured = inventory.IsFeatured,
                Dealer = inventory.Dealer,
                Vehicle = inventory.Vehicle,
                RemoveBy = userName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                Country = customer.Country,
                DateRemoved = DateTime.Now,
                Street = customer.Street,
                OptionCodes = inventory.OptionCodes
            };
        }

        public static SoldoutInventory ConvertToSoldoutInventory(Appraisal appraisal, string userName, CustomeInfoModel customer)
        {
            return new SoldoutInventory()
            {
                AppraisalId = appraisal.AppraisalId,
                ACV = appraisal.ACV,
                Mileage = appraisal.Mileage.GetValueOrDefault(),
                Stock = appraisal.Stock,
                CarFaxOwner = appraisal.CARFAXOwner,
                InventoryStatusCodeId = Constanst.InventoryStatus.Wholesale,
                ThumbnailUrl = appraisal.ThumbnailUrl,
                DealerId = appraisal.DealerId,
                PhotoUrl = appraisal.PhotoUrl,
                VehicleId = appraisal.VehicleId,
                SalePrice = appraisal.SalePrice,
                LastUpdated = appraisal.LastUpdated,
                MarketTrim = appraisal.MarketTrim,
                AdditionalOptions = appraisal.AdditionalOptions,
                AdditionalPackages = appraisal.AdditionalPackages,
                CarRanking = appraisal.CarRanking,
                Certified = appraisal.Certified,
                DealerCost = appraisal.DealerCost,
                Descriptions = appraisal.Descriptions,
                ExteriorColor = appraisal.ExteriorColor,
                Dealer = appraisal.Dealer,
                Vehicle = appraisal.Vehicle,
                RemoveBy = userName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                Country = customer.Country,
                DateRemoved = DateTime.Now,
                Street = customer.Street,
                OptionCodes = appraisal.OptionCodes
            };
        }
    }
}