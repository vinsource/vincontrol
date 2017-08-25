using System;

namespace vincontrol.DomainObject
{
    public class CarResult
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int DealerId { get; set; }
        public decimal? SalePrice { get; set; }
        public long? Mileage { get; set; }
        public int ListingId { get; set; }
        public string Stock { get; set; }
        public string ExteriorColor { get; set; }
        public int VehicleStatus { get; set; }
        public string PhotoUrl { get; set; }
        [ScriptIgnore]
        public string DefaultStockImage { get; set; }
        [ScriptIgnore]
        public string ThumbnailUrl { get; set; }
    }

    public class ScriptIgnoreAttribute : Attribute
    {
    }

    public class VehicleQuery
    {
        public string AppraisalQuery { get; set; }
        public string InventoryQuery { get; set; }
        public string SoldoutQuery { get; set; }
    }

    public class VehicleResult : CarResult
    {
        public string Vin { get; set; }
        public decimal? ACV { get; set; }
        public decimal? DealerCost { get; set; }
        public string InteriorColor { get; set; }
        public bool Condition { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? NumberOfCar { get; set; }
    }

    public class DwVehicleResult : VehicleResult
    {
        public string Descriptions { get; set; }
        
    }
}