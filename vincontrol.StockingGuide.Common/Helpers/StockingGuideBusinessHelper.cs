using System;

namespace vincontrol.StockingGuide.Common.Helpers
{
    public static class StockingGuideBusinessHelper
    {
        public static int GetSupplyFromStockAndHistory(int stock, int history)
        {
            return history != 0 ? (int)Math.Ceiling(((double)stock * 30) / history) : 0;
        }

        public static double GetTurnOverFromStockAndHistory(int stock, int history)
        {
            var result= stock != 0 ? ((double)history * 52) / stock : 0;
            result=Math.Round(result, 2);
            return result;
        }

        public static int GetAgeFromNow(DateTime startDate)
        {
            return GetAge(startDate, DateTime.Now);
        }

        public static int GetAge(DateTime startDate, DateTime endDate)
        {
            return endDate.Subtract(startDate).Days;
        }
    }
}
