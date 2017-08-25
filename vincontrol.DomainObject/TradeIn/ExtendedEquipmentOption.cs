using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject.TradeIn
{
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
