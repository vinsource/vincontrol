using System.Collections.Generic;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;

namespace Vincontrol.Web.Models
{
    public class InventoryFormViewModel
    {
        public InventoryFormViewModel()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public List<CarInfoFormViewModel> CarsList { get; set; }

        public List<CarInfoFormViewModel> SubSetList { get; set; }

        public string previousCriteria { get; set; }

        public bool sortASCOrder { get; set; }

        public bool CurrentOrSoldInventory { get; set; }

        public bool CombineInventory { get; set; }

        public DealerGroupViewModel DealerGroup { get; set; }

        public string SelectedDealership { get; set; }

        public IEnumerable<ExtendedSelectListItem> DealerList { get; set; }

        public IEnumerable<ExtendedSelectListItem> SortSetList { get; set; }

        public string SelectedSortSet { get; set; }

        public bool IsCompactView { get; set; }

        public bool IsMissingContent { get; set; }
        
        public IEnumerable<DealershipActivityViewModel> DealershipActivities { get; set; }
    }
}
