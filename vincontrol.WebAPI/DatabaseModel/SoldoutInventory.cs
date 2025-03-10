//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vincontrol.WebAPI.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class SoldoutInventory
    {
        public int SoldoutInventoryId { get; set; }
        public string Stock { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> DealerMsrp { get; set; }
        public Nullable<long> Mileage { get; set; }
        public string ExteriorColor { get; set; }
        public Nullable<bool> Certified { get; set; }
        public string AdditionalOptions { get; set; }
        public string AdditionalPackages { get; set; }
        public string Descriptions { get; set; }
        public string PhotoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public Nullable<System.DateTime> DateInStock { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<System.DateTime> DateRemoved { get; set; }
        public int DealerId { get; set; }
        public Nullable<decimal> DealerCost { get; set; }
        public Nullable<decimal> ACV { get; set; }
        public bool Condition { get; set; }
        public string RemoveBy { get; set; }
        public Nullable<int> AddToInventoryById { get; set; }
        public Nullable<int> AppraisalId { get; set; }
        public Nullable<bool> DataFeed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Nullable<int> WarrantyInfo { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public Nullable<decimal> WindowStickerPrice { get; set; }
        public Nullable<decimal> DealerDiscount { get; set; }
        public Nullable<decimal> ManufacturerRebate { get; set; }
        public Nullable<bool> PriorRental { get; set; }
        public Nullable<int> CarFaxOwner { get; set; }
        public string Disclaimer { get; set; }
        public Nullable<bool> DealerDemo { get; set; }
        public Nullable<bool> Unwind { get; set; }
        public Nullable<int> CarRanking { get; set; }
        public string AdditionalTitle { get; set; }
        public string Country { get; set; }
        public Nullable<bool> EnableAutoDescription { get; set; }
        public Nullable<int> Template { get; set; }
        public string PackageDescriptions { get; set; }
        public Nullable<int> BucketJumpCompleteDay { get; set; }
        public string MarketTrim { get; set; }
        public Nullable<bool> ACar { get; set; }
        public Nullable<int> TempOldListingId { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public Nullable<bool> BrandedTitle { get; set; }
        public Nullable<short> InventoryStatusCodeId { get; set; }
        public int VehicleId { get; set; }
    
        public virtual Dealer Dealer { get; set; }
        public virtual InventoryStatusCode InventoryStatusCode { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual User User { get; set; }
    }
}
