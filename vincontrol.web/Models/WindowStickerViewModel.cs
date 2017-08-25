using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.HelperClass;
//using Vincontrol.Web.com.chromedata.services.Description7a;

namespace Vincontrol.Web.Models
{
    public class WindowStickerViewModel
    {
        //public override string ToString()
        //{
        //    return string.Format("Age:{0}, ListingId:{1}", DaysInInvenotry, ListingId);
        //}

        public WindowStickerViewModel()
        {
        //    ListingId = tmp.InventoryId;
        //    Year = tmp.Vehicle.Year.GetValueOrDefault();
        //    Stock = tmp.Stock;
        //    Model = tmp.Vehicle.Model;
        //    Make = tmp.Vehicle.Make;
        //    Mileage = tmp.Mileage.GetValueOrDefault();
        //    Trim = tmp.Vehicle.Trim;
        //    Vin = tmp.Vehicle.Vin;
        //    ExteriorColor = tmp.ExteriorColor ?? string.Empty;
        //    DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
        //    SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
        //                      ? tmp.Vehicle.DefaultStockImage
        //                      : tmp.ThumbnailUrl.Split(new string[] { ",", "|" },
        //                                              StringSplitOptions.RemoveEmptyEntries).
        //                            FirstOrDefault();
        //    SalePrice = tmp.SalePrice.GetValueOrDefault();
        //    MarketRange = tmp.MarketRange.GetValueOrDefault();
        //    IsFeatured = tmp.IsFeatured.GetValueOrDefault();
        //    CarRanking = tmp.CarRanking.GetValueOrDefault();
        //    NumberOfCar = tmp.NumberOfCar.GetValueOrDefault();
        //    SalePrice = tmp.SalePrice.GetValueOrDefault();
        //    Condition = tmp.Condition;
        //    InventoryStatus = tmp.InventoryStatusCodeId;
        //    HealthLevel = LogicHelper.GetHealthLevel(tmp);
        //    CarFaxOwner = tmp.CarfaxOwner??0;
        //    IsUsed = tmp.Condition;
        }

        //public WindowStickerViewModel(SoldoutInventory tmp)
        //{
        //    //ListingId = tmp.SoldoutInventoryId;
        //    //Year = tmp.Vehicle.Year.GetValueOrDefault();
        //    //Stock = tmp.Stock;
        //    //Model = tmp.Vehicle.Model;
        //    //Make = tmp.Vehicle.Make;
        //    //Mileage = tmp.Mileage.GetValueOrDefault();
        //    //Trim = tmp.Vehicle.Trim;
        //    //Vin = tmp.Vehicle.Vin;
        //    //ExteriorColor = tmp.ExteriorColor ?? string.Empty;
        //    //DaysInInvenotry = DateTime.Now.Subtract(tmp.DateRemoved.GetValueOrDefault()).Days;
        //    //SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
        //    //                  ? tmp.Vehicle.DefaultStockImage
        //    //                  : tmp.ThumbnailUrl.Split(new string[] {",", "|"},
        //    //                                          StringSplitOptions.RemoveEmptyEntries).
        //    //                        FirstOrDefault();
        //    //SalePrice = tmp.SalePrice.GetValueOrDefault();
        //    //IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            
        //    //Condition = tmp.Condition;
        //    //CarFaxOwner = tmp.CarFaxOwner??0;
        //    //IsUsed = tmp.Condition;
        //}

        public string BarCodeUrl { get; set; }
        public string Title { get; set; }
        public string Trim { get; set; }
        public int? Cylinders { get; set; }
        public string FuelType { get; set; }

        public int Mileage
        {
            get; set; }

        public string Stock { get; set; }

        public string ExteriorColor
        {
            get;
            set;
        }

        public string Transmission
        {
            get;
            set;
        }

        public List<string> PackageAndOptions
        {
            get;
            set;
        }

        public decimal? RetailPrice
        {
            get;
            set;
        }

        public decimal? DealerDiscount
        {
            get;
            set;
        }

        public decimal? SalePrice
        {
            get;
            set;
        }

        public decimal? ManufacturerRebate { get; set; }

        public string RetailPriceText
        {
            get;
            set;
        }

        public string DealerDiscountText
        {
            get;
            set;
        }

        public string SalePriceText
        {
            get;
            set;
        }

        public string ManufacturerRebateText { get; set; }

        public string VIN
        {
            get;
            set;
        }

        public string InteriorColor { get; set; }

        public string Engine { get; set; }

        public int FuelEconomyCity { get; set; }

        public int FuelEconomyHighWay { get; set; }
        public int FuelEconomyAverage
        {
            get { return ((FuelEconomyCity + FuelEconomyHighWay)/2); }
        }

        public List<string> StandardOptions { get; set; }

        public string TemplateUrl { get; set; }

        //    [Required]
    //    public string Vin { get; set; }
    //    public int ListingId { get; set; }
    //    public string Stock { get; set; }
    //    public int Year { get; set; }
    //    public string Make { get; set; }
    //    public string Model { get; set; }
    //    public string Trim { get; set; }
    //    public decimal SalePrice { get; set; }
    //    public string ExteriorColor { get; set; }
    //    public long Mileage { get; set; }
    //    public string SinglePhoto { get; set; }
    //    public int MarketRange { get; set; }
    //    public int DaysInInvenotry { get; set; }
    //    public int CurrentScreen { get; set; }
    //    public int Type { get; set; }
    //    public bool? IsFeatured { get; set; }
    //    public int CarRanking { get; set; }
    //    public string MarketData { get; set; }
    //    public int NumberOfCar { get; set; }
    //    public bool? Condition { get; set; }
    //    public string ClassFilter { get; set; }
    //    public short InventoryStatus { get; set; }
    //    public int HealthLevel { get; set; }
    //    public bool NotDoneBucketJump { get; set; }
    //    public int CarFaxOwner { get; set; }
    //    public bool IsUsed { get; set; }
    //    public short VehicleStatusCodeId { get; set; }
    }
}