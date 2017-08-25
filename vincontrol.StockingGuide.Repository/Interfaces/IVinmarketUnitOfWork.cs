using System;
using System.Data.Objects;

namespace vincontrol.StockingGuide.Repository.Interfaces {
    public interface IVinmarketUnitOfWork
    {
        void Commit();
        ISoldMarketVehicleRepository SoldMarketVehicleRepository { get; }
        IMarketVehicleRepository MarketVehicleRepository { get; }
     
    }
}