using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.Forms;
using vincontrol.Application.Vinsocial.Forms.CommonManagement;
using vincontrol.Application.Vinsocial.Forms.SurveyManagement;
using vincontrol.Application.Vinsocial.ViewModels.CustomerManagement;
using vincontrol.ConfigurationManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using vincontrol.EmailHelper;

namespace vincontrol.Application.Vinsocial.Forms.CustomerManagement
{
    public class CustomerManagementForm : BaseForm, ICustomerManagementForm
    {
        #region Members
        private IEmail _emailHelper;
        private ISurveyManagementForm _surveyManagementForm;
        private ICommonManagementForm _commonManagementForm;
        #endregion

        #region Ctors
        public CustomerManagementForm()
            : this(new SqlUnitOfWork())
        {
            _surveyManagementForm = new SurveyManagementForm();
            _commonManagementForm = new CommonManagementForm();
            _emailHelper = new Email();
        }

        public CustomerManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region Customer Management Implementation
        public CustomerProfileViewModel GetCustomerProfile(int surveyId)
        {
            var surveyModel = _surveyManagementForm.GetSurveyDetails(surveyId);
            if (surveyModel != null)
            {
                var customer = UnitOfWork.Customer.GetCustomer(surveyModel.CustomerInformationId);
                var survey = UnitOfWork.Survey.GetSurvey(surveyId);
                var model = new CustomerProfileViewModel(survey) { Survey = surveyModel };
                model.CustomerVehicle.TrimName = UnitOfWork.Common.GetChromeTrimName(model.CustomerVehicle.Trim);
                model.CustomerVehicle.ModelName = UnitOfWork.Common.GetChromeModelName(model.CustomerVehicle.Model);
                model.CustomerVehicle.MakeName = UnitOfWork.Common.GetChromeMakeName(model.CustomerVehicle.Make);

                var cachedUsers = new List<User>();
                foreach (var item in model.Communications)
                {
                    var user = GetUser(item.UserId, cachedUsers);
                    if (user != null)
                        item.EmployeeName = user.Name;
                    var manager = GetUser(item.ManagerId, cachedUsers);
                    if (manager != null)
                        item.ManagerName = manager.Name;
                    var createdUser = GetUser(item.CreatedBy, cachedUsers);
                    if (createdUser != null)
                        item.CreatedUserName = createdUser.Name;
                }

                foreach (var item in model.Comments)
                {
                    var user = GetUser(item.UserId, cachedUsers);
                    if (user != null)
                        item.UserName = user.Name;
                }

                if (surveyModel.SurveyStatusId == vincontrol.Constant.Constanst.SurveyStatusIds.Followup)
                    model.SurveyType = "Follow-up surveys";
                else if (surveyModel.Rating >= 4)
                    model.SurveyType = "Good surveys";
                else
                    model.SurveyType = "Bad surveys";


                return model;
            }
            return null;
        }

        public void UpdateCustomer(int customerId, string notes = null, int? customerLevelId = null)
        {
            UnitOfWork.Customer.UpdateCustomer(customerId, notes, customerLevelId);
            UnitOfWork.CommitVinreviewModel();
        }

        public CustomerInformationViewModel GetCustomerInfo(int customerId)
        {
            var existingCustomer = UnitOfWork.Customer.GetCustomer(customerId);
            if (existingCustomer != null)
                return new CustomerInformationViewModel(existingCustomer);
            return new CustomerInformationViewModel();
        }

        public CustomerCommunication PrepareCommunication(int customerId, int surveyId, int communicationId)
        {
            User currentUser, currentManager;
            CustomerCommunication model;
            if (communicationId == 0)
            {
                var customer = UnitOfWork.Customer.GetCustomer(customerId);
                model = new CustomerCommunication(customer);
                var survey = UnitOfWork.Survey.GetSurvey(surveyId);
                currentUser = UnitOfWork.User.GetUser(survey.UserId);
                currentManager = UnitOfWork.User.GetUser(survey.ManagerId);
            }
            else
            {
                model = GetCommunication(communicationId);
                currentUser = UnitOfWork.User.GetUser(model.UserId);
                currentManager = UnitOfWork.User.GetUser(model.ManagerId);
            }

            if (currentUser != null)
                model.EmployeeName = currentUser.Name;
            if (currentManager != null)
                model.ManagerName = currentManager.Name;
            model.CustomerVehicle.TrimName = UnitOfWork.Common.GetChromeTrimName(model.CustomerVehicle.Trim);
            model.CustomerVehicle.ModelName = UnitOfWork.Common.GetChromeModelName(model.CustomerVehicle.Model);
            model.CustomerVehicle.MakeName = UnitOfWork.Common.GetChromeMakeName(model.CustomerVehicle.Make);
            return model;
        }

        public CustomerCommunication SaveCustomerCommunication(CustomerCommunication model)
        {
            int id = model.CommunicationId;
            var survey = UnitOfWork.Survey.GetSurvey(model.SurveyId);
            model.UserId = survey.UserId;
            model.ManagerId = survey.ManagerId;
            if (model.CommunicationId == 0)//insert
            {
                var communication = model.ToEntity();
                if (model.CommunicationStatusId == vincontrol.Constant.Constanst.CommunicationStatusIds.Followup)
                {
                    if (model.Date != null && model.Time != null)
                        communication.Datestamp = new DateTime(model.Date.Value.Year, model.Date.Value.Month, model.Date.Value.Day, model.Time.Value.Hour, model.Time.Value.Minute, model.Time.Value.Second);
                }
                else
                {
                    communication.Datestamp = DateTime.Now;
                }

                var statusCommunication = model.ToEntity();
                statusCommunication.Datestamp = DateTime.Now;

                switch (communication.CommunicationStatusId)
                {
                    case vincontrol.Constant.Constanst.CommunicationStatusIds.Resolved:
                        survey.SurveyStatusId = vincontrol.Constant.Constanst.SurveyStatusIds.Resolved;
                        statusCommunication.CommunicationTypeId = vincontrol.Constant.Constanst.CommunicationType.Resolve;
                        statusCommunication.Notes = "Status update - Resolve";
                        break;
                    case vincontrol.Constant.Constanst.CommunicationStatusIds.Closed:
                        survey.SurveyStatusId = vincontrol.Constant.Constanst.SurveyStatusIds.Closed;
                        statusCommunication.CommunicationTypeId = vincontrol.Constant.Constanst.CommunicationType.Close;
                        statusCommunication.Notes = "Status update - Close";
                        break;
                    case vincontrol.Constant.Constanst.CommunicationStatusIds.Followup:
                        survey.SurveyStatusId = vincontrol.Constant.Constanst.SurveyStatusIds.Followup;
                        statusCommunication.CommunicationTypeId = vincontrol.Constant.Constanst.CommunicationType.Followup;
                        statusCommunication.Notes = "Status update - Followup";
                        break;
                }
                UnitOfWork.Customer.AddNewCommunication(communication);
                UnitOfWork.Customer.AddNewCommunication(statusCommunication);
                UnitOfWork.CommitVinreviewModel();
                id = communication.CommunicationId;
            }
            else//update
            {
                var entity = UnitOfWork.Customer.GetCommunication(model.CommunicationId);
                if (entity != null)
                {
                    model.ToEntity(entity);
                    UnitOfWork.CommitVinreviewModel();
                }
            }

            return GetCommunication(id);
        }

        public CustomerCommunication GetCommunication(int id)
        {
            var entity = UnitOfWork.Customer.GetCommunication(id);
            if (entity != null)
            {
                var model = new CustomerCommunication(entity);
                var user = UnitOfWork.User.GetUser(model.UserId);
                var manager = UnitOfWork.User.GetUser(model.ManagerId);
                if (user != null)
                    model.EmployeeName = user.Name;
                if (manager != null)
                    model.ManagerName = manager.Name;

                return model;
            }
            return new CustomerCommunication();
        }

        public void SaveCustomerComment(int surveyId, int userId, string commentText)
        {
            var comment = new Comment
            {
                SurveyId = surveyId,
                UserId = userId,
                Text = commentText,
                DateStamp = DateTime.Now,
            };

            UnitOfWork.Customer.AddNewComment(comment);
            UnitOfWork.CommitVinreviewModel();
        }

        public void LogRequestCommunication(int requestUserId, int receiverId, int surveyId)
        {
            var requestUser = UnitOfWork.User.GetUser(requestUserId);
            var receiver = UnitOfWork.User.GetUser(receiverId);

            var entity = new Communication
            {
                SurveyId = surveyId,
                UserId = requestUserId,
                CommunicationTypeId = Constant.Constanst.CommunicationType.Request,
                Notes = string.Format("Request sent to {0}", receiver.Name),
                Datestamp = DateTime.Now,
            };

            UnitOfWork.Customer.AddNewCommunication(entity);
            UnitOfWork.CommitVinreviewModel();
        }

        public void SendCommunicationEmail(int requestUserId, int receiverId, int surveyId)
        {
            var requestUser = UnitOfWork.User.GetUser(requestUserId);
            var receiver = UnitOfWork.User.GetUser(receiverId);
            var dealer = UnitOfWork.Dealer.GetDealerById(requestUser.DefaultLogin.GetValueOrDefault());
            var survey = UnitOfWork.Survey.GetSurvey(surveyId);
            var customer = UnitOfWork.Customer.GetCustomer(survey.CustomerId);

            var emailContent = EmailTemplateReader.GetCommunicationEmailContent();
            emailContent = emailContent.Replace(EmailTemplateReader.UserFullName, requestUser.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.ReceiverName, receiver.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.DealerName, dealer.Name);
            emailContent = emailContent.Replace(EmailTemplateReader.CustomerName, string.Format("{0} {1}", customer.FirstName, customer.LastName));
            emailContent = emailContent.Replace(EmailTemplateReader.CommunicationUrl,
                                                String.Format(
                                                    "http://{0}/csi/viewcommunicationhistory?survey={1}",
                                                    ConfigurationHandler.VINSocial, EncryptString(surveyId.ToString())));

            var emailSubject = string.Format("VINSocial request from {0} regarding {1}'s case", requestUser.Name, string.Format("{0} {1}", customer.FirstName, customer.LastName));
            _emailHelper.SendEmail(new List<string> { receiver.Email }, emailSubject, emailContent);
        }

        public List<CustomerCommunication> GetCommunications(int surveyId)
        {
            var list = UnitOfWork.Customer.GetCommunications(surveyId).ToList();
            var result = list.Any() ? list.AsEnumerable().Select(x => new CustomerCommunication(x)).ToList() : new List<CustomerCommunication>();

            var cachedUsers = new List<User>();
            foreach (var item in result)
            {
                var user = GetUser(item.UserId, cachedUsers);
                if (user != null)
                    item.EmployeeName = user.Name;
                var manager = GetUser(item.ManagerId, cachedUsers);
                if (manager != null)
                    item.ManagerName = manager.Name;
                var createdUser = GetUser(item.CreatedBy, cachedUsers);
                if (createdUser != null)
                    item.CreatedUserName = createdUser.Name;
            }
            result = result.OrderByDescending(x => x.Datestamp).ToList();
            return result;
        }

        #endregion

        #region Private Methods

        private User GetUser(int userId, List<User> cachedUsers)
        {
            var user = cachedUsers.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                user = UnitOfWork.User.GetUser(userId);
                if (user != null)
                    cachedUsers.Add(user);
            }
            return user;
        }

        #endregion
    }
}
