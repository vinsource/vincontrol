using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace WhitmanEnterpriseMVC.Models
{
    [DataContract]
    public class ChartModel
    {
        [DataContract]
        public class TitleInfo
        {
            [DataMember(Name = "year")]
            public Int32 Year { get; set; }
            [DataMember(Name = "make")]
            public string Make { get; set; }
            [DataMember(Name = "model")]
            public string Model { get; set; }
            [DataMember(Name = "trim")]
            public string Trim { get; set; }
        }

        [DataContract]
        public class ColorInfo
        {
            [DataMember(Name = "exterior")]
            public string Exterior { get; set; }
            [DataMember(Name = "interior")]
            public string Interior { get; set; }
        }

        [DataContract]
        public class OptionInfo
        {
            [DataMember(Name = "navigation")]
            public bool Navigation { get; set; }
            [DataMember(Name = "sunroof")]
            public bool Sunroof { get; set; }
            [DataMember(Name = "moonroof")]
            public bool Moonroof { get; set; }
        }

        [DataContract]
        public class SellerInfo
        {
            [DataMember(Name = "sellername")]
            public string SellerName { get; set; }
            [DataMember(Name = "selleraddress")]
            public string SellerAddress { get; set; }

        }
        [DataMember(Name = "listingid")]
        public int ListingId { get; set; }
        [DataMember(Name = "vin", IsRequired = true)]
        public string VIN { get; set; }
        [DataMember(Name = "miles")]
        public int Miles { get; set; }
        [DataMember(Name = "title")]
        public TitleInfo Title { get; set; }
        [DataMember(Name = "price")]
        public int Price { get; set; }
        [DataMember(Name = "distance")]
        public int Distance { get; set; }
        [DataMember(Name = "franchise")]
        public string Franchise { get; set; }
        [DataMember(Name = "autotrader")]
        public bool AutoTrader { get; set; }
        [DataMember(Name = "carscom")]
        public bool CarsCom { get; set; }
        [DataMember(Name = "certified")]
        public bool Certified { get; set; }
        [DataMember(Name = "thumbnail")]
        public string ThumbnailURL { get; set; }
        [DataMember(Name = "seller")]
        public SellerInfo Seller { get; set; }
        [DataMember(Name = "carscomlistingurl")]
        public string CarsComListingURL { get; set; }
        [DataMember(Name = "autotraderlistingurl")]
        public string AutoTraderListingURL { get; set; }
        [DataMember(Name = "option")]
        public OptionInfo Option { get; set; }
        [DataMember(Name = "trims")]
        public List<string> Trims { get; set; }
        [DataMember(Name = "images")]
        public List<string> Images { get; set; }
        [DataMember(Name = "uptime")]
        public Int32 Uptime { get; set; }
        [DataMember(Name = "color")]
        public ColorInfo Color { get; set; }
        [DataMember(Name = "isTargetCar")]
        public bool IsTargetCar { get; set; }
        [DataMember(Name = "longtitude")]
        public string Longtitude { get; set; }
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }
    }

    [DataContract]
    public class ChartGraph
    {
        [DataContract]
        public class MarketInfo
        {
            [DataMember(Name = "carsOnMarket")]
            public int CarsOnMarket { get; set; }
            [DataMember(Name = "minimumPrice")]
            public string MinimumPrice { get; set; }
            [DataMember(Name = "averagePrice")]
            public string AveragePrice { get; set; }
            [DataMember(Name = "maximumPrice")]
            public string MaximumPrice { get; set; }

            [DataMember(Name = "minimumColor")]
            public string MinimumColor { get; set; }

            [DataMember(Name = "maximumColor")]
            public string MaximumColor { get; set; }

            [DataMember(Name = "minimumMileage")]
            public string MinimumMileage { get; set; }

            [DataMember(Name = "averageMileage")]
            public string AverageMileage { get; set; }

            [DataMember(Name = "maximumMileage")]
            public string MaximumMileage { get; set; }



            [DataMember(Name = "averageDays")]
            public string AverageDays { get; set; }
        }

        [DataContract]
        public class TargetCar
        {
            public TargetCar()
            {
                Distance = 0;
                CarsCom = false;
                AutoTrader = true;
            }

            [DataMember(Name = "appraisalId")]
            public int AppraisalId { get; set; }
            [DataMember(Name = "listingId")]
            public int ListingId { get; set; }
            [DataMember(Name = "mileage")]
            public int Mileage { get; set; }
            [DataMember(Name = "salePrice")]
            public int SalePrice { get; set; }
            [DataMember(Name = "thumbnail")]
            public string ThumbnailImageUrl { get; set; }
            [DataMember(Name = "distance")]
            public int Distance { get; set; }
            [DataMember(Name = "title")]
            public ChartModel.TitleInfo Title { get; set; }
            [DataMember(Name = "trim")]
            public string Trim { get; set; }
            [DataMember(Name = "autotrader")]
            public bool AutoTrader { get; set; }
            [DataMember(Name = "carscom")]
            public bool CarsCom { get; set; }
            [DataMember(Name = "certified")]
            public bool Certified { get; set; }
            [DataMember(Name = "seller")]
            public ChartModel.SellerInfo Seller { get; set; }
            [DataMember(Name = "ranking")]
            public int Ranking { get; set; }
            
        }

        [DataMember(Name = "carlist")]
        public List<ChartModel> ChartModels { get; set; }

        [DataMember(Name = "market")]
        public MarketInfo Market { get; set; }

        [DataMember(Name = "target")]
        public TargetCar Target { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }

    }

}