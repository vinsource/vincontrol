using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    public class DealerReviewViewModel
    {
        public DealerReviewViewModel()
        {
            
        }

        public DealerReviewViewModel(DealerReview obj)
        {
            OverallScore = obj.OverallScore??0;
            Url = obj.Url;
            Category = obj.CategoryReview.Name;
            SiteId = obj.SiteId;
            SiteName = obj.SiteReview.Name;
            SiteLogo = obj.SiteReview.Logo;
            SiteBanner = obj.SiteReview.Banner;
            SiteUrl = obj.SiteReview.Url;
            CategoryId = obj.CategoryReviewId ?? 0;
            DealerId = obj.DealerId;
            DealerReviewId = obj.DealerReviewId;
        }
        
        public decimal OverallScore { get; set; }
        public List<UserReviewViewModel> UserReviews { get; set; }
        public string Url { get; set; }
        public string Category { get; set;  }
        public int CategoryId { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public string SiteLogo { get; set; }
        public string SiteBanner { get; set; }
        public string SiteUrl { get; set; }
        public int DealerId { get; set; }
        public int DealerReviewId { get; set; }
    }
}
