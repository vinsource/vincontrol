using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vincontrol.StockingGuide.Entity.Custom
{
    public class DealerLocation
    {
        public int DealerId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
