using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace vincontrol.DomainObject
{
    public class DealerUser
    {
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public DealerSetting DealerSetting { get; set; }
        public CookieContainer KbbCookieContainer { get; set; }
        public CookieCollection KbbCookieCollection { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
    }

    public enum RoleType
    {
        Admin, Manager, Employee, Master
    }
}
