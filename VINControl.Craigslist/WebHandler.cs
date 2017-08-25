using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace VINControl.Craigslist
{
    public class WebHandler
    {
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)";
        private const string ContentType = "text/xml;charset=\"utf-8\"";
        private const int ReadWriteTimeOut = 100000;
        private const int TimeOut = 100000;

        public static string DownloadContent(string url, int retryNumber = 1)
        {
            string content = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = ContentType;
            request.UserAgent = UserAgent;
            request.Accept = "application/xml";
            request.KeepAlive = false;
            request.ReadWriteTimeout = ReadWriteTimeOut;
            request.Timeout = TimeOut;
            request.AllowWriteStreamBuffering = false;

            while (retryNumber > 0)
            {
                try
                {
                    retryNumber--;
                    using (var response = request.GetResponse())
                    {
                        using (var objStream = response.GetResponseStream())
                        {
                            using (var objReader = new StreamReader(objStream))
                            {
                                content = objReader.ReadToEnd();
                                objReader.Close();
                                objReader.Dispose();

                            }
                            objStream.Flush();
                            objStream.Close();
                            objStream.Dispose();
                        }
                        response.Close();
                    }


                    request = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                catch (OutOfMemoryException ex)
                {
                    GC.Collect();
                    break;
                }
                catch (WebException ex)
                {
                    GC.Collect();
                    break;
                }
                catch (Exception ex)
                {
                    GC.Collect();
                    break;
                }
                finally
                {
                    GC.Collect();
                }
                retryNumber = 0;
            }

            return content;
        }

        public static string DownloadContent(string url, CookieContainer cookieContainer, CookieCollection cookieCollection, int retryNumber = 1)
        {
            string content = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = ContentType;
            request.UserAgent = UserAgent;
            request.Accept = "application/xml";
            request.KeepAlive = false;
            request.ReadWriteTimeout = ReadWriteTimeOut;
            request.Timeout = TimeOut;
            request.AllowWriteStreamBuffering = false;
            request.CookieContainer = cookieContainer;
            request.CookieContainer.Add(cookieCollection);

            while (retryNumber > 0)
            {
                try
                {
                    retryNumber--;
                    using (var response = request.GetResponse())
                    {
                        using (var objStream = response.GetResponseStream())
                        {
                            using (var objReader = new StreamReader(objStream))
                            {
                                content = objReader.ReadToEnd();
                                objReader.Close();
                                objReader.Dispose();

                            }
                            objStream.Flush();
                            objStream.Close();
                            objStream.Dispose();
                        }
                        response.Close();
                    }


                    request = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                catch (OutOfMemoryException ex)
                {
                    GC.Collect();
                    break;
                }
                catch (WebException ex)
                {
                    GC.Collect();
                    break;
                }
                catch (Exception ex)
                {
                    GC.Collect();
                    break;
                }
                finally
                {
                    GC.Collect();
                }
                retryNumber = 0;
            }

            return content;
        }

        public static XmlDocument DownloadDocument(string content)
        {
            try
            {
                var doc = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
                var i = content.IndexOf("<rss", System.StringComparison.Ordinal);
                if (i == -1)
                {
                    using (var xhtmlConverter = new Sgml.SgmlReader())
                    {
                        xhtmlConverter.DocType = "HTML";
                        xhtmlConverter.WhitespaceHandling = WhitespaceHandling.All;
                        xhtmlConverter.CaseFolding = Sgml.CaseFolding.ToLower;
                        xhtmlConverter.InputStream = new System.IO.StringReader(content);
                        doc.Load(xhtmlConverter);
                        xhtmlConverter.Close();
                    }
                }
                else
                {
                    content = content.Substring(i);
                    doc.LoadXml(content);
                }

                return doc;
            }
            catch (OutOfMemoryException ex)
            {
                throw;
            }
            catch (WebException ex)
            {
                throw;
            }
        }

        public static string GetString(XmlNode mainNode, string context, string pattern, ArrayList replaces, bool ignoreCase)
        {
            return GetString(mainNode, null, context, pattern, replaces, ignoreCase);
        }

        private static string GetString(XmlNode mainNode, XmlNamespaceManager nsManager, string context, string pattern, ArrayList replaces, bool ignoreCase)
        {
            if (mainNode == null || string.IsNullOrEmpty(mainNode.InnerText))
                return "";

            string ret;

            if (context != string.Empty)
            {
                XmlNode node;
                if (nsManager == null)
                    node = mainNode.SelectSingleNode(context);
                else
                    node = mainNode.SelectSingleNode(context, nsManager);
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

            var flag = RegexOptions.None;
            if (ignoreCase)
                flag |= RegexOptions.IgnoreCase;

            if (!string.IsNullOrEmpty(pattern))
            {
                var regex = new Regex(pattern, flag);
                Match match = regex.Match(ret.Replace("\r", "").Replace("\n", ""));
                ret = match.Groups.Count > 0 ? match.Groups[1].Value : string.Empty;
            }

            if (replaces != null)
            {
                string replaceString;
                foreach (ReplaceType replaceType in replaces)
                {
                    replaceString = replaceType.Replace;
                    //execute C# code to replace
                    if (replaceType.IsExpression)
                        replaceString = DynamicExpression.Evaluate(replaceType.Replace);

                    ret = Regex.Replace(ret, replaceType.Original, replaceString, flag);

                }
            }
            GC.Collect();
            ret = ret.Replace("&amp;", "&");
            return ret.Trim();
        }
    }

    public class ReplaceType
    {
        public string Original = string.Empty;
        public string Replace = string.Empty;
        public bool IsExpression;
    }
}
