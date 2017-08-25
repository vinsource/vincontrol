using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class VinsellChartVehicle
    {
        public int VehicleAutoId { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public int Distance { get; set; }

        public string Seller { get; set; }

        public string SellerAddress { get; set; }

        public double Mileage { get; set; }

        public double Price { get; set; }

        public bool AutoTrader { get; set; }

        public bool CarsCom { get; set; }

        public bool Franchise { get; set; }

        public bool Indepedent { get; set; }

        public string AutoTraderListingURL { get; set; }

        public string ThumbnailURL { get; set; }
    }
}
