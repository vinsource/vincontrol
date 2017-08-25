#region Copyright
//	This program is licensed under the terms of the eBay Common Development and
//  Distribution License (CDDL) Version 1.0 (the "License") and any subsequent
//  version thereof released by eBay.  The then-current version of the License
//  can be found at http://www.opensource.org/licenses/cddl1.php
#endregion

using System;
using System.Configuration;
using System.Xml;
using System.Net;
using System.Text;
using System.IO;

namespace Vincontrol.Web.HelperClass
{
    public class APICall
    {
        public APICall()
        {
            //
            // 
            //
        }

        public XmlDocument MakeApiCall(string requestBody, string callname, string error)
        {
            var xmlDoc = new XmlDocument();

            //string APISandBoxServerURL = ConfigurationManager.AppSettings["APISandboxServerURL"];

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APISandBoxServerURL);

            string apiServerUrl = ConfigurationManager.AppSettings["APIServerURL"];

            var request = (HttpWebRequest)WebRequest.Create(apiServerUrl);

            //Add the request headers
            //-------------------------------------------------------------------------
            //Add the keys
            
  
            request.Headers.Add("X-EBAY-API-DEV-NAME", ConfigurationManager.AppSettings["EbaydevId"]);
            request.Headers.Add("X-EBAY-API-APP-NAME", ConfigurationManager.AppSettings["EbayappId"]);
            request.Headers.Add("X-EBAY-API-CERT-NAME", ConfigurationManager.AppSettings["EbaycertId"]);

            //Add the version
            request.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", ConfigurationManager.AppSettings["Version"]);

            //Add the SiteID
            request.Headers.Add("X-EBAY-API-SITEID", ConfigurationManager.AppSettings["SiteID"]);

            //Add the call name
            request.Headers.Add("X-EBAY-API-CALL-NAME", callname);

            //Set the request properties
            request.Method = "POST";
            request.ContentType = "text/xml";
            //-------------------------------------------------------------------------

            //Put the data into a UTF8 encoded  byte array
            UTF8Encoding encoding = new UTF8Encoding();
            int dataLen = encoding.GetByteCount(requestBody);
            byte[] utf8Bytes = new byte[dataLen];
            Encoding.UTF8.GetBytes(requestBody, 0, requestBody.Length, utf8Bytes, 0);

            Stream str = null;
            try
            {
                //Set the request Stream
                str = request.GetRequestStream();
                //Write the request to the Request Steam
                str.Write(utf8Bytes, 0, utf8Bytes.Length);
                str.Close();
                //Get response into stream
                WebResponse response = request.GetResponse();
                str = response.GetResponseStream();

                // Get Response into String
                StreamReader sr = new StreamReader(str);
                xmlDoc.LoadXml(sr.ReadToEnd());
                sr.Close();
                str.Close();
            }
            catch (Exception Ex)
            {
               error = Ex.Message;
               System.Web.HttpContext.Current.Response.Write("error=" + error);
            }
            return xmlDoc;
        }

    }
}
