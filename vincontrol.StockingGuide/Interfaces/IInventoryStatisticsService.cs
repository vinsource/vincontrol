using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IInventoryStatisticsService
    {
        SGInventoryStatistic GetInventoryStatistics(int dealershipId);
        void AddInventoryStatistics(SGInventoryStatistic item);
        void SaveChanges();
    }
}
