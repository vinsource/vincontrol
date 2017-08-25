using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.AdvancedSearch
{
    public class AdvanceSearchItem
    {
        public AdvanceSearchItem(SoldoutInventory item, int oldType)
        {
            OldType = oldType;
            ListingId = item.SoldoutInventoryId;
            VehicleId = item.VehicleId;
            Vehicle = item.Vehicle;
            DealerId = item.DealerId;
            Stock = item.Stock ?? string.Empty;
            Mileage = item.Mileage;
            ExteriorColor = item.ExteriorColor ?? string.Empty;
            PhotoUrl = item.PhotoUrl;
            SalePrice = item.SalePrice;
            DateInStock = item.DateInStock;
            ThumbnailUrl = item.ThumbnailUrl;
            MarketRange = 0; // TODO: Soldout table doesn't have reference to MarketRange
            CarfaxOwner = item.CarFaxOwner ?? 0;
            Descriptions = item.Descriptions ?? string.Empty;
        }

        public AdvanceSearchItem(Inventory item, int oldType)
        {
            OldType = oldType;
            ListingId = item.InventoryId;
            VehicleId = item.VehicleId;
            Vehicle = item.Vehicle;
            DealerId = item.DealerId;
            Stock = item.Stock ?? string.Empty;
            Mileage = item.Mileage;
            ExteriorColor = item.ExteriorColor ?? string.Empty;
            PhotoUrl = item.PhotoUrl;
            SalePrice = item.SalePrice;
            DateInStock = item.DateInStock;
            ThumbnailUrl = item.ThumbnailUrl;
            MarketRange = item.MarketRange;
            CarfaxOwner = item.CarfaxOwner ?? 0;
            Descriptions = item.Descriptions ?? string.Empty;
        }

        public AdvanceSearchItem(Appraisal item, int oldType)
        {
            OldType = oldType;
            ListingId = item.AppraisalId;
            VehicleId = item.VehicleId;
            Vehicle = item.Vehicle;
            DealerId = item.DealerId;
            Stock = item.Stock ?? string.Empty;
            Mileage = item.Mileage;
            ExteriorColor = item.ExteriorColor ?? string.Empty;
            PhotoUrl = item.PhotoUrl;
            SalePrice = item.SalePrice;
            DateInStock = DateTime.Now;
            ThumbnailUrl = item.ThumbnailUrl;
            MarketRange = item.MarketRange;
            CarfaxOwner = item.CARFAXOwner ?? 0;
            Descriptions = item.Descriptions ?? string.Empty;
        }

        public int ListingId { get; set; }
        public int DealerId { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public string Stock { get; set; }
        public long? Mileage { get; set; }
        public string ExteriorColor { get; set; }
        public string PhotoUrl { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? DateInStock { get; set; }
        public string ThumbnailUrl { get; set; }
        public int? MarketRange { get; set; }
        public int CarfaxOwner { get; set; }
        public string Descriptions { get; set; }
        public int OldType { get; set; }
    }
}
