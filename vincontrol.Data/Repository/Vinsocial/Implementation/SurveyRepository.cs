using System;
using System.Linq;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class SurveyRepository : ISurveyRepository
    {
        private VinReviewEntities _context;

        public SurveyRepository(VinReviewEntities context)
        {
            _context = context;
        }

        #region IReviewRepository' Members

        public void AddNewSurvey(Survey obj)
        {
            _context.AddToSurveys(obj);
        }

        public void AddNewAnswer(CustomerAnswer obj)
        {
            _context.AddToCustomerAnswers(obj);
        }

        public IQueryable<Survey> GetSurveys(int userId)
        {
            return _context.Surveys.Where(s => s.UserId == userId);
        }

        public IQueryable<Survey> GetSurveysByTemplate(int templateId)
        {
            return _context.Surveys.Where(s => s.SurveyTemplateId == templateId);
        }

        public Survey GetSurvey(int surveyId)
        {
            return _context.Surveys.Include("Comments1").Include("SurveyTemplate").FirstOrDefault(s => s.SurveyId == surveyId);
        }
        public SurveyGoal GetSurveyGoal(int dealerId, int deparmentId)
        {
            return _context.SurveyGoals.FirstOrDefault(s => s.DealerId == dealerId && s.DepartmentId == deparmentId);
        }
        public IQueryable<SurveyQuestion> GetQuestions(int templateId)
        {
            return _context.SurveyQuestions.Where(i => i.SurveyTemplateId == templateId).OrderBy(x => x.Order);
        }

        public CustomerAnswer GetAnswer(int surveyId, int surveyQuestionId)
        {
            return _context.CustomerAnswers.Include("SurveyAnswer").FirstOrDefault(i => i.SurveyId == surveyId && i.SurveyQuestionId == surveyQuestionId);
        }

        public IQueryable<Survey> GetAll()
        {
            return _context.Surveys;
        }

        public IQueryable<SurveyGoal> GetAllSurveyGoal()
        {
            return _context.SurveyGoals;
        }

        public IQueryable<CustomerAnswer> GetCustomerAnswers(int surveyId)
        {
            return _context.CustomerAnswers.Include("SurveyAnswer").Where(ca => ca.SurveyId == surveyId);
        }

        public IQueryable<CustomerAnswer> GetCustomerAnswersByQuestion(int questionId)
        {
            return _context.CustomerAnswers.Include("SurveyAnswer").Where(ca => ca.SurveyQuestionId == questionId);
        }

        public IQueryable<SurveyAnswer> GetSurveyAnswers()
        {
            return _context.SurveyAnswers;
        }

        public double CalculateAverageRating(Survey survey)
        {
            return (Math.Ceiling(survey.CustomerAnswers.Aggregate<CustomerAnswer, double>(0,
                                                                             (current, answer) =>
                                                                             current + (Convert.ToInt32(answer.SurveyAnswer.Value) * (answer.SurveyQuestion.Point * 0.01)).GetValueOrDefault()) * 2)) / 2;
        }

        public void UpdateSurvey(int surveyId, int statusId)
        {
            var existingSurvey = GetSurvey(surveyId);
            // Don't update status if the current status is Submitted & new status is Viewed
            if (existingSurvey != null && !(existingSurvey.SurveyStatusId == Constanst.SurveyStatusIds.Submitted && statusId == Constanst.SurveyStatusIds.Viewed))
            {
                existingSurvey.SurveyStatusId = statusId;
                existingSurvey.DateStamp = DateTime.Now;
            }
        }

        public void UpdateSurveySalesperson(int surveyId, int salespersonId)
        {
            var existingSurvey = GetSurvey(surveyId);
            if (existingSurvey != null)
            {
                existingSurvey.UserId = salespersonId;
            }
        }

        public void UpdateSurveyManager(int surveyId, int managerId)
        {
            var existingSurvey = GetSurvey(surveyId);
            if (existingSurvey != null)
            {
                existingSurvey.ManagerId = managerId;
            }

        }

        public void AddSurveyGoal(SurveyGoal obj)
        {
            _context.AddToSurveyGoals(obj);
            _context.SaveChanges();
        }

        public void UpdateGoal(int dealerId, int goal, int departmentId)
        {
            var existingSurveyGoal = GetSurveyGoal(dealerId, departmentId);
            if (existingSurveyGoal != null)
            {
                existingSurveyGoal.Goal = goal;
                existingSurveyGoal.DateStamp = DateTime.Now;
                _context.SaveChanges();
            }
            else
            {
                AddSurveyGoal(new SurveyGoal()
                    {
                        DealerId = dealerId,
                        Goal = goal,
                        DepartmentId = departmentId,
                        DateStamp = DateTime.Now
                    });


            }
        }

        public void DeleteCustomerAnswer(CustomerAnswer obj)
        {
            _context.DeleteObject(obj);
        }

        public void DeleteSurvey(Survey obj)
        {
            _context.DeleteObject(obj);
        }

        public IQueryable<SurveyQuestion> GetQuestionsByDealer(int dealerId)
        {
            return _context.SurveyQuestions.Include("CustomerAnswers.SurveyAnswer").Where(x => x.SurveyTemplate.DealerId == dealerId);
        }

        public SurveyTemplate GetSurveyTemplate(int templateId)
        {
            return _context.SurveyTemplates.FirstOrDefault(x => x.SurveyTemplateId == templateId);
        }
        public IQueryable<SurveyQuestion> GetQuestionsByTemplate(int templateId)
        {
            return _context.SurveyQuestions.Include("CustomerAnswers.SurveyAnswer").Where(x => x.SurveyTemplateId == templateId);
        }

        public IQueryable<Survey> GetTodayFollowupSurveys()
        {
            var today = DateTime.Now;
            var tomorrow = today.AddHours(24);
            var query = from c in _context.Communications
                        where c.CommunicationStatusId == vincontrol.Constant.Constanst.CommunicationStatusIds.Followup
                        group c by c.SurveyId into g
                        select new
                        {
                            Communication = g.OrderByDescending(x => x.Datestamp).FirstOrDefault()
                        };
            query = query.Where(x => x.Communication.Datestamp >= today && x.Communication.Datestamp <= tomorrow);
            var surveyIds = query.Select(x => x.Communication.SurveyId);
            return _context.Surveys.Include("Communications").Where(x => surveyIds.Contains(x.SurveyId));
        }
        #endregion
    }
}
