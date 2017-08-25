using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace vincontrol.Crawler.Entities
{
    public class Page : ObjectBase
    {
        #region Xml Attributes

        [XmlAttribute("dealerId")]
        public int DealerId;

        [XmlAttribute("url")]
        public string Url = string.Empty;

        [XmlAttribute("hasAdditionalDealerReview")]
        public bool HasAdditionalDealerReview = false;

        [XmlAttribute("hasAdditionalUserReview")]
        public bool HasAdditionalUserReview = false;

        [XmlAttribute("hasDetailUserReview")]
        public bool HasDetailUserReview = false;

        [XmlAttribute("category")]
        public string Category = "Sales";

        public int CategoryId;

        #endregion

        #region Excute methods

        public virtual void Execute(ReviewSite site) { }

        #endregion
    }
}
