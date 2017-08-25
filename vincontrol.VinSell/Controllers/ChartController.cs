using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.ViewModels.Chart;
using vincontrol.Data.Model;
using vincontrol.Helper;
using vincontrol.VinSell.Extensions;
using vincontrol.VinSell.Handlers;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Constant;

namespace vincontrol.VinSell.Controllers
{
    [DataContract]
    public class SelectionData
    {
        [DataMember(Name = "webSource")]
        public string Source { get; set; }
        [DataMember(Name = "dealerType")]
        public string DealerType { get; set; }
        [DataMember(Name = "option")]
        public string Options { get; set; }
        [DataMember(Name = "trims")]
        public List<string> Trims { get; set; }

        public bool IsCertified { get; set; }
    }

    public class ChartController : BaseController
    {
        private IAccountManagementForm _accountManagement;
        private IDealerManagementForm _dealerManagementForm;

        public ChartController()
        {
            _dealerManagementForm = new DealerManagementForm();
            _accountManagement = new AccountManagementForm();
        }

        [HttpPost]
        public string SaveSelections(string vin, string isCarsCom, string options, string trims,
                                     string isCertified, string isAll, string isFranchise, string isIndependant)
        {
            //if (SessionHandler.ChartScreen != null && SessionHandler.ChartScreen == Constanst.Appraisal)
            //    screen = Constanst.Appraisal;
            try
            {
                using (var context = new VinsellEntities())
                {
                    int dealerId = SessionHandler.User.DealerId;
                    //var receivedListingId = Convert.ToInt32(listingId);
                    var sourceType = Convert.ToBoolean(isCarsCom) ? "CarsCom" : "AutoTrader";
                    var existingChartSelection = context.manheimchartselections.FirstOrDefault(s => s.vin == vin && s.sourceType == sourceType && s.dealerId == dealerId);
                    if (existingChartSelection != null)
                    {
                        existingChartSelection.isAll = Convert.ToBoolean(isAll);
                        existingChartSelection.isCarsCom = Convert.ToBoolean(isCarsCom);
                        existingChartSelection.isCertified = Convert.ToBoolean(isCertified);
                        existingChartSelection.isFranchise = Convert.ToBoolean(isFranchise);
                        existingChartSelection.isIndependant = Convert.ToBoolean(isIndependant);
                        existingChartSelection.options = options.IndexOf(',') > 0
                                                             ? (options.Split(',')[0].Equals("0") ? "0" : options)
                                                             : options.ToLower();
                        existingChartSelection.trims = trims.IndexOf(',') > 0
                                                           ? (trims.Split(',')[0].Equals("0") ? "0" : trims)
                                                           : trims.ToLower();
                        existingChartSelection.dealerId = dealerId;

                        context.SaveChanges();
                    }
                    else
                    {
                        var newSelection = new manheimchartselection()
                        {
                            vin = vin,
                            isAll = Convert.ToBoolean(isAll),
                            isCarsCom = Convert.ToBoolean(isCarsCom),
                            isCertified = Convert.ToBoolean(isCertified),
                            isFranchise = Convert.ToBoolean(isFranchise),
                            isIndependant = Convert.ToBoolean(isIndependant),
                            options =
                                options.IndexOf(',') > 0
                                    ? (options.Split(',')[0].Equals("0") ? "0" : options)
                                    : options,
                            trims = trims.IndexOf(',') > 0 ? (trims.Split(',')[0].Equals("0") ? "0" : trims) : trims,
                            sourceType = sourceType,
                            dealerId = dealerId
                        };
                        context.AddTomanheimchartselections(newSelection);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Your selections have been saved successfully";
        }
       
        public ActionResult ViewFullChart(int listingId, string filterOptions)
        {
            return GetChartData(listingId, "ViewGraphNationwide", filterOptions);
        }

        public ActionResult ViewGoogleGraph(int listingId, string filterOptions)
        {
            return GetChartData(listingId, "ViewGoogleGraph", filterOptions);
        }

        private ActionResult GetChartData(int listingId, string viewName, string filterOptions)
        {
            
            var contextVinSell = new VinsellEntities();
            var model = GetChartItems(listingId, filterOptions, contextVinSell);
            GetSavedSelections(listingId, model, contextVinSell, filterOptions);
            model.Type = Constanst.CarInfoType.Used;
            model.InventoryStatus = Constanst.InventoryStatus.Inventory;
            return View(viewName, model);
        }

        private static void GetSavedSelections(int  id, ChartSelectionViewModel model, VinsellEntities contextVinControl, string filterOptions)
        {
            model.Id = id;
            if (!String.IsNullOrEmpty(filterOptions))
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(filterOptions)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(SelectionData));
                    var selectionData = (SelectionData)serializer.ReadObject(ms);

                    model.IsCarsCom = selectionData.Source == "carscom";
                    model.IsAll = selectionData.DealerType == "all";
                    model.IsCertified = selectionData.IsCertified;
                    model.IsFranchise = selectionData.DealerType == "franchise";
                    model.IsIndependant = selectionData.DealerType == "independant";
                    model.Options = selectionData.Options;
                    model.Trims = String.Join(",", selectionData.Trims);

                }
            }
       

        }

        private ChartSelectionViewModel GetChartItems(int listingId, string filterOptions, VinsellEntities contextVinSell)
        {
            // Create a Session to identify what the chart screen is
            SessionHandler.ChartScreen = Constanst.Inventory;

            var model = new ChartSelectionViewModel
                {
                    FilterOptions = filterOptions != null ? filterOptions.Replace("\"", "'") : null,
                    CarsCom = new Application.ViewModels.Chart.ChartSelection()
                };


            var targetCar = contextVinSell.manheim_vehicles.FirstOrDefault(i => i.Id == listingId);
        
            ViewData[Constanst.ListingId] = listingId;

            if (targetCar != null)
            {
                ViewData[Constanst.CarTitle] = targetCar.Year + " " + targetCar.Make + " " + targetCar.Model + " " +
                                               targetCar.Trim + " - Mileage : " + targetCar.Mileage
                                               + " / Price : " +
                                               targetCar.Mmr;
                model.Vin = targetCar.Vin;
            }
            return model;
        }
        
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public JsonResult GetMarketDataByListingNationwideWithHttpPost(int listingId, short screen)
        {
            var context = new VinsellEntities();

            var inventory = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

            var dealer = _dealerManagementForm.GetDealerById(SessionHandler.User.DealerId);

            var chartGraph = MarketHelper.GetMarketCarsForNationwideMarketForVinsell(inventory, dealer, false);
            return new DataContractJsonResult(chartGraph);

        }
    }

    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }

    public class CacheFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the cache duration in seconds. The default is 10 seconds.
        /// </summary>
        /// <value>The cache duration in seconds.</value>
        public int Duration
        {
            get;
            set;
        }

        public CacheFilterAttribute()
        {
            Duration = 15;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Duration <= 0) return;

            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(cacheDuration));
            cache.SetMaxAge(cacheDuration);
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }
    }
}
