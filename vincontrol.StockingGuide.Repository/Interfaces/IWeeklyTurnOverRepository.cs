using System;
using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface IWeeklyTurnOverRepository : IRepository<SGWeeklyTurnOver>
    {
        IQueryable<SGWeeklyTurnOver> GetWeeklyTurnOverForMonth(DateTime dateTime, int dealerId);
        IQueryable<SGWeeklyTurnOver> GetWeeklyTurnOverForMonthPeriod(int startMonth, int startYear, int endMonth,int endYear, int dealerId);
        IQueryable<SGWeeklyTurnOver> GetWeeklyTurnOverForWeek(DateTime dateTime, int dealerId);
    }
}
