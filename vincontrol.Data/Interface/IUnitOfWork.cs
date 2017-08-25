using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Repository.Interface;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        void CommitVinControlScrapping();
        void CommitVincontrolModel();
        void CommitVinreviewModel();
        void CommitVinchatModel();
        void CommitVinmarketModel();
        void CommitVinCommercialTruckModel();
        void CommitVinSell();
       
        Repository.Interface.ICommonRepository Common { get; }
        IContactReposity Contact { get; }
        IUserRepository User { get; }
        IExtendedUserRepository ExtendedUser { get; }
        IGroupPermissionRepository GroupPermission { get; }
        IManheimAuctionRepository ManheimAuction { get; }
        IAdminRepository Admin { get; }
        IVehicleLogRepository VehicleLog { get; }
        IEmailWaitingListRepository EmailWaitingLog { get; }
        IBucketJumpWaitingListRepository BucketJump { get; }
        INotificationRepository Notification { get; }
        IVideoTrackingRepository VideoTracking { get; }

        IInventoryRepository Inventory { get; }
        IAppraisalRepository Appraisal { get; }
        IDealerRepository Dealer { get; }
        ITradeInRepository TradeIn { get; }

        ICarFaxRepository CarFax { get; }
        IManheimRepository Manheim { get; }
        IKBBRepository KBB { get; }

        Repository.Vinsocial.Interface.ICommonRepository VinsocialCommon { get; }
        IReviewRepository Review { get; }
        IFacebookRepository Facebook { get; }
        ISurveyRepository Survey { get; }
        ITemplateRepository Template { get; }
        ICustomerRepository Customer { get; }
        ITeamRepository Team { get; }

        IDepartmentRepository Department { get; }
        IYoutubeRepository Youtube { get; }

        Repository.Vinchat.Interface.ICommonRepository VinchatCommon { get; }

        Repository.VinMarket.Interface.ICommonRepository VinmarketCommon { get; }
    }
}
