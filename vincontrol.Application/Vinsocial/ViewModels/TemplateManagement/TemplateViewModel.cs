using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using SurveyQuestion = vincontrol.Application.Vinsocial.ViewModels.SurveyManagement.SurveyQuestion;

namespace vincontrol.Application.Vinsocial.ViewModels.TemplateManagement
{
    public class TemplateViewModel
    {
        public TemplateViewModel()
        {
            TotalPoints = 100;
            QuestionList = new List<SurveyQuestion>()
                {
                    new SurveyQuestion(){ Order = 1, Point = 5, Content = string.Empty }
                };
        }

        public TemplateViewModel(SurveyTemplate obj)
        {
            TemplateId = obj.SurveyTemplateId;
            Name = obj.Name;
            DealerId = obj.DealerId.GetValueOrDefault();
            DepartmentId = obj.DepartmentId.GetValueOrDefault();
            EmailContent = obj.EmailContent;
            TotalPoints = obj.TotalPoints;
            DateStamp = obj.DateStamp.GetValueOrDefault();
            if (obj.SurveyQuestions != null && obj.SurveyQuestions.Count > 0)
            {
                QuestionList = obj.SurveyQuestions.Select(i => new SurveyManagement.SurveyQuestion(i)).ToList();
            }
        }

        public int TemplateId { get; set; }
        public int TotalPoints { get; set; }
        public string Name { get; set; }
        //public string Content { get; set; }
        public int DealerId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string EmailContent { get; set; }
        public DateTime DateStamp { get; set; }

        public string Questions { get; set; }
        public List<SurveyManagement.SurveyQuestion> QuestionList { get; set; }
    }
}
