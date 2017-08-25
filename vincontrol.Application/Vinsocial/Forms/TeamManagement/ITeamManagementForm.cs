using System.Collections.Generic;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Application.Vinsocial.ViewModels.TeamManagement;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.Forms.TeamManagement
{
    public interface ITeamManagementForm
    {
        List<TeamViewModel> GetTeams(int dealerId);
        List<TeamRatingViewModel> GetTeamRating(int dealerId);
        List<UserReviewViewModel> GetUserReviewByTeamId(int teamId);
        List<TeamUserReviewBreakdownViewModel> GetUserReviewDetailByTeamId(int teamId);
        List<TeamSurveysBreakdownViewModel> GetSurveysDetailByTeamId(int teamId);
        List<SelectListItem> GetTeamNames(int dealerId);
        List<SelectListItem> GetTeamNamesByUser(int userId);
        void AddNewTeam(TeamViewModel obj);
        void AddNewTeamMember(TeamMemberViewModel obj);
        void AddNewTeamMembers(TeamMemberViewModel obj);
        void DeleteTeam(int teamId);
        void DeleteTeamMember(int memberId);
        void DeleteTeamMembers(int teamId);
        List<TeamStatisticViewModel> GetTeamsStatistic(int userId);
    }
}
