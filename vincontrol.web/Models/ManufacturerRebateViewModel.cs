using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Vincontrol.Web.Models
{
    [DataContract]
    public class ManufacturerRebateViewModel
    {
        [DataMember(Name = "Year")]
        public int Year { get; set; }
        [DataMember(Name = "MakeList")]
        public List<string> MakeList { get; set; }
        [DataMember(Name = "ModelList")]
        public List<string> ModelList { get; set; }

        [DataMember(Name = "TrimList")]
        public List<string> TrimList { get; set; }

        [DataMember(Name = "BodyTypeList")]
        public List<string> BodyTypeList { get; set; }
    }

    public class ManafacturerRebateDistinctModel
    {
        public int UniqueId { get; set; }

        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Trim { get; set; }

        public string BodyType { get; set; }

        public string Disclaimer { get; set; }

        public string RebateAmount { get; set; }

        public DateTime ExpirationDate { get; set; }

        public DateTime CreateDate { get; set; } 

    }

}
