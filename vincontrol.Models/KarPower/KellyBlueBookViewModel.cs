using System.Collections.Generic;
using System.Web.Mvc;
using vincontrol.Models.KBBServiceEndPoint;

namespace vincontrol.Models.KarPower
{
    public class KellyBlueBookViewModel
    {
        public int ListingId { get; set; }
        public int TrimId { get; set; }

        public int AppraisalId { get; set; }


        public string Vin { get; set; }

        //public List<KellyBlueBookTrimDetail> TrimList { get; set; }

        public List<KellyBlueBookTrimReport> TrimReportList { get; set; }

        public bool Success { get; set; }

        public int StatusExistInDatabase { get; set; }


        public bool SessionTimeOut { get; set; }

        public string ZipCode { get; set; }

        public long Mileage { get; set; }

        public string Disclaimer { get; set; }

        public IEnumerable<SelectListItem> TrimList { get; set; }

        public string SelectedTrim { get; set; }

    }

    public class KellyBlueBookTrimReport
    {
        public string TrimName { get; set; }

        public int TrimId { get; set; }

        public KellyBlueBookTradeInDetail TradeInPrice { get; set; }

        public KellyBlueBookAuctionDetail AuctionPrice { get; set; }

        public string Retail { get; set; }

        public string FairPurchasePrice { get; set; }

        public decimal MileageAdjustment { get; set; }

        public int MileageZeroPoint { get; set; }

        public string BaseRetail { get; set; }

        public decimal BaseWholesale { get; set; }

        public decimal WholeSale { get; set; }

        public string CPO { get; set; }

        public string Invoice { get; set; }

        public string MSRP { get; set; }

        public List<OptionValuation> OptionValuation { get; set; }

        public List<ExtendedEquipmentOption> OptionalEquipment { get; set; }

        public List<Specification> SpecificationList { get; set; }

        public IdStringPair Tranmission { get; set; }

        public IdStringPair Engine { get; set; }

        public IdStringPair DriveTrain { get; set; }

        public int VehicleId { get; set; }

        public VehicleConfiguration VehicleConfiguration { get; set; }

        public string LendingRetailPriceWithoutAdditonalOption { get; set; }

        public string LendingRetailPriceWithAdditonalOption { get; set; }

        public string FinalLendingRetailPrice { get; set; }

    }

    public class KellyBlueBookTradeInDetail
    {


        public decimal TradeInFairPrice { get; set; }

        public decimal TradeInGoodPrice { get; set; }

        public decimal TradeInVeryGoodPrice { get; set; }

        public decimal TradeInExcellentPrice { get; set; }

    }


    public class KellyBlueBookAuctionDetail
    {

        public string AuctionFairPrice { get; set; }

        public string AuctionGoodPrice { get; set; }

        public string AuctionVeryGoodPrice { get; set; }

        public string AuctionExcellentPrice { get; set; }


    }

    public class ExtendedEquipmentOption
    {
        public int VehicleOptionId { get; set; }

        public string DisplayName { get; set; }

        public string DisplayNameAdditionalData { get; set; }

        public decimal Price { get; set; }

        public string PriceAdjustmentForWholeSale { get; set; }

        public string PriceAdjustmentForRetail { get; set; }

        public string PriceType { get; set; }

        public bool IsSelected { get; set; }

        public bool IsSaved { get; set; }
    }
}
