using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface ITeamRepository
    {
        List<int> GetTeamManagerIds();
        List<int> GetTeamUserIds();
        List<int> GetTeamUserIds(int dealerId);
        Team GetTeam(int teamId);
        Team GetTeamByUser(int userId);
        string GetTeamNameByUser(int userId);
        IQueryable<Team> GetTeams();
        List<Team> GetTeams(int dealerId);
        void AddNewTeam(Team obj);        
        void DeleteTeam(int teamId);        
    }
}
