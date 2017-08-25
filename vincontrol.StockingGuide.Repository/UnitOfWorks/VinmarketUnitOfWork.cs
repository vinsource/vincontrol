using System.Configuration;
using System.Data.Objects;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.Repositories;

namespace vincontrol.StockingGuide.Repository.UnitOfWorks
{
    public class VinmarketUnitOfWork : IVinmarketUnitOfWork
    {
        readonly ObjectContext _context;
        private ISoldMarketVehicleRepository _soldMarketVehicleRepository;
        private IMarketVehicleRepository _marketVehicleRepository;
       

        const string ConnectionStringName = "VinMarketEntities";

        public VinmarketUnitOfWork()
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings[ConnectionStringName]
                    .ConnectionString;

            _context = new ObjectContext(connectionString);
            _context.CommandTimeout = 360;
            _context.ContextOptions.LazyLoadingEnabled = true;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public ISoldMarketVehicleRepository SoldMarketVehicleRepository
        {
            get { return _soldMarketVehicleRepository ?? (_soldMarketVehicleRepository = new SoldMarketVehicleRepository(_context)); }
        }

        public IMarketVehicleRepository MarketVehicleRepository
        {
            get { return _marketVehicleRepository ?? (_marketVehicleRepository = new MarketVehicleRepository(_context)); }
        }

    
    }
}
