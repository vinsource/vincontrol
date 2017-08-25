using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Backend.Model
{
    public class DealerGroup
    {
        public string DealerGroupId { get; set; }
        public string DealerGroupName { get; set; }
        public DateTime DateAdded { get; set; }
        public string MasterUserName { get; set; }
        public string MasterLogin { get; set; }
    }
}
