using System.Collections.Generic;
using vincontrol.Application.Vinsocial.ViewModels.SurveyManagement;

namespace vincontrol.Application.Vinsocial.Forms.SurveyManagement
{
    public interface ISurveyManagementForm
    {
        void SendingSurvey(SurveyViewModel viewModel);
        void ResponseToSurvey(SurveyQuestionAnswerViewModel model);
        SurveyQuestionAnswerViewModel InitializeSurveyQuestionsAnswers(int salespersonId, int surveyId);
        List<SurveyViewModel> GetSurveys(int userId);
        SurveyViewModel GetSurvey(int surveyId);
        List<SurveyQuestion> GetQuestions(int templateId);
        List<SurveyAnswer> GetSurveyAnswers();
        List<SurveysStatisticsViewModel> GetStatisticsSurvey(int dealerId,  string from, string to);
        SurveyViewModel GetSurveyDetails(int surveyId);
        void UpdateSurvey(int surveyId, int statusId);
        bool CheckExpirationDate(int surveyId);
        List<SurveysOverviewViewModel> GetSurveyByUserId(int userId);
        List<SurveysOverviewViewModel> GetSurveyByManagerId(int managerId, int userId);
        void SendNewSurvey(int surveyId);
        void UpdateCustomerAnswers(int surveyId, List<CustomerAnswer> customerAnswers);
        void UpdateSurveySalesperson(int surveyId, int salespersonId);
        void UpdateSurveyManager(int surveyId, int managerId);
        void UpdateGoal(int dealerId, int goal, int departmentId);
        List<SpotlightSurveysViewModel> GetSpotlightSurveys(int dealerId);
        List<SurveyViewModel> GetSurveysByTime(int dealerId, int time, string unit);
        List<SurveysOverviewViewModel> GetOrverviewSurveys(int userId);
        SurveyTemplateViewModel GetSurveyTemplateStatistic(int templateId);
        List<SurveyViewModel> GetTodayFollowupSurveys();
    }
}