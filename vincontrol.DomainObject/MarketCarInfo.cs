using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class MarketCarInfo
    {
        public int? AutoTraderDealerId { get; set; }
        public string CarscomListingId { get; set; }
        public int RegionalListingId { get; set; }
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
        public string State { get; set; }
        public bool? Certified { get; set; }
        public bool? MoonRoof { get; set; }
        public bool? SunRoof { get; set; }
        public int? Year { get; set; }
        public bool? Franchise { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? DateAdded { get; set; }
        public int SoldDays { get; set; }
        public string AutoTraderThumbnailURL { get; set; }
        public string CarsComThumbnailURL { get; set; }
        public string CarsMaxThumbnailURL { get; set; }
        public bool? CarsCom { get; set; }
        public string CarsComListingURL { get; set; }
        public bool? AutoTrader { get; set; }
        public bool? CommercialTruck { get; set; }
        public string AutoTraderListingURL { get; set; }
        public string Dealershipname { get; set; }
        public string Address { get; set; }
        public string AutoTraderListingId { get; set; }
        public bool? CarMax { get; set; }
        public long? CarMaxListingId { get; set; }
        public string CommercialTruckListingUrl { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string AuctionCode { get; set; }
        public bool? Highlighted { get; set; }
        public int MarketSupply { get; set; }
    }
}
