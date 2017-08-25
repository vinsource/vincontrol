using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class AutoDescriptionViewModel
    {
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
        public int Mileage { get; set; }
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
        public string MSRP { get; set; }
        public int DealerId { get; set; }
    }
}