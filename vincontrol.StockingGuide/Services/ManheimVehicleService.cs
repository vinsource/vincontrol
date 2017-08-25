using System;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class ManheimVehicleService:IManheimVehicleService
    {
        private IVinsellUnitOfWork _vinsellUnitOfWork;

        public ManheimVehicleService()
        {
            _vinsellUnitOfWork = new VinsellUnitOfWork();
        }

        public IQueryable<manheim_vehicles> GetManheimWithTrim(string make, string model,string trim)
        {
            return GetManheimWithModel(make, model).Where(i => i.Trim == trim);

        }

        public IQueryable<manheim_vehicles> GetManheimWithModel(string make, string model)
        {
            return GetManheimWithMake(make).Where(i => i.Model == model);

        }

        public IQueryable<manheim_vehicles> GetManheimWithMake(string make)
        {
            int currentYear = DateTime.Now.Year;
            return _vinsellUnitOfWork.ManheimVehicleRepository.Find(x => x.Make == make && x.Year >= currentYear - 4).OrderBy(x=>x.Year).ThenBy(x=>x.Trim);
        }
    }
}
