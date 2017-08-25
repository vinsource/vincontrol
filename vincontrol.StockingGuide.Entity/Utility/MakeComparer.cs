using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Entity.Utility
{
    public class MakeComparer : IEqualityComparer<MakeModelDateInStock>
    {
        public bool Equals(MakeModelDateInStock x, MakeModelDateInStock y)
        {
            return x.Make == y.Make;
        }

        public int GetHashCode(MakeModelDateInStock obj)
        {
            return obj.Make.GetHashCode();
        }
    }
}
