using System;
using System.Collections.Generic;
using System.Linq;

namespace vincontrol.StockingGuide.Common.Helpers
{
    public class Parser
    {
        public static List<string> GetListBySeparatingCommas(string brandNames)
        {
            return brandNames==null?new List<string>() : brandNames.Split(',').Where(i=>!String.IsNullOrEmpty(i)).Select(i=>i.Trim()).ToList();
        }

        public static string GetStringFromList(IEnumerable<string> deletedMakeList)
        {
           return deletedMakeList.Aggregate(String.Empty, (current, item) => current + (item + ","));
        }
     
    }
}
