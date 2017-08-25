using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.CommonManagement
{
    public class ChromeVinStyleCheck
    {
        public bool DecodeSuccess { get; set; }
        public int TrimNumber { get; set; }
        public IEnumerable<ExtendedSelectListItem> TrimList { get; set; }
    }
}
