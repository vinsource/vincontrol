using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private VincontrolEntities _context;

        public UserRepository(VincontrolEntities context)
        {
            _context = context;
        }
        
        #region IUserRepository Members

        public User LogOn(string userName, string password)
        {

            var existingUser = _context.Users.FirstOrDefault(i => i.UserName.Equals(userName)
                                                                  && i.Password.Equals(password)
                                                                  && i.Active.HasValue &&
                                                                  i.Active.Value);
            if (existingUser == null) return null;

            var existingDealer = _context.Dealers.FirstOrDefault(i => i.DealerId == existingUser.DefaultLogin);

            if (existingDealer != null)
            {
                existingUser.DealerName = existingDealer.Name;
                existingUser.DealerAddress = existingDealer.Address;
                existingUser.DealerCity = existingDealer.City;
                existingUser.DealerState = existingDealer.State;
                existingUser.DealerZipCode = existingDealer.ZipCode;
                existingUser.DealerLatitude = existingDealer.Lattitude.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
                existingUser.DealerLongtitude = existingDealer.Longtitude.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
                existingUser.Setting = existingDealer.Setting;
                
            }
            return existingUser;

        }

        public User GetActiveUser(string userName, string password)
        {
            return  _context.Users.FirstOrDefault(i => i.UserName==userName
                                                                  && i.Password.Equals(password)
                                                                  && i.Active.HasValue &&
                                                                  i.Active.Value);

        }

        public User GetUser(int id)
        {
            var existingUser = _context.Users.FirstOrDefault(i => i.Active.HasValue && i.Active.Value && i.UserId == id);
            if (existingUser == null) return null;

            var existingDealer = _context.Dealers.FirstOrDefault(i => i.DealerId == existingUser.DefaultLogin);

            if (existingDealer != null)
            {
                existingUser.DealerName = existingDealer.Name;
                existingUser.DealerAddress = existingDealer.Address;
                existingUser.DealerCity = existingDealer.City;
                existingUser.DealerState = existingDealer.State;
                existingUser.DealerZipCode = existingDealer.ZipCode;
                existingUser.DealerLatitude = existingDealer.Lattitude.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
                existingUser.DealerLongtitude = existingDealer.Longtitude.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
                existingUser.Setting = existingDealer.Setting;

            }
            return existingUser;

        }

        public User InitializeWithDealerId(int dealerId)
        {
            var existingDealer = _context.Dealers.FirstOrDefault(i => i.DealerId == dealerId);
            return new User()
            {
                DealerGroupId = existingDealer.DealerGroupId,
                UserName = string.Empty,
                Name = existingDealer.Name,
                DealerName = existingDealer.Name,
                DealerAddress = existingDealer.Address,
                DealerCity = existingDealer.City,
                DealerState = existingDealer.State,
                DealerZipCode = existingDealer.ZipCode,
                DealerLatitude = existingDealer.Lattitude.GetValueOrDefault().ToString(),
                DealerLongtitude = existingDealer.Longtitude.GetValueOrDefault().ToString(),
                Setting = existingDealer.Setting
            };
        }

        public IQueryable<User> GetEmployees(int dealerId)
        {
            return _context.Users.Include("UserPermissions").Where(i => i.Active.HasValue && i.Active.Value && i.DefaultLogin == dealerId && i.UserPermissions.FirstOrDefault().RoleId == vincontrol.Constant.Constanst.RoleType.Employee);
        }

        public IQueryable<User> GetEmployees(int dealerId, List<int> excludedIds)
        {
            return _context.Users.Include("UserPermissions").Where(i => i.Active.HasValue && i.Active.Value && i.DefaultLogin == dealerId && i.UserPermissions.FirstOrDefault().RoleId == vincontrol.Constant.Constanst.RoleType.Employee && !excludedIds.Contains(i.UserId));
        }

        public IQueryable<User> GetManagers(int dealerId)
        {
            return _context.Users.Include("UserPermissions").Where(i => i.Active.HasValue && i.Active.Value && i.DefaultLogin == dealerId && i.UserPermissions.FirstOrDefault().RoleId == vincontrol.Constant.Constanst.RoleType.Manager);
        }

        public IQueryable<User> GetManagers(int dealerId, List<int> excludedIds)
        {
            return _context.Users.Include("UserPermissions").Where(i => i.Active.HasValue && i.Active.Value && i.DefaultLogin == dealerId && i.UserPermissions.FirstOrDefault().RoleId == vincontrol.Constant.Constanst.RoleType.Manager && !excludedIds.Contains(i.UserId));
        }

        public IList<User> GetAll()
        {
            return _context.Users.Include("UserPermissions").Where(i => i.Active.HasValue && i.Active.Value).ToList();
        }

        public IQueryable<User> GetAllByDealer(int dealerId)
        {
            return _context.Users.Include("UserPermissions").Where(i => i.Active.HasValue && i.Active.Value && i.DefaultLogin == dealerId);
        }

        public Dealer GetDealer(int dealerId)
        {
            return _context.Dealers.FirstOrDefault(i => i.DealerId == dealerId);
        }

        public NotificationSetting GetNotificationSetting(int dealerId)
        {
            return _context.NotificationSettings.FirstOrDefault(i => i.DealerId == dealerId);
        }

        public UserNotification GetUserNotification(int userId, int dealerId)
        {
            return _context.UserNotifications.FirstOrDefault(i => i.UserId == userId && i.DealerId == dealerId);
        }

        public IQueryable<UserPermission> GetUserPermissions(int userId)
        {
            return _context.UserPermissions.Where(x => x.UserId == userId);
            
        }

        public IQueryable<UserPermission> GetUserPermissionsByDealerId(int dealerId)
        {
            return _context.UserPermissions.Where(x => x.DealerId == dealerId && x.User.Active.Value && x.RoleId!=Constanst.RoleType.Master);
        }


        public List<vincontrol.DomainObject.UserNotification> GetUserNotifications(int dealerId)
        {
            var notifications =
                from users in _context.UserPermissions.Where(i => i.DealerId == dealerId && i.RoleId != Constanst.RoleType.Master && i.User.Active.Value)
                join n in _context.UserNotifications.Where(i => i.DealerId == dealerId) on users.UserId equals n.UserId into result
                from r in result.DefaultIfEmpty()
                //join ur in context.Users.Where(i => i.Active.Value)
                //    on users.UserId equals ur.UserId
                select new DomainObject.UserNotification
                {
                    Name = users.User.Name,
                    UserId = users.User.UserId,
                    Username = users.User.UserName,
                    Password = users.User.Password,
                    Email = users.User.Email,
                    Cellphone = users.User.CellPhone,
                    //et.RoleName,
                    RoleId = users.RoleId,
                    //userNotification.NotificationTypeId,
                    Active = users.User.Active ?? false,
                    AppraisalNotification = r != null && r.AppraisalNotified,
                    WholeSaleNotfication = r != null && r.WholesaleNotified,
                    InventoryNotfication = r != null && r.InventoryNotified,
                    TwentyFourHourNotification = r != null && r.C24hNotified,
                    NoteNotification = r != null && r.NoteNotified,
                    PriceChangeNotification = r != null && r.PriceChangeNotified,
                    AgeingBucketJumpNotification = r != null && r.AgingNotified,
                    BucketJumpReportNotification = r != null && r.BucketJumpNotified,
                    MarketPriceRangeChangeNotification = r != null && r.MarketPriceRangeNotified,
                    ImageUploadNotification = r != null && r.ImageUploadNotified,
                    GoodReviewNotification = r != null && r.GoodReviewNotified,
                    BadReviewNotification = r != null && r.BadReviewNotified,
                    GoodSurveyNotification = r != null && r.GoodSurveyNotified,
                    BadSurveyNotification = r != null && r.BadSurveyNotified,
                    AgingSurveyNotification = r != null && r.AgingSurveyNotified
                };
            return notifications.ToList();
        }

        public void UpdateUser(int userId, string photo)
        {
            var existingUser = GetUser(userId);
            if (existingUser != null)
            {
                existingUser.Photo = photo;
            }
        }

        public IQueryable<User> GetDealerEmployees(int dealerId)
        {
            return
                _context.Users.Include("UserPermissions").Include("Department")
                        .Where(
                            i =>
                            i.Active.HasValue && i.Active.Value && i.DefaultLogin == dealerId &&
                            !i.UserPermissions.FirstOrDefault()
                              .RoleId.Equals(vincontrol.Constant.Constanst.RoleType.Master))
                        .OrderByDescending(i => i.Expiration);
        }

        public IQueryable<User> GetUsersByTeam(int teamId)
        {
            return _context.Users.Include("UserPermissions").Where(i => i.TeamId == teamId);
        }

        public void AddUser(User obj)
        {
            _context.AddToUsers(obj);
        }

        public void AddUserPermission(UserPermission obj)
        {
            _context.AddToUserPermissions(obj);
        }

        public void DeleteUser(int id)
        {
            var existingUser = GetUser(id);
            if (existingUser != null)
            {
                existingUser.Active = false;
                existingUser.TeamId = null;
            }
        }

        public void DeletePermission(int id)
        {
            var permission = _context.UserPermissions.Where(i => i.UserId == id);
            if (permission.Any())
            {
                foreach (var item in permission)
                {
                    _context.DeleteObject(item);
                }
            }
         
        }

        public bool CheckExistingUsername(string username)
        {
            return _context.Users.Any(i => i.UserName.ToLower().Equals(username.ToLower()));
        }

        public bool CheckExistingUsername(string username, int userId)
        {
            return _context.Users.Any(i => i.UserName.ToLower().Equals(username.ToLower()) && i.UserId != userId /*&& i.Active.HasValue && i.Active.Value*/);
        }

        public void UpdatePass(int id, string pass)
        {
            var existingUser = GetUser(id);
            if (existingUser != null)
            {
                existingUser.Password = pass;
            }
        }

        public void UpdateEmail(int id, string email)
        {
            var existingUser = GetUser(id);
            if (existingUser != null)
            {
                existingUser.Email = email;
            }
        }

        public void UpdatePhone(int id, string phone)
        {
            var existingUser = GetUser(id);
            if (existingUser != null)
            {
                existingUser.CellPhone = phone;
            }
        }

        public void UpdateDefaultLogin(int id, int defaultLogin)
        {
            var existingUser = GetUser(id);
            if (existingUser != null)
            {
                existingUser.DefaultLogin = defaultLogin;
            }
        }

        public IQueryable<User> GetUsersByDealer(int dealerId)
        {
            return _context.Users.Where(i => i.Dealer.DealerId == dealerId);
        }

        public bool IsValidResetPasswordRequest(int userId, string forgotPasswordId, bool bDeleteRequest)
        {
            var id = new Guid(forgotPasswordId);

            var request = _context.ForgotPasswords.FirstOrDefault(x => x.UserId == userId && x.ForgotPasswordId == id);

            if (request != null)
            {
                DateTime sent = ((ForgotPassword)request).DateStamp;

                if (bDeleteRequest)
                {
                    _context.DeleteObject(request);

                }

                if ((DateTime.Now - sent).TotalDays < 1)
                {
                    return true;
                }
            }
            return false;
        }

        public Guid AddGetPasswordRequest(int userId)
        {
            Guid forgotPasswordId = Guid.NewGuid();


            var request = new ForgotPassword
            {
                ForgotPasswordId = forgotPasswordId,
                UserId = userId,
                DateStamp = DateTime.Now,
            };

            var existedRequest = _context.ForgotPasswords.FirstOrDefault(x => x.UserId == userId);

            if (existedRequest != null)
            {
                _context.DeleteObject(existedRequest);
            }

            _context.AddToForgotPasswords(request);

            return forgotPasswordId;
        }

        public IQueryable<Button> GetButtons(string screen)
        {
            return _context.Buttons.Where(i => i.Screen.ToLower().Equals(screen.ToLower()));
        }

        public bool CheckExistingActiveEmail(string email)
        {
            if (_context.Users.Any(o => o.Email== email&&o.Active.HasValue && o.Active.Value ))
            {
                return true;
            }

            return false;
        }

        public UserPermission GetUserPermission(int userId, int dealerId)
        {
            return _context.UserPermissions.FirstOrDefault(i => i.UserId==userId&&i.DealerId==dealerId);
        }

        public IQueryable<UserPermission> GetUserPermissionList(int userId)
        {
            return _context.UserPermissions.Where(i => i.UserId == userId);
        }

        public void ChangeRole(int roleId, int userId)
        {
            var searchResult = _context.UserPermissions.Where(x => x.UserId == userId);

            if (searchResult.Any())
            {
                foreach (var tmp in searchResult)
                {
                    tmp.RoleId = roleId;
                }

            }
        }

        public void UpdateNotification(bool notify, int dealerId, int notificationkind)
        {
            var setting = _context.NotificationSettings.FirstOrDefault(x => x.DealerId == dealerId);

            if (setting != null)
            {
                var usersetting = _context.UserNotifications.Where(x => x.DealerId == dealerId);
                switch (notificationkind)
                {
                    case 0:
                        setting.AppraisalNotified = notify;
                  

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.AppraisalNotified = notify;
                            }
                        }
                        break;
                    case 1:
                        setting.WholesaleNotified = notify;
                     

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.WholesaleNotified = notify;
                            }
                        }
                        break;
                    case 2:
                        setting.InventoryNotified = notify;
                        

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.InventoryNotified = notify;
                            }
                        }
                        break;
                    case 3:
                        setting.C24hNotified = notify;
                   

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.C24hNotified = notify;
                            }
                        }
                        break;
                    case 4:
                        setting.NoteNotified = notify;
                 

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.NoteNotified = notify;
                            }
                        }
                        break;
                    case 5:
                        setting.PriceChangeNotified = notify;
                   
                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.PriceChangeNotified = notify;
                            }
                        }
                        break;
                    case 6:
                        setting.AgingNotified = notify;
                      
                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.AgingNotified = notify;
                            }
                        }
                        break;
                    case 7:
                        setting.MarketPriceRangeNotified = notify;
                        

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.MarketPriceRangeNotified = notify;
                            }
                        }
                        break;
                    case 8:
                        setting.BucketJumpNotified = notify;
                   

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.BucketJumpNotified = notify;
                            }
                        }
                        break;
                    case 9:
                        setting.ImageUploadNotified = notify;
                       

                        if (notify == false)
                        {
                            foreach (var tmp in usersetting)
                            {
                                tmp.ImageUploadNotified = notify;
                            }
                        }
                        break;
                }
      
            }
        }

        public void UpdateNotificationPerUser(bool notify, int dealerId, int userId, int notificationkind)
        {
            var setting = _context.UserNotifications.FirstOrDefault(x => x.DealerId == dealerId && x.UserId == userId);
            if (setting == null)
            {
                setting = new UserNotification {UserId = userId, DealerId = dealerId, DateStamp = DateTime.Now};
                switch (notificationkind)
                {
                    case 0:
                        setting.AppraisalNotified = notify;
                        break;
                    case 1:
                        setting.WholesaleNotified = notify;
                        break;
                    case 2:
                        setting.InventoryNotified = notify;
                        break;
                    case 3:
                        setting.C24hNotified = notify;
                        break;
                    case 4:
                        setting.NoteNotified = notify;
                        break;
                    case 5:
                        setting.PriceChangeNotified = notify;
                        break;
                    case 6:
                        setting.AgingNotified = notify;
                        break;
                    case 7:
                        setting.MarketPriceRangeNotified = notify;
                        break;
                    case 8:
                        setting.BucketJumpNotified = notify;
                        break;
                    case 9:
                        setting.ImageUploadNotified = notify;
                        break;
                }
                _context.AddToUserNotifications(setting);
            }
            else
            {
                switch (notificationkind)
                {
                    case 0:
                        setting.AppraisalNotified = notify;
                        break;
                    case 1:
                        setting.WholesaleNotified = notify;
                        break;
                    case 2:
                        setting.InventoryNotified = notify;
                        break;
                    case 3:
                        setting.C24hNotified = notify;
                        break;
                    case 4:
                        setting.NoteNotified = notify;
                        break;
                    case 5:
                        setting.PriceChangeNotified = notify;
                        break;
                    case 6:
                        setting.AgingNotified = notify;
                        break;
                    case 7:
                        setting.MarketPriceRangeNotified = notify;
                        break;
                    case 8:
                        setting.BucketJumpNotified = notify;
                        break;
                    case 9:
                        setting.ImageUploadNotified = notify;
                        break;
                }
            }
        }

        public void UpdateDefaultLogin(string username, int defaultLogin)
        {
            var searchResult = _context.Users.FirstOrDefault(x => x.UserName == username);

            if (searchResult != null)
            {
                searchResult.DefaultLogin = defaultLogin;

                
            }
        }

        #endregion
    }
}
