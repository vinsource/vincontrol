using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinsocial.Interface;

namespace vincontrol.Data.Repository.Vinsocial.Implementation
{
    public class ContactReposity : IContactReposity
    {
        private VincontrolEntities _context;
        public ContactReposity(VincontrolEntities context)
        {
            _context = context;
        }
        public void AddNewContact(VPContactInfo contact)
        {
            _context.AddToVPContactInfoes(contact);
            _context.SaveChanges();
        }
    }
}
