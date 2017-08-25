using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using vincontrol.Application.VinMarket.Forms.CommonManagement;
using vincontrol.Application.VinMarket.ViewModels.CommonManagement;
using VINControl.Logging;
using VINControl.WebHelper;

namespace VINControl.TruckTrader
{
    public class TruckTraderService
    {
        private ICommonManagementForm _commonManagementForm;
        private ILoggingService _logging;
        #region Const
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:26.0) Gecko/20100101 Firefox/26.0; " +
                                         "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                         ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                         "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private const string AcceptLanguage = "Accept-Language: en-US,en;q=0.5";
        private const string AcceptEncoding = "Accept-Encoding: gzip, deflate";
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string Host = "http://www.commercialtrucktrader.com";
        #endregion

        public TruckTraderService() 
        {
            _commonManagementForm = new CommonManagementForm();
            _logging = new LoggingService();
        }

        #region Implementation

        public void InsertTruckDealerIntoDatabase()
        {
            try
            {
                //_logging.Info("*** START to scrap Truck Dealer ***");
                Console.WriteLine("START to scrap Truck Dealer");
                var stateUrls = GetStateSearchUrls();
                GetDealerInfo(stateUrls);
                //_logging.Info("*** END to scrap Truck Dealer ***");
                Console.WriteLine("END to scrap Truck Dealer");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR {0}", ex.Message + " " + ex.StackTrace);
                Console.ReadLine();
            }
        }

        public void InsertTruckDealerIntoDatabase(List<CommercialTruckDealerViewModel> dealers)
        {
            foreach (var dealer in dealers)
            {
                try
                {
                    _commonManagementForm.AddNewCommercialTruckDealer(dealer);                    
                }
                catch (Exception ex)
                {
                    _logging.Error(String.Format("Failed to insert {0}", dealer.Name), ex);
                }
            }
        }

        public void InsertTruckIntoDatabase(List<CommercialTruckViewModel> trucks)
        {
            foreach (var truck in trucks)
            {
                try
                {
                    _commonManagementForm.AddNewCommercialTruck(truck);                    
                }
                catch (Exception ex)
                {
                    _logging.Error(String.Format("Failed to insert {0} {1} {2}", truck.Year, truck.Make, truck.Model), ex);
                }
            }
        }

        public List<string> GetStateSearchUrls()
        {
            const string findUrl = "http://www.commercialtrucktrader.com/find";
            var list = new List<string>();
            var content = WebHandler.DownloadContent("http://www.commercialtrucktrader.com/find/dealer-search/");
            var xmlDocument = WebHandler.DownloadDocument(content);
            //var nsManager = WebHandler.GetXmlNamespaceManager(xmlDocument);
            var stateNodes = xmlDocument.SelectNodes("//*[@id='stCntr']/*[@class='stLst']/a/@href");
            if (stateNodes == null) return list;
            list.AddRange(from XmlNode node in stateNodes select node.Value.Replace("..", findUrl));

            return list;
        }

        public List<string> GetAllCategories()
        {
            var list = new List<string>();
            var content = WebHandler.DownloadContent("http://www.commercialtrucktrader.com");
            var xmlDocument = WebHandler.DownloadDocument(content);
            //var nsManager = WebHandler.GetXmlNamespaceManager(xmlDocument);
            var stateNodes = xmlDocument.SelectNodes("//*[@id='typeAny']/option");
            if (stateNodes == null) return list;
            list.AddRange(from XmlNode node in stateNodes select node.Value);

            return list;
        }

        public List<CommercialTruckDealerViewModel> GetDealerInfo(List<string> urls)
        {
            var dealers = new List<CommercialTruckDealerViewModel>();
            foreach (var url in urls)
            {
                var nextPageUrl = url;
                while (!string.IsNullOrEmpty(nextPageUrl))
                {
                    var content = WebHandler.DownloadContent(nextPageUrl.Contains(Host) ? nextPageUrl : String.Format("{0}{1}", Host, nextPageUrl));
                    Console.WriteLine(nextPageUrl);
                    var xmlDocument = WebHandler.DownloadDocument(content);
                    if (xmlDocument == null) break;

                    var dealerNodes = xmlDocument.SelectNodes("//*[@class='gdBx2 prmDlr' or @class='gdBx2 bscDlr']");
                    if (dealerNodes == null) break;
                    foreach (XmlNode node in dealerNodes)
                    {
                        try
                        {
                            var id = node.Attributes != null && node.Attributes["id"] != null ? Convert.ToInt32(node.Attributes["id"].Value) : 0;
                            if (id.Equals(0))
                                dealers.Add(ParseStandardDealer(node));
                            else
                                dealers.Add(ParseFeaturedDealer(node));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR when parsing dealer info {0}", ex.Message + " " + ex.StackTrace);
                        }                        
                    }
                    
                    var nextPageNode = xmlDocument.SelectSingleNode("//*[@id='sortnpaginate']/*[@class='lfloat paginate']/a[@title='Next Page']/@href");
                    nextPageUrl = nextPageNode != null ? nextPageNode.Value : string.Empty;

                    InsertTruckDealerIntoDatabase(dealers);
                    dealers.Clear();
                }
                
            }

            return dealers;
        }

        public void GetNewTrucks()
        {            
            GetTrucks("http://www.commercialtrucktrader.com/search-results?condition=N&schemecode=AD&&");
        }

        public void GetUsedTrucks()
        {
            GetTrucks("http://www.commercialtrucktrader.com/search-results?condition=U&schemecode=AD&&");
        }

        public void GetTrucksByCategory(string category)
        {
            try
            {
                //_logging.Info(String.Format("START to scrap truck by {0}", category));
                Console.WriteLine("START to getting cars on CommercialTrucks with {0}", category);
                //var url = String.Format("http://www.commercialtrucktrader.com/{0} For Sale/search-results?category={1}", category, category);
                var url = ConfigurationManager.AppSettings["commercialtruck:Url"];
                GetTrucks(url, category);
                //_logging.Info(String.Format("END to scrap truck by {0}", category));
                Console.WriteLine("END to getting cars on CommercialTrucks with {0}", category);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR getting trucks in {0}: {1}", category, e.Message + " " + e.StackTrace);
                Console.ReadLine();
            }
            
        }

        public void MarkSoldCommercialTrucks()
        {
            //_logging.Info(String.Format("START to mark sold truck"));
            Console.WriteLine("START to marking sold on CommercialTrucks");
            _commonManagementForm.MarkSoldCommercialTrucks();
            //_logging.Info(String.Format("START to mark sold truck"));
            Console.WriteLine("END to marking sold on CommercialTrucks");
        }

        #endregion

        #region Private Methods

        private void GetTrucks(string url, string category = "Other Truck")
        {
            var list = new List<CommercialTruckViewModel>();
            var nextPageUrl = url;
            while (!string.IsNullOrEmpty(nextPageUrl))
            {
                Console.WriteLine("PAGE: {0}", nextPageUrl);
                var content = WebHandler.DownloadContent(nextPageUrl, 2);
                if (String.IsNullOrEmpty(content)) break;
                var xmlDocument = WebHandler.DownloadDocument(content);
                if (xmlDocument == null) break;

                var truckNodes = xmlDocument.SelectNodes("//*[@class='listing']");
                if (truckNodes == null) break;
                foreach (XmlNode node in truckNodes)
                {
                    try
                    {
                        var newTruck = GetDetailTruck(node);
                        newTruck.BodyStyle = category;
                        list.Add(newTruck);
                        Console.WriteLine("{0} -- {1} {2} {3}", list.Count, newTruck.Year, newTruck.Make, newTruck.Model);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: {0}", ex.StackTrace);
                    }
                }

                var nextPageNode = xmlDocument.SelectSingleNode("//a[@class='listings-pag-selected']").NextSibling;
                nextPageUrl = nextPageNode != null && !nextPageNode.InnerText.Equals("Next") ? (nextPageNode.InnerText.Equals(". . .") ? Host + xmlDocument.SelectSingleNode("//a[@class='listings-pag-next-selectexpand']").Attributes["href"].Value : Host + nextPageNode.Attributes["href"].Value) : string.Empty;

                InsertTruckIntoDatabase(list);
                list.Clear();
            }
        }

        private void GetTrucks(string type, int startPage, int endPage)
        {
            var list = new List<CommercialTruckViewModel>();
            for (var i = startPage; i <= endPage; i++)
            {
                var nextPageUrl = String.Format("http://www.commercialtrucktrader.com/search-results?condition={0}&page={1}&schemecode=AD&&", type, i);
                Console.WriteLine("PAGE: {0}", nextPageUrl);
                var content = WebHandler.DownloadContent(nextPageUrl, 2);
                if (String.IsNullOrEmpty(content)) break;
                var xmlDocument = WebHandler.DownloadDocument(content);
                if (xmlDocument == null) break;

                var truckNodes = xmlDocument.SelectNodes("//*[@class='result noborder highlight-green' or @class='result noborder']");
                if (truckNodes == null) break;
                foreach (XmlNode node in truckNodes)
                {
                    try
                    {
                        var newTruck = GetDetailTruck(node);
                        list.Add(newTruck);
                        Console.WriteLine("{0} -- {1} {2} {3}", list.Count, newTruck.Year, newTruck.Make, newTruck.Model);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: {0}", ex.StackTrace);
                    }
                }

                //var nextPageNode = xmlDocument.SelectSingleNode("//*[@class='pageNumber']/a[text()='Next ›']/@href");
                //nextPageUrl = nextPageNode != null ? nextPageNode.Value : string.Empty;

                InsertTruckIntoDatabase(list);
                list.Clear();
            }
        }

        private CommercialTruckDealerViewModel ParseFeaturedDealer(XmlNode node)
        {
            var id = node.Attributes != null && node.Attributes["id"] != null ? Convert.ToInt32(node.Attributes["id"].Value) : 0;

            var nameNode = node.SelectSingleNode("./*[@class='lfloat prmDlrLft']/*[@class='prmDlrLogo']/*[@class='prmDlrLogoFx']");
            var name = nameNode != null ? nameNode.InnerText.Trim() : string.Empty;
            if (String.IsNullOrEmpty(name)) return null;

            var addressNode = node.SelectSingleNode("./*[@class='lfloat prmDlrLft']/*[@class='prmDlrAddy']");
            var addressContent = addressNode != null ? addressNode.InnerXml : string.Empty;
            var addressRegex = new Regex(@"(.*)<br\s/>");
            Match addressMatch = addressRegex.Match(addressContent);
            var address = addressMatch.Groups.Count > 0 ? addressMatch.Groups[1].Value : string.Empty;
            var citystatezipcodeRegex = new Regex(@"<br\s/>(.+)\s\[");
            Match citystatezipcodeMatch = citystatezipcodeRegex.Match(addressContent);
            var citystatezipcode = citystatezipcodeMatch.Groups.Count > 0 ? citystatezipcodeMatch.Groups[1].Value : string.Empty;
            var temp = citystatezipcode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var city = temp[0];
            var state = temp[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[0];
            var zipcode = Convert.ToInt32(temp[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1]);

            var mapUrl = String.Format("http://www.commercialtrucktrader.com/getmap.php?address={0}&city={1}&stateabbr={2}&zip={3}&dealerId={4}", address, city, state, zipcode, id.ToString());
            var mapContent = WebHandler.DownloadContent(mapUrl);
            var latlongRegex = new Regex(@"VELatLong\((.+)\);");
            Match latlongMatch = latlongRegex.Match(mapContent);
            var latlong = latlongMatch.Groups.Count > 0 ? latlongMatch.Groups[1].Value : string.Empty;
            var lat = 0.0;
            double.TryParse(latlong.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[0], out lat);
            var log = 0.0;
            double.TryParse(latlong.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1], out log);

            var phoneNode = node.SelectSingleNode("./*[@class='lfloat prmDlrLft']/*[@class='prmDlrPhone']/ul/li");
            var phone = phoneNode != null ? phoneNode.InnerText.Replace("Toll Free: ", "") : string.Empty;

            return new CommercialTruckDealerViewModel()
            {
                TruckDealerId = id,
                Name = name,
                Address = address,
                City = city,
                State = state,
                ZipCode = zipcode,
                Phone = phone,
                Latitude = lat,
                Longitude = log,
                DateStamp = DateTime.Now
            };
        }

        private CommercialTruckDealerViewModel ParseStandardDealer(XmlNode node)
        {
            var id = 0;

            var nameNode = node.SelectSingleNode("./*[@class='lfloat bscDlrInfo']/span");
            var name = nameNode != null ? nameNode.InnerText.Trim() : string.Empty;
            if (String.IsNullOrEmpty(name)) return null;

            var addressNode = node.SelectSingleNode("./*[@class='lfloat bscDlrInfo']");
            var addressContent = addressNode != null ? addressNode.InnerXml : string.Empty;
            var addresscitystatezipcodeRegex = new Regex(@"php\?(.+)\&");
            Match addresscitystatezipcodeMatch = addresscitystatezipcodeRegex.Match(addressContent);
            var addresscitystatezipcode = addresscitystatezipcodeMatch.Groups.Count > 0 ? HttpUtility.HtmlDecode(addresscitystatezipcodeMatch.Groups[1].Value) : string.Empty;
            var temp = addresscitystatezipcode.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var address = temp[0].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1];
            var city = temp[1].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1];
            var state = temp[2].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1];
            var zipcode = Convert.ToInt32(temp[3].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1]);
            if (id.Equals(0))
            {
                var idRegex = new Regex(@"dealerId\=(.+)\',\s");
                Match idMatch = idRegex.Match(addressContent);
                id = idMatch.Groups.Count > 0 ? Convert.ToInt32(idMatch.Groups[1].Value) : 0;
            }

            var mapUrl = String.Format("http://www.commercialtrucktrader.com/getmap.php?address={0}&city={1}&stateabbr={2}&zip={3}&dealerId={4}", address, city, state, zipcode, id.ToString());
            var mapContent = WebHandler.DownloadContent(mapUrl);
            var latlongRegex = new Regex(@"VELatLong\((.+)\);");
            Match latlongMatch = latlongRegex.Match(mapContent);
            var latlong = latlongMatch.Groups.Count > 0 ? latlongMatch.Groups[1].Value : string.Empty;
            var lat = 0.0;
            double.TryParse(latlong.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[0], out lat);
            var log = 0.0;
            double.TryParse(latlong.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[1],out log);

            return new CommercialTruckDealerViewModel()
            {
                TruckDealerId = id,
                Name = name,
                Address = address,
                City = city,
                State = state,
                ZipCode = zipcode,
                Latitude = lat,
                Longitude = log,
                DateStamp = DateTime.Now
            };
        }

        private CommercialTruckViewModel GetDetailTruck(XmlNode node)
        {
            if (node == null || node.Attributes == null) return null;

            var newTruck = new CommercialTruckViewModel() { Vin = String.Empty };
            //var idRegex = new Regex(@"listing.id\:(.*)\.dealer_type");
            //Match idMatch = idRegex.Match(node.Attributes["id"].Value);
            //newTruck.CommercialTruckId = idMatch.Groups.Count > 0 ? Convert.ToInt32(idMatch.Groups[1].Value) : 0;
            var urlDetailNode = node.SelectSingleNode("./div[@class='listing-info lfloat']/div/a");
            if (urlDetailNode == null) return null;
            newTruck.CommercialTruckId = Convert.ToInt32(urlDetailNode.Attributes["adid"].Value);

            if (newTruck.CommercialTruckId.Equals(0)) return null;

            //var singleNode = node.SelectSingleNode("./div/h3/a/@href");
            //if (singleNode != null)
            {
                var detailUrl = String.Format("{0}{1}", Host, urlDetailNode.Attributes["href"].Value);
                var subcontent = WebHandler.DownloadContent(detailUrl, 2);
                if (String.IsNullOrEmpty(subcontent)) return null;

                var subxmlDocument = WebHandler.DownloadDocument(subcontent);
                var selectSingleNode = subxmlDocument.SelectSingleNode("//div[@class='details-right']/strong");
                if (selectSingleNode != null)
                {
                    var dealer = _commonManagementForm.GetByCommercialTruckDealerName(selectSingleNode.InnerText);
                    newTruck.DealerId = dealer != null ? dealer.DealerId : 0;
                }

                newTruck.Url = detailUrl;
                //var selectSingleNode1 = subxmlDocument.SelectSingleNode("//span[@itemprop='releaseDate']");
                //if (selectSingleNode1 != null) newTruck.Year = Convert.ToInt32(selectSingleNode1.InnerText);
                //var singleNode1 = subxmlDocument.SelectSingleNode("//span[@itemprop='manufacturer']");
                //if (singleNode1 != null) newTruck.Make = singleNode1.InnerText;
                //var xmlNode = subxmlDocument.SelectSingleNode("//span[@itemprop='model']");
                //if (xmlNode != null) newTruck.Model = xmlNode.InnerText;
                //var categoryNode = subxmlDocument.SelectSingleNode("//span[@itemprop='category']");
                //newTruck.Category = categoryNode != null ? categoryNode.InnerText : string.Empty;

                var itemNodes = subxmlDocument.SelectNodes("//div[@class='details-right']/ul/li");
                if (itemNodes != null)
                    foreach (XmlNode item in itemNodes)
                    {
                        var name = item.SelectSingleNode("./span").InnerText;
                        var value = item.LastChild.Value;
                        switch (name)
                        {
                            case "Year:": newTruck.Year = Convert.ToInt32(value); break;
                            case "Make:": newTruck.Make = value; break;
                            case "Model:": newTruck.Model = value; break;
                            case "Trim:": newTruck.Trim = value; break;
                            case "Class:": newTruck.Class = value; break;
                            case "Mileage:": newTruck.Mileage = Convert.ToInt32(value); break;
                            case "New/Used:": newTruck.IsNew = !value.Equals("U"); break;
                            case "Color:": newTruck.ExteriorColor = value; break;
                            case "Fuel Tank Size:": newTruck.Litter = value; break;
                            case "Engine Model:": newTruck.Engine = value; break;
                            case "Fuel Type:": newTruck.Fuel = value; break;
                            case "Body Style:": newTruck.Fuel = value; break;
                            case "Transmission Speed:": newTruck.Transmission = value; break;
                            case "Drivetrain:": newTruck.Drivetrain = value; break;
                            case "VIN:": newTruck.Vin = value; break;
                            case "Price:":
                                newTruck.Price = value.Trim().Equals("Call for Price") ? 0 : Convert.ToInt32(value.Trim().Replace("$", "").Replace(",", ""));
                                break;
                        }
                    }

                var hiddenItemNodes = subxmlDocument.SelectNodes("//div[@class='details-right']/ul[@class='details-specs-more hide']/li");
                if (hiddenItemNodes != null)
                    foreach (XmlNode item in hiddenItemNodes)
                    {
                        var name = item.SelectSingleNode("./span").InnerText;
                        var value = item.LastChild.Value;
                        switch (name)
                        {
                            case "Year:": newTruck.Year = Convert.ToInt32(value); break;
                            case "Make:": newTruck.Make = value; break;
                            case "Model:": newTruck.Model = value; break;
                            case "Trim:": newTruck.Trim = value; break;
                            case "Class:": newTruck.Class = value; break;
                            case "Mileage:": newTruck.Mileage = Convert.ToInt32(value); break;
                            case "New/Used:": newTruck.IsNew = !value.Equals("U"); break;
                            case "Color:": newTruck.ExteriorColor = value; break;
                            case "Fuel Tank Size:": newTruck.Litter = value; break;
                            case "Engine Model:": newTruck.Engine = value; break;
                            case "Fuel Type:": newTruck.Fuel = value; break;
                            case "Body Style:": newTruck.Fuel = value; break;
                            case "Transmission Speed:": newTruck.Transmission = value; break;
                            case "Drivetrain:": newTruck.Drivetrain = value; break;
                            case "VIN:": newTruck.Vin = value; break;
                            case "Price:":
                                newTruck.Price = value.Trim().Equals("Call for Price") ? 0 : Convert.ToInt32(value.Trim().Replace("$", "").Replace(",", ""));
                                break;
                        }
                    }


                var imageNode = subxmlDocument.SelectSingleNode("//div[@class='details-left']/span/img");
                newTruck.Images = imageNode != null ? imageNode.Attributes["src"].Value : string.Empty;
                var descriptionNodes = subxmlDocument.SelectNodes("//div[@class='details-left']/h3[@class='bold font1-1']");
                foreach (XmlNode item in descriptionNodes)
                {
                    var name = item.InnerText;
                    var value = item.NextSibling != null && item.NextSibling.NextSibling != null ? item.NextSibling.InnerText : string.Empty;
                    switch (name)
                    {
                        case "Features": newTruck.Package = value; break;
                        case "Description": newTruck.Description = value; break;
                    }
                }
            }

            newTruck.DateStamp = DateTime.Now;
            newTruck.Updated = DateTime.Now;
            return newTruck;
        }

        #endregion
    }
}
