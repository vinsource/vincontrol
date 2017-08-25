using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class TemplateRepository : ITemplateRepository
    {
        private VinReviewEntities _context;

        public TemplateRepository(VinReviewEntities context)
        {
            _context = context;
        }

        #region ITemplateRepository' Members

        public SurveyTemplate GetTemplate(int id)
        {
            return _context.SurveyTemplates.Include("SurveyQuestions").FirstOrDefault(i => i.SurveyTemplateId == id);
        }

        public SurveyTemplate GetTemplate(string name)
        {
            return _context.SurveyTemplates.Include("SurveyQuestions").FirstOrDefault(i => i.Name.ToLower() == name.ToLower());
        }

        public SurveyTemplate GetTemplate(string name, int dealerId)
        {
            return _context.SurveyTemplates.Include("SurveyQuestions").FirstOrDefault(i => i.Name.ToLower() == name.ToLower() && i.DealerId == dealerId);
        }

        public SurveyTemplate GetTemplate(string name, int surveyTemplateId, int dealerId)
        {
            return _context.SurveyTemplates.Include("SurveyQuestions").FirstOrDefault(i => i.Name.ToLower() == name.ToLower() && i.DealerId == dealerId && i.SurveyTemplateId != surveyTemplateId);
        }

        public SurveyQuestion GetQuestion(int questionId)
        {
            return _context.SurveyQuestions.FirstOrDefault(i => i.SurveyQuestionId == questionId);
        }

        public IQueryable<SurveyTemplate> GetSurveyTemplates(int dealerId)
        {
            return _context.SurveyTemplates.Include("SurveyQuestions").Where(i => i.DealerId == dealerId);
        }

        public void AddNewSurveyTemplate(SurveyTemplate obj)
        {
            _context.AddToSurveyTemplates(obj);
        }

        public void AddNewSurveyQuestion(SurveyQuestion obj)
        {
            _context.AddToSurveyQuestions(obj);
        }

        public void DeleteSurveyTemplate(int templateId)
        {
            var existingTemplate = GetTemplate(templateId);
            if (existingTemplate != null)
            {
                _context.DeleteObject(existingTemplate);
            }
        }

        public void DeleteSurveyQuestion(int questionId)
        {
            var existingQuestion = GetQuestion(questionId);
            if (existingQuestion != null)
            {
                _context.DeleteObject(existingQuestion);
            }
        }

        public void AddNewScript(Script obj)
        {
            _context.AddToScripts(obj);
        }

        public Script GetScript(int id)
        {
            return _context.Scripts.Include("CommunicationType").FirstOrDefault(x => x.ScriptId == id);
        }

        public IQueryable<Script> GetScripts(int dealerId)
        {
            return _context.Scripts.Include("CommunicationType").Where(x => x.DealerId == dealerId);
        }

        public bool CheckScriptExisting(int scriptId, string name, int dealerId)
        {
            return _context.Scripts.Any(x => x.DealerId == dealerId && x.Name == name && x.ScriptId != scriptId);
        }

        public void DeleteScript(int id)
        {
            var entity = GetScript(id);
            if (entity != null)
                _context.Scripts.DeleteObject(entity);
        }

        public IQueryable<Script> GetScripts(int departmentId, int dealerId)
        {
            return _context.Scripts.Include("CommunicationType").Where(x => x.DepartmentId == departmentId && x.DealerId == dealerId);
        }

        public IQueryable<SurveyTemplate> GetSurveyTemplates(int dealerId, int departmentId)
        {
            return _context.SurveyTemplates.Where(i => i.DealerId == dealerId && i.DepartmentId == departmentId);
        }
        #endregion
    }
}
