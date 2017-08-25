using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ManheimTrim = vincontrol.Application.ViewModels.CommonManagement.ManheimTrim;

namespace vincontrol.Helper
{
    public class ManheimServiceBAK
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

        public ManheimServiceBAK()
        {
            CookieContainer = new CookieContainer();
            CookieCollection = new CookieCollection();
            ManheimWholesale = new ManheimWholesaleViewModel();
            ManheimWholesales = new List<ManheimWholesaleViewModel>();
            ManheimTransactions = new List<ManheimTransactionViewModel>();
            ManheimTrims = new List<ManheimTrim>();
        }

        #region Public Methods
        public void LogOn(string userName, string password)
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

        //public void Execute(string vin)
        //{
        //    GetDefaultManheimLogin();
        //    // Step 1:
        //    WebRequestGet();
        //    // Step 2:
        //    GetAuthenticityToken();            
        //    // Step 3:
        //    WebRequestPost();
        //    // Step 4:
        //    GetSid();
        //    if (!string.IsNullOrEmpty(Sid))
        //    {
        //        // Step 5:
        //        BuildReportParameters(vin);
        //        // Step 6:
        //        //WebRequestGetByVin();
        //        WebRequestGetByVinWithMultipleTrims();
        //    }
        //}

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

        //public void GetMake(string userName, string password)
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
        //    // Step 6:
        //    GetMake();
        //}

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

        //public void GetTrim(string year, string make, int[] models)
        //{
        //    UserName = DefaultUserName;
        //    Password = DefaultPassword;
        //    // Step 1:
        //    WebRequestGet();
        //    // Step 2:
        //    GetAuthenticityToken();
        //    // Step 3:
        //    WebRequestPost();
        //    // Step 4:
        //    GetSid();
        //    // Step 5:
        //    foreach (var model in models)
        //    {
        //        PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
        //                        year, make, model.ToString());
        //        Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
        //        Request.Method = "POST";
        //        Request.UserAgent = UserAgent;
        //        Request.ContentLength = PostData.Length;
        //        Request.ContentType = ContentType;
        //        Request.Referer = RefererUrl;
        //        Request.CookieContainer = CookieContainer;
        //        Request.CookieContainer.Add(CookieCollection);

        //        try
        //        {
        //            StreamWriter = new StreamWriter(Request.GetRequestStream());
        //            StreamWriter.Write(PostData);
        //            StreamWriter.Close();
        //            Response = (HttpWebResponse)Request.GetResponse();
        //            StreamReader = new StreamReader(Response.GetResponseStream());
        //            Result = StreamReader.ReadToEnd();
        //            StreamReader.Close();
        //        }
        //        catch (WebException ex)
        //        {
        //            if (ex.Status == WebExceptionStatus.ProtocolError) { }
        //            else { }
        //        }

        //        var xmlDocument = DownloadDocument(Result);
        //        var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
        //        if (options != null && options.Count > 0)
        //        {
        //            var maheimModel = GetModelByServiceId(Convert.ToInt32(model));
        //            var modelId = maheimModel.id;
        //            foreach (XmlNode option in options)
        //            {
        //                var bodyName = GetString(option, "text()", null, null, true);
        //                var bodyValue = GetString(option, "@value", null, null, true);
        //                var bodyId = Convert.ToInt32(bodyValue);
        //                //using (var context = new whitmanenterprisewarehouseEntities())
        //                //{
        //                //    if (GetTrimByServiceIdAndModelId(bodyId, modelId) == null)
        //                //    {
        //                //        var trimToInsert = new manheimtrim()
        //                //        {
        //                //            serviceId = bodyId,
        //                //            name = bodyName.ToLower(),
        //                //            modelId = modelId
        //                //        };

        //                //        context.AddTomanheimtrims(trimToInsert);
        //                //        context.SaveChanges();
        //                //    }
        //                //}
        //                ManheimTrims.Add(new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName });
        //            }
        //        }
        //    }

        //}

        //public ManheimTrim GetTrimItem(string year, string make, string model)
        //{
        //    UserName = DefaultUserName;
        //    Password = DefaultPassword;
        //    // Step 1:
        //    WebRequestGet();
        //    // Step 2:
        //    GetAuthenticityToken();
        //    // Step 3:
        //    WebRequestPost();
        //    // Step 4:
        //    GetSid();
        //    // Step 5:

        //        PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
        //                        year, make, model.ToString());
        //        Request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
        //        Request.Method = "POST";
        //        Request.UserAgent = UserAgent;
        //        Request.ContentLength = PostData.Length;
        //        Request.ContentType = ContentType;
        //        Request.Referer = RefererUrl;
        //        Request.CookieContainer = CookieContainer;
        //        Request.CookieContainer.Add(CookieCollection);

        //        try
        //        {
        //            StreamWriter = new StreamWriter(Request.GetRequestStream());
        //            StreamWriter.Write(PostData);
        //            StreamWriter.Close();
        //            Response = (HttpWebResponse)Request.GetResponse();
        //            StreamReader = new StreamReader(Response.GetResponseStream());
        //            Result = StreamReader.ReadToEnd();
        //            StreamReader.Close();
        //        }
        //        catch (WebException ex)
        //        {
        //            if (ex.Status == WebExceptionStatus.ProtocolError) { }
        //            else { }
        //        }

        //        var xmlDocument = DownloadDocument(Result);
        //        var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
        //        if (options != null && options.Count > 0)
        //        {
        //            var maheimModel = GetModelByServiceId(Convert.ToInt32(model));
        //            var modelId = maheimModel.id;
        //            foreach (XmlNode option in options)
        //            {
        //                var bodyName = GetString(option, "text()", null, null, true);
        //                var bodyValue = GetString(option, "@value", null, null, true);
        //                var bodyId = Convert.ToInt32(bodyValue);
        //                //using (var context = new whitmanenterprisewarehouseEntities())
        //                //{
        //                //    if (GetTrimByServiceIdAndModelId(bodyId, modelId) == null)
        //                //    {
        //                //        var trimToInsert = new manheimtrim()
        //                //        {
        //                //            serviceId = bodyId,
        //                //            name = bodyName.ToLower(),
        //                //            modelId = modelId
        //                //        };

        //                //        context.AddTomanheimtrims(trimToInsert);
        //                //        context.SaveChanges();
        //                //    }
        //                //}
        //                return new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName };
        //            }
        //        }
        //        return null;


        //}


        //public void GetTrim(string year, string make, int[] models, string userName, string password)
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
        //    if (!string.IsNullOrEmpty(Sid))
        //    {
        //        // Step 5:
        //        foreach (var model in models)
        //        {
        //            PostData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}",
        //                            year, make, model.ToString());
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
        //            var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");
        //            if (options != null && options.Count > 0)
        //            {
        //                var modelId = GetModelByServiceId(Convert.ToInt32(model)).id;
        //                foreach (XmlNode option in options)
        //                {
        //                    var bodyName = GetString(option, "text()", null, null, true);
        //                    var bodyValue = GetString(option, "@value", null, null, true);
        //                    var bodyId = Convert.ToInt32(bodyValue);
        //                    //using (var context = new whitmanenterprisewarehouseEntities())
        //                    //{
        //                    //    if (GetTrimByServiceIdAndModelId(bodyId, modelId) == null)
        //                    //    {
        //                    //        var trimToInsert = new manheimtrim()
        //                    //        {
        //                    //            serviceId = bodyId,
        //                    //            name = bodyName.ToLower(),
        //                    //            modelId = modelId
        //                    //        };
        //                    //        context.AddTomanheimtrims(trimToInsert);
        //                    //        context.SaveChanges();
        //                    //    }
        //                    //}
        //                    ManheimTrims.Add(new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName });
        //                }
        //            }
        //        }
        //    }
        //}

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
                        var modelId = GetModelByServiceId(Convert.ToInt32(model)).ManheimModelId;
                        foreach (XmlNode option in options)
                        {
                            var bodyName = GetString(option, "text()", null, null, true);
                            var bodyValue = GetString(option, "@value", null, null, true);
                            var bodyId = Convert.ToInt32(bodyValue);

                            ManheimTrims.Add(new ManheimTrim() { Selected = false, ServiceId = bodyId, Name = bodyName });
                        }
                    }
                }
            }
        }

        public int MatchingMake(string make)
        {
            using (var context = new VincontrolEntities())
            {
                var matchingMake = context.ManheimMakes.FirstOrDefault(i => i.Name.ToLower().Equals(make.ToLower()));
                return matchingMake == null ? 0 : matchingMake.ServiceId;
            }
        }

        public int MatchingModel(string model, int makeServiceId)
        {
            using (var context = new VincontrolEntities())
            {
                var makeId = GetMakeByServiceId(makeServiceId).ManheimMakeId;
                var matchingModel = context.ManheimMakeModels.FirstOrDefault(i => i.ManheimModel.Name.ToLower().Contains(model.ToLower()) && i.ManheimYearMake.MakeId == makeId);
                return matchingModel == null ? 0 : matchingModel.ManheimModel.ServiceId;
            }
        }

        public int[] MatchingModels(string model, int makeServiceId)
        {
            using (var context = new VincontrolEntities())
            {
                var makeId = GetMakeByServiceId(makeServiceId).ManheimMakeId;
                var matchingModels = context.ManheimMakeModels.Where(i => i.ManheimModel.Name.ToLower().Contains(model.ToLower()) && i.ManheimYearMake.MakeId == makeId).Select(i => i.ManheimModel.ServiceId).ToArray();
                return matchingModels;
            }
        }

        public int MatchingTrim(string trim, int modelServiceId)
        {
            using (var context = new VincontrolEntities())
            {
                var modelId = GetModelByServiceId(modelServiceId).ManheimModelId;
                var matchingTrim = context.ManheimModelTrims.FirstOrDefault(i => i.ManheimTrim.Name.ToLower().Contains(trim.ToLower()) && i.ManheimMakeModel.ModelId == modelId);
                return matchingTrim == null ? 0 : matchingTrim.ManheimTrim.ServiceId;
            }
        }

        public List<vincontrol.Data.Model.ManheimTrim> MatchingTrimsByModelId(int modelServiceId)
        {
            using (var context = new VincontrolEntities())
            {
                var modelId = GetModelByServiceId(modelServiceId).ManheimModelId;
                var matchingTrims = context.ManheimModelTrims.Where(i => i.ManheimMakeModel.ModelId == modelId).OrderByDescending(i => i.ManheimModelTrimId).Skip(0).Take(5).Select(i => i.ManheimTrim).Distinct();
                return matchingTrims.ToList();
            }
        }

        private ManheimMake GetMakeByServiceId(int id)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimMakes.FirstOrDefault(m => m.ServiceId == id);
            }
        }

        private ManheimMake GetMakeById(int id)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimMakes.FirstOrDefault(m => m.ManheimMakeId == id);
            }
        }

        private List<ManheimMake> GetAllMakes()
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimMakes.ToList();
            }
        }

        private ManheimModel GetModelByServiceId(int id)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimModels.FirstOrDefault(m => m.ServiceId == id);
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

        public void WebRequestGet(string url)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);            
            Request.UserAgent = UserAgent;
            Request.CookieContainer = CookieContainer;
            Request.CookieContainer.Add(CookieCollection);            
            using (var response = (HttpWebResponse)Request.GetResponse())
            {

                CookieCollection = response.Cookies;

                StreamReader = new StreamReader(response.GetResponseStream());
                Result = StreamReader.ReadToEnd();
                StreamReader.Close();
            }
        }

        public void PostSimulcastData(string url, SimulcastContract contract)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://simulcast.manheim.com/simulcast/registerDealerships.do");
            request.Method = "POST";
            request.Accept = "application/atom+xml,application/xml";
            request.UserAgent = UserAgent;

            PostData = String.Format("locale={0}&vehicleGroupGoto={1}&redirect={2}&redirectVg={3}&isManheimAVPluginInstalled={4}&manheim_mobile_application_flag={5}&modifyOrContinue={6}&saleEventKey={7}&vehicleGroupKey={8}&dealerships={9}&initialDealer,a={10}&email={11}&cellphoneNPA={12}&cellphoneNXX={13}&cellphoneStationCode={14}&faxNPA={15}&faxNXX={16}&faxStationCode={17}&paymentMethod={18}&floorPlanProvider={19}&comments={20}&postSaleInspection={21}&title={22}&transportation={23}&transportContactName={24}&transportNPA={25}&transportNXX={26}&transportStationCode={27}&confirmPreferences={28}"
                , contract.locale
                , contract.vehicleGroupGoto
                , contract.redirect
                , contract.redirectVg
                , contract.isManheimAVPluginInstalled
                , contract.manheim_mobile_application_flag
                , contract.modifyOrContinue
                , contract.saleEventKey
                , contract.vehicleGroupKey
                , contract.dealerships
                , contract.initalDealer
                , contract.email
                , contract.cellphoneNPA
                , contract.cellphoneNXX
                , contract.cellphoneStationCode
                , contract.faxNPA
                , contract.faxNXX
                , contract.faxStationCode
                , contract.paymentMethod
                , contract.floorPlanProvider
                , contract.comments
                , contract.postSaleInspection
                , contract.title
                , contract.transportation
                , contract.transportContactName
                , contract.transportNPA
                , contract.transportNXX
                , contract.transportStationCode
                , contract.confirmPreferences
                );

            request.ContentLength = PostData.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json; charset=utf-8";
            //PostData = CreateDataToSimulcastPopUp(contract);
            request.Referer = url;
            request.Host = "simulcast.manheim.com";
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Post to the login form.
            var streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(PostData);
            streamWriter.Close();

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            Result = streamReader.ReadToEnd();
            streamReader.Close();
        }

        public string GetSimulcastUrl(string content)
        {
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);

            return "https://simulcast.manheim.com" + CommonHelper.ConvertToString((JValue)(jsonObj["windowUrl"]));
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
            using (var response = (HttpWebResponse)Request.GetResponse())
            {

                CookieCollection = response.Cookies;

                StreamReader = new StreamReader(response.GetResponseStream());
                Result = StreamReader.ReadToEnd();
                StreamReader.Close();
            }
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
                using (Response = (HttpWebResponse)Request.GetResponse())
                {

                    // Have some cookies.
                    CookieCollection = Response.Cookies;

                    // Read the response
                    StreamReader = new StreamReader(Response.GetResponseStream());
                    Result = StreamReader.ReadToEnd();
                    StreamReader.Close();
                }
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
                using (Response = (HttpWebResponse)Request.GetResponse())
                {
                    StreamReader = new StreamReader(Response.GetResponseStream());
                    Result = StreamReader.ReadToEnd();
                    StreamReader.Close();
                }
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

        private string CreateDataToSimulcastPopUp(SimulcastContract contract)
        {
            return JsonConvert.SerializeObject(contract);
        }
        #endregion
    }

    public class ReplaceType
    {
        public string Original = string.Empty;
        public string Replace = string.Empty;
        public bool IsExpression;
    }

    public class SimulcastContract
    {
        public string locale { get; set; }
        public string vehicleGroupGoto { get; set; } //a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43
        public string redirect { get; set; }
        public string redirectVg { get; set; }
        public string isManheimAVPluginInstalled { get; set; } //false
        public string manheim_mobile_application_flag { get; set; }
        public string modifyOrContinue { get; set; }
        public string saleEventKey { get; set; } //CADE_76108_01
        public string vehicleGroupKey { get; set; } //a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900
        public string dealerships { get; set; } //5131094,a:CADE_s:76108_c:OPEN_l:1_v:1_q:1-43,a:CADE_s:76108_c:OPEN_l:1_v:1_q:76-900,a:CADE_s:76108_c:REDL_l:1_v:1_q:44-75,a:CADE_s:76108_c:OPEN_l:1_v:31_q:1-900
        public string initalDealer { get; set; }
        public string email { get; set; } //sbrown@jlr-mv.com
        public string cellphoneNPA { get; set; } //714
        public string cellphoneNXX { get; set; } //348
        public string cellphoneStationCode { get; set; } //8351
        public string faxNPA { get; set; } //714
        public string faxNXX { get; set; } //242
        public string faxStationCode { get; set; } //1875
        public string paymentMethod { get; set; } //CHECK
        public string floorPlanProvider { get; set; }
        public string comments { get; set; }
        public string postSaleInspection { get; set; } //7
        public string title { get; set; } //LOT
        public string transportation { get; set; } //DEALER
        public string transportContactName { get; set; } ///al american transport
        public string transportNPA { get; set; } //714
        public string transportNXX { get; set; } //400
        public string transportStationCode { get; set; } //7057
        public string confirmPreferences { get; set; } //on
    }
}