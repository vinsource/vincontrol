using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.AccountManagement
{
    public interface IAccountManagementForm
    {
        UserViewModel LogOn(string userName, string password);
        UserViewModel InitializeWithDealerId(int dealerId);
        List<UserViewModel> GetEmployees(int dealerId);
        List<UserViewModel> GetTeamEmployees(int dealerId);
        DealerViewModel GetDealer(int dealerId);
        UserViewModel GetUser(int userId);
        void UpdateUser(int userId, string photo);
        List<UserViewModel> GetManagers(int dealerId);
        List<UserViewModel> GetTeamManagers(int dealerId);
        List<UserViewModel> GetUsers(int dealerId);
        List<int> GetUserIds(int dealerId);
        List<UserNotificationViewModel> GetUserNotifications(int dealerId);
        void AddUser(UserViewModel obj);
        void AddUser(UserViewModel obj, string[] dealerList);
        void ChangeUserPermission(UserRoleViewModel obj, string[] dealerList);
        void AddUserPermission(int dealerId, int userId, int roleId);
        void UpdatePass(int id,string pass);
        void UpdateEmail(int id, string email);
        void UpdatePhone(int id, string phone);
        void DeleteUser(int id);
        void UpdateUser(UserViewModel obj);
        bool CheckExistingUsername(string username);
        bool CheckExistingActiveEmail(string email);
       
        bool CheckExistingUsername(string username, int userId);
        void UpdateNotificationSetting(int dealerId, int notificationType, bool status);
        void UpdateUserNotification(int userId, int dealerId, int notificationType, bool status);
        void UpdateUserNotifications(List<int> userIds, int dealerId, int notificationType, bool status);
        bool IsValidResetPasswordRequest(int userId, string forgotPasswordId, bool bDeleteRequest);
        Guid AddGetPasswordRequest(int userId);
        UserRoleViewModel CheckUserExistWithStatus(string userName, string passWord);
        ButtonPermissionViewModel GetButtonList(UserRoleViewModel user, string screen);
        DealerLoginResult MasterLogin(UserRoleViewModel searchuser);
        DealerLoginResult LoginMultipleStore(UserRoleViewModel searchuser);
        DealerLoginResult LoginSingleStore(UserRoleViewModel searchuser);
        IQueryable<UserRoleViewModel> GetUserList(int dealerId);
        User GetUserById(int userId);
        UserPermission GetUserPermission(int userId, int dealerId);
        void ChangeRole(int roleId, int userId);
        void UpdateNotification(bool notify, int dealerId, int notificationkind);
        IQueryable<UserPermission> GetUserPermissionList(int userId);
        void UpdateNotificationPerUser(bool notify, int dealerId, int userId, int notificationkind);
        void UpdateDefaultLogin(string username, int defaultLogin);
    }
}
