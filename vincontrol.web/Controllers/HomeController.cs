using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Xml;
using System.Net;
using vincontrol.CarFax;
using Vincontrol.Web.Handlers;

namespace Vincontrol.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (SessionHandler.CurrentUser != null)
                return RedirectPermanent("/Market/ViewKpi");

            return View();
        }

        public ActionResult IndexForTimeOut()
        {
            return View();
        }
        
        public string TradeIn(string dealerName)
        {
            return dealerName;
        }

        public ActionResult KeepALive()
        {
            return Content("I am alive!");
        }
    }
}
