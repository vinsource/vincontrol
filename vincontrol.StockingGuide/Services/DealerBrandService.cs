using System.Linq;
using System.Collections.Generic;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class DealerBrandService: IDealerBrandService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public DealerBrandService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public void UpsertDealerBrand(SGDealerBrand sgDealerBrand)
        {
        }

        public SGDealerBrand GetDealerBrand(string make,string model, int dealerId)
        {
            return _vincontrolUnitOfWork.DealerBrandRepository.Find(
                i => i.Make == make && i.Model == model && i.DealerId == dealerId).FirstOrDefault();
        }

        public IEnumerable<SGDealerBrand> GetDealerBrandByDealerID(int dealerId)
        {
            return _vincontrolUnitOfWork.DealerBrandRepository.Find(
                i => i.DealerId == dealerId).OrderByDescending(x => x.History).ThenByDescending(x => x.Stock).ThenByDescending(x => x.Guide).ThenBy(x => x.Make).ThenBy(x => x.Model).ToList();
        }

        public void AddDealerBrand(SGDealerBrand dealerBrand)
        {
            _vincontrolUnitOfWork.DealerBrandRepository.Add(dealerBrand);
            _vincontrolUnitOfWork.Commit();
        }

        public void AddDealerBrand(List<SGDealerBrand> dealerBrands)
        {
            foreach (var item in dealerBrands)
            {
                _vincontrolUnitOfWork.DealerBrandRepository.Add(item);
            }

            _vincontrolUnitOfWork.Commit();
        }

        public void DeleteDealerBrands(IEnumerable<string> deletedMakeList)
        {
            foreach (var make in deletedMakeList)
            {
                string makeItem = make;
                var makeList = _vincontrolUnitOfWork.DealerBrandRepository.Find(i => i.Make == makeItem);
                foreach (var item in makeList)
                {
                    _vincontrolUnitOfWork.DealerBrandRepository.Remove(item);
                   
                }
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void DeleteDealerBrandsNotInWishlist(IEnumerable<string> deletedMakeList)
        {
            foreach (var make in deletedMakeList)
            {
                string makeItem = make;
                var makeList = _vincontrolUnitOfWork.DealerBrandRepository.Find(i => i.Make == makeItem);
                foreach (var item in makeList)
                {
                    if (!(item.IsWishList ?? false))
                    {
                        _vincontrolUnitOfWork.DealerBrandRepository.Remove(item);
                    }

                }
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void RemoveDealerBrands(List<SGDealerBrand> dealerBrandList)
        {
            foreach (var item in dealerBrandList)
            {
                _vincontrolUnitOfWork.DealerBrandRepository.Remove(item);
            }
            _vincontrolUnitOfWork.Commit();
        }

        public List<MakeModel> GetMakeModelListByDealer(int dealerId)
        {
            return _vincontrolUnitOfWork.DealerBrandRepository.Find(i => i.DealerId == dealerId).Select(i=>new MakeModel(){Make = i.Make,Model=i.Model}).Distinct().ToList();
        }


        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }

        public IQueryable<SGDealerBrand> GetDealerBrandForDealer(int dealerId)
        {
            return _vincontrolUnitOfWork.DealerBrandRepository.Find(i => i.DealerId == dealerId);
        }

        public void UpdateGuide(int id, int guide)
        {
            var obj = _vincontrolUnitOfWork.DealerBrandRepository.Find(x => x.SGDealerBrandId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Guide = guide;
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void UpdateWishList(int id, bool isWishList)
        {
            var obj = _vincontrolUnitOfWork.DealerBrandRepository.Find(x => x.SGDealerBrandId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.IsWishList = isWishList;
                _vincontrolUnitOfWork.Commit();
            }
        }
        public void RemoveFromWishList(int id)
        {
            var obj = _vincontrolUnitOfWork.DealerBrandRepository.Find(x => x.SGDealerBrandId == id).FirstOrDefault();
            if (obj != null)
            {
               _vincontrolUnitOfWork.DealerBrandRepository.Remove(obj);
                _vincontrolUnitOfWork.Commit();
            }
        }

        public void UpdateGrossPerUnit(int id, int grossPerUnit)
        {
            var obj = _vincontrolUnitOfWork.DealerBrandRepository.Find(x => x.SGDealerBrandId == id).FirstOrDefault();
            if (obj != null)
            {
                obj.GrossPerUnit = grossPerUnit;
                _vincontrolUnitOfWork.Commit();
            }
        }
    }
}
