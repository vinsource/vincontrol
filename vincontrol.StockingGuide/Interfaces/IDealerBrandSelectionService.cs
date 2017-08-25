using vincontrol.Data.Model;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IDealerBrandSelectionService
    {
        void AddDealerBrandSelection(SGDealerBrandSelection dealerBrand);
        SGDealerBrandSelection GetDealerBrandSelection(int dealerID);
    }
}