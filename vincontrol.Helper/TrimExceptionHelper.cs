using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;

namespace vincontrol.Helper
{
    public class VinsocialExceptionHelper
    {
        public static IEnumerable<ExtendedSelectListItem> FilterMakes(List<ExtendedSelectListItem> makeList)
        {
            return makeList.Where(x => !x.Text.Contains("Tesla") && !x.Text.Contains("Rolls-Royce"));
        }

        public static IEnumerable<ExtendedSelectListItem> FilterTrims(List<ExtendedSelectListItem> trimList)
        {
            var returnList = new List<ExtendedSelectListItem>();

            var hashSet = new HashSet<string>();

            foreach (var tmp in trimList)
            {
                if (tmp.Text.Contains("("))
                    tmp.Text = tmp.Text.Substring(0, tmp.Text.IndexOf("(", System.StringComparison.Ordinal));

                if (!hashSet.Contains(tmp.Text))
                    returnList.Add(tmp);

                hashSet.Add(tmp.Text);


            }

            return returnList.Where(x => !x.Text.Contains("ULEV") && !x.Text.Contains("SULEV") && !x.Text.Contains("South Africa"));
        }
    }
}
