﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vincontrol.VinSocial.Controllers
{
    public class ReviewsController : Controller
    {
        //
        // GET: /Reviews/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReviewManager()
        {
            return View();
        }

    }
}
