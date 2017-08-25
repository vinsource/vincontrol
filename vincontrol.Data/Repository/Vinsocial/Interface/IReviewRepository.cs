using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface IReviewRepository 
    {
        DealerReview GetDealerReview(int dealerReviewId);
        DealerReview GetDealerReview(int dealerId, int siteId, int categoryId);
        UserReview GetUserReview(int dealerReviewId, string author, DateTime reviewDate, decimal rating);
        AdditionalDealerReview GetAdditionalDealerReview(int dealerReviewId);
        IQueryable<DealerReview> GetDealerReviews(int dealerId);
        IQueryable<DealerReview> GetDealerReviewsBySite(int siteId);
        IQueryable<SiteReview> GetDealerSiteReview(int dealerId);
        IQueryable<UserReview> GetUserReviews(int dealerId);
        IQueryable<UserReview> GetUserReviewsByUser(int userId);
        UserReview GetUserReview(int id);
        IQueryable<SiteReview> GetAllSiteReview();
        void AddDealerReview(int dealerId, int siteId, string url, decimal overallScore, string about);
        void AddAdditionalDealerReview(AdditionalDealerReview review);
        void AddDealerReview(DealerReview dealerReview);
        void AddUserReview(int dealerReviewId, string author, DateTime reviewDate, decimal rating, string comment, bool isFilterred);
        void AddUserReview(UserReview userReview);
        void AddAdditionalUserReview(int userReviewId, decimal customerService, decimal qualityOfWork,
                                     decimal friendliness,
                                     decimal overallExperience, decimal pricing, decimal buyingProcess,
                                     decimal overallFacilities, string reasonForVisit, bool recommentThisDealer,
                                     bool purchasedAVehicle);
        UserReview GetLatestUserReview(int dealerReviewId);
        IQueryable<UserReview> GetTotalRating(int userId);
        IQueryable<UserReview> GetUserReviewsByUserId(int userId);
        IQueryable<UserReview> GetRatingBreakdownByUserId(int userId);
        void UpdateReview(int userReviewId, int userId, int departmentId);
        void UpdateDealerReview(int dealerReviewId, string url);
        IQueryable<Setting> GetDealerWebsite(int dealerId);
        IQueryable<UserReview> GetAllUserReviews();
    }
}
