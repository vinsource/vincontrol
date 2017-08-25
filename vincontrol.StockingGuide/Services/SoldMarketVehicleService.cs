using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class SoldMarketVehicleService : ISoldMarketVehicleService
    {
        private IVinmarketUnitOfWork _vinmarketUnitOfWork;
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;
        public SoldMarketVehicleService()
        {
            _vinmarketUnitOfWork = new VinmarketUnitOfWork();
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

     

        public int GetHistory(DateTime dateTime, int dealerId)
        {
            var currentMonth = GetSoldVehicleCountForWeek(dateTime, dealerId);
            return currentMonth;
        }

        public List<List<MakeHistory>> GetHistoryList(DateTime dateTime, int dealerId)
        {
            return new List<List<MakeHistory>>()
            {
               GetSoldVehicleHistoryListByMakeForDealer(dateTime, dealerId),
               GetSoldVehicleHistoryListByMakeForDealer(dateTime.AddMonths(-1), dealerId),
               GetSoldVehicleHistoryListByMakeForDealer(dateTime.AddMonths(-2), dealerId)
               
            };
        }

 

        private List<MakeHistory> GetSoldVehicleHistoryListByMakeForDealer(DateTime dateTime, int dealerId)
        {
            return _vincontrolUnitOfWork.SoldInventoryRepository.GetSoldVehicleForMonth(dateTime, dealerId)
                     .GroupBy(i => new { i.Vehicle.Make, i.Vehicle.Model })
                     .Select(i => new MakeHistory { Make = i.Key.Make, Model = i.Key.Model, History = i.Select(j=>j.Vehicle.Vin).Distinct().Count()}).ToList();

        }

        public List<MakeHistory> GetSoldVehicleHistoryListByMake(DateTime dateTime, double longitude, double lattitude)
        {
            return _vinmarketUnitOfWork.SoldMarketVehicleRepository.GetSoldVehicleForMonthWithin100Miles(dateTime,
                 longitude, lattitude).Select(i=>new{i.Make,i.Model,i.Latitude,i.Longitude, i.Vin}).ToList().Where(i=>Math.Abs(CommonHelper.DistanceBetweenPlaces(lattitude,longitude,i.Latitude??0,i.Longitude??0))<=100)
                 .GroupBy(i => new { i.Make, i.Model })
                 .Select(i => new MakeHistory { Make = i.Key.Make, Model = i.Key.Model, History = i.Select(j=>j.Vin).Distinct().Count() }).ToList();
        }



        private int GetSoldVehicleCountForWeek(DateTime dateTime, int dealerId)
        {
            return _vincontrolUnitOfWork.SoldInventoryRepository.GetSoldVehicleForWeek(dateTime, dealerId).Count();
        }


        public List<List<MakeHistory>> GetHistoryList(DateTime dateTime, double longitude, double lattitude)
        {
            return new List<List<MakeHistory>>()
            {
               GetSoldVehicleHistoryListByMake(dateTime,longitude,lattitude),
               GetSoldVehicleHistoryListByMake(dateTime.AddMonths(-1),longitude,lattitude),
               GetSoldVehicleHistoryListByMake(dateTime.AddMonths(-2),longitude, lattitude),
               
            };
        }
    }
}
