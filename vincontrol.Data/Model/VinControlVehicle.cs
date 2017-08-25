using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Data.Model
{
    public class VinControlVehicle
    {
        public int ListingId { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string ModelNumber { get; set; }

        public string Trim { get; set; }

        public string VIN { get; set; }

        public int DaysInInventory { get; set; }

        public string StockNumber { get; set; }

        public decimal SalePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public string MonthlyPayment { get; set; }

        public decimal MSRP { get; set; }

        public decimal DealerMSRP { get; set; }

        public long Mileage { get; set; }

        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string InteriorSurface { get; set; }

        public string BodyType { get; set; }

        public string EngineType { get; set; }

        public string DriveTrain { get; set; }

        public int Cylinders { get; set; }

        public double Litter { get; set; }

        public string FuelType { get; set; }

        public string Tranmission { get; set; }

        public int Doors { get; set; }

        public bool Certified { get; set; }

        public string StandardOptions { get; set; }

        public string AdditionalOptions { get; set; }

        public string AdditionalPackages { get; set; }

        public string OptionCodes { get; set; }


        public string Descriptions { get; set; }

        public string DefaultStockImage { get; set; }


        public string ThumbnailUrl { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime DateInStock { get; set; }

        public string DealerName { get; set; }

        public string DealerAddress { get; set; }

        public string DealerCity { get; set; }

        public string DealerState { get; set; }

        public string DealerZipCode { get; set; }

        public string DealerPhone { get; set; }

        public string DealerEmail { get; set; }

        public int DealerId { get; set; }

        public decimal DealerCost { get; set; }

        public decimal ACV { get; set; }

        public decimal WindowStickerPrice { get; set; }

        public bool Recon { get; set; }

        public bool WholeSale { get; set; }

        public bool IsRetail { get; set; }



        public string AddToInventoryBy { get; set; }

        public string AppraisalId { get; set; }


        public string FuelEconomyCity { get; set; }

        public string FuelEconomyHighWay { get; set; }
        public short VehicleType { get; set; }
        public bool IsTruck { get; set; }

        public bool PriorRental { get; set; }

        public bool DealerDemo { get; set; }

        public bool Unwind { get; set; }

        public string TruckClass { get; set; }

        public string TruckCategory { get; set; }
        public string TruckType { get; set; }

        public int Warranty { get; set; }

        public decimal DealerDiscount { get; set; }

        public decimal ManufacturerRebate { get; set; }

        public decimal Invoice { get; set; }

        public string Disclaimer { get; set; }

        public int Age { get; set; }

        public int CarFaxOwner { get; set; }

        public string OneCarFaxOnwer { get; set; }

        public string AddtionalTitle { get; set; }

        public string KBBTrimOption { get; set; }

        public int KBBtrimId { get; set; }

        public int TruckClassId { get; set; }
        public bool Condition { get; set; }

        public string StatusType { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string ExternalPhotoUrl { get; set; }
        /*
         * 0:Retail New
         * 1:Retail Used
         * 2:Recon
         * 5:Wholesale
         * 6:Loaner
         * 7:Auction
         */

        //public static class InventoryStatus
        //{
        //    public const int Retail = 0;
        //    public const int SoldOut = 1;
        //    public const int Inventory = 2;
        //    public const int Wholesale = 3;
        //    public const int Recon = 4;
        //    public const int Auction = 5;
        //    public const int Loaner = 6;
        //    public const int TradeNotClear = 7;
        //}

        public short InventoryStatus { get; set; }

        public bool Loaner { get; set; }

        public bool Auction { get; set; }

        public bool TradeNotClear { get; set; }

        public bool Featured { get; set; }

        public int WarrantyType { get; set; }

        public bool PriceFromThirdParty { get; set; }

        public bool PhotoFromThirdParty { get; set; }

        public int VinListingId { get; set; }

        public int MarketRange { get; set; }
    }
}
