using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Application.VinMarket.Forms.CommonManagement;
using vincontrol.Application.VinMarket.ViewModels.CommonManagement;
using vincontrol.Data.Model;
using vincontrol.Helper;
using VINControl.Logging;
using VINControl.WebHelper;
using vincontrol.Data;
using vincontrol.Data.Query;
using System.Configuration;

namespace VINControl.CarMax
{
    public class CarMaxService
    {
        #region Const
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:26.0) Gecko/20100101 Firefox/26.0; " +
                                         "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                         ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                         "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private const string AcceptLanguage = "Accept-Language: en-US,en;q=0.5";
        private const string AcceptEncoding = "Accept-Encoding: gzip, deflate";
        private const string ContentType = "application/x-www-form-urlencoded";        
        #endregion
        private ICommonManagementForm _commonManagementForm;
        private ILoggingService _logging;

        #region Constructor
        public CarMaxService()
        {
            _commonManagementForm = new CommonManagementForm();
            _logging = new LoggingService();
        }
        #endregion

        #region Public Methods
        public void ExecuteByApi()
        {
            LoadCars(ConfigurationManager.AppSettings["carmax:ApiKey"]);
        }

        public void ExecuteByStore()
        {
            _logging.Info("*** START to scrap Car Max Vehicles ***");
            var stores = _commonManagementForm.GetCarMaxStores();
            foreach (var store in stores)
            {
                LoadCars(store);
            }
            _logging.Info("*** START to scrap Car Max Vehicles ***");
        }
        
        public void ExecuteByMake()
        {
            _logging.Info("*** START to scrap Car Max Vehicles ***");
            var makes = LoadMakes();
            foreach (var make in makes)
            {
                foreach (var model in make.Models)
                {
                    LoadCars(make.Value, make.Name, model.Value, model.Name, 90, 92627);
                }
            }
            _logging.Info("*** START to scrap Car Max Vehicles ***");
        }

        public void ExecuteByMake(string makeName, long makeValue)
        {
            _logging.Info("*** START to scrap Car Max Vehicles by " + makeName + " ***");
            var models = LoadModels(makeValue);
            foreach (var model in models)
            {
                LoadCars(makeValue, makeName, model.Value, model.Name, 90, 92627);
            }

            //RunCarMaxVehiclesMissingStoreId(makeName);

            _logging.Info("*** END to scrap Car Max Vehicles by " + makeName + " ***");
        }

        public void MarkSoldCarMaxVehicles()
        {
            //_logging.Info(String.Format("START to mark sold Car Max Vehicles"));
            Console.WriteLine(String.Format("START to mark sold Car Max Vehicles"));
            //_commonManagementForm.MarkSoldCarMaxVehicles();
            (new VINMarketSqlHelper()).QueryText(CarMaxQuery.MarkSoldQuery());
            //_logging.Info(String.Format("END to mark sold Car Max Vehicles"));
            Console.WriteLine(String.Format("END to mark sold Car Max Vehicles"));
        }

        public void RunCarMaxVehiclesMissingStoreId(string make = "")
        {
            _logging.Info(String.Format("START to update store id for Car Max Vehicles"));
            //var list = String.IsNullOrEmpty(make) ? _commonManagementForm.GetCarMaxVehiclesMissingStoreId() : _commonManagementForm.GetCarMaxVehiclesMissingStoreId(make);
            var list = (new VINMarketSqlHelper()).QueryText<CarMaxVehicle>("SELECT CarMaxVehicleId, Year, Make, Model, Trim, Url FROM CarMaxVehicle WHERE StoreId is null " + (!string.IsNullOrEmpty(make) ? "AND Make = '" + make + "'" : string.Empty));
            foreach (var item in list)
            {
                AddNewCarMaxVehicle(item.Year, item.Make, item.Model, item.Trim, item.CarMaxVehicleId, item.Url);
            }
            _logging.Info(String.Format("END to update store id for Car Max Vehicles"));
        }

        public void InsertStoresIntoDatabase()
        {
            //_logging.Info("*** START to scrap Car Max Stores ***");
            Console.WriteLine("*** START to scrap Car Max Stores ***");
            var states = LoadStates();
            foreach (var state in states)
            {
                var stateUrl = String.Format("https://www.carmax.com/stores/states/{0}", state.Value);
                var content = WebHandler.DownloadContent(stateUrl);
                var xmlDocument = WebHandler.DownloadDocument(content);
                var storeNodes = xmlDocument.SelectNodes("//div[@class='store-locator--result']");
                if (storeNodes != null)
                {
                    foreach (XmlNode node in storeNodes)
                    {
                        try
                        {
                            var newStore = GetStoreDetail(node);
                            _commonManagementForm.AddNewCarMaxStore(newStore);
                            Console.WriteLine("{0}", newStore.Name);
                        }
                        catch (Exception ex) 
                        {
                            //_logging.Error(String.Format("Failed to insert store at {0}", stateUrl), ex);
                            Console.WriteLine(String.Format("Failed to insert store at {0}", stateUrl), ex);
                        }                        
                    }
                }
            }

            //_logging.Info("*** END to scrap Car Max Stores ***");
            Console.WriteLine("*** END to scrap Car Max Stores ***");
        }
        #endregion

        #region Private Methods
        private CarMaxStoreViewModel GetStoreDetail(XmlNode node)
        {
            try
            {
                var fullNameNode = node.SelectSingleNode(".//h4[@class='store-locator--result--name']/a");
                var fullName = fullNameNode.InnerText.Trim();
                var url = "https://www.carmax.com" + fullNameNode.Attributes["href"].Value;

                var name = node.SelectSingleNode("./div[@class='store-locator--result--header']") != null ? node.SelectSingleNode("./div[@class='store-locator--result--header']").InnerText.TrimEnd() : fullName;

                var address = node.SelectSingleNode(".//*[@data-react='GetDirectionsButton']/*[@data-scope='store']/*[@data-prop='Street']").InnerText;
                var city = node.SelectSingleNode(".//*[@data-react='GetDirectionsButton']/*[@data-scope='store']/*[@data-prop='City']").InnerText;
                var stateStr = node.SelectSingleNode(".//*[@data-react='GetDirectionsButton']/*[@data-scope='store']/*[@data-prop='State']").InnerText;
                var zip = Convert.ToInt32(node.SelectSingleNode(".//*[@data-react='GetDirectionsButton']/*[@data-scope='store']/*[@data-prop='ZipCode']").InnerText);
                var phone = node.SelectSingleNode(".//span[@class='store-locator--result--phone unclickable']").InnerText.Trim();
                var id = Convert.ToInt64(Path.GetFileName(url));

                var newStore = new CarMaxStoreViewModel()
                {
                    CarMaxStoreId = id,
                    Name = name,
                    FullName = fullName,
                    Url = url,
                    Address = address,
                    City = city,
                    State = stateStr,
                    ZipCode = zip,
                    Phone = phone,
                    CreatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now),
                    UpdatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now)
                };

                return newStore;
            }
            catch (Exception ex)
            {
                throw;
            }            
            
        }

        private List<CarMaxState> LoadStates()
        {
            var list = new List<CarMaxState>();
            var content = WebHandler.DownloadContent("https://www.carmax.com/stores/states");
            var xmlDocument = WebHandler.DownloadDocument(content);
            var stateNodes = xmlDocument.SelectNodes("//*[@class='stores-states--list']/*[.]");
            if (stateNodes != null)
            {
                foreach (XmlNode node in stateNodes)
                {
                    var stateName = node.FirstChild.InnerText.Trim();
                    var state = new CarMaxState();
                    state.Value = (Path.GetFileName(node.FirstChild.Attributes["href"].Value));
                    state.Name = stateName;
                    
                    list.Add(state);
                }
            }

            return list;
        }

        private List<CarMaxMake> LoadMakes()
        {
            var list = new List<CarMaxMake>();
            var content = WebHandler.DownloadContent("http://www.carmax.com/enus/car-search/used-cars.html");
            var xmlDocument = WebHandler.DownloadDocument(content);
            //var nsManager = WebHandler.GetXmlNamespaceManager(xmlDocument);
            var makeNodes = xmlDocument.SelectNodes("//*[@id='make']/*[.]");
            if (makeNodes != null)
            {
                foreach (XmlNode node in makeNodes)
                {
                    var makeName = node.InnerText.Trim();
                    if (new string[] { "All Makes", "All Domestic", "All Imports" }.Contains(makeName)) continue;
                    var make = new CarMaxMake();
                    make.Value = Convert.ToInt64(node.Attributes["value"].Value);
                    make.Name = makeName;
                    make.Models = LoadModels(make.Value);
                    list.Add(make);
                }
            }

            return list;
        }

        private List<CarMaxModel> LoadModels(long makeId)
        {
            var list = new List<CarMaxModel>();

            var url = "https://api.carmax.com/v1/api/vehicles/models/4294963045?platform=carmax.com&apikey=adfb3ba2-b212-411e-89e1-35adab91b600";
            var content = WebHandler.DownloadContent(url);
            var xmlDocument = WebHandler.DownloadDocument(content);

            var modelNodes = xmlDocument.SelectNodes("//RefinementModel");

            //var request = (HttpWebRequest)WebRequest.Create("http://www.carmax.com/data/LookupOASCapable.aspx/GetFilteredModelOptions");
            //request.Method = "POST";
            //request.Accept = "application/atom+xml,application/xml";
            //request.UserAgent = UserAgent;
            //var postData = CreateDataToLoadModel(new CarMaxModelPost()
            //{
            //    makeId = makeId,
            //    typeId = 0,
            //    usedNew = "u"
            //});
            //request.ContentLength = postData.Length;
            //request.ContentType = "application/json; charset=utf-8";
            //request.Referer = "http://www.carmax.com/data/LookupOASCapable.aspx/GetFilteredModelOptions";
            //// Post to the login form.
            //var streamWriter = new StreamWriter(request.GetRequestStream());
            //streamWriter.Write(postData);
            //streamWriter.Close();

            //// Get the response.
            //var response = (HttpWebResponse)request.GetResponse();

            //// Read the response
            //var streamReader = new StreamReader(response.GetResponseStream());
            //var result = streamReader.ReadToEnd();            
            //streamReader.Close();

            //var jsonObj = (JObject)JsonConvert.DeserializeObject(result);
            //var xmlDocument = WebHandler.DownloadDocument(Convert.ToString(((JValue)(jsonObj["d"])).Value));
            //var list = new List<CarMaxModel>();
            //var modelNodes = xmlDocument.SelectNodes("//*[.]");
            //if (modelNodes != null)
            //{
            //    foreach (XmlNode node in modelNodes)
            //    {
            //        var modelName = node.InnerText.Trim();
            //        if (node.Name.Equals("html") || new string[] { "All Models" }.Contains(modelName)) continue;
            //        var model = new CarMaxModel();
            //        model.Value = Convert.ToInt64(node.Attributes["value"].Value);
            //        model.Name = node.InnerText.Trim();

            //        list.Add(model);
            //    }
            //}

            return list;
        }

        private void LoadCars(string apiKey)
        {
            var page = 1;
            var url = string.Format("https://api.carmax.com/v1/api/vehicles/?SortKey=0&Distance=all&PerPage=50&Zip=92627&Page={1}&platform=carmax.com&apikey={0}", apiKey, page);
            var content = WebHandler.DownloadContent(url);
            var xmlDocument = WebHandler.DownloadDocument(content);
            var totalNode = xmlDocument.SelectSingleNode("//*[local-name()='resultcount']");
            if (totalNode == null) return;
            var totalOfRecords = Convert.ToInt32(totalNode.InnerText);
            var numberOfPages = totalOfRecords % 50 == 0 ? totalOfRecords / 50 : (totalOfRecords / 50) + 1;

            do
            {
                if (page > 1)
                {
                    //url = string.Format("https://api.carmax.com/v1/api/vehicles/?SortKey=0&Distance=all&PerPage=50&Zip=92627&Page={1}&platform=carmax.com&apikey={0}", apiKey, page);
                    xmlDocument = WebHandler.DownloadDocument(WebHandler.DownloadContent(url));
                }

                var resultNodes = xmlDocument.SelectNodes("//*[local-name()='results']/*[local-name()='resultsrecordmodel']");
                if (resultNodes != null)
                {
                    var vinmarketSqlHelper = new VINMarketSqlHelper();
                    var insertQueries = new List<string>();
                    var updateQueries = new List<string>();

                    foreach (XmlNode item in resultNodes)
                    {
                        try
                        {
                            var newVehicle = LoadDetailCar(item);

                            var id = vinmarketSqlHelper.GetId(string.Format("SELECT CarMaxVehicleId FROM CarMaxVehicle WHERE CarMaxVehicleId = @carId"), new { carId = newVehicle.CarMaxVehicleId });
                            if (id == 0)
                            {
                                var insert = CarMaxQuery.InsertQuery(newVehicle);
                                insertQueries.Add(insert);
                            }
                            else
                            {
                                var update = CarMaxQuery.UpdateQuery(newVehicle);
                                updateQueries.Add(update);
                            }
                            Console.WriteLine("{0} {1} - {2}", newVehicle.Make, newVehicle.Model, newVehicle.Url);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("CARMAX - InsertVehicleToDatabase {0}", ex.Message);
                        }
                    }

                    if (insertQueries.Any()) vinmarketSqlHelper.QueryText(string.Join(" ", insertQueries));
                    if (updateQueries.Any()) vinmarketSqlHelper.QueryText(string.Join(" ", updateQueries));
                }

                var urlNode = xmlDocument.SelectSingleNode("//*[local-name()='links']/*[local-name()='resourcelink']/*[local-name()='href']");
                if (urlNode != null) url = urlNode.InnerText;
                else url = null;

                page++;
                Console.WriteLine("Processing next page {0}", page);
            } while (page <= numberOfPages && url != null);
            
        }

        private void LoadCars(CarMaxStore store)
        {
            var searchUrl = store.InventoryUrl;
            
            while (!String.IsNullOrEmpty(searchUrl))
            {
                var content = WebHandler.DownloadContent(searchUrl);
                var xmlDocument = WebHandler.DownloadDocument(content);
                var aNodes = xmlDocument.SelectNodes("//div[@id='resultsList']/div[@class='car']");
                if (aNodes != null)
                {
                    foreach (XmlNode node in aNodes)
                    {
                        var carId = Convert.ToInt64(node.Attributes["sn"].Value);
                        try
                        {
                            _commonManagementForm.UpdateCarMaxVehicleStore(carId,store.StoreId);
                            
                        }
                        catch (Exception ex)
                        {
                            _logging.Error("CARMAX - InsertVehicleToDatabase", ex);
                            Console.WriteLine("CARMAX - InsertVehicleToDatabase {0}", ex.Message);
                        }
                    }
                }

                var nextNode = xmlDocument.SelectSingleNode("//li[@id='next']");
                searchUrl = nextNode != null && nextNode.Attributes["data-url"] != null ? store.InventoryUrl + "&" + nextNode.Attributes["data-url"].Value : string.Empty;
            }
        }

        private void LoadCars(long makeId, string make, long modelId, string model, int distance, int zipcode)
        {
            var vinmarketSqlHelper = new VINMarketSqlHelper();
            var originalUrl = string.Format("http://www.carmax.com/search?ANa={0}&D={1}&zip={2}&N={3}&Ep=search:results:results%20page", modelId, distance, zipcode, makeId);
            var searchUrl = originalUrl;
            while (!String.IsNullOrEmpty(searchUrl))
            {
                var content = WebHandler.DownloadContent(searchUrl);
                var xmlDocument = WebHandler.DownloadDocument(content);
                var aNodes = xmlDocument.SelectNodes("//div[@id='resultsList']/div[@class='car']");
                if (aNodes != null && aNodes.Count > 0)
                {
                    var insertQueries = new List<string>();
                    var updateQueries = new List<string>();

                    try
                    {
                        foreach (XmlNode node in aNodes)
                        {
                            var carUrl = "http://www.carmax.com" + node.Attributes["data-details-url"].Value;
                            var carId = Convert.ToInt64(node.Attributes["sn"].Value);
                            var vehicleNameNode = node.SelectSingleNode(".//a[@class='vehicleName']//h3");
                            var year = Convert.ToInt32(vehicleNameNode.InnerText.Substring(0, 4));
                            var trim = vehicleNameNode.InnerText.Replace(year.ToString(), "").Replace(make, "").Replace(model, "").Trim();
                            var newVehicle = LoadDetailCar(year, make, model, trim, carId, carUrl);
                            if (newVehicle == null) continue;

                            var imgNode = node.SelectSingleNode(".//div[@class='photo']//img");
                            newVehicle.FullPhotos = imgNode != null ? imgNode.Attributes["src"].Value : string.Empty;
                            newVehicle.ThumbnailPhotos = newVehicle.FullPhotos;

                            try
                            {
                                var id = vinmarketSqlHelper.GetId(string.Format("SELECT VehicleId FROM CarMaxVehicle WHERE VehicleID = @carId"), new { carId });
                                if (id == 0)
                                {
                                    var insert = CarMaxQuery.InsertQuery(newVehicle);
                                    insertQueries.Add(insert);
                                }
                                else
                                {
                                    newVehicle.VehicleId = id;
                                    var update = CarMaxQuery.UpdateQuery(newVehicle);
                                    updateQueries.Add(update);
                                }
                                //_commonManagementForm.AddNewCarMaxVehicle(newVehicle);
                                Console.WriteLine("{0} {1} - {2}", make, model, carUrl);
                            }
                            catch (Exception ex)
                            {
                                _logging.Error("CARMAX - InsertVehicleToDatabase", ex);
                                Console.WriteLine("CARMAX - InsertVehicleToDatabase {0}", ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    

                    if (insertQueries.Any()) vinmarketSqlHelper.QueryText(string.Join(" ", insertQueries));
                    if (updateQueries.Any()) vinmarketSqlHelper.QueryText(string.Join(" ", updateQueries));
                }

                var nextNode = xmlDocument.SelectSingleNode("//li[@id='next']");
                searchUrl = nextNode != null && nextNode.Attributes["data-url"] != null ? originalUrl + "&" + nextNode.Attributes["data-url"].Value : string.Empty;
            }
            
        }

        public void AddNewCarMaxVehicle(int year, string make, string model, string trim, long carId, string url) 
        {
            var newVehicle = LoadDetailCar(year, make, model, trim, carId, url);
            var vinmarketSqlHelper = new VINMarketSqlHelper();
            try
            {
                var id = vinmarketSqlHelper.GetId(string.Format("SELECT VehicleId FROM CarMaxVehicle WHERE VehicleID = @carId"), new { carId });
                if (id == 0)
                {
                    var insert = CarMaxQuery.InsertQuery(newVehicle);
                    vinmarketSqlHelper.QueryText(insert);
                }
                else
                {
                    newVehicle.VehicleId = id;
                    var update = CarMaxQuery.UpdateQuery(newVehicle);
                    vinmarketSqlHelper.QueryText(update);
                }
                Console.WriteLine("{0} {1} - {2}", make, model, url);
            }
            catch (Exception ex)
            {
                _logging.Error("CARMAX - InsertVehicleToDatabase", ex);
                Console.WriteLine("CARMAX - InsertVehicleToDatabase {0}", ex.Message);
            }
        }

        public CarMaxVehicle LoadDetailCar(XmlNode node)
        {
            var newVehicle = new CarMaxVehicle();
            try
            {
                var apiUrlNode = node.SelectSingleNode("*[local-name()='links']/*[local-name()='resourcelink']/*[local-name()='href']");
                if (apiUrlNode == null) return null;
                newVehicle.CarMaxVehicleId = Convert.ToInt32(Path.GetFileName(apiUrlNode.InnerText));
                newVehicle.Url = string.Format("https://www.carmax.com/cars/{0}", newVehicle.CarMaxVehicleId);
                newVehicle.Year = Convert.ToInt32(node.SelectSingleNode("*[local-name()='year']").InnerText);
                newVehicle.Make = node.SelectSingleNode("*[local-name()='make']").InnerText;
                newVehicle.Model = node.SelectSingleNode("*[local-name()='model']").InnerText;
                newVehicle.Price = Convert.ToInt32(node.SelectSingleNode("*[local-name()='price']").InnerText);
                newVehicle.Miles = Convert.ToInt32(node.SelectSingleNode("*[local-name()='miles']").InnerText.Replace("K", "000"));
                newVehicle.ExteriorColor = node.SelectSingleNode("*[local-name()='exteriorcolor']").InnerText;
                newVehicle.InteriorColor = node.SelectSingleNode("*[local-name()='interiorcolor']").InnerText;
                newVehicle.Used = node.SelectSingleNode("*[local-name()='isnew']").InnerText.Equals("false");
                newVehicle.FullPhotos = node.SelectSingleNode("*[local-name()='photourl']").InnerText;
                newVehicle.ThumbnailPhotos = newVehicle.FullPhotos;
                newVehicle.Stock = node.SelectSingleNode("*[local-name()='stocknumber']").InnerText;
                newVehicle.DriveTrain = node.SelectSingleNode("*[local-name()='drivetrain']").InnerText;
                newVehicle.Transmission = node.SelectSingleNode("*[local-name()='transmission']").InnerText;
                newVehicle.Vin = node.SelectSingleNode("*[local-name()='vin']").InnerText;
                var carmaxStoreId = Convert.ToInt32(node.SelectSingleNode("*[local-name()='storeid']").InnerText);
                newVehicle.StoreId = (new VINMarketSqlHelper()).GetId("SELECT StoreId FROM CarMaxStore WHERE CarMaxStoreId = " + carmaxStoreId);

                newVehicle.UpdatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
                newVehicle.CreatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            }
            catch (Exception ex)
            {
                throw;
            }

            return newVehicle;
        }

        public CarMaxVehicle LoadDetailCar(int year, string make, string model, string trim, long carId, string url)
        {
            var newVehicle = new CarMaxVehicle() { CarMaxVehicleId = carId, Year = year, Make = make, Model = model, Trim = trim, Url = url };
            try
            {                
                var content = WebHandler.DownloadContent(url);
                var xmlDocument = WebHandler.DownloadDocument(content);
                var vinNode = xmlDocument.SelectSingleNode("//*[@class='vin']");
                if (vinNode == null) return LoadDetailCarNew(newVehicle);

                newVehicle.Vin = vinNode.ChildNodes[1].Value.Trim();
                newVehicle.Price = CommonHelper.RemoveSpecialCharactersAndReturnNumber(xmlDocument.SelectSingleNode("//*[@class='sansser']/*").ChildNodes[0].Value.Trim());
                //var certifiedNode = xmlDocument.SelectSingleNode("//*[@class='info-container cqc_tombstone_pos']/*[last()]/*/*/*/*/*[last()-1]/*[last()]");
                //newVehicle.Certified = certifiedNode != null && certifiedNode.InnerText.Contains("CarMax Quality Certified");
                var infoNodes = xmlDocument.SelectNodes("//*[@class='info-container cqc_tombstone_pos']/*[last()]/*/*/*/*[last()]/*");
                if (infoNodes != null)
                    foreach (XmlNode item in infoNodes)
                    {
                        if (!(item.Name.Equals("li")) || item.ChildNodes[1] == null) continue;

                        var value = item.ChildNodes[1].Value;
                        switch (item.FirstChild.InnerText)
                        {
                            case "Miles":
                                newVehicle.Miles = value.Equals("New") ? 0 : Convert.ToInt32(value.ToLower().Replace("k", "000").Replace("<", "").Replace(">", ""));
                                newVehicle.Used = !value.Equals("New");
                                break;
                            case "Drive":
                                newVehicle.DriveTrain = value;
                                break;
                            case "Transmission":
                                newVehicle.Transmission = value;
                                break;
                            case "Exterior":
                                newVehicle.ExteriorColor = value;
                                break;
                            case "Interior":
                                newVehicle.InteriorColor = value;
                                break;
                            case "EPA Mileage":
                                var tmp = value.Replace("MPG", "").Split('/');
                                newVehicle.MPGCity = value.Equals("Not Available") ? 0 : Convert.ToInt32(tmp[0]);
                                newVehicle.MPGHighway = value.Equals("Not Available") ? 0 : Convert.ToInt32(tmp[1]);
                                break;
                            case "Stock #":
                                newVehicle.Stock = value;
                                break;
                            case "Rating":
                                newVehicle.Rating = Convert.ToDecimal(item.ChildNodes[1].InnerText.Replace("(", "").Replace(")", ""));
                                break;
                        }
                    }

                var storeNode = xmlDocument.SelectSingleNode("//*[@id='qualifiers-global']");
                if (storeNode != null)
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(storeNode.Attributes["data-qualifiers"].Value);
                    var carmaxStoreId = Convert.ToInt32(((JValue)jsonObj["StoreId"]).Value);
                    newVehicle.StoreId = (new VINMarketSqlHelper()).GetId("SELECT StoreId FROM CarMaxStore WHERE CarMaxStoreId = " + carmaxStoreId);
                }

                var features = xmlDocument.SelectNodes("//*[@class='features']/*[2]/*");
                if (features != null)
                {
                    var temp = (from XmlNode item in features select item.InnerText.Trim()).ToList();
                    newVehicle.Features = temp.Any() ? temp.Aggregate((a, b) => a + ',' + b) : string.Empty;
                }

                //var thumbnailImages = xmlDocument.SelectNodes("//*[@id='thumbs']/*");
                //var fullImages = xmlDocument.SelectNodes("//*[@id='photos']/*");
                //if (fullImages != null)
                //{
                //    var temp = (from XmlNode item in fullImages select item.Attributes["src"].Value).ToList();
                //    newVehicle.FullPhotos = temp.Any() ? temp.Aggregate((a, b) => a + ',' + b) : string.Empty;
                //    newVehicle.ThumbnailPhotos = newVehicle.FullPhotos;
                //}

                newVehicle.UpdatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
                newVehicle.CreatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            }
            catch (Exception ex)
            {
                _logging.Error("CARMAX - LoadDetailVehicle", ex);
            }

            return newVehicle;
        }

        public CarMaxVehicle LoadDetailCarNew(CarMaxVehicle newVehicle)
        {
            //imageUrl = https://img2.carmax.com/api/images/12702986
            var content = WebHandler.DownloadContent("https://beta.carmax.com/cars/" + newVehicle.CarMaxVehicleId);
            var xmlDocument = WebHandler.DownloadDocument(content);
            var vinNode = xmlDocument.SelectSingleNode("//*[@class='card-grid-section--action-bar-item'][2]/span[2]");
            if (vinNode == null) return null;
            newVehicle.Vin = vinNode.InnerText.Trim();
            var infoNodes = xmlDocument.SelectNodes("//div[@class='card--text-block-list--row']");
            if (infoNodes != null)
                foreach (XmlNode item in infoNodes)
                {
                    if (item.ChildNodes[1] == null || item.ChildNodes[3] == null) continue;

                    var text = item.ChildNodes[1].InnerText.Trim();
                    var value = item.ChildNodes[3].InnerText.Trim();
                    switch (text)
                    {
                        case "Drive":
                            newVehicle.DriveTrain = value;
                            break;
                        case "Transmission":
                            newVehicle.Transmission = value;
                            break;
                        case "Exterior":
                            newVehicle.ExteriorColor = value;
                            break;
                        case "Interior":
                            newVehicle.InteriorColor = value;
                            break;                        
                    }
                }

            var featureNodes = xmlDocument.SelectNodes("//ul/li[@class='card-grid-section--item-list-element']");
            if (featureNodes != null)
            {
                var list = new List<string>();
                foreach (XmlNode item in featureNodes)
                {
                    list.Add(item.InnerText);
                }
                newVehicle.Features = string.Join(",", list);
            }

            var storeNode = xmlDocument.SelectSingleNode("//*[@id='qualifiers-global']");
            if (storeNode != null)
            {
                var jsonObj = (JObject)JsonConvert.DeserializeObject(storeNode.Attributes["data-qualifiers"].Value);
                var carmaxStoreId = Convert.ToInt32(((JValue)jsonObj["StoreId"]).Value);
                newVehicle.StoreId = (new VINMarketSqlHelper()).GetId("SELECT StoreId FROM CarMaxStore WHERE CarMaxStoreId = " + carmaxStoreId);
            }

            var stockNode = xmlDocument.SelectSingleNode("//*[@class='card-grid-section--action-bar-item'][1]/span[2]");
            if (stockNode != null) newVehicle.Stock = stockNode.InnerText.Trim();
            var mpgCityNode = xmlDocument.SelectSingleNode("//*[@class='mpg']/*[@class='mpg--value']");
            if (mpgCityNode != null) newVehicle.MPGCity = Convert.ToInt32(mpgCityNode.InnerText.Trim());
            var mpgHighway = xmlDocument.SelectSingleNode("//*[@class='mpg'][2]/*[@class='mpg--value']");
            if (mpgHighway != null) newVehicle.MPGHighway = Convert.ToInt32(mpgHighway.InnerText.Trim());
            var priceNode = xmlDocument.SelectSingleNode("//*[@class='price-mileage--price-container']/*[@class='price-mileage--value']");
            if (priceNode != null) newVehicle.Price = CommonHelper.RemoveSpecialCharactersAndReturnNumber(priceNode.InnerText.Trim());
            var milesNode = xmlDocument.SelectSingleNode("//*[@class='price-mileage--mileage-container']/*[@class='price-mileage--value']");
            if (milesNode != null) newVehicle.Miles = Convert.ToInt32(milesNode.InnerText.Trim().ToLower().Replace("k", "000").Replace("<", "").Replace(">", ""));
            newVehicle.Used = newVehicle.Miles > 1000;
            newVehicle.UpdatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);
            newVehicle.CreatedDate = DataCommonHelper.GetChicagoDateTime(DateTime.Now);            
            
            return newVehicle;
        }

        private int GettingStoreId(XmlDocument xmlDocument, string url)
        {
            var store = xmlDocument.SelectSingleNode("//*[@class='visitus-appt']/*[2]");
            if (store != null)
            {
                return _commonManagementForm.GetCarMaxStore(store.InnerText.Trim()).StoreId;
            }
            else
            {
                var tryTo = 2;
                while (tryTo > 0)
                {
                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        request.UserAgent = UserAgent;
                        var postData = new StringBuilder();
                        postData.Append((String.Format("ScriptManager1={0}&", "ScriptManager1%7Cctl08%24ucTabs%24tab8Submit")));
                        postData.Append((String.Format("__EVENTTARGET={0}&", "ctl08%24ucTabs%24tab8Submit")));
                        postData.Append((String.Format("__ASYNCPOST=true")));

                        request.ContentLength = postData.Length;
                        request.ContentType = ContentType;
                        request.Referer = url;
                        var streamWriter = new StreamWriter(request.GetRequestStream());
                        streamWriter.Write(postData);
                        streamWriter.Close();

                        // Get the response.
                        var response = (HttpWebResponse)request.GetResponse();

                        // Read the response
                        var streamReader = new StreamReader(response.GetResponseStream());
                        var result = streamReader.ReadToEnd();
                        streamReader.Close();
                        var subXmlDocument = WebHandler.DownloadDocument(result);
                        var storeNode = subXmlDocument.SelectSingleNode("//*[@id='spnNearestStore']/*[2]");
                        if (storeNode == null) _logging.Error("NULL Store ID: " + url + "/n " + subXmlDocument);
                        tryTo = 0;
                        return storeNode != null ? _commonManagementForm.GetCarMaxStore(storeNode.InnerText.Trim()).StoreId : 0;
                    }
                    catch (Exception)
                    {
                        tryTo--;
                    }
                }

                return 0;
            }
        }

        private string CreateDataToLoadModel(CarMaxModelPost contract)
        {
            return JsonConvert.SerializeObject(contract);
        }
        #endregion
    }

    public class CarMaxState
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }

    public class CarMaxMake
    {
        public long Value { get; set; }
        public string Name { get; set; }
        public List<CarMaxModel> Models { get; set; }
    }

    public class CarMaxModel
    {
        public long Value { get; set; }
        public string Name { get; set; }
    }

    public class CarMaxModelPost
    {
        public long makeId { get; set; }
        public string usedNew { get; set; }
        public int typeId { get; set; }
    }
}
