using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class ReportDetailedViewModel
    {
        public IEnumerable<AppraisalReportItemViewModel> Appraisals;
        public IEnumerable<InventoryReportItemViewModel> Inventories;
    }
}
