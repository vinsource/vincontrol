using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using vincontrol.Crawler.Expressions;

namespace vincontrol.Crawler.Entities
{
    public class AdditionalReviewPage : ObjectBase
    {
        #region XmlElement

        [XmlElement("customerService")]
        public MatchType CustomerService;

        [XmlElement("qualityOfWork")]
        public MatchType QualityOfWork;

        [XmlElement("friendliness")]
        public MatchType Friendliness;

        [XmlElement("overallExperience")]
        public MatchType OverallExperience;

        [XmlElement("pricing")]
        public MatchType Pricing;

        [XmlElement("buyingProcess")]
        public MatchType BuyingProcess;

        [XmlElement("overallFacilities")]
        public MatchType OverallFacilities;

        #endregion
    }
}
