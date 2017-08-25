using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.Models;
using Appraisal = vincontrol.Data.Model.Appraisal;
using Inventory = vincontrol.Data.Model.Inventory;
using ManheimTrim = vincontrol.Data.Model.ManheimTrim;
using SoldoutInventory = vincontrol.Data.Model.SoldoutInventory;

using CommonHelper = vincontrol.Helper.CommonHelper;
using vincontrol.Manheim;
using Setting = vincontrol.Data.Model.Setting;

namespace Vincontrol.Web.HelperClass
{
    public static class VincontrolLinqHelper
    {
       
      
        public static bool CanViewBucketJumpReport(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var allowedDealer =
                    context.Dealers.FirstOrDefault(i => i.DealerId == dealerId && i.DealerGroupId.ToLower().Equals("g1010"));

                return allowedDealer != null;
            }
        }

        public static List<ManheimWholesaleViewModel> ManheimReport(Inventory inventory)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            
            var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (context.ManheimValues.Any(x => x.Vin == inventory.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory))
            {
                var searchResult = context.ManheimValues.Where(x => x.Vin == inventory.Vehicle.Vin
                    && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory
                    && x.DateAdded > dtCompareToday);
                result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                    {
                        LowestPrice = Convert.ToInt32(tmp.AuctionLowestPrice.GetValueOrDefault()),
                        AveragePrice = Convert.ToInt32(tmp.AuctionAveragePrice),
                        HighestPrice = Convert.ToInt32(tmp.AuctionHighestPrice),
                        Year = tmp.Year.GetValueOrDefault(),
                        MakeServiceId = tmp.MakeServiceId ?? 0,
                        ModelServiceId = tmp.ModelServiceId ?? 0,
                        TrimServiceId = tmp.TrimServiceId ?? 0,
                        TrimName = tmp.Trim
                    }));
            }
           

            return result;
        }



        public static List<ManheimWholesaleViewModel> ManheimReport(Inventory inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {
                if (context.ManheimValues.Any(x => x.Vin == inventory.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory))
                {
                    var searchResult = context.ManheimValues.Where(x => x.Vin == inventory.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory).ToList();
                    if (searchResult.Count > 0)
                    {
                        result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                            {
                                LowestPrice = Convert.ToInt32(tmp.AuctionLowestPrice.GetValueOrDefault()),
                                AveragePrice = Convert.ToInt32(tmp.AuctionAveragePrice.GetValueOrDefault()),
                                HighestPrice = Convert.ToInt32(tmp.AuctionHighestPrice.GetValueOrDefault()),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            }));
                    }
                    else
                    {
                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(inventory.Vehicle.Vin))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.Vehicle.Vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue
                                    {
                                        AuctionLowestPrice = tmp.LowestPrice,
                                        AuctionAveragePrice = tmp.AveragePrice,
                                        AuctionHighestPrice = tmp.HighestPrice,
                                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = CommonHelper.GetNextFriday(),
                                        Vin = inventory.Vehicle.Vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = Constanst.VehicleStatus.Inventory
                                    };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                            var matchingTrims = new List<ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", inventory.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModel.ToString(CultureInfo.InvariantCulture),
                                                       trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                                      manheimService.ManheimWholesale.AveragePrice == 0 ||
                                      manheimService.ManheimWholesale.HighestPrice == 0) &&
                                    manheimService.ManheimWholesale.LowestPrice > 0)
                                {
                                    var subResult = new ManheimWholesaleViewModel
                                        {
                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                            Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var manheimService = new ManheimService();
                    if (!string.IsNullOrEmpty(inventory.Vehicle.Vin))
                    {
                        manheimService.ExecuteByVin(userName, password, inventory.Vehicle.Vin.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                                {
                                    AuctionLowestPrice = tmp.LowestPrice,
                                    AuctionAveragePrice = tmp.AveragePrice,
                                    AuctionHighestPrice = tmp.HighestPrice,
                                    Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = CommonHelper.GetNextFriday(),
                                    Vin = inventory.Vehicle.Vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = Constanst.VehicleStatus.Inventory
                                };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", inventory.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                               manheimService.ManheimWholesale.AveragePrice == 0 ||
                               manheimService.ManheimWholesale.HighestPrice == 0) &&
                               manheimService.ManheimWholesale.LowestPrice > 0)
                            {
                                var subResult = new ManheimWholesaleViewModel
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };

                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForSoldCars(SoldoutInventory inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {
                if (context.ManheimValues.Any(x => x.Vin == inventory.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut))
                {
                    var searchResult = context.ManheimValues.Where(x => x.Vin == inventory.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut).ToList();
                    if (searchResult.Count > 0)
                    {
                        result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                            {
                                LowestPrice = Convert.ToInt32(tmp.AuctionLowestPrice.GetValueOrDefault()),
                                AveragePrice = Convert.ToInt32(tmp.AuctionAveragePrice.GetValueOrDefault()),
                                HighestPrice = Convert.ToInt32(tmp.AuctionHighestPrice.GetValueOrDefault()),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            }));
                    }
                    else
                    {


                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(inventory.Vehicle.Vin))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.Vehicle.Vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue
                                    {
                                        AuctionLowestPrice = tmp.LowestPrice,
                                        AuctionAveragePrice = tmp.AveragePrice,
                                        AuctionHighestPrice = tmp.HighestPrice,
                                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = CommonHelper.GetNextFriday(),
                                        Vin = inventory.Vehicle.Vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut
                                    };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                            var matchingTrims = new List<ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", inventory.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModel.ToString(CultureInfo.InvariantCulture),
                                                       trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                                      manheimService.ManheimWholesale.AveragePrice == 0 ||
                                      manheimService.ManheimWholesale.HighestPrice == 0) &&
                                    manheimService.ManheimWholesale.LowestPrice > 0)
                                {
                                    var subResult = new ManheimWholesaleViewModel
                                        {
                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                            Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var manheimService = new ManheimService();
                    if (!string.IsNullOrEmpty(inventory.Vehicle.Vin))
                    {
                        manheimService.ExecuteByVin(userName, password, inventory.Vehicle.Vin.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                                {
                                    AuctionLowestPrice = tmp.LowestPrice,
                                    AuctionAveragePrice = tmp.AveragePrice,
                                    AuctionHighestPrice = tmp.HighestPrice,
                                    Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = CommonHelper.GetNextFriday(),
                                    Vin = inventory.Vehicle.Vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut
                                };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            if (inventory.Vehicle.Year != null)
                                manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", inventory.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                               manheimService.ManheimWholesale.AveragePrice == 0 ||
                               manheimService.ManheimWholesale.HighestPrice == 0) &&
                               manheimService.ManheimWholesale.LowestPrice > 0)
                            {
                                var subResult = new ManheimWholesaleViewModel
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };

                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForWholesale(Inventory wholesale)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();

                if (!string.IsNullOrEmpty(wholesale.Vehicle.Vin.Trim()))
                {
                    manheimService.Execute(wholesale.Vehicle.Vin.Trim());
                    result = manheimService.ManheimWholesales;
                }
                else
                {
                    var matchingMake = manheimService.MatchingMake(wholesale.Vehicle.Make);
                    var matchingModel = 0;
                    var matchingModels = manheimService.MatchingModels(wholesale.Vehicle.Model, matchingMake);
                    var matchingTrims = new List<ManheimTrim>();

                    foreach (var model in matchingModels)
                    {
                        matchingTrims = manheimService.MatchingTrimsByModelId(model);
                        if (matchingTrims.Count > 0)
                        {
                            matchingModel = model;
                            break;
                        }
                    }

                    // don't have trims in database
                    if (matchingTrims.Count == 0)
                    {
                        if (wholesale.Vehicle.Year != null)
                            manheimService.GetTrim(wholesale.Vehicle.Year.Value.ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels);
                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }
                    }

                    foreach (var trim in matchingTrims)
                    {
                        manheimService.Execute("US", wholesale.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA");
                        if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                           manheimService.ManheimWholesale.AveragePrice == 0 ||
                           manheimService.ManheimWholesale.HighestPrice == 0))
                        {
                            var subResult = new ManheimWholesaleViewModel
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = wholesale.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.ServiceId,
                                    TrimName = trim.Name
                                };
                            result.Add(subResult);
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForWholesale(Inventory wholesale, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {

                if (context.ManheimValues.Any(x => x.Vin == wholesale.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory))
                {
                    var searchResult = context.ManheimValues.Where(x => x.Vin == wholesale.Vehicle.Vin
                        && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory).ToList();
                    if (searchResult.Count > 0)
                    {
                        result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                            {
                                LowestPrice = Convert.ToInt32((tmp.AuctionLowestPrice.GetValueOrDefault())),
                                AveragePrice = Convert.ToInt32((tmp.AuctionAveragePrice.GetValueOrDefault())),
                                HighestPrice = Convert.ToInt32((tmp.AuctionHighestPrice.GetValueOrDefault())),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            }));
                    }
                    else
                    {
                        foreach (var vincontrolmanheimvalue in searchResult)
                        {
                            context.Attach(vincontrolmanheimvalue);
                            context.DeleteObject(vincontrolmanheimvalue);
                        }
                        context.SaveChanges();

                        var manheimService = new ManheimService();

                        if (!string.IsNullOrEmpty(wholesale.Vehicle.Vin))
                        {
                            manheimService.ExecuteByVin(userName, password, wholesale.Vehicle.Vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue
                                    {
                                        AuctionLowestPrice = (tmp.LowestPrice),
                                        AuctionAveragePrice = (tmp.AveragePrice),
                                        AuctionHighestPrice = (tmp.HighestPrice),
                                        Year = wholesale.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = CommonHelper.GetNextFriday(),
                                        Vin = wholesale.Vehicle.Vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = Constanst.VehicleStatus.Inventory
                                    };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(wholesale.Vehicle.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(wholesale.Vehicle.Model, matchingMake);
                            var matchingTrims = new List<ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(wholesale.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", wholesale.Vehicle.Year.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModel.ToString(CultureInfo.InvariantCulture),
                                                       trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                                      manheimService.ManheimWholesale.AveragePrice == 0 ||
                                      manheimService.ManheimWholesale.HighestPrice == 0) &&
                                    (manheimService.ManheimWholesale.LowestPrice > 0))
                                {
                                    var subResult = new ManheimWholesaleViewModel
                                        {
                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                            Year = wholesale.Vehicle.Year.GetValueOrDefault(),
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var manheimService = new ManheimService();
                    if (!string.IsNullOrEmpty(wholesale.Vehicle.Vin))
                    {
                        manheimService.ExecuteByVin(userName, password, wholesale.Vehicle.Vin.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                                {
                                    AuctionLowestPrice = (tmp.LowestPrice),
                                    AuctionAveragePrice = (tmp.AveragePrice),
                                    AuctionHighestPrice = (tmp.HighestPrice),
                                    Year = wholesale.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = CommonHelper.GetNextFriday(),
                                    Vin = wholesale.Vehicle.Vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = Constanst.VehicleStatus.Inventory
                                };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(wholesale.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(wholesale.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            manheimService.GetTrim(wholesale.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", wholesale.Vehicle.Year.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                               manheimService.ManheimWholesale.AveragePrice == 0 ||
                               manheimService.ManheimWholesale.HighestPrice == 0)
                               )
                            {
                                var subResult = new ManheimWholesaleViewModel
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = wholesale.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };

                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForSoldout(SoldoutInventory inventory)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();

                if (!string.IsNullOrEmpty(inventory.Vehicle.Vin.Trim()))
                {
                    manheimService.Execute(inventory.Vehicle.Vin.Trim());
                    result = manheimService.ManheimWholesales;
                }
                else
                {
                    var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                    var matchingModel = 0;
                    var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                    var matchingTrims = new List<ManheimTrim>();

                    foreach (var model in matchingModels)
                    {
                        matchingTrims = manheimService.MatchingTrimsByModelId(model);
                        if (matchingTrims.Count > 0)
                        {
                            matchingModel = model;
                            break;
                        }
                    }

                    // don't have trims in database
                    if (matchingTrims.Count == 0)
                    {
                        manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels);
                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }
                    }

                    foreach (var trim in matchingTrims)
                    {
                        manheimService.Execute("US", inventory.Vehicle.Year.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA");
                        if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                           manheimService.ManheimWholesale.AveragePrice == 0 ||
                           manheimService.ManheimWholesale.HighestPrice == 0))
                        {
                            var subResult = new ManheimWholesaleViewModel
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.ServiceId,
                                    TrimName = trim.Name
                                };
                            result.Add(subResult);
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForSoldout(SoldoutInventory inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {
                var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (context.ManheimValues.Any(x => x.Vin == inventory.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut))
                {
                    var searchResult = context.ManheimValues.Where(x => x.Vin == inventory.Vehicle.Vin
                        && x.VehicleStatusCodeId.Equals(Constanst.VehicleStatus.SoldOut)
                        && x.DateAdded > dtCompareToday).OrderBy(x => x.DateAdded).ToList();
                    if (searchResult.Count > 0)
                    {
                        result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                            {
                                LowestPrice = Convert.ToInt32(tmp.AuctionLowestPrice.GetValueOrDefault()),
                                AveragePrice = Convert.ToInt32(tmp.AuctionAveragePrice.GetValueOrDefault()),
                                HighestPrice = Convert.ToInt32(tmp.AuctionHighestPrice.GetValueOrDefault()),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            }));
                    }
                    else
                    {
                        foreach (var vincontrolmanheimvalue in searchResult)
                        {
                            context.Attach(vincontrolmanheimvalue);
                            context.DeleteObject(vincontrolmanheimvalue);
                        }
                        context.SaveChanges();

                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(inventory.Vehicle.Vin))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.Vehicle.Vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue
                                    {
                                        AuctionLowestPrice = tmp.LowestPrice,
                                        AuctionAveragePrice = tmp.AveragePrice,
                                        AuctionHighestPrice = tmp.HighestPrice,
                                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = DateTime.Now.AddDays(1),
                                        Vin = inventory.Vehicle.Vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut
                                    };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                            var matchingTrims = new List<ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", inventory.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModel.ToString(CultureInfo.InvariantCulture),
                                                       trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                                      manheimService.ManheimWholesale.AveragePrice == 0 ||
                                      manheimService.ManheimWholesale.HighestPrice == 0) &&
                                  manheimService.ManheimWholesale.LowestPrice > 0)
                                {
                                    var subResult = new ManheimWholesaleViewModel
                                        {
                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                            Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var manheimService = new ManheimService();

                    if (!string.IsNullOrEmpty(inventory.Vehicle.Vin.Trim()))
                    {
                        manheimService.ExecuteByVin(userName, password, inventory.Vehicle.Vin.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                                {
                                    AuctionLowestPrice = tmp.LowestPrice,
                                    AuctionAveragePrice = tmp.AveragePrice,
                                    AuctionHighestPrice = tmp.HighestPrice,
                                    Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = DateTime.Now.AddDays(1),
                                    Vin = inventory.Vehicle.Vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut
                                };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(inventory.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(inventory.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            manheimService.GetTrim(inventory.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", inventory.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                               manheimService.ManheimWholesale.AveragePrice == 0 ||
                               manheimService.ManheimWholesale.HighestPrice == 0) &&
                               manheimService.ManheimWholesale.LowestPrice > 0)
                            {
                                var subResult = new ManheimWholesaleViewModel
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = inventory.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForAppraisal(Appraisal appraisal)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();

                if (!string.IsNullOrEmpty(appraisal.Vehicle.Vin.Trim()))
                {
                    manheimService.Execute(appraisal.Vehicle.Vin.Trim());
                    result = manheimService.ManheimWholesales;
                }
                else
                {
                    var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
                    var matchingModel = 0;
                    var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
                    var matchingTrims = new List<ManheimTrim>();

                    foreach (var model in matchingModels)
                    {
                        matchingTrims = manheimService.MatchingTrimsByModelId(model);
                        if (matchingTrims.Count > 0)
                        {
                            matchingModel = model;
                            break;
                        }
                    }

                    // don't have trims in database
                    if (matchingTrims.Count == 0)
                    {
                        if (appraisal.Vehicle.Year != null)
                            manheimService.GetTrim(appraisal.Vehicle.Year.Value.ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels);
                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }
                    }

                    foreach (var trim in matchingTrims)
                    {
                        manheimService.Execute("US", appraisal.Vehicle.Year.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA");
                        if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                           manheimService.ManheimWholesale.AveragePrice == 0 ||
                           manheimService.ManheimWholesale.HighestPrice == 0))
                        {
                            var subResult = new ManheimWholesaleViewModel
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.ServiceId,
                                    TrimName = trim.Name
                                };
                            result.Add(subResult);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForAppraisal(Appraisal appraisal, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {

                if (context.ManheimValues.Any(x => x.Vin == appraisal.Vehicle.Vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal))
                {
                    var searchResult = context.ManheimValues.Where(x => x.Vin == appraisal.Vehicle.Vin
                        && x.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal).ToList();
                    if (searchResult.Count > 0)
                    {
                        result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                            {
                                LowestPrice = Convert.ToInt32(tmp.AuctionLowestPrice.GetValueOrDefault()),
                                AveragePrice = Convert.ToInt32(tmp.AuctionAveragePrice.GetValueOrDefault()),
                                HighestPrice = Convert.ToInt32(tmp.AuctionHighestPrice.GetValueOrDefault()),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            }));
                    }
                    else
                    {

                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(appraisal.Vehicle.Vin))
                        {
                            manheimService.ExecuteByVin(userName, password, appraisal.Vehicle.Vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue
                                    {
                                        AuctionLowestPrice = tmp.LowestPrice,
                                        AuctionAveragePrice = tmp.AveragePrice,
                                        AuctionHighestPrice = tmp.HighestPrice,
                                        Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = CommonHelper.GetNextFriday(),
                                        Vin = appraisal.Vehicle.Vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal
                                    };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
                            var matchingTrims = new List<ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(appraisal.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", appraisal.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModel.ToString(CultureInfo.InvariantCulture),
                                                       trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                                      manheimService.ManheimWholesale.AveragePrice == 0 ||
                                      manheimService.ManheimWholesale.HighestPrice == 0) &&
                                    manheimService.ManheimWholesale.LowestPrice > 0)
                                {
                                    var subResult = new ManheimWholesaleViewModel
                                        {
                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                            Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var manheimService = new ManheimService();

                    if (!string.IsNullOrEmpty(appraisal.Vehicle.Vin.Trim()))
                    {
                        manheimService.ExecuteByVin(userName, password, appraisal.Vehicle.Vin.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                                {
                                    AuctionLowestPrice = tmp.LowestPrice,
                                    AuctionAveragePrice = tmp.AveragePrice,
                                    AuctionHighestPrice = tmp.HighestPrice,
                                    Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = CommonHelper.GetNextFriday(),
                                    Vin = appraisal.Vehicle.Vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal
                                };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            manheimService.GetTrim(appraisal.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", appraisal.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                               manheimService.ManheimWholesale.AveragePrice == 0 ||
                               manheimService.ManheimWholesale.HighestPrice == 0) &&
                               manheimService.ManheimWholesale.LowestPrice > 0
                                )
                            {
                                var subResult = new ManheimWholesaleViewModel
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForAppraisalWihtoutVin(Appraisal appraisal, string vin, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {
                var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (context.ManheimValues.Any(x => x.Vin == vin && x.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal))
                {
                    var searchResult = context.ManheimValues.Where(x => x.Vin == vin
                        && x.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal
                        && x.DateAdded > dtCompareToday).OrderBy(x => x.DateAdded).ToList();
                    if (searchResult.Count > 0)
                    {
                        result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                            {
                                LowestPrice = Convert.ToInt32(tmp.AuctionLowestPrice.GetValueOrDefault()),
                                AveragePrice = Convert.ToInt32(tmp.AuctionAveragePrice.GetValueOrDefault()),
                                HighestPrice = Convert.ToInt32(tmp.AuctionHighestPrice.GetValueOrDefault()),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            }));
                    }
                    else
                    {
                        foreach (var vincontrolmanheimvalue in searchResult)
                        {
                            context.Attach(vincontrolmanheimvalue);
                            context.DeleteObject(vincontrolmanheimvalue);
                        }
                        context.SaveChanges();

                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(vin))
                        {
                            manheimService.ExecuteByVin(userName, password, vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue
                                    {
                                        AuctionLowestPrice = tmp.LowestPrice,
                                        AuctionAveragePrice = tmp.AveragePrice,
                                        AuctionHighestPrice = tmp.HighestPrice,
                                        Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = DateTime.Now.AddDays(1),
                                        Vin = vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal
                                    };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
                            var matchingTrims = new List<ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(appraisal.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", appraisal.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture),
                                                       matchingModel.ToString(CultureInfo.InvariantCulture),
                                                       trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                                      manheimService.ManheimWholesale.AveragePrice == 0 ||
                                      manheimService.ManheimWholesale.HighestPrice == 0) &&
                                    manheimService.ManheimWholesale.LowestPrice > 0)
                                {
                                    var subResult = new ManheimWholesaleViewModel
                                        {
                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                            Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var manheimService = new ManheimService();

                    if (!string.IsNullOrEmpty(vin))
                    {
                        manheimService.ExecuteByVin(userName, password, vin);
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                                {
                                    AuctionLowestPrice = tmp.LowestPrice,
                                    AuctionAveragePrice = tmp.AveragePrice,
                                    AuctionHighestPrice = tmp.HighestPrice,
                                    Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = DateTime.Now.AddDays(1),
                                    Vin = vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal
                                };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            manheimService.GetTrim(appraisal.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", appraisal.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice == 0 ||
                               manheimService.ManheimWholesale.AveragePrice == 0 ||
                               manheimService.ManheimWholesale.HighestPrice == 0) &&
                              manheimService.ManheimWholesale.LowestPrice > 0)
                            {
                                var subResult = new ManheimWholesaleViewModel
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public static List<ManheimTransactionViewModel> ManheimTransactions(string year, string make, string model, string trim, string region)
        {
            var manheimService = new ManheimService();
            manheimService.Execute("US", year, make, model, trim, region);
            return manheimService.ManheimTransactions;
        }

        public static Setting GetManheimCredential(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var manheimCredentail = context.Settings.FirstOrDefault(m => m.DealerId == dealerId && !string.IsNullOrEmpty(m.Manheim.Trim()));
                return manheimCredentail;
            }
        }


        public static List<WarrantyTypeViewModel> GetWarrantyTypeList(DealershipViewModel dealer)
        {
            using (var context = new VincontrolEntities())
            {
                var warrantyList = new List<WarrantyTypeViewModel>();

                var basicList = context.BasicWarrantyTypes.ToList();
                
                if (dealer.DealerGroupId != null)
                {
                    if (dealer.DealerGroupId.ToLower().Equals(ConfigurationManager.AppSettings["Pendragon"].ToString(CultureInfo.InvariantCulture)))
                    {
                        basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Dealer Certified")));
                    }
                }

                if (ConfigurationManager.AppSettings["MINIOfUniversalCity"] != null && dealer.DealershipId.Equals(Convert.ToInt32(ConfigurationManager.AppSettings["MINIOfUniversalCity"])))
                {
                    basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Manufacturer Warranty")));
                    basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Dealer Certified")));
                    basicList.RemoveAt(basicList.FindIndex(x => x.Name.Equals("Manufacturer Certified")));
                }
                
                foreach (var tmp in basicList)
                {
                    var warrantyType = new WarrantyTypeViewModel()
                    {
                        DealerId = dealer.DealershipId,
                        EnglishVersionUrl =
                            "/Report/CreateBuyerGuide?type=" + tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                        SpanishVersionUrl =
                            "/Report/CreateBuyerGuideSpanish?type=" +
                            tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                        Name = tmp.Name,
                        Id = tmp.WarrantyTypeId,
                        CategoryId = tmp.Category.GetValueOrDefault()
                    };

                    warrantyList.Add(warrantyType);
                    
                }
                     
                var addtionalList = context.WarrantyTypes.Where(i => i.DealerId == dealer.DealershipId).ToList();
                
                if (addtionalList.Any())
                {
                    foreach (var tmp in addtionalList)
                    {
                        var warrantyType = new WarrantyTypeViewModel()
                        {
                            DealerId = dealer.DealershipId,
                            EnglishVersionUrl =
                                "/Report/CreateBuyerGuide?type=" +
                                tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                            SpanishVersionUrl =
                                "/Report/CreateBuyerGuideSpanish?type=" +
                                tmp.WarrantyTypeId.ToString(CultureInfo.InvariantCulture),
                            Name = tmp.Name,
                            Id = tmp.WarrantyTypeId,
                            CategoryId = tmp.Category.GetValueOrDefault()
                        };

                        warrantyList.Add(warrantyType);

                    }
                }




                return warrantyList;
            }

           
        }

        public static void SaveBucketJumpHistory(int listingId, int dealershipId, string dealerName, string vin,
            string stock, decimal salePrice, decimal retailPrice, decimal certifiedAmount, decimal mileageAdjustment,
            string note, short type, byte[] bytes, string user, string userFullName, int marketListingId, int daysAged)
        {
            using (var context = new VincontrolEntities())
            {
                var path = System.Web.HttpContext.Current.Server.MapPath("\\BucketJumpReports") + "\\" + dealershipId +
                           "\\" + (type.Equals(Constanst.VehicleStatus.Inventory) ? "Inventory" : "Appraisal") + "\\" +
                           listingId;
                var fileName = DateTime.Now.ToString("MMddyyyyhhmmsstt") + "_" + listingId + ".pdf";
                var directory = new DirectoryInfo(path);
                if (!directory.Exists)
                    directory.Create();

                var temp = new FileInfo(Path.Combine(@path, fileName));
                if (temp.Exists)
                    temp.Delete();

                var fileToupload = new FileStream(Path.Combine(@path, fileName), FileMode.Create);

                fileToupload.Write(bytes, 0, bytes.Length);
                fileToupload.Close();
                fileToupload.Dispose();

                var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (
                    context.BucketJumpHistories.Any(
                        x =>
                            x.ListingId == listingId && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory &&
                            x.DateStamp > todayDate))
                {
                    var bucketJumpHistory = context.BucketJumpHistories.FirstOrDefault(x =>
                            x.ListingId == listingId && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory &&
                            x.DateStamp > todayDate);
                    if (bucketJumpHistory != null)
                    {
                        bucketJumpHistory.UserStamp = user;
                        bucketJumpHistory.UserFullName = userFullName;
                        bucketJumpHistory.DealerId = dealershipId;
                        bucketJumpHistory.Store = dealerName;
                        bucketJumpHistory.VIN = vin;
                        bucketJumpHistory.Note = note;
                        bucketJumpHistory.AttachFile = fileName;
                        bucketJumpHistory.SalePrice = salePrice;
                        bucketJumpHistory.RetailPrice = retailPrice;
                        bucketJumpHistory.DateStamp = DateTime.Now;
                        bucketJumpHistory.CertifiedAmount = certifiedAmount;
                        bucketJumpHistory.MileageAdjustment = mileageAdjustment;
                        bucketJumpHistory.Note = note;
                        bucketJumpHistory.DaysAged = daysAged;
                        context.SaveChanges();
                    }
                }
                else
                {
                    var bucketJumpHistory = new vincontrol.Data.Model.BucketJumpHistory
                    {
                        UserStamp = user,
                        UserFullName = userFullName,
                        DealerId = dealershipId,
                        Store = dealerName,
                        DateStamp = DateTime.Now,
                        ListingId = listingId,
                        VIN = vin,
                        Stock = stock,
                        AttachFile = fileName,
                        SalePrice = salePrice,
                        VehicleStatusCodeId = type,
                        RetailPrice = retailPrice,
                        CertifiedAmount = certifiedAmount,
                        MileageAdjustment = mileageAdjustment,
                        Note = note,
                        DaysAged = daysAged
                    };

                    if (type.Equals(Constanst.VehicleStatus.Inventory))
                    {
                        var log = new VehicleLog()
                        {
                            DateStamp = DateTime.Now,
                            Description =
                                Constanst.VehicleLogSentence.BucketJumpCreatedByUser.Replace("OLDPRICE",
                                    salePrice.ToString("C0"))
                                    .Replace("NEWPRICE", retailPrice.ToString("C0"))
                                    .Replace("USER", SessionHandler.CurrentUser.FullName),
                            InventoryId = listingId,
                            UserId = SessionHandler.CurrentUser.UserId
                        };
                        context.AddToVehicleLogs(log);
                    }

                    context.AddToBucketJumpHistories(bucketJumpHistory);
                    context.SaveChanges();
                }

                var contexMarket = new VinMarketEntities();
                var marketCar = contexMarket.years.FirstOrDefault(x => x.RegionalListingId == marketListingId);
                if (marketCar != null)
                {
                    marketCar.Highlighted = true;
                    var savingMarketCar = new SavedBucketJumpMarketCar()
                    {
                        AutoTraderListingId = marketCar.AutoTraderListingId,
                        CarmaxListingId = marketCar.CarMaxVehicleId.GetValueOrDefault(),
                        CarsComListingId = marketCar.CarsComListingId,
                        DateStamp = DateTime.Now,
                        Vin = marketCar.Vin
                    };

                    contexMarket.AddToSavedBucketJumpMarketCars(savingMarketCar);

                    contexMarket.SaveChanges();
                }
            }
        }

        public static IEnumerable<DealershipActivityViewModel> GetAllActivities(short type)
        {
            if (type == Constanst.ActivityTypeId.Appraisal)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerAppraisalActivity(context).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.Inventory)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerInventoryActivity(context).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList().Where(x => !string.IsNullOrEmpty(x.Title))
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.User)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerUserActivity(context).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.ShareFlyer)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerShareFlyerActivity(context).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.ShareBrochure)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerShareBrochureActivity(context).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            return new LinkedList<DealershipActivityViewModel>();
        }

        public static IEnumerable<DealershipActivityViewModel> FilterActivitiesByDate(DateTime fromDate, DateTime toDate, short type)
        {
            if (type == Constanst.ActivityTypeId.Appraisal)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerAppraisalActivity(context).Where(
                            i => i.DateStamp >= fromDate && i.DateStamp <= toDate).OrderByDescending(
                                i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.Inventory)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerInventoryActivity(context).Where(
                            i => i.DateStamp >= fromDate && i.DateStamp <= toDate).OrderByDescending(
                                i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.User)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerUserActivity(context).Where(
                                    i => i.DateStamp >= fromDate && i.DateStamp <= toDate).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.ShareFlyer)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerShareFlyerActivity(context).Where(
                                    i => i.DateStamp >= fromDate && i.DateStamp <= toDate).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            if (type == Constanst.ActivityTypeId.ShareBrochure)
            {
                using (var context = new VincontrolEntities())
                {
                    var list =
                        InventoryQueryHelper.GetSingleOrGroupDealerShareBrochureActivity(context).Where(
                                    i => i.DateStamp >= fromDate && i.DateStamp <= toDate).OrderByDescending(
                            i => i.DateStamp).ToList();
                    return list.Count > 0
                               ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                               : new List<DealershipActivityViewModel>();
                }
            }
            return new LinkedList<DealershipActivityViewModel>();
         
        }

        public static IEnumerable<UserRoleViewModel> GetUserRoleViewModel(int dealershipId, string search)
        {
            using (var context = new VincontrolEntities())
            {
                return
                 (from users in
                      context.UserPermissions.Where(GetSearchPr(search)).Where(
                         i =>
                             i.DealerId == dealershipId && i.RoleId != Constanst.RoleType.Master &&
                             i.User.Active.Value)
                  select new UserRoleViewModel
                   {
                       Name = users.User.Name,
                       UserId = users.User.UserId,
                       Username = users.User.UserName,
                       Password = users.User.Password,
                       Email = users.User.Email,
                       Cellphone = users.User.CellPhone,
                       //et.RoleName,
                       RoleId = users.RoleId,
                       //userNotification.NotificationTypeId,
                       Active = users.User.Active ?? false
                       //AppraisalNotification = r != null && r.AppraisalNotified,
                       //WholeSaleNotfication = r != null && r.WholesaleNotified,
                       //InventoryNotfication = r != null && r.InventoryNotified,
                       //TwentyFourHourNotification = r != null && r.C24hNotified,
                       //NoteNotification = r != null && r.NoteNotified,
                       //PriceChangeNotification = r != null && r.PriceChangeNotified,
                       //AgeingBucketJumpNotification = r != null && r.AgingNotified,
                       //BucketJumpReportNotification = r != null && r.BucketJumpNotified,
                       //MarketPriceRangeChangeNotification = r != null && r.MarketPriceRangeNotified,
                       //ImageUploadNotification = r != null && r.ImageUploadNotified
                   }).ToList();
            }

        }

        private static Expression<Func<UserPermission, bool>> GetSearchPr(string search)
        {
            if (String.IsNullOrEmpty(search))
            {
                return i => true;
            }
            return i => (i.User.UserName.Contains(search) || i.User.Name.Contains(search) || i.User.Email.Contains(search));
        }

       
        public static List<vincontrol.DomainObject.ExtendedSelectListItem> GetChromeMake(int yearFrom, int yearTo)
        {
            using (var context = new VincontrolEntities())
            {
                var list = (from y in context.YearMakes
                            join m in context.Makes on y.MakeId equals m.MakeId
                            where y.Year <= yearTo && y.Year >= yearFrom
                            select new vincontrol.DomainObject.ExtendedSelectListItem
                                       {
                                           Text = m.Value,
                                           Value = SqlFunctions.StringConvert((double)m.MakeId).Trim()
                                       }).Distinct().ToList();

                return list;
            }
        }

        public static List<vincontrol.DomainObject.ExtendedSelectListItem> GetChromeModel(int yearFrom, int yearTo, int makeID)
        {
            using (var context = new VincontrolEntities())
            {
                var list = (from y in context.YearMakes
                            join m in context.Makes on y.MakeId equals m.MakeId
                            join mo in context.Models on y.YearMakeId equals mo.YearMakeId
                            where y.Year <= yearTo && y.Year >= yearFrom && m.MakeId == makeID

                            select new vincontrol.DomainObject.ExtendedSelectListItem
                                       {
                                           Text = mo.Value,
                                           Value = SqlFunctions.StringConvert((double)mo.ModelId).Trim()
                                       }).OrderBy(x => x.Text)
                                         .GroupBy(r => r.Text)
                                         .Select(g => g.FirstOrDefault()).ToList();

                return list;
            }
        }

        public static List<vincontrol.DomainObject.ExtendedSelectListItem> GetTrimModel(int yearFrom, int yearTo, int makeID, int modelID)
        {
            using (var context = new VincontrolEntities())
            {
                var list = (from y in context.YearMakes
                            join m in context.Makes on y.MakeId equals m.MakeId
                            join mo in context.Models on y.YearMakeId equals mo.YearMakeId
                            join t in context.Trims on mo.ModelId equals t.ModelId
                            where y.Year <= yearTo && y.Year >= yearFrom && m.MakeId == makeID && mo.ModelId == modelID && t.TrimName.Trim() != string.Empty

                            select new vincontrol.DomainObject.ExtendedSelectListItem
                            {
                                Text = t.TrimName,
                                Value = SqlFunctions.StringConvert((double)t.TrimId).Trim()
                            }).OrderBy(x => x.Text)
                                         .GroupBy(r => r.Text)
                                         .Select(g => g.FirstOrDefault()).ToList();

                return list;
            }
        }
    }
}
