using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using EmployeeData.Custom;
using vincontrol.StockingGuide.Repository;
using vincontrol.StockingGuide.Repository.EntityModels;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace Vincontrol.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        //public IStockingGuideService service;
        public ActionResult Index()
        {
            var sqlUnitOfWork = new VincontrolUnitOfWork();
            sqlUnitOfWork.WeeklyTurnOverRepository.Add(new SGWeeklyTurnOver(){ DealerId=1000,Month = 1,Sale=1,SGWeeklyTurnOverId = 5,Stock=7,Turnover = 23,Week=9,Year=89});
            sqlUnitOfWork.Commit();
            return View();
        }

        //public TestController(IStockingGuideService srv)
        //{
        //    service = srv;
        //}

    }
}
