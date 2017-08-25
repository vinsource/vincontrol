using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.Vinchat.Forms.CommonManagement;
using vincontrol.Application.Vinsocial.Forms.TeamManagement;
using vincontrol.Constant;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using Button = vincontrol.Application.ViewModels.CommonManagement.Button;

namespace vincontrol.Application.Forms.AccountManagement
{
    public class AccountManagementForm : BaseForm, IAccountManagementForm
    {
        private ICommonManagementForm _commonManagementForm;
        private IDealerManagementForm _dealerManagementForm;

        #region Constructors
        public AccountManagementForm() : this(new SqlUnitOfWork())
        {
            _commonManagementForm = new CommonManagementForm();
            _dealerManagementForm=new DealerManagementForm();
        }

        public AccountManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IAccountManagementForm Members

        public UserViewModel LogOn(string userName, string password)
        {
            var existingUser = UnitOfWork.User.LogOn(userName, password);
            if (existingUser == null) return null;

            var dealer = UnitOfWork.User.GetDealer(existingUser.DefaultLogin.GetValueOrDefault());
            var user = new UserViewModel(existingUser)
            {
                DealerName = dealer.Name,
                DealerLatitude = dealer.Lattitude.GetValueOrDefault().ToString(),
                DealerLongtitude = dealer.Longtitude.GetValueOrDefault().ToString(),
                Email = dealer.Email,
                DealerAddress = dealer.Address,
                DealerCity = dealer.City,
                DealerState = dealer.State,
                DealerZipCode = dealer.ZipCode
            };

            return user;
        }

        public UserViewModel GetUser(int userId)
        {
            var existingUser = UnitOfWork.User.GetUser(userId);
            if (existingUser == null) return new UserViewModel();

            var dealer = UnitOfWork.User.GetDealer(existingUser.DefaultLogin.GetValueOrDefault());
            var user = new UserViewModel(existingUser)
            {
                DealerName = dealer.Name,
                DealerLatitude = dealer.Lattitude.GetValueOrDefault().ToString(),
                DealerLongtitude = dealer.Longtitude.GetValueOrDefault().ToString(),
                DealerEmail = dealer.Email,
                DealerAddress = dealer.Address,
                DealerCity = dealer.City,
                DealerState = dealer.State,
                DealerZipCode = dealer.ZipCode
            };

            return user;
        }

        public UserViewModel InitializeWithDealerId(int dealerId)
        {
            var existingUser = UnitOfWork.User.InitializeWithDealerId(dealerId);
            return existingUser != null ? new UserViewModel(existingUser) : null;
        }

        public List<UserViewModel> GetEmployees(int dealerId)
        {
            var users = UnitOfWork.User.GetEmployees(dealerId);
            return users.Any()
                       ? users.AsEnumerable().Select(i => new UserViewModel(i)).ToList()
                       : new List<UserViewModel>();
        }

        public List<UserViewModel> GetTeamEmployees(int dealerId)
        {
            var users = UnitOfWork.User.GetEmployees(dealerId, UnitOfWork.Team.GetTeamUserIds(dealerId));
            return users.Any()
                       ? users.AsEnumerable().Select(i => new UserViewModel(i)).ToList()
                       : new List<UserViewModel>();
        }

        public DealerViewModel GetDealer(int dealerId)
        {
            var dealer = UnitOfWork.User.GetDealer(dealerId);
            return dealer != null ? new DealerViewModel(dealer) : new DealerViewModel();
        }
        
    

        public void UpdateUser(int userId, string photo)
        {
            UnitOfWork.User.UpdateUser(userId, photo);
            UnitOfWork.CommitVincontrolModel();
        }

        public List<UserViewModel> GetManagers(int dealerId)
        {
            var users = UnitOfWork.User.GetManagers(dealerId);
            return users.Any()
                       ? users.AsEnumerable().Select(i => new UserViewModel(i)).ToList()
                       : new List<UserViewModel>();
        }

        public List<UserViewModel> GetTeamManagers(int dealerId)
        {
            var users = UnitOfWork.User.GetManagers(dealerId, UnitOfWork.Team.GetTeamManagerIds());
            return users.Any()
                       ? users.AsEnumerable().Select(i => new UserViewModel(i)).ToList()
                       : new List<UserViewModel>();
        }

        public List<UserViewModel> GetUsers(int dealerId)
        {
            var list = UnitOfWork.User.GetDealerEmployees(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new UserViewModel(i)).ToList() : new List<UserViewModel>();
        }

        public List<int> GetUserIds(int dealerId)
        {
            var list = UnitOfWork.User.GetDealerEmployees(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => i.UserId).ToList() : new List<int>();
        }

        public List<UserNotificationViewModel> GetUserNotifications(int dealerId)
        {
            var notifications = UnitOfWork.User.GetUserNotifications(dealerId);
            return notifications.Any()
                       ? notifications.Select(i => new UserNotificationViewModel(i)).ToList()
                       : new List<UserNotificationViewModel>();
        }

        public void UpdateNotificationSetting(int dealerId, int notificationType, bool status)
        {
            var setting = UnitOfWork.User.GetNotificationSetting(dealerId);
            var users = UnitOfWork.User.GetEmployees(dealerId);
            switch (notificationType)
            {
                //case Constant.Constanst.NotificationType.GoodReview:
                //    setting.GoodReviewNotified = status; break;
                //case Constant.Constanst.NotificationType.BadReview:
                //    setting.BadReviewNotified = status; break;
                //case Constant.Constanst.NotificationType.GoodSurvey:
                //    setting.GoodSurveyNotified = status; break;
                //case Constant.Constanst.NotificationType.BadSurvey:
                //    setting.BadSurveyNotified = status; break;
                //case Constant.Constanst.NotificationType.AgingSurvey:
                //    setting.AgingSurveyNotified = status; break;
                //default: break;
            }

            if (!status)
                UpdateUserNotifications(users.Select(i => i.UserId).ToList(), dealerId, notificationType, status);
            
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateUserNotifications(List<int> userIds, int dealerId, int notificationType, bool status)
        {
            foreach (var id in userIds)
            {
                UpdateUserNotification(id, dealerId, notificationType, status);
            }
        }

        public bool IsValidResetPasswordRequest(int userId, string forgotPasswordId, bool bDeleteRequest)
        {
            var flag = UnitOfWork.User.IsValidResetPasswordRequest(userId, forgotPasswordId, bDeleteRequest);
            if (flag)
                UnitOfWork.CommitVincontrolModel();
            return flag;
        }

        public Guid AddGetPasswordRequest(int userId)
        {
            var guid=UnitOfWork.User.AddGetPasswordRequest(userId);
            UnitOfWork.CommitVincontrolModel();
            return guid;
        }

        public UserRoleViewModel CheckUserExistWithStatus(string userName, string passWord)
        {
            var user = new UserRoleViewModel();

            var result = UnitOfWork.User.GetActiveUser(userName, passWord);
                
            if (result != null)
            {

                var associateRoles = UnitOfWork.User.GetUserPermissions(result.UserId);

                var accessDealerPermissions = associateRoles.Select(i => new RoleDealerAccess
                {
                    DealerId = i.DealerId,
                    RoleId = i.RoleId

                }).ToList();

                if (associateRoles.Any())
                {
                    if (associateRoles.Count() == 1 &&
                        associateRoles.First().RoleId == Constanst.RoleType.Master)
                    {


                        user = new UserRoleViewModel
                        {
                            UserId = result.UserId,
                            Username = result.UserName,
                            Password = result.Password,
                            MasterLogin = true,
                            Name = result.DealerGroup.DealerGroupName,
                            FullName = result.Name,
                            Cellphone = result.CellPhone,
                            MultipleDealerLogin = true,
                            DealershipId = result.DefaultLogin.GetValueOrDefault(),
                            DefaultLogin = result.DefaultLogin.GetValueOrDefault(),
                            DealerGroupId = result.DealerGroupId,
                            RoleId = Constanst.RoleType.Master,
                            Role = Constanst.RoleTypeText.Master,
                            AccessDealerPermissions = accessDealerPermissions

                        };
                    }
                    else
                    {
                        user = new UserRoleViewModel
                        {
                            UserId = result.UserId,
                            Username = result.UserName,
                            Password = result.Password,
                            Name = associateRoles.First().Dealer.Name,
                            FullName = result.Name,
                            Cellphone = result.CellPhone,
                            DealershipId = result.DefaultLogin.GetValueOrDefault(),
                            DefaultLogin = result.DefaultLogin.GetValueOrDefault(),
                            DealerGroupId = result.DealerGroupId,
                            RoleId = associateRoles.First().RoleId,
                            AccessDealerPermissions = accessDealerPermissions

                        };

                        if (user.RoleId == Constanst.RoleType.Admin)
                            user.Role = Constanst.RoleTypeText.Admin;
                        else if (user.RoleId == Constanst.RoleType.Manager)
                        {
                            user.Role = Constanst.RoleTypeText.Manager;

                            user.ProfileButtonPermissions = GetButtonList(user, "Profile");

                        }
                        else if (user.RoleId == Constanst.RoleType.Employee)
                        {
                            user.Role = Constanst.RoleTypeText.Employee;
                            user.ProfileButtonPermissions = GetButtonList(user, "Profile");
                        }

                        if (associateRoles.Count() > 1)
                            user.MultipleDealerLogin = true;
                    }
                }

            }



            return user;
        }

        public ButtonPermissionViewModel GetButtonList(UserRoleViewModel user, string screen)
        {
            var result = new ButtonPermissionViewModel();

            var buttons = UnitOfWork.User.GetButtons(screen).ToList();

            if (buttons.Any())
            {

                var buttonModels = buttons.Select(i => new Button()
                {
                    ButtonId = i.ButtonId,
                    ButtonName = i.Button1,
                    CanSee = i.DealerButtons != null &&
                        i.DealerButtons.Any(ii => ii.DealerId == user.DealershipId && ii.ButtonId == i.ButtonId && ii.GroupId == user.RoleId)
                            ? i.DealerButtons.First(
                                ii =>
                                ii.DealerId == user.DealershipId && ii.ButtonId == i.ButtonId && ii.GroupId == user.RoleId).CanSee
                            : false
                }).ToList();


                result = new ButtonPermissionViewModel()
                {
                    Buttons = buttonModels,
                    DealershipId = user.DealershipId,
                    GroupId = user.RoleId,
                    GroupName = user.Role
                };

            }


            return result;
        }

        public DealerLoginResult MasterLogin(UserRoleViewModel searchuser)
        {
            DealerGroupViewModel dealerGroup = null;
           
            dealerGroup = new DealerGroupViewModel()
            {
                DealershipGroupId = searchuser.DealerGroupId,

                DealershipGroupName = searchuser.Name,

                DealershipGroupDefaultLogin = searchuser.DefaultLogin
            };

            var defaultDealer = _dealerManagementForm.GetDealerById(searchuser.DealershipId);
            
            dealerGroup.DealerList = new List<DealershipViewModel>();

            var dealerList = _dealerManagementForm.GetDealers(searchuser.DealerGroupId);

            foreach (var row in dealerList)
            {
                var tmp = new DealershipViewModel(row);
                dealerGroup.DealerList.Add(tmp);
            }


            return new DealerLoginResult() {Dealer = defaultDealer, DealerGroup = dealerGroup};
        }

        public DealerLoginResult LoginMultipleStore(UserRoleViewModel searchuser)
        {
            var dealerGroup = new DealerGroupViewModel {DealerList = new List<DealershipViewModel>()};

            var dealer=new DealershipViewModel();

            var accessibleDealerList =
                _dealerManagementForm.GetDealers(searchuser.AccessDealerPermissions.Select(x => x.DealerId));

            foreach (var row in accessibleDealerList)
            {   
                var tmp = new DealershipViewModel(row);
                dealerGroup.DealerList.Add(tmp);
            }

            if (searchuser.DefaultLogin > 0)
            {
                var defaultDealer = accessibleDealerList.FirstOrDefault(x => x.DealerId == searchuser.DefaultLogin);
                dealerGroup.DealershipGroupId = defaultDealer.DealerGroupId;
                dealerGroup.DealershipGroupName = defaultDealer.DealerGroup.DealerGroupName;
                dealerGroup.DealershipGroupDefaultLogin = defaultDealer.DealerGroup.DefaultDealerId;
                dealer = new DealershipViewModel(defaultDealer)
                {
                    DealershipId = searchuser.DefaultLogin
                };
            }
            else
            {
                var defaultDealer = accessibleDealerList.First();
                dealerGroup.DealershipGroupId = defaultDealer.DealerGroupId;
                dealerGroup.DealershipGroupName = defaultDealer.DealerGroup.DealerGroupName;
                dealerGroup.DealershipGroupDefaultLogin = defaultDealer.DealerGroup.DefaultDealerId;
                dealer = new DealershipViewModel(defaultDealer)
                {
                    DealershipId = searchuser.DefaultLogin
                };
            }

           
            return new DealerLoginResult {Dealer = dealer, DealerGroup = dealerGroup};

        }

        public DealerLoginResult LoginSingleStore(UserRoleViewModel searchuser)
        {
            DealershipViewModel dealer = null;

            var defaultDealer = _dealerManagementForm.GetSpecificDealer(searchuser.DealershipId);

            if (defaultDealer != null)
            {
                dealer = new DealershipViewModel(defaultDealer);
            }

            return new DealerLoginResult() {Dealer = dealer};
        }

        public IQueryable<UserRoleViewModel> GetUserList(int dealerId)
        {
            var userFilter =
                UnitOfWork.User.GetUserPermissionsByDealerId(dealerId).Select(x => new UserRoleViewModel()
                {
                    Name = x.User.Name,
                    UserId = x.User.UserId,
                    Username = x.User.UserName,
                    Password = x.User.Password,
                    Email = x.User.Email,
                    Cellphone = x.User.CellPhone,
                    RoleId = x.RoleId,
                    Active = x.User.Active ?? false,
                    AppraisalNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().AppraisalNotified,
                    WholeSaleNotfication = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().WholesaleNotified,
                    InventoryNotfication = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().InventoryNotified,
                    TwentyFourHourNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().C24hNotified,
                    NoteNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().NoteNotified,
                    PriceChangeNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().PriceChangeNotified,
                    AgeingBucketJumpNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().AgingNotified,
                    BucketJumpReportNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().BucketJumpNotified,
                    MarketPriceRangeChangeNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().MarketPriceRangeNotified,
                    ImageUploadNotification = x.User.UserNotifications.FirstOrDefault() != null && x.User.UserNotifications.FirstOrDefault().ImageUploadNotified,
                });

            return userFilter;
        }

        public User GetUserById(int userId)
        {
            return UnitOfWork.User.GetUser(userId);
        }

        public UserPermission GetUserPermission(int userId, int dealerId)
        {
            return UnitOfWork.User.GetUserPermission(userId, dealerId);
        }

        public void ChangeRole(int roleId, int userId)
        {
            UnitOfWork.User.ChangeRole(roleId,userId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateNotification(bool notify, int dealerId, int notificationkind)
        {
            UnitOfWork.User.UpdateNotification(notify, dealerId, notificationkind);
            UnitOfWork.CommitVincontrolModel();
        }

        public IQueryable<UserPermission> GetUserPermissionList(int userId)
        {
            return UnitOfWork.User.GetUserPermissionList(userId);
        }

        public void UpdateNotificationPerUser(bool notify, int dealerId, int userId, int notificationkind)
        {
            UnitOfWork.User.UpdateNotificationPerUser(notify, dealerId,userId, notificationkind);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateDefaultLogin(string username, int defaultLogin)
        {
            UnitOfWork.User.UpdateDefaultLogin(username,defaultLogin);
            UnitOfWork.CommitVincontrolModel();
        }


        public void UpdateUserNotification(int userId, int dealerId, int notificationType, bool status)
        {
            var notification = UnitOfWork.User.GetUserNotification(userId, dealerId);
            if (notification == null) return;

            switch (notificationType)
            {
                //case Constant.Constanst.NotificationType.GoodReview:
                //    notification.GoodReviewNotified = status; break;
                //case Constant.Constanst.NotificationType.BadReview:
                //    notification.BadReviewNotified = status; break;
                //case Constant.Constanst.NotificationType.GoodSurvey:
                //    notification.GoodSurveyNotified = status; break;
                //case Constant.Constanst.NotificationType.BadSurvey:
                //    notification.BadSurveyNotified = status; break;
                //case Constant.Constanst.NotificationType.AgingSurvey:
                //    notification.AgingSurveyNotified = status; break;
                default: break;
            }
            UnitOfWork.CommitVincontrolModel();
        }

        public void AddUser(UserViewModel obj)
        {
            var user = MappingHandler.ConvertViewModelToUser(obj);
            UnitOfWork.User.AddUser(user);
            UnitOfWork.CommitVincontrolModel();

            obj.Id = user.UserId;
            AddUserPermission(obj.DealerId, user.UserId, obj.RoleId);
        }

        public void AddUser(UserViewModel obj, string[] dealerList)
        {
            var user = MappingHandler.ConvertViewModelToUser(obj);
            UnitOfWork.User.AddUser(user);
            UnitOfWork.CommitVincontrolModel();
            obj.Id = user.UserId;
            foreach (var tmp in dealerList)
            {
                AddUserPermission(Convert.ToInt32(tmp), user.UserId, obj.RoleId);

            }
            UnitOfWork.CommitVincontrolModel();

        }

        public void ChangeUserPermission(UserRoleViewModel obj, string[] dealerList)
        {

            UnitOfWork.User.UpdateDefaultLogin(obj.UserId, obj.DefaultLogin);
            UnitOfWork.User.DeletePermission(obj.UserId);

            foreach (var tmp in dealerList)
            {
                AddUserPermission(Convert.ToInt32(tmp), obj.UserId, obj.RoleId);

            }
            UnitOfWork.CommitVincontrolModel();
        }

        public void AddUserPermission(int dealerId, int userId, int roleId)
        {
            UnitOfWork.User.AddUserPermission(new UserPermission(){ DealerId = dealerId, UserId = userId, RoleId = roleId });
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdatePass(int id, string pass)
        {
            UnitOfWork.User.UpdatePass(id, pass);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdateEmail(int id, string email)
        {
            UnitOfWork.User.UpdateEmail(id, email);
            UnitOfWork.CommitVincontrolModel();
        }

        public void UpdatePhone(int id, string phone)
        {
            UnitOfWork.User.UpdatePhone(id, phone);
            UnitOfWork.CommitVincontrolModel();
        }


        public void UpdateUser(UserViewModel obj)
        {
            var existingUser = UnitOfWork.User.GetUser(obj.Id);
            if (existingUser != null)
            {
                existingUser.Name = obj.Name;
                existingUser.UserName = obj.UserName;
                existingUser.Password = obj.Password;
                existingUser.Email = obj.Email;
                existingUser.CellPhone = obj.Phone;
                existingUser.Description = obj.Description;
                existingUser.Photo = obj.Photo;
                foreach (var permission in existingUser.UserPermissions)
                {
                    permission.RoleId = obj.RoleId;
                }
                existingUser.DepartmentId = obj.DepartmentId;
                if (!obj.TeamId.Equals(0) && obj.RoleId.Equals(Constant.Constanst.RoleType.Employee)) existingUser.TeamId = obj.TeamId;                

                // Update manager's team
                if (obj.RoleId.Equals(Constant.Constanst.RoleType.Manager))
                {
                    var assignedTeam = UnitOfWork.Team.GetTeam(obj.TeamId);
                    if (assignedTeam != null)
                    {
                        var assignedTeamManager = UnitOfWork.User.GetUser(assignedTeam.ManagerId);
                        var currentTeam = UnitOfWork.Team.GetTeamByUser(existingUser.UserId);

                        if (assignedTeamManager != null)
                            assignedTeamManager.TeamId = null;

                        existingUser.TeamId = obj.TeamId;
                        assignedTeam.ManagerId = existingUser.UserId;

                        if (currentTeam != null)
                            currentTeam.ManagerId = existingUser.UserId;
                    }
                }

                UnitOfWork.CommitVincontrolModel();

                try
                {
                    _commonManagementForm.UpdateUserPassword(obj.UserName, obj.Password);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public void DeleteUser(int id)
        {
            UnitOfWork.User.DeleteUser(id);
            UnitOfWork.CommitVincontrolModel();
        }

        public bool CheckExistingUsername(string username)
        {
            return UnitOfWork.User.CheckExistingUsername(username);
        }

        public bool CheckExistingActiveEmail(string email)
        {
            return UnitOfWork.User.CheckExistingActiveEmail(email);
        }

        public bool CheckExistingUsername(string username, int userId)
        {
            return UnitOfWork.User.CheckExistingUsername(username, userId);
        }

      
        #endregion
    }
}
