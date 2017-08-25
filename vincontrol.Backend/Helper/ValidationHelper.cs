using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace vincontrol.Backend.Helper
{
    public static class ValidationHelper
    {
        public static string ValidateEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return "Email should have value";
            if (!Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                return "Please fill in correct email format";
            return null;
        }
    }
}
