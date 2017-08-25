using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;

namespace vincontrol.Application.ViewModels.AccountManagement
{
    public class DealerGroupViewModel
    {
        public string DealershipGroupId { get; set; }

        public string DealershipGroupName { get; set; }

        public int DealershipGroupDefaultLogin { get; set; }

        public int DefaultLoginForUser{ get; set; }

        public List<DealershipViewModel> DealerList { get; set; }
        
    }
}
