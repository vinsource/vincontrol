using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using vincontrol.Data.Model;

namespace Vincontrol.Brochure
{
    public static class ChromeHelper
    {
        public static List<int> GetYears()
        {
            using (var context = new VincontrolEntities())
            {
                return context.YearMakes.Select(i => i.Year).Distinct().OrderByDescending(i => i).ToList();
            }
        }

        public static List<YearMakeItem> GetMakes(int year)
        {
            using (var context = new VincontrolEntities())
            {
                return
                    context.YearMakes.Where(i => i.Year == year).Select(i => new YearMakeItem { MakeValue = i.Make.Value, YearMakeId = i.YearMakeId }).Distinct().OrderBy(x => x.MakeValue)
                        .ToList();
            }
        }

        public static List<ModelItem> GetModels(int yearMakeId)
        {
            using (var context = new VincontrolEntities())
            {
                return
                    context.Models.Where(i => i.YearMakeId == yearMakeId).Select(i => new ModelItem { ModelId = i.ModelId, ModelValue = i.Value }).Distinct().OrderBy(x => x.ModelValue)
                        .ToList();
            }
        }

        public static List<TrimItem> GetTrims(int modelId)
        {
            using (var context = new VincontrolEntities())
            {
                return
                    context.Trims.Where(i => i.ModelId == modelId)
                           .Select(i => new TrimItem { TrimId = i.TrimId, TrimValue = i.StyleName })
                           .OrderBy(x => x.TrimValue)
                           .GroupBy(r => r.TrimValue)
                           .Select(g => g.FirstOrDefault()).ToList();
            }
        }

        public static TradeinInfo GetTradeInInfo(int trimId)
        {
            using (var context = new VincontrolEntities())
            {
                var trimTradeIns = context.TrimTradeIns.FirstOrDefault(i => i.TrimId == trimId);
                if (trimTradeIns == null)
                    return null;
                return new TradeinInfo(){EstimatedZeroPointMileage = trimTradeIns.EstimatedZeroPointMileage,TradeInValue = trimTradeIns.TradeInValue, SampleVIN = trimTradeIns.SampleVIN};
            }
        }

        public static List<TradeInReport> GetTradeInReport()
        {
            using (var context = new VincontrolEntities())
            {
                var result = (from y in context.YearMakes
                              join m in context.Makes on y.MakeId equals m.MakeId
                              join mo in context.Models on y.YearMakeId equals mo.YearMakeId
                              join tr in context.Trims.Distinct() on mo.ModelId equals tr.ModelId
                              join tt in context.TrimTradeIns on tr.TrimId equals tt.TrimId into temp
                              from tt in temp.DefaultIfEmpty()
                              where tt == null
                              select
                                  new TradeInReport
                                      {
                                          Year = y.Year,
                                          Make = m.Value,
                                          Model = mo.Value,
                                          TrimName = tr.StyleName
                                      }).Distinct().OrderByDescending(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.TrimName).ToList();
                return result;
            }
        }

        public static List<TradeInReport> GetSampleVinReport()
        {
            using (var context = new VincontrolEntities())
            {
                var result = (from y in context.YearMakes
                              join m in context.Makes on y.MakeId equals m.MakeId
                              join mo in context.Models on y.YearMakeId equals mo.YearMakeId
                              join tr in context.Trims.Distinct() on mo.ModelId equals tr.ModelId
                              join tt in context.TrimTradeIns on tr.TrimId equals tt.TrimId into temp
                              from tt in temp.DefaultIfEmpty()
                              where tt == null || string.IsNullOrEmpty(tt.SampleVIN)
                              select
                                  new TradeInReport
                                      {
                                          Year = y.Year,
                                          Make = m.Value,
                                          Model = mo.Value,
                                          TrimName = tr.TrimName
                                      }).Distinct()
                                        .OrderByDescending(x => x.Year)
                                        .ThenBy(x => x.Make)
                                        .ThenBy(x => x.Model)
                                        .ThenBy(x => x.TrimName)
                                        .ToList();
                return result;
            }
        }


    }

    public class YearMakeItem
    {
        public int YearMakeId { get; set; }
        public string MakeValue { get; set; }
    }

    public class ModelItem
    {
        public int ModelId { get; set; }
        public string ModelValue { get; set; }
    }

    public class TrimItem
    {
        public int TrimId { get; set; }
        public string TrimValue { get; set; }
    }

    public class TradeInReport
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string TrimName { get; set; }
    }

    public class YearMakeComparer : IEqualityComparer<YearMake>
    {
        public bool Equals(YearMake x, YearMake y)
        {
            return x.Make == y.Make;
        }

        public int GetHashCode(YearMake obj)
        {
            return obj.GetHashCode();
        }
    }

    public class TradeinInfo
    {
        public long EstimatedZeroPointMileage { get; set; }
        public string SampleVIN { get; set; }
        public decimal TradeInValue { get; set; }
    }
}
