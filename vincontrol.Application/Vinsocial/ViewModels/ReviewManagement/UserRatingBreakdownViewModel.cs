using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    public class UserRatingBreakdownViewModel
    {
        public UserRatingBreakdownViewModel() { }
        public int UserId { get; set; }
        public int DealerId { get; set; }
        public int New { get; set; }
        public decimal AverageRating { get; set; }
        public string SiteName { get; set; }
        public int Today { get; set; }
        public int Total { get; set; }
        public bool IsFitlerd { get; set; }
        public DateTime ReviewDate { get; set; }
        public string SiteLogo { get; set; }
        public int SiteId { get; set; }
        public string StarsNo { get; set; }
    }
}
