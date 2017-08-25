using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Constant;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Security;
using Vincontrol.Web.Models;
using vincontrol.Helper;

namespace Vincontrol.Web.Controllers
{
    public class TrackingController : SecurityController
    {
        private const string PermissionCode = "ACTIVITY";
        private const string AcceptedValues = "READONLY, ALLACCESS";

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewAllDealerActivity(short? type)
        {
            if (type == null)
                type = Constanst.ActivityTypeId.Inventory;
            ViewData["Type"] = type;
            return View("DealerActivity", null);
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult GetAllDealerActivity(short? type,int pageIndex=1,int pageSize=50)
        {
            //.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            if (type == null)
                type = Constanst.ActivityTypeId.Inventory;
            ViewData["Type"] = type;
            var activities = VincontrolLinqHelper.GetAllActivities(type.Value).Where(x => !String.IsNullOrEmpty(x.UserStamp));
            var startDate = DateTime.Now;

            if(activities.Any())
            startDate = activities.Min(x => x.DateStamp);
            //if (dealershipActivityViewModel != null)
            // startDate = dealershipActivityViewModel.DateStamp;
           
            ViewData["StartYear"] = startDate.Year;
            ViewData["StartMonth"] = startDate.Month;
            ViewData["StartDay"] = startDate.Day;
            ViewData["IsFirstime"] = true;
            ViewData["NumberOfRecord"] = activities.Count();
            return PartialView("DealerActivityDetail", activities.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult GetDealerActivity(short? type, int pageIndex, int pageSize)
        {
            //.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            if (type == null)
                type = Constanst.ActivityTypeId.Inventory;
            ViewData["Type"] = type;
            ViewData["IsFirstime"] = false;

            var activities = VincontrolLinqHelper.GetAllActivities(type.Value).Where(x => !String.IsNullOrEmpty(x.UserStamp)).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return PartialView("DealerActivityDetail", activities.ToList());
        }

        public ActionResult FilterDealerActivityNew(string fromdate, string todate, short type = Constanst.ActivityTypeId.Inventory, int pageIndex = 1, int pageSize = 50)
        {
            var list = FilterActiviity(fromdate, todate, type);
            ViewData["Type"] = type;
            ViewData["IsFirstime"] = true;
            ViewData["NumberOfRecord"] = list.Count();
            return PartialView("DealerActivityDetail", list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        private static IEnumerable<DealershipActivityViewModel> FilterActiviity(string fromdate, string todate, short type)
        {
            string[] arrFrom =
                fromdate.Split('/');
            int fromDay = 1;
            int fromMonth = 1;
            int fromYear = 2000;
            if (arrFrom.Count() > 2)
            {
                fromMonth = Convert.ToInt32(arrFrom[0]);
                fromDay = Convert.ToInt32(arrFrom[1]);
                fromYear = Convert.ToInt32(arrFrom[2]);
            }

            DateTime fromDate = new DateTime(fromYear, fromMonth, fromDay, 0, 0, 0);

            string[] arrTo =
                todate.Split('/');
            int toDay = 1;
            int toMonth = 1;
            int toYear = 2000;
            if (arrTo.Count() > 2)
            {
                toMonth = Convert.ToInt32(arrTo[0]);
                toDay = Convert.ToInt32(arrTo[1]);
                toYear = Convert.ToInt32(arrTo[2]);
            }

            DateTime toDate = new DateTime(toYear, toMonth, toDay, 23, 59, 59);
            var list =
                VincontrolLinqHelper.FilterActivitiesByDate(fromDate, toDate, type)
                    .Where(x => !String.IsNullOrEmpty(x.UserStamp));
            return list;
        }

        public ActionResult FilterDealerActivityPaging(string fromdate, string todate, short type = Constanst.ActivityTypeId.Inventory, int pageIndex = 1, int pageSize = 50)
        {
            var list = FilterActiviity(fromdate, todate, type);
            ViewData["Type"] = type;
            ViewData["IsFirstime"] = false;
            return PartialView("DealerActivityDetail", list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        public ActionResult RedirectToDetailActivity(int id)
        {
            throw new Exception("Unimplemented");
            //using (var context = new VincontrolEntities())
            //{
            //    var activity = context.DealerActivities.FirstOrDefault(i => i.DealerActivityId == id);
            //    if (activity == null) return RedirectToAction("ViewKpi");

            //    var listingId = 0;
            //    switch (activity.DealerActivitySubTypeCode)
            //    {
            //        case Constanst.SubActivityType.AddToInventory:
            //            listingId = GetListingIdFromActivityContent(activity.Detail.Split(';').ToArray()[1]);
            //            return RedirectToAction("ViewIProfile", "Inventory", new {ListingID = listingId});
            //        case Constanst.SubActivityType.NewAppraisal:
            //            var appraisalId = GetAppraisalIdFromActivityContent(activity.Detail.Split(';').ToArray()[0]);
            //            return RedirectToAction("ViewProfileForAppraisal", "Appraisal", new { appraisalId });
            //        case Constanst.SubActivityType.NewUser:
            //            return RedirectToAction("AdminSecurity", "Admin");
            //        case Constanst.SubActivityType.PriceChange:
            //            listingId = GetListingIdFromActivityContent(activity.Detail.Split(';').ToArray()[0]);
            //            return RedirectToAction("ViewIProfile", "Inventory", new { listingId });
            //        default: return RedirectToAction("ViewKpi");
            //    }
            //}
        }

        private int GetAppraisalIdFromActivityContent(string content)
        {
            try
            {
                return Convert.ToInt32(content.Replace("Appraisal Id: ", ""));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetListingIdFromActivityContent(string content)
        {
            try
            {
                return Convert.ToInt32(content.Replace("Listing Id: ", ""));
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }
}
