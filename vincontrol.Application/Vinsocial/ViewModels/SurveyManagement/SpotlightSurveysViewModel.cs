using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.Vinsocial.ViewModels.SurveyManagement
{
    public class SpotlightSurveysViewModel
    {
        public SpotlightSurveysViewModel()
        {
        }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public double? HighestRating { get; set; }
        public double? LowestRating { get; set; }
        public int? BestSurveyTemplateId { get; set; }
        public int? WorstSurveyTemplateId { get; set; }
        public string HighestDescriptions { get; set; }
        public string LowestDescriptions { get; set; }
    }
}
