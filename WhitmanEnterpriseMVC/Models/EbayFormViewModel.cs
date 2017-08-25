using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
{
    public class EbayFormViewModel
    {
        public string ListingId { get; set; }

        public string SellerProvidedTitle { get; set; }

        public string EbayListingId { get; set; }

        public IEnumerable<SelectListItem> AuctionType { get; set; }

        public string SelectedAuctionType { get; set; }

        public bool LimitedWarranty { get; set; }

        public bool BoldTitle { get; set; }

        public bool Highlight { get; set; }

        public bool Border { get; set; }

        public bool Propackbundle { get; set; }

        public bool Featured { get; set; }

        public IEnumerable<SelectListItem> Gallerys { get; set; }

        public string SelectedGallery { get; set; }

        public string StartingPrice { get; set; }

        public string ReservePrice { get; set; }

        public string MinimumPrice { get; set; }

        public string BuyItNowPrice { get; set; }
             
        public bool PaPalDeposit { get; set; }

        public bool HitCounter { get; set; }

        public IEnumerable<SelectListItem> HoursToDeposit { get; set; }

        public string SelectedHoursToDeposit { get; set; }

        public IEnumerable<SelectListItem> AuctionLength { get; set; }

        public string SelectedAuctionLength { get; set; }

        public IEnumerable<SelectListItem> ExteriorColorList { get; set; }

        public string SelectedExteriorColor { get; set; }

        public IEnumerable<SelectListItem> InteriorColorList { get; set; }

        public string SelectedInteriorColor { get; set; }
        
        public IEnumerable<SelectListItem> StartTime { get; set; }

        public string SelectedStartTime { get; set; }


        public CarInfoFormViewModel VehicleInfo { get; set; }

        public DealershipViewModel Dealer { get; set; }

        public string TotalListingFee { get; set; }

        public string EbayCategoryID { get; set; }

        public string XMLListingEnhancement { get; set; }

        public List<PostEbayAds> PostEbayList { get; set; }

        public bool SessionTimeOut { get; set; }


    }

    public class PostEbayAds
    {
        public int ListingId { get; set; }

        public string Title { get; set; }

        public int ModelYear { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string ebayAdID { get; set; }

        public string ebayAdURL { get; set; }

        public DateTime ebayAdStartTime { get; set; }

        public DateTime ebayAdEndTime { get; set; }

        public string ebayThumbNailPic { get; set; }
    }


    
}
