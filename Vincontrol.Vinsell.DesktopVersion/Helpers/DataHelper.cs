using System;
using System.Collections.Generic;
using System.Globalization;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.Manheim;
using KarPowerService = vincontrol.KBB.KBBService;

namespace Vincontrol.Vinsell.DesktopVersion.Helpers
{
    public static class DataHelper
    {
        public static ChartGraph MarketData(string vin, IAuctionManagement manheimAuctionManagement)
        {
            try
            {
                var user = new UserViewModel();
                return MarketDataHelper.GetMarketCarsOnAutoTraderVersion2(manheimAuctionManagement.GetVehicle(vin), user);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<ManheimWholesaleViewModel> ManheimData(string vin, string manheimUsername, string manheimPassword, IAuctionManagement manheimAuctionManagement)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(vin))
                {
                    var model = manheimAuctionManagement.GetVehicle(vin);

                    if (model.Mmr > 0 && model.MmrAbove > 0 && model.MmrBelow > 0)
                    {
                        var newRecord = new ManheimWholesaleViewModel()
                                            {
                                                LowestPrice = (model.MmrBelow),
                                                AveragePrice = (model.Mmr),
                                                HighestPrice = (model.MmrAbove),
                                                Year = model.Year,
                                                TrimName = model.Trim
                                            };
                        result.Add(newRecord);
                    }
                    else
                    {
                        //manheimService.ExecuteByVin(SessionHandler.User.Setting.Manheim, SessionHandler.User.Setting.ManheimPassword, vin.Trim());
                        var manheimService = new ManheimService();
                        result = manheimService.ManheimReport(model, manheimUsername, manheimPassword);
                    }
                }
                else
                {
                    result = new List<ManheimWholesaleViewModel>();
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return result;
        }

        public static List<SmallKarPowerViewModel> KarpowerData(string vin, string mileage, string kellyBlueBook, string kellyPassword, int dealerId)
        {
            List<SmallKarPowerViewModel> result;
            try
            {
                result = LinqHelper.GetKbbReport(vin);
                if (result == null)
                {
                    var karpowerService = new KarPowerService();
                    result = karpowerService.Execute(vin, mileage, DateTime.Now, kellyBlueBook, kellyPassword, Constanst.VehicleStatus.Inventory);
                }

                var savedKbbTrim = LinqHelper.GetSavedKbbTrim(vin, dealerId);
                if (savedKbbTrim != null)
                {
                    foreach (var smallKarPowerViewModel in result)
                    {
                        smallKarPowerViewModel.IsSelected = smallKarPowerViewModel.SelectedTrimId == savedKbbTrim.TrimId;
                    }
                }
            }
            catch (Exception)
            {
                result = new List<SmallKarPowerViewModel>();
            }

            return result;
        }
    }
}