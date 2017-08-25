using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class CostRetail
    {
        public decimal VehicleTireCost { get; set; }
        public decimal FrontBumperCost { get; set; }
        public decimal RearBumperCost { get; set; }
        public decimal GlassCost { get; set; }
        public decimal FrontEndCost { get; set; }
        public decimal RearEndCost { get; set; }
        public decimal DriverSideCost { get; set; }
        public decimal PassengerSideCost { get; set; }
        public decimal LightBulbCost { get; set; }
        public decimal VehicleTireRebate { get; set; }
        public decimal FrontBumperRebate { get; set; }
        public decimal RearBumperRebate { get; set; }
        public decimal GlassRebate { get; set; }
        public decimal FrontEndRebate { get; set; }
        public decimal RearEndRebate { get; set; }
        public decimal DriverSideRebate { get; set; }
        public decimal PassengerSideRebate { get; set; }
        public decimal LightBulbRebate { get; set; }
        public decimal AverageEstimatedCost { get; set; }
        public decimal AverageDesiredProfit { get; set; }
        public decimal AverageDesiredProfitPercentage { get; set; }
    }
}