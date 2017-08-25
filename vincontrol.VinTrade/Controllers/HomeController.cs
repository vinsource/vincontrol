using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vincontrol.VinTrade.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            return RedirectToRoute("TradeStepOne", new { dealer = "newportcoastauto" });
        }

    }
}
