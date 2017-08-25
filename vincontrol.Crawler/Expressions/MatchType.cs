using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace vincontrol.Crawler.Expressions
{
    public class MatchType
    {
        [XmlArray("replace")]
        [XmlArrayItem("filter", typeof(ReplaceType))]
        public ArrayList Replaces = new ArrayList();

        [XmlAttribute("context")]
        public string Context = string.Empty;

        [XmlAttribute("pattern")]
        public string Pattern = string.Empty;

        [XmlAttribute("ignoreCase")]
        public bool IgnoreCase = true;

        public ArrayList ParseData(XmlNode node)
        {
            ArrayList list = ParseByContext(node);
            //will parse by pattern if the context string is empty
            if (list.Count == 0)
                list.Add(node.InnerXml);
            list = ParseByPattern(list);
            return list;
        }

        public string GetString(XmlNode mainNode)
        {
            return GetString(mainNode, null);
        }

        public string GetString(XmlNode mainNode, XmlNamespaceManager nsManager)
        {
            if (mainNode == null || string.IsNullOrEmpty(mainNode.InnerText) || string.IsNullOrEmpty(Context))
                return "";

            string ret;

            if (Context != string.Empty)
            {
                XmlNode node;
                if (nsManager == null)
                    node = mainNode.SelectSingleNode(Context);
                else
                    node = mainNode.SelectSingleNode(Context, nsManager);
                if (node != null)
                {
                    if (node is XmlElement)
                        ret = node.InnerXml;
                    else
                        ret = node.Value;
                }
                else
                    ret = string.Empty;
            }
            else
                ret = mainNode.InnerXml;

            RegexOptions flag = RegexOptions.None;
            if (IgnoreCase)
                flag |= RegexOptions.IgnoreCase;

            if (Pattern != string.Empty)
            {
                var regex = new Regex(Pattern, flag);
                Match match = regex.Match(ret.Replace("\r", "").Replace("\n", ""));
                ret = match.Groups.Count > 0 ? match.Groups[1].Value : string.Empty;
            }

            if (Replaces != null)
            {
                string replaceString;
                foreach (ReplaceType replaceType in Replaces)
                {
                    replaceString = replaceType.Replace;
                    //execute C# code to replace
                    if (replaceType.IsExpression)
                        replaceString = DynamicExpression.EvaluateWithStringResult(replaceType.Replace);

                    ret = Regex.Replace(ret, replaceType.Original, replaceString, flag);

                }
            }
            GC.Collect();
            ret = ret.Replace("&amp;", "&");
            return ret.Trim();
        }

        private ArrayList ParseByContext(XmlNode mainNode)
        {
            var list = new ArrayList();
            if (Context == string.Empty)
                return list;

            XmlNodeList nodeList = mainNode.SelectNodes(Context);
            foreach (XmlNode node in nodeList)
            {
                list.Add(node.InnerXml);
            }

            return list;
        }

        private ArrayList ParseByPattern(ArrayList list)
        {
            if (Pattern == string.Empty)
                return list;
            //set flag for Regular Expression
            RegexOptions flag = RegexOptions.None;
            if (IgnoreCase)
                flag |= RegexOptions.IgnoreCase;
            var regex = new Regex(Pattern, flag);
            MatchCollection mc;
            var returnList = new ArrayList();
            foreach (string data in list)
            {
                mc = regex.Matches(data);
                for (int i = 0; i < mc.Count; i++)
                {
                    returnList.Add(mc[i].Groups[1].Value);
                }
            }

            return returnList;
        }

        public string ParseByPattern(string value)
        {
            if (Pattern == string.Empty)
                return value;
            //set flag for Regular Expression
            RegexOptions flag = RegexOptions.None;
            if (IgnoreCase)
                flag |= RegexOptions.IgnoreCase;
            Regex regex = new Regex(Pattern, flag);
            Match match = regex.Match(value);

            return match.Groups[1].Value;
        }
    }
}
