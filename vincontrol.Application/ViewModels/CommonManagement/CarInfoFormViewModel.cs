using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.AdvancedSearch;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using ChartSelection = vincontrol.Data.Model.ChartSelection;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class SelectDetailListItem : SelectListItem
    {
        public string Description { get; set; }
    }

    public interface ISelectedTrimItem
    {
        string SelectedTrimItem { get; set; }
    }

    public interface ICarAutoLoanPayment
    {
        double Monthsof60Payment { get; set; }
        double Monthsof48Payment { get; set; }
        double Monthsof36Payment { get; set; }

    }

    public class CarInfoFormViewModel : ISelectedTrimItem, ICarAutoLoanPayment
    {
        public static int GetHealthLevel(Inventory tmp)
        {
            int hasImage = String.IsNullOrEmpty(tmp.ThumbnailUrl) ? 1 : 0;

            int hasDescription = String.IsNullOrEmpty(tmp.Descriptions) ? 1 : 0;

            int hasPrice = tmp.SalePrice.GetValueOrDefault() > 0 ? 0 : 1;

            if (!String.IsNullOrEmpty(tmp.ThumbnailUrl) && !String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage))
            {
                hasImage = tmp.ThumbnailUrl.Equals(tmp.Vehicle.DefaultStockImage) ? 1 : 0;
            }


            int valueReturn = hasImage + hasDescription + hasPrice;

            return valueReturn;
        }

        public CarInfoFormViewModel() { }

        public CarInfoFormViewModel(Inventory tmp)
        {
            DealerId = tmp.DealerId;
            ListingId = tmp.InventoryId;
            OrginalName = tmp.Vehicle.Year.GetValueOrDefault() + " " + tmp.Vehicle.Make + " " + tmp.Vehicle.Model + " " + tmp.Vehicle.Trim;
            VehicleId = tmp.Vehicle.VehicleId;
            Year = tmp.Vehicle.Year.GetValueOrDefault();
            ModelYear = tmp.Vehicle.Year.GetValueOrDefault();
            Stock = tmp.Stock;
            Model = tmp.Vehicle.Model;
            VehicleModel = tmp.Vehicle.Model;
            Make = tmp.Vehicle.Make;
            Mileage = tmp.Mileage.GetValueOrDefault();
            Trim = tmp.Vehicle.Trim;
            Title = tmp.AdditionalTitle ?? string.Empty;
            ChromeStyleId = tmp.Vehicle.ChromeStyleId;
            ChromeModelId = tmp.Vehicle.ChromeModelId;
            Vin = tmp.Vehicle.Vin;
            ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            InteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            HasImage = !String.IsNullOrEmpty(tmp.PhotoUrl);
            HasDescription = !String.IsNullOrEmpty(tmp.Descriptions);
            HasSalePrice = tmp.SalePrice.HasValue;
            IsSold = false;
            IsCertified = tmp.Certified.GetValueOrDefault();
            PriorRental = tmp.PriorRental.GetValueOrDefault();
            CarName = tmp.Vehicle.Year + " " + tmp.Vehicle.Make + " " + tmp.Vehicle.Model;
            DateInStock = tmp.DateInStock.GetValueOrDefault();
            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days ==
                              DateTime.Now.Subtract(DateTime.MinValue).Days
                                  ? -1
                                  : DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
            HealthLevel = GetHealthLevel(tmp);
            CarImageUrl = String.IsNullOrEmpty(tmp.PhotoUrl) ? "" : tmp.PhotoUrl;
            CarThumbnailUrl = tmp.ThumbnailUrl;
            Cylinder = tmp.Vehicle.Cylinders.GetValueOrDefault();
            Litter = tmp.Vehicle.Litter.GetValueOrDefault();
            Door = tmp.Vehicle.Doors.GetValueOrDefault();
            Fuel = tmp.Vehicle.FuelType;
            Msrp =tmp.DealerMsrp.GetValueOrDefault()>0?tmp.DealerMsrp.GetValueOrDefault(): tmp.Vehicle.Msrp.GetValueOrDefault();

            WheelDrive =
                String.IsNullOrEmpty(tmp.Vehicle.DriveTrain) ? "" : tmp.Vehicle.DriveTrain;

            DefaultImageUrl =
                String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage)
                    ? ""
                    : tmp.Vehicle.DefaultStockImage;

            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                              ? tmp.Vehicle.DefaultStockImage
                              : tmp.ThumbnailUrl.Split(new string[] { ",", "|" },
                                                      StringSplitOptions.RemoveEmptyEntries).
                                    FirstOrDefault();

            ThumbnailPhotosUrl = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                                                ? null
                                                : (from data in tmp.ThumbnailUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();
            UploadPhotosUrl = String.IsNullOrEmpty(tmp.PhotoUrl)
                                                ? new List<string>()
                                                : (from data in tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            ExistOptions = String.IsNullOrEmpty(tmp.AdditionalOptions)
                                         ? null
                                         : (from data in tmp.AdditionalOptions.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            ExistPackages = String.IsNullOrEmpty(tmp.AdditionalPackages)
                                          ? null
                                          : (from data in tmp.AdditionalPackages.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();
            CarsOptions = tmp.AdditionalOptions;
            StandardOptions = String.IsNullOrEmpty(tmp.Vehicle.StandardOptions)
                                         ? null
                                         : (from data in tmp.Vehicle.StandardOptions.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            SalePrice = tmp.SalePrice.GetValueOrDefault();
            Price = (tmp.SalePrice.GetValueOrDefault());
            MarketRange = tmp.MarketRange.GetValueOrDefault();
            Reconstatus = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Recon;
            CarFaxOwner = tmp.CarfaxOwner.GetValueOrDefault();
            IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            IsTruck = false;
            ACar = tmp.ACar.GetValueOrDefault();
            MarketLowestPrice = tmp.Vehicle.MarketLowestPrice.GetValueOrDefault().ToString("c0");
            MarketAveragePrice = tmp.Vehicle.MarketAveragePrice.GetValueOrDefault().ToString("c0");
            MarketHighestPrice = tmp.Vehicle.MarketHighestPrice.GetValueOrDefault().ToString("c0");
            IsTruck = tmp.Vehicle.VehicleType == Constanst.VehicleType.Truck;
            Loaner = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner;
            Auction = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Auction;
            CarRanking = tmp.CarRanking.GetValueOrDefault();
            NumberOfCar = tmp.NumberOfCar.GetValueOrDefault();
            SalePrice = tmp.SalePrice.GetValueOrDefault();
            DealerCost = tmp.DealerCost;
            Acv = tmp.ACV;
            RetailPrice = tmp.RetailPrice.GetValueOrDefault();
            DealerDiscount = tmp.DealerDiscount.GetValueOrDefault();
            WindowStickerPrice = tmp.WindowStickerPrice.GetValueOrDefault();
            ManufacturerRebate = tmp.ManufacturerRebate.GetValueOrDefault();
            IsCertified = tmp.Certified.GetValueOrDefault();
            PriorRental = tmp.PriorRental.GetValueOrDefault();
            Description = tmp.Descriptions;

            BodyType = tmp.Vehicle.BodyType ?? string.Empty;
            WheelDrive = tmp.Vehicle.DriveTrain ?? string.Empty;
            WarrantyInfo = tmp.WarrantyInfo.GetValueOrDefault();
            BrandedTitle = tmp.BrandedTitle.GetValueOrDefault();
            DealerDemo = tmp.DealerDemo.GetValueOrDefault();
            Unwind = tmp.Unwind.GetValueOrDefault();
            Tranmission = tmp.Vehicle.Tranmission;
            Engine = tmp.Vehicle.EngineType;

            SelectedExteriorColorValue = tmp.ExteriorColor ?? string.Empty;
            SelectedExteriorColorCode = tmp.Vehicle.ColorCode ?? string.Empty;
            SelectedInteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            CusExteriorColor = tmp.ExteriorColor ?? string.Empty;
            CusInteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            Condition = tmp.Condition;
            CarFaxOwner = tmp.CarfaxOwner.GetValueOrDefault();
            InventoryStatus = tmp.InventoryStatusCodeId;
            VehicleStatusCodeId = Constanst.VehicleStatus.Inventory;
            HasSalePrice = SalePrice > 0;
            Type = tmp.Condition == Constanst.ConditionStatus.New ? Constanst.CarInfoType.New : Constanst.CarInfoType.Used;
            InventoryStatus = tmp.InventoryStatusCodeId;
            if (String.IsNullOrEmpty(tmp.ThumbnailUrl))
            {
                HasImage = false;
            }
            else
            {
                var splitArray =
                    tmp.ThumbnailUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       .ToArray();

                if (splitArray.Count() > 1)
                {
                    HasImage = true;

                }
                else
                {

                    if (!String.IsNullOrEmpty(SinglePhoto) &&
                        !String.IsNullOrEmpty(tmp.Dealer.Setting.DefaultStockImageUrl) &&
                        SinglePhoto.Equals(tmp.Dealer.Setting.DefaultStockImageUrl))
                    {
                        HasImage = false;
                    }

                }
            }

            TruckType = String.IsNullOrEmpty(tmp.Vehicle.TruckType) ? string.Empty : tmp.Vehicle.TruckType;
            SelectedTruckType = String.IsNullOrEmpty(tmp.Vehicle.TruckType) ? string.Empty : tmp.Vehicle.TruckType;
            SelectedTruckCategoryId = tmp.Vehicle.TruckCategoryId.GetValueOrDefault();

            try
            {
                TruckCategory = tmp.Vehicle.TruckCategory != null ? tmp.Vehicle.TruckCategory.CategoryName : string.Empty;
            }
            catch (Exception) { TruckCategory = string.Empty; }

            SelectedTruckClassId = tmp.Vehicle.TruckClassId.GetValueOrDefault();

            try
            {
                TruckClass = tmp.Vehicle.TruckClass != null ? tmp.Vehicle.TruckClass.ClassName : string.Empty;
            }
            catch (Exception) { TruckClass = string.Empty; }

            IsCommercialTruck = (SelectedTruckType.Equals(Constanst.TruckType.Trailer) || SelectedTruckType.Equals(Constanst.TruckType.Truck) || SelectedTruckType.Equals(Constanst.TruckType.TruckBody))

            && SelectedTruckCategoryId > 0 && SelectedTruckClassId > 0;
        }

        public CarInfoFormViewModel(SoldoutInventory tmp)
        {
            DealerId = tmp.DealerId;
            ListingId = tmp.SoldoutInventoryId;
            OrginalName = tmp.Vehicle.Year.GetValueOrDefault() + " " + tmp.Vehicle.Make + " " + tmp.Vehicle.Model + " " + tmp.Vehicle.Trim;
            VehicleId = tmp.Vehicle.VehicleId;
            Year = tmp.Vehicle.Year.GetValueOrDefault();
            ModelYear = tmp.Vehicle.Year.GetValueOrDefault();
            Stock = tmp.Stock;
            Model = tmp.Vehicle.Model;
            VehicleModel = tmp.Vehicle.Model;
            Make = tmp.Vehicle.Make;
            Mileage = tmp.Mileage.GetValueOrDefault();
            Trim = tmp.Vehicle.Trim;
            Title = tmp.AdditionalTitle ?? string.Empty;
            ChromeStyleId = tmp.Vehicle.ChromeStyleId;
            ChromeModelId = tmp.Vehicle.ChromeModelId;
            Vin = tmp.Vehicle.Vin;
            ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            InteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            HasImage = !String.IsNullOrEmpty(tmp.PhotoUrl);
            HasDescription = !String.IsNullOrEmpty(tmp.Descriptions);
            HasSalePrice = tmp.SalePrice.HasValue;
            CarName = tmp.Vehicle.Year + " " + tmp.Vehicle.Make + " " + tmp.Vehicle.Model;
            DateInStock = tmp.DateInStock.GetValueOrDefault();
            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days ==
                              DateTime.Now.Subtract(DateTime.MinValue).Days
                                  ? -1
                                  : DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
            CarImageUrl = String.IsNullOrEmpty(tmp.PhotoUrl) ? "" : tmp.PhotoUrl;
            CarThumbnailUrl = tmp.ThumbnailUrl;
            Cylinder = tmp.Vehicle.Cylinders.GetValueOrDefault();
            Litter = tmp.Vehicle.Litter.GetValueOrDefault();
            Door = tmp.Vehicle.Doors.GetValueOrDefault();
            Fuel = tmp.Vehicle.FuelType;
            Msrp = tmp.Vehicle.Msrp.GetValueOrDefault();

            DefaultImageUrl =
                String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage)
                    ? ""
                    : tmp.Vehicle.DefaultStockImage;

            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                              ? tmp.Vehicle.DefaultStockImage
                              : tmp.ThumbnailUrl.Split(new string[] { ",", "|" },
                                                      StringSplitOptions.RemoveEmptyEntries).
                                    FirstOrDefault();

            ThumbnailPhotosUrl = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                                                ? null
                                                : (from data in tmp.ThumbnailUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();
            UploadPhotosUrl = String.IsNullOrEmpty(tmp.PhotoUrl)
                                                ? null
                                                : (from data in tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            ExistOptions = String.IsNullOrEmpty(tmp.AdditionalOptions)
                                         ? null
                                         : (from data in tmp.AdditionalOptions.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            ExistPackages = String.IsNullOrEmpty(tmp.AdditionalPackages)
                                          ? null
                                          : (from data in tmp.AdditionalPackages.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();
            CarsOptions = tmp.AdditionalOptions;
            StandardOptions = String.IsNullOrEmpty(tmp.Vehicle.StandardOptions)
                                         ? null
                                         : (from data in tmp.Vehicle.StandardOptions.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            SalePrice = tmp.SalePrice.GetValueOrDefault();
            Price = (tmp.SalePrice.GetValueOrDefault());
            Reconstatus = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Recon;
            CarFaxOwner = tmp.CarFaxOwner.GetValueOrDefault();
            IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            IsTruck = false;
            ACar = tmp.ACar.GetValueOrDefault();
            MarketLowestPrice = tmp.Vehicle.MarketLowestPrice.GetValueOrDefault().ToString("c0");
            MarketAveragePrice = tmp.Vehicle.MarketAveragePrice.GetValueOrDefault().ToString("c0");
            MarketHighestPrice = tmp.Vehicle.MarketHighestPrice.GetValueOrDefault().ToString("c0");
            IsTruck = tmp.Vehicle.VehicleType == Constanst.VehicleType.Truck;
            Loaner = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner;
            Auction = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Auction;
            CarRanking = tmp.CarRanking.GetValueOrDefault();
            SalePrice = tmp.SalePrice.GetValueOrDefault();
            DealerCost = tmp.DealerCost;
            Acv = tmp.ACV;
            RetailPrice = tmp.RetailPrice.GetValueOrDefault();
            DealerDiscount = tmp.DealerDiscount.GetValueOrDefault();
            WindowStickerPrice = tmp.WindowStickerPrice.GetValueOrDefault();
            ManufacturerRebate = tmp.ManufacturerRebate.GetValueOrDefault();
            IsCertified = tmp.Certified.GetValueOrDefault();
            PriorRental = tmp.PriorRental.GetValueOrDefault();
            Description = tmp.Descriptions;

            BodyType = tmp.Vehicle.BodyType ?? string.Empty;
            WheelDrive = tmp.Vehicle.DriveTrain ?? string.Empty;
            WarrantyInfo = tmp.WarrantyInfo.GetValueOrDefault();
            BrandedTitle = tmp.BrandedTitle.GetValueOrDefault();
            DealerDemo = tmp.DealerDemo.GetValueOrDefault();
            Unwind = tmp.Unwind.GetValueOrDefault();
            Tranmission = tmp.Vehicle.Tranmission;
            Engine = tmp.Vehicle.EngineType;

            SelectedExteriorColorValue = tmp.ExteriorColor ?? string.Empty;
            SelectedExteriorColorCode = tmp.Vehicle.ColorCode ?? string.Empty;
            SelectedInteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            CusExteriorColor = tmp.ExteriorColor ?? string.Empty;
            CusInteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            Condition = Constanst.ConditionStatus.Used; //tmp.Condition;
            InventoryStatus = Constanst.InventoryStatus.SoldOut;
            Type = Constanst.CarInfoType.Sold;
            VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut;
            IsSold = true;
            HasSalePrice = SalePrice > 0;
            InventoryStatus = Constanst.InventoryStatus.SoldOut;
            TruckType = String.IsNullOrEmpty(tmp.Vehicle.TruckType) ? string.Empty : tmp.Vehicle.TruckType;
            SelectedTruckType = String.IsNullOrEmpty(tmp.Vehicle.TruckType) ? string.Empty : tmp.Vehicle.TruckType;
            SelectedTruckCategoryId = tmp.Vehicle.TruckCategoryId.GetValueOrDefault();

            try
            {
                TruckCategory = tmp.Vehicle.TruckCategory != null ? tmp.Vehicle.TruckCategory.CategoryName : string.Empty;
            }
            catch (Exception) { TruckCategory = string.Empty; }

            SelectedTruckClassId = tmp.Vehicle.TruckClassId.GetValueOrDefault();

            try
            {
                TruckClass = tmp.Vehicle.TruckClass != null ? tmp.Vehicle.TruckClass.ClassName : string.Empty;
            }
            catch (Exception) { TruckClass = string.Empty; }
            IsCommercialTruck = (SelectedTruckType.Equals(Constanst.TruckType.Trailer) || SelectedTruckType.Equals(Constanst.TruckType.Truck) || SelectedTruckType.Equals(Constanst.TruckType.TruckBody))

            && SelectedTruckCategoryId > 0 && SelectedTruckClassId > 0;
        }

        public CarInfoFormViewModel(AdvanceSearchItem tmp)
        {
            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                             ? tmp.Vehicle.DefaultStockImage
                             : tmp.ThumbnailUrl.Split(new string[] { ",", "|" },
                                                     StringSplitOptions.RemoveEmptyEntries).
                                   FirstOrDefault();
            ListingId = tmp.ListingId;
            VehicleId = tmp.VehicleId;
            VehicleId = tmp.Vehicle.VehicleId;
            Year = tmp.Vehicle.Year.GetValueOrDefault();
            ModelYear = tmp.Vehicle.Year.GetValueOrDefault();
            Stock = tmp.Stock;
            Model = tmp.Vehicle.Model;
            VehicleModel = tmp.Vehicle.Model;
            Make = tmp.Vehicle.Make;
            Mileage = tmp.Mileage.GetValueOrDefault();
            Trim = tmp.Vehicle.Trim;
            //Title = tmp.AdditionalTitle ?? string.Empty;
            ChromeStyleId = tmp.Vehicle.ChromeStyleId;
            ChromeModelId = tmp.Vehicle.ChromeModelId;
            DealerId = tmp.DealerId;
            Stock = tmp.Stock ?? string.Empty;
            Mileage = tmp.Mileage ?? 0;
            ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            SinglePhoto = tmp.PhotoUrl;
            SalePrice = tmp.SalePrice ?? 0;
            DateInStock = tmp.DateInStock;
            MarketRange = tmp.MarketRange ?? 0;
            CarFaxOwner = tmp.CarfaxOwner;
            Description = tmp.Descriptions ?? string.Empty;
        }

        public static void UpdateCarInfoFormViewModel(CarInfoFormViewModel viewModel, Inventory tmp)
        {
            viewModel.OrginalName = tmp.Vehicle.Year.GetValueOrDefault() + " " + tmp.Vehicle.Make + " " + tmp.Vehicle.Model + " " + tmp.Vehicle.Trim;
            viewModel.ListingId = tmp.InventoryId;
            viewModel.VehicleId = tmp.Vehicle.VehicleId;
            viewModel.ModelYear = tmp.Vehicle.Year.GetValueOrDefault();
            viewModel.Stock = tmp.Stock;
            viewModel.Model = tmp.Vehicle.Model;
            viewModel.VehicleModel = tmp.Vehicle.Model;
            viewModel.Make = tmp.Vehicle.Make;
            viewModel.Mileage = tmp.Mileage.GetValueOrDefault();
            viewModel.Trim = tmp.Vehicle.Trim;
            viewModel.Title = tmp.AdditionalTitle ?? string.Empty;
            viewModel.ChromeStyleId = tmp.Vehicle.ChromeStyleId;
            viewModel.ChromeModelId = tmp.Vehicle.ChromeModelId;
            viewModel.Vin = tmp.Vehicle.Vin;
            viewModel.ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            viewModel.InteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            viewModel.HasImage = !String.IsNullOrEmpty(tmp.PhotoUrl);
            viewModel.HasDescription = !String.IsNullOrEmpty(tmp.Descriptions);
            viewModel.HasSalePrice = tmp.SalePrice.HasValue;
            viewModel.IsSold = false;
            viewModel.CarName = tmp.Vehicle.Year + " " + tmp.Vehicle.Make + " " + tmp.Vehicle.Model;
            viewModel.DateInStock = tmp.DateInStock.GetValueOrDefault();
            viewModel.DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
            viewModel.HealthLevel = GetHealthLevel(tmp);
            viewModel.CarImageUrl = String.IsNullOrEmpty(tmp.PhotoUrl) ? "" : tmp.PhotoUrl;
            viewModel.CarThumbnailUrl = String.IsNullOrEmpty(tmp.ThumbnailUrl) ? "" : tmp.ThumbnailUrl;
            viewModel.SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                              ? tmp.Vehicle.DefaultStockImage
                              : tmp.ThumbnailUrl.Split(new string[] { ",", "|" },
                                                      StringSplitOptions.RemoveEmptyEntries).
                                    FirstOrDefault();
            viewModel.UploadPhotosUrl = String.IsNullOrEmpty(tmp.PhotoUrl)
                                                ? null
                                                : (from data in tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();
            viewModel.SalePrice = tmp.SalePrice.GetValueOrDefault();
            viewModel.Price = (tmp.SalePrice.GetValueOrDefault());
            viewModel.MarketRange = tmp.MarketRange.GetValueOrDefault();
            viewModel.Reconstatus = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Recon;
            viewModel.CarFaxOwner = tmp.CarfaxOwner.GetValueOrDefault();
            viewModel.IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            viewModel.IsTruck = false;
            viewModel.ACar = tmp.ACar.GetValueOrDefault();
            viewModel.MarketLowestPrice = tmp.Vehicle.MarketLowestPrice.GetValueOrDefault().ToString("c0");
            viewModel.MarketAveragePrice = tmp.Vehicle.MarketAveragePrice.GetValueOrDefault().ToString("c0");
            viewModel.MarketHighestPrice = tmp.Vehicle.MarketHighestPrice.GetValueOrDefault().ToString("c0");
            viewModel.IsTruck = tmp.Vehicle.VehicleType.Equals(Constanst.VehicleType.Truck);
            viewModel.Loaner = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner;
            viewModel.Auction = tmp.InventoryStatusCodeId == Constanst.InventoryStatus.Auction;
            viewModel.CarRanking = tmp.CarRanking.GetValueOrDefault();
            viewModel.NumberOfCar = tmp.NumberOfCar.GetValueOrDefault();
            viewModel.SalePrice = tmp.SalePrice.GetValueOrDefault();
            viewModel.DealerCost = tmp.DealerCost;
            viewModel.Acv = tmp.ACV;
            viewModel.RetailPrice = tmp.RetailPrice.GetValueOrDefault();
            viewModel.DealerDiscount = tmp.DealerDiscount.GetValueOrDefault();
            viewModel.WindowStickerPrice = tmp.WindowStickerPrice.GetValueOrDefault();
            viewModel.ManufacturerRebate = tmp.ManufacturerRebate.GetValueOrDefault();
            viewModel.IsCertified = tmp.Certified.GetValueOrDefault();
            viewModel.PriorRental = tmp.PriorRental.GetValueOrDefault();

            viewModel.BodyType = tmp.Vehicle.BodyType ?? string.Empty;
            viewModel.WheelDrive = tmp.Vehicle.DriveTrain ?? string.Empty;
            viewModel.WarrantyInfo = tmp.WarrantyInfo.GetValueOrDefault();
            viewModel.BrandedTitle = tmp.BrandedTitle.GetValueOrDefault();
            viewModel.DealerDemo = tmp.DealerDemo.GetValueOrDefault();
            viewModel.Unwind = tmp.Unwind.GetValueOrDefault();

            viewModel.SelectedExteriorColorValue = tmp.ExteriorColor ?? string.Empty;
            viewModel.SelectedExteriorColorCode = tmp.Vehicle.ColorCode ?? string.Empty;
            viewModel.SelectedInteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            viewModel.CusExteriorColor = tmp.ExteriorColor ?? string.Empty;
            viewModel.CusInteriorColor = tmp.Vehicle.InteriorColor ?? string.Empty;
            viewModel.Condition = tmp.Condition;
            viewModel.Tranmission = tmp.Vehicle.Tranmission;

            viewModel.TruckType = tmp.Vehicle.TruckType;
            viewModel.SelectedTruckType = tmp.Vehicle.TruckType;
            viewModel.SelectedTruckCategoryId = tmp.Vehicle.TruckCategoryId.GetValueOrDefault();
            viewModel.TruckCategory = tmp.Vehicle.TruckCategory != null ? tmp.Vehicle.TruckCategory.CategoryName : string.Empty;
            viewModel.SelectedTruckClassId = tmp.Vehicle.TruckClassId.GetValueOrDefault();
            viewModel.TruckClass = tmp.Vehicle.TruckClass != null ? tmp.Vehicle.TruckClass.ClassName : string.Empty;
        }

        [Required]
        public string Vin { get; set; }

        public string SortVin { get; set; }

        public int ListingId { get; set; }

        public int VehicleId { get; set; }

        public string UrlImage { get; set; }

        public bool VinDecodeSuccess { get; set; }

        public string CarName { get; set; }

        public string Stock { get; set; }

        public decimal Msrp { get; set; }

        public decimal TotalMsrp { get; set; }

        public string OrginalName { get; set; }

        public string Title { get; set; }

        public int ModelYear { get; set; }

        public int Year { get; set; }

        public IEnumerable<ExtendedSelectListItem> ModelYears { get; set; }

        public string Make { get; set; }

        public string ShortMake { get; set; }

        public string Model { get; set; }

        public string ShortModel { get; set; }

        public string VehicleModel { get; set; }

        public string Trim { get; set; }

        public string StrTrim { get; set; }

        public string ChromeStyleId { get; set; }

        public string ChromeModelId { get; set; }

        public string ChromeModel { get; set; }

        public decimal? Acv { get; set; }

        public decimal? DealerCost { get; set; }

        public string Style { get; set; }

        public int Cylinder { get; set; }

        public IdentifiedString[] MakeNameList { get; set; }

        public string BodyType { get; set; }

        public int DealerId { get; set; }

        public string DealerName { get; set; }

        public string DealerAddress { get; set; }

        public string DealerPhoneNumber { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal WindowStickerPrice { get; set; }

        public decimal DealerDiscount { get; set; }

        public decimal ManufacturerRebate { get; set; }

        public decimal SalePrice { get; set; }

        public string StrSalePrice { get; set; }

        public string ZipCode { get; set; }

        public string ExteriorColor { get; set; }

        public string StrExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string CusExteriorColor { get; set; }

        public string CusInteriorColor { get; set; }

        public List<ChromeAutoService.AutomativeService.Color> ExteriorColorList { get; set; }

        public List<ChromeAutoService.AutomativeService.Color> InteriorColorList { get; set; }

        public List<IdentifiedString> ModelList { get; set; }

        public List<Style> StyleList { get; set; }

        public long Mileage { get; set; }

        public string StrMileage { get; set; }

        public string Description { get; set; }

        public List<ExtendedFactoryOptions> FactoryNonInstalledOptions { get; set; }

        public List<ExtendedFactoryOptions> FactoryPackageOptions { get; set; }

        public List<string> ExistOptions { get; set; }

        public List<string> ExistPackages { get; set; }

        public List<string> StandardPackages { get; set; }

        public List<string> StandardOptions { get; set; }

        public string CarImageUrl { get; set; }

        public string CarThumbnailUrl { get; set; }

        public int Door { get; set; }

        public string Fuel { get; set; }

        public double Litter { get; set; }

        public string Tranmission { get; set; }

        public string WheelDrive { get; set; }

        public string Engine { get; set; }

        public SelectList UserOrNew { get; set; }

        public List<SelectedOption> SelectedOptions { get; set; }

        public bool Success { get; set; }

        public List<string> MutiplePhotos { get; set; }

        public string SinglePhoto { get; set; }

        public string DefaultImageUrl { get; set; }

        public string CurrentDistance { get; set; }

        public int DistanceFromDealer { get; set; }

        public decimal AveragePriceOnMarket { get; set; }

        public decimal MilageDecimal { get; set; }

        public int MarketRange { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateInStock { get; set; }

        public int DaysInInvenotry { get; set; }

        public string StrDaysInInvenotry { get; set; }

        public int NumberofCarOnAutoTrader { get; set; }

        public int NumberofCarOnCarsCom { get; set; }

        public int NumberofCarOnEbay { get; set; }

        public decimal Price { get; set; }

        public Hashtable CarsOnMarket { get; set; }

        public List<string> ThumbnailPhotosUrl { get; set; }

        public List<string> UploadPhotosUrl { get; set; }

        public decimal LowestPrice { get; set; }

        public decimal HighestPrice { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public List<string> TrimList { get; set; }

        public string CarsOptions { get; set; }

        public string CarPackages { get; set; }

        public bool IsManual { get; set; }

        public string AppraisalId { get; set; }

        public List<string> MechanicalList { get; set; }

        public List<string> ExteriorList { get; set; }

        public List<string> EntertainmentList { get; set; }

        public List<string> InteriorList { get; set; }

        public List<string> SafetyList { get; set; }

        public string FuelEconomyCity { get; set; }

        public string FuelEconomyHighWay { get; set; }

        public int HealthLevel { get; set; }

        public bool IsElectric { get; set; }

        public bool HasImage { get; set; }

        public bool HasDescription { get; set; }

        public bool HasSalePrice { get; set; }

        public bool IsSold { get; set; }

        public bool IsCertified { get; set; }

        public bool PriorRental { get; set; }

        public bool DealerDemo { get; set; }

        public bool Unwind { get; set; }

        public string AppraisalType { get; set; }

        public string AppraisalGenerateId { get; set; }

        public int SoldOutDaysLeft { get; set; }

        public string LogoUrl { get; set; }

        //public bool Condition { get; set; }

        public string EbayHtmlSource { get; set; }

        public string CarFaxDealerId { get; set; }

        public CarFaxViewModel CarFax { get; set; }

        public KellyBlueBookViewModel KBB { get; set; }

        public BlackBookViewModel BB { get; set; }

        public bool SessionTimeOut { get; set; }

        public bool MultipleDealers { get; set; }

        public IEnumerable<ExtendedSelectListItem> TransferDealerGroup { get; set; }

        public string SelectedDealerTransfer { get; set; }

        public string NewStockNumber { get; set; }

        public DealerGroupViewModel DealerGroup { get; set; }

        public string SelectedDealer { get; set; }

        public IEnumerable<ExtendedSelectListItem> DealerList { get; set; }

        public bool IsTruck { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckTypeList { get; set; }

        public string TruckType { get; set; }

        public string SelectedTruckType { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckCategoryList { get; set; }

        public string TruckCategory { get; set; }

        public int SelectedTruckCategoryId { get; set; }

        public IEnumerable<ExtendedSelectListItem> TruckClassList { get; set; }

        public string TruckClass { get; set; }

        public int SelectedTruckClassId { get; set; }

        public int WarrantyInfo { get; set; }

        public int CarFaxOwner { get; set; }

        public string StrCarFaxOwner { get; set; }

        public bool Reconstatus { get; set; }

        public int CurrentScreen { get; set; }

        public IEnumerable<ExtendedSelectListItem> VehicleTypeList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeExteriorColorList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeInteriorColorList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeTranmissionList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ChromeDriveTrainList { get; set; }

        public string SelectedVehicleType { get; set; }

        //public string SelectedExteriorColor { get; set; }

        public string SelectedExteriorColorCode { get; set; }

        public string SelectedInteriorColor { get; set; }

        public string SelectedTranmission { get; set; }

        public string SelectedDriveTrain { get; set; }

        public IEnumerable<SelectDetailListItem> ChromeFactoryNonInstalledOptions { get; set; }

        public IEnumerable<SelectDetailListItem> ChromeFactoryPackageOptions { get; set; }

        public IEnumerable<ExtendedSelectListItem> EditTrimList { get; set; }
        // 0: New
        // 1: Used
        // 2: Wholesale
        // 3: Appraisal
        // 4: Sold
        public int Type { get; set; }

        public int PreviousListingId { get; set; }

        public int NextListingId { get; set; }

        public bool? IsFeatured { get; set; }

        public Chart.ChartSelection SavedSelections { get; set; }

        public int CarRanking { get; set; }

        public string MarketData { get; set; }

        public int NumberOfCar { get; set; }

        public string MarketLowestPrice { get; set; }

        public string MarketAveragePrice { get; set; }

        public string MarketHighestPrice { get; set; }

        public List<ManheimWholesaleViewModel> ManheimWholesales { get; set; }

        public bool UseColorCodeId { get; set; }

        public string SelectedExteriorColorValue { get; set; }

        public string CusTrim { get; set; }

        public IEnumerable<SmallKarPowerViewModel> KarPowerData { get; set; }

        public bool Loaner { get; set; }

        public bool Auction { get; set; }

        public string SelectedPackagesDescription { get; set; }

        public bool EnableAutoDescriptionSetting { get; set; }

        public bool? Condition { get; set; }

        public int? KBBTrimId { get; set; }

        public string KBBOptionsId { get; set; }

        public bool NotDoneBucketJump { get; set; }

        public bool ACar { get; set; }

        public bool BrandedTitle { get; set; }

        public IEnumerable<WarrantyTypeViewModel> WarrantyTypes { get; set; }

        public string SelectedTrimItem { get; set; }

        public IList<ButtonPermissionViewModel> ButtonPermissions { get; set; }

        public string AfterSelectedPackage { get; set; }

        public string AfterSelectedOptions { get; set; }

        public string AfterSelectedOptionCodes{ get; set; }

        public string URLDetail { get; set; }

        public string URLEdit { get; set; }

        public string URLEbay { get; set; }

        public string URLWS { get; set; }

        public string URLBG { get; set; }

        public string ClassFilter { get; set; }

        public short InventoryStatus { get; set; }

        public short VehicleStatusCodeId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsCommercialTruck { get; set; }

        public Dealer Dealer { get; set; }

        public User CurrentUser { get; set; }

        public double Monthsof60Payment { get; set; }
        public double Monthsof48Payment { get; set; }
        public double Monthsof36Payment { get; set; }
    }

    
    public class AutoLoanCalculatorModel
    {
        public double VehiclePrice { get; set; }

        public double DownPayment { get; set; }

        public double TradeInValue { get; set; }

        public double SaleTax { get; set; }

        public double InterestRate { get; set; }

        public int Terms { get; set; }

        public double MonthlyPayment { get; set; }

        public double Principal { get; set; }

        public double TotalInterest { get; set; }

        public double TotalToPay { get; set; }

        public double FinalCost { get; set; }

    }
}
