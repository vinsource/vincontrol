using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;

namespace vincontrol.Helper
{
    public class PriceChangeItem
    {
        public DateTime ChangedDate { get; set; }
        public decimal ChangedPrice { get; set; }
    }

    public static class DataHelper
    {
        public static bool IsPotentialTruck(string make)
        {
            make = make.ToLower();
            bool flag = make.Equals("ford") || make.Equals("chevrolet") || make.Equals("isuzu");

            return flag;
        }

        public static string FilterCarModelForMarket(string modelWord)
        {
            if (!String.IsNullOrEmpty(modelWord))
            {
                modelWord = modelWord.Replace("new", "");
                
                modelWord = modelWord.Replace("sdn", "");

                modelWord = modelWord.Replace("xl", "");

                modelWord = modelWord.Replace("wagon", "");

                modelWord = modelWord.Replace("(natl)", "");

                modelWord = modelWord.Replace("sedan", "");
                
                modelWord = modelWord.Replace("sdn", "");

                modelWord = modelWord.Replace("coupe", "");

                modelWord = modelWord.Replace("cpe", "");

                modelWord = modelWord.Replace("convertible", "");

                modelWord = modelWord.Replace("utility", "");

                modelWord = modelWord.Replace("2d", "");

                modelWord = modelWord.Replace("4d", "");

                modelWord = modelWord.Replace("4matic", "");

                modelWord = modelWord.Replace("unlimited", "");

                modelWord = modelWord.Replace("sportWagen", "");

                modelWord = modelWord.Replace("lwb", "");

                modelWord = modelWord.Replace("classic", "");

                modelWord = modelWord.Replace("cargo van", "");

                modelWord = modelWord.Replace("commercial cutaway", "");

                modelWord = modelWord.Replace("commercial chassis", "");

                modelWord = modelWord.Replace("passenger", "");

                modelWord = modelWord.Replace("super duty", "");

                modelWord = modelWord.Replace("drw", "");

                modelWord = modelWord.Replace(" ", "");

                modelWord = modelWord.Replace("-", "");

                modelWord = modelWord.Replace("4wdtruck", "");

                modelWord = modelWord.Replace("2wdtruck", "");

                modelWord = modelWord.Replace("awdtruck", "");

                modelWord = modelWord.Replace("srw", "");

                modelWord = modelWord.Replace("lwb", "");

                modelWord = modelWord.Replace("swb", "");
                
                return modelWord.Trim();
            }

            else return String.Empty;
        }

        #region Model mappings
        public static void BMW(string model)
        {

        }
        #endregion

        public static IQueryable<MarketCarInfo> GetNationwideMarketData(IQueryable<MarketCarInfo> query, string make, string modelWord, string trim, bool ignoredTrim)
        {
            var originalModelWorld = modelWord.ToLower();
            
                make = make.ToLower();
                modelWord = modelWord.ToLower();
                if (make.Equals("bmw") && modelWord.Contains("series"))
                {
                    modelWord = trim.ToLower();
                }

            if (make.Equals("mercedes-benz") && modelWord.Contains("class"))
            {
                modelWord = trim.ToLower();

                if (string.IsNullOrEmpty(modelWord)) modelWord = string.Empty;

                modelWord = modelWord.Replace("sport", "");

                modelWord = modelWord.Replace("luxury", "");

                modelWord = modelWord.Replace("bluetec", "");

                modelWord = modelWord.Replace("btc", "");

                modelWord = modelWord.Replace("cdi", "");

                modelWord = modelWord.Replace("blk", "");

                if (modelWord.Length >= 2)
                {
                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("s4"))

                        modelWord = modelWord.Replace("s4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("v4"))

                        modelWord = modelWord.Replace("v4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("w4"))

                        modelWord = modelWord.Replace("w4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("ae"))

                        modelWord = modelWord.Replace("ae", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("wz"))

                        modelWord = modelWord.Replace("wz", "");
                }

                if (modelWord.Length >= 1)
                {
                    if (modelWord[modelWord.Length - 1] == 'w')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);

                    if (modelWord[modelWord.Length - 1] == 'r')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);

                    if (modelWord[modelWord.Length - 1] == 'v')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);

                    if (modelWord[modelWord.Length - 1] == 'c')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);

                    if (modelWord[modelWord.Length - 1] == 'a')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);

                    if (modelWord[modelWord.Length - 1] == 'k')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);
                }

            }
             
            if (make.Equals("jaguar") && modelWord.Contains("xk"))
            {
                modelWord = "xk";
            }

            if (make.Equals("jaguar") && modelWord.Contains("xj"))
            {
                modelWord = "xj";
            }

            if (make.Equals("audi") && modelWord.Contains("a8 l"))
            {
                modelWord = "a8";
            }

            if (make.Equals("toyota") && modelWord.Contains("rav4 ev"))
            {
                modelWord = "rav4";
            }
            modelWord = FilterCarModelForMarket(modelWord);

            if (!String.IsNullOrEmpty(trim))
            {
                trim = trim.Replace("Base/Other Trim", "");
                trim = trim.ToLower();
            }

            if (make.Equals("land rover"))
            {
                if (modelWord.Contains("rangeroversport"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "range rover sport" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Contains("rangeroverevoque"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "range rover evoque" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("rangerover"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "range rover" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("lr4"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "lr4" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("lr3"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "lr3" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("lr2"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "lr2" && i.CurrentPrice > 0 && i.Mileage > 0);
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

            if (make.Equals("jaguar"))
            {
                if (modelWord.Equals("xf"))
                { 
                    var result = query.Where(i => i.Make == make && i.Model == "XF"&& i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("xj") || modelWord.Equals("xjl"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "XJ" || i.Model=="XJL" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
           
                if (modelWord.Equals("xk"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "XK" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                   
                }
            }

            if (make.Equals("mini"))
            {
                if (modelWord.Equals("cooperhardtop"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "cooper hardtop" &&
                                                     i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
             
                }

                if (modelWord.Equals("cooper"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "cooper" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("coopercountryman"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "cooper countryman" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("cooperclubman"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "cooper clubman" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("nissan"))
            {
                if (modelWord.Equals("370z"))
                {
                    if (trim.Equals("touring"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "370Z" && i.Trim.ToLower().Contains("touring") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else if (trim.Equals("coupe"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "370Z" && i.Trim.ToLower().Contains("coupe") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "370Z" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                
                if (modelWord.Equals("altima"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "altima" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "altima" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("murano"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "murano" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else if (trim.Contains("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "murano" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "murano" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("armada"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "armada" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else if (trim.Contains("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "armada" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "armada" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("quest"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "quest" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "quest" && i.Trim.ToLower().Contains("sv") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "quest" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("juke"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "juke" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "juke" && i.Trim.ToLower().Contains("sv") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                   
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "juke" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("rogue"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rogue" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rogue" && i.Trim.ToLower().Contains("sv") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
               
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rogue" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("sentra"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sr"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" && i.Trim.ToLower().Contains("sr") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("se-r"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" && i.Trim.ToLower().Contains("se-r") && i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
              
                  
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("pathfinder"))
                {
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "pathfinder" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "pathfinder" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "pathfinder" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("xterra"))
                {
                    
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "xterra" && i.Trim.ToLower().Contains("se")&& i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("x"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "xterra" && i.Trim.ToLower().Contains("x")&& i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "xterra" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }


            if (make.Equals("toyota"))
            {
                if (modelWord.Equals("avalon"))
                {
                    if (trim.Contains("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("xls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" && i.Trim.ToLower().Contains("xls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("camry"))
                {
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" && i.Trim.ToLower().Equals("le")&& i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("camrysolara"))
                {
                    if (trim.Equals("sle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry solara" && i.Trim.ToLower().Contains("sle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry solara" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry solara" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("venza"))
                {
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" && i.Trim.ToLower().Equals("le")  && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("corolla"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("l"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ce"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("ce") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xrs"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("xrs") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                  
                }
                if (modelWord.Equals("prius"))
                {
                    if (trim.Equals("one") || trim.ToLower().Equals("i"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("one") || i.Trim.ToLower().Equals("i")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("two")|| trim.ToLower().Equals("ii"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("two") || i.Trim.ToLower().Equals("ii")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("three")|| trim.ToLower().Equals("iii"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("three") || i.Trim.ToLower().Equals("iii")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("four")|| trim.ToLower().Equals("iv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("four") || i.Trim.ToLower().Equals("iv")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("five")|| trim.ToLower().Equals("v"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("five") || i.Trim.ToLower().Equals("v")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("sequoia"))
                {
                   
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sequoia" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sequoia" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("rav4"))
                {
                    if (trim.Equals("ltd")||trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rav4" && (i.Trim.ToLower().Contains("ltd")||i.Trim.ToLower().Contains("limited")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sport"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rav4" && i.Trim.ToLower().Contains("sport") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rav4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("camryhybrid"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "camry hybrid" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                      return result;
                    

                }
                if (modelWord.Equals("highlanderhybrid"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "highlander hybrid" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;


                }
                if (modelWord.Equals("camrysolara"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "camry solara" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;


                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("honda"))
            {
                if (modelWord.Equals("crv"))
                {

                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                }
                if (modelWord.Equals("accord"))
                {
                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("lx-s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-p"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("lx-p") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("ex") && !i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("odyssey"))
                {
                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("touring"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" && i.Trim.ToLower().Contains("touring") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                   
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("insight"))
                {
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "insight" && i.Trim.ToLower().Contains("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "insight" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "insight" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("civic"))
                {
                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("dx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("dx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("gx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("gx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("lx-s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-p"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("lx-p") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("ex")  && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("porsche"))
            {
                if (modelWord.Equals("cayenne"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cayenne" && i.Trim.ToLower().Contains("s") && !i.Trim.ToLower().Contains("gts") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("gts"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cayenne" && i.Trim.ToLower().Contains("gts") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cayenne" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("911"))
                {
                    if (trim.Equals("turbo"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "911" && i.Trim.ToLower().Contains("turbo") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("carrera"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "911" && i.Trim.ToLower().Contains("carrera") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "911" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
            
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("gmc"))
            {
                if (modelWord.Equals("yukon"))
                {
                    if (trim.Equals("slt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "yukon" && i.Trim.ToLower().Contains("slt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("denali"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "yukon" && i.Trim.ToLower().Contains("denali") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "yukon" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("chevrolet"))
            {
                if (modelWord.Equals("tahoe"))
                {
                    if (trim.Equals("ltz"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" && i.Trim.ToLower().Contains("ltz") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" && i.Trim.ToLower().Contains("lt") && !i.Trim.ToLower().Contains("ltz") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("suburban"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "suburban" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "suburban" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "suburban" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("trailblazer"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "trailblazer" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "trailblazer" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "trailblazer" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("camaro"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ss"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" && i.Trim.ToLower().Contains("ss") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("cruze"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ltz"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("ltz") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("eco"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("eco") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("cadillac"))
            {
                if (modelWord.Equals("escalade"))
                {
                    if (trim.Contains("platinum"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escalade" && i.Trim.ToLower().Contains("platinum") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("luxury"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escalade" && i.Trim.ToLower().Contains("luxury") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escalade" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("srx"))
                {
                    if (trim.Contains("performance"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" && i.Trim.ToLower().Contains("performance") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("luxury"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" && i.Trim.ToLower().Contains("luxury") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("premium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" && i.Trim.ToLower().Contains("premium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("jeep"))
            {
                if (modelWord.Equals("wrangler"))
                {
                    if (trim.Contains("x"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("x") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sahara"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("sahara") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("rubicon"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("rubicon") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("unlimited"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("unlimited") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                         
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "wrangler" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("liberty"))
                {
                    if (trim.Contains("sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "liberty" && i.Trim.ToLower().Contains("sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "liberty" && i.Trim.ToLower().Contains("limited") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                   
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "liberty" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("grandcherokee"))
                {
                    if (trim.Contains("overland"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "grand cherokee" && i.Trim.ToLower().Contains("overland") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("laredo"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "grand cherokee" && i.Trim.ToLower().Contains("laredo") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "grand cherokee" && i.Trim.ToLower().Contains("limited") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "grand cherokee" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("dodge"))
            {
                if (modelWord.Equals("durango"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "durango" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "durango" && i.Trim.ToLower().Contains("sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "durango" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                 
                }
                if (modelWord.Equals("challenger"))
                {
                    if (trim.Contains("sxt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "challenger" && i.Trim.ToLower().Contains("sxt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("r/t") || trim.Contains("rt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "challenger" && (i.Trim.ToLower().Contains("r/t") ||i.Trim.ToLower().Contains("rt") )&&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "challenger" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("ram1500"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "ram 1500" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "ram 1500" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("ram2500"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "ram 2500" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "ram 2500" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("ram3500"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "ram 3500" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "ram 3500" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }


            if (make.Equals("mazda"))
            {
                if (modelWord.Equals("tribute"))
                {
                    if (trim.Contains("grand touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tribute" && i.Trim.ToLower().Contains("touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("s"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tribute" && i.Trim.ToLower().Contains("s") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tribute" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("mazda3"))
                {
                    if (trim.Contains("i sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "mazda3" && i.Trim.ToLower().Contains("i sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "mazda3" && i.Trim.ToLower().Contains("touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "mazda3" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("cx9"))
                {
                    if (trim.Contains("grand touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cx-9" && i.Trim.ToLower().Contains("grand touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cx-9" && i.Trim.ToLower().Contains("touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cx-9" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

             if (make.Equals("volkswagen"))
            {
                if (modelWord.Equals("cc"))
                {
                    if (trim.Contains("luxury"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cc" && i.Trim.ToLower().Contains("luxury") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cc" && i.Trim.ToLower().Contains("sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cc" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("tiguan"))
                {
                       if (trim.Equals("sel"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tiguan" && i.Trim.ToLower().Contains("sel") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("s"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tiguan" && i.Trim.ToLower().Contains("s") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tiguan" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

          

            if (make.Equals("audi"))
            {
                if (modelWord.Equals("a6"))
                {
                    if (trim.Contains("premium plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium plus quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium plus quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("prestige quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("prestige quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium")&&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a6" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("a5"))
                {
                    if (trim.Contains("premium plus quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium plus quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                   
                    if (trim.Contains("premium quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium cabriolet"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium cabriolet") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("prestige quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("prestige quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a5" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("s5"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s5" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s5" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s5" && i.Trim.ToLower().Contains("premium") &&
                                    
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "s5" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("q5"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q5" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q5" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q5" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "q5" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("q7"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q7" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q7" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q7" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "q7" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("a4"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a4" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium")&&trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a4" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a4" && i.Trim.ToLower().Contains("premium")&&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("a3"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a3" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a3" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a3" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a3" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("s4"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s4" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s4" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s4" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "s4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("tt"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tt" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tt" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tt" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tt" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("ford"))
            {
                if (modelWord.Equals("flex"))
                {
                   
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                     if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("titanium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("titanium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("escape"))
                {
                    if (trim.Contains("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("xls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("xls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Equals("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("hybrid"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("hybrid") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("expedition"))
                {
                    if (trim.Contains("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "expedition" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                   
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "expedition" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("mustang"))
                {
                    if (trim.Contains("gt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "mustang" && i.Trim.ToLower().Contains("gt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "mustang" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("fiesta"))
                {
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ses"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("ses") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("sel") && !i.Trim.ToLower().Contains("ses") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                   
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("s") && !i.Trim.ToLower().Contains("sel") && !i.Trim.ToLower().Contains("ses") && !i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("fusion"))
                {
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && (i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("sel")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    if (trim.Equals("sport"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && i.Trim.ToLower().Contains("sport") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("titanium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && i.Trim.ToLower().Contains("titanium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
               
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("taurus"))
                {
                    if (trim.Equals("sel"))
                    {
                        var result =query.Where(i =>i.Make == make && i.Model == "taurus" && i.Trim.ToLower().Contains("sel") &&i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sho"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && i.Trim.ToLower().Contains("sho") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && (i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("sho") && !i.Trim.ToLower().Contains("sel")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("focus"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Equals("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Equals("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ses"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Contains("ses")&& i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("edge"))
                {
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Equals("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Equals("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("sport"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Contains("sport") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("explorer"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "explorer" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "explorer" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "explorer" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f150"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f250"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f350"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f450"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("mercedes-benz"))
            {
                if (originalModelWorld.Contains("class"))
                {
                    var classCategory = "";
                    
                    if(originalModelWorld.Contains("-"))
                        classCategory = originalModelWorld.Substring(0, originalModelWorld.IndexOf("-", System.StringComparison.Ordinal));
                    else
                    {
                        if(originalModelWorld.Contains(" "))
                            classCategory = originalModelWorld.Substring(0, originalModelWorld.IndexOf(" ", System.StringComparison.Ordinal));
                    }
                      if (classCategory.Equals("clk"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "clk"
                            && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("cls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "cls"
                             && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("c"))
                    {
                        var result = query.Where(i => i.Make == make &&i.Model.Substring(0,1) == "C" 
                            && !i.Model.Contains("CL")&&i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("cl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 2) == "CL"
                            && !i.Model.Contains("CLK") && !i.Model.Contains("CLS") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    if (classCategory.Equals("e"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "e" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("m"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "m" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("r"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "r" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                     if (classCategory.Equals("glk"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "glk" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("g"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "g" && !i.Model.Contains("gl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("gl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 2) == "gl" && !i.Model.Contains("glk") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "s" && !i.Model.Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 2) == "sl" && !i.Model.Contains("slk") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("slk"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "slk" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make &&
                                                      ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                        i.Trim.Replace(" ", "").ToLower()).Contains(
                                                            modelWord.Replace(" ", "").ToLower())) &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                }


                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }

                
            }

            if (make.Equals("bmw"))
            {
                if (modelWord.Equals("328ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "328i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("328i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "328i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("335i xdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "335i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("335i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "335i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("550ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "550i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("550i") || modelWord.Equals("530ia"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "550i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("535ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "535i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("535i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "535i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750li"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "750li" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750lixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && (i.Model == "750li xdrive" || i.Model == "750lixdrive") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750liactivehybrid"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains("750li") && i.Model.Contains("activehybrid") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "750i"&& i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && (i.Model == "750i xdrive" || i.Model == "750ixdrive") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750iactivehybrid"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains("750i") && i.Model.Contains("activehybrid") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("650ci"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "650ci" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Contains("alpinab7"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains("alpina b7") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                } 
                if (modelWord.Equals("x1"))
                {
                    if (trim.Equals("xdrive35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x1" && (i.Trim.Contains("xdrive35i") || i.Trim.Contains("xdrive 35i") ) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x1" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
               

                }
                if (modelWord.Equals("x3"))
                {
                    if (trim.Equals("28i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x3" && i.Trim.Contains("28i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x3" && i.Trim.Contains("35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x3" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x5"))
                {
                    if (trim.Equals("30i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("30i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("35d"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("35d") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("48i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("48i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("3.0i"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "x5" && i.Trim.Contains("3.0i") && i.CurrentPrice > 0 &&
                                    i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("4.8i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("4.8i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("3.0si"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("3.0si") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    if (trim.Equals("35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("4.4i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("4.4i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x5" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("z4"))
                {
                    if (trim.Equals("2.5i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("2.5i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("3.0i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("3.0i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sdrive35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("sdrive35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sdrive30i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("sdrive30i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "z4" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x6"))
                {
                    if (trim.Equals("xdrive35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x6" && i.Trim.Contains("xdrive35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xdrive50i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x6" && i.Trim.Contains("xdrive50i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x6" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x6m"))
                {

                    var result =
                          query.Where(i => i.Make == make && i.Model == "x6 m" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;



                }
                else
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains(modelWord) && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }



            }
            if (make.Equals("mitsubishi"))
            {
                if (modelWord.Equals("lancer"))
                {
                    if (trim.Equals("gts"))
                    {
                        var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("gts") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("es"))
                    {
                        var result =
                              query.Where(
                                  i => i.Make == make && i.Model == "lancer"&& i.Trim.ToLower().Contains("es") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("de"))
                    {
                        var result =
                              query.Where(
                                  i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("de") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ralliart"))
                    {
                        var result =
                              query.Where(
                                  i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("ralliart") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                  
                }

                if (modelWord.Equals("lancerevolution"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer evolution" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("lancersportback"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer sportback" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("outlander"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "outlander" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("outlander sport"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "outlander sport" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
            }


            if (make.Equals("infiniti"))
            {
                if (modelWord.Equals("g37"))
                {

                    if (trim.Contains("journey"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "g37" && i.Trim.Contains("journey") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result =
                    query.Where(
                        i => i.Make == make && i.Model == "g37"  && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

             
                }

             
            }

            if (make.Equals("kia"))
            {
                if (modelWord.Equals("sportage"))
                {

                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" && i.Trim.ToLower().Contains("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" && i.Trim.ToLower().Contains("sx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }
            if (make.Equals("hyundai"))
            {
                if (modelWord.Contains("genesis"))
                {

                    if (trim.Contains("r-spec"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Contains("genesis") && i.Trim.ToLower().Contains("r-spec") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("premium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Contains("genesis") && i.Trim.ToLower().Contains("premium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Contains("genesis") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }

            if (make.Equals("saturn"))
            {
                if (modelWord.Contains("vue"))
                {

                    if (trim.Contains("xe"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model=="vue" && i.Trim.ToLower().Contains("xe") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xr"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "vue" && i.Trim.ToLower().Contains("xr") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model=="vue"&&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }


            if (make.Equals("dodge"))
            {
                if (modelWord.Contains("nitro"))
                {

                    if (trim.Contains("slt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "nitro" && i.Trim.ToLower().Contains("slt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sxt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "nitro" && i.Trim.ToLower().Contains("sxt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "nitro" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }
         
            return query.Where(i => i.Make == make &&
                                    ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                      i.Trim.Replace(" ", "").ToLower()).Contains(modelWord.Replace(" ", "").ToLower())) &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
        }


        public static IQueryable<MarketCarInfo> GetNationwideMarketDataExactMatch(IQueryable<MarketCarInfo> query, string make,
                                                                         string modelWord, string trim, bool ignoredTrim)
        {
            var originalModelWorld = modelWord.ToLower();
          
                make = make.ToLower();
                modelWord = modelWord.ToLower();
                if (make.Equals("bmw") && modelWord.Contains("series"))
                {
                    modelWord = trim.ToLower();
                }

            if (make.Equals("mercedes-benz") && modelWord.Contains("class"))
            {
                modelWord = trim.ToLower();

                if (string.IsNullOrEmpty(modelWord))
                    modelWord = string.Empty;

                modelWord = modelWord.Replace("sport", "");

                modelWord = modelWord.Replace("luxury", "");

                modelWord = modelWord.Replace("bluetec", "");

                modelWord = modelWord.Replace("btc", "");

                modelWord = modelWord.Replace("cdi", "");

                modelWord = modelWord.Replace("blk", "");

                if (modelWord.Length >= 2)
                {

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("s4"))

                        modelWord = modelWord.Replace("s4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("v4"))

                        modelWord = modelWord.Replace("v4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("w4"))

                        modelWord = modelWord.Replace("w4", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("ae"))

                        modelWord = modelWord.Replace("ae", "");

                    if (modelWord.Substring(modelWord.Length - 2, 2).Equals("wz"))

                        modelWord = modelWord.Replace("wz", "");
                }

                if (modelWord.Length >= 1)
                {

                    if (modelWord[modelWord.Length - 1] == 'w')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);


                    if (modelWord[modelWord.Length - 1] == 'r')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);



                    if (modelWord[modelWord.Length - 1] == 'v')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);
                    if (modelWord[modelWord.Length - 1] == 'c')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);


                    if (modelWord[modelWord.Length - 1] == 'a')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);
                    if (modelWord[modelWord.Length - 1] == 'k')

                        modelWord = modelWord.Substring(0, modelWord.Length - 1);
                }

            }



            //if (make.Equals("mazda") && modelWord.Contains("mx-5 miata"))
            //{
            //    modelWord = "miata mx5";
            //} 
            if (make.Equals("jaguar") && modelWord.Contains("xk"))
            {
                modelWord = "xk";
            }
            if (make.Equals("jaguar") && modelWord.Contains("xj"))
            {
                modelWord = "xj";
            }
            if (make.Equals("audi") && modelWord.Contains("a8 l"))
            {
                modelWord = "a8";
            }
            if (make.Equals("toyota") && modelWord.Contains("rav4 ev"))
            {
                modelWord = "rav4";
            }

            modelWord = FilterCarModelForMarket(modelWord);

            if (!String.IsNullOrEmpty(trim))
            {
                trim = trim.Replace("Base/Other Trim", "");
                trim = trim.ToLower();
            }



            if (make.Equals("land rover"))
            {
                if (modelWord.Equals("rangeroversport"))
                {
                    if (trim.Contains("hse lux"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover sport" && i.Trim.ToLower().Contains("hse lux") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Contains("hse"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover sport" && i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }

                    if (trim.Equals("sc") || trim.Equals("supercharged"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover sport" && i.Trim.ToLower().Contains("sc") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover sport" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Contains("rangeroverevoque"))
                {
                    if (trim.Contains("pure plus"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover evoque" && i.Trim.ToLower().Contains("pure plus") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Contains("pure premium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover evoque" && i.Trim.ToLower().Contains("pure premium") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Contains("pure"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover evoque" && i.Trim.ToLower().Equals("pure") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover evoque" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("rangerover"))
                {
                    if (trim.Contains("hse lux"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover" && i.Trim.ToLower().Contains("hse lux") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Contains("hse"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover" && i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Equals("sc"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover" && i.Trim.ToLower().Contains("sc") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "range rover" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("lr4"))
                {
                    if (trim.Contains("hse"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr4" && i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr4" && i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("lr3"))
                {
                    if (trim.Contains("hse"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr3" && i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr3" && i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr3" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("lr2"))
                {
                    if (trim.Contains("hse"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr2" && i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr2" && i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("hse") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "lr2" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

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

            if (make.Equals("jaguar"))
            {
                if (modelWord.Equals("xf"))
                {

                    if (trim.Equals("supercharged"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XF" && (i.Trim.ToLower().Contains("supercharged") || i.Trim.ToLower().Contains("sc")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("portfolio"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XF" && i.Trim.ToLower().Contains("portfolio") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("luxury") || trim.Contains("premium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XF" && (i.Trim.ToLower().Contains("luxury") || i.Trim.ToLower().Contains("premium")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XF" && !i.Trim.ToLower().Contains("supercharged") && !i.Trim.ToLower().Contains("portfolio") &&
                                             !i.Trim.ToLower().Contains("premium") && !i.Trim.ToLower().Contains("luxury") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("xj") || modelWord.Equals("xjl"))
                {
                    if (trim.Equals("xjl supercharged"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XJ" && i.Trim.ToLower().Contains("xjl supercharged") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("supercharged"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XJ" && i.Trim.ToLower().Contains("supercharged") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("vdp"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XJ" && i.Trim.ToLower().Contains("vdp") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    if (trim.Equals("xjl ultimate"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XJ" && i.Trim.ToLower().Contains("xjl ultimate") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xjl portfolio"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XJ" && i.Trim.ToLower().Contains("xjl portfolio") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XJ" && !i.Trim.ToLower().Contains("supercharged") &&
                                                      !i.Trim.ToLower().Contains("xjl supercharged") &&
                                                      !i.Trim.ToLower().Contains("xjl ultimate") && !i.Trim.ToLower().Contains("xjl portfolio") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("xk"))
                {
                    if (trim.Contains("xkr"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XK" && i.Trim.ToLower().Contains("xkr") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("touring"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XK" && i.Trim.ToLower().Contains("touring") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "XK" && !i.Trim.ToLower().Contains("touring") &&
                                                      !i.Trim.ToLower().Contains("xkr") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;


                }
            }
            if (make.Equals("mini"))
            {
                if (modelWord.Equals("cooperhardtop"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper hardtop" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper hardtop" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("cooper"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("coopercountryman"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper countryman" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper countryman" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("cooperclubman"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper clubman" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cooper clubman" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }


            if (make.Equals("nissan"))
            {
                if (modelWord.Equals("370z"))
                {
                    if (trim.Equals("touring"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "370Z" && i.Trim.ToLower().Contains("touring") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else if (trim.Equals("coupe"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "370Z" && i.Trim.ToLower().Contains("coupe") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "370Z" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("altima"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "altima" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "altima" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("murano"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "murano" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                    else if (trim.Contains("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "murano" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "murano" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("armada"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "armada" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else if (trim.Contains("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "armada" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "armada" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("quest"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "quest" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "quest" && i.Trim.ToLower().Contains("sv") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "quest" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("juke"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "juke" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "juke" && i.Trim.ToLower().Contains("sv") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
               
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "juke" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("rogue"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rogue" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rogue" && i.Trim.ToLower().Contains("sv") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                 
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rogue" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("sentra"))
                {
                    if (trim.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" && i.Trim.ToLower().Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sr"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" && i.Trim.ToLower().Contains("sr") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("se-r"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" && i.Trim.ToLower().Contains("se-r") && i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                 

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sentra" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("pathfinder"))
                {
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "pathfinder" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "pathfinder" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "pathfinder" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("xterra"))
                {
                  
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "xterra" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("x"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "xterra" && i.Trim.ToLower().Contains("x") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "xterra" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }


            if (make.Equals("toyota"))
            {
                if (modelWord.Equals("avalon"))
                {
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" && i.Trim.ToLower().Contains("xls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "avalon" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("camry"))
                {
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camry" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("venza"))
                {
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "venza" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("corolla"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("l"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("le"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("le") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ce"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("ce") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xle"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("xle") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xrs"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" && i.Trim.ToLower().Contains("xrs") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "corolla" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("prius"))
                {
                    if (trim.Equals("one") || trim.ToLower().Equals("i"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("one") || i.Trim.ToLower().Equals("i")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("two") || trim.ToLower().Equals("ii"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("two") || i.Trim.ToLower().Equals("ii")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("three") || trim.ToLower().Equals("iii"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("three") || i.Trim.ToLower().Equals("iii")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("four") || trim.ToLower().Equals("iv"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("four") || i.Trim.ToLower().Equals("iv")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("five") || trim.ToLower().Equals("v"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" && (i.Trim.ToLower().Contains("five") || i.Trim.ToLower().Equals("v")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "prius" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("sequoia"))
                {
                   
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sequoia" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sequoia" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("rav4"))
                {
                    if (trim.Equals("ltd") || trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rav4" && (i.Trim.ToLower().Contains("ltd") || i.Trim.ToLower().Contains("limited")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sport"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rav4" && i.Trim.ToLower().Contains("sport") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "rav4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("camryhybrid"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "camry hybrid" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;


                }
                if (modelWord.Equals("highlanderhybrid"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "highlander hybrid" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;


                }
                if (modelWord.Equals("camrysolara"))
                {
                    var result = query.Where(i => i.Make == make && i.Model == "camry solara" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;


                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("honda"))
            {
                if (modelWord.Equals("crv"))
                {

                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Equals("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cr-v" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("accord"))
                {
                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("lx-s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-p"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("lx-p") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" && i.Trim.ToLower().Contains("ex") && !i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "accord" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("odyssey"))
                {
                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("touring"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" && i.Trim.ToLower().Contains("touring") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "odyssey" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("insight"))
                {
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "insight" && i.Trim.ToLower().Contains("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "insight" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "insight" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("civic"))
                {
                    if (trim.Equals("ex-l") || trim.ToLower().Equals("exl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("ex-l") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("dx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("dx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("gx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("gx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("lx-s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx-p"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("lx-p") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" && i.Trim.ToLower().Contains("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "civic" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("porsche"))
            {
                if (modelWord.Equals("cayenne"))
                {
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cayenne" && i.Trim.ToLower().Contains("s") && !i.Trim.ToLower().Contains("gts") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("gts"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cayenne" && i.Trim.ToLower().Contains("gts") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cayenne" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("911"))
                {
                    if (trim.Equals("turbo"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "911" && i.Trim.ToLower().Contains("turbo") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("carrera"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "911" && i.Trim.ToLower().Contains("carrera") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "911" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("gmc"))
            {
                if (modelWord.Equals("yukon"))
                {
                    if (trim.Equals("slt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "yukon" && i.Trim.ToLower().Contains("slt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("denali"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "yukon" && i.Trim.ToLower().Contains("denali") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "yukon" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("chevrolet"))
            {
                if (modelWord.Equals("tahoe"))
                {
                    if (trim.Equals("ltz"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" && i.Trim.ToLower().Contains("ltz") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" && i.Trim.ToLower().Contains("lt") && !i.Trim.ToLower().Contains("ltz") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tahoe" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("suburban"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "suburban" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "suburban" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "suburban" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("trailblazer"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "trailblazer" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "trailblazer" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "trailblazer" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("camaro"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ss"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" && i.Trim.ToLower().Contains("ss") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "camaro" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("cruze"))
                {
                    if (trim.Equals("lt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("lt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("ls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ltz"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("ltz") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("eco"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" && i.Trim.ToLower().Contains("eco") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cruze" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("cadillac"))
            {
                if (modelWord.Equals("escalade"))
                {
                    if (trim.Contains("platinum"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escalade" && i.Trim.ToLower().Contains("platinum") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("luxury"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escalade" && i.Trim.ToLower().Contains("luxury") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escalade" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("srx"))
                {
                    if (trim.Contains("performance"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" && i.Trim.ToLower().Contains("performance") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("luxury"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" && i.Trim.ToLower().Contains("luxury") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("premium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" && i.Trim.ToLower().Contains("premium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "srx" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }
            if (make.Equals("jeep"))
            {
                if (modelWord.Equals("wrangler"))
                {
                    if (trim.Contains("x"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("x") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sahara"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("sahara") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("rubicon"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("rubicon") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("unlimited"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "wrangler" && i.Trim.ToLower().Contains("unlimited") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "wrangler" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("liberty"))
                {
                    if (trim.Contains("sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "liberty" && i.Trim.ToLower().Contains("sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "liberty" && i.Trim.ToLower().Contains("limited") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "liberty" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("grandcherokee"))
                {
                    if (trim.Contains("overland"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "grand cherokee" && i.Trim.ToLower().Contains("overland") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("laredo"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "grand cherokee" && i.Trim.ToLower().Contains("laredo") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "grand cherokee" && i.Trim.ToLower().Contains("limited") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "grand cherokee" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("dodge"))
            {
                if (modelWord.Equals("durango"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "durango" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "durango" && i.Trim.ToLower().Contains("sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "durango" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("ram1500"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "ram 1500" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "ram 1500" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("ram2500"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "ram 2500" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "ram 2500" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("ram3500"))
                {
                    if (trim.Contains("slt"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "ram 3500" && i.Trim.ToLower().Contains("slt") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "ram 3500" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }


            if (make.Equals("mazda"))
            {
                if (modelWord.Equals("tribute"))
                {
                    if (trim.Contains("grand touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tribute" && i.Trim.ToLower().Contains("touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("s"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tribute" && i.Trim.ToLower().Contains("s") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tribute" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("mazda3"))
                {
                    if (trim.Contains("i sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "mazda3" && i.Trim.ToLower().Contains("i sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "mazda3" && i.Trim.ToLower().Contains("touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "mazda3" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("cx9"))
                {
                    if (trim.Contains("grand touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cx-9" && i.Trim.ToLower().Contains("grand touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("touring"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cx-9" && i.Trim.ToLower().Contains("touring") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cx-9" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("volkswagen"))
            {
                if (modelWord.Equals("cc"))
                {
                    if (trim.Contains("luxury"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cc" && i.Trim.ToLower().Contains("luxury") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sport"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "cc" && i.Trim.ToLower().Contains("sport") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "cc" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("tiguan"))
                {
                    if (trim.Equals("s"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tiguan" && i.Trim.ToLower().Contains("s")&&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sel"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tiguan" && i.Trim.ToLower().Contains("sel") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tiguan" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }



            if (make.Equals("audi"))
            {
                if (modelWord.Equals("a6"))
                {
                    if (trim.Contains("premium plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium plus quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium plus quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("prestige quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("prestige quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a6" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a6" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("a5"))
                {
                    if (trim.Contains("premium plus quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium plus quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium cabriolet"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium cabriolet") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("prestige quatro"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("prestige quatro") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a5" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a5" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("s5"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s5" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s5" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s5" && i.Trim.ToLower().Contains("premium") &&

                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "s5" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("q5"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q5" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q5" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q5" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "q5" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("q7"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q7" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q7" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "q7" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "q7" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("a4"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a4" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a4" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a4" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("a3"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a3" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a3" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "a3" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "a3" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("s4"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s4" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium") && trim.ToLower().Contains("plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s4" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "s4" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "s4" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }
                if (modelWord.Equals("tt"))
                {
                    if (trim.Contains("prestige"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tt" && i.Trim.ToLower().Contains("prestige") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("premium plus"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tt" && i.Trim.ToLower().Contains("premium plus") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Contains("premium"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "tt" && i.Trim.ToLower().Contains("premium") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "tt" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("ford"))
            {
                if (modelWord.Equals("flex"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("titanium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" && i.Trim.ToLower().Contains("titanium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "flex" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("escape"))
                {
                    if (trim.Contains("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("xls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("xls") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("hybrid"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "escape" && i.Trim.ToLower().Contains("hybrid") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "escape" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("expedition"))
                {
                    if (trim.Contains("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "expedition" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "expedition" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("mustang"))
                {
                    if (trim.Contains("gt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "mustang" && i.Trim.ToLower().Contains("gt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "mustang" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("fiesta"))
                {
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ses"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("ses") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("sel") && !i.Trim.ToLower().Contains("ses") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" && i.Trim.ToLower().Contains("s") && !i.Trim.ToLower().Contains("sel") && !i.Trim.ToLower().Contains("ses") && !i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fiesta" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("fusion"))
                {
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && (i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("sel")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("sport"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && i.Trim.ToLower().Contains("sport") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("titanium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" && i.Trim.ToLower().Contains("titanium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "fusion" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("taurus"))
                {
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sho"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && i.Trim.ToLower().Contains("sho") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && (i.Trim.ToLower().Contains("se") && !i.Trim.ToLower().Contains("sho") && !i.Trim.ToLower().Contains("sel")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "taurus" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("focus"))
                {
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Contains("s") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ses"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Contains("ses") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "focus" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }

                if (modelWord.Equals("edge"))
                {
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sel"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Contains("sel") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("se"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Contains("se") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Contains("sport"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" && i.Trim.ToLower().Contains("sport") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "edge" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("explorer"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "explorer" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("limited"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "explorer" && i.Trim.ToLower().Contains("limited") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "explorer" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f150"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f150" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f250"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f250" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f350"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f350" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                if (modelWord.Equals("f450"))
                {
                    if (trim.Equals("xlt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" && i.Trim.ToLower().Contains("xlt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" && i.Trim.ToLower().Contains("xl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lariat"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" && i.Trim.ToLower().Contains("lariat") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "f450" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }
            }

            if (make.Equals("mercedes-benz"))
            {
                if (originalModelWorld.Contains("class"))
                {
                    var classCategory = "";
                    if (originalModelWorld.Contains("-"))
                        classCategory = originalModelWorld.Substring(0, originalModelWorld.IndexOf("-", System.StringComparison.Ordinal));
                    else
                    {
                        if (originalModelWorld.Contains(" "))
                            classCategory = originalModelWorld.Substring(0, originalModelWorld.IndexOf(" ", System.StringComparison.Ordinal));
                    }

                    if (classCategory.Equals("c"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "C"
                            && !i.Model.Contains("CL") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("cl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 2) == "CL"
                            && !i.Model.Contains("CLK") && !i.Model.Contains("CLS") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("clk"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "clk"
                            && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("cls"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "cls"
                             && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("e"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "e" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("m"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "m" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("r"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "r" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (classCategory.Equals("g"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "g" && !i.Model.Contains("gl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("gl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 2) == "gl" && !i.Model.Contains("glk") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("glk"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "glk" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("s"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 1) == "s" && !i.Model.Contains("sl") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("sl"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 2) == "sl" && !i.Model.Contains("slk") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (classCategory.Equals("slk"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Substring(0, 3) == "slk" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make &&
                                                      ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                        i.Trim.Replace(" ", "").ToLower()).Contains(
                                                            modelWord.Replace(" ", "").ToLower())) &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);

                        return result;
                    }
                }


                else
                {
                    var result = query.Where(i => i.Make == make &&
                                                  ((i.Model.Replace("-", "").Replace(" ", "").ToLower() +
                                                    i.Trim.Replace(" ", "").ToLower()).Contains(
                                                        modelWord.Replace(" ", "").ToLower())) &&
                                                  i.CurrentPrice > 0 && i.Mileage > 0);

                    return result;
                }


            }

            if (make.Equals("bmw"))
            {
                if (modelWord.Equals("328ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "328i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("328i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "328i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("335i xdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "335i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("335i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "335i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("550ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "550i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("550i") || modelWord.Equals("530ia"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "550i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("535ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "535i xdrive" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("535i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "535i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750li"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "750li" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750lixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && (i.Model == "750li xdrive" || i.Model == "750lixdrive") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750liactivehybrid"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains("750li") && i.Model.Contains("activehybrid") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750i"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "750i" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750ixdrive"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && (i.Model == "750i xdrive" || i.Model == "750ixdrive") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("750iactivehybrid"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains("750i") && i.Model.Contains("activehybrid") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("650ci"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model == "650ci" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Contains("alpinab7"))
                {
                    var result =
                        query.Where(
                            i => i.Make == make && i.Model.Contains("alpina b7") && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("x1"))
                {
                    if (trim.Equals("xdrive35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x1" && (i.Trim.Contains("xdrive35i") || i.Trim.Contains("xdrive 35i")) && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x1" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x3"))
                {
                    if (trim.Equals("28i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x3" && i.Trim.Contains("28i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x3" && i.Trim.Contains("35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x3" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x5"))
                {
                    if (trim.Equals("30i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("30i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("35d"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("35d") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("48i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("48i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("3.0i"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "x5" && i.Trim.Contains("3.0i") && i.CurrentPrice > 0 &&
                                    i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("4.8i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("4.8i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("3.0si"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("3.0si") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    if (trim.Equals("35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("4.4i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x5" && i.Trim.Contains("4.4i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x5" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("z4"))
                {
                    if (trim.Equals("2.5i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("2.5i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("3.0i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("3.0i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sdrive35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("sdrive35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sdrive30i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "z4" && i.Trim.Contains("sdrive30i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "z4" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x6"))
                {
                    if (trim.Equals("xdrive35i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x6" && i.Trim.Contains("xdrive35i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xdrive50i"))
                    {
                        var result =
                            query.Where(
                                i => i.Make == make && i.Model == "x6" && i.Trim.Contains("xdrive50i") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "x6" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }
                if (modelWord.Equals("x6m"))
                {

                    var result =
                        query.Where(i => i.Make == make && i.Model == "x6 m" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;



                }
                else
                {
                    var result = query.Where(i => i.Make == make && i.Model.Contains(modelWord) && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }



            }
            if (make.Equals("mitsubishi"))
            {
                if (modelWord.Equals("lancer"))
                {
                    if (trim.Equals("gts"))
                    {
                        var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("gts") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("es"))
                    {
                        var result =
                              query.Where(
                                  i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("es") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("de"))
                    {
                        var result =
                              query.Where(
                                  i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("ed") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("ralliart"))
                    {
                        var result =
                              query.Where(
                                  i => i.Make == make && i.Model == "lancer" && i.Trim.ToLower().Contains("ralliart") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                }
               

                if (modelWord.Equals("lancerevolution"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer evolution" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }

                if (modelWord.Equals("lancersportback"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "lancer sportback" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("outlander"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "outlander" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
                if (modelWord.Equals("outlander sport"))
                {
                    var result =
                           query.Where(
                               i => i.Make == make && i.Model == "outlander sport" && i.CurrentPrice > 0 && i.Mileage > 0);
                    return result;
                }
            }


            if (make.Equals("infiniti"))
            {
                if (modelWord.Equals("g37"))
                {

                    if (trim.Contains("journey"))
                    {
                        var result =
                            query.Where(
                                i =>
                                    i.Make == make && i.Model == "g37" && i.Trim.Contains("journey") &&
                                    i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result =
                    query.Where(
                        i => i.Make == make && i.Model == "g37" && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }


                }


            }


            if (make.Equals("kia"))
            {
                if (modelWord.Equals("sportage"))
                {

                    if (trim.Equals("ex"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" && i.Trim.ToLower().Contains("ex") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("lx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" && i.Trim.ToLower().Contains("lx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sx"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" && i.Trim.ToLower().Contains("sx") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "sportage" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }

            if (make.Equals("hyundai"))
            {
                if (modelWord.Contains("genesis"))
                {

                    if (trim.Contains("r-spec"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Contains("genesis") && i.Trim.ToLower().Contains("r-spec") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("premium"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Contains("genesis") && i.Trim.ToLower().Contains("premium") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                  
                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model.Contains("genesis") &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }

            if (make.Equals("saturn"))
            {
                if (modelWord.Contains("vue"))
                {

                    if (trim.Contains("xe"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "vue" && i.Trim.ToLower().Contains("xe") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("xr"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "vue" && i.Trim.ToLower().Contains("xr") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "vue" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }

            if (make.Equals("dodge"))
            {
                if (modelWord.Contains("nitro"))
                {

                    if (trim.Contains("slt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "nitro" && i.Trim.ToLower().Contains("slt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }
                    if (trim.Equals("sxt"))
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "nitro" && i.Trim.ToLower().Contains("sxt") && i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                    else
                    {
                        var result = query.Where(i => i.Make == make && i.Model == "nitro" &&
                                                      i.CurrentPrice > 0 && i.Mileage > 0);
                        return result;
                    }

                }


            }

            return query.Where(i => i.Make == make && (i.Model.Replace("-", "").Replace(" ", "").ToLower() + i.Trim.Replace(" ", "").ToLower()).Contains(modelWord) &&
                                                i.CurrentPrice > 0 && i.Mileage > 0);
        }


    }
}
