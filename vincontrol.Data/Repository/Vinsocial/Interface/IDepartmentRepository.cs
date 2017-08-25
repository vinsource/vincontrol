using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Vinsocial.Interface
{
    public interface IDepartmentRepository
    {
        IQueryable<Department> GetAll();
    }
}
