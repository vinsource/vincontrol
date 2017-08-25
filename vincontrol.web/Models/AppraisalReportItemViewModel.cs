using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincontrol.Web.Models
{
    public class AppraisalReportItemViewModel : ReportItemViewModel
    {
        public string Date { get { return FullDate.ToShortDateString(); } }
        public DateTime FullDate { get; set; }
    }
}
