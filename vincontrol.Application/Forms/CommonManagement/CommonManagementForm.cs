using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;

namespace vincontrol.Application.Forms.CommonManagement
{
    public class CommonManagementForm : BaseForm, ICommonManagementForm
    {
        #region Constructors
        public CommonManagementForm() : this(new SqlUnitOfWork()) { }

        public CommonManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ICommonManagementForm Members

        public List<ExtendedSelectListItem> GetChromeYear()
        {
            return UnitOfWork.Common.GetChromeYear();
        }

        public List<ExtendedSelectListItem> GetChromeMake(int year)
        {
            return UnitOfWork.Common.GetChromeMake(year);
        }

        public List<ExtendedSelectListItem> GetChromeModel(int year, int makeId)
        {
            return UnitOfWork.Common.GetChromeModel(year, makeId);
        }

        public List<ExtendedSelectListItem> GetChromeTrim(int year, int makeId, int modelId)
        {
            return UnitOfWork.Common.GetChromeTrim(year, makeId, modelId);
        }

        public List<ExtendedSelectListItem> GetTruckTypes()
        {
            return UnitOfWork.Common.GetTruckTypes();
        }

        public List<ExtendedSelectListItem> GetTruckTypes(string selectedTruckType)
        {
            return UnitOfWork.Common.GetTruckTypes(selectedTruckType);
        }
        
        public List<ExtendedSelectListItem> GetTruckCategories()
        {
            return UnitOfWork.Common.GetTruckCategories();
        }

        public List<ExtendedSelectListItem> GetTruckCategories(int selectedTruckCategory)
        {
            return UnitOfWork.Common.GetTruckCategories(selectedTruckCategory);
        }

        public List<ExtendedSelectListItem> GetTruckCategoriesByType(string truckType)
        {
            return UnitOfWork.Common.GetTruckCategoriesByType(truckType);
        }

        public List<ExtendedSelectListItem> GetTruckClasses()
        {
            return UnitOfWork.Common.GetTruckClasses();
        }

        public List<ExtendedSelectListItem> GetTruckClasses(int selectedTruckClass)
        {
            return UnitOfWork.Common.GetTruckClasses(selectedTruckClass);
        }

        public string GetChromeMakeName(int makeId)
        {
            return UnitOfWork.Common.GetChromeMakeName(makeId);
        }

        public string GetChromeModelName(int modelId)
        {
            return UnitOfWork.Common.GetChromeModelName(modelId);
        }

        public string GetChromeTrimName(int trimId)
        {
            return UnitOfWork.Common.GetChromeTrimName(trimId);
        }

        public List<StateViewModel> GetStates()
        {
            var states = UnitOfWork.Common.GetStates();
            return states.Any() ? states.AsEnumerable().Select(i => new StateViewModel(i)).ToList() : new List<StateViewModel>();
        }

        public int AddNewState(string name)
        {
            if (UnitOfWork.Common.CheckStateExisting(name)) return 0;
            var newState = new State() {Name = name};
            UnitOfWork.Common.AddNewState(newState);
            UnitOfWork.CommitVincontrolModel();
            return newState.StateId;
        }

        public int AddNewCity(int stateId, string name, string url)
        {
            if (UnitOfWork.Common.CheckCityExisting(name)) return 0;
            var newCity = new City() {StateId = stateId, Name = name, Url = url};
            UnitOfWork.Common.AddNewCity(newCity);
            UnitOfWork.CommitVincontrolModel();
            return newCity.CityId;
        }

        public void AddNewVSRSchedule(VSRScheduleViewModel obj)
        {
            UnitOfWork.Common.AddNewVSRSchedule(MappingHandler.ToEntity(obj));
            UnitOfWork.CommitVincontrolModel();
        }

        #endregion
    }
}
