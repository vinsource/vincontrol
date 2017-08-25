using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;

namespace vincontrol.Application.ViewModels.TradeInManagement
{
    public class LandingViewModel
    {
        public LandingViewModel()
        {
            DealerInfo = new DealershipViewModel();
            VehicleInfo = new TradeInVehicleModel();
            DealerReview = new DealerReviewViewModel();
        }

        public DealershipViewModel DealerInfo { get; set; }
        public TradeInVehicleModel VehicleInfo { get; set; }
        public DealerReviewViewModel DealerReview { get; set; }
    }
}