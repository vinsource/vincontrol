using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class CarStatusViewModel
    {
        public int Type { get; set; }
        public int ListingId { get; set; }
        public int InventoryStatus { get; set; }
    }
}