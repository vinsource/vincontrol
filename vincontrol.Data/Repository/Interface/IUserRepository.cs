using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IUserRepository
    {
        User LogOn(string userName, string password);
        User GetActiveUser(string userName, string password);
        User InitializeWithDealerId(int dealerId);
        IList<User> GetAll();
        IQueryable<User> GetAllByDealer(int dealerId);
        IQueryable<User> GetEmployees(int dealerId);
        IQueryable<User> GetEmployees(int dealerId, List<int> excludedIds);
        User GetUser(int id);
        Dealer GetDealer(int dealerId);
        NotificationSetting GetNotificationSetting(int dealerId);
        UserNotification GetUserNotification(int userId, int dealerId);
        IQueryable<UserPermission> GetUserPermissions(int userId);
        IQueryable<UserPermission> GetUserPermissionsByDealerId(int dealerId);
        void UpdateUser(int userId, string photo);
        IQueryable<User> GetManagers(int dealerId);
        IQueryable<User> GetManagers(int dealerId, List<int> excludedIds);
        IQueryable<User> GetDealerEmployees(int dealerId);
        IQueryable<User> GetUsersByTeam(int teamId);
        List<DomainObject.UserNotification> GetUserNotifications(int dealerId);
        void AddUser(User obj);
        void AddUserPermission(UserPermission obj);
        void DeleteUser(int id);
        void DeletePermission(int id);
        bool CheckExistingUsername(string username);
        bool CheckExistingUsername(string username, int userId);
        void UpdatePass(int id, string pass);
        void UpdateEmail(int id, string email);
        void UpdatePhone(int id, string phone);
        void UpdateDefaultLogin(int id, int defaultLogin);
        IQueryable<User> GetUsersByDealer(int dealerId);
        bool IsValidResetPasswordRequest(int userId, string forgotPasswordId, bool bDeleteRequest);
        Guid AddGetPasswordRequest(int userId);
        IQueryable<Button> GetButtons(string screen);
        bool CheckExistingActiveEmail(string email);
        UserPermission GetUserPermission(int userId, int dealerId);
        IQueryable<UserPermission> GetUserPermissionList(int userId);
        void ChangeRole(int roleId, int userId);
        void UpdateNotification(bool notify, int dealerId, int notificationkind);
        void UpdateNotificationPerUser(bool notify, int dealerId, int userId, int notificationkind);
        void UpdateDefaultLogin(string username, int defaultLogin);
    }

}
