using vincontrol.StockingGuide.Repository.Repositories;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface IVinsellUnitOfWork
    {
        void Commit();
        IManheimVehicleRepository ManheimVehicleRepository { get; }
        IManheimAuctionRepository ManheimAuctionRepository { get; }
        IStateRepository StateRepository{get;}
    }
}