using System.Configuration;
using System.Data.Objects;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.Repositories;

namespace vincontrol.StockingGuide.Repository.UnitOfWorks
{
    public class CommercialTruckUnitOfWork : ICommercialTruckUnitOfWork
    {
        readonly ObjectContext _context;

        private IMarketTruckRepository _marketTruckRepository;
        private ISoldMarketTruckRepository _soldMarketTruckRepository;

        const string ConnectionStringName = "VinCommercialTruckEntities";

          public CommercialTruckUnitOfWork()
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


        public IMarketTruckRepository MarketTruckRepository
        {
            get { return _marketTruckRepository ?? (_marketTruckRepository = new MarketTruckRepository(_context)); }

        }

        public ISoldMarketTruckRepository SoldMarketTruckRepository
        {
            get { return _soldMarketTruckRepository ?? (_soldMarketTruckRepository = new SoldMarketTruckRepository(_context)); }

        }
    }
}
