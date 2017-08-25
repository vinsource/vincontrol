using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class PriceChangeHistory
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public string UserStamp { get; set; }
        public DateTime DateStamp { get; set; }
        public string AttachFile { get; set; }
        public decimal OldSalePrice { get; set; }
        public decimal NewSalePrice { get; set; }
    }
}