using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class CommonRepository : ICommonRepository
    {
        private VinReviewEntities _context;

        public CommonRepository(VinReviewEntities context)
        {
            _context = context;
        }

        #region ICommonRepository' Members

        public IQueryable<SiteReview> GetAllSites()
        {
            return _context.SiteReviews;
        }

        public IQueryable<SurveyTemplate> GetAllTemplates(int dealerId)
        {
            return _context.SurveyTemplates.Where(i => i.DealerId == dealerId);
        }
        
        public Customer GetCustomer(string email, string cellphone)
        {
            return _context.Customers.FirstOrDefault(i => i.Email.ToLower().Equals(email.ToLower()) && i.CellNumber.Equals(cellphone));
        }

        public void AddNewCustomer(Customer obj)
        {
            _context.AddToCustomers(obj);
        }

        public void AddNewCustomerVehicle(CustomerVehicle obj)
        {
            _context.AddToCustomerVehicles(obj);
        }
        
        public void AddLog(string siteName, int dealerReviewId, string message, string stackTrace)
        {
            var newLog = new DailyLogReview()
            {
                SiteName = siteName,
                DealerReviewId = dealerReviewId,
                Message = message,
                StackTrace = stackTrace,
                DateStamp = DateTime.Now
            };
            _context.AddToDailyLogReviews(newLog);
            _context.SaveChanges();
        }

        public void TruncateDailyLog()
        {
            _context.ExecuteStoreCommand("TRUNCATE TABLE [DailyLogReview]");
        }

        public IQueryable<SurveyStatu> GetAllStatuses()
        {
            return _context.SurveyStatus.OrderBy(i => i.Name);
        }

        public IQueryable<CommunicationStatu> GetAllCommunicationStatuses()
        {
            return _context.CommunicationStatus.OrderBy(i => i.Name);
        }

        public IQueryable<CommunicationType> GetAllCommunicationTypes()
        {
            return _context.CommunicationTypes.Where(x => !x.IsSystemType).OrderBy(i => i.CommunicationTypeId);
        }

        public IQueryable<CustomerLevel> GetAllCustomerLevels()
        {
            return _context.CustomerLevels;
        }

        #endregion
    }
}
