using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class InventoryStatisticsService: IInventoryStatisticsService
    {
        private readonly VincontrolUnitOfWork _vincontrolUnitOfWork;

        public InventoryStatisticsService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }
        public SGInventoryStatistic GetInventoryStatistics(int dealershipId)
        {
            return _vincontrolUnitOfWork.InventoryStatisticsRepository.Find(i => i.DealerId == dealershipId).FirstOrDefault();
        }

        public void AddInventoryStatistics(SGInventoryStatistic item)
        {
            _vincontrolUnitOfWork.InventoryStatisticsRepository.Add(item);
            _vincontrolUnitOfWork.Commit();
        }

        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }
    }
}
