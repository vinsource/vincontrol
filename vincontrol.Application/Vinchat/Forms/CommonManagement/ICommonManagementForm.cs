using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.Vinchat.Forms.CommonManagement
{
    public interface ICommonManagementForm
    {
        void AddNewUser(string username, string password);
        void DeleteUser(string username, string password);
        void AddNewRosterUser(string username, string jid);
        void DeleteRosterUsers(string username);
        void UpdateUserPassword(string username, string newPassword);
    }
}
