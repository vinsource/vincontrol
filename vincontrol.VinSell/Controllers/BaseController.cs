using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHandler.User == null) filterContext.Result = new RedirectResult(Url.Action("Index", "Home", new { area = string.Empty }));

            base.OnActionExecuting(filterContext);
        }
    }
}
