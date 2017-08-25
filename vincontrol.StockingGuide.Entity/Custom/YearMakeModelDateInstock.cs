using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vincontrol.StockingGuide.Entity.Custom
{
    public class YearMakeModelDateInStock
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime DateInStock { get; set; }
    }
}
