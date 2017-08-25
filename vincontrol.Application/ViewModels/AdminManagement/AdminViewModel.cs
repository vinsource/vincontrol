using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.AdminManagement
{
    public class AdminViewModel
    {
        public AdminViewModel(){ }

        public AdminViewModel(Setting setting)
        {
            CarFax = setting.CarFax;
            CarFaxPassword = setting.CarFaxPassword;
            Manheim = setting.Manheim;
            ManheimPassword = setting.ManheimPassword;
            KellyBlueBook = setting.KellyBlueBook;
            KellyPassword = setting.KellyPassword;
            FirstTimeRangeSurvey = setting.FirstTimeRangeSurvey.GetValueOrDefault();
            SecondTimeRangeSurvey = setting.SecondTimeRangeSurvey.GetValueOrDefault();
            IntervalSurvey = setting.IntervalSurvey.GetValueOrDefault();
        }

        public int DealershipId { get; set; }
        public string UserStamp { get; set; }

        public string CarFax { get; set; }
        public string CarFaxPassword { get; set; }
        public bool CarFaxPasswordChanged { get; set; }

        public string Manheim { get; set; }
        public string ManheimPassword { get; set; }
        public bool ManheimPasswordChanged { get; set; }

        public string KellyBlueBook { get; set; }
        public string KellyPassword { get; set; }
        public bool KellyPasswordChanged { get; set; }

        public string BlackBook { get; set; }
        public string BlackBookPassword { get; set; }
        public bool BlackBookPasswordChanged { get; set; }

        public int FirstTimeRangeSurvey { get; set; }
        public int SecondTimeRangeSurvey { get; set; }
        public int IntervalSurvey { get; set; }
    }
}
