using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class ButtonPermissionViewModel
    {
        public int Id { get; set; }
        public int DealershipId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public IList<Button> Buttons { get; set; }
    }

    public class Button
    {
        public int ButtonId { get; set; }
        public string ButtonName { get; set; }
        public string Screen { get; set; }
        public bool CanSee { get; set; }
    }
}
