using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class WeeklyTurnOverService : IWeeklyTurnOverService
    {
        private readonly IVincontrolUnitOfWork _vincontrolUnitOfWork;
        public WeeklyTurnOverService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public List<SGWeeklyTurnOver> GetTurnOverListByMonth(DateTime currentDate, int dealerId)
        {
            var pastDate = DateTime.Now.AddMonths(-12);
            return _vincontrolUnitOfWork.WeeklyTurnOverRepository.GetWeeklyTurnOverForMonthPeriod(pastDate.Month, pastDate.Year, currentDate.Month, currentDate.Year, dealerId).ToList();
        }

        public SGWeeklyTurnOver GetCurrentWeeklyTurnOver(DateTime dateTime, int dealerId)
        {
            return _vincontrolUnitOfWork.WeeklyTurnOverRepository.GetWeeklyTurnOverForWeek(dateTime, dealerId).FirstOrDefault();
        }

        public void AddWeeklyTurnOvers(List<SGWeeklyTurnOver> list)
        {
            foreach (var item in list)
            {
                _vincontrolUnitOfWork.WeeklyTurnOverRepository.Add(item);
            }
            _vincontrolUnitOfWork.Commit();
        }

        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }

        public IQueryable<SGWeeklyTurnOver> GetAllTurnOversForDealer(int dealerId)
        {
           return _vincontrolUnitOfWork.WeeklyTurnOverRepository.Find(i => i.DealerId == dealerId);
        }

        public void UpdateWeeklyTurnOver(SGWeeklyTurnOver sgWeeklyTurnOver)
        {
            _vincontrolUnitOfWork.Commit();
        }

        public void AddWeeklyTurnOver(SGWeeklyTurnOver sgWeeklyTurnOver)
        {
            _vincontrolUnitOfWork.WeeklyTurnOverRepository.Add(sgWeeklyTurnOver);
            _vincontrolUnitOfWork.Commit();
        }
    }
}
