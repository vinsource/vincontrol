using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Backend.Model
{
    public class DealerSetting
    {
        public string DealerPassword { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateAdded { get; set; }
        public string Lattitude { get; set; }
        public string Longtitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public int DealerGroupID { get; set; }
        public string Email { get; set; }
        public int EmailFormat { get; set; }
        public bool OverideDealerKBBReport { get; set; }
    }
}
