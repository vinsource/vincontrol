using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class ImageViewModel
    {
        public int ListingId { get; set; }

        public int AppraisalId { get; set; }

        public string Vin { get; set; }

        public int DealerId { get; set; }

        public string ImageUploadFiles { get; set; }

        public string ThumbnailImageUploadFiles { get; set; }

        public string CarImageUrl { get; set; }

        public int InventoryStatus { get; set; }

        public bool SessionTimeOut { get; set; }
        
        public string UserUpload { get; set; }
    }
}
