using System.Collections.Generic;
using vincontrol.Data.Model;

namespace Vincontrol.Web.Models
{
    public class VinDatabaseInfo
    {
        public Inventory InventoryItem { get; set; }
        //public Appraisal AppraisalItem { get; set; }
        public SoldoutInventory SoldoutItem { get; set; }
        public IEnumerable<Appraisal> Appraisals { get; set; }
    }
}