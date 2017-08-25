using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhitmanEnterpriseMVC.Interfaces
{
    public class MarketCarInfo
    {
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
        public System.Nullable<bool> Certified { get; set; }
        public System.Nullable<bool> MoonRoof { get; set; }
        public System.Nullable<bool> SunRoof { get; set; }
        public System.Nullable<int> Year { get; set; }
        public System.Nullable<bool> Franchise { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public System.Nullable<global::System.DateTime> DateAdded { get; set; }
        public string AutoTraderThumbnailURL { get; set; }
        public string CarsComThumbnailURL { get; set; }
        public System.Nullable<bool> CarsCom { get; set; }
        public string CarsComListingURL { get; set; }
        public System.Nullable<bool> AutoTrader { get; set; }
        public string AutoTraderListingURL { get; set; }
        public string Dealershipname { get; set; }
        public string Address { get; set; }

    }
}
