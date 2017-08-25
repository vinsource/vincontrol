using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Vinsocial.ViewModels.TemplateManagement;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.Forms.TemplateManagement
{
    public interface ITemplateManagementForm
    {
        List<TemplateViewModel> GetSurveyTemplates(int dealerId);
        TemplateViewModel GetTemplate(int id);
        bool CheckSurveyTemplateExisting(string name, int dealerId);
        bool CheckSurveyTemplateExisting(string name, int surveyTemplateId, int dealerId);
        void AddNewSurveyTemplate(TemplateViewModel model);
        void UpdateSurveyTemplate(TemplateViewModel model);
        void DeleteSurveyTemplate(int templateId);
        void DeleteSurveyQuestion(int questionId);
        void SaveScript(ScriptViewModel model);
        ScriptViewModel GetScript(int id);
        List<ScriptViewModel> GetScripts(int dealerId);
        bool CheckScriptExisting(int scriptId, string name, int dealerId);
        void DeleteScript(int id);
        List<ScriptViewModel> GetScripts(int departmentId, int dealerId);
        List<SelectListItem> GetSurveyTemplates(int dealerId, int departmentId);
    }
}
