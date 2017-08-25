using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.Helper;
using vincontrol.Data.Model;
using vincontrol.Manheim;
using vincontrol.WebAPI.Helper;
using Constanst = vincontrol.Constant.Constanst;
using vincontrol.DomainObject;
using KarPowerService = vincontrol.KBB.KBBService;

namespace vincontrol.WebAPI.Controllers
{
    public class AppraisalController : ApiController
    {

        private ICarFaxService _carFaxService;

        public AppraisalController()
        {
            _carFaxService = new CarFaxService();
        }

        [HttpPost]
        [HttpGet]
        public List<SmallKarPowerViewModel> KarPowerData(int listingId, int dealerId, string zipCode)
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealerId);

                short type = Constanst.VehicleStatus.Appraisal;

                string vin = null; int vehicleid = 0; long mileage = 0; int? kbbTrimId = null; string kbbOptionsId = null; int? kbbModelId = null;
                var existingAppraisal = context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);
                if (existingAppraisal != null)
                {
                    vehicleid = existingAppraisal.Vehicle.VehicleId;
                    vin = existingAppraisal.Vehicle.Vin;
                    mileage = existingAppraisal.Mileage.GetValueOrDefault();
                    kbbModelId = existingAppraisal.Vehicle.KBBModelId;
                    kbbTrimId = existingAppraisal.Vehicle.KBBTrimId;
                    kbbOptionsId = existingAppraisal.Vehicle.KBBOptionsId;
                }


                var result = new List<SmallKarPowerViewModel>();
                if (setting != null)
                {
                    try
                    {
                        if (context.KellyBlueBooks.Any(x => x.VehicleId == vehicleid))
                        {
                            var searchResult = context.KellyBlueBooks.Where(x => x.VehicleId == vehicleid).ToList();
                            // This VIN has one model
                            {
                                result = searchResult.Select(tmp => new SmallKarPowerViewModel
                                {
                                    BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                                    MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                                    SelectedModelId = tmp.ModelId.GetValueOrDefault(),
                                    SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                    SelectedTrimName = tmp.Trim,
                                    Wholesale = tmp.WholeSale.GetValueOrDefault(),
                                    IsSelected = (kbbTrimId.GetValueOrDefault() == 0 || (tmp.TrimId == kbbTrimId)),
                                }).ToList();

                             }
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(vin))
                            {
                                var dealer = new DealershipViewModel(setting.Dealer);
                                
                                var chartGraph =
                                    MarketHelper.GetMarketCarsForNationwideMarketForAppraisalOnChart(listingId, dealer,
                                        false);
                                if (chartGraph.TypedChartModels.Any())
                                {
                                    var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                                    if (firstOrDefault != null)
                                        vin = firstOrDefault.VIN;
                                }
                            }

                            if (!String.IsNullOrEmpty(vin))
                            {
                                var karpowerService = new KarPowerService();
                                result = karpowerService.Execute(vehicleid, vin,
                                    mileage.ToString(CultureInfo.InvariantCulture), DateTime.Now,
                                    setting.KellyBlueBook, setting.KellyPassword, type);
                                if (kbbTrimId > 0)
                                {
                                    var smallKarPowerViewModel = result.FirstOrDefault(x => x.SelectedTrimId == kbbTrimId);
                                    if (smallKarPowerViewModel != null)
                                        smallKarPowerViewModel.IsSelected = true;
                                }
                            }
                            else
                            {
                                result = new List<SmallKarPowerViewModel>();
                            }

                           
                        }

                    }
                    catch (Exception)
                    {
                        result = new List<SmallKarPowerViewModel>();
                    }

                }
                else
                    result = new List<SmallKarPowerViewModel>();

                foreach (var item in result)
                {
                    item.TotalCount = result.Count;
                }

                return result.Any(i => i.IsSelected) ? result.Where(i => i.IsSelected).ToList() : result;
           
            }

        }

        [HttpPost]
        [HttpGet]
        public List<ManheimWholesaleViewModel> ManheimData(int listingId, int dealerId)
        {
            List<ManheimWholesaleViewModel> result;
            var manheimService = new ManheimService();
            try
            {
                using (var context = new VincontrolEntities())
                {
                    var row = context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);
                    var manheimCredential = context.Settings.FirstOrDefault(m => m.DealerId == dealerId && !string.IsNullOrEmpty(m.Manheim.Trim()));
                    if (manheimCredential != null)
                    {
                        if (String.IsNullOrEmpty(row.Vehicle.Vin))
                        {
                            var dealer = new DealershipViewModel(manheimCredential.Dealer);

                            var chartGraph =
                                MarketHelper.GetMarketCarsForNationwideMarketForAppraisalOnChart(listingId, dealer,
                                    false);
                            if (chartGraph.TypedChartModels.Any())
                            {
                                var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                                if (firstOrDefault != null)
                                    row.Vehicle.Vin = firstOrDefault.VIN;
                            }
                        }
                        result = manheimService.ManheimReportForAppraisal(row, manheimCredential.Manheim.Trim(), manheimCredential.ManheimPassword.Trim()).GroupBy(p => p.TrimName).Select(g => g.First()).ToList();
                        if (row.Vehicle.ManheimTrimId.GetValueOrDefault() > 0)
                        {
                            var trimId = row.Vehicle.ManheimTrimId.GetValueOrDefault();
                            var smallManheimViewModel = result.FirstOrDefault(x => x.TrimServiceId == trimId);
                            if (smallManheimViewModel != null)
                                smallManheimViewModel.IsSelected = true;
                        }
                    }

                    else
                        result = new List<ManheimWholesaleViewModel>();
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return result.Any(i => i.IsSelected) ? result.Where(i => i.IsSelected).ToList() : result;
        }

        [HttpPost]
        [HttpGet]
        public CarFaxViewModel CarFaxData(int listingId)
        {
            using (var context = new VincontrolEntities())
            {
                var appraisal = context.Appraisals.FirstOrDefault(x => x.AppraisalId == listingId);

                var carfax = WebApiCarFaxHelper.GetCarFaxReportInDatabase(context, appraisal.Vehicle.VehicleId);

                if (carfax.Success)
                {
                    return carfax;
                }

                carfax = _carFaxService.ConvertXmlToCarFaxModelAndSave(appraisal.Vehicle.VehicleId, appraisal.Vehicle.Vin, appraisal.Dealer.Setting.CarFax,
                    appraisal.Dealer.Setting.CarFaxPassword);

                return carfax;


            }
        }


        [HttpPost]
        [HttpGet]
        public ChartGraph GetMarketDataByListingNationwideWithHttpPost(int listingId, int dealerId, string chartScreen, bool isSold = false)
        {
            var chartGraph = new ChartGraph();

            try
            {
                using (var context = new VincontrolEntities())
                {
                    var queryDealer = context.Dealers.FirstOrDefault(d => d.DealerId == dealerId);

                    var dealer = new DealershipViewModel(queryDealer);

                    if (chartScreen != null && chartScreen == Constanst.Appraisal)
                    {
                        chartGraph = MarketHelper.GetMarketCarsForNationwideMarketForAppraisalOnChart(listingId, dealer, isSold);
                    }
                    else
                    {
                        chartGraph = MarketHelper.GetMarketCarsForNationwideMarket(listingId, dealer, isSold);
                    }
                }
            }
            catch (Exception ex)
            {
                chartGraph.Error = ex.Message;
            }

            return chartGraph;
        }

    }
}
