using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class ChromeService : IChromeService
    {
        private IVincontrolUnitOfWork _vincontrolUnitOfWork;

        public ChromeService()
        {
            _vincontrolUnitOfWork = new VincontrolUnitOfWork();
        }

        public List<string> GetModels(string make)
        {
            return _vincontrolUnitOfWork.ModelRepository.Find(i => i.YearMake.Make.Value == make).Select(i => i.Value).Distinct().ToList();
        }

        public List<Model> GetModels(int startIndex, int pageSize)
        {
            return _vincontrolUnitOfWork.ModelRepository.FindAll().OrderBy(i=>i.ModelId).Skip(startIndex*pageSize).Take(pageSize).ToList();
        }

        public IQueryable<Model> GetModelByYearMakeModel(string year, string make, string model)
        {
            int yearValue = int.Parse(year);
            return _vincontrolUnitOfWork.ModelRepository.Find(i => i.YearMake.Make.Value == make && i.YearMake.Year == yearValue && i.Value == model);
        }

        public void SaveChanges()
        {
            _vincontrolUnitOfWork.Commit();
        }

        //public short? GetSegmentId(int year, string make, string model)
        //{
        //    var firstOrDefault = _vincontrolUnitOfWork.ModelRepository.Find(i => i.YearMake.Make.Value == make && i.YearMake.Year == year && i.Value == model).Select(i=>i.SGSegmentId).FirstOrDefault();
        //    if (firstOrDefault != null)
        //        return firstOrDefault.Value;
        //    return null;
        //}

        public short? GetSegmentId(string make, string model)
        {
            var firstOrDefault = _vincontrolUnitOfWork.ModelRepository.Find(i => i.YearMake.Make.Value == make && i.Value == model && i.SGSegmentId!=null).Select(i => i.SGSegmentId).FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.Value;
            return null;
        }

        public List<MakeModelSegmentId> GetMakeModelSegmentList()
        {
           return  _vincontrolUnitOfWork.ModelRepository.FindAll()
                .Select(
                    i =>
                        new MakeModelSegmentId()
                        {
                            Make = i.YearMake.Make.Value,
                            Model = i.Value,
                            SegmentId = i.SGSegmentId
                        }).ToList();
        }

        public IQueryable<Model> GetModelByMakeModel(string make, string model)
        {
            return _vincontrolUnitOfWork.ModelRepository.Find(i => i.YearMake.Make.Value == make && i.Value == model);
        }

        public List<MakeModelTrim> GetMakeModelTrimList()
        {
            return
                _vincontrolUnitOfWork.TrimRepository.FindAll()
                    .Select(
                        i =>
                            new MakeModelTrim
                            {
                                Trim = i.TrimName,
                                Model = i.Model.Value,
                                Make = i.Model.YearMake.Make.Value
                            }).ToList();
        }
    }
}
