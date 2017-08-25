using System;
using System.Data.Objects;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Repository.Interfaces;
using System.Linq;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class WeeklyTurnOverRepository : SqlRepository<SGWeeklyTurnOver>, IWeeklyTurnOverRepository
    {
        public WeeklyTurnOverRepository(ObjectContext context)
            : base(context)
        {

        }

        public IQueryable<SGWeeklyTurnOver> GetWeeklyTurnOverForMonth(DateTime dateTime, int dealerId)
        {
            return _objectSet.Where(i => i.Month == dateTime.Month && i.Year == dateTime.Year && i.DealerId == dealerId);
        }

        public IQueryable<SGWeeklyTurnOver> GetWeeklyTurnOverForWeek(DateTime dateTime, int dealerId)
        {
            var week = dateTime.GetWeekNumber();
            return _objectSet.Where(i => i.Month == dateTime.Month && i.Year == dateTime.Year && i.DealerId == dealerId && i.Week==week);
        }

        public IQueryable<SGWeeklyTurnOver> GetWeeklyTurnOverForMonthPeriod(int startMonth,int startYear, int endMonth, int endYear, int dealerId)
        {
            if (startYear == endYear)
            {
                return _objectSet.Where(i => i.Month >= startMonth && i.Month<=endMonth && i.Year == endYear && i.DealerId == dealerId);
            }

            return _objectSet.Where(i => ((i.Month >= startMonth && i.Year == startYear) || (i.Month <= endMonth && i.Year == endYear)) && i.DealerId == dealerId);
        }
    }
}
