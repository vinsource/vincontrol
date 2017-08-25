using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Constant
{
    public enum Quarter
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class DateUtilities
    {
        #region Quarters

        public static DateTime GetStartOfQuarter(int year, Quarter qtr)
        {
            switch (qtr)
            {
                case Quarter.First:
                    return new DateTime(year, 1, 1, 0, 0, 0, 0);
                case Quarter.Second:
                    return new DateTime(year, 4, 1, 0, 0, 0, 0);
                case Quarter.Third:
                    return new DateTime(year, 7, 1, 0, 0, 0, 0);
                default:
                    return new DateTime(year, 10, 1, 0, 0, 0, 0);
            }
        }

        public static DateTime GetEndOfQuarter(int year, Quarter qtr)
        {
            switch (qtr)
            {
                case Quarter.First:
                    return new DateTime(year, 3, DateTime.DaysInMonth(year, 3), 23, 59, 59, 999);
                case Quarter.Second:
                    return new DateTime(year, 6, DateTime.DaysInMonth(year, 6), 23, 59, 59, 999);
                case Quarter.Third:
                    return new DateTime(year, 9, DateTime.DaysInMonth(year, 9), 23, 59, 59, 999);
                default:
                    return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
            }
        }

        public static Quarter GetQuarter(Month month)
        {
            if (month <= Month.March)
                // 1st Quarter = January 1 to March 31
                return Quarter.First;
            else if ((month >= Month.April) && (month <= Month.June))
                // 2nd Quarter = April 1 to June 30
                return Quarter.Second;
            else if ((month >= Month.July) && (month <= Month.September))
                // 3rd Quarter = July 1 to September 30
                return Quarter.Third;
            else // 4th Quarter = October 1 to December 31
                return Quarter.Fourth;
        }

        public static DateTime GetEndOfLastQuarter()
        {
            return (Month) Now().Month <= Month.March
                ? GetEndOfQuarter(Now().Year - 1, Quarter.Fourth)
                : GetEndOfQuarter(Now().Year, GetQuarter((Month)Now().Month));
        }

        public static DateTime GetStartOfLastQuarter()
        {
            return (Month)Now().Month <= Month.March
                ? GetStartOfQuarter(Now().Year - 1, Quarter.Fourth)
                : GetStartOfQuarter(Now().Year, GetQuarter((Month)Now().Month));
        }

        public static DateTime GetStartOfCurrentQuarter()
        {
            return GetStartOfQuarter(Now().Year, GetQuarter((Month)Now().Month));
        }

        public static DateTime GetEndOfCurrentQuarter()
        {
            return GetEndOfQuarter(Now().Year, GetQuarter((Month)Now().Month));
        }
        #endregion

        #region Weeks
        public static DateTime GetStartOfLastWeek()
        {
            var daysToSubtract = (int)Now().DayOfWeek + 7;
            var dt = Now().Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfLastWeek()
        {
            var dt = GetStartOfLastWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        public static DateTime GetStartOfCurrentWeek()
        {
            var daysToSubtract = (int)Now().DayOfWeek;
            var dt = Now().Subtract(System.TimeSpan.FromDays(daysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfCurrentWeek()
        {
            var dt = GetStartOfCurrentWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }
        #endregion

        #region Months

        public static DateTime GetStartOfMonth(Month month, int year)
        {
            return new DateTime(year, (int)month, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfMonth(Month month, int year)
        {
            return new DateTime(year, (int)month, DateTime.DaysInMonth(year, (int)month), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastMonth()
        {
            return Now().Month == 1
                ? GetStartOfMonth(Month.December, Now().Year - 1)
                : GetStartOfMonth((Month)Now().Month - 1, Now().Year);
        }

        public static DateTime GetEndOfLastMonth()
        {
            return Now().Month == 1
                ? GetEndOfMonth((Month)12, Now().Year - 1)
                : GetEndOfMonth((Month)Now().Month - 1, Now().Year);
        }

        public static DateTime GetStartOfCurrentMonth()
        {
            return GetStartOfMonth((Month)Now().Month, Now().Year);
        }

        public static DateTime GetEndOfCurrentMonth()
        {
            return GetEndOfMonth((Month)Now().Month, Now().Year);
        }
        #endregion

        #region Years
        public static DateTime GetStartOfYear(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfYear(int year)
        {
            return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(Now().Year - 1);
        }

        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(Now().Year - 1);
        }

        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(Now().Year);
        }

        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(Now().Year);
        }
        #endregion

        #region Days

        public static DateTime Now()
        {
            return DateTime.Now;
        }

        public static DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public static DateTime Date()
        {
            return new DateTime(Now().Year, Now().Month, Now().Day);
        }

        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
        #endregion
    }
}
