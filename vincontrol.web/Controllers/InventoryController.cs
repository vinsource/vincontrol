using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.MobileControls;
using HiQPdf;
using vincontrol.Application.Forms.AppraisalManagement;
using vincontrol.Application.Forms.CommonManagement;
using vincontrol.Application.Forms.DealerManagement;
using vincontrol.Application.Forms.EmailWaitingListManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.VehicleLogManagement;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.AdvancedSearch;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.CarFax;
using vincontrol.ChromeAutoService;
using vincontrol.ChromeAutoService.AutomativeService;
using vincontrol.Constant;
using vincontrol.Data.Model;
using vincontrol.DomainObject;
using vincontrol.Helper;
using Vincontrol.Web.Handlers;
using Vincontrol.Web.HelperClass;
using Vincontrol.Web.Models;
using Vincontrol.Web.Security;
using DataHelper = Vincontrol.Web.HelperClass.DataHelper;
using Inventory = vincontrol.Data.Model.Inventory;
using KarPowerService = vincontrol.KBB.KBBService;
using SoldoutInventory = vincontrol.Data.Model.SoldoutInventory;

namespace Vincontrol.Web.Controllers
{
    public class LargeJsonResult : JsonResult
    {
        private const string JsonRequestGetNotAllowed =
            "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.";

        public LargeJsonResult()
        {
            MaxJsonLength = Int32.MaxValue;
            RecursionLimit = 10000;
        }

        public int MaxJsonLength { get; set; }
        public int RecursionLimit { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(JsonRequestGetNotAllowed);

            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = MaxJsonLength,
                    RecursionLimit = RecursionLimit
                };
                response.Write(serializer.Serialize(Data));
            }
        }
    }

    public class InventoryController : SecurityController
    {
        private ICommonManagementForm _commonManagementForm;
        private IInventoryManagementForm _inventoryManagementForm;
        private IAppraisalManagementForm _appraisalManagementForm;
        private ICarFaxService _carFaxService;
        private IVehicleLogManagementForm _vehicleLogForm;
        private IEmailWaitingListManagementForm _emailWaitingForm;
        private IDealerManagementForm _dealerManagementForm;

        public InventoryController()
        {
            _commonManagementForm = new CommonManagementForm();
            _inventoryManagementForm = new InventoryManagementForm();
            _appraisalManagementForm = new AppraisalManagementForm();
            _carFaxService = new CarFaxService();
            _vehicleLogForm = new VehicleLogManagementForm();
            _emailWaitingForm=new EmaiLWaitingListManagementForm();
            _dealerManagementForm=new DealerManagementForm();
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult TransferToWholeSaleFromInventoryNew(int listingId, int type)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false, url = Url.Action("LogOff", "Account") });
            }
            if (type == Constanst.CarInfoType.Sold)
                SQLHelper.TransferToWholeSaleFromSoldInventory(listingId);
            else
                SQLHelper.TransferToWholeSaleFromInventory(listingId);

            return Json(new { success = true, url = Url.Action("ViewInventory", "Inventory") });
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult TransferToWholeSaleFromSoldInventory(int listingId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            SQLHelper.TransferToWholeSaleFromSoldInventory(listingId);



            return RedirectToAction("ViewInventory");
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult TransferToInventoryFromWholesale(int listingId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            int autoListingId = SQLHelper.TransferToInventoryFromWholesale(listingId);

            return RedirectToAction("ViewIProfile", new { ListingID = autoListingId });
        }

        public ActionResult ViewSearchInventory(string searchString, string searchBy)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;

            var context = new VincontrolEntities();

            if (searchBy.Equals("Stock"))
            {

                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (SessionHandler.DealerGroup != null)
                {
                    viewModel.DealerGroup = SessionHandler.DealerGroup;

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list =
                    InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        .ToList()
                        .Where(i => i.Stock.Contains(searchString))
                        .Select(tmp => new CarInfoFormViewModel(tmp));


                //SET SORTING
                if (dealer.DealerSetting.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.DealerSetting.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.DealerSetting.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.DealerSetting.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.DealerSetting.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

               
                return View("ViewSmallInventory", viewModel);
            }
            else
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where e.Condition && e.Vehicle.Vin.Contains(searchString)
                    select e;


                var viewModel = new InventoryFormViewModel { IsCompactView = false };

                if (SessionHandler.DealerGroup != null)
                {
                    viewModel.DealerGroup = SessionHandler.DealerGroup;

                    viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
                }
                else
                    viewModel.DealerList = SelectListHelper.InitialDealerList();

                var list = avaiInventory.Select(tmp => new CarInfoFormViewModel(tmp)).ToList();


                //SET SORTING
                if (dealer.DealerSetting.InventorySorting.Equals("Year"))
                    viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
                else if (dealer.DealerSetting.InventorySorting.Equals("Make"))
                    viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
                else if (dealer.DealerSetting.InventorySorting.Equals("Model"))
                    viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
                else if (dealer.DealerSetting.InventorySorting.Equals("Age"))
                    viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
                else
                    viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

                viewModel.previousCriteria = dealer.DealerSetting.InventorySorting;

                viewModel.sortASCOrder = false;

                viewModel.CurrentOrSoldInventory = true;

                //SessionHandler.InventoryObject = viewModel;

                return View("ViewSmallInventory", viewModel);
            }
        }

        public ActionResult GetImages(int id, int status)
        {
            var result = GetImageResult(id, status);
            return Content(result);
        }

        public ActionResult GetImageLinks(int listingId, int inventoryStatus)
        {
            var result = GetImageResultForGallery(listingId, inventoryStatus);
            ViewData["result"] = result;
            return View("ImageLink");
        }

        private string GetImageResult(int id, int status)
        {
            var result = String.Empty;

            if (status == 1)
            {
                var content = _inventoryManagementForm.GetInventory(id).ThumbnailUrl;


                if (!String.IsNullOrEmpty(content))
                {
                    var list = CommonHelper.GetArrayFromString(content);
                    var index = 1;
                    foreach (var tmp in list)
                    {
                        result += " <li class=\"selector\">" + Environment.NewLine;
                        result += " <div class=\"centerImage\">" + Environment.NewLine;
                        string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                        result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage +
                                  "\">" +
                                  Environment.NewLine;
                        //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                        result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                  Environment.NewLine;

                        result += "</a>" + Environment.NewLine;
                        result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                  "\" name=\"image" +
                                  index++ + "\" />" + Environment.NewLine;

                        result += "</div>" + Environment.NewLine;
                        result += "</li>" + Environment.NewLine;
                    }

                    if (list.Length < 75)
                    {
                        var loopNumber = 75 - list.Length;


                        for (int i = 0; i < loopNumber; i++)
                        {
                            var urlHelper = new UrlHelper(Request.RequestContext);
                            string tmp = "";

                            if (i%3 == 0)
                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                            else if (i%3 == 1)
                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;

                            result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" +
                                      Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" +
                                      Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                      "\" name=\"image" +
                                      index++ + "\" />" + Environment.NewLine;

                            result += "</div>";
                            result += "</li>" + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    //int index = 1;

                    for (int i = 0; i < 75; i++)
                    {
                        var urlHelper = new UrlHelper(Request.RequestContext);
                        string tmp = "";

                        if (i%3 == 0)

                            tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                        else if (i%3 == 1)

                            tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                        else
                            tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                        result += " <li class=\"selector\">";
                        result += " <div class=\"centerImage\">";
                        result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                        //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                        result += "</div>";
                        result += "</li>" + Environment.NewLine;
                    }
                }
            }
            else if (status == -1)
            {
                var content = _inventoryManagementForm.GetSoldInventory(id).ThumbnailUrl;

                if (!String.IsNullOrEmpty(content))
                {
                    var list = CommonHelper.GetArrayFromString(content);
                    var index = 1;
                    foreach (var tmp in list)
                    {
                        result += " <li class=\"selector\">" + Environment.NewLine;
                        result += " <div class=\"centerImage\">" + Environment.NewLine;
                        string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                        result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage +
                                  "\">" +
                                  Environment.NewLine;
                        //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                        result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                  Environment.NewLine;

                        result += "</a>" + Environment.NewLine;
                        result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                  "\" name=\"image" +
                                  index++ + "\" />" + Environment.NewLine;

                        result += "</div>" + Environment.NewLine;
                        result += "</li>" + Environment.NewLine;
                    }

                    if (list.Length < 75)
                    {
                        var loopNumber = 75 - list.Length;


                        for (int i = 0; i < loopNumber; i++)
                        {
                            var urlHelper = new UrlHelper(Request.RequestContext);
                            string tmp = "";

                            if (i%3 == 0)
                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                            else if (i%3 == 1)
                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;

                            result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" +
                                      Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" +
                                      Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                      "\" name=\"image" +
                                      index++ + "\" />" + Environment.NewLine;

                            result += "</div>";
                            result += "</li>" + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    //int index = 1;

                    for (int i = 0; i < 75; i++)
                    {
                        var urlHelper = new UrlHelper(Request.RequestContext);
                        string tmp = "";

                        if (i%3 == 0)

                            tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                        else if (i%3 == 1)

                            tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                        else
                            tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                        result += " <li class=\"selector\">";
                        result += " <div class=\"centerImage\">";
                        result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                        //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                        result += "</div>";
                        result += "</li>" + Environment.NewLine;
                    }
                }
            }
            else
            {
                var content = _inventoryManagementForm.GetInventory(id).ThumbnailUrl;

                if (!String.IsNullOrEmpty(content))
                {
                    var list = CommonHelper.GetArrayFromString(content);
                    var index = 1;
                    foreach (var tmp in list)
                    {
                        result += " <li class=\"selector\">" + Environment.NewLine;
                        result += " <div class=\"centerImage\">" + Environment.NewLine;
                        string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                        result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage +
                                  "\">" +
                                  Environment.NewLine;
                        //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                        result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                  Environment.NewLine;

                        result += "</a>" + Environment.NewLine;
                        result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                  "\" name=\"image" +
                                  index++ + "\" />" + Environment.NewLine;

                        result += "</div>" + Environment.NewLine;
                        result += "</li>" + Environment.NewLine;
                    }

                    if (list.Length < 75)
                    {
                        var loopNumber = 75 - list.Length;


                        for (int i = 0; i < loopNumber; i++)
                        {
                            var urlHelper = new UrlHelper(Request.RequestContext);
                            string tmp = "";

                            if (i%3 == 0)
                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                            else if (i%3 == 1)
                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;

                            result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" +
                                      Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" +
                                      Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index +
                                      "\" name=\"image" +
                                      index++ + "\" />" + Environment.NewLine;

                            result += "</div>";
                            result += "</li>" + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    //int index = 1;

                    for (int i = 0; i < 75; i++)
                    {
                        var urlHelper = new UrlHelper(Request.RequestContext);
                        string tmp = "";

                        if (i%3 == 0)

                            tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                        else if (i%3 == 1)

                            tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                        else
                            tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                        result += " <li class=\"selector\">";
                        result += " <div class=\"centerImage\">";
                        result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                        //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                        result += "</div>";
                        result += "</li>" + Environment.NewLine;
                    }
                }
            }


            return result;
        }

        private string GetImageResultForGallery(int listingId, int inventoryStatus)
        {
            var result = String.Empty;


            switch (inventoryStatus)
            {
                case Constanst.InventoryStatus.SoldOut:
                    var soldOutInventory = _inventoryManagementForm.GetSoldInventory(listingId);


                    if (!String.IsNullOrEmpty(soldOutInventory.ThumbnailUrl))
                    {
                        var list = CommonHelper.GetArrayFromString(soldOutInventory.ThumbnailUrl);
                        result = list.Aggregate(result,
                            (current, tmp) =>
                                current +
                                ("<img src=\"" +
                                 tmp.Replace("ThumbnailSizeImages", "NormalSizeImages") +
                                 "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                 Environment.NewLine));

                    }
                    else
                    {
                        result = "<img src=\"" +
                                 (String.IsNullOrEmpty(soldOutInventory.Vehicle.DefaultStockImage)
                                     ? "http://vincontrol.com/alpha/no-images.jpg"
                                     : soldOutInventory.Vehicle.DefaultStockImage) +
                                 "\"width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                    }
                    break;
                default:
                    var inventory = _inventoryManagementForm.GetInventory(listingId);

                    if (!String.IsNullOrEmpty(inventory.ThumbnailUrl))
                    {
                        var list = CommonHelper.GetArrayFromString(inventory.ThumbnailUrl);
                        result = list.Aggregate(result,
                            (current, tmp) =>
                                current +
                                ("<img src=\"" +
                                 tmp.Replace("ThumbnailSizeImages", "NormalSizeImages") +
                                 "\" width=\"40\" height=\"40\" value=\"Upload\" />" +
                                 Environment.NewLine));

                    }
                    else
                    {
                        result = "<img src=\"" +
                                 (String.IsNullOrEmpty(inventory.Vehicle.DefaultStockImage)
                                     ? "http://vincontrol.com/alpha/no-images.jpg"
                                     : inventory.Vehicle.DefaultStockImage) +
                                 "\"width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                    }
                    break;

            }



            return result;
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult AdvancedSearch(string text)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var viewModel = new AdvancedSearchViewModel();
            var usedInventory = new List<AdvanceSearchItem>();
            var newInventory = new List<AdvanceSearchItem>();
            var loanerInventory = new List<AdvanceSearchItem>();
            var auctionInventory = new List<AdvanceSearchItem>();
            var reconInventory = new List<AdvanceSearchItem>();
            var wholesaleInventory = new List<AdvanceSearchItem>();
            var tradenotclearInventory = new List<AdvanceSearchItem>();
        
            var avaiInventory = new List<AdvanceSearchItem>();
            var allInventory = GetAllInventories();
            foreach (var item in allInventory)
            {
                if (item.IsUsedInventory())
                {
                    usedInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.Used));
                }
                else if (item.IsNewInventory())
                {
                    newInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.New));
                }
                else if (item.IsLoaner())
                {
                    loanerInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.Loaner));
                }
                else if (item.IsAuction())
                {
                    auctionInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.Auction));
                }
                else if (item.IsRecon())
                {
                    reconInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.Recon));
                }
                else if (item.IsWholeSale())
                {
                    wholesaleInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.Wholesale));
                }
                else if (item.IsTradeNotClear())
                {
                    tradenotclearInventory.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.TradeNotClear));
                }
            }
            //var usedInventory = GetUsedInventory();
            //var newInventory = GetNewInventory();

            SessionHandler.AdvanceSearchUsedInventory = usedInventory;
            SessionHandler.AdvanceSearchNewInventory = newInventory;
            //avaiInventory.AddRange(usedInventory);
            //avaiInventory.AddRange(newInventory);

            avaiInventory.AddRange(newInventory);
            avaiInventory.AddRange(usedInventory);
            if (SessionHandler.CurrentUser.RoleId != Constanst.RoleType.Employee)
            {
                avaiInventory.AddRange(loanerInventory);
                avaiInventory.AddRange(auctionInventory);
                avaiInventory.AddRange(reconInventory);
                avaiInventory.AddRange(wholesaleInventory);
                avaiInventory.AddRange(tradenotclearInventory);

                var soldInventory = GetSoldInventory().Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Soldout)).ToList();

                SessionHandler.AdvanceSearchLoanerInventory = loanerInventory;
                SessionHandler.AdvanceSearchAuctionInventory = auctionInventory;
                SessionHandler.AdvanceSearchReconInventory = reconInventory;
                SessionHandler.AdvanceSearchWholesaleInventory = wholesaleInventory;
                SessionHandler.AdvanceSearchTradeNotClearInventory = tradenotclearInventory;
                SessionHandler.AdvanceSearchSoldInventory = soldInventory;
               
                avaiInventory.AddRange(soldInventory);
            }
         

            var allAppraisal = GetAppraisal();
            var recentAppraisal = new List<AdvanceSearchItem>();
            var pendingAppraisal = new List<AdvanceSearchItem>();
            foreach (var item in allAppraisal)
            {
                if (item.IsPendingAppraisal())
                {
                    pendingAppraisal.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.PendingAppraisal));
                }
                else if (item.IsRecentAppraisal())
                {
                    recentAppraisal.Add(new AdvanceSearchItem(item, Constanst.AdvancedResultType.RecentAppraisal));
                }
            }

            //var recentAppraisal = GetRecentAppraisal();
            //var pendingAppraisal = GetPendingAppraisal();
            SessionHandler.AdvanceSearchRecentAppraisal = recentAppraisal;
            SessionHandler.AdvanceSearchPendingAppraisal = pendingAppraisal;
            avaiInventory.AddRange(recentAppraisal);
            avaiInventory.AddRange(pendingAppraisal);

            var year =
                avaiInventory.Where(i => i.Vehicle.Year != null)
                    .Select(i => i.Vehicle.Year.GetValueOrDefault())
                    .Distinct()
                    .OrderByDescending(i => i)
                    .ToList();
            viewModel.Years.AddRange(
                year.Select(
                    i =>
                        new System.Web.Mvc.SelectListItem
                        {
                            Selected = false,
                            Text = i.ToString(CultureInfo.InvariantCulture),
                            Value = i.ToString(CultureInfo.InvariantCulture)
                        }));

            viewModel.Text = text;

            return View("AdvancedSearch", viewModel);
        }

        private bool ReadyToSearch(AdvancedSearchViewModel model)
        {
            return (!string.IsNullOrEmpty(model.Text) ||
                    !string.IsNullOrEmpty(model.SelectedCategory) ||
                    !string.IsNullOrEmpty(model.SelectedYear) ||
                    !string.IsNullOrEmpty(model.SelectedMake) ||
                    !string.IsNullOrEmpty(model.SelectedModel));
        }

        [HttpPost]
        public ActionResult GenerateMakes(AdvancedSearchViewModel model)
        {
            model.Makes.Clear();
            if (string.IsNullOrEmpty(model.SelectedYear))
            {
                //model.Makes.Add(new SelectListItem() {Selected = true, Text = "----", Value = "" });
            }
            else
            {
                var avaiInventory = GetCachedSearchResult();

                var selectedYear = Convert.ToInt32(model.SelectedYear);
                var make =
                    avaiInventory.Where(
                        i => !String.IsNullOrEmpty(i.Vehicle.Make) && i.Vehicle.Year.GetValueOrDefault() == selectedYear)
                        .Select(i => i.Vehicle.Make)
                        .Distinct()
                        .OrderBy(i => i)
                        .ToList();
                model.Makes.AddRange(
                    make.Select(
                        i =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Selected = false,
                                Text = i.ToString(CultureInfo.InvariantCulture),
                                Value = i.ToString(CultureInfo.InvariantCulture)
                            }));
            }

            return PartialView(model);
        }

        private List<AdvanceSearchItem> GetCachedSearchResult()
        {
            var allInventory = new List<Inventory>();
            var allApraisal = new List<Appraisal>();

            if (SessionHandler.CurrentUser.RoleId != Constanst.RoleType.Employee)
            {
                if (SessionHandler.AdvanceSearchUsedInventory == null || SessionHandler.AdvanceSearchNewInventory == null ||
                    SessionHandler.AdvanceSearchLoanerInventory == null
                    || SessionHandler.AdvanceSearchAuctionInventory == null ||
                    SessionHandler.AdvanceSearchReconInventory == null ||
                    SessionHandler.AdvanceSearchWholesaleInventory == null ||
                    SessionHandler.AdvanceSearchTradeNotClearInventory == null)
                    allInventory = GetAllInventories();
            }
            else if (SessionHandler.AdvanceSearchUsedInventory == null || SessionHandler.AdvanceSearchNewInventory == null)
            {
                allInventory = GetAllInventories();
            }

            if (SessionHandler.AdvanceSearchPendingAppraisal == null || SessionHandler.AdvanceSearchRecentAppraisal == null)
                allApraisal = GetAppraisal();

            var avaiInventory = new List<AdvanceSearchItem>();
            var usedInventory = SessionHandler.AdvanceSearchUsedInventory ??
                                allInventory.Where(InventoryTypeCheckingHelper.IsUsedFunc)
                                    .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Used));

            var newInventory = SessionHandler.AdvanceSearchNewInventory ??
                               allInventory.Where(InventoryTypeCheckingHelper.IsNewFunc)
                                   .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.New));


            var recentAppraisal = SessionHandler.AdvanceSearchRecentAppraisal ??
                                  allApraisal.Where(InventoryTypeCheckingHelper.IsRecentAppraisalFunc)
                                      .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.RecentAppraisal));

            var pendingAppraisal = SessionHandler.AdvanceSearchPendingAppraisal ??
                                   allApraisal.Where(InventoryTypeCheckingHelper.IsPendingAppraisalFunc)
                                       .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.PendingAppraisal));

            avaiInventory.AddRange(usedInventory);
            avaiInventory.AddRange(newInventory);
            
            avaiInventory.AddRange(recentAppraisal);
            avaiInventory.AddRange(pendingAppraisal);

            if (SessionHandler.CurrentUser.RoleId != Constanst.RoleType.Employee)
            {

                var loanerInventory = SessionHandler.AdvanceSearchLoanerInventory ??
                                      allInventory.Where(InventoryTypeCheckingHelper.IsLoanerFunc)
                                          .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Loaner));

                var auctionInventory = SessionHandler.AdvanceSearchAuctionInventory ??
                                       allInventory.Where(InventoryTypeCheckingHelper.IsAuctionFunc)
                                           .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Auction));

                var reconInventory = SessionHandler.AdvanceSearchReconInventory ??
                                     allInventory.Where(InventoryTypeCheckingHelper.IsReconFunc)
                                         .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Recon));

                var wholesaleInventory = SessionHandler.AdvanceSearchWholesaleInventory ??
                                         allInventory.Where(InventoryTypeCheckingHelper.IsWholeSaleFunc)
                                             .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Wholesale));

                var tradenotclearInventory = SessionHandler.AdvanceSearchTradeNotClearInventory ??
                                             allInventory.Where(InventoryTypeCheckingHelper.IsTradeNotClearFunc)
                                                 .Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.TradeNotClear));

                var soldInventory = SessionHandler.AdvanceSearchSoldInventory ??
                                    GetSoldInventory().Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Soldout));

                avaiInventory.AddRange(loanerInventory);
                avaiInventory.AddRange(auctionInventory);
                avaiInventory.AddRange(reconInventory);
                avaiInventory.AddRange(wholesaleInventory);
                avaiInventory.AddRange(tradenotclearInventory);
                avaiInventory.AddRange(soldInventory);
            }
            return avaiInventory;
        }

        [HttpPost]
        public ActionResult GenerateModels(AdvancedSearchViewModel model)
        {
            model.Models.Clear();
            if (string.IsNullOrEmpty(model.SelectedMake))
            {
                //model.Models.Add(new SelectListItem() {Selected = true, Text = "----", Value = "" });
            }
            else
            {
                var dealer = SessionHandler.Dealer;
                var avaiInventory = GetCachedSearchResult();
                
                var selectedYear = Convert.ToInt32(model.SelectedYear);
                if (
                    avaiInventory.Any(
                        i =>
                            !String.IsNullOrEmpty(i.Vehicle.Make) &&
                            !String.IsNullOrEmpty(i.Vehicle.Model) &&
                            i.Vehicle.Make.ToLower() == model.SelectedMake.ToLower() &&
                            i.Vehicle.Year.GetValueOrDefault() == selectedYear && i.DealerId == dealer.DealershipId))
                {
                    var modelList =
                        avaiInventory.Where(
                            i =>
                                !String.IsNullOrEmpty(i.Vehicle.Make) &&
                                !String.IsNullOrEmpty(i.Vehicle.Model) &&
                                i.Vehicle.Make.ToLower() == model.SelectedMake.ToLower() &&
                                i.Vehicle.Year.GetValueOrDefault() == selectedYear && i.DealerId == dealer.DealershipId)
                            .Select(i => i.Vehicle.Model)
                            .Distinct()
                            .OrderBy(i => i)
                            .ToList();

                    model.Models.AddRange(
                        modelList.Select(
                            i =>
                                new System.Web.Mvc.SelectListItem
                                {
                                    Selected = false,
                                    Text = i.ToString(CultureInfo.InvariantCulture),
                                    Value = i.ToString(CultureInfo.InvariantCulture)
                                }));

                }
            }

            return PartialView(model);
        }

        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult AdvancedSearchResult(AdvancedSearchViewModel model)
        {
            var avaiInventory = new List<AdvanceSearchItem>();
            var listOfResults = new List<CarInfoFormViewModel>();
            TempData["HasMultipleCategory"] = 0;

            if (ReadyToSearch(model))
            {
                bool isScrolling = (model.PageIndex > 1);

                //try
                //{
                    if (!string.IsNullOrEmpty(model.SelectedCategory))
                    {
                        switch (model.SelectedCategory)
                        {
                            case "Used":
                                avaiInventory = SessionHandler.AdvanceSearchUsedInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchUsedInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsUsedFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Used)).ToList();
                                break;
                            case "New":
                                avaiInventory = SessionHandler.AdvanceSearchNewInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchNewInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsNewFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.New)).ToList();
                                break;
                            case "Loaner":
                                avaiInventory = SessionHandler.AdvanceSearchLoanerInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchLoanerInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsLoanerFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Loaner)).ToList();
                                break;
                            case "Auction":
                                avaiInventory = SessionHandler.AdvanceSearchAuctionInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchAuctionInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsAuctionFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Auction)).ToList();
                                break;
                            case "Recon":
                                avaiInventory = SessionHandler.AdvanceSearchReconInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchReconInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsReconFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Recon)).ToList();
                                break;
                            case "Wholesale":
                                avaiInventory = SessionHandler.AdvanceSearchWholesaleInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchWholesaleInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsWholeSaleFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Wholesale)).ToList();
                                break;
                            case "TradeNotClear":
                                avaiInventory = SessionHandler.AdvanceSearchTradeNotClearInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchTradeNotClearInventory
                                    : GetAllInventories(InventoryTypeCheckingHelper.IsTradeNotClearFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.TradeNotClear)).ToList();

                                break;
                            case "Sold":
                                avaiInventory = SessionHandler.AdvanceSearchSoldInventory != null && isScrolling
                                    ? SessionHandler.AdvanceSearchSoldInventory
                                    : GetSoldInventory().Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.Soldout)).ToList();
                                break;
                            case "Recent":
                                avaiInventory = SessionHandler.AdvanceSearchRecentAppraisal != null && isScrolling
                                    ? SessionHandler.AdvanceSearchRecentAppraisal
                                    : GetAppraisal(InventoryTypeCheckingHelper.IsRecentAppraisalFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.RecentAppraisal)).ToList();
                                break;
                            case "Pending":
                                avaiInventory = SessionHandler.AdvanceSearchPendingAppraisal != null && isScrolling
                                    ? SessionHandler.AdvanceSearchPendingAppraisal
                                    : GetAppraisal(InventoryTypeCheckingHelper.IsPendingAppraisalFunc).Select(i => new AdvanceSearchItem(i, Constanst.AdvancedResultType.PendingAppraisal)).ToList();
                                break;
                            default: // Get All Inventories
                                avaiInventory = GetCachedSearchResult();
                                break;
                        }
                    }
                //}
              /*  catch (Exception)
                {
                    return RedirectToAction("HttpError", "Error");
                }*/

                if (!string.IsNullOrEmpty(model.SelectedYear))
                    avaiInventory =
                        avaiInventory.Where(tmp => tmp.Vehicle.Year == int.Parse(model.SelectedYear)).ToList();

                if (!string.IsNullOrEmpty(model.SelectedMake))
                    avaiInventory = avaiInventory.Where(tmp => tmp.Vehicle.Make == model.SelectedMake).ToList();

                if (!string.IsNullOrEmpty(model.SelectedModel))
                    avaiInventory = avaiInventory.Where(tmp => tmp.Vehicle.Model == model.SelectedModel).ToList();

                if (!string.IsNullOrEmpty(model.Text))
                    avaiInventory =
                        avaiInventory.Where(
                            tmp =>
                                (!String.IsNullOrEmpty(tmp.Stock) && tmp.Stock.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.Vin) &&
                                 tmp.Vehicle.Vin.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.Model) &&
                                 tmp.Vehicle.Model.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.Make) &&
                                 tmp.Vehicle.Make.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.Trim) &&
                                 tmp.Vehicle.Trim.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.ExteriorColor) &&
                                 tmp.ExteriorColor.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.InteriorColor) &&
                                 tmp.Vehicle.InteriorColor.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.Tranmission) &&
                                 tmp.Vehicle.Tranmission.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Descriptions) &&
                                 tmp.Descriptions.ToLower().Contains(model.Text.ToLower())) ||
                                (!String.IsNullOrEmpty(tmp.Vehicle.StandardOptions) &&
                                 tmp.Vehicle.StandardOptions.ToLower().Contains(model.Text.ToLower())) ||
                                (tmp.Vehicle.Doors.GetValueOrDefault() > 0 &&
                                 tmp.Vehicle.Doors.ToString().ToLower().Contains(model.Text.ToLower()))).ToList();

                listOfResults = avaiInventory.ToList().Select(tmp => new CarInfoFormViewModel(tmp)
                {
                    Type = Convert.ToInt32(tmp.OldType)
                }).ToList();

                TempData["NumberOfUsedInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Used);
                TempData["NumberOfNewInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.New);
                TempData["NumberOfLoanerInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Loaner);
                TempData["NumberOfAuctionInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Auction);
                TempData["NumberOfReconInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Recon);
                TempData["NumberOfWholesaleInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Wholesale);
                TempData["NumberOfTradeNotClearInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.TradeNotClear);
                TempData["NumberOfSoldInventory"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Sold);
                TempData["NumberOfRecentAppraisal"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Recent);
                TempData["NumberOfPendingAppraisal"] = listOfResults.Count(i => i.Type == Constanst.CarInfoType.Pending);

                //Sorting
                var isAsc = Session["AdvancedSearch_IsAsc"] != null && (bool)Session["AdvancedSearch_IsAsc"];
                switch (model.SortField)
                {
                    case "Year":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.ModelYear).ToList()
                            : listOfResults.OrderByDescending(i => i.ModelYear).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Make":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.Make).ToList()
                            : listOfResults.OrderByDescending(i => i.Make).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Model":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.Model).ToList()
                            : listOfResults.OrderByDescending(i => i.Model).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Trim":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.Trim).ToList()
                            : listOfResults.OrderByDescending(i => i.Trim).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Stock":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.Stock).ToList()
                            : listOfResults.OrderByDescending(i => i.Stock).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Age":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.DaysInInvenotry).ToList()
                            : listOfResults.OrderByDescending(i => i.DaysInInvenotry).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Mile":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.Mileage).ToList()
                            : listOfResults.OrderByDescending(i => i.Mileage).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    case "Price":
                        listOfResults = isAsc
                            ? listOfResults.OrderBy(i => i.SalePrice).ToList()
                            : listOfResults.OrderByDescending(i => i.SalePrice).ToList();
                        Session["AdvancedSearch_IsAsc"] = !isAsc;
                        break;
                    default:
                        Session["AdvancedSearch_IsAsc"] = true;
                        break;
                }
            }

            string content = BuildInventory(listOfResults.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToList(), false);
            var te = new LargeJsonResult
            {
                Data = new
                {
                    success = true,
                    count = listOfResults.Count,
                    NumberOfUsedInventory = TempData["NumberOfUsedInventory"],
                    NumberOfNewInventory = TempData["NumberOfNewInventory"],
                    NumberOfLoanerInventory = TempData["NumberOfLoanerInventory"],
                    NumberOfAuctionInventory = TempData["NumberOfAuctionInventory"],
                    NumberOfReconInventory = TempData["NumberOfReconInventory"],
                    NumberOfWholesaleInventory = TempData["NumberOfWholesaleInventory"],
                    NumberOfTradeNotClearInventory = TempData["NumberOfTradeNotClearInventory"],
                    NumberOfSoldInventory = TempData["NumberOfSoldInventory"],
                    NumberOfRecentAppraisal = TempData["NumberOfRecentAppraisal"],
                    NumberOfPendingAppraisal = TempData["NumberOfPendingAppraisal"],
                    content
                }
            };
            return te;
            //return PartialView(listOfResults);
        }

        private ActionResult ViewSoldInventoryJson(bool? isUsed, string sortBy, bool isUp, int pageSize, string fromDate,
            string toDate, bool usingFilter, string make, string model, string trim, string year, int price,
            int fromMile, int toMile)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(Constanst.Message.Unauthorized);
            }


            DateTime dtFromDate = DataHelper.ParseDateTimeFromString(fromDate, true);
            DateTime dtToDate = DataHelper.ParseDateTimeFromString(toDate, false);

            IQueryable<SoldoutInventory> soldInventory;

            if (!SessionHandler.Single)
            {
                if (SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
                {
                    soldInventory =
                        _inventoryManagementForm.GetSoldInventoriesInDateRange(
                            SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId),
                            isUsed, dtFromDate,
                            dtToDate);
                }
                else
                {
                    soldInventory =
                        _inventoryManagementForm.GetSoldInventoriesInDateRange(SessionHandler.Dealer.DealershipId,
                            isUsed, dtFromDate,
                            dtToDate);
                }
            }
            else
            {
                soldInventory =
                    _inventoryManagementForm.GetSoldInventoriesInDateRange(SessionHandler.Dealer.DealershipId,
                        isUsed, dtFromDate,
                        dtToDate);
            }


            var list = (from tmp in soldInventory.ToList()
                select
                    new InventoryInfo(tmp)
                    {
                        Type = Constanst.CarInfoType.Sold
                    }).ToList();

            if (usingFilter == true)
            {
                list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
            }

            list = SortCacheInventory(list, sortBy, isUp);
            SessionHandler.InventoryList = list;

            list = list.Take(pageSize).ToList();

            SessionHandler.CurrentView = CurrentViewEnum.SoldInventory.ToString();
            ViewData["inventoryType"] = Constanst.InventoryTab.SoldCar;
            ViewData["isShowExpand"] = "true";
            ViewData["VehicleStatusCodeId"] = Constanst.VehicleStatus.SoldOut;

            return new LargeJsonResult
            {
                Data = new
                {
                    count = SessionHandler.InventoryList.Count,
                    list
                }
            };

        }

        public ActionResult ExpressBucketJump(bool? isShowExpand)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;

            var avaiInventory = _inventoryManagementForm.GetAllUsedInventories(dealer.DealershipId);

            var viewModel = new InventoryFormViewModel { IsCompactView = false };

            if (SessionHandler.DealerGroup != null)
            {
                viewModel.DealerGroup = SessionHandler.DealerGroup;

                viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
            }
            else
                viewModel.DealerList = SelectListHelper.InitialDealerList();

            var list =
                avaiInventory.ToList().Select(tmp => new CarInfoFormViewModel(tmp) { Type = Constanst.CarInfoType.Used });

            //SET SORTING
            if (dealer.DealerSetting.InventorySorting.Equals("Year"))
                viewModel.CarsList = list.OrderBy(x => x.ModelYear).ThenBy(x => x.Make).ToList();
            else if (dealer.DealerSetting.InventorySorting.Equals("Make"))
                viewModel.CarsList = list.OrderBy(x => x.Make).ThenBy(x => x.Model).ToList();
            else if (dealer.DealerSetting.InventorySorting.Equals("Model"))
                viewModel.CarsList = list.OrderBy(x => x.Model).ToList();
            else if (dealer.DealerSetting.InventorySorting.Equals("Age"))
                viewModel.CarsList = list.OrderBy(x => x.DaysInInvenotry).ToList();
            else
                viewModel.CarsList = list.OrderBy(x => x.Make).ToList();

            viewModel.previousCriteria = dealer.DealerSetting.InventorySorting;

            viewModel.sortASCOrder = false;

            viewModel.CurrentOrSoldInventory = true;

            ViewData["inventoryType"] = Constanst.InventoryTab.Used;
            SessionHandler.CurrentView = CurrentViewEnum.Inventory.ToString();
            ViewData["isShowExpand"] = "true";
            
            if (isShowExpand != null && isShowExpand.Value)
                return View("ViewSmallInventory", viewModel);
            return View("ViewSmallInventory", viewModel);
        }

        public ActionResult TodayBucketJump(bool? isShowExpand)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var dealer = SessionHandler.Dealer;
            var avaiInventory = _inventoryManagementForm.GetTodayBucketJumpInventories(dealer.DealershipId);

            var viewModel = new InventoryFormViewModel { IsCompactView = false };

            if (SessionHandler.DealerGroup != null)
            {
                viewModel.DealerGroup = SessionHandler.DealerGroup;

                viewModel.DealerList = SelectListHelper.InitialDealerList(viewModel.DealerGroup);
            }
            else
                viewModel.DealerList = SelectListHelper.InitialDealerList();

            var list = avaiInventory.Select(tmp => new CarInfoFormViewModel(tmp)
            {
                NotDoneBucketJump = true, Type = Constanst.CarInfoType.Used
            }).ToList();

            viewModel.CarsList = list.OrderByDescending(x => x.DaysInInvenotry).ToList();

            viewModel.CurrentOrSoldInventory = true;

            ViewData["inventoryType"] = Constanst.InventoryTab.Used;
            SessionHandler.CurrentView = CurrentViewEnum.Inventory.ToString();

            ViewData["isShowExpand"] = "true";
            if (isShowExpand != null && isShowExpand.Value)
                return View("ViewSmallInventory", viewModel);
            return View("ViewSmallInventory", viewModel);
        }

        private ActionResult ViewNewInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            if (SessionHandler.Dealer != null)
            {
                IQueryable<Inventory> avaiInventory;
                if (SessionHandler.AllStore)
                {
                    if (SessionHandler.DealerGroup != null && SessionHandler.IsMaster)
                    {
                        avaiInventory =
                          _inventoryManagementForm.GetAllNewInventories(SessionHandler.DealerGroup.DealerList.Select(x => x.DealershipId));
                    }
                    else
                    {
                        avaiInventory =
                        _inventoryManagementForm.GetAllNewInventories(SessionHandler.Dealer.DealershipId);
                    }
                }
                else
                {
                    avaiInventory =
                       _inventoryManagementForm.GetAllNewInventories(SessionHandler.Dealer.DealershipId);
                }

                var list =
                    avaiInventory.ToList()
                        .Select(tmp => new InventoryInfo(tmp) { Type = Constanst.CarInfoType.New }).ToList();

                if (usingFilter == true)
                {
                    list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
                }

                list = SortCacheInventory(list, sortBy, isUp);
                SessionHandler.InventoryList = list;

                list = list.Take(pageSize).ToList();

                SessionHandler.CurrentView = CurrentViewEnum.NewInventory.ToString();
                ViewData["inventoryType"] = Constanst.InventoryTab.New;
                ViewData["isShowExpand"] = "true";
                

                return new LargeJsonResult
                {
                    Data = new
                    {
                        count = SessionHandler.InventoryList.Count,
                        list
                    }
                };
            }
            return Json(Constanst.Message.Unauthorized);
        }

        private void ResetSessionValue()
        {
            SessionHandler.AutoTrader = null;
            SessionHandler.CarsCom = null;
            SessionHandler.AutoTraderNationwide = null;
            SessionHandler.CarsComNationwide = null;
            SessionHandler.ChromeTrimList = null;
        }

        public ActionResult Error()
        {
            return View();
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewIProfile(int listingId, string page = "Inventory")
        {
            ResetSessionValue();
            if (SessionHandler.Dealer == null) return RedirectToAction("LogOff", "Account");
            
            var dealer = SessionHandler.Dealer;
            var dealerGroup = SessionHandler.DealerGroup;
            var row = _inventoryManagementForm.GetInventory(listingId);

            if (row == null)
            {
                return RedirectToAction("ViewISoldProfile", new { listingId });
            }
            if ((row.DealerId != dealer.DealershipId && dealerGroup == null) ||
                (row.DealerId != dealer.DealershipId && dealerGroup != null &&
                 !dealerGroup.DealerList.Any(i => i.DealershipId.Equals(row.DealerId))))
            {

                return RedirectToAction("Error");
            }

                
                
            if (row.DealerId != dealer.DealershipId) SwitchDealerInGroup(row.DealerId);

            SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

            var profileIndex = GetProfileIndex(listingId, page);
            var viewModel = new CarInfoFormViewModel(row)
            {
                SavedSelections = new vincontrol.Application.ViewModels.Chart.ChartSelection(),
                PreviousListingId = profileIndex.Previous,
                NextListingId = profileIndex.Next,
                BodyType = String.IsNullOrEmpty(row.Vehicle.BodyType) ? "" : row.Vehicle.BodyType,
                DealerId = dealer.DealershipId,
                CurrentScreen = Constanst.ScreenType.InventoryScreen,
               
            };

            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model+ " " + viewModel.Trim;
              
            viewModel.MultipleDealers = SessionHandler.DealerGroup != null;

            viewModel.CarFaxDealerId = dealer.DealerSetting.CarFax;

            viewModel.NotDoneBucketJump = CheckToDayBucketJump(SessionHandler.Dealer, row);

            return View("iProfile", viewModel);
        }

        private bool CheckToDayBucketJump(DealershipViewModel dealer, Inventory tmp)
        {
            var dateatMidnight = new DateTime(tmp.DateInStock.GetValueOrDefault().Year,
                        tmp.DateInStock.GetValueOrDefault().Month,
                        tmp.DateInStock.GetValueOrDefault().Day);

            int daysInInventory = DateTime.Now.Subtract(dateatMidnight).Days;
            int bucketDay = tmp.BucketJumpCompleteDay.GetValueOrDefault();
            int nextBucketDay = 0;
            if (bucketDay == 0 || bucketDay < dealer.DealerSetting.FirstTimeRangeBucketJump)
                nextBucketDay = dealer.DealerSetting.FirstTimeRangeBucketJump;
            else if (bucketDay < dealer.DealerSetting.SecondTimeRangeBucketJump)
                nextBucketDay = dealer.DealerSetting.SecondTimeRangeBucketJump;
            else if (bucketDay >= dealer.DealerSetting.SecondTimeRangeBucketJump)
                nextBucketDay = bucketDay + dealer.DealerSetting.IntervalBucketJump;

            return nextBucketDay <= daysInInventory;
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewIProfileOnMobile(string token, int listingId)
        {
            ResetSessionValue();
            SessionHandler.Single = true;
            Session["IsEmployee"] = false;
            SessionHandler.CurrentUser = new UserRoleViewModel { Role = "Admin" };
            var dealerId = -1;

            var context = new VincontrolEntities();
            try
            {
                dealerId = Convert.ToInt32(vincontrol.Helper.EncryptionHelper.DecryptString(token).Split('|')[0]);
                var existingDealer = context.Dealers.FirstOrDefault(i => i.DealerId == dealerId);
                if (existingDealer != null)
                {
                    var dealerViewModel = new DealershipViewModel(existingDealer);
                    SessionHandler.Dealer = dealerViewModel;
                }

            }
            catch
            {
            }

            SessionHandler.MobileDealerId = dealerId;

            var row = context.Inventories.FirstOrDefault(x => x.InventoryId == listingId && x.DealerId == dealerId);
            if (row != null && row.DealerId != SessionHandler.Dealer.DealershipId)
                return RedirectToAction("Error");

            SessionHandler.CanViewBucketJumpReport = true;//SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;
            var profileIndex = GetProfileIndex(listingId);
            var viewModel = new CarInfoFormViewModel(row)
            {
                SavedSelections = new vincontrol.Application.ViewModels.Chart.ChartSelection(),
                PreviousListingId = profileIndex.Previous,
                NextListingId = profileIndex.Next,

                BodyType =
                    String.IsNullOrEmpty(row.Vehicle.BodyType) ? "" : row.Vehicle.BodyType,
                ButtonPermissions = SQLHelper.GetButtonList(dealerId, "Profile"),
                DealerId = dealerId,
                CurrentScreen = Constanst.ScreenType.InventoryScreen,
                InventoryStatus = Constanst.VehicleStatus.Inventory
            };


            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

            if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                viewModel.OrginalName += " " + viewModel.Trim;

            if (row.Condition == Constanst.ConditionStatus.Used)
            {
                viewModel.CarFax = _carFaxService.ConvertXmlToCarFaxModelAndSave(row.VehicleId, row.Vehicle.Vin,
                     SessionHandler.Dealer.DealerSetting.CarFax, SessionHandler.Dealer.DealerSetting.CarFaxPassword);

            }
            else
            {
                var carfax = new CarFaxViewModel { ReportList = new List<CarFaxWindowSticker>(), Success = false };
                viewModel.CarFax = carfax;

            }

            var existingChartSelection =
                context.ChartSelections.FirstOrDefault(
                    s =>
                        s.ListingId == listingId && s.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory);
            if (existingChartSelection != null)
            {
                existingChartSelection.IsAll = existingChartSelection.IsAll != null &&
                                               Convert.ToBoolean(existingChartSelection.IsAll);
                existingChartSelection.IsCarsCom = existingChartSelection.IsCarsCom != null &&
                                                   Convert.ToBoolean(existingChartSelection.IsCarsCom);
                existingChartSelection.IsCertified = existingChartSelection.IsCertified != null &&
                                                     Convert.ToBoolean(existingChartSelection.IsCertified);
                existingChartSelection.IsFranchise = existingChartSelection.IsFranchise != null &&
                                                     Convert.ToBoolean(existingChartSelection.IsFranchise);
                existingChartSelection.IsIndependant = existingChartSelection.IsIndependant != null &&
                                                       Convert.ToBoolean(existingChartSelection.IsIndependant);
                existingChartSelection.Options = existingChartSelection.Options != null &&
                                                 existingChartSelection.Options != "0"
                    ? existingChartSelection.Options.ToLower()
                    : "";
                existingChartSelection.Trims = existingChartSelection.Trims != null &&
                                               existingChartSelection.Trims != "0"
                    ? existingChartSelection.Trims.ToLower()
                    : "";
            }

            viewModel.MultipleDealers = SessionHandler.DealerGroup != null;

            viewModel.CarFaxDealerId = SessionHandler.Dealer.DealerSetting.CarFax;

            return View("iProfileOnMobile", viewModel);
        }

        public ActionResult ManheimDataOnMobile(string listingId, int inventoryStatus)
        {
            List<ManheimWholesaleViewModel> result;
            try
            {
                using (var context = new VincontrolEntities())
                {
                    int convertedListingId = Convert.ToInt32(listingId);
                    if (inventoryStatus == 1)
                    {
                        var row = context.Inventories.FirstOrDefault(x => x.InventoryId == convertedListingId);
                        var dealer = SessionHandler.Dealer;
                        var manheimCredential = VincontrolLinqHelper.GetManheimCredential(dealer.DealershipId);
                        result = manheimCredential != null
                            ? VincontrolLinqHelper.ManheimReport(row, manheimCredential.Manheim.Trim(),
                                manheimCredential.ManheimPassword.Trim())
                            : new List<ManheimWholesaleViewModel>();
                    }
                    else if (inventoryStatus == -1)
                    {
                        var row =
                            context.SoldoutInventories.FirstOrDefault(x => x.SoldoutInventoryId == convertedListingId);
                        var dealer = SessionHandler.Dealer;
                        var manheimCredential = VincontrolLinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = VincontrolLinqHelper.ManheimReportForSoldCars(row, manheimCredential.Manheim.Trim(),
                                manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                    else
                    {
                        var row = context.Inventories.FirstOrDefault(x => x.InventoryId == convertedListingId);
                        var dealer = SessionHandler.Dealer;
                        var manheimCredential = VincontrolLinqHelper.GetManheimCredential(dealer.DealershipId);
                        if (manheimCredential != null)
                            result = VincontrolLinqHelper.ManheimReportForWholesale(row, manheimCredential.Manheim.Trim(),
                                manheimCredential.ManheimPassword.Trim());
                        else
                            result = new List<ManheimWholesaleViewModel>();
                    }
                }

            }
            catch (Exception)
            {
                result = new List<ManheimWholesaleViewModel>();
            }

            return PartialView("ManheimData", result);
        }
      
        public ActionResult KarPowerDataOnMobile(int listingId, int inventoryStatus)
        {
            ViewData["LISTINGID"] = listingId;

            var kbbStatus = string.Empty;
            try
            {
                kbbStatus = System.Configuration.ConfigurationManager.AppSettings["KBBStatus"];
            }
            catch
            {
                // in case we forgot include KBBStatus in web.config
            }

            using (var context = new VincontrolEntities())
            {
                int convertedListingId = Convert.ToInt32(listingId);
                //var dealer = SessionHandler.Dealer;
                var setting =_dealerManagementForm.GetDealerSettingById(SessionHandler.MobileDealerId);

                CarInfoFormViewModel row = null;
                string returnedView = "KarPowerData";
                var type = Constanst.VehicleStatus.Inventory;

                if (inventoryStatus == 1)
                {
                    var existingInventory = _inventoryManagementForm.GetInventory(convertedListingId);
                    if (existingInventory != null)
                        row = new CarInfoFormViewModel
                        {
                            Vin = existingInventory.Vehicle.Vin,
                            Mileage = existingInventory.Mileage.GetValueOrDefault(),
                            KBBTrimId = existingInventory.Vehicle.KBBTrimId,
                            KBBOptionsId = existingInventory.Vehicle.KBBOptionsId,
                            Condition = existingInventory.Condition
                        };
                }
                else if (inventoryStatus == -1)
                {
                    var existingSoldOutInventory = _inventoryManagementForm.GetSoldInventory(convertedListingId);
                    if (existingSoldOutInventory != null)
                        row = new CarInfoFormViewModel
                        {
                            Vin = existingSoldOutInventory.Vehicle.Vin,
                            Mileage = existingSoldOutInventory.Mileage.GetValueOrDefault(),
                            KBBTrimId = existingSoldOutInventory.Vehicle.KBBTrimId,
                            KBBOptionsId = existingSoldOutInventory.Vehicle.KBBOptionsId,
                            Condition = existingSoldOutInventory.Condition
                        };

                    returnedView = "KarPowerDataSoldOut";
                    type = Constanst.VehicleStatus.SoldOut;
                }
                else
                {
                    var existingWholesaleInventory = _inventoryManagementForm.GetInventory(convertedListingId);
                    if (existingWholesaleInventory != null)
                        row = new CarInfoFormViewModel
                        {
                            Vin = existingWholesaleInventory.Vehicle.Vin,
                            Mileage = existingWholesaleInventory.Mileage.GetValueOrDefault(),
                            KBBTrimId = existingWholesaleInventory.Vehicle.KBBTrimId,
                            KBBOptionsId = existingWholesaleInventory.Vehicle.KBBOptionsId,
                            Condition = existingWholesaleInventory.Condition
                        };

                    returnedView = "KarPowerDataWholesale";
                    //type = Constanst.VehicleStatus.WholeSale;
                }

                {
                    List<SmallKarPowerViewModel> result = null;
                    if (setting != null)
                    {
                        try
                        {
                            if (context.KellyBlueBooks.Any(x => x.Vin == row.Vin && x.VehicleStatusCodeId == type))
                            {
                                var searchResult =
                                    context.KellyBlueBooks.Where(x => x.Vin == row.Vin && x.VehicleStatusCodeId == type)
                                        .OrderBy(x => x.DateStamp)
                                        .ToList();

                                result = searchResult.Select(tmp => new SmallKarPowerViewModel
                                {
                                    BaseWholesale = tmp.BaseWholeSale.GetValueOrDefault(),
                                    MileageAdjustment = tmp.MileageAdjustment.GetValueOrDefault(),
                                    SelectedTrimId = tmp.TrimId.GetValueOrDefault(),
                                    SelectedTrimName = tmp.Trim,
                                    Wholesale = tmp.WholeSale.GetValueOrDefault(),
                                    IsSelected =
                                        row != null &&
                                        (row.KBBTrimId.GetValueOrDefault() == 0 || (tmp.TrimId == row.KBBTrimId)),
                                }).ToList();

                                if (result.Count > 0 && !result.Any(i => i.IsSelected))
                                {
                                    foreach (var item in result)
                                    {
                                        item.IsSelected = true;
                                    }
                                }
                            }
                            else
                            {
                                var karpowerService = new KarPowerService();
                                if (row != null && (row.KBBTrimId == null || String.IsNullOrEmpty(row.KBBOptionsId)))
                                    result = karpowerService.Execute(row.VehicleId, row.Vin,
                                        row.Mileage.ToString(CultureInfo.InvariantCulture), DateTime.Now,
                                        setting.KellyBlueBook, setting.KellyPassword, type);
                                else
                                {
                                    if (row != null)
                                        result = karpowerService.Execute(row.VehicleId, row.Vin,
                                            row.Mileage.ToString(CultureInfo.InvariantCulture),
                                            row.KBBTrimId.GetValueOrDefault(), row.KBBOptionsId, DateTime.Now,
                                            setting.KellyBlueBook, setting.KellyPassword, type);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            result = new List<SmallKarPowerViewModel>();
                        }

                    }
                    else
                        result = new List<SmallKarPowerViewModel>();

                    ViewData["LISTINGID"] = convertedListingId;
                    return PartialView(returnedView, result);
                }

            }
        }

        [HttpPost]
        public string UpdateMarketUp(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                _inventoryManagementForm.UpdateMarketRange(listingId, Constanst.MarketRange.MarketUp);
                return "Updated successfully.";

            }
            catch (Exception ex)
            {
                return "This vehicle doesn't exist in inventory.";
            }
        }

        [HttpPost]
        public string UpdateMarketIn(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                _inventoryManagementForm.UpdateMarketRange(listingId, Constanst.MarketRange.MarketIn);
                return "Updated successfully.";

            }
            catch (Exception ex)
            {
                return "This vehicle doesn't exist in inventory.";
            }
        }

        [HttpPost]
        public string UpdateMarketDown(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);
                _inventoryManagementForm.UpdateMarketRange(listingId, Constanst.MarketRange.MarketDown);
                return "Updated successfully.";

            }
            catch (Exception ex)
            {
                return "This vehicle doesn't exist in inventory.";
            }
        }

        private ProfileIndex GetProfileIndex(int listingId, string page = "Inventory")
        {
            var profileIndex = new ProfileIndex { Previous = -1, Next = -1 };

            if (page.Equals("KPI"))
            {
                var viewModel = SessionHandler.KpiConditon > 0 ? SessionHandler.KpiConditionInventoryList.Select(i => new InventoryInfo(i)).ToList() : SessionHandler.KpiInventoryList.CarsList.Select(i => new InventoryInfo(i)).ToList();
                if (SessionHandler.KPIViewInfo.SortFieldName != null)
                {
                    var searchIndex = viewModel.FindIndex(x => x.ListingId == listingId);

                    if (searchIndex == viewModel.Count - 1)
                    {
                    }
                    else if (searchIndex <= viewModel.Count - 2)
                        profileIndex.Next = viewModel.ElementAt(searchIndex + 1).ListingId;

                    if (searchIndex == 0)
                    {
                    }
                    else if (searchIndex > 0 && searchIndex <= viewModel.Count)
                        profileIndex.Previous = viewModel.ElementAt(searchIndex - 1).ListingId;
                }
            }
            else
            {
                if (SessionHandler.InventoryList == null) return profileIndex;

                var viewModel = SessionHandler.InventoryList;
                if (SessionHandler.InventoryViewInfo.SortFieldName != null)
                {
                    var searchIndex = viewModel.FindIndex(x => x.ListingId == listingId);

                    if (searchIndex == viewModel.Count - 1)
                    {
                    }
                    else if (searchIndex <= viewModel.Count - 2)
                        profileIndex.Next = viewModel.ElementAt(searchIndex + 1).ListingId;

                    if (searchIndex == 0)
                    {
                    }
                    else if (searchIndex > 0 && searchIndex <= viewModel.Count)
                        profileIndex.Previous = viewModel.ElementAt(searchIndex - 1).ListingId;
                }
            }

            return profileIndex;
        }

        private static IEnumerable<InventoryInfo> SortList(IEnumerable<InventoryInfo> viewModel, string sortFieldName)
        {
            switch (sortFieldName)
            {
                case "year":
                    return viewModel.OrderBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "make":
                    return viewModel.OrderBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "model":
                    return viewModel.OrderBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "trim":
                    return viewModel.OrderBy(i => i.Trim).ThenBy(i => i.Stock);
                case "vin":
                    return viewModel.OrderBy(i => i.Vin).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "stock":
                    return viewModel.OrderBy(i => i.Stock);
                case "color":
                    return viewModel.OrderBy(i => i.ExteriorColor).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "market":
                    return viewModel.OrderByDescending(i => i.MarketRange).ThenBy(i => i.CarRanking).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "miles":
                    return viewModel.OrderBy(i => i.Mileage).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "price":
                    return viewModel.OrderBy(i => i.SalePrice).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "owners":
                    return viewModel.OrderBy(i => i.CarFaxOwner).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                case "age":
                    return viewModel.OrderBy(i => i.DaysInInvenotry).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
                default:
                    return viewModel.OrderBy(i => i.DaysInInvenotry).ThenBy(i => i.Year).ThenBy(i => i.Make).ThenBy(i => i.Model).ThenBy(i => i.Trim).ThenBy(i => i.Stock);
            }
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewISoldProfile(int listingId, string page = "Inventory")
        {

            ResetSessionValue();

            if (SessionHandler.Dealer == null) return RedirectToAction("LogOff", "Account");
            
         
            var dealer = SessionHandler.Dealer;
            var row = _inventoryManagementForm.GetSoldInventory(listingId);

            var profileIndex = GetProfileIndex(listingId, page);

            var viewModel = new CarInfoFormViewModel(row)
            {
                SavedSelections = new vincontrol.Application.ViewModels.Chart.ChartSelection(),
                Type = Constanst.CarInfoType.Sold,
                PreviousListingId = profileIndex.Previous,
                NextListingId = profileIndex.Next,
                DealerId = SessionHandler.Dealer.DealershipId,
                
            };

            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model + " " + viewModel.Trim;

            viewModel.CurrentScreen = Constanst.ScreenType.SoldoutScreen;

            viewModel.CarFaxDealerId = dealer.DealerSetting.CarFax;

            SessionHandler.CanViewBucketJumpReport = SessionHandler.Dealer.DealerSetting.CanViewBucketJumpReport;

            viewModel.CarFaxDealerId = dealer.DealerSetting.CarFax;

            viewModel.MultipleDealers = SessionHandler.DealerGroup != null;

            return View("iProfile", viewModel);
        }

        public ActionResult EditDescription(int listingId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            var context = new VincontrolEntities();

            var viewModel = new CarDescriptionModel
            {
                ListingId = listingId,
                DescriptionList = new List<DescriptionSentenceGroup>()
            };

            var descriptionList = context.Descriptions.ToList();

            foreach (var tmp in descriptionList.Select(x => x.Title).Distinct())
            {
                var description = new DescriptionSentenceGroup
                {
                    Title = tmp,
                    Sentences = new List<DesctiptionSentence>()
                };


                foreach (var des in descriptionList.Where(x => x.Title == description.Title))
                {


                    var detailDescript = new DesctiptionSentence
                    {
                        DescriptionSentence = des.DescriptionSentence,
                        YesNo = des.YesNo.GetValueOrDefault()
                    };

                    description.Sentences.Add(detailDescript);

                }

                viewModel.DescriptionList.Add(description);
            }

            Session["Description"] = viewModel.DescriptionList;


            return View("EditDescription", viewModel);
        }

        public ActionResult EditIProfile(int listingId)
        {
            ResetSessionValue();
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            
            var dealer = SessionHandler.Dealer;
            var dealerGroup = SessionHandler.DealerGroup;
            var setting = SessionHandler.Dealer.DealerSetting;
            var row = _inventoryManagementForm.GetInventory(listingId);
            var autoService = new ChromeAutoService();

            if (row != null)
            {
                if ((row.DealerId != dealer.DealershipId && dealerGroup == null) ||
                    (row.DealerId != dealer.DealershipId && dealerGroup != null &&
                     !dealerGroup.DealerList.Any(i => i.DealershipId.Equals(row.DealerId))))
                    return RedirectToAction("Error");
                if (row.DealerId != dealer.DealershipId) SwitchDealerInGroup(row.DealerId);

                if (row.IsManualDecode)
                {
                    var viewModel = InitCarInfoDataWhenVehicleIsNull(listingId, dealer, row, setting, autoService);
                    viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.Vehicle.TruckType);
                    viewModel.TruckCategoryList = _commonManagementForm.GetTruckCategoriesByType(row.Vehicle.TruckType);
                    viewModel.TruckClassList =
                        _commonManagementForm.GetTruckClasses(row.Vehicle.TruckClassId.GetValueOrDefault());
                    viewModel.VehicleTypeList =
                        SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType.GetValueOrDefault());
                    viewModel.InventoryStatus = Constanst.InventoryStatus.Inventory;
                    return View("EditProfile", viewModel);
                }
                else
                {
                    var vehicleInfo = autoService.GetVehicleInformationFromVin(row.Vehicle.Vin);

                    if (vehicleInfo != null && vehicleInfo.style != null && !row.IsManualDecode)
                    {
                        var viewModel = InitCarInfoDataWhenVehicleNotNull(row, setting, autoService, vehicleInfo);

                        if (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                            vehicleInfo.responseStatus.responseCode ==
                            ResponseStatusResponseCode.ConditionallySuccessful)
                            viewModel.VinDecodeSuccess = true;

                        viewModel.BodyType = String.IsNullOrEmpty(row.Vehicle.BodyType) ? "" : row.Vehicle.BodyType;
                        viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.Vehicle.TruckType);
                        viewModel.TruckCategoryList =
                            _commonManagementForm.GetTruckCategoriesByType(row.Vehicle.TruckType);
                        viewModel.TruckClassList =
                            _commonManagementForm.GetTruckClasses(row.Vehicle.TruckClassId.GetValueOrDefault());
                        viewModel.VehicleTypeList =
                            SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType.GetValueOrDefault());
                        viewModel.InventoryStatus = Constanst.InventoryStatus.Inventory;
                        return View("EditProfile", viewModel);
                    }
                    else
                    {
                        var viewModel = InitCarInfoDataWhenVehicleIsNull(listingId, dealer, row, setting, autoService);
                        viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.Vehicle.TruckType);
                        viewModel.TruckCategoryList =
                            _commonManagementForm.GetTruckCategoriesByType(row.Vehicle.TruckType);
                        viewModel.TruckClassList =
                            _commonManagementForm.GetTruckClasses(row.Vehicle.TruckClassId.GetValueOrDefault());
                        viewModel.VehicleTypeList =
                            SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType.GetValueOrDefault());
                        viewModel.InventoryStatus = Constanst.InventoryStatus.Inventory;
                        return View("EditProfile", viewModel);
                    }
                }
            }
            else
            {

                var soldOut = _inventoryManagementForm.GetSoldInventory(listingId);

                if (soldOut != null)
                {
                    var vehicleInfo = autoService.GetVehicleInformationFromVin(soldOut.Vehicle.Vin);
                    row = new Inventory(soldOut, Constanst.InventoryStatus.SoldOut);

                    if (vehicleInfo != null && vehicleInfo.style != null && !row.IsManualDecode)
                    {
                        var viewModel = InitCarInfoDataWhenVehicleNotNull(row, setting, autoService, vehicleInfo);

                        if (vehicleInfo.responseStatus.responseCode == ResponseStatusResponseCode.Successful ||
                            vehicleInfo.responseStatus.responseCode ==
                            ResponseStatusResponseCode.ConditionallySuccessful)
                            viewModel.VinDecodeSuccess = true;

                        viewModel.BodyType = String.IsNullOrEmpty(row.Vehicle.BodyType) ? "" : row.Vehicle.BodyType;
                        viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.Vehicle.TruckType);
                        viewModel.TruckCategoryList =
                            _commonManagementForm.GetTruckCategoriesByType(row.Vehicle.TruckType);
                        viewModel.TruckClassList =
                            _commonManagementForm.GetTruckClasses(row.Vehicle.TruckClassId.GetValueOrDefault());
                        viewModel.VehicleTypeList =
                            SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType.GetValueOrDefault());
                        viewModel.Type = Constanst.CarInfoType.Sold;
                        viewModel.ListingId = soldOut.SoldoutInventoryId;
                        viewModel.InventoryStatus = Constanst.InventoryStatus.SoldOut;
                        return View("EditProfile", viewModel);
                    }
                    else
                    {
                        var viewModel = InitCarInfoDataWhenVehicleIsNull(listingId, dealer, row, setting, autoService);
                        viewModel.Type = Constanst.CarInfoType.Sold;
                        viewModel.TruckTypeList = _commonManagementForm.GetTruckTypes(row.Vehicle.TruckType);
                        viewModel.TruckCategoryList =
                            _commonManagementForm.GetTruckCategoriesByType(row.Vehicle.TruckType);
                        viewModel.TruckClassList =
                            _commonManagementForm.GetTruckClasses(row.Vehicle.TruckClassId.GetValueOrDefault());
                        viewModel.VehicleTypeList =
                            SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType.GetValueOrDefault());
                        viewModel.ListingId = soldOut.SoldoutInventoryId;
                        viewModel.InventoryStatus = Constanst.InventoryStatus.SoldOut;
                        return View("EditProfile", viewModel);
                    }
                }

            }
            return null;
        }

        private static CarInfoFormViewModel InitCarInfoDataWhenVehicleIsNull(int listingId, DealershipViewModel dealer,
            Inventory row, DealerSettingViewModel setting, ChromeAutoService autoService)
        {
            var viewModel = new CarInfoFormViewModel(row);

            if (!String.IsNullOrEmpty(row.Vehicle.ChromeStyleId))
            {
                int chromeStyleId;
                Int32.TryParse(row.Vehicle.ChromeStyleId, out chromeStyleId);

                var styleArray = autoService.GetVehicleInformationFromStyleId(chromeStyleId);
                if (styleArray != null)
                {
                    int styleId;
                    if (Int32.TryParse(viewModel.ChromeStyleId, out styleId))
                    {
                        bool existed;
                        viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, viewModel.Trim,
                            styleArray.style,
                            styleId, out existed);
                        if (!existed)
                        {
                            viewModel.CusTrim = viewModel.Trim;
                        }
                    }
                    else if (!String.IsNullOrEmpty(viewModel.Trim))
                    {
                        bool existed;
                        viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, styleArray.style,
                            viewModel.Trim,
                            out existed);
                        if (!existed)
                        {
                            viewModel.CusTrim = viewModel.Trim;
                        }
                    }
                    else
                    {
                        viewModel.EditTrimList = SelectListHelper.InitalTrimList(styleArray.style);
                    }
                }

                viewModel.Title = row.AdditionalTitle ?? string.Empty;

                viewModel.ChromeFactoryPackageOptions =
                    SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

                viewModel.ChromeFactoryNonInstalledOptions =
                    SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

                viewModel.WarrantyInfo = row.WarrantyInfo.GetValueOrDefault();


                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

              
                viewModel.VehicleTypeList =
                    SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType != null
                                                               ? row.Vehicle.VehicleType.Value
                                                               : Constanst.VehicleType.Car);

                viewModel.BrandedTitle = row.BrandedTitle.GetValueOrDefault();

                viewModel.ListingId = Convert.ToInt32(listingId);

                viewModel.DealerId = dealer.DealershipId;

                viewModel.VehicleModel = row.Vehicle.Model ?? string.Empty;

                viewModel.SelectedExteriorColorValue = row.ExteriorColor ?? string.Empty;

                viewModel.SelectedExteriorColorCode = row.Vehicle.ColorCode ?? string.Empty;

                viewModel.SelectedInteriorColor = row.Vehicle.InteriorColor ?? string.Empty;

                viewModel.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(null,
                    viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

                viewModel.ChromeInteriorColorList = SelectListHelper.InitalInteriorColorList(null,
                    viewModel.SelectedInteriorColor);

                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;

                viewModel.CusInteriorColor = row.Vehicle.InteriorColor ?? string.Empty;

                viewModel.CusTrim = row.Vehicle.Trim ?? string.Empty;

                viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

                viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

                viewModel.ExistOptions = String.IsNullOrEmpty(row.AdditionalOptions)
                    ? null
                    : (from data in row.AdditionalOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       select data).ToList();

                viewModel.ExistPackages = String.IsNullOrEmpty(row.AdditionalPackages)
                    ? null
                    : (from data in
                           row.AdditionalPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       select data).ToList();

                viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
                viewModel.EnableAutoDescriptionSetting = setting.AutoDescription;
                viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
                viewModel.WarrantyTypes = VincontrolLinqHelper.GetWarrantyTypeList(SessionHandler.Dealer);
                viewModel.DateInStock = row.DateInStock;
            }
            else if (!String.IsNullOrEmpty(row.Vehicle.ChromeModelId))
            {
                int chromeModelId;
                Int32.TryParse(row.Vehicle.ChromeStyleId, out chromeModelId);
                var styleArray = autoService.GetStyles(Convert.ToInt32(chromeModelId));
                if (styleArray != null)
                 viewModel.EditTrimList = SelectListHelper.InitalTrimList(styleArray);

                viewModel.Title = row.AdditionalTitle ?? string.Empty;

                viewModel.ChromeFactoryPackageOptions =
                    SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

                viewModel.ChromeFactoryNonInstalledOptions =
                    SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

               
                viewModel.VehicleTypeList =
                    SelectListHelper.InitalVehicleTypeList(row.Vehicle.VehicleType != null
                                                               ? row.Vehicle.VehicleType.Value
                                                               : Constanst.VehicleType.Car);

                viewModel.ListingId = Convert.ToInt32(listingId);

                viewModel.DealerId = dealer.DealershipId;

                viewModel.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(null,
                    viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

                viewModel.ChromeInteriorColorList = SelectListHelper.InitalInteriorColorList(null,
                    viewModel.SelectedInteriorColor);

                viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

                viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

                viewModel.ExistOptions = String.IsNullOrEmpty(row.AdditionalOptions)
                    ? null
                    : (from data in row.AdditionalOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       select data).ToList();

                viewModel.ExistPackages = String.IsNullOrEmpty(row.AdditionalPackages)
                    ? null
                    : (from data in
                           row.AdditionalPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       select data).ToList();

                viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
                viewModel.EnableAutoDescriptionSetting = setting.AutoDescription;
                viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
                viewModel.WarrantyTypes = VincontrolLinqHelper.GetWarrantyTypeList(SessionHandler.Dealer);
                viewModel.DateInStock = row.DateInStock;

                viewModel.CusTrim = row.Vehicle.Trim ?? string.Empty;
            }
            else
            {
                viewModel.EditTrimList = SelectListHelper.InitalTrimList(new vincontrol.ChromeAutoService.AutomativeService.IdentifiedString[]{});

                viewModel.ChromeFactoryPackageOptions =
                    SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

                viewModel.ChromeFactoryNonInstalledOptions =
                    SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

                viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

                if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                    viewModel.OrginalName += " " + viewModel.Trim;

                viewModel.ChromeExteriorColorList = SelectListHelper.InitalExteriorColorList(null,
                    viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim());

                viewModel.ChromeInteriorColorList = SelectListHelper.InitalInteriorColorList(null,
                    viewModel.SelectedInteriorColor);

                viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

                viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

                viewModel.ExistOptions = String.IsNullOrEmpty(row.AdditionalOptions)
                    ? null
                    : (from data in row.AdditionalOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       select data).ToList();

                viewModel.ExistPackages = String.IsNullOrEmpty(row.AdditionalPackages)
                    ? null
                    : (from data in
                           row.AdditionalPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                       select data).ToList();

                viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
                viewModel.EnableAutoDescriptionSetting = setting.AutoDescription;
                viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
                viewModel.WarrantyTypes = VincontrolLinqHelper.GetWarrantyTypeList(SessionHandler.Dealer);
                viewModel.DateInStock = row.DateInStock;
            }

            return viewModel;
        }

        private static CarInfoFormViewModel InitCarInfoDataWhenVehicleNotNull(Inventory row,
            DealerSettingViewModel setting, ChromeAutoService autoService, VehicleDescription vehicleInfo)
        {
            VehicleDescription styleInfo=null;

           
            if (!String.IsNullOrEmpty(row.Vehicle.ChromeStyleId))
            {
                int chromeStyleId;
                Int32.TryParse(row.Vehicle.ChromeStyleId, out chromeStyleId);
                styleInfo = autoService.GetStyleInformationFromStyleId(chromeStyleId);
            }
            else
            {
                if (vehicleInfo.style != null)
                {
                    var element = vehicleInfo.style.FirstOrDefault(x => x.trim == row.Vehicle.Trim);
                    styleInfo =
                        autoService.GetStyleInformationFromStyleId(element != null
                            ? element.id
                            : vehicleInfo.style.First().id);
                }
            }

            var viewModel = ConvertHelper.GetVehicleInfoFromChromeDecodeWithStyleForEdit(vehicleInfo, styleInfo);
            CarInfoFormViewModel.UpdateCarInfoFormViewModel(viewModel, row);

            //get all the style and set the selected style
            int styleId;
            if (viewModel.ChromeStyleId != null && Int32.TryParse(viewModel.ChromeStyleId, out styleId))
            {
                bool existed;
                if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimListForMercedesBenz(viewModel, viewModel.Trim,
                        vehicleInfo.style, styleId, out existed);
                }
                else
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, viewModel.Trim,
                        vehicleInfo.style, styleId, out existed);
                }

                if (!existed)
                {
                    viewModel.CusTrim = viewModel.Trim;
                }
            }
            else if (!String.IsNullOrEmpty(viewModel.Trim))
            {
                bool existed;
                if (vehicleInfo.bestMakeName.Equals("Mercedes-Benz") && vehicleInfo.modelYear <= 2009)
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimListForMercedesBenz(viewModel, vehicleInfo.style,
                        viewModel.Trim, out existed);
                }
                else
                {
                    viewModel.EditTrimList = SelectListHelper.InitalTrimList(viewModel, vehicleInfo.style,
                        viewModel.Trim, out existed);
                }

                if (!existed)
                {
                    viewModel.CusTrim = viewModel.Trim;
                }
            }
            else
            {
                viewModel.EditTrimList = SelectListHelper.InitalTrimList(vehicleInfo.style);
            }

            viewModel.ChromeFactoryPackageOptions =
                SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryPackageOptions);

            viewModel.ChromeFactoryNonInstalledOptions =
                SelectListHelper.InitalFactoryPackagesOrOption(viewModel.FactoryNonInstalledOptions);

            viewModel.OrginalName = viewModel.ModelYear + " " + viewModel.Make + " " + viewModel.Model;

            if (!String.IsNullOrEmpty(viewModel.Trim) && !viewModel.Trim.Equals("NA"))
                viewModel.OrginalName += " " + viewModel.Trim;

            viewModel.ChromeExteriorColorList = viewModel.ExteriorColorList != null && viewModel.ExteriorColorList.Any()
                ? SelectListHelper.InitalExteriorColorList(viewModel.ExteriorColorList.ToArray(),
                    viewModel.SelectedExteriorColorCode, viewModel.SelectedExteriorColorValue.Trim())
                : SelectListHelper.InitalExteriorColorList(null, viewModel.SelectedExteriorColorCode,
                    viewModel.SelectedExteriorColorValue.Trim());

            if (viewModel.ChromeExteriorColorList.Any(x => x.Selected))
            {
                viewModel.SelectedExteriorColorCode = viewModel.ChromeExteriorColorList.First(x => x.Selected).Value;
            }


            viewModel.ChromeInteriorColorList = viewModel.InteriorColorList != null && viewModel.InteriorColorList.Any()
                ? SelectListHelper.InitalInteriorColorList(viewModel.InteriorColorList.ToArray(),
                    viewModel.SelectedInteriorColor)
                : SelectListHelper.InitalInteriorColorList(null, viewModel.SelectedInteriorColor);

            if (!String.IsNullOrEmpty(viewModel.SelectedExteriorColorCode))
            {
                if (viewModel.ExteriorColorList != null && viewModel.ExteriorColorList.Any())
                {
                    var list =
                        viewModel.ExteriorColorList.Where(
                            t => t.colorName.Equals(viewModel.SelectedExteriorColorValue.Trim()));

                    if (!list.Any())
                    {
                        viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;
                    }
                    else
                        viewModel.CusExteriorColor = string.Empty;
                }
                else
                    viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;
            }
            else
            {
                viewModel.CusExteriorColor = row.ExteriorColor ?? string.Empty;
            }

            if (viewModel.InteriorColorList != null && viewModel.InteriorColorList.Any())
            {
                var list = viewModel.InteriorColorList.Where(t => t.colorName.Equals(viewModel.SelectedInteriorColor));

                if (!list.Any())
                {
                    viewModel.CusInteriorColor = row.Vehicle.InteriorColor ?? string.Empty;

                }
                else
                    viewModel.CusInteriorColor = string.Empty;
            }
            else
                viewModel.CusInteriorColor = row.Vehicle.InteriorColor ?? string.Empty;

            viewModel.ChromeTranmissionList = SelectListHelper.InitialEditTranmmissionList(viewModel.Tranmission);

            viewModel.ChromeDriveTrainList = SelectListHelper.InitalEditDriveTrainList(viewModel.WheelDrive);

            viewModel.IsManual = row.EnableAutoDescription != null && !row.EnableAutoDescription.GetValueOrDefault();
            viewModel.EnableAutoDescriptionSetting = setting.AutoDescription;
            viewModel.MarketRange = row.MarketRange.GetValueOrDefault();
            viewModel.WarrantyTypes = VincontrolLinqHelper.GetWarrantyTypeList(SessionHandler.Dealer);
            viewModel.DateInStock = row.DateInStock;


            viewModel.ExistPackages = String.IsNullOrEmpty(row.AdditionalPackages)
                ? null
                : (from data in row.AdditionalPackages.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                   select data).ToList();


            viewModel.ExistOptions = String.IsNullOrEmpty(row.AdditionalOptions)
                ? null
                : (from data in row.AdditionalOptions.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries)
                   select data).ToList();

            if (row.Vehicle.Msrp != null && row.Vehicle.Msrp > 0)
                viewModel.Msrp = row.Vehicle.Msrp.Value;

            viewModel.Cylinder = row.Vehicle.Cylinders != null ? row.Vehicle.Cylinders.Value : 0;
            viewModel.Litter = row.Vehicle.Litter != null ? row.Vehicle.Litter.Value : 0;
            viewModel.Door = row.Vehicle.Doors != null ? row.Vehicle.Doors.Value : 0;
            viewModel.Fuel = row.Vehicle.FuelType;

            viewModel.Description = row.Descriptions;

            viewModel.SelectedTrimItem = null;
            return viewModel;
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SaveIProfile(FormCollection form, CarInfoFormViewModel car)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            car.Trim = car.SelectedTrimItem;

            var dealer = SessionHandler.Dealer;

            //var additionalPackages = string.Empty;

            //var additionalOptions = string.Empty;

            //var additionaloptionCodes = string.Empty;

            //if (!String.IsNullOrEmpty(car.AfterSelectedPackage))
            //    additionalPackages = car.AfterSelectedPackage.Substring(0, car.AfterSelectedPackage.Length - 1);

            //if (!String.IsNullOrEmpty(car.AfterSelectedOptions))
            //    additionalOptions = car.AfterSelectedOptions.Substring(0, car.AfterSelectedOptions.Length - 1);

          
            decimal finalMsrp = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["MSRP"]));

            decimal retailPrice = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["RetailPrice"]));

            decimal discountPrice = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["DealerDiscount"]));

            decimal manufacturerRebate = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["ManufacturerRebate"]));

            decimal windowStickerPrice = Convert.ToDecimal(CommonHelper.RemoveSpecialCharactersForMsrp(form["WindowStickerPrice"]));

            long mileage = Convert.ToInt64(CommonHelper.RemoveSpecialCharactersForMsrp(form["Mileage"]));

            if ((!String.IsNullOrEmpty(car.SelectedExteriorColorValue) &&
                 car.SelectedExteriorColorValue.Trim().Equals("Other Colors")) ||
                (!String.IsNullOrEmpty(car.SelectedExteriorColorCode) &&
                 car.SelectedExteriorColorCode.Trim().Equals("Other Colors")))
                car.SelectedExteriorColorValue = car.CusExteriorColor;


            if (car.SelectedInteriorColor.Equals("Other Colors"))
                car.SelectedInteriorColor = car.CusInteriorColor;

            car.Mileage = mileage;
            car.RetailPrice = retailPrice;
            car.DealerDiscount = discountPrice;
            car.ManufacturerRebate = manufacturerRebate;
            car.WindowStickerPrice = windowStickerPrice;
            car.Msrp = finalMsrp;
           

            SQLHelper.UpdateIProfile(car, dealer);
            return RedirectToAction("ViewIProfile", new { ListingID = car.ListingId });
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CancelIProfile(CarInfoFormViewModel car)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            return RedirectToAction("ViewIProfile", "Inventory", new { car.ListingId });

        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        public ActionResult ViewIProfileByVinInDistance(string distance)
        {

            var dealer = SessionHandler.Dealer;

            string sessionUniqueName = dealer.DealershipId + "SessionDistance100";

            var viewModel = (CarInfoFormViewModel)Session[sessionUniqueName];

            viewModel.CurrentDistance = distance;

            return View("iProfile", viewModel);
        }

        public void UpdateAutoDescriptionStatus(int listingId, bool status)
        {
            _inventoryManagementForm.UpdateAutoDescriptionStatus(listingId, status);
            
        }

        public ActionResult GetGenerateAutoDescription(int listingId)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json("SessionTimeOut");
            }
            if (Request.IsAjaxRequest())
            {
                return Json(GenerateAutoDescription(listingId));
            }
            return Json(" NOT UPDATED ");
        }

        public string GenerateAutoDescription(int listingId)
        {
            var autoDescription = new AutoDescription();
            var dealer = SessionHandler.Dealer;

            
            var inventory = _inventoryManagementForm.GetInventory(listingId);

            if (autoDescription.AllowAutoDescription(inventory, dealer.DealershipId))
            {
                var result = autoDescription.GenerateAutoDescription(inventory, dealer);
                return result;

            }

            return "";
        }

        public ActionResult SaveDescription(int listingId, string description, string optionSelect, string startEnd)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json("SessionTimeOut");

            }
            var dealer = SessionHandler.Dealer;

            var finalDescription = "";

            if (!string.IsNullOrEmpty(startEnd))
            {
                var row = _dealerManagementForm.GetDealerSettingById(dealer.DealershipId);

                if (row != null)
                {
                    var startSentence = String.IsNullOrEmpty(row.StartDescriptionSentence)
                        ? ""
                        : row.StartDescriptionSentence;

                    var endSentence = String.IsNullOrEmpty(row.EndDescriptionSentence)
                        ? ""
                        : row.EndDescriptionSentence;

                    if (startEnd.Equals("Start"))
                    {
                        finalDescription = row.StartDescriptionSentence + description;



                        if (!string.IsNullOrEmpty(optionSelect))
                        {

                            finalDescription = startSentence + " " + description + " " +
                                               "This vehicle is equipped with " +
                                               optionSelect.Substring(0, optionSelect.Length - 1) + ".";

                        }

                    }
                    else if (startEnd.Equals("End"))
                    {
                        finalDescription = description + row.EndDescriptionSentence;

                        if (!string.IsNullOrEmpty(optionSelect))
                        {

                            finalDescription = description + " " +
                                               "This vehicle is equipped with " +
                                               optionSelect.Substring(0, optionSelect.Length - 1) + ". " + endSentence;

                        }
                    }
                    else if (startEnd.Equals("StartEnd"))
                    {
                        finalDescription = row.StartDescriptionSentence + description + row.EndDescriptionSentence;

                        if (!string.IsNullOrEmpty(optionSelect))
                        {

                            finalDescription = startSentence + " " + description + " " +
                                               "This vehicle is equipped with " +
                                               optionSelect.Substring(0, optionSelect.Length - 1) + ". " + endSentence;

                        }
                    }
                }
            }
            else
            {
                finalDescription = description;

                if (!string.IsNullOrEmpty(optionSelect))
                {

                    finalDescription = description +
                                       "This vehicle is equipped with " +
                                       optionSelect.Substring(0, optionSelect.Length - 1) + ".";

                }
            }

            _inventoryManagementForm.UpdateDescription(listingId, description);
            _vehicleLogForm.AddVehicleLog(
                listingId, SessionHandler.CurrentUser.UserId,
                  Constanst.VehicleLogSentence.DescriptionChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName), null);

            if (Request.IsAjaxRequest())
            {
                return Json(finalDescription);

            }
            return Json(listingId + " NOT UPDATED ");



        }

        public ActionResult GetDealerAuctionDescription()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json("SessionTimeOut");

            }
            var dealer = SessionHandler.Dealer;

            if (Request.IsAjaxRequest())
            {
                return Json(dealer.DealerSetting.AuctionSentence);

            }
            return Json(" NOT UPDATED ");



        }

        public ActionResult GetDealerLoanerDescription()
        {
            if (SessionHandler.Dealer == null)
            {
                return Json("SessionTimeOut");

            }
            var dealer = SessionHandler.Dealer;

            if (Request.IsAjaxRequest())
            {
                return Json(dealer.DealerSetting.LoanerSentence);

            }
            return Json(" NOT UPDATED ");



        }

        public ActionResult SaveKbbOptions(int listingId, string optionSelect, int trimId, decimal baseWholeSale,
            decimal wholeSale, decimal mileageAdjustment)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json("SessionTimeOut");

            }
            SQLHelper.UpdateKBBOptions(listingId, optionSelect, trimId, baseWholeSale, wholeSale, mileageAdjustment);


            if (Request.IsAjaxRequest())
            {
                return Json("Success");

            }
            return Json(listingId + " NOT UPDATED ");



        }

        public ActionResult UpdateAcv(int listingId, string acv)
        {
            acv = CommonHelper.RemoveSpecialCharactersForPurePrice(acv);

            decimal newacv = 0;
            Decimal.TryParse(acv, out newacv);

            var inventory = _inventoryManagementForm.GetInventory(listingId);
            if (inventory != null)
            {
                var currentAcv = inventory.ACV.GetValueOrDefault();
                _inventoryManagementForm.UpdateAcv(listingId, newacv);
                if (currentAcv != newacv)
                {

                    _vehicleLogForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                         Constanst.VehicleLogSentence.AcvChangeByUser.Replace("OLDPRICE", currentAcv.ToString("C0"))
                            .Replace("NEWPRICE", newacv.ToString("C0"))
                            .Replace("USER", SessionHandler.CurrentUser.FullName),
                        null);

                    var emailWaitingList = new EmailWaitingList()
                    {
                        ListingId = inventory.InventoryId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.AcvChange,
                        OldValue = currentAcv,
                        NewValue = newacv,
                        UserId = SessionHandler.CurrentUser.UserId

                    };
                    _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);

                }

            }
            else
            {
                var soldInventory = _inventoryManagementForm.GetSoldInventory(listingId);
                if (soldInventory != null)
                {
                    var currentAcv = soldInventory.ACV.GetValueOrDefault();
                    _inventoryManagementForm.UpdateSoldDealerCost(soldInventory.SoldoutInventoryId, newacv);
                    if (currentAcv != newacv)
                    {

                        _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                             Constanst.VehicleLogSentence.AcvChangeByUser.Replace("OLDPRICE", currentAcv.ToString("C0"))
                                .Replace("NEWPRICE", newacv.ToString("C0"))
                                .Replace("USER", SessionHandler.CurrentUser.FullName),
                            soldInventory.SoldoutInventoryId);

                    }

                }
            }

            return Request.IsAjaxRequest() ? Json(listingId + "Success") : Json(listingId + " NOT UPDATED " + newacv.ToString("#,##0"));
        }

        public ActionResult UpdateDealerCost(int listingId, string dealerCost)
        {
            dealerCost = CommonHelper.RemoveSpecialCharactersForPurePrice(dealerCost);
         
            decimal newdealerCost = 0;
            Decimal.TryParse(dealerCost, out newdealerCost);

            var inventory = _inventoryManagementForm.GetInventory(listingId);
            if (inventory != null)
            {
                var currentDealerCost = inventory.DealerCost.GetValueOrDefault();
                _inventoryManagementForm.UpdateDealerCost(listingId, newdealerCost);
                if (currentDealerCost != newdealerCost)
                {

                    _vehicleLogForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                         Constanst.VehicleLogSentence.DealerCostChangeByUser.Replace("OLDPRICE", currentDealerCost.ToString("C0"))
                            .Replace("NEWPRICE", newdealerCost.ToString("C0"))
                            .Replace("USER", SessionHandler.CurrentUser.FullName),
                        null);

                    var emailWaitingList = new EmailWaitingList()
                    {
                        ListingId = inventory.InventoryId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.DealerCostChange,
                        OldValue = currentDealerCost,
                        NewValue = newdealerCost,
                        UserId = SessionHandler.CurrentUser.UserId

                    };
                    _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);

                }

            }
            else
            {
                var soldInventory = _inventoryManagementForm.GetSoldInventory(listingId);
                if (soldInventory != null)
                {
                    var currentDealerCost = soldInventory.DealerCost.GetValueOrDefault();
                    _inventoryManagementForm.UpdateSoldDealerCost(soldInventory.SoldoutInventoryId, newdealerCost);
                    if (currentDealerCost != newdealerCost)
                    {

                        _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                             Constanst.VehicleLogSentence.DealerCostChangeByUser.Replace("OLDPRICE", currentDealerCost.ToString("C0"))
                                .Replace("NEWPRICE", newdealerCost.ToString("C0"))
                                .Replace("USER", SessionHandler.CurrentUser.FullName),
                            soldInventory.SoldoutInventoryId);

                    }

                }
            }

            return Request.IsAjaxRequest() ? Json(listingId + "Success") : Json(listingId + " NOT UPDATED " + newdealerCost.ToString("#,##0"));
        }

        public ActionResult UpdateMsrp(int listingId, string msrp)
        {
            msrp = CommonHelper.RemoveSpecialCharactersForPurePrice(msrp);
            decimal? msrpNumber = null;
            if (!string.IsNullOrEmpty(msrp))
                msrpNumber = Convert.ToDecimal(msrp);

            SQLHelper.UpdateMsrp(listingId, msrpNumber);

            double priceFormat;

            bool flag = Double.TryParse(msrp, out priceFormat);

            if (flag)
                msrp = priceFormat.ToString("#,##0");

            if (Request.IsAjaxRequest())
            {
                return Json(listingId + "Success");

            }
            return Json(listingId + " NOT UPDATED " + msrp);

        }

        public ActionResult UpdateDealerDiscount(int listingId, string discount)
        {
            discount = CommonHelper.RemoveSpecialCharactersForPurePrice(discount);
            decimal? discountNumber = null;
            if (!string.IsNullOrEmpty(discount))
                discountNumber = Convert.ToDecimal(discount);

            SQLHelper.UpdateDealerDiscount(listingId, discountNumber);

            double priceFormat;

            bool flag = Double.TryParse(discount, out priceFormat);

            if (flag)
                discount = priceFormat.ToString("#,##0");

            if (Request.IsAjaxRequest())
            {
                return Json(listingId + "Success");


            }
            return Json(listingId + " NOT UPDATED " + discount);

        }

        public ActionResult UpdateSalePrice(int listingId, string salePrice, short vehicleStatusCodeId)
        {
            salePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(salePrice);

            decimal newPrice = 0;
            decimal.TryParse(salePrice, out newPrice);

            if (vehicleStatusCodeId.Equals(Constanst.VehicleStatus.Inventory))
            {
                var inventory = _inventoryManagementForm.GetInventory(listingId);
                if (inventory != null)
                {
                    var currentSalePrice = inventory.SalePrice.GetValueOrDefault();
                    _inventoryManagementForm.UpdateSalePrice(listingId, newPrice);
                    if (currentSalePrice != newPrice)
                    {
                        if (currentSalePrice > 0)

                            _vehicleLogForm.AddVehicleLog(inventory.InventoryId, SessionHandler.CurrentUser.UserId,
                             Constanst.VehicleLogSentence.PriceChangeByUser.Replace("OLDPRICE", currentSalePrice.ToString("C0"))
                                .Replace("NEWPRICE", newPrice.ToString("C0"))
                                .Replace("USER", SessionHandler.CurrentUser.FullName),
                            null);

                        else
                        {
                            _vehicleLogForm.AddVehicleLog(inventory.InventoryId, SessionHandler.CurrentUser.UserId,
                          Constanst.VehicleLogSentence.PriceChangeFromZeroByUser.Replace("NEWPRICE", newPrice.ToString("C0"))
                             .Replace("USER", SessionHandler.CurrentUser.FullName),
                         null);

                        }

                        var emailWaitingList = new EmailWaitingList()
                        {
                            ListingId = inventory.InventoryId,
                            DateStamp = DateTime.Now,
                            Expire = false,
                            NotificationTypeCodeId = Constanst.NotificationType.SalePriceChange,
                            OldValue = currentSalePrice,
                            NewValue = newPrice,
                            UserId = SessionHandler.CurrentUser.UserId

                        };
                        var newPriceChangeHistory = new vincontrol.Data.Model.PriceChangeHistory
                        {
                            DateStamp = DateTime.Now,
                            ListingId = inventory.InventoryId,
                            OldPrice = currentSalePrice,
                            NewPrice = newPrice,
                            UserStamp = SessionHandler.CurrentUser.Username,
                            AttachFile = string.Empty,
                            VehicleStatusCodeId = vehicleStatusCodeId
                        };

                        _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);
                        _inventoryManagementForm.AddPriceChangeHistory(newPriceChangeHistory);

                    }

                }
            } else if (vehicleStatusCodeId.Equals(Constanst.VehicleStatus.SoldOut))
            {
                var soldInventory = _inventoryManagementForm.GetSoldInventory(listingId);
                if (soldInventory != null)
                {
                    var currentSalePrice = soldInventory.SalePrice.GetValueOrDefault();
                    _inventoryManagementForm.UpdateSoldSalePrice(soldInventory.SoldoutInventoryId, newPrice);
                    if (currentSalePrice != newPrice)
                    {

                        if (currentSalePrice > 0)

                            _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                              Constanst.VehicleLogSentence.PriceChangeByUser.Replace("OLDPRICE", currentSalePrice.ToString("C0"))
                                 .Replace("NEWPRICE", newPrice.ToString("C0"))
                                 .Replace("USER", SessionHandler.CurrentUser.FullName),
                             soldInventory.SoldoutInventoryId);

                        else
                        {
                            _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                           Constanst.VehicleLogSentence.PriceChangeFromZeroByUser.Replace("NEWPRICE", newPrice.ToString("C0"))
                              .Replace("USER", SessionHandler.CurrentUser.FullName),
                          soldInventory.SoldoutInventoryId);

                        }
                        var newPriceChangeHistory = new vincontrol.Data.Model.PriceChangeHistory
                        {
                            DateStamp = DateTime.Now,
                            ListingId = soldInventory.SoldoutInventoryId,
                            OldPrice = currentSalePrice,
                            NewPrice = newPrice,
                            UserStamp = SessionHandler.CurrentUser.Username,
                            AttachFile = string.Empty,
                            VehicleStatusCodeId = vehicleStatusCodeId
                        };
                        _inventoryManagementForm.AddPriceChangeHistory(newPriceChangeHistory);
                    }

                }
            }
            

            if (Request.IsAjaxRequest())
            {
                return Json(listingId + "Success" + newPrice);

            }
            return Json(listingId + " NOT UPDATED " + newPrice);

        }

        public ActionResult UpdateSalePriceFromInventoryPage(string salePrice, int listingId,short vehicleStatusCodeId = Constanst.VehicleStatus.Inventory)
        {

            salePrice = CommonHelper.RemoveSpecialCharactersForPurePrice(salePrice);

            decimal newPrice = 0;
            decimal.TryParse(salePrice, out newPrice);

            var inventory = _inventoryManagementForm.GetInventory(listingId);
            if (inventory != null)
            {
                var currentSalePrice = inventory.SalePrice.GetValueOrDefault();
                _inventoryManagementForm.UpdateSalePrice(listingId, newPrice);
                if (currentSalePrice != newPrice)
                {
                    if (currentSalePrice > 0)

                        _vehicleLogForm.AddVehicleLog(inventory.InventoryId, SessionHandler.CurrentUser.UserId,
                         Constanst.VehicleLogSentence.PriceChangeByUser.Replace("OLDPRICE", currentSalePrice.ToString("C0"))
                            .Replace("NEWPRICE", newPrice.ToString("C0"))
                            .Replace("USER", SessionHandler.CurrentUser.FullName),
                        null);

                    else
                    {
                        _vehicleLogForm.AddVehicleLog(inventory.InventoryId, SessionHandler.CurrentUser.UserId,
                      Constanst.VehicleLogSentence.PriceChangeFromZeroByUser.Replace("NEWPRICE", newPrice.ToString("C0"))
                         .Replace("USER", SessionHandler.CurrentUser.FullName),
                     null);

                    }

                    var emailWaitingList = new EmailWaitingList()
                    {
                        ListingId = inventory.InventoryId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.SalePriceChange,
                        OldValue = currentSalePrice,
                        NewValue = newPrice,
                        UserId = SessionHandler.CurrentUser.UserId

                    };
                    var newPriceChangeHistory = new vincontrol.Data.Model.PriceChangeHistory
                    {
                        DateStamp = DateTime.Now,
                        ListingId = inventory.InventoryId,
                        OldPrice = currentSalePrice,
                        NewPrice = newPrice,
                        UserStamp = SessionHandler.CurrentUser.Username,
                        AttachFile = string.Empty,
                        VehicleStatusCodeId = vehicleStatusCodeId
                    };

                    _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);
                    _inventoryManagementForm.AddPriceChangeHistory(newPriceChangeHistory);

                }

            }
       
            return Json("Not Updated");
        }

        public ActionResult UpdateStatusFromInventoryPage(short status, int listingId)
        {

            var currentStatusCode = _inventoryManagementForm.GetInventory(listingId).InventoryStatusCode.Value;

            var newStatusCode = _inventoryManagementForm.GetStausCodeName(status);

            _inventoryManagementForm.UpdateStatus(listingId, status);

            _vehicleLogForm.AddVehicleLog(
                listingId, SessionHandler.CurrentUser.UserId,
                Constanst.VehicleLogSentence.ChangeStatusByUser.Replace("OLDSTATUS", currentStatusCode)
                    .Replace("NEWSTATUS", newStatusCode)
                    .Replace("USER", SessionHandler.CurrentUser.FullName), null);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }

        public ActionResult UpdateStatusForSoldTab(int listingId)
        {

            SQLHelper.MarkUnsoldVehicle(listingId);

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }

        public ActionResult UpdateStatusChange(short currentStatus, short status, int listingId)
        {

            if (currentStatus == Constanst.InventoryStatus.SoldOut)
                SQLHelper.MarkUnsoldVehicle(listingId, status);
            else
            {
                var currentStatusCode = _inventoryManagementForm.GetInventory(listingId).InventoryStatusCode.Value;

                var newStatusCode = _inventoryManagementForm.GetStausCodeName(status);

                _inventoryManagementForm.UpdateStatus(listingId, status);

                _vehicleLogForm.AddVehicleLog(
                       listingId, SessionHandler.CurrentUser.UserId,
                        Constanst.VehicleLogSentence.ChangeStatusByUser.Replace("OLDSTATUS", currentStatusCode).Replace("NEWSTATUS", newStatusCode)
                        .Replace("USER", SessionHandler.CurrentUser.FullName), null);
               
            }
                

            if (status == Constanst.InventoryStatus.SoldOut)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.SoldInventory.ToString();
            if (status == Constanst.InventoryStatus.Inventory)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.Inventory.ToString();
            if (status == Constanst.InventoryStatus.Wholesale)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.WholesaleInventory.ToString();
            if (status == Constanst.InventoryStatus.Recon)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.ReconInventory.ToString();
            if (status == Constanst.InventoryStatus.Auction)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.AuctionInventory.ToString();
            if (status == Constanst.InventoryStatus.Loaner)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.LoanerInventory.ToString();
            if (status == Constanst.InventoryStatus.TradeNotClear)
                SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.TradeNotClear.ToString();

            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");
            }
            return Json("Not Updated");
        }

        public ActionResult UpdateMileageFromInventoryPage(string mileage, int listingId)
        {
            mileage = CommonHelper.RemoveSpecialCharactersForPurePrice(mileage);

            long newMileage = 0;
            Int64.TryParse(mileage, out newMileage);

            var inventory = _inventoryManagementForm.GetInventory(listingId);
            if (inventory != null)
            {
                var currentMileage = inventory.Mileage.GetValueOrDefault();
                _inventoryManagementForm.UpdateMileage(listingId, newMileage);
                if (currentMileage != newMileage)
                {

                    _vehicleLogForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                        Constanst.VehicleLogSentence.MileageChangeByUser.Replace("OLDMILEAGE", currentMileage.ToString("N0")).
                        Replace("NEWMILEAGE", newMileage.ToString("N0")).Replace("USER", SessionHandler.CurrentUser.FullName),
                        null);

                    var emailWaitingList = new EmailWaitingList()
                    {
                        ListingId = listingId,
                        DateStamp = DateTime.Now,
                        Expire = false,
                        NotificationTypeCodeId = Constanst.NotificationType.MileageChange,
                        OldValue = currentMileage,
                        NewValue = newMileage,
                        UserId = SessionHandler.CurrentUser.UserId

                    };
                    _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);

                }
          
            }
            else
            {
                var soldInventory= _inventoryManagementForm.GetSoldInventory(listingId);
                if (soldInventory != null)
                {
                    var currentMileage = soldInventory.Mileage.GetValueOrDefault();
                    _inventoryManagementForm.UpdateSoldMileage(soldInventory.SoldoutInventoryId, newMileage);
                    if (currentMileage != newMileage)
                    {

                        _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                            Constanst.VehicleLogSentence.MileageChangeByUser.Replace("OLDMILEAGE", currentMileage.ToString("N0")).
                            Replace("NEWMILEAGE", newMileage.ToString("N0")).Replace("USER", SessionHandler.CurrentUser.FullName),
                            soldInventory.SoldoutInventoryId);

                        

                    }

                }
            }

          
            if (Request.IsAjaxRequest())
            {
                return Json("Update Successfully");

            }

            return Json("Not Updated");
        }

        [HttpPost]
        public string UpdateCertifiedAmount(int listingId, decimal amount, short type = 2)
        {
            try
            {
                switch (type)
                {
                    case Constanst.VehicleStatus.Inventory: _inventoryManagementForm.UpdateCertifiedAmount(listingId, amount); break;
                    case Constanst.VehicleStatus.Appraisal: _appraisalManagementForm.UpdateCertifiedAmount(listingId, amount); break;
                    case Constanst.VehicleStatus.SoldOut: break;
                }
                
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        [HttpPost]
        public string UpdateMileageAdjustment(int listingId, decimal amount, short type = 2)
        {
            try
            {
                switch (type)
                {
                    case Constanst.VehicleStatus.Inventory: _inventoryManagementForm.UpdateMileageAdjustment(listingId, amount); break;
                    case Constanst.VehicleStatus.Appraisal: _appraisalManagementForm.UpdateMileageAdjustment(listingId, amount); break;
                    case Constanst.VehicleStatus.SoldOut: break;
                }
                
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        [HttpPost]
        public string UpdateNote(int listingId, string note, short type = 2)
        {
            try
            {
                switch (type)
                {
                    case Constanst.VehicleStatus.Inventory: _inventoryManagementForm.UpdateNote(listingId, note); break;
                    case Constanst.VehicleStatus.Appraisal: _appraisalManagementForm.UpdateNote(listingId, note); break;
                    case Constanst.VehicleStatus.SoldOut: break;
                }
                
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkUnsold(int listingId)
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            int returnId = SQLHelper.MarkUnsoldVehicle(listingId);

            return RedirectToAction("ViewIProfile", new { ListingID = returnId });
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkSold(CustomeInfoModel customer)
        {

            var soldInventoryid= SQLHelper.MarkSoldVehicle(Convert.ToInt32(customer.ListingId),customer);

            SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.SoldInventory.ToString();

            if (Request.IsAjaxRequest())
            {
                return Json(soldInventoryid);
            }

            return Json(0);
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult ViewMarkSold(string listingId)
        {
            if (SessionHandler.Dealer != null)
            {
                var viewModel = new CustomeInfoModel
                {
                    ListingId =Convert.ToInt32( listingId),
                    States = SelectListHelper.InitialStateList(),
                    Countries = SelectListHelper.InitialCountryList()
                };
                return View("MarkSold", viewModel);
            }
            return Json("SessionTimeOut");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewPriceTracking(string listingId)
        {
            if (SessionHandler.Dealer != null)
            {
                return View("PriceTracking",
                    new PriceChangeViewModel
                    {
                        Id = listingId,
                        PriceChangeHistory =
                            DataHelper.GetPriceChangeList(listingId, ChartTimeType.Last7Days, 1),
                        InventoryStatus = 1
                    });
            }
            return Json("SessionTimeOut");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewPriceTrackingForSold(string listingId)
        {
            if (SessionHandler.Dealer != null)
            {
                return View("PriceTracking",
                    new PriceChangeViewModel
                    {
                        Id = listingId,
                        PriceChangeHistory =
                            DataHelper.GetPriceChangeList(listingId, ChartTimeType.Last7Days, -1),
                        InventoryStatus = -1
                    });
            }
            return Json("SessionTimeOut");
        }

        public ActionResult PriceTrackingChart(string itemId, ChartTimeType type, int inventoryStatus)
        {
            return PartialView("PriceTrackingChart",
                new PriceChangeViewModel
                {
                    PriceChangeHistory = DataHelper.GetPriceChangeList(itemId, type, inventoryStatus),
                    Id = itemId,
                    ChartType = type,
                    InventoryStatus = inventoryStatus
                });

        }

        public ActionResult RenderPriceChart(string itemId, ChartTimeType chartTimeType, int inventoryStatus)
        {
            var createdDate = DataHelper.GetCreatedDate(itemId, inventoryStatus);
            if (createdDate != null)
            {
                var createDate = createdDate.Value;

                var chart =
                    PDFController.CreateChart(
                        DataHelper.GetPriceChangeListForChart(itemId, chartTimeType, createDate, inventoryStatus),
                        RenderType.BinaryStreaming, 800, 300, chartTimeType, createDate);
                using (var ms = new MemoryStream())
                {
                    chart.SaveImage(ms, ChartImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms.ToArray(), "image/png", "mychart.png");
                }
            }
            return null;
        }

        public ActionResult DownloadBucketJumpReport(string name)
        {
            using (var context = new VincontrolEntities())
            {
                try
                {
                    var bucketJumpReportFile = context.BucketJumpHistories.FirstOrDefault(i => i.AttachFile.ToLower().Equals(name.ToLower()));
                    if (bucketJumpReportFile != null)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath("\\BucketJumpReports") + "\\" +
                            SessionHandler.Dealer.DealershipId + "\\" + (bucketJumpReportFile.VehicleStatusCodeId == Constanst.VehicleStatus.Appraisal ? "Appraisal" : "Inventory") +
                                      "\\" + bucketJumpReportFile.ListingId;
                        var dir = new DirectoryInfo(path);
                        if (!dir.Exists) return File(new byte[] { }, "application/pdf");

                        string filename = bucketJumpReportFile.AttachFile;
                        string fullPath = Path.Combine(@path, filename);

                        return File(fullPath, "application/pdf", filename.Split('_')[0] + "_BucketJumpTracking.pdf");
                    }
                }
                catch (Exception)
                {
                    return File(new byte[] { }, "application/pdf");
                }
            }
            return null;
        }

        public ActionResult PrintPriceTracking(string itemId, string type, int inventoryStatus)
        {
            return RedirectToAction("PrintPriceTracking", "PDF", new { itemId, type, inventoryStatus });
        }

        public ActionResult ViewBucketJumpTracking(string listingId, string type)
        {
            if (SessionHandler.Dealer != null)
            {

                var convertedListingId = Convert.ToInt32(listingId);
                var history = _inventoryManagementForm.GetBucketJumpHistory(convertedListingId, Convert.ToInt16(type)).ToList();

                if (history.Count > 0)
                {
                    return View("BucketJumpTracking", history.Select(i => new Models.BucketJumpHistory
                    {
                        AttachFile = i.AttachFile,
                        UserStamp = i.UserFullName,
                        DateStamp = i.DateStamp.GetValueOrDefault(),
                        SalePrice = i.SalePrice.GetValueOrDefault(),
                        RetailPrice = i.RetailPrice.GetValueOrDefault(),
                        Type = i.VehicleStatusCode.ToString(),
                        ListingId = i.ListingId,
                        BucketJumpDayAlert = i.BucketJumpDayAlert.GetValueOrDefault(),
                        BucketJumpCompleteDate = i.BucketJumpCompleteDate.GetValueOrDefault()
                    }).ToList());
                }

                return View("BucketJumpTracking", new List<Models.BucketJumpHistory>());
            }

            return Json("SessionTimeOut");
        }

        public ActionResult ViewBucketJumpTrackingForSold(string listingId)
        {
            if (SessionHandler.Dealer != null)
            {

                var convertedListingId = Convert.ToInt32(listingId);

                var history = _inventoryManagementForm.GetBucketJumpHistory(convertedListingId,
                    Constanst.VehicleStatus.SoldOut).ToList();

                if (history.Count > 0)
                {
                    return View("BucketJumpTracking", history.Select(i => new Models.BucketJumpHistory
                    {
                        AttachFile = i.AttachFile,
                        UserStamp = i.UserStamp,
                        DateStamp = i.DateStamp.GetValueOrDefault(),
                        SalePrice = i.SalePrice.GetValueOrDefault(),
                        RetailPrice = i.RetailPrice.GetValueOrDefault(),
                        Type = i.VehicleStatusCode.ToString(),
                        ListingId = i.ListingId,
                        BucketJumpDayAlert = i.BucketJumpDayAlert.GetValueOrDefault(),
                        BucketJumpCompleteDate = i.BucketJumpCompleteDate.GetValueOrDefault()
                    }).ToList());
                }

                return View("BucketJumpTracking", new List<Models.BucketJumpHistory>());
            }
            return Json("SessionTimeOut");
        }

        public ActionResult SoldOutAlert(int listingId)
        {
            using (var context = new VincontrolEntities())
            {
                var row = _inventoryManagementForm.GetSoldInventory(listingId);

                if (row != null)
                {
                    ViewData["Year"] = (row.Vehicle.Year.GetValueOrDefault());

                    ViewData["Make"] = String.IsNullOrEmpty(row.Vehicle.Make) ? "NA" : row.Vehicle.Make;

                    ViewData["Model"] = String.IsNullOrEmpty(row.Vehicle.Model) ? "NA" : row.Vehicle.Model;

                    ViewData["Trim"] = String.IsNullOrEmpty(row.Vehicle.Trim) ? "NA" : row.Vehicle.Trim;

                    if (row.DateInStock != null) ViewData["DateInStock"] = row.DateInStock.Value;

                    ViewData["Vin"] = String.IsNullOrEmpty(row.Vehicle.Vin) ? "NA" : row.Vehicle.Vin;
                }


                ViewData["ListingId"] = listingId;

            }



            return View("SoldOutAlert");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "ALLACCESS")]
        public ActionResult MarkUnSoldFromVinDecode(FormCollection form)
        {
            string lisitngId = form["ListingId"];
            int lid = Convert.ToInt32(lisitngId);

            int autoId = SQLHelper.MarkUnsoldVehicle(lid);


            return ViewIProfile(autoId);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewAppraisal(FormCollection form)
        {
            string vin = form["Vin"];
            return RedirectToAction("DecodeProcessingByVin", "Decode", new { Vin = vin });
        }

        public ActionResult MarkUnSoldFromVinDecodeNew(int listingId)
        {
            int autoId = SQLHelper.MarkUnsoldVehicle(listingId);


            return ViewIProfile(autoId);
        }

        public ActionResult NewAppraisalFromSoldAlert(string vin)
        {
            return RedirectToAction("DecodeProcessingByVin", "Decode", new { Vin = vin });
        }

        public ActionResult OpenSilverlightUploadWindow(int listingId, int inventoryStatus)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;
           
                using (var context = new VincontrolEntities())
                {
                    if (inventoryStatus == Constanst.InventoryStatus.Inventory)

                    {

                        var row = _inventoryManagementForm.GetInventory(listingId);
                        if (row != null)
                        {
                            var viewModel = new SilverlightImageViewModel
                            {
                                ListingId = listingId,
                                Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "" : row.Vehicle.Vin,
                                DealerId = dealer.DealershipId,
                                InventoryStatus = Constanst.VehicleStatus.Inventory,
                                ImageServiceURL =
                                    (System.Web.HttpContext.Current.Request.Url.Port == 80)
                                        ? String.Format("{0}://{1}/ImageHandlers/SilverlightHandler.ashx",
                                            System.Web.HttpContext.Current.Request.Url.Scheme,
                                            System.Web.HttpContext.Current.Request.Url.Host)
                                        : String.Format("{0}://{1}:{2}/ImageHandlers/SilverlightHandler.ashx",
                                            System.Web.HttpContext.Current.Request.Url.Scheme,
                                            System.Web.HttpContext.Current.Request.Url.Host,
                                            System.Web.HttpContext.Current.Request.Url.Port)
                            };

                            return View("imageSilverlightSortFrame", viewModel);
                        }
                    }

                    if (inventoryStatus == Constanst.InventoryStatus.SoldOut)
                    {

                        var row = _inventoryManagementForm.GetSoldInventory(listingId);
                        if (row != null)
                        {
                            var viewModel = new SilverlightImageViewModel
                            {
                                ListingId = listingId,
                                Vin = String.IsNullOrEmpty(row.Vehicle.Vin) ? "" : row.Vehicle.Vin,
                                DealerId = dealer.DealershipId,
                                InventoryStatus = Constanst.VehicleStatus.SoldOut,
                                ImageServiceURL =
                                    (System.Web.HttpContext.Current.Request.Url.Port == 80)
                                        ? String.Format("{0}://{1}/ImageHandlers/SilverlightHandler.ashx",
                                            System.Web.HttpContext.Current.Request.Url.Scheme,
                                            System.Web.HttpContext.Current.Request.Url.Host)
                                        : String.Format("{0}://{1}:{2}/ImageHandlers/SilverlightHandler.ashx",
                                            System.Web.HttpContext.Current.Request.Url.Scheme,
                                            System.Web.HttpContext.Current.Request.Url.Host,
                                            System.Web.HttpContext.Current.Request.Url.Port)
                            };

                            return View("imageSilverlightSortFrame", viewModel);
                        }
                    }

                }
            }
            else
            {
                var viewModel = new SilverlightImageViewModel { SessionTimeOut = true };
                return View("imageSilverlightSortFrame", viewModel);
            }
            return null;
        }

        public ActionResult OpenUploadWindow(int listingId, int inventoryStatus)
        {
            if (SessionHandler.Dealer != null)
            {
                var dealer = SessionHandler.Dealer;

                using (var context = new VincontrolEntities())
                {
                    if (inventoryStatus == Constanst.InventoryStatus.Inventory)
                    {

                        var row = _inventoryManagementForm.GetInventory(listingId);
                        return (row != null) ? View(new CarInfoFormViewModel(row)) : null;
                    }

                    if (inventoryStatus == Constanst.InventoryStatus.SoldOut)
                    {

                        var row = _inventoryManagementForm.GetSoldInventory(listingId);
                        return (row != null) ? View(new CarInfoFormViewModel(row)) : null;
                    }

                }
            }
            
            return View(new CarInfoFormViewModel { SessionTimeOut = true });
        }
        
        public ActionResult OpenWholeSale(int listingId)
        {
            if (SessionHandler.Dealer != null && SessionHandler.DealerGroup != null)
            {
                var vehicle = new CarInfoFormViewModel { ListingId = listingId, Type = Constanst.CarInfoType.New };

                return View("WholeSale", vehicle);
            }
            var viewModel = new CarInfoFormViewModel { SessionTimeOut = true };
            return View("WholeSale", viewModel);
        }

        public ActionResult OpenWholeSaleFromSold(int listingId)
        {
            if (SessionHandler.Dealer != null && SessionHandler.DealerGroup != null)
            {
                var vehicle = new CarInfoFormViewModel { ListingId = listingId, Type = Constanst.CarInfoType.Sold };

                return View("WholeSale", vehicle);
            }
            var viewModel = new CarInfoFormViewModel { SessionTimeOut = true };
            return View("WholeSale", viewModel);
        }

        public ActionResult OpenVehicleTransferWindow(int listingId)
        {
            if (SessionHandler.Dealer != null && SessionHandler.DealerGroup != null)
            {
                var dealer = SessionHandler.Dealer;

                var vehicle = new CarInfoFormViewModel();

                var row = _inventoryManagementForm.GetInventory(listingId);

                var dealerGroup = SessionHandler.DealerGroup;

                vehicle.TransferDealerGroup = SelectListHelper.InitialDealerListExtract(dealerGroup, dealer.DealershipId);

                if (row != null)
                {
                    vehicle.ModelYear = row.Vehicle.Year.GetValueOrDefault();

                    vehicle.Make = row.Vehicle.Make;

                    vehicle.Model = row.Vehicle.Model;

                    vehicle.Trim = row.Vehicle.Trim;

                    vehicle.Stock = row.Stock;

                    vehicle.ListingId = row.InventoryId;
                }

                vehicle.DealerName = dealer.DealershipName;

                return View("VehicleTransfer", vehicle);
            }
            var viewModel = new CarInfoFormViewModel { SessionTimeOut = true };
            return View("VehicleTransfer", viewModel);
        }

        public ActionResult TransferVehicle(CarInfoFormViewModel car)
        {
            var dealerGroup = SessionHandler.DealerGroup;
            if (!String.IsNullOrEmpty(car.Stock.Trim()))
            {
                if (!SQLHelper.CheckStockNumberExist(car.Stock, Convert.ToInt32(car.SelectedDealerTransfer)))
                {
                    SQLHelper.TransferVehicle(car.ListingId, Convert.ToInt32(car.SelectedDealerTransfer), car.Stock,
                        dealerGroup);
                    return Json("Success");
                }
                return Json("DuplicateStock");
            }
            SQLHelper.TransferVehicle(car.ListingId, Convert.ToInt32(car.SelectedDealerTransfer), car.Stock, dealerGroup);
            return Json("Success");
        }

        public ActionResult UpdateWarrantyInfo(int warrantyInfo, int listingId)
        {

            if (SessionHandler.Dealer != null)
            {
               _inventoryManagementForm.UpdateWarrantyInfo(warrantyInfo,listingId);
                _vehicleLogForm.AddVehicleLog(
                    listingId, SessionHandler.CurrentUser.UserId, 
                    Constanst.VehicleLogSentence.WarrantyChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName),null);
                
                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        public ActionResult PriorRentalUpdate(bool priorRental, int listingId)
        {

            if (SessionHandler.Dealer != null)
            {

                _inventoryManagementForm.UpdatePriorRental(priorRental, listingId);
                _vehicleLogForm.AddVehicleLog(
                    listingId, SessionHandler.CurrentUser.UserId,
                    Constanst.VehicleLogSentence.PriorRentalChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName), null);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        public ActionResult DealerDemoUpdate(bool dealerDemo, int listingId)
        {

            if (SessionHandler.Dealer != null)
            {

                _inventoryManagementForm.UpdateDealerDemo(dealerDemo, listingId);
                _vehicleLogForm.AddVehicleLog(
                    listingId, SessionHandler.CurrentUser.UserId,
                      Constanst.VehicleLogSentence.DealerDemoChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName), null);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        public ActionResult UnwindUpdate(bool unwind, int listingId)
        {

            if (SessionHandler.Dealer != null)
            {

                _inventoryManagementForm.UpdateUnwind(unwind, listingId);
                _vehicleLogForm.AddVehicleLog(
                    listingId, SessionHandler.CurrentUser.UserId,
                      Constanst.VehicleLogSentence.UnWindChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName), null);

                if (Request.IsAjaxRequest())
                {
                    return Json("Update Successfully");

                }

                return Json("Not Updated");
            }
            var user = new UserRoleViewModel
            {
                SessionTimeOut = "TimeOut"
            };
            return Json(user);
        }

        [HttpPost]
        public string UpdateIsFeatured(string id)
        {
            try
            {
                var listingId = Convert.ToInt32(id);

                var vehicle = _inventoryManagementForm.GetInventory(listingId);
                if (vehicle != null)
                {
                    if (vehicle.IsFeatured.HasValue && vehicle.IsFeatured.Value)
                    {
                        _inventoryManagementForm.UpdateIsFeatured(vehicle.InventoryId, false);
                        _vehicleLogForm.AddVehicleLog(vehicle.InventoryId, SessionHandler.CurrentUser.UserId, 
                            Constanst.VehicleLogSentence.NonFeaturedChangeByUser.Replace("USER",SessionHandler.CurrentUser.FullName), null);
                    }
                       

                    else
                    {
                        _inventoryManagementForm.UpdateIsFeatured(vehicle.InventoryId, true);
                          _vehicleLogForm.AddVehicleLog(vehicle.InventoryId, SessionHandler.CurrentUser.UserId,
                              Constanst.VehicleLogSentence.IsFeaturedChangeByUser.Replace("USER",SessionHandler.CurrentUser.FullName),null);
                    }
                        
                    return "Updated successfully.";
                }
                var soldvehicle = _inventoryManagementForm.GetSoldInventory(listingId);
                 
                if (soldvehicle != null)
                {
                    if (soldvehicle.IsFeatured.HasValue && soldvehicle.IsFeatured.Value)
                    {
                        _inventoryManagementForm.UpdateIsFeaturedForSoldCar(soldvehicle.SoldoutInventoryId, false);
                        _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                            Constanst.VehicleLogSentence.NonFeaturedChangeByUser.Replace("USER",
                                SessionHandler.CurrentUser.FullName), soldvehicle.SoldoutInventoryId);
                    }
                    else
                    {

                        _inventoryManagementForm.UpdateIsFeaturedForSoldCar(soldvehicle.SoldoutInventoryId, false);
                        _vehicleLogForm.AddVehicleLog(null, SessionHandler.CurrentUser.UserId,
                            Constanst.VehicleLogSentence.IsFeaturedChangeByUser.Replace("USER", SessionHandler.CurrentUser.FullName)
                            , soldvehicle.SoldoutInventoryId);
                    }
                    
                    return "Updated successfully.";
                }

                return "This vehicle doesn't exist in inventory.";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public ActionResult GetCityAndStateFromZip(string zipCode)
        {
            using (var context = new VincontrolEntities())
            {
                int result;
                if (int.TryParse(zipCode, out result))
                {
                    var zip = context.UsaZipCodes.FirstOrDefault(z => z.UsaZipCodeId == result);
                    if (zip != null)
                    {
                        var returnValue = Json(new { zip.CityName, zip.StateName, zip.StateAbbr },
                            JsonRequestBehavior.AllowGet);
                        return returnValue;
                    }
                }
            }
            return Json(new { CityName = String.Empty, StateName = String.Empty, StateAbbr = String.Empty });
        }

        #region Advanced search actions

        //private List<Inventory> GetNewInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var result = InventoryQueryHelper.GetSingleOrGroupInventory(context, true)
        //            .Where(e => e.Condition == Constanst.ConditionStatus.New &&
        //                        e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory)
        //            .ToList();
        //        foreach (var item in result)
        //        {
        //            item.OldType = "0";
        //        }

        //        return result;
        //    }
        //}

        private List<Inventory> GetAllInventories()
        {
            return GetAllInventories(i => true);
        }

        private List<Inventory> GetAllInventories(Func<Inventory,bool> filter)
        {
            using (var context = new VincontrolEntities())
            {
                return InventoryQueryHelper.GetSingleOrGroupInventory(context, true, true, true).Where(filter).ToList();
            }
        }

        //private List<Inventory> GetUsedInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var result = InventoryQueryHelper.GetSingleOrGroupInventory(context, true)
        //            .Where(e => e.Condition == Constanst.ConditionStatus.Used &&
        //                        e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory)
        //            .ToList();

        //        foreach (var item in result)
        //        {
        //            item.OldType = "1";
        //        }

        //        return result;
        //    }
        //}

        private List<SoldoutInventory> GetSoldInventory()
        {
            using (var context = new VincontrolEntities())
            {
               return InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context, true).ToList();
            }
        }

        //private List<Inventory> GetWholesaleInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var wholesaleInventory = InventoryQueryHelper.GetSingleOrGroupWholesaleInventory(context, true).ToList();
        //        return wholesaleInventory.Select(e => new Inventory
        //        {
        //            InventoryId = e.InventoryId,
        //            Vehicle = e.Vehicle,
        //            DealerId = e.DealerId,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.SalePrice,
        //            DateInStock = e.DateInStock,
        //            ThumbnailUrl = e.ThumbnailUrl,
        //            MarketRange = e.MarketRange.GetValueOrDefault(), // TODO: Soldout table doesn't have reference to MarketRange
        //            CarfaxOwner = e.CarfaxOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "2",
        //            Condition = e.Condition,
        //        }).ToList();
        //    }
        //}

        //private List<Inventory> GetLoanerInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var loanerInventory = InventoryQueryHelper.GetSingleOrGroupLoanerInventory(context).ToList();
        //        return loanerInventory.Select(e => new Inventory
        //        {
        //            InventoryId = e.InventoryId,
        //            Vehicle = e.Vehicle,
        //            DealerId = e.DealerId,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.SalePrice,
        //            DateInStock = e.DateInStock,
        //            ThumbnailUrl = e.ThumbnailUrl,
        //            MarketRange = e.MarketRange.GetValueOrDefault(), 
        //            CarfaxOwner = e.CarfaxOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "5",
        //        }).ToList();
        //    }
        //}

        //private List<Inventory> GetAuctionInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var auctionInventory = InventoryQueryHelper.GetSingleOrGroupAuctionInventory(context).ToList();
        //        return auctionInventory.Select(e => new Inventory
        //        {
        //            InventoryId = e.InventoryId,
        //            Vehicle = e.Vehicle,
        //            DealerId = e.DealerId,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.SalePrice,
        //            DateInStock = e.DateInStock,
        //            ThumbnailUrl = e.ThumbnailUrl,
        //            MarketRange = 0, // TODO: Soldout table doesn't have reference to MarketRange
        //            CarfaxOwner = e.CarfaxOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "6"
        //        }).ToList();
        //    }
        //}

        //private List<Inventory> GetReconInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var reconInventory = InventoryQueryHelper.GetSingleOrGroupReconInventory(context).ToList();
        //        return reconInventory.Select(e => new Inventory
        //        {
        //            InventoryId = e.InventoryId,
        //            Vehicle = e.Vehicle,
        //            DealerId = e.DealerId,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.SalePrice,
        //            DateInStock = e.DateInStock,
        //            ThumbnailUrl = e.ThumbnailUrl,
        //            MarketRange = 0, // TODO: Soldout table doesn't have reference to MarketRange
        //            CarfaxOwner = e.CarfaxOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "7"
        //        }).ToList();
        //    }
        //}

        //private List<Inventory> GetTradeNotClearInventory()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var tradenotclearInventory = InventoryQueryHelper.GetSingleOrGroupTradeNotClearInventory(context).ToList();
        //        return tradenotclearInventory.Select(e => new Inventory
        //        {
        //            InventoryId = e.InventoryId,
        //            Vehicle = e.Vehicle,
        //            DealerId = e.DealerId,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.SalePrice,
        //            DateInStock = e.DateInStock,
        //            ThumbnailUrl = e.ThumbnailUrl,
        //            MarketRange = 0, // TODO: Soldout table doesn't have reference to MarketRange
        //            CarfaxOwner = e.CarfaxOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "8"
        //        }).ToList();
        //    }
        //}

        //private List<Inventory> GetRecentAppraisal()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var recentAppraisal = new List<Appraisal>();

        //        if (SessionHandler.IsEmployee)
        //        {
        //            recentAppraisal = InventoryQueryHelper.GetSingleOrGroupAppraisalIncludeUser(context)
        //                .Where(x => x.AppraisalById == SessionHandler.UserId && (x.AppraisalStatusCodeId == null || x.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending))
        //                .OrderByDescending(x => x.AppraisalDate.Value).ToList();
        //        }
        //        else
        //        {
        //            recentAppraisal = InventoryQueryHelper.GetSingleOrGroupAppraisalIncludeUser(context)
        //                .Where(x => (x.AppraisalStatusCodeId == null || x.AppraisalStatusCodeId != Constanst.AppraisalStatus.Pending))
        //                .OrderByDescending(x => x.AppraisalDate.Value).ToList();
        //        }

        //        return recentAppraisal.Select(e => new Inventory
        //        {
        //            InventoryId = e.AppraisalId,
        //            DealerId = e.DealerId,
        //            Vehicle = e.Vehicle,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.ACV,
        //            DateInStock = DateTime.Now,
        //            ThumbnailUrl = e.PhotoUrl,
        //            MarketRange = 0, // TODO: Soldout table doesn't have reference to MarketRange
        //            CarfaxOwner = e.CARFAXOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "9"
        //        }).ToList();
        //    }
        //}

        //private List<Inventory> GetPendingAppraisal()
        //{
        //    using (var context = new VincontrolEntities())
        //    {
        //        var pendingAppraisal = new List<Appraisal>();

        //        if (SessionHandler.IsEmployee)
        //        {
        //            pendingAppraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
        //                .Where(e => e.AppraisalById == SessionHandler.UserId && e.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending)
        //                .OrderByDescending(x => x.AppraisalDate.Value).ToList();
        //        }
        //        else
        //        {
        //            pendingAppraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context)
        //                .Where(e => e.AppraisalStatusCodeId == Constanst.AppraisalStatus.Pending)
        //                .OrderByDescending(x => x.AppraisalDate.Value).ToList();
        //        }

        //        return pendingAppraisal.Select(e => new Inventory
        //        {
        //            InventoryId = e.AppraisalId,
        //            DealerId = e.DealerId,
        //            Vehicle = e.Vehicle,
        //            Stock = e.Stock ?? string.Empty,
        //            Mileage = e.Mileage,
        //            ExteriorColor = e.ExteriorColor ?? string.Empty,
        //            PhotoUrl = e.PhotoUrl,
        //            SalePrice = e.ACV,
        //            DateInStock = DateTime.Now,
        //            ThumbnailUrl = e.PhotoUrl,
        //            MarketRange = 0, // TODO: Soldout table doesn't have reference to MarketRange
        //            CarfaxOwner = e.CARFAXOwner.GetValueOrDefault(),
        //            Descriptions = e.Descriptions ?? string.Empty,
        //            OldType = "10"
        //        }).ToList();
        //    }
        //}

        private List<Appraisal> GetAppraisal(Func<Appraisal,bool> filter)
        {
            using (var context = new VincontrolEntities())
            {
                var appraisal = new List<Appraisal>();

                if (SessionHandler.IsEmployee)
                {
                    appraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context,true)
                        .Where(e => e.AppraisalById == SessionHandler.UserId)
                        .OrderByDescending(x => x.AppraisalDate.Value).Where(filter).ToList();
                }
                else
                {
                    appraisal = InventoryQueryHelper.GetSingleOrGroupAppraisal(context,true)
                        .OrderByDescending(x => x.AppraisalDate.Value).Where(filter).ToList();
                }

                return appraisal;
            }
        }

        private List<Appraisal> GetAppraisal()
        {
            return GetAppraisal(i => true);
        }

        #endregion

        #region Json Actions For Inventory List

        public ActionResult UpdateViewInfoStatus(ViewInfo viewInfo)
        {
            SessionHandler.InventoryViewInfo = viewInfo;
            return Json("success");
        }

        

        private ActionResult GetInventoryJson(IEnumerable<Inventory> avaiInventory, int carType, string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            var result = avaiInventory.ToList()
                .Select(tmp => new InventoryInfo(tmp) { Type = carType }).ToList();

            if (usingFilter == true)
            {
                result = FilterInventory(result, make, model, trim, year, price, fromMile, toMile);
            }

            result = SortCacheInventory(result, sortBy, isUp);
            SessionHandler.InventoryList = result;

            result = result.Take(pageSize).ToList();
            ViewData["VehicleStatusCodeId"] = Constanst.VehicleStatus.Inventory;
            
            return new LargeJsonResult
            {
                Data = new
                {
                    count = SessionHandler.InventoryList.Count,
                    list = result,
                }
            };
        }

        private List<InventoryInfo> FilterInventory(List<InventoryInfo> orgList, string make, string model, string trim, string year, int price, int? fromMile, int? toMile)
        {
            var result = new List<InventoryInfo>(orgList);

            if (make != "All")
            {
                result = result.Where(x => x.Make == make).ToList();
            }

            if (model != "All")
            {
                result = result.Where(x => x.Model == model).ToList();
            }

            if (trim != "All")
            {
                result = result.Where(x => x.Trim == trim).ToList();
            }

            if (year != "All")
            {
                result = result.Where(x => x.Year == int.Parse(year)).ToList();
            }

            if (price != 0)
            {
                var priceIncrement = 10000;
                var minPrice = priceIncrement * (price - 1);

                if (price > 10)
                {
                    result = result.Where(x => x.SalePrice >= minPrice).ToList();
                }
                else
                {
                    var maxPrice = priceIncrement * price;
                    result = result.Where(x => x.SalePrice <= maxPrice && x.SalePrice >= minPrice).ToList();
                }
            }

            result = result.Where(x => (!fromMile.HasValue || !toMile.HasValue) || (x.Mileage <= toMile.Value && x.Mileage >= fromMile.Value)).ToList();

            return result;
        }

        private ActionResult ViewWholeSaleInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(Constanst.Message.Unauthorized);
            }

            using (var context = new VincontrolEntities())
            {
                var list = 
                    InventoryQueryHelper.GetSingleOrGroupWholesaleInventory(context, true, true, true)
                        .ToList()
                        .Select(tmp => new InventoryInfo(tmp)
                        {
                            Type = Constanst.CarInfoType.Wholesale,
                        }).ToList();

                if (usingFilter == true)
                {
                    list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
                }

                list = SortCacheInventory(list, sortBy, isUp);
                SessionHandler.InventoryList = list;

                list = list.Take(pageSize).ToList();

                ViewData["inventoryType"] = Constanst.InventoryTab.Wholesale;
                SessionHandler.CurrentView = CurrentViewEnum.WholesaleInventory.ToString();

                return new LargeJsonResult
                {
                    Data = new
                    {
                        count = SessionHandler.InventoryList.Count,
                        list
                    }
                };
            }
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewInventory()
        {
            if (SessionHandler.Dealer == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            // Reset KPI Condition
            SessionHandler.KpiConditon = 0;

            ViewData["inventoryType"] = Constanst.InventoryTab.Used;
            ViewData["isShowExpand"] = "true";
            SessionHandler.CurrentView = CurrentViewEnum.Inventory.ToString();

            if (SessionHandler.InventoryViewInfo == null)
            {
                SessionHandler.InventoryViewInfo = new ViewInfo { IsUp = true, SortFieldName = SessionHandler.Dealer.DealerSetting.InventorySorting, CurrentState = 0, CurrentView = CurrentViewEnum.Inventory.ToString() };
            }
            else
            {
                if (SessionHandler.InventoryViewInfo.CurrentView == CurrentViewEnum.TodayBucketJump.ToString() ||
                    SessionHandler.InventoryViewInfo.CurrentView == CurrentViewEnum.ExpressBucketJump.ToString() ||
                    SessionHandler.InventoryViewInfo.CurrentView == CurrentViewEnum.ACar.ToString() ||
                    SessionHandler.InventoryViewInfo.CurrentView == CurrentViewEnum.MissingContent.ToString() ||
                    SessionHandler.InventoryViewInfo.CurrentView == CurrentViewEnum.NoContent.ToString())
                {
                    SessionHandler.InventoryViewInfo.CurrentView = CurrentViewEnum.Inventory.ToString();
                }
            }
            ViewData["Year"] = ChromeHelper.GetChromeYear();
            return View("ViewSmallInventory",
                new InventoryFormViewModel { IsCompactView = false, CombineInventory = false });
        }

        private ActionResult ViewUsedInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            ViewData["inventoryType"] = Constanst.InventoryTab.Used;
            SessionHandler.CurrentView = CurrentViewEnum.Inventory.ToString();

            return HandleInventoryViewJson(delegate(DealershipViewModel dealer, VincontrolEntities context)
            {
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                    where
                        e.Condition == Constanst.ConditionStatus.Used &&
                        e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory
                    select e;

                return GetInventoryJson(avaiInventory, Constanst.CarInfoType.Used, sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile);
            });
        }

        private ActionResult ViewLoanerInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            ViewData["inventoryType"] = Constanst.InventoryTab.Loaner;
            SessionHandler.CurrentView = CurrentViewEnum.LoanerInventory.ToString();
            return HandleInventoryViewJson(delegate(DealershipViewModel dealer, VincontrolEntities context)
            {
                var avaiInventory = InventoryQueryHelper.GetSingleOrGroupLoanerInventory(context);
                return GetInventoryJson(avaiInventory, Constanst.CarInfoType.Loaner, sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile);
            });
        }

        private ActionResult ViewTradeNotClearInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            ViewData["inventoryType"] = Constanst.InventoryTab.TradeNotClear;
            SessionHandler.CurrentView = CurrentViewEnum.TradeNotClear.ToString();
            return HandleInventoryViewJson(delegate(DealershipViewModel dealer, VincontrolEntities context)
            {
                var avaiInventory = InventoryQueryHelper.GetSingleOrGroupTradeNotClearInventory(context);
                return GetInventoryJson(avaiInventory, Constanst.CarInfoType.TradeNotClear, sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile);
            });
        }

        private ActionResult ViewAuctionInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            ViewData["inventoryType"] = Constanst.InventoryTab.Auction;
            SessionHandler.CurrentView = CurrentViewEnum.AuctionInventory.ToString();
            return HandleInventoryViewJson(delegate(DealershipViewModel dealer, VincontrolEntities context)
            {
                var avaiInventory = InventoryQueryHelper.GetSingleOrGroupAuctionInventory(context);
                return GetInventoryJson(avaiInventory, Constanst.CarInfoType.Auction, sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile);
            });
        }

        private ActionResult ViewReconInventoryJson(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            ViewData["inventoryType"] = Constanst.InventoryTab.Recon;
            SessionHandler.CurrentView = CurrentViewEnum.ReconInventory.ToString();
            return HandleInventoryViewJson(delegate(DealershipViewModel dealer, VincontrolEntities context)
            {
                var avaiInventory = InventoryQueryHelper.GetSingleOrGroupReconInventory(context);
                return GetInventoryJson(avaiInventory, Constanst.CarInfoType.Recon, sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile);
            });
        }

        #region Filter

        private ActionResult FilterACarInventory(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            using (var context = new VincontrolEntities())
            {
                var list = new List<InventoryInfo>();
                if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                    SessionHandler.CurrentView == CurrentViewEnum.Inventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.Used &&
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory && (e.ACar == true)
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.NewInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.New &&
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory &&
                            (e.ACar != null && e.ACar.Value)
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.LoanerInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.Used &&
                            (e.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner) &&
                            (e.InventoryStatusCodeId != Constanst.InventoryStatus.Recon) &&
                            (e.ACar != null && e.ACar.Value)
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.AuctionInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.Used &&
                            (e.InventoryStatusCodeId == Constanst.InventoryStatus.Auction) &&
                            (e.InventoryStatusCodeId != Constanst.InventoryStatus.Recon) &&
                            (e.ACar != null && e.ACar.Value)
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.ReconInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Recon &&
                            (e.ACar != null && e.ACar.Value)
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.WholesaleInventory.ToString())
                {
                    var avaiInventory =
                        InventoryQueryHelper.GetSingleOrGroupWholesaleInventory(context);
                    list = MapToCarInfoFromVincontrolwholesaleinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.SoldInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                        where
                            (e.Condition == Constanst.ConditionStatus.Used ||
                             e.Condition == Constanst.ConditionStatus.New) &&
                            (e.ACar != null && e.ACar.Value)
                        select e;
                    list =
                        MapToCarInfoFromWhitmanenterprisedealershipinventorysoldout(
                            avaiInventory.ToList());
                }

                if (usingFilter == true)
                {
                    list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
                }

                list = SortCacheInventory(list, sortBy, isUp);
                SessionHandler.InventoryList = list;

                list = list.Take(pageSize).ToList();

                return new LargeJsonResult
                {
                    Data = new
                    {
                        count = SessionHandler.InventoryList.Count,
                        list
                    }
                };
            }
        }

        private ActionResult FilterNoContentInventory(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            var list = BuildFilter();
            list = list.Where(x => x.HealthLevel == 3).ToList();

            if (usingFilter == true)
            {
                list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
            }

            list = SortCacheInventory(list, sortBy, isUp);
            SessionHandler.InventoryList = list;

            list = list.Take(pageSize).ToList();

            return new LargeJsonResult
            {
                Data = new
                {
                    count = SessionHandler.InventoryList.Count,
                    list
                }
            };
        }

        private ActionResult FilterMissingContentInventory(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            var list = BuildFilter();
            list = list.Where(x => x.HealthLevel > 0 && x.HealthLevel < 3).ToList();

            if (usingFilter == true)
            {
                list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
            }

            list = SortCacheInventory(list, sortBy, isUp);
            SessionHandler.InventoryList = list;

            list = list.Take(pageSize).ToList();

            return new LargeJsonResult
            {
                Data = new
                {
                    count = SessionHandler.InventoryList.Count,
                    list
                }
            };
        }

        private ActionResult FilterTodayBucketJumpInventory(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int? fromMile, int? toMile, string store, string bucketView = "")
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false });
            }

            using (var context = new VincontrolEntities())
            {
                var dealer = SessionHandler.Dealer;
                var avaiInventory =from e in InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate())
                    where e.Condition == Constanst.ConditionStatus.Used
                    select e;
                
                var list = new List<InventoryInfo>();
                IEnumerable<Inventory> tempList;
                // filter data by view
                if (!String.IsNullOrEmpty(bucketView))
                {
                    switch (bucketView)
                    {
                        case "LandRover":
                            tempList = avaiInventory.Where(i => i.Vehicle.Make.Equals("Land Rover"));
                            break;
                        case "Jaguar":
                            tempList = avaiInventory.Where(i => i.Vehicle.Make.Equals("Jaguar"));
                            break;
                        case "AL":

                            tempList = avaiInventory.AsEnumerable().Where(i => Regex.IsMatch(i.Vehicle.Make.Replace(" ", "").Replace("-",""), @"^[a-lA-L]\w+$") && !i.Vehicle.Make.Equals("Land Rover") && !i.Vehicle.Make.Equals("Jaguar"));
                            break;
                        case "MZ":
                           tempList = avaiInventory.AsEnumerable().Where(i => Regex.IsMatch(i.Vehicle.Make.Replace(" ","").Replace("-",""), @"^[m-zM-Z]\w+$"));
                            break;
                        case "GroupTodayBucketJump":
                            tempList = avaiInventory.AsEnumerable();
                            break;
                        case "Saved":
                            tempList = avaiInventory.Where(i => i.SavedBucketJump.HasValue && i.SavedBucketJump.Value);
                            break;
                        default:
                            tempList = avaiInventory.Where(i => i.Vehicle.Make.Equals("Land Rover"));
                            break;

                    }
                }
                else
                {
                    tempList = avaiInventory.AsEnumerable();
                }

                foreach (var tmp in tempList)
                {
                    var dateatMidnight = new DateTime(tmp.DateInStock.GetValueOrDefault().Year,
                        tmp.DateInStock.GetValueOrDefault().Month,
                        tmp.DateInStock.GetValueOrDefault().Day);

                    var daysInInventory = DateTime.Now.Subtract(dateatMidnight).Days;
                    var bucketDay = tmp.BucketJumpCompleteDay.GetValueOrDefault();
                    var nextBucketDay = 0;
                    if (bucketDay == 0 || bucketDay < dealer.DealerSetting.FirstTimeRangeBucketJump)
                        nextBucketDay = dealer.DealerSetting.FirstTimeRangeBucketJump;
                    else if (bucketDay < dealer.DealerSetting.SecondTimeRangeBucketJump)
                        nextBucketDay = dealer.DealerSetting.SecondTimeRangeBucketJump;
                    else if (bucketDay >= dealer.DealerSetting.SecondTimeRangeBucketJump)
                        nextBucketDay = bucketDay + dealer.DealerSetting.IntervalBucketJump;

                    var flag = /*bucketDay == 0 || */ nextBucketDay <= daysInInventory;

                    if (!flag) continue;
                    var car = new InventoryInfo(tmp)
                    {
                        NotDoneBucketJump = true,
                        Type = Constanst.CarInfoType.Used
                    };
                    list.Add(car);
                }

                if (usingFilter)
                {
                    list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
                    list = list.Where(x => x.Dealer == store).ToList();
                }

                list = SortCacheInventory(list, sortBy, isUp);
                SessionHandler.InventoryList = list;

                list = list.Take(pageSize).ToList();

                return new LargeJsonResult
                {
                    Data = new
                    {
                        count = SessionHandler.InventoryList.Count,
                        list
                    }
                };
            }
        }

        private ActionResult FilterExpressBucketJumpInventory(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int fromMile, int toMile)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false });
            }

            using (var context = new VincontrolEntities())
            {
                var list =
                    InventoryQueryHelper.GetSingleOrGroupInventory(context, true, true, true).Where(CommonHelper.IsInventoryPredicate())
                        .Where(e => e.Condition == Constanst.ConditionStatus.Used)
                        .ToList()
                        .Select(tmp => new InventoryInfo(tmp) { Type = Constanst.CarInfoType.Used });

                if (usingFilter == true)
                {
                    list = FilterInventory(list.ToList(), make, model, trim, year, price, fromMile, toMile);
                }

                list = SortCacheInventory(list.ToList(), sortBy, isUp);
                SessionHandler.InventoryList = list.ToList();

                list = list.Take(pageSize);

                return new LargeJsonResult
                {
                    Data = new
                    {
                        count = SessionHandler.InventoryList.Count,
                        list
                    }
                };
            }
        }

        private ActionResult FilterSavedBucketJumpInventory(string sortBy, bool isUp, int pageSize, bool usingFilter, string make, string model, string trim, string year, int price, int? fromMile, int? toMile,string store, string screen)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false });
            }

            using (var context = new VincontrolEntities())
            {
                //var dealer = SessionHandler.Dealer;
                var avaiInventory =
                    from e in InventoryQueryHelper.GetSingleOrGroupInventory(context).Where(CommonHelper.IsInventoryPredicate())
                    where e.Condition == Constanst.ConditionStatus.Used
                    select e;

                var list = new List<InventoryInfo>();
                avaiInventory = avaiInventory.Where(i => i.SavedBucketJump.HasValue && i.SavedBucketJump.Value).OrderByDescending(x=>x.SavedBucketJumpDate);
                foreach (var item in avaiInventory)
                {
                    var inventoryInfo = new InventoryInfo(item)
                    {
                        NotDoneBucketJump = false,
                        Type = Constanst.CarInfoType.Used,
                        
                    };
                    var massBucketJump = _inventoryManagementForm.GetLatestBucketJumpHistory(item.InventoryId,Constanst.VehicleStatus.Inventory);
                    if (massBucketJump != null)
                    {
                        //inventoryInfo.MassBucketJumpCertified = massBucketJump.Certified.GetValueOrDefault();
                        //inventoryInfo.MassBucketJumpCertifiedAmount = massBucketJump.CertifiedAmount.GetValueOrDefault();
                        //inventoryInfo.MassBucketJumpACar = massBucketJump.ACar.GetValueOrDefault();
                        //inventoryInfo.MassBucketJumpACarAmount = massBucketJump.ACarAmount.GetValueOrDefault();
                        inventoryInfo.UserStamp = massBucketJump.UserFullName;
                        list.Add(inventoryInfo);
                    }

                   
                }
                
                if (usingFilter)
                {
                    list = FilterInventory(list, make, model, trim, year, price, fromMile, toMile);
                    list = list.Where(x => x.Dealer == store).ToList();
                }

                //list = SortCacheInventory(list, "bucketJumpDate", isUp);
                SessionHandler.InventoryList = list;

                list = list.Take(pageSize).ToList();

                return new LargeJsonResult
                {
                    Data = new
                    {
                        count = SessionHandler.InventoryList.Count,
                        list
                    }
                };
            }
        }

        public int DoneTodayBucketJump(int listingId, string day)
        {
            var dealer = (DealershipViewModel)Session["Dealership"];

            var convertedId = Convert.ToInt32(listingId);
            var convertedDay = Convert.ToInt32(day);

            //if (!_inventoryManagementForm.CheckMassBucketJumpExisting(convertedId)) return -1;

            using (var context = new VincontrolEntities())
            {
                var inventory = context.Inventories.FirstOrDefault(i => i.InventoryId == convertedId);
                if (inventory != null)
                {

                    var remain = convertedDay % dealer.DealerSetting.IntervalBucketJump;

                    inventory.BucketJumpCompleteDay = convertedDay < dealer.DealerSetting.IntervalBucketJump ? dealer.DealerSetting.IntervalBucketJump : convertedDay - remain;
                    inventory.SavedBucketJump = true;
                    inventory.SavedBucketJumpDate = DateTime.Now;
                    // Saved bucket jump
                    //if (_inventoryManagementForm.CheckMassBucketJumpExisting(listingId))
                    //{
                    //    inventory.SavedBucketJump = true;
                    //    inventory.SavedBucketJumpDate = DateTime.Now;
                    //}
                    context.SaveChanges();

                    var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var bucketJumpReportFile = context.BucketJumpHistories.FirstOrDefault(i => i.ListingId == listingId && i.VehicleStatusCodeId == Constanst.VehicleStatus.Inventory && i.DateStamp > todayDate);
                    if (bucketJumpReportFile != null)
                    {
                        //bucketJumpReportFile.BucketJumpDayAlert = convertedDay - remain;
                        //bucketJumpReportFile.BucketJumpCompleteDate = DateTime.Now;

                        var emailWaitingList = new EmailWaitingList()
                        {
                            AppraisalId = inventory.AppraisalId,
                            ListingId = inventory.InventoryId,
                            DateStamp = DateTime.Now,
                            Expire = false,
                            FileUrl = bucketJumpReportFile.AttachFile,
                            NotificationTypeCodeId = Constanst.NotificationType.BucketJump,
                            OldValue = inventory.SalePrice,
                            NewValue = bucketJumpReportFile.RetailPrice,
                            UserId = SessionHandler.CurrentUser.UserId

                        };

                        _vehicleLogForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                            Constanst.VehicleLogSentence.BucketJumpDoneByUser.Replace("OLDPRICE",
                                inventory.SalePrice.GetValueOrDefault().ToString("C0"))
                                .Replace("NEWPRICE", bucketJumpReportFile.RetailPrice.GetValueOrDefault().ToString("C0"))
                                .Replace("USER", SessionHandler.CurrentUser.FullName), null);

                        _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);
                        context.SaveChanges();


                    }

                    else
                    {
                        var bucketJumpHistory = new vincontrol.Data.Model.BucketJumpHistory
                        {
                            UserStamp = SessionHandler.CurrentUser.Username,
                            UserFullName = SessionHandler.CurrentUser.FullName,
                            DealerId = SessionHandler.Dealer.DealershipId,
                            Store = SessionHandler.Dealer.DealershipName,
                            DateStamp = DateTime.Now,
                            ListingId = listingId,
                            VIN = inventory.Vehicle.Vin,
                            Stock = inventory.Stock,
                            SalePrice = inventory.SalePrice,
                            RetailPrice = inventory.SalePrice + inventory.CertifiedAmount.GetValueOrDefault() + inventory.MileageAdjustment.GetValueOrDefault(),
                            VehicleStatusCodeId = Constanst.VehicleStatus.Inventory,
                            DaysAged = DateTime.Now.Subtract(inventory.DateInStock.GetValueOrDefault()).Days,
                            BucketJumpDayAlert = convertedDay - remain,
                            BucketJumpCompleteDate = DateTime.Now
                        };

                        var emailWaitingList = new EmailWaitingList()
                        {
                            AppraisalId = inventory.AppraisalId,
                            ListingId = inventory.InventoryId,
                            DateStamp = DateTime.Now,
                            Expire = false,
                            FileUrl = bucketJumpHistory.AttachFile,
                            NotificationTypeCodeId = Constanst.NotificationType.BucketJump,
                            OldValue = inventory.SalePrice,
                            NewValue = bucketJumpHistory.RetailPrice,
                            UserId = SessionHandler.CurrentUser.UserId

                        };

                        _vehicleLogForm.AddVehicleLog(listingId, SessionHandler.CurrentUser.UserId,
                            Constanst.VehicleLogSentence.BucketJumpDoneByUser.Replace("OLDPRICE",
                                inventory.SalePrice.GetValueOrDefault().ToString("C0"))
                                .Replace("NEWPRICE", bucketJumpHistory.RetailPrice.GetValueOrDefault().ToString("C0"))
                                .Replace("USER", SessionHandler.CurrentUser.FullName), null);

                        _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);
                     
                        context.AddToBucketJumpHistories(bucketJumpHistory);
                        context.SaveChanges();
                    }
                }
            }

            return 0;
        }

        #endregion

        #endregion

        #region Helpers

       
        private ActionResult HandleInventoryViewJson(RetrieveInventoryFunction retrieveInventoryJson)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(Constanst.Message.Unauthorized);
            }
            return retrieveInventoryJson(SessionHandler.Dealer, new VincontrolEntities());
        }


        private delegate ActionResult RetrieveInventoryFunction(DealershipViewModel dealer, VincontrolEntities context);

        


        #endregion

        #region Filter

        public string BuildInventory(List<CarInfoFormViewModel> cars, bool isGrid)
        {
            if (cars.Count == 0)
            {
                return string.Empty;
            }

            var _params = new Dictionary<string, object>();
            foreach (var car in cars)
            {
                if (car.Type == Constanst.CarInfoType.Wholesale)
                {
                    car.ClassFilter = "Wholesale";
                    car.URLDetail = Url.Action("ViewIProfile",
                        "Inventory", new { ListingID = car.ListingId });

                    car.URLEdit = Url.Action("ViewIProfile",
                        "Inventory", new { ListingID = car.ListingId });

                    car.UrlImage = ImageLink("ViewIProfile", car.SinglePhoto, "Inventory",
                        new { ListingID = car.ListingId }, null,
                        new { Alt = "Image" });
                }
                else if (car.Type == Constanst.CarInfoType.Sold)
                {
                    car.ClassFilter = "Sold";
                    car.URLDetail = Url.Action("ViewISoldProfile",
                        "Inventory", new { ListingID = car.ListingId });
                    car.URLEdit = Url.Action("ViewISoldProfile",
                        "Inventory", new { ListingID = car.ListingId });
                    if (car.ListingId == 45133)
                    {
                        car.UrlImage = ImageLink("ViewISoldProfile", car.SinglePhoto, "Inventory",
                            new { ListingID = car.ListingId }, null,
                            new { Alt = "Image" });
                    }
                    else
                    {
                        car.UrlImage = ImageLink("ViewISoldProfile", car.SinglePhoto, "Inventory",
                            new { ListingID = car.ListingId }, null,
                            new { Alt = "Image" });
                    }
                }
                else if (car.Type == Constanst.CarInfoType.Recent || car.Type == Constanst.CarInfoType.Pending)
                {
                    car.ClassFilter = "Appraisals";
                    car.URLDetail = Url.Action("ViewProfileForAppraisal",
                        "Appraisal", new { appraisalId = car.ListingId });
                    car.URLEdit = Url.Action("ViewProfileForAppraisal",
                        "Appraisal", new { appraisalId = car.ListingId });

                    car.UrlImage = ImageLink("ViewProfileForAppraisal", car.SinglePhoto, "Appraisal",
                        new { ListingID = car.ListingId }, null,
                        new { Alt = "Image" });
                }
                else
                {
                    if (car.Type == Constanst.CarInfoType.New)
                    {
                        car.ClassFilter = "New";
                    }
                    else if (car.Type == Constanst.CarInfoType.Used)
                    {
                        car.ClassFilter = "Used";
                    }
                    car.URLDetail = Url.Action("ViewIProfile",
                        "Inventory", new { ListingID = car.ListingId });
                    car.URLEdit = Url.Action("EditIProfile",
                        "Inventory", new { ListingID = car.ListingId });

                    car.UrlImage = ImageLink("ViewIProfile", car.SinglePhoto, "Inventory",
                        new { ListingID = car.ListingId }, null,
                        new { Alt = "Image" });
                }

                if (car.CarRanking == 0 && car.NumberOfCar == 0)
                    car.MarketData = "NA";
                else
                    car.MarketData = string.Format("{0}/{1}", car.CarRanking, car.NumberOfCar);

                if (!String.IsNullOrEmpty(car.Make) && car.Make.Length > 10)
                    car.ShortMake = car.Make.Substring(0, 9);
                else
                {
                    car.ShortMake = car.Make;
                }

                if (!String.IsNullOrEmpty(car.Model) && car.Model.Length > 10)
                    car.ShortModel = car.Model.Substring(0, 9);
                else
                {
                    car.ShortModel = car.Model;
                }

                if (!String.IsNullOrEmpty(car.Vin) && car.Vin.Length > 8)
                    car.SortVin = car.Vin.Substring(car.Vin.Length - 8, 8);
                else
                {
                    car.SortVin = car.Vin;
                }

                if (!String.IsNullOrEmpty(car.Trim) && car.Trim.Length > 7)
                    car.StrTrim = car.Trim.Substring(0, 8);
                else
                    car.StrTrim = car.Trim;

                if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 6)
                    car.StrExteriorColor = car.ExteriorColor.Substring(0, 7);
                else
                    car.StrExteriorColor = car.ExteriorColor;

                if (car.CarFaxOwner < 1)
                    car.StrCarFaxOwner = "NA";
                else if (car.CarFaxOwner == 1)
                    car.StrCarFaxOwner = car.CarFaxOwner + " Owner";
                else
                    car.StrCarFaxOwner = car.CarFaxOwner + " Owners";

                if (car.DaysInInvenotry == 1)
                    car.StrDaysInInvenotry = car.DaysInInvenotry + " Day";
                else
                {
                    car.StrDaysInInvenotry = car.DaysInInvenotry + " Days";
                }

                car.URLEbay = Url.Action("ViewEbay", "Market", new { car.ListingId });

                car.URLBG = Url.Action("ViewBuyerGuide", "Report", new { car.ListingId });

                car.URLWS = Url.Action("PrintSticker", "PDF", new { car.ListingId });

                #region Mileage

                long odometerNumber;
                bool odometerFlag = Int64.TryParse(car.Mileage.ToString(CultureInfo.InvariantCulture),
                    out odometerNumber);
                car.StrMileage = odometerFlag
                    ? odometerNumber.ToString("#,##0")
                    : car.Mileage.ToString(CultureInfo.InvariantCulture);

                #endregion

                #region Price

                decimal salePriceNumber;
                bool salePriceFlag = Decimal.TryParse(car.SalePrice.ToString(CultureInfo.InvariantCulture),
                    out salePriceNumber);
                car.StrSalePrice = salePriceFlag
                    ? salePriceNumber.ToString("#,##0")
                    : car.SalePrice.ToString(CultureInfo.InvariantCulture);

                #endregion
            }
            
            _params.Add("Inventories", cars);
            _params.Add("AllowBG", SessionHandler.UserRight.Inventory.ViewProfile_BG);
            _params.Add("AllowWS", SessionHandler.UserRight.Inventory.ViewProfile_WS);
            _params.Add("AllowEbay", SessionHandler.UserRight.Inventory.ViewProfile_Ebay);
            _params.Add("AllowCraigslist", SessionHandler.UserRight.Inventory.Craigslist);

            string content = isGrid
                ? NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\Inventory\InventoryGrid.htm", _params)
                    .ToString()
                : NVelocityExtension.NVelocityHelper.GetHTMLTemplate(@"\Inventory\Inventory.htm", _params).ToString();

            return content;
        }

        private List<InventoryInfo> BuildFilter()
        {
            using (var context = new VincontrolEntities())
            {
                var list = new List<InventoryInfo>();
                if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                    SessionHandler.CurrentView == CurrentViewEnum.Inventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.Used &&
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.NewInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.New &&
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Inventory
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.LoanerInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Loaner
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.AuctionInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Auction
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.ReconInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupInventory(context)
                        where
                            e.InventoryStatusCodeId == Constanst.InventoryStatus.Recon
                        select e;
                    list = MapToCarInfoFromWhitmanenterprisedealershipinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.WholesaleInventory.ToString())
                {
                    var avaiInventory =
                        InventoryQueryHelper.GetSingleOrGroupWholesaleInventory(context);
                    list = MapToCarInfoFromVincontrolwholesaleinventory(avaiInventory.ToList());
                }
                else if (!string.IsNullOrEmpty(SessionHandler.CurrentView) &&
                         SessionHandler.CurrentView == CurrentViewEnum.SoldInventory.ToString())
                {
                    var avaiInventory =
                        from e in InventoryQueryHelper.GetSingleOrGroupSoldoutInventory(context)
                        where
                            e.Condition == Constanst.ConditionStatus.Used ||
                            e.Condition == Constanst.ConditionStatus.New
                        select e;
                    list =
                        MapToCarInfoFromWhitmanenterprisedealershipinventorysoldout(
                            avaiInventory.ToList());
                }

                return list;
            }
        }

        private List<InventoryInfo> MapToCarInfoFromWhitmanenterprisedealershipinventory(List<Inventory> inventories)
        {
            return inventories.Select(tmp => new InventoryInfo(tmp) { Type = Constanst.CarInfoType.Used }).ToList();
        }

        private List<InventoryInfo> MapToCarInfoFromVincontrolwholesaleinventory(List<Inventory> inventories)
        {
            return inventories.Select(tmp => new InventoryInfo(tmp) { Type = Constanst.CarInfoType.Wholesale }).ToList();
        }

        private List<InventoryInfo> MapToCarInfoFromWhitmanenterprisedealershipinventorysoldout(
            IEnumerable<SoldoutInventory> inventories)
        {
            var list = new List<InventoryInfo>();
            if (SessionHandler.Dealer == null)
            {
                return list;
            }
            list.AddRange(from tmp in inventories.ToList()
                          let daysInInventory = DateTime.Now.Subtract(tmp.DateRemoved.GetValueOrDefault()).Days
                          where daysInInventory <= 30
                          select new InventoryInfo(tmp) { Type = Constanst.CarInfoType.Sold });
            return list;
        }

        #endregion

        #region Utilities

        private string ImageLink(string actionName, string imageUrl, string alternateText, object routeValues,
            object linkHtmlAttributes, object imageHtmlAttributes)
        {
            var url = Url.Action(actionName, routeValues);

            // Create link
            var linkTagBuilder = new TagBuilder("a");
            linkTagBuilder.MergeAttribute("href", url);
            linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            // Create image

            var imageTagBuilder = new TagBuilder("img");
            if (!String.IsNullOrEmpty(imageUrl))
                imageTagBuilder.MergeAttribute("src", Url.Content(imageUrl));
            imageTagBuilder.MergeAttribute("alt", Url.Encode(alternateText));
            imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

            // Add image to link
            linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

            return linkTagBuilder.ToString();
        }

        public ActionResult CheckStockNumber(int listingId, string stock)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false });
            }
            if (string.IsNullOrEmpty(stock))
            {
                return Json(new { success = true });
            }
            using (var context = new VincontrolEntities())
            {
                var dealer = SessionHandler.Dealer;

                var item =
                    context.Inventories.FirstOrDefault(
                        x => x.InventoryId != listingId && x.DealerId == dealer.DealershipId && x.Stock == stock);
                if (item == null)
                    return Json(new { success = true });
                return Json(new { success = false });
            }
        }

        public ActionResult CheckStockNumberExist(string stock)
        {
            if (SessionHandler.Dealer == null)
            {
                return Json(new { success = false });
            }
            if (string.IsNullOrEmpty(stock))
            {
                return Json(new { success = true });
            }
            using (var context = new VincontrolEntities())
            {
                var dealer = SessionHandler.Dealer;

                var item =
                    context.Inventories.FirstOrDefault(
                        x => x.DealerId == dealer.DealershipId && x.Stock == stock);
                if (item == null)
                    return Json(new { success = true });
                return Json(new { success = false });
            }
        }

        #endregion

        public ActionResult CustomerInfo(int listingId)
        {
            if (SessionHandler.Dealer == null) return Json("SessionTimeOut");
            var row = _inventoryManagementForm.GetSoldInventory(listingId);
            var viewModel = new CustomeInfoModel
            {
                ListingId = listingId,
                States = SelectListHelper.InitialStateList(),
                Countries = SelectListHelper.InitialCountryList()
            };
            if (row != null)
            {
                viewModel = new CustomeInfoModel
                {
                    ListingId = listingId,
                    States = SelectListHelper.InitialStateList(),
                    Countries = SelectListHelper.InitialCountryList(),
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Country = row.Country,
                    Address = row.Address,
                    ZipCode = row.ZipCode,
                    City = row.City,
                    State = row.State,
                    Street = row.Street
                };
            }
            return View("CustomerInfo", viewModel);
        }

        public ActionResult UpdateCustomerInfo(CustomeInfoModel customeInfo)
        {
            _inventoryManagementForm.UpdateCustomerInfoForSold(customeInfo.ListingId, customeInfo.FirstName, customeInfo.LastName, customeInfo.Address, customeInfo.Street
                ,customeInfo.City,customeInfo.State,customeInfo.ZipCode);
            return Json("Success");
        }

        public ActionResult OpenStatus(int listingId, short inventoryStatus, bool isSoldOut)
        {
            if (SessionHandler.Dealer != null)
            {
                var context = new VincontrolEntities();
                string title = string.Empty;
                string currentStatus = string.Empty;
                if (inventoryStatus == Constanst.InventoryStatus.SoldOut)
                    currentStatus = Constanst.InventoryStatusText.SoldOut;
                else if (inventoryStatus == Constanst.InventoryStatus.Inventory)
                    currentStatus = Constanst.InventoryStatusText.Inventory;
                else if (inventoryStatus == Constanst.InventoryStatus.Wholesale)
                    currentStatus = Constanst.InventoryStatusText.Wholesale;
                else if (inventoryStatus == Constanst.InventoryStatus.Recon)
                    currentStatus = Constanst.InventoryStatusText.Recon;
                else if (inventoryStatus == Constanst.InventoryStatus.Auction)
                    currentStatus = Constanst.InventoryStatusText.Auction;
                else if (inventoryStatus == Constanst.InventoryStatus.Loaner)
                    currentStatus = Constanst.InventoryStatusText.Loaner;
                else if (inventoryStatus == Constanst.InventoryStatus.TradeNotClear)
                    currentStatus = Constanst.InventoryStatusText.TradeNotClear;
                if (isSoldOut)
                {
                    var row =
                        context.SoldoutInventories.Include("Vehicle").FirstOrDefault(
                            x => x.SoldoutInventoryId == listingId);

                    title =
                        string.Format("{0} {1} {2} {3}", row.Vehicle.Year, row.Vehicle.Make,
                                      row.Vehicle.Model, row.Vehicle.Trim);
                }
                else
                {
                    var rowInventory =_inventoryManagementForm.GetInventory(listingId);
                    if (rowInventory != null)
                    {
                        title =
                            string.Format("{0} {1} {2} {3}", rowInventory.Vehicle.Year, rowInventory.Vehicle.Make,
                                          rowInventory.Vehicle.Model, rowInventory.Vehicle.Trim);
                    }
                }
                var viewModel = new StatusInfoModel()
                                    {
                                        ListingID = listingId,
                                        CurrentStatusID = inventoryStatus,
                                        Title = title,
                                        CurrentStatus = currentStatus,
                                        ListStatus = SelectListHelper.InitialStatusList(inventoryStatus)
                                    };

                return View(viewModel);
            }
            return Json("SessionTimeOut");
        }

        public ActionResult DownloadBrochure(int year, string make, string model, int index = 0)
        {

            try
            {

                string path = System.Web.HttpContext.Current.Server.MapPath("\\PDF Files") + "\\" + "Brochures" +
                              "\\" +
                              year + "\\" + make +
                              "\\" + model;
                var dir = new DirectoryInfo(path);
                if (!dir.Exists) return File(new byte[] {}, "application/pdf");
              
                FileInfo file;
                if (index > 0)
                    file = dir.GetFiles()[1];
                else
                    file = dir.GetFiles()[0];

                return File(file.FullName, "application/pdf",
                    String.Format("{0}_{1}_{2}_brochure.pdf", year, make, model));
            
            }
            catch (Exception)
            {
                return File(new byte[] {}, "application/pdf");
            }

            return null;
        }

        public ActionResult ViewBrochure(int year, string make, string model, int index)
        {

            try
            {

                string path = System.Web.HttpContext.Current.Server.MapPath("\\PDF Files") + "\\" + "Brochures" +
                              "\\" +
                              year + "\\" + make +
                              "\\" + model;
                var dir = new DirectoryInfo(path);
                if (!dir.Exists)
                    return RedirectToAction("EmptyBrochure", "Error");

                FileInfo file = dir.GetFiles()[0];

                if (index > 0)
                    return File(path + "\\" + String.Format("{0}_{1}_{2}_{3}.pdf", year, make, model, index),
                        "application/pdf");
                else
                {
                    return File(path + "\\" + String.Format("{0}_{1}_{2}.pdf", year, make, model), "application/pdf");
                }
                
            }
            catch (Exception)
            {
                return File(new byte[] {}, "application/pdf");
            }

            return null;
        }

        public ActionResult CheckBrochure(int year, string make, string model, int index)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("\\PDF Files") + "\\" + "Brochures" +
                          "\\" +
                          year + "\\" + make +
                          "\\" + model;
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
                return Json(new { isExisted = false });
            return Json(new { isExisted = true });
        }

        public void SendBrochure(int year, string make, string model, string email, string name, string photoUrl,
            int index)
        {

            var emailWaitingList = new EmailWaitingList()
            {
                DateStamp = DateTime.Now,
                Expire = false,
                NotificationTypeCodeId = Constanst.NotificationType.SendBrochure,
                UserId = SessionHandler.CurrentUser.UserId,
                Year = year,
                Make = make,
                Model = model,
                CustomerEmails = email

            };

            _emailWaitingForm.AddNewEmailWaitingList(emailWaitingList);

        }

    
        public byte[] GenerateFlyerByteContent(string htmlToConvert, string dealerName)
        {
            //// instantiate the HiQPdf HTML to PDF converter
            var htmlToPdfConverter = new HtmlToPdf();

            PDFHelper.ConfigureConverter(htmlToPdfConverter);
            PdfDocument pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocument(htmlToConvert, null);
            FormatHeader(pdfDocument, dealerName);
            return (pdfDocument.WriteToMemory());
        }

        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName)
        {
            FormatHeader(pdfDocument, dealershipName, true);
        }

        private static void FormatHeader(PdfDocument pdfDocument, string dealershipName, bool showDateTime)
        {
            pdfDocument.Header = pdfDocument.CreateHeaderCanvas(pdfDocument.Pages[0].DrawableRectangle.Width, 10);
            var sysFont = new Font("Times New Roman", 10, GraphicsUnit.Point);
            //pdfDocument.CreateFont(sysFont);
            PdfFont pdfFontEmbed = pdfDocument.CreateFont(sysFont, true);

            if (showDateTime)
            {
                pdfDocument.Header.Layout(new PdfText { Text = DateTime.Now.ToShortDateString(), TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Right });
            }
            pdfDocument.Header.Layout(new PdfText { Text = dealershipName, TextFont = pdfFontEmbed, HorizontalAlign = PdfTextHAlign.Center });
        }


        [HttpPost]
        public ActionResult GetMakesFromChrome(int year)
        {
            List<ExtendedSelectListItem> listMake = _commonManagementForm.GetChromeMake(year);
            return PartialView(listMake);
        }

        [HttpPost]
        public ActionResult GetModelsFromChrome(int year, int makeID)
        {
            List<ExtendedSelectListItem> listModel = _commonManagementForm.GetChromeModel(year, makeID);
            return PartialView(listModel);
        }

        [HttpPost]
        public ActionResult GetImageFromChrome(int year, int makeID, int modelID, string make, string model)
        {
            string photoUrl = string.Empty;
            List<ExtendedSelectListItem> listTrim = _commonManagementForm.GetChromeTrim(year, makeID, modelID);
            if (listTrim.Count > 1)
            {
                photoUrl = listTrim[1].ImageUrl;
            }

            string path = System.Web.HttpContext.Current.Server.MapPath("\\PDF Files") + "\\" + "Brochures" +
                          "\\" +
                          year + "\\" + make +
                          "\\" + model;
            var dir = new DirectoryInfo(path);
            int count = 0;
            if (dir.Exists)
                count = dir.GetFiles().Count();

            if (string.IsNullOrEmpty(photoUrl))
                photoUrl = string.Format("{0}/Content/images/vincontrol/car_default.png",
                                         System.Web.Configuration.WebConfigurationManager.AppSettings["WebServerURL"
                                             ]);

            return Json(new { photoUrl = photoUrl, photoEndCodeUrl = HttpUtility.UrlEncode(photoUrl), brochureFiles = count });
        }

        [CompressFilter(Order = 1)]
        [CacheFilter(Order = 2)]
        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewInventoryJson(string screen, string sortBy, bool isUp, int pageSize, string fromDate, string toDate, bool usingFilter = false, string make = "", string model = "", string trim = "", string year = "", int price = 0, int? fromMile = null, int? toMile = null, string store = "")
        {
            switch (screen)
            {
                case "Inventory":
                    return ViewUsedInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "AuctionInventory":
                    return ViewAuctionInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "NewInventory":
                    return ViewNewInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "LoanerInventory":
                    return ViewLoanerInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "ReconInventory":
                    return ViewReconInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "WholesaleInventory":
                    return ViewWholeSaleInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "TradeNotClear":
                    return ViewTradeNotClearInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "SoldInventory":
                    return ViewSoldInventoryJson(null, sortBy, isUp, pageSize, fromDate, toDate, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "TodayBucketJump":
                    return FilterTodayBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile,store);
                case "ExpressBucketJump":
                    return FilterExpressBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "LandRover":
                    return FilterTodayBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile,store, BucketJumpView.LandRover.ToString());
                case "Jaguar":
                    return FilterTodayBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile, store, BucketJumpView.Jaguar.ToString());
                case "AL":
                    return FilterTodayBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile, store, BucketJumpView.AL.ToString());
                case "MZ":
                    return FilterTodayBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile, store, BucketJumpView.MZ.ToString());
                case "GroupTodayBucketJump":
                    return FilterTodayBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile, store, BucketJumpView.GroupTodayBucketJump.ToString());
                case "Saved":
                    return FilterSavedBucketJumpInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile, toMile,store, BucketJumpView.Saved.ToString());
                case "ACar":
                    return FilterACarInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "MissingContent":
                    return FilterMissingContentInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "NoContent":
                    return FilterNoContentInventory(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "NewSold":
                    return ViewSoldInventoryJson(false, sortBy, isUp, pageSize, fromDate, toDate, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                case "UsedSold":
                    return ViewSoldInventoryJson(true, sortBy, isUp, pageSize, fromDate, toDate, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
                default:
                    return ViewUsedInventoryJson(sortBy, isUp, pageSize, usingFilter, make, model, trim, year, price, fromMile ?? 0, toMile ?? 0);
            }
        }

        [VinControlAuthorization(PermissionCode = "INVENTORY", AcceptedValues = "READONLY, ALLACCESS")]
        public ActionResult ViewCacheInventoryJson(int pageIndex, int pageSize)
        {
            var result = SessionHandler.InventoryList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new LargeJsonResult
            {
                Data = new
                {
                    count = SessionHandler.InventoryList.Count,
                    list = result,
                }
            };
        }

        private List<InventoryInfo> SortCacheInventory(List<InventoryInfo> list, string sortBy, bool isUp)
        {
            List<InventoryInfo> result;

            switch (sortBy)
            {
                case "year":
                    result = isUp ? list.OrderBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "make":
                    result = isUp ? list.OrderBy(x => x.Make).ThenBy(x => x.Year).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Make).ThenByDescending(x => x.Year).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "model":
                    result = isUp ? list.OrderBy(x => x.Model).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Model).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "trim":
                    result = isUp ? list.OrderBy(x => x.Trim).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Trim).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "age":
                    result = isUp ? list.OrderBy(x => x.DaysInInvenotry).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "market":
                    result = isUp ? list.OrderBy(x => x.MarketRange).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ToList() : list.OrderByDescending(x => x.MarketRange).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ToList();
                    break;
                case "miles":
                    result = isUp ? list.OrderBy(x => x.Mileage).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Mileage).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "price":
                    result = isUp ? list.OrderBy(x => x.SalePrice).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.SalePrice).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "color":
                    result = isUp ? list.OrderBy(x => x.ExteriorColor).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.ExteriorColor).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "stock":
                    result = isUp ? list.OrderBy(x => x.Stock).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Stock).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "vin":
                    result = isUp ? list.OrderBy(x => x.Vin).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.Vin).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "owners":
                    result = isUp ? list.OrderBy(x => x.CarFaxOwner).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.CarFaxOwner).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "certified":
                    result = isUp ? list.OrderByDescending(x => x.Certified).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderBy(x => x.Certified).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                case "bucketJumpDate":
                    result = isUp ? list.OrderBy(x => x.SavedBucketJumpDate).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ThenBy(x => x.DaysInInvenotry).ThenBy(x => x.MarketRange).ToList() : list.OrderByDescending(x => x.SavedBucketJumpDate).ThenByDescending(x => x.Year).ThenByDescending(x => x.Make).ThenByDescending(x => x.Model).ThenByDescending(x => x.Trim).ThenByDescending(x => x.DaysInInvenotry).ThenByDescending(x => x.MarketRange).ToList();
                    break;
                default:
                   
                    result = list.OrderBy(x => x.DaysInInvenotry).ThenBy(x => x.Year).ThenBy(x => x.Make).ThenBy(x => x.Model).ThenBy(x => x.Trim).ToList();
                   
                    break;
            }

            return result;
        }

        public ActionResult WindowSticker(int listingId)
        {
            ViewData["listingId"] = listingId;
            return View();
        }

        public ActionResult GetWSTemplate()
        {

            var templates = _dealerManagementForm.GetDealerWindowStickerTemplate(SessionHandler.Dealer.DealershipId)
                .Select(t => new
                {
                    templateId = t.TemplateId,
                    templateName = t.WindowStickerTemplate.TemplateName,
                    templateUrl = t.PhotoURL
                });


            return Json(templates.ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult PrintStickerWithTemplate(int listingId, int templateId)
        {
            var template = _dealerManagementForm.GetDealerWindowStickerTemplate(SessionHandler.Dealer.DealershipId,
                templateId);
            return RedirectToAction("PrintStickerWithTemplate", "PDF", new {listingId, templateUrl = template.PhotoURL});

        }

        public ActionResult AutoLoanCalculator(int inventoryId, int type)
        {
           
            CarInfoFormViewModel inventoryViewModel;

            if (type == Constanst.CarInfoType.Sold)
            {
                var inventory = _inventoryManagementForm.GetSoldInventory(inventoryId);
            
                inventoryViewModel = inventory == null
                    ? new CarInfoFormViewModel()
                    : new CarInfoFormViewModel(inventory);
            }
            else
            {
                var inventory = _inventoryManagementForm.GetInventory(inventoryId);
                inventoryViewModel = inventory == null
                    ? new CarInfoFormViewModel()
                    : new CarInfoFormViewModel(inventory);
            }

            

            var autoLoanModel = new AutoLoanCalculatorModel()
            {
                DownPayment =(double) inventoryViewModel.SalePrice*.2,
                InterestRate = 10,
                SaleTax = 10,
                Terms = 60,
                TradeInValue = 0,
                VehiclePrice = (double)inventoryViewModel.SalePrice

            };
            
            return View(autoLoanModel);
        }

        public ActionResult AutoLoanCalculatorSummary(AutoLoanCalculatorModel model)
        {
           var autoLoanModel = new AutoLoanCalculatorModel()
            {
                MonthlyPayment =Math.Round( AutoLoanHelper.MonthlyPayment(model.VehiclePrice, model.SaleTax, model.InterestRate, model.DownPayment,model.TradeInValue, model.Terms),2),
                DownPayment = model.DownPayment,
                InterestRate = model.InterestRate,
                SaleTax = model.SaleTax,
                Terms = model.Terms,
                TradeInValue = model.TradeInValue,
                VehiclePrice = model.VehiclePrice,
                Principal = model.VehiclePrice - model.TradeInValue - model.DownPayment,
                
            };


            autoLoanModel.TotalInterest = (autoLoanModel.MonthlyPayment*autoLoanModel.Terms) - autoLoanModel.Principal;
            autoLoanModel.TotalToPay = autoLoanModel.Principal + autoLoanModel.TotalInterest;
            autoLoanModel.FinalCost = autoLoanModel.TotalToPay + autoLoanModel.DownPayment;

            return View(autoLoanModel);
        }

        public ActionResult BackAutoLoanCalculator(AutoLoanCalculatorModel model)
        {
            return View("AutoLoanCalculator",model);
        }

        public ActionResult VehicleLog(int inventoryId, int type)
        {
            var vehicleLogs = type == Constanst.CarInfoType.Sold ? _inventoryManagementForm.GetSoldVehicleLogs(inventoryId).ToList() : _inventoryManagementForm.GetVehicleLogs(inventoryId).ToList();
            ViewData["LISTINGID"] = inventoryId;
            ViewData["TYPE"] = type;
            return View("VehicleLogs", vehicleLogs);
        }

        #region Silent Salesman

        public ActionResult SilentSalesman(int listingId)
        {
            var model = _inventoryManagementForm.GetCarInfo(listingId);
            var silentSalesman = _inventoryManagementForm.GetSilentSalesman(listingId, SessionHandler.Dealer.DealershipId);
            model.Title = silentSalesman == null ? String.Format("{0} {1} {2} {3}", model.ModelYear, model.Make, model.Model, model.Trim) : silentSalesman.Title;
         
            model.SelectedOptions = silentSalesman == null ? string.Empty : (silentSalesman.AdditionalOptions ?? string.Empty);
            model.OtherOptions = silentSalesman == null ? string.Empty : (silentSalesman.OtherOptions ?? string.Empty);

            model.Engine = silentSalesman != null
                ? silentSalesman.Engine
                : model.Cylinder + " Cylinder(s); " + model.Litters + "L";

            return View(model);
        }

        [HttpPost]
        public string SilentSalesman(CarShortViewModel viewModel)
        {
            try
            {
                _inventoryManagementForm.NewSilentSalesman(viewModel.ListingId, viewModel.DealerId, viewModel.Title,
                    viewModel.Engine, viewModel.AdditonalOptions, viewModel.OtherOptions,
                    SessionHandler.CurrentUser.UserId);
                _vehicleLogForm.AddVehicleLog(viewModel.ListingId, SessionHandler.CurrentUser.UserId,
                    Constanst.VehicleLogSentence.SilentSalemanByUser.Replace("USER", SessionHandler.CurrentUser.FullName),
                    null);
            }
            catch (Exception)
            {
                return Constanst.AjaxMessage.Error;
            }

            return Constanst.AjaxMessage.Success;
        }

        #endregion
    }

    public class ProfileIndex
    {
        public int Previous { get; set; }
        public int Next { get; set; }
    }
}

