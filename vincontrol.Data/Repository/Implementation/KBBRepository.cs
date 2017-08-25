using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class KBBRepository : IKBBRepository
    {
        private VincontrolEntities _context;

        public KBBRepository(VincontrolEntities context)
        {
            _context = context;
        }
    }
}
