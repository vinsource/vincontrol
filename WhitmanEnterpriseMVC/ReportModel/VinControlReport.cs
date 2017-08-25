using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.ReportModel
{
    public class VinControlReport
    {
        #region Public Methods

        public List<VinControlVehicleReport> GetVinControlUsedVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new whitmanenterprisewarehouseEntities();

            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                where e.NewUsed == "Used" && (e.Recon == false || e.Recon == null)
                select e;

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                var v = new VinControlVehicleReport
                {
                    AutoId = mProducts.Count + 1,
                    ListingId = tmp.ListingID,
                    ModelYear = tmp.ModelYear.GetValueOrDefault(),
                    Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                    Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                    Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                    StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                    Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                    Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                    Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                    Cylinder = String.IsNullOrEmpty(tmp.Cylinders) ? "" : tmp.Cylinders,
                    Tranmission = String.IsNullOrEmpty(tmp.Tranmission) ? "" : tmp.Tranmission,
                    ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                    SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                    DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                    DealershipName = String.IsNullOrEmpty(tmp.DealershipName) ? "" : tmp.DealershipName,
                    RetailPrice = String.IsNullOrEmpty(tmp.RetailPrice) ? "" : tmp.RetailPrice,
                    Style = String.IsNullOrEmpty(tmp.BodyType) ? "" : tmp.BodyType,
                };


                if (v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.CarImageUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                            !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl) &&
                            !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                            !tmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
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

        public List<VinControlVehicleReport> GetVinControlUsedVehiclesRange(int dealerId,int min, int? max)
        {
            long result = 0;
            return max.HasValue
                       ? GetVinControlUsedVehicles(dealerId).Where(
                           i => long.TryParse(i.SalePrice, out result) && result >= min && result <= max.Value).ToList()
                       : GetVinControlUsedVehicles(dealerId).Where(
                           i => long.TryParse(i.SalePrice, out result) && result >= min).ToList();
        }

        public List<VinControlVehicleReport> GetVinControlNewVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new whitmanenterprisewarehouseEntities();

            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                where e.NewUsed == "New" && (e.Recon == false || e.Recon == null)
                select e;

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            var dtDealerInfo = InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                var v = new VinControlVehicleReport
                {
                    AutoId = mProducts.Count + 1,
                    ModelYear = tmp.ModelYear.GetValueOrDefault(),
                    Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                    Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                    Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                    StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                    Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                    Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                    Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                    Cylinder = String.IsNullOrEmpty(tmp.Cylinders) ? "" : tmp.Cylinders,
                    Tranmission = String.IsNullOrEmpty(tmp.Tranmission) ? "" : tmp.Tranmission,
                    ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                    SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                    DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                    DealershipName =
                        String.IsNullOrEmpty(dtDealerInfo.FirstOrDefault(i => tmp.DealershipId == i.idWhitmanenterpriseDealership).DealershipName) ? "" : dtDealerInfo.FirstOrDefault(i => tmp.DealershipId == i.idWhitmanenterpriseDealership).DealershipName,
                    RetailPrice = String.IsNullOrEmpty(tmp.RetailPrice) ? "" : tmp.RetailPrice,
                    Style = String.IsNullOrEmpty(tmp.BodyType) ? "" : tmp.BodyType,
                };

                if (v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.CarImageUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                            !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl) &&
                            !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                            !tmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
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

            var context = new whitmanenterprisewarehouseEntities();

            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                  from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                  where e.Recon.Value
                  select e;

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            var dtDealerInfo = InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                var v = new VinControlVehicleReport
                {
                    AutoId = mProducts.Count + 1,
                    ModelYear = tmp.ModelYear.GetValueOrDefault(),
                    Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                    Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                    Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                    StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                    Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                    Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                    Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                    Cylinder = String.IsNullOrEmpty(tmp.Cylinders) ? "" : tmp.Cylinders,
                    Tranmission = String.IsNullOrEmpty(tmp.Tranmission) ? "" : tmp.Tranmission,
                    ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                    SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                    DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                    DealershipName =
                        String.IsNullOrEmpty(dtDealerInfo.FirstOrDefault(i => tmp.DealershipId == i.idWhitmanenterpriseDealership).DealershipName) ? "" : dtDealerInfo.FirstOrDefault(i => tmp.DealershipId == i.idWhitmanenterpriseDealership).DealershipName,
                    RetailPrice = String.IsNullOrEmpty(tmp.RetailPrice) ? "" : tmp.RetailPrice,
                    Style = String.IsNullOrEmpty(tmp.BodyType) ? "" : tmp.BodyType,
                };


                if (v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.CarImageUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                            !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl) &&
                            !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                            !tmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
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

            var context = new whitmanenterprisewarehouseEntities();

            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                    select e;

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            var dtDealerInfo = InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                int daysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.GetValueOrDefault()).Days;

                bool flag = ((daysInInvenotry == dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).FirstTimeRangeBucketJump) || (daysInInvenotry == dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).SecondTimeRangeBucketJump) || ((daysInInvenotry - dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).SecondTimeRangeBucketJump) % dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).IntervalBucketJump) == 0);

                if (flag)
                {
                    var v = new VinControlVehicleReport
                    {
                        AutoId = mProducts.Count + 1,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                        Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                        Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                        StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                        Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                        Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        DealershipName = String.IsNullOrEmpty(dtDealerInfo.FirstOrDefault(i => tmp.DealershipId == i.idWhitmanenterpriseDealership).DealershipName)
                                             ? ""
                                             : dtDealerInfo.FirstOrDefault(i => tmp.DealershipId == i.idWhitmanenterpriseDealership).DealershipName,
                        Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                    };


                    if (v.Engine.Contains("Engine"))
                    {
                        v.Engine = v.Engine.Replace("Engine", "");
                    }

                    if (String.IsNullOrEmpty(tmp.CarImageUrl))
                    {
                        v.Pics = "0";
                    }
                    else
                    {
                        string[] splitArray =
                            tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).
                                ToArray
                                ();

                        if (splitArray.Count() > 1)
                        {
                            v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                                !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl) &&
                                !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                                !tmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
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

            var context = new whitmanenterprisewarehouseEntities();

            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.NewUsed.ToLower().Equals("used") && (e.Recon == null || !((bool)e.Recon))
                    select e;

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            var dtDealerInfo = InventoryQueryHelper.GetSingleOrGroupDealer(context).ToList();

            for (int i = 1; i <= 7; i++)
            {
                foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
                {
                    int daysInInvenotry = DateTime.Now.AddDays(i).Subtract(tmp.DateInStock.GetValueOrDefault()).Days;

                    bool flag = dtDealerSetting.FirstOrDefault(item => tmp.DealershipId == item.DealershipId).IntervalBucketJump != 0 && ((daysInInvenotry == dtDealerSetting.FirstOrDefault(item => tmp.DealershipId == item.DealershipId).FirstTimeRangeBucketJump) || (daysInInvenotry == dtDealerSetting.FirstOrDefault(item => tmp.DealershipId == item.DealershipId).SecondTimeRangeBucketJump) || ((daysInInvenotry - dtDealerSetting.FirstOrDefault(item => tmp.DealershipId == item.DealershipId).SecondTimeRangeBucketJump) % dtDealerSetting.FirstOrDefault(item => tmp.DealershipId == item.DealershipId).IntervalBucketJump) == 0);

                    if (flag)
                    {
                        var v = new VinControlVehicleReport
                        {
                            AutoId = mProducts.Count + 1,
                            ModelYear = tmp.ModelYear.GetValueOrDefault(),
                            Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                            Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                            Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                            StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                            Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                            Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                            ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                            SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                            DealershipName = String.IsNullOrEmpty(dtDealerInfo.FirstOrDefault(item => tmp.DealershipId == item.idWhitmanenterpriseDealership).DealershipName)
                                                 ? ""
                                                 : dtDealerInfo.FirstOrDefault(item => tmp.DealershipId == item.idWhitmanenterpriseDealership).DealershipName,
                            Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                            Style = String.IsNullOrEmpty(tmp.BodyType) ? "" : tmp.BodyType,
                            Date = new DateTime(DateTime.Now.AddDays(i).Year, DateTime.Now.AddDays(i).Month, DateTime.Now.AddDays(i).Day)
                        };


                        if (v.Engine.Contains("Engine"))
                        {
                            v.Engine = v.Engine.Replace("Engine", "");
                        }


                        mProducts.Add(v);
                    }
                }
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetManheimInventoryVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();
            var context = new whitmanenterprisewarehouseEntities();

            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                where (e.Recon == false || e.Recon == null)
                select e;

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                if (dtDealerSetting != null)
                {
                    var manheimWholesales = LinqHelper.ManheimReport(tmp/*, dtDealerSetting.Manheim.Trim(), dtDealerSetting.ManheimPassword.Trim()*/);
                    if (manheimWholesales.Count > 0)
                    {
                        foreach (var item in manheimWholesales)
                        {
                            var v = new VinControlVehicleReport
                            {
                                AutoId = mProducts.Count + 1,
                                ListingId = tmp.ListingID,
                                ModelYear = tmp.ModelYear.GetValueOrDefault(),
                                Make = tmp.Make ?? string.Empty,
                                Model = tmp.Model ?? string.Empty,
                                Trim = tmp.Trim ?? string.Empty,
                                StockNumber = tmp.StockNumber ?? string.Empty,
                                Vin = tmp.VINNumber ?? string.Empty,
                                Mileage = tmp.Mileage ?? string.Empty,
                                Engine = tmp.EngineType ?? string.Empty,
                                Cylinder = tmp.Cylinders ?? string.Empty,
                                Tranmission = tmp.Tranmission ?? string.Empty,
                                ExteriorColor = tmp.ExteriorColor ?? string.Empty,
                                SalePrice = tmp.SalePrice ?? string.Empty,
                                DealerCost = tmp.DealerCost ?? string.Empty,
                                DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                                DealershipName = tmp.DealershipName ?? string.Empty,
                                RetailPrice = tmp.RetailPrice ?? string.Empty,
                                Style = tmp.BodyType ?? string.Empty,
                                ManheimLowestPrice = item.LowestPrice ?? string.Empty,
                                ManheimAveragePrice = item.AveragePrice ?? string.Empty,
                                ManheimHighestPrice = item.HighestPrice ?? string.Empty,
                                ManheimTrim = item.TrimName ?? string.Empty
                            };

                            if (v.Engine.Contains("Engine"))
                            {
                                v.Engine = v.Engine.Replace("Engine", "");
                            }

                            if (String.IsNullOrEmpty(tmp.CarImageUrl))
                            {
                                v.Pics = "0";
                            }
                            else
                            {
                                string[] splitArray = tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                                if (splitArray.Count() > 1)
                                {
                                    v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                                        !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl) &&
                                        !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                                        !tmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
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
                        var v = new VinControlVehicleReport
                        {
                            AutoId = mProducts.Count + 1,
                            ListingId = tmp.ListingID,
                            ModelYear = tmp.ModelYear.GetValueOrDefault(),
                            Make = tmp.Make ?? string.Empty,
                            Model = tmp.Model ?? string.Empty,
                            Trim = tmp.Trim ?? string.Empty,
                            StockNumber = tmp.StockNumber ?? string.Empty,
                            Vin = tmp.VINNumber ?? string.Empty,
                            Mileage = tmp.Mileage ?? string.Empty,
                            Engine = tmp.EngineType ?? string.Empty,
                            Cylinder = tmp.Cylinders ?? string.Empty,
                            Tranmission = tmp.Tranmission ?? string.Empty,
                            ExteriorColor = tmp.ExteriorColor ?? string.Empty,
                            SalePrice = tmp.SalePrice ?? string.Empty,
                            DealerCost = tmp.DealerCost ?? string.Empty,
                            DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                            DealershipName = tmp.DealershipName ?? string.Empty,
                            RetailPrice = tmp.RetailPrice ?? string.Empty,
                            Style = tmp.BodyType ?? string.Empty,
                            ManheimLowestPrice = string.Empty,
                            ManheimAveragePrice = string.Empty,
                            ManheimHighestPrice = string.Empty,
                            ManheimTrim = tmp.Trim ?? string.Empty
                        };

                        if (v.Engine.Contains("Engine"))
                        {
                            v.Engine = v.Engine.Replace("Engine", "");
                        }

                        if (String.IsNullOrEmpty(tmp.CarImageUrl))
                        {
                            v.Pics = "0";
                        }
                        else
                        {
                            string[] splitArray = tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                            if (splitArray.Count() > 1)
                            {
                                v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                                    !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl) &&
                                    !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                                    !tmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => tmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
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
            }

            return mProducts;
        }

        public List<VinControlVehicleReport> GetKarPowerVehicles(int dealerId)
        {
            var mProducts = new List<VinControlVehicleReport>();

            var context = new whitmanenterprisewarehouseEntities();

            var avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                from et in context.whitmanenterprisekbbs
                where
                    e.NewUsed.ToLower().Equals("used") && e.KBBTrimId > 0 &&
                    e.KBBTrimId == et.TrimId && e.VINNumber == et.Vin &&
                    (e.Recon ==
                     null ||
                     !((bool)
                       e.Recon))
                select new
                {
                    e.Make,
                    e.ModelYear,
                    e.Model,
                    e.StockNumber,
                    e.VINNumber,
                    e.Mileage,
                    e.ExteriorColor,
                    e.SalePrice,
                    e.DateInStock,
                    e.DealershipName,
                    e.KBBTrimId,
                    et.BaseWholeSale,
                    et.MileageAdjustment,
                    et.WholeSale,
                    e.EngineType
                };

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                var number = CommonHelper.RemoveSpecialCharactersAndReturnNumber(tmp.BaseWholeSale);

                if (number > 0)
                {
                    var v = new VinControlVehicleReport
                    {
                        AutoId = mProducts.Count + 1,
                        ModelYear = tmp.ModelYear.GetValueOrDefault(),
                        Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                        Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                        StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                        Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                        Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                        ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                        SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                        DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                        DealershipName = tmp.DealershipName,
                        BasewholeSale = tmp.BaseWholeSale,
                        MileageAdjustment = tmp.MileageAdjustment,
                        WholeSale = tmp.WholeSale,
                        Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                    };

                    if (v.Engine.Contains("Engine"))
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

            var context = new whitmanenterprisewarehouseEntities();


            IQueryable<whitmanenterprisedealershipinventory> avaiInventory =
                from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                where e.Certified.Value
                select e;

            var dtDealerSetting = context.whitmanenterprisesettings.FirstOrDefault(x => x.DealershipId == dealerId);

            var dtDealerInfo = context.whitmanenterprisedealerships.FirstOrDefault(x => x.idWhitmanenterpriseDealership == dealerId);

            foreach (var tmp in avaiInventory.OrderBy(x => x.Make))
            {
                var v = new VinControlVehicleReport
                {
                    AutoId = mProducts.Count + 1,
                    ModelYear = tmp.ModelYear.GetValueOrDefault(),
                    Make = String.IsNullOrEmpty(tmp.Make) ? "" : tmp.Make,
                    Model = String.IsNullOrEmpty(tmp.Model) ? "" : tmp.Model,
                    Trim = String.IsNullOrEmpty(tmp.Trim) ? "" : tmp.Trim,
                    StockNumber = String.IsNullOrEmpty(tmp.StockNumber) ? "" : tmp.StockNumber,
                    Vin = String.IsNullOrEmpty(tmp.VINNumber) ? "" : tmp.VINNumber,
                    Mileage = String.IsNullOrEmpty(tmp.Mileage) ? "" : tmp.Mileage,
                    ExteriorColor = String.IsNullOrEmpty(tmp.ExteriorColor) ? "" : tmp.ExteriorColor,
                    SalePrice = String.IsNullOrEmpty(tmp.SalePrice) ? "" : tmp.SalePrice,
                    DaysInInvenotry = DateTime.Now.Subtract(tmp.DateInStock.Value).Days,
                    DealershipName =
                        String.IsNullOrEmpty(dtDealerInfo.DealershipName) ? "" : dtDealerInfo.DealershipName,
                    Engine = String.IsNullOrEmpty(tmp.EngineType) ? "" : tmp.EngineType,
                };

                if (v.Engine.Contains("Engine"))
                {
                    v.Engine = v.Engine.Replace("Engine", "");
                }

                if (String.IsNullOrEmpty(tmp.CarImageUrl))
                {
                    v.Pics = "0";
                }
                else
                {
                    string[] splitArray =
                        tmp.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        v.Pics = splitArray.Count().ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tmp.DefaultImageUrl) &&
                            !String.IsNullOrEmpty(dtDealerSetting.DefaultStockImageUrl) &&
                            !tmp.CarImageUrl.Equals(tmp.DefaultImageUrl) &&
                            !tmp.CarImageUrl.Equals(dtDealerSetting.DefaultStockImageUrl))
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
            var context = new whitmanenterprisewarehouseEntities();
            var finalInventory = (context.vincontrolpricechangeshistories.Where(GetMonthQuery(month)).Join(InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(i => i.DealershipId == dealerId).Where(GetYearQuery(year)).Where(GetMakeQuery(make)).Where(GetModelQuery(model)).Where(GetStockQuery(stock)).Where(GetVinQuery(vin)), p => p.ListingId, i => i.ListingID,
                                                                (p, i) => new VehiclePriceChange()
                                                                {
                                                                    Make = i.Make,
                                                                    Model = i.Model,
                                                                    Month = p.DateStamp.Value.Month,
                                                                    OldPrice = (long)p.OldPrice.Value,
                                                                    Price = (long)p.NewPrice.Value,
                                                                    StockNumber = i.StockNumber,
                                                                    UpdatedDate = p.DateStamp.Value,
                                                                    User = p.UserStamp,
                                                                    VINNumber = i.VINNumber,
                                                                    DealershipName = i.DealershipName,
                                                                    Year = i.ModelYear ?? 0
                                                                })).OrderBy(x => x.Make).ThenBy(x => x.UpdatedDate).ToList();

            var soldInventory = (context.vincontrolpricechangeshistories.Where(GetMonthQuery(month)).Join(InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context).Where(i => i.DealershipId == dealerId).Where(GetYearQueryForSold(year)).Where(GetMakeQueryForSold(make)).Where(GetModelQueryForSold(model)).Where(GetStockQueryForSold(stock)).Where(GetVinQueryForSold(vin)), p => p.ListingId, i => i.OldListingId,
                                                              (p, i) => new VehiclePriceChange()
                                                              {
                                                                  Make = i.Make,
                                                                  Model = i.Model,
                                                                  Month = p.DateStamp.Value.Month,
                                                                  OldPrice = (long)p.OldPrice.Value,
                                                                  Price = (long)p.NewPrice.Value,
                                                                  StockNumber = i.StockNumber,
                                                                  UpdatedDate = p.DateStamp.Value,
                                                                  User = p.UserStamp,
                                                                  VINNumber = i.VINNumber,
                                                                  DealershipName = i.DealershipName,
                                                                  Year = i.ModelYear ?? 0
                                                              })).OrderBy(x => x.Make).ThenBy(x => x.UpdatedDate).ToList();

            finalInventory.AddRange(soldInventory);

            return finalInventory;

        }

        public List<KBBInfo> GetProfitManagement()
        {
            var context = new whitmanenterprisewarehouseEntities();
            var result =
                from i in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                join k in context.whitmanenterprisekbbs.Where(j => j.Type == Constanst.VehicleTable.Inventory)
                    on new { i.KBBTrimId, i.VINNumber } equals new { KBBTrimId = k.TrimId, VINNumber = k.Vin }
                into gj
                from g in gj.DefaultIfEmpty()
                where i.NewUsed=="Used"
                select new KBBInfo
                           {
                               VIN = i.VINNumber,
                               StockNumber=i.StockNumber,
                               DateInStock =i.DateInStock.Value,
                               Year = i.ModelYear ?? 0,
                               Make = i.Make,
                               Model = i.Model,
                               Trim = i.Trim,
                               WholeSale = g.WholeSale,
                               DealerCost =String.IsNullOrEmpty(i.DealerCost) ? "NA" : i.DealerCost,
                               DealershipName = i.DealershipName,
                               
                           };
            var sorted = result.ToList();
            foreach (var kbbInfo in sorted)
            {
                kbbInfo.DaysInInvenotry = DateTime.Now.Subtract(kbbInfo.DateInStock).Days;
                GetCost(kbbInfo);
            }


            return sorted.OrderByDescending(i => i.KBBCost).ThenByDescending(i=>i.Year).ThenBy(i=>i.Make).ThenBy(i=>i.Model).ToList();
        }

        private static void GetCost(KBBInfo kbbInfo)
        {
            try
            {
                var wholeSale = double.Parse(kbbInfo.WholeSale, NumberStyles.Currency);
                var dealerCost = (kbbInfo.DealerCost == null) ? 0 : double.Parse(kbbInfo.DealerCost);
                kbbInfo.WholeSale = wholeSale.ToString();
                kbbInfo.KBBCost = wholeSale - dealerCost;
            }
            catch (Exception)
            {
                kbbInfo.KBBCost = null;
            }
        }

        #endregion

        #region Private Methods

        private static System.Linq.Expressions.Expression<Func<vincontrolpricechangeshistory, bool>> GetMonthQuery(int month)
        {
            if (month != 0)
                return i => i.DateStamp.Value.Month == month;
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetYearQuery(int year)
        {
            if (year != 0)
                return i => i.ModelYear == year;
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetMakeQuery(string make)
        {
            if (!String.IsNullOrEmpty(make))
                return i => i.Make.Equals(make);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetModelQuery(string model)
        {
            if (!String.IsNullOrEmpty(model))
                return i => i.Model.Equals(model);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetStockQuery(string stock)
        {
            if (!String.IsNullOrEmpty(stock))
                return i => i.StockNumber.Contains(stock);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventory, bool>> GetVinQuery(string vin)
        {
            if (!String.IsNullOrEmpty(vin))
                return i => i.VINNumber.Contains(vin);

            return i => true;
        }


        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventorysoldout, bool>> GetYearQueryForSold(int year)
        {
            if (year != 0)
                return i => i.ModelYear == year;
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventorysoldout, bool>> GetMakeQueryForSold(string make)
        {
            if (!String.IsNullOrEmpty(make))
                return i => i.Make.Equals(make);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventorysoldout, bool>> GetModelQueryForSold(string model)
        {
            if (!String.IsNullOrEmpty(model))
                return i => i.Model.Equals(model);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventorysoldout, bool>> GetStockQueryForSold(string stock)
        {
            if (!String.IsNullOrEmpty(stock))
                return i => i.StockNumber.Contains(stock);
            return i => true;
        }

        private static System.Linq.Expressions.Expression<Func<whitmanenterprisedealershipinventorysoldout, bool>> GetVinQueryForSold(string vin)
        {
            if (!String.IsNullOrEmpty(vin))
                return i => i.VINNumber.Contains(vin);
            return i => true;
        }

        #endregion
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
        public string DealerCost { get; set; }
        public string WholeSale { get; set; }

        public string Cost
        {
            get { return !KBBCost.HasValue ? "NA" : KBBCost.Value.ToString(); }
        }

        public double? KBBCost { get; set; }
        public string DealershipName { get; set; }
        //public string CostInString
        //{
        //    get { return Cost == null ? "NA" : Cost.ToString(); }
        //}

    }

    public class VinControlVehicleReport
    {
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

        public string RetailPrice { get; set; }

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
        public long OldPrice { get; set; }
        public long Price { get; set; }
        public string StockNumber { get; set; }
        public string VINNumber { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string DealershipName { get; set; }
        public int Month { get; set; }
    }
}
