using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class DepartmentRepository : IDepartmentRepository   
    {
        #region Members
        
        private VincontrolEntities _vincontrol;
        public DepartmentRepository(VincontrolEntities vincontrol)
        {
            _vincontrol = vincontrol;
        }
        #endregion
        public IQueryable<Department> GetAll()
        {
            return _vincontrol.Departments.AsQueryable();
        }
    }
}
