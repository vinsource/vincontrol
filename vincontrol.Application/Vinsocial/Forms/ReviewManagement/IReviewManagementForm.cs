using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;

namespace vincontrol.Application.Vinsocial.Forms.ReviewManagement
{
    public interface IReviewManagementForm
    {
        List<UserReviewViewModel> GetUserReviews(int dealerId);
        List<UserReviewViewModel> GetNewUserReviews(int dealerId);
        List<UserReviewViewModel> GetAssignedUserReviews(int dealerId);
        List<UserReviewViewModel> GetGoodUserReviews(int dealerId);
        List<DealerReviewViewModel> GetListThirdParty(int dealerId) ;
        List<SiteReviewViewModel> GetAllSiteReview();
        List<UserReviewViewModel> GetUserReviewsByUserId(int userId);
        UserReviewViewModel GetTotalRatingByUserId(int userId);
        List<UserRatingBreakdownViewModel> GetRatingBreakdownByUserId(int userId);
        List<UserRatingBreakdownViewModel> GetRatingBreakdownByDealerId(int dealerId);
        void UpdateReview(int userReviewId, int userId, int departmentId);
        void UpdateDealerReview(int dealerReviewId, string url );
        void AddUserReview(UserReviewViewModel model);
        void AddNewDealerReviews(DealerReviewViewModel dealerReview);
        List<UserReviewViewModel> GetReviewsByTime(int dealerId);
        List<DealerUrlViewModel> GetDealerWebsite(int dealerId);
        List<DealerReviewViewModel> GetDealerSiteReviews(int dealerId);
        void UpdateReviews(List<UserReviewViewModel> model);
        bool CheckExistingUserReview(int dealerReviewId, string author, DateTime reviewDate, decimal rating);
    }
}
