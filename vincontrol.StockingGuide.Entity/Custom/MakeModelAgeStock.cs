using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace vincontrol.StockingGuide.Entity.Custom
{
    public class MakeModelAgeStock
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Stock { get; set; }
        public double Age { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", Make, Model, Stock);
        }
    }
}
