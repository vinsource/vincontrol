using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vincontrol.Facebook
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
    }
}
