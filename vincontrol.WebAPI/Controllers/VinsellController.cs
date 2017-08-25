using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.CarFax;
using vincontrol.Constant;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.Data.Model;
using vincontrol.Manheim;
using vincontrol.WebAPI.Helper;
using KarPowerService = vincontrol.KBB.KBBService;
namespace vincontrol.WebAPI.Controllers
{
    public class VinsellController : ApiController
    {
        //
        // GET: /Vinsell/
        private IDealerManagementForm _dealerManagementForm;
        private ICarFaxService _carFaxService;
        public VinsellController()
        {
            _dealerManagementForm=new DealerManagementForm();
            _carFaxService = new CarFaxService();
        }


        [HttpPost]
        [HttpGet]
        public CarFaxViewModel CarFaxData(int listingId, int dealerId)
        {
            using (var context = new VinsellEntities())
            {
                var dealer = _dealerManagementForm.GetDealerById(dealerId);
                var inventory = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

                if (inventory != null)
                {
                    var carfax = WebApiCarFaxHelper.GetCarFaxReportInDatabase(context, inventory.Id);

                    if (carfax.Success)
                        return carfax;

                    carfax = _carFaxService.ConvertXmlToCarFaxModelForVinsell(inventory.Id, inventory.Vin, dealer.DealerSetting.CarFax, dealer.DealerSetting.CarFaxPassword);

                    return carfax;
                }

                return new CarFaxViewModel();

            }
        }


        [HttpPost]
        [HttpGet]
        public List<SmallKarPowerViewModel> KarPowerData(int listingId, int dealerId,string zipcode)
        {
            using (var context = new VinsellEntities())
            {
                var dealer = _dealerManagementForm.GetDealerById(dealerId);

                var inventory = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

                List<SmallKarPowerViewModel> result;
                if (inventory!=null&& !String.IsNullOrEmpty(dealer.DealerSetting.KellyBlueBook )&&!String.IsNullOrEmpty(dealer.DealerSetting.KellyPassword ))
                {
                    try
                    {
                        if (context.manheim_KellyBlueBook.Any(x => x.VehicleId == inventory.Id))
                        {
                            var searchResult = context.manheim_KellyBlueBook.Where(x => x.VehicleId == inventory.Id).ToList();

                            {
                                //this VIN has one model
                                result = searchResult.Select(tmp => new SmallKarPowerViewModel
                                {
                                    BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                                    MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                                    SelectedModelId = tmp.ModelId.GetValueOrDefault(),
                                    SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                    SelectedTrimName = tmp.Trim,
                                    Wholesale = tmp.WholeSale.GetValueOrDefault(),
                                    AddsDeducts = tmp.WholeSale.GetValueOrDefault() - tmp.BaseWholeSale.GetValueOrDefault() - tmp.MileageAdjustment.GetValueOrDefault(),
                                    
                                }).ToList();

                                if (result.Count > 0 && !result.Any(i => i.IsSelected))
                                {
                                    foreach (var item in result)
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var karpowerService = new KarPowerService();
                            result = karpowerService.ExecuteVinsell(inventory.Id, inventory.Vin,
                                      inventory.Mileage.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), DateTime.Now,
                                      dealer.DealerSetting.KellyBlueBook, dealer.DealerSetting.KellyPassword);
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

              return result;
            }
        }


        [HttpPost]
        [HttpGet]
        public List<ManheimWholesaleViewModel> ManheimData(int listingId,int dealerId,int inventoryStatus)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new VinsellEntities())
                {
                    var dealer = _dealerManagementForm.GetDealerById(dealerId);
                    var inventory = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

                  

                    if (inventory != null && !String.IsNullOrEmpty(dealer.DealerSetting.KellyBlueBook) &&
                        !String.IsNullOrEmpty(dealer.DealerSetting.KellyPassword))
                    {
                        var model = new VehicleViewModel()
                        {
                            VehicleId = inventory.Id,
                            Vin=inventory.Vin,
                            Year = inventory.Year
                        };
                        var manheimService = new ManheimService();
                        result = manheimService.ManheimReportNew(context, model, dealer.DealerSetting.Manheim,
                            dealer.DealerSetting.ManheimPassword);

                        return result;
                    }
                    return new List<ManheimWholesaleViewModel>();
                    
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return result;
        }

        [HttpPost]
        [HttpGet]
        public ChartGraph GetMarketDataByListingNationwideWithHttpPost(int listingId, int dealerId, string chartScreen, bool isSold = false)
        {
            var chartGraph = new ChartGraph();

            try
            {
                using (var context = new VinsellEntities())
                {
                    var inventory = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

                    var dealer = _dealerManagementForm.GetDealerById(dealerId);
                    
                    chartGraph = MarketHelper.GetMarketCarsForNationwideMarketForVinsell(inventory, dealer, isSold);
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
