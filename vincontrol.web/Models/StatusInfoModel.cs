using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.DomainObject;

namespace Vincontrol.Web.Models
{
    public class StatusInfoModel
    {
        public string Title { get; set; }
        public string CurrentStatus { get; set; }
        public short CurrentStatusID { get; set; }
        public IEnumerable<ExtendedSelectListItem> ListStatus { get; set; }
        public int ListingID { get; set; }
        public int AppraisalID { get; set; }
        public string Vin { get; set; }
    }
}
