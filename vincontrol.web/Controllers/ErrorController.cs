using System.Web.Mvc;

namespace Vincontrol.Web.Controllers
{
    public class ErrorController : Controller
    {
        //public ActionResult Index()
        //{

        //    return View("Error");

        //}

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }  

        public ActionResult HttpError()
        {
         
            ViewData["Title"] = "Oops. We're sorry. An error occurred and we're on the case.";

            return View("Error");
        }

        public ActionResult Http404()
        {
            ViewData["Title"] = "The page you requested was not found";

            return View("Error");
        }

        public ActionResult EmptyBrochure()
        {
            return View();
        }

    }



}

