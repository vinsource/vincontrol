using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class ExtendedUserRepository : IExtendedUserRepository
    {
        private VincontrolEntities _context;

        public ExtendedUserRepository(VincontrolEntities context)
        {
            _context = context;
        }
        
        #region IExtendedUserRepository Members

        public IList<User> GetAll()
        {
            return _context.Users.ToList();
        }

        #endregion
    }
}
