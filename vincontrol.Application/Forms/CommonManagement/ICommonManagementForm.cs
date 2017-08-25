using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.DomainObject;

namespace vincontrol.Application.Forms.CommonManagement
{
    public interface ICommonManagementForm
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
        List<StateViewModel> GetStates();
        int AddNewState(string name);
        int AddNewCity(int stateId, string name, string url);
        void AddNewVSRSchedule(VSRScheduleViewModel obj);
    }
}
