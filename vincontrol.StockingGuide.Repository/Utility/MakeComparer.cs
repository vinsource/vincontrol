using System.Collections.Generic;
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
