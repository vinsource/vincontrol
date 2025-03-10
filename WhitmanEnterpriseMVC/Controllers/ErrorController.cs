using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Xml;
using System.Net;
using System.Text;
using System.Collections;
using System.IO;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.HelperClass;
using System.Configuration;
using System.Data;
using WhitmanEnterpriseMVC.KBBServiceEndPoint;

namespace WhitmanEnterpriseMVC.Controllers
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


    }



}

