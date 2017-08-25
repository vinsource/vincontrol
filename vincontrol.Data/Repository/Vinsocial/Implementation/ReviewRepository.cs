using System;
using System.Linq;
using System.Security.Policy;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class ReviewRepository : IReviewRepository
    {
        private VinReviewEntities _context;
        private VincontrolEntities _vincontrol;
        public ReviewRepository(VinReviewEntities context)
        {
            _context = context;
            _vincontrol = new VincontrolEntities();
        }

        #region IReviewRepository' Members

        public DealerReview GetDealerReview(int dealerReviewId)
        {
            return _context.DealerReviews.FirstOrDefault(i => i.DealerReviewId == dealerReviewId);
        }

        public DealerReview GetDealerReview(int dealerId, int siteId, int categoryId)
        {
            return _context.DealerReviews.FirstOrDefault(i => i.DealerId == dealerId && i.SiteId == siteId && i.CategoryReviewId == categoryId);
        }

        public IQueryable<DealerReview> GetDealerReviews(int dealerId)
        {
            return _context.DealerReviews.Where(i => i.DealerId == dealerId);
        }

        public IQueryable<DealerReview> GetDealerReviewsBySite(int siteId)
        {
            return _context.DealerReviews.Where(i => i.SiteId == siteId);
        }

        public IQueryable<SiteReview> GetDealerSiteReview(int dealerId)
        {
            return _context.SiteReviews.Where(i => i.DealerReviews.Any(ii => ii.DealerId == dealerId));
        }

        public UserReview GetUserReview(int dealerReviewId, string author, DateTime reviewDate, decimal rating)
        {
            return
                _context.UserReviews.FirstOrDefault(
                    i =>
                    i.DealerReviewId == dealerReviewId && i.Author.ToLower().Equals(author.ToLower()) &&
                    i.ReviewDate.Equals(reviewDate) && i.Rating.Equals(rating));
        }

        public AdditionalDealerReview GetAdditionalDealerReview(int dealerReviewId)
        {
            return _context.AdditionalDealerReviews.FirstOrDefault(i => i.DealerReviewId == dealerReviewId);
        }

        public IQueryable<UserReview> GetUserReviews(int dealerId)
        {
            var dealerReviews = GetDealerReviews(dealerId).Select(i => i.DealerReviewId);
            return _context.UserReviews.Where(i => dealerReviews.Contains(i.DealerReviewId)).OrderByDescending(i => i.ReviewDate);
        }

        public IQueryable<UserReview> GetUserReviewsByUser(int userId)
        {
            return _context.UserReviews.Where(i => i.UserId == userId).OrderByDescending(i => i.ReviewDate);
        }

        public UserReview GetUserReview(int id)
        {
            return _context.UserReviews.FirstOrDefault(i => i.UserReviewId == id);
        }

        public IQueryable<SiteReview> GetAllSiteReview()
        {
            return _context.SiteReviews.AsQueryable();
        }

        public void AddDealerReview(int dealerId, int siteId, string url, decimal overallScore, string about)
        {
            _context.AddToDealerReviews(new DealerReview()
                                            {
                                                DealerId = dealerId,
                                                SiteId = siteId,
                                                Url = url,
                                                OverallScore = overallScore,
                                                About = about,
                                                DateStamp = DateTime.Now
                                            });

        }

        public void AddAdditionalDealerReview(AdditionalDealerReview review)
        {
            _context.AddToAdditionalDealerReviews(review);
        }

        public void AddDealerReview(DealerReview dealerReview)
        {
            _context.AddToDealerReviews(dealerReview);

        }

        public void AddUserReview(int dealerReviewId, string author, DateTime reviewDate, decimal rating, string comment, bool isFilterred)
        {
            var existingUserReview = GetUserReview(dealerReviewId, author, reviewDate, rating);
            if (existingUserReview == null)
            {
                _context.AddToUserReviews(new UserReview()
                                              {
                                                  DealerReviewId = dealerReviewId,
                                                  Author = author,
                                                  ReviewDate = reviewDate,
                                                  Rating = rating,
                                                  Comment = comment,
                                                  DateStamp = DateTime.Now,
                                                  IsFilterred = isFilterred
                                              });
            }
        }

        public void AddUserReview(UserReview userReview)
        {
            _context.AddToUserReviews(userReview);
        }

        public void AddAdditionalUserReview(int userReviewId, decimal customerService, decimal qualityOfWork, decimal friendliness, decimal overallExperience, decimal pricing, decimal buyingProcess, decimal overallFacilities, string reasonForVisit, bool recommentThisDealer, bool purchasedAVehicle)
        {
            var existingReview = _context.AdditionalUserReviews.FirstOrDefault(i => i.UserReviewId == userReviewId);
            if (existingReview == null)
            {
                var newReview = new AdditionalUserReview()
                                    {
                                        UserReviewId = userReviewId,
                                        CustomerService = customerService,
                                        QualityOfWork = qualityOfWork,
                                        Friendliness = friendliness,
                                        OverallExperience = overallExperience,
                                        Pricing = pricing,
                                        BuyingProcess = buyingProcess,
                                        OverallFacilities = overallFacilities,
                                        ReasonForVisit = reasonForVisit,
                                        RecommendThisDealer = recommentThisDealer,
                                        PurchasedAVehicle = purchasedAVehicle
                                    };
                _context.AddToAdditionalUserReviews(newReview);
            }
        }

        public UserReview GetLatestUserReview(int dealerReviewId)
        {
            return _context.UserReviews.Where(i => i.DealerReviewId == dealerReviewId).OrderByDescending(i => i.ReviewDate).FirstOrDefault();
        }

        public IQueryable<UserReview> GetTotalRating(int userId)
        {
            return _context.UserReviews.Where(i => i.UserId == userId);
        }

        public IQueryable<UserReview> GetUserReviewsByUserId(int userId)
        {
            return _context.UserReviews.Where(i => i.UserId == userId);
        }

        public IQueryable<UserReview> GetRatingBreakdownByUserId(int userId)
        {
            return _context.UserReviews.AsQueryable();
        }

        public void UpdateReview(int userReviewId, int userId, int departmentId)
        {
            var existingReview = GetUserReview(userReviewId);
            if (existingReview != null)
            {
                existingReview.UserId = userId;
                existingReview.DepartmentId = departmentId;
            }
        }

        public void UpdateDealerReview(int dealerReviewId, string url)
        {
            var existingDealerReview = GetDealerReview(dealerReviewId);
            if (existingDealerReview != null)
            {
                if (url == null)
                {
                    url = string.Empty;
                }
                existingDealerReview.Url = url;
            }
        }

        public IQueryable<Setting> GetDealerWebsite(int dealerId)
        {
            return _vincontrol.Settings.AsQueryable();
        }

        public IQueryable<UserReview> GetAllUserReviews()
        {
            return _context.UserReviews;
        }

        #endregion
    }
}
