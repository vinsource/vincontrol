using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class PrintBaseViewModel
    {
        public string ModelYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string StockNumber { get; set; }
        public string Vin { get; set; }
        public string Mileage { get; set; }
        public string ExteriorColor { get; set; }
        public string SalePrice { get; set; }
        public int Days { get; set; }
        public string imagesNum { get; set; }       
    }
}
