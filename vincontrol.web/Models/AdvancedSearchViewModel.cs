using System.Collections.Generic;
using System.Web.Mvc;
using vincontrol.Constant;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;

namespace Vincontrol.Web.Models
{
    public class AdvancedSearchViewModel
    {
        public AdvancedSearchViewModel()
        {
            if (SessionHandler.CurrentUser.RoleId == Constanst.RoleType.Employee)
            {
                Categories = new List<SelectListItem>()
                             {
                                 new SelectListItem(){ Selected = false, Text = "All", Value = "All"},
                                 new SelectListItem(){ Selected = false, Text = "Used", Value = "Used"},
                                 new SelectListItem(){ Selected = false, Text = "New", Value = "New"},
                                 new SelectListItem(){ Selected = false, Text = "Recent appraisal", Value = "Recent"},
                                 new SelectListItem(){ Selected = false, Text = "Pending appraisal", Value = "Pending"},
                             };
            }
            else
            {
                Categories = new List<SelectListItem>()
                             {
                                 new SelectListItem(){ Selected = false, Text = "All", Value = "All"},
                                 new SelectListItem(){ Selected = false, Text = "Used", Value = "Used"},
                                 new SelectListItem(){ Selected = false, Text = "New", Value = "New"},
                                 new SelectListItem(){ Selected = false, Text = "Loaner", Value = "Loaner"},
                                 new SelectListItem(){ Selected = false, Text = "Auction", Value = "Auction"},
                                 new SelectListItem(){ Selected = false, Text = "Recon", Value = "Recon"},
                                 new SelectListItem(){ Selected = false, Text = "Wholesale", Value = "Wholesale"},
                                 new SelectListItem(){ Selected = false, Text = "Trade Not Clear", Value = "TradeNotClear"},
                                 new SelectListItem(){ Selected = false, Text = "Sold", Value = "Sold"},
                                 new SelectListItem(){ Selected = false, Text = "Recent appraisal", Value = "Recent"},
                                 new SelectListItem(){ Selected = false, Text = "Pending appraisal", Value = "Pending"},
                             };
            }

            Years = new List<SelectListItem>()
                        {
                        };

            Makes = new List<SelectListItem>()
                        {
                        };

            Models = new List<SelectListItem>()
                         {
                         };
            PageIndex = 1;
            PageSize = 10;
        }

        public int DealershipId { get; set; }
        public string Text { get; set; }
        public string SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string SelectedYear { get; set; }
        public List<SelectListItem> Years { get; set; }
        public string SelectedMake { get; set; }
        public List<SelectListItem> Makes { get; set; }
        public string SelectedModel { get; set; }
        public List<SelectListItem> Models { get; set; }
        public int NoOfNew { get; set; }
        public int NoOfUsed { get; set; }
        public int NoOfSold { get; set; }
        public int NoOfWholesale { get; set; }
        public int NoOfAppraisals { get; set; }
        public string SortField { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

namespace Vincontrol.Web.DatabaseModel
{
    public partial class whitmanenterprisedealershipinventory
    {
        // 0: New
        // 1: Used
        // 2: Wholesale
        // 3: Appraisal
        // 4: Sold
        public int Type { get; set; }
    }
}
