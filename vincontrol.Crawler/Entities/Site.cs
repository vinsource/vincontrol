using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using vincontrol.Data.Model;

namespace vincontrol.Crawler.Entities
{
    [XmlRoot("reviewSite")]
    public class ReviewSite : ObjectBase
    {
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)";
        private const string ContentType = "text/xml;charset=\"utf-8\"";
        private const int ReadWriteTimeOut = 100000;
        private const int TimeOut = 100000;

        public static class AjaxRequestSite
        {
            public const string CitySearch = "orangecounty.citysearch.com";
            public const string LocalYahoo = "local.yahoo.com";
            public const string PlusGoogle = "plus.google.com";
        }

        [XmlArray("pages")]
        [XmlArrayItem("reviewPage", typeof(ReviewPage))]
        public CollectionBaseObject ReviewPages = new CollectionBaseObject();

        [XmlArray("namespaces")]
        [XmlArrayItem("namespace", typeof(NamespaceUriItem))]
        public ArrayList Namespaces = new ArrayList();

        [XmlElement("reviews")]
        public ReviewPost ReviewPosts;

        public List<DealerReview> DealerReviews;

        #region Attributes

        [XmlAttribute("url")]
        public string Url = string.Empty;

        [XmlAttribute("id")]
        public int Id = 0;

        #endregion

        #region Execution

        public override void Execute()
        {
            try
            {
                //var list = this.ReviewPages.Cast<ReviewPage>().ToList();
                var xmlReviewPages = this.ReviewPages.Cast<ReviewPage>().FirstOrDefault();
                var list = this.DealerReviews.Select(i => xmlReviewPages != null ? new ReviewPage()
                {
                    DealerId = i.DealerId,
                    Name = String.Format("{0}_{1}", i.DealerId, i.CategoryReviewId),
                    Category = i.CategoryReview.Name,
                    CategoryId = i.CategoryReviewId.GetValueOrDefault(),
                    Url = i.Url,
                    About = xmlReviewPages.About,
                    AdditionalReview = xmlReviewPages.AdditionalReview,
                    HasAdditionalDealerReview = xmlReviewPages.HasAdditionalDealerReview,
                    HasAdditionalUserReview = xmlReviewPages.HasAdditionalUserReview,
                    HasDetailUserReview = xmlReviewPages.HasDetailUserReview,
                    OverallScore = xmlReviewPages.OverallScore
                } : null).ToList();
                //run all subforums in Parallel mode.
                //var options = new ParallelOptions
                //{
                //    MaxDegreeOfParallelism = 3
                //};

                //Parallel.ForEach(list, options, item => item.Execute(this));
                foreach (var reviewPage in list.Where(i => i != null))
                {
                    reviewPage.Execute(this);
                }
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine("Site.cs - Execute(): AggregateException \n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                // because we run in Parallel mode, so we shouldn't throw exception. It will cause the whole process failed.
                Console.WriteLine("Site.cs - Execute(): Exception \n" + ex.Message);
            }
        }

        #endregion

        #region Public Method

        public XmlNamespaceManager GetXmlNamespaceManager(XmlDocument doc)
        {
            var nsManager = new XmlNamespaceManager(doc.NameTable);
            foreach (NamespaceUriItem item in this.Namespaces)
            {
                nsManager.AddNamespace(item.Name, item.Value);
            }
            return nsManager;
        }

        public XmlDocument DownloadDocument(string url)
        {
            var content = GetContentFromWebPage(url, 3);
            var doc = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
            var i = content.IndexOf("<rss");
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

        public string DownloadContent(string url)
        {
            return GetContentFromWebPage(url, 3);
        }

        public string CombineUrl(string siteUrl, string pageUrl)
        {
            pageUrl = pageUrl.Trim();
            if (pageUrl.StartsWith("http://") || pageUrl.StartsWith("https://"))
                return pageUrl;
            if (!pageUrl.StartsWith("/"))
                pageUrl = "/" + pageUrl;

            return "http://" + siteUrl + pageUrl;
        }

        public bool IsAjaxRequestSite(string site)
        {
            return site == AjaxRequestSite.CitySearch 
                    || site == AjaxRequestSite.LocalYahoo 
                    || site == AjaxRequestSite.PlusGoogle;
        }

        #endregion

        #region Private Method

        private string GetContentFromWebPage(string url, int retryNumber)
        {
            string content = string.Empty;

            //at least, try to do once
            retryNumber = retryNumber > 0 ? retryNumber : 1;

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
                    Console.WriteLine("Common.cs - DownloadWebPage(): OutOfMemoryException \n" + ex.Message);
                    GC.Collect();
                    break;
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Common.cs - DownloadWebPage(): WebException \n" + ex.Message);
                    GC.Collect();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Common.cs - DownloadWebPage(): Exception \n" + ex.Message);
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
        
        #endregion
    }
}
