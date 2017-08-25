using System.Collections.Generic;


namespace Vincontrol.Web.Models
{
    public class AppraisalInfoPrintViewModel : PrintPageBaseViewModel
    {
        public IEnumerable<AppraisalInfoViewModel> CarInfoList { get; set; }
        public int NoOfDays { get; set; }       
    }
}
