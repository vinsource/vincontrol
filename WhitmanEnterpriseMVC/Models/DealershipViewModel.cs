using System.ComponentModel.DataAnnotations;
namespace WhitmanEnterpriseMVC.Models
{
    public class DealershipViewModel 
    {

        public string DealershipName { get; set; }
        
        public string DealershipPhoneNumber { get; set; }

        public string DealershipAddress { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int DealershipId { get; set; }

        public string DealerGroupId { get; set; }

        public string Latitude { get; set; }

        public string Longtitude { get; set; }

        public string InventorySorting { get; set; }

        public string DefaultStockImageUrl { get; set; }

        public string SoldOut { get; set; }

        public bool ValidUser { get; set; }

        public bool OverrideStockImage { get; set; }

        public bool OverrideDealerKbbReport { get; set; }

        public bool DefaultLogin { get; set; }

        public string PayPalEmailAddress { get; set; }

        public string DealerInfo { get; set; }

        public string DealerWarranty { get; set; }

        public string TermConditon { get; set; }

        public string EbayToken { get; set; }

        public string EbayInventoryUrl { get; set; }

        public string CreditUrl { get; set; }

        public string WebSiteUrl { get; set; }

        public string ContactUsUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string LogoUrl { get; set; }

        public string ContactPerson { get; set; }

        public string CarFax { get; set; }

        [DataType(DataType.Password)]
        public string CarFaxPassword { get; set; }

        public string Manheim { get; set; }


        [DataType(DataType.Password)]
        public string ManheimPassword { get; set; }

        public string KellyBlueBook { get; set; }

        [DataType(DataType.Password)]
        public string KellyPassword { get; set; }

        public string BlackBook { get; set; }

        [DataType(DataType.Password)]
        public string BlackBookPassword { get; set; }

        public string StartSentence { get; set; }

        public string EndSentence { get; set; }

        public string AuctionSentence { get; set; }

        public string LoanerSentence { get; set; }

        public string EncryptDealerId { get; set; }

        public int EmailFormat { get; set; }

        public bool EnableAutoDescription { get; set; }

        public decimal PriceVariance { get; set; }

        public int FirstIntervalJump { get; set; }

        public int SecondIntervalJump { get; set; }

        public int IntervalBucketJump { get; set; }
    }
}
