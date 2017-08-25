using System.Web.Mvc;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Objects;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class MaintenanceController : Controller
    {
        public ActionResult GetMaintenanceInfo()
        {
            var maintenanceObject = MaintenanceInfo.GetServerMaintenance(Session.Timeout);
            return Json(maintenanceObject, JsonRequestBehavior.AllowGet);
        }
    }
}
