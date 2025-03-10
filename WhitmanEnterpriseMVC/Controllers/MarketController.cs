using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.DatabaseModel;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.Security;
using WhitmanEnterpriseMVC.Handlers;

namespace WhitmanEnterpriseMVC.Controllers
{
    public class MarketController : SecurityController
    {
        private const string PermissionCode = "KPI";
        private const string AcceptedValues = "ALLACCESS";

        public ActionResult Index()
        {
            return View();
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewKPIReport(ReportType reportType)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            switch (reportType)
            {
                case ReportType.Pdf:
                    return RedirectToAction("PrintExcelCarInfo", "PDF");                    
                default:
                case ReportType.Excel:
                    var viewModel = (InventoryFormViewModel)Session["InventoryObject"];
                    var dealer = (DealershipViewModel)Session["Dealership"];
                    return File(ReportHelper.ExportToCSV(viewModel.SubSetList, dealer.DealershipId, "KPI Report"), "application/vnd.ms-excel", "KPIReport.xlsx");                    
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewNewKPIReport(ReportType reportType)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            switch (reportType)
            {
                case ReportType.Pdf:
                    return RedirectToAction("PrintExcelCarInfo", "PDF");
                default:
                case ReportType.Excel:
                    var viewModel = (InventoryFormViewModel)Session["InventoryObject"];
                    var dealer = (DealershipViewModel)Session["Dealership"];
                    return File(ReportHelper.ExportToCSV(viewModel.SubSetList, dealer.DealershipId, "KPI Report"), "application/vnd.ms-excel", "KPIReport.xlsx");
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewKPI()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
        
            var context = new whitmanenterprisewarehouseEntities();

            var dtDealerShip = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(x=>x.NewUsed == "Used");

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();
            
            var viewModel = new InventoryFormViewModel {IsCompactView = false};

            var list = new List<CarInfoFormViewModel>();

            foreach (var drTmp in dtDealerShip)
            {
                var car = new CarInfoFormViewModel()
                              {
                                  ListingId = drTmp.ListingID,
                                  ModelYear = drTmp.ModelYear.GetValueOrDefault(),
                                  StockNumber = String.IsNullOrEmpty(drTmp.StockNumber) ? "" : drTmp.StockNumber,
                                  Model = String.IsNullOrEmpty(drTmp.Model) ? "" : drTmp.Model,
                                  Make = String.IsNullOrEmpty(drTmp.Make) ? "" : drTmp.Make,
                                  Vin = String.IsNullOrEmpty(drTmp.VINNumber) ? "" : drTmp.VINNumber,
                                  ExteriorColor = String.IsNullOrEmpty(drTmp.ExteriorColor) ? "" : drTmp.ExteriorColor,
                                  HasDescription = !String.IsNullOrEmpty(drTmp.Descriptions),
                                  SalePrice = String.IsNullOrEmpty(drTmp.SalePrice) ? "" : drTmp.SalePrice,
                                  Mileage = String.IsNullOrEmpty(drTmp.Mileage) ? "" : drTmp.Mileage,
                                  Trim = String.IsNullOrEmpty(drTmp.Trim) ? "" : drTmp.Trim,
                                  ChromeStyleId = drTmp.ChromeStyleId,
                                  ChromeModelId = drTmp.ChromeModelId,
                                  DateInStock = drTmp.DateInStock.GetValueOrDefault(),
                                  DaysInInvenotry = DateTime.Now.Subtract(drTmp.DateInStock.Value).Days,
                                  MarketRange = drTmp.MarketRange.GetValueOrDefault(),
                                  Reconstatus = drTmp.Recon.GetValueOrDefault(),
                                  CarImageUrl = drTmp.CarImageUrl,
                                  Loaner = drTmp.Loaner.GetValueOrDefault()
                              };

                if (!String.IsNullOrEmpty(drTmp.SalePrice) && !drTmp.SalePrice.Trim().Equals("0"))
                {
                   
                        car.HasSalePrice = true;
                }
                else
                {
                    car.HasSalePrice = false;
                }

                if (String.IsNullOrEmpty(drTmp.ThumbnailImageURL))
                {
                    car.HasImage = false;
                    car.SinglePhoto = String.IsNullOrEmpty(drTmp.DefaultImageUrl) ? "" : drTmp.DefaultImageUrl;
                }
                else
                {
                    string[] splitArray =
                        drTmp.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        car.HasImage = true;
                        car.SinglePhoto = splitArray.First();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(drTmp.DefaultImageUrl) &&
                            !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => drTmp.DealershipId==i.DealershipId).DefaultStockImageUrl) &&
                            !drTmp.CarImageUrl.Equals(drTmp.DefaultImageUrl) &&
                            !drTmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => drTmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
                        {
                            car.HasImage = true;
                            car.SinglePhoto = drTmp.ThumbnailImageURL;
                        }
                        else
                        {
                            car.SinglePhoto = String.IsNullOrEmpty(drTmp.DefaultImageUrl) ? "" : drTmp.DefaultImageUrl;
                        }
                    }
                }




                list.Add(car);
            }

            viewModel.CarsList = list;

            viewModel.SubSetList = list;

            viewModel.previousCriteria = "Year";

            viewModel.sortASCOrder = false;

            viewModel.SortSetList = SelectListHelper.InitalSortSetList();

            viewModel.DealershipActivities = LinqHelper.GetTop20Activities();

            Session["InventoryObject"] = viewModel;

            return View("ViewKPI", viewModel);
        }

        public ActionResult ViewNewKPI()
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = (DealershipViewModel)Session["Dealership"];

            var context = new whitmanenterprisewarehouseEntities();

            var dtDealerShip = InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(x =>x.NewUsed == "New");

            var dtDealerSetting = InventoryQueryHelper.GetSingleOrGroupSetting(context).ToList();


            var viewModel = new InventoryFormViewModel {IsCompactView = false};

            var list = new List<CarInfoFormViewModel>();


            foreach (var drTmp in dtDealerShip)
            {
                var car = new CarInfoFormViewModel()
                              {
                                  ListingId = drTmp.ListingID,
                                  ModelYear = drTmp.ModelYear.GetValueOrDefault(),
                                  StockNumber = String.IsNullOrEmpty(drTmp.StockNumber) ? "" : drTmp.StockNumber,
                                  Model = String.IsNullOrEmpty(drTmp.Model) ? "" : drTmp.Model,
                                  Make = String.IsNullOrEmpty(drTmp.Make) ? "" : drTmp.Make,
                                  Vin = String.IsNullOrEmpty(drTmp.VINNumber) ? "" : drTmp.VINNumber,
                                  ExteriorColor = String.IsNullOrEmpty(drTmp.ExteriorColor) ? "" : drTmp.ExteriorColor,
                                  HasDescription = !String.IsNullOrEmpty(drTmp.Descriptions),
                                  SalePrice = String.IsNullOrEmpty(drTmp.SalePrice) ? "" : drTmp.SalePrice,
                                  Mileage = String.IsNullOrEmpty(drTmp.Mileage) ? "" : drTmp.Mileage,
                                  ChromeStyleId = drTmp.ChromeStyleId,
                                  ChromeModelId = drTmp.ChromeModelId,
                                  Trim = String.IsNullOrEmpty(drTmp.Trim) ? "" : drTmp.Trim,
                                  DateInStock = drTmp.DateInStock.GetValueOrDefault(),
                                  DaysInInvenotry = DateTime.Now.Subtract(drTmp.DateInStock.Value).Days,
                                  MarketRange = drTmp.MarketRange.GetValueOrDefault(),
                                  Reconstatus = drTmp.Recon.GetValueOrDefault(),
                                  CarImageUrl = drTmp.CarImageUrl
                              };
                decimal price = 0;

                if (!String.IsNullOrEmpty(drTmp.SalePrice))
                {
                    bool flagtmp = Decimal.TryParse(drTmp.SalePrice.Substring(1, drTmp.SalePrice.Length - 1), out price);

                    if (flagtmp && price > 0)
                        car.HasSalePrice = true;
                }
                else
                {
                    car.HasSalePrice = false;
                }

                car.Price = price;


                if (String.IsNullOrEmpty(drTmp.ThumbnailImageURL))
                {
                    car.HasImage = false;
                    car.SinglePhoto = String.IsNullOrEmpty(drTmp.DefaultImageUrl) ? "" : drTmp.DefaultImageUrl;
                }
                else
                {
                    string[] splitArray =
                        drTmp.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries).ToArray
                            ();

                    if (splitArray.Count() > 1)
                    {
                        car.HasImage = true;
                        car.SinglePhoto = splitArray.First();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(drTmp.DefaultImageUrl) &&
                            !String.IsNullOrEmpty(dtDealerSetting.FirstOrDefault(i => drTmp.DealershipId==i.DealershipId).DefaultStockImageUrl) &&
                            !drTmp.CarImageUrl.Equals(drTmp.DefaultImageUrl) &&
                            !drTmp.CarImageUrl.Equals(dtDealerSetting.FirstOrDefault(i => drTmp.DealershipId == i.DealershipId).DefaultStockImageUrl))
                        {
                            car.HasImage = true;
                            car.SinglePhoto = drTmp.ThumbnailImageURL;
                        }
                        else
                        {
                            car.SinglePhoto = String.IsNullOrEmpty(drTmp.DefaultImageUrl) ? "" : drTmp.DefaultImageUrl;
                        }
                    }
                }






                list.Add(car);
            }

            viewModel.CarsList = list;

            viewModel.SubSetList = list;

            viewModel.previousCriteria = "Year";

            viewModel.sortASCOrder = false;

            viewModel.SortSetList = SelectListHelper.InitalSortSetList();

            viewModel.DealershipActivities = LinqHelper.GetTop20Activities();

            Session["InventoryObject"] = viewModel;

            return View("ViewNewKPI", viewModel);
        }

        public ActionResult ViewKPIForCarsWithCondition(int Condition)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (Session["InventoryObject"] == null)
                ViewKPI();
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];
            switch (Condition)
            {
                case 1:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry <= 15 && x.Loaner == false).ToList();
                    break;
                case 2:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30 && x.Loaner == false ).ToList();
                    break;
                case 3:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60 && x.Loaner == false).ToList();
                    break;
                case 4:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90 && x.Loaner == false).ToList();
                    break;
                case 5:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 90 && x.Loaner == false).ToList();
                    break;
                case 6:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => !x.HasImage && x.Loaner==false).ToList();
                    break;
                case 7:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => !x.HasDescription && x.Loaner == false).ToList();
                    break;
                case 8:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => !x.HasSalePrice && x.Loaner == false).ToList();
                    break;
                case 9:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.MarketRange == 3).ToList();
                    break;
                case 10:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.MarketRange == 2).ToList();
                    break;
                case 11:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.MarketRange == 1).ToList();
                    break;
                default:
                    ViewKPI();
                    break;
            }

            return View("ViewKPI", viewModel);
        }

        public ActionResult ViewNewKPIForCarsWithCondition(int Condition)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (Session["InventoryObject"] == null)
                ViewKPI();
            var viewModel = (InventoryFormViewModel)Session["InventoryObject"];
            switch (Condition)
            {
                case 1:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry <= 15).ToList();
                    break;
                case 2:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30).ToList();
                    break;
                case 3:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60).ToList();
                    break;
                case 4:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90).ToList();
                    break;
                case 5:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.DaysInInvenotry > 90).ToList();
                    break;
                case 6:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => !x.HasImage).ToList();
                    break;
                case 7:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => !x.HasDescription).ToList();
                    break;
                case 8:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => !x.HasSalePrice).ToList();
                    break;
                case 9:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.MarketRange == 1).ToList();
                    break;
                case 10:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.MarketRange == 0).ToList();
                    break;
                case 11:
                    viewModel.SubSetList = viewModel.CarsList.Where(x => x.MarketRange == -1).ToList();
                    break;
                default:
                    ViewKPI();
                    break;
            }

            return View("ViewNewKPI", viewModel);
        }

        public ActionResult SortBy(InventoryFormViewModel inventory)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            else
            {
                var viewModel = (InventoryFormViewModel)Session["InventoryObject"];


                switch (inventory.SelectedSortSet)
                {
                    case "Year":
                        viewModel.SubSetList = viewModel.SubSetList.OrderBy(x => x.ModelYear).ToList();
                        break;
                    case "Make":
                        viewModel.SubSetList = viewModel.SubSetList.OrderBy(x => x.Make).ToList();
                        break;
                    case "Model":
                        viewModel.SubSetList = viewModel.SubSetList.OrderBy(x => x.Model).ToList();
                        break;

                }

                return View("ViewKPI", viewModel);
            }
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult ViewEbay(int listingId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var vehicle = ConvertHelper.GetVehicleInfoForEbay(listingId, dealer.DealershipId);

                var viewModel = new EbayFormViewModel()
                {
                    AuctionType = SelectListHelper.InitalAuctionTypeList(),
                    Gallerys = SelectListHelper.InitalGalleryList(),
                    HoursToDeposit = SelectListHelper.InitalHoursList(),
                    AuctionLength = SelectListHelper.InitalAuctionLength(),
                    ExteriorColorList = SelectListHelper.InitalEbayExteriorColorList(),
                    InteriorColorList = SelectListHelper.InitalEbayInteriorColorList(),
                    ListingId = listingId.ToString(CultureInfo.InvariantCulture),
                    BuyItNowPrice = vehicle.SalePrice.Replace(",", ""),
                    HitCounter = true,

                    VehicleInfo = vehicle

                };
                Session["EbayAds"] = viewModel;

                return View("EbayForm", viewModel);
            }
            else
            {
                var viewModel = new EbayFormViewModel
                    {
                        AuctionType = SelectListHelper.InitalAuctionTypeList(),
                        Gallerys = SelectListHelper.InitalGalleryList(),
                        HoursToDeposit = SelectListHelper.InitalHoursList(),
                        AuctionLength = SelectListHelper.InitalAuctionLength(),
                        ExteriorColorList = SelectListHelper.InitalEbayExteriorColorList(),
                        InteriorColorList = SelectListHelper.InitalEbayInteriorColorList(),
                        ListingId = listingId.ToString(CultureInfo.InvariantCulture),
                        HitCounter = true,
                        SessionTimeOut = true,

                    };
                return View("EbayForm", viewModel);
            }
        }

        public ActionResult GetEbayAdsPrice(EbayFormViewModel ebay)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            var postEbayList = EbayHelper.GetPostEbayList(dealer.DealershipId, ebay.ListingId);

            var currentSessionEbay = (EbayFormViewModel)Session["EbayAds"];

            ebay.VehicleInfo = currentSessionEbay.VehicleInfo;

            ebay.VehicleInfo.DealershipAddress = dealer.DealershipAddress;

            ebay.Dealer = dealer;

            ebay.PostEbayList = postEbayList;

            var call = new APICall();

            string error = "";

            var builder = new StringBuilder();

            if (ebay.Propackbundle)
                builder.AppendLine("<ListingEnhancement>ProPackBundle</ListingEnhancement>");
            else
            {

                if (ebay.BoldTitle)
                    builder.AppendLine("<ListingEnhancement>BoldTitle</ListingEnhancement>");
                if (ebay.Highlight)
                    builder.AppendLine("<ListingEnhancement>Highlight</ListingEnhancement>");
                if (ebay.Border)
                    builder.AppendLine("<ListingEnhancement>Border</ListingEnhancement>");

            }
            ebay.XMLListingEnhancement = builder.ToString();
            
            ebay.EbayCategoryID = SQLHelper.GetEbayCategoryId(ebay.VehicleInfo.Make, ebay.VehicleInfo.Model);
            
            string strReq = EbayHelper.BuildEbayItemToVerify(ebay, dealer);

            XmlDocument xmlDoc = call.MakeAPICall(strReq, "VerifyAddItem", error);

            if (String.IsNullOrEmpty(error))
            {
                XmlNode root = xmlDoc["VerifyAddItemResponse"];

                XmlNode successNode = root["Ack"];
                

                //Session["MakeANdModel"] = root.InnerXml;
                if (successNode != null && (successNode.InnerText.Equals("Success") || successNode.InnerText.Equals("Warning")))
                {
                    double totalListingFee = 0;

                    if (root["Fees"] != null)
                    {

                        foreach (XmlNode node in root["Fees"].ChildNodes)
                        {
                            if (node.FirstChild.InnerText.Equals("ListingFee"))
                                totalListingFee = Convert.ToDouble(node.LastChild.InnerText);

                        }
                    }
                    string formatFeeByCurrency = String.Format("{0:C}", totalListingFee);
                    ebay.TotalListingFee = formatFeeByCurrency;

                }
                

            }

            Session["EbayAds"] = ebay;

            if (Request.IsAjaxRequest())
            {
                return Json("Price Successful");

            }
            return Json("Not Successful");
        }

        public ActionResult PreviewEbayAds()
        {

            var ebay = (EbayFormViewModel)Session["EbayAds"];

           //  DealershipViewModel dealer = (DealershipViewModel)Session["Dealership"];

            //string strReq = EbayHelper.BuildEbay  HTMLSource(ebay, dealer);
            //Session["XML"] = strReq;

            return View("PreviewEbayAds", ebay);
        }

        public ActionResult PostEbayAds()
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var ebay = (EbayFormViewModel)Session["EbayAds"];


                var call = new APICall();

                var builder = new StringBuilder();


                if (ebay.Propackbundle)
                    builder.AppendLine("<ListingEnhancement>ProPackBundle</ListingEnhancement>");
                else
                {

                    if (ebay.BoldTitle)
                        builder.AppendLine("<ListingEnhancement>BoldTitle</ListingEnhancement>");
                    if (ebay.Highlight)
                        builder.AppendLine("<ListingEnhancement>Highlight</ListingEnhancement>");
                    if (ebay.Border)
                        builder.AppendLine("<ListingEnhancement>Border</ListingEnhancement>");

                }

             
                var strReq = EbayHelper.BuildEbayItem(ebay, EbayHelper.BuildEbayHtmlSource(ebay, dealer));

                string error = "";

                var ebayItem = new PostEbayAds();

                string ebayItemId = "";

                var xmlDoc = call.MakeAPICall(strReq, "AddItem", error);

                if (String.IsNullOrEmpty(error))
                {


                    XmlNode root = xmlDoc["AddItemResponse"];

                    XmlNode successNode = root["Ack"];

                    if (successNode != null && (successNode.InnerText.Equals("Success") || successNode.InnerText.Equals("Warning")))
                    {
                        ebayItemId = root["ItemID"].InnerText;

                        if (!String.IsNullOrEmpty(ebayItemId))
                        {
                            ebayItem = RetrieveEbayItem(ebayItemId, dealer);

                            SQLHelper.InsertOrUpdateEbayAd(ebay.ListingId, ebayItem, dealer.DealershipId);
                        }

                    }


                }

                if (!String.IsNullOrEmpty(ebayItemId))

                    return Json(ebayItem.ebayAdURL);

                return Json("Fail");

                //return RedirectToAction("ViewInventory", "Inventory");

            }
            else
                return Json("SessionTimeOut");

        }

        public PostEbayAds RetrieveEbayItem(string itemId, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();

            var ebayAd = new PostEbayAds()
            {
                ebayAdID = itemId
            };

            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            builder.AppendLine("<GetItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">");

            builder.AppendLine("<ItemID>" + itemId + "</ItemID>");

            builder.AppendLine("<RequesterCredentials>");

            //builder.AppendLine("<eBayAuthToken>" + ConfigurationManager.AppSettings["UserToken"] + "</eBayAuthToken>");

            builder.AppendLine("<eBayAuthToken>" + dealer.EbayToken + "</eBayAuthToken>");

            builder.AppendLine("</RequesterCredentials>");


            builder.AppendLine("<Version>" + ConfigurationManager.AppSettings["Version"] + "</Version>");

            builder.AppendLine("<WarningLevel>High</WarningLevel>");

            builder.AppendLine("</GetItemRequest>");

            var call = new APICall();
            string error = "";
            
            XmlDocument xmlDoc = call.MakeAPICall(builder.ToString(), "GetItem", error);
            //string ebayItemUrl = "";

            if (String.IsNullOrEmpty(error))
            {
                XmlNode root = xmlDoc["GetItemResponse"];

                XmlNode successNode = root["Ack"];



                if (successNode != null && (successNode.InnerText.Equals("Success") || successNode.InnerText.Equals("Warning")))
                {
                    ebayAd.ebayAdURL = root["Item"]["ListingDetails"]["ViewItemURL"].InnerText;

                    ebayAd.ebayAdStartTime = DateTime.Parse(root["Item"]["ListingDetails"]["StartTime"].InnerText);

                    ebayAd.ebayAdEndTime = DateTime.Parse(root["Item"]["ListingDetails"]["EndTime"].InnerText);
                }


            }

            return ebayAd;

        }

        public ActionResult WholeSale()
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            return View("WholeSale");
        }

        public void OpenManaheimLoginWindow(string Vin)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            var encode = System.Text.Encoding.GetEncoding("utf-8");

            var manheimLink =
                "https://www.manheim.com/members/internetmmr/?vin=" + Vin;

            var myRequest = (HttpWebRequest)WebRequest.Create(manheimLink);

            myRequest.Method = "GET";

            var myResponse = myRequest.GetResponse();

            var ReceiveStream = myResponse.GetResponseStream();

            var ReadStream = new StreamReader(ReceiveStream, encode);


            string result = "";
            string line;
            while ((line = ReadStream.ReadLine()) != null)
            {
                if (line.Contains("stylesheets"))

                    line = line.Replace("href=\"/stylesheets", "href=\"https://www.manheim.com/stylesheets");
                else if (line.Contains("javascripts"))
                    line = line.Replace("src=\"/javascripts", "src=\"https://www.manheim.com/javascripts");
                else if (line.Contains("form accept-charset=\"UTF-8\" action"))
                    line = line.Replace("action=\"/login/authenticate\"", "action=\"https://www.manheim.com/login/authenticate\"");
                else
                {
                    if (!line.Contains("https://www.manheim.com"))
                        line = line.Replace("href=\"", "href=\"https://www.manheim.com");
                }

                //FINAL LOGIN
                if (line.Contains("Username:"))
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + dealer.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + dealer.ManheimPassword + "\" />" + Environment.NewLine;

                if (line.Contains("</body>"))
                {
                    line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                }

                result += line;

            }


            Response.Write(result);


        }

        public void OpenManaheimLoginWindowNoVin()
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            var encode = System.Text.Encoding.GetEncoding("utf-8");

            var manheimLink =
                "https://www.manheim.com/members/internetmmr/" ;

            var myRequest = (HttpWebRequest)WebRequest.Create(manheimLink);

            myRequest.Method = "GET";

            var myResponse = myRequest.GetResponse();

            var ReceiveStream = myResponse.GetResponseStream();

            var ReadStream = new StreamReader(ReceiveStream, encode);


            string result = "";
            string line;
            while ((line = ReadStream.ReadLine()) != null)
            {
                if (line.Contains("stylesheets"))

                    line = line.Replace("href=\"/stylesheets", "href=\"https://www.manheim.com/stylesheets");
                else if (line.Contains("javascripts"))
                    line = line.Replace("src=\"/javascripts", "src=\"https://www.manheim.com/javascripts");
                else if (line.Contains("form accept-charset=\"UTF-8\" action"))
                    line = line.Replace("action=\"/login/authenticate\"", "action=\"https://www.manheim.com/login/authenticate\"");
                else
                {
                    if (!line.Contains("https://www.manheim.com"))
                        line = line.Replace("href=\"", "href=\"https://www.manheim.com");
                }

                //FINAL LOGIN
                if (line.Contains("Username:"))
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + dealer.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + dealer.ManheimPassword + "\" />" + Environment.NewLine;

                if (line.Contains("</body>"))
                {
                    line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                }

                result += line;

            }


            Response.Write(result);


        }

        public void OpenManaheimLoginWindowForAppraisal(int appraisalId)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            var encode = System.Text.Encoding.GetEncoding("utf-8");

            var context = new whitmanenterprisewarehouseEntities();

            var contextMarket = new vincontrolscrappingEntities();

            var sampleVin = "";

            var searchAppraisal = context.whitmanenterpriseappraisals.First(x => x.idAppraisal == appraisalId);

            if (String.IsNullOrEmpty(searchAppraisal.VINNumber))
            {

                var query = MapperFactory.GetCarsComMarketCarQuery(contextMarket, searchAppraisal.ModelYear);

                var sampleCar = DataHelper.GetNationwideMarketData(query, searchAppraisal.Make, searchAppraisal.Model, searchAppraisal.Trim).First(x => !String.IsNullOrEmpty(x.Vin));

                if (sampleCar != null)

                    sampleVin = sampleCar.Vin;
            }
            
            var manheimLink = "";

            if (searchAppraisal != null)
            {
                if (!String.IsNullOrEmpty(searchAppraisal.VINNumber))
                {

                    manheimLink =
                        "https://www.manheim.com/members/internetmmr/?vin=" + searchAppraisal.VINNumber;
                }
                else
                {
                    if(!String.IsNullOrEmpty(sampleVin))
                        manheimLink =
                      "https://www.manheim.com/members/internetmmr/?vin=" + sampleVin;
                    else
                    {
                        manheimLink =
                 "https://www.manheim.com/members/internetmmr/";
                    }
                }
            }

            var myRequest = (HttpWebRequest)WebRequest.Create(manheimLink);

            myRequest.Method = "GET";

            var myResponse = myRequest.GetResponse();

            var ReceiveStream = myResponse.GetResponseStream();

            var ReadStream = new StreamReader(ReceiveStream, encode);


            string result = "";
            string line;
            while ((line = ReadStream.ReadLine()) != null)
            {
                if (line.Contains("stylesheets"))

                    line = line.Replace("href=\"/stylesheets", "href=\"https://www.manheim.com/stylesheets");
                else if (line.Contains("javascripts"))
                    line = line.Replace("src=\"/javascripts", "src=\"https://www.manheim.com/javascripts");
                else if (line.Contains("form accept-charset=\"UTF-8\" action"))
                    line = line.Replace("action=\"/login/authenticate\"", "action=\"https://www.manheim.com/login/authenticate\"");
                else
                {
                    if (!line.Contains("https://www.manheim.com"))
                        line = line.Replace("href=\"", "href=\"https://www.manheim.com");
                }

                //FINAL LOGIN
                if (line.Contains("Username:"))
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + dealer.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + dealer.ManheimPassword + "\" />" + Environment.NewLine;

                if (line.Contains("</body>"))
                {
                    line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                }

                result += line;

            }


            Response.Write(result);


        }

        public ActionResult GetKellyBlueBookSummaryByVin(string Vin, string Mileage)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                if (String.IsNullOrEmpty(Mileage))
                    Mileage = "0";

                var viewModel = KellyBlueBookHelper.GetDirectFullReport(Vin, dealer.ZipCode, Mileage);

                viewModel.Mileage = Mileage;

                return View("KellyBlueBookSummary", viewModel);

            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummary", viewModel);
            }
        }

        public ActionResult ResetKbbTrim(int ListingId)
        {
            var context = new whitmanenterprisewarehouseEntities();

            var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == ListingId);

            row.KBBTrimId = null;

            context.SaveChanges();

            return RedirectToAction("ViewIProfile", "Inventory", new { ListingId = ListingId });

        }
        
        public ActionResult GetKellyBlueBookSummary(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                string EffectiveDate = String.Format("{0:M/d/yyyy}", CommonHelper.GetLastFridayForKbb()) + "-" + String.Format("{0:M/d/yyyy}", DateTime.Now);

                var context = new whitmanenterprisewarehouseEntities();

                int LID = Convert.ToInt32(ListingId);

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                if (row.KBBTrimId == null)
                {

                    var viewModel = KellyBlueBookHelper.GetDirectFullReport(row.VINNumber, dealer.ZipCode, row.Mileage);

                    if (viewModel.Success)
                    {

                        viewModel.Mileage = row.Mileage;

                        viewModel.ListingId = ListingId;

                        viewModel.Disclaimer =
                            "© " + DateTime.Now.Year + " by Kelley Blue Book Co., Inc. " + EffectiveDate +
                            " .  All Rights Reserved. Vehicle valuations are opinions and may vary from vehicle to vehicle. Actual valuations will vary based upon market conditions, specifications, vehicle condition or other particular circumstances pertinent to this particular vehicle or the transaction or the parties to the transaction. This pricing is intended for the use of the individual generating this pricing only and shall not be sold or transmitted to another party. Kelley Blue Book assumes no responsibility for errors or omissions.";

                        ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;

                        if (viewModel.TrimReportList.Count > 1)
                        {
                            return View("KellyBlueBookSummary", viewModel);
                        }
                        else
                        {
                            viewModel.TrimId = viewModel.TrimReportList.First().TrimId;

                            return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
                        }
                    }
                    else
                    {
                        return View("KellyBlueBookSummary", viewModel);
                    }
                }
                else
                {
                    var viewModel = KellyBlueBookHelper.GetDirectFullReport(row.VINNumber, dealer.ZipCode, row.Mileage,
                                                                            row.KBBTrimId.Value, row.KBBOptionsId);

                    if (viewModel.Success)
                    {

                        viewModel.Mileage = row.Mileage;

                        viewModel.ListingId = ListingId;

                        viewModel.Disclaimer =
                            "© " + DateTime.Now.Year + " by Kelley Blue Book Co., Inc. " + EffectiveDate +
                            " .  All Rights Reserved. Vehicle valuations are opinions and may vary from vehicle to vehicle. Actual valuations will vary based upon market conditions, specifications, vehicle condition or other particular circumstances pertinent to this particular vehicle or the transaction or the parties to the transaction. This pricing is intended for the use of the individual generating this pricing only and shall not be sold or transmitted to another party. Kelley Blue Book assumes no responsibility for errors or omissions.";

                        ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;

                        viewModel.TrimId = viewModel.TrimReportList.First().TrimId;

                        return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
                    }
                    else
                    {
                        return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
                    }

                }
            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummary", viewModel);
            }
        }

        public ActionResult GetKellyBlueBookSummaryByTrim(string listingId, int trimId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                string EffectiveDate = String.Format("{0:M/d/yyyy}", CommonHelper.GetLastFridayForKbb()) + "-" + String.Format("{0:M/d/yyyy}", DateTime.Now);

                var context = new whitmanenterprisewarehouseEntities();

                int LID = Convert.ToInt32(listingId);

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                var viewModel = KellyBlueBookHelper.GetDirectFullReport(row.VINNumber, dealer.ZipCode, row.Mileage, trimId, row.KBBOptionsId);

                viewModel.Mileage = row.Mileage;

                viewModel.ListingId = listingId;

                viewModel.TrimId = trimId;


                viewModel.Disclaimer =
                    "© " + DateTime.Now.Year + " by Kelley Blue Book Co., Inc. " + EffectiveDate + " .  All Rights Reserved. Vehicle valuations are opinions and may vary from vehicle to vehicle. Actual valuations will vary based upon market conditions, specifications, vehicle condition or other particular circumstances pertinent to this particular vehicle or the transaction or the parties to the transaction. This pricing is intended for the use of the individual generating this pricing only and shall not be sold or transmitted to another party. Kelley Blue Book assumes no responsibility for errors or omissions.";

                ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;

                return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
            }
        }

        public ActionResult GetKellyBlueBookSummaryAppraisalByTrim(int appraisalId, int trimId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                string effectiveDate = String.Format("{0:M/d/yyyy}", CommonHelper.GetLastFridayForKbb()) + "-" + String.Format("{0:M/d/yyyy}", DateTime.Now);
                  var context = new whitmanenterprisewarehouseEntities();

                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == appraisalId);

              
                if (!String.IsNullOrEmpty(row.VINNumber))
                {

                    var viewModel = KellyBlueBookHelper.GetDirectFullReport(row.VINNumber, dealer.ZipCode, row.Mileage,
                                                                            trimId, row.KBBOptionsId);

                    viewModel.Mileage = row.Mileage;

                    viewModel.AppraisalId = appraisalId.ToString(CultureInfo.InvariantCulture);



                    ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;

                    viewModel.Disclaimer =
                        "© " + DateTime.Now.Year + " by Kelley Blue Book Co., Inc. " + effectiveDate +
                        " .  All Rights Reserved. Vehicle valuations are opinions and may vary from vehicle to vehicle. Actual valuations will vary based upon market conditions, specifications, vehicle condition or other particular circumstances pertinent to this particular vehicle or the transaction or the parties to the transaction. This pricing is intended for the use of the individual generating this pricing only and shall not be sold or transmitted to another party. Kelley Blue Book assumes no responsibility for errors or omissions.";

                    return View("KellyBlueBookSummaryAppraisalForSpecificTrim", viewModel);
                }
                else
                {
                    var sampleVin = "";

                    var contextMarket=new vincontrolscrappingEntities();

                    if (String.IsNullOrEmpty(row.VINNumber))
                    {

                        var query = MapperFactory.GetCarsComMarketCarQuery(contextMarket, row.ModelYear);

                        var sampleCar = DataHelper.GetNationwideMarketData(query, row.Make, row.Model, row.Trim).First(x => !String.IsNullOrEmpty(x.Vin));

                        if (sampleCar != null)

                            sampleVin = sampleCar.Vin;
                    }
                   

                    var viewModel = KellyBlueBookHelper.GetDirectFullReport(sampleVin, dealer.ZipCode, row.Mileage,
                                                                   trimId, row.KBBOptionsId);

                    viewModel.Mileage = row.Mileage;

                    viewModel.AppraisalId = appraisalId.ToString(CultureInfo.InvariantCulture);



                    ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;

                    viewModel.Disclaimer =
                        "© " + DateTime.Now.Year + " by Kelley Blue Book Co., Inc. " + effectiveDate +
                        " .  All Rights Reserved. Vehicle valuations are opinions and may vary from vehicle to vehicle. Actual valuations will vary based upon market conditions, specifications, vehicle condition or other particular circumstances pertinent to this particular vehicle or the transaction or the parties to the transaction. This pricing is intended for the use of the individual generating this pricing only and shall not be sold or transmitted to another party. Kelley Blue Book assumes no responsibility for errors or omissions.";

                    return View("KellyBlueBookSummaryAppraisalForSpecificTrim", viewModel);
                }


                

            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummaryAppraisalForSpecificTrim", viewModel);
            }
        }

        public ActionResult GetKellyBlueBookSummaryAppraisal(int appraisalId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == appraisalId);

                if (!String.IsNullOrEmpty(row.VINNumber))
                {

                    var viewModel = KellyBlueBookHelper.GetDirectFullReport(row.VINNumber, dealer.ZipCode, row.Mileage,
                                                                            row.KBBOptionsId);

                    viewModel.AppraisalId = appraisalId.ToString(CultureInfo.InvariantCulture);

                    viewModel.Mileage = row.Mileage;

                    ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;

                    if (viewModel.TrimReportList.Any())
                    {

                        if (viewModel.TrimReportList.Count > 1)
                        {

                            return View("KellyBlueBookSummaryAppraisal", viewModel);
                        }
                        else
                        {
                            viewModel.TrimId = viewModel.TrimReportList.First().TrimId;

                            return View("KellyBlueBookSummaryAppraisalForSpecificTrim", viewModel);
                        }
                    }
                    else
                    {
                        return View("KellyBlueBookSummaryAppraisal", viewModel);
                    }
                }
                else
                {
                    var sampleVin = "";

                    var contextMarket = new vincontrolscrappingEntities();

                    if (String.IsNullOrEmpty(row.VINNumber))
                    {

                        var query = MapperFactory.GetCarsComMarketCarQuery(contextMarket, row.ModelYear);

                        var sampleCar = DataHelper.GetNationwideMarketData(query, row.Make, row.Model, row.Trim).First(x => !String.IsNullOrEmpty(x.Vin));

                        if (sampleCar != null)

                            sampleVin = sampleCar.Vin;
                    }

                    var viewModel = KellyBlueBookHelper.GetDirectFullReport(sampleVin, dealer.ZipCode, row.Mileage,
                                                                            row.KBBOptionsId);

                    viewModel.AppraisalId = appraisalId.ToString(CultureInfo.InvariantCulture);

                    viewModel.Mileage = row.Mileage;

                    ViewData["CarTitle"] = row.ModelYear + " " + row.Make + " " + row.Model + " " + row.Trim;
                    if (viewModel.TrimReportList.Any())
                    {

                        if (viewModel.TrimReportList.Count > 1)
                        {

                            return View("KellyBlueBookSummaryAppraisal", viewModel);
                        }
                        else
                        {
                            viewModel.TrimId = viewModel.TrimReportList.First().TrimId;

                            return View("KellyBlueBookSummaryAppraisalForSpecificTrim", viewModel);
                        }
                    }
                    else
                    {
                        return View("KellyBlueBookSummaryAppraisal", viewModel);
                    }
                }


            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummaryAppraisal", viewModel);
            }
        }

        public ActionResult GetKellyBlueBookSummaryAdjustMileage(KellyBlueBookViewModel model)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var viewModel = KellyBlueBookHelper.GetDirectFullReport(model.Vin, dealer.ZipCode, model.Mileage);

                viewModel.Mileage = model.Mileage;
                viewModel.ListingId = model.ListingId;
                return View("KellyBlueBookSummary", viewModel);
            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummary", viewModel);
            }
        }

        public ActionResult GetKellyBlueBookSummaryAdjustMileageForSpecificTrim(KellyBlueBookViewModel model)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                int LID = Convert.ToInt32(model.ListingId);

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                var viewModel = KellyBlueBookHelper.GetDirectFullReport(model.Vin, dealer.ZipCode, model.Mileage, model.TrimId, row.KBBOptionsId);

                viewModel.Mileage = model.Mileage;

                viewModel.TrimId = model.TrimId;

                viewModel.ListingId = model.ListingId;

                return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
            }
        }

        public ActionResult GetKellyBlueBookSummaryAdjustMileageForSpecificTrimInAppraisal(KellyBlueBookViewModel model)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                int LID = Convert.ToInt32(model.AppraisalId);

                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == LID);

                var viewModel = KellyBlueBookHelper.GetDirectFullReport(model.Vin, dealer.ZipCode, model.Mileage, model.TrimId, row.KBBOptionsId);

                viewModel.Mileage = model.Mileage;

                viewModel.TrimId = model.TrimId;

                viewModel.ListingId = model.ListingId;

                return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
            }
            else
            {
                var viewModel = new KellyBlueBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("KellyBlueBookSummaryForSpecificTrim", viewModel);
            }
        }
        
        public ActionResult GetBlackBookSummaryByVin(string Vin, string Mileage)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                if (String.IsNullOrEmpty(Mileage))
                    Mileage = "0";

                BlackBookViewModel viewModel = BlackBookService.GetDirectFullReport(Vin, Mileage, dealer.State);

                viewModel.Mileage = Mileage;

                return View("BlackBookSummary", viewModel);
            }
            else
            {
                BlackBookViewModel viewModel = new BlackBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("BlackBookSummary", viewModel);
            }
        }

        public ActionResult GetBlackBookSummary(string ListingId)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var context = new whitmanenterprisewarehouseEntities();

                int LID = Convert.ToInt32(ListingId);

                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == LID);

                BlackBookViewModel viewModel = BlackBookService.GetDirectFullReport(row.VINNumber, row.Mileage, dealer.State);

                viewModel.Mileage = row.Mileage;

                return View("BlackBookSummary", viewModel);
            }
            else
            {
                BlackBookViewModel viewModel = new BlackBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("BlackBookSummary", viewModel);
            }
        }

        public ActionResult GetBlackBookSummaryAdjustMileage(BlackBookViewModel model)
        {
            if (Session["Dealership"] != null)
            {
                var dealer = (DealershipViewModel)Session["Dealership"];

                var viewModel = BlackBookService.GetDirectFullReport(model.Vin, model.Mileage, dealer.State);

                viewModel.Mileage = model.Mileage;
                return View("BlackBookSummary", viewModel);
            }
            else
            {
                var viewModel = new BlackBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("BlackBookSummary", viewModel);
            }
        }

        public ActionResult ViewAllDealerActivity()
        {
            return View("DealerActivity", LinqHelper.GetAllActivities());
        }

        public ActionResult FilterDealerActivity(int month, int year)
        {
            return PartialView("DealerActivityDetail", LinqHelper.FilterActivities(month, year));
        }

        public ActionResult RedirectToDetailActivity(int id)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var activity = context.vincontroldealershipactivities.FirstOrDefault(i => i.Id == id);
                if (activity == null) return RedirectToAction("ViewKPI");

                var listingId = 0;
                switch (activity.Type)
                {
                    case Constanst.ActivityType.AddToInventory:
                        listingId = GetListingIdFromActivityContent(activity.Detail.Split(';').ToArray()[1]);
                        return RedirectToAction("ViewIProfile", "Inventory", new {ListingID = listingId});
                    case Constanst.ActivityType.NewAppraisal:
                        var appraisalId = GetAppraisalIdFromActivityContent(activity.Detail.Split(';').ToArray()[0]);
                        return RedirectToAction("ViewProfileForAppraisal", "Appraisal", new { appraisalId });
                    case Constanst.ActivityType.NewUser:
                        return RedirectToAction("AdminSecurity", "Admin");
                    case Constanst.ActivityType.PriceChange:
                        listingId = GetListingIdFromActivityContent(activity.Detail.Split(';').ToArray()[0]);
                        return RedirectToAction("ViewIProfile", "Inventory", new { listingId });
                    default: return RedirectToAction("ViewKPI");
                }
            }
        }

        private int GetAppraisalIdFromActivityContent(string content)
        {
            try
            {
                return Convert.ToInt32(content.Replace("Appraisal Id: ", ""));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetListingIdFromActivityContent(string content)
        {
            try
            {
                return Convert.ToInt32(content.Replace("Listing Id: ", ""));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #region KarPower

        private int ConvertToInt32(JValue obj)
        {
            return obj != null ? Convert.ToInt32(obj.Value) : 0;
        }

        private string ConvertToString(JValue obj)
        {
            return obj != null ? Convert.ToString(obj.Value) : string.Empty;
        }

        private DateTime ConvertToDateTime(JValue obj)
        {
            return obj != null ? Convert.ToDateTime(obj.Value) : DateTime.Now;
        }

        private List<SelectListItem> CreateDataList(JToken obj, int selectedId)
        {
            var result = new List<SelectListItem>();
            if (obj != null)
            {
                result.AddRange(obj.Children().Where(item => !item.Value<string>("DisplayName").Equals("[Choose One]")).Select(item => new SelectListItem()
                {
                    Text = item.Value<string>("DisplayName"),
                    Value = item.Value<string>("Id"),
                    Selected = item.Value<int>("Id") == selectedId
                }));
            }

            return result;
        }

        private List<OptionContract> CreateOptionalEquipmentList(string data)
        {
            if (String.IsNullOrEmpty(data)) return new List<OptionContract>();

            var result = new List<OptionContract>();
            //var doc = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
            //doc.LoadXml(model.OptionalEquipmentMarkup);
            var doc = new HtmlDocument();
            doc.LoadHtml(data);
            var tdOptions = doc.DocumentNode.SelectNodes("//table/tr/td");
            foreach (var tdOption in tdOptions)
            {
                var span = tdOption.SelectSingleNode("span");
                var input = tdOption.SelectSingleNode("span/input");
                var label = tdOption.SelectSingleNode("span/label");
                if (span == null || input == null || label == null) continue;

                //if (model.OptionalEquipmentMarkupList != null && !model.OptionalEquipmentMarkupList.Any(i => i.Id == Convert.ToString(span.Attributes["recordnumber"])))
                //{
                result.Add(new OptionContract()
                {
                    __type = "KBB.Karpower.WebServices.LightOption",
                    Id = Convert.ToString(span.Attributes["recordnumber"].Value),
                    IsSelected = (input.Attributes["checked"] != null && Convert.ToString(input.Attributes["checked"].Value) == "checked") ? true : false,
                    DisplayName = label.InnerText
                });
                //}
            }

            return result;
        }

        private IList<OptionContract> AddEngineTransmissionDriveTrainAsOptions(IList<OptionContract> list, int engineId, int transmissionId, int driveTrainId)
        {
            var result = list;
            result.Add(new OptionContract()
            {
                __type = "KBB.Karpower.WebServices.LightOption",
                Id = engineId.ToString(),
                IsSelected = true,
                DisplayName = string.Empty
            });

            result.Add(new OptionContract()
            {
                __type = "KBB.Karpower.WebServices.LightOption",
                Id = transmissionId.ToString(),
                IsSelected = true,
                DisplayName = string.Empty
            });

            result.Add(new OptionContract()
            {
                __type = "KBB.Karpower.WebServices.LightOption",
                Id = driveTrainId.ToString(),
                IsSelected = true,
                DisplayName = string.Empty
            });

            return result;
        }

        private void CheckMultipleEngines(KarPowerViewModel model, KarPowerService karPowerService, string vin, DateTime valuationDate)
        {
            if (model.SelectedEngineId == 0)
            {
                var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                if (firstEngine != null)
                {
                    firstEngine.Selected = true;
                    model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                }

                karPowerService.ExecuteGetPartiallyDecodedTransmissionsWithUser(vin, valuationDate, model.SelectedTrimId, model.SelectedEngineId);
                if (!String.IsNullOrEmpty(karPowerService.GetPartiallyDecodedTransmissionsWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetPartiallyDecodedTransmissionsWithUserResult);
                    model.SelectedTransmissionId = ConvertToInt32((JValue)(jsonObj["d"]["transmissionId"]));
                    model.SelectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));

                    model.Transmissions = CreateDataList(jsonObj["d"]["transmissions"], model.SelectedTransmissionId);
                    model.DriveTrains = CreateDataList(jsonObj["d"]["drivetrains"], model.SelectedDriveTrainId);
                    model.OptionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));
                }
            }
        }

        private void CheckMultipleTranmissions(KarPowerViewModel model, KarPowerService karPowerService, string vin, DateTime valuationDate)
        {
            if (model.SelectedTransmissionId == 0)
            {
                try
                {
                    var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                    if (firstTransmission != null)
                    {
                        firstTransmission.Selected = true;
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }

                    karPowerService.ExecuteGetPartiallyDecodedTrainsWithUser(vin, valuationDate, model.SelectedTrimId, model.SelectedEngineId, model.SelectedTransmissionId);
                    if (!String.IsNullOrEmpty(karPowerService.GetPartiallyDecodedTrainsWithUserResult))
                    {
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetPartiallyDecodedTrainsWithUserResult);
                        model.SelectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));
                        model.OptionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                        // get drive trains
                        model.DriveTrains = CreateDataList(jsonObj["d"]["drivetrains"], model.SelectedDriveTrainId);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void CheckMultipleDriveTrains(KarPowerViewModel model, KarPowerService karPowerService, string vin, DateTime valuationDate)
        {
            if (model.SelectedDriveTrainId == 0)
            {
                try
                {
                    karPowerService.ExecuteGetPartiallyDecodedTrainsWithUser(vin, valuationDate, model.SelectedTrimId, model.SelectedEngineId, model.SelectedTransmissionId);
                    if (!String.IsNullOrEmpty(karPowerService.GetPartiallyDecodedTrainsWithUserResult))
                    {
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetPartiallyDecodedTrainsWithUserResult);
                        model.SelectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));
                        
                        // get drive trains
                        model.DriveTrains = CreateDataList(jsonObj["d"]["drivetrains"], model.SelectedDriveTrainId);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //[HttpPost]
        public ActionResult UpdateValuationByOptionalEquipment(string listingId, string isChecked)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.IsMultipleTrims = true;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResult", model);
        }

        public ActionResult UpdateValuationByOptionalEquipmentForBucketJump(string listingId, string isChecked)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.IsMultipleTrims = true;
            var selectedOptions = model.OptionalEquipmentMarkupList.Where(i => i.IsSelected).Select(i => i.DisplayName).ToList();
            Session["StoreKarPowerOptions"] = selectedOptions.Aggregate<string>((first, second) => first + "," + second);

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResultForBucketJump", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingTrim(int trimId)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(trimId);
            model.IsMultipleTrims = true;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingTrimAndTransmission(int trimId, int transmissionId)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(Convert.ToInt32(trimId), Convert.ToInt32(transmissionId));
            model.IsMultipleTrims = false;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResult", model);
        }
        
        [HttpPost]
        public ActionResult PrintReport(KarPowerViewModel submittedModel)
        {
            var model = new KarPowerViewModel();
            if (Session["KarPowerViewModel"] != null)
                model = (KarPowerViewModel)Session["KarPowerViewModel"];
            try
            {
                var dealer = (DealershipViewModel)Session["Dealership"];
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"],
                    UserName = dealer.KellyBlueBook,
                    Password = dealer.KellyPassword
                };

                var selectedMake = model.Makes.FirstOrDefault(i => i.Value == model.SelectedMakeId.ToString());
                var selectedModel = model.Models.FirstOrDefault(i => i.Value == model.SelectedModelId.ToString());
                var selectedTrim = model.Trims.FirstOrDefault(i => i.Value == model.SelectedTrimId.ToString());
                var selectedEngine = model.Engines.FirstOrDefault(i => i.Value == submittedModel.SelectedEngineId.ToString());
                var selectedTransmission = model.Transmissions.FirstOrDefault(i => i.Value == submittedModel.SelectedTransmissionId.ToString());
                var selectedDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value == submittedModel.SelectedDriveTrainId.ToString());
                var selectedReport = model.Reports.FirstOrDefault(i => i.Value == submittedModel.SelectedReport);
                var dataToSave = new SaveVrsVehicleContract()
                {
                    category = "ca090fcb-597d-482d-b5aa-f528dd7bba21", // Appraisal
                    certified = false,
                    drivetrain = selectedDriveTrain.Text,
                    drivetrainId = selectedDriveTrain.Value,
                    engine = selectedEngine.Text,
                    engineId = selectedEngine.Value,
                    exteriorColor = "Select Color or Enter Manually",
                    interiorColor = "Select Color or Enter Manually",
                    initialDate = model.ValuationDate.ToString("M/d/yyyy"),
                    inventoryEntryId = "",
                    isPreOwnedSessionVehicle = true,
                    make = selectedMake.Text,
                    makeId = selectedMake.Value,
                    mileage = model.SelectedMileage.ToString(),
                    model = selectedModel.Text,
                    modelId = selectedModel.Value,
                    optionHistory = model.OptionalEquipmentHistoryList.ToArray(),
                    options = model.OptionalEquipmentMarkupList.ToArray(),
                    sellPrice = "",
                    stockNumber = "",
                    transmission = selectedTransmission.Text,
                    transmissionId = selectedTransmission.Value,
                    trim = selectedTrim.Text,
                    trimId = selectedTrim.Value,
                    valuationDate = model.ValuationDate.ToString("M/d/yyyy"),
                    vehicleCondition = "2",
                    vin = model.Vin,
                    webSellPrice = "",
                    year = model.SelectedYearId.ToString(),
                    yearId = model.SelectedYearId.ToString()
                };

                karPowerServiceWrapper.ExecuteSaveVrsVehicle(dataToSave);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.SaveVrsVehicleResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.SaveVrsVehicleResult);
                    model.EncodedInventoryId = ConvertToString((JValue)(jsonObj["d"]["id"]));
                    model.EncodedVin = ConvertToString((JValue)(jsonObj["d"]["vin"]));
                    model.EncodedStockNumber = ConvertToString((JValue)(jsonObj["d"]["stockNumber"]));
                    model.EncodedInventoryCategoryId = ConvertToString((JValue)(jsonObj["d"]["inventoryCategoryId"]));
                    model.CategoryName = ConvertToString((JValue)(jsonObj["d"]["categoryName"]));
                }

                // Save trim & options on karpower
                try
                {
                    SaveKarPowerOptions(model.Vin, selectedTrim.Value, submittedModel.SelectedOptionIds, selectedEngine.Value, selectedTransmission.Value, selectedDriveTrain.Value, submittedModel.BaseWholesale, submittedModel.Wholesale, submittedModel.Type);
                }
                catch (Exception)
                {
                    
                }

                karPowerServiceWrapper.ExecuteGetReportParameters(model.EncodedInventoryId, selectedReport.Value, selectedReport.Text, "", model.Vin);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetReportParametersResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetReportParametersResult);
                    var hfReportParams = ConvertToString((JValue)(jsonObj["d"]));

                    //downloadTokenValue will have been provided in the form submit via the hidden input field
                    //var response = HttpContext.Response;
                    //response.Clear();
                    //response.AppendCookie(new HttpCookie("fileDownloadToken", model.DownloadTokenValueId)); 
                    
                    //response.Flush();

                    return File(karPowerServiceWrapper.ExecuteGetPreOwnedVehicleReport(hfReportParams), "application/pdf", selectedReport.Text + " " + DateTime.Now.ToString("MM/dd/yyy"));
                }
            }
            catch (Exception)
            {

            }

            return File(new byte[] { }, "application/pdf", "Error");
        }

        [HttpPost]
        public ActionResult KarPowerResult(string vin, string mileage)
        {
            var model = CreateViewModelForKarPowerResult(vin, mileage);
            model.IsMultipleTrims = true;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResult", model);
        }

        //[HttpPost]
        //public ActionResult KarPowerResultForBucketJump(string vin, string mileage)
        //{
        //    var model = CreateViewModelForKarPowerResultForBucketJump(vin, mileage);
        //    model.IsMultipleTrims = true;

        //    Session["KarPowerViewModel"] = model;
        //    return PartialView("KarPowerResultForBucketJump", model);
        //}

        [HttpPost]
        public ActionResult KarPowerResultForBucketJump(string vin, string mileage, string trimId, string type, bool hasVin)
        {
            var model = CreateViewModelForKarPowerResult(vin, mileage, trimId, type);
            //model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;
            model.HasVin = hasVin;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResultForBucketJump", model);
        }

        [HttpPost]
        public ActionResult KarPowerResultInSingleMode(string vin, string mileage, string trimId, string type, bool hasVin)
        {
            var model = CreateViewModelForKarPowerResult(vin, mileage, trimId, type);
            //model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;
            model.HasVin = hasVin;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public void SaveKarPowerOptions(string vin, string trimId, string selectedOptionIds, string engineId, string transmissionId, string driveTrainId, string baseWholesale, string wholesale, string type)
        {
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var convertedTrimId = Convert.ToInt32(trimId);
                var convertedEngineId = Convert.ToInt32(engineId);
                var convertedTransmissionId = Convert.ToInt32(transmissionId);
                var convertedDriveTrainId = Convert.ToInt32(driveTrainId);
                switch (type)
                {
                    case "Inventory":
                        var inventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == SessionHandler.Dealership.DealershipId);
                        if (inventory != null)
                        {
                            inventory.KBBTrimId = convertedTrimId;
                            inventory.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            inventory.KBBEngineId = convertedEngineId;
                            inventory.KBBTransmissionId = convertedTransmissionId;
                            inventory.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();
                        }
                        break;
                    case "SoldOut":
                        var soldoutInventory = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == SessionHandler.Dealership.DealershipId);
                        if (soldoutInventory != null)
                        {
                            soldoutInventory.KBBTrimId = convertedTrimId;
                            soldoutInventory.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            soldoutInventory.KBBEngineId = convertedEngineId;
                            soldoutInventory.KBBTransmissionId = convertedTransmissionId;
                            soldoutInventory.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();
                        }
                        break;
                    case "Wholesale":
                        var wholesaleInventory = context.vincontrolwholesaleinventories.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == SessionHandler.Dealership.DealershipId);
                        if (wholesaleInventory != null)
                        {
                            wholesaleInventory.KBBTrimId = convertedTrimId;
                            wholesaleInventory.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            wholesaleInventory.KBBEngineId = convertedEngineId;
                            wholesaleInventory.KBBTransmissionId = convertedTransmissionId;
                            wholesaleInventory.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();
                        }
                        break;
                    case "Appraisal":
                        var appraisal = context.whitmanenterpriseappraisals.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == SessionHandler.Dealership.DealershipId);
                        if (appraisal != null)
                        {
                            appraisal.KBBTrimId = convertedTrimId;
                            appraisal.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            appraisal.KBBEngineId = convertedEngineId;
                            appraisal.KBBTransmissionId = convertedTransmissionId;
                            appraisal.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();

                            if (String.IsNullOrEmpty(appraisal.KarPowerEntryId))
                            {
                                // save selected trim & selected options on karpower
                                var dealer = (DealershipViewModel)Session["Dealership"];
                                var karpowerService = new KarPowerService();
                                karpowerService.ExecuteSaveVehicleWithVin(vin, appraisal.KarPowerEntryId, convertedTrimId, selectedOptionIds, appraisal.Mileage, appraisal.ExteriorColor, appraisal.InteriorColor, dealer.KellyBlueBook, dealer.KellyPassword);
                                if (!String.IsNullOrEmpty(karpowerService.EntryId))
                                {
                                    appraisal.KarPowerEntryId = karpowerService.EntryId;
                                    context.SaveChanges();
                                }
                            }
                        }
                        break;
                    default: break;
                }

                if (!String.IsNullOrEmpty(baseWholesale))
                {
                    var existingKbb = context.whitmanenterprisekbbs.OrderByDescending(i => i.DateAdded).FirstOrDefault(i => i.Vin == vin && i.TrimId == convertedTrimId);
                    if (existingKbb != null)
                    {
                        existingKbb.BaseWholeSale = baseWholesale;
                        existingKbb.WholeSale = wholesale;
                        context.SaveChanges();
                    }
                }
            }
        }

        public ActionResult UpdateValuationByOptionalEquipmentInSingleMode(string listingId, string isChecked, string trimId)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;

            Session["KarPowerViewModel"] = model;
            return PartialView("KarPowerResult", model);
        }
        
        private KarPowerViewModel CreateViewModelForUpdateValuationByOptionalEquipment(string listingId, string isChecked)
        {
            var model = new KarPowerViewModel();
            if (Session["KarPowerViewModel"] != null)
                model = (KarPowerViewModel)Session["KarPowerViewModel"];

            try
            {
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"]
                };
                var changedOption = model.OptionalEquipmentMarkupList.FirstOrDefault(i => i.Id == listingId);
                if (changedOption != null)
                {
                    changedOption.IsSelected = Convert.ToBoolean(isChecked);
                    var existingOptionHistory = model.OptionalEquipmentHistoryList.FirstOrDefault(i => i.Id == changedOption.Id);
                    if (existingOptionHistory != null)
                        existingOptionHistory.IsSelected = Convert.ToBoolean(isChecked);
                    else
                        model.OptionalEquipmentHistoryList.Add(changedOption);
                }

                // get kar power values without options
                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, new OptionContract[] { }, new OptionContract[] { });

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesaleWithoutOptions = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), model.OptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }
            }
            catch (Exception)
            {

            }

            return model;
        }

        private KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId)
        {
            var model = new KarPowerViewModel();
            if (Session["KarPowerViewModel"] != null)
                model = (KarPowerViewModel)Session["KarPowerViewModel"];

            model.SelectedTrimId = trimId;
            model.SelectedEngineId = 0;
            model.SelectedTransmissionId = 0;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<SelectListItem>();

            try
            {
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"]
                };

                // get Engines list
                karPowerServiceWrapper.ExecuteGetEngines(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetEnginesResult);
                    model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }

                // get Transmissions list
                karPowerServiceWrapper.ExecuteGetTransmissions(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetTransmissionsResult);
                    model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
                }

                // get Drive Trains list
                karPowerServiceWrapper.ExecuteGetDriveTrains(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDriveTrainsResult);
                    model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new SelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }

            }
            catch (Exception)
            {

            }

            return model;
        }

        private KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId, int transmissionId)
        {
            var model = new KarPowerViewModel();
            if (Session["KarPowerViewModel"] != null)
                model = (KarPowerViewModel)Session["KarPowerViewModel"];

            model.SelectedTrimId = trimId;
            model.SelectedEngineId = 0;
            model.SelectedTransmissionId = transmissionId;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<SelectListItem>();

            try
            {
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"]
                };

                // get Engines list
                karPowerServiceWrapper.ExecuteGetEngines(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetEnginesResult);
                    model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }

                // get Transmissions list
                karPowerServiceWrapper.ExecuteGetTransmissions(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetTransmissionsResult);
                    model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
                }

                // get Drive Trains list
                karPowerServiceWrapper.ExecuteGetDriveTrains(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDriveTrainsResult);
                    model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(trimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new SelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }

            }
            catch (Exception)
            {

            }

            return model;
        }

        private KarPowerViewModel CreateViewModelForKarPowerResult(string vin, string mileage)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            var karPowerServiceWrapper = new KarPowerService();

            try
            {
                var dealer = (DealershipViewModel) Session["Dealership"];
                var kbbUserName = string.Empty;
                var kbbPassword = string.Empty;
                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealer.DealershipId);
                    if (setting != null)
                    {
                        kbbUserName = setting.KellyBlueBook;
                        kbbPassword = setting.KellyPassword;
                    }
                }

                if (String.IsNullOrEmpty(kbbUserName) || String.IsNullOrEmpty(kbbPassword))
                    return model;

                karPowerServiceWrapper.ExecuteGetVehicleByVin(vin, valuationDate, kbbUserName, kbbPassword);
                Session["CookieContainer"] = karPowerServiceWrapper.CookieContainer;
                Session["CookieCollection"] = karPowerServiceWrapper.CookieCollection;

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetVehicleByVinResult);

                    model.SelectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    model.SelectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    model.SelectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    model.SelectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    model.SelectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    model.SelectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    model.SelectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    model.ValuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    model.OptionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    model.Makes = CreateDataList(jsonObj["d"]["makes"], model.SelectedMakeId);

                    // get models
                    model.Models = CreateDataList(jsonObj["d"]["models"], model.SelectedModelId);

                    // get trims
                    model.Trims = CreateDataList(jsonObj["d"]["trims"], model.SelectedTrimId);

                    // get makes
                    model.Transmissions = CreateDataList(jsonObj["d"]["transmissions"], model.SelectedTransmissionId);

                    // get makes
                    model.Engines = CreateDataList(jsonObj["d"]["engines"], model.SelectedEngineId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // multiple trims
            if (model.SelectedTrimId == 0)
            {
                var firstTrim = model.Trims.FirstOrDefault(i => i.Value != "0");
                model.SelectedTrimId = Convert.ToInt32(firstTrim.Value);
            }

            // multiple engines
            CheckMultipleEngines(model, karPowerServiceWrapper, vin, valuationDate);

            // multiple transmissions
            CheckMultipleTranmissions(model, karPowerServiceWrapper, vin, valuationDate);

            CheckMultipleDriveTrains(model, karPowerServiceWrapper, vin, valuationDate);

            model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(model.OptionalEquipmentMarkup);

            try
            {
                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, valuationDate, Convert.ToInt32(mileage), 2,
                                                       0, new OptionContract[] { }, model.OptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }

                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, valuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new SelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        private KarPowerSavedDataViewModel GetKarPowerSavedData(string vin, string type, int dealershipId)
        {
            var viewModel = new KarPowerSavedDataViewModel();
            using (var context = new whitmanenterprisewarehouseEntities())
            {
                switch (type)
                {
                    case "Inventory":
                        var inventory = context.whitmanenterprisedealershipinventories.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == dealershipId);
                        if (inventory != null)
                        {
                            viewModel.SavedTrimId = inventory.KBBTrimId.GetValueOrDefault();
                            viewModel.SavedOptionIds = inventory.KBBOptionsId;
                            viewModel.SavedEngineId = inventory.KBBEngineId.GetValueOrDefault();
                            viewModel.SavedTransmissionId = inventory.KBBTransmissionId.GetValueOrDefault();
                            viewModel.SavedDriveTrainId = inventory.KBBDriveTrainId.GetValueOrDefault();
                            viewModel.SavedModel = inventory.Model;
                            viewModel.SavedTrim = inventory.Trim;
                        }
                        break;
                    case "SoldOut":
                        var soldout = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == dealershipId);
                        if (soldout != null)
                        {
                            viewModel.SavedTrimId = soldout.KBBTrimId.GetValueOrDefault();
                            viewModel.SavedOptionIds = soldout.KBBOptionsId;
                            viewModel.SavedEngineId = soldout.KBBEngineId.GetValueOrDefault();
                            viewModel.SavedTransmissionId = soldout.KBBTransmissionId.GetValueOrDefault();
                            viewModel.SavedDriveTrainId = soldout.KBBDriveTrainId.GetValueOrDefault();
                            viewModel.SavedModel = soldout.Model;
                            viewModel.SavedTrim = soldout.Trim;
                        }
                        break;
                    case "Wholesale":
                        var wholesale = context.vincontrolwholesaleinventories.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == dealershipId);
                        if (wholesale != null)
                        {
                            viewModel.SavedTrimId = wholesale.KBBTrimId.GetValueOrDefault();
                            viewModel.SavedOptionIds = wholesale.KBBOptionsId;
                            viewModel.SavedEngineId = wholesale.KBBEngineId.GetValueOrDefault();
                            viewModel.SavedTransmissionId = wholesale.KBBTransmissionId.GetValueOrDefault();
                            viewModel.SavedDriveTrainId = wholesale.KBBDriveTrainId.GetValueOrDefault();
                            viewModel.SavedModel = wholesale.Model;
                            viewModel.SavedTrim = wholesale.Trim;
                        }
                        break;
                    case "Appraisal":
                        var appraisal = context.whitmanenterpriseappraisals.FirstOrDefault(i => i.VINNumber == vin && i.DealershipId == dealershipId);
                        if (appraisal != null)
                        {
                            viewModel.SavedTrimId = appraisal.KBBTrimId.GetValueOrDefault();
                            viewModel.SavedOptionIds = appraisal.KBBOptionsId;
                            viewModel.SavedEngineId = appraisal.KBBEngineId.GetValueOrDefault();
                            viewModel.SavedTransmissionId = appraisal.KBBTransmissionId.GetValueOrDefault();
                            viewModel.SavedDriveTrainId = appraisal.KBBDriveTrainId.GetValueOrDefault();
                            viewModel.SavedModel = appraisal.Model;
                            viewModel.SavedTrim = appraisal.Trim;
                        }
                        break;
                    default: break;

                }
            }

            return viewModel;
        }

        private KarPowerViewModel CreateViewModelForKarPowerResultWithMultipleModel(string vin, string mileage, string trimId, string type, KarPowerSavedDataViewModel karpowerSavedData, KarPowerService karPowerServiceWrapper)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            try
            {
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetVehicleByVinResult);
                    model.SelectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    model.SelectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    // get models
                    model.Models = CreateDataList(jsonObj["d"]["models"], model.SelectedModelId);

                    // matching model
                    var matchingModel =
                        model.Models.Any(i => i.Text.ToLower().Contains(karpowerSavedData.SavedModel.ToLower()))
                            ? model.Models.First(i => i.Text.ToLower().Contains(karpowerSavedData.SavedModel.ToLower()))
                            : model.Models.FirstOrDefault();

                    karPowerServiceWrapper.GetTrims(model.SelectedYearId, Convert.ToInt32(matchingModel.Value), valuationDate);
                    model.Trims = CreateDataList(jsonObj["d"], model.SelectedTrimId);

                    // matching model
                    var matchingTrim =
                        model.Trims.Any(i => i.Text.ToLower().Contains(karpowerSavedData.SavedTrim.ToLower()))
                            ? model.Trims.First(i => i.Text.ToLower().Contains(karpowerSavedData.SavedTrim.ToLower()))
                            : model.Trims.FirstOrDefault();
                    model.SelectedTrimId = Convert.ToInt32(matchingTrim.Value);

                }
            }
            catch (Exception)
            {
                
            }

            model.SelectedEngineId = karpowerSavedData.SavedTrimId == model.SelectedTrimId ? karpowerSavedData.SavedEngineId : 0;
            model.SelectedTransmissionId = karpowerSavedData.SavedTrimId == model.SelectedTrimId ? karpowerSavedData.SavedTransmissionId : 0;
            model.SelectedDriveTrainId = karpowerSavedData.SavedTrimId == model.SelectedTrimId ? karpowerSavedData.SavedDriveTrainId : 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<SelectListItem>();

            try
            {
                // get Engines list
                if (!model.Engines.Any())
                {
                    karPowerServiceWrapper.ExecuteGetEngines(model.SelectedTrimId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetEnginesResult))
                    {
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetEnginesResult);
                        model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                        if (model.SelectedEngineId == 0)
                        {
                            var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                            model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                        }
                    }
                }

                // get Transmissions list
                if (!model.Transmissions.Any())
                {
                    karPowerServiceWrapper.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetTransmissionsResult))
                    {
                        var jsonObj =
                            (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetTransmissionsResult);
                        model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                        if (model.SelectedTransmissionId == 0)
                        {
                            var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                            model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                        }
                    }
                }

                // get Drive Trains list
                if (!model.DriveTrains.Any())
                {
                    karPowerServiceWrapper.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDriveTrainsResult))
                    {
                        var jsonObj =
                            (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDriveTrainsResult);
                        model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                        if (model.SelectedDriveTrainId == 0)
                        {
                            var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                            model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                        }
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                // loading save options in database
                if (model.SelectedTrimId == karpowerSavedData.SavedTrimId)
                {
                    foreach (var optionContract in model.OptionalEquipmentMarkupList)
                    {
                        optionContract.IsSelected = karpowerSavedData.SavedOptionIds.Contains(optionContract.Id);
                    }
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new SelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }
            }
            catch (Exception)
            {

            }

            return model;
        }

        private KarPowerViewModel CreateViewModelForKarPowerResult(string vin, string mileage, string trimId, string type)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            var karPowerServiceWrapper = new KarPowerService();
            var dealer = (DealershipViewModel)Session["Dealership"];
            var karpowerSavedData = GetKarPowerSavedData(vin, type, dealer.DealershipId);

            try
            {
                var kbbUserName = string.Empty;
                var kbbPassword = string.Empty;

                using (var context = new whitmanenterprisewarehouseEntities())
                {
                    var setting = context.whitmanenterprisesettings.FirstOrDefault(i => i.DealershipId == dealer.DealershipId);
                    if (setting != null)
                    {
                        kbbUserName = setting.KellyBlueBook;
                        kbbPassword = setting.KellyPassword;
                    }
                }

                if (String.IsNullOrEmpty(kbbUserName) || String.IsNullOrEmpty(kbbPassword) || String.IsNullOrEmpty(vin))
                    return model;

                karPowerServiceWrapper.ExecuteGetVehicleByVin(vin, valuationDate, kbbUserName, kbbPassword);
                Session["CookieContainer"] = karPowerServiceWrapper.CookieContainer;
                Session["CookieCollection"] = karPowerServiceWrapper.CookieCollection;

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetVehicleByVinResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetVehicleByVinResult);
                    
                    model.SelectedYearId = ConvertToInt32((JValue)(jsonObj["d"]["yearId"]));
                    model.SelectedMakeId = ConvertToInt32(((JValue)(jsonObj["d"]["makeId"])));
                    model.SelectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    if (model.SelectedModelId == 0)
                        return CreateViewModelForKarPowerResultWithMultipleModel(vin, mileage, trimId, type,
                                                                                 karpowerSavedData,
                                                                                 karPowerServiceWrapper);

                    model.SelectedTrimId = ConvertToInt32(((JValue)(jsonObj["d"]["trimId"])));
                    model.SelectedEngineId = ConvertToInt32(((JValue)(jsonObj["d"]["engineId"])));
                    model.SelectedTransmissionId = ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"])));
                    model.SelectedDriveTrainId = ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"])));
                    model.ValuationDate = ConvertToDateTime(((JValue)(jsonObj["d"]["valuationDate"])));
                    model.OptionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    // get makes
                    model.Makes = CreateDataList(jsonObj["d"]["makes"], model.SelectedMakeId);

                    // get models
                    model.Models = CreateDataList(jsonObj["d"]["models"], model.SelectedModelId);

                    // get trims
                    model.Trims = CreateDataList(jsonObj["d"]["trims"], model.SelectedTrimId);

                    // get transmissions
                    model.Transmissions = CreateDataList(jsonObj["d"]["transmissions"], model.SelectedTransmissionId);

                    // get engines
                    model.Engines = CreateDataList(jsonObj["d"]["engines"], model.SelectedEngineId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            model.SelectedTrimId = Convert.ToInt32(trimId);
            model.SelectedEngineId = karpowerSavedData.SavedTrimId == model.SelectedTrimId ? karpowerSavedData.SavedEngineId : 0;
            model.SelectedTransmissionId = karpowerSavedData.SavedTrimId == model.SelectedTrimId ? karpowerSavedData.SavedTransmissionId : 0;
            model.SelectedDriveTrainId = karpowerSavedData.SavedTrimId == model.SelectedTrimId ? karpowerSavedData.SavedDriveTrainId : 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<SelectListItem>();

            try
            {
                // get Engines list
                if (!model.Engines.Any())
                {
                    karPowerServiceWrapper.ExecuteGetEngines(model.SelectedTrimId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetEnginesResult))
                    {
                        var jsonObj = (JObject) JsonConvert.DeserializeObject(karPowerServiceWrapper.GetEnginesResult);
                        model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                        if (model.SelectedEngineId == 0)
                        {
                            var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                            model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                        }
                    }
                }

                // get Transmissions list
                if (!model.Transmissions.Any())
                {
                    karPowerServiceWrapper.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetTransmissionsResult))
                    {
                        var jsonObj =
                            (JObject) JsonConvert.DeserializeObject(karPowerServiceWrapper.GetTransmissionsResult);
                        model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                        if (model.SelectedTransmissionId == 0)
                        {
                            var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                            model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                        }
                    }
                }

                // get Drive Trains list
                if (!model.DriveTrains.Any())
                {
                    karPowerServiceWrapper.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDriveTrainsResult))
                    {
                        var jsonObj =
                            (JObject) JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDriveTrainsResult);
                        model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                        if (model.SelectedDriveTrainId == 0)
                        {
                            var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                            model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                        }
                    }
                }

                IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                // loading save options in database
                if (model.SelectedTrimId == karpowerSavedData.SavedTrimId)
                {
                    foreach (var optionContract in model.OptionalEquipmentMarkupList)
                    {
                        optionContract.IsSelected = karpowerSavedData.SavedOptionIds.Contains(optionContract.Id);
                    }
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value);
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value);
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new SelectListItem()
                    {
                        Text = item.Value<string>("ReportTitle"),
                        Value = item.Value<string>("Id"),
                        Selected = false
                    }));
                }
            }
            catch (Exception)
            {
                
            }

            return model;
        }

        public ActionResult GetKarPowerSummary(string listingId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int receivedListingId = Convert.ToInt32(listingId);
                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();
                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i => i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(modelWord)));
                            if (query.Any())
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                        }    
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;
                }
            }

            return View("KarPowerSummary");
        }

        public ActionResult GetKarPowerSummaryForBuckerJump(string listingId, string dealer, string price, string year, string make, string model, string color, string miles)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var hasVin = true;
                int receivedListingId = Convert.ToInt32(listingId);
                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();
                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i => i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(
                                                         modelWord)));
                            if (query.Any())
                            {
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                                hasVin = false;
                            }
                        }
                    }

                    if(row.KBBTrimId.GetValueOrDefault() > 0)
                        ViewData["TRIMID"] = row.KBBTrimId;
                    else
                    {
                        var findKBB = context.whitmanenterprisekbbs.FirstOrDefault(x => x.Vin == row.VINNumber);

                        if (findKBB != null)
                        {
                            ViewData["TRIMID"] = findKBB.TrimId;

                        }
                        else
                        {
                            ViewData["TRIMID"] = 0;
                        }
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = CommonHelper.RemoveSpecialCharacters(miles);
                    ViewData["DEALER"] = dealer;
                    ViewData["PRICE"] = price;
                    ViewData["YEAR"] = year;
                    ViewData["MAKE"] = make;
                    ViewData["MODEL"] = model;
                    ViewData["COLOR"] = color;
                    ViewData["LISTINGID"] = listingId;
                    ViewData["TYPE"] = "Inventory";
                    ViewData["HASVIN"] = hasVin;
                }

            }

            return View("KarPowerSummaryForBucketJump");
        }

        public ActionResult GetKarPowerSummaryForBuckerJumpInAppraisal(string listingId, string dealer, string price, string year, string make, string model, string color, string miles)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var hasVin = true;
                int receivedListingId = Convert.ToInt32(listingId);
                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            var sampleCar = DataHelper.GetNationwideMarketData(query, row.Make, row.Model, row.Trim).FirstOrDefault(x => !String.IsNullOrEmpty(x.Vin));
                            if (sampleCar != null)
                            {
                                row.VINNumber = sampleCar.Vin;
                                hasVin = false;
                            }
                        }
                    }

                    if (row.KBBTrimId.GetValueOrDefault() > 0)
                        ViewData["TRIMID"] = row.KBBTrimId;
                    else
                    {
                        var findKBB = context.whitmanenterprisekbbs.FirstOrDefault(x => x.Vin == row.VINNumber);

                        if (findKBB != null)
                        {
                            ViewData["TRIMID"] = findKBB.TrimId;

                        }
                        else
                        {
                            ViewData["TRIMID"] = 0;
                        }
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = CommonHelper.RemoveSpecialCharacters(miles);
                    ViewData["DEALER"] = dealer;
                    ViewData["PRICE"] = price;
                    ViewData["YEAR"] = year;
                    ViewData["MAKE"] = make;
                    ViewData["MODEL"] = model;
                    ViewData["COLOR"] = color;
                    ViewData["LISTINGID"] = listingId;
                    ViewData["TYPE"] = "Appraisal";
                    ViewData["HASVIN"] = hasVin;
                }

            }

            return View("KarPowerSummaryForBucketJump");
        }

        public ActionResult GetSingleKarPowerSummary(int listingId, string trimId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var hasVin = true;
                int receivedListingId = listingId;
                var row = context.whitmanenterprisedealershipinventories.FirstOrDefault(x => x.ListingID == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();
                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i =>  i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(
                                                         modelWord)));
                            if (query.Any())
                            {
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                                hasVin = false;
                            }
                        }
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;
                    ViewData["TRIMID"] = trimId;
                    ViewData["TYPE"] = "Inventory";
                    ViewData["HASVIN"] = hasVin;
                }
                
            }

            return View("SingleKarPowerSummary");
        }

        public ActionResult GetSingleKarPowerSummaryForSoldCars(string listingId, string trimId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var hasVin = true;
                int receivedListingId = Convert.ToInt32(listingId);
                var row = context.whitmanenterprisedealershipinventorysoldouts.FirstOrDefault(x => x.ListingID == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();

                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i =>i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(
                                                         modelWord)));
                            if (query.Any())
                            {
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                                hasVin = false;
                            }
                        }
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;
                    ViewData["TRIMID"] = trimId;
                    ViewData["TYPE"] = "SoldOut";
                    ViewData["HASVIN"] = hasVin;
                }

            }

            return View("SingleKarPowerSummary");
        }

        public ActionResult GetSingleKarPowerSummaryForWholesale(string listingId, string trimId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var hasVin = true;
                int receivedListingId = Convert.ToInt32(listingId);
                var row = context.vincontrolwholesaleinventories.FirstOrDefault(x => x.ListingID == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();
                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i => i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(
                                                         modelWord)));
                            if (query.Any())
                            {
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                                hasVin = false;
                            }
                        }
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;
                    ViewData["TRIMID"] = trimId;
                    ViewData["TYPE"] = "Wholesale";
                    ViewData["HASVIN"] = hasVin;
                }

            }

            return View("SingleKarPowerSummary");
        }

        public ActionResult GetKarPowerSummaryForAppraisal(string appraisalId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                int receivedListingId = Convert.ToInt32(appraisalId);
                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();
                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i => i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(modelWord)));
                            if (query.Any())
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                        }     
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;
                }
            }

            return View("KarPowerSummary");
        }

        public ActionResult GetSingleKarPowerSummaryForAppraisal(string appraisalId, string trimId)
        {
            if (Session["Dealership"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new whitmanenterprisewarehouseEntities())
            {
                var hasVin = true;
                int receivedListingId = Convert.ToInt32(appraisalId);
                var row = context.whitmanenterpriseappraisals.FirstOrDefault(x => x.idAppraisal == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.VINNumber))
                    {
                        var modelWord = row.Model.Trim();

                        modelWord = DataHelper.FilterCarModelForMarket(modelWord);
                        using (var scrappingContext = new vincontrolscrappingEntities())
                        {
                            var query = MapperFactory.GetCarsComMarketCarQuery(scrappingContext, row.ModelYear);

                            query = query.Where(i =>  i.Year == row.ModelYear &&
                                                     i.Make == row.Make &&
                                                     ((i.Model.Replace(" ", "") + i.Trim.Replace(" ", "")).Contains(modelWord)));
                            if (query.Any())
                            {
                                row.VINNumber = query.FirstOrDefault(i => !String.IsNullOrEmpty(i.Vin)).Vin;
                                hasVin = false;
                            }
                        }    
                    }

                    ViewData["VIN"] = row.VINNumber;
                    ViewData["MILEAGE"] = String.IsNullOrEmpty(row.Mileage) ? "0" : row.Mileage;
                    ViewData["TRIMID"] = trimId;
                    ViewData["TYPE"] = "Appraisal";
                    ViewData["HASVIN"] = hasVin;
                }
            }

            return View("SingleKarPowerSummary");
        }

        #endregion
    }
}
