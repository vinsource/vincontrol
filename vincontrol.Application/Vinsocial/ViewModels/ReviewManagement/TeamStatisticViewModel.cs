using System.Collections.Generic;
using vincontrol.Application.Vinsocial.ViewModels.TeamManagement;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    public class TeamStatisticViewModel
    {
        public TeamStatisticViewModel()
        {
            TopSales = new List<UserStatisticViewModel>();
            Teams = new List<SelectListItem>();
        }

        public TeamViewModel TeamInfo { get; set; }
        public List<UserStatisticViewModel> TopSales { get; set; }
        public int NewReviews { get; set; }
        public int TotalReviews { get; set; }
        public int NewSurveys { get; set; }
        public int TotalSurveys { get; set; }
        public decimal AverageRating { get; set; }
        public List<SelectListItem> Teams { get; set; }
    }
}
