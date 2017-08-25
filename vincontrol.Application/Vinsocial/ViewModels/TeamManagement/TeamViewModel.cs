using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Application.Vinsocial.ViewModels.SurveyManagement;
using vincontrol.Data.Model;

namespace vincontrol.Application.Vinsocial.ViewModels.TeamManagement
{
    public class TeamViewModel
    {
        public TeamViewModel(){}

        public TeamViewModel(Team obj)
        {
            TeamId = obj.TeamId;
            TeamName = obj.Name;
            ManagerId = obj.ManagerId;
            DealerId = obj.DealerId;
            //Members = obj.TeamUsers.Any()
            //              ? obj.TeamUsers.Select(i => new TeamMemberViewModel(i)).ToList()
            //              : new List<TeamMemberViewModel>();
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public List<TeamMemberViewModel> Members { get; set; }
        public int DealerId { get; set; }
    }

    public class TeamMemberViewModel
    {
        public TeamMemberViewModel(){}

        public TeamMemberViewModel(User obj)
        {
            //TeamUserId = obj.TeamUserId;
            TeamId = obj.TeamId ?? 0;
            UserId = obj.UserId;
            NumberOfReviews = 0;
            NumberOfSurveys = 0;
        }

        public int TeamUserId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public string UserIds { get; set; }
        public string Name { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfSurveys { get; set; }
    }

    public class TeamRatingViewModel
    {
        public TeamRatingViewModel()
        {
        }
        public string TeamName { get; set; }
        public decimal Rating { get; set; }
        public int TeamId { get; set; }
    }

    public class TeamUserReviewBreakdownViewModel
    {
        public TeamUserReviewBreakdownViewModel()
        {
            
        }

        public int TeamId { get; set; }
        public string FullName { get; set; }
        public List<UserReviewViewModel> UserReviews { get; set; }
        public int UserId { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfSurveys { get; set; }
    }

    public class TeamSurveysBreakdownViewModel
    {
        public TeamSurveysBreakdownViewModel()
        {
            
        }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public List<SurveyViewModel> Surveys { get; set; }
    }

}
