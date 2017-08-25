using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class NotificationRepository : INotificationRepository
    {
        private VincontrolEntities _context;

        public NotificationRepository(VincontrolEntities context)
        {
            _context = context;
        }

        public IQueryable<string> GetEmailsForAppraisalsNotification(int dealerId)
        {
            var result =
               _context.UserNotifications.Where(
                   x => x.DealerId == dealerId && x.AppraisalNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForWholesaleNotification(int dealerId)
        {
            var result =
            _context.UserNotifications.Where(
                x => x.DealerId == dealerId && x.WholesaleNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForAddToInventoryNotification(int dealerId)
        {
            var result =
            _context.UserNotifications.Where(
                x => x.DealerId == dealerId && x.InventoryNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsFor24HAppraisalAlertNotification(int dealerId)
        {
            var result =
            _context.UserNotifications.Where(
                x => x.DealerId == dealerId && x.C24hNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForNoteNotification(int dealerId)
        {
            var result =
              _context.UserNotifications.Where(
                  x => x.DealerId == dealerId && x.NoteNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForPriceChangesNotification(int dealerId)
        {
            var result =
           _context.UserNotifications.Where(
               x => x.DealerId == dealerId && x.PriceChangeNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForAgingNotification(int dealerId)
        {
            var result =
             _context.UserNotifications.Where(
                 x => x.DealerId == dealerId && x.AgingNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForBucketJumpReportNotification(int dealerId)
        {
            var result =
           _context.UserNotifications.Where(
               x => x.DealerId == dealerId && x.BucketJumpNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForMarketPriceRangeNotification(int dealerId)
        {
            var result =
             _context.UserNotifications.Where(
                 x => x.DealerId == dealerId && x.MarketPriceRangeNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IQueryable<string> GetEmailsForImageUploadNotification(int dealerId)
        {
            var result =
            _context.UserNotifications.Where(
                x => x.DealerId == dealerId && x.ImageUploadNotified && x.User.Active.HasValue && x.User.Active.Value);

            return result.Select(x => x.User.Email);
        }

        public IList<string> GetEmailsForAdmin(int dealerId)
        {
            var result =
                _context.UserPermissions.Where(
                    x => x.DealerId == dealerId && x.RoleId == 1);

            return result.Select(x => x.User.Email).ToList();
        }

        public IQueryable<EmailWaitingList> GetEmailsInWaitingList()
        {
            var result =
           _context.EmailWaitingLists.Where(
               x => x.Expire.HasValue && x.Expire==false);

            return result;
        }

        public void UpdateExpiredEmailInWaitingList(int notificationEmailId)
        {
            var notificationEmail =
                _context.EmailWaitingLists.FirstOrDefault(x => x.NotificationEmailId == notificationEmailId);

            if (notificationEmail != null)
            {
                notificationEmail.Expire = true;
                notificationEmail.SentTime = DateTime.Now;

            }
        }


        public void DeleteExpiredEmailInWaitingList()
        {
            var notificationEmails =
                _context.EmailWaitingLists.Where(x => x.Expire==true);

            foreach (var tmp in notificationEmails)
            {
                _context.DeleteObject(tmp);
            }
        }

        public void AddFlyerShareActivity(FlyerShareDealerActivity acitivity)
        {
            _context.AddToFlyerShareDealerActivities(acitivity);
        }
    }
}
