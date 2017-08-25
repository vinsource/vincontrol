using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
using Vincontrol.Web.Controllers;


namespace Vincontrol.Web.Models
{
    public class PriceChangeViewModel
    {
        public IEnumerable<PriceChangeHistory> PriceChangeHistory { get; set; }
        
        public string Id { get; set; }

        public int  InventoryStatus { get; set; }

        public ChartTimeType ChartType { get; set; }
    }
}
