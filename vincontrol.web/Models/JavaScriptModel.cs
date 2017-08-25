using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class JavaScriptModel
    {
        public JavaScriptModel()
        {
          
        }


        public string ListingId { get; set; }

        public string AppraisalId { get; set; }

        public string Status { get; set; }

        public string Vin { get; set; }
        public string Stock { get; set; }

        public bool IsEdit { get; set; }

        public string AppraisalDate { get; set; }
    }
}
