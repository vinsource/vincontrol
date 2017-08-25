using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.Forms;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.Forms.CommonManagement
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

        #region ICommonManagementForm' Members

        public List<SelectListItem> GetAllSites()
        {
            var sites = UnitOfWork.VinsocialCommon.GetAllSites();
            return sites.Any()
                       ? sites.AsEnumerable().Select(i => new SelectListItem(i.SiteId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllDepartments()
        {
            var departments = UnitOfWork.Common.GetAllDepartments();
            return departments.Any()
                       ? departments.AsEnumerable().Select(i => new SelectListItem(i.DepartmentId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllTemplates(int dealerId)
        {
            var templates = UnitOfWork.VinsocialCommon.GetAllTemplates(dealerId);
            return templates.Any()
                       ? templates.AsEnumerable().Select(i => new SelectListItem(i.SurveyTemplateId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItemTemplates> GetAllTemplatesDepartment(int dealerId)
        {
            var templates = UnitOfWork.VinsocialCommon.GetAllTemplates(dealerId);
            return templates.Any()
                       ? templates.AsEnumerable().Select(i => new SelectListItemTemplates(i.SurveyTemplateId.ToString(), i.Name, false,i.DepartmentId??0)).ToList()
                       : new List<SelectListItemTemplates>();
        }

        public List<SelectListItem> GetAllRatings()
        {
            return new List<SelectListItem>()
                {
                    new SelectListItem("5", "5 stars", false),
                    new SelectListItem("4.5", "4.5 stars", false),
                    new SelectListItem("4", "4 stars", false),
                    new SelectListItem("3.5", "3.5 stars", false),
                    new SelectListItem("3", "3 stars", false),
                    new SelectListItem("2.5", "2.5 stars", false),
                    new SelectListItem("2", "2 stars", false),
                    new SelectListItem("1.5", "1.5 stars", false),
                    new SelectListItem("1", "1 stars", false),
                    new SelectListItem("0.5", "0.5 stars", false),
                    new SelectListItem("0", "0 stars", false)
                };
        }

        public List<SelectListItem> GetAllRatingsReduceMode()
        {
            return new List<SelectListItem>()
                {
                    new SelectListItem("5", "5 stars", false),
                    new SelectListItem("4", "4 stars", false),
                    new SelectListItem("3", "3 stars", false),
                    new SelectListItem("2", "2 stars", false),
                    new SelectListItem("1", "1 stars", false),
                    new SelectListItem("0", "0 stars", false)
                };
        }

        public List<SelectListItem> GetBDCs()
        {
            var bdcs = UnitOfWork.Common.GetBDCs();
            return bdcs.Any()
                       ? bdcs.AsEnumerable().Select(i => new SelectListItem(i.BDCId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllStatuses()
        {
            var statuses = UnitOfWork.VinsocialCommon.GetAllStatuses();
            return statuses.Any()
                       ? statuses.AsEnumerable().Select(i => new SelectListItem(i.SurveyStatusId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllCommunicationStatuses()
        {
            var statuses = UnitOfWork.VinsocialCommon.GetAllCommunicationStatuses();
            return statuses.Any()
                       ? statuses.AsEnumerable().Select(i => new SelectListItem(i.CommunicationStatusId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllCommunicationTypes()
        {
            var communicationTypes = UnitOfWork.VinsocialCommon.GetAllCommunicationTypes();
            return communicationTypes.Any()
                       ? communicationTypes.AsEnumerable().Select(i => new SelectListItem(i.CommunicationTypeId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllCustomerLevels()
        {
            var customerLevels = UnitOfWork.VinsocialCommon.GetAllCustomerLevels();
            return customerLevels.Any()
                       ? customerLevels.AsEnumerable().Select(i => new SelectListItem(i.CustomerLevelId, i.Name, false)).ToList()
                       : new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllSurveyPoints()
        {
            return new List<SelectListItem>()
                {
                    new SelectListItem("5", "5pts", false),
                    new SelectListItem("10", "10pts", false),
                    new SelectListItem("15", "15pts", false),
                    new SelectListItem("20", "20pts", false),
                };
        }

        #endregion
    }
}
