using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Helper;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class MockSoldMarketTruckService : ISoldMarketTruckService
    {
      //private IVinmarketUnitOfWork _vinmarketUnitOfWork;
      public MockSoldMarketTruckService()
        {
            //_vinmarketUnitOfWork = new VinmarketUnitOfWork();
        }

     

        public int GetHistory(DateTime dateTime, int dealerId)
        {
            return 0;
        }

        public List<List<MakeHistory>> GetHistoryList(DateTime dateTime, int dealerId)
        {
            return new List<List<MakeHistory>>()
            {
             new List<MakeHistory>(),
                          new List<MakeHistory>(),
                         new List<MakeHistory>()
               //GetSoldVehicleHistoryListByMakeForDealer(dateTime.AddMonths(-3), dealerId)
            };
        }

        //private List<MakeModelTrimHistory> GetSoldVehicleHistoryListByMakeModelTrim(DateTime dateTime)
        //{
        //    return _vinmarketUnitOfWork.SoldMarketVehicleRepository.GetSoldVehicleForMonth(dateTime)
        //             .GroupBy(i => new { i.Make, i.Model, i.Trim })
        //             .Select(i => new MakeModelTrimHistory() { Make = i.Key.Make, Model = i.Key.Model, Trim = i.Key.Trim, History = i.Count() }).ToList();

        //}

        private List<MakeHistory> GetSoldVehicleHistoryListByMakeForDealer(DateTime dateTime, int dealerId)
        {
            //return _vinmarketUnitOfWork.SoldMarketTruckRepository.GetSoldVehicleForMonth(dateTime, dealerId)
            //         .GroupBy(i => new { i.Make, i.Model })
            //         .Select(i => new MakeHistory { Make = i.Key.Make, Model = i.Key.Model, History = i.Select(j=>j.Vin).Distinct().Count()}).ToList();
            return new List<MakeHistory>();
        }

        public List<MakeHistory> GetSoldVehicleHistoryListByMake(DateTime dateTime, double longitude, double lattitude)
        {
            //return _vinmarketUnitOfWork.SoldMarketTruckRepository.GetSoldVehicleForMonthWithin100Miles(dateTime,
            //     longitude, lattitude).Select(i => new { i.Make, i.Model, i.CommercialTruckDealer.Latitude, i.CommercialTruckDealer.Longitude, i.Vin }).ToList().Where(i => Math.Abs(CommonHelper.DistanceBetweenPlaces(lattitude, longitude, i.Latitude ?? 0, i.Longitude ?? 0)) <= 100)
            //     .GroupBy(i => new { i.Make, i.Model })
            //     .Select(i => new MakeHistory { Make = i.Key.Make, Model = i.Key.Model, History = i.Select(j=>j.Vin).Distinct().Count() }).ToList();
            return new List<MakeHistory>();
        }

        //public string GetSoldVehicleHistoryListByMakeQueryContent(DateTime dateTime, double longitude, double lattitude)
        //{
        //    return ((System.Data.Objects.ObjectQuery) GetSoldVehicleHistoryListByMakeQuery(dateTime, longitude, lattitude))
        //        .ToTraceString();
        //}

        //private IQueryable<MakeHistory> GetSoldVehicleHistoryListByMakeQuery(DateTime dateTime, double longitude, double lattitude)
        //{
        //    return ;
        //}


        private double GetSoldVehicleCountForMonth(DateTime dateTime, int dealerId)
        {
            //return _vinmarketUnitOfWork.SoldMarketTruckRepository.GetSoldVehicleForMonth(dateTime, dealerId).Count();
            return 0;
        }

        //public List<List<MakeModelTrimHistory>> GetHistoryListByTrim(DateTime dateTime)
        //{
        //    return new List<List<MakeModelTrimHistory>>()
        //    {
        //       GetSoldVehicleHistoryListByMakeModelTrim(dateTime.AddMonths(-1)),
        //       GetSoldVehicleHistoryListByMakeModelTrim(dateTime.AddMonths(-2)),
        //       GetSoldVehicleHistoryListByMakeModelTrim(dateTime.AddMonths(-3))
        //    };
        //}

        public List<List<MakeHistory>> GetHistoryList(DateTime dateTime, double longitude, double lattitude)
        {
            return new List<List<MakeHistory>>()
            {
               new List<MakeHistory>(),
                          new List<MakeHistory>(),
                         new List<MakeHistory>()
               //GetSoldVehicleHistoryListByMake(dateTime.AddMonths(-3),longitude, lattitude)
            };
        }
    }
}
