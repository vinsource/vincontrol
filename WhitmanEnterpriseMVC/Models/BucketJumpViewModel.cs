using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class BucketJumpViewModel
    {
        public string DealerName { get; set; }
        public int[] HighlightedDaysInInventory { get; set; }
        public int[] AvailableDaysInInventory { get; set; }
        public DealerCar CarOfDealer { get; set; }
        public DealerCar CarOnMarket { get; set; }
        public int  MileageAdjustmentDiff { get; set; }
        public ChartGraph ChartGraph { get; set; }
    }

    public class DealerCar
    {
        public int ListingId { get; set; }
        public string Vin { get; set; }
        public string StockNumber { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int Miles { get; set; }
        public string Dealer { get; set; }
        public int DaysInInventory { get; set; }
        public int KBBTrimId { get; set; }
        public decimal MileageAdjustment { get; set; }
        public int SuggestedRetailPrice { get; set; }
        public string Note { get; set; }

    }
}