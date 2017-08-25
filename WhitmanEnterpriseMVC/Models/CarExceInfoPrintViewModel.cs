using System.Collections.Generic;

namespace WhitmanEnterpriseMVC.Models
{   
    public class CarExcelInfoPrintViewModel : PrintPageBaseViewModel
    {
        public IEnumerable<CarExcelInfoViewModel> CarInfoList { get; set; }
    }
}