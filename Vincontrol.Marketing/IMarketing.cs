using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincontrol.Marketing
{
    public abstract class IMarketing
    {
        public abstract void SendFlyer(int inventoryId, string emails, string names, int userId);
    }
}
