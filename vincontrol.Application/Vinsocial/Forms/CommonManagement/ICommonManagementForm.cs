using System.Collections.Generic;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.Forms.CommonManagement
{
    public interface ICommonManagementForm
    {
        List<SelectListItem> GetAllSites();
        List<SelectListItem> GetAllDepartments();
        List<SelectListItem> GetAllRatings();
        List<SelectListItem> GetAllRatingsReduceMode();
        List<SelectListItem> GetAllTemplates(int dealerId);
        List<SelectListItemTemplates> GetAllTemplatesDepartment(int dealerId);
        List<SelectListItem> GetBDCs();
        List<SelectListItem> GetAllStatuses();
        List<SelectListItem> GetAllCommunicationStatuses();
        List<SelectListItem> GetAllCommunicationTypes();
        List<SelectListItem> GetAllCustomerLevels();
        List<SelectListItem> GetAllSurveyPoints();
    }
}
