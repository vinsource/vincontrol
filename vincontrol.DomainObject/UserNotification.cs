using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class UserNotification
    {
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
    }
}
