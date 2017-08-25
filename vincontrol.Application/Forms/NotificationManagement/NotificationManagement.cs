using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.NotificationManagement
{
    public class NotificationManagement :BaseForm, INotificationManagement
    {
        public NotificationManagement() : this(new SqlUnitOfWork()) { /*_carfaxService = new CarFaxService();*/ }

        public NotificationManagement(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IList<string> GetEmailsForAppraisalsNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForAppraisalsNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForWholesaleNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForWholesaleNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForAddToInventoryNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForAddToInventoryNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsFor24HAppraisalAlertNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsFor24HAppraisalAlertNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForNoteNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForNoteNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForPriceChangesNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForPriceChangesNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForAgingNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForAgingNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForBucketJumpReportNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForBucketJumpReportNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForMarketPriceRangeNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForMarketPriceRangeNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForImageUploadNotification(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForImageUploadNotification(dealerId);
            return list.ToList();
        }

        public IList<string> GetEmailsForAdmin(int dealerId)
        {
            var list = UnitOfWork.Notification.GetEmailsForAdmin(dealerId);
            return list.ToList();
        }

        public IList<EmailWaitingList> GetEmailsInWaitingList()
        {
            var list = UnitOfWork.Notification.GetEmailsInWaitingList();
            return list.ToList();
        }

        public void UpdateExpiredEmailInWaitingList(int notificationEmailId)
        {
            UnitOfWork.Notification.UpdateExpiredEmailInWaitingList(notificationEmailId);
            UnitOfWork.CommitVincontrolModel();
        }

        public void DeleteExpiredEmailInWaitingList()
        {
            UnitOfWork.Notification.DeleteExpiredEmailInWaitingList();
            UnitOfWork.CommitVincontrolModel();
        }

        public void AddFlyerShareActivity(FlyerShareDealerActivity acitivity)
        {
            UnitOfWork.Notification.AddFlyerShareActivity(acitivity);
            UnitOfWork.CommitVincontrolModel();
        }
    }
}
