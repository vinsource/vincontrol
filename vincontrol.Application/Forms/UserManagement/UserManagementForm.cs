using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.UserManagement
{
    public class UserManagementForm: BaseForm, IUserManagementForm
    {
          public UserManagementForm() : this(new SqlUnitOfWork()) { /*_carfaxService = new CarFaxService();*/ }

          public UserManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public User GetUserById(int userId)
        {
            return UnitOfWork.User.GetUser(userId);
        }
    }
}
