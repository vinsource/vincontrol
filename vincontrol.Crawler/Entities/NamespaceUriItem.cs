using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace vincontrol.Crawler.Entities
{
    public class NamespaceUriItem : ObjectBase
    {
        [XmlAttribute("value")]
        public string Value = string.Empty;
    }
}
