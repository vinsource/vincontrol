using System.Configuration;
using System.Data.Objects;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.Repositories;

namespace vincontrol.StockingGuide.Repository.UnitOfWorks {

    public class VincontrolUnitOfWork : IVincontrolUnitOfWork {
        IWeeklyTurnOverRepository _weeklyTurnOverRepository;
        IInventoryRepository _inventoryRepository; ISoldInventoryRepository _soldinventoryRepository;
        IAppraisalRepository _appraisalRepository;
        private IVehicleRepository _vehicleRepository;

        const string ConnectionStringName = "VincontrolEntities";
        readonly ObjectContext _context;
     
        private IModelRepository _modelRepository;
        private IDealerBrandRepository _dealerBrandRepository;
        private IDealerBrandSelectionRepository _dealerBrandSelectionRepository;
        private ISegmentRepository _segmentRepository;
        private IInventorySegmentDetailRepository _inventorySegmentDetailRepository;
        private IDealerSegmentRepository _dealerSegmentRepository;
        private IMarketSegmentDetailRepository _marketSegmentDetailRepository;
        private IDealerRepository _dealerRepository;
        private ISettingRepository _settingRepository;
        private IInventoryStatisticsRepository _inventoryStatisticsRepository;
        private ITrimRepository _trimRepository;
        private IDataDealerExportRepository _exportRepository;
        

        public IInventoryStatisticsRepository InventoryStatisticsRepository
        {
            get { return _inventoryStatisticsRepository ?? (_inventoryStatisticsRepository = new InventoryStatisticsRepository(_context)); }

        }

        public ITrimRepository TrimRepository
        {
            get { return _trimRepository ?? (_trimRepository = new TrimRepository(_context)); }

        }


        

        public IWeeklyTurnOverRepository WeeklyTurnOverRepository
        {
            get { return _weeklyTurnOverRepository ?? (_weeklyTurnOverRepository = new WeeklyTurnOverRepository(_context)); }
        }

        public IInventoryRepository InventoryRepository
        {
            get { return _inventoryRepository ?? (_inventoryRepository = new InventoryRepository(_context)); }
        }

        public ISoldInventoryRepository SoldInventoryRepository
        {
            get { return _soldinventoryRepository ?? (_soldinventoryRepository = new SoldInventoryRepository(_context)); }
        }

        public IAppraisalRepository AppraisalRepository
        {
            get { return _appraisalRepository ?? (_appraisalRepository = new AppraisalRepository(_context)); }
        }

        public IVehicleRepository VehicleRepository
        {
            get { return _vehicleRepository ?? (_vehicleRepository = new VehicleRepository(_context)); }
        }
        
        public IModelRepository ModelRepository
        {
            get { return _modelRepository ?? (_modelRepository = new ModelRepository(_context)); }
        }

        public ISegmentRepository SegmentRepository
        {
            get { return _segmentRepository ?? (_segmentRepository = new SegmentRepository(_context)); }
            
        }

        public IInventorySegmentDetailRepository InventorySegmentDetailRepository
        {
            get { return _inventorySegmentDetailRepository ?? (_inventorySegmentDetailRepository = new InventorySegmentDetailRepository(_context)); }

        }

        public IDealerSegmentRepository DealerSegmentRepository
        {
            get { return _dealerSegmentRepository ?? (_dealerSegmentRepository = new DealerSegmentRepository(_context)); }

        }
        public IMarketSegmentDetailRepository MarketSegmentDetailRepository
        {
            get { return _marketSegmentDetailRepository ?? (_marketSegmentDetailRepository = new MarketSegmentDetailRepository(_context)); }

        }

        public IDealerRepository DealerRepository
        {
            get { return _dealerRepository ?? (_dealerRepository = new DealerRepository(_context)); }

        }

        public ISettingRepository SettingRepository
        {
            get { return _settingRepository ?? (_settingRepository = new SettingRepository(_context)); }

        }


        public IDataDealerExportRepository ExportRepository
        {
            get { return _exportRepository ?? (_exportRepository = new DataDealerExportRepository(_context)); }

        }

        public IDealerBrandRepository DealerBrandRepository { get { return _dealerBrandRepository ?? (_dealerBrandRepository = new DealerBrandRepository(_context)); } }

        public IDealerBrandSelectionRepository DealerBrandSelectionRepository { get { return _dealerBrandSelectionRepository ?? (_dealerBrandSelectionRepository = new DealerBrandSelectionRepository(_context)); } }

        public VincontrolUnitOfWork() {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings[ConnectionStringName]
                    .ConnectionString;

            _context = new ObjectContext(connectionString);
            _context.ContextOptions.LazyLoadingEnabled = true;
        }

        public void Commit() {
            _context.SaveChanges();
        }
    }
}