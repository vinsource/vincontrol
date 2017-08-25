using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using vincontrol.DomainObject;

namespace vincontrol.VinTrade.Helpers
{
    public class ExceptionHelper
    {
        public static IEnumerable<ExtendedSelectListItem> FilterMakes(List<ExtendedSelectListItem> makeList)
        {
            return makeList.Where(x => !x.Text.Contains("Tesla")&& !x.Text.Contains("Rolls-Royce") );
        }

        public static IEnumerable<ExtendedSelectListItem> FilterTrims(List<ExtendedSelectListItem> trimList)
        {
            var returnList = new List<ExtendedSelectListItem>();

            var hashSet = new HashSet<string>();

            foreach (var tmp in trimList)
            {
                var options = RegexOptions.None;
                var regex = new Regex(@"[ ]{2,}", options);

                var filterText = "";
                filterText = regex.Replace(tmp.Text, @" ");

                //if (tmp.Text.Contains("("))
                //    tmp.Text = tmp.Text.Substring(0, tmp.Text.IndexOf("(", System.StringComparison.Ordinal));

                if (!hashSet.Contains(filterText))
                    returnList.Add(tmp);

                hashSet.Add(filterText);

                
            }
            return returnList;
            //return returnList.Where(x => !x.Text.Contains("ULEV")&& !x.Text.Contains("SULEV") && !x.Text.Contains("South Africa"));
        }
    }
}