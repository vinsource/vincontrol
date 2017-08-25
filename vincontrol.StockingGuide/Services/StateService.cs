using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Services
{
    public class StateService : IStateService
    {
        private readonly IVinsellUnitOfWork _vinsellUnitOfWork;

        public StateService()
        {
            _vinsellUnitOfWork = new VinsellUnitOfWork();
        }
        public List<manheim_states> GetRegionCodeMapping()
        {
            return _vinsellUnitOfWork.StateRepository.FindAll().ToList();
        }
    }
}
