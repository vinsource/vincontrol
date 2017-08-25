using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface INotificationRepository
    {
        IQueryable<string> GetEmailsForAppraisalsNotification(int dealerId);
        IQueryable<string> GetEmailsForWholesaleNotification(int dealerId);
        IQueryable<string> GetEmailsForAddToInventoryNotification(int dealerId);
        IQueryable<string> GetEmailsFor24HAppraisalAlertNotification(int dealerId);
        IQueryable<string> GetEmailsForNoteNotification(int dealerId);
        IQueryable<string> GetEmailsForPriceChangesNotification(int dealerId);
        IQueryable<string> GetEmailsForAgingNotification(int dealerId);
        IQueryable<string> GetEmailsForBucketJumpReportNotification(int dealerId);
        IQueryable<string> GetEmailsForMarketPriceRangeNotification(int dealerId);
        IQueryable<string> GetEmailsForImageUploadNotification(int dealerId);
        IList<string> GetEmailsForAdmin(int dealerId);
        IQueryable<EmailWaitingList> GetEmailsInWaitingList();
        void UpdateExpiredEmailInWaitingList(int notificationEmailId);
        void DeleteExpiredEmailInWaitingList();
        void AddFlyerShareActivity(FlyerShareDealerActivity acitivity);
    }
}
