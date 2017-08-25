using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.ViewModels.ManheimAuctionManagement
{
    public class AuctionTransactionStatistic
    {
        public string RegionName { get; set; }

        public int NoOfVehicles { get; set; }

        public decimal AvgOdometer { get; set; }
        
        public decimal AvgAuctionPrice { get; set; }

        public short AuctionRegion { get; set; }

        public int ListingId { get; set; }

        public int VehicleStatus { get; set; }
    }
}
