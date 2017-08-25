using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.Helper;

namespace Vincontrol.Web.Models
{
    public class InventoryInfo
    {
        public override string ToString()
        {
            return string.Format("Age:{0}, ListingId:{1}", DaysInInvenotry, ListingId);
        }

        public InventoryInfo(Inventory tmp)
        {
            ListingId = tmp.InventoryId;
            Year = tmp.Vehicle.Year.GetValueOrDefault();
            Stock = tmp.Stock;
            Model = tmp.Vehicle.Model;
            Make = tmp.Vehicle.Make;
            Mileage = tmp.Mileage.GetValueOrDefault();
            Trim = tmp.Vehicle.Trim;
            Vin = tmp.Vehicle.Vin;
            ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days == DateTime.Now.Subtract(DateTime.MinValue).Days ? -1 : DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                              ? tmp.Vehicle.DefaultStockImage
                              : tmp.ThumbnailUrl.Split(new string[] { ",", "|" },
                                                      StringSplitOptions.RemoveEmptyEntries).
                                    FirstOrDefault();
            SalePrice = tmp.SalePrice.GetValueOrDefault();
            MarketRange = tmp.MarketRange.GetValueOrDefault();
            IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            Certified = tmp.Certified.GetValueOrDefault();
            ACar = tmp.ACar.GetValueOrDefault();
            CarRanking = tmp.CarRanking.GetValueOrDefault();
            NumberOfCar = tmp.NumberOfCar.GetValueOrDefault();
            SalePrice = tmp.SalePrice.GetValueOrDefault();
            Condition = tmp.Condition;
            InventoryStatus = tmp.InventoryStatusCodeId;
            HealthLevel = LogicHelper.GetHealthLevel(tmp);
            CarFaxOwner = tmp.CarfaxOwner??0;
            IsUsed = tmp.Condition;

            AllowEBay = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_Ebay;
            AllowBG = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_BG;
            AllowWS = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_WS;
            AllowChangeStatus = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_Status;
            AllowEditProfile = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_EditProfile;
            AllowCraigslist = Handlers.SessionHandler.UserRight.Inventory.Craigslist;

            SavedBucketJump = tmp.SavedBucketJump.GetValueOrDefault();
            SavedBucketJumpDate = tmp.SavedBucketJumpDate.GetValueOrDefault();
            DisplaySavedBucketJumpDate = tmp.SavedBucketJumpDate != null
                ? tmp.SavedBucketJumpDate.GetValueOrDefault().ToString("MM/dd/yyyy HH:mm:ss")
                : string.Empty;

            Location = tmp.Dealer.City;
            Dealer = tmp.Dealer.Name;
        }

        public InventoryInfo(SoldoutInventory tmp)
        {
            ListingId = tmp.SoldoutInventoryId;
            Year = tmp.Vehicle.Year.GetValueOrDefault();
            Stock = tmp.Stock;
            Model = tmp.Vehicle.Model;
            Make = tmp.Vehicle.Make;
            Mileage = tmp.Mileage.GetValueOrDefault();
            Trim = tmp.Vehicle.Trim;
            Vin = tmp.Vehicle.Vin;
            ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateRemoved.GetValueOrDefault()).Days == DateTime.Now.Subtract(DateTime.MinValue).Days ? -1 : DateTime.Now.Subtract(tmp.DateRemoved.GetValueOrDefault()).Days;
            SinglePhoto = String.IsNullOrEmpty(tmp.ThumbnailUrl)
                              ? tmp.Vehicle.DefaultStockImage
                              : tmp.ThumbnailUrl.Split(new string[] {",", "|"},
                                                      StringSplitOptions.RemoveEmptyEntries).
                                    FirstOrDefault();
            SalePrice = tmp.SalePrice.GetValueOrDefault();
            IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            
            Condition = tmp.Condition;
            CarFaxOwner = tmp.CarFaxOwner??0;
            IsUsed = tmp.Condition;

            AllowEBay = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_Ebay;
            AllowBG = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_BG;
            AllowWS = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_WS;
            AllowChangeStatus = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_Status;
            AllowEditProfile = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_EditProfile;
            AllowCraigslist = Handlers.SessionHandler.UserRight.Inventory.Craigslist;

            Location = tmp.Dealer.City;
            Dealer = tmp.Dealer.Name;
        }

        public InventoryInfo(CarInfoFormViewModel tmp)
        {
            ListingId = tmp.ListingId;
            Year = tmp.Year;
            Stock = tmp.Stock;
            Model = tmp.Model;
            Make = tmp.Make;
            Mileage = tmp.Mileage;
            Trim = tmp.Trim;
            Vin = tmp.Vin;
            ExteriorColor = tmp.ExteriorColor ?? string.Empty;
            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days == DateTime.Now.Subtract(DateTime.MinValue).Days ? -1 : DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;
            SinglePhoto = tmp.SinglePhoto;
            SalePrice = tmp.SalePrice;
            MarketRange = tmp.MarketRange;
            IsFeatured = tmp.IsFeatured.GetValueOrDefault();
            CarRanking = tmp.CarRanking;
            NumberOfCar = tmp.NumberOfCar;
            SalePrice = tmp.SalePrice;
            Condition = tmp.Condition;
            InventoryStatus = tmp.InventoryStatus;
            CarFaxOwner = tmp.CarFaxOwner;
            IsUsed = tmp.Condition.GetValueOrDefault();

            AllowEBay = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_Ebay;
            AllowBG = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_BG;
            AllowWS = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_WS;
            AllowChangeStatus = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_Status;
            AllowEditProfile = Handlers.SessionHandler.UserRight.Inventory.ViewProfile_EditProfile;
            AllowCraigslist = Handlers.SessionHandler.UserRight.Inventory.Craigslist;
            //Dealer = tmp.Dealer.Name;

            
        }

        [Required]
        public string Vin { get; set; }
        public int ListingId { get; set; }
        public string Stock { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public decimal SalePrice { get; set; }
        public string ExteriorColor { get; set; }
        public long Mileage { get; set; }
        public string SinglePhoto { get; set; }
        public int MarketRange { get; set; }
        public int DaysInInvenotry { get; set; }
        public int CurrentScreen { get; set; }
        public int Type { get; set; }
        public bool? IsFeatured { get; set; }
        public bool Certified { get; set; }
        public bool ACar { get; set; }
        public int CarRanking { get; set; }
        public string MarketData { get; set; }
        public int NumberOfCar { get; set; }
        public bool? Condition { get; set; }
        public string ClassFilter { get; set; }
        public short InventoryStatus { get; set; }
        public int HealthLevel { get; set; }
        public bool NotDoneBucketJump { get; set; }
        public int CarFaxOwner { get; set; }
        public bool IsUsed { get; set; }
        public short VehicleStatusCodeId { get; set; }

        public bool AllowEBay { get; private set; }
        public bool AllowBG { get; private set; }
        public bool AllowWS { get; private set; }
        public bool AllowChangeStatus { get; private set; }
        public bool AllowEditProfile { get; private set; }
        public bool AllowCraigslist { get; private set; }

        public bool SavedBucketJump { get; set; }
        public DateTime SavedBucketJumpDate { get; set; }
        public string DisplaySavedBucketJumpDate { get; set; }
        public bool MassBucketJumpCertified { get; set; }
        public decimal MassBucketJumpCertifiedAmount { get; set; }
        public bool MassBucketJumpACar { get; set; }
        public decimal MassBucketJumpACarAmount { get; set; }
        public string Location { get; set; }
        public string Dealer { get; set; }
        public string UserStamp { get; set; }
    }
}