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
using vincontrol.Application.Forms.AppraisalManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;
using BlackBookViewModel = Vincontrol.Web.Models.BlackBookViewModel;
using CommonHelper = vincontrol.Helper.CommonHelper;
using DataHelper = Vincontrol.Web.HelperClass.DataHelper;
using SelectListItem = vincontrol.DomainObject.ExtendedSelectListItem;
using KarPowerService = vincontrol.KBB.KBBService;
using vincontrol.Application.Forms.InventoryManagement;
using ReportType = vincontrol.Constant.ReportType;

namespace Vincontrol.Web.Controllers
{
    public class MarketController : SecurityController
    {
        private const string PermissionCode = "KPI";
        private const string AcceptedValues = "ALLACCESS";
        private IInventoryManagementForm _inventoryManagementForm;
        private IAppraisalManagementForm _appraisalManagementForm;

        public MarketController()
        {
            _inventoryManagementForm = new InventoryManagementForm();
            _appraisalManagementForm=new AppraisalManagementForm();
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewKpiReport(ReportType reportType)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            switch (reportType)
            {
                case ReportType.Pdf:
                    return RedirectToAction("PrintExcelCarInfo", "PDF");
                
                case ReportType.Excel:
                    var viewModel = SessionHandler.KpiConditionInventoryList ?? SessionHandler.KpiInventoryList.CarsList;
                    var dealer = SessionHandler.Dealer;
                 
                    return File(ReportHelper.ExportToCSV(viewModel, dealer.DealershipId, "KPI Report"), "application/vnd.ms-excel", "KPIReport.xlsx");
                default:
                    viewModel = SessionHandler.KpiConditionInventoryList ?? SessionHandler.KpiInventoryList.CarsList;
                    dealer = SessionHandler.Dealer;
                 
                    return File(ReportHelper.ExportToCSV(viewModel, dealer.DealershipId, "KPI Report"), "application/vnd.ms-excel", "KPIReport.xlsx");
            }
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewKpi()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
        
            // Reset KPI Condition
            SessionHandler.KpiConditon = 0;

            IQueryable<Inventory> inventoryList = null;

            if (SessionHandler.AllStore && SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
            {

                inventoryList = _inventoryManagementForm.GetAllUsedInventories(
                    SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId))
                    .Where(x => x.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory);

            }
            else
            {
                inventoryList = _inventoryManagementForm.GetAllUsedInventories(SessionHandler.Dealer.DealershipId)
                    .Where(x => x.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory);
            }




            var viewModel = new InventoryFormViewModel {IsCompactView = false};

            var list = new List<CarInfoFormViewModel>();

            foreach (var drTmp in inventoryList)
            {
                var car = new CarInfoFormViewModel(drTmp) {IsUsed = true};

                list.Add(car);
            }

            SessionHandler.KPIViewInfo = new ViewInfo { SortFieldName = "make", IsUp = true };
            viewModel.CarsList = DataHelper.SortList(list, SessionHandler.KPIViewInfo.SortFieldName, SessionHandler.KPIViewInfo.IsUp);

            viewModel.SortSetList = SelectListHelper.InitalSortSetList();

            SessionHandler.KpiInventoryList = viewModel;

            return View("ViewKPI", viewModel);
        }

        public ActionResult GetKpiList()
        {
            return PartialView("KPIListData", SessionHandler.KpiInventoryList );
        }

        public ActionResult GetKpiListJson()
        {
            SessionHandler.KpiInventoryList.CarsList = DataHelper.SortList(SessionHandler.KpiInventoryList.CarsList, SessionHandler.KPIViewInfo.SortFieldName, SessionHandler.KPIViewInfo.IsUp);

            if (SessionHandler.KpiConditon > 0)
                return ViewConditionKpiJson(SessionHandler.KpiConditon);

            return new LargeJsonResult
            {
                Data = SessionHandler.KpiInventoryList.CarsList.Select(tmp => new InventoryInfo(tmp)).ToList()
            };
        }

        [VinControlAuthorization(PermissionCode = PermissionCode, AcceptedValues = AcceptedValues)]
        public ActionResult ViewNewKpi()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            // Reset KPI Condition
            SessionHandler.KpiConditon = 0;


            IQueryable<Inventory> inventoryList = null;

            if (!SessionHandler.Single && SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
            {

                inventoryList = _inventoryManagementForm.GetAllNewInventories(
                    SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId))
                    .Where(x => x.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory);

            }
            else
            {
                inventoryList = _inventoryManagementForm.GetAllNewInventories(SessionHandler.Dealer.DealershipId)
                    .Where(x => x.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory);
            }


          
            var viewModel = new InventoryFormViewModel {IsCompactView = false};

            var list = new List<CarInfoFormViewModel>();


            foreach (var drTmp in inventoryList)
            {
                var car = new CarInfoFormViewModel(drTmp) { IsUsed = true };

                list.Add(car);
            }
            viewModel.CarsList = list;

            viewModel.SortSetList = SelectListHelper.InitalSortSetList();

            SessionHandler.KpiInventoryList = viewModel;

            return View("ViewNewKPI", viewModel);
        }

        public ActionResult ViewConditionKpiJson(int condition)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            
            if (SessionHandler.KpiInventoryList == null) return ViewKpi();

            SessionHandler.KpiConditon = condition;
            switch (condition)
            {
                case Constanst.KpiCondition.MissingPics:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => !x.HasImage && x.Loaner == false).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.MissingDescription:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => !x.HasDescription).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.MissingPrice:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => !x.HasSalePrice).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.AboveMarket:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.MarketRange == 3).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.AverageMarket:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.MarketRange == 2).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.BelowMarket:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.MarketRange == 1).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From0To15:
                case Constanst.KpiCondition.From0To15Perecent:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.DaysInInvenotry >= 0 && x.DaysInInvenotry <= 15).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From16To30:
                case Constanst.KpiCondition.From16To30Perecent:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From31To60:
                case Constanst.KpiCondition.From31To60Perecent:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From61To90:
                case Constanst.KpiCondition.From61To90Perecent:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.Above90:
                case Constanst.KpiCondition.Above90Perecent:
                    SessionHandler.KpiConditionInventoryList = SessionHandler.KpiInventoryList.CarsList.Where(x => x.DaysInInvenotry > 90).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };

                case Constanst.KpiCondition.From0To15Above:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry >= 0 && x.DaysInInvenotry <= 15 && x.MarketRange == 3).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From0To15At:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry >= 0 && x.DaysInInvenotry <= 15 && x.MarketRange == 2).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From0To15Below:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry >= 0 && x.DaysInInvenotry <= 15 && x.MarketRange == 1).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From0To15Other:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x =>
                            x.DaysInInvenotry >= 0 && x.DaysInInvenotry <= 15 &&
                            (x.MarketRange == 0 || x.MarketRange == 4)).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };

                case Constanst.KpiCondition.From16To30Above:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30 && x.MarketRange == 3).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From16To30At:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30 && x.MarketRange == 2).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From16To30Below:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30 && x.MarketRange == 1).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From16To30Other:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 15 && x.DaysInInvenotry <= 30 &&
                            (x.MarketRange == 0 || x.MarketRange == 4)).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };

                case Constanst.KpiCondition.From31To60Above:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60 && x.MarketRange == 3).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From31To60At:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60 && x.MarketRange == 2).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From31To60Below:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60 && x.MarketRange == 1).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From31To60Other:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 30 && x.DaysInInvenotry <= 60 &&
                            (x.MarketRange == 0 || x.MarketRange == 4)).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };

                case Constanst.KpiCondition.From61To90Above:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90 && x.MarketRange == 3).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From61To90At:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90 && x.MarketRange == 2).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From61To90Below:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90 && x.MarketRange == 1).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.From61To90Other:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 60 && x.DaysInInvenotry <= 90 &&
                            (x.MarketRange == 0 || x.MarketRange == 4)).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };

                case Constanst.KpiCondition.Over90Above:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 90 && x.MarketRange == 3).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.Over90At:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 90 && x.MarketRange == 2).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.Over90Below:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 90 && x.MarketRange == 1).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                case Constanst.KpiCondition.Over90Other:
                    SessionHandler.KpiConditionInventoryList =
                        SessionHandler.KpiInventoryList.CarsList.Where(
                            x => x.DaysInInvenotry > 90 &&
                            (x.MarketRange == 0 || x.MarketRange == 4)).ToList();
                    return new LargeJsonResult { Data = SessionHandler.KpiConditionInventoryList };
                default:
                    return ViewKpi();
            }
        }

        public ActionResult UpdateKPIViewInfoStatus(ViewInfo viewInfo)
        {
            SessionHandler.KPIViewInfo = viewInfo;
            return Json("success");
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewEbay(int listingId)
        {
            if (!SessionHandler.UserRight.Inventory.ViewProfile_Ebay)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var vehicle = ConvertHelper.GetVehicleInfoForEbay(listingId);

                var viewModel = new EbayFormViewModel()
                                    {
                                        AuctionType = SelectListHelper.InitalAuctionTypeList(),
                                        Gallerys = SelectListHelper.InitalGalleryList(),
                                        HoursToDeposit = SelectListHelper.InitalHoursList(),
                                        AuctionLength = SelectListHelper.InitalAuctionLength(),
                                      
                                        ListingId = listingId.ToString(CultureInfo.InvariantCulture),
                                        BuyItNowPrice = vehicle.SalePrice.ToString(),
                                        HitCounter = true,
                                        Dealer = dealer,
                                        VehicleInfo = vehicle

                                    };
                SessionHandler.EbayAds = viewModel;

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

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewEbayWithoutPopUp(int listingId, short inventoryStatus)
        {
            if (!SessionHandler.UserRight.Inventory.ViewProfile_Ebay)
            {
                return RedirectToAction("Unauthorized", "Security");
            }

            if (SessionHandler.Dealer != null)
            {
             
                var row = _inventoryManagementForm.GetInventory(listingId);

                var vehicle = new CarInfoFormViewModel(row);
              
                var viewModel = new EbayFormViewModel()
                                    {
                                        AuctionType = SelectListHelper.InitalAuctionTypeList(),
                                        Gallerys = SelectListHelper.InitalGalleryList(),
                                        HoursToDeposit = SelectListHelper.InitalHoursList(),
                                        AuctionLength = SelectListHelper.InitalAuctionLength(),
                                     
                                        ListingId = listingId.ToString(CultureInfo.InvariantCulture),
                                        BuyItNowPrice = vehicle.SalePrice.ToString(CultureInfo.InvariantCulture),
                                        HitCounter = true,
                                        VehicleInfo = vehicle,
                                        InventoryStatus = inventoryStatus

                                    };
                SessionHandler.EbayAds = viewModel;
                
                ViewData["detailType"] = "Ebay_Tab";
                return View("EbayFormWithoutPopUp", viewModel);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult GetEbayAdsPrice(EbayFormViewModel ebay)
        {
            var dealer = SessionHandler.Dealer;

            var postEbayList = EbayHelper.GetPostEbayList(dealer.DealershipId, ebay.ListingId);

            var currentSessionEbay = SessionHandler.EbayAds;

            ebay.VehicleInfo = currentSessionEbay.VehicleInfo;

            ebay.VehicleInfo.DealerAddress = dealer.DealershipAddress;

            ebay.Dealer = dealer;

            ebay.ListingId = currentSessionEbay.ListingId;

            ebay.PostEbayList = postEbayList;

            var call = new APICall();

            string error = "";

            var builder = new StringBuilder();

            if (ebay.Propackbundle)
            {
                builder.AppendLine("<ListingEnhancement>ProPackBundle</ListingEnhancement>");
            }
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

            var htmlSource = PDFRender.RenderViewAsString("NewEbayAdsView", ebay, ControllerContext);

            ebay.HtmlSource = htmlSource;
            
            string strReq = EbayHelper.BuildEbayItemToVerify(ebay,dealer);

            XmlDocument xmlDoc = call.MakeApiCall(strReq, "VerifyAddItem", error);

            //XmlDocument xmlDoc = new XmlDocument();

            if (String.IsNullOrEmpty(error))
            {
                XmlNode root = xmlDoc["VerifyAddItemResponse"];

                XmlNode successNode = root["Ack"];

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

            if (!string.IsNullOrEmpty(ebay.SellerProvidedTitle))
            {
                if (ebay.SellerProvidedTitle.Length > 80)
                    ebay.SellerProvidedTitle = ebay.SellerProvidedTitle.Substring(0, 79) + "...";
            }

            SessionHandler.EbayAds = ebay;

            if (Request.IsAjaxRequest())
            {
                return Json("Price Successful");
            }

            return Json("Not Successful");
        }

        public ActionResult PreviewEbayAds()
        {
            var ebay = SessionHandler.EbayAds;
            return View("NewPreviewEbayAds", ebay);
        }

        public ActionResult PostEbayAds()
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var ebay = SessionHandler.EbayAds;

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

                var strReq = EbayHelper.BuildEbayItem(ebay, dealer);

                string error = "";

                var ebayItem = new PostEbayAds();

                string ebayItemId = "";

                var xmlDoc = call.MakeApiCall(strReq, "AddItem", error);

                //var xmlDoc = new XmlDocument();

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

                            SQLHelper.InsertOrUpdateEbayAd(dealer.DealershipId, ebay.ListingId, ebayItem);
                        }

                    }


                }

                if (!String.IsNullOrEmpty(ebayItemId))

                    return Json(ebayItem.EbayAdUrl);

                
                return Json("Fail");

              

            }
            else
                return Json("TimeOut");

        }

        public PostEbayAds RetrieveEbayItem(string itemId, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();

            var ebayAd = new PostEbayAds()
            {
                EbayAdID = itemId
            };

            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            builder.AppendLine("<GetItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">");

            builder.AppendLine("<ItemID>" + itemId + "</ItemID>");

            builder.AppendLine("<RequesterCredentials>");

            //builder.AppendLine("<eBayAuthToken>" + ConfigurationManager.AppSettings["UserToken"] + "</eBayAuthToken>");

            builder.AppendLine("<eBayAuthToken>" + dealer.DealerSetting.EbayToken + "</eBayAuthToken>");

            builder.AppendLine("</RequesterCredentials>");


            builder.AppendLine("<Version>" + ConfigurationManager.AppSettings["Version"] + "</Version>");

            builder.AppendLine("<WarningLevel>High</WarningLevel>");

            builder.AppendLine("</GetItemRequest>");

            var call = new APICall();
            string error = "";
            
            XmlDocument xmlDoc = call.MakeApiCall(builder.ToString(), "GetItem", error);
            //string ebayItemUrl = "";

            if (String.IsNullOrEmpty(error))
            {
                XmlNode root = xmlDoc["GetItemResponse"];

                XmlNode successNode = root["Ack"];



                if (successNode != null && (successNode.InnerText.Equals("Success") || successNode.InnerText.Equals("Warning")))
                {
                    ebayAd.EbayAdUrl = root["Item"]["ListingDetails"]["ViewItemURL"].InnerText;

                    ebayAd.EbayAdStartTime = DateTime.Parse(root["Item"]["ListingDetails"]["StartTime"].InnerText);

                    ebayAd.EbayAdEndTime = DateTime.Parse(root["Item"]["ListingDetails"]["EndTime"].InnerText);
                }


            }

            return ebayAd;

        }

        public ActionResult WholeSale()
        {
            var dealer = SessionHandler.Dealer;

            return View("WholeSale");
        }

        public void OpenManaheimLoginWindow(string Vin)
        {
            var dealer = SessionHandler.Dealer;

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
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + dealer.DealerSetting.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + dealer.DealerSetting.ManheimPassword + "\" />" + Environment.NewLine;

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
            var dealer = SessionHandler.Dealer;

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
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + dealer.DealerSetting.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + dealer.DealerSetting.ManheimPassword + "\" />" + Environment.NewLine;

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
            var dealer = SessionHandler.Dealer;

            var encode = System.Text.Encoding.GetEncoding("utf-8");

            var context = new VincontrolEntities();

            var contextMarket = new VinMarketEntities();

            var sampleVin = "";

            var searchAppraisal = context.Appraisals.First(x => x.AppraisalId == appraisalId);

            if (String.IsNullOrEmpty(searchAppraisal.Vehicle.Vin))
            {

                var query = MapperFactory.GetMarketCarQuery(contextMarket, searchAppraisal.Vehicle.Year,false);

                var sampleCar = DataHelper.GetNationwideMarketData(query, searchAppraisal.Vehicle.Make, searchAppraisal.Vehicle.Model, searchAppraisal.Vehicle.Trim,false).First(x => !String.IsNullOrEmpty(x.Vin));

                if (sampleCar != null)

                    sampleVin = sampleCar.Vin;
            }
            
            var manheimLink = "";

            if (searchAppraisal != null)
            {
                if (!String.IsNullOrEmpty(searchAppraisal.Vehicle.Vin))
                {

                    manheimLink =
                        "https://www.manheim.com/members/internetmmr/?vin=" + searchAppraisal.Vehicle.Vin;
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
                    line = " <label>Username:</label><input class=\"textbox\" id=\"user_username\" name=\"user[username]\" size=\"30\" tabindex=\"1\" type=\"text\" value=\"" + dealer.DealerSetting.Manheim + "\" />" + Environment.NewLine;

                if (line.Contains("Password:"))

                    line = " <label>Password:</label><input class=\"textbox\" id=\"user_password\" name=\"user[password]\" size=\"30\" tabindex=\"2\" type=\"password\" value=\"" + dealer.DealerSetting.ManheimPassword + "\" />" + Environment.NewLine;

                if (line.Contains("</body>"))
                {
                    line = "<script type=\"text/javascript\"> jQuery(document).ready(function($){document.forms[1].submit(); });</script></body>" + Environment.NewLine;
                }

                result += line;

            }


            Response.Write(result);


        }

        public ActionResult ResetKbbTrim(int listingId)
        {
            _inventoryManagementForm.ResetKbbTrim(listingId, Constanst.VehicleStatus.Inventory);
            return RedirectToAction("ViewIProfile", "Inventory", new { ListingId = listingId });

        }

        public ActionResult ResetKbbApraisalTrim(int appraisalID)
        {
            _inventoryManagementForm.ResetKbbTrim(appraisalID, Constanst.VehicleStatus.Appraisal);

            return RedirectToAction("ViewProfileForAppraisal", "Appraisal", new { appraisalId = appraisalID });

        }

        public ActionResult ResetManheimTrim(int listingId)
        {
            _inventoryManagementForm.ResetManheimTrim(listingId, Constanst.VehicleStatus.Inventory);

            return RedirectToAction("ViewIProfile", "Inventory", new { ListingId = listingId });

        }

        public ActionResult ResetManheimApraisalTrim(int appraisalID)
        {
            _inventoryManagementForm.ResetManheimTrim(appraisalID, Constanst.VehicleStatus.Appraisal);
            
            return RedirectToAction("ViewProfileForAppraisal", "Appraisal", new { appraisalId = appraisalID });

        }
        
        public ActionResult GetBlackBookSummaryByVin(string Vin, long? Mileage)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                var viewModel = BlackBookService.GetDirectFullReport(Vin, Mileage??0, dealer.State);

                viewModel.Mileage = Mileage??0;

                return View("BlackBookSummary", viewModel);
            }
            else
            {
                var viewModel = new BlackBookViewModel();
                viewModel.SessionTimeOut = true;
                return View("BlackBookSummary", viewModel);
            }
        }

        public ActionResult GetBlackBookSummary(int listingId)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;


                var row = _inventoryManagementForm.GetInventory(listingId);

                BlackBookViewModel viewModel = BlackBookService.GetDirectFullReport(row.Vehicle.Vin, row.Mileage.GetValueOrDefault(), dealer.State);

                viewModel.Mileage = row.Mileage.GetValueOrDefault();

                return View("BlackBookSummary", viewModel);
            }
            else
            {
                var viewModel = new BlackBookViewModel {SessionTimeOut = true};
                return View("BlackBookSummary", viewModel);
            }
        }

        public ActionResult GetBlackBookSummaryAdjustMileage(BlackBookViewModel model)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

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

        private List<ExtendedSelectListItem> CreateDataList(JToken obj, int selectedId)
        {
            var result = new List<ExtendedSelectListItem>();
            if (obj != null)
            {
                result.AddRange(obj.Children().Where(item => !item.Value<string>("DisplayName").Equals("[Choose One]")).Select(item => new ExtendedSelectListItem()
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
                karPowerService.ExecuteGetPartiallyDecodedTransmissionsWithUser(vin, valuationDate, model.SelectedTrimId, model.SelectedEngineId);
                if (!String.IsNullOrEmpty(karPowerService.GetPartiallyDecodedTransmissionsWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetPartiallyDecodedTransmissionsWithUserResult);
                    model.SelectedTransmissionId = ConvertToInt32((JValue)(jsonObj["d"]["transmissionId"]));
                    model.SelectedDriveTrainId = ConvertToInt32((JValue)(jsonObj["d"]["drivetrainId"]));

                    model.Transmissions = CreateDataList(jsonObj["d"]["transmissions"], model.SelectedTransmissionId);
                    model.DriveTrains = CreateDataList(jsonObj["d"]["drivetrains"], model.SelectedDriveTrainId);
                    model.Engines = CreateDataList(jsonObj["d"]["engines"], model.SelectedEngineId);
                    model.OptionalEquipmentMarkup = ConvertToString((JValue)(jsonObj["d"]["optionalEquipmentMarkup"]));

                    var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                    if (firstEngine != null)
                    {
                        firstEngine.Selected = true;
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
                }
            }
            else
            {
                karPowerService.ExecuteGetEngines(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerService.GetEnginesResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetEnginesResult);
                    model.Engines = CreateDataList(jsonObj["d"], model.SelectedEngineId);
                    if (model.SelectedEngineId == 0)
                    {
                        var firstEngine = model.Engines.FirstOrDefault(i => i.Value != "0");
                        model.SelectedEngineId = Convert.ToInt32(firstEngine.Value);
                    }
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
            else
            {
                karPowerService.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerService.GetTransmissionsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetTransmissionsResult);
                    model.Transmissions = CreateDataList(jsonObj["d"], model.SelectedTransmissionId);
                    if (model.SelectedTransmissionId == 0)
                    {
                        var firstTransmission = model.Transmissions.FirstOrDefault(i => i.Value != "0");
                        model.SelectedTransmissionId = Convert.ToInt32(firstTransmission.Value);
                    }
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
            else
            {
                karPowerService.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerService.GetDriveTrainsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerService.GetDriveTrainsResult);
                    model.DriveTrains = CreateDataList(jsonObj["d"], model.SelectedDriveTrainId);
                    if (model.SelectedDriveTrainId == 0)
                    {
                        var firstDriveTrain = model.DriveTrains.FirstOrDefault(i => i.Value != "0");
                        model.SelectedDriveTrainId = Convert.ToInt32(firstDriveTrain.Value);
                    }
                }
            }
        }

        //[HttpPost]
        public ActionResult UpdateValuationByOptionalEquipment(string listingId, string isChecked)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.IsMultipleTrims = true;

            SessionHandler.KarPowerViewModel= model;
            return PartialView("KarPowerResult", model);
        }

        public ActionResult UpdateValuationByOptionalEquipmentForBucketJump(string listingId, string isChecked)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.IsMultipleTrims = true;
            var selectedOptions = model.OptionalEquipmentMarkupList.Where(i => i.IsSelected).Select(i => i.DisplayName).ToList();
            Session["StoreKarPowerOptions"] = selectedOptions.Aggregate<string>((first, second) => first + "," + second);

            SessionHandler.KarPowerViewModel= model;
            return PartialView("_KarPowerSummaryForBucketJumpPartial", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingModel(int modelId)
        {
            var model = CreateViewModelForUpdateValuationByChangingModel(modelId);
            
            SessionHandler.KarPowerViewModel = model;

            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingModelForBucketJump(int modelId)
        {
            var model = CreateViewModelForUpdateValuationByChangingModel(modelId);

            SessionHandler.KarPowerViewModel = model;

            return PartialView("_KarPowerSummaryForBucketJumpPartial", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingTrim(int trimId)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(trimId);
            model.IsMultipleTrims = true;

            SessionHandler.KarPowerViewModel= model;
            
            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingTrimForBucketJump(int trimId)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(trimId);
            model.IsMultipleTrims = true;

            SessionHandler.KarPowerViewModel = model;

            return PartialView("_KarPowerSummaryForBucketJumpPartial", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingTrimAndTransmission(int trimId, int transmissionId, int engineId = 0)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(Convert.ToInt32(trimId), Convert.ToInt32(transmissionId), engineId);
            model.IsMultipleTrims = false;

            SessionHandler.KarPowerViewModel= model;
            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public ActionResult UpdateValuationByChangingTrimAndTransmissionForBucketJump(int trimId, int transmissionId, int engineId = 0)
        {
            var model = CreateViewModelForUpdateValuationByChangingTrim(Convert.ToInt32(trimId), Convert.ToInt32(transmissionId), engineId);
            model.IsMultipleTrims = false;

            SessionHandler.KarPowerViewModel = model;
            return PartialView("_KarPowerSummaryForBucketJumpPartial", model);
        }
        
        [HttpPost]
        public ActionResult PrintReport(KarPowerViewModel submittedModel)
        {
            var model = new KarPowerViewModel();
            if (SessionHandler.KarPowerViewModel!= null)
                model = SessionHandler.KarPowerViewModel;
            try
            {
                var dealer = SessionHandler.Dealer;
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"],
                    UserName = dealer.DealerSetting.KellyBlueBook,
                    Password = dealer.DealerSetting.KellyPassword
                };

                var selectedMake = model.Makes.Any(i => i.Value == model.SelectedMakeId.ToString()) ? model.Makes.FirstOrDefault(i => i.Value == model.SelectedMakeId.ToString()) : model.Makes.FirstOrDefault();
                var selectedModel = model.Models.Any(i => i.Value == model.SelectedModelId.ToString()) ? model.Models.FirstOrDefault(i => i.Value == model.SelectedModelId.ToString()) : model.Models.FirstOrDefault();
                var selectedTrim = model.Trims.Any(i => i.Value == model.SelectedTrimId.ToString()) ? model.Trims.FirstOrDefault(i => i.Value == model.SelectedTrimId.ToString()) : model.Trims.FirstOrDefault();
                var selectedEngine = model.Engines.Any(i => i.Value == model.SelectedEngineId.ToString()) ? model.Engines.FirstOrDefault(i => i.Value == model.SelectedEngineId.ToString()) : model.Engines.FirstOrDefault();
                var selectedTransmission = model.Transmissions.Any(i => i.Value == model.SelectedTransmissionId.ToString()) ? model.Transmissions.FirstOrDefault(i => i.Value == model.SelectedTransmissionId.ToString()) : model.Transmissions.FirstOrDefault();
                var selectedDriveTrain = model.DriveTrains.Any(i => i.Value == model.SelectedDriveTrainId.ToString()) ? model.DriveTrains.FirstOrDefault(i => i.Value == model.SelectedDriveTrainId.ToString()) : model.DriveTrains.FirstOrDefault();
                var selectedReport = model.Reports.Any(i => i.Value == submittedModel.SelectedReport) ? model.Reports.FirstOrDefault(i => i.Value == submittedModel.SelectedReport) : model.Reports.FirstOrDefault();
                var dataToSave = new SaveVrsVehicleContract()
                {
                    category = "ca090fcb-597d-482d-b5aa-f528dd7bba21", // Appraisal
                    certified = false,
                    drivetrain = selectedDriveTrain!= null?selectedDriveTrain.Text:"",
                    drivetrainId = selectedDriveTrain!=null?selectedDriveTrain.Value:"",
                    engine = selectedEngine!=null?selectedEngine.Text:"",
                    engineId = selectedEngine!=null?selectedEngine.Value:"",
                    exteriorColor = "Select Color or Enter Manually",
                    interiorColor = "Select Color or Enter Manually",
                    initialDate = model.ValuationDate.ToString("M/d/yyyy"),
                    inventoryEntryId = "",
                    isPreOwnedSessionVehicle = true,
                    make = selectedMake!= null ? selectedMake.Text : "",
                    makeId = selectedMake != null ? selectedMake.Value : "",
                    mileage = model.SelectedMileage,
                    model = selectedModel!=null?selectedModel.Text:"",
                    modelId = selectedModel!=null?selectedModel.Value:"",
                    optionHistory = model.OptionalEquipmentHistoryList.ToArray(),
                    options = model.OptionalEquipmentMarkupList.ToArray(),
                    sellPrice = "",
                    stockNumber = "",
                    transmission = selectedTransmission != null ? selectedTransmission.Text : "",
                    transmissionId = selectedTransmission != null ? selectedTransmission.Value : "",
                    trim = selectedTrim != null?selectedTrim.Text:"",
                    trimId = selectedTrim != null ? selectedTrim.Value : "",
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
                    SaveKarPowerOptions(model.InventoryId, model.Vin, selectedModel.Value, selectedTrim.Value, submittedModel.SelectedOptionIds, selectedEngine.Value, selectedTransmission.Value, selectedDriveTrain.Value, submittedModel.BaseWholesale, submittedModel.Wholesale, submittedModel.Type);
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

                    return File(karPowerServiceWrapper.ExecuteGetPreOwnedVehicleReport(hfReportParams), "application/pdf", selectedReport.Text + " " + DateTime.Now.ToString("MM/dd/yyy") + ".pdf");
                }
            }
            catch (Exception)
            {

            }

            return File(new byte[] { }, "application/pdf", "Error.pdf");
        }

        [HttpPost]
        public ActionResult KarPowerResult(string vin, string mileage)
        {
            var model = CreateViewModelForKarPowerResult(vin, mileage);
            model.IsMultipleTrims = true;

            SessionHandler.KarPowerViewModel= model;
            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public ActionResult KarPowerResultForBucketJump(string vin, string mileage)
        {
            var model = CreateViewModelForKarPowerResultForBucketJump(vin, mileage);
            if (model != null)
            {
                model.IsMultipleTrims = true;
                SessionHandler.KarPowerViewModel = model;
            }
            return PartialView("_KarPowerSummaryForBucketJumpPartial", model);
        }

        [HttpPost]
        public ActionResult KarPowerResultForMassBucketJump(string listingId, string dealer, string price, string year, string make, string model, string color, string miles, string plusPrice, bool certified, string independentAdd, string certifiedAdd, string wholesaleWithoutOptions, string wholesaleWithOptions, string[] options, int chartCarType, string trims, bool isAll, bool isFranchise, bool isIndependant, int inventoryType, int ranges, string selectedVin, string image)
        {
            var pdfController = new PDFController();
            var bucketJumpModel = pdfController.ProcessBucketJumpWithKarPowerOptions(listingId, dealer, price, year, make, model,
                color, miles, plusPrice, certified, independentAdd, certifiedAdd, wholesaleWithoutOptions,
                wholesaleWithOptions, options, chartCarType, trims, isAll, isFranchise, isIndependant, inventoryType,
                ranges, selectedVin,0, 0,image);
            
            //bucketJumpModel.ListOfSimilarUsedInventories =
            //    _inventoryManagementForm.GetSimilarBucketJumpInventories(SessionHandler.Dealer.DealerGroupId,
            //        Convert.ToInt32(listingId), Convert.ToInt32(year), make, model, string.Empty,
            //        SessionHandler.Dealer.DealerSetting.FirstTimeRangeBucketJump, SessionHandler.Dealer.DealerSetting.SecondTimeRangeBucketJump,
            //        SessionHandler.Dealer.DealerSetting.IntervalBucketJump).ToList();
            bucketJumpModel.ListOfSimilarUsedInventories =
                _inventoryManagementForm.GetSimilarUsedInventories(SessionHandler.Dealer.DealerGroupId,
                    Convert.ToInt32(listingId), Convert.ToInt32(year), make, model, string.Empty).ToList();
            
            bucketJumpModel.Certified = certified;
            return PartialView("_KarPowerSummaryForMassBucketJumpPartial", bucketJumpModel);
        }

        [HttpPost]
        public ActionResult KarPowerResultInSingleMode(int inventoryid,string vin, string mileage, string modelId, string trimId, string type, bool hasVin)
        {
            var model = CreateViewModelForKarPowerResult(inventoryid,vin, mileage, modelId, trimId, type);
            //model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;
            model.HasVin = hasVin;

            SessionHandler.KarPowerViewModel= model;
            return PartialView("KarPowerResult", model);
        }

        [HttpPost]
        public void SaveKarPowerOptions(int inventoryId,string vin, string modelId, string trimId, string selectedOptionIds, string engineId, string transmissionId, string driveTrainId, string baseWholesale, string wholesale, string type)
        {
            using (var context = new VincontrolEntities())
            {
                var convertedModelId = Convert.ToInt32(modelId);
                var convertedTrimId = Convert.ToInt32(trimId);
                var convertedEngineId = Convert.ToInt32(engineId);
                var convertedTransmissionId = Convert.ToInt32(transmissionId);
                var convertedDriveTrainId = Convert.ToInt32(driveTrainId);
                switch (type)
                {
                    case "Inventory":
                        var inventory = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                        if (inventory != null)
                        {
                            inventory.Vehicle.KBBModelId = convertedModelId;
                            inventory.Vehicle.KBBTrimId = convertedTrimId;
                            inventory.Vehicle.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            inventory.Vehicle.KBBEngineId = convertedEngineId;
                            inventory.Vehicle.KBBTransmissionId = convertedTransmissionId;
                            inventory.Vehicle.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();

                            if (!String.IsNullOrEmpty(baseWholesale))
                            {
                                var existingKbb = context.KellyBlueBooks.FirstOrDefault(i => i.VehicleId == inventory.Vehicle.VehicleId && i.TrimId == convertedTrimId);
                                if (existingKbb != null)
                                {
                                    existingKbb.BaseWholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(baseWholesale));
                                    existingKbb.WholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(wholesale));
                                    context.SaveChanges();
                                }
                            }
                        }
                        break;
                    case "SoldOut":
                        var soldoutInventory = context.SoldoutInventories.FirstOrDefault(i => i.SoldoutInventoryId == inventoryId);
                        if (soldoutInventory != null)
                        {
                            soldoutInventory.Vehicle.KBBModelId = convertedModelId;
                            soldoutInventory.Vehicle.KBBTrimId = convertedTrimId;
                            soldoutInventory.Vehicle.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            soldoutInventory.Vehicle.KBBEngineId = convertedEngineId;
                            soldoutInventory.Vehicle.KBBTransmissionId = convertedTransmissionId;
                            soldoutInventory.Vehicle.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();
                            if (!String.IsNullOrEmpty(baseWholesale))
                            {
                                var existingKbb = context.KellyBlueBooks.FirstOrDefault(i => i.VehicleId == soldoutInventory.Vehicle.VehicleId && i.TrimId == convertedTrimId);
                                if (existingKbb != null)
                                {
                                    existingKbb.BaseWholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(baseWholesale));
                                    existingKbb.WholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(wholesale));
                                    context.SaveChanges();
                                }
                            }
                        }
                        break;
                    case "Wholesale":
                        var wholesaleinventory = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                        if (wholesaleinventory != null)
                        {
                            wholesaleinventory.Vehicle.KBBModelId = convertedModelId;
                            wholesaleinventory.Vehicle.KBBTrimId = convertedTrimId;
                            wholesaleinventory.Vehicle.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            wholesaleinventory.Vehicle.KBBEngineId = convertedEngineId;
                            wholesaleinventory.Vehicle.KBBTransmissionId = convertedTransmissionId;
                            wholesaleinventory.Vehicle.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();
                            if (!String.IsNullOrEmpty(baseWholesale))
                            {
                                var existingKbb = context.KellyBlueBooks.FirstOrDefault(i => i.VehicleId == wholesaleinventory.Vehicle.VehicleId && i.TrimId == convertedTrimId);
                                if (existingKbb != null)
                                {
                                    existingKbb.BaseWholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(baseWholesale));
                                    existingKbb.WholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(wholesale));
                                    context.SaveChanges();
                                }
                            }
                        }
                        break;
                    case "Appraisal":
                        var appraisal = context.Appraisals.FirstOrDefault(i => i.AppraisalId == inventoryId);
                        if (appraisal != null)
                        {
                            appraisal.Vehicle.KBBModelId = convertedModelId;
                            appraisal.Vehicle.KBBTrimId = convertedTrimId;
                            appraisal.Vehicle.KBBOptionsId = selectedOptionIds.Substring(0, selectedOptionIds.Length - 1);
                            appraisal.Vehicle.KBBEngineId = convertedEngineId;
                            appraisal.Vehicle.KBBTransmissionId = convertedTransmissionId;
                            appraisal.Vehicle.KBBDriveTrainId = convertedDriveTrainId;
                            context.SaveChanges();

                            if (String.IsNullOrEmpty(appraisal.KarPowerEntryId))
                            {
                                // save selected trim & selected options on karpower
                                var dealer = SessionHandler.Dealer;
                                var karpowerService = new KarPowerService();
                                karpowerService.ExecuteSaveVehicleWithVin(vin, appraisal.KarPowerEntryId, convertedTrimId, selectedOptionIds, appraisal.Mileage.GetValueOrDefault(), appraisal.ExteriorColor, appraisal.Vehicle.InteriorColor, dealer.DealerSetting.KellyBlueBook, dealer.DealerSetting.KellyPassword);
                                if (!String.IsNullOrEmpty(karpowerService.EntryId))
                                {
                                    appraisal.KarPowerEntryId = karpowerService.EntryId;
                                    context.SaveChanges();
                                }
                            }

                            if (!String.IsNullOrEmpty(baseWholesale))
                            {
                                var existingKbb = context.KellyBlueBooks.FirstOrDefault(i => i.VehicleId == appraisal.Vehicle.VehicleId && i.TrimId == convertedTrimId);
                                if (existingKbb != null)
                                {
                                    existingKbb.BaseWholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(baseWholesale));
                                    existingKbb.WholeSale = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersAndReturnNumber(wholesale));
                                    context.SaveChanges();
                                }
                            }
                        }
                        break;
                    default: break;
                }

             
            }
        }

        [HttpPost]
        public void SaveSimpleKarPowerOptions(int inventoryId, string trimId,string type)
        {
            using (var context = new VincontrolEntities())
            {
                var convertedTrimId = Convert.ToInt32(trimId);
                switch (type)
                {
                    case "Inventory":
                        var inventory = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                        if (inventory != null)
                        {
                            inventory.Vehicle.KBBTrimId = convertedTrimId;
                            context.SaveChanges();

                          
                        }
                        break;
                    case "SoldOut":
                        var soldoutInventory = context.SoldoutInventories.FirstOrDefault(i => i.SoldoutInventoryId == inventoryId);
                        if (soldoutInventory != null)
                        {
                           
                            soldoutInventory.Vehicle.KBBTrimId = convertedTrimId;
                        
                         
                            context.SaveChanges();
                          
                        }
                        break;
              
                    case "Appraisal":
                        var appraisal = context.Appraisals.FirstOrDefault(i => i.AppraisalId == inventoryId);
                        if (appraisal != null)
                        {
                          
                            appraisal.Vehicle.KBBTrimId = convertedTrimId;
                            
                            context.SaveChanges();

                        }
                        break;
                    default: break;
                }


            }
        }

        [HttpPost]
        public void SaveSimpleManheimTrim(int inventoryId, string trimId, string type)
        {
            using (var context = new VincontrolEntities())
            {
                var convertedTrimId = Convert.ToInt32(trimId);
                switch (type)
                {
                    case "Inventory":
                        var inventory = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                        if (inventory != null)
                        {
                            inventory.Vehicle.ManheimTrimId = convertedTrimId;
                            context.SaveChanges();


                        }
                        break;
                    case "SoldOut":
                        var soldoutInventory = context.SoldoutInventories.FirstOrDefault(i => i.SoldoutInventoryId == inventoryId);
                        if (soldoutInventory != null)
                        {

                            soldoutInventory.Vehicle.ManheimTrimId = convertedTrimId;


                            context.SaveChanges();

                        }
                        break;

                    case "Appraisal":
                        var appraisal = context.Appraisals.FirstOrDefault(i => i.AppraisalId == inventoryId);
                        if (appraisal != null)
                        {

                            appraisal.Vehicle.ManheimTrimId = convertedTrimId;

                            context.SaveChanges();

                        }
                        break;
                    default: break;
                }


            }
        }

        public ActionResult UpdateValuationByOptionalEquipmentInSingleMode(string listingId, string isChecked, string trimId)
        {
            var model = CreateViewModelForUpdateValuationByOptionalEquipment(listingId, isChecked);
            model.SelectedTrimId = Convert.ToInt32(trimId);
            model.IsMultipleTrims = false;

            SessionHandler.KarPowerViewModel= model;
            return PartialView("KarPowerResult", model);
        }
        
        private KarPowerViewModel CreateViewModelForUpdateValuationByOptionalEquipment(string listingId, string isChecked)
        {
            var model = new KarPowerViewModel();
            if (SessionHandler.KarPowerViewModel!= null)
                model = SessionHandler.KarPowerViewModel;

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
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }
            }
            catch (Exception)
            {

            }

            return model;
        }

        private KarPowerViewModel CreateViewModelForUpdateValuationByChangingModel(int modelId)
        {
            var model = new KarPowerViewModel();
            if (SessionHandler.KarPowerViewModel != null)
                model = SessionHandler.KarPowerViewModel;

            model.SelectedModelId = modelId;
            model.SelectedTrimId = 0;
            model.SelectedEngineId = 0;
            model.SelectedTransmissionId = 0;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

            try
            {
                var karPowerServiceWrapper = new KarPowerService
                {
                    CookieContainer = (CookieContainer)Session["CookieContainer"],
                    CookieCollection = (CookieCollection)Session["CookieCollection"]
                };

                var trimsResult = karPowerServiceWrapper.ExecuteGetTrims(model.SelectedYearId, modelId, model.ValuationDate);
                if (!String.IsNullOrEmpty(trimsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(trimsResult);
                    model.Trims = CreateDataList(jsonObj["d"], model.SelectedTrimId);
                    model.SelectedTrimId = Convert.ToInt32(model.Trims.First().Value);
                }

                // get Engines list
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

                // get Transmissions list
                karPowerServiceWrapper.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
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
                karPowerServiceWrapper.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
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
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
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
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new vincontrol.DomainObject.ExtendedSelectListItem()
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

        private KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId)
        {
            var model = new KarPowerViewModel();
            if (SessionHandler.KarPowerViewModel!= null)
                model = SessionHandler.KarPowerViewModel;

            model.SelectedTrimId = trimId;
            model.SelectedEngineId = 0;
            model.SelectedTransmissionId = 0;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

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
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new vincontrol.DomainObject.ExtendedSelectListItem()
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

        private KarPowerViewModel CreateViewModelForUpdateValuationByChangingTrim(int trimId, int transmissionId, int engineId)
        {
            var model = new KarPowerViewModel();
            if (SessionHandler.KarPowerViewModel!= null)
                model = SessionHandler.KarPowerViewModel;

            model.SelectedTrimId = trimId;
            model.SelectedEngineId = engineId;
            model.SelectedTransmissionId = transmissionId;
            model.SelectedDriveTrainId = 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

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
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new vincontrol.DomainObject.ExtendedSelectListItem()
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
                var dealer = (DealershipViewModel) SessionHandler.Dealer;
                var kbbUserName = string.Empty;
                var kbbPassword = string.Empty;
                using (var context = new VincontrolEntities())
                {
                    var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealer.DealershipId);
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

            // get Trims list
            //NOTE: more than 1 model
            if (model.SelectedModelId.Equals(0))
            {
                model.IsMultipleModels = true;
                model.SelectedModelId = Convert.ToInt32(model.Models.First().Value);
                var trimsResult = karPowerServiceWrapper.ExecuteGetTrims(model.SelectedYearId, model.SelectedModelId, model.ValuationDate);
                if (!String.IsNullOrEmpty(trimsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(trimsResult);
                    model.Trims = CreateDataList(jsonObj["d"], model.SelectedTrimId);
                    model.SelectedTrimId = Convert.ToInt32(model.Trims.First().Value);
                }
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
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }

                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, valuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new ExtendedSelectListItem()
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

        private KarPowerViewModel CreateViewModelForKarPowerResultForBucketJump(string vin, string mileage)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() { Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            var karPowerServiceWrapper = new KarPowerService();

            var savedTrimId = 0;
            var savedOptionIds = string.Empty;
            var savedEngineId = 0;
            var savedTransmissionId = 0;
            var savedDriveTrainId = 0;

            var context = new VincontrolEntities();
            var dealer = SessionHandler.Dealer;
            try
            {
                
                var kbbUserName = string.Empty;
                var kbbPassword = string.Empty;
                var setting = context.Settings.FirstOrDefault(i => i.DealerId == dealer.DealershipId);
                if (setting != null)
                {
                    kbbUserName = setting.KellyBlueBook;
                    kbbPassword = setting.KellyPassword;
                }

                var inventory = context.Inventories.FirstOrDefault(i => i.Vehicle.Vin == vin && i.DealerId == dealer.DealershipId);
                if (inventory != null)
                {
                    savedTrimId = inventory.Vehicle.KBBTrimId.GetValueOrDefault();
                    savedOptionIds = inventory.Vehicle.KBBOptionsId;
                    savedEngineId = inventory.Vehicle.KBBEngineId.GetValueOrDefault();
                    savedTransmissionId = inventory.Vehicle.KBBTransmissionId.GetValueOrDefault();
                    savedDriveTrainId = inventory.Vehicle.KBBDriveTrainId.GetValueOrDefault();
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
                    if (model.SelectedMakeId.Equals(0)) // Cannot decode this VIN
                        return null;

                    model.SelectedModelId = ConvertToInt32(((JValue)(jsonObj["d"]["modelId"])));
                    model.SelectedTrimId = savedTrimId == 0 ? ConvertToInt32(((JValue)(jsonObj["d"]["trimId"]))) : savedTrimId;
                    model.SelectedEngineId = savedEngineId == 0 ? ConvertToInt32(((JValue)(jsonObj["d"]["engineId"]))) : savedEngineId;
                    model.SelectedTransmissionId = savedTransmissionId == 0 ? ConvertToInt32(((JValue)(jsonObj["d"]["transmissionId"]))) : savedTransmissionId;
                    model.SelectedDriveTrainId = savedDriveTrainId == 0 ? ConvertToInt32(((JValue)(jsonObj["d"]["drivetrainId"]))) : savedDriveTrainId;
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
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // get Trims list
            //NOTE: more than 1 model
            if (model.SelectedModelId.Equals(0))
            {
                model.IsMultipleModels = true;
                var isMatch = false;
                while (!isMatch)
                {
                    model.SelectedModelId = Convert.ToInt32(model.Models.First().Value);
                    var trimsResult = karPowerServiceWrapper.ExecuteGetTrims(model.SelectedYearId, model.SelectedModelId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(trimsResult))
                    {
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(trimsResult);
                        model.Trims = CreateDataList(jsonObj["d"], model.SelectedTrimId);
                        isMatch = model.SelectedTrimId.Equals(0) || model.Trims.Any(i => i.Value.Equals(model.SelectedTrimId.ToString()));
                    }
                }
            }

            // multiple trims
            if (model.SelectedTrimId == 0)
            {
                var firstTrim = model.Trims.FirstOrDefault(i => i.Value != "0");
                model.SelectedTrimId = Convert.ToInt32(firstTrim.Value);
            }
            //model.SelectedEngineId = savedTrimId == model.SelectedTrimId ? savedEngineId : 0;
            //model.SelectedTransmissionId = savedTrimId == model.SelectedTrimId ? savedTransmissionId : 0;
            //model.SelectedDriveTrainId = savedTrimId == model.SelectedTrimId ? savedDriveTrainId : 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();
            
            // multiple engines
            CheckMultipleEngines(model, karPowerServiceWrapper, vin, valuationDate);

            // multiple transmissions
            CheckMultipleTranmissions(model, karPowerServiceWrapper, vin, valuationDate);

            CheckMultipleDriveTrains(model, karPowerServiceWrapper, vin, valuationDate);
            
            //model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(model.OptionalEquipmentMarkup);
            IList<OptionContract> TempOptionalEquipmentMarkupList = new List<OptionContract>();
            karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
            if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
            {
                var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
            }
            var selectedOptions = model.OptionalEquipmentMarkupList.Where(i => i.IsSelected).Select(i => i.DisplayName).ToList();
            Session["StoreKarPowerOptions"] = selectedOptions.Count > 0 ? selectedOptions.Aggregate<string>((first, second) => first + ", " + second) : "";

            // loading save options in database
            if (model.SelectedTrimId == savedTrimId && !String.IsNullOrEmpty(savedOptionIds))
            {
                foreach (var optionContract in model.OptionalEquipmentMarkupList)
                {
                    optionContract.IsSelected = savedOptionIds.Contains(optionContract.Id);
                }
            }

            try
            {

                // get karpower values without options
                try
                {
                    karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, valuationDate, Convert.ToInt32(mileage), 2, 0, new OptionContract[] { }, new OptionContract[] { });                    
                }
                catch (Exception) { }
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesaleWithoutOptions = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value);
                }

                try
                {
                    karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, valuationDate, Convert.ToInt32(mileage), 2, 0, new OptionContract[] { }, TempOptionalEquipmentMarkupList.ToArray());
                    
                }
                catch (Exception) { }
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }

                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, valuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new ExtendedSelectListItem()
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

        private KarPowerViewModel CreateViewModelForKarPowerResult(int inventoryid,string vin, string mileage, string modelId, string trimId, string type)
        {
            var valuationDate = DateTime.Now;
            var model = new KarPowerViewModel() {InventoryId = inventoryid,Vin = vin, ValuationDate = valuationDate, SelectedMileage = Convert.ToInt32(mileage) };
            var karPowerServiceWrapper = new KarPowerService();

            var savedModelId = 0;
            var savedTrimId = 0;
            var savedOptionIds = string.Empty;
            var savedEngineId = 0;
            var savedTransmissionId = 0;
            var savedDriveTrainId = 0;

            try
            {
                var dealer = SessionHandler.Dealer;
                var kbbUserName = dealer.DealerSetting.KellyBlueBook;
                var kbbPassword = dealer.DealerSetting.KellyPassword;
                
                
                using (var context = new VincontrolEntities())
                {
                  
                    switch (type)
                    {
                        case "Inventory":
                            var inventory = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryid && i.DealerId == dealer.DealershipId);
                            if (inventory != null)
                            {
                                savedModelId = inventory.Vehicle.KBBModelId.GetValueOrDefault();
                                savedTrimId = inventory.Vehicle.KBBTrimId.GetValueOrDefault();
                                savedOptionIds = inventory.Vehicle.KBBOptionsId;
                                savedEngineId = inventory.Vehicle.KBBEngineId.GetValueOrDefault();
                                savedTransmissionId = inventory.Vehicle.KBBTransmissionId.GetValueOrDefault();
                                savedDriveTrainId = inventory.Vehicle.KBBDriveTrainId.GetValueOrDefault();
                            }
                            break;
                        case "SoldOut":
                            var soldout = context.SoldoutInventories.FirstOrDefault(i => i.SoldoutInventoryId == inventoryid && i.DealerId == dealer.DealershipId);
                            if (soldout != null)
                            {
                                savedModelId = soldout.Vehicle.KBBModelId.GetValueOrDefault();
                                savedTrimId = soldout.Vehicle.KBBTrimId.GetValueOrDefault();
                                savedOptionIds = soldout.Vehicle.KBBOptionsId;
                                savedEngineId = soldout.Vehicle.KBBEngineId.GetValueOrDefault();
                                savedTransmissionId = soldout.Vehicle.KBBTransmissionId.GetValueOrDefault();
                                savedDriveTrainId = soldout.Vehicle.KBBDriveTrainId.GetValueOrDefault();
                            }
                            break;
                        case "Wholesale":
                            var wholesale = context.Inventories.FirstOrDefault(i => i.InventoryId == inventoryid && i.DealerId == dealer.DealershipId && i.InventoryStatusCodeId == Constanst.InventoryStatus.Wholesale);
                            if (wholesale != null)
                            {
                                savedModelId = wholesale.Vehicle.KBBModelId.GetValueOrDefault();
                                savedTrimId = wholesale.Vehicle.KBBTrimId.GetValueOrDefault();
                                savedOptionIds = wholesale.Vehicle.KBBOptionsId;
                                savedEngineId = wholesale.Vehicle.KBBEngineId.GetValueOrDefault();
                                savedTransmissionId = wholesale.Vehicle.KBBTransmissionId.GetValueOrDefault();
                                savedDriveTrainId = wholesale.Vehicle.KBBDriveTrainId.GetValueOrDefault();
                            }
                            break;
                        case "Appraisal":
                            var appraisal = context.Appraisals.FirstOrDefault(i => i.AppraisalId == inventoryid && i.DealerId == dealer.DealershipId);
                            if (appraisal != null)
                            {
                                savedModelId = appraisal.Vehicle.KBBModelId.GetValueOrDefault();
                                savedTrimId = appraisal.Vehicle.KBBTrimId.GetValueOrDefault();
                                savedOptionIds = appraisal.Vehicle.KBBOptionsId;
                                savedEngineId = appraisal.Vehicle.KBBEngineId.GetValueOrDefault();
                                savedTransmissionId = appraisal.Vehicle.KBBTransmissionId.GetValueOrDefault();
                                savedDriveTrainId = appraisal.Vehicle.KBBDriveTrainId.GetValueOrDefault();
                            }
                            break;
                        default: break;
                            
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
            model.SelectedEngineId = savedTrimId == model.SelectedTrimId ? savedEngineId : 0;
            model.SelectedTransmissionId = savedTrimId == model.SelectedTrimId ? savedTransmissionId : 0;
            model.SelectedDriveTrainId = savedTrimId == model.SelectedTrimId ? savedDriveTrainId : 0;
            // reset option list
            model.OptionalEquipmentHistoryList = new List<OptionContract>();
            model.OptionalEquipmentMarkupList = new List<OptionContract>();
            model.Reports = new List<ExtendedSelectListItem>();

            try
            {
                // get Trims list
                //NOTE: more than 1 model
                if (model.SelectedModelId.Equals(0))
                {
                    model.IsMultipleModels = true;

                    model.SelectedModelId = modelId.Equals("0") ? Convert.ToInt32(model.Models.First().Value) : Convert.ToInt32(modelId);
                    var trimsResult = karPowerServiceWrapper.ExecuteGetTrims(model.SelectedYearId, model.SelectedModelId, model.ValuationDate);
                    if (!String.IsNullOrEmpty(trimsResult))
                    {
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(trimsResult);
                        model.Trims = CreateDataList(jsonObj["d"], model.SelectedTrimId);
                        model.SelectedTrimId = modelId.Equals("0")
                            ? Convert.ToInt32(model.Trims.First().Value)
                            : model.SelectedTrimId;
                    }
                }

                // get Engines list
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

                // get Transmissions list
                karPowerServiceWrapper.ExecuteGetTransmissions(model.SelectedTrimId, model.ValuationDate);
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
                karPowerServiceWrapper.ExecuteGetDriveTrains(model.SelectedTrimId, model.ValuationDate);
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
                karPowerServiceWrapper.ExecuteGetDefaultOptionalEquipmentWithUser(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetDefaultOptionalEquipmentWithUserResult);
                    model.OptionalEquipmentMarkupList = CreateOptionalEquipmentList(ConvertToString((JValue)(jsonObj["d"])));
                    TempOptionalEquipmentMarkupList = AddEngineTransmissionDriveTrainAsOptions(model.OptionalEquipmentMarkupList, model.SelectedEngineId, model.SelectedTransmissionId, model.SelectedDriveTrainId);
                }

                // loading save options in database
                if (model.SelectedTrimId == savedTrimId && savedOptionIds != null)
                {
                    foreach (var optionContract in model.OptionalEquipmentMarkupList)
                    {
                        optionContract.IsSelected = savedOptionIds.Contains(optionContract.Id);
                    }
                }

                karPowerServiceWrapper.ExecuteGetValuation(model.SelectedTrimId, model.ValuationDate, model.SelectedMileage, 2, 0, model.OptionalEquipmentHistoryList.ToArray(), TempOptionalEquipmentMarkupList.ToArray());

                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetValuationResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetValuationResult);
                    model.BaseWholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleBase"])).Value).Replace("&mdash;", "N/A");
                    model.MileageAdjustment = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleMileageAdjusted"])).Value).Replace("&mdash;", "N/A");
                    model.Wholesale = Convert.ToString(((JValue)(jsonObj["d"]["wholesaleKBB"])).Value).Replace("&mdash;", "N/A");
                }


                karPowerServiceWrapper.ExecuteGetListCustomerReports(model.SelectedTrimId, model.ValuationDate);
                if (!String.IsNullOrEmpty(karPowerServiceWrapper.GetListCustomerReportsResult))
                {
                    var jsonObj = (JObject)JsonConvert.DeserializeObject(karPowerServiceWrapper.GetListCustomerReportsResult);

                    model.Reports.AddRange(jsonObj["d"].Children().Select(item => new ExtendedSelectListItem()
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
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }


            int receivedListingId = Convert.ToInt32(listingId);
            var row = _inventoryManagementForm.GetInventory(receivedListingId);
            if (row != null)
            {
                if (String.IsNullOrEmpty(row.Vehicle.Vin))
                {
                    var dealer = new DealershipViewModel(row.Dealer);

                    var chartGraph =
                        MarketHelper.GetMarketCarsForNationwideMarket(row.InventoryId, dealer, false);
                    if (chartGraph.TypedChartModels.Any())
                    {
                        var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            row.Vehicle.Vin = firstOrDefault.VIN;

                        }
                    }

                }

                ViewData["VIN"] = row.Vehicle.Vin;
                ViewData["MILEAGE"] = row.Mileage.GetValueOrDefault();
            }


            return View("KarPowerSummary");
        }

        public ActionResult GetKarPowerSummaryForBuckerJump(string listingId, string dealer, string price, string year,
            string make, string model, string color, string miles, bool isAllCar, bool isAuto, bool isCarscom,
            string trims, bool isAll, bool isFranchise, bool isIndependant, int inventoryType, int ranges,
            string selectedVin, string image = "")
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            
            int receivedListingId = Convert.ToInt32(listingId);
            var dateInStock = new DateTime();

            var row = _inventoryManagementForm.GetInventory(receivedListingId);
            Vehicle vehicle = null;
            if (row == null)
            {
                var existingAppraisal = _appraisalManagementForm.GetAppraisal(receivedListingId);
                if (existingAppraisal != null)
                {
                    vehicle = existingAppraisal.Vehicle;
                    dateInStock = existingAppraisal.AppraisalDate.GetValueOrDefault();
                }
            }
            else
            {
                vehicle = row.Vehicle;
                dateInStock = row.DateInStock.GetValueOrDefault();
            }

            if (vehicle != null)
            {
                ViewData["SelectedVin"] = string.IsNullOrEmpty(selectedVin) ? string.Empty : selectedVin;
                ViewData["VIN"] = vehicle.Vin;
                ViewData["MILEAGE"] = CommonHelper.RemoveSpecialCharacters(miles);
                ViewData["DEALER"] = dealer;
                ViewData["PRICE"] = price;
                ViewData["YEAR"] = year;
                ViewData["MAKE"] = make;
                ViewData["MODEL"] = model;
                ViewData["COLOR"] = color;
                ViewData["IMAGE"] = image;
                ViewData["LISTINGID"] = listingId;

                if (isAllCar)
                    ViewData["ChartCarType"] = Constanst.ChartCarType.All;
                else if (isAuto)
                    ViewData["ChartCarType"] = Constanst.ChartCarType.Auto;
                else
                    ViewData["ChartCarType"] = Constanst.ChartCarType.CarCom;

                ViewData["Trims"] = trims;
                ViewData["isAll"] = isAll;
                ViewData["isFranchise"] = isFranchise;
                ViewData["isIndependant"] = isIndependant;
                ViewData["inventoryType"] = inventoryType;
                ViewData["ranges"] = ranges;
                ViewData["daysAged"] = DateTime.Now.Subtract(dateInStock).Days;
                ViewData["IsMassBucketJump"] = SessionHandler.Dealer.IsPendragon &&
                                               (new string[] {"Jaguar", "Land Rover"}).Contains(
                                                   (String) ViewData["MAKE"]);
            }


            return View("KarPowerSummaryForBucketJump");
        }

        public ActionResult GetSingleKarPowerSummary(int listingId, string trimId, string modelId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new VincontrolEntities())
            {
                var hasVin = true;
                int receivedListingId = listingId;
                var row = context.Inventories.FirstOrDefault(x => x.InventoryId == receivedListingId) ??
                          new Inventory(context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == listingId), Constanst.VehicleStatus.SoldOut);

                {
                    if (String.IsNullOrEmpty(row.Vehicle.Vin))
                    {
                        var dealer = new DealershipViewModel(row.Dealer);

                        var chartGraph =
                            MarketHelper.GetMarketCarsForNationwideMarket(listingId, dealer,false);
                        if (chartGraph.TypedChartModels.Any())
                        {
                            var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                            if (firstOrDefault != null)
                            {
                                row.Vehicle.Vin = firstOrDefault.VIN;

                            }
                        }
                        hasVin = false;
                    }

                    ViewData["INVENTORYID"] = row.InventoryId;
                    ViewData["VIN"] = row.Vehicle.Vin;
                    ViewData["MILEAGE"] = row.Mileage.GetValueOrDefault();
                    ViewData["MODELID"] = modelId;
                    ViewData["TRIMID"] = trimId;
                    ViewData["TYPE"] = "Inventory";
                    ViewData["HASVIN"] = hasVin;
                    ViewData["URLDeatail"] = Url.Action("ViewIProfile", "Inventory", new { ListingID = listingId });
                }
                
            }

            return View("SingleKarPowerSummary");
        }

        public ActionResult GetSingleKarPowerSummaryForSoldCars(string listingId, string trimId, string modelId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }


            var hasVin = true;
            int receivedListingId = Convert.ToInt32(listingId);
            var row = _inventoryManagementForm.GetSoldInventory(receivedListingId);
            if (row != null)
            {
                if (String.IsNullOrEmpty(row.Vehicle.Vin))
                {
                    var dealer = new DealershipViewModel(row.Dealer);

                    var chartGraph =
                        MarketHelper.GetMarketCarsForNationwideMarket(row.SoldoutInventoryId, dealer, false);
                    if (chartGraph.TypedChartModels.Any())
                    {
                        var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            row.Vehicle.Vin = firstOrDefault.VIN;

                        }
                    }
                    hasVin = false;
                }
                ViewData["INVENTORYID"] = row.SoldoutInventoryId;
                ViewData["VIN"] = row.Vehicle.Vin;
                ViewData["MILEAGE"] = row.Mileage.GetValueOrDefault();
                ViewData["MODELID"] = modelId;
                ViewData["TRIMID"] = trimId;
                ViewData["TYPE"] = "SoldOut";
                ViewData["HASVIN"] = hasVin;
                ViewData["URLDeatail"] = Url.Action("ViewISoldProfile", "Inventory", new {ListingID = listingId});
            }



            return View("SingleKarPowerSummary");
        }

        public ActionResult GetSingleKarPowerSummaryForWholesale(string listingId, string trimId, string modelId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var hasVin = true;
            int receivedListingId = Convert.ToInt32(listingId);
            var row = _inventoryManagementForm.GetInventory(receivedListingId);
            if (row != null)
            {
                if (String.IsNullOrEmpty(row.Vehicle.Vin))
                {
                    var dealer = new DealershipViewModel(row.Dealer);

                    var chartGraph =
                        MarketHelper.GetMarketCarsForNationwideMarket(row.InventoryId, dealer, false);
                    if (chartGraph.TypedChartModels.Any())
                    {
                        var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            row.Vehicle.Vin = firstOrDefault.VIN;

                        }
                    }
                    hasVin = false;
                }
                ViewData["INVENTORYID"] = row.InventoryId;
                ViewData["VIN"] = row.Vehicle.Vin;
                ViewData["MILEAGE"] = row.Mileage.GetValueOrDefault();
                ViewData["MODELID"] = modelId;
                ViewData["TRIMID"] = trimId;
                ViewData["TYPE"] = "Wholesale";
                ViewData["HASVIN"] = hasVin;
                ViewData["URLDeatail"] = Url.Action("ViewIProfile", "Inventory", new {ListingID = listingId});
            }



            return View("SingleKarPowerSummary");
        }

        public ActionResult GetKarPowerSummaryForAppraisal(string appraisalId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new VincontrolEntities())
            {
                int receivedListingId = Convert.ToInt32(appraisalId);
                var row = context.Appraisals.FirstOrDefault(x => x.AppraisalId == receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.Vehicle.Vin))
                    {
                        var dealer = new DealershipViewModel(row.Dealer);

                        var chartGraph =
                            MarketHelper.GetMarketCarsForNationwideMarketForAppraisalOnChart(row.AppraisalId, dealer, false);
                        if (chartGraph.TypedChartModels.Any())
                        {
                            var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                            if (firstOrDefault != null)
                            {
                                row.Vehicle.Vin = firstOrDefault.VIN;

                            }
                        }
                        
                    }

                    ViewData["VIN"] = row.Vehicle.Vin;
                    ViewData["MILEAGE"] = row.Mileage.GetValueOrDefault();
                }
            }

            return View("KarPowerSummary");
        }

        public ActionResult GetSingleKarPowerSummaryForAppraisal(string appraisalId, string trimId, string modelId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            using (var context = new VincontrolEntities())
            {
                var hasVin = true;
                int receivedListingId = Convert.ToInt32(appraisalId);
                var row = _appraisalManagementForm.GetAppraisal(receivedListingId);
                if (row != null)
                {
                    if (String.IsNullOrEmpty(row.Vehicle.Vin))
                    {
                        var dealer = new DealershipViewModel(row.Dealer);

                        var chartGraph =
                            MarketHelper.GetMarketCarsForNationwideMarketForAppraisalOnChart(row.AppraisalId, dealer, false);
                        if (chartGraph.TypedChartModels.Any())
                        {
                            var firstOrDefault = chartGraph.TypedChartModels.FirstOrDefault();
                            if (firstOrDefault != null)
                            {
                                row.Vehicle.Vin = firstOrDefault.VIN;

                            }
                        }
                        hasVin = false;
                    }
                    ViewData["INVENTORYID"] = row.AppraisalId;
                    ViewData["VIN"] = row.Vehicle.Vin;
                    ViewData["MILEAGE"] =  row.Mileage.GetValueOrDefault();
                    ViewData["MODELID"] = modelId;
                    ViewData["TRIMID"] = trimId;
                    ViewData["TYPE"] = "Appraisal";
                    ViewData["HASVIN"] = hasVin;
                    ViewData["URLDeatail"] = Url.Action("ViewProfileForAppraisal", "Appraisal", new { AppraisalId = appraisalId });
                }
            }

            return View("SingleKarPowerSummary");
        }

        #endregion
    }
}
