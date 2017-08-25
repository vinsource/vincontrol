using System.Collections.Generic;


namespace WhitmanEnterpriseMVC.Models
{
    public class AppraisalInfoPrintViewModel : PrintPageBaseViewModel
    {
        public IEnumerable<AppraisalInfoViewModel> CarInfoList { get; set; }
        public int NoOfDays { get; set; }       
    }
}
