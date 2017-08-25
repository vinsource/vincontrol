using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using vincontrol.DomainObject;
using vincontrol.Application.ViewModels.CommonManagement;

namespace Vincontrol.Web.Models
{
    public class BucketJumpViewModel
    {
        public string DealerName { get; set; }
        public int[] HighlightedDaysInInventory { get; set; }
        public int[] AvailableDaysInInventory { get; set; }
        public DealerCar CarOfDealer { get; set; }
        public DealerCar CarOnMarket { get; set; }
        public int  MileageAdjustmentDiff { get; set; }
        public int OptionsPrice { get; set; }
        public int WholeSaleWithOptions { get; set; }
        public int PlusPrice { get; set; }
        public bool Certified { get; set; }
        public int IndependentAdd { get; set; }
        public int CertifiedAdd { get; set; }
        public ChartGraph ChartGraph { get; set; }
        public List<CarInfoViewModel> ListOfSimilarUsedInventories { get; set; }
       
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
        public decimal Price { get; set; }
        public long Miles { get; set; }
        public string Dealer { get; set; }
        public int DaysInInventory { get; set; }
        public int KBBTrimId { get; set; }
        public decimal MileageAdjustment { get; set; }
        public decimal SuggestedRetailPrice { get; set; }
        public string Note { get; set; }
        public string[] Options { get; set; }
        public string Image { get; set; }
        public bool IsCertified { get; set; }
        public decimal CertifiedAmount { get; set; }
        public bool ACar { get; set; }
        public decimal ExpandedMileageAdjustment { get; set; }
        public string ExplainedNote { get; set; }
    }
}