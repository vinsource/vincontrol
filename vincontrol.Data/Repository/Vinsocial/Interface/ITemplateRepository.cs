using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface ITemplateRepository
    {
        SurveyTemplate GetTemplate(int id);
        SurveyTemplate GetTemplate(string name);
        SurveyTemplate GetTemplate(string name, int dealerId);
        SurveyTemplate GetTemplate(string name, int surveyTemplateId, int dealerId);
        SurveyQuestion GetQuestion(int questionId);
        IQueryable<SurveyTemplate> GetSurveyTemplates(int dealerId);
        void AddNewSurveyTemplate(SurveyTemplate obj);
        void AddNewSurveyQuestion(SurveyQuestion obj);
        void DeleteSurveyTemplate(int templateId);
        void DeleteSurveyQuestion(int questionId);
        void AddNewScript(Script obj);
        Script GetScript(int id);
        bool CheckScriptExisting(int scriptId, string name, int dealerId);
        IQueryable<Script> GetScripts(int id);
        void DeleteScript(int id);
        IQueryable<Script> GetScripts(int departmentId, int dealerId);
        IQueryable<SurveyTemplate> GetSurveyTemplates(int dealerId, int departmentId);
    }
}
