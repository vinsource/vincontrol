using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class AppraisalReportItemViewModel : ReportItemViewModel
    {
        public string Date { get { return FullDate.ToShortDateString(); } }
        public DateTime FullDate { get; set; }
    }
}
