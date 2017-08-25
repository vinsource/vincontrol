using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Vinsocial.ViewModels.DepartmentManagement;

namespace vincontrol.Application.Vinsocial.Forms.DepartmentManagement
{
    public interface IDepartmentManagementForm
    {
        List<DepartmentViewModel> GetAll(int DealerId);
    }
}
