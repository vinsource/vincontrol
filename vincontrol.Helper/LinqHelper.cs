using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
//using vincontrol.Manheim;
using ManheimTransactionViewModel = vincontrol.Application.ViewModels.CommonManagement.ManheimTransactionViewModel;
using ManheimWholesaleViewModel = vincontrol.Application.ViewModels.CommonManagement.ManheimWholesaleViewModel;
using ManheimYearMakeModel = vincontrol.Application.ViewModels.CommonManagement.ManheimYearMakeModel;


namespace vincontrol.Helper
{
    public class LinqHelper
    {
 


        //public static List<ManheimWholesaleViewModel> ManheimReportForAppraisal(Appraisal appraisal, string userName, string password)
        //{
        //    var result = new List<ManheimWholesaleViewModel>();
        //    var context = new VincontrolEntities();
        //    try
        //    {

        //        if (context.ManheimValues.Any(x => x.VehicleId == appraisal.Vehicle.VehicleId))
        //        {
        //            var searchResult = context.ManheimValues.Where(x => x.VehicleId == appraisal.Vehicle.VehicleId).ToList();

        //            result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
        //            {
        //                LowestPrice =tmp.AuctionLowestPrice.GetValueOrDefault(),
        //                AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
        //                HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
        //                Year = tmp.Year.GetValueOrDefault(),
        //                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
        //                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
        //                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
        //                TrimName = tmp.Trim
        //            }));

        //        }
        //        else
        //        {
        //            var manheimService = new ManheimService();

        //            if (!string.IsNullOrEmpty(appraisal.Vehicle.Vin.Trim()))
        //            {
        //                manheimService.ExecuteByVin(userName, password, appraisal.Vehicle.Vin.Trim());
        //                result = manheimService.ManheimWholesales;

        //                foreach (var tmp in result)
        //                {
        //                    var manheimRecord = new ManheimValue
        //                    {
        //                        AuctionLowestPrice = Convert.ToDecimal(tmp.LowestPrice),
        //                        AuctionAveragePrice = Convert.ToDecimal(tmp.AveragePrice),
        //                        AuctionHighestPrice = Convert.ToDecimal(tmp.HighestPrice),
        //                        Year = appraisal.Vehicle.Year.GetValueOrDefault(),
        //                        MakeServiceId = tmp.MakeServiceId,
        //                        ModelServiceId = tmp.ModelServiceId,
        //                        TrimServiceId = tmp.TrimServiceId,
        //                        Trim = tmp.TrimName,
        //                        DateAdded = DateTime.Now,
        //                        Expiration = CommonHelper.GetNextFriday(),
        //                        Vin = appraisal.Vehicle.Vin,
        //                        LastUpdated = DateTime.Now,
        //                        VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal,
        //                        VehicleId = appraisal.Vehicle.VehicleId
        //                    };

        //                    context.AddToManheimValues(manheimRecord);
        //                }

        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
        //                var matchingModel = 0;
        //                var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
        //                var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

        //                foreach (var model in matchingModels)
        //                {
        //                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                    if (matchingTrims.Count > 0)
        //                    {
        //                        matchingModel = model;
        //                        break;
        //                    }
        //                }

        //                // don't have trims in database
        //                if (matchingTrims.Count == 0)
        //                {
        //                    manheimService.GetTrim(appraisal.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
        //                    foreach (var model in matchingModels)
        //                    {
        //                        matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                        if (matchingTrims.Count > 0)
        //                        {
        //                            matchingModel = model;
        //                            break;
        //                        }
        //                    }
        //                }

        //                foreach (var trim in matchingTrims)
        //                {
        //                    manheimService.Execute("US", appraisal.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
        //                    if (!(Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) == 0 ||
        //                       Convert.ToInt32(manheimService.ManheimWholesale.AveragePrice) == 0 ||
        //                       Convert.ToInt32(manheimService.ManheimWholesale.HighestPrice) == 0) &&
        //                       Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) > 0
        //                        )
        //                    {
        //                        var subResult = new ManheimWholesaleViewModel
        //                        {
        //                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                            Year = appraisal.Vehicle.Year.GetValueOrDefault(),
        //                            MakeServiceId = matchingMake,
        //                            ModelServiceId = matchingModel,
        //                            TrimServiceId = trim.ServiceId,
        //                            TrimName = trim.Name
        //                        };
        //                        result.Add(subResult);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return result;
        //}

        public static bool CanViewBucketJumpReport(int dealerId)
        {
            using (var context = new VincontrolEntities())
            {
                var allowedDealer =
                    context.Dealers.Where(
                        i => i.DealerId == dealerId && i.DealerGroupId.ToLower().Equals("g1010")).
                        FirstOrDefault();

                return allowedDealer == null ? false : true;
            }
        }

        public static List<SmallKarPowerViewModel> GetKbbReport(string vin)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.KellyBlueBooks.Any(x => x.Vin == vin))
                {
                    var searchResult = context.KellyBlueBooks.Where(x => x.Vin == vin).OrderBy(x => x.DateStamp).ToList();

                    var result = new List<SmallKarPowerViewModel>();

                    foreach (var tmp in searchResult)
                    {
                        var kbbModel = new SmallKarPowerViewModel()
                        {
                            BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                            MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                            SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                            SelectedTrimName = tmp.Trim,
                            Wholesale = tmp.WholeSale.GetValueOrDefault(),

                        };

                        result.Add(kbbModel);
                    }

                    return result;
                }
                return null;
            }
        }

        public static Carfax CheckVinHasCarFaxReport(string vin)
        {
            using (var context = new VincontrolEntities())
            {
                if (context.Carfaxes.Any(o => o.Vin == vin))
                {
                    return context.Carfaxes.First(o => o.Vin == vin);

                }
            }
            return null;
        }

        public static void AddCarFaxReport(CarFaxViewModel carfax)
        {
            using (var context = new VincontrolEntities())
            {
                int number = 0;

                bool flag =
              Int32.TryParse(
                 carfax.NumberofOwners, out number);

                var e = new Carfax()
                {
                    Vin = carfax.Vin,
                    DateStamp = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3),
                    Owner = number,
                    ServiceRecord = Convert.ToInt32(carfax.ServiceRecords),
                    PriorRental = carfax.PriorRental,
                    Accident = carfax.AccidentCounts



                };
                var builder = new StringBuilder();
                foreach (CarFaxWindowSticker tmp in carfax.ReportList)
                {
                    builder.Append(tmp.Text);
                    builder.Append("|");
                    builder.Append(tmp.Image);
                    builder.Append(",");
                }
                if (!String.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Remove(builder.ToString().Length - 1, 1);
                    e.WindowSticker = builder.ToString();
                }

                //Add to memory

                context.AddToCarfaxes(e);


                context.SaveChanges();
            }
        }

        public static void UpdateCarFaxReport(CarFaxViewModel carfax)
        {
            using (var context = new VincontrolEntities())
            {
                var builder = new StringBuilder();
                int number;
                Int32.TryParse(
                    carfax.NumberofOwners, out number);


                foreach (CarFaxWindowSticker tmp in carfax.ReportList)
                {
                    builder.Append(tmp.Text);
                    builder.Append("|");
                    builder.Append(tmp.Image);
                    builder.Append(",");
                }
                if (!String.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Remove(builder.ToString().Length - 1, 1);

                }

                var findCarFax = context.Carfaxes.First(x => x.Vin.Equals(carfax.Vin));

                findCarFax.Owner = number;

                findCarFax.WindowSticker = builder.ToString();

                findCarFax.PriorRental = carfax.PriorRental;

                findCarFax.Accident = carfax.AccidentCounts;

                findCarFax.LastUpdated = DateTime.Now;

                findCarFax.Expiration = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(3);

                context.SaveChanges();
            }
        }

        //public static List<ManheimWholesaleViewModel> ManheimReport(VehicleViewModel inventory, string userName, string password)
        //{
        //    var result = new List<ManheimWholesaleViewModel>();
        //    using (var context = new VincontrolEntities())
        //    {
        //        try
        //        {
        //            if (context.ManheimValues.Any(x => x.Vin == inventory.Vin) && !inventory.Year.Equals(0))
        //            {
        //                var searchResult = context.ManheimValues.Where(x => x.Vin == inventory.Vin).ToList();
        //                if (searchResult.Count > 0)
        //                {
        //                    foreach (var tmp in searchResult.Distinct())
        //                    {
        //                        if (!result.Any(i => i.TrimServiceId == tmp.TrimServiceId))
        //                        {
        //                            var subResult = new ManheimWholesaleViewModel()
        //                            {

        //                                LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
        //                                AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
        //                                HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
        //                                Year = inventory.Year.Equals(0) ? tmp.Year.GetValueOrDefault() : inventory.Year,
        //                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
        //                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
        //                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
        //                                TrimName = tmp.Trim
        //                            };
        //                            result.Add(subResult);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var manheimService = new ManheimService();
        //                    if (!string.IsNullOrEmpty(inventory.Vin))
        //                    {
        //                        manheimService.ExecuteByVin(userName, password, inventory.Vin.Trim());
        //                        result = manheimService.ManheimWholesales;

        //                        foreach (var tmp in result)
        //                        {
        //                            var manheimRecord = new ManheimValue()
        //                            {
        //                                AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
        //                                AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
        //                                AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
        //                                Year = inventory.Year.Equals(0) ? tmp.Year : inventory.Year,
        //                                MakeServiceId = tmp.MakeServiceId,
        //                                ModelServiceId = tmp.ModelServiceId,
        //                                TrimServiceId = tmp.TrimServiceId,
        //                                Trim = tmp.TrimName,
        //                                DateAdded = DateTime.Now,
        //                                Expiration = CommonHelper.GetNextFriday(),
        //                                Vin = inventory.Vin,
        //                                LastUpdated = DateTime.Now,
        //                                VehicleStatusCodeId = 1
        //                            };

        //                            context.AddToManheimValues(manheimRecord);
        //                        }

        //                        context.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        var matchingMake = manheimService.MatchingMake(inventory.Make);
        //                        var matchingModel = 0;
        //                        var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
        //                        var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

        //                        foreach (var model in matchingModels)
        //                        {
        //                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                            if (matchingTrims.Count > 0)
        //                            {
        //                                matchingModel = model;
        //                                break;
        //                            }
        //                        }

        //                        // don't have trims in database
        //                        if (matchingTrims.Count == 0)
        //                        {
        //                            manheimService.GetTrim(inventory.Year.ToString(), matchingMake.ToString(),
        //                                                   matchingModels, userName, password);
        //                            foreach (var model in matchingModels)
        //                            {
        //                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                                if (matchingTrims.Count > 0)
        //                                {
        //                                    matchingModel = model;
        //                                    break;
        //                                }
        //                            }
        //                        }

        //                        foreach (var trim in matchingTrims)
        //                        {
        //                            manheimService.Execute("US", inventory.Year.ToString(), matchingMake.ToString(),
        //                                                   matchingModel.ToString(),
        //                                                   trim.ServiceId.ToString(), "NA", userName,
        //                                                   password);
        //                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals(0) ||
        //                                  manheimService.ManheimWholesale.AveragePrice.Equals(0) ||
        //                                  manheimService.ManheimWholesale.HighestPrice.Equals(0)))
        //                            {
        //                                var subResult = new ManheimWholesaleViewModel()
        //                                {
        //                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                                    Year = inventory.Year,
        //                                    MakeServiceId = matchingMake,
        //                                    ModelServiceId = matchingModel,
        //                                    TrimServiceId = trim.ServiceId,
        //                                    TrimName = trim.Name
        //                                };

        //                                result.Add(subResult);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var manheimService = new ManheimService();
        //                if (!string.IsNullOrEmpty(inventory.Vin))
        //                {
        //                    manheimService.ExecuteByVin(userName, password, inventory.Vin.Trim());
        //                    result = manheimService.ManheimWholesales;

        //                    foreach (var tmp in result)
        //                    {
        //                        var manheimRecord = new ManheimValue()
        //                        {
        //                            AuctionLowestPrice =Convert.ToDecimal((tmp.LowestPrice)),
        //                            AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
        //                            AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
        //                            Year = inventory.Year.Equals(0) ? tmp.Year : inventory.Year,
        //                            MakeServiceId = tmp.MakeServiceId,
        //                            ModelServiceId = tmp.ModelServiceId,
        //                            TrimServiceId = tmp.TrimServiceId,
        //                            Trim = tmp.TrimName,
        //                            DateAdded = DateTime.Now,
        //                            Expiration = CommonHelper.GetNextFriday(),
        //                            Vin = inventory.Vin,
        //                            LastUpdated = DateTime.Now,
        //                            VehicleStatusCodeId = 1
        //                        };

        //                        context.AddToManheimValues(manheimRecord);
        //                    }

        //                    context.SaveChanges();
        //                }
        //                else
        //                {
        //                    var matchingMake = manheimService.MatchingMake(inventory.Make);
        //                    var matchingModel = 0;
        //                    var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
        //                    var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

        //                    foreach (var model in matchingModels)
        //                    {
        //                        matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                        if (matchingTrims.Count > 0)
        //                        {
        //                            matchingModel = model;
        //                            break;
        //                        }
        //                    }

        //                    // don't have trims in database
        //                    if (matchingTrims.Count == 0)
        //                    {
        //                        manheimService.GetTrim(inventory.Year.ToString(), matchingMake.ToString(),
        //                                               matchingModels, userName, password);
        //                        foreach (var model in matchingModels)
        //                        {
        //                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                            if (matchingTrims.Count > 0)
        //                            {
        //                                matchingModel = model;
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    foreach (var trim in matchingTrims)
        //                    {
        //                        manheimService.Execute("US", inventory.Year.ToString(), matchingMake.ToString(),
        //                                               matchingModel.ToString(), trim.ServiceId.ToString(), "NA",
        //                                               userName, password);
        //                        if (!(manheimService.ManheimWholesale.LowestPrice.Equals(0) ||
        //                              manheimService.ManheimWholesale.AveragePrice.Equals(0) ||
        //                              manheimService.ManheimWholesale.HighestPrice.Equals(0)))
        //                        {
        //                            var subResult = new ManheimWholesaleViewModel()
        //                            {
        //                                LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                                AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                                HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                                Year = inventory.Year,
        //                                MakeServiceId = matchingMake,
        //                                ModelServiceId = matchingModel,
        //                                TrimServiceId = trim.ServiceId,
        //                                TrimName = trim.Name
        //                            };

        //                            result.Add(subResult);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }

        //    return result;
        //}

        //public static List<ManheimWholesaleViewModel> ManheimReportNew(VincontrolEntities context, VehicleViewModel inventory, string userName, string password)
        //{
        //    var result = new List<ManheimWholesaleViewModel>();

        //    try
        //    {
        //        if (context.ManheimValues.Any(x => x.VehicleId == inventory.VehicleId))
        //        {
        //            var searchResult = context.ManheimValues.Where(x => x.VehicleId == inventory.VehicleId).ToList();

        //            result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
        //            {
        //                LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
        //                AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
        //                HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
        //                Year = tmp.Year.GetValueOrDefault(),
        //                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
        //                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
        //                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
        //                TrimName = tmp.Trim
        //            }));

        //        }
        //        else
        //        {
        //            var manheimService = new ManheimService();
        //            if (!string.IsNullOrEmpty(inventory.Vin))
        //            {
        //                manheimService.ExecuteByVin(userName, password, inventory.Vin.Trim());
        //                result = manheimService.ManheimWholesales;

        //                foreach (var tmp in result)
        //                {
        //                    var manheimRecord = new ManheimValue
        //                    {
        //                        AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
        //                        AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
        //                        AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
        //                        Year = inventory.Year,
        //                        MakeServiceId = tmp.MakeServiceId,
        //                        ModelServiceId = tmp.ModelServiceId,
        //                        TrimServiceId = tmp.TrimServiceId,
        //                        Trim = tmp.TrimName,
        //                        DateAdded = DateTime.Now,
        //                        Expiration = CommonHelper.GetNextFriday(),
        //                        Vin = inventory.Vin,
        //                        LastUpdated = DateTime.Now,
        //                        VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,
        //                        VehicleId = inventory.VehicleId
        //                    };

        //                    context.AddToManheimValues(manheimRecord);
        //                }

        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                var matchingMake = manheimService.MatchingMake(inventory.Make);
        //                var matchingModel = 0;
        //                var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
        //                var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

        //                foreach (var model in matchingModels)
        //                {
        //                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                    if (matchingTrims.Count > 0)
        //                    {
        //                        matchingModel = model;
        //                        break;
        //                    }
        //                }

        //                // don't have trims in database
        //                if (matchingTrims.Count == 0)
        //                {
        //                    manheimService.GetTrim(inventory.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
        //                    foreach (var model in matchingModels)
        //                    {
        //                        matchingTrims = manheimService.MatchingTrimsByModelId(model);
        //                        if (matchingTrims.Count > 0)
        //                        {
        //                            matchingModel = model;
        //                            break;
        //                        }
        //                    }
        //                }

        //                foreach (var trim in matchingTrims)
        //                {
        //                    manheimService.Execute("US", inventory.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
        //                    if (!(Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) == 0 ||
        //                       Convert.ToInt32(manheimService.ManheimWholesale.AveragePrice) == 0 ||
        //                       Convert.ToInt32(manheimService.ManheimWholesale.HighestPrice) == 0) &&
        //                       Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) > 0)
        //                    {
        //                        var subResult = new ManheimWholesaleViewModel
        //                        {
        //                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                            Year = inventory.Year,
        //                            MakeServiceId = matchingMake,
        //                            ModelServiceId = matchingModel,
        //                            TrimServiceId = trim.ServiceId,
        //                            TrimName = trim.Name
        //                        };

        //                        result.Add(subResult);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return result;
        //}


        //public static List<ManheimWholesaleViewModel> ManheimReportNew(VinsellEntities context, VehicleViewModel inventory, string userName, string password)
        //{
        //    var result = new List<ManheimWholesaleViewModel>();

        //    try
        //    {
        //        if (context.manheim_ManheimValue.Any(x => x.VehicleId == inventory.VehicleId))
        //        {
        //            var searchResult = context.manheim_ManheimValue.Where(x => x.VehicleId == inventory.VehicleId).ToList();

        //            result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
        //            {
        //                LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
        //                AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
        //                HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
        //                Year = tmp.Year.GetValueOrDefault(),
        //                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
        //                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
        //                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
        //                TrimName = tmp.Trim
        //            }));

        //        }
        //        else
        //        {
        //            var manheimService = new ManheimService();
        //            if (!string.IsNullOrEmpty(inventory.Vin))
        //            {
        //                manheimService.ExecuteByVin(userName, password, inventory.Vin.Trim());
        //                result = manheimService.ManheimWholesales;

        //                foreach (var tmp in result)
        //                {
        //                    var manheimRecord = new manheim_ManheimValue()
        //                    {
        //                        AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
        //                        AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
        //                        AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
        //                        Year = inventory.Year,
        //                        MakeServiceId = tmp.MakeServiceId,
        //                        ModelServiceId = tmp.ModelServiceId,
        //                        TrimServiceId = tmp.TrimServiceId,
        //                        Trim = tmp.TrimName,
        //                        DateAdded = DateTime.Now,
        //                        Expiration = CommonHelper.GetNextFriday(),
        //                        Vin = inventory.Vin,
        //                        LastUpdated = DateTime.Now,
        //                        VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,
        //                        VehicleId = inventory.VehicleId
        //                    };

        //                    context.AddTomanheim_ManheimValue(manheimRecord);
        //                }

        //                context.SaveChanges();
        //            }
                  
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return result;
        //}

        public static ManheimWholesaleViewModel ManheimReportForTradeIn(string vin, int year, string make, string model, string trim, string userName, string password)
        {
            var result = new ManheimWholesaleViewModel();

            try
            {
                var searchResultList = GetManheimAuctionMarketData(year, make, model, trim);

                if (searchResultList.Any())
                {
                    var searchResult = searchResultList.First();
                    var lowestPrice = searchResult.MmrBelow.GetValueOrDefault();
                    var averagePrice = searchResult.Mmr;
                    var highestPrice = searchResult.MmrAbove.GetValueOrDefault();
                    result = new ManheimWholesaleViewModel()
                    {
                        LowestPrice = lowestPrice,
                        AveragePrice = averagePrice,
                        HighestPrice = highestPrice,
                        Year = year,
                    };
                }
                else
                {
                    result = new ManheimWholesaleViewModel()
                    {
                        LowestPrice = 0,
                        AveragePrice = 0,
                        HighestPrice = 0,
                        Year = year
                    };
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static IQueryable<manheim_vehicles> GetManheimAuctionMarketData(int year, string make, string modelWord, string trim)
        {
            var context = new VinsellEntities();

            if (make.ToLower().Equals("bmw") && modelWord.ToLower().Contains("series"))
            {
                modelWord = trim;
            }

            if (make.ToLower().Equals("mercedes-benz") && modelWord.ToLower().Contains("class"))
            {
                modelWord = trim;

                modelWord = modelWord.Replace("Sport", "");

                modelWord = modelWord.Replace("Luxry", "");

                modelWord = modelWord.Replace("BlueTEC", "");

                modelWord = modelWord.Replace("BTC", "");

                modelWord = modelWord.Replace("CDI", "");

                modelWord = modelWord.Replace("BLK", "");

                if (modelWord.Length >= 2)
                {

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("S4"))

                        modelWord = modelWord.Replace("S4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("V4"))

                        modelWord = modelWord.Replace("V4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("W4"))

                        modelWord = modelWord.Replace("W4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("AE"))

                        modelWord = modelWord.Replace("AE", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("WZ"))

                        modelWord = modelWord.Replace("WZ", "");
                }

                if (modelWord[modelWord.Length - 1] == 'W')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

                if (modelWord[modelWord.Length - 1] == 'R')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

                if (modelWord[modelWord.Length - 1] == 'V')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

                if (modelWord[modelWord.Length - 1] == 'C')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

                if (modelWord[modelWord.Length - 1] == 'A')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);
                if (modelWord[modelWord.Length - 1] == 'K')

                    modelWord = modelWord.Substring(0, modelWord.Length - 1);

            }

            modelWord = DataHelper.FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {

                var result = context.manheim_vehicles.Where(i => i.Year == year && i.Make == make &&
                                              ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                i.Trim.Replace(" ", "").Trim().ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) && i.Mmr > 0);

                return result;
            }

            else
            {
                if (modelWord.ToLower().Contains("rangeroversport"))
                {
                    var result = context.manheim_vehicles.Where(i => i.Year == year && i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroversport")
                                                   && i.Mmr > 0));

                    return result;
                }
                else if (modelWord.ToLower().Equals("rangerover"))
                {
                    var result = context.manheim_vehicles.Where(i => i.Year == year && i.Make == make &&
                                                 ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("rangerover") &&
                                                   !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("sport")
                                                   && i.Mmr > 0));

                    return result;
                }
                else
                {
                    var result = context.manheim_vehicles.Where(i => i.Year == year && i.Make == make &&
                                           ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                             i.Trim.Replace(" ", "").Trim().ToLower()).Contains(modelWord.Replace(" ", "").ToLower()))
                                           && i.Mmr > 0);

                    return result;
                }
            }
        }

        //public static List<ManheimTransactionViewModel> ManheimTransactions(string year, string make, string model, string trim, string region)
        //{
        //    var manheimService = new ManheimService();
        //    manheimService.Execute("US", year, make, model, trim, region);
        //    return manheimService.ManheimTransactions;
        //}

        public static void AddSimpleKbbReportFromKarPower(int vehicleId, List<SmallKarPowerViewModel> karpowerResult, string vin, short type)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (var tmp in karpowerResult)
                {

                    var e = new KellyBlueBook()
                    {
                        Trim = tmp.SelectedTrimName,
                        BaseWholeSale = Convert.ToInt32(tmp.BaseWholesale),
                        MileageAdjustment = Convert.ToInt32(tmp.MileageAdjustment),
                        WholeSale = Convert.ToInt32(tmp.Wholesale),
                        TrimId = tmp.SelectedTrimId,
                        DateStamp = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Expiration = CommonHelper.GetNextFriday(),
                        Vin = vin,
                        VehicleStatusCodeId = type,
                        VehicleId = vehicleId
                    };

                    //Add to memory

                    context.AddToKellyBlueBooks(e);

                }

                if (karpowerResult.Count == 1)
                {
                    var searchVehicleResult = context.Vehicles.FirstOrDefault(x => x.VehicleId == vehicleId);

                    if (searchVehicleResult != null)
                    {
                        searchVehicleResult.KBBTrimId = karpowerResult.First().SelectedTrimId;
                    }


                }

                context.SaveChanges();
            }
        }

        public static void AddSimpleKbbReportFromKarPower(List<SmallKarPowerViewModel> karpowerResult, string vin, string type)
        {
            using (var context = new VincontrolEntities())
            {
                foreach (var tmp in karpowerResult)
                {
                    var existingKbb = context.KellyBlueBooks.OrderByDescending(i => i.DateStamp).FirstOrDefault(i => i.Vin == vin && i.TrimId == tmp.SelectedTrimId && i.VehicleStatusCodeId.Equals(type == "Inventory" ? 1 : 2));
                    if (existingKbb == null)
                    {
                        var e = new KellyBlueBook()
                        {
                            Trim = tmp.SelectedTrimName,
                            BaseWholeSale = Convert.ToInt32(tmp.BaseWholesale),
                            MileageAdjustment = Convert.ToInt32(tmp.MileageAdjustment),
                            WholeSale = Convert.ToInt32(tmp.Wholesale),
                            TrimId = tmp.SelectedTrimId,
                            DateStamp = DateTime.Now,
                            LastUpdated = DateTime.Now,
                            Expiration = CommonHelper.GetNextFriday(),
                            Vin = vin,

                        };

                        //Add to memory

                        context.AddToKellyBlueBooks(e);
                    }
                }


                context.SaveChanges();
            }
        }

        public static IList<ManheimYearMakeModel> GetManheimYearMakeModelForAdvancedSearch()
        {
            var returnList = new List<ManheimYearMakeModel>();
            using (var context = new VinsellEntities())
            {
                returnList.AddRange(context.manheim_makes.Select(tmp => new ManheimYearMakeModel()
                {
                    Year = tmp.Year,
                    Make = tmp.Name
                }));
            }

            return returnList;
        }

        public static IList<ExtendedSelectListItem> GetManheimMakeForAdvancedSearch(int year)
        {
            using (var context = new VinsellEntities())
            {
                var models = context.manheim_makes.Where(i => i.Year.Equals(year)).ToList();
                return models.Count == 0
                           ? new List<ExtendedSelectListItem>()
                           : models.Select(i => i.Name).Distinct().OrderBy(i => i).Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();
            }
        }

        public static IList<ExtendedSelectListItem> GetManheimModelForAdvancedSearch(int year, string make)
        {
            using (var context = new VinsellEntities())
            {
                var models = context.manheim_models.Where(i => i.Make.ToLower().Equals(make.ToLower()) && i.Year == year).ToList();
                return models.Count == 0
                           ? new List<ExtendedSelectListItem>()
                           : models.Select(i => i.Name).Distinct().OrderBy(i => i).Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList();
            }
        }

        public static void SaveKarPowerOptions(string vin, string trimId, string selectedOptionIds, string transmissionId, string driveTrainId, string type, int dealershipId)
        {
            using (var context = new VinsellEntities())
            {
                var convertedTrimId = Convert.ToInt32(trimId);
                var convertedTransmissionId = Convert.ToInt32(transmissionId);
                var convertedDriveTrainId = Convert.ToInt32(driveTrainId);

                var existingKbbTrim = context.manheim_kbbtrims.OrderByDescending(i => i.DateStamp).FirstOrDefault(i => i.DealershipId == dealershipId && i.Vin == vin);
                if (existingKbbTrim != null)
                {
                    existingKbbTrim.TrimId = convertedTrimId;
                    existingKbbTrim.TransmissionId = convertedTransmissionId;
                    existingKbbTrim.DriveTrainId = convertedDriveTrainId;
                    existingKbbTrim.Options = selectedOptionIds;
                    existingKbbTrim.DateStamp = DateTime.Now;
                    context.SaveChanges();
                }
                else
                {
                    var newKbbTrim = new manheim_kbbtrims()
                    {
                        DealershipId = dealershipId,
                        Vin = vin,
                        TrimId = convertedTrimId,
                        TransmissionId = convertedTransmissionId,
                        DriveTrainId = convertedDriveTrainId,
                        Options = selectedOptionIds,
                        DateStamp = DateTime.Now
                    };
                    context.manheim_kbbtrims.AddObject(newKbbTrim);
                    context.SaveChanges();
                }
            }
        }

        public static manheim_kbbtrims GetSavedKbbTrim(string vin, int dealershipId)
        {
            using (var context = new VinsellEntities())
            {
                return context.manheim_kbbtrims.FirstOrDefault(i => i.Vin == vin && i.DealershipId == dealershipId);
            }
        }


    

    }
}
