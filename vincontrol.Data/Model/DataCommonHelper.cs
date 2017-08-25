using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace vincontrol.Data.Model
{
    public sealed class DataCommonHelper
    {


        public static string RemoveSpecialCharactersWithSpace(string input)
        {
            string tmp = "";

            if (!String.IsNullOrEmpty(input) && input.Length < 45)

                tmp = Regex.Replace(input, @"[\r\n\b\t\x00\%\x1a\\'""]", @"", RegexOptions.Compiled);

            return tmp;
        }

        public static DateTime GetChicagoDateTime(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(-8);
        }
    }


}

