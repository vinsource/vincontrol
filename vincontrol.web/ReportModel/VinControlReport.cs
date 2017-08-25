using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.DatabaseModel;
using Vincontrol.Web.Models;
using PriceChangeHistory = vincontrol.Data.Model.PriceChangeHistory;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Helper;

namespace Vincontrol.Web.ReportModel
{
    public class VinControlReport
    {
        #region Public Methods

        public List<FlyerShareReportItem> GetSharedFlyersHistory()
        {
            using (var context = new VincontrolEntities())
            {
               return InventoryQueryHelper.GetSingleOrGroupDealerShareFlyerActivity(context).OrderByDescending(
                        i => i.DateStamp).ToList().Select(i => new FlyerShareReportItem(i)).ToList();

            }
        }

        public List<VinControlVehicleReport> GetVinControlUsedVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory =
                InventoryQueryHelper.GetSingleOrGroupInventory(context).Where((CommonHelper.IsInventoryPredicate())).Where(e => e.Condition == Constanst.ConditionStatus.Used);

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
            {
                var v = new VinControlVehicleReport(tmp)
                {
                    AutoId = mProducts.Count + 1
                };

                if (v.Engine != null && v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.PhotoUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                        if (firstOrDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !String.IsNullOrEmpty(firstOrDefault.DefaultStockImageUrl) &&
                                                                                                        !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !tmp.PhotoUrl.Equals(firstOrDefault.DefaultStockImageUrl)))
                        {
                            v.Pics = "1";
                        }
                        else
                        {
                            v.Pics = "1(D)";
                        }
                    }
                }



                mProducts.Add(v);
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetVinControlUsedVehiclesByAging(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory =
                InventoryQueryHelper.GetSingleOrGroupInventory(context).Where((CommonHelper.IsInventoryPredicate())).Where(e => e.Condition == Constanst.ConditionStatus.Used);

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            foreach (var tmp in avaiInventory)
            {
                var v = new VinControlVehicleReport(tmp)
                {
                    AutoId = mProducts.Count + 1
                };

                if (v.Engine != null && v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.PhotoUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                        if (firstOrDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !String.IsNullOrEmpty(firstOrDefault.DefaultStockImageUrl) &&
                                                                                                        !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !tmp.PhotoUrl.Equals(firstOrDefault.DefaultStockImageUrl)))
                        {
                            v.Pics = "1";
                        }
                        else
                        {
                            v.Pics = "1(D)";
                        }
                    }
                }



                mProducts.Add(v);
            }

            return mProducts.OrderByDescending(x=>x.DaysInInvenotry).ToList();
        }

        public List<VinControlVehicleReport> GetVinControlUsedVehiclesRange(int dealerId, int min, int? max)
        {
            long result;
            return max.HasValue
                       ? GetVinControlUsedVehicles(dealerId).Where(
                           i => long.TryParse(i.SalePrice, out result) && result >= min && result <= max.Value).ToList()
                       : GetVinControlUsedVehicles(dealerId).Where(
                           i => long.TryParse(i.SalePrice, out result) && result >= min).ToList();
        }

        public List<VinControlVehicleReport> GetVinControlNewVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory =
                InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(
                    e => e.Condition == Constanst.ConditionStatus.New);

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            //InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
            {
                var v = new VinControlVehicleReport(tmp)
                {
                    AutoId = mProducts.Count + 1,
                };

                if (v.Engine != null && v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.PhotoUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                        if (firstOrDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !String.IsNullOrEmpty(firstOrDefault.DefaultStockImageUrl) &&
                                                                                                        !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !tmp.PhotoUrl.Equals(firstOrDefault.DefaultStockImageUrl)))
                        {
                            v.Pics = "1";
                        }
                        else
                        {
                            v.Pics = "1(D)";
                        }
                    }
                }



                mProducts.Add(v);
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetVinControlReconVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsReconPredicate());

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            //InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
            {
                var v = new VinControlVehicleReport(tmp)
                {
                    AutoId = mProducts.Count + 1,
                };


                if (v.Engine != null && v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.PhotoUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                        if (firstOrDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !String.IsNullOrEmpty(firstOrDefault.DefaultStockImageUrl) &&
                                                                                                        !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                        !tmp.PhotoUrl.Equals(firstOrDefault.DefaultStockImageUrl)))
                        {
                            v.Pics = "1";
                        }
                        else
                        {
                            v.Pics = "1(D)";
                        }
                    }
                }



                mProducts.Add(v);
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetTodayBucketJumpVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory =
                InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(
                    e => e.Condition == Constanst.ConditionStatus.Used);

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            //InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
            {
                int daysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;

                var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                bool flag = firstOrDefault != null && ((daysInInvenotry == firstOrDefault.FirstTimeRangeBucketJump) || (daysInInvenotry == firstOrDefault.SecondTimeRangeBucketJump) || ((daysInInvenotry - firstOrDefault.SecondTimeRangeBucketJump) % firstOrDefault.IntervalBucketJump) == 0);

                if (flag)
                {
                    var v = new VinControlVehicleReport(tmp)
                    {
                        AutoId = mProducts.Count + 1
                    };


                    if (v.Engine != null && v.Engine.Contains("Engine"))
                    {
                        v.Engine = v.Engine.Replace("Engine", "");
                    }

                    if (String.IsNullOrEmpty(tmp.PhotoUrl))
                    {
                        v.Pics = "0";
                    }
                    else
                    {
                        string[] splitArray =
                            tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).
                                ToArray
                                ();

                        if (splitArray.Count() > 1)
                        {
                            v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var orDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                            if (orDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                            !String.IsNullOrEmpty(orDefault.DefaultStockImageUrl) &&
                                                                                                            !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                            !tmp.PhotoUrl.Equals(orDefault.DefaultStockImageUrl)))
                            {
                                v.Pics = "1";
                            }
                            else
                            {
                                v.Pics = "1(D)";
                            }
                        }
                    }



                    mProducts.Add(v);
                }
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetNext7DaysBucketJumpVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory =
                InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(
                    e => e.Condition == Constanst.ConditionStatus.Used);

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            //InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            for (int i = 1; i <= 7; i++)
            {
                foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
                {
                    int daysInInvenotry = DateTime.Now.AddDays(i).Subtract(tmp.DateInStock.GetValueOrDefault()).Days;

                    var firstOrDefault = dtDealerSetting.FirstOrDefault(item => tmp.DealerId == item.DealerId);
                    bool flag = firstOrDefault != null && (firstOrDefault.IntervalBucketJump != 0 && ((daysInInvenotry == firstOrDefault.FirstTimeRangeBucketJump) || (daysInInvenotry == firstOrDefault.SecondTimeRangeBucketJump) || ((daysInInvenotry - firstOrDefault.SecondTimeRangeBucketJump) % firstOrDefault.IntervalBucketJump) == 0));

                    if (flag)
                    {
                        var v = new VinControlVehicleReport(tmp)
                        {
                            AutoId = mProducts.Count + 1,
                            Date = new DateTime(DateTime.Now.AddDays(i).Year, DateTime.Now.AddDays(i).Month, DateTime.Now.AddDays(i).Day)
                        };

                        mProducts.Add(v);
                    }
                }
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetManheimInventoryVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();
            var context = new VincontrolEntities();

            IQueryable<Inventory> avaiInventory = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate());

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
            {

                var manheimWholesales = VincontrolLinqHelper.ManheimReport(tmp/*, dtDealerSetting.Manheim.Trim(), dtDealerSetting.ManheimPassword.Trim()*/);
                if (manheimWholesales.Count > 0)
                {
                    foreach (var item in manheimWholesales)
                    {
                        var v = new VinControlVehicleReport(tmp, item)
                            {
                                AutoId = mProducts.Count + 1,
                            };

                        if (v.Engine != null && v.Engine.Contains("Engine"))
                        {
                            v.Engine = v.Engine.Replace("Engine", "");
                        }

                        if (String.IsNullOrEmpty(tmp.PhotoUrl))
                        {
                            v.Pics = "0";
                        }
                        else
                        {
                            string[] splitArray = tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                            if (splitArray.Count() > 1)
                            {
                                v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                                if (firstOrDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                                !String.IsNullOrEmpty(firstOrDefault.DefaultStockImageUrl) &&
                                                                                                                !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                                !tmp.PhotoUrl.Equals(firstOrDefault.DefaultStockImageUrl)))
                                {
                                    v.Pics = "1";
                                }
                                else
                                {
                                    v.Pics = "1(D)";
                                }
                            }
                        }

                        mProducts.Add(v);
                    }
                }
                else
                {
                    var v = new VinControlVehicleReport(tmp)
                        {
                            AutoId = mProducts.Count + 1,
                            ManheimTrim = tmp.Vehicle.Trim ?? string.Empty
                        };

                    if (v.Engine != null && v.Engine.Contains("Engine"))
                    {
                        v.Engine = v.Engine.Replace("Engine", "");
                    }

                    if (String.IsNullOrEmpty(tmp.PhotoUrl))
                    {
                        v.Pics = "0";
                    }
                    else
                    {
                        string[] splitArray = tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                        if (splitArray.Count() > 1)
                        {
                            v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var firstOrDefault = dtDealerSetting.FirstOrDefault(i => tmp.DealerId == i.DealerId);
                            if (firstOrDefault != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                                                                            !String.IsNullOrEmpty(firstOrDefault.DefaultStockImageUrl) &&
                                                                                                            !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                                                                            !tmp.PhotoUrl.Equals(firstOrDefault.DefaultStockImageUrl)))
                            {
                                v.Pics = "1";
                            }
                            else
                            {
                                v.Pics = "1(D)";
                            }
                        }
                    }

                    mProducts.Add(v);
                }
            }


            return mProducts;
        }

        public List<VinControlVehicleReport> GetKarPowerVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();

            var avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate())
                from et in context.KellyBlueBooks
                where
                    e.Condition == Constanst.ConditionStatus.Used && e.Vehicle.KBBTrimId > 0 &&
                    e.Vehicle.KBBTrimId == et.TrimId && e.Vehicle.Vin == et.Vin &&
                    (e.InventoryStatusCodeId != Constanst.InventoryStatus.Recon)
                select new
                {
                    e.Vehicle.Make,
                    e.Vehicle.Year,
                    e.Vehicle.Model,
                    e.Stock,
                    e.Vehicle.Vin,
                    e.Mileage,
                    e.ExteriorColor,
                    e.SalePrice,
                    e.DateInStock,
                    e.Dealer.Name,
                    e.Vehicle.KBBTrimId,
                    et.BaseWholeSale,
                    et.MileageAdjustment,
                    et.WholeSale,
                    e.Vehicle.EngineType
                };

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                var number = (tmp.BaseWholeSale);

                if (number > 0)
                {
                    var v = new VinControlVehicleReport
                    {
                        AutoId = mProducts.Count + 1,
                        ModelYear = tmp.Year.GetValueOrDefault(),
                        Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                        Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                        StockNumber = String.IsNullOrEmpty(tmp.Stock) ? "" : tmp.Stock,
                        Vin = String.IsNullOrEmpty(tmp.Vin) ? "" : tmp.Vin,
                        Mileage = tmp.Mileage.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        SalePrice = tmp.SalePrice.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        DealershipName = tmp.Name,
                        BasewholeSale = tmp.BaseWholeSale.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                        MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                        WholeSale = tmp.WholeSale.GetValueOrDefault().ToString(CultureInfo.InvariantCulture),
                        Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                    };

                    if (v.Engine != null && v.Engine.Contains("Engine"))
                    {
                        v.Engine = v.Engine.Replace("Engine", "");
                    }

                    mProducts.Add(v);
                }
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetCertifiedVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new VincontrolEntities();


            IQueryable<Inventory> avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate())
                where e.Certified.Value
                select e;

            var dtDealerSetting = context.Settings.FirstOrDefault(x => x.DealerId == dealerId);

            var dtDealerInfo = context.Dealers.FirstOrDefault(x => x.DealerId == dealerId);

            foreach (var tmp in avaiInventory.OrderBy(x => x.Vehicle.Make))
            {
                var v = new VinControlVehicleReport(tmp)
                {
                    AutoId = mProducts.Count + 1,
                    DealershipName = dtDealerInfo != null && !String.IsNullOrEmpty(dtDealerInfo.Name) ? dtDealerInfo.Name : String.Empty,
                };

                if (v.Engine != null && v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.PhotoUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.PhotoUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (dtDealerSetting != null && (!String.IsNullOrEmpty(tmp.Vehicle.DefaultStockImage) &&
                                                        !String.IsNullOrEmpty(dtDealerSetting.DefaultStockImageUrl) &&
                                                        !tmp.PhotoUrl.Equals(tmp.Vehicle.DefaultStockImage) &&
                                                        !tmp.PhotoUrl.Equals(dtDealerSetting.DefaultStockImageUrl)))
                        {
                            v.Pics = "1";
                        }
                        else
                        {
                            v.Pics = "1(D)";
                        }
                    }
                }



                mProducts.Add(v);
            }

            return mProducts;
        }

        public List<VehiclePriceChange> GetHistoryChanged(int dealerId, int month, int year, string make, string model, string stock, string vin)
        {
            var context = new VincontrolEntities();
            var finalInventory = (context.PriceChangeHistories.Where(GetMonthQuery(month)).Join(InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(i => i.DealerId == dealerId).Where(GetYearQuery(year)).Where(GetMakeQuery(make)).Where(GetModelQuery(model)).Where(GetStockQuery(stock)).Where(GetVinQuery(vin)), p => p.ListingId, i => i.InventoryId,
                                                                (p, i) => new VehiclePriceChange
                                                                    {
                                                                        Make = i.Vehicle.Make,
                                                                        Model = i.Vehicle.Model,
                                                                        Month = p.DateStamp.Value.Month,
                                                                        OldPrice = p.OldPrice.Value,
                                                                        Price = p.NewPrice.Value,
                                                                        StockNumber = i.Stock,
                                                                        UpdatedDate = p.DateStamp.Value,
                                                                        User = p.UserStamp,
                                                                        VINNumber = i.Vehicle.Vin,
                                                                        DealershipName = i.Dealer.Name,
                                                                        Year = i.Vehicle.Year ?? 0
                                                                    })).OrderBy(x => x.Make).ThenBy(x => x.UpdatedDate).ToList();

            var soldInventory = (context.PriceChangeHistories.Where(GetMonthQuery(month)).Join(InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context).Where(i => i.DealerId == dealerId).Where(GetYearQueryForSold(year)).Where(GetMakeQueryForSold(make)).Where(GetModelQueryForSold(model)).Where(GetStockQueryForSold(stock)).Where(GetVinQueryForSold(vin)), p => p.ListingId, i => i.SoldoutInventoryId,
                                                              (p, i) => new VehiclePriceChange
                                                                  {
                                                                      Make = i.Vehicle.Make,
                                                                      Model = i.Vehicle.Model,
                                                                      Month = p.DateStamp.Value.Month,
                                                                      OldPrice = p.OldPrice.Value,
                                                                      Price = p.NewPrice.Value,
                                                                      StockNumber = i.Stock,
                                                                      UpdatedDate = p.DateStamp.Value,
                                                                      User = p.UserStamp,
                                                                      VINNumber = i.Vehicle.Vin,
                                                                      DealershipName = i.Dealer.Name,
                                                                      Year = i.Vehicle.Year ?? 0
                                                                  })).OrderBy(x => x.Make).ThenBy(x => x.UpdatedDate).ToList();

            finalInventory.AddRange(soldInventory);

            return finalInventory;

        }

        public List<KBBInfo> GetProfitManagement()
        {
            var context = new VincontrolEntities();
            var result =
                from i in InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate()).Where(e => e.Condition == Constanst.ConditionStatus.Used)
                join k in context.KellyBlueBooks.Where(j => j.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory)
                on new { i.Vehicle.KBBTrimId, i.Vehicle.Vin } equals new { KBBTrimId = k.TrimId, k.Vin }
                into gj
                from g in gj.DefaultIfEmpty()
                select new KBBInfo
                           {
                               VIN = i.Vehicle.Vin,
                               StockNumber = i.Stock,
                               DateInStock = i.DateInStock.Value,
                               Year = i.Vehicle.Year ?? 0,
                               Make = i.Vehicle.Make,
                               Model = i.Vehicle.Model,
                               Trim = i.Vehicle.Trim,
                               WholeSale = g.WholeSale ?? 0,
                               DealerCost = i.DealerCost ?? 0,
                               DealershipName = i.Dealer.Name,

                           };
            var sorted = result.ToList();
            foreach (var kbbInfo in sorted)
            {
                kbbInfo.DaysInInvenotry = DateTime.Now.Subtract(kbbInfo.DateInStock).Days;
                kbbInfo.KBBCost = kbbInfo.WholeSale - kbbInfo.DealerCost;
            }

            return sorted.OrderByDescending(i => i.KBBCost).ThenByDescending(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ToList();
        }

        #endregion

        #region Private Methods

        private static System.Linq.Expressions.Expression<Func<PriceChangeHistory, bool>> GetMonthQuery(int month)
        {
            if (month != 0)
                return i => i.DateStamp.Value.Month == month;
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<Inventory, bool>> GetYearQuery(int year)
        {
            if (year != 0)
                return i => i.Vehicle.Year == year;
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<Inventory, bool>> GetMakeQuery(string make)
        {
            if (!String.IsNullOrEmpty(make))
                return i => i.Vehicle.Make.Equals(make);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<Inventory, bool>> GetModelQuery(string model)
        {
            if (!String.IsNullOrEmpty(model))
                return i => i.Vehicle.Model.Equals(model);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<Inventory, bool>> GetStockQuery(string stock)
        {
            if (!String.IsNullOrEmpty(stock))
                return i => i.Stock.Contains(stock);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<Inventory, bool>> GetVinQuery(string vin)
        {
            if (!String.IsNullOrEmpty(vin))
                return i => i.Vehicle.Vin.Contains(vin);

            return i => true;
        }


        private static System.Linq.Expressions.Expression<Func<SoldoutInventory, bool>> GetYearQueryForSold(int year)
        {
            if (year != 0)
                return i => i.Vehicle.Year == year;
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<SoldoutInventory, bool>> GetMakeQueryForSold(string make)
        {
            if (!String.IsNullOrEmpty(make))
                return i => i.Vehicle.Make.Equals(make);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<SoldoutInventory, bool>> GetModelQueryForSold(string model)
        {
            if (!String.IsNullOrEmpty(model))
                return i => i.Vehicle.Model.Equals(model);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<SoldoutInventory, bool>> GetStockQueryForSold(string stock)
        {
            if (!String.IsNullOrEmpty(stock))
                return i => i.Stock.Contains(stock);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<SoldoutInventory, bool>> GetVinQueryForSold(string vin)
        {
            if (!String.IsNullOrEmpty(vin))
                return i => i.Vehicle.Vin.Contains(vin);
            return i => true;
        }

        #endregion
    }

    public class FlyerShareReportItem
    {
        public FlyerShareReportItem(FlyerShareDealerActivity flyerShareDealerActivity)
        {
            CustomerEmail = flyerShareDealerActivity.CustomerEmail;
            CustomerName = flyerShareDealerActivity.CustomerName;
            UserName = flyerShareDealerActivity.User.UserName;
            DateStamp = flyerShareDealerActivity.DateStamp;
            DealershipName = SessionHandler.Dealer.DealershipName;
        }

        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime DateStamp { get; set; }
        public string DealershipName { get; set; }
    }

    public class KBBInfo
    {
        public string VIN { get; set; }
        public string StockNumber { get; set; }
        public int DaysInInvenotry { get; set; }
        public DateTime DateInStock { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public decimal DealerCost { get; set; }
        public decimal WholeSale { get; set; }

        public string Cost
        {
            get { return !KBBCost.HasValue ? "NA" : KBBCost.Value.ToString(CultureInfo.InvariantCulture); }
        }

        public decimal? KBBCost { get; set; }
        public string DealershipName { get; set; }
        //public string CostInString
        //{
        //    get { return Cost == null ? "NA" : Cost.ToString(); }
        //}

    }

    public class VinControlVehicleReport
    {
        public VinControlVehicleReport() { }

        public VinControlVehicleReport(Inventory tmp)
        {
            ListingId = tmp.InventoryId;
            ModelYear = tmp.Vehicle.Year.GetValueOrDefault();
            Make = tmp.Vehicle.Make;
            Model = tmp.Vehicle.Model;
            Trim = tmp.Vehicle.Trim;
            StockNumber = tmp.Stock;
            Vin = tmp.Vehicle.Vin;
            Mileage = tmp.Mileage.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
            Engine = tmp.Vehicle.EngineType;
            Cylinder = tmp.Vehicle.Cylinders.ToString();
            Tranmission = tmp.Vehicle.Tranmission;
            ExteriorColor = tmp.ExteriorColor;
            SalePrice = tmp.SalePrice.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
            if (tmp.DateInStock != null) DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days;
            DealershipName = SessionHandler.Dealer.DealershipName;
            RetailPrice = tmp.RetailPrice.GetValueOrDefault();
            Style = tmp.Vehicle.BodyType;
            DealerCost = tmp.DealerCost == null ? "0" : tmp.DealerCost.ToString();
        }

        public VinControlVehicleReport(ManheimWholesaleViewModel tmp)
        {
            ModelYear = tmp.Year;
        }

        public VinControlVehicleReport(Inventory tmp, ManheimWholesaleViewModel item)
            : this(tmp)
        {
            ManheimTrim = item.TrimName ?? string.Empty;
        }

        public int AutoId { get; set; }

        public int ListingId { get; set; }

        public int ModelYear { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string StockNumber { get; set; }

        public string Vin { get; set; }

        public string Mileage { get; set; }

        public string Engine { get; set; }

        public string Cylinder { get; set; }

        public string Tranmission { get; set; }

        public string ExteriorColor { get; set; }

        public string SalePrice { get; set; }

        public string DealerCost { get; set; }

        public int DaysInInvenotry { get; set; }

        public string Pics { get; set; }

        public string DealershipName { get; set; }

        public string BasewholeSale { get; set; }

        public string MileageAdjustment { get; set; }

        public string WholeSale { get; set; }

        public int NumberofCars { get; set; }

        public decimal RetailPrice { get; set; }

        public string Style { get; set; }

        public DateTime Date { get; set; }

        //public List<ManheimWholesaleViewModel> ManheimWholesales { get; set; }
        public string ManheimLowestPrice { get; set; }

        public string ManheimAveragePrice { get; set; }

        public string ManheimHighestPrice { get; set; }

        public string ManheimTrim { get; set; }
    }

    public class VehiclePriceChange
    {
        public string User { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public string StockNumber { get; set; }
        public string VINNumber { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string DealershipName { get; set; }
        public int Month { get; set; }
    }
}
