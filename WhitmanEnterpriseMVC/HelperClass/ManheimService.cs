using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.Models;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class ManheimService
    {
        #region Const
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; " +
                                         "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                         ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                         "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string AuthenticityTokenPattern = "<input name=\"authenticity_token\" type=\"hidden\" value=\"([^\\\"]*)\" />";
        private const string LogInUrl = "https://www.manheim.com/login";
        private const string SubmitLogInUrl = "https://www.manheim.com/login/authenticate";
        private const string RefererUrl = "https://www.manheim.com/members/mymanheim";
        private const string CrawlDataUrl = "https://www.manheim.com/members/internetmmr/vehicleSelectorPage.mmr";
        private const string DefaultUserName = "9493957285";
        private const string DefaultPassword = "beach1";
        #endregion

        #region Properties
        public CookieContainer CookieContainer { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public string Result { get; set; }
        public string AuthenticityToken { get; set; }
        public string PostData { get; set; }
        public string Sid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReportParameters { get; set; }
        public ManheimWholesaleViewModel ManheimWholesale { get; set; }
        public List<ManheimWholesaleViewModel> ManheimWholesales { get; set; }
        public List<ManheimTransactionViewModel> ManheimTransactions { get; set; }
        public List<ManheimTrim> ManheimTrims { get; set; }
        public string RecallUrl { get; set; }        
        #endregion

        public ManheimService()
        {
            CookieContainer = new CookieContainer();
            CookieCollection = new CookieCollection();
            ManheimWholesale = new ManheimWholesaleViewModel();
            ManheimWholesales = new List<ManheimWholesaleViewModel>();
            ManheimTransactions = new List<ManheimTransactionViewModel>();
            ManheimTrims = new List<ManheimTrim>();
        }

        #region Public Methods
        public void Execute(string userName, string password)
        {
            UserName = userName;
            Password = password;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            // Step 5:
            BuildReportParameters();
            // Step 6:
            GetReportContent();
        }
        
        public void ExecuteByVin(string userName, string password, string vin)
        {
            UserName = userName;
            Password = password;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            if (!string.IsNullOrEmpty(Sid))
            {
                // Step 5:
                BuildReportParameters(vin);
                // Step 6:
                WebRequestGetByVinWithMultipleTrims();
            }
        }

        public void Execute(string vin)
        {
            GetDefaultManheimLogin();
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();            
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            if (!string.IsNullOrEmpty(Sid))
            {
                // Step 5:
                BuildReportParameters(vin);
                // Step 6:
                //WebRequestGetByVin();
                WebRequestGetByVinWithMultipleTrims();
            }
        }

        public void Execute(string country, string year, string make, string model, string body, string region)
        {
            if (string.IsNullOrEmpty(Sid))
            {
                UserName = DefaultUserName;
                Password = DefaultPassword;
                // Step 1:
                WebRequestGet();
                // Step 2:
                GetAuthenticityToken();
                // Step 3:
                WebRequestPost();
                // Step 4:
                GetSid();
            }
            if (!string.IsNullOrEmpty(Sid))
            {
                // Step 5:
                BuildReportParameters(country, year, make, model, body, region);
                // Step 6:
                GetReportContent();
            }
        }

        public void Execute(string country, string year, string make, string model, string body, string region, string userName, string password)
        {
            if (string.IsNullOrEmpty(Sid))
            {
                UserName = userName;
                Password = password;
                // Step 1:
                WebRequestGet();
                // Step 2:
                GetAuthenticityToken();
                // Step 3:
                WebRequestPost();
                // Step 4:
                GetSid();
            }

            if (!string.IsNullOrEmpty(Sid))
            {
                // Step 5:
                BuildReportParameters(country, year, make, model, body, region);
                // Step 6:
                GetReportContent();
            }
        }

        public void Execute(string trim, string region, string url)
        {
            UserName = DefaultUserName;
            Password = DefaultPassword;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            // Step 5:
            BuildReportParameters(trim, region, url);
            // Step 6:
            GetTransactionContent();
        }

        public void GetMake(string userName, string password)
        {
            UserName = userName;
            Password = password;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            // Step 6:
            GetMake();
        }

        //public void GetModel(string userName, string password)
        //{
        //    UserName = userName;
        //    Password = password;
        //    // Step 1:
        //    WebRequestGet();
        //    // Step 2:
        //    GetAuthenticityToken();
        //    // Step 3:
        //    WebRequestPost();
        //    // Step 4:
        //    GetSid();
        //    // Step 5:
        //    GetModel();
        //}

        //public void GetTrim(string userName, string password)
        //{
        //    UserName = userName;
        //    Password = password;
        //    // Step 1:
        //    WebRequestGet();
        //    // Step 2:
        //    GetAuthenticityToken();
        //    // Step 3:
        //    WebRequestPost();
        //    // Step 4:
        //    GetSid();
        //    // Step 5:
        //    GetTrim();
        //}

        public void GetTrim(string year, string make, int[] models)
        {
            UserName = DefaultUserName;
            Password = DefaultPassword;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            // Step 5:
            foreach (var model in models)
            {
                PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
                                year, make, model.ToString());
                Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
                Request.Method = "POST";
                Request.UserAgent = UserAgent;
                Request.ContentLength = PostData.Length;
                Request.ContentType = ContentType;
                Request.Referer = RefererUrl;
                Request.CookieContainer = CookieContainer;
                Request.CookieContainer.Add(CookieCollection);

                try
                {
                    StreamWriter = new StreamWriter(Request.GetRequestStream());
                    StreamWriter.Write(PostData);
                    StreamWriter.Close();
                    Response = (HttpWebResponse)Request.GetResponse();
                    StreamReader = new StreamReader(Response.GetResponseStream());
                    Result = StreamReader.ReadToEnd();
                    StreamReader.Close();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError) { }
                    else { }
                }

                var xmlDocument = DownloadDocument(Result);
                var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
                if (options != null && options.Count > 0)
                {
                    var maheimModel = GetModelByServiceId(Convert.ToInt32(model));
                    var modelId = maheimModel.id;
                    foreach (XmlNode option in options)
                    {
                        var bodyName = GetString(option, "text()", null, null, true);
                        var bodyValue = GetString(option, "@value", null, null, true);
                        var bodyId = Convert.ToInt32(bodyValue);
                        //using (var context = new whitmanenterprisewarehouseEntities())
                        //{
                        //    if (GetTrimByServiceIdAndModelId(bodyId, modelId) == null)
                        //    {
                        //        var trimToInsert = new manheimtrim()
                        //        {
                        //            serviceId = bodyId,
                        //            name = bodyName.ToLower(),
                        //            modelId = modelId
                        //        };
                              
                        //        context.AddTomanheimtrims(trimToInsert);
                        //        context.SaveChanges();
                        //    }
                        //}
                        ManheimTrims.Add(new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName });
                    }
                }
            }

        }

        public ManheimTrim GetTrimItem(string year, string make, string model)
        {
            UserName = DefaultUserName;
            Password = DefaultPassword;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            // Step 5:
          
                PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
                                year, make, model.ToString());
                Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
                Request.Method = "POST";
                Request.UserAgent = UserAgent;
                Request.ContentLength = PostData.Length;
                Request.ContentType = ContentType;
                Request.Referer = RefererUrl;
                Request.CookieContainer = CookieContainer;
                Request.CookieContainer.Add(CookieCollection);

                try
                {
                    StreamWriter = new StreamWriter(Request.GetRequestStream());
                    StreamWriter.Write(PostData);
                    StreamWriter.Close();
                    Response = (HttpWebResponse)Request.GetResponse();
                    StreamReader = new StreamReader(Response.GetResponseStream());
                    Result = StreamReader.ReadToEnd();
                    StreamReader.Close();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError) { }
                    else { }
                }

                var xmlDocument = DownloadDocument(Result);
                var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
                if (options != null && options.Count > 0)
                {
                    var maheimModel = GetModelByServiceId(Convert.ToInt32(model));
                    var modelId = maheimModel.id;
                    foreach (XmlNode option in options)
                    {
                        var bodyName = GetString(option, "text()", null, null, true);
                        var bodyValue = GetString(option, "@value", null, null, true);
                        var bodyId = Convert.ToInt32(bodyValue);
                        //using (var context = new whitmanenterprisewarehouseEntities())
                        //{
                        //    if (GetTrimByServiceIdAndModelId(bodyId, modelId) == null)
                        //    {
                        //        var trimToInsert = new manheimtrim()
                        //        {
                        //            serviceId = bodyId,
                        //            name = bodyName.ToLower(),
                        //            modelId = modelId
                        //        };

                        //        context.AddTomanheimtrims(trimToInsert);
                        //        context.SaveChanges();
                        //    }
                        //}
                        return new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName };
                    }
                }
                return null;
            

        }


        public void GetTrim(string year, string make, int[] models, string userName, string password)
        {
            UserName = userName;
            Password = password;
            // Step 1:
            WebRequestGet();
            // Step 2:
            GetAuthenticityToken();
            // Step 3:
            WebRequestPost();
            // Step 4:
            GetSid();
            if (!string.IsNullOrEmpty(Sid))
            {
                // Step 5:
                foreach (var model in models)
                {
                    PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
                                    year, make, model.ToString());
                    Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
                    Request.Method = "POST";
                    Request.UserAgent = UserAgent;
                    Request.ContentLength = PostData.Length;
                    Request.ContentType = ContentType;
                    Request.Referer = RefererUrl;
                    Request.CookieContainer = CookieContainer;
                    Request.CookieContainer.Add(CookieCollection);

                    try
                    {
                        StreamWriter = new StreamWriter(Request.GetRequestStream());
                        StreamWriter.Write(PostData);
                        StreamWriter.Close();
                        Response = (HttpWebResponse)Request.GetResponse();
                        StreamReader = new StreamReader(Response.GetResponseStream());
                        Result = StreamReader.ReadToEnd();
                        StreamReader.Close();
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status == WebExceptionStatus.ProtocolError) { }
                        else { }
                    }

                    var xmlDocument = DownloadDocument(Result);
                    var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
                    if (options != null && options.Count > 0)
                    {
                        var modelId = GetModelByServiceId(Convert.ToInt32(model)).id;
                        foreach (XmlNode option in options)
                        {
                            var bodyName = GetString(option, "text()", null, null, true);
                            var bodyValue = GetString(option, "@value", null, null, true);
                            var bodyId = Convert.ToInt32(bodyValue);
                            //using (var context = new whitmanenterprisewarehouseEntities())
                            //{
                            //    if (GetTrimByServiceIdAndModelId(bodyId, modelId) == null)
                            //    {
                            //        var trimToInsert = new manheimtrim()
                            //        {
                            //            serviceId = bodyId,
                            //            name = bodyName.ToLower(),
                            //            modelId = modelId
                            //        };
                            //        context.AddTomanheimtrims(trimToInsert);
                            //        context.SaveChanges();
                            //    }
                            //}
                            ManheimTrims.Add(new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName });
                        }
                    }
                }
            }
        }

        public int MatchingMake(string make)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var matchingMake = context.manheimmakes.FirstOrDefault(i => i.name.ToLower().Equals(make.ToLower()));
                return matchingMake == null ? 0 : matchingMake.serviceId.Value;
            }
        }

        public int MatchingModel(string model, int makeServiceId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var makeId = GetMakeByServiceId(makeServiceId).id;
                var matchingModel = context.manheimmakemodels.FirstOrDefault(i => i.manheimmodel.name.ToLower().Contains(model.ToLower()) && i.manheimyearmake.makeId == makeId);
                return matchingModel == null ? 0 : matchingModel.manheimmodel.serviceId.Value;
            }
        }

        public int[] MatchingModels(string model, int makeServiceId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var makeId = GetMakeByServiceId(makeServiceId).id;
                var matchingModels = context.manheimmakemodels.Where(i => i.manheimmodel.name.ToLower().Contains(model.ToLower()) && i.manheimyearmake.makeId == makeId).Select(i => i.manheimmodel.serviceId.Value).ToArray();
                return matchingModels;
            }
        }

        public int MatchingTrim(string trim, int modelServiceId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var modelId = GetModelByServiceId(modelServiceId).id;
                var matchingTrim = context.manheimmodeltrims.FirstOrDefault(i => i.manheimtrim.name.ToLower().Contains(trim.ToLower()) && i.manheimmakemodel.modelId == modelId);
                return matchingTrim == null ? 0 : matchingTrim.manheimtrim.serviceId.Value;
            }
        }

        public List<manheimtrim> MatchingTrimsByModelId(int modelServiceId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var modelId = GetModelByServiceId(modelServiceId).id;
                var matchingTrims = context.manheimmodeltrims.Where(i => i.manheimmakemodel.modelId == modelId).OrderByDescending(i => i.id).Skip(0).Take(5).Select(i=>i.manheimtrim).Distinct();
                return matchingTrims.ToList();
            }
        }

        public List<SelectItem> GetRegion()
        {
            return new List<SelectItem>()
            {
                new SelectItem("National", "NA", false),
                new SelectItem("South East", "SE", false),
                new SelectItem("North East", "NE", false),
                new SelectItem("Mid West", "MW", false),
                new SelectItem("South West", "SW", false),
                new SelectItem("West Coast", "WC", false),
            };
        }
        #endregion

        #region Private Methods
        private void WebRequestGet()
        {
            Request = (HttpWebRequest)WebRequest.Create(LogInUrl);
            //var proxyObject = new WebProxy("http://198.57.44.193:3128", true);
            //webRequest.Proxy = proxyObject;
            Request.UserAgent = UserAgent;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);
            //Get the response from the server and save the cookies from the first request..
            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            CookieCollection = response.Cookies;

            StreamReader = new StreamReader(response.GetResponseStream());
            Result = StreamReader.ReadToEnd();
            StreamReader.Close();
        }

        private void WebRequestGetByVin()
        {
            Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
            //var proxyObject = new WebProxy("http://198.57.44.193:3128", true);
            //webRequest.Proxy = proxyObject;
            Request.UserAgent = UserAgent;
            Request.ContentType = "text/html;charset=ISO-8859-1";
            Request.Referer = RefererUrl;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);
            //Get the response from the server and save the cookies from the first request..
            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            CookieCollection = response.Cookies;

            StreamReader = new StreamReader(response.GetResponseStream());
            Result = StreamReader.ReadToEnd();
            StreamReader.Close();

            var xmlDocument = DownloadDocument(Result);
            var isSingleTrim = GetString(xmlDocument, "//meta[@id='WT.z_make']/@content", null, null, true);
            if (string.IsNullOrEmpty(isSingleTrim))
            {
                var firstTrim = GetString(xmlDocument, "//table/tr[2]/td[2]/table/tr/td/table/tr[2]/span/td/a/@href", null, null, true);
                var parameters = firstTrim.Split('&').ToArray();
                ManheimWholesale.Year = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("year")).Replace("year=", ""));
                ManheimWholesale.MakeServiceId = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("makeID")).Replace("makeID=", ""));
                ManheimWholesale.ModelServiceId = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("modelID")).Replace("modelID=", ""));
                ManheimWholesale.TrimServiceId = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("bodyID")).Replace("bodyID=", ""));
                ManheimWholesale.TrimName = GetString(xmlDocument, "//table/tr[2]/td[2]/table/tr/td/table/tr[2]/span/td[4]/a/text()", null, null, true);
            }
            else
            {
                ManheimWholesale.Year = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectYear']/@value", null, null, true));
                ManheimWholesale.MakeServiceId = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectMake']/@value", null, null, true));
                ManheimWholesale.ModelServiceId = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectModel']/@value", null, null, true));
                ManheimWholesale.TrimServiceId = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectBody']/@value", null, null, true));
                ManheimWholesale.TrimName = GetString(xmlDocument, "//select[@name='collectionSelectBody']/option[position() = 2]/text()", null, null, true);
            }

            BuildReportParameters(ManheimWholesale.Year.ToString(), ManheimWholesale.MakeServiceId.ToString(), ManheimWholesale.ModelServiceId.ToString(), ManheimWholesale.TrimServiceId.ToString());
            Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
            //var proxyObject = new WebProxy("http://198.57.44.193:3128", true);
            //webRequest.Proxy = proxyObject;
            Request.UserAgent = UserAgent;
            Request.ContentType = "text/html;charset=ISO-8859-1";
            Request.Referer = RefererUrl;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);
            //Get the response from the server and save the cookies from the first request..
            response = (HttpWebResponse)Request.GetResponse();
            CookieCollection = response.Cookies;

            StreamReader = new StreamReader(response.GetResponseStream());
            Result = StreamReader.ReadToEnd();
            StreamReader.Close();

            xmlDocument = DownloadDocument(Result);
            ManheimWholesale.HighestPrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true);
            ManheimWholesale.AveragePrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true);
            ManheimWholesale.LowestPrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true);
        }

        private void WebRequestGetByVinWithMultipleTrims()
        {
            //clear wholesale values before getting new values
            ManheimWholesales.Clear();

            Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
            //var proxyObject = new WebProxy("http://198.57.44.193:3128", true);
            //webRequest.Proxy = proxyObject;
            Request.UserAgent = UserAgent;
            Request.ContentType = "text/html;charset=ISO-8859-1";
            Request.Referer = RefererUrl;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);
            //Get the response from the server and save the cookies from the first request..
            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            CookieCollection = response.Cookies;

            StreamReader = new StreamReader(response.GetResponseStream());
            Result = StreamReader.ReadToEnd();
            StreamReader.Close();

            var xmlDocument = DownloadDocument(Result);
            var isSingleTrim = GetString(xmlDocument, "//meta[@id='WT.z_make']/@content", null, null, true);
            if (string.IsNullOrEmpty(isSingleTrim))
            {
                var nodes = xmlDocument.SelectNodes("//table/tr[2]/td[2]/table/tr/td/table/tr[position() >= 2]");
                foreach (XmlNode node in nodes)
                {
                    var wholesale = new ManheimWholesaleViewModel();
                    var firstTrim = GetString(node, "./span/td/a/@href", null, null, true);
                    var parameters = firstTrim.Split('&').ToArray();
                    wholesale.Year = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("year")).Replace("year=", ""));
                    wholesale.MakeServiceId = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("makeID")).Replace("makeID=", ""));
                    wholesale.ModelServiceId = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("modelID")).Replace("modelID=", ""));
                    wholesale.TrimServiceId = Convert.ToInt32(parameters.FirstOrDefault(i => i.Contains("bodyID")).Replace("bodyID=", ""));
                    wholesale.TrimName = GetString(node, "./span/td[4]/a/text()", null, null, true).ToLower();

                    BuildReportParameters(wholesale.Year.ToString(), wholesale.MakeServiceId.ToString(), wholesale.ModelServiceId.ToString(), wholesale.TrimServiceId.ToString());
                    Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
                    //var proxyObject = new WebProxy("http://198.57.44.193:3128", true);
                    //webRequest.Proxy = proxyObject;
                    Request.UserAgent = UserAgent;
                    Request.ContentType = "text/html;charset=ISO-8859-1";
                    Request.Referer = RefererUrl;
                    Request.CookieContainer = CookieContainer;
                    Request.CookieContainer.Add(CookieCollection);
                    //Get the response from the server and save the cookies from the first request..
                    response = (HttpWebResponse)Request.GetResponse();
                    CookieCollection = response.Cookies;

                    StreamReader = new StreamReader(response.GetResponseStream());
                    Result = StreamReader.ReadToEnd();
                    StreamReader.Close();

                    xmlDocument = DownloadDocument(Result);
                    wholesale.HighestPrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true);
                    wholesale.AveragePrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true);
                    wholesale.LowestPrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true);

                    ManheimWholesales.Add(wholesale);
                }                
            }
            else
            {
                var wholesale = new ManheimWholesaleViewModel();
                wholesale.Year = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectYear']/@value", null, null, true));
                wholesale.MakeServiceId = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectMake']/@value", null, null, true));
                wholesale.ModelServiceId = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectModel']/@value", null, null, true));
                wholesale.TrimServiceId = Convert.ToInt32(GetString(xmlDocument, "//input[@name='collectionSelectBody']/@value", null, null, true));
                wholesale.TrimName = GetString(xmlDocument, "//select[@name='collectionSelectBody']/option[@selected='selected']/text()", null, null, true).ToLower();

                BuildReportParameters(wholesale.Year.ToString(), wholesale.MakeServiceId.ToString(), wholesale.ModelServiceId.ToString(), wholesale.TrimServiceId.ToString());
                Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
                //var proxyObject = new WebProxy("http://198.57.44.193:3128", true);
                //webRequest.Proxy = proxyObject;
                Request.UserAgent = UserAgent;
                Request.ContentType = "text/html;charset=ISO-8859-1";
                Request.Referer = RefererUrl;
                Request.CookieContainer = CookieContainer;
                Request.CookieContainer.Add(CookieCollection);
                //Get the response from the server and save the cookies from the first request..
                response = (HttpWebResponse)Request.GetResponse();
                CookieCollection = response.Cookies;

                StreamReader = new StreamReader(response.GetResponseStream());
                Result = StreamReader.ReadToEnd();
                StreamReader.Close();

                xmlDocument = DownloadDocument(Result);
                wholesale.HighestPrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true);
                wholesale.AveragePrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true);
                wholesale.LowestPrice = GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true);

                ManheimWholesales.Add(wholesale);
            }            
        }

        private void GetAuthenticityToken()
        {
            var authTokenPattern = new Regex(AuthenticityTokenPattern);
            AuthenticityToken = authTokenPattern.Match(Result).Groups[1].Value;
        }

        private void WebRequestPost()
        {
            PostData = String.Format("user[username]={0}&user[password]={1}&authenticity_token={2}&submit=Login&remember=1", UserName, Password, AuthenticityToken);

            // Setup the http request.
            Request = (HttpWebRequest)WebRequest.Create(SubmitLogInUrl);
            Request.Method = "POST";
            Request.UserAgent = UserAgent;
            Request.ContentLength = PostData.Length;
            Request.ContentType = ContentType;
            Request.Referer = RefererUrl;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);
            try
            {
                // Post to the login form.
                StreamWriter = new StreamWriter(Request.GetRequestStream());
                StreamWriter.Write(PostData);
                StreamWriter.Close();

                // Get the response.
                Response = (HttpWebResponse)Request.GetResponse();

                // Have some cookies.
                CookieCollection = Response.Cookies;

                // Read the response
                StreamReader = new StreamReader(Response.GetResponseStream());
                Result = StreamReader.ReadToEnd();
                StreamReader.Close();
            }
            catch (Exception)
            {
                Result = string.Empty;
            }            
        }

        private void GetSid()
        {
            Sid = string.Empty;
            // now we can send out cookie along with a request for the protected page           
            if (CookieCollection[0] != null && CookieCollection[0].Name.Equals("JSESSIONID"))
            {
                Sid = CookieCollection[0].Value.Replace(".", "");
            }
        }

        private void BuildReportParameters()
        {
            ReportParameters = String.Format("https://www.manheim.com/members/internetmmr/reportSelection.mmr?sid={0}&country=US&collectionSelectYear=2012&collectionSelectMake=6&collectionSelectModel=18&collectionSelectBody=9786&beanCollectionSelectRegion=NA&beanCollectionSelectSeasonalAdj=off&mileage=&searchMethod=m_mmr_ymm-selection&vehicleMileageProperty=", Sid);
        }

        private void BuildReportParameters(string vin)
        {
            ReportParameters = String.Format("https://www.manheim.com/members/internetmmr/decoder.mmr?sid=275DB020A096CC1D560AD9198E09CD10tomcat0&vin={0}&year=&makeID=&modelID=&bodyID=&country=US&regionid=NA&seasonal=off&mileage=&userid={1}&searchMethod=&decoded=", vin, UserName);
            //String.Format("https://www.manheim.com/members/internetmmr/?vin={0}", vin);
        }

        private void BuildReportParameters(string trim, string region, string url)
        {
            ReportParameters = url.Replace("[trim]", trim).Replace("[region]", region);
        }

        private void BuildReportParameters(string year, string make, string model, string trim)
        {
            ReportParameters = String.Format("https://www.manheim.com/members/internetmmr/pricesPageData.mmr?action=MileageSet&tab=pricesPage&sid={4}&country=US&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}&collectionSelectBody={3}&beanCollectionSelectRegion=NA&beanCollectionSelectSeasonalAdj=off&mileage=&searchMethod=&Reset=&vehicleMileageProperty=", year, make, model, trim, Sid);
        }

        private void BuildReportParameters(string country, string year, string make, string model, string body, string region)
        {
            ReportParameters = String.Format("https://www.manheim.com/members/internetmmr/reportSelection.mmr?sid={0}&country={1}" +
                "&collectionSelectYear={2}" +
                "&collectionSelectMake={3}" +
                "&collectionSelectModel={4}" +
                "&collectionSelectBody={5}" +
                "&beanCollectionSelectRegion={6}" +
                "&beanCollectionSelectSeasonalAdj=off&mileage=&searchMethod=m_mmr_ymm-selection&vehicleMileageProperty=", Sid, country, year, make, model, body, region);

            RecallUrl = String.Format("https://www.manheim.com/members/internetmmr/reportSelection.mmr?sid=" + Sid + "&country=US" +
                "&collectionSelectYear=" + year + "" +
                "&collectionSelectMake=" + make + "" +
                "&collectionSelectModel=" + model + "" +
                "&collectionSelectBody=[trim]" +
                "&beanCollectionSelectRegion=[region]" +
                "&beanCollectionSelectSeasonalAdj=off&mileage=&searchMethod=m_mmr_ymm-selection&vehicleMileageProperty=");
        }

        private void GetReportContent()
        {
            PostData = "reportsProperty=valuationExact&action=PrintReports";
            Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
            Request.Method = "POST";
            Request.UserAgent = UserAgent;
            Request.ContentLength = PostData.Length;
            Request.ContentType = ContentType;
            Request.Referer = RefererUrl;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);

            try
            {
                StreamWriter = new StreamWriter(Request.GetRequestStream());
                StreamWriter.Write(PostData);
                StreamWriter.Close();
                Response = (HttpWebResponse)Request.GetResponse();
                StreamReader = new StreamReader(Response.GetResponseStream());
                Result = StreamReader.ReadToEnd();
                StreamReader.Close();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //throw new Exception("HTTP response error. " + (int)(((HttpWebResponse)ex.Response).StatusCode) + ((HttpWebResponse)ex.Response).StatusDescription);
                }
                else
                {
                    //throw new Exception("HTTP response error with status: " + ex.Status.ToString());
                }
            }

            var xmlDocument = DownloadDocument(Result);

            ManheimWholesale.HighestPrice = GetString(xmlDocument, "//table[2]/tr/td[2]/table/tr[3]/td[2]", null, null, true);
            ManheimWholesale.AveragePrice = GetString(xmlDocument, "//table[2]/tr/td[2]/table/tr[3]/td[3]", null, null, true);
            ManheimWholesale.LowestPrice = GetString(xmlDocument, "//table[2]/tr/td[2]/table/tr[3]/td[4]", null, null, true);

            var transactionNodes = xmlDocument.SelectNodes("//table[2]/tr[position() >= 2]");
            if (transactionNodes != null)
            {
                foreach (XmlNode node in transactionNodes)
                {
                    var transaction = new ManheimTransactionViewModel();
                    transaction.Type = GetString(node, "./td[1]", null, null, true);
                    transaction.Odometer = GetString(node, "./td[2]", null, null, true);
                    transaction.Price = GetString(node, "./td[3]", null, null, true);
                    transaction.SaleDate = GetString(node, "./td[4]/center", null, null, true);
                    transaction.Auction = GetString(node, "./td[5]", null, null, true);
                    transaction.Engine = GetString(node, "./td[6]", null, null, true);
                    transaction.TR = GetString(node, "./td[7]", null, null, true);
                    transaction.Cond = GetString(node, "./td[8]", null, null, true);
                    transaction.Color = GetString(node, "./td[9]", null, null, true);
                    transaction.Sample = GetString(node, "./td[10]", null, null, true);
                    ManheimTransactions.Add(transaction);
                }
            }
        }

        private void GetTransactionContent()
        {
            PostData = "reportsProperty=valuationExact&action=PrintReports";
            Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
            Request.Method = "POST";
            Request.UserAgent = UserAgent;
            Request.ContentLength = PostData.Length;
            Request.ContentType = ContentType;
            Request.Referer = RefererUrl;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);

            try
            {
                StreamWriter = new StreamWriter(Request.GetRequestStream());
                StreamWriter.Write(PostData);
                StreamWriter.Close();
                Response = (HttpWebResponse)Request.GetResponse();
                StreamReader = new StreamReader(Response.GetResponseStream());
                Result = StreamReader.ReadToEnd();
                StreamReader.Close();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    //throw new Exception("HTTP response error. " + (int)(((HttpWebResponse)ex.Response).StatusCode) + ((HttpWebResponse)ex.Response).StatusDescription);
                }
                else
                {
                    //throw new Exception("HTTP response error with status: " + ex.Status.ToString());
                }
            }

            var xmlDocument = DownloadDocument(Result);

            var transactionNodes = xmlDocument.SelectNodes("//table[2]/tr[position() >= 2]");
            if (transactionNodes != null)
            {
                foreach (XmlNode node in transactionNodes)
                {
                    var transaction = new ManheimTransactionViewModel();
                    transaction.Type = GetString(node, "./td[1]", null, null, true);
                    transaction.Odometer = GetString(node, "./td[2]", null, null, true);
                    transaction.Price = GetString(node, "./td[3]", null, null, true);
                    transaction.SaleDate = GetString(node, "./td[4]/center", null, null, true);
                    transaction.Auction = GetString(node, "./td[5]", null, null, true);
                    transaction.Engine = GetString(node, "./td[6]", null, null, true);
                    transaction.TR = GetString(node, "./td[7]", null, null, true);
                    transaction.Cond = GetString(node, "./td[8]", null, null, true);
                    transaction.Color = GetString(node, "./td[9]", null, null, true);
                    transaction.Sample = GetString(node, "./td[10]", null, null, true);
                    ManheimTransactions.Add(transaction);
                }
            }
        }

        private void GetMake()
        {
            for (int year = 1981; year <= 2013; year++)
            {
                PostData = "action=year&collectionSelectYear=" + year;
                Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
                Request.Method = "POST";
                Request.UserAgent = UserAgent;
                Request.ContentLength = PostData.Length;
                Request.ContentType = ContentType;
                Request.Referer = RefererUrl;
                Request.CookieContainer = CookieContainer;
                Request.CookieContainer.Add(CookieCollection);

                try
                {
                    StreamWriter = new StreamWriter(Request.GetRequestStream());
                    StreamWriter.Write(PostData);
                    StreamWriter.Close();
                    Response = (HttpWebResponse)Request.GetResponse();
                    StreamReader = new StreamReader(Response.GetResponseStream());
                    Result = StreamReader.ReadToEnd();
                    StreamReader.Close();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError) { }
                    else { }
                }

                var xmlDocument = DownloadDocument(Result);
                var options = xmlDocument.SelectNodes("//select[@name='collectionSelectMake']/option[position() > 1]");
                if (options != null && options.Count > 0)
                {
                    foreach (XmlNode option in options)
                    {
                        var makeName = GetString(option, "text()", null, null, true);
                        var makeValue = GetString(option, "@value", null, null, true);
                        var makeId = Convert.ToInt32(makeValue);
                        using (var context = new whitmanenterprisewarehouseEntities())
                        {
                            if (GetMakeByServiceId(makeId) == null)
                            {
                                var makeToInsert = new manheimmake()
                                {
                                    serviceId = makeId,
                                    name = makeName.ToLower()
                                };
                                context.AddTomanheimmakes(makeToInsert);
                                context.SaveChanges();
                                System.Console.WriteLine(String.Format("Make: {0}", makeName));
                            }
                        }

                    }

                }
            }
        }

        //private void GetModel()
        //{
        //    var manheimMakes = GetAllMakes();
        //    for (int year = 1981; year <= 2013; year++)
        //    {
        //        foreach (var make in manheimMakes)
        //        {
        //            PostData = "action=make&collectionSelectYear=" + year + "&collectionSelectMake=" + make.serviceId.Value; // for testing: "action=make&collectionSelectYear=2013&collectionSelectMake=69"                
        //            Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
        //            Request.Method = "POST";
        //            Request.UserAgent = UserAgent;
        //            Request.ContentLength = PostData.Length;
        //            Request.ContentType = ContentType;
        //            Request.Referer = RefererUrl;
        //            Request.CookieContainer = CookieContainer;
        //            Request.CookieContainer.Add(CookieCollection);

        //            try
        //            {
        //                StreamWriter = new StreamWriter(Request.GetRequestStream());
        //                StreamWriter.Write(PostData);
        //                StreamWriter.Close();
        //                Response = (HttpWebResponse)Request.GetResponse();
        //                StreamReader = new StreamReader(Response.GetResponseStream());
        //                Result = StreamReader.ReadToEnd();
        //                StreamReader.Close();
        //            }
        //            catch (WebException ex)
        //            {
        //                if (ex.Status == WebExceptionStatus.ProtocolError) { }
        //                else { }
        //            }

        //            var xmlDocument = DownloadDocument(Result);
        //            var options = xmlDocument.SelectNodes("//select[@name='collectionSelectModel']/option[position() > 1]");
        //            if (options != null && options.Count > 0)
        //            {
        //                foreach (XmlNode option in options)
        //                {
        //                    var modelName = GetString(option, "text()", null, null, true);
        //                    var modelValue = GetString(option, "@value", null, null, true);
        //                    var modelId = Convert.ToInt32(modelValue);
        //                    using (var context = new whitmanenterprisewarehouseEntities())
        //                    {
        //                        if (GetModelByServiceId(modelId) == null)
        //                        {
        //                            var makeToInsert = new manheimmodel()
        //                            {
        //                                serviceId = modelId,
        //                                name = modelName.ToLower(),
        //                                 makeId = make.id
        //                            };
        //                            context.AddTomanheimmodels(makeToInsert);
        //                            context.SaveChanges();
        //                            System.Console.WriteLine(String.Format("Model: {0}", modelName));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void GetTrim()
        //{
        //    var manheimMakes = GetAllMakes();
        //    for (int year = 1981; year <= 2013; year++)
        //    {
        //        foreach (var make in manheimMakes)
        //        {
        //            var models = GetModelsByMakeId(make.id);
        //            foreach (var model in models)
        //            {
        //                PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
        //                        year, make.serviceId.Value, model.serviceId.Value);
        //                Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
        //                Request.Method = "POST";
        //                Request.UserAgent = UserAgent;
        //                Request.ContentLength = PostData.Length;
        //                Request.ContentType = ContentType;
        //                Request.Referer = RefererUrl;
        //                Request.CookieContainer = CookieContainer;
        //                Request.CookieContainer.Add(CookieCollection);

        //                try
        //                {
        //                    StreamWriter = new StreamWriter(Request.GetRequestStream());
        //                    StreamWriter.Write(PostData);
        //                    StreamWriter.Close();
        //                    Response = (HttpWebResponse)Request.GetResponse();
        //                    StreamReader = new StreamReader(Response.GetResponseStream());
        //                    Result = StreamReader.ReadToEnd();
        //                    StreamReader.Close();
        //                }
        //                catch (WebException ex)
        //                {
        //                    if (ex.Status == WebExceptionStatus.ProtocolError) { }
        //                    else { }
        //                }

        //                var xmlDocument = DownloadDocument(Result);
        //                var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
        //                if (options != null && options.Count > 0)
        //                {
        //                    foreach (XmlNode option in options)
        //                    {
        //                        var bodyName = GetString(option, "text()", null, null, true);
        //                        var bodyValue = GetString(option, "@value", null, null, true);
        //                        var bodyId = Convert.ToInt32(bodyValue);
        //                        using (var context = new whitmanenterprisewarehouseEntities())
        //                        {
        //                            if (GetTrimByServiceIdAndModelId(bodyId, model.id) == null)
        //                            {
        //                                var trimToInsert = new manheimtrim()
        //                                {
        //                                    serviceId = bodyId,
        //                                    name = bodyName.ToLower(),
        //                                    modelId = model.id
        //                                };
        //                                context.AddTomanheimtrims(trimToInsert);
        //                                context.SaveChanges();
        //                                System.Console.WriteLine(String.Format("Trim: {0}", bodyName));
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}

        private XmlDocument DownloadDocument(string content)
        {
            try
            {
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
            catch (OutOfMemoryException ex)
            {
                throw;
            }
            catch (WebException ex)
            {
                throw;
            }
        }

        private string GetString(XmlNode mainNode, string context, string pattern, ArrayList replaces, bool ignoreCase)
        {
            return GetString(mainNode, null, context, pattern, replaces, ignoreCase);
        }

        private string GetString(XmlNode mainNode, XmlNamespaceManager nsManager, string context, string pattern, ArrayList replaces, bool ignoreCase)
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

            RegexOptions flag = RegexOptions.None;
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

        private manheimmake GetMakeByServiceId(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimmakes.FirstOrDefault(m => m.serviceId == id);
            }
        }

        private manheimmake GetMakeById(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimmakes.FirstOrDefault(m => m.id == id);
            }
        }

        private List<manheimmake> GetAllMakes()
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimmakes.ToList();
            }
        }

        private manheimmodel GetModelByServiceId(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimmodels.FirstOrDefault(m => m.serviceId == id);
            }
        }

        private List<manheimmakemodel> GetModelsByMakeId(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimmakemodels.Where(m => m.manheimyearmake.makeId == id).ToList();
            }
        }

        private manheimtrim GetTrimByServiceId(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimtrims.FirstOrDefault(m => m.serviceId == id);
            }
        }

        private manheimtrim GetTrimByServiceIdAndModelId(int id, int modelId)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                return context.manheimmodeltrims.Where(m => m.manheimtrim.serviceId == id && m.manheimmakemodel.modelId == modelId).Select(i=>i.manheimtrim).FirstOrDefault();
            }
        }

        private void GetDefaultManheimLogin()
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var setting = context.whitmanenterprisesettings.FirstOrDefault(m => m.Manheim != "vincontrol");
                if (setting != null)
                {
                    UserName = setting.Manheim;
                    Password = setting.ManheimPassword;
                }
                else
                {
                    UserName = DefaultUserName;
                    Password = DefaultPassword;
                }
            }
        }
        #endregion
    }

    public class ReplaceType
    {
        public string Original = string.Empty;
        public string Replace = string.Empty;
        public bool IsExpression;
    }
}