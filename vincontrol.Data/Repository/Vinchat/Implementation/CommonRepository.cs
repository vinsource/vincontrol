using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Vinchat.Interface;

namespace vincontrol.Data.Repository.Vinchat.Implementation
{
    public class CommonRepository : ICommonRepository
    {
        private VinChatEntities _context;

        public CommonRepository(VinChatEntities context)
        {
            _context = context;
        }

        #region ICommonRepository' Members

        public user GetUser(string username)
        {
            return _context.users.FirstOrDefault(i => i.username.Equals(username));
        }

        public user GetUser(string username, string password)
        {
            return _context.users.FirstOrDefault(i => i.username.Equals(username) && i.password.Equals(password));
        }

        public IQueryable<rosteruser> GetRosterUsers(string username)
        {
            return _context.rosterusers.Where(i => i.username.Equals(username));
        }
        
        public void AddNewUser(user obj)
        {
            _context.AddTousers(obj);
        }

        public void DeleteUser(user obj)
        {
            _context.DeleteObject(obj);
        }

        public void AddNewRosterUser(rosteruser obj)
        {
            _context.AddTorosterusers(obj);
        }

        public void DeleteRosterUser(rosteruser obj)
        {
            _context.DeleteObject(obj);
        }

        public void UpdateUserPassword(string username, string newPassword)
        {
            var user = GetUser(username);
            if (user != null)
            {
                user.password = newPassword;
            }
        }
        #endregion
    }
}
