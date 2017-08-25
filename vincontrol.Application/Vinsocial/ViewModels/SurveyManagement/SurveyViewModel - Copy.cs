using System;
using System.Collections.Generic;
using vincontrol.Application.Vinsocial.ViewModels.CustomerManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.ViewModels.SurveyManagement
{
    public class SurveyViewModel
    {
        public SurveyViewModel() { }

        public SurveyViewModel(Survey survey)
        {
            SurveyId = survey.SurveyId;
            UserId = survey.UserId;
            ManagerId = survey.ManagerId;
            BDCId = survey.BDCId;
            DepartmentId = survey.DepartmentId;
            TemplateId = survey.SurveyTemplateId;
            TemplateName = survey.SurveyTemplate != null ? survey.SurveyTemplate.Name : string.Empty;
            SurveyStatusId = survey.SurveyStatusId;
            Rating = survey.Rating;
            Description = survey.Description;
            Comments = survey.Comments;
            DateStamp = survey.DateStamp;

            CustomerInformationId = survey.CustomerId;
            if (survey.Customer != null)
            {                
                CustomerInformation = new CustomerInformationViewModel
                    {
                        CustomerInformationId = survey.CustomerId,
                        CustomerLevelId = survey.Customer.CustomerLevelId,
                        CustomerLevelName =
                            survey.Customer.CustomerLevel != null
                                ? survey.Customer.CustomerLevel.Name
                                : Constant.Constanst.CustomerLevelNames.Moderate,
                        Email = survey.Customer.Email,
                        FirstName = survey.Customer.FirstName,
                        LastName = survey.Customer.LastName,
                        HomeNumber = survey.Customer.HomeNumber,
                        CellNumber = survey.Customer.CellNumber,
                    };
            }

            SurveyStatusId = survey.SurveyStatusId;
            if (survey.SurveyStatu != null)
            {
                Status = survey.SurveyStatu.Name;
            }

            //if (survey.Department != null)
            //{
            //    Department = survey.Department.Name;
            //}

            CustomerAnwers = new List<CustomerAnswer>();
        }

        public int SurveyId { get; set; }
        public string EncryptedSurveyId { get; set; }
        public int UserId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int BDCId { get; set; }
        public int DepartmentId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int SurveyStatusId { get; set; }
        public string SurveyUrl { get; set; }
        public string Status { get; set; }
        public int CustomerInformationId { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public DateTime DateStamp { get; set; }

        public CustomerInformationViewModel CustomerInformation { get; set; }
        public CustomerVehicleViewModel CustomerVehicle { get; set; }

        public List<CustomerAnswer> CustomerAnwers { get; set; }
        public List<SurveyQuestion> Questions { get; set; }
        public List<SurveyAnswer> Answers { get; set; }

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
    }

    public class SurveyEmailViewModel
    {
        public string Content { get; set; }
        public string SurveyUrl { get; set; }
        public string SalespersonName { get; set; }
        public string SalespersonPhone { get; set; }
        public string SalespersonAddress { get; set; }
    }

    public class SurveyQuestionAnswerViewModel
    {
        public SurveyQuestionAnswerViewModel()
        {
            Questions = new List<SurveyQuestion>();
        }

        public int UserId { get; set; }
        public int SurveyId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int TemplateId { get; set; }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string SalespersonName { get; set; }
        public string SalespersonPhone { get; set; }
        public string SalespersonAddress { get; set; }
        public string SalespersonEmail { get; set; }
        public string ManagerName { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerAddress { get; set; }
        public string ManagerEmail { get; set; }
        public decimal OverallRating { get; set; }
        public string ProfileUrl { get; set; }
        public string Comment { get; set; }
        public List<SurveyQuestion> Questions { get; set; }
        public List<CustomerAnswer> Answers { get; set; }
        public string AnswersString { get; set; } //TODO: temporarily use
    }

    public class SurveyQuestion
    {
        public SurveyQuestion() { }

        public SurveyQuestion(vincontrol.Data.Model.SurveyQuestion obj)
        {
            SurveyQuestionId = obj.SurveyQuestionId;
            Content = obj.Content;
            SurveyTemplateId = obj.SurveyTemplateId.GetValueOrDefault();
            Point = obj.Point.GetValueOrDefault();
            Order = obj.Order.GetValueOrDefault();
            DateStamp = obj.DateStamp.GetValueOrDefault();
        }

        public int SurveyQuestionId { get; set; }
        public double AverageRating { get; set; }
        public string Content { get; set; }
        public int SurveyTemplateId { get; set; }
        public int? Point { get; set; }
        public int? Order { get; set; }
        public DateTime DateStamp { get; set; }

        public CustomerAnswer CustomerAnswer { get; set; }
    }

    public class SurveyAnswer
    {
        public SurveyAnswer() { }

        public SurveyAnswer(vincontrol.Data.Model.SurveyAnswer obj)
        {
            SurveyAnswerId = obj.SurveyAnswerId;
            Description = obj.Description;
            Value = obj.Value;
        }

        public int SurveyAnswerId { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
    }

    public class CustomerAnswer
    {
        public CustomerAnswer() { }

        public CustomerAnswer(vincontrol.Data.Model.CustomerAnswer obj)
        {
            SurveyId = obj.SurveyId;
            SurveyQuestionId = obj.SurveyQuestionId;
            SurveyAnswerId = obj.SurveyAnswerId;
            if (obj.SurveyAnswer != null)
            {
                AnswerValue = obj.SurveyAnswer.Value;
            }
        }

        public int SurveyId { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyAnswerId { get; set; }
        public double AnswerValue { get; set; }

        public string Stars
        {
            get
            {
                string result;
                if (AnswerValue == 1d)
                    result = "one";
                else if (AnswerValue == 2d)
                    result = "two";
                else if (AnswerValue == 3d)
                    result = "three";
                else if (AnswerValue == 4d)
                    result = "four";
                else if (AnswerValue == 5d)
                    result = "five";
                else
                    result = "no";

                return result;
            }
        }
    }

    public class SurveysOverviewViewModel
    {
        /* leo */
        public int SurveyId { get; set; }
        public string EncryptedSurveyId { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public int Year { get; set; }
        public int Make { get; set; }
        public string MakeName { get; set; }
        public int Model { get; set; }
        public string ModelName { get; set; }
        public DateTime DateStamp { get; set; }
        public string UserName { get; set; }
        public string Customer { get; set; }
        public double Rating { get; set; }
        public string Level { get; set; }
        public int SurveyStatus { get; set; }
        public string ManagerName { get; set; }
        /* end leo */
    }

    public class SurveysStatisticsViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalSurveys { get; set; }
        public int FiveStarsSurveys { get; set; }
        public double BluePercentage { get; set; }
        public double SumScore { get; set; }
        public int Goal { get; set; }
    }

    public class SurveysGoalViewModel
    {
        public int DealerId { get; set; }
        public int DeparmentId { get; set; }
        public int Goal { get; set; }
        public DateTime DateStamp { get; set; }
    }

    public class SurveyTemplateViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int SurveyTemplateId { get; set; }
        public string SurveyTemplateName { get; set; }
        public int HighestQuestionId { get; set; }
        public int LowestQuestionId { get; set; }
        public List<SurveyQuestion> Questions { get; set; }
    }

    public class DashboardSurveTemplatesViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<SelectListItem> SurveyTemplates { get; set; }
        public SurveyTemplateViewModel SelectedTemplate { get; set; }
    }
}
