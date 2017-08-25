using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Application.Vinsocial.ViewModels.TemplateManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.Forms.TemplateManagement
{
    public class TemplateManagementForm : BaseForm, ITemplateManagementForm
    {
        #region Constructors
        public TemplateManagementForm() : this(new SqlUnitOfWork()) { }

        public TemplateManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ITemplateManagementForm' Members

        public List<TemplateViewModel> GetSurveyTemplates(int dealerId)
        {
            var list = UnitOfWork.Template.GetSurveyTemplates(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new TemplateViewModel(i){ DepartmentName = UnitOfWork.Common.GetDepartment(i.DepartmentId.GetValueOrDefault()).Name }).ToList() : new List<TemplateViewModel>();
        }

        public TemplateViewModel GetTemplate(int id)
        {
            var existingTemplate = UnitOfWork.Template.GetTemplate(id);
            return existingTemplate == null ? new TemplateViewModel() : new TemplateViewModel(existingTemplate);
        }

        public bool CheckSurveyTemplateExisting(string name, int dealerId)
        {
            return UnitOfWork.Template.GetTemplate(name, dealerId) != null;
        }

        public bool CheckSurveyTemplateExisting(string name, int surveyTemplateId, int dealerId)
        {
            return UnitOfWork.Template.GetTemplate(name, surveyTemplateId, dealerId) != null;
        }

        public void AddNewSurveyTemplate(TemplateViewModel model)
        {
            var newSurveyTemplate = MappingHandler.ConvertViewModelToSurveyTemplate(model);
            UnitOfWork.Template.AddNewSurveyTemplate(newSurveyTemplate);
            UnitOfWork.CommitVinreviewModel();

            if (model.QuestionList.Any())
            {
                foreach (var question in model.QuestionList)
                {
                    question.SurveyTemplateId = newSurveyTemplate.SurveyTemplateId;
                    UnitOfWork.Template.AddNewSurveyQuestion(MappingHandler.ConvertViewModelToSurveyQuestion(question));
                }
                UnitOfWork.CommitVinreviewModel();
            }
        }

        public void UpdateSurveyTemplate(TemplateViewModel model)
        {
            var surveyTemplate = UnitOfWork.Template.GetTemplate(model.TemplateId);
            if (surveyTemplate != null)
            {
                surveyTemplate.Name = model.Name;
                surveyTemplate.EmailContent = model.EmailContent;
                surveyTemplate.DepartmentId = model.DepartmentId;
                surveyTemplate.DateStamp = DateTime.Now;
                surveyTemplate.TotalPoints = model.TotalPoints;
                UnitOfWork.CommitVinreviewModel();
                if (model.QuestionList.Any())
                {
                    foreach (var question in model.QuestionList)
                    {
                        if (question.SurveyQuestionId.Equals(0))
                        {
                            question.SurveyTemplateId = surveyTemplate.SurveyTemplateId;
                            UnitOfWork.Template.AddNewSurveyQuestion(MappingHandler.ConvertViewModelToSurveyQuestion(question));
                        }
                        else
                        {
                            var existingQuestion = UnitOfWork.Template.GetQuestion(question.SurveyQuestionId);
                            if (existingQuestion != null)
                            {
                                existingQuestion.Content = question.Content;
                                existingQuestion.Point = question.Point;
                                existingQuestion.Order = question.Order;
                                existingQuestion.DateStamp = DateTime.Now;
                            }
                        }
                    }
                    UnitOfWork.CommitVinreviewModel();
                }
            }
        }

        public void DeleteSurveyTemplate(int templateId)
        {
            var surveys = UnitOfWork.Survey.GetSurveysByTemplate(templateId);
            if (surveys.Any())
            {
                foreach (var survey in surveys)
                {
                    var customerAnswers = UnitOfWork.Survey.GetCustomerAnswers(survey.SurveyId);
                    if (customerAnswers.Any())
                    {
                        foreach (var customerAnswer in customerAnswers)
                        {
                            UnitOfWork.Survey.DeleteCustomerAnswer(customerAnswer);
                        }
                    }
                    UnitOfWork.Survey.DeleteSurvey(survey);
                    UnitOfWork.CommitVinreviewModel();
                }
            }

            UnitOfWork.Template.DeleteSurveyTemplate(templateId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void DeleteSurveyQuestion(int questionId)
        {
            var customerAnswers = UnitOfWork.Survey.GetCustomerAnswersByQuestion(questionId);
            if (customerAnswers.Any())
            {
                foreach (var customerAnswer in customerAnswers)
                {
                    UnitOfWork.Survey.DeleteCustomerAnswer(customerAnswer);
                }
                UnitOfWork.CommitVinreviewModel();
            }

            UnitOfWork.Template.DeleteSurveyQuestion(questionId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void SaveScript(ScriptViewModel model)
        {
            if (model.ScriptId == 0)//insert new script
            {
                var newScript = model.ToEntity();
                UnitOfWork.Template.AddNewScript(newScript);
            }
            else//update script
            {
                var entity = UnitOfWork.Template.GetScript(model.ScriptId);
                model.ToEntity(entity);
            }

            UnitOfWork.CommitVinreviewModel();
        }

        public ScriptViewModel GetScript(int id)
        {
            var entity = UnitOfWork.Template.GetScript(id);
            return new ScriptViewModel(entity);
        }

        public List<ScriptViewModel> GetScripts(int dealerId)
        {
            var list = UnitOfWork.Template.GetScripts(dealerId);
            return list.Any() ? list.AsEnumerable()
                                    .Select(i => new ScriptViewModel(i) 
                                    { 
                                        DepartmentName = UnitOfWork.Common.GetDepartment(i.DepartmentId.GetValueOrDefault()).Name 
                                    }).ToList() 
                              : new List<ScriptViewModel>();
        }

        public bool CheckScriptExisting(int scriptId, string name, int dealerId)
        {
            return UnitOfWork.Template.CheckScriptExisting(scriptId, name, dealerId);
        }

        public void DeleteScript(int id)
        {
            UnitOfWork.Template.DeleteScript(id);
            UnitOfWork.CommitVinreviewModel();
        }

        public List<ScriptViewModel> GetScripts(int departmentId, int dealerId)
        {
            var list = UnitOfWork.Template.GetScripts(departmentId, dealerId);
            return list.Any() ? list.AsEnumerable()
                                    .Select(i => new ScriptViewModel(i)
                                    {
                                        DepartmentName = UnitOfWork.Common.GetDepartment(i.DepartmentId.GetValueOrDefault()).Name
                                    }).ToList()
                              : new List<ScriptViewModel>();
        }

        public List<SelectListItem> GetSurveyTemplates(int dealerId, int departmentId)
        {
            var list = UnitOfWork.Template.GetSurveyTemplates(dealerId, departmentId);
            return list.Any() ? list.AsEnumerable().Select(i => new SelectListItem 
            { 
                Text = i.Name,
                Value = i.SurveyTemplateId.ToString(),
            }).ToList() : new List<SelectListItem>();
        }
        #endregion
    }
}
