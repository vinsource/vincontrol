using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DataFeed.Model
{
    public class CompanyViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string FtpHost { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public List<DealerViewModel> Dealerships { get; set; }
    }
}
