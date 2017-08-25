using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Vincontrol.Web.Models
{
    [DataContract]
    public class SelectionData
    {
        [DataMember(Name = "webSource")]
        public string Source { get; set; }
        [DataMember(Name = "dealerType")]
        public string DealerType { get; set; }
        [DataMember(Name = "option")]
        public string Options { get; set; }
        [DataMember(Name = "trims")]
        public List<string> Trims { get; set; }

        public bool IsCertified { get; set; }
    }
}