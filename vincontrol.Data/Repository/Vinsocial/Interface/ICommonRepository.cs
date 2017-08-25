using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface ICommonRepository
    {
        IQueryable<SiteReview> GetAllSites();
        IQueryable<SurveyTemplate> GetAllTemplates(int dealerId);
        Customer GetCustomer(string email, string cellphone);
        void AddNewCustomer(Customer obj);
        void AddNewCustomerVehicle(CustomerVehicle obj);
        void AddLog(string siteName, int dealerReviewId, string message, string stackTrace);
        void TruncateDailyLog();
        IQueryable<SurveyStatu> GetAllStatuses();
        IQueryable<CommunicationStatu> GetAllCommunicationStatuses();
        IQueryable<CommunicationType> GetAllCommunicationTypes();
        IQueryable<CustomerLevel> GetAllCustomerLevels();
    }
}
