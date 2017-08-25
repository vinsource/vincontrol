using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Helper
{
    public class MarketDataHelper
    {
        public static ChartGraph GetMarketCarsOnAutoTraderVersion2(VehicleViewModel targetCar, UserViewModel dealer)
        {
            var context = new VinMarketEntities();

            var chartGraph = new ChartGraph();

            var query = context.daily_market.Where(x => x.Year == targetCar.Year);

            var searchResult = GetNationwideMarketDataVersion2(query, targetCar.Make, targetCar.Model, targetCar.Trim);

            if (searchResult != null && searchResult.Any())
            {

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = searchResult.Select(x => x.NoOfCarInNation).Sum().GetValueOrDefault(),
                    MinimumPrice = searchResult.Select(x => x.BelowMarketPrice).Min().GetValueOrDefault(),
                    AveragePrice = searchResult.Select(x => x.AverageMarketPrice).Average().GetValueOrDefault(),
                    MaximumPrice = searchResult.Select(x => x.AboveMarketPrice).Max().GetValueOrDefault(),

                };
            }
            else
            {

                chartGraph.Market = new ChartGraph.MarketInfo
                {
                    CarsOnMarket = 0,
                    MinimumPrice = 0,
                    AveragePrice = 0,
                    MaximumPrice = 0,

                };
            }


            return chartGraph;

        }
      
        public static IQueryable<MarketCarInfo> GetNationwideMarketData(IQueryable<MarketCarInfo> query, string make, string modelWord, string trim)
        {
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

            if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xk"))
            {
                modelWord = "xk";
            }
            if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xj"))
            {
                modelWord = "xj";
            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {

                var result = query.Where(i => i.Make == make &&
                                              ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                              i.CurrentPrice > 0 && i.Mileage > 0);

                return result;
            }

            else
            {
                if (modelWord.ToLower().Contains("rangeroversport"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroversport") &&
                                                   i.CurrentPrice > 0 && i.Mileage > 0));
                    return result;
                }
                else if (modelWord.ToLower().Equals("rangerover"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                 ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("rangerover") &&
                                                   !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("sport") &&
                                                i.CurrentPrice > 0 && i.Mileage > 0));

                    return result;
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                           ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                             i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                            i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }





        }

        public static IEnumerable<daily_market> GetNationwideMarketDataVersion2(IQueryable<daily_market> query, string make, string modelWord, string trim)
        {
            if (modelWord != null)
            {
                if (make.ToLower().Equals("bmw") && modelWord.ToLower().Contains("series"))
                {
                    modelWord = trim;
                }

                if (make.ToLower().Equals("mercedes-benz") && modelWord.ToLower().Contains("class"))
                {
                    modelWord = trim;

                    if (string.IsNullOrEmpty(modelWord))
                        modelWord = string.Empty;

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

                    if (modelWord.Length >= 1)
                    {

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

                }

                if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xk"))
                {
                    modelWord = "xk";
                }
                if (make.ToLower().Equals("jaguar") && modelWord.ToLower().Contains("xj"))
                {
                    modelWord = "xj";
                }
            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!make.ToLower().Equals("land rover"))
            {

                var result = query.Where(i => i.Make == make &&
                                              ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())));

                return result;
            }

            else
            {
                if (modelWord.ToLower().Contains("rangeroversport"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroversport") ));
                    return result;
                }
                else if (modelWord.ToLower().Contains("rangeroverevoque"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains("rangeroverevoque")));
                    return result;
                }
                else if (modelWord.ToLower().Equals("rangerover"))
                {
                    var result = query.Where(i => i.Make == make &&
                                                 ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("rangerover") &&
                                                   !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("sport") &&
                                                       !(i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                   i.Trim.Replace(" ", "").ToLower()).Contains("evoque")));

                    return result;
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                           ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                             i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) );

                    return result;
                }
            }
        }

        private static string FilterCarModelForMarket(string modelWord)
        {
            if (!String.IsNullOrEmpty(modelWord))
            {
                modelWord = modelWord.Replace("Sdn", "");

                modelWord = modelWord.Replace("XL", "");



                modelWord = modelWord.Replace("Sedan", "");

                modelWord = modelWord.Replace("Coupe", "");

                modelWord = modelWord.Replace("Cpe", "");

                modelWord = modelWord.Replace("Convertible", "");

                modelWord = modelWord.Replace("Utility", "");

                modelWord = modelWord.Replace("New", "");

                modelWord = modelWord.Replace("Hybrid", "");

                modelWord = modelWord.Replace("2D", "");

                modelWord = modelWord.Replace("4D", "");

                modelWord = modelWord.Replace("4MATIC", "");

                modelWord = modelWord.Replace("Unlimited", "");

                if (modelWord.ToLower().Contains("silverado 2500hd classic"))
                    modelWord = "Silverado 2500";

                if (modelWord.ToLower().Contains("silverado 2500hd"))
                    modelWord = "Silverado 2500";

                if (modelWord.ToLower().Contains("silverado 3500hd"))
                    modelWord = "Silverado 2500";

                if (modelWord.Contains("Super Duty"))
                    modelWord = modelWord.Replace("Super Duty", "");

                modelWord = modelWord.Replace(" ", "");

                modelWord = modelWord.Replace("-", "");

                modelWord = modelWord.Replace("4WDTruck", "");

                modelWord = modelWord.Replace("2WDTruck", "");

                modelWord = modelWord.Replace("AWDTruck", "");

                modelWord = modelWord.Replace("SRW", "");



                return modelWord.Trim();

            }

            else
                return String.Empty;
        }
    }
}
