using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Data.Model
{
    public class VehicleStandardModel
    {
        public int AutoId { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string Vin { get; set; }

        public string AutoTraderStockNo { get; set; }

        public string CarsComStockNo { get; set; }

        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string BodyStyle { get; set; }

        public int StartingPrice { get; set; }

        public int CurrentPrice { get; set; }

        public string MSRP { get; set; }

        public int Mileage { get; set; }

        public string Tranmission { get; set; }

        public string Engine { get; set; }

        public string DriveType { get; set; }

        public string Doors { get; set; }

        public string FuelType { get; set; }

        public bool AutoTrader { get; set; }

        public int AutoTraderListingId { get; set; }

        public string AutoTraderListingName { get; set; }

        public string AutoTraderListingURL { get; set; }

        public string AutoTraderCarImageURL { get; set; }

        public string AutoTraderThumbnailURL { get; set; }

        public string AutoTraderDescription { get; set; }

        public string AutoTraderInstalledFeatures { get; set; }

        public bool CarsCom { get; set; }

        public string CarsComListingId { get; set; }

        public string CarsComListingName { get; set; }

        public string CarsComListingURL { get; set; }

        public string CarsComCarImageURL { get; set; }

        public string CarsComThumbnailURL { get; set; }

        public string CarsComDescription { get; set; }

        public string CarsComInstalledFeatures { get; set; }

        public bool Ebay { get; set; }

        public string EbayListingId { get; set; }

        public string EbayListingName { get; set; }

        public string EbayURL { get; set; }

        public string EbayThumbnailURL { get; set; }

        public string CarFaxURL { get; set; }

        public int CarFaxType { get; set; }

        public string AutoCheckURL { get; set; }

        public bool Navigation { get; set; }

        public bool SunRoof { get; set; }

        public bool MoonRoof { get; set; }

        public bool Certified { get; set; }

        public bool UsedNew { get; set; }

        public int VinControlDealerId { get; set; }

        public int AutoTraderDealerId { get; set; }

        public int CarsComDealerId { get; set; }

        public string ZipCode { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime LastUpdatedPrice { get; set; }

        public string AutoTraderDealerName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string CountyName { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public bool? Franchise { get; set; }
    }
}
