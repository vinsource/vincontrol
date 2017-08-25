using System.Collections.Generic;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.ViewModels.TemplateManagement
{
    public class ScriptViewModel
    {
        public ScriptViewModel() { }

        public ScriptViewModel(Script obj)
        {
            CommunicationTypeId = obj.CommunicationTypeId;
            DepartmentId = obj.DepartmentId.GetValueOrDefault();
            DealerId = obj.DealerId.GetValueOrDefault();
            ScriptId = obj.ScriptId;
            Name = obj.Name;
            Text = obj.Text;
            if (obj.CommunicationType != null)
                CommunicationTypeName = obj.CommunicationType.Name;
        }

        public int ScriptId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int CommunicationTypeId { get; set; }
        public string CommunicationTypeName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DealerId { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> CommunicationTypes { get; set; }
    }
}
