using System;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Model.Truck;
using vincontrol.Data.Repository.Implementation;
using vincontrol.Data.Repository.Interface;
using vincontrol.Data.Repository.Vinsocial.Implementation;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        #region Private Varaiables

        private VinMarketEntities _vinmarketEntities;
        private VincontrolEntities _vincontrolEntities;
        private VinReviewEntities _vinreviewEntities;
        private VinsellEntities _vinsellEntities;
        private VinChatEntities _vinchatEntities;
        private VinCommercialTruckEntities _vinCommercialTruckEntities;
      
        
        private Implementation.CommonRepository _common;
        private IContactReposity _contact;
        private UserRepository _user;
        private ExtendedUserRepository _extendedUser;
        private GroupPermissionRepository _groupPermission;
        private ManheimAuctionRepository _manheimAuction;
        private AdminRepository _admin;
        private VehicleLogRepository _vehicleLog;
        private EmailWaitingListRepository _emailWaitingLog;
        private BucketJumpWaitingListRepository _bucketJump;
        private NotificationRepository _notification;
        private VideoTrackingRepository _videoTracking;

        private InventoryRepository _inventory;
        private AppraisalRepository _appraisal;
        private DealerRepository _dealer;
        private TradeInRepository _tradeIn;
        

        private CarFaxRepository _carfax;
        private ManheimRepository _manheim;
        private KBBRepository _kbb;
        
        private Vinsocial.Implementation.CommonRepository _vinsocialCommon;
        private ReviewRepository _review;
        private FacebookRepository _facebook;
        private SurveyRepository _survey;
        private TemplateRepository _template;
        private CustomerRepository _customer;
        private TeamRepository _team;
        private DepartmentRepository _department;
        private YoutubeRepository _youtube;

        private Vinchat.Implementation.CommonRepository _vinchatCommon;

        private VinMarket.Implementation.CommonRepository _vinmarketCommon;
        #endregion

        public SqlUnitOfWork()
        {
            _vinmarketEntities = new VinMarketEntities();
            _vinmarketEntities.ContextOptions.LazyLoadingEnabled = true;
            _vinmarketEntities.CommandTimeout = 3 * 60; // in seconds

            _vincontrolEntities = new VincontrolEntities();
            _vincontrolEntities.ContextOptions.LazyLoadingEnabled = true;
            _vincontrolEntities.CommandTimeout = 3 * 60;

            _vinreviewEntities = new VinReviewEntities();
            _vinreviewEntities.ContextOptions.LazyLoadingEnabled = true;
            _vinreviewEntities.CommandTimeout = 3 * 60;

            _vinsellEntities = new VinsellEntities();
          

            _vinchatEntities = new VinChatEntities();
            _vinchatEntities.ContextOptions.LazyLoadingEnabled = true;
            _vinchatEntities.CommandTimeout = 3 * 60;

            _vinCommercialTruckEntities = new VinCommercialTruckEntities();
            _vinCommercialTruckEntities.ContextOptions.LazyLoadingEnabled = true;
            _vinCommercialTruckEntities.CommandTimeout = 3 * 60;

           
        }

        #region IUnitOfWork Members

        #region New Vincontrol

        public void CommitVincontrolModel()
        {
            _vincontrolEntities.SaveChanges();
        }

      

        public INotificationRepository Notification
        {
            get { return _notification ?? (_notification = new NotificationRepository(_vincontrolEntities)); }
        }

        public void CommitVinSell()
        {
            _vinsellEntities.SaveChanges();
        }

        public Interface.ICommonRepository Common
        {
            get { return _common ?? (_common = new Implementation.CommonRepository(_vincontrolEntities)); }
        }
        
        public IInventoryRepository Inventory
        {
            get { return _inventory ?? (_inventory = new InventoryRepository(_vincontrolEntities)); }
        }
        
        public IAppraisalRepository Appraisal
        {
            get { return _appraisal ?? (_appraisal = new AppraisalRepository(_vincontrolEntities)); }
        }

        public IDealerRepository Dealer
        {
            get { return _dealer ?? (_dealer = new DealerRepository(_vincontrolEntities)); }
        }

        public ITradeInRepository TradeIn
        {
            get { return _tradeIn ?? (_tradeIn = new TradeInRepository(_vincontrolEntities)); }
        }

        public IContactReposity Contact
        {
            get { return _contact ?? (_contact = new ContactReposity(_vincontrolEntities)); }
        }

        public IUserRepository User
        {
            get { return _user ?? (_user = new UserRepository(_vincontrolEntities)); }
        }

        public IExtendedUserRepository ExtendedUser
        {
            get { return _extendedUser ?? (_extendedUser = new ExtendedUserRepository(_vincontrolEntities)); }
        }

        public IGroupPermissionRepository GroupPermission
        {
            get { return _groupPermission ?? (_groupPermission = new GroupPermissionRepository(_vincontrolEntities)); }
        }

        public IManheimAuctionRepository ManheimAuction
        {
            get { return _manheimAuction ?? (_manheimAuction = new ManheimAuctionRepository(_vinsellEntities)); }
        }

        public IAdminRepository Admin
        {
            get { return _admin ?? (_admin = new AdminRepository(_vincontrolEntities)); }
        }

        public IVehicleLogRepository VehicleLog
        {
            get { return _vehicleLog ?? (_vehicleLog = new VehicleLogRepository(_vincontrolEntities)); }
        }
        public IEmailWaitingListRepository EmailWaitingLog
        {
            get { return _emailWaitingLog ?? (_emailWaitingLog = new EmailWaitingListRepository(_vincontrolEntities)); }
        }

        public IBucketJumpWaitingListRepository BucketJump
        {
            get { return _bucketJump ?? (_bucketJump = new BucketJumpWaitingListRepository(_vincontrolEntities)); }
        }

        public IVideoTrackingRepository VideoTracking
        {
            get { return _videoTracking ?? (_videoTracking = new VideoTrackingRepository(_vincontrolEntities)); }
        }
       
        #endregion
        
        #region Third Party Company

        public ICarFaxRepository CarFax
        {
            get { return _carfax ?? (_carfax = new CarFaxRepository(_vincontrolEntities,_vinsellEntities)); }
        }
        
        public IManheimRepository Manheim
        {
            get { return _manheim ?? (_manheim = new ManheimRepository(_vincontrolEntities)); }
        }

        public IKBBRepository KBB
        {
            get { return _kbb ?? (_kbb = new KBBRepository(_vincontrolEntities)); }
        }

        #endregion

        #region VINSocial

        public void CommitVinreviewModel()
        {
            _vinreviewEntities.SaveChanges();
        }

        public Vinsocial.Interface.ICommonRepository VinsocialCommon
        {
            get { return _vinsocialCommon ?? (_vinsocialCommon = new Vinsocial.Implementation.CommonRepository(_vinreviewEntities)); }
        }

        public IReviewRepository Review
        {
            get { return _review ?? (_review = new ReviewRepository(_vinreviewEntities)); }
        }

        public IFacebookRepository Facebook
        {
            get { return _facebook ?? (_facebook = new FacebookRepository(_vinreviewEntities,_vincontrolEntities)); }
        }

        public ISurveyRepository Survey
        {
            get { return _survey ?? (_survey = new SurveyRepository(_vinreviewEntities)); }
        }

        public ITemplateRepository Template
        {
            get { return _template ?? (_template = new TemplateRepository(_vinreviewEntities)); }
        }

        public ICustomerRepository Customer
        {
            get { return _customer ?? (_customer = new CustomerRepository(_vinreviewEntities)); }
        }

        public ITeamRepository Team
        {
            get { return _team ?? (_team = new TeamRepository(_vincontrolEntities)); }
        }

        public IDepartmentRepository Department
        {
            get { return _department ?? (_department = new DepartmentRepository(_vincontrolEntities)); }
        }

        public IYoutubeRepository Youtube
        {
            get { return _youtube ?? (_youtube = new YoutubeRepository(_vinreviewEntities)); }
        }

        #endregion

        #region VINChat
        public void CommitVinchatModel()
        {
            _vinchatEntities.SaveChanges();
        }

        public Vinchat.Interface.ICommonRepository VinchatCommon
        {
            get { return _vinchatCommon ?? (_vinchatCommon = new Vinchat.Implementation.CommonRepository(_vinchatEntities)); }
        }
        #endregion

        #region VINMarket
        public void CommitVinmarketModel()
        {
            _vinmarketEntities.SaveChanges();
        }

        public VinMarket.Interface.ICommonRepository VinmarketCommon
        {
            get { return _vinmarketCommon ?? (_vinmarketCommon = new VinMarket.Implementation.CommonRepository(_vinmarketEntities, _vinCommercialTruckEntities)); }
        }
        #endregion

        #region VINScrapping
        public void CommitVinControlScrapping()
        {
            _vinmarketEntities.SaveChanges();
        }
        #endregion

        #region CommercialTruck
        public void CommitVinCommercialTruckModel()
        {
            _vinCommercialTruckEntities.SaveChanges();
        }
        #endregion

        #region CarMax
      
        #endregion

        #endregion

        #region IDisposable Members

        public void DisposeVinControlScrapping()
        {
            if (_vinmarketEntities != null)
            {
                _vinmarketEntities.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (_vinmarketEntities != null)
            {
                _vinmarketEntities.Dispose();
            }

            if (_vincontrolEntities != null)
            {
                _vincontrolEntities.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
