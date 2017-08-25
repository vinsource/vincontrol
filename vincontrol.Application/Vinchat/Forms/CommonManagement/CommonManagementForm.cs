using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Vinchat.Forms.CommonManagement
{
    public class CommonManagementForm : BaseForm, ICommonManagementForm
    {
        #region Constructors
        public CommonManagementForm() : this(new SqlUnitOfWork()) { }

        public CommonManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ICommonManagementForm' Members

        public void AddNewUser(string username, string password)
        {
            var newUser = new user()
            {
                username = username,
                password = password,
                created = DateTime.Now
            };

            UnitOfWork.VinchatCommon.AddNewUser(newUser);
            UnitOfWork.CommitVinchatModel();
        }

        public void DeleteUser(string username, string password)
        {
            var existingUser = UnitOfWork.VinchatCommon.GetUser(username, password);
            if (existingUser != null)
            {
                UnitOfWork.VinchatCommon.DeleteUser(existingUser);
                UnitOfWork.CommitVinchatModel();
            }
        }

        public void AddNewRosterUser(string username, string jid)
        {
            var newRosterUser = new rosteruser()
            {
                username = username,
                jid = jid,
                nick = string.Empty,
                subscription = "B",
                ask = "N",
                askmessage = string.Empty,
                server = "N",
                subscribe = string.Empty,
                type = "item"
            };

            UnitOfWork.VinchatCommon.AddNewRosterUser(newRosterUser);
            UnitOfWork.CommitVinchatModel();
        }

        public void DeleteRosterUsers(string username)
        {
            var rosterUsers = UnitOfWork.VinchatCommon.GetRosterUsers(username);
            if (rosterUsers.Any())
            {
                foreach (var rosteruser in rosterUsers)
                {
                    UnitOfWork.VinchatCommon.DeleteRosterUser(rosteruser);
                }
                UnitOfWork.CommitVinchatModel();
            }
        }

        public void DeleteRosterUsers(string username, int dealerId, string suffix)
        {
            var rosterUsers = UnitOfWork.VinchatCommon.GetRosterUsers(username);
            var user = UnitOfWork.User.GetUsersByDealer(dealerId).ToList();
            if (rosterUsers.Any())
            {
                foreach (var rosteruser in rosterUsers)
                {
                    if(user.Any(i=>i.UserName.Equals(rosteruser.jid.Replace(suffix,""))))
                    UnitOfWork.VinchatCommon.DeleteRosterUser(rosteruser);
                }
                UnitOfWork.CommitVinchatModel();
            }
        }


        public void UpdateUserPassword(string username, string newPassword)
        {
            UnitOfWork.VinchatCommon.UpdateUserPassword(username, newPassword);
            UnitOfWork.CommitVinchatModel();
        }

        #endregion
    }
}
