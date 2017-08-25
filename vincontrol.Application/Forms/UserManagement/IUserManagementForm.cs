using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.UserManagement
{
    public interface IUserManagementForm
    {
        User GetUserById(int userId);
    }
}
