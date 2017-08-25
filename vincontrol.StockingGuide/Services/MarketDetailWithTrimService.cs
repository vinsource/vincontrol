using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Services
{
    public class MarketDetailWithTrimService : IMarketDetailWithTrimService
    {
        private readonly IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public MarketDetailWithTrimService(IVincontrolUnitOfWork vincontrolUnitOfWork)
        {
            _vincontrolUnitOfWork = vincontrolUnitOfWork;
        }

        //public List<SGMarketDetailWithTrim> GetMarketDetailsForDealer(int dealerId)
        //{
        //    return _vincontrolUnitOfWork.MarketDetailWithTrimRepository.Find(i => i.DealerId == dealerId).ToList();
        //}

        //public void AddMarketDetailWithTrims(List<SGMarketDetailWithTrim> newMarketDetails)
        //{
        //    foreach (var item in newMarketDetails)
        //    {
        //        _vincontrolUnitOfWork.MarketDetailWithTrimRepository.Add(item);
        //    }
        //    _vincontrolUnitOfWork.Commit();
        //}

        public void SaveChanges()
        {
           _vincontrolUnitOfWork.Commit();
        }
    }
}
