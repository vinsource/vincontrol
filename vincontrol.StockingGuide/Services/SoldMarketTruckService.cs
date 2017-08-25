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
    public class SoldMarketTruckService : ISoldMarketTruckService
    {
        private ICommercialTruckUnitOfWork _commercialTruckUnitOfWork;

        public SoldMarketTruckService()
        {
            _commercialTruckUnitOfWork = new CommercialTruckUnitOfWork();
        }

        public int GetHistory(DateTime dateTime, int dealerId)
        {
            double currentMonth = GetSoldVehicleCountForMonth(dateTime, dealerId);
            var oneMonthBeforeCount = GetSoldVehicleCountForMonth(dateTime.AddMonths(-1), dealerId);
            double twoMonthBeforeCount = GetSoldVehicleCountForMonth(dateTime.AddMonths(-2), dealerId);
            return (int)Math.Ceiling((oneMonthBeforeCount + twoMonthBeforeCount + currentMonth) / 3);
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
            return _commercialTruckUnitOfWork.SoldMarketTruckRepository.GetSoldVehicleForMonth(dateTime, dealerId)
                     .GroupBy(i => new { i.Make, i.Model })
                     .Select(i => new MakeHistory { Make = i.Key.Make, Model = i.Key.Model, History = i.Select(j=>j.Vin).Distinct().Count()}).ToList();

        }

        public List<MakeHistory> GetSoldVehicleHistoryListByMake(DateTime dateTime, double longitude, double lattitude)
        {
            return _commercialTruckUnitOfWork.SoldMarketTruckRepository.GetSoldVehicleForMonthWithin100Miles(dateTime,
                 longitude, lattitude).Select(i => new { i.Make, i.Model, i.CommercialTruckDealer.Latitude, i.CommercialTruckDealer.Longitude, i.Vin }).ToList().Where(i => Math.Abs(CommonHelper.DistanceBetweenPlaces(lattitude, longitude, i.Latitude ?? 0, i.Longitude ?? 0)) <= 100)
                 .GroupBy(i => new { i.Make, i.Model })
                 .Select(i => new MakeHistory { Make = i.Key.Make, Model = i.Key.Model, History = i.Select(j=>j.Vin).Distinct().Count() }).ToList();
        }


        private double GetSoldVehicleCountForMonth(DateTime dateTime, int dealerId)
        {
            return _commercialTruckUnitOfWork.SoldMarketTruckRepository.GetSoldVehicleForMonth(dateTime, dealerId).Count();
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
