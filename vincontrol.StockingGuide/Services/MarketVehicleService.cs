using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class MarketVehicleService: IMarketVehicleService
    {
        private IVinmarketUnitOfWork _vinmarketUnitOfWork;

        public MarketVehicleService()
        {
            _vinmarketUnitOfWork = new VinmarketUnitOfWork();    
        }

        //public List<GetVehiclesWithin100Miles_Result> GetCarsWithin100Miles(decimal longitude, decimal latitude)
        //{
        //    return _vinmarketUnitOfWork.MarketVehicleRepository.GetCarsWithin100Miles(longitude, latitude);
        //    //LocationHelper.DistanceBetweenPlaces();
        //}

        //public int GetNumberOfCarOnMarketWithin100Miles(double longitude, double lattitude, string make, string model)
        //{
        //    return _vinmarketUnitOfWork.MarketVehicleRepository.GetNumberOfCarWithin100MilesOnMarket(longitude, lattitude, make,model);
        //}

        //public MarketInfo GetNumberOfCarOnMarketWithin100Miles(double longitude, double lattitude, string make, string model, string trim)
        //{
        //    return _vinmarketUnitOfWork.MarketVehicleRepository.GetNumberOfCarWithin100MilesOnMarket(longitude, lattitude, make, model,trim);
        //}

        public List<MakeModelQuantityPrice> GetCarOnMarketWithin100MilesByMakeModel(double longitude, double lattitude)
        {
            return
                _vinmarketUnitOfWork.MarketVehicleRepository.GetCarsWithin100MilesOnMarket(longitude, lattitude).GroupBy(i => new {i.Make, i.Model}).Select(i=>new MakeModelQuantityPrice{Make =i.Key.Make,Model= i.Key.Model,Count= i.Count(), Age = i.Average(j=>(EntityFunctions.DiffDays(j.DateAdded,j.LastUpdated))), MaxPrice = i.Max(j=>j.CurrentPrice), MinPrice = i.Min(j=>j.CurrentPrice), AveragePrice = (int)i.Average(j=>j.CurrentPrice)}).ToList();
        }
    }
}
