//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vincontrol.WebAPI.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class InventoryStatusCode
    {
        public InventoryStatusCode()
        {
            this.Inventories = new HashSet<Inventory>();
            this.SoldoutInventories = new HashSet<SoldoutInventory>();
        }
    
        public short InventoryStatusCodeId { get; set; }
        public string Value { get; set; }
    
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<SoldoutInventory> SoldoutInventories { get; set; }
    }
}
