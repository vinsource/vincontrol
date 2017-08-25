using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class SilverlightImageViewModel
    {
        public int ListingId { get; set; }

        public string Vin { get; set; }

        public int DealerId { get; set; }       

        public int InventoryStatus { get; set; }

        public bool SessionTimeOut { get; set; }

        public string ImageServiceURL { get; set; }

    }
}
