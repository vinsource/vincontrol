using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Vinsocial.Forms.ReviewManagement;
using vincontrol.Application.Vinsocial.ViewModels.CustomerManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Application.Vinsocial.ViewModels.SurveyManagement;
using vincontrol.ConfigurationManagement;
using vincontrol.Constant;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using vincontrol.EmailHelper;
using CustomerAnswer = vincontrol.Application.Vinsocial.ViewModels.SurveyManagement.CustomerAnswer;
using SurveyAnswer = vincontrol.Application.Vinsocial.ViewModels.SurveyManagement.SurveyAnswer;
using SurveyQuestion = vincontrol.Application.Vinsocial.ViewModels.SurveyManagement.SurveyQuestion;

namespace vincontrol.Application.Vinsocial.Forms.SurveyManagement
{
    public class SurveyManagementForm : BaseForm, ISurveyManagementForm
    {
        private IEmail _emailHelper;
        private IAccountManagementForm _accountManagementForm;
        private ICommonManagementForm _vincontrolCommonManagementForm;
        private IReviewManagementForm _reviewManagementForm;

        #region Constructors
        public SurveyManagementForm()
            : this(new SqlUnitOfWork())
        {
            _emailHelper = new Email();
            _accountManagementForm = new AccountManagementForm();
            _vincontrolCommonManagementForm = new CommonManagementForm();
            _reviewManagementForm = new ReviewManagementForm();
        }

        public SurveyManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ISurveyManagementForm' Members

        public List<SpotlightSurveysViewModel> GetSpotlightSurveys(int dealerId)
        {
            var questions = UnitOfWork.Survey.GetQuestionsByDealer(dealerId).ToList();
            var model = new List<SpotlightSurveysViewModel>();

            var departments = UnitOfWork.Common.GetAllDepartments().ToList();

            foreach (var department in departments)
            {
                var departmentQuestions = questions.Where(x => x.SurveyTemplate.DepartmentId == department.DepartmentId && x.CustomerAnswers.Count > 0).ToList();
                if (departmentQuestions != null & departmentQuestions.Count > 0)
                {
                    var query = from question in departmentQuestions
                                select new
                                {
                                    Question = question,
                                    AverageRating = question.CustomerAnswers.Average(x => x.SurveyAnswer.Value),
                                };
                    var highestQuestion = query.OrderByDescending(x => x.AverageRating).ThenByDescending(x => x.Question.Point).FirstOrDefault();
                    var lowestQuestion = query.OrderBy(x => x.AverageRating).ThenBy(x => x.Question.Point).FirstOrDefault();
                    var spotlight = new SpotlightSurveysViewModel();
                    spotlight.DepartmentId = department.DepartmentId;
                    spotlight.DepartmentName = department.Name;
                    if (highestQuestion != null)
                    {
                        spotlight.BestSurveyTemplateId = highestQuestion.Question.SurveyTemplateId.GetValueOrDefault();
                        spotlight.HighestDescriptions = highestQuestion.Question.Content;
                        spotlight.HighestRating = highestQuestion.AverageRating;
                    }
                    if (lowestQuestion != null)
                    {
                        spotlight.WorstSurveyTemplateId = lowestQuestion.Question.SurveyTemplateId.GetValueOrDefault();
                        spotlight.LowestDescriptions = lowestQuestion.Question.Content;
                        spotlight.LowestRating = lowestQuestion.AverageRating;
                    }

                    model.Add(spotlight);
                }
            }
            return model;
        }

        public List<SurveyViewModel> GetSurveysByTime(int dealerId, int time, string unit)
        {
            var surveys = UnitOfWork.Survey.GetAll();
            var users = UnitOfWork.User.GetDealerEmployees(dealerId);
            var userIds = users.Select(x => x.UserId).ToList();
            var query = surveys;
            //Haryley: if unit == Days, the date variable will be reduce twice????
            var date = DateTime.Now.AddDays(-time).Date;
            if (unit == "Days")
            {
                date = DateTime.Now.AddDays(-time).Date;
                query = query.Where(i => i.DateStamp > date);
            }
            else
            {
                if (unit == "Months")
                {
                    date = DateTime.Now.AddMonths(-time).Date;
                    query = query.Where(i => i.DateStamp > date);
                }
                else
                {
                    if (unit == "Weeks")
                    {
                        date = DateTime.Now.AddDays(-time * 7).Date;
                        query = query.Where(i => i.DateStamp > date);
                    }
                }
            }

            var models = query.Select(i => new SurveyViewModel()
                {
                    SurveyId = i.SurveyId,
                    ManagerId = i.ManagerId,
                    Rating = i.Rating,
                    DateStamp = i.DateStamp,
                    UserId = i.UserId,
                    DepartmentId = i.DepartmentId
                }).ToList();

            return models;
        }

        public List<SurveyViewModel> GetReviewsByTime(int dealerId, int time, string unit)
        {
            throw new NotImplementedException();
        }

        public SurveyQuestionAnswerViewModel InitializeSurveyQuestionsAnswers(int salespersonId, int surveyId)
        {
            var currentSurvey = GetSurvey(surveyId);
            var currentUser = _accountManagementForm.GetUser(salespersonId);
            var currentManager = _accountManagementForm.GetUser(currentSurvey.ManagerId);
            var questions = GetQuestions(currentSurvey.TemplateId);
            var currentUserTotalRating = _reviewManagementForm.GetTotalRatingByUserId(salespersonId);
            return new SurveyQuestionAnswerViewModel()
                {
                    UserId = salespersonId,
                    SurveyId = surveyId,
                    CustomerId = currentSurvey.CustomerInformationId,
                    CustomerName = string.Format("{0} {1}", currentSurvey.CustomerInformation.FirstName, currentSurvey.CustomerInformation.LastName),
                    CustomerEmail = currentSurvey.CustomerInformation.Email,
                    TemplateId = currentSurvey.TemplateId,
                    DealerId = currentUser.DealerId,
                    DealerName = currentUser.DealerName,
                    SalespersonName = currentUser.Name,
                    SalespersonPhone = currentUser.Phone,
                    SalespersonEmail = currentUser.Email,
                    SalespersonAddress = currentUser.DealerAddress,
                    ManagerName = currentManager.Name,
                    ManagerPhone = currentManager.Phone,
                    ManagerEmail = currentManager.Email,
                    ManagerAddress = currentManager.DealerAddress,
                    OverallRating = currentUserTotalRating != null ? (Math.Ceiling(currentUserTotalRating.Rating * 2)) / 2 : 0,
                    ProfileUrl = String.Format("http://{0}/{1}/{2}/InventoryProfile/{3}/{4}", "vinsocial.net", currentUser.DealerName.Replace(" ", "-"), currentUser.Name.Replace(" ", "-"), EncodeTo64(salespersonId.ToString()), EncodeTo64(currentUser.DealerId.ToString())),
                    Questions = questions,
                    CreatedDate = currentSurvey.CreatedDate,
                };
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public void SendingSurvey(SurveyViewModel viewModel)
        {
            // Create a new customer with level "Moderate"
            viewModel.CustomerInformation.CustomerLevelId = Constant.Constanst.CustomerLevelIds.Moderate;
            var newCustomer = MappingHandler.ConvertViewModelToCustomer(viewModel.CustomerInformation);
            var existingCustomer = UnitOfWork.VinsocialCommon.GetCustomer(viewModel.CustomerInformation.Email, viewModel.CustomerInformation.CellNumber);
            if (existingCustomer == null)
            {
                UnitOfWork.VinsocialCommon.AddNewCustomer(newCustomer);
                UnitOfWork.CommitVinreviewModel();
            }
            else newCustomer.CustomerId = existingCustomer.CustomerId;

            // Create a new vehicle for the customer
            viewModel.CustomerVehicle.CustomerInformationId = newCustomer.CustomerId;
            var newCustomerVehicle = MappingHandler.ConvertViewModelToCustomerVehicle(viewModel.CustomerVehicle);
            UnitOfWork.VinsocialCommon.AddNewCustomerVehicle(newCustomerVehicle);
            UnitOfWork.CommitVinreviewModel();

            // Create a new survey
            var newSurvey = MappingHandler.ConvertViewModelToSurvey(viewModel);
            newSurvey.CustomerId = newCustomer.CustomerId;
            newSurvey.SurveyStatusId = Constant.Constanst.SurveyStatusIds.Sent;
            newSurvey.CreatedDate = DateTime.Now;
            UnitOfWork.Survey.AddNewSurvey(newSurvey);
            UnitOfWork.CommitVinreviewModel();

            //TODO: Sending email
            var currentUser = _accountManagementForm.GetUser(viewModel.UserId);

            var surveyEmailTemplate = UnitOfWork.Template.GetTemplate(viewModel.TemplateId).EmailContent;
            surveyEmailTemplate = surveyEmailTemplate.Replace(Constanst.SurveyEmailTemplate.FirstName, newCustomer.FirstName);
            surveyEmailTemplate = surveyEmailTemplate.Replace(Constanst.SurveyEmailTemplate.LastName, newCustomer.LastName);
            if (surveyEmailTemplate.Contains(Constanst.SurveyEmailTemplate.Vehicle))
                surveyEmailTemplate = surveyEmailTemplate.Replace(Constanst.SurveyEmailTemplate.Vehicle,
                                                                  string.Format("{0} {1} {2}", newCustomerVehicle.Year,
                                                                                UnitOfWork.Common.GetChromeMakeName(newCustomerVehicle.Make),
                                                                                UnitOfWork.Common.GetChromeModelName(newCustomerVehicle.Model)));
            surveyEmailTemplate = surveyEmailTemplate.Replace(Constanst.SurveyEmailTemplate.DealerName, currentUser.DealerName);

            var emailContent = EmailTemplateReader.GetSurveyEmailContent();
            if (String.IsNullOrEmpty(currentUser.DealerName)) currentUser.DealerName = _accountManagementForm.GetDealer(currentUser.DealerId).Name;
            emailContent = emailContent.Replace(EmailTemplateReader.UserFullName, currentUser.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.Phone, currentUser.Phone);
            emailContent = emailContent.Replace(EmailTemplateReader.Address, currentUser.DealerAddress);
            emailContent = emailContent.Replace(EmailTemplateReader.SurveyTemplate, surveyEmailTemplate);
            emailContent = emailContent.Replace(EmailTemplateReader.SurveyUrl,
                                                String.Format(
                                                    "http://{0}/reviews/surveyquestionsanswers?salesperson={1}&survey={2}",
                                                    ConfigurationHandler.VINSocial, EncryptString(viewModel.UserId.ToString()),
                                                    EncryptString(newSurvey.SurveyId.ToString())));
            _emailHelper.SendEmail(new List<string> { viewModel.CustomerInformation.Email },
                                   currentUser.Name + " sent to you a survey" +
                                   (string.IsNullOrEmpty(currentUser.DealerName) ? "" : " from " + currentUser.DealerName), emailContent, currentUser.DealerName);
        }

        public void ResponseToSurvey(SurveyQuestionAnswerViewModel model)
        {
            var existingSurvey = UnitOfWork.Survey.GetSurvey(model.SurveyId);
            if (existingSurvey != null)
            {
                existingSurvey.SurveyStatusId = Constant.Constanst.SurveyStatusIds.Viewed;
                existingSurvey.Comments = model.Comment;
                existingSurvey.DateStamp = DateTime.Now;

                // update customer' answers
                foreach (var answer in model.Answers)
                {
                    var existingAnswer = UnitOfWork.Survey.GetAnswer(answer.SurveyId, answer.SurveyQuestionId);
                    if (existingAnswer != null)
                    {
                        existingAnswer.SurveyAnswerId = answer.SurveyAnswerId;
                    }
                    else
                    {
                        UnitOfWork.Survey.AddNewAnswer(MappingHandler.ConvertViewModelToCustomerAnswer(answer));
                    }
                }
                UnitOfWork.CommitVinreviewModel();

                // update Rating of this survey
                existingSurvey.Rating = UnitOfWork.Survey.CalculateAverageRating(existingSurvey);

                // update survey's status as "Submitted"
                existingSurvey.SurveyStatusId = Constanst.SurveyStatusIds.Submitted;
                UnitOfWork.CommitVinreviewModel();

                //log customer's answer into communication 
                var communication = new Communication
                {
                    SurveyId = model.SurveyId,
                    CommunicationTypeId = vincontrol.Constant.Constanst.CommunicationType.SurveyResult,
                    Datestamp = DateTime.Now,
                    UserId = existingSurvey.UserId,
                    ManagerId = existingSurvey.ManagerId,
                    SurveyResult = existingSurvey.Rating,
                };
                var questions = GetQuestions(existingSurvey.SurveyTemplateId);

                communication.Notes += "<div class='client-survey' style='border:none;0; margin:0;'><ul>";
                for (int i = 0; i < questions.Count; i++)
                {
                    var question = questions[i];
                    var answer = UnitOfWork.Survey.GetAnswer(existingSurvey.SurveyId, question.SurveyQuestionId);
                    communication.Notes += string.Format("<li><b>{0}.</b><span>{1}</span><div class='stars {2}-stars'></div></li>", 
                                                                i + 1, question.Content, GetStarString(answer.SurveyAnswer.Value));
                }
                communication.Notes += "</ul></div>";
                UnitOfWork.Customer.AddNewCommunication(communication);
                UnitOfWork.CommitVinreviewModel();

                // sending thank you email for good survey
                if (existingSurvey.Rating >= 4)
                    SendingGoodSurveyEmail(model);
            }
        }

        public void SendingGoodSurveyEmail(SurveyQuestionAnswerViewModel model)
        {
            var emailContent = EmailTemplateReader.GetGoodSurveyEmailContent();
            emailContent = emailContent.Replace(EmailTemplateReader.CustomerName, model.CustomerName);
            emailContent = emailContent.Replace(EmailTemplateReader.DealerName, model.DealerName);
            var reviewWebsites = _reviewManagementForm.GetDealerSiteReviews(model.DealerId).Where(i => i.Url != String.Empty).ToList();
            emailContent = emailContent.Replace(EmailTemplateReader.DealerReviewWebsite, GenerateReviewWebsitesToString(reviewWebsites));
            _emailHelper.SendEmail(new List<string> { model.CustomerEmail },
                                   "Thank you for your feedback, " + model.CustomerName, emailContent, model.DealerName);
        }

        public List<SurveyViewModel> GetSurveys(int userId)
        {
            var currentUser = UnitOfWork.User.GetUser(userId);
            var currentUserRoleIds = currentUser.UserPermissions.Select(x => x.RoleId);
            var dealerId = currentUser.DefaultLogin.Value;
            var users = UnitOfWork.User.GetDealerEmployees(dealerId);
            var userIds = users.Select(x => x.UserId).ToList();
            var query = UnitOfWork.Survey.GetAll();

            if (currentUserRoleIds.Contains(Constanst.RoleType.Employee))
            {
                query = query.Where(s => s.UserId == userId);
            }
            else if (currentUserRoleIds.Contains(Constanst.RoleType.Manager))
            {
                query = query.Where(s => s.ManagerId == userId);
            }
            else
            {
                query = query.Where(s => userIds.Contains(s.UserId));
            }

            if (query.Any())
            {
                var surveys = query.ToList()
                                   .OrderByDescending(s => s.DateStamp)
                                   .ThenBy(s => s.Rating)
                                   .ThenBy(s => s.Customer.FirstName)
                                   .ThenBy(s => s.Description)
                                   .Select(i => new SurveyViewModel(i)).ToList();

                foreach (var survey in surveys)
                {
                    var employee = users.FirstOrDefault(x => x.UserId == survey.UserId);
                    if (employee != null)
                    {
                        survey.EmployeeName = employee.Name;
                    }

                    var department = UnitOfWork.Common.GetDepartment(survey.DepartmentId);
                    if (department != null)
                        survey.Department = department.Name;

                }
                return surveys;
            }
            return new List<SurveyViewModel>();
        }

        public SurveyViewModel GetSurvey(int surveyId)
        {
            var existingSurvey = UnitOfWork.Survey.GetSurvey(surveyId);
            return existingSurvey == null ? new SurveyViewModel() : new SurveyViewModel(existingSurvey);
        }

        public List<SurveyQuestion> GetQuestions(int templateId)
        {
            var questions = UnitOfWork.Survey.GetQuestions(templateId);
            return questions.Any()
                       ? questions.AsEnumerable().Select(i => new SurveyQuestion(i)).ToList()
                       : new List<SurveyQuestion>();
        }

        public List<CustomerAnswer> GetCustomerAnswers(int surveyId)
        {
            var customerAnswers = UnitOfWork.Survey.GetCustomerAnswers(surveyId);
            return customerAnswers.Any()
                       ? customerAnswers.AsEnumerable().Select(i => new CustomerAnswer(i)).ToList()
                       : new List<CustomerAnswer>();
        }

        public List<SurveyAnswer> GetSurveyAnswers()
        {
            var answers = UnitOfWork.Survey.GetSurveyAnswers();
            return answers.Any()
                        ? answers.AsEnumerable().Select(a => new SurveyAnswer(a)).ToList()
                        : new List<SurveyAnswer>();
        }

        public List<SurveysStatisticsViewModel> GetStatisticsSurvey(int dealerId, string from, string to)
        {
            var surveys = UnitOfWork.Survey.GetAll();
            var users = UnitOfWork.User.GetDealerEmployees(dealerId);
            var userIds = users.Select(x => x.UserId).ToList();
            DateTime fromDate = Convert.ToDateTime(from);
            DateTime toDate = Convert.ToDateTime(to);
            var query = surveys.Where(i => userIds.Contains(i.UserId));

            query = query.Where(i => i.DateStamp > fromDate && i.DateStamp <= toDate);


            var models =
                query.ToList().GroupBy(i => i.DepartmentId).Select(t => new SurveysStatisticsViewModel()
                    {
                        TotalSurveys = t.Count(),
                        FiveStarsSurveys = t.Count(j => j.Rating > 4.5),
                        DepartmentId = t.Select(j => j.DepartmentId).FirstOrDefault(),
                        DepartmentName = UnitOfWork.Common.GetDepartment(t.FirstOrDefault().DepartmentId).Name,

                        BluePercentage = (t.Sum(j => j.Rating / 5) / (t.Count())) * 100,
                        SumScore = t.Sum(j => j.Rating / 5),
                        Goal = GetSurveyGoal(dealerId, t.Select(j => j.DepartmentId).FirstOrDefault())


                    }).ToList();
            return models;
        }

        public int GetSurveyGoal(int dealerId, int departmentId)
        {
            var listSurveyGoal = UnitOfWork.Survey.GetAllSurveyGoal();
            var surveyGoal =
                listSurveyGoal.FirstOrDefault(i => i.DealerId == dealerId && i.DepartmentId == departmentId);
            int goal = 100;
            if (surveyGoal != null) goal = surveyGoal.Goal;

            return goal;
        }

        public SurveyViewModel GetSurveyDetails(int surveyId)
        {
            var currentSurvey = GetSurvey(surveyId);
            var questions = GetQuestions(currentSurvey.TemplateId);
            var customerAnswers = GetCustomerAnswers(surveyId);

            foreach (var question in questions)
            {
                question.CustomerAnswer = customerAnswers.FirstOrDefault(x => x.SurveyQuestionId == question.SurveyQuestionId);
            }

            var answers = GetSurveyAnswers();

            currentSurvey.Questions = questions;
            currentSurvey.Answers = answers;

            var currentUser = UnitOfWork.User.GetUser(currentSurvey.UserId);
            var currentManager = UnitOfWork.User.GetUser(currentSurvey.ManagerId);

            if (currentUser != null)
            {
                currentSurvey.EmployeeName = currentUser.Name;
                if (currentUser.Department != null)
                    currentSurvey.Department = currentUser.Department.Name;
            }
            if (currentManager != null)
                currentSurvey.ManagerName = currentManager.Name;

            var department = UnitOfWork.Common.GetDepartment(currentSurvey.DepartmentId);
            if (department != null)
                currentSurvey.DepartmentName = department.Name;

            return currentSurvey;
        }

        public bool CheckExpirationDate(int surveyId)
        {
            var surveys = UnitOfWork.Survey.GetAll();
            var date = DateTime.Now.AddDays(-60).Date;
            var currentSurvey = surveys.Where(i => i.SurveyId == surveyId && i.CreatedDate >= date);

            return currentSurvey.Any();
        }

        public List<SurveysOverviewViewModel> GetSurveyByUserId(int userId)
        {
            var surveys = UnitOfWork.Survey.GetAll();
            var currentUser = _accountManagementForm.GetUser(userId);

            var results = surveys.Where(i => i.UserId == userId && i.SurveyStatusId != Constant.Constanst.SurveyStatusIds.Sent).ToList().Select(i => new SurveysOverviewViewModel()
                {
                    SurveyId = i.SurveyId,
                    UserId = i.UserId,
                    DateStamp = i.DateStamp,
                    Year = i.Customer.CustomerVehicles.Select(j => j.Year).FirstOrDefault(),
                    Make = i.Customer.CustomerVehicles.Select(j => j.Make).FirstOrDefault(),
                    MakeName = _vincontrolCommonManagementForm.GetChromeMakeName(i.Customer.CustomerVehicles.Select(j => j.Make).FirstOrDefault()),
                    Model = i.Customer.CustomerVehicles.Select(j => j.Model).FirstOrDefault(),
                    ModelName = _vincontrolCommonManagementForm.GetChromeModelName(i.Customer.CustomerVehicles.Select(j => j.Model).FirstOrDefault()),
                    UserName = currentUser.Name,
                    Customer = i.Customer.FirstName + " " + i.Customer.LastName,
                    Rating = i.Rating,
                    ManagerId = i.ManagerId,
                    Level = i.Customer.CustomerLevel.Name,
                    SurveyStatus = i.SurveyStatusId,
                    ManagerName = _accountManagementForm.GetUser(i.ManagerId).Name

                }).OrderByDescending(i => i.Rating).ToList();

            return results;
        }

        public List<SurveysOverviewViewModel> GetSurveyByManagerId(int managerId, int userId)
        {
            var results = GetSurveyByUserId(userId).Where(i => i.ManagerId == managerId).OrderByDescending(i => i.Rating).ToList();

            return results;
        }

        public void UpdateSurvey(int surveyId, int statusId)
        {
            UnitOfWork.Survey.UpdateSurvey(surveyId, statusId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void SendNewSurvey(int surveyId)
        {
            var lastSurvey = UnitOfWork.Survey.GetSurvey(surveyId);

            // Create a new survey
            var newSurvey = new vincontrol.Data.Model.Survey
            {
                CustomerId = lastSurvey.CustomerId,
                BDCId = lastSurvey.BDCId,
                UserId = lastSurvey.UserId,
                ManagerId = lastSurvey.ManagerId,
                DepartmentId = lastSurvey.DepartmentId,
                SurveyTemplateId = lastSurvey.SurveyTemplateId,
                SurveyStatusId = Constant.Constanst.SurveyStatusIds.Sent,
                DateStamp = DateTime.Now,
                CreatedDate = DateTime.Now,
            };
            UnitOfWork.Survey.AddNewSurvey(newSurvey);
            UnitOfWork.CommitVinreviewModel();

            //TODO: Sending email
            var user = _accountManagementForm.GetUser(lastSurvey.UserId);
            var emailContent = EmailTemplateReader.GetSurveyEmailContent();
            emailContent = emailContent.Replace(EmailTemplateReader.UserFullName, user.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.Phone, user.Phone);
            emailContent = emailContent.Replace(EmailTemplateReader.Address, user.DealerAddress);
            emailContent = emailContent.Replace(EmailTemplateReader.SurveyTemplate, UnitOfWork.Template.GetTemplate(lastSurvey.SurveyTemplateId).Question);
            emailContent = emailContent.Replace(EmailTemplateReader.SurveyUrl,
                                                String.Format(
                                                    "http://{0}/reviews/surveyquestionsanswers?salesperson={1}&survey={2}",
                                                    "vinsocial.net", EncryptString(lastSurvey.UserId.ToString()),
                                                    EncryptString(newSurvey.SurveyId.ToString())));
            _emailHelper.SendEmail(new List<string> { lastSurvey.Customer.Email },
                                   user.Name + " sent to you a survey" +
                                   (string.IsNullOrEmpty(user.DealerName) ? "" : " from " + user.DealerName), emailContent);
        }

        public void UpdateCustomerAnswers(int surveyId, List<CustomerAnswer> customerAnswers)
        {
            var survey = UnitOfWork.Survey.GetSurvey(surveyId);
            var existingCustomerAnswers = UnitOfWork.Survey.GetCustomerAnswers(surveyId).ToList();

            foreach (var customerAnswer in customerAnswers)
            {
                var existingAnswer = existingCustomerAnswers.FirstOrDefault(x => x.SurveyQuestionId == customerAnswer.SurveyQuestionId);
                if (existingAnswer != null)
                {
                    existingAnswer.SurveyAnswerId = customerAnswer.SurveyAnswerId;
                }
            }
            UnitOfWork.CommitVinreviewModel();

            survey.Rating = UnitOfWork.Survey.CalculateAverageRating(survey);
            UnitOfWork.CommitVinreviewModel();

            // sending thank you email for good survey
            if (survey.Rating >= 4)
            {
                var currentUser = _accountManagementForm.GetUser(survey.UserId);
                var model = new SurveyQuestionAnswerViewModel()
                    {
                        CustomerId = survey.CustomerId,
                        CustomerName = survey.Customer.FirstName + " " + survey.Customer.LastName,
                        CustomerEmail = survey.Customer.Email,
                        DealerId = currentUser.DealerId,
                        DealerName = currentUser.DealerName
                    };
                SendingGoodSurveyEmail(model);
            }
        }

        public void UpdateSurveySalesperson(int surveyId, int salespersonId)
        {
            UnitOfWork.Survey.UpdateSurveySalesperson(surveyId, salespersonId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void UpdateSurveyManager(int surveyId, int managerId)
        {
            UnitOfWork.Survey.UpdateSurveyManager(surveyId, managerId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void UpdateGoal(int dealerId, int goal, int departmentId)
        {
            UnitOfWork.Survey.UpdateGoal(dealerId, goal, departmentId);
        }

        public List<SurveysOverviewViewModel> GetOrverviewSurveys(int userId)
        {
            var currentUser = UnitOfWork.User.GetUser(userId);
            var currentUserRoleIds = currentUser.UserPermissions.Select(x => x.RoleId);
            var dealerId = currentUser.DefaultLogin.Value;
            var users = UnitOfWork.User.GetDealerEmployees(dealerId);
            var userIds = users.Select(x => x.UserId).ToList();
            var query = UnitOfWork.Survey.GetAll()
                    .Where(x => x.SurveyStatusId == Constant.Constanst.SurveyStatusIds.Submitted
                             || (x.SurveyStatusId == Constant.Constanst.SurveyStatusIds.Followup
                                    && x.DateStamp <= DateTime.Now));

            if (currentUserRoleIds.Contains(Constanst.RoleType.Employee))
            {
                query = query.Where(s => s.UserId == userId);
            }
            else if (currentUserRoleIds.Contains(Constanst.RoleType.Manager))
            {
                query = query.Where(s => s.ManagerId == userId);
            }
            else
            {
                query = query.Where(s => userIds.Contains(s.UserId));
            }

            if (query.Any())
            {
                var surveys = query.ToList();
                var result = new List<SurveysOverviewViewModel>();
                foreach (var survey in surveys)
                {
                    var surveyOverview = new SurveysOverviewViewModel();
                    surveyOverview.SurveyId = survey.SurveyId;

                    surveyOverview.DateStamp = survey.DateStamp;
                    if (survey.SurveyStatusId == Constanst.SurveyStatusIds.Followup)
                    {
                        if (survey.Communications != null && survey.Communications.Count > 0)
                        {
                            var lastFollowupCommunication = survey.Communications.Where(x => x.CommunicationStatusId == Constanst.CommunicationStatusIds.Followup)
                                                                                 .OrderByDescending(x => x.Datestamp).FirstOrDefault();
                            if (lastFollowupCommunication != null)
                                surveyOverview.DateStamp = lastFollowupCommunication.Datestamp;
                        }
                    }

                    if (survey.Customer != null)
                    {
                        if (survey.Customer.CustomerVehicles != null && survey.Customer.CustomerVehicles.Count > 0)
                        {
                            surveyOverview.Year = survey.Customer.CustomerVehicles.Select(j => j.Year).FirstOrDefault();
                            surveyOverview.Make = survey.Customer.CustomerVehicles.Select(j => j.Make).FirstOrDefault();
                            if (surveyOverview.Make != 0)
                                surveyOverview.MakeName = _vincontrolCommonManagementForm.GetChromeMakeName(surveyOverview.Make);
                            surveyOverview.Model = survey.Customer.CustomerVehicles.Select(j => j.Model).FirstOrDefault();
                            if (surveyOverview.Model != 0)
                                surveyOverview.ModelName = _vincontrolCommonManagementForm.GetChromeModelName(surveyOverview.Model);
                        }

                        surveyOverview.Customer = survey.Customer.FirstName + " " + survey.Customer.LastName;
                        if (survey.Customer.CustomerLevel != null)
                            surveyOverview.Level = survey.Customer.CustomerLevel.Name;
                    }
                    surveyOverview.Rating = survey.Rating;
                    surveyOverview.SurveyStatus = survey.SurveyStatusId;

                    surveyOverview.UserId = survey.UserId;
                    var employee = users.FirstOrDefault(x => x.UserId == survey.UserId);
                    if (employee != null)
                        surveyOverview.UserName = employee.Name;

                    surveyOverview.ManagerId = survey.ManagerId;
                    var manager = users.FirstOrDefault(x => x.UserId == survey.ManagerId);
                    if (manager != null)
                        surveyOverview.ManagerName = manager.Name;

                    result.Add(surveyOverview);
                }

                var today = DateTime.Now.Date;
                result = result.Where(x => x.DateStamp < today.AddDays(1)).OrderByDescending(x => x.DateStamp).ToList();
                return result;
            }
            return new List<SurveysOverviewViewModel>();
        }

        public SurveyTemplateViewModel GetSurveyTemplateStatistic(int templateId)
        {
            var surveyTemplate = UnitOfWork.Survey.GetSurveyTemplate(templateId);
            var model = new SurveyTemplateViewModel
            {
                DepartmentId = surveyTemplate.DepartmentId.GetValueOrDefault(),
                DepartmentName = UnitOfWork.Common.GetDepartment(surveyTemplate.DepartmentId.GetValueOrDefault()).Name,
                SurveyTemplateId = surveyTemplate.SurveyTemplateId,
                SurveyTemplateName = surveyTemplate.Name,
                Questions = new List<SurveyQuestion>(),
            };

            var questions = UnitOfWork.Survey.GetQuestionsByTemplate(templateId).Where(x => x.CustomerAnswers.Count > 0).ToList();
            if (questions != null && questions.Count > 0)
            {
                var query = from question in questions
                            select new
                            {
                                QuestionId = question.SurveyQuestionId,
                                Content = question.Content,
                                Point = question.Point,
                                Order = question.Order,
                                AverageRating = question.CustomerAnswers.Average(x => x.SurveyAnswer.Value)
                            };
                model.Questions = query.Select(x => new SurveyQuestion
                                    {
                                        SurveyQuestionId = x.QuestionId,
                                        Content = x.Content,
                                        Point = x.Point,
                                        Order = x.Order,
                                        AverageRating = x.AverageRating,
                                    }).ToList();

                var highestQuestion = model.Questions.OrderByDescending(x => x.AverageRating).ThenByDescending(x => x.Point).FirstOrDefault();
                var lowestQuestion = model.Questions.OrderBy(x => x.AverageRating).ThenBy(x => x.Point).FirstOrDefault();

                if (highestQuestion != null)
                    model.HighestQuestionId = highestQuestion.SurveyQuestionId;
                if (lowestQuestion != null)
                    model.LowestQuestionId = lowestQuestion.SurveyQuestionId;
            }
            return model;
        }

        public List<SurveyViewModel> GetTodayFollowupSurveys()
        {
            var query = UnitOfWork.Survey.GetTodayFollowupSurveys();
            if (query.Any())
            {
                var surveys = query.ToList()
                                   .OrderByDescending(s => s.DateStamp)
                                   .ThenBy(s => s.Rating)
                                   .ThenBy(s => s.Customer.FirstName)
                                   .ThenBy(s => s.Description)
                                   .Select(s => new SurveyViewModel
                                   {
                                       SurveyId = s.SurveyId,
                                       CustomerInformation = new CustomerInformationViewModel
                                       {
                                           FirstName = s.Customer.FirstName,
                                           LastName = s.Customer.LastName
                                       },
                                       FollowupDate = s.Communications.OrderByDescending(c => c.Datestamp).First().Datestamp,
                                   }).ToList();
                return surveys;
            }
            return new List<SurveyViewModel>();
        }

        #endregion

        #region Private Methods

        public string GenerateReviewWebsitesToString(List<DealerReviewViewModel> list)
        {
            var builder = new StringBuilder();
            foreach (var item in list)
            {
                builder.Append("<a href=\"" + item.Url + "\" style=\"text-decoration:none;margin: 3px 4px;\"><img src=\"" + item.SiteBanner + "\" alt=\"" + item.SiteName + "\" style=\"border:0;width:100px;\" /></a><br/>");
            }

            return builder.ToString();
        }

        private string GetStarString(double value)
        {
            if (value == 1)
                return "one";
            if (value == 2)
                return "two";
            if (value == 3)
                return "three";
            if (value == 4)
                return "four";
            if (value == 5)
                return "five";
            return string.Empty;
        }

        #endregion
    }
}
