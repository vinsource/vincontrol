using System.Collections.Generic;
using vincontrol.Application.Vinsocial.ViewModels.CustomerManagement;

namespace vincontrol.Application.Vinsocial.Forms.CustomerManagement
{
    public interface ICustomerManagementForm
    {
        CustomerProfileViewModel GetCustomerProfile(int surveyId);
        void UpdateCustomer(int customerId, string notes = null, int? customerLevelId = null);
        CustomerInformationViewModel GetCustomerInfo(int customerId);
        CustomerCommunication PrepareCommunication(int customerId, int surveyId, int communicationId);
        CustomerCommunication SaveCustomerCommunication(CustomerCommunication model);
        CustomerCommunication GetCommunication(int id);
        List<CustomerCommunication> GetCommunications(int surveyId);
        void SaveCustomerComment(int surveyId, int userId, string commentText);
        void LogRequestCommunication(int requestUserId, int receiverId, int surveyId);
        void SendCommunicationEmail(int requestUserId, int receiverId, int surveyId);
    }
}
