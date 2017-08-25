using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace vincontrol.DomainObject
{
    [DataContract]
    public class ChartModel
    {
        [DataMember(Name = "listingid")]
        public int ListingId { get; set; }
        [DataMember(Name = "vin", IsRequired = true)]
        public string VIN { get; set; }
        [DataMember(Name = "miles")]
        public long Miles { get; set; }
        [DataMember(Name = "title")]
        public TitleInfo Title { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }
        [DataMember(Name = "distance")]
        public int Distance { get; set; }
        [DataMember(Name = "franchise")]
        public string Franchise { get; set; }
        [DataMember(Name = "autotrader")]
        public bool AutoTrader { get; set; }
        [DataMember(Name = "carscom")]
        public bool CarsCom { get; set; }
        [DataMember(Name = "commercialtruck")]
        public bool CommercialTruck { get; set; }
        [DataMember(Name = "carmax")]
        public bool Carmax { get; set; }
        [DataMember(Name = "carfax")]
        public string CarFax { get; set; }
        [DataMember(Name = "certified")]
        public bool Certified { get; set; }
        [DataMember(Name = "highlighted")]
        public bool Highlighted { get; set; }
        [DataMember(Name = "thumbnail")]
        public string ThumbnailURL { get; set; }
        [DataMember(Name = "seller")]
        public SellerInfo Seller { get; set; }
        [DataMember(Name = "bodyType")]
        public string BodyType { get; set; }
        //[DataMember(Name = "carscomlistingurl")]
        //public string CarsComListingURL { get; set; }
        //[DataMember(Name = "autotraderlistingurl")]
        //public string AutoTraderListingURL { get; set; }
        [DataMember(Name = "option")]
        public OptionInfo Option { get; set; }
        [DataMember(Name = "trims")]
        public List<string> Trims { get; set; }
        [DataMember(Name = "bodyStyles")]
        public List<string> BodyStyles { get; set; }
        [DataMember(Name = "images")]
        public List<string> Images { get; set; }
        [DataMember(Name = "uptime")]
        public Int32 Uptime { get; set; }
        [DataMember(Name = "color")]
        public ColorInfo Color { get; set; }
        [DataMember(Name = "isTargetCar")]
        public bool IsTargetCar { get; set; }
        [DataMember(Name = "longtitude")]
        public double? Longtitude { get; set; }
        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }
        [DataMember(Name = "CommercialTruckListingUrl")]
        public string CommercialTruckListingUrl { get; set; }

        public int? AutoTraderDealerId { get; set; }
        public string AutoTraderListingId { get; set; }
        public string CarsComListingId { get; set; }
        public long CarMaxListingId { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Price, Miles);
        }
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
        [DataMember(Name = "bodyStyle")]
        public string BodyStyle { get; set; }
    }

    [DataContract]
    public class SellerInfo
    {
        [DataMember(Name = "sellername")]
        public string SellerName { get; set; }
        [DataMember(Name = "selleraddress")]
        public string SellerAddress { get; set; }

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
            public decimal MinimumPrice { get; set; }
            
            [DataMember(Name = "averagePrice")]
            public decimal AveragePrice { get; set; }

            [DataMember(Name = "maximumPrice")]
            public decimal MaximumPrice { get; set; }

            [DataMember(Name = "minimumColor")]
            public string MinimumColor { get; set; }

            [DataMember(Name = "maximumColor")]
            public string MaximumColor { get; set; }

            [DataMember(Name = "minimumMileage")]
            public int MinimumMileage { get; set; }

            [DataMember(Name = "averageMileage")]
            public int AverageMileage { get; set; }

            [DataMember(Name = "maximumMileage")]
            public int MaximumMileage { get; set; }

            [DataMember(Name = "averageDays")]
            public string AverageDays { get; set; }

            [DataMember(Name = "AboveThumbnailUrl")]
            public string AboveThumbnailUrl { get; set; }

            [DataMember(Name = "BelowThumbnailUrl")]
            public string BelowThumbnailUrl { get; set; }
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
            public long Mileage { get; set; }
            [DataMember(Name = "salePrice")]
            public decimal SalePrice { get; set; }
            [DataMember(Name = "thumbnail")]
            public string ThumbnailImageUrl { get; set; }
            [DataMember(Name = "distance")]
            public int Distance { get; set; }
            [DataMember(Name = "title")]
            public TitleInfo Title { get; set; }
            [DataMember(Name = "trim")]
            public string Trim { get; set; }
            [DataMember(Name = "bodyStyle")]
            public string BodyStyle { get; set; }
            [DataMember(Name = "autotrader")]
            public bool AutoTrader { get; set; }
            [DataMember(Name = "carscom")]
            public bool CarsCom { get; set; }
            [DataMember(Name = "commercialtruck")]
            public bool CommercialTruck { get; set; }
            [DataMember(Name = "certified")]
            public bool Certified { get; set; }
            [DataMember(Name = "seller")]
            public SellerInfo Seller { get; set; }
            [DataMember(Name = "ranking")]
            public int Ranking { get; set; }
            [DataMember(Name = "vin")]
            public string Vin { get; set; }
        }

        [DataMember(Name = "cars")]
        public List<ArrayList> ChartModels { get; set; }
        public List<ChartModel> TypedChartModels { get; set; }

        [DataMember(Name = "market")]
        public MarketInfo Market { get; set; }

        [DataMember(Name = "target")]
        public TargetCar Target { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "trims")]
        public List<TrimDistance> Trims { get; set; }

        [DataMember(Name = "bodyStyles")]
        public List<BodyStyleDistance> BodyStyles { get; set; }

        [DataMember(Name = "userselectedtrims")]
        public List<string> UserSelectedTrims { get; set; }
        
        [DataMember(Name = "userselectedoptions")]
        public List<string> UserSelectedOptions { get; set; }

        [DataMember(Name = "isCarComs")]
        public bool IsCarComs { get; set; }

        [DataMember(Name = "isCertified")]
        public bool? IsCertified { get; set; }

     

    }

    [DataContract]
    public class TrimDistance
    {
        [DataMember(Name = "distance")]
        public int Distance { get; set; }

        [DataMember(Name = "trim")]
        public string Trim { get; set; }
    }

    [DataContract]
    public class BodyStyleDistance
    {
        [DataMember(Name = "distance")]
        public int Distance { get; set; }

        [DataMember(Name = "bodyStyle")]
        public string BodyStyle { get; set; }
    }


    [DataContract]
    public class ManheimChartModel
    {
        [DataMember(Name = "listingid")]
        public int ListingId { get; set; }
        [DataMember(Name = "vin", IsRequired = true)]
        public string VIN { get; set; }
        [DataMember(Name = "miles")]
        public long Miles { get; set; }
        [DataMember(Name = "title")]
        public TitleInfo Title { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }
        [DataMember(Name = "distance")]
        public int Distance { get; set; }
        [DataMember(Name = "franchise")]
        public string Franchise { get; set; }
        [DataMember(Name = "autotrader")]
        public bool AutoTrader { get; set; }
        [DataMember(Name = "carscom")]
        public bool CarsCom { get; set; }
        [DataMember(Name = "commercialtruck")]
        public bool CommercialTruck { get; set; }
        [DataMember(Name = "carmax")]
        public bool Carmax { get; set; }
        [DataMember(Name = "carfax")]
        public string CarFax { get; set; }
        [DataMember(Name = "certified")]
        public bool Certified { get; set; }
        [DataMember(Name = "thumbnail")]
        public string ThumbnailURL { get; set; }
        [DataMember(Name = "seller")]
        public SellerInfo Seller { get; set; }
        [DataMember(Name = "bodyType")]
        public string BodyType { get; set; }
        [DataMember(Name = "option")]
        public OptionInfo Option { get; set; }
        [DataMember(Name = "trims")]
        public List<string> Trims { get; set; }
        [DataMember(Name = "bodyStyles")]
        public List<string> BodyStyles { get; set; }
        [DataMember(Name = "images")]
        public List<string> Images { get; set; }
        [DataMember(Name = "uptime")]
        public Int32 Uptime { get; set; }
        [DataMember(Name = "color")]
        public ColorInfo Color { get; set; }
        [DataMember(Name = "isTargetCar")]
        public bool IsTargetCar { get; set; }
        [DataMember(Name = "longtitude")]
        public double? Longtitude { get; set; }
        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }
        [DataMember(Name = "State")]
        public string State { get; set; }
        [DataMember(Name = "ManheimRegion")]
        public string ManheimRegion { get; set; }
     
        public override string ToString()
        {
            return string.Format("{0} - {1}", Price, Miles);
        }
    }
}
