using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    public class MarketAnalysisController : BaseController
    {
        private IAuctionManagement _manheimAuctionManagement;
        private List<DateTime> _soldDays;

        public MarketAnalysisController()
        {
            _manheimAuctionManagement = new AuctionManagement();
            _soldDays = new List<DateTime>() { DateTime.Now.Date, DateTime.Now.AddDays(-1).Date };
        }

        public ActionResult Index()
        {
            if (SessionHandler.ManheimYearMakeModelList == null)
                SessionHandler.ManheimYearMakeModelList = new AuctionListViewModel(LinqHelper.GetManheimYearMakeModelForAdvancedSearch());
            //var soldVehicles = GetSoldVehiclesQuery(_soldDays);
            var soldVehicles = _manheimAuctionManagement.GetSoldVehiclesQuery(_soldDays);
            //SessionHandler.SoldVehicles = soldVehicles;
            //var model = new AuctionListViewModel() { AuctionList = soldVehicles };
            var model = new AuctionListViewModel();
            var result = (from i in soldVehicles
                          group i by i.Year into g
                          select new YearItem { Year = g.Key, Count = g.Count() }).OrderByDescending(i => i.Year).ToList();

            var serializer = new DataContractJsonSerializer(typeof(List<YearItem>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, result);
                model.JsonAuctionList = Encoding.Default.GetString(stream.ToArray());
            }

            //Initialize dropdownlists
            model.Year = result.OrderByDescending(i => i.Year).Select(i => i.Year).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();

            return View(model);
        }

        public ActionResult GenerateMakePieChart(string year)
        {
            var convertedYear = Convert.ToInt32(year);
            //if (SessionHandler.SoldVehicles == null)
            //    SessionHandler.SoldVehicles = _manheimAuctionManagement.GetSoldVehicles(_soldDays);

            var soldVehicles = _manheimAuctionManagement.GetSoldVehiclesQuery(_soldDays).Where(i => i.Year == convertedYear);
            //var model = new AuctionListViewModel() { AuctionList = soldVehicles };
            var model = new AuctionListViewModel();
            var result = (from i in soldVehicles
                          group i by i.Make into g
                          select new MakeItem { Make = g.Key, Count = g.Count() }).OrderBy(i => i.Make).ToList();

            var serializer = new DataContractJsonSerializer(typeof(List<MakeItem>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, result);
                model.JsonAuctionList = Encoding.Default.GetString(stream.ToArray());
            }

            //Initialize dropdownlists
            model.Make = result.OrderBy(i => i.Make).Select(i => i.Make).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();

            return PartialView(model);
        }

        public ActionResult GenerateModelPieChart(string year, string make)
        {
            var convertedYear = Convert.ToInt32(year);
            //if (SessionHandler.SoldVehicles == null)
            //    SessionHandler.SoldVehicles = _manheimAuctionManagement.GetSoldVehicles(_soldDays);

            var soldVehicles = _manheimAuctionManagement.GetSoldVehiclesQuery(_soldDays).Where(i => i.Year == convertedYear && i.Make == make);
            var model = new AuctionListViewModel();
            //var model = new AuctionListViewModel() { AuctionList = soldVehicles };
            var result = (from i in soldVehicles
                          group i by i.Model into g
                          select new ModelItem { Model = g.Key, Count = g.Count() }).OrderBy(i => i.Model).ToList();

            var serializer = new DataContractJsonSerializer(typeof(List<ModelItem>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, result);
                model.JsonAuctionList = Encoding.Default.GetString(stream.ToArray());
            }

            //Initialize dropdownlists
            model.Model = result.OrderBy(i => i.Model).Select(i => i.Model).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();

            return PartialView(model);
        }

        public string GetMarketValue(string year, string make, string model)
        {
            var convertedYear = Convert.ToInt32(year);
            //if (SessionHandler.SoldVehicles == null)
            //    SessionHandler.SoldVehicles = _manheimAuctionManagement.GetSoldVehicles(_soldDays);
            var days = new List<DateTime>() { DateTime.Now.Date, DateTime.Now.AddDays(-6).Date };
            var soldVehicles = _manheimAuctionManagement.GetSoldVehiclesQuery(days).Where(i => i.Year == convertedYear && i.Make == make && i.Model == model);
            var result = (from s in soldVehicles
                          group s by EntityFunctions.TruncateTime(s.DateStampSold)
                              into g
                              select new DatePriceItem {Day= g.Key, AveragePrice = g.Average(i => i.Mmr)}).ToList();
            AddContinuosValue(result, DateTime.Now.AddDays(-6).Date, DateTime.Now.Date);
            var serializer = new DataContractJsonSerializer(typeof(List<DatePriceItem>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, result.OrderBy(i=>i.Day).ToList());
                return Encoding.Default.GetString(stream.ToArray());
            }
            //var latestDay = soldVehicles.OrderByDescending(i => i.SoldDate).Select(i => i.SoldDate).FirstOrDefault();
            //var todaySoldVehicles = soldVehicles/*.Where(i => i.SoldDate == latestDay)*/;
            //return todaySoldVehicles.Count() + "_" + todaySoldVehicles.Average(i => i.Mmr).ToString("c0");
        }

        private void AddContinuosValue(List<DatePriceItem> result, DateTime minDate, DateTime maxDate)
        {
           for (DateTime currentDate = minDate; currentDate <= maxDate; currentDate = currentDate.AddDays(1))
           {
               if (result.All(i => i.Day != currentDate))
               {
                   var firstOrDefault = result.Where(i => i.Day < currentDate).OrderByDescending(j => j.Day).FirstOrDefault();
                   if (firstOrDefault != null)
                       result.Add(new DatePriceItem(){ Day = currentDate,AveragePrice = firstOrDefault.AveragePrice});
               }
           }
        }
    }

    [DataContract]
    public class DatePriceItem
    {
        [DataMember(Name = "day")]
        public DateTime? Day { get; set; }
        [DataMember(Name = "averageprice")]
        public double AveragePrice { get; set; }
    }
}
