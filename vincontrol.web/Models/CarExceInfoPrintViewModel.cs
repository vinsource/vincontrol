using System.Collections.Generic;

namespace Vincontrol.Web.Models
{   
    public class CarExcelInfoPrintViewModel : PrintPageBaseViewModel
    {
        public IEnumerable<CarExcelInfoViewModel> CarInfoList { get; set; }
    }
}