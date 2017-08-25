using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.AccountManagement
{
    public class AdminNotificationViewModel
    {
        public AdminNotificationViewModel(){}
        public NotificationSettingViewModel NotificationSetting { get; set; }
        public List<UserNotificationViewModel> UserNotifications { get; set; }
    }

    public class UserNotificationViewModel
    {
        public UserNotificationViewModel() { }

        public UserNotificationViewModel(vincontrol.DomainObject.UserNotification obj)
        {
            DealershipId = obj.DealershipId;
            Name = obj.Name;
            FullName = obj.FullName;
            UserId = obj.UserId;
            Username = obj.Username;
            Password = obj.Password;
            Email = obj.Email;
            Cellphone = obj.Cellphone;
            Role = obj.Role;
            RoleId = obj.RoleId;
            Active = obj.Active;
            AppraisalNotification = obj.AppraisalNotification;
            WholeSaleNotfication = obj.WholeSaleNotfication;
            InventoryNotfication = obj.InventoryNotfication;
            TwentyFourHourNotification = obj.TwentyFourHourNotification;
            NoteNotification = obj.NoteNotification;
            PriceChangeNotification = obj.PriceChangeNotification;
            MarketPriceRangeChangeNotification = obj.MarketPriceRangeChangeNotification;
            AgeingBucketJumpNotification = obj.AgeingBucketJumpNotification;
            BucketJumpReportNotification = obj.BucketJumpReportNotification;
            ImageUploadNotification = obj.ImageUploadNotification;
            GoodReviewNotification = obj.GoodReviewNotification;
            BadReviewNotification = obj.BadReviewNotification;
            GoodSurveyNotification = obj.GoodSurveyNotification;
            BadSurveyNotification = obj.BadSurveyNotification;
            AgingSurveyNotification = obj.AgingSurveyNotification;
            DealerGroupId = obj.DealerGroupId;
        }

        public int DealershipId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Cellphone { get; set; }

        public string Role { get; set; }

        public int RoleId { get; set; }

        public bool Active { get; set; }

        public bool AppraisalNotification { get; set; }

        public bool WholeSaleNotfication { get; set; }

        public bool InventoryNotfication { get; set; }

        public bool TwentyFourHourNotification { get; set; }

        public bool NoteNotification { get; set; }

        public bool PriceChangeNotification { get; set; }

        public bool MarketPriceRangeChangeNotification { get; set; }

        public bool AgeingBucketJumpNotification { get; set; }

        public bool BucketJumpReportNotification { get; set; }

        public bool ImageUploadNotification { get; set; }

        public bool GoodReviewNotification { get; set; }

        public bool BadReviewNotification { get; set; }

        public bool GoodSurveyNotification { get; set; }

        public bool BadSurveyNotification { get; set; }

        public bool AgingSurveyNotification { get; set; }

        public string IsUserExist { get; set; }

        public string SessionTimeOut { get; set; }

        public string DealerGroupId { get; set; }

        public bool MultipleDealerLogin { get; set; }

        public bool MasterLogin { get; set; }

        public int DefaultLogin { get; set; }

        public bool CanSeeAllStores { get; set; }

        public ButtonPermissionViewModel ProfileButtonPermissions { get; set; }

        public IEnumerable<RoleDealerAccess> AccessDealerPermissions { get; set; }
    }

    public class NotificationSettingViewModel
    {
        public NotificationSettingViewModel(){ }

        public NotificationSettingViewModel(NotificationSetting setting)
        {
            AppraisalNotification = setting.AppraisalNotified;
            WholeSaleNotfication = setting.WholesaleNotified;
            InventoryNotfication = setting.InventoryNotified;
            TwentyFourHourNotification = setting.C24hNotified;
            NoteNotification = setting.NoteNotified;
            PriceChangeNotification = setting.PriceChangeNotified;
            MarketPriceRangeChangeNotification = setting.MarketPriceRangeNotified;
            AgeingBucketJumpNotification = setting.AgingNotified;
            ImageUploadNotification = setting.ImageUploadNotified;
            BucketJumpReportNotification = setting.BucketJumpNotified;
            GoodReviewNotification = setting.GoodReviewNotified;
            BadReviewNotification = setting.BadReviewNotified;
            GoodSurveyNotification = setting.GoodSurveyNotified;
            BadSurveyNotification = setting.BadSurveyNotified;
            AgingSurveyNotification = setting.AgingSurveyNotified;
        }

        public bool AppraisalNotification { get; set; }
        public bool WholeSaleNotfication { get; set; }
        public bool InventoryNotfication { get; set; }
        public bool TwentyFourHourNotification { get; set; }
        public bool NoteNotification { get; set; }
        public bool PriceChangeNotification { get; set; }
        public bool MarketPriceRangeChangeNotification { get; set; }
        public bool AgeingBucketJumpNotification { get; set; }
        public bool ImageUploadNotification { get; set; }
        public bool BucketJumpReportNotification { get; set; }
        public bool GoodReviewNotification { get; set; }
        public bool BadReviewNotification { get; set; }
        public bool GoodSurveyNotification { get; set; }
        public bool BadSurveyNotification { get; set; }
        public bool AgingSurveyNotification { get; set; }
    }

    public class RoleDealerAccess
    {
        public int DealerId { get; set; }
        public int RoleId { get; set; }
    }
}
