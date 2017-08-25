using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace vincontrol.Crawler.Expressions
{
    public class ReplaceType
    {
        [XmlAttribute("original")]
        public string Original = string.Empty;

        [XmlAttribute("replace")]
        public string Replace = string.Empty;

        [XmlAttribute("isExpression")]
        public bool IsExpression;

        [XmlAttribute("type")]
        public string Type;
    }
}
