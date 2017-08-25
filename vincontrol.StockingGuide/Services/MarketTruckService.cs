using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class MarketTruckService : IMarketTruckService
    {
        private ICommercialTruckUnitOfWork _commercialTruckUnitOfWork;

        public MarketTruckService()
        {
            _commercialTruckUnitOfWork = new CommercialTruckUnitOfWork();    
        }
        public List<MakeModelQuantityPrice> GetCarOnMarketWithin100MilesByMakeModel(double longitude, double latitude)
        {
            return
                _commercialTruckUnitOfWork.MarketTruckRepository.GetCarsWithin100MilesOnMarket(longitude, latitude).GroupBy(i => new { i.Make, i.Model }).Select(i => new MakeModelQuantityPrice { Make = i.Key.Make, Model = i.Key.Model, Count = i.Count(), Age = i.Average(j => (EntityFunctions.DiffDays(j.DateStamp, DateTime.Now))), MaxPrice = i.Max(j => j.Price), MinPrice = i.Min(j => j.Price), AveragePrice = (int)i.Average(j => j.Price) }).ToList();
   
        }
    }
}
