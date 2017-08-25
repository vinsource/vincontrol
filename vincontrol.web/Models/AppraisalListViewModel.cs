using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.DomainObject;

namespace Vincontrol.Web.Models
{
    public class AppraisalListViewModel
    {
        public AppraisalListViewModel()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public List<AppraisalViewFormModel> RecentAppraisals { get; set; }

        public List<AppraisalViewFormModel> UnlimitedAppraisals { get; set; }

        public IEnumerable<ExtendedSelectListItem> SortSetList { get; set; }

        public string SelectedSortSet { get; set; }

        public int NumberOfRecords { get; set; }
    }
}
