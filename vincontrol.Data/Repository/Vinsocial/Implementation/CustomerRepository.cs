using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        #region Members
        private VinReviewEntities _context;
        private VincontrolEntities _vincontrol;
        #endregion

        #region Ctor
        public CustomerRepository(VinReviewEntities context)
        {
            _context = context;
            _vincontrol = new VincontrolEntities();
        }
        #endregion

        #region Customer Repository Implementation
        public Customer GetCustomer(int id)
        {
            return _context.Customers.Include("CustomerVehicles")
                                     .FirstOrDefault(c => c.CustomerId == id);
        }

        public void UpdateCustomer(int customerId, string notes = null, int? customerLevelId = null)
        {
            var existingCutomer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (existingCutomer != null)
            {
                if (notes != null)
                    existingCutomer.Notes = notes;
                if (customerLevelId != null)
                {
                    if (customerLevelId != 0)
                        existingCutomer.CustomerLevelId = customerLevelId.Value;
                    else
                        existingCutomer.CustomerLevelId = null;
                }
            }
        }

        public void AddNewCommunication(Communication entity)
        {
            _context.AddToCommunications(entity);
        }

        public Communication GetCommunication(int id)
        {
            return _context.Communications.FirstOrDefault(x => x.CommunicationId == id);
        }

        public Comment GetLastComment(int surveyId)
        {
            return _context.Comments.OrderByDescending(x => x.DateStamp).FirstOrDefault(x => x.SurveyId == surveyId);
        }

        public void AddNewComment(Comment entity)
        {
            _context.AddToComments(entity);
        }

        public IQueryable<Communication> GetCommunications(int surveyId)
        {
            return _context.Communications.Where(x => x.SurveyId == surveyId);
        }
        #endregion
    }
}
