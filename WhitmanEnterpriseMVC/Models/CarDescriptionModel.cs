using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class CarDescriptionModel
    {
        public int ListingId { get; set; }

        public List<DescriptionSentenceGroup> DescriptionList { get; set; }

    }


    public class DescriptionSentenceGroup
    {
        public string Title { get; set; }

        public List<DesctiptionSentence> Sentences { get; set; }
     
    }

    public class DesctiptionSentence
    {
        public string DescriptionSentence { get; set; }

        public bool YesNo { get; set; }
    }
}
