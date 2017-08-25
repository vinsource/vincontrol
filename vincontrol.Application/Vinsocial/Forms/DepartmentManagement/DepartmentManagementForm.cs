using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Application.Vinsocial.ViewModels.DepartmentManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Vinsocial.Forms.DepartmentManagement
{
    public class DepartmentManagementForm : BaseForm, IDepartmentManagementForm
    {
        public DepartmentManagementForm()
            : this(new SqlUnitOfWork()){}

        public DepartmentManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public List<DepartmentViewModel> GetAll(int dealerId)
        {
            var departments = UnitOfWork.Department.GetAll();

            return departments.ToList().Select(i => new DepartmentViewModel()
                {

                    DepartmentId = i.DepartmentId,
                    Name = i.Name,
                    Goal = GetSurveyGoal(dealerId, i.DepartmentId)
                }).ToList();
        }

        public int GetSurveyGoal(int dealerId, int departmentId)
        {
            var listSurveyGoal = UnitOfWork.Survey.GetAllSurveyGoal();
            var surveyGoal =
                listSurveyGoal.FirstOrDefault(i => i.DealerId == dealerId && i.DepartmentId == departmentId);
            int goal = 100;
            if (surveyGoal != null) goal = surveyGoal.Goal;

            return goal;
        }

    }
}
