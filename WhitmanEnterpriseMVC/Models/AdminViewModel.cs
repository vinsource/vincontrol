using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
{
    public class AdminViewModel
    {
        [Required]
        public string SortSet { get; set; }

        public IEnumerable<SelectListItem> SortSetList { get; set; }
        
        public string SoldAction { get; set; }

        public IEnumerable<SelectListItem> SoldActionList { get; set; }

        public string Cragislist { get; set; }

        [DataType(DataType.Password)]
        public string CraigslistPassword { get; set; }
        
        public string Ebay { get; set; }

        [DataType(DataType.Password)]
        public string EbayPassword { get; set; }
        
        public string CarFax { get; set; }

        [DataType(DataType.Password)]
        public string CarFaxPassword { get; set; }
        
        public bool CarFaxPasswordChanged { get; set; }
        
        public string Manheim { get; set; }

        [DataType(DataType.Password)]
        public string ManheimPassword { get; set; }
        
        public bool ManheimPasswordChanged { get; set; }
        
        public string KellyBlueBook { get; set; }

        [DataType(DataType.Password)]
        public string KellyPassword { get; set; }
        
        public bool KellyPasswordChanged { get; set; }

        public string BlackBook { get; set; }

        [DataType(DataType.Password)]
        public string BlackBookPassword { get; set; }
        
        public bool BlackBookPasswordChanged { get; set; }
        
        public string AutoCheck { get; set; }

        [DataType(DataType.Password)]
        public string AutoCheckPassword { get; set; }

        public IEnumerable<UserRoleViewModel> Users{ get; set; }

        public string DealershipName { get; set; }

        public string DealershipPhoneNumber { get; set; }

        public string DealershipAddress { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public int DealershipId { get; set; }

        public string DefaultStockImageURL { get; set; }

        public bool AppraisalNotification { get; set; }

        public bool WholeSaleNotfication { get; set; }

        public bool InventoryNotfication { get; set; }

        public bool TwentyFourHourNotification { get; set; }

        public bool NoteNotification { get; set; }

        public bool PriceChangeNotification { get; set; }

        public bool MarketPriceRangeChangeNotification { get; set; }

        public bool AgeingBucketJumpNotification { get; set; }

        public bool BucketJumpReportNotification { get; set; }

        public bool ImageUploadNotification { get; set; }

        public bool OverrideStockImage { get; set; }
        
        public bool MutipleDealer { get; set; }

        public bool RetailPriceWSNotification { get; set; }

        public bool DealerDiscountWSNotification { get; set; }

        public bool ManufacturerReabteWsNotification { get; set; }

        public bool SalePriceWsNotification { get; set; }

        public bool EnableAutoDescription { get; set; }
        
        public bool AutoDescriptionSubscribe { get; set; }

        public string RetailPriceWSNotificationText { get; set; }

        public string DealerDiscountWSNotificationText { get; set; }

        public string ManufacturerReabteWsNotificationText { get; set; }

        public string SalePriceWsNotificationText { get; set; }

        public DealershipViewModel Dealer { get; set; }

        public DealerGroupViewModel DealerGroup { get; set; }

        public string SelectedDealership { get; set; }

        public IEnumerable<SelectListItem> DealerList { get; set; }

        public string ManufacturerWarranty { get; set; }

        public string DealerCertified { get; set; }

        public string ManufacturerCertified { get; set; }

        public string ManufacturerWarrantyDuration { get; set; }

        public string DealerCertifiedDuration { get; set; }

        public string ManufacturerCertifiedDuration { get; set; }

        public string DealerInfo { get; set; }

        public string DealerWarrantyInfo { get; set; }

        public string TermConditon { get; set; }

        public string ShippingInfo { get; set; }

        public string StartSentence { get; set; }

        public string EndSentence { get; set; }

        public string AuctionSentence { get; set; }

        public string LandingPage { get; set; }

        public int FirstRange { get; set; }

        public int SecondRange { get; set; }

        public int SelectedInterval { get; set; }

        public IEnumerable<SelectListItem> IntervalList { get; set; }

        public IEnumerable<SelectListItem> YearsList { get; set; }

        public IEnumerable<SelectListItem> MakesList { get; set; }

        public IEnumerable<SelectListItem> ModelsList { get; set; }

        public IEnumerable<SelectListItem> TrimsList { get; set; }

        public IEnumerable<SelectListItem> BodyTypeList { get; set; }

        public string SelectedYear { get; set; }

        public string SelectedMake { get; set; }

        public string SelectedModel { get; set; }

        public string SelectedTrim { get; set; }
        
        public string SelectedBodyType { get; set; }

        public List<ManafacturerRebateDistinctModel> RebateList { get; set; }

        public string BuyerGuide1 { get; set; }

        public string BuyerGuide2 { get; set; }

        public string BuyerGuide3 { get; set; }

        public string BuyerGuide4 { get; set; }

        public IEnumerable<TradeinCommentViewModel> Comments { get; set; }

        public VariantCodeViewModel VarianceCost  { get; set; }

        public IEnumerable<WarrantyTypeViewModel> WarrantyTypes { get; set; }

        public int SelectedWarrantyType { get; set; }

        public int SelectedWarrantyTypeForEdit { get; set; }

        public IList<SelectListItem> BasicWarrantyTypes { get; set; }

        public IList<ButtonPermissionViewModel> ButtonPermissions { get; set; }
    }
}
