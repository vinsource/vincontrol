using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Vincontrol.Web.Handlers;

namespace Vincontrol.Web.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SessionController : Controller
    {
        [HttpGet]
        public ActionResult GetStartSessionTime()
        {
            return Json(new{StartSessionTime = SessionHandler.StartSessionTime, CurrentTime =DateTime.Now}, JsonRequestBehavior.AllowGet);
        }
    }
}
