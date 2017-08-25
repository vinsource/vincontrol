using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.Vinsocial.ViewModels.DepartmentManagement
{
    public class DepartmentViewModel
    {
        
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Goal { get; set; }
    }
}
