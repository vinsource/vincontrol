using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace Vincontrol.Web.Models
{
    public class EbayFormViewModel
    {
        public string ListingId { get; set; }

        public string SellerProvidedTitle { get; set; }

        public string EbayListingId { get; set; }

        public IEnumerable<ExtendedSelectListItem> AuctionType { get; set; }

        public string SelectedAuctionType { get; set; }

        public bool LimitedWarranty { get; set; }

        public bool BoldTitle { get; set; }

        public bool Highlight { get; set; }

        public bool Border { get; set; }

        public bool Propackbundle { get; set; }

        public bool Featured { get; set; }

        public IEnumerable<ExtendedSelectListItem> Gallerys { get; set; }

        public string SelectedGallery { get; set; }

        public string StartingPrice { get; set; }

        public string ReservePrice { get; set; }

        public string MinimumPrice { get; set; }

        public string BuyItNowPrice { get; set; }
             
        public bool PaPalDeposit { get; set; }

        public bool HitCounter { get; set; }

        public IEnumerable<ExtendedSelectListItem> HoursToDeposit { get; set; }

        public string SelectedHoursToDeposit { get; set; }

        public IEnumerable<ExtendedSelectListItem> AuctionLength { get; set; }

        public string SelectedAuctionLength { get; set; }

        public IEnumerable<ExtendedSelectListItem> ExteriorColorList { get; set; }

        public string SelectedExteriorColor { get; set; }

        public IEnumerable<ExtendedSelectListItem> InteriorColorList { get; set; }

        public string SelectedInteriorColor { get; set; }

        public IEnumerable<ExtendedSelectListItem> StartTime { get; set; }

        public string SelectedStartTime { get; set; }


        public CarInfoFormViewModel VehicleInfo { get; set; }

        public DealershipViewModel Dealer { get; set; }

        public string TotalListingFee { get; set; }

        public int EbayCategoryID { get; set; }

        public string XMLListingEnhancement { get; set; }

        public List<PostEbayAds> PostEbayList { get; set; }

        public bool SessionTimeOut { get; set; }

        public short InventoryStatus { get; set; }

        public string HtmlSource { get; set; }
    }

    public class PostEbayAds
    {
        public int ListingId { get; set; }

        public string Title { get; set; }

        public int ModelYear { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string EbayAdID { get; set; }

        public string EbayAdUrl { get; set; }

        public DateTime EbayAdStartTime { get; set; }

        public DateTime EbayAdEndTime { get; set; }

        public string EbayThumbNailPic { get; set; }

        public decimal SalePrice { get; set; }
    }


    
}
