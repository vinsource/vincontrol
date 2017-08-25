using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Application.Vinsocial.ViewModels.ReviewManagement
{
    public class DealerUrlViewModel
    {
        public DealerUrlViewModel()
        {
        }
        public int DealerId { get; set; }
        public string WebSiteUrl { get; set; }
        public string WebSiteLogo { get; set; }
        public string WebSiteBanner { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string Name { get; set; }
    }
}
