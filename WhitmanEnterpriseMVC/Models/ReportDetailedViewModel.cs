using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class ReportDetailedViewModel
    {
        public IEnumerable<WhitmanEnterpriseMVC.Models.AppraisalReportItemViewModel> Appraisals;
        public IEnumerable<WhitmanEnterpriseMVC.Models.InventoryReportItemViewModel> Inventories;
    }
}
