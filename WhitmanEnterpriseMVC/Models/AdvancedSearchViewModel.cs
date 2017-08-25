using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.Models
{
    public class AdvancedSearchViewModel
    {
        public AdvancedSearchViewModel()
        {
            Categories = new List<SelectListItem>()
                             {
                                 new SelectListItem(){ Selected = false, Text = "All", Value = "All"},
                                 new SelectListItem(){ Selected = false, Text = "New", Value = "New"},
                                 new SelectListItem(){ Selected = false, Text = "Used", Value = "Used"},
                                 new SelectListItem(){ Selected = false, Text = "Sold", Value = "Sold"},
                                 new SelectListItem(){ Selected = false, Text = "Wholesale", Value = "Wholesale"},
                                 new SelectListItem(){ Selected = false, Text = "Appraisals", Value = "Appraisals"}
                             };

            Years = new List<SelectListItem>()
                        {
                        };

            Makes = new List<SelectListItem>()
                        {
                        };

            Models = new List<SelectListItem>()
                         {
                         };
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
    }
}

namespace WhitmanEnterpriseMVC.DatabaseModel
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
