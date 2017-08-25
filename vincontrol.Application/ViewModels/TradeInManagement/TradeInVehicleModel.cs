using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.TradeInManagement;
//using vincontrol.CarFax;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using ExtendedEquipmentOption = vincontrol.DomainObject.TradeIn.ExtendedEquipmentOption;

namespace vincontrol.Application.ViewModels.TradeInManagement
{
    public class TradeInVehicleModel
    {
        private string[] _popularColors = new[] { "Black", "White", "Silver", "Gray" };

        public TradeInVehicleModel() { }

        private bool CheckLowMiles(int year, int mileage)
        {
            if (DateTime.Now.Year <= year) return true;
            return mileage/(DateTime.Now.Year - year) <= 12000;
        }

        private bool CheckLowPrice(int marketRange)
        {
            return marketRange == 1;
        }

        private bool CheckFuelEfficient(int fuelHighway)
        {
            return fuelHighway >= 28;
        }

        private bool CheckPopularColor(string color)
        {
            return _popularColors.Contains(color);
        }

        private bool CheckNewArrival(int daysInStock)
        {
            return daysInStock <= 30;
        }

        private bool CheckCarfax1Owner(int owner)
        {
            return owner == 1;
        }

        public TradeInVehicleModel(Inventory inventory)
        {
            Vin = inventory.Vehicle.Vin;
            VehicleId = inventory.VehicleId;
            SelectedYear = inventory.Vehicle.Year.GetValueOrDefault();
            SelectedMakeValue = inventory.Vehicle.Make;
            SelectedModelValue = inventory.Vehicle.Model;
            SelectedTrimValue = inventory.Vehicle.Trim;
            StockNumber = inventory.Stock;
            try
            {
                SalePrice = Convert.ToDecimal(inventory.SalePrice);
            }
            catch (Exception)
            {
                SalePrice = 0;
            }

            try
            {
                MileageNumber = Convert.ToInt32(inventory.Mileage);
            }
            catch (Exception)
            {
                MileageNumber = 0;
            }

            Engine = inventory.Vehicle.EngineType;
            ExteriorColor = inventory.ExteriorColor;
            Transmission = inventory.Vehicle.Tranmission;
            Description = inventory.Descriptions;
            ImageList = String.IsNullOrEmpty(inventory.PhotoUrl) ? new string[] { } : inventory.PhotoUrl.Split(',');
            Options = String.IsNullOrEmpty(inventory.AdditionalOptions) ? new string[] { } : inventory.AdditionalOptions.Split(',');
            Packages = String.IsNullOrEmpty(inventory.AdditionalPackages) ? new string[] { } : inventory.AdditionalPackages.Split(',');
            FuelHighway = String.IsNullOrEmpty(inventory.Vehicle.FuelEconomyHighWay)
                              ? 0
                              : Convert.ToInt32(inventory.Vehicle.FuelEconomyHighWay);
            DaysInInventory = DateTime.Now.Subtract(inventory.DateInStock.GetValueOrDefault()).Days;
            CarfaxOwner = inventory.CarfaxOwner.GetValueOrDefault();
            MarketRange = inventory.MarketRange.GetValueOrDefault();

            IsLowMiles = CheckLowMiles(inventory.Vehicle.Year.GetValueOrDefault(), MileageNumber);
            IsLowPrice = CheckLowPrice(MarketRange);
            IsFuelEfficient = CheckFuelEfficient(FuelHighway);
            IsPopularColor = CheckPopularColor(ExteriorColor);
            IsNewArrival = CheckNewArrival(DaysInInventory);
            IsCarfax1Owner = CheckCarfax1Owner(CarfaxOwner);
            InventoryCondition = inventory.Condition;
            DealerRef=new DealershipViewModel(inventory.Dealer);
            DealerId = inventory.DealerId;
        }

        public IEnumerable<ExtendedSelectListItem> YearsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> MakesList { get; set; }

        public IEnumerable<ExtendedSelectListItem> ModelsList { get; set; }

        public IEnumerable<ExtendedSelectListItem> TrimsList { get; set; }

        public List<ExtendedEquipmentOption> OptionalEquipment { get; set; }

        public string TradeInCustomerId { get; set; }

        public int VehicleId { get; set; }

        public string EncryptVehicleId { get; set; }

        public string Vin { get; set; }

        public string ZipCode { get; set; }

        public bool ValidVin { get; set; }

        public string SelectedMake { get; set; }

        public string SelectedModel { get; set; }

        public string SelectedTrim { get; set; }

        public string SelectedMakeValue { get; set; }

        public string SelectedModelValue { get; set; }

        public string SelectedTrimValue { get; set; }

        public int SelectedYear { get; set; }

        public string SelectedOptions { get; set; }

        public decimal TradeInFairPrice { get; set; }

        public decimal TradeInGoodPrice { get; set; }

        public decimal TradeInVeryGoodPrice { get; set; }


        public decimal MarketHighestPrice { get; set; }

        public decimal MarketAveragePrice { get; set; }

        public decimal MarketLowestPrice { get; set; }


        public long MarketHighestMileage { get; set; }

        public long MarketAverageMileage { get; set; }

        public long MarketLowestMileage { get; set; }

        public string AboveThumnailUrl { get; set; }

        public string BelowThumbnailUrl { get; set; }

     


        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }
        public DealershipViewModel DealerRef { get; set; }

        public string Dealer { get; set; }

        public int DealerId { get; set; }

        public string DealerName { get; set; }

        public string Mileage { get; set; }

        public int MileageNumber { get; set; }

        public string Condition { get; set; }

        public bool InventoryCondition { get; set; }

        public decimal SelectedOptionAdjustment { get; set; }
        
        public bool CarFaxSuccess { get; set; }

        public CarFaxViewModel CarFax { get; set; }

        public string EmailTextContent { get; set; }

        public string EmailAdfContent { get; set; }

        public List<string> Receivers { get; set; }

        public bool SkipStepTwo { get; set; }

        public bool IsValidDealer { get; set; }

        public string ImageUrl { get; set; }

        public string InterestedVehicle { get; set; }

        #region Landing Page

        public decimal SalePrice { get; set; }
        public bool IsLowMiles { get; set; }
        public bool IsLowPrice { get; set; }
        public bool IsFuelEfficient { get; set; }
        public bool IsPopularColor { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsCarfax1Owner { get; set; }
        public string[] ImageList { get; set; }
        public string StockNumber { get; set; }
        public string Engine { get; set; }
        public string ExteriorColor { get; set; }
        public string Transmission { get; set; }
        public string Description { get; set; }
        public string[] Options { get; set; }
        public string[] Packages { get; set; }
        public int FuelHighway { get; set; }
        public int DaysInInventory { get; set; }
        public int CarfaxOwner { get; set; }
        public int MarketRange { get; set; }

        #endregion
    }
}