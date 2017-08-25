using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using Vincontrol.Web.DatabaseModel;

namespace Vincontrol.Web.HelperClass
{
    public class ChromeHelper
    {
        public static List<ExtendedSelectListItem> GetChromeYear()
        {
            var result = new List<ExtendedSelectListItem> { new ExtendedSelectListItem() { Text = "Select...", Value = "0", Selected = true } };
            using (var _context = new VincontrolEntities())
            {
                var yearList = _context.YearMakes.Select(x => x.Year).Distinct().ToList();
                result.AddRange(yearList.OrderByDescending(y => y)
                    .Select(ym => new ExtendedSelectListItem
                    {
                        Text = ym.ToString(),
                        Value = ym.ToString()
                    })
                    .ToList());

                return result;
            }
        }
    }
}