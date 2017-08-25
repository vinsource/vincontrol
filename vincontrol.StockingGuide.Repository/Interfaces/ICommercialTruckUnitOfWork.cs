using System;
using System.Data.Objects;

namespace vincontrol.StockingGuide.Repository.Interfaces {
    public interface ICommercialTruckUnitOfWork
    {
        void Commit();
        IMarketTruckRepository MarketTruckRepository { get; }
        ISoldMarketTruckRepository SoldMarketTruckRepository { get; }
    }
}