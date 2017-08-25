using System.Collections.Generic;

namespace Vincontrol.Web.Models
{
    public class AdvisorListViewModel
    {
        public AdvisorListViewModel()
        {
            AdvisorList = new List<TradeinCustomerViewModel>();
        }

        public List<TradeinCustomerViewModel> AdvisorList { get; set; }
    }
}
