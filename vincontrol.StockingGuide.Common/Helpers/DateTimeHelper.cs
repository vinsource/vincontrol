using System;
using System.Collections.Generic;
using System.Globalization;

namespace vincontrol.StockingGuide.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static int GetWeekNumber(this DateTime dateTime)
        {
            var first = new DateTime(dateTime.Year, dateTime.Month, 1);
            return dateTime.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            var ci = CultureInfo.CurrentCulture;
            return ci.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static Dictionary<short,DateTime> GetEndOfWeekList(this DateTime dateTime)
        {
            var endOfWeekList = new Dictionary<short, DateTime>();
            var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var lastDayOfTheMonth = firstDayOfTheMonth.AddMonths(1).AddDays(-1);
            short index = 1;
            while (firstDayOfTheMonth <= lastDayOfTheMonth)
            {
                if (firstDayOfTheMonth.DayOfWeek == DayOfWeek.Sunday)
                {
                    endOfWeekList.Add(index,firstDayOfTheMonth);
                    index++;
                }
                firstDayOfTheMonth = firstDayOfTheMonth.AddDays(1);
                
            }
            return endOfWeekList;
        }

        public static Dictionary<short, DateTime> GetEndOfWeekListToCurrentDate(this DateTime dateTime)
        {
            var endOfWeekList = new Dictionary<short, DateTime>();
            var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            short index = 1;
            while (firstDayOfTheMonth <= dateTime)
            {
                if (firstDayOfTheMonth.DayOfWeek == DayOfWeek.Sunday)
                {
                    endOfWeekList.Add(index, firstDayOfTheMonth);
                    index++;
                }
                firstDayOfTheMonth = firstDayOfTheMonth.AddDays(1);

            }

            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return endOfWeekList;
            }

            endOfWeekList.Add(index,dateTime);
            return endOfWeekList;
        }
    }
}
