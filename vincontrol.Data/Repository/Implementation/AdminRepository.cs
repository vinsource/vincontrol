using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private VincontrolEntities _context;

        public AdminRepository(VincontrolEntities context)
        {
            _context = context;
        }

        public Setting GetSetting(int dealerId)
        {
            return _context.Settings.FirstOrDefault(i => i.DealerId == dealerId);
        }
    }
}
