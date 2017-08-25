using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class GroupPermissionRepository : IGroupPermissionRepository
    {
        private VincontrolEntities _context;

        public GroupPermissionRepository(VincontrolEntities context)
        {
            _context = context;
        }

        #region IGroupPermissionRepository Members

        public IQueryable<RolePermission> GetAll()
        {
            return _context.RolePermissions;
        }

        #endregion
    }
}
