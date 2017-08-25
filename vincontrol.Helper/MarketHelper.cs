using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Application.Vinsocial.Forms.TemplateManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
//using vincontrol.Manheim;
using ChartSelection = vincontrol.DomainObject.ChartSelection;
using ManheimWholesaleViewModel = vincontrol.Application.ViewModels.CommonManagement.ManheimWholesaleViewModel;

namespace vincontrol.Helper
{
    public class MarketHelper
    {
    
        private static Inventory GetTargetCar(int listingId)
        {
            using (var contextVinControl = new VincontrolEntities())
            {
                Inventory targetCar = contextVinControl.Inventories.Include("Vehicle").Include("Vehicle.TruckCategory").FirstOrDefault(x => x.InventoryId == listingId);

                if (targetCar != null)
                {
                    return targetCar;
                }

                var soldOutVehicle = contextVinControl.SoldoutInventories.Include("Vehicle").Include("Vehicle.TruckCategory").FirstOrDefault(x => x.SoldoutInventoryId == listingId);

                targetCar = new Inventory();

                if (soldOutVehicle != null)
                {
                    targetCar.Vehicle = soldOutVehicle.Vehicle;
                    targetCar.Certified = soldOutVehicle.Certified;
                }

                return targetCar;
            }
        }

        //GET TARGET CAR
        private static Appraisal GetAppraisalTargetCar(int appraisalId)
        {
            Appraisal targetCar;
            using (var contextVinControl = new VincontrolEntities())
            {
                targetCar = contextVinControl.Appraisals.Include("Vehicle").Include("Vehicle.TruckCategory").Include("Dealer").FirstOrDefault((x => x.AppraisalId == appraisalId));
            }

            return targetCar;

        }

     
        //public static List<ManheimWholesaleViewModel> ManheimReportForSoldCars(VehicleViewModel inventory, string userName, string password)
        //{
        //    var result = new List<ManheimWholesaleViewModel>();
        //    var context = new VincontrolEntities();
        //    try
        //    {
        //        if (context.ManheimValues.Any(x => x.VehicleId == inventory.VehicleId && x.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut))
        //        {
        //            var searchResult = context.ManheimValues.Where(x => x.VehicleId == inventory.VehicleId && x.VehicleStatusCodeId == Constanst.VehicleStatus.SoldOut).ToList();

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
        //                        AuctionLowestPrice = Convert.ToDecimal(tmp.LowestPrice),
        //                        AuctionAveragePrice = Convert.ToDecimal(tmp.AveragePrice),
        //                        AuctionHighestPrice = Convert.ToDecimal(tmp.HighestPrice),
        //                        Year = inventory.Year,
        //                        MakeServiceId = tmp.MakeServiceId,
        //                        ModelServiceId = tmp.ModelServiceId,
        //                        TrimServiceId = tmp.TrimServiceId,
        //                        Trim = tmp.TrimName,
        //                        DateAdded = DateTime.Now,
        //                        Expiration = CommonHelper.GetNextFriday(),
        //                        Vin = inventory.Vin,
        //                        LastUpdated = DateTime.Now,
        //                        VehicleStatusCodeId = Constanst.VehicleStatus.SoldOut,
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
        //                    if (inventory.Year != null)
        //                        manheimService.GetTrim(inventory.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
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

        //public static List<ManheimWholesaleViewModel> ManheimReportForWholesale(VehicleViewModel wholesale, string userName, string password)
        //{
        //    var result = new List<ManheimWholesaleViewModel>();
        //    var context = new VincontrolEntities();
        //    try
        //    {

        //        if (context.ManheimValues.Any(x => x.VehicleId == wholesale.VehicleId && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory))
        //        {

        //            var searchResult = context.ManheimValues.Where(x => x.VehicleId == wholesale.VehicleId
        //                && x.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory).ToList();

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
        //            if (!string.IsNullOrEmpty(wholesale.Vin))
        //            {
        //                manheimService.ExecuteByVin(userName, password, wholesale.Vin.Trim());
        //                result = manheimService.ManheimWholesales;

        //                foreach (var tmp in result)
        //                {
        //                    var manheimRecord = new ManheimValue
        //                    {
        //                        AuctionLowestPrice = Convert.ToDecimal(tmp.LowestPrice),
        //                        AuctionAveragePrice = Convert.ToDecimal(tmp.AveragePrice),
        //                        AuctionHighestPrice = Convert.ToDecimal(tmp.HighestPrice),
        //                        Year = wholesale.Year,
        //                        MakeServiceId = tmp.MakeServiceId,
        //                        ModelServiceId = tmp.ModelServiceId,
        //                        TrimServiceId = tmp.TrimServiceId,
        //                        Trim = tmp.TrimName,
        //                        DateAdded = DateTime.Now,
        //                        Expiration = CommonHelper.GetNextFriday(),
        //                        Vin = wholesale.Vin,
        //                        LastUpdated = DateTime.Now,
        //                        VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,
        //                        VehicleId = wholesale.VehicleId
        //                    };

        //                    context.AddToManheimValues(manheimRecord);
        //                }

        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                var matchingMake = manheimService.MatchingMake(wholesale.Make);
        //                var matchingModel = 0;
        //                var matchingModels = manheimService.MatchingModels(wholesale.Model, matchingMake);
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
        //                    manheimService.GetTrim(wholesale.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
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
        //                    manheimService.Execute("US", wholesale.Year.ToString(CultureInfo.InvariantCulture), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
        //                    if (!(Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) == 0 ||
        //                       Convert.ToInt32(manheimService.ManheimWholesale.AveragePrice) == 0 ||
        //                       Convert.ToInt32(manheimService.ManheimWholesale.HighestPrice) == 0)
        //                       )
        //                    {
        //                        var subResult = new ManheimWholesaleViewModel
        //                        {
        //                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                            Year = wholesale.Year,
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
        
        private static ChartSelection GetChartSelectionForAppraisal(int appraisalId)
        {
            var contextVinControl = new VincontrolEntities();

            var existingChartSelection = contextVinControl.ChartSelections.FirstOrDefault(s => s.ListingId == appraisalId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal);
            var savedSelection = new ChartSelection();
            if (existingChartSelection != null)
            {
                savedSelection.IsAll = existingChartSelection.IsAll != null && Convert.ToBoolean(existingChartSelection.IsAll);
                savedSelection.IsCarsCom = existingChartSelection.IsCarsCom != null && Convert.ToBoolean(existingChartSelection.IsCarsCom);
                savedSelection.IsCertified = existingChartSelection.IsCertified;
                savedSelection.IsFranchise = existingChartSelection.IsFranchise != null && Convert.ToBoolean(existingChartSelection.IsFranchise);
                savedSelection.IsIndependant = existingChartSelection.IsIndependant != null && Convert.ToBoolean(existingChartSelection.IsIndependant);
                savedSelection.Options = existingChartSelection.Options != null && existingChartSelection.Options != "0"
                                             ? existingChartSelection.Options.ToLower()
                                             : "";
                savedSelection.Trims = existingChartSelection.Trims != null && existingChartSelection.Trims != "0"
                                           ? existingChartSelection.Trims.ToLower()
                                           : "";
            }

            return savedSelection;
        }

        private static bool SatifySaveSelection(MarketCarInfo item, ChartSelection savedSelection, bool skipTrimFilter)
        {
            if (savedSelection != null)
            {
                if (savedSelection.IsFranchise)
                {
                    if (!item.Franchise.HasValue || !item.Franchise.Value) return false;
                }

                if (savedSelection.IsIndependant)
                {
                    if (item.Franchise.HasValue && item.Franchise.Value) return false;
                }

                if (!skipTrimFilter)
                {
                    if (!string.IsNullOrEmpty(savedSelection.Trims) && !savedSelection.Trims.Equals("0"))
                    {
                        var arrayTrim = String.Format(",{0},", savedSelection.Trims.Replace(" ", "").ToLower());
                        var trimEqualOther = savedSelection.Trims.ToLower().Equals("other");
                        var trimContainOther = savedSelection.Trims.ToLower().Contains("other");
                        return (trimEqualOther
                            ? (string.IsNullOrEmpty(item.Trim.Trim()))
                            : trimContainOther
                                ? string.IsNullOrEmpty(item.Trim.Trim()) ||
                                  arrayTrim.Contains("," + item.Trim.Replace(" ", "").ToLower() + ",")
                                : !string.IsNullOrEmpty(item.Trim.Trim()) &&
                                  arrayTrim.Contains("," + item.Trim.Replace(" ", "").ToLower() + ","));
                    }

                }
            }
            return true;
        }

        private static List<ChartModel> BuildQuery(DealershipViewModel dealer, ChartSelection savedSelection, List<MarketCarInfo> query, int? stateDistance, ChartGraph chartGraph, bool skipTrimFilter, bool isSold)
        {
            return BuildQueryInner(dealer, savedSelection, query, chartGraph, skipTrimFilter, isSold, (distance, date) => (!stateDistance.HasValue || distance <= stateDistance.Value));
        }

        public static List<ChartModel> BuildQueryInner(DealershipViewModel dealer, ChartSelection savedSelection, List<MarketCarInfo> marketCarInfoList, ChartGraph chartGraph, bool skipTrimFilter, bool isSold, Func<int, DateTime?, bool> filterFunc)
        {
            var list = new List<ChartModel>();

            if (marketCarInfoList.Any())
            {
                var dealerLatitude = double.Parse(dealer.Latitude.ToString());

                var dealerLongtitude = double.Parse(dealer.Longtitude.ToString());

                foreach (var item in marketCarInfoList)
                {
                    int distance = CommonHelper.DistanceBetweenPlaces(item.Latitude, item.Longitude, dealerLatitude, dealerLongtitude);

                    if (SatifySaveSelection(item, savedSelection, skipTrimFilter))
                    {

                        int validMileage = item.Mileage;

                        int validPrice = item.CurrentPrice;
                       
                        if (filterFunc(distance, item.LastUpdatedDate))
                        {
                            var chart = new ChartModel
                            {
                                ListingId = item.RegionalListingId,
                                Title =
                                    new TitleInfo
                                    {
                                        Make = item.Make,
                                        Model = item.Model.TrimEnd(),
                                        Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                        BodyStyle = string.IsNullOrEmpty(item.BodyStyle) ? "other" : item.BodyStyle.Replace(",", ""),
                                        Year = item.Year.GetValueOrDefault()
                                    },
                                Color =
                                    new ColorInfo
                                    {
                                        Exterior = item.ExteriorColor,
                                        Interior = item.InteriorColor
                                    },
                                Option = new OptionInfo
                                {
                                    Moonroof = item.MoonRoof.GetValueOrDefault(),
                                    Navigation = item.MoonRoof.GetValueOrDefault(),
                                    Sunroof = item.SunRoof.GetValueOrDefault()
                                },
                                Certified = item.Certified.GetValueOrDefault(),
                                Highlighted = item.Highlighted.GetValueOrDefault(),
                                VIN = item.Vin,
                                Franchise = item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                                Distance = distance,
                                Uptime = isSold ? item.LastUpdatedDate.Subtract(item.DateAdded.GetValueOrDefault()).Days : DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                                Miles = validMileage,
                                Price = validPrice,
                                ThumbnailURL = String.Empty,
                                Longtitude = item.Longitude,
                                Latitude = item.Latitude,
                                CarsCom = item.CarsCom.GetValueOrDefault(),
                                AutoTrader = item.AutoTrader.GetValueOrDefault(),
                                CommercialTruck = item.CommercialTruck.GetValueOrDefault(),
                                Carmax = item.CarMax.GetValueOrDefault(),
                                CarMaxListingId = item.CarMaxListingId.GetValueOrDefault(),
                                CarsComListingId = item.CarscomListingId,
                                AutoTraderListingId = item.AutoTraderListingId,
                                AutoTraderDealerId = item.AutoTraderDealerId,
                                CarFax = item.Vin,
                                CommercialTruckListingUrl = item.CommercialTruckListingUrl,
                                BodyType = string.IsNullOrEmpty(item.BodyStyle) ? "other" : item.BodyStyle.Replace(",",""),
                                Seller =
                                    new SellerInfo
                                    {
                                        SellerName = item.Dealershipname,
                                        SellerAddress = item.Address
                                    },
                            };

                            if (chart.AutoTrader && chart.CarsCom)
                            {
                                if (!String.IsNullOrEmpty(item.AutoTraderThumbnailURL))
                                    chart.ThumbnailURL = item.AutoTraderThumbnailURL;
                                if (!String.IsNullOrEmpty(item.CarsComThumbnailURL))
                                    chart.ThumbnailURL = item.CarsComThumbnailURL;
                            }
                            else if (chart.AutoTrader)
                            {
                                  if (!String.IsNullOrEmpty(item.AutoTraderThumbnailURL))
                                    chart.ThumbnailURL = item.AutoTraderThumbnailURL;
                            }
                               else if (chart.CarsCom)
                            {
                                 if (!String.IsNullOrEmpty(item.CarsComThumbnailURL))
                                    chart.ThumbnailURL = item.CarsComThumbnailURL;
                            }

                            else if (chart.Carmax)
                            {
                                 if (!String.IsNullOrEmpty(item.CarsMaxThumbnailURL))
                                    chart.ThumbnailURL = item.CarsMaxThumbnailURL;
                            }
                          
                            list.Add(chart);
                        }
                    }
                }

     
            }
            return list;
        }

        public static List<MarketCarInfo> BuildQueryInner(string latitude, string longitude, List<MarketCarInfo> marketCarInfoList, Func<int, DateTime?, bool> filterFunc)
        {
            var list = new List<MarketCarInfo>();

            if (marketCarInfoList.Any())
            {
                var dealerLatitude = Convert.ToDouble(latitude);

                var dealerLongtitude = Convert.ToDouble(longitude);

                foreach (var item in marketCarInfoList)
                {
                    var distance = CommonHelper.DistanceBetweenPlaces(item.Latitude, item.Longitude, dealerLatitude, dealerLongtitude);
                    {
                        if (filterFunc(distance, item.LastUpdatedDate))
                        {
                            
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public static List<ChartModel> GetMarketCarList(DealershipViewModel dealer, VinMarketEntities context, Vehicle targetCar, ChartSelection savedSelection, int? stateDistance, int chartCarType, ChartGraph chartGraph, bool skipTrimFilter, bool isSold, bool isTruck = false)
        {
            var query = DataHelper.GetNationwideMarketData(MapperFactory.GetMarketCarQuery(context, targetCar.Year, isSold), targetCar.Make, targetCar.Model, targetCar.Trim, false).ToList();
            if (isTruck)
            {
                var truckManagementForm = new Application.VinMarket.Forms.CommonManagement.CommonManagementForm();
                query = query.Union(truckManagementForm.GetNationwideMarketDataForTruck(targetCar.Year.GetValueOrDefault(), targetCar.Make, targetCar.Model, targetCar.TruckCategory.CategoryName)).ToList();
            }

            var unionList = BuildQuery(dealer, savedSelection, query, stateDistance, chartGraph, skipTrimFilter, isSold);
            
            return unionList;
        }

        public static List<ChartModel> GetMarketCarList(int yearFrom, int yearTo, DealershipViewModel dealer, VinMarketEntities context, Vehicle targetCar, ChartSelection savedSelection, int? stateDistance, int chartCarType, ChartGraph chartGraph, bool skipTrimFilter, bool isSold, bool isTruck = false)
        {
            var query = DataHelper.GetNationwideMarketDataExactMatch(MapperFactory.GetMarketCarQueryByYearRange(context,yearFrom,yearTo), targetCar.Make, targetCar.Model, targetCar.Trim, false).ToList();

            if (DataHelper.IsPotentialTruck(targetCar.Make))
            {
                var truckManagementForm = new Application.VinMarket.Forms.CommonManagement.CommonManagementForm();
                query = query.Union(truckManagementForm.GetNationwideMarketDataForTruck(yearFrom, yearTo, targetCar.Make, targetCar.Model, null)).ToList();
            }
           

            var unionList = BuildQuery(dealer, savedSelection, query, stateDistance, chartGraph, skipTrimFilter, isSold);

            return unionList;
        }

        private static List<ChartModel> GetMergedWithMilesAndPriceList(IEnumerable<ChartModel> unionList)
        {
            var groupList = unionList.GroupBy(i => new { i.Miles, i.Price });
          
            var result = new List<ChartModel>();
            foreach (var item in groupList)
            {
                var tempResult = item.ToList();

                if (tempResult.Count() == 1)
                {
                    result.Add(tempResult[0]);
                }
                else
                {
                    var carscomItem = tempResult.FirstOrDefault(i => i.CarsCom);
                    var autoTraderItem = tempResult.FirstOrDefault(i => i.AutoTrader);
                    if (carscomItem != null && autoTraderItem != null)
                    {
                        autoTraderItem.CarsCom = true;
                        autoTraderItem.CarsComListingId = carscomItem.CarsComListingId;
                        result.Add(autoTraderItem);
                    }
                    else if (carscomItem != null)
                    {
                        result.Add(carscomItem);
                    }
                    else if (autoTraderItem != null)
                    {
                        result.Add(autoTraderItem);
                    }
                }
            }
            return result;
        }

        private static List<ArrayList> ConvertToCarsArrayList(IEnumerable<ChartModel> sourceList)
        {
            return sourceList.Select(chartModel => new ArrayList
                                                       {
                                                           chartModel.ListingId,
                                                           chartModel.VIN,
                                                           chartModel.Title.Year,
                                                           chartModel.Title.Make,
                                                           chartModel.Title.Model,
                                                           chartModel.Title.Trim,
                                                           chartModel.Color.Interior,
                                                           chartModel.Color.Exterior,
                                                           chartModel.Miles,
                                                           chartModel.Price,
                                                           chartModel.Latitude,
                                                           chartModel.Longtitude,
                                                           chartModel.AutoTrader,
                                                           chartModel.AutoTraderDealerId,
                                                           chartModel.CarsCom,
                                                           chartModel.CarsComListingId,
                                                           chartModel.CarFax,
                                                           chartModel.Images,
                                                           chartModel.Seller.SellerAddress,
                                                           chartModel.Seller.SellerName,
                                                           chartModel.ThumbnailURL,
                                                           chartModel.IsTargetCar,
                                                           chartModel.Franchise,
                                                           chartModel.Uptime,
                                                           chartModel.Distance,
                                                           chartModel.AutoTraderListingId,
                                                           chartModel.Certified,
                                                           chartModel.CommercialTruck,
                                                           chartModel.CommercialTruckListingUrl,
                                                           chartModel.Title.BodyStyle,
                                                           chartModel.BodyType,
                                                           chartModel.Carmax,
                                                           chartModel.CarMaxListingId,
                                                           chartModel.Highlighted
                                                       }).ToList();
        }

        public static ArrayList ConvertToCarsArrayListItem(ChartGraph.TargetCar chartModel)
        {
            return new ArrayList
            {
                chartModel.ListingId,
                null,
                chartModel.Title.Year,
                chartModel.Title.Make,
                chartModel.Title.Model,
                chartModel.Title.Trim,
                null,
                null,
                chartModel.Mileage,
                chartModel.SalePrice,
                null,
                null,
                chartModel.AutoTrader,
                null,
                chartModel.CarsCom,
                null,
                null,
                null,
                chartModel.Seller.SellerAddress,
                chartModel.Seller.SellerName,
                null,
                true,
                null,
                0,
                chartModel.Distance,
                null,
                chartModel.CommercialTruck,
                string.Empty,
                chartModel.Title.BodyStyle
            };
        }
        
        public static void SetValuesOnMarketToCarList(ChartGraph chartGraph, List<ChartModel> result)
        {
            if (result.Any())
            {
                // set ranking
                var ranking = 1 + result.Select(x => x.Price).Count(price => chartGraph.Target.SalePrice >= price);
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = result.Select(x => x.Price).Count() + 1,
                    MinimumPrice = result.Select(x => x.Price).Min(),
                    AveragePrice = Math.Round(result.Select(x => x.Price).Average()),
                    MaximumPrice = result.Select(x => x.Price).Max(),
                    AverageDays = Math.Round(result.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)
                };

                chartGraph.TypedChartModels = result.OrderBy(x => x.Distance).ToList();
                chartGraph.ChartModels = ConvertToCarsArrayList(chartGraph.TypedChartModels);
                chartGraph.Trims = result.Select(i => new TrimDistance()
                {
                    Distance = i.Distance,
                    Trim = string.IsNullOrEmpty(i.Title.Trim)
                        ? "other"
                        : Regex.Replace(i.Title.Trim, @"\s+", " ").ToLower()
                }).GroupBy(i => i.Trim)
                         .Select(i =>
                                 new TrimDistance() { Trim = i.Key, Distance = i.Min(j => j.Distance) }).ToList();
                if (chartGraph.Target.CommercialTruck)
                {
                    chartGraph.BodyStyles = result.Select(i => new BodyStyleDistance()
                    {
                        Distance = i.Distance,
                        BodyStyle = string.IsNullOrEmpty(i.Title.BodyStyle)
                            ? "other"
                            : Regex.Replace(i.Title.BodyStyle, @"\s+", " ").ToLower()
                    }).GroupBy(i => i.BodyStyle)
                        .Select(i =>
                            new BodyStyleDistance() {BodyStyle = i.Key, Distance = i.Min(j => j.Distance)}).ToList();
                }
                else
                {
                    chartGraph.BodyStyles = result.Select(i => new BodyStyleDistance()
                    {
                        Distance = i.Distance,
                        BodyStyle = i.BodyType
                    }).GroupBy(i => i.BodyStyle)
                      .Select(i =>
                          new BodyStyleDistance() { BodyStyle = i.Key, Distance = i.Min(j => j.Distance) }).ToList();
                }
            }
        }

        public static ChartGraph GetChartDataWithYearRange(int yearFrom, int yearTo, string make, string model, string trim, DealershipViewModel dealer, bool isSold)
        {
            using (var context = new VinMarketEntities())
            {
                var chartGraph = new ChartGraph();

                var savedSelection = new ChartSelection();
                chartGraph.Target = GetCarInformationByYear(yearFrom, make, model, dealer);
                var targetCar = new Vehicle()
                {
                    Year = yearFrom,
                    Make = make,
                    Model = model,
                    Trim = trim,
                    
                    
                };
                var unionList = GetMarketCarList(yearFrom, yearTo, dealer, context, targetCar, savedSelection, null, Constanst.ChartCarType.All, chartGraph, true, false, false);
                
                SetValuesOnMarketToCarList(chartGraph, unionList);
                
                return chartGraph;
            }
        }


        public static ChartGraph GetChartData(DealershipViewModel dealer, VinMarketEntities context, Vehicle targetCar, ChartSelection savedSelection, ChartGraph chartGraph, int? stateDistance, int chartCarType, bool skipTrimFilter, bool isSold, bool isTruck = false)
        {
            if (savedSelection.Trims != null) chartGraph.UserSelectedTrims = savedSelection.Trims.Split(',').ToList();
            if (savedSelection.Options != null) chartGraph.UserSelectedOptions = savedSelection.Options.Split(',').ToList();
            var unionList = GetMarketCarList(dealer, context, targetCar, savedSelection, stateDistance, chartCarType, chartGraph, skipTrimFilter, isSold, isTruck);

            SetValuesOnMarketToCarList(chartGraph, isSold ? GetMergedWithMilesAndPriceList(unionList) : unionList);

            chartGraph.IsCarComs = savedSelection.IsCarsCom;
            chartGraph.IsCertified = savedSelection.IsCertified;
            
            return chartGraph;
        }

        public static ChartGraph GetMarketCarsForNationwideMarketForAppraisal(int appraisalId, DealershipViewModel dealer, ChartSelection cs, int miles, int chartCarType, bool isSold)
        {
            using (var context = new VinMarketEntities())
            {
                var chartGraph = new ChartGraph();
                var targetCar = GetAppraisalTargetCar(appraisalId);
               
                chartGraph.Target = new ChartGraph.TargetCar
                {
                    ListingId = appraisalId,
                    Mileage = targetCar.Mileage.GetValueOrDefault(),
                    SalePrice = targetCar.SalePrice.GetValueOrDefault(),

                    ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.Vehicle.DefaultStockImage)
                                            ? ""
                                            : targetCar.Vehicle.DefaultStockImage
                ,
                    Distance = 0,
                    Title =
                        new TitleInfo
                        {
                            Make = targetCar.Vehicle.Make,
                            Model =
                                targetCar.Vehicle.Model != null
                                    ? targetCar.Vehicle.Model.TrimEnd()
                                    : string.Empty,
                            Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim)
                                    ? "other"
                                    : targetCar.Vehicle.Trim,
                            BodyStyle = targetCar.Vehicle.TruckCategoryId > 0
                                        ? targetCar.Vehicle.TruckCategory.CategoryName
                                        : (string.IsNullOrEmpty(targetCar.Vehicle.BodyType) ? "other" : targetCar.Vehicle.BodyType),
                            Year = targetCar.Vehicle.Year ?? 2013
                        },
                    Certified = targetCar.Certified ?? false,
                    Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim) ? "other" : targetCar.Vehicle.Trim,
                    BodyStyle = targetCar.Vehicle.TruckCategoryId > 0
                                ? targetCar.Vehicle.TruckCategory.CategoryName
                                : (string.IsNullOrEmpty(targetCar.Vehicle.BodyType) ? "other" : targetCar.Vehicle.BodyType),
                    Seller =
                        new SellerInfo
                        {
                            SellerName = targetCar.Dealer.Name,
                            SellerAddress =
                                dealer.Address + " " +
                                dealer.City +
                                "," + dealer.State +
                                " " +
                                dealer.ZipCode
                        },
                    CommercialTruck = IsCommercialTruck(targetCar.Vehicle)
                };

                return GetChartData(dealer, context, targetCar.Vehicle, cs, chartGraph, miles, chartCarType, false, isSold, IsCommercialTruck(targetCar.Vehicle));
            }
        }

        public static ChartGraph GetMarketCarsForNationwideMarket(int listingId, DealershipViewModel dealer)
        {
            return GetMarketCarsForNationwideMarket(listingId, dealer, false);
        }
 
        public static ChartGraph GetMarketCarsForNationwideMarketForAppraisalOnChart(int appraisalId, DealershipViewModel dealer, bool isSold, int? stateDistance = null)
        {
            using (var context = new VinMarketEntities())
            {
                var targetCar = GetAppraisalTargetCar(appraisalId);
                var chartGraph = new ChartGraph
                {
                    Target = GetCarInformationForAppraisal(appraisalId, dealer, targetCar)
                };
                var savedSelection = GetChartSelectionForAppraisal(appraisalId);
                return GetChartData(dealer, context, targetCar.Vehicle, savedSelection, chartGraph, stateDistance, Constanst.ChartCarType.All, true, isSold, IsCommercialTruck(targetCar.Vehicle));
            }
        }
        
        public static ChartGraph GetMarketCarsForNationwideMarket(int listingId, DealershipViewModel dealer, bool isSold, int? stateDistance = null)
        {
            using (var context = new VinMarketEntities())
            {
                var targetCar = GetTargetCar(listingId);
                var chartGraph = new ChartGraph
                {
                    Target = GetCarInformation(listingId, dealer, targetCar)
                };
                var savedSelection = GetChartSelection(listingId);
                return GetChartData(dealer, context, targetCar.Vehicle, savedSelection, chartGraph, stateDistance, Constanst.ChartCarType.All, true, isSold, IsCommercialTruck(targetCar.Vehicle));
            }
        }

        public static ChartGraph GetMarketCarsForNationwideMarketForVinsell(manheim_vehicles manheimVehicle, DealershipViewModel dealer, bool isSold, int? stateDistance = null)
        {
            using (var context = new VinMarketEntities())
            {
                var chartGraph = new ChartGraph
                {
                    Target = GetCarInformationForManehim(dealer, manheimVehicle)
                };

                var vehicle = new Vehicle()
                {
                    Year = chartGraph.Target.Title.Year,
                    Make = chartGraph.Target.Title.Make,
                    Model = chartGraph.Target.Title.Model,
                    Trim = chartGraph.Target.Title.Trim
                };
                var chartSelecetion = new ChartSelection();

                return GetChartData(dealer, context, vehicle, chartSelecetion, chartGraph, stateDistance, Constanst.ChartCarType.All, true, isSold, false);
            }
        }

        public static ChartGraph GetMarketCarsForNationwideMarketOnVinsell(VehicleViewModel vehicle, DealershipViewModel dealer, int? stateDistance = null)
        {
            using (var context = new VinMarketEntities())
            {
                var targetCar = new Inventory()
                {
                    Vehicle = new Vehicle()
                    {
                        Year = vehicle.Year,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Trim = vehicle.Trim,
                        Vin = vehicle.Vin,
                        BodyType = vehicle.BodyStyle
                        
                    },
                    Mileage = vehicle.Mileage,
                    SalePrice = vehicle.Mmr,

                };
                var chartGraph = new ChartGraph
                {
                    Target = new ChartGraph.TargetCar()
                    {
                        ListingId = 0,
                        Mileage = Convert.ToInt32(targetCar.Mileage.GetValueOrDefault()),
                        SalePrice = Convert.ToInt32(targetCar.SalePrice.GetValueOrDefault()),

                     
                        Distance = 0,
                        Title =
                            new TitleInfo
                            {
                                Make = targetCar.Vehicle.Make,
                                Model = targetCar.Vehicle.Model.TrimEnd(),
                                Trim =
                                    string.IsNullOrEmpty(targetCar.Vehicle.Trim)
                                        ? "other"
                                        : targetCar.Vehicle.Trim,
                                Year = targetCar.Vehicle.Year ?? 2013,
                                
                            },
                            BodyStyle = targetCar.Vehicle.BodyType,
                        Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim) ? "other" : targetCar.Vehicle.Trim,
                        Seller =
                            new SellerInfo
                            {
                                SellerName = dealer.DealershipName,
                                SellerAddress =
                                    dealer.Address + " " +
                                    dealer.City +
                                    "," + dealer.State +
                                    " " +
                                    dealer.ZipCode
                            },
                        Vin = targetCar.Vehicle.Vin,
                        CommercialTruck = false
                    }
                };
                var savedSelection = new ChartSelection();
                
                return GetChartData(dealer, context, targetCar.Vehicle, savedSelection, chartGraph, stateDistance,
                    Constanst.ChartCarType.All, true, false);
            }
        }

        public static ChartGraph GetMarketCarsForNationwideMarket(int listingId, DealershipViewModel dealer, ChartSelection savedSelection, int range, int chartCarType)
        {
            using (var context = new VinMarketEntities())
            {
                var targetCar = GetTargetCar(listingId);
                var chartGraph = new ChartGraph
                {
                    Target = GetCarInformation(listingId, dealer, targetCar)
                };
                return GetChartData(dealer, context, targetCar.Vehicle, savedSelection, chartGraph, range, chartCarType, false, false, IsCommercialTruck(targetCar.Vehicle));
            }
        }

        public static ChartGraph.TargetCar GetCarInformation(int listingId, DealershipViewModel dealer, Inventory targetCar)
        {
            return new ChartGraph.TargetCar
            {
                ListingId = listingId,
                Mileage = Convert.ToInt32(targetCar.Mileage.GetValueOrDefault()),
                SalePrice = Convert.ToInt32(targetCar.SalePrice.GetValueOrDefault()),
                ThumbnailImageUrl =
                    String.IsNullOrEmpty(targetCar.Vehicle.DefaultStockImage)
                        ? string.Empty
                        : targetCar.Vehicle.DefaultStockImage,
                Distance = 0,
                Title =
                    new TitleInfo
                    {
                        Make = targetCar.Vehicle.Make,
                        Model = targetCar.Vehicle.Model.TrimEnd(),
                        Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim)
                                ? "other"
                                : targetCar.Vehicle.Trim,
                        Year = targetCar.Vehicle.Year ?? DateTime.Now.Year,
                        BodyStyle = targetCar.Vehicle.BodyType
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim) ? "other" : targetCar.Vehicle.Trim,
                Seller =
                    new SellerInfo
                    {
                        SellerName = dealer.DealershipName,
                        SellerAddress = dealer.Address + " " + dealer.City + "," + dealer.State + " " + dealer.ZipCode
                    },
                Vin = targetCar.Vehicle.Vin,
                CommercialTruck = IsCommercialTruck(targetCar.Vehicle)
            };
        }

        public static ChartGraph.TargetCar GetCarInformationForManehim(DealershipViewModel dealer, manheim_vehicles targetCar)
        {
            return new ChartGraph.TargetCar
            {
                ListingId = targetCar.Id,
                Mileage =targetCar.Mileage.GetValueOrDefault(),
                SalePrice =targetCar.Mmr,
                ThumbnailImageUrl =null,
                Distance = 0,
                Title =
                    new TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = string.IsNullOrEmpty(targetCar.Model) ? string.Empty : targetCar.Model,
                        Trim = string.IsNullOrEmpty(targetCar.Trim) ? string.Empty : targetCar.Trim,
                        Year = targetCar.Year,
                        BodyStyle = targetCar.BodyStyle
                    },
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                  new SellerInfo
                  {
                      SellerName = dealer.DealershipName,
                      SellerAddress = dealer.Address + " " + dealer.City + "," + dealer.State + " " + dealer.ZipCode
                  },
                Vin = targetCar.Vin,
                CommercialTruck = false
            };
        }

        public static ChartGraph.TargetCar GetCarInformationForAppraisal(int listingId, DealershipViewModel dealer, Appraisal targetCar)
        {
            return new ChartGraph.TargetCar
            {
                ListingId = listingId,
                Mileage = Convert.ToInt32(targetCar.Mileage.GetValueOrDefault()),
                SalePrice = Convert.ToInt32(targetCar.SalePrice.GetValueOrDefault()),

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.Vehicle.DefaultStockImage)
                                        ? ""
                                        : targetCar.Vehicle.DefaultStockImage
                ,
                Distance = 0,
                Title =
                    new TitleInfo
                    {
                        Make = targetCar.Vehicle.Make,
                        Model = targetCar.Vehicle.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Vehicle.Trim)
                                ? "other"
                                : targetCar.Vehicle.Trim,
                        Year = targetCar.Vehicle.Year ?? DateTime.Now.Year,
                        BodyStyle = targetCar.Vehicle.BodyType
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Vehicle.Trim) ? "other" : targetCar.Vehicle.Trim,
                Seller =
                    new SellerInfo
                    {
                        SellerName = dealer.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
                Vin = targetCar.Vehicle.Vin,
                CommercialTruck = IsCommercialTruck(targetCar.Vehicle)
            };
        }

        private static ChartGraph.TargetCar GetCarInformationByYear(int year, string make, string model, DealershipViewModel dealer)
        {
            return new ChartGraph.TargetCar
            {
                Mileage = 0,
                SalePrice = 0,
                ThumbnailImageUrl = string.Empty,
                Distance = 0,
                Title =
                    new TitleInfo
                    {
                        Make = make,
                        Model = model,
                        Trim = string.Empty,
                        Year = year
                    },
                Certified = false,
                Trim = string.Empty,
                Seller =
                    new SellerInfo
                    {
                        SellerName = dealer.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };
        }

        private static ChartSelection GetChartSelection(int listingId)
        {
            var contextVinControl = new VincontrolEntities();


            var existingChartSelection =
                contextVinControl.ChartSelections.FirstOrDefault(s => s.ListingId == listingId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory);
            var savedSelection = new ChartSelection();
            if (existingChartSelection != null)
            {
                savedSelection.IsAll = existingChartSelection.IsAll != null && Convert.ToBoolean(existingChartSelection.IsAll);
                savedSelection.IsCarsCom = existingChartSelection.IsCarsCom != null && Convert.ToBoolean(existingChartSelection.IsCarsCom);
                savedSelection.IsCertified = existingChartSelection.IsCertified;
                savedSelection.IsFranchise = existingChartSelection.IsFranchise != null && Convert.ToBoolean(existingChartSelection.IsFranchise);
                savedSelection.IsIndependant = existingChartSelection.IsIndependant != null && Convert.ToBoolean(existingChartSelection.IsIndependant);
                savedSelection.Options = existingChartSelection.Options != null && existingChartSelection.Options != "0"
                                             ? existingChartSelection.Options.ToLower()
                                             : "";
                savedSelection.Trims = existingChartSelection.Trims != null && existingChartSelection.Trims != "0"
                                           ? existingChartSelection.Trims.ToLower()
                                           : "";
            }

            return savedSelection;
        }

        public static List<ManheimTransactionViewModel> GetManheimTransaction(int listingId, short vehicleStatus, int auctionRegion, DealershipViewModel dealer)
        {
            var list = new List<ManheimTransactionViewModel>();

            var context = new VinsellEntities();

            var targetCar = vehicleStatus==Constanst.VehicleStatus.Appraisal ? GetAppraisalTargetCar(listingId).Vehicle : GetTargetCar(listingId).Vehicle;

            var query = DataHelper.GetNationwideMarketData(MapperFactory.GetManheimTransactionQuery(context, targetCar.Year), targetCar.Make, targetCar.Model, targetCar.Trim, false).ToList();

            var manheimAuctions = context.manheim_auctions.ToList();

            foreach (var item in query)
            {
                var transaction = new ManheimTransactionViewModel
                {
                    Odometer = item.Mileage.ToString("##,###"),
                    Price = item.CurrentPrice.ToString("c0"),
                    SaleDate = item.DateAdded.GetValueOrDefault().ToShortDateString(),
                    Engine = item.Engine,
                    Vin = item.Vin,
                    Color = item.ExteriorColor,

                };
                var auction = manheimAuctions.FirstOrDefault(x => x.Code == item.AuctionCode);

                if (auction != null)
                {
                    transaction.Auction = auction.Name;
                    transaction.Region = auction.Region;
                }

                list.Add(transaction);
            }

            if (auctionRegion == Constanst.AuctionRegion.Regional)
            {
                return list.Where(x => x.Region == dealer.Region).ToList();
            }
            return list;
        }

        public static List<ManheimTransactionViewModel> GetManheimTransactionForVinsell(int listingId, short vehicleStatus, int auctionRegion, DealershipViewModel dealer)
        {
            var list = new List<ManheimTransactionViewModel>();

            var context = new VinsellEntities();

            var manheimVehicle = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

            var chartGraph = new ChartGraph
            {
                Target = GetCarInformationForManehim(dealer, manheimVehicle)
            };

            var vehicle = new Vehicle()
            {
                Year = chartGraph.Target.Title.Year,
                Make = chartGraph.Target.Title.Make,
                Model = chartGraph.Target.Title.Model,
                Trim = chartGraph.Target.Title.Trim
            };

            var query = DataHelper.GetNationwideMarketData(MapperFactory.GetManheimTransactionQuery(context, vehicle.Year), vehicle.Make, vehicle.Model, vehicle.Trim, false).ToList();

            var manheimAuctions = context.manheim_auctions.ToList();

            foreach (var item in query)
            {
                var transaction = new ManheimTransactionViewModel
                {
                    Odometer = item.Mileage.ToString("##,###"),
                    Price = item.CurrentPrice.ToString("c0"),
                    SaleDate = item.DateAdded.GetValueOrDefault().ToShortDateString(),
                    Engine = item.Engine,
                    Vin = item.Vin,
                    Color = item.ExteriorColor,

                };
                var auction = manheimAuctions.FirstOrDefault(x => x.Code == item.AuctionCode);

                if (auction != null)
                {
                    transaction.Auction = auction.Name;
                    transaction.Region = auction.Region;
                }

                list.Add(transaction);
            }

            if (auctionRegion == Constanst.AuctionRegion.Regional)
            {
                return list.Where(x => x.Region == dealer.Region).ToList();
            }
            return list;
        }

        public static List<AuctionTransactionStatistic> GetManheimTransactionWebApi(int listingId,DealershipViewModel dealer, int vehicleStatus)
        {
            

            if (vehicleStatus == Constanst.VehicleStatus.Appraisal)
            {
                var targetCar = GetAppraisalTargetCar(listingId);

                var manheimStaticList = GetManehimTransactionList(listingId, vehicleStatus, dealer, targetCar.Vehicle);

                return manheimStaticList;    
            }
            else if (vehicleStatus == Constanst.VehicleStatus.Vinsell)
            {
                using (var context = new VinsellEntities())
                {
                    var inventory = context.manheim_vehicles.FirstOrDefault(x => x.Id == listingId);

                    if (inventory != null)
                    {
                        var vehicle = new Vehicle()
                        {
                            Year = inventory.Year,
                            Make = inventory.Make,
                            Model = inventory.Model,
                            Trim = inventory.Trim
                        };
                        var manheimStaticList = GetManehimTransactionList(listingId, vehicleStatus, dealer, vehicle);

                        return manheimStaticList;
                    }
                }
                
                return new List<AuctionTransactionStatistic>();

            }
            else
            {
                var targetCar = GetTargetCar(listingId);

                var manheimStaticList = GetManehimTransactionList(listingId,vehicleStatus, dealer, targetCar.Vehicle);

                return manheimStaticList;    
            }

        }

        public static List<AuctionTransactionStatistic> GetManehimTransactionList(int listingId,int vehicleStatus, DealershipViewModel dealer, Vehicle targetCar)
        {
            using (var context = new VinsellEntities())
            {
                var query = DataHelper.GetNationwideMarketData(MapperFactory.GetManheimTransactionQuery(context, targetCar.Year), targetCar.Make, targetCar.Model, targetCar.Trim, false).ToList();

                var unionList = BuildQueryManheim(context, query);

                return SetValuesOnManheimTransactionToCarList(listingId, vehicleStatus, dealer, unionList);
            }
        }

        public static List<AuctionTransactionStatistic> SetValuesOnManheimTransactionToCarList(int listingId,int vehicleStatus, DealershipViewModel dealer, List<ManheimChartModel> result)
        {
            var returnList = new List<AuctionTransactionStatistic>();
            if (result.Any())
            {
                returnList.Add(new AuctionTransactionStatistic()
                {
                    RegionName = "Nation",
                    AuctionRegion = Constanst.AuctionRegion.National,
                    AvgAuctionPrice = (decimal) Math.Round(result.Select(x => x.Price).Average()),
                    NoOfVehicles = result.Count,
                    AvgOdometer = (decimal)Math.Round(result.Select(x => x.Miles).Average()),
                    ListingId = listingId,
                    VehicleStatus=vehicleStatus
                });

                var dealerRegion = result.Where(x => x.ManheimRegion == dealer.Region).ToList();

                if (dealerRegion.Any())
                {
                    returnList.Add(new AuctionTransactionStatistic()
                    {
                        RegionName = dealer.Region,
                        AuctionRegion = Constanst.AuctionRegion.Regional,
                        AvgAuctionPrice = (decimal)Math.Round(dealerRegion.Select(x => x.Price).Average()),
                        NoOfVehicles = dealerRegion.Count,
                        AvgOdometer = (decimal)Math.Round(dealerRegion.Select(x => x.Miles).Average()),
                        ListingId = listingId,
                        VehicleStatus = vehicleStatus
                    });
                }
                else
                {
                    returnList.Add(new AuctionTransactionStatistic());
                }

            }

            return returnList;
        }


        public static List<ManheimChartModel> BuildQueryManheim(VinsellEntities context, List<MarketCarInfo> marketCarInfoList)
        {
            //var auctions = context.manheim_auctions.ToList();
            var list = new List<ManheimChartModel>();

            if (marketCarInfoList.Any())
            {
                foreach (var item in marketCarInfoList)
                {
                    var chart = new ManheimChartModel
                    {
                        ListingId = item.RegionalListingId,

                        Miles = item.Mileage,
                        Price = item.CurrentPrice,
                      

                    };
                    //var auction = auctions.FirstOrDefault(x => x.Code == item.AuctionCode);
                    //if (auction != null)
                    //{
                    //    chart.ManheimRegion = auction.Region;
                    //}

                    list.Add(chart);
                }

            }

            return list;
        }


        private static ChartGraph GetChartDataByYearRange(DealershipViewModel dealer, VinMarketEntities context, Vehicle targetCar, ChartSelection savedSelection, ChartGraph chartGraph, int? stateDistance, int chartCarType, bool skipTrimFilter, int yearFrom, int yearTo, bool isSold)
        {
           
            var unionList = GetMarketCarListByYearRange(dealer, context, targetCar, savedSelection, stateDistance,
                                                        chartCarType, chartGraph, skipTrimFilter, yearFrom, yearTo, isSold);
            SetValuesOnMarketToCarList(chartGraph, unionList);
            return chartGraph;
        }

        private static List<ChartModel> GetMarketCarListByYearRange(DealershipViewModel dealer, VinMarketEntities context, Vehicle targetCar, ChartSelection savedSelection, int? stateDistance, int chartCarType, ChartGraph chartGraph, bool skipTrimFilter, int yearFrom, int yearTo, bool isSold)
        {
            var unionList = new List<ChartModel>();
            var bIgnoredTrim = targetCar.Trim == string.Empty ? true : false;

            if (chartCarType == Constanst.ChartCarType.All)
            {
                var query = MapperFactory.GetMarketCarQueryByYearRange(context, yearFrom, yearTo);

                query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model, targetCar.Trim, bIgnoredTrim);
                unionList = BuildQuery(dealer, savedSelection, query.ToList(), stateDistance, chartGraph, skipTrimFilter, isSold);
            }
           
            return unionList;
        }

        public static SoldInfo GetSoldCarWithin90DaysPeriod(int? stateDistance, int year, string make, string model, string trim, DealershipViewModel dealer, ChartSelection savedSelection)
        {
            var chartGraph = new ChartGraph();
            //Need Make, Model and Trim
            using (var context = new VinMarketEntities())
            {

                var query = MapperFactory.GetMarketCarQueryForSold(context, year, 90);
                var result = DataHelper.GetNationwideMarketData(query, make, model, trim, false).ToList();
                var last60To90Days = GetCountWithDistinctMilesAndPrice(stateDistance, dealer, savedSelection, result, chartGraph, 90, 60);
                var last30To60Days = GetCountWithDistinctMilesAndPrice(stateDistance, dealer, savedSelection, result, chartGraph, 60, 30);
                var last30Days = GetCountWithDistinctMilesAndPrice(stateDistance, dealer, savedSelection, result, chartGraph, 30, 0);
                return new SoldInfo() { Last30Days = last30Days, Last30To60Days = last30To60Days, Last60To90Days = last60To90Days };
            }
        }

        //public static RecommendationSoldInfo GetRecommendationSoldCarWithin90DaysPeriod(int? stateDistance, int year, string make, string model, string latitude, string longitude)
        //{
        //    var data = new RecommendationSoldInfo();
        //    try
        //    {
        //        using (var context = new VinMarketEntities())
        //        {
        //            var query = MapperFactory.GetMarketCarQueryForSold(context, year, 90);
        //            var result = DataHelper.GetNationwideMarketData(query, make, model, string.Empty, false).ToList();

        //            var last90Days = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-90);
        //            var last60Days = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-60);
        //            var last30Days = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-30);
        //            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        //            data.Last60To90Days = BuildQueryInner(latitude, longitude, result, (distance, date) => (!stateDistance.HasValue || distance <= stateDistance.Value) && date >= last90Days && date <= last60Days);
        //            data.Last30To60Days = BuildQueryInner(latitude, longitude, result, (distance, date) => (!stateDistance.HasValue || distance <= stateDistance.Value) && date >= last60Days && date <= last30Days);
        //            data.Last30Days = BuildQueryInner(latitude, longitude, result, (distance, date) => (!stateDistance.HasValue || distance <= stateDistance.Value) && date >= last30Days && date <= currentDate);
        //        }
        //    }
        //    catch (Exception){ }

        //    return data;
        //}

        public static List<MarketCarInfo> GetRecommendationSoldCarWithin90DaysPeriod(decimal latitude, decimal longitude, int distance = 100)
        {
            try
            {
                using (var context = new VinMarketEntities())
                {
                    var query =
                        context.GetSoldOutVehiclesByDistance(Convert.ToDecimal(latitude), Convert.ToDecimal(longitude),
                            distance).AsQueryable();
                    if (!query.Any()) return new List<MarketCarInfo>();

                    return query.AsEnumerable().Select(i => new MarketCarInfo()
                    {
                        Year = i.Year,
                        Make = i.Make,
                        Model = i.Model,
                        Trim = i.Trim,
                        SoldDays = i.SoldDays.GetValueOrDefault()
                    }).ToList();

                }
            }
            catch (Exception)
            {
                return new List<MarketCarInfo>()
                {
                    new MarketCarInfo(){ Year = 2000, Make = "Mercedes-Benz", Model = "S500", Trim = "SA", SoldDays = 25, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 2000, Make = "Mercedes-Benz", Model = "S500", Trim = "Other", SoldDays = 35, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 2000, Make = "Mercedes-Benz", Model = "S500", Trim = "Other", SoldDays = 65, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 1998, Make = "BMW", Model = "Z3", Trim = "1.9 roadster", SoldDays = 25, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 1998, Make = "BMW", Model = "Z3", Trim = "1.9 roadster", SoldDays = 35, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 1998, Make = "BMW", Model = "Z3", Trim = "1.9 roadster", SoldDays = 65, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 1998, Make = "BMW", Model = "Z1", Trim = "1.9 roadster", SoldDays = 25, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 1998, Make = "BMW", Model = "Z1", Trim = "1.9 roadster", SoldDays = 35, MarketSupply = 10 },
                    new MarketCarInfo(){ Year = 1998, Make = "BMW", Model = "Z1", Trim = "Other", SoldDays = 65, MarketSupply = 10 },
                };
            }

        }

        private static int GetCountWithDistinctMilesAndPrice(int? stateDistance, DealershipViewModel dealer, ChartSelection savedSelection, List<MarketCarInfo> result, ChartGraph chartGraph, int daysStart, int daysEnd)
        {
            if (daysStart < daysEnd) throw new ArgumentException("DaysStart should be bigger than DaysEnd");
            var minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-1 * daysStart);
            var maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-1 * daysEnd);
            var marketResult = MarketHelper.BuildQueryInner(dealer, savedSelection, result, chartGraph, false, true, (distance, date) => (!stateDistance.HasValue || distance <= stateDistance.Value) && date >= minDate && date <= maxDate);
            return marketResult.GroupBy(i=>new{i.Miles, i.Price}).Count();
        }

        private static bool IsCommercialTruck(Vehicle vehicle)
        {
            return vehicle.VehicleType.Equals(Constanst.VehicleType.Truck) && vehicle.TruckCategoryId > 0 &&
                   vehicle.TruckClassId > 0 && !String.IsNullOrEmpty(vehicle.TruckType);
        }
      
    }
}
