using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.Vinchat.Forms.CommonManagement;
using vincontrol.Application.Vinsocial.Forms.ReviewManagement;
using vincontrol.Application.Vinsocial.Forms.SurveyManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Application.Vinsocial.ViewModels.TeamManagement;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;
using vincontrol.Constant;
using vincontrol.Data.Model;
using System.Data.Entity;
namespace vincontrol.Application.Vinsocial.Forms.TeamManagement
{
    public class TeamManagementForm : BaseForm, ITeamManagementForm
    {
        #region Constructors
        private IAccountManagementForm _accountManagementForm;
        private IReviewManagementForm _reviewManagementForm;
        private ISurveyManagementForm _surveyManagementForm;
        private ICommonManagementForm _commonManagementForm;

        public TeamManagementForm()
            : this(new SqlUnitOfWork())
        {
            _reviewManagementForm = new ReviewManagementForm();
            _accountManagementForm = new AccountManagementForm();
            _surveyManagementForm = new SurveyManagementForm();
            _commonManagementForm = new CommonManagementForm();
        }

        public TeamManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ITeamManagementForm' Members

        public List<TeamViewModel> GetTeams(int dealerId)
        {
            var allTeams = UnitOfWork.Team.GetTeams(dealerId).ToList();
            var result = new List<TeamViewModel>();

            foreach (var team in allTeams)
            {
                var teamModel = new TeamViewModel(team);
                var manager = _accountManagementForm.GetUser(team.ManagerId);
                if (manager != null)
                    teamModel.ManagerName = manager.Name;
                teamModel.Members = team.Users.Any()
                                  ? team.Users.Select(
                                      j =>
                                      new TeamMemberViewModel(j)
                                          {
                                              Name = _accountManagementForm.GetUser(j.UserId).Name,
                                              NumberOfReviews = UnitOfWork.Review.GetUserReviewsByUser(j.UserId).Count(),
                                              NumberOfSurveys = UnitOfWork.Survey.GetSurveys(j.UserId).Count()
                                          }).ToList()
                                  : new List<TeamMemberViewModel>();
                result.Add(teamModel);
            }

            return result;
        }

        public List<SelectListItem> GetTeamNames(int dealerId)
        {
            var allTeams = UnitOfWork.Team.GetTeams(dealerId);
            return allTeams.Any()
                       ? allTeams.AsEnumerable().Select(i => new SelectListItem(i.TeamId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetTeamNamesByUser(int userId)
        {
            var user = UnitOfWork.User.GetUser(userId);
            var userRoleIds = user.UserPermissions.Select(x => x.RoleId);
            var query = UnitOfWork.Team.GetTeams();
            

            if (userRoleIds.Contains(Constanst.RoleType.Employee))
            {
                return new List<SelectListItem>();
            }
            else if (userRoleIds.Contains(Constanst.RoleType.Manager))
            {
                query = query.Where(x => x.ManagerId == userId);
                return query.Any()
                       ? query.AsEnumerable().Select(i => new SelectListItem(i.TeamId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
            }
            else
            {
                return GetTeamNames(user.DefaultLogin.GetValueOrDefault());
            }
        }

        public List<TeamRatingViewModel> GetTeamRating(int dealerId)
        {
            var allTeams = UnitOfWork.User.GetDealerEmployees(dealerId);
            return allTeams.Where(i => i.TeamId.HasValue).GroupBy(i => i.TeamId).ToList().Select(i => new TeamRatingViewModel()
                {
                    TeamName = i.Select(j => j.Team.Name).FirstOrDefault(),
                    Rating = i.Average(j => GetAverageRatingByUserId(j.UserId)),
                    TeamId = i.FirstOrDefault().TeamId.Value
                }).ToList();
        }

        public List<UserReviewViewModel> GetUserReviewByTeamId(int teamId)
        {
            var allTeams = UnitOfWork.User.GetUsersByTeam(teamId);
            return
                allTeams.GroupBy(i => i.UserId)
                        .ToList()
                        .Select(i => new UserReviewViewModel()
                            {
                                Rating = GetAverageRatingByUserId(i.Select(j => j.UserId).FirstOrDefault()),
                                UserFullName = _accountManagementForm.GetUser(i.Select(j => j.UserId).FirstOrDefault()).Name,
                                UserId = i.Select(j => j.UserId).FirstOrDefault()
                            }).ToList();
        }

        public List<TeamUserReviewBreakdownViewModel> GetUserReviewDetailByTeamId(int teamId)
        {
            var allTeamUsers = UnitOfWork.User.GetUsersByTeam(teamId);

            var model = new List<TeamUserReviewBreakdownViewModel>();
            var teamUserIds = allTeamUsers.Select(x => x.UserId).ToList();
            var teamReviews = UnitOfWork.Review.GetAllUserReviews().Where(x => x.UserId != null && teamUserIds.Contains(x.UserId.Value)).ToList();
            var teamSurveys = UnitOfWork.Survey.GetAll().Where(x => x.UserId != null && teamUserIds.Contains(x.UserId));

            foreach (var user in allTeamUsers)
            {
                var modelItem = new TeamUserReviewBreakdownViewModel();
                var reviews = teamReviews.Where(x => x.UserId == user.UserId).ToList();

                modelItem.UserId = user.UserId;
                modelItem.TeamId = user.TeamId ?? 0;
                modelItem.UserReviews = reviews.Select(x => new UserReviewViewModel(x)).ToList();
                modelItem.NumberOfReviews = teamReviews.Count(x => x.UserId == user.UserId);
                modelItem.NumberOfSurveys = teamSurveys.Count(x => x.UserId == user.UserId);
                modelItem.FullName = user.Name;

                model.Add(modelItem);
            }

            return model;
        }

        public List<TeamSurveysBreakdownViewModel> GetSurveysDetailByTeamId(int teamId)
        {
            var allTeams = UnitOfWork.User.GetUsersByTeam(teamId);
            return allTeams.ToList().Select(i => new TeamSurveysBreakdownViewModel()
                {
                    UserId = i.UserId,
                    TeamId = i.TeamId ?? 0,
                    Surveys = _surveyManagementForm.GetSurveys(i.UserId)
                }).ToList();
        }

        public decimal GetAverageRatingByUserId(int userId)
        {
            var list = UnitOfWork.Review.GetUserReviewsByUserId(userId).ToList();
            return list.Any() ? list.Average(i => i.Rating) : 0;
        }

        public void AddNewTeam(TeamViewModel obj)
        {
            var team = MappingHandler.ConvertViewModelToTeam(obj);
            UnitOfWork.Team.AddNewTeam(team);
            //var manager = UnitOfWork.User.GetUser(team.ManagerId);
            //if (manager != null)
            //    manager.Team = team;
            UnitOfWork.CommitVincontrolModel();

            // Adding basic schedule for new team
            try
            {
                for (int i = 0; i <= 6; i++)
                {
                    var newSchedule = new VSRScheduleViewModel()
                    {
                        StartTime = 28800,
                        FinishTime = 68400,
                        Day = i,
                        Status = 0,
                        DealerId = obj.DealerId,
                        TeamId = team.TeamId
                    };
                    UnitOfWork.Common.AddNewVSRSchedule(MappingHandler.ToEntity(newSchedule));
                    UnitOfWork.CommitVincontrolModel();
                }
                
            }
            catch (Exception)
            {
                
            }
        }

        public void AddNewTeamMember(TeamMemberViewModel obj)
        {
            var existingUser = UnitOfWork.User.GetUser(obj.UserId);
            if (existingUser != null)
            {
                existingUser.TeamId = obj.TeamId;
                UnitOfWork.CommitVincontrolModel();

                try
                {
                    _commonManagementForm.AddNewUser(existingUser.UserName, existingUser.Password);

                    //var list = UnitOfWork.User.GetUsersByTeam(obj.TeamId);
                    AddRosterUser(existingUser);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public void AddRosterUser(User existingUser, int dealerId = 0)
        {
            var list =
                UnitOfWork.User.GetAllByDealer(dealerId.Equals(0) ? existingUser.DefaultLogin.GetValueOrDefault() : dealerId)
                    .Where(i => (i.TeamId != null && i.UserPermissions.FirstOrDefault().RoleId == Constanst.RoleType.Employee)
                                || i.UserPermissions.FirstOrDefault().RoleId == Constanst.RoleType.Admin
                                || i.UserPermissions.FirstOrDefault().RoleId == Constanst.RoleType.Manager);
            foreach (var user in list)
            {
                if (!existingUser.UserId.Equals(user.UserId))
                    _commonManagementForm.AddNewRosterUser(existingUser.UserName, user.UserName.ToLower() + "@localhost");
            }
        }

        public void AddNewTeamMembers(TeamMemberViewModel obj)
        {
            if (!String.IsNullOrEmpty(obj.UserIds) && obj.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Any())
            {
                foreach (var userId in obj.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList())
                {
                    obj.UserId = Convert.ToInt32(userId);
                    AddNewTeamMember(obj);
                }
            }
        }

        public void DeleteTeam(int teamId)
        {
            var users = UnitOfWork.User.GetUsersByTeam(teamId).ToList();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    user.TeamId = null;
                }
                UnitOfWork.CommitVincontrolModel();
            }
            UnitOfWork.Team.DeleteTeam(teamId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void DeleteTeamMember(int memberId)
        {
            var existingUser = UnitOfWork.User.GetUser(memberId);
            if (existingUser != null)
            {
                existingUser.TeamId = null;
                UnitOfWork.CommitVincontrolModel();

                try
                {
                    _commonManagementForm.DeleteUser(existingUser.UserName, existingUser.Password);
                    _commonManagementForm.DeleteRosterUsers(existingUser.UserName);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public void DeleteTeamMembers(int teamId)
        {
            var users = UnitOfWork.User.GetUsersByTeam(teamId).ToList();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    user.TeamId = null;
                    try
                    {
                        _commonManagementForm.DeleteUser(user.UserName, user.Password);
                        _commonManagementForm.DeleteRosterUsers(user.UserName);
                    }
                    catch (Exception)
                    {

                    }
                }
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public List<TeamStatisticViewModel> GetTeamsStatistic(int userId)
        {
            var model = new List<TeamStatisticViewModel>();
            var currentUser = UnitOfWork.User.GetUser(userId);
            var userRoleIds = currentUser.UserPermissions.Select(x => x.RoleId).ToList();

            var teams = new List<Team>();

            if (userRoleIds.Contains(Constanst.RoleType.Manager))
            {
                var teamByUser = UnitOfWork.Team.GetTeamByUser(userId);
                if (teamByUser != null)
                    teams.Add(teamByUser);
            }
            else if (userRoleIds.Contains(Constanst.RoleType.Master) || userRoleIds.Contains(Constanst.RoleType.Admin))
            {
                //TODO: need to put relationship for managerId column in team table
                var result = UnitOfWork.Team.GetTeams(currentUser.DefaultLogin.GetValueOrDefault()).ToList();
                if (result != null && result.Count > 0)
                    teams.AddRange(result);
            }

            int unassignedReviews = 0;
            if (currentUser.DefaultLogin != null)
            {
                var dealerUserIds = _accountManagementForm.GetUserIds(currentUser.DefaultLogin.Value);
                unassignedReviews = UnitOfWork.Review.GetUserReviews(currentUser.DefaultLogin.Value).Count(x => !x.UserId.HasValue || (x.UserId.HasValue && !dealerUserIds.Contains(x.UserId.Value)));
            }

            foreach (var team in teams)
            {
                var userIds = team.Users.Select(x => x.UserId).ToList();
                var teamStatistic = new TeamStatisticViewModel();
                teamStatistic.TeamInfo = new TeamViewModel(team);

                var topFive = UnitOfWork.Review.GetAllUserReviews()
                                               .Where(x => userIds.Contains(x.UserId.Value))
                                               .GroupBy(x => x.UserId)
                                               .Select(group => new
                                               {
                                                   UserId = group.Key,
                                                   AverageRating = group.Average(x => x.Rating)
                                               })
                                               .OrderByDescending(x => x.AverageRating)
                                               .Take(5);

                foreach (var item in topFive)
                {
                    var employee = UnitOfWork.User.GetUser(item.UserId.Value);
                    teamStatistic.TopSales.Add(new UserStatisticViewModel
                    {
                        UserId = item.UserId.Value,
                        EmployeeName = employee.Name,
                        AvatarUrl = employee.Photo,
                        AverageRating = (Math.Ceiling(item.AverageRating * 2)) / 2
                    });
                }

                var teamReviews = UnitOfWork.Review.GetAllUserReviews().Where(x => userIds.Contains(x.UserId.Value));
                var teamSurveys = UnitOfWork.Survey.GetAll().Where(x => userIds.Contains(x.UserId));
                teamStatistic.TotalReviews = teamReviews.Count();
                teamStatistic.NewReviews = unassignedReviews;
                teamStatistic.TotalSurveys = teamSurveys.Count();
                var today = DateTime.Now.Date;
                var tomorrow = today.AddDays(1);
                teamStatistic.NewSurveys = teamSurveys.Count(x => x.SurveyStatusId == vincontrol.Constant.Constanst.SurveyStatusIds.Submitted
                                                                    && x.DateStamp >= today && x.DateStamp < tomorrow);
                if (teamStatistic.TotalReviews > 0)
                {
                    teamStatistic.AverageRating = teamReviews.Average(x => x.Rating);
                    teamStatistic.AverageRating = (Math.Ceiling(teamStatistic.AverageRating * 2)) / 2;
                }

                model.Add(teamStatistic);
            }

            return model;
        }

        #endregion
    }
}
