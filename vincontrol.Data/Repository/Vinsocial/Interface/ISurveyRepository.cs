using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface ISurveyRepository
    {
        void AddNewSurvey(Survey obj);
        void AddNewAnswer(CustomerAnswer obj);
        IQueryable<Survey> GetSurveys(int userId);
        IQueryable<Survey> GetSurveysByTemplate(int templateId);
        Survey GetSurvey(int surveyId);
        IQueryable<SurveyQuestion> GetQuestions(int templateId);
        CustomerAnswer GetAnswer(int surveyId, int surveyQuestionId);
        IQueryable<Survey> GetAll();
        IQueryable<SurveyGoal> GetAllSurveyGoal();
        IQueryable<CustomerAnswer> GetCustomerAnswers(int surveyId);
        IQueryable<CustomerAnswer> GetCustomerAnswersByQuestion(int questionId);
        IQueryable<SurveyAnswer> GetSurveyAnswers();
        double CalculateAverageRating(Survey survey);
        void UpdateSurvey(int surveyId, int statusId);
        void UpdateSurveySalesperson(int surveyId, int salespersonId);
        void UpdateSurveyManager(int surveyId, int managerId);
        void UpdateGoal(int dealerId, int goal, int departmentId);
        void DeleteCustomerAnswer(CustomerAnswer obj);
        void DeleteSurvey(Survey obj);
        IQueryable<SurveyQuestion> GetQuestionsByDealer(int dealerId);
        SurveyTemplate GetSurveyTemplate(int templateId);
        IQueryable<SurveyQuestion> GetQuestionsByTemplate(int templateId);
        IQueryable<Survey> GetTodayFollowupSurveys();
    }
}