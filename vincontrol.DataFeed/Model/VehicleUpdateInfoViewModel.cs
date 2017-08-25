using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DataFeed.Model
{
    public class VehicleUpdateInfoViewModel
    {
        public int ListingId { get; set; }

        public string StockNumber { get; set; }

        public int ModelYear { get; set; }

        public string Vin { get; set; }

        public string Mileage { get; set; }

        public string Description { get; set; }

        public string ExteriorColor { get; set; }

        public string InteriorColor { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string SalePrice { get; set; }

        public string WindowStickerPrice { get; set; }

        public string CarImageURL { get; set; }

        public string NewUsed { get; set; }

        public string Tranmission { get; set; }

        public bool Recon { get; set; }

        public bool Certified { get; set; }

        public DateTime DateInStock { get; set; }

        public int Age { get; set; }

        public string VehicleType { get; set; }

        public int CarFaxOwner { get; set; }

        public string ACV { get; set; }

        public string DealerCost { get; set; }

        public string MSRP { get; set; }

        public bool WholeSale { get; set; }

        public bool PriorRental { get; set; }
    }
}
