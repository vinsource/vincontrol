using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    public class UserReviewViewModel
    {
        public UserReviewViewModel() { }

        public UserReviewViewModel(UserReview review)
        {
            UserId = review.UserId ?? 0;
            DepartmentId = review.DepartmentId ?? 0;
            UserReviewId = review.UserReviewId;
            DealerReviewId = review.DealerReviewId;
            DealerReviewUrl = review.DealerReview.Url;
            DealerId = review.DealerReview.DealerId;
            SiteId = review.DealerReview.SiteReview.SiteId;
            SiteUrl = review.DealerReview.SiteReview.Url;
            SiteName = review.DealerReview.SiteReview.Name;
            SiteLogo = review.DealerReview.SiteReview.Logo;
            Author = review.Author;
            ReviewDate = review.ReviewDate;
            Rating = review.Rating;
            Comment = review.Comment;
        }
        
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int DepartmentId { get; set; }
        public int UserReviewId { get; set; }
        public int DealerReviewId { get; set; }
        public string DealerReviewUrl { get; set; }
        public int DealerId { get; set; }
        public int SiteId { get; set; }
        public string SiteUrl { get; set; }
        public string SiteName { get; set; }
        public string SiteLogo { get; set; }
        public string Author { get; set; }
        public DateTime ReviewDate { get; set; }
        public decimal Rating { get; set; }
        public string StarsNo
        {
            get
            {
                string result;
                if (Math.Floor(Rating) == 1)
                    result = "one";
                else if (Math.Floor(Rating) == 2)
                    result = "two";
                else if (Math.Floor(Rating) == 3)
                    result = "three";
                else if (Math.Floor(Rating) == 4)
                    result = "four";
                else if (Math.Floor(Rating) == 5)
                    result = "five";
                else
                    result = "no";

                if (Math.Ceiling(Rating) != Math.Floor(Rating))
                    result += "-half";

                return result;
            }
        }

        public string Comment { get; set; }
        public bool Filtered { get; set; }
    }
}
