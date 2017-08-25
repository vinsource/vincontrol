using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinchat.Interface
{
    public interface ICommonRepository
    {
        user GetUser(string username);
        user GetUser(string username, string password);
        IQueryable<rosteruser> GetRosterUsers(string username);
        void AddNewUser(user obj);
        void DeleteUser(user obj);
        void AddNewRosterUser(rosteruser obj);
        void DeleteRosterUser(rosteruser obj);
        void UpdateUserPassword(string username, string newPassword);
    }
}
