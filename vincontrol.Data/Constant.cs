using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Data
{
    public class Constant
    {
        public static DateTime CurrentDate
        {
            get
            {
                return DateTime.Now.Date;
            }
        }

        public class Role
        {
            public const string King = "King";
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string Employee = "Employee";
        }
    }
}
