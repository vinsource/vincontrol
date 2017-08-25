using System.Configuration;
using System.Data.Objects;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.Repositories;

namespace vincontrol.StockingGuide.Repository.UnitOfWorks
{
    public class VinsellUnitOfWork : IVinsellUnitOfWork
    {
        readonly ObjectContext _context;
        private IManheimVehicleRepository _manheimVehicleRepository;
        private IManheimAuctionRepository _manheimAuctionRepository;
        private IStateRepository _stateRepository;

        const string ConnectionStringName = "VinsellEntities";

        public VinsellUnitOfWork()
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings[ConnectionStringName]
                    .ConnectionString;

            _context = new ObjectContext(connectionString);
            _context.CommandTimeout = 180;
            _context.ContextOptions.LazyLoadingEnabled = true;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IManheimVehicleRepository ManheimVehicleRepository
        {
            get { return _manheimVehicleRepository ?? (_manheimVehicleRepository = new ManheimVehicleRepository(_context)); }
        }


        public IManheimAuctionRepository ManheimAuctionRepository
        {
            get { return _manheimAuctionRepository ?? (_manheimAuctionRepository = new ManheimAuctionRepository(_context)); }
        }

        public IStateRepository StateRepository
        {
            get { return _stateRepository ?? (_stateRepository = new StateRepository(_context)); }
        }
    }
}
