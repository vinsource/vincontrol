using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class TeamRepository : ITeamRepository
    {
        private VincontrolEntities _context;

        public TeamRepository(VincontrolEntities context)
        {
            _context = context;
        }

        #region ITeamRepository' Members

        public List<int> GetTeamManagerIds()
        {
            return _context.Teams.Where(x => x.ManagerId != null).Select(i => i.ManagerId).ToList();
        }

        public List<int> GetTeamUserIds()
        {
            return _context.Users.Where(i => i.TeamId.HasValue).Select(i => i.UserId).ToList();
        }

        public List<int> GetTeamUserIds(int dealerId)
        {
            return _context.Users.Where(i => i.TeamId.HasValue && i.DefaultLogin == dealerId).Select(i => i.UserId).ToList();
        }

        public Team GetTeam(int teamId)
        {
            return _context.Teams.Include("Users").FirstOrDefault(i => i.TeamId == teamId);
        }

        public Team GetTeamByUser(int userId)
        {
            //if user is Manager
            var team = _context.Teams.Include("Users").FirstOrDefault(i => i.ManagerId == userId);

            return team ?? _context.Teams.Where(i => i.User.UserId== userId).Select(i => i).FirstOrDefault();
        }

        public IQueryable<Team> GetTeams()
        {
            return _context.Teams.Include("Users");
        }

        public List<Team> GetTeams(int dealerId)
        {
            return _context.Teams.Include("Users").Where(x => x.DealerId == dealerId).ToList();
            //return _context.Teams.Include("Users").ToList().Join(_context.Users.Where(u => u.DefaultLogin == dealerId && u.Active.HasValue && u.Active.Value).ToList(),
            //    t => t.ManagerId,
            //    u => u.UserId,
            //    (t, u) => t).Select(t => t).ToList();               
        }
        
        public string GetTeamNameByUser(int userId)
        {
            var item = GetTeamByUser(userId);
            return item != null ? item.Name : string.Empty;
        }
        
        public void AddNewTeam(Team obj)
        {
            _context.AddToTeams(obj);
        }

        public void DeleteTeam(int teamId)
        {
            var existingTeam = GetTeam(teamId);
            if (existingTeam != null)
            {
                _context.DeleteObject(existingTeam);
            }
        }

        #endregion
    }
}
