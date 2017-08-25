using System.Web.Mvc;
using Vincontrol.Web.Objects;


namespace Vincontrol.Web.Controllers
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
