using System.Collections.Generic;
using vincontrol.DomainObject;
using SelectListItem = System.Web.Mvc.SelectListItem;


namespace Vincontrol.Web.Models
{
    public class MarketFilterSource
    {
        public List<ExtendedSelectListItem> Makes { get; set; }
        public List<ExtendedSelectListItem> Models { get; set; }
        public List<ExtendedSelectListItem> Year { get; set; }
        public string SelectedMakeId { get; set; }
        public string SeletedModelId { get; set; }
        public string SeletedYearFrom { get; set; }
        public string SeletedYearTo { get; set; }

    }
}