using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Data.Repository.Interface
{
    public interface ICommonRepository
    {
        List<ExtendedSelectListItem> GetChromeYear();
        List<ExtendedSelectListItem> GetChromeMake(int year);
        List<ExtendedSelectListItem> GetChromeModel(int year, int makeId);
        List<ExtendedSelectListItem> GetChromeTrim(int year, int makeId, int modelId);
        List<ExtendedSelectListItem> GetTruckTypes();
        List<ExtendedSelectListItem> GetTruckTypes(string selectedTruckType);
        List<ExtendedSelectListItem> GetTruckCategories();
        List<ExtendedSelectListItem> GetTruckCategories(int selectedTruckCategory);
        List<ExtendedSelectListItem> GetTruckCategoriesByType(string truckType);
        List<ExtendedSelectListItem> GetTruckClasses();
        List<ExtendedSelectListItem> GetTruckClasses(int selectedTruckClass);
        string GetChromeMakeName(int makeId);
        string GetChromeModelName(int modelId);
        string GetChromeTrimName(int trimId);
        Department GetDepartment(int departmentId);
        IQueryable<Department> GetAllDepartments();
        BDC GetBDC(int bdcId);
        IQueryable<BDC> GetBDCs();
        IQueryable<State> GetStates();
        bool CheckStateExisting(string name);
        bool CheckCityExisting(string city);
        void AddNewState(State obj);
        void AddNewCity(City obj);
        void AddNewVSRSchedule(VSRSchedule obj);
    }
}
