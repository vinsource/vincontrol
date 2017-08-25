using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class AppraisalService : IAppraisalService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public AppraisalService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public int GetCurrentMonthUsedStock(int dealerId)
        {
            return _vincontrolUnitOfWork.InventoryRepository.Find(
                i => i.DealerId == dealerId && i.InventoryStatusCodeId == Constants.InventoryStatus.Inventory && i.Condition == Constants.ConditionStatus.Used).Count();
        }

        public double GetCurrentMonthUsedStockForModel(string make, string model, int dealerId)
        {
            return GetAppraisalWithModel(make, model, dealerId).Count();
        }

        public IQueryable<Appraisal> GetAppraisalWithModel(string make, string model, int dealerId)
        {
            return GetAppraisalWithMake(make, dealerId).Where(i => i.Vehicle.Model == model);

        }

        public IQueryable<Appraisal> GetAppraisalWithMake(string make, int dealerId)
        {
            return GetAppraisalForDealer(dealerId).Where(i => i.Vehicle.Make == make);
        }

        public IQueryable<Appraisal> GetAppraisalForDealer(int dealerId)
        {
            return
                _vincontrolUnitOfWork.AppraisalRepository.Find(x => x.DealerId == dealerId);
        }

    }
}
