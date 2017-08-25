using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class EmailWaitingListRepository : IEmailWaitingListRepository
    {
        private VincontrolEntities _context;

        public EmailWaitingListRepository(VincontrolEntities context)
        {
            _context = context;
        }

        public void AddNewEmailWaitingList(EmailWaitingList obj)
        {
            _context.EmailWaitingLists.AddObject(obj);
        }
    }
}
