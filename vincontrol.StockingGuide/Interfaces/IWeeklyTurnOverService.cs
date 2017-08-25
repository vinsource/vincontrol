using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IWeeklyTurnOverService
    {
        List<SGWeeklyTurnOver> GetTurnOverListByMonth(DateTime currentDate, int dealerId);
        void UpdateWeeklyTurnOver(SGWeeklyTurnOver sgWeeklyTurnOver);
        void AddWeeklyTurnOver(SGWeeklyTurnOver sgWeeklyTurnOver);
        SGWeeklyTurnOver GetCurrentWeeklyTurnOver(DateTime dateTime, int dealerId);
        void AddWeeklyTurnOvers(List<SGWeeklyTurnOver> list);
        void SaveChanges();
        IQueryable<SGWeeklyTurnOver> GetAllTurnOversForDealer(int dealerId);
    }
}