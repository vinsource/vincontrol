using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Drawing;
using vincontrol.Data.Model;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.DatabaseModel;

namespace Vincontrol.Web.HelperClass
{
    public class EmailQueryHelpers
    {
        public static List<string> GetEmailsForAppraisalsNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                context.UserNotifications.Where(
                    x => x.DealerId == dealerId && x.AppraisalNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static List<string> GetEmailsForWholesaleNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                    context.UserNotifications.Where(
                        x =>x.DealerId == dealerId && x.WholesaleNotified && x.User.Active.HasValue &&x.User.Active.Value);
                    
                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static IEnumerable<string> GetEmailsForAddToInventoryNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                   context.UserNotifications.Where(
                       x => x.DealerId == dealerId && x.InventoryNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static IEnumerable<string> GetEmailsFor24HAppraisalAlertNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                  context.UserNotifications.Where(
                      x => x.DealerId == dealerId && x.C24hNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static IEnumerable<string> GetEmailsForNoteNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                 context.UserNotifications.Where(
                     x => x.DealerId == dealerId && x.NoteNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static List<string> GetEmailsForPriceChangesNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                 context.UserNotifications.Where(
                     x => x.DealerId == dealerId && x.PriceChangeNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static List<string> GetEmailsForAgingNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                  context.UserNotifications.Where(
                      x => x.DealerId == dealerId && x.AgingNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static List<string> GetEmailsForBucketJumpReportNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                context.UserNotifications.Where(
                    x => x.DealerId == dealerId && x.BucketJumpNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static List<string> GetEmailsForMarketPriceRangeNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                context.UserNotifications.Where(
                    x => x.DealerId == dealerId && x.MarketPriceRangeNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }

        public static List<string> GetEmailsForImageUploadNotification(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var result =
                   context.UserNotifications.Where(
                       x => x.DealerId == dealerId && x.ImageUploadNotified && x.User.Active.HasValue && x.User.Active.Value);

                return result.Select(x => x.User.Email).ToList();
            }
        }
    }
}