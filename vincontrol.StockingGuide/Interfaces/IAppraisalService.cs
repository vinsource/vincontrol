using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IAppraisalService
    {
        int GetCurrentMonthUsedStock(int dealerId);
        double GetCurrentMonthUsedStockForModel(string make, string model, int dealerId);
        IQueryable<Appraisal> GetAppraisalWithModel(string make, string model, int dealerId);
        IQueryable<Appraisal> GetAppraisalWithMake(string make, int dealerId);
        IQueryable<Appraisal> GetAppraisalForDealer(int dealerId);
    }
}