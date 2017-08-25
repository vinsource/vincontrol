using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.Models;
using System.Web.Mvc;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public static class LinqHelper
    {
        //APPRAISAL
        private static ChartSelection GetChartSelectionAutoTraderForAppraisal(int appraisalId)
        {
            var contextVinControl = new whitmanenterprisewarehouseEntities();

            var existingChartSelection = contextVinControl.vincontrolchartselections.FirstOrDefault(s => s.listingId == appraisalId && s.screen == Constanst.Appraisal && s.sourceType == Constanst.AutoTrader);
            var savedSelection = new ChartSelection();
            if (existingChartSelection != null)
            {
                savedSelection.IsAll = existingChartSelection.isAll != null && Convert.ToBoolean(existingChartSelection.isAll);
                savedSelection.IsCarsCom = existingChartSelection.isCarsCom != null && Convert.ToBoolean(existingChartSelection.isCarsCom);
                savedSelection.IsCertified = existingChartSelection.isCertified != null && Convert.ToBoolean(existingChartSelection.isCertified);
                savedSelection.IsFranchise = existingChartSelection.isFranchise != null && Convert.ToBoolean(existingChartSelection.isFranchise);
                savedSelection.IsIndependant = existingChartSelection.isIndependant != null && Convert.ToBoolean(existingChartSelection.isIndependant);
                savedSelection.Options = existingChartSelection.options != null && existingChartSelection.options != "0"
                                             ? existingChartSelection.options.ToLower()
                                             : "";
                savedSelection.Trims = existingChartSelection.trims != null && existingChartSelection.trims != "0"
                                           ? existingChartSelection.trims.ToLower()
                                           : "";
            }

            return savedSelection;
        }

        private static ChartSelection GetChartSelectionCarsComForAppraisal(int appraisalId)
        {
            using (var contextVinControl = new whitmanenterprisewarehouseEntities())
            {

                var existingChartSelection = contextVinControl.vincontrolchartselections.FirstOrDefault(s => s.listingId == appraisalId && s.screen == Constanst.Appraisal && s.sourceType == Constanst.CarsCom);
                var savedSelection = new ChartSelection();
                if (existingChartSelection != null)
                {
                    savedSelection.IsAll = existingChartSelection.isAll != null && Convert.ToBoolean(existingChartSelection.isAll);
                    savedSelection.IsCarsCom = existingChartSelection.isCarsCom != null && Convert.ToBoolean(existingChartSelection.isCarsCom);
                    savedSelection.IsCertified = existingChartSelection.isCertified != null &&
                                                 Convert.ToBoolean(existingChartSelection.isCertified);
                    savedSelection.IsFranchise = existingChartSelection.isFranchise != null &&
                                                 Convert.ToBoolean(existingChartSelection.isFranchise);
                    savedSelection.IsIndependant = existingChartSelection.isIndependant != null &&
                                                   Convert.ToBoolean(existingChartSelection.isIndependant);
                    savedSelection.Options = existingChartSelection.options != null &&
                                             existingChartSelection.options != "0"
                                                 ? existingChartSelection.options.ToLower()
                                                 : "";
                    savedSelection.Trims = existingChartSelection.trims != null && existingChartSelection.trims != "0"
                                               ? existingChartSelection.trims.ToLower()
                                               : "";
                }

                return savedSelection;
            }
        }

        public static ChartGraph GetMarketCarsOnAutoTraderWithin100MilesRadiusForAppraisalChart(int appraisalId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetAppraisalTargetCar(appraisalId);

            var savedSelection = GetChartSelectionAutoTraderForAppraisal(appraisalId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);

            if (savedSelection.IsFranchise)
                query = query.Where(i => i.Franchise.HasValue && i.Franchise.Value);
            else if (savedSelection.IsIndependant)
                query = query.Where(i => i.Franchise.HasValue && !i.Franchise.Value);

            if (!string.IsNullOrEmpty(savedSelection.Trims))
            {
                var arrayTrim = String.Format(",{0},", savedSelection.Trims.Replace(" ", "").ToLower());
                var trimEqualOther = savedSelection.Trims.ToLower().Equals("other");
                var trimContainOther = savedSelection.Trims.ToLower().Contains("other");
                query = trimEqualOther
                            ? query.Where(i => string.IsNullOrEmpty(i.Trim.Trim()))
                            : trimContainOther
                                  ? query.Where(
                                      i =>
                                      string.IsNullOrEmpty(i.Trim.Trim()) ||
                                      arrayTrim.Contains("," + i.Trim.Replace(" ", "").ToLower() + ","))
                                  : query.Where(
                                      i =>
                                      !string.IsNullOrEmpty(i.Trim.Trim()) &&
                                      arrayTrim.Contains("," + i.Trim.Replace(" ", "").ToLower() + ","));
            }
            
            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim) ? "other" : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    int distance = CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                                      Convert.ToDouble(item.Longitude),
                                                                      Convert.ToDouble(dealer.Latitude),
                                                                      Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;
                   
                    int validPrice = item.CurrentPrice;
                    
                    if (validMileage > 0 && validPrice > 0 && distance <= 100)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                                    ? ""
                                    : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                          ? ""
                          : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }
            
            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.idAppraisal,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.DefaultImageUrl)
                       ? "" : targetCar.DefaultImageUrl
                                                     ,
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model =targetCar.Model,
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                             
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    }
             
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice >= price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count() + 1,
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays = Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static ChartGraph GetMarketCarsOnAutoTraderForLocalMarketForAppraisal(int appraisalId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetAppraisalTargetCar(appraisalId);

            var savedSelection = GetChartSelectionAutoTraderForAppraisal(appraisalId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();
            
            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetStateMarketData(query, targetCar.Make, targetCar.Model,dealer.State,targetCar.Trim);
            
            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;
                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                                                              ? ""
                                                              : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
? ""
: item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }
            
            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.idAppraisal,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.DefaultImageUrl)
                       ? "" : targetCar.DefaultImageUrl
                        ,
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static ChartGraph GetMarketCarsOnAutoTraderForNationwideMarketForAppraisal(int appraisalId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetAppraisalTargetCar(appraisalId);

            var savedSelection = GetChartSelectionAutoTraderForAppraisal(appraisalId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();
            
            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);

         
            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;
                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                                                              ? ""
                                                              : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
? ""
: item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }
            
            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.idAppraisal,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.DefaultImageUrl)
                       ? "" : targetCar.DefaultImageUrl
                                                     ,
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static ChartGraph GetMarketCarsOnCarsComForLocalMarketForAppraisal(int appraisalId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetAppraisalTargetCar(appraisalId);

            var savedSelection = GetChartSelectionCarsComForAppraisal(appraisalId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();
            
            var query = MapperFactory.GetCarsComMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetStateMarketData(query, targetCar.Make, targetCar.Model, dealer.State,targetCar.Trim);

            if (targetCar.BodyType.ToLower().Contains("coupe"))
            {
                query = query.Where(x => x.BodyStyle.Contains("coupe"));
            }
            else if (targetCar.BodyType.ToLower().Contains("convertible"))
            {
                query = query.Where(x => x.BodyStyle.Contains("convertible"));
            }

            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.CarsComThumbnailURL)
                                    ? ""
                                    : item.CarsComThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                                                              ? ""
                                                              : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
? ""
: item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }

            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.idAppraisal,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.DefaultImageUrl)
                       ? "" : targetCar.DefaultImageUrl
                                                     ,
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                           dealer.Address + " " +
                           dealer.City +
                           "," + dealer.State +
                           " " +
                           dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static ChartGraph GetMarketCarsOnCarsComForNationwideMarketForAppraisal(int appraisalId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetAppraisalTargetCar(appraisalId);

            var savedSelection = GetChartSelectionCarsComForAppraisal(appraisalId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            string modelWord = DataHelper.FilterCarModelForMarket(targetCar.Model);

            var query = MapperFactory.GetCarsComMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);
            
            if (targetCar.BodyType.ToLower().Contains("coupe"))
            {
                query = query.Where(x => x.BodyStyle.Contains("coupe"));
            }
            else if (targetCar.BodyType.ToLower().Contains("convertible"))
            {
                query = query.Where(x => x.BodyStyle.Contains("convertible"));
            }

            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.CarsComThumbnailURL)
                                    ? ""
                                    : item.CarsComThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }

            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.idAppraisal,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.DefaultImageUrl)
                       ? "" : targetCar.DefaultImageUrl
                                                     ,
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }


        //GET TARGET CAR
        private static whitmanenterpriseappraisal GetAppraisalTargetCar(int appraisalId)
        {
            whitmanenterpriseappraisal targetCar;
            using (var contextVinControl = new whitmanenterprisewarehouseEntities())
            {
                targetCar = contextVinControl.whitmanenterpriseappraisals.FirstOrDefault((x => x.idAppraisal == appraisalId));
            }

            return targetCar;

        }

        private static vincontrolbannercustomer GetBannerTargetCar(int tradeId)
        {
            vincontrolbannercustomer targetCar;
            using (var contextVinControl = new whitmanenterprisewarehouseEntities())
            {
                targetCar = contextVinControl.vincontrolbannercustomers.FirstOrDefault((x => x.TradeInCustomerId == tradeId));
            }

            return targetCar;
        }

        private static whitmanenterprisedealershipinventory GetTargetCar(int listingId)
        {
            whitmanenterprisedealershipinventory targetCar;
            using (var contextVinControl = new whitmanenterprisewarehouseEntities())
            {
                targetCar = contextVinControl.whitmanenterprisedealershipinventories.FirstOrDefault((x => x.ListingID == listingId));

                if (targetCar == null)
                {
                    targetCar = new whitmanenterprisedealershipinventory();
                    // check to see if the car is in wholesale
                    var wholesalevehicle =
                        contextVinControl.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == listingId);
                    if (wholesalevehicle != null)
                    {
                        targetCar.VINNumber = wholesalevehicle.VINNumber;
                        targetCar.Trim = wholesalevehicle.Trim;
                        targetCar.ModelYear = wholesalevehicle.ModelYear;
                        targetCar.Make = wholesalevehicle.Make;
                        targetCar.Model = wholesalevehicle.Model;
                        targetCar.Certified = wholesalevehicle.Certified;
                        targetCar.DealershipName = wholesalevehicle.DealershipName;
                        targetCar.DealershipId = wholesalevehicle.DealershipId;
                        targetCar.DealershipZipCode = wholesalevehicle.DealershipZipCode;
                        targetCar.DealershipState = wholesalevehicle.DealershipState;
                        targetCar.DealershipPhone = wholesalevehicle.DealershipPhone;
                        targetCar.DealershipCity = wholesalevehicle.DealershipCity;
                        targetCar.DealershipAddress = wholesalevehicle.DealershipAddress;

                    }
                    else
                    {
                        var soldOutVehicle =
                    contextVinControl.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == listingId);
                        if (soldOutVehicle != null)
                        {
                            targetCar.VINNumber = soldOutVehicle.VINNumber;
                            targetCar.Trim = soldOutVehicle.Trim;
                            targetCar.ModelYear = soldOutVehicle.ModelYear;
                            targetCar.Make = soldOutVehicle.Make;
                            targetCar.Model = soldOutVehicle.Model;
                            targetCar.Certified = soldOutVehicle.Certified;
                            targetCar.DealershipName = soldOutVehicle.DealershipName;
                            targetCar.DealershipId = soldOutVehicle.DealershipId;
                            targetCar.DealershipZipCode = soldOutVehicle.DealershipZipCode;
                            targetCar.DealershipState = soldOutVehicle.DealershipState;
                            targetCar.DealershipPhone = soldOutVehicle.DealershipPhone;
                            targetCar.DealershipCity = soldOutVehicle.DealershipCity;
                            targetCar.DealershipAddress = soldOutVehicle.DealershipAddress;

                        }
                    }
                }

                return targetCar;
            }
        }

        private static whitmanenterprisedealershipinventory GetTargetCarForWholeSale(int listingId)
        {
            whitmanenterprisedealershipinventory targetCar;
            using (var contextVinControl = new whitmanenterprisewarehouseEntities())
            {
                targetCar = new whitmanenterprisedealershipinventory();
                var wholesalevehicle =
                       contextVinControl.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == listingId);
                if (wholesalevehicle != null)
                {
                    targetCar.VINNumber = wholesalevehicle.VINNumber;
                    targetCar.Trim = wholesalevehicle.Trim;
                    targetCar.ModelYear = wholesalevehicle.ModelYear;
                    targetCar.Make = wholesalevehicle.Make;
                    targetCar.Model = wholesalevehicle.Model;
                    targetCar.Certified = wholesalevehicle.Certified;
                    targetCar.DealershipName = wholesalevehicle.DealershipName;
                    targetCar.DealershipId = wholesalevehicle.DealershipId;
                    targetCar.DealershipZipCode = wholesalevehicle.DealershipZipCode;
                    targetCar.DealershipState = wholesalevehicle.DealershipState;
                    targetCar.DealershipPhone = wholesalevehicle.DealershipPhone;
                    targetCar.DealershipCity = wholesalevehicle.DealershipCity;
                    targetCar.DealershipAddress = wholesalevehicle.DealershipAddress;

                }
               
                return targetCar;
            }
        }

        //AutoTrader

        private static ChartSelection GetChartSelectionForAutoTrader(int listingId)
        {
            var contextVinControl = new whitmanenterprisewarehouseEntities();
            
            var existingChartSelection =
                contextVinControl.vincontrolchartselections.FirstOrDefault(s => s.listingId == listingId && s.screen == Constanst.Inventory && s.sourceType == Constanst.AutoTrader);
            var savedSelection = new ChartSelection();
            if (existingChartSelection != null)
            {
                savedSelection.IsAll = existingChartSelection.isAll != null && Convert.ToBoolean(existingChartSelection.isAll);
                savedSelection.IsCarsCom = existingChartSelection.isCarsCom != null && Convert.ToBoolean(existingChartSelection.isCarsCom);
                savedSelection.IsCertified = existingChartSelection.isCertified != null && Convert.ToBoolean(existingChartSelection.isCertified);
                savedSelection.IsFranchise = existingChartSelection.isFranchise != null && Convert.ToBoolean(existingChartSelection.isFranchise);
                savedSelection.IsIndependant = existingChartSelection.isIndependant != null && Convert.ToBoolean(existingChartSelection.isIndependant);
                savedSelection.Options = existingChartSelection.options != null && existingChartSelection.options != "0"
                                             ? existingChartSelection.options.ToLower()
                                             : "";
                savedSelection.Trims = existingChartSelection.trims != null && existingChartSelection.trims != "0"
                                           ? existingChartSelection.trims.ToLower()
                                           : "";
            }

            return savedSelection;
        }

        public static List<ChartModel> GetMarketCarsOnAutoTraderRadiusForBanner(int tradeAutoId)
        {

            var context = new vincontrolscrappingEntities();

            var targetCar = GetBannerTargetCar(tradeAutoId);

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.Year);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);
            
            if (query.Any())
            {

             
                foreach (var item in query)
                {

                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;
                  

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Miles = validMileage,
                            Price = validPrice,
                           
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }


            return list;

        }

        public static ChartGraph GetMarketCarsOnAutoTraderWithin100MilesRadius(int ListingId, DealershipViewModel dealer)
        {

            var context = new vincontrolscrappingEntities();

            var targetCar = GetTargetCar(ListingId);

            var savedSelection = GetChartSelectionForAutoTrader(ListingId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);

            if (savedSelection.IsFranchise)
                query = query.Where(i => i.Franchise.HasValue && i.Franchise.Value);
            else if (savedSelection.IsIndependant)
                query = query.Where(i => i.Franchise.HasValue && !i.Franchise.Value);

            if (!string.IsNullOrEmpty(savedSelection.Trims))
            {
                var arrayTrim = String.Format(",{0},", savedSelection.Trims.Replace(" ", "").ToLower());
                var trimEqualOther = savedSelection.Trims.ToLower().Equals("other");
                var trimContainOther = savedSelection.Trims.ToLower().Contains("other");
                query = trimEqualOther
                            ? query.Where(i => string.IsNullOrEmpty(i.Trim.Trim()))
                            : trimContainOther
                                  ? query.Where(
                                      i =>
                                      string.IsNullOrEmpty(i.Trim.Trim()) ||
                                      arrayTrim.Contains("," + i.Trim.Replace(" ", "").ToLower() + ","))
                                  : query.Where(
                                      i =>
                                      !string.IsNullOrEmpty(i.Trim.Trim()) &&
                                      arrayTrim.Contains("," + i.Trim.Replace(" ", "").ToLower() + ","));
            }
      
            if (query.Any())
            {

                var table = new HashSet<string>();



                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }



                foreach (var item in query)
                {
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0 && distance <= 100)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }

            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.ListingID,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.ThumbnailImageURL)
                       ? String.IsNullOrEmpty(targetCar.DefaultImageUrl) ? "" : targetCar.DefaultImageUrl
                                                     : targetCar.ThumbnailImageURL.Split(new string[] { "," },
                                                                                         StringSplitOptions.None).
                                                           First(),
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model,
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice >= price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count() + 1,
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };


                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;

        }


        public static ChartGraph GetMarketCarsOnAutoTraderWithin100MilesRadiusForWholeSaleChart(int ListingId, DealershipViewModel dealer)
        {

            var context = new vincontrolscrappingEntities();

            var targetCar = GetTargetCarForWholeSale(ListingId);

            //var savedSelection = GetChartSelectionForAutoTrader(ListingId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model, targetCar.Trim);

            //if (savedSelection.IsFranchise)
            //    query = query.Where(i => i.Franchise.HasValue && i.Franchise.Value);
            //else if (savedSelection.IsIndependant)
            //    query = query.Where(i => i.Franchise.HasValue && !i.Franchise.Value);

            //if (!string.IsNullOrEmpty(savedSelection.Trims))
            //{
            //    var arrayTrim = String.Format(",{0},", savedSelection.Trims.Replace(" ", "").ToLower());
            //    var trimEqualOther = savedSelection.Trims.ToLower().Equals("other");
            //    var trimContainOther = savedSelection.Trims.ToLower().Contains("other");
            //    query = trimEqualOther
            //                ? query.Where(i => string.IsNullOrEmpty(i.Trim.Trim()))
            //                : trimContainOther
            //                      ? query.Where(
            //                          i =>
            //                          string.IsNullOrEmpty(i.Trim.Trim()) ||
            //                          arrayTrim.Contains("," + i.Trim.Replace(" ", "").ToLower() + ","))
            //                      : query.Where(
            //                          i =>
            //                          !string.IsNullOrEmpty(i.Trim.Trim()) &&
            //                          arrayTrim.Contains("," + i.Trim.Replace(" ", "").ToLower() + ","));
            //}

            if (query.Any())
            {

                var table = new HashSet<string>();



                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }



                foreach (var item in query)
                {
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0 && distance <= 100)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = string.IsNullOrEmpty(item.Trim) ? "other" : item.Trim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }

            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.ListingID,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.ThumbnailImageURL)
                       ? String.IsNullOrEmpty(targetCar.DefaultImageUrl) ? "" : targetCar.DefaultImageUrl
                                                     : targetCar.ThumbnailImageURL.Split(new string[] { "," },
                                                                                         StringSplitOptions.None).
                                                           First(),
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model,
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice >= price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count() + 1,
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };


                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;

        }

        public static ChartGraph GetMarketCarsOnAutoTraderForLocalMarket(int listingId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetTargetCar(listingId);

            //var savedSelection = GetChartSelectionForAutoTrader(listingId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            var result = DataHelper.GetStateMarketData(query, targetCar.Make, targetCar.Model, dealer.State, targetCar.Trim).ToList();
            //var result = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model, targetCar.Trim).ToList();
            
            if (/*query.Any()*/result.Count > 0)
            {
                var table = new HashSet<string>();

                foreach (var row in result)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in result)
                {
                    string filtertrim = string.IsNullOrEmpty(item.Trim)
                                          ? "other"
                                          : Regex.Replace(item.Trim, @"\s+", " ").ToLower();

                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = filtertrim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }

            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.ListingID,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.ThumbnailImageURL)
                       ? String.IsNullOrEmpty(targetCar.DefaultImageUrl) ? "" : targetCar.DefaultImageUrl
                                                     : targetCar.ThumbnailImageURL.Split(new string[] { "," },
                                                                                         StringSplitOptions.None).
                                                           First(),
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }
            else
            {
                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = 0,
                    MinimumPrice = "0",
                    AveragePrice = "0",
                    MaximumPrice = "0",
                    AverageDays =
                       "0",

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static ChartGraph GetMarketCarsOnAutoTraderForNationwideMarket(int listingId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetTargetCar(listingId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetAutoTraderMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);
            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    string filtertrim = string.IsNullOrEmpty(item.Trim)
                                        ? "other"
                                        : Regex.Replace(item.Trim, @"\s+", " ").ToLower();
                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = filtertrim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.AutoTraderThumbnailURL)
                                    ? ""
                                    : item.AutoTraderThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }



            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.ListingID,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.ThumbnailImageURL)
                       ? String.IsNullOrEmpty(targetCar.DefaultImageUrl) ? "" : targetCar.DefaultImageUrl
                                                     : targetCar.ThumbnailImageURL.Split(new string[] { "," },
                                                                                         StringSplitOptions.None).
                                                           First(),
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };


                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        //***********************************************CarsCom*************************************************************

        private static ChartSelection GetChartSelectionForCarsCom(int listingId)
        {
            var contextVinControl = new whitmanenterprisewarehouseEntities();


            var existingChartSelection =
                contextVinControl.vincontrolchartselections.FirstOrDefault(s => s.listingId == listingId && s.screen == Constanst.Inventory && s.sourceType == Constanst.CarsCom);
            var savedSelection = new ChartSelection();
            if (existingChartSelection != null)
            {
                savedSelection.IsAll = existingChartSelection.isAll != null && Convert.ToBoolean(existingChartSelection.isAll);
                savedSelection.IsCarsCom = existingChartSelection.isCarsCom != null && Convert.ToBoolean(existingChartSelection.isCarsCom);
                savedSelection.IsCertified = existingChartSelection.isCertified != null && Convert.ToBoolean(existingChartSelection.isCertified);
                savedSelection.IsFranchise = existingChartSelection.isFranchise != null && Convert.ToBoolean(existingChartSelection.isFranchise);
                savedSelection.IsIndependant = existingChartSelection.isIndependant != null && Convert.ToBoolean(existingChartSelection.isIndependant);
                savedSelection.Options = existingChartSelection.options != null && existingChartSelection.options != "0"
                                             ? existingChartSelection.options.ToLower()
                                             : "";
                savedSelection.Trims = existingChartSelection.trims != null && existingChartSelection.trims != "0"
                                           ? existingChartSelection.trims.ToLower()
                                           : "";
            }

            return savedSelection;
        }

       
        public static ChartGraph GetMarketCarsOnCarsComForLocalMarket(int listingId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetTargetCar(listingId);

            //var savedSelection = GetChartSelectionForCarsCom(listingId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();

            var query = MapperFactory.GetCarsComMarketCarQuery(context, targetCar.ModelYear);

            query = DataHelper.GetStateMarketData(query, targetCar.Make, targetCar.Model, dealer.State,targetCar.Trim);
            
            if (targetCar.BodyType.ToLower().Contains("coupe"))
            {
                query = query.Where(x => x.BodyStyle.Contains("coupe"));
            }
            else if (targetCar.BodyType.ToLower().Contains("convertible"))
            {
                query = query.Where(x => x.BodyStyle.Contains("convertible"));
            }

            var result = query.ToList();

            if (/*query.Any()*/result.Count > 0)
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    string filtertrim = string.IsNullOrEmpty(item.Trim)
                                          ? "other"
                                          : Regex.Replace(item.Trim, @"\s+", " ").ToLower();

                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = filtertrim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.CarsComThumbnailURL)
                                    ? ""
                                    : item.CarsComThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }

            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.ListingID,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.ThumbnailImageURL)
                       ? String.IsNullOrEmpty(targetCar.DefaultImageUrl) ? "" : targetCar.DefaultImageUrl
                                                     : targetCar.ThumbnailImageURL.Split(new string[] { "," },
                                                                                         StringSplitOptions.None).
                                                           First(),
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                            dealer.Address + " " +
                            dealer.City +
                            "," + dealer.State +
                            " " +
                            dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static ChartGraph GetMarketCarsOnCarsComForNationwideMarket(int listingId, DealershipViewModel dealer)
        {
            var context = new vincontrolscrappingEntities();

            var targetCar = GetTargetCar(listingId);

            //var savedSelection = GetChartSelectionForCarsCom(listingId);

            var chartGraph = new ChartGraph();

            var list = new List<ChartModel>();

            var listPrice = new List<decimal>();
            //string modelWord = DataHelper.FilterCarModelForMarket(targetCar.Model);

            var query = MapperFactory.GetCarsComMarketCarQuery(context, targetCar.ModelYear);


            query = DataHelper.GetNationwideMarketData(query, targetCar.Make, targetCar.Model,targetCar.Trim);

      

            if (targetCar.BodyType.ToLower().Contains("coupe"))
            {
                query = query.Where(x => x.BodyStyle.Contains("coupe"));
            }
            else if (targetCar.BodyType.ToLower().Contains("convertible"))
            {
                query = query.Where(x => x.BodyStyle.Contains("convertible"));
            }

            if (query.Any())
            {
                var table = new HashSet<string>();

                foreach (var row in query)
                {
                    string filtertrim = string.IsNullOrEmpty(row.Trim)
                                            ? "other"
                                            : Regex.Replace(row.Trim, @"\s+", " ").ToLower();
                    table.Add(filtertrim);
                }

                foreach (var item in query)
                {
                    string filtertrim = string.IsNullOrEmpty(item.Trim)
                                        ? "other"
                                        : Regex.Replace(item.Trim, @"\s+", " ").ToLower();


                    int distance =
                        CommonHelper.DistanceBetweenPlaces(Convert.ToDouble(item.Latitude),
                                                           Convert.ToDouble(item.Longitude),
                                                           Convert.ToDouble(dealer.Latitude),
                                                           Convert.ToDouble(dealer.Longtitude));
                    int validMileage = item.Mileage;

                    int validPrice = item.CurrentPrice;

                    if (validMileage > 0 && validPrice > 0)
                    {
                        var chart = new ChartModel
                        {
                            ListingId = item.RegionalListingId,
                            Title =
                                new ChartModel.TitleInfo
                                {
                                    Make = item.Make,
                                    Model = item.Model.TrimEnd(),
                                    Trim = filtertrim,
                                    Year = item.Year.GetValueOrDefault()
                                },
                            Color =
                                new ChartModel.ColorInfo { Exterior = item.ExteriorColor, Interior = item.InteriorColor },
                            Option = new ChartModel.OptionInfo
                            {
                                Moonroof = item.MoonRoof.GetValueOrDefault(),
                                Navigation = item.MoonRoof.GetValueOrDefault(),
                                Sunroof = item.SunRoof.GetValueOrDefault()
                            },
                            Certified = item.Certified.GetValueOrDefault(),
                            VIN = item.Vin,
                            Franchise =
                                item.Franchise.GetValueOrDefault() ? "franchise" : "independant",
                            Distance = distance,
                            Uptime = DateTime.Now.Subtract(item.DateAdded.GetValueOrDefault()).Days,
                            Trims = table.ToList(),
                            Miles = validMileage,
                            Price = validPrice,
                            ThumbnailURL =
                                String.IsNullOrEmpty(item.CarsComThumbnailURL)
                                    ? ""
                                    : item.CarsComThumbnailURL,
                            Longtitude = String.IsNullOrEmpty(item.Longitude)
                          ? ""
                          : item.Longitude.Trim(),
                            Latitude = String.IsNullOrEmpty(item.Latitude)
                                    ? ""
                                    : item.Latitude.Trim(),
                            CarsCom = item.CarsCom != null && item.CarsCom.GetValueOrDefault(),
                            CarsComListingURL =
                                String.IsNullOrEmpty(item.CarsComListingURL)
                                    ? ""
                                    : item.CarsComListingURL,
                            AutoTrader =
                                item.AutoTrader != null && item.AutoTrader.GetValueOrDefault(),
                            AutoTraderListingURL =
                                String.IsNullOrEmpty(item.AutoTraderListingURL)
                                    ? ""
                                    : item.AutoTraderListingURL,
                            Seller =
                                new ChartModel.SellerInfo()
                                {
                                    SellerName = item.Dealershipname,
                                    SellerAddress = item.Address
                                },
                        };
                        listPrice.Add(validPrice);
                        list.Add(chart);
                    }
                }
            }




            int mileageNumber = 0;
            Int32.TryParse(targetCar.Mileage, out mileageNumber);

            int salePriceNumber = 0;
            Int32.TryParse(targetCar.SalePrice, out salePriceNumber);

            chartGraph.Target = new ChartGraph.TargetCar()
            {
                ListingId = targetCar.ListingID,
                Mileage = mileageNumber,
                SalePrice = salePriceNumber,

                ThumbnailImageUrl = String.IsNullOrEmpty(targetCar.ThumbnailImageURL)
                       ? String.IsNullOrEmpty(targetCar.DefaultImageUrl) ? "" : targetCar.DefaultImageUrl
                                                     : targetCar.ThumbnailImageURL.Split(new string[] { "," },
                                                                                         StringSplitOptions.None).
                                                           First(),
                Distance = 0,
                Title =
                    new ChartModel.TitleInfo
                    {
                        Make = targetCar.Make,
                        Model = targetCar.Model.TrimEnd(),
                        Trim =
                            string.IsNullOrEmpty(targetCar.Trim)
                                ? "other"
                                : targetCar.Trim,
                        Year = targetCar.ModelYear ?? 2012
                    },
                Certified = targetCar.Certified ?? false,
                Trim = string.IsNullOrEmpty(targetCar.Trim) ? "other" : targetCar.Trim,
                Seller =
                    new ChartModel.SellerInfo()
                    {
                        SellerName = targetCar.DealershipName,
                        SellerAddress =
                             dealer.Address + " " +
                             dealer.City +
                             "," + dealer.State +
                             " " +
                             dealer.ZipCode
                    },
            };

            if (listPrice.Any())
            {
                // set ranking
                var ranking = 1;
                foreach (var price in listPrice)
                {
                    if (chartGraph.Target.SalePrice > price)
                        ranking++;
                }
                chartGraph.Target.Ranking = ranking;

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = listPrice.Count(),
                    MinimumPrice = listPrice.Min().ToString("c0"),
                    AveragePrice = Math.Round(listPrice.Average()).ToString("c0"),
                    MaximumPrice = listPrice.Max().ToString("c0"),
                    AverageDays =
                        Math.Round(list.Select(x => x.Uptime).Average(), 0).ToString(CultureInfo.InvariantCulture)

                };

                chartGraph.ChartModels = list.OrderBy(x => x.Distance).ToList();
            }

            return chartGraph;
        }

        public static bool CanViewBucketJumpReport(int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var allowedDealer =
                    context.whitmanenterprisedealerships.Where(
                        i => i.idWhitmanenterpriseDealership == dealerId && i.DealerGroupID.ToLower().Equals("g1010")).
                        FirstOrDefault();

                return allowedDealer == null ? false : true;
            }
        }

        public static List<ManheimWholesaleViewModel> ManheimReport(whitmanenterprisedealershipinventory inventory)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
                var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == inventory.VINNumber && x.Type.Equals(Constanst.VehicleTable.Inventory)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == inventory.VINNumber 
                        && x.Type.Equals(Constanst.VehicleTable.Inventory)
                        && x.DateAdded > dtCompareToday);
                    foreach (var tmp in searchResult)
                    {
                        var subResult = new ManheimWholesaleViewModel()
                                            {
                                                LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                                AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                                HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                                Year = tmp.Year.GetValueOrDefault(),
                                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                                TrimName = tmp.Trim
                                            };
                        result.Add(subResult);
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static ManheimWholesaleViewModel ManheimReportForTradeIn(string vin, int year, string make, string model, string trim, string userName, string password)
        {
            var result = new ManheimWholesaleViewModel();

        
            try
            {
                var searchResultList =
                    DataHelper.GetManheimAuctionMarketData(year, make, model, trim);

                if (searchResultList.Any())
                {

                    var searchResult = searchResultList.First();
                    var lowestPrice = searchResult.MmrBelow.GetValueOrDefault();
                    var averagePrice = searchResult.Mmr;
                    var highestPrice = searchResult.MmrAbove.GetValueOrDefault();
                    result = new ManheimWholesaleViewModel()
                        {
                            LowestPrice = lowestPrice.ToString(CultureInfo.InvariantCulture),
                            AveragePrice = averagePrice.ToString(CultureInfo.InvariantCulture),
                            HighestPrice = highestPrice.ToString(CultureInfo.InvariantCulture),
                            Year = year,

                        };

                }
                else
                {
                    result = new ManheimWholesaleViewModel()
                    {
                        LowestPrice = 0.ToString(),
                        AveragePrice = 0.ToString(),
                        HighestPrice = 0.ToString(),
                        Year = year,

                    };

                }


            }
            catch (Exception)
            {

            }

            return result;
        }

        //public static List<ManheimWholesaleViewModel> ManheimReportForTradeIn(string vin, int year, int makeId, int modelId, int trimId, string userName, string password)
        //{
        //    //var dealer = (DealershipViewModel)Session["Dealership"];

        //    var result = new List<ManheimWholesaleViewModel>();
        //    var context = new whitmanenterprisewarehouseEntities();
        //    try
        //    {
        //        var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        if (context.vincontrolmanheimvalues.Any(x => x.Vin == vin))
        //        {
        //            var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == vin);
        //            if (searchResult.First().DateAdded > dtCompareToday)
        //            {
        //                foreach (var tmp in searchResult)
        //                {
        //                    var subResult = new ManheimWholesaleViewModel()
        //                    {
        //                        LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
        //                        AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
        //                        HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
        //                        Year = tmp.Year.GetValueOrDefault(),
        //                        MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
        //                        ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
        //                        TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
        //                        TrimName = tmp.Trim
        //                    };
        //                    result.Add(subResult);
        //                }
        //            }
        //            else
        //            {
        //                foreach (var vincontrolmanheimvalue in searchResult)
        //                {
        //                    context.Attach(vincontrolmanheimvalue);
        //                    context.DeleteObject(vincontrolmanheimvalue);
        //                }
        //                context.SaveChanges();

        //                var manheimService = new ManheimService();
        //                if (!string.IsNullOrEmpty(vin))
        //                {
        //                    manheimService.ExecuteByVin(userName, password, vin.Trim());
        //                    result = manheimService.ManheimWholesales;

        //                    foreach (var tmp in result)
        //                    {
        //                        var manheimRecord = new vincontrolmanheimvalue()
        //                        {
        //                            AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
        //                            AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
        //                            AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
        //                            Year = year,
        //                            MakeServiceId = tmp.MakeServiceId,
        //                            ModelServiceId = tmp.ModelServiceId,
        //                            TrimServiceId = tmp.TrimServiceId,
        //                            Trim = tmp.TrimName,
        //                            DateAdded = DateTime.Now,
        //                            ExpiredDate = DateTime.Now.AddDays(1),
        //                            Vin = vin,
        //                            LastUpdated = DateTime.Now
        //                        };

        //                        context.AddTovincontrolmanheimvalues(manheimRecord);
        //                    }

        //                    context.SaveChanges();
        //                }
        //                else
        //                {
        //                    var matchingMake = context.manheimmakes.FirstOrDefault(i => i.id == makeId).serviceId;
        //                    var matchingModel = context.manheimmodels.FirstOrDefault(i => i.id == modelId).serviceId;
        //                    var matchingTrims = context.manheimtrims.Where(i => i.id == trimId).ToList();

        //                    foreach (var trim in matchingTrims)
        //                    {
        //                        manheimService.Execute("US", year.ToString(), matchingMake.ToString(),
        //                                               matchingModel.ToString(),
        //                                               trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
        //                                               password);
        //                        if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
        //                              manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
        //                              manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
        //                            !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
        //                        {
        //                            var subResult = new ManheimWholesaleViewModel()
        //                            {
        //                                LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                                AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                                HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                                Year = year,
        //                                MakeServiceId = matchingMake.GetValueOrDefault(),
        //                                ModelServiceId = matchingModel.GetValueOrDefault(),
        //                                TrimServiceId = trim.serviceId.GetValueOrDefault(),
        //                                TrimName = trim.name
        //                            };

        //                            result.Add(subResult);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var manheimService = new ManheimService();
        //            if (!string.IsNullOrEmpty(vin))
        //            {
        //                manheimService.ExecuteByVin(userName, password, vin.Trim());
        //                result = manheimService.ManheimWholesales;

        //                foreach (var tmp in result)
        //                {
        //                    var manheimRecord = new vincontrolmanheimvalue()
        //                    {
        //                        AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
        //                        AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
        //                        AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
        //                        Year = year,
        //                        MakeServiceId = tmp.MakeServiceId,
        //                        ModelServiceId = tmp.ModelServiceId,
        //                        TrimServiceId = tmp.TrimServiceId,
        //                        Trim = tmp.TrimName,
        //                        DateAdded = DateTime.Now,
        //                        ExpiredDate = DateTime.Now.AddDays(1),
        //                        Vin = vin,
        //                        LastUpdated = DateTime.Now
        //                    };

        //                    context.AddTovincontrolmanheimvalues(manheimRecord);
        //                }

        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                var matchingMake = context.manheimmakes.FirstOrDefault(i => i.id == makeId).serviceId;
        //                var matchingModel = context.manheimmodels.FirstOrDefault(i => i.id == modelId).serviceId;
        //                var matchingTrims = context.manheimtrims.Where(i => i.id == trimId).ToList();

        //                foreach (var trim in matchingTrims)
        //                {
        //                    manheimService.Execute("US", year.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
        //                    if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
        //                       manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
        //                       manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
        //                       !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
        //                    {
        //                        var subResult = new ManheimWholesaleViewModel()
        //                        {
        //                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
        //                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
        //                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
        //                            Year = year,
        //                            MakeServiceId = matchingMake.GetValueOrDefault(),
        //                            ModelServiceId = matchingModel.GetValueOrDefault(),
        //                            TrimServiceId = trim.serviceId.GetValueOrDefault(),
        //                            TrimName = trim.name
        //                        };

        //                        result.Add(subResult);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return result;
        //}

        public static List<ManheimWholesaleViewModel> ManheimReport(whitmanenterprisedealershipinventory inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == inventory.VINNumber && x.Type.Equals(Constanst.VehicleTable.Inventory)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == inventory.VINNumber && x.Type.Equals(Constanst.VehicleTable.Inventory)).OrderByDescending(x => x.DateAdded).ToList();
                    if (searchResult.Count > 0)
                    {
                        foreach (var tmp in searchResult)
                        {
                            if (result.Any(x => x.TrimName.ToLower().Contains(tmp.Trim.ToLower()))) continue;
                            
                            var subResult = new ManheimWholesaleViewModel()
                                                {
                                                    LowestPrice =CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                                    AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                                    HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                                    Year = tmp.Year.GetValueOrDefault(),
                                                    MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                                    ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                                    TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                                    TrimName = tmp.Trim
                                                };
                            result.Add(subResult);
                        }
                    }
                    else
                    {
                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(inventory.VINNumber))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.VINNumber.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new vincontrolmanheimvalue()
                                {
                                    AuctionLowestPrice =CommonHelper.RemoveSpecialCharactersForPurePrice( tmp.LowestPrice),
                                    AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                    AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                    Year = inventory.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    ExpiredDate = CommonHelper.GetNextFriday(),
                                    Vin = inventory.VINNumber,
                                    LastUpdated = DateTime.Now,
                                    Type = Constanst.VehicleTable.Inventory
                                };

                                context.AddTovincontrolmanheimvalues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                            var matchingTrims = new List<manheimtrim>();

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
                                manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                                manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(),
                                                       trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                                    !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                                        {
                                                            LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                                            AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                                            HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                                            Year = inventory.ModelYear.GetValueOrDefault(),
                                                            MakeServiceId = matchingMake,
                                                            ModelServiceId = matchingModel,
                                                            TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                                            TrimName = trim.name
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
                    if (!string.IsNullOrEmpty(inventory.VINNumber))
                    {
                        manheimService.ExecuteByVin(userName, password, inventory.VINNumber.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new vincontrolmanheimvalue()
                            {
                                AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                Year = inventory.ModelYear.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                ExpiredDate = CommonHelper.GetNextFriday(),
                                Vin = inventory.VINNumber,
                                LastUpdated = DateTime.Now,
                                Type = Constanst.VehicleTable.Inventory
                            };

                            context.AddTovincontrolmanheimvalues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(inventory.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                        var matchingTrims = new List<manheimtrim>();

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
                            manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                            manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                               manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                               manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                               !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                            {
                                var subResult = new ManheimWholesaleViewModel()
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = inventory.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                    TrimName = trim.name
                                };
                             
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForSoldCars(whitmanenterprisedealershipinventorysoldout inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == inventory.VINNumber && x.Type.Equals(Constanst.VehicleTable.SoldOut)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == inventory.VINNumber 
                        && x.Type.Equals(Constanst.VehicleTable.SoldOut)).ToList();
                    if (searchResult.Count > 0)
                    {
                        foreach (var tmp in searchResult)
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            };
                            result.Add(subResult);
                        }
                    }
                    else
                    {
                     

                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(inventory.VINNumber))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.VINNumber.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new vincontrolmanheimvalue()
                                {
                                    AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                    AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                    AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                    Year = inventory.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    ExpiredDate = CommonHelper.GetNextFriday(),
                                    Vin = inventory.VINNumber,
                                    LastUpdated = DateTime.Now,
                                    Type = Constanst.VehicleTable.SoldOut
                                };

                                context.AddTovincontrolmanheimvalues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                            var matchingTrims = new List<manheimtrim>();

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
                                manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                                manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(),
                                                       trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                                    !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = inventory.ModelYear.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                        TrimName = trim.name
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
                    if (!string.IsNullOrEmpty(inventory.VINNumber))
                    {
                        manheimService.ExecuteByVin(userName, password, inventory.VINNumber.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new vincontrolmanheimvalue()
                            {
                                AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                Year = inventory.ModelYear.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                ExpiredDate = CommonHelper.GetNextFriday(),
                                Vin = inventory.VINNumber,
                                LastUpdated = DateTime.Now,
                                Type = Constanst.VehicleTable.SoldOut
                            };

                            context.AddTovincontrolmanheimvalues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(inventory.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                        var matchingTrims = new List<manheimtrim>();

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
                            manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                            manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                               manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                               manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                               !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                            {
                                var subResult = new ManheimWholesaleViewModel()
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = inventory.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                    TrimName = trim.name
                                };

                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForWholesale(vincontrolwholesaleinventory wholesale)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();

                if (!string.IsNullOrEmpty(wholesale.VINNumber.Trim()))
                {
                    manheimService.Execute(wholesale.VINNumber.Trim());                    
                    result = manheimService.ManheimWholesales;
                }
                else
                {
                    var matchingMake = manheimService.MatchingMake(wholesale.Make);
                    var matchingModel = 0;
                    var matchingModels = manheimService.MatchingModels(wholesale.Model, matchingMake);
                    var matchingTrims = new List<manheimtrim>();

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
                        manheimService.GetTrim(wholesale.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels);
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
                        manheimService.Execute("US", wholesale.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA");
                        if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                           manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                           manheimService.ManheimWholesale.HighestPrice.Equals("$0")))
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                Year = wholesale.ModelYear.GetValueOrDefault(),
                                MakeServiceId = matchingMake,
                                ModelServiceId = matchingModel,
                                TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                TrimName = trim.name
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

        public static List<ManheimWholesaleViewModel> ManheimReportForWholesale(vincontrolwholesaleinventory wholesale, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
          
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == wholesale.VINNumber && x.Type.Equals(Constanst.VehicleTable.WholeSale)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == wholesale.VINNumber
                        && x.Type.Equals(Constanst.VehicleTable.WholeSale)).ToList();
                    if (searchResult.Count > 0)
                    {
                        foreach (var tmp in searchResult)
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            };
                            result.Add(subResult);
                        }
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

                        if (!string.IsNullOrEmpty(wholesale.VINNumber))
                        {
                            manheimService.ExecuteByVin(userName, password, wholesale.VINNumber.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new vincontrolmanheimvalue()
                                {
                                    AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                    AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                    AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                    Year = wholesale.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    ExpiredDate = CommonHelper.GetNextFriday(),
                                    Vin = wholesale.VINNumber,
                                    LastUpdated = DateTime.Now,
                                    Type = Constanst.VehicleTable.WholeSale
                                };

                                context.AddTovincontrolmanheimvalues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(wholesale.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(wholesale.Model, matchingMake);
                            var matchingTrims = new List<manheimtrim>();

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
                                manheimService.GetTrim(wholesale.ModelYear.Value.ToString(), matchingMake.ToString(),
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
                                manheimService.Execute("US", wholesale.ModelYear.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(),
                                                       trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                                    !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = wholesale.ModelYear.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                        TrimName = trim.name
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
                    if (!string.IsNullOrEmpty(wholesale.VINNumber))
                    {
                        manheimService.ExecuteByVin(userName, password, wholesale.VINNumber.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new vincontrolmanheimvalue()
                            {
                                AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                Year = wholesale.ModelYear.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                ExpiredDate = CommonHelper.GetNextFriday(),
                                Vin = wholesale.VINNumber,
                                LastUpdated = DateTime.Now,
                                Type = Constanst.VehicleTable.WholeSale
                            };

                            context.AddTovincontrolmanheimvalues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(wholesale.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(wholesale.Model, matchingMake);
                        var matchingTrims = new List<manheimtrim>();

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
                            manheimService.GetTrim(wholesale.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                            manheimService.Execute("US", wholesale.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                               manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                               manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                               !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                            {
                                var subResult = new ManheimWholesaleViewModel()
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = wholesale.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                    TrimName = trim.name
                                };

                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForSoldout(whitmanenterprisedealershipinventorysoldout inventory)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();

                if (!string.IsNullOrEmpty(inventory.VINNumber.Trim()))
                {
                    manheimService.Execute(inventory.VINNumber.Trim());                    
                    result = manheimService.ManheimWholesales;
                }
                else
                {
                    var matchingMake = manheimService.MatchingMake(inventory.Make);
                    var matchingModel = 0;
                    var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                    var matchingTrims = new List<manheimtrim>();

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
                        manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels);
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
                        manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA");
                        if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                           manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                           manheimService.ManheimWholesale.HighestPrice.Equals("$0")))
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                Year = inventory.ModelYear.GetValueOrDefault(),
                                MakeServiceId = matchingMake,
                                ModelServiceId = matchingModel,
                                TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                TrimName = trim.name
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

        public static List<ManheimWholesaleViewModel> ManheimReportForSoldout(whitmanenterprisedealershipinventorysoldout inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
                var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == inventory.VINNumber && x.Type.Equals(Constanst.VehicleTable.SoldOut)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == inventory.VINNumber
                        && x.Type.Equals(Constanst.VehicleTable.SoldOut)
                        && x.DateAdded > dtCompareToday).OrderBy(x => x.DateAdded).ToList();
                    if (searchResult.Count > 0)
                    {
                        foreach (var tmp in searchResult)
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            };
                            result.Add(subResult);
                        }
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
                        if (!string.IsNullOrEmpty(inventory.VINNumber))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.VINNumber.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new vincontrolmanheimvalue()
                                {
                                    AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                    AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                    AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                    Year = inventory.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    ExpiredDate = DateTime.Now.AddDays(1),
                                    Vin = inventory.VINNumber,
                                    LastUpdated = DateTime.Now,
                                    Type = Constanst.VehicleTable.SoldOut
                                };

                                context.AddTovincontrolmanheimvalues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                            var matchingTrims = new List<manheimtrim>();

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
                                manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(),
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
                                manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(),
                                                       trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                                    !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = inventory.ModelYear.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                        TrimName = trim.name
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

                    if (!string.IsNullOrEmpty(inventory.VINNumber.Trim()))
                    {
                        manheimService.ExecuteByVin(userName, password, inventory.VINNumber.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new vincontrolmanheimvalue()
                            {
                                AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                Year = inventory.ModelYear.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                ExpiredDate = DateTime.Now.AddDays(1),
                                Vin = inventory.VINNumber,
                                LastUpdated = DateTime.Now,
                                Type = Constanst.VehicleTable.SoldOut
                            };

                            context.AddTovincontrolmanheimvalues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(inventory.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                        var matchingTrims = new List<manheimtrim>();

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
                            manheimService.GetTrim(inventory.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                            manheimService.Execute("US", inventory.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                               manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                               manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                               !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                            {
                                var subResult = new ManheimWholesaleViewModel()
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = inventory.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                    TrimName = trim.name
                                };
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForAppraisal(whitmanenterpriseappraisal appraisal)
        {
            var result = new List<ManheimWholesaleViewModel>();
            try
            {
                var manheimService = new ManheimService();

                if (!string.IsNullOrEmpty(appraisal.VINNumber.Trim()))
                {
                    manheimService.Execute(appraisal.VINNumber.Trim());                    
                    result = manheimService.ManheimWholesales;
                }
                else
                {
                    var matchingMake = manheimService.MatchingMake(appraisal.Make);
                    var matchingModel = 0;
                    var matchingModels = manheimService.MatchingModels(appraisal.Model, matchingMake);
                    var matchingTrims = new List<manheimtrim>();

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
                        manheimService.GetTrim(appraisal.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels);
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
                        manheimService.Execute("US", appraisal.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA");
                        if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                           manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                           manheimService.ManheimWholesale.HighestPrice.Equals("$0")))
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                Year = appraisal.ModelYear.GetValueOrDefault(),
                                MakeServiceId = matchingMake,
                                ModelServiceId = matchingModel,
                                TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                TrimName = trim.name
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

        public static List<ManheimWholesaleViewModel> ManheimReportForAppraisal(whitmanenterpriseappraisal appraisal, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
               
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == appraisal.VINNumber && x.Type.Equals(Constanst.VehicleTable.Appraisal)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == appraisal.VINNumber 
                        && x.Type.Equals(Constanst.VehicleTable.Appraisal)).OrderByDescending(x => x.DateAdded).ToList();
                    if (searchResult.Count > 0)
                    {
                        foreach (var tmp in searchResult)
                        {
                            if (result.Any(x => x.TrimName.ToLower().Equals(tmp.Trim.ToLower()))) continue;

                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim                                
                            };
                            result.Add(subResult);
                        }
                    }
                    else
                    {
                       
                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(appraisal.VINNumber))
                        {
                            manheimService.ExecuteByVin(userName, password, appraisal.VINNumber.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new vincontrolmanheimvalue()
                                {
                                    AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                    AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                    AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                    Year = appraisal.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    ExpiredDate = CommonHelper.GetNextFriday(),
                                    Vin = appraisal.VINNumber,
                                    LastUpdated = DateTime.Now,
                                    Type = Constanst.VehicleTable.Appraisal
                                };

                                context.AddTovincontrolmanheimvalues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(appraisal.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(appraisal.Model, matchingMake);
                            var matchingTrims = new List<manheimtrim>();

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
                                manheimService.GetTrim(appraisal.ModelYear.Value.ToString(), matchingMake.ToString(),
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
                                manheimService.Execute("US", appraisal.ModelYear.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(),
                                                       trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                                    !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = appraisal.ModelYear.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                        TrimName = trim.name
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

                    if (!string.IsNullOrEmpty(appraisal.VINNumber.Trim()))
                    {
                        manheimService.ExecuteByVin(userName, password, appraisal.VINNumber.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new vincontrolmanheimvalue()
                            {
                                AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                Year = appraisal.ModelYear.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                ExpiredDate = CommonHelper.GetNextFriday(),
                                Vin = appraisal.VINNumber,
                                LastUpdated = DateTime.Now,
                                Type = Constanst.VehicleTable.Appraisal
                            };

                            context.AddTovincontrolmanheimvalues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(appraisal.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(appraisal.Model, matchingMake);
                        var matchingTrims = new List<manheimtrim>();

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
                            manheimService.GetTrim(appraisal.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                            manheimService.Execute("US", appraisal.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                               manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                               manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                               !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                            {
                                var subResult = new ManheimWholesaleViewModel()
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = appraisal.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                    TrimName = trim.name
                                };
                                result.Add(subResult);
                            }
                        }
                    }
                }               
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static List<ManheimWholesaleViewModel> ManheimReportForAppraisalWihtoutVin(whitmanenterpriseappraisal appraisal, string vin, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new whitmanenterprisewarehouseEntities();
            try
            {
                var dtCompareToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (context.vincontrolmanheimvalues.Any(x => x.Vin == vin && x.Type.Equals(Constanst.VehicleTable.Appraisal)))
                {
                    var searchResult = context.vincontrolmanheimvalues.Where(x => x.Vin == vin
                        && x.Type.Equals(Constanst.VehicleTable.Appraisal)
                        && x.DateAdded > dtCompareToday).OrderBy(x => x.DateAdded).ToList();
                    if (searchResult.Count > 0)
                    {
                        foreach (var tmp in searchResult)
                        {
                            var subResult = new ManheimWholesaleViewModel()
                            {
                                LowestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionLowestPrice),
                                AveragePrice = CommonHelper.ConvertToCurrency(tmp.AuctionAveragePrice),
                                HighestPrice = CommonHelper.ConvertToCurrency(tmp.AuctionHighestPrice),
                                Year = tmp.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                TrimName = tmp.Trim
                            };
                            result.Add(subResult);
                        }
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
                                var manheimRecord = new vincontrolmanheimvalue()
                                {
                                    AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                    AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                    AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                    Year = appraisal.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    ExpiredDate = DateTime.Now.AddDays(1),
                                    Vin = vin,
                                    LastUpdated = DateTime.Now,
                                    Type = Constanst.VehicleTable.Appraisal
                                };

                                context.AddTovincontrolmanheimvalues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(appraisal.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(appraisal.Model, matchingMake);
                            var matchingTrims = new List<manheimtrim>();

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
                                manheimService.GetTrim(appraisal.ModelYear.Value.ToString(), matchingMake.ToString(),
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
                                manheimService.Execute("US", appraisal.ModelYear.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(),
                                                       trim.serviceId.GetValueOrDefault().ToString(), "NA", userName,
                                                       password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                                    !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = appraisal.ModelYear.GetValueOrDefault(),
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                        TrimName = trim.name
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
                            var manheimRecord = new vincontrolmanheimvalue()
                            {
                                AuctionLowestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.LowestPrice),
                                AuctionAveragePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.AveragePrice),
                                AuctionHighestPrice = CommonHelper.RemoveSpecialCharactersForPurePrice(tmp.HighestPrice),
                                Year = appraisal.ModelYear.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                ExpiredDate = DateTime.Now.AddDays(1),
                                Vin = vin,
                                LastUpdated = DateTime.Now,
                                Type = Constanst.VehicleTable.Appraisal
                            };

                            context.AddTovincontrolmanheimvalues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(appraisal.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(appraisal.Model, matchingMake);
                        var matchingTrims = new List<manheimtrim>();

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
                            manheimService.GetTrim(appraisal.ModelYear.Value.ToString(), matchingMake.ToString(), matchingModels, userName, password);
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
                            manheimService.Execute("US", appraisal.ModelYear.ToString(), matchingMake.ToString(), matchingModel.ToString(), trim.serviceId.GetValueOrDefault().ToString(), "NA", userName, password);
                            if (!(manheimService.ManheimWholesale.LowestPrice.Equals("$0") ||
                               manheimService.ManheimWholesale.AveragePrice.Equals("$0") ||
                               manheimService.ManheimWholesale.HighestPrice.Equals("$0")) &&
                               !string.IsNullOrEmpty(manheimService.ManheimWholesale.LowestPrice))
                            {
                                var subResult = new ManheimWholesaleViewModel()
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = appraisal.ModelYear.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.serviceId.GetValueOrDefault(),
                                    TrimName = trim.name
                                };
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch (Exception)
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

        public static whitmanenterprisesetting GetManheimCredential(int dealerId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var manheimCredentail = context.whitmanenterprisesettings.FirstOrDefault(m => m.DealershipId == dealerId && !string.IsNullOrEmpty(m.Manheim.Trim()));
                return manheimCredentail;
            }
        }

        public static void SavePriceChangeHistory(int listingId, string type, decimal oldPrice, decimal newPrice, string attachFile, string userStamp)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var newPriceChangeHistory = new vincontrolpricechangeshistory()
                {
                    DateStamp = DateTime.Now,
                    ListingId = listingId,
                    Type = type,
                    OldPrice = oldPrice,
                    NewPrice = newPrice,
                    UserStamp = userStamp,
                    AttachFile = string.Empty
                };
                context.AddTovincontrolpricechangeshistories(newPriceChangeHistory);
                context.SaveChanges();

                var stockNumber = string.Empty;
                switch (type.ToLower())
                {
                    case "inventory":
                        var inventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(i => i.ListingID == listingId);
                        if (inventory != null) { stockNumber = inventory.StockNumber; }
                        break;
                    case "wholesale":
                        var wholesale = context.vincontrolwholesaleinventories.FirstOrDefault(i => i.ListingID == listingId);
                        if (wholesale != null) { stockNumber = wholesale.StockNumber; }
                        break;
                    case "soldout":
                        var soldout = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(i => i.ListingID == listingId);
                        if (soldout != null) { stockNumber = soldout.StockNumber; }
                        break;
                    case "appraisal":
                        var appraisal = context.whitmanenterpriseappraisals.FirstOrDefault(i => i.idAppraisal == listingId);
                        if (appraisal != null) { stockNumber = appraisal.StockNumber; }
                        break;
                    default: break;
                }

                AddNewActivity(SessionHandler.Dealership.DealershipId, "Price Change From " + oldPrice.ToString("c0") + " To " + newPrice.ToString("c0") + " For " + type + " " + stockNumber, String.Format("Listing Id: {0},Type: {1}, Old Price: {2};New Price: {3}", listingId, type, oldPrice.ToString("c0"), newPrice.ToString("c0")), Constanst.ActivityType.PriceChange);
            }
        }

        public static List<WarrantyTypeViewModel> GetWarrantyTypeList(DealershipViewModel dealer)
        {
            try
            {
                using (var context = new whitmanenterprisewarehouseEntities())
                {

                    var list = context.vincontrolwarrantytypes.Where(i => i.DealerId != null && (i.DealerId == 0 || i.DealerId == dealer.DealershipId)).ToList();

                    if (dealer.DealerGroupId.ToLower().Equals("g1010"))
                    {
                        list.RemoveAt(list.FindIndex(x => x.Name.Equals("Dealer Certified")));
                    }

                    if (list.Count > 0)
                    {
                        return list.Select(i => new WarrantyTypeViewModel()
                                                    {
                                                        DealerId = i.DealerId.GetValueOrDefault(),
                                                        EnglishVersionUrl = "/Report/CreateBuyerGuide?type=" + i.Id.ToString(),
                                                        SpanishVersionUrl = "/Report/CreateBuyerGuideSpanish?type=" + i.Id.ToString(),
                                                        Name = i.Name,
                                                        Id = i.Id,
                                                        CategoryId = i.Category.GetValueOrDefault()
                                                    }).ToList();
                    }
                }
            }
            catch (Exception)
            {
                
            }

            return new List<WarrantyTypeViewModel>();
        }

        public static void SaveBucketJumpHistory(int listingId, int dealershipId, int salePrice, int retailPrice, string type, byte[] bytes, string user)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var path = System.Web.HttpContext.Current.Server.MapPath("\\BucketJumpReports") + "\\" + dealershipId + "\\" + type + "\\" + listingId;
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

                var bucketJumpHistory = new vincontrolbucketjumphistory()
                {
                    UserStamp = user,
                    DateStamp = DateTime.Now,
                    ListingId = listingId,
                    Type = type,
                    AttachFile = fileName,
                    SalePrice = salePrice,
                    
                    RetailPrice = retailPrice
                };
                context.AddTovincontrolbucketjumphistories(bucketJumpHistory);
                context.SaveChanges();
            }
        }

        public static void AddNewActivity(int dealerId, string title, string detail, string type)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var newActivity = new vincontroldealershipactivity()
                {
                    DealerId = dealerId,
                    UserStamp = HttpContext.Current.User.Identity.Name,
                    DateStamp = DateTime.Now,
                    Type = type,
                    Title = title,
                    Detail = detail
                };
                context.AddTovincontroldealershipactivities(newActivity);
                context.SaveChanges();
            }
        }

        public static IEnumerable<DealershipActivityViewModel> GetTop20Activities()
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var list = InventoryQueryHelper.GetSingleOrGroupDealerActivity(context).OrderByDescending(i => i.DateStamp).Take(20).Skip(0).ToList();
                return list.Count > 0
                           ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                           : new List<DealershipActivityViewModel>();
            }
        }

        public static IEnumerable<DealershipActivityViewModel> GetAllActivities()
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var list = InventoryQueryHelper.GetSingleOrGroupDealerActivity(context).OrderByDescending(i => i.DateStamp).ToList();
                return list.Count > 0
                           ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                           : new List<DealershipActivityViewModel>();
            }
        }

        public static IEnumerable<DealershipActivityViewModel> FilterActivities(int month, int year)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var list = InventoryQueryHelper.GetSingleOrGroupDealerActivity(context).Where(i=>(month == 0 ? true : i.DateStamp.Value.Month == month) && (year == 0 ? true : i.DateStamp.Value.Year == year)).OrderByDescending(i => i.DateStamp).ToList();
                return list.Count > 0
                           ? list.Select(i => new DealershipActivityViewModel(i)).ToList()
                           : new List<DealershipActivityViewModel>();
            }
        }

       
    }
}
