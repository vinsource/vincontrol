using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.CarFax;
using vincontrol.Helper;
using vincontrol.Data.Model;
using vincontrol.Manheim;
using vincontrol.WebAPI.Helper;
using Constanst = vincontrol.Constant.Constanst;
using vincontrol.DomainObject;
using CarFaxViewModel = vincontrol.Application.ViewModels.CommonManagement.CarFaxViewModel;
using CarFaxWindowSticker = vincontrol.Application.ViewModels.CommonManagement.CarFaxWindowSticker;
using KarPowerService = vincontrol.KBB.KBBService;

namespace vincontrol.WebAPI.Controllers
{
    public class InventoryController : ApiController
    {
        private ICarFaxService _carFaxService;

        public InventoryController()
        {
            _carFaxService = new CarFaxService();
        }

        [HttpPost]
        [HttpGet]
        public List<SmallKarPowerViewModel> KarPowerData(int listingId, int inventoryStatus, int dealerId, string zipCode)
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealerId);

                short type = Constanst.VehicleStatus.Inventory;

                string vin = null; int vehicleid = 0; long mileage = 0; int? kbbModelId = null; int? kbbTrimId = null; string kbbOptionsId = null;
             
                switch (inventoryStatus)
                {
                    case 1:
                        {
                            var existingInventory = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId);
                            if (existingInventory != null)
                            {

                                vehicleid = existingInventory.Vehicle.VehicleId;
                                vin = existingInventory.Vehicle.Vin;
                                mileage = existingInventory.Mileage.GetValueOrDefault();
                                kbbModelId = existingInventory.Vehicle.KBBModelId;
                                kbbTrimId = existingInventory.Vehicle.KBBTrimId;
                                kbbOptionsId = existingInventory.Vehicle.KBBOptionsId;
                            }
                        }
                        break;
                    case -1:
                        {
                            var existingSoldOutInventory =
                                context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId);
                            if (existingSoldOutInventory != null)
                            {
                                vehicleid = existingSoldOutInventory.Vehicle.VehicleId;
                                vin = existingSoldOutInventory.Vehicle.Vin;
                                mileage = existingSoldOutInventory.Mileage.GetValueOrDefault();
                                kbbModelId = existingSoldOutInventory.Vehicle.KBBModelId;
                                kbbTrimId = existingSoldOutInventory.Vehicle.KBBTrimId;
                                kbbOptionsId = existingSoldOutInventory.Vehicle.KBBOptionsId;
                            }
                            type = Constanst.VehicleStatus.SoldOut;
                        }
                        break;
                    default:
                        return new List<SmallKarPowerViewModel>();
                }

                var result = new List<SmallKarPowerViewModel>();
                if (setting != null)
                {
                    try
                    {
                        if (context.KellyBlueBooks.Any(x => x.VehicleId == vehicleid))
                        {
                            var searchResult = context.KellyBlueBooks.Where(x => x.VehicleId == vehicleid).ToList();
                            
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
                                    AddsDeducts=tmp.WholeSale.GetValueOrDefault() -  tmp.BaseWholeSale.GetValueOrDefault()-tmp.MileageAdjustment.GetValueOrDefault(),
                                    IsSelected = tmp.TrimId == kbbTrimId,
                                }).ToList();

                              
                            }
                        }
                        else
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
        public List<ManheimWholesaleViewModel> ManheimData(int listingId, int inventoryStatus, int dealerId)
        {
            List<ManheimWholesaleViewModel> result;
            var manheimService = new ManheimService();
            try
            {
                using (var context = new VincontrolEntities())
                {
                    var model = !inventoryStatus.Equals(-1)
                        ? new VehicleViewModel(context.Inventories.Include("Vehicle")
                            .FirstOrDefault(x => x.InventoryId == listingId))
                        : new VehicleViewModel(context.SoldoutInventories.Include("Vehicle")
                            .FirstOrDefault(x => x.SoldoutInventoryId == listingId));
                    
                    var manheimCredential = context.Settings.FirstOrDefault(m => m.DealerId == dealerId && !string.IsNullOrEmpty(m.Manheim.Trim()));
                    result = manheimCredential != null
                        ? manheimService.ManheimReportNew(context,model, manheimCredential.Manheim.Trim(),
                            manheimCredential.ManheimPassword.Trim())
                        : new List<ManheimWholesaleViewModel>();

                    if (model.ManheimTrimid > 0)
                    {
                        var smallManheimViewModel = result.FirstOrDefault(x => x.TrimServiceId == model.ManheimTrimid);
                        if (smallManheimViewModel != null)
                            smallManheimViewModel.IsSelected = true;
                    }
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
        public CarFaxViewModel CarFaxData(int listingId, int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var inventory = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId) ??
                              new Inventory(context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId), Constanst.VehicleStatus.SoldOut);

                {
                    if (inventory.Condition == Constanst.ConditionStatus.New && inventory.InventoryStatusCodeId.Equals(Constanst.VehicleStatus.Inventory))
                        return new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), Success = false };

                    var carfax = WebApiCarFaxHelper.GetCarFaxReportInDatabase(context, inventory.Vehicle.VehicleId);

                    if (carfax.Success)
                        return carfax;
                    
                    carfax = _carFaxService.ConvertXmlToCarFaxModelAndSave(inventory.Vehicle.VehicleId, inventory.Vehicle.Vin, inventory.Dealer.Setting.CarFax, inventory.Dealer.Setting.CarFaxPassword);
                
                    return carfax;
                }
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
