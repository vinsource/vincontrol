using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class BucketJumpHistory
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public string UserStamp { get; set; }
        public DateTime DateStamp { get; set; }
        public string AttachFile { get; set; }
        public decimal SalePrice { get; set; }
        public decimal RetailPrice { get; set; }
        public string Type { get; set; }
        public int BucketJumpDayAlert { get; set; }
        public DateTime BucketJumpCompleteDate { get; set; }
    }
}