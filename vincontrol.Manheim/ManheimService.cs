using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Constant;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using VINControl.Logging;
using ManheimTrim = vincontrol.Application.ViewModels.CommonManagement.ManheimTrim;

namespace vincontrol.Manheim
{
    public class ManheimService : IManheimService
    {
        protected IUnitOfWork UnitOfWork;
        protected ILoggingService Logging;
        
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
        private const string DefaultPassword = "Happy123";
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
        public int NumberOfManheimTransactions { get; set; }
        public string HighPrice { get; set; }
        public string AveragePrice { get; set; }
        public string LowPrice { get; set; }
        public string AverageOdometer { get; set; }
        public List<vincontrol.Application.ViewModels.CommonManagement.ManheimTrim> ManheimTrims { get; set; }
        public string RecallUrl { get; set; }
        #endregion

        public ManheimService() : this(new SqlUnitOfWork())
        {
            CookieContainer = new CookieContainer();
            CookieCollection = new CookieCollection();
            ManheimWholesale = new ManheimWholesaleViewModel();
            ManheimWholesales = new List<ManheimWholesaleViewModel>();
            ManheimTransactions = new List<ManheimTransactionViewModel>();
            ManheimTrims = new List<ManheimTrim>();
            if (Logging == null) Logging = new LoggingService();
        }

        public ManheimService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
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

        public void Execute(string country, string year, string make, string model, string body, string region, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(Sid))
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
            }
            if (!string.IsNullOrEmpty(Sid))
            {
                // Step 5:
                BuildReportParameters(country, year, make, model, body, region);
                // Step 6:
                GetReportContent(pageIndex, pageSize);
            }
        }

        public void Execute(string country, string year, string make, string model, string body, string region, string userName, string password, int pageIndex = 1, int pageSize = 10)
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
                GetReportContent(pageIndex, pageSize);
            }
        }

        public void Execute(string trim, string region, string url)
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
            // Step 5:
            BuildReportParameters(trim, region, url);
            // Step 6:
            GetTransactionContent();
        }

        public void GetTrim(string year, string make, int[] models)
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
                    var modelId = maheimModel.ManheimModelId;
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
            return UnitOfWork.Manheim.MatchingMake(make);
        }

        public int MatchingModel(string model, int makeServiceId)
        {
            return UnitOfWork.Manheim.MatchingModel(model, makeServiceId);
        }

        public int[] MatchingModels(string model, int makeServiceId)
        {
            return UnitOfWork.Manheim.MatchingModels(model, makeServiceId);
        }

        public int MatchingTrim(string trim, int modelServiceId)
        {
            return UnitOfWork.Manheim.MatchingTrim(trim, modelServiceId);
        }

        public List<vincontrol.Data.Model.ManheimTrim> MatchingTrimsByModelId(int modelServiceId)
        {
            return UnitOfWork.Manheim.MatchingTrimsByModelId(modelServiceId);
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

        public string WebRequestGet(string url)
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
                var result = Result;
                StreamReader.Close();
                return result;
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

            return "https://simulcast.manheim.com" + ConvertToString((JValue)(jsonObj["windowUrl"]));
        }

        public ManheimAuction GetManheimAuctionFromUrl(string url)
        {
            if (!String.IsNullOrEmpty(url) && url.Contains("vin=") && url.Contains("&auction=") &&
                url.Contains("&fromPresales=") && url.Contains("&saleDate=") && url.Contains("&locale"))
            {
                var indexOfVin = url.IndexOf("vin=");

                var indexOfAuction = url.IndexOf("&auction=");

                var indexOfPreSales = url.IndexOf("&fromPresales=");

                var indexOfsaleDate = url.IndexOf("&saleDate=");

                var indexOflocale = url.IndexOf("&locale");

                var vin = url.Substring(indexOfVin + 4, indexOfAuction - indexOfVin - 4);

                var region = url.Substring(indexOfAuction + 9, indexOfPreSales - indexOfAuction - 9);

                var auctionDate = url.Substring(indexOfsaleDate + 10, indexOflocale - indexOfsaleDate - 10);

                var manheimAuction = new ManheimAuction()
                {
                    Vin = vin,
                    Aucton = region,
                    Url = url
                };

                try
                {
                    DateTime theTime = DateTime.ParseExact(auctionDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    manheimAuction.SaleDate = theTime;
                    return manheimAuction;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public void CleanUpData()
        {
            var context = new VinsellEntities();

            context.ExecuteStoreCommand("CleanUpData");
        }

        public void GetManheimAuctionDataInRegion(string auctionSaleLink, string auctionCode, XmlDocument xmlDocument)
        {
            using (var context = new VinsellEntities())
            {
                try
                {
                    //var listOfVehicleInLaneLink = new List<string>();
                    //listOfVehicleInLaneLink = GetVehicleInLaneLinks(xmlDocument, auctionSaleLink);

                    var links = GetCategoryInLaneLinks(xmlDocument, auctionSaleLink);

                    var manheimList = new List<ManheimAuction>();

                    foreach (var link in links)
                    {
                        var listOfPowerSearchLink = new List<string>();
                        listOfPowerSearchLink = GetPowerSearchLinks(xmlDocument, new List<string>() { link.Url.Replace("format=enhanced", "format=standard") });
                        foreach (var tmp in listOfPowerSearchLink.Where(x => !String.IsNullOrEmpty(x.Trim())))
                        {
                            var manheimAuction = GetManheimAuctionFromUrl(tmp);
                            manheimAuction.LaneDescription = link.LaneDescription;
                            manheimAuction.Category = link.Category;
                            if (manheimAuction != null)
                                manheimList.Add(manheimAuction);
                        }
                    }

                    // Get List Of Vehicles
                    if (manheimList.Count == 0) return;

                    var manheimAuctionList = context.manheim_vehicles.Where(i => i.Auction == auctionCode).ToList();

                    foreach (var vehicle in manheimAuctionList)
                    {
                        if (manheimList.All(x => x.Vin != vehicle.Vin))
                        {
                            var manheimSold = new manheim_vehicles_sold()
                            {
                                VehicleId = vehicle.Id,
                                AsIs = vehicle.AsIs,
                                Auction = vehicle.Auction,
                                Vin = vehicle.Vin,
                                DateStamp = vehicle.DateStamp,
                                VehicleType = vehicle.VehicleType,
                                SaleDate = vehicle.SaleDate,
                                BodyStyle = vehicle.BodyStyle,
                                Cr = vehicle.Cr,
                                DateStampSold = DateTime.Now,
                                Doors = vehicle.Doors,
                                DriveTrain = vehicle.DriveTrain,
                                Engine = vehicle.Engine,
                                ExteriorColor = vehicle.ExteriorColor,
                                FuelType = vehicle.FuelType,
                                Images = vehicle.Images,
                                InteriorColor = vehicle.InteriorColor,
                                Lane = vehicle.Lane,
                                Litters = vehicle.Litters,
                                Make = vehicle.Make,
                                Mileage = vehicle.Mileage,
                                Mmr = vehicle.Mmr,
                                Model = vehicle.Model,
                                Run = vehicle.Run,
                                Seller = vehicle.Seller,
                                Status = vehicle.Status,
                                Transmission = vehicle.Transmission,
                                Trim = vehicle.Trim,
                                Url = vehicle.Url,
                                Year = vehicle.Year,
                                Equipment = vehicle.Equipment,
                                MmrAbove = vehicle.MmrAbove,
                                MmrBelow = vehicle.MmrBelow,
                                InteriorType = vehicle.InteriorType,
                                Airbags = vehicle.Airbags,
                                Stereo = vehicle.Stereo,
                                Comment = vehicle.Comment,
                                LaneDescription = vehicle.LaneDescription,
                                Category = vehicle.Category,
                                CrUrl = vehicle.CrUrl
                            };

                            context.manheim_vehicles_sold.AddObject(manheimSold);

                            context.manheim_vehicles.DeleteObject(vehicle);

                            var manheimFavorites = context.manheim_favorites.Where(x => x.VehicleId == vehicle.Id).ToList();

                            var manheimNodes = context.manheim_notes.Where(x => x.VehicleId == vehicle.Id).ToList();

                            var favoriteIds = new List<int>();

                            var notesId = new List<int>();

                            if (manheimFavorites.Any())
                            {
                                favoriteIds = manheimFavorites.Select(x => x.Id).ToList();

                                foreach (var tmp in manheimFavorites)
                                {
                                    tmp.VehicleId = null;
                                }
                            }


                            if (manheimNodes.Any())
                            {
                                notesId = manheimNodes.Select(x => x.Id).ToList();

                                foreach (var tmp in manheimNodes)
                                {
                                    tmp.VehicleId = null;
                                }
                            }

                            context.SaveChanges();


                            foreach (var tmp in context.manheim_favorites.Where(x => x.VehicleId == null).ToList())
                            {
                                if (favoriteIds.Contains(tmp.Id))
                                {
                                    tmp.SoldVehicleId = manheimSold.Id;
                                }
                            }
                            foreach (var tmp in context.manheim_notes.Where(x => x.VehicleId == null).ToList())
                            {
                                if (notesId.Contains(tmp.Id))
                                {
                                    tmp.SoldVehicleId = manheimSold.Id;
                                }
                            }


                            context.SaveChanges();
                        }
                    }


                    foreach (var manheimAutionPar in manheimList)
                    {
                        if (manheimAuctionList.All(x => x.Vin != manheimAutionPar.Vin))
                        {
                            var newRecord = GetManheimAuctionDataWithUrl(manheimAutionPar, xmlDocument, context);

                            if (newRecord != null)
                            {
                                Console.WriteLine("Auction: {0} - NEW: {1}", newRecord.Auction, newRecord.Url);
                                newRecord = ParseMakeModelTrim(newRecord);

                                context.manheim_vehicles.AddObject(newRecord);

                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            var existingRecord = manheimList.FirstOrDefault(x => x.Vin == manheimAutionPar.Vin);

                            if (existingRecord != null)
                            {
                                Console.WriteLine("Auction: {0} - UPDATE: {1}", existingRecord.Aucton, existingRecord.Url);
                                existingRecord.Vin = manheimAutionPar.Vin;
                                existingRecord.SaleDate = manheimAutionPar.SaleDate;
                                existingRecord.Aucton = manheimAutionPar.Aucton;
                                existingRecord.VehicleType = manheimAutionPar.VehicleType;
                                existingRecord.BodyStyle = manheimAutionPar.BodyStyle;
                                existingRecord.Cr = manheimAutionPar.Cr;
                                existingRecord.Doors = manheimAutionPar.Doors;
                                existingRecord.DriveTrain = manheimAutionPar.DriveTrain;
                                existingRecord.ExteriorColor = manheimAutionPar.ExteriorColor;
                                existingRecord.FuelType = manheimAutionPar.FuelType;
                                existingRecord.Images = manheimAutionPar.Images;
                                existingRecord.InteriorColor = manheimAutionPar.InteriorColor;
                                existingRecord.Lane = manheimAutionPar.Lane;
                                existingRecord.Litters = manheimAutionPar.Litters;
                                existingRecord.Mileage = manheimAutionPar.Mileage;
                                existingRecord.Make = manheimAutionPar.Make;
                                existingRecord.Mmr = manheimAutionPar.Mmr;
                                existingRecord.Model = manheimAutionPar.Model;
                                existingRecord.Run = manheimAutionPar.Run;
                                existingRecord.Seller = manheimAutionPar.Seller;
                                existingRecord.Status = manheimAutionPar.Status;
                                existingRecord.Transmission = manheimAutionPar.Transmission;
                                existingRecord.Trim = manheimAutionPar.Trim;
                                existingRecord.Url = manheimAutionPar.Url;
                                existingRecord.Year = manheimAutionPar.Year;
                                existingRecord.MmrAbove = manheimAutionPar.MmrAbove;
                                existingRecord.MmrBelow = manheimAutionPar.MmrBelow;
                                existingRecord.CrUrl = manheimAutionPar.CrUrl;
                                existingRecord.LaneDescription = manheimAutionPar.LaneDescription;
                                existingRecord.Category = manheimAutionPar.Category;
                                context.SaveChanges();
                            }


                        }
                    }


                    Logging.Info("COMPLETE at region " + auctionCode);
                }
                catch (Exception ex)
                {
                    Logging.Error("ERROR in GetManheimAuctionDataInRegion at region " + auctionCode + " ", ex);
                }
            }

            
        }

        private manheim_vehicles ParseMakeModelTrim(manheim_vehicles vehicle)
        {
            if (vehicle.Make.Equals("Mini"))
            {
                if (!vehicle.Model.Equals("Not Available") && !vehicle.Model.ToLower().Contains("cooper"))
                {
                    vehicle.Model = "Cooper " + vehicle.Model;
                }
            }
            if(!String.IsNullOrEmpty(vehicle.Trim))

            vehicle.Trim = vehicle.Trim.Replace("Not Specified", "");

            return vehicle;
        }

        public manheim_vehicles GetManheimAuctionDataWithUrl(ManheimAuction manheimAuctionPar, XmlDocument xmlDocument, VinsellEntities context)
        {
            try
            {
                var region = manheimAuctionPar.Url.Split('&').ToArray().FirstOrDefault(i => i.Contains("auction=")).Split('=').ToArray()[1];
                var Result = WebRequestGet(manheimAuctionPar.Url);
                xmlDocument = DownloadDocument(Result);

                if (Result.IndexOf("No results found. Please search again.", System.StringComparison.Ordinal) > 0) return null;

                var manheimAuction = new ManheimAuction()
                {
                    Cr = "Condition Report",
                    Mmr = "0",
                    Lane = 0,
                    Run = 0,
                    Status = "",
                    Seller = string.Empty,
                    SaleDate = manheimAuctionPar.SaleDate,
                    Images = string.Empty,
                    Aucton = region,
                    Url = manheimAuctionPar.Url,
                    LaneDescription = manheimAuctionPar.LaneDescription,
                    Category = manheimAuctionPar.Category
                };

                try
                {
                    manheimAuction.Images = xmlDocument.SelectSingleNode("//*[@id='image']").Attributes["src"].Value;
                    //if (manheimAuction.Images.Contains("scaled_images"))//scaled images
                    //{
                    //    var imagePattern = "var imageList = new Array ([^;]*)";
                    //    var newRegex = new Regex(imagePattern);
                    //    var tmpImages = newRegex.Match(Result).Groups[1].Value;
                    //    tmpImages = tmpImages.Replace("(", "").Replace(")", "").Replace("'", "");
                    //    manheimAuction.Images = tmpImages.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray().Aggregate((current, next) => current.Trim() + ";" + next.Trim());
                    //}
                    //else
                    {
                        if (manheimAuction.Images.Contains("no-image")) manheimAuction.Images = "https://www.manheim.com//members/powersearch/" + manheimAuction.Images;
                        else
                        {
                            manheimAuction.Images = string.Empty;
                            var smallImages = xmlDocument.SelectSingleNode("//*[@id='thumbnail-slider']");
                            if (smallImages != null)
                            {
                                foreach (XmlNode item in smallImages.ChildNodes)
                                {
                                    if (!(item.Name.Equals("a"))) continue;
                                    var newImage = item.FirstChild.Attributes["src"].Value.Replace("tn_small", "tn_large");
                                    if (!manheimAuction.Images.Equals(newImage))
                                        manheimAuction.Images += ";" + newImage;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Error("ERROR in GetManheimAuctionDataWithUrl with Images: ", ex);
                    return null;
                }

                try
                {
                    if (xmlDocument.SelectSingleNode("//*[@id='mmrHover']") != null)
                    {
                        manheimAuction.Mmr = (xmlDocument.SelectSingleNode("//*[@id='mmrHover']").InnerText);
                        try
                        {
                            if (manheimAuction.Mmr.Contains("Internet MMR"))
                            {
                                manheimAuction.Mmr = "$0";
                                manheimAuction.MmrAbove = "$0";
                                manheimAuction.MmrBelow = "$0";
                            }
                            else
                            {
                                var mousehover = xmlDocument.SelectSingleNode("//*[@id='mmrHover']").Attributes["onmouseover"].Value.Replace("getMMRPrices", "").Replace("(", "").Replace(")", "").Replace("'", "");
                                var tempArray = mousehover.Split(',').ToArray();
                                Result = GetMMRPrices(tempArray[1].Trim(), tempArray[2].Trim(), tempArray[3].Trim(), tempArray[4].Trim(), tempArray[6].Trim().Split(';').ToArray()[0]);
                                if (!String.IsNullOrEmpty(Result))
                                {
                                    var jsonObj = (JObject)JsonConvert.DeserializeObject(Result);
                                    var status = ConvertToString((JValue)(jsonObj["status"]));
                                    if (status.ToLower().Equals("success"))
                                    {
                                        manheimAuction.MmrAbove = ConvertToString((JValue)(jsonObj["aboveAverage"])).Replace("¤ ", "$");
                                        manheimAuction.MmrBelow = ConvertToString((JValue)(jsonObj["belowAverage"])).Replace("¤ ", "$");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Error("ERROR in GetManheimAuctionDataWithUrl with MMR: ", ex);
                        }
                    }

                    var statusNode = xmlDocument.SelectSingleNode("//*[@class='timeIndicatorPresale']");
                    if (statusNode != null)
                        manheimAuction.Status = statusNode.InnerText;
                    var sellerNode = xmlDocument.SelectSingleNode("//*[@class='span1-2 mui-dl-default']/*[1]/*[last()]");
                    if (sellerNode != null)
                        manheimAuction.Seller = sellerNode.InnerText.Trim();

                    var tmp = xmlDocument.SelectSingleNode("//*[@title='View run list']");
                    if (tmp != null)
                    {
                        manheimAuction.Lane = Convert.ToInt32(tmp.InnerText.Split('/').ToArray()[0].Trim().Replace("Lane", "").Trim());
                        manheimAuction.Run = Convert.ToInt32(tmp.InnerText.Split('/').ToArray()[1].Trim().Replace("Run", "").Trim());
                    }

                    var crNode = xmlDocument.SelectSingleNode("//*[@class='span1-2 mui-dl-default']/*[1]/*[2]/*[2]");
                    if (crNode != null)
                    {
                        manheimAuction.Cr = crNode.InnerText;
                        var onclickAtt = crNode.Attributes["onclick"];

                        if (onclickAtt != null)
                        {
                            var openClickText = onclickAtt.Value.Replace("openECR( ", "").Replace("\'", "");
                            openClickText = openClickText.Substring(0, openClickText.IndexOf(",", System.StringComparison.Ordinal));
                            manheimAuction.CrUrl = openClickText;
                        }
                    }
                    else
                    {
                        manheimAuction.Cr = "Condition Report: Not Available";
                        manheimAuction.CrUrl = "";
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                try
                {
                    var dateText = manheimAuctionPar.Url.Split('&').ToArray().FirstOrDefault(i => i.Contains("saleDate")).Split('=').ToArray()[1];
                    manheimAuction.SaleDate = new DateTime(Convert.ToInt32(dateText.Substring(0, 4)),
                                                              Convert.ToInt32(dateText.Substring(4, 2)),
                                                              Convert.ToInt32(dateText.Substring(6, 2)));
                }
                catch (Exception)
                {
                    return null;
                }

                var newRecord = new manheim_vehicles()
                {
                    Cr = manheimAuction.Cr,
                    CrUrl = manheimAuction.CrUrl,
                    Mmr = String.IsNullOrEmpty(manheimAuction.Mmr) ? 0 : ConvertStringToInterger((manheimAuction.Mmr.Replace("$", "").Replace(",", "").Replace(".", ""))),
                    MmrAbove = String.IsNullOrEmpty(manheimAuction.MmrAbove) ? 0 : ConvertStringToInterger((manheimAuction.MmrAbove.Replace("$", "").Replace(",", "").Replace(".", ""))),
                    MmrBelow = String.IsNullOrEmpty(manheimAuction.MmrBelow) ? 0 : ConvertStringToInterger((manheimAuction.MmrBelow.Replace("$", "").Replace(",", "").Replace(".", ""))),
                    Auction = manheimAuction.Aucton,
                    Url = manheimAuction.Url,
                    Images = manheimAuction.Images,
                    SaleDate = manheimAuction.SaleDate,
                    Status = manheimAuction.Status,
                    Seller = UpperFirstLetterOfEachWord(manheimAuction.Seller),
                    Lane = manheimAuction.Lane,
                    Run = manheimAuction.Run,
                    DateStamp = DateTime.Now,
                    LaneDescription = manheimAuction.LaneDescription,
                    Category = manheimAuction.Category
                };

                var table = xmlDocument.SelectNodes("//*[@id='vdpTab_detail-1']/*[1]/*[@class='span1-2']/*[@class='ui-hm']");

                var tableEquipment = xmlDocument.SelectSingleNode("//*[@class='linkBullets mui-column']");

                var comment = xmlDocument.SelectSingleNode(".//*[@id='comments']/*[2]/*[1]");

                if (table != null)
                {
                    foreach (XmlNode tr in table)
                    {
                        foreach (XmlNode td in tr.ChildNodes)
                        {
                            if (!(td.Name.Equals("label"))) continue;

                            var name = td.InnerText;
                            var value = td.NextSibling.NextSibling.InnerText;

                            if (name.Equals(ManheimConstants.Year))
                            {
                                newRecord.Year = Convert.ToInt32(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Doors))
                            {
                                if (String.IsNullOrEmpty(value) || value.Equals(ManheimConstants.NotSpecified) || value.Equals(ManheimConstants.NotAvailable))
                                {
                                    newRecord.Doors = 0;
                                }
                                else
                                {
                                    var doors = 0;
                                    Int32.TryParse(value, out doors);
                                    newRecord.Doors = doors;
                                }

                                continue;
                            }
                            if (name.Equals(ManheimConstants.Mileage))
                            {
                                if (value.Equals(ManheimConstants.NotSpecified) || value.Equals(ManheimConstants.NotAvailable))
                                {
                                    newRecord.Mileage = 0;
                                }
                                else if (value.Contains("mi"))
                                {
                                    var mileage = 0; Int32.TryParse(value.Replace(" mi", "").Replace(",", ""), out mileage);
                                    newRecord.Mileage = mileage;
                                }
                                else if (value.Contains("km"))
                                {
                                    var mileage = 0; Int32.TryParse(value.Replace(" km", "").Replace(",", ""), out mileage);
                                    newRecord.Mileage = mileage*1000;
                                    
                                }

                                continue;
                            }
                            if (name.Equals(ManheimConstants.AsIs))
                            {
                                newRecord.AsIs = value.Trim().Equals("Yes") ? true : false;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Vin))
                            {
                                newRecord.Vin = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Make))
                            {
                                newRecord.Make = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Model))
                            {
                                newRecord.Model = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Trim))
                            {
                                newRecord.Trim = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.FuelType))
                            {
                                newRecord.FuelType = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Engine))
                            {
                                newRecord.Engine = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Litters))
                            {
                                newRecord.Litters = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.BodyStyle))
                            {
                                newRecord.BodyStyle = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.VehicleType))
                            {
                                newRecord.VehicleType = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.DriveTrain))
                            {
                                newRecord.DriveTrain = value;
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Transmission))
                            {
                                newRecord.Transmission = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.ExteriorColor))
                            {
                                newRecord.ExteriorColor = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.InteriorColor))
                            {
                                newRecord.InteriorColor = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.InteriorType))
                            {
                                newRecord.InteriorType = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Airbags))
                            {
                                newRecord.Airbags = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                            if (name.Equals(ManheimConstants.Stereo))
                            {
                                newRecord.Stereo = UpperFirstLetterOfEachWord(value);
                                continue;
                            }
                        }
                    }
                }

                if (tableEquipment != null)
                {
                    var builder = new StringBuilder();

                    foreach (XmlNode tmp in tableEquipment)
                    {
                        if (!(tmp.Name.Equals("li"))) continue;
                        
                        {
                            builder.Append(tmp.InnerText.Trim());
                            builder.Append(",");
                        }
                    }

                    if (!String.IsNullOrEmpty(builder.ToString()))
                    {
                        newRecord.Equipment = builder.ToString().Substring(0, builder.ToString().Length - 1);
                    }
                }

                if (comment != null)
                {
                    newRecord.Comment = comment.InnerText;
                }

                if (newRecord.Year > 0)
                    return newRecord;

                return null;
            }
            catch (Exception ex)
            {
                Logging.Error("ERROR in GetManheimAuctionDataWithUrl: ", ex);
                return null;
            }
        }

        public List<ManehimRegion> GetAuctionSaleLinks(XmlDocument xmlDocument)
        {
            var listOfAuctionSaleLink = new List<ManehimRegion>();
            var tier0Nodes = xmlDocument.SelectNodes("//*[@id='tab0']/*[3]/*[@class='tier0']");
            if (tier0Nodes != null && tier0Nodes.Count > 0)
            {
                using (var context = new VinsellEntities())
                {
                    foreach (XmlNode tier0 in tier0Nodes)
                    {
                        var state = tier0.SelectSingleNode("./*[1]/*[1]").InnerText;
                        var tier1 = tier0.SelectSingleNode("./*[1]/*[@class='tier1']");
                        foreach (XmlNode subTier1 in tier1.ChildNodes)
                        {
                            if (!(subTier1.Name.Equals("li"))) continue;
                            var region = subTier1.FirstChild.InnerText.TrimEnd();
                            var url = subTier1.FirstChild.Attributes["href"].Value;
                            var code = url.Split('&').ToArray().FirstOrDefault(i => i.Contains("auctionID")).Split('=').ToArray()[1];

                            var newManhimRegion = new ManehimRegion()
                            {
                                Link ="https://www.manheim.com/members/presale/control/" + subTier1.FirstChild.Attributes["href"].Value,
                                Region = region,
                                Code = code,
                                State = state,
                            };

                            listOfAuctionSaleLink.Add(newManhimRegion);
                            if (!context.manheim_auctions.Any(i => i.Code == code))
                            {
                                var newRegion = new manheim_auctions()
                                {
                                    State = state,
                                    Code = code,
                                    Name = region,
                                    DateStamp = DateTime.Now
                                };
                                context.manheim_auctions.AddObject(newRegion);
                            }
                        }

                        context.SaveChanges();
                    }

                    var manehimAuctions = context.manheim_auctions.ToList();
                    try
                    {
                        foreach (var tmp in listOfAuctionSaleLink)
                        {
                            var auction = manehimAuctions.FirstOrDefault(x => x.Code == tmp.Code);

                            if (auction != null && !String.IsNullOrEmpty(auction.Region))
                            {
                                tmp.Region = auction.Region.Trim();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
          

            return listOfAuctionSaleLink.Where(x=>!String.IsNullOrEmpty(x.Region)).ToList();
        }

        public ManheimCredential GetManheimCredentialByDate()
        {
            using (var context = new VinsellEntities())
            {
                var dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek);
                var manheimCredentails = context.ManheimCredentials.OrderBy(x => x.CredentialId).Skip(dayOfWeek).Take(1);
                return manheimCredentails.FirstOrDefault();
            }
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

        public void GetModel(string userName, string password)
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
            GetModel();
        }

        public void GetTrim(string userName, string password)
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
            GetTrim();
        }

        public void GetTrimByVin(string userName, string password)
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
            GetTrimByVin();
        }

        public void GetTrimByVinForAppraisal(string userName, string password)
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
            GetTrimByVinForAppraisal();
        }

        public List<ManheimTransactionViewModel> GetManheimTransactions(string year, string make, string model, string trim, string region)
        {
            Execute("US", year, make, model, trim, region);
            return ManheimTransactions;
        }

        public List<ManheimWholesaleViewModel> ManheimReportNew(VincontrolEntities context, VehicleViewModel inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();

            try
            {
                if (context.ManheimValues.Any(x => x.VehicleId == inventory.VehicleId))
                {
                    var searchResult = context.ManheimValues.Where(x => x.VehicleId == inventory.VehicleId).ToList();

                    result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                    {
                        LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
                        AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
                        HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
                        Year = tmp.Year.GetValueOrDefault(),
                        MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                        ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                        TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                        TrimName = tmp.Trim
                    }));

                }
                else
                {
                    if (!string.IsNullOrEmpty(inventory.Vin))
                    {
                        ExecuteByVin(userName, password, inventory.Vin.Trim());
                        result = ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                            {
                                AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
                                AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
                                AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
                                Year = inventory.Year,
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                Expiration = GetNextFriday(),
                                Vin = inventory.Vin,
                                LastUpdated = DateTime.Now,
                                VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,
                                VehicleId = inventory.VehicleId
                            };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = MatchingMake(inventory.Make);
                        var matchingModel = 0;
                        var matchingModels = MatchingModels(inventory.Model, matchingMake);
                        var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            GetTrim(inventory.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            Execute("US", inventory.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(Convert.ToInt32(ManheimWholesale.LowestPrice) == 0 ||
                               Convert.ToInt32(ManheimWholesale.AveragePrice) == 0 ||
                               Convert.ToInt32(ManheimWholesale.HighestPrice) == 0) &&
                               Convert.ToInt32(ManheimWholesale.LowestPrice) > 0)
                            {
                                var subResult = new ManheimWholesaleViewModel
                                {
                                    LowestPrice = ManheimWholesale.LowestPrice,
                                    AveragePrice = ManheimWholesale.AveragePrice,
                                    HighestPrice = ManheimWholesale.HighestPrice,
                                    Year = inventory.Year,
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.ServiceId,
                                    TrimName = trim.Name
                                };

                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }


        public List<ManheimWholesaleViewModel> ManheimReportNew(VinsellEntities context, VehicleViewModel inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();

            try
            {
                if (context.manheim_ManheimValue.Any(x => x.VehicleId == inventory.VehicleId))
                {
                    var searchResult = context.manheim_ManheimValue.Where(x => x.VehicleId == inventory.VehicleId).ToList();

                    result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                    {
                        LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
                        AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
                        HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
                        Year = tmp.Year.GetValueOrDefault(),
                        MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                        ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                        TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                        TrimName = tmp.Trim
                    }));

                }
                else
                {
                    if (!string.IsNullOrEmpty(inventory.Vin))
                    {
                        ExecuteByVin(userName, password, inventory.Vin.Trim());
                        result = ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new manheim_ManheimValue()
                            {
                                AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
                                AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
                                AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
                                Year = inventory.Year,
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                Expiration = GetNextFriday(),
                                Vin = inventory.Vin,
                                LastUpdated = DateTime.Now,
                                VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,
                                VehicleId = inventory.VehicleId
                            };

                            context.AddTomanheim_ManheimValue(manheimRecord);
                        }

                        context.SaveChanges();
                    }

                }
            }
            catch
            {

            }

            return result;
        }

        public List<ManheimWholesaleViewModel> ManheimReport(VehicleViewModel inventory, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            using (var context = new VincontrolEntities())
            {
                try
                {
                    if (context.ManheimValues.Any(x => x.Vin == inventory.Vin) && !inventory.Year.Equals(0))
                    {
                        var searchResult = context.ManheimValues.Where(x => x.Vin == inventory.Vin).ToList();
                        if (searchResult.Count > 0)
                        {
                            foreach (var tmp in searchResult.Distinct())
                            {
                                if (!result.Any(i => i.TrimServiceId == tmp.TrimServiceId))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {

                                        LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
                                        AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
                                        HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
                                        Year = inventory.Year.Equals(0) ? tmp.Year.GetValueOrDefault() : inventory.Year,
                                        MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                                        ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                                        TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                                        TrimName = tmp.Trim
                                    };
                                    result.Add(subResult);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(inventory.Vin))
                            {
                                ExecuteByVin(userName, password, inventory.Vin.Trim());
                                result = ManheimWholesales;

                                foreach (var tmp in result)
                                {
                                    var manheimRecord = new ManheimValue()
                                    {
                                        AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
                                        AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
                                        AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
                                        Year = inventory.Year.Equals(0) ? tmp.Year : inventory.Year,
                                        MakeServiceId = tmp.MakeServiceId,
                                        ModelServiceId = tmp.ModelServiceId,
                                        TrimServiceId = tmp.TrimServiceId,
                                        Trim = tmp.TrimName,
                                        DateAdded = DateTime.Now,
                                        Expiration = GetNextFriday(),
                                        Vin = inventory.Vin,
                                        LastUpdated = DateTime.Now,
                                        VehicleStatusCodeId = 1
                                    };

                                    context.AddToManheimValues(manheimRecord);
                                }

                                context.SaveChanges();
                            }
                            else
                            {
                                var matchingMake = MatchingMake(inventory.Make);
                                var matchingModel = 0;
                                var matchingModels = MatchingModels(inventory.Model, matchingMake);
                                var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }

                                // don't have trims in database
                                if (matchingTrims.Count == 0)
                                {
                                    GetTrim(inventory.Year.ToString(), matchingMake.ToString(),
                                                           matchingModels, userName, password);
                                    foreach (var model in matchingModels)
                                    {
                                        matchingTrims = MatchingTrimsByModelId(model);
                                        if (matchingTrims.Count > 0)
                                        {
                                            matchingModel = model;
                                            break;
                                        }
                                    }
                                }

                                foreach (var trim in matchingTrims)
                                {
                                    Execute("US", inventory.Year.ToString(), matchingMake.ToString(),
                                                           matchingModel.ToString(),
                                                           trim.ServiceId.ToString(), "NA", userName,
                                                           password);
                                    if (!(ManheimWholesale.LowestPrice.Equals(0) ||
                                          ManheimWholesale.AveragePrice.Equals(0) ||
                                          ManheimWholesale.HighestPrice.Equals(0)))
                                    {
                                        var subResult = new ManheimWholesaleViewModel()
                                        {
                                            LowestPrice = ManheimWholesale.LowestPrice,
                                            AveragePrice = ManheimWholesale.AveragePrice,
                                            HighestPrice = ManheimWholesale.HighestPrice,
                                            Year = inventory.Year,
                                            MakeServiceId = matchingMake,
                                            ModelServiceId = matchingModel,
                                            TrimServiceId = trim.ServiceId,
                                            TrimName = trim.Name
                                        };

                                        result.Add(subResult);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var manheimService = new ManheimService();
                        if (!string.IsNullOrEmpty(inventory.Vin))
                        {
                            manheimService.ExecuteByVin(userName, password, inventory.Vin.Trim());
                            result = manheimService.ManheimWholesales;

                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue()
                                {
                                    AuctionLowestPrice = Convert.ToDecimal((tmp.LowestPrice)),
                                    AuctionAveragePrice = Convert.ToDecimal((tmp.AveragePrice)),
                                    AuctionHighestPrice = Convert.ToDecimal((tmp.HighestPrice)),
                                    Year = inventory.Year.Equals(0) ? tmp.Year : inventory.Year,
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = GetNextFriday(),
                                    Vin = inventory.Vin,
                                    LastUpdated = DateTime.Now,
                                    VehicleStatusCodeId = 1
                                };

                                context.AddToManheimValues(manheimRecord);
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            var matchingMake = manheimService.MatchingMake(inventory.Make);
                            var matchingModel = 0;
                            var matchingModels = manheimService.MatchingModels(inventory.Model, matchingMake);
                            var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }

                            // don't have trims in database
                            if (matchingTrims.Count == 0)
                            {
                                manheimService.GetTrim(inventory.Year.ToString(), matchingMake.ToString(),
                                                       matchingModels, userName, password);
                                foreach (var model in matchingModels)
                                {
                                    matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                    if (matchingTrims.Count > 0)
                                    {
                                        matchingModel = model;
                                        break;
                                    }
                                }
                            }

                            foreach (var trim in matchingTrims)
                            {
                                manheimService.Execute("US", inventory.Year.ToString(), matchingMake.ToString(),
                                                       matchingModel.ToString(), trim.ServiceId.ToString(), "NA",
                                                       userName, password);
                                if (!(manheimService.ManheimWholesale.LowestPrice.Equals(0) ||
                                      manheimService.ManheimWholesale.AveragePrice.Equals(0) ||
                                      manheimService.ManheimWholesale.HighestPrice.Equals(0)))
                                {
                                    var subResult = new ManheimWholesaleViewModel()
                                    {
                                        LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                        AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                        HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                        Year = inventory.Year,
                                        MakeServiceId = matchingMake,
                                        ModelServiceId = matchingModel,
                                        TrimServiceId = trim.ServiceId,
                                        TrimName = trim.Name
                                    };

                                    result.Add(subResult);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return result;
        }

        public List<ManheimWholesaleViewModel> ManheimReportForAppraisal(Appraisal appraisal, string userName, string password)
        {
            var result = new List<ManheimWholesaleViewModel>();
            var context = new VincontrolEntities();
            try
            {

                if (context.ManheimValues.Any(x => x.VehicleId == appraisal.Vehicle.VehicleId))
                {
                    var searchResult = context.ManheimValues.Where(x => x.VehicleId == appraisal.Vehicle.VehicleId).ToList();

                    result.AddRange(searchResult.Select(tmp => new ManheimWholesaleViewModel
                    {
                        LowestPrice = tmp.AuctionLowestPrice.GetValueOrDefault(),
                        AveragePrice = tmp.AuctionAveragePrice.GetValueOrDefault(),
                        HighestPrice = tmp.AuctionHighestPrice.GetValueOrDefault(),
                        Year = tmp.Year.GetValueOrDefault(),
                        MakeServiceId = tmp.MakeServiceId.GetValueOrDefault(),
                        ModelServiceId = tmp.ModelServiceId.GetValueOrDefault(),
                        TrimServiceId = tmp.TrimServiceId.GetValueOrDefault(),
                        TrimName = tmp.Trim
                    }));

                }
                else
                {
                    var manheimService = new ManheimService();

                    if (!string.IsNullOrEmpty(appraisal.Vehicle.Vin.Trim()))
                    {
                        manheimService.ExecuteByVin(userName, password, appraisal.Vehicle.Vin.Trim());
                        result = manheimService.ManheimWholesales;

                        foreach (var tmp in result)
                        {
                            var manheimRecord = new ManheimValue
                            {
                                AuctionLowestPrice = Convert.ToDecimal(tmp.LowestPrice),
                                AuctionAveragePrice = Convert.ToDecimal(tmp.AveragePrice),
                                AuctionHighestPrice = Convert.ToDecimal(tmp.HighestPrice),
                                Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                MakeServiceId = tmp.MakeServiceId,
                                ModelServiceId = tmp.ModelServiceId,
                                TrimServiceId = tmp.TrimServiceId,
                                Trim = tmp.TrimName,
                                DateAdded = DateTime.Now,
                                Expiration = GetNextFriday(),
                                Vin = appraisal.Vehicle.Vin,
                                LastUpdated = DateTime.Now,
                                VehicleStatusCodeId = Constanst.VehicleStatus.Appraisal,
                                VehicleId = appraisal.Vehicle.VehicleId
                            };

                            context.AddToManheimValues(manheimRecord);
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        var matchingMake = manheimService.MatchingMake(appraisal.Vehicle.Make);
                        var matchingModel = 0;
                        var matchingModels = manheimService.MatchingModels(appraisal.Vehicle.Model, matchingMake);
                        var matchingTrims = new List<vincontrol.Data.Model.ManheimTrim>();

                        foreach (var model in matchingModels)
                        {
                            matchingTrims = manheimService.MatchingTrimsByModelId(model);
                            if (matchingTrims.Count > 0)
                            {
                                matchingModel = model;
                                break;
                            }
                        }

                        // don't have trims in database
                        if (matchingTrims.Count == 0)
                        {
                            manheimService.GetTrim(appraisal.Vehicle.Year.GetValueOrDefault().ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModels, userName, password);
                            foreach (var model in matchingModels)
                            {
                                matchingTrims = manheimService.MatchingTrimsByModelId(model);
                                if (matchingTrims.Count > 0)
                                {
                                    matchingModel = model;
                                    break;
                                }
                            }
                        }

                        foreach (var trim in matchingTrims)
                        {
                            manheimService.Execute("US", appraisal.Vehicle.Year.ToString(), matchingMake.ToString(CultureInfo.InvariantCulture), matchingModel.ToString(CultureInfo.InvariantCulture), trim.ServiceId.ToString(CultureInfo.InvariantCulture), "NA", userName, password);
                            if (!(Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) == 0 ||
                               Convert.ToInt32(manheimService.ManheimWholesale.AveragePrice) == 0 ||
                               Convert.ToInt32(manheimService.ManheimWholesale.HighestPrice) == 0) &&
                               Convert.ToInt32(manheimService.ManheimWholesale.LowestPrice) > 0
                                )
                            {
                                var subResult = new ManheimWholesaleViewModel
                                {
                                    LowestPrice = manheimService.ManheimWholesale.LowestPrice,
                                    AveragePrice = manheimService.ManheimWholesale.AveragePrice,
                                    HighestPrice = manheimService.ManheimWholesale.HighestPrice,
                                    Year = appraisal.Vehicle.Year.GetValueOrDefault(),
                                    MakeServiceId = matchingMake,
                                    ModelServiceId = matchingModel,
                                    TrimServiceId = trim.ServiceId,
                                    TrimName = trim.Name
                                };
                                result.Add(subResult);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }
        #endregion

        #region Private Methods

        private void WebRequestGet()
        {
            Request = (HttpWebRequest)WebRequest.Create(LogInUrl);
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
            ManheimWholesale.HighestPrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true));
            ManheimWholesale.AveragePrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true));
            ManheimWholesale.LowestPrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true));
        }

        private void WebRequestGetByVinWithMultipleTrims()
        {
            //clear wholesale values before getting new values
            ManheimWholesales.Clear();

            Request = (HttpWebRequest)WebRequest.Create(ReportParameters);
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
                    wholesale.HighestPrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true));
                    wholesale.AveragePrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true));
                    wholesale.LowestPrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true));

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
                wholesale.HighestPrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true));
                wholesale.AveragePrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true));
                wholesale.LowestPrice = RemoveSpecialCharactersAndReturnNumber(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true));

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
        }

        private void BuildReportParameters(string trim, string region, string url)
        {
            ReportParameters = url.Replace("[trim]", trim).Replace("[region]", region);
        }

        private string BuildReportParameters(string year, string make, string model, string trim)
        {
            ReportParameters = String.Format("https://www.manheim.com/members/internetmmr/pricesPageData.mmr?action=MileageSet&tab=pricesPage&sid={4}&country=US&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}&collectionSelectBody={3}&beanCollectionSelectRegion=NA&beanCollectionSelectSeasonalAdj=off&mileage=&searchMethod=&Reset=&vehicleMileageProperty=", year, make, model, trim, Sid);
            var url = ReportParameters;
            return url;
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

        private void GetMake()
        {
            for (int maheimYear = 1981; maheimYear <= (DateTime.Now.Year + 1); maheimYear++)
            {
                PostData = "action=year&collectionSelectYear=" + maheimYear;
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
                        using (var context = new VincontrolEntities())
                        {
                            if (GetMakeByServiceId(makeId) == null)
                            {
                                var makeToInsert = new ManheimMake()
                                {
                                    ServiceId = makeId,
                                    Name = makeName.ToLower()
                                };
                                context.AddToManheimMakes(makeToInsert);
                                context.SaveChanges();

                                var yearMakeToInsert = new ManheimYearMake()
                                {
                                    Year = maheimYear,
                                    MakeId = makeToInsert.ManheimMakeId
                                };

                                context.AddToManheimYearMakes(yearMakeToInsert);

                                context.SaveChanges();
                                System.Console.WriteLine(String.Format("Make: {0}", makeName));
                            }
                        }

                    }

                }
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

        private void GetModel()
        {
            using (var context = new VincontrolEntities())
            {
                var yearMakes = context.ManheimYearMakes.ToList();

                foreach (var yearMake in yearMakes)
                {
                    PostData = "action=make&collectionSelectYear=" + yearMake.Year + "&collectionSelectMake=" + yearMake.ManheimMake.ServiceId;
                    // for testing: "action=make&collectionSelectYear=2013&collectionSelectMake=69"                
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
                    }

                    var xmlDocument = DownloadDocument(Result);
                    var options = xmlDocument.SelectNodes("//select[@name='collectionSelectModel']/option[position() > 1]");

                    if (options == null || options.Count <= 0) continue;
                    foreach (XmlNode option in options)
                    {
                        var modelName = GetString(option, "text()", null, null, true);
                        var modelValue = GetString(option, "@value", null, null, true);
                        var modelId = Convert.ToInt32(modelValue);
                        using (var context1 = new VincontrolEntities())
                        {
                            var manheimModel = GetModelByServiceId(modelId);
                            int manheimModelId;
                            if (manheimModel == null)
                            {
                                var modelToInsert = new ManheimModel()
                                {
                                    ServiceId = modelId,
                                    Name = modelName.ToLower()
                                };
                                context1.AddToManheimModels(modelToInsert);
                                context1.SaveChanges();
                                Console.WriteLine(String.Format("Model: {0}", modelName));
                                manheimModelId = modelToInsert.ManheimModelId;
                            }
                            else
                            {
                                manheimModelId = manheimModel.ManheimModelId;
                            }

                            if (GetMakeModelByModelIdAndYearMakeId(manheimModelId, yearMake.ManheimYearMakeId) != null) continue;

                            var makemodelToInsert = new ManheimMakeModel()
                            {
                                YearMakeId = yearMake.ManheimYearMakeId,
                                ModelId = manheimModelId
                            };
                            context1.AddToManheimMakeModels(makemodelToInsert);
                            context1.SaveChanges();
                            Console.WriteLine(String.Format("MakeModel: {0}+{1}", modelName, yearMake.Year));
                        }
                    }
                }
            }
        }

        private void GetTrim()
        {
            var makeModels = new List<ManheimMakeModel>();
            using (var context = new VincontrolEntities())
            {
                makeModels = context.ManheimMakeModels.Include("ManheimYearMake").Include("ManheimYearMake.ManheimMake").Include("ManheimModel").ToList();
            }

            if (!makeModels.Any()) return;

            var tmpIndex = 0;
            int numCores = System.Environment.ProcessorCount;

            var tasks = new List<Task>();
            for (int ii = 0; ii < numCores / 2; ii++)
            {
                var tmpInventory = makeModels[tmpIndex];
                var t = Task.Factory.StartNew(() =>
                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    RunningInventory(tmpInventory);
                },
                TaskCreationOptions.None
                );
                tasks.Add(t);
                tmpIndex++;
            }

            while (tmpIndex < makeModels.Count)
            {
                int index = Task.WaitAny(tasks.ToArray());

                tasks.RemoveAt(index);
                Thread.Sleep(1000);

                {
                    var tmpInventory = makeModels[tmpIndex];
                    var t = Task.Factory.StartNew(() =>
                    {
                        var sw = System.Diagnostics.Stopwatch.StartNew();

                        RunningInventory(tmpInventory);
                    },
                    TaskCreationOptions.None
                    );
                    tasks.Add(t);
                    tmpIndex++;
                }
            }
        }

        private void RunningInventory(ManheimMakeModel makeModel)
        {
            using (var context = new VincontrolEntities())
            {
                var postData = string.Format("action=model&collectionSelectYear={0}&collectionSelectMake={1}&collectionSelectModel={2}", makeModel.ManheimYearMake.Year, makeModel.ManheimYearMake.ManheimMake.ServiceId, makeModel.ManheimModel.ServiceId);

                var request = (HttpWebRequest)WebRequest.Create(CrawlDataUrl + "?sid=" + Sid);
                request.Method = "POST";
                request.UserAgent = UserAgent;
                request.ContentLength = postData.Length;
                request.ContentType = ContentType;
                request.Referer = RefererUrl;
                request.CookieContainer = CookieContainer;
                request.CookieContainer.Add(CookieCollection);
                string result = null;
                try
                {
                    var streamWriter = new StreamWriter(request.GetRequestStream());
                    streamWriter.Write(postData);
                    streamWriter.Close();
                    var response = (HttpWebResponse)request.GetResponse();
                    var streamReader = new StreamReader(response.GetResponseStream());
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError) { }
                    else { }
                }

                var xmlDocument = DownloadDocument(result);
                var options = xmlDocument.SelectNodes("//select[@name='collectionSelectBody']/option[position() > 1]");

                if (options != null && options.Count > 0)
                {
                    foreach (XmlNode option in options)
                    {
                        var bodyName = GetString(option, "text()", null, null, true);
                        var bodyValue = GetString(option, "@value", null, null, true);
                        var bodyId = Convert.ToInt32(bodyValue);

                        {
                            var existingTrim = GetTrimByServiceId(bodyId);

                            if (existingTrim == null || existingTrim.ExpirationDate <= DateTime.Now)
                            {
                                if (existingTrim == null)
                                {
                                    var trimToInsert = new vincontrol.Data.Model.ManheimTrim()
                                    {
                                        ServiceId = bodyId,
                                        Name = bodyName.ToLower(),
                                        //modelId = makeModel.id
                                    };
                                    context.AddToManheimTrims(trimToInsert);
                                    context.SaveChanges();
                                    existingTrim = trimToInsert;
                                    Console.WriteLine(String.Format("Trim: {0}", bodyName));
                                }

                                var ReportParameters = BuildReportParameters(makeModel.ManheimYearMake.Year.ToString(), makeModel.ManheimYearMake.ManheimMake.ServiceId.ToString(), makeModel.ManheimModel.ServiceId.ToString(), bodyId.ToString(CultureInfo.InvariantCulture));
                                request = (HttpWebRequest)WebRequest.Create(ReportParameters);
                                request.UserAgent = UserAgent;
                                request.ContentType = "text/html;charset=ISO-8859-1";
                                request.Referer = RefererUrl;
                                request.CookieContainer = CookieContainer;
                                request.CookieContainer.Add(CookieCollection);
                                //Get the response from the server and save the cookies from the first request..
                                var response = (HttpWebResponse)request.GetResponse();
                                CookieCollection = response.Cookies;

                                var streamReader = new StreamReader(response.GetResponseStream());
                                result = streamReader.ReadToEnd();
                                streamReader.Close();

                                xmlDocument = DownloadDocument(result);
                                existingTrim.HighestPrice = Convert.ToDecimal(RemoveSpecialCharactersForPrice(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[2]", null, null, true)));
                                existingTrim.AveragePrice = Convert.ToDecimal(RemoveSpecialCharactersForPrice(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[3]", null, null, true)));
                                existingTrim.LowestPrice = Convert.ToDecimal(RemoveSpecialCharactersForPrice(GetString(xmlDocument, "//table/tr/td/table[3]/tr/td/table/tr/td/table[2]/tr[3]/td[4]", null, null, true)));
                                existingTrim.ExpirationDate = DateTime.Now.AddDays(7);
                                context.SaveChanges();
                                System.Console.WriteLine(String.Format("Trim: {0} - {1}|{2}|{3} ({4})", bodyName, existingTrim.HighestPrice, existingTrim.AveragePrice, existingTrim.LowestPrice, makeModel.ManheimYearMake.Year + " " + makeModel.ManheimYearMake.ManheimMake.Name + " " + makeModel.ManheimModel.Name));

                                if (GetModelTrimByMakeModelIdAndTrimId(makeModel.ManheimMakeModelId, existingTrim.ManheimTrimId) == null)
                                {
                                    var modelTrimToInsert = new ManheimModelTrim()
                                    {
                                        MakeModelId = makeModel.ManheimMakeModelId,
                                        TrimId = existingTrim.ManheimTrimId
                                    };
                                    context.AddToManheimModelTrims(modelTrimToInsert);
                                    context.SaveChanges();
                                    Console.WriteLine(String.Format("MakeModelANdTrim: {0}+{1}", makeModel.ManheimMakeModelId, bodyName));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetTrimByVin()
        {
            try
            {
                var manheimService = new ManheimService();
                List<Inventory> inventories;
                using (var context = new VincontrolEntities())
                {
                    inventories = context.Inventories.Include("Vehicle").Where(x => x.Condition == Constanst.ConditionStatus.Used).ToList();
                }
                if (!inventories.Any()) return;

                //log.ErrorLog(String.Format("Number of INVENTORY cars {0}", inventories.Count));

                var tmpIndex = 0;
                int numCores = System.Environment.ProcessorCount;

                var tasks = new List<Task>();
                for (int ii = 0; ii < numCores / 2; ii++)
                {
                    var tmpInventory = inventories[tmpIndex];
                    var t = Task.Factory.StartNew(() =>
                    {
                        var sw = System.Diagnostics.Stopwatch.StartNew();

                        RunningInventoryWithVin(manheimService, tmpInventory);
                    },
                    TaskCreationOptions.None
                    );
                    tasks.Add(t);
                    tmpIndex++;
                }

                while (tmpIndex < inventories.Count)
                {
                    int index = Task.WaitAny(tasks.ToArray());

                    tasks.RemoveAt(index);
                    Thread.Sleep(1000);

                    {
                        var tmpInventory = inventories[tmpIndex];
                        var t = Task.Factory.StartNew(() =>
                        {
                            var sw = System.Diagnostics.Stopwatch.StartNew();

                            RunningInventoryWithVin(manheimService, tmpInventory);
                        },
                        TaskCreationOptions.None
                        );
                        tasks.Add(t);
                        tmpIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private vincontrol.Data.Model.ManheimTrim GetTrimByServiceId(int id)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimTrims.FirstOrDefault(m => m.ServiceId == id);
            }
        }

        private void GetTrimByVinForAppraisal()
        {
            try
            {
                var manheimService = new ManheimService();
                var appraisals = new List<Appraisal>();
                using (var context = new VincontrolEntities())
                {
                    appraisals = context.Appraisals.Include("Vehicle").ToList();
                }
                if (!appraisals.Any()) return;

                //log.ErrorLog(String.Format("Number of APPRAISAL cars {0}", appraisals.Count));

                var tmpIndex = 0;
                int numCores = System.Environment.ProcessorCount;

                var tasks = new List<Task>();
                for (int ii = 0; ii < numCores / 2; ii++)
                {
                    var tmpAppraisal = appraisals[tmpIndex];
                    var t = Task.Factory.StartNew(() =>
                    {
                        var sw = System.Diagnostics.Stopwatch.StartNew();

                        RunningAppraisalWithVin(manheimService, tmpAppraisal);
                    },
                    TaskCreationOptions.None
                    );
                    tasks.Add(t);
                    tmpIndex++;
                }

                while (tmpIndex < appraisals.Count)
                {
                    int index = Task.WaitAny(tasks.ToArray());

                    tasks.RemoveAt(index);
                    Thread.Sleep(1000);

                    {
                        var tmpAppraisals = appraisals[tmpIndex];
                        var t = Task.Factory.StartNew(() =>
                        {
                            var sw = System.Diagnostics.Stopwatch.StartNew();

                            RunningAppraisalWithVin(manheimService, tmpAppraisals);
                        },
                        TaskCreationOptions.None
                        );
                        tasks.Add(t);
                        tmpIndex++;
                    }
                }
            }
            catch (Exception ex)
            {                

            }            
        }

        private void RunningInventoryWithVin(ManheimService manheimService, Inventory inventory)
        {
            using (var context = new VincontrolEntities())
            {
                if (String.IsNullOrEmpty(inventory.Vehicle.Vin)) return;

                if (context.ManheimValues.Any(x => x.Vin == inventory.Vehicle.Vin && x.Expiration > DateTime.Now))
                    return;

                if (!string.IsNullOrEmpty(inventory.Vehicle.Vin))
                {
                    try
                    {
                        manheimService.ExecuteByVin(UserName, Password, inventory.Vehicle.Vin.Trim());
                        var result = manheimService.ManheimWholesales;
                        if (result.Count > 0)
                        {
                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue()
                                {
                                    AuctionLowestPrice = (tmp.LowestPrice),
                                    AuctionAveragePrice = (tmp.AveragePrice),
                                    AuctionHighestPrice = (tmp.HighestPrice),
                                    Year = inventory.Vehicle.Year,
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = DateTime.Now.AddDays(7),
                                    Vin = inventory.Vehicle.Vin,
                                    LastUpdated = DateTime.Now
                                };

                                context.AddToManheimValues(manheimRecord);
                                Console.WriteLine(String.Format("{0} -- {1}|{2}|{3}", manheimRecord.Trim, manheimRecord.AuctionLowestPrice, manheimRecord.AuctionAveragePrice, manheimRecord.AuctionHighestPrice));
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine(String.Format("No matching trims -- {0}|{1}|{2}|{3}", inventory.Vehicle.Vin, inventory.Vehicle.Year, inventory.Vehicle.Make, inventory.Vehicle.Model));
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }

        private ManheimMakeModel GetMakeModelByModelIdAndYearMakeId(int modelId, int yearMakeId)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimMakeModels.FirstOrDefault(m => m.ModelId == modelId && m.YearMakeId == yearMakeId);
            }
        }

        private ManheimModelTrim GetModelTrimByMakeModelIdAndTrimId(int makemodelId, int trimId)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimModelTrims.FirstOrDefault(m => m.MakeModelId == makemodelId && m.TrimId == trimId);
            }
        }

        private void RunningAppraisalWithVin(ManheimService manheimService, Appraisal appraisal)
        {
            using (var context = new VincontrolEntities())
            {
                if (String.IsNullOrEmpty(appraisal.Vehicle.Vin)) return;

                if (context.ManheimValues.Any(x => x.Vin == appraisal.Vehicle.Vin && x.Expiration > DateTime.Now))
                    return;

                if (!string.IsNullOrEmpty(appraisal.Vehicle.Vin))
                {
                    try
                    {
                        manheimService.ExecuteByVin(UserName, Password, appraisal.Vehicle.Vin.Trim());
                        var result = manheimService.ManheimWholesales;
                        if (result.Count > 0)
                        {
                            foreach (var tmp in result)
                            {
                                var manheimRecord = new ManheimValue()
                                {
                                    AuctionLowestPrice = (tmp.LowestPrice),
                                    AuctionAveragePrice = (tmp.AveragePrice),
                                    AuctionHighestPrice = (tmp.HighestPrice),
                                    Year = appraisal.Vehicle.Year,
                                    MakeServiceId = tmp.MakeServiceId,
                                    ModelServiceId = tmp.ModelServiceId,
                                    TrimServiceId = tmp.TrimServiceId,
                                    Trim = tmp.TrimName,
                                    DateAdded = DateTime.Now,
                                    Expiration = DateTime.Now.AddDays(7),
                                    Vin = appraisal.Vehicle.Vin,
                                    LastUpdated = DateTime.Now
                                };

                                context.AddToManheimValues(manheimRecord);
                                Console.WriteLine(String.Format("{0} -- {1}|{2}|{3}", manheimRecord.Trim, manheimRecord.AuctionLowestPrice, manheimRecord.AuctionAveragePrice, manheimRecord.AuctionHighestPrice));
                            }

                            context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine(String.Format("No matching trims -- {0}|{1}|{2}|{3}", appraisal.Vehicle.Vin, appraisal.Vehicle.Year, appraisal.Vehicle.Make, appraisal.Vehicle.Model));
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }

        private void GetReportContent(int pageIndex = 1, int pageSize = 10)
        {
            var postData = "reportsProperty=valuationExact&action=PrintReports";
            var request = (HttpWebRequest)WebRequest.Create(ReportParameters);
            request.Method = "POST";
            request.UserAgent = UserAgent;
            request.ContentLength = postData.Length;
            request.ContentType = ContentType;
            request.Referer = RefererUrl;
            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            var result = string.Empty;
            try
            {
                var streamWriter = new StreamWriter(request.GetRequestStream());
                streamWriter.Write(postData);
                streamWriter.Close();
                var response = (HttpWebResponse)request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());
                result = streamReader.ReadToEnd();
                streamReader.Close();
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

            var xmlDocument = DownloadDocument(result);

            var summaryNode = xmlDocument.SelectNodes("//table[@class='reportText']/tr");
            if (summaryNode != null && summaryNode.Count > 0)
            {
                foreach (XmlNode node in summaryNode)
                {
                    var nodeName = node.FirstChild.InnerText;
                    var nodeValue = node.LastChild.InnerText;
                    switch (nodeName)
                    {
                        case "Prices": HighPrice = nodeValue; break;
                        case "Low:": LowPrice = nodeValue; break;
                        case "Average:": AveragePrice = nodeValue; break;
                        case "Average Odometer:": AverageOdometer = nodeValue; break;
                        case "# of Vehicles:": break;
                        default: break;
                    }
                }
            }

            var transactionNodes = xmlDocument.SelectNodes("//table[2]/tr[position() >= 2]");
            if (transactionNodes != null && transactionNodes.Count > 0)
            {
                NumberOfManheimTransactions = transactionNodes.Count;

                if (pageIndex.Equals(0))
                
                    foreach (XmlNode node in transactionNodes)
                    {
                        if (node == null) break;

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
                
                else
                    for (int i = (pageIndex - 1) * pageSize; i < pageIndex * pageSize; i++)
                    {
                        var node = (XmlNode)transactionNodes[i];
                        if (node == null) break;

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

        private List<string> GetVehicleInLaneLinks(XmlDocument xmlDocument, string auctionSaleLink)
        {
            // Get List of Vehicles In Lane link
            var listOfVehicleInLaneLink = new List<string>();
            var Result = WebRequestGet(auctionSaleLink);

            xmlDocument = DownloadDocument(Result);
            var enhancedSaleNodes = xmlDocument.SelectNodes("//*[@id='mainColumn']/*[@class='sale']/*[@class='enhancedSales']");
            foreach (XmlNode node in enhancedSaleNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (!(subNode.Name.Equals("a"))) continue;
                    listOfVehicleInLaneLink.Add("https://www.manheim.com/members/presale/control/" + subNode.Attributes["href"].Value);
                }
            }

            return listOfVehicleInLaneLink;
        }

        private List<ManheimCategory> GetCategoryInLaneLinks(XmlDocument xmlDocument, string auctionSaleLink)
        {
            // Get List of Vehicles In Lane link
            xmlDocument.PreserveWhitespace = false;
            var listOfVehicleInLaneLink = new List<ManheimCategory>();
            var Result = WebRequestGet(auctionSaleLink);

            Regex regex = new Regex(@">\s*<");
            Result = regex.Replace(Result, "><");
            xmlDocument = DownloadDocument(Result);
            var laneNodes = xmlDocument.SelectNodes("//*[@class='lane']");
            foreach (XmlNode lane in laneNodes)
            {
                var description = lane.FirstChild.InnerText;
                description = description.Substring(description.IndexOf("-") + 2);
                foreach (XmlNode subNode in lane.LastChild)
                {
                    try
                    {
                        var category = subNode.FirstChild.InnerText;

                        var url = ("https://www.manheim.com/members/presale/control/" + subNode.LastChild.Attributes["href"].Value);
                        listOfVehicleInLaneLink.Add(new ManheimCategory() { Category = category, Url = url, LaneDescription = description });
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return listOfVehicleInLaneLink;
        }

        private List<string> GetVehicleInLaneLinks(XmlDocument xmlDocument, List<string> listOfAuctionSaleLink)
        {
            // Get List of Vehicles In Lane link
            var listOfVehicleInLaneLink = new List<string>();
            foreach (var auctionSaleLink in listOfAuctionSaleLink)
            {

                var Result = WebRequestGet(auctionSaleLink);

                xmlDocument = DownloadDocument(Result);
                var enhancedSaleNodes = xmlDocument.SelectNodes("//*[@id='mainColumn']/*[@class='sale']/*[@class='enhancedSales']");
                foreach (XmlNode node in enhancedSaleNodes)
                {
                    foreach (XmlNode subNode in node.ChildNodes)
                    {
                        if (!(subNode.Name.Equals("a"))) continue;
                        listOfVehicleInLaneLink.Add("https://www.manheim.com/members/presale/control/" + subNode.Attributes["href"].Value);

                    }
                }
            }

            return listOfVehicleInLaneLink;
        }

        private List<string> GetPowerSearchLinks(XmlDocument xmlDocument, List<string> listOfVehicleInLaneLink)
        {
            // Get List of Power Search link
            var listOfPowerSearchLink = new List<string>();
            using (var context = new VinMarketEntities())
            {
                foreach (var vehicleInLaneLink in listOfVehicleInLaneLink)
                {
                    var Result = WebRequestGet(vehicleInLaneLink);

                    xmlDocument = DownloadDocument(Result);
                    var mainNode = xmlDocument.SelectSingleNode("//*[@id='mainColumn']");
                    foreach (XmlNode node in mainNode.ChildNodes)
                    {
                        if ((node.Name.Equals("pre")))
                        {
                            foreach (XmlNode subNode in node.ChildNodes)
                            {
                                if (!(subNode.Name.Equals("a")) || listOfPowerSearchLink.Contains("https://www.manheim.com/" + subNode.Attributes["href"].Value)) continue;

                                listOfPowerSearchLink.Add("https://www.manheim.com/" + subNode.Attributes["href"].Value);

                            }
                            break;
                        }
                    }
                }
            }

            return listOfPowerSearchLink;
        }

        private void GetDefaultManheimLogin()
        {
            using (var context = new VincontrolEntities())
            {
                var setting = context.Settings.FirstOrDefault(m => m.Manheim != "vincontrol" && m.DealerId.Equals(37695));
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

        private ManheimModel GetModelByServiceId(int id)
        {
            using (var context = new VincontrolEntities())
            {
                return context.ManheimModels.FirstOrDefault(m => m.ServiceId == id);
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

        public XmlDocument DownloadDocument(string content)
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

        public static string ConvertToString(JValue obj)
        {
            return obj != null ? Convert.ToString(obj.Value) : string.Empty;
        }

        private int RemoveSpecialCharactersAndReturnNumber(string input)
        {
            int number = 0;

            if (!String.IsNullOrEmpty(input))
            {
                string tmp = Regex.Replace(input, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

                Int32.TryParse(tmp, out number);
            }
            return number;
        }

        private string GetMMRPrices(string mid, string vin, string odometer, string region, string locale)
        {
            // Setup the http request.
            var url = String.Format("https://www.manheim.com/mmrPrices?mid={0}&vin={1}&odometer={2}&region={3}&locale={4}", mid, vin, odometer, region, locale);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";
            //PostData = CreateDataToMMRPrices(new MMRPriceContract()
            //{
            //    mid = mid,
            //    vin = vin,
            //    odometer = odometer,
            //    region = region,
            //    locale = locale
            //});

            //request.ContentLength = url.Length;
            request.ContentType = "application/json";
            request.Referer = "https://www.manheim.com//members/powersearch/redirect.do?redirectPage=VDP&vin=JN1AZ4EH9BM550652&auction=PXAA&fromPresales=true&format=standard&saleNumber=24&saleDate=20130611&locale=en_US&country=USA&WT.svl=pre_inv_ps-vdp";

            request.CookieContainer = CookieContainer;
            request.CookieContainer.Add(CookieCollection);

            // Get the response.
            var response = (HttpWebResponse)request.GetResponse();

            // Read the response
            var streamReader = new StreamReader(response.GetResponseStream());
            var Result = streamReader.ReadToEnd();
            streamReader.Close();

            return Result;
        }

        private int ConvertStringToInterger(string s)
        {
            int oNumber = 0;

            if (!String.IsNullOrEmpty(s))
            {
                bool flag = Int32.TryParse(s, out oNumber);
            }

            return oNumber;
        }

        private string UpperFirstLetterOfEachWord(string value)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        private string RemoveSpecialCharactersForMSRP(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                input = input.Replace("$", "").Replace(",", "");
                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+,$.", "", RegexOptions.Compiled);
            }
            return "0";
        }

        private string RemoveSpecialCharactersForPrice(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                input = input.Replace("$", "").Replace(",", "");
                if (input.Contains("."))
                    input = input.Substring(0, input.IndexOf("."));

                return Regex.Replace(input, "[^a-zA-Z0-9]+,$.", "", RegexOptions.Compiled);
            }

            return "0";
        }

        public DateTime GetNextFriday()
        {
            var dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            for (int i = 1; i < 8; i++)
            {
                DateTime dt = dtNow.AddDays(i);
                if (dt.DayOfWeek.Equals(DayOfWeek.Friday))
                    return dt;
            }
            return dtNow;

        }
        #endregion
    }

    public class ManheimAuction
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Mileage { get; set; }
        public string FuelType { get; set; }
        public string Engine { get; set; }
        public string Litters { get; set; }
        public int Doors { get; set; }
        public string BodyStyle { get; set; }
        public string VehicleType { get; set; }
        public string DriveTrain { get; set; }
        public string Transmission { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public bool AsIs { get; set; }
        public string Cr { get; set; }
        public string Mmr { get; set; }
        public string MmrAbove { get; set; }
        public string MmrBelow { get; set; }
        public int Lane { get; set; }
        public int Run { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; }
        public string Aucton { get; set; }
        public string Url { get; set; }
        public DateTime DateStamp { get; set; }
        public string Images { get; set; }
        public string Seller { get; set; }
        public string LaneDescription { get; set; }
        public string Category { get; set; }
        public string CrUrl { get; set; }
    }

    public class ManheimConstants
    {
        public const string Year = "Year:";
        public const string Make = "Make:";
        public const string Model = "Model:";
        public const string Trim = "Trim Level:";
        public const string Mileage = "Odometer:";
        public const string FuelType = "Fuel Type:";
        public const string Engine = "Engine:";
        public const string Litters = "Displacement:";
        public const string Transmission = "Transmission:";
        public const string ExteriorColor = "Exterior Color:";
        public const string InteriorColor = "Interior Color:";
        public const string Vin = "VIN:";
        public const string BodyStyle = "Body Style:";
        public const string Doors = "Doors:";
        public const string VehicleType = "Vehicle Type:";
        public const string AsIs = "As Is:";
        public const string DriveTrain = "Drive Train:";
        public const string Stereo = "Stereo:";
        public const string Airbags = "Airbags:";
        public const string InteriorType = "Interior Type:";
        public const string NotSpecified = "Not Specified";
        public const string NotAvailable = "Not Available";

    }

    public class MMRPriceContract
    {
        public string mid { get; set; }
        public string vin { get; set; }
        public string odometer { get; set; }
        public string region { get; set; }
        public string locale { get; set; }
    }

    public class ManehimRegion
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
    }

    public class ManheimCategory
    {
        public string Lane { get; set; }
        public string LaneDescription { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
    }
}
