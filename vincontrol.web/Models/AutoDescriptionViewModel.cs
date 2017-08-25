using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;

namespace Vincontrol.Web.Models
{
    public class AutoDescriptionViewModel
    {
        public AutoDescriptionViewModel() { }

        public AutoDescriptionViewModel(Inventory newInventory)
        {
            Year = newInventory.Vehicle.Year.GetValueOrDefault();
            Make = newInventory.Vehicle.Make;
            Model = newInventory.Vehicle.Model;
            Trim = newInventory.Vehicle.Trim;

            Mileage = newInventory.Mileage.GetValueOrDefault();
            Color = String.IsNullOrEmpty(newInventory.ExteriorColor) ? string.Empty : newInventory.ExteriorColor;
            Packages = String.IsNullOrEmpty(newInventory.AdditionalPackages) ? string.Empty : newInventory.AdditionalOptions;
            PackagesList = String.IsNullOrEmpty(newInventory.AdditionalPackages) ? new string[] { } : newInventory.AdditionalPackages.Split(';').ToArray();
            AdditionalOptions = newInventory.AdditionalOptions;
            MarketRange = newInventory.MarketRange.GetValueOrDefault();
            CarsOnMarket = newInventory.Vehicle.NumberOfCar.GetValueOrDefault();
            FuelHighway = String.IsNullOrEmpty(newInventory.Vehicle.FuelEconomyHighWay) ? 0 : Convert.ToInt32(newInventory.Vehicle.FuelEconomyHighWay);



            Transmission = String.IsNullOrEmpty(newInventory.Vehicle.Tranmission) ? "" : newInventory.Vehicle.Tranmission;
            DaysInInventory = DateTime.Now.Subtract(newInventory.DateInStock.GetValueOrDefault()).Days;
            Warranty = newInventory.WarrantyInfo.GetValueOrDefault();
            Certified = newInventory.Certified.GetValueOrDefault();
            PriorRental = newInventory.PriorRental.GetValueOrDefault();
            Unwind = newInventory.Unwind.GetValueOrDefault();
            DealerDemo = newInventory.DealerDemo.GetValueOrDefault();
            BrandedTitle = newInventory.BrandedTitle.GetValueOrDefault();
            BodyType = String.IsNullOrEmpty(newInventory.Vehicle.BodyType) ? "" : newInventory.Vehicle.BodyType;

            Msrp = 0;
        }

        public string DealershipName { get; set; }
        public int CarfaxOwner { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Transmission { get; set; }
        public string BodyType { get; set; }
        public string Color { get; set; }
        public string Packages { get; set; }
        public string[] PackagesList { get; set; }
        public string[] PackageOptionsList { get; set; }
        public string AdditionalOptions { get; set; }
        public int MarketRange { get; set; }
        public long Mileage { get; set; }
        public string EndingSentences { get; set; }
        public int FuelHighway { get; set; }
        public int CarsOnMarket { get; set; }
        public int DaysInInventory { get; set; }
        public bool Certified { get; set; }
        public int Warranty { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool PriorRental { get; set; }
        public bool Unwind { get; set; }
        public bool DealerDemo { get; set; }
        public bool BrandedTitle { get; set; }
        public string Disclaimer { get; set; }
        public decimal Msrp { get; set; }
        public int DealerId { get; set; }
        public bool Condition { get; set; }
    }
}