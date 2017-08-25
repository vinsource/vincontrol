using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class DealerBrandSelectionService: IDealerBrandSelectionService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public DealerBrandSelectionService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public void AddDealerBrandSelection(SGDealerBrandSelection dealerBrandSelection)
        {
            var obj =
                _vincontrolUnitOfWork.DealerBrandSelectionRepository.Find(
                    x => x.DealerId == dealerBrandSelection.DealerId).SingleOrDefault();
            if (obj == null)
            {
                _vincontrolUnitOfWork.DealerBrandSelectionRepository.Add(dealerBrandSelection);
            }
            else
            {
                obj.ModelSelections = dealerBrandSelection.ModelSelections;
            }
            _vincontrolUnitOfWork.Commit();
        }

        public SGDealerBrandSelection GetDealerBrandSelection(int dealerID)
        {
            return _vincontrolUnitOfWork.DealerBrandSelectionRepository.Find(
                x => x.DealerId == dealerID).SingleOrDefault();
        }
    }
}
