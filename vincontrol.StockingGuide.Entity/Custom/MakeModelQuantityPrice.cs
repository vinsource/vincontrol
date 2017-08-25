using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vincontrol.StockingGuide.Entity.Custom
{
    public class MakeModelQuantityPrice
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Count { get; set; }
        public double? Age { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
        public int? AveragePrice { get; set; }
    }
}
