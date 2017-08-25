using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    public class SiteReviewViewModel
    {
        public SiteReviewViewModel(SiteReview obj)
        {
            Id = obj.SiteId;
            Name = obj.Name;
            Url = obj.Url;
            Logo = obj.Logo;
            Banner = obj.Banner;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public string Banner { get; set; }
    }
}
