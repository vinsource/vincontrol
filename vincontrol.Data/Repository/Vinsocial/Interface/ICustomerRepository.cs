using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(int id);
        void UpdateCustomer(int customerId, string notes = null, int? customerLevelId = null);
        void AddNewCommunication(Communication entity);
        Communication GetCommunication(int id);
        IQueryable<Communication> GetCommunications(int surveyId);
        Comment GetLastComment(int surveyId);
        void AddNewComment(Comment entity);
    }
}
