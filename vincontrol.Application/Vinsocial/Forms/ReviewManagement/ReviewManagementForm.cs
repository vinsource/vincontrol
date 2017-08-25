using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using vincontrol.Application.Forms;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Vinsocial.Forms.ReviewManagement
{
    public class ReviewManagementForm : BaseForm, IReviewManagementForm
    {
        private IAccountManagementForm _accountManagementForm;
        #region Constructors
        public ReviewManagementForm() : this(new SqlUnitOfWork())
        {
            _accountManagementForm = new AccountManagementForm();
        }

        public ReviewManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IReviewManagementForm' Members

        public List<UserReviewViewModel> GetUserReviews(int dealerId)
        {
            var userReviews = UnitOfWork.Review.GetUserReviews(dealerId);
            return userReviews.Any() ? userReviews.AsEnumerable().Select(i => new UserReviewViewModel(i)).ToList() : new List<UserReviewViewModel>();
        }

        public List<UserReviewViewModel> GetNewUserReviews(int dealerId)
        {
            var userIds = _accountManagementForm.GetUserIds(dealerId);
            var userReviews = UnitOfWork.Review.GetUserReviews(dealerId).Where(i => !i.UserId.HasValue || (i.UserId.HasValue && !userIds.Contains(i.UserId.Value)));
            return userReviews.Any() ? userReviews.AsEnumerable().Select(i => new UserReviewViewModel(i)).ToList() : new List<UserReviewViewModel>();
        }

        public List<UserReviewViewModel> GetAssignedUserReviews(int dealerId)
        {
            var userIds = _accountManagementForm.GetUserIds(dealerId);
            var userReviews = UnitOfWork.Review.GetUserReviews(dealerId).Where(i => i.UserId.HasValue && userIds.Contains(i.UserId.Value));
            return userReviews.Any() ? userReviews.AsEnumerable().Select(i => new UserReviewViewModel(i)).ToList() : new List<UserReviewViewModel>();
        }

        public List<UserReviewViewModel> GetGoodUserReviews(int dealerId)
        {
            var userReviews = UnitOfWork.Review.GetUserReviews(dealerId).Where(x => x.Rating > 4);
            return userReviews.Any() ? userReviews.AsEnumerable().Select(i => new UserReviewViewModel(i)).ToList() : new List<UserReviewViewModel>();
        }

        public List<DealerReviewViewModel> GetListThirdParty(int dealerId)
        {
            var dealerReview = UnitOfWork.Review.GetDealerReviews(dealerId);
            return dealerReview.Any()
                       ? dealerReview.AsEnumerable().Select(i => new DealerReviewViewModel(i)).ToList()
                       : new List<DealerReviewViewModel>();
        }

        public List<SiteReviewViewModel> GetAllSiteReview()
        {
            var siteReviews = UnitOfWork.Review.GetAllSiteReview();
            return siteReviews.Any() ? siteReviews.AsEnumerable().Select(i => new SiteReviewViewModel(i)).ToList() : new List<SiteReviewViewModel>();
        }

        public List<UserReviewViewModel> GetUserReviewsByUserId(int userId)
        {
            var userReviews = UnitOfWork.Review.GetUserReviewsByUserId(userId);
            return userReviews.Any() ? userReviews.ToList()
                                       .OrderByDescending(x => x.ReviewDate)
                                       .ThenByDescending(x => x.Rating)
                                       .ThenBy(x => x.Author)
                                       .ThenBy(x => x.Comment).Select(i => new UserReviewViewModel(i)).ToList()
                                     : new List<UserReviewViewModel>();
        }

        public UserReviewViewModel GetTotalRatingByUserId(int userId)
        {
            var userReviews = UnitOfWork.Review.GetTotalRating(userId);
            var currentUser = _accountManagementForm.GetUser(userId);
            return
                userReviews.GroupBy(i => i.UserId)
                           .Select(i => new UserReviewViewModel() { UserId = userId, Rating = i.Average(j => j.Rating), UserFullName = currentUser.Name })
                           .FirstOrDefault();
        }

        public List<UserRatingBreakdownViewModel> GetRatingBreakdownByUserId(int userId)
        {
            var userReviews = UnitOfWork.Review.GetRatingBreakdownByUserId(userId);
            return userReviews.Where(i => i.UserId == userId)
                              .GroupBy(i => i.DealerReview.SiteId)
                              .Select(
                                  i =>
                                  new UserRatingBreakdownViewModel()
                                      {
                                          UserId = i.Select(j => j.UserId).FirstOrDefault() ?? 0,
                                          SiteName = i.Select(j => j.DealerReview.SiteReview.Name).FirstOrDefault(),
                                          AverageRating = i.Average(j => j.Rating),
                                          Today = i.Count(j => EntityFunctions.TruncateTime(j.ReviewDate) == Today),
                                          Total = i.Count(),
                                          IsFitlerd = i.Select(j => j.IsFilterred).FirstOrDefault()
                                      })
                              .OrderByDescending(i => i.AverageRating).ToList();
        }

        public List<UserRatingBreakdownViewModel> GetRatingBreakdownByDealerId(int dealerId)
        {
            var userReviews = UnitOfWork.Review.GetRatingBreakdownByUserId(dealerId);
            return userReviews.Where(i => i.DealerReview.DealerId == dealerId)
                              .GroupBy(i => i.DealerReview.SiteId)
                              .Select(
                                  i =>
                                  new UserRatingBreakdownViewModel()
                                  {
                                       DealerId = i.Select(j => j.DealerReview.DealerId).FirstOrDefault(),
                                      SiteName = i.Select(j => j.DealerReview.SiteReview.Name).FirstOrDefault(),
                                      SiteId = i.Select(j=>j.DealerReview.SiteId).FirstOrDefault(),
                                      AverageRating = i.Average(j => j.Rating),
                                      New = i.Count(j => j.UserId == null),
                                      Total = i.Count(),
                                      IsFitlerd = i.Select(j => j.IsFilterred).FirstOrDefault(),
                                       SiteLogo = i.Select(j=>j.DealerReview.SiteReview.Logo).FirstOrDefault()
                                  })
                              .OrderByDescending(i => i.AverageRating).ToList();
        }

        public void UpdateReview(int userReviewId, int userId, int departmentId)
        {
            UnitOfWork.Review.UpdateReview(userReviewId, userId, departmentId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void UpdateDealerReview(int dealerReviewId,string url)
        {
            UnitOfWork.Review.UpdateDealerReview(dealerReviewId,url);
            UnitOfWork.CommitVinreviewModel();
        }

        public void AddNewDealerReviews(DealerReviewViewModel dealerReview)
        {
            var drvs = MappingHandler.ConvertViewModelToDealerReview((dealerReview));

            UnitOfWork.Review.AddDealerReview(drvs);
            UnitOfWork.CommitVinreviewModel();         
        }

        public void AddUserReview(UserReviewViewModel model)
        {
            UnitOfWork.Review.AddUserReview(MappingHandler.ConvertViewModelToUserReview(model));
            UnitOfWork.CommitVinreviewModel();
        }

        public List<UserReviewViewModel> GetReviewsByTime(int dealerId)
        {
            var reviews = UnitOfWork.Review.GetAllUserReviews();
            
            var query = reviews.Where(i=>i.DealerReview.DealerId == dealerId);
            
            var models = query.Select(i => new UserReviewViewModel()
            {
               SiteId = i.DealerReview.SiteId,
               ReviewDate = i.ReviewDate,
               SiteName = i.DealerReview.SiteReview.Name
            }).ToList();

            return models;
        }

        public List<DealerUrlViewModel> GetDealerWebsite(int dealerId)
        {
            var dealerWebsite = UnitOfWork.Review.GetDealerWebsite(dealerId).Where(i => i.DealerId == dealerId);
            return dealerWebsite.Select(i => new DealerUrlViewModel() {Name = i.Dealer.Name,Phone = i.Dealer.Phone,DealerId = i.DealerId, WebSiteUrl = i.WebSiteUrl, WebSiteLogo = i.LogoUrl, Email = i.Dealer.Email, Address = i.Dealer.Address, City = i.Dealer.City,State = i.Dealer.State,Postal = i.Dealer.ZipCode}).ToList();
        }

        public List<DealerReviewViewModel> GetDealerSiteReviews(int dealerId)
        {
            var list = UnitOfWork.Review.GetDealerReviews(dealerId);
            return list.Any() ? list.AsEnumerable().GroupBy(i => i.SiteId).Select(ii => new DealerReviewViewModel(ii.FirstOrDefault())).ToList() : new List<DealerReviewViewModel>();
        }

        public void UpdateReviews(List<UserReviewViewModel> model)
        {
            foreach (var item in model)
            {
                UnitOfWork.Review.UpdateReview(item.UserReviewId, item.UserId, item.DepartmentId);
            }
            UnitOfWork.CommitVinreviewModel();
        }

        public bool CheckExistingUserReview(int dealerReviewId, string author, DateTime reviewDate, decimal rating)
        {
            return UnitOfWork.Review.GetUserReview(dealerReviewId, author, reviewDate, rating) != null;
        }

        #endregion
    }
}
