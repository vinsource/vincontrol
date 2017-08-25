using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IDealerBrandService
    {
        void UpsertDealerBrand(SGDealerBrand sgDealerBrand);
        SGDealerBrand GetDealerBrand(string make, string model, int dealerId);
        IEnumerable<SGDealerBrand> GetDealerBrandByDealerID(int dealerId);
        void RemoveFromWishList(int id);
        void AddDealerBrand(SGDealerBrand dealerBrand);
        void SaveChanges();
        IQueryable<SGDealerBrand> GetDealerBrandForDealer(int dealerId);

        void UpdateWishList(int id, bool isWishList);
        void UpdateGuide(int id, int guide);
        void UpdateGrossPerUnit(int id, int grossPerUnit);
        void AddDealerBrand(List<SGDealerBrand> dealerBrands);
        void DeleteDealerBrands(IEnumerable<string> deletedMakeList);
        void DeleteDealerBrandsNotInWishlist(IEnumerable<string> deletedMakeList);
        void RemoveDealerBrands(List<SGDealerBrand> dealerBrandList);
        List<MakeModel> GetMakeModelListByDealer(int dealerId);
    }
}