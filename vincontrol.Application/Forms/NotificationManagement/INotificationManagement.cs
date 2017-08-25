using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Forms.NotificationManagement
{
    public interface INotificationManagement
    {
        IList<string> GetEmailsForAppraisalsNotification(int dealerId);
        IList<string> GetEmailsForWholesaleNotification(int dealerId);
        IList<string> GetEmailsForAddToInventoryNotification(int dealerId);
        IList<string> GetEmailsFor24HAppraisalAlertNotification(int dealerId);
        IList<string> GetEmailsForNoteNotification(int dealerId);
        IList<string> GetEmailsForPriceChangesNotification(int dealerId);
        IList<string> GetEmailsForAgingNotification(int dealerId);
        IList<string> GetEmailsForBucketJumpReportNotification(int dealerId);
        IList<string> GetEmailsForMarketPriceRangeNotification(int dealerId);
        IList<string> GetEmailsForImageUploadNotification(int dealerId);
        IList<string> GetEmailsForAdmin(int dealerId);
        IList<EmailWaitingList> GetEmailsInWaitingList();
        void UpdateExpiredEmailInWaitingList(int notificationEmailId);
        void DeleteExpiredEmailInWaitingList();
        void AddFlyerShareActivity(FlyerShareDealerActivity acitivity);
    }
}
