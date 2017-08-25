using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;



/// <summary>
/// Summary description for XMLHelper
/// </summary>
/// 
namespace Vincontrol.Web.HelperClass
{
    public class XMLHelper
    {
        public XMLHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static XmlNodeList selectElements(string pathToElement, string pathToFile)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathToFile);
                String XPathExpression = "//" + pathToElement;
                XmlNodeList nodelist = doc.SelectNodes(XPathExpression);
                if (nodelist.Count > 0)
                    return nodelist;
                return null;

            }
            catch (System.Exception ex)
            {
                throw ex;

            }

        }

        public static XmlNode selectFirstElement(string pathToElement, string pathToFile)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathToFile);
                String XPathExpression = "//" + pathToElement;
                XmlNodeList nodelist = doc.SelectNodes(XPathExpression);
                if (nodelist.Count > 0)
                    return nodelist.Item(0);
                return null;

            }
            catch (System.Exception ex)
            {
                throw ex;

            }

        }

        public static XmlNodeList selectElements(string pathToElement, string pathToFile, string condition)
        {

            try
            {
                String XPathExpression;
                if (condition.Contains("&") || condition.Contains("||"))
                {
                    string[] paramlist = new string[2];
                    paramlist[0] = "&"; paramlist[1] = "||";

                    string[] splitString;

                    splitString = condition.Split(paramlist, StringSplitOptions.RemoveEmptyEntries);

                    StringBuilder builder = new StringBuilder();
                    builder.Append("[");
                    foreach (string temp in splitString)
                    {
                        int index = temp.Trim().IndexOf('=');
                        string value = temp.Trim().Substring(index + 1);
                        string atrribute = temp.Trim().Substring(0, index);
                        builder.Append("@" + atrribute.Trim() + "=" + "\"" + value.Trim() + "\"");
                        builder.Append(" and ");
                    }
                    builder.Append("]");
                    int lastEnd = builder.ToString().LastIndexOf(" and ");
                    XPathExpression = pathToElement + builder.ToString().Remove(lastEnd, 5);

                }
                else
                {
                    int index = condition.Trim().IndexOf('=');
                    string value = condition.Trim().Substring(index + 1);
                    string atrribute = condition.Trim().Substring(0, index);

                    XPathExpression = pathToElement + "[translate" + "(@" + atrribute.Trim() + ",'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')" + "=" + "\"" + value.ToLower().Trim() + "\"" + "]";

                }
                return selectElements(XPathExpression, pathToFile);
            }
            catch (System.Exception ex)
            {
                throw ex;

            }

        }

        public static XmlNode selectOneElement(string pathToElement, string pathToFile, string condition)
        {
            try
            {
                XmlNodeList nodelist = selectElements(pathToElement, pathToFile, condition);
                if (nodelist == null)
                    return null;
                else
                {
                    if (nodelist.Count == 1)
                        return nodelist.Item(0);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
        }

        public static string getAttributeValue(XmlNode node, string attributeName)
        {
            try
            {
                foreach (XmlAttribute att in node.Attributes)
                {
                    if (att.Name.ToLower().Equals(attributeName.ToLower()))
                        return att.Value;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
        }



    }
}
