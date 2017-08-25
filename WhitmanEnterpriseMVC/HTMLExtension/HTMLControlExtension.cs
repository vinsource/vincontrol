using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.Xml;
using WhitmanEnterpriseMVC.Handlers;
using WhitmanEnterpriseMVC.HelperClass;
using WhitmanEnterpriseMVC.HTMLExtension;
using WhitmanEnterpriseMVC.Models;
using WhitmanEnterpriseMVC.com.chromedata.services.Description7a;
using System.Web.Script.Serialization;

namespace System.Web.Mvc.Html
{
    public static class HTMLControlExtension
    {
        public static string DatePicker(this HtmlHelper htmlHelper, string name)
        {
            return "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " runat=\"server\"" + " />";
        }

        public static string SubmitButton(this HtmlHelper helper, string text)
        {
            return "<input id=\"LoginButton\" type=\"submit\" class=\"menuButton\" value=\"" + text + "\" />";
        }

        public static string ImageButton(this HtmlHelper<CarInfoFormViewModel> helper, string text)
        {
            var model = helper.ViewData.Model;

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            if (model.InventoryStatus == 1)

                return "<a class=\"iframe\" href=\"" + urlHelper.Content("~/Inventory/OpenUploadWindow?ListingId=" + model.ListingId) + "\"><input class=\"pad\"  type=\"button\" name=\"manangePhotos\" value=\"" + text + "\" /></a>";
            else if (model.InventoryStatus == -1)
                return "<a class=\"iframe\" href=\"" + urlHelper.Content("~/Inventory/OpenUploadWindowForSold?ListingId=" + model.ListingId) + "\"><input class=\"pad\"  type=\"button\" name=\"manangePhotos\" value=\"" + text + "\" /></a>";
            else
            {
                return "<a class=\"iframe\" href=\"" + urlHelper.Content("~/Inventory/OpenUploadWindow?ListingId=" + model.ListingId) + "\"><input class=\"pad\"  type=\"button\" name=\"manangePhotos\" value=\"" + text + "\" /></a>";
            }
        }

        public static string ReportButtonEbayGroup(this HtmlHelper<EbayFormViewModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            builder.AppendLine("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Market/ViewEbay?ListingId=" + model.ListingId) + "\"><input class=\"btn\"  type=\"button\" name=\"EbayEdit\" value=\"Edit Ad\" /></a>" + Environment.NewLine);

            builder.AppendLine("<a  href=\"" + urlHelper.Content("~/Inventory/ViewIProfile?ListingId=" + model.ListingId) + "\"><input class=\"btn\"  type=\"button\" name=\"EbayCancel\" value=\"Cancel Ad\" /></a>" + Environment.NewLine);

            return builder.ToString();
        }

        public static string ReportButtonGroup(this HtmlHelper<CarInfoFormViewModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            
            if (model.InventoryStatus == 1)
            {
                if (CanSeeButton(model, Constanst.ProfileButton.EditProfile))
                builder.Append("<a class=\"pad_tab\" title=\"Edit\" href=\"" +
                          urlHelper.Content("~/Inventory/EditIProfile?ListingId=" + model.ListingId) +
                          "\">Edit Profile</a>" +
                          Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.Ebay))
                builder.Append("<a class=\"iframe\" title=\"Ebay\" href=\"" +
                                urlHelper.Content("~/Market/ViewEbay?ListingId=" + model.ListingId) +
                                "\"><input class=\"pad_tab\"  type=\"button\" name=\"ebayCraigslist\" value=\"Ebay\" /></a>" +
                                Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.WS))
                builder.Append("<input class=\"pad_tab\" type=\"button\" name=\"ws\" value=\"WS\" onclick=\"javascript:openWindowSticker(" +
                    model.ListingId + ")\">" + Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.BG))
                builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" +
                               urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + model.ListingId) +
                               "\"><input class=\"pad_tab\" type=\"button\" name=\"bg\" value=\"BG\" /></a>" +
                               Environment.NewLine);

                if (model.MultipleDealers)
                    if (CanSeeButton(model, Constanst.ProfileButton.Transfer))
                    builder.Append("<a title=\"Transfer\" class=\"iframe\" href=\"" +
                                   urlHelper.Content("~/Inventory/OpenVehicleTransferWindow?ListingId=" + model.ListingId) +
                                   "\"><input class=\"pad_tab\" type=\"button\" name=\"Transfer\" value=\"Transfer\" /></a>" +
                                   Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.MarkSold))
                builder.Append("<a title=\"Mark Sold\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewMarkSold?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"Marksold\" value=\"Mark Sold\"  /></a>" +
                              Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.Wholesale))
                builder.Append("<a title=\"Whole Sale\" id=\"btnHeadWholesale\" href=\"" + urlHelper.Content("~/Inventory/TransferToWholeSaleFromInventory?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\" type=\"button\" name=\"wholesale\" value=\"Wholesale\"  /></a>" + Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.PriceTracking))
                builder.Append("<a title=\"Price Changes Tracking\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewPriceTracking?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"PriceTracking\" value=\"Price Tracking\"  /></a>" +
                              Environment.NewLine);
                
                if (SessionHandler.CanViewBucketJumpReport != null && SessionHandler.CanViewBucketJumpReport.Value && CanSeeButton(model, Constanst.ProfileButton.BucketJumpTracking))
                    builder.Append("<a title=\"Bucket Jump Tracking\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewBucketJumpTracking?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"BucketJumpTracking\" value=\"Bucket Jump Tracking\"  /></a>" +
                              Environment.NewLine);
            }

            else if (model.InventoryStatus == -1)
            {
                if (CanSeeButton(model, Constanst.ProfileButton.MarkSold))
                builder.Append("<a title=\"Mark Unsold\" href=\"" +
                           urlHelper.Content("~/Inventory/MarkUnsold?ListingId=" + model.ListingId) +
                           "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"Marknsold\" value=\"Mark Unsold\"  /></a>" +
                           Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.Wholesale))
                builder.Append("<a title=\"Whole Sale\" href=\"" + urlHelper.Content("~/Inventory/TransferToWholeSaleFromSoldInventory?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\" type=\"button\" name=\"wholesale\" value=\"Wholesale\"  /></a>" + Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.PriceTracking))
                builder.Append("<a title=\"Price Changes Tracking\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewPriceTrackingForSold?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"PriceTracking\" value=\"Price Tracking\"  /></a>" +
                              Environment.NewLine);

                if (SessionHandler.CanViewBucketJumpReport != null && SessionHandler.CanViewBucketJumpReport.Value && CanSeeButton(model, Constanst.ProfileButton.BucketJumpTracking))
                    builder.Append("<a title=\"Bucket Jump Tracking\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewBucketJumpTrackingForSold?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"BucketJumpTracking\" value=\"Bucket Jump Tracking\"  /></a>" +
                              Environment.NewLine);
            }

            else if (model.InventoryStatus == 2)
            {
                builder.Append("<a title=\"Back To Inventory\" href=\"" + urlHelper.Content("~/Inventory/TransferToInventoryFromWholesale?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\" type=\"button\" name=\"Inventory\" value=\"Back To Inventory\"  /></a>" + Environment.NewLine);
            }

            builder.Append("<br/>" + Environment.NewLine);
            return builder.ToString();
        }

        public static string ReportButtonGroupForAppraisal(this HtmlHelper<AppraisalViewFormModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            builder.Append("<a class=\"pad_tab\" title=\"Edit\" href=\"" +
                             urlHelper.Content("~/Appraisal/EditAppraisal?AppraisalId=" + model.AppraisalGenerateId) +
                             "\">Edit Appraisal</a>" +
                             Environment.NewLine);


          
            return builder.ToString();
        }

        public static string ReportButtonGroupForAppraisalForTruck(this HtmlHelper<AppraisalViewFormModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            builder.Append("<a class=\"pad_tab\" title=\"Edit\" href=\"" +
                             urlHelper.Content("~/Appraisal/EditAppraisalForTruck?AppraisalId=" + model.AppraisalGenerateId) +
                             "\">Edit Appraisal</a>" +
                             Environment.NewLine);


            builder.Append("<br/>" + Environment.NewLine);
            return builder.ToString();
        }

        public static string ReportButtonGroupForTruck(this HtmlHelper<CarInfoFormViewModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);


            if (model.InventoryStatus == 1)
            {
                if (CanSeeButton(model, Constanst.ProfileButton.EditProfile))
                builder.Append("<a class=\"pad_tab\" title=\"Edit\" href=\"" +
                         urlHelper.Content("~/Inventory/EditIProfileForTruck?ListingId=" + model.ListingId) +
                         "\">Edit Profile</a>" +
                         Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.Ebay))
                builder.Append("<a class=\"iframe\" title=\"Ebay\" href=\"" +
                                urlHelper.Content("~/Market/ViewEbay?ListingId=" + model.ListingId) +
                                "\"><input class=\"pad_tab\"  type=\"button\" name=\"ebayCraigslist\" value=\"Ebay\" /></a>" +
                                Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.WS))
                builder.Append("<input class=\"pad_tab\" type=\"button\" name=\"ws\" value=\"WS\" onclick=\"javascript:openWindowSticker(" +
                    model.ListingId + ")\">" + Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.BG))
                builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" +
                               urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + model.ListingId) +
                               "\"><input class=\"pad_tab\" type=\"button\" name=\"bg\" value=\"BG\" /></a>" +
                               Environment.NewLine);

                if (model.MultipleDealers)
                    if (CanSeeButton(model, Constanst.ProfileButton.Transfer))
                    builder.Append("<a title=\"Transfer\" class=\"iframe\" href=\"" +
                                   urlHelper.Content("~/Inventory/OpenVehicleTransferWindow?ListingId=" + model.ListingId) +
                                   "\"><input class=\"pad_tab\" type=\"button\" name=\"Transfer\" value=\"Transfer\" /></a>" +
                                   Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.MarkSold))
                builder.Append("<a title=\"Mark Sold\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewMarkSold?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"Marksold\" value=\"Mark Sold\"  /></a>" +
                              Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.Wholesale))
                builder.Append("<a title=\"Whole Sale\" href=\"" + urlHelper.Content("~/Inventory/TransferToWholeSaleFromInventory?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\" type=\"button\" name=\"wholesale\" value=\"Wholesale\"  /></a>" + Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.PriceTracking))
                builder.Append("<a title=\"Price Changes Tracking\" class=\"iframe\" href=\"" +
                     urlHelper.Content("~/Inventory/ViewPriceTracking?ListingId=" + model.ListingId) +
                     "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"PriceTracking\" value=\"Price Tracking\"  /></a>" +
                     Environment.NewLine);

                if (SessionHandler.CanViewBucketJumpReport != null && SessionHandler.CanViewBucketJumpReport.Value && CanSeeButton(model, Constanst.ProfileButton.BucketJumpTracking))
                    builder.Append("<a title=\"Bucket Jump Tracking\" class=\"iframe\" href=\"" +
                              urlHelper.Content("~/Inventory/ViewBucketJumpTracking?ListingId=" + model.ListingId) +
                              "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"BucketJumpTracking\" value=\"Bucket Jump Tracking\"  /></a>" +
                              Environment.NewLine);
            }

            else if (model.InventoryStatus == -1)
            {
                if (CanSeeButton(model, Constanst.ProfileButton.MarkSold))
                builder.Append("<a title=\"Mark Unsold\" href=\"" +
                           urlHelper.Content("~/Inventory/MarkUnsold?ListingId=" + model.ListingId) +
                           "\"><input class=\"pad_tab\" type=\"button\" name=\"sold\" id=\"Marknsold\" value=\"Mark Unsold\"  /></a>" +
                           Environment.NewLine);

                if (CanSeeButton(model, Constanst.ProfileButton.Wholesale))
                builder.Append("<a title=\"Whole Sale\" href=\"" + urlHelper.Content("~/Inventory/TransferToWholeSaleFromSoldInventory?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\" type=\"button\" name=\"wholesale\" value=\"Wholesale\"  /></a>" + Environment.NewLine);
            }

            else if (model.InventoryStatus == 2)
            {
                builder.Append("<a title=\"Back To Inventory\" href=\"" + urlHelper.Content("~/Inventory/TransferToInventoryFromWholesale?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\" type=\"button\" name=\"Inventory\" value=\"Back To Inventory\"  /></a>" + Environment.NewLine);
            }

            builder.Append("<br/>" + Environment.NewLine);

            return builder.ToString();
        }

        public static bool CanSeeButton(CarInfoFormViewModel model, string buttonName)
        {
            var group = model.ButtonPermissions.FirstOrDefault(i => i.GroupName.ToLower().Equals(SessionHandler.CurrentUser.Role.ToLower()));
            if (SessionHandler.CurrentUser.Role.ToLower().Equals("admin")
                || SessionHandler.CurrentUser.Role.ToLower().Equals("king")
                || !SQLHelper.CheckDealershipButtonGroupExist(SessionHandler.Dealership.DealershipId, SessionHandler.CurrentUser.Role.ToLower())
                || (group != null && group.Buttons.First(i => i.ButtonName.ToLower().Equals(buttonName.ToLower())).CanSee))
                return true;
            return false;
        }

        private static bool CanSeeButton(string buttonName)
        {
            if (SessionHandler.CurrentUser.Role.ToLower().Equals("admin")
                || SessionHandler.CurrentUser.Role.ToLower().Equals("king")
                || !SQLHelper.CheckDealershipButtonGroupExist(SessionHandler.Dealership.DealershipId, SessionHandler.CurrentUser.Role.ToLower())
                || (SessionHandler.CurrentUser.ProfileButtonPermissions != null && SessionHandler.CurrentUser.ProfileButtonPermissions.Buttons.First(i => i.ButtonName.ToLower().Equals(buttonName.ToLower())).CanSee))
                return true;
            return false;
        }

        public static string ExpandChartButton(this HtmlHelper<CarInfoFormViewModel> htmlHelper, int ListingId)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);


            builder.Append("<a class=\"iframe\" id=\"graph-fancybox\" href=\"" + urlHelper.Content("~/Chart/ViewFullChart?ListingId=" + ListingId) + "\"></a>" + Environment.NewLine);


            return builder.ToString();
        }

        public static string ExpandChartButton(this HtmlHelper<AppraisalViewFormModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);


            builder.Append("<a class=\"iframe\" id=\"graph-fancybox\" href=\"" + urlHelper.Content("~/Chart/ViewFullChartInAppraisal?AppraisalId=" + model.AppraisalGenerateId) + "\"></a>" + Environment.NewLine);


            return builder.ToString();
        }

        public static string ReportKellyBlueBookButton(this HtmlHelper<CarInfoFormViewModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            builder.AppendLine("<a style=\"float:left; font-weight:normal; font-size: .9em;\" href=\"" + urlHelper.Content("~/Market/ResetKbbTrim?ListingId=" + model.ListingId) + "\">Not a correct trim? Click here</a><br/>");

            builder.AppendLine("<a class=\"iframe\" target=\"_blank\" style=\"float:right; font-weight:normal; font-size: .9em;\" href=\"" + urlHelper.Content("~/Market/GetKellyBlueBookSummary?ListingId=" + model.ListingId) + "\">View KBB Summary Report</a><br/>");

            builder.AppendLine("<font size='1' >* Total Value = Base WholeSale +/- Mileage Adjustment</font>");

            return builder.ToString();
        }

        public static string ReportBlackBookButton(this HtmlHelper<CarInfoFormViewModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            builder.AppendLine("<a class=\"iframe\" target=\"_blank\" style=\"float:right; font-weight:normal; font-size: .9em;\" href=\"" + urlHelper.Content("~/Market/GetBlackBookSummary?ListingId=" + model.ListingId) + "\">View Black Book Summary Report</a>");

            return builder.ToString();
        }

        public static string ReportKellyBlueBookButton(this HtmlHelper<AppraisalViewFormModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            builder.AppendLine("<a class=\"iframe\" target=\"_blank\" style=\"float:right; font-weight:normal; font-size: .9em;\" href=\"" + urlHelper.Content("~/Market/GetKellyBlueBookSummaryByVin?Vin=" + model.VinNumber + "&Mileage=" + model.Mileage) + "\">View KBB Summary Report</a>");

            return builder.ToString();
        }

        public static string ReportKellyBlueBookButtonInAppraisal(this HtmlHelper<AppraisalViewFormModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            builder.AppendLine("<a class=\"iframe\" target=\"_blank\" style=\"float:right; font-weight:normal; font-size: .9em;\" href=\"" + urlHelper.Content("~/Market/GetKellyBlueBookSummaryAppraisal?AppraisalId=" + model.AppraisalGenerateId) + "\">View KBB Summary Report</a>");

            return builder.ToString();
        }

        public static string ReportBlackBookButton(this HtmlHelper<AppraisalViewFormModel> htmlHelper)
        {
            var builder = new StringBuilder();

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            builder.AppendLine("<a class=\"iframe\" target=\"_blank\" style=\"float:right; font-weight:normal; font-size: .9em;\" href=\"" + urlHelper.Content("~/Market/GetBlackBookSummaryByVin?Vin=" + model.VinNumber + "&Mileage=" + model.Mileage) + "\">View Black Book Summary Report</a>");

            return builder.ToString();
        }

        private static string generateGraphJavaScript(CarInfoFormViewModel car)
        {
            var builder = new StringBuilder();

            var hash = car.CarsOnMarket;

            var dtDefault = new Hashtable();

            dtDefault = (Hashtable)hash["Nation"];


            //switch (car.CurrentDistance)
            //{
            //    case "100":
            //        dtDefault = (Hashtable)hash["100"];
            //        break;
            //    case "250":
            //        dtDefault = (Hashtable)hash["250"];

            //        break;
            //    case "500":
            //        dtDefault = (Hashtable)hash["500"];

            //        break;
            //    case "Nation":
            //        dtDefault = (Hashtable)hash["Nation"];

            //        break;
            //    default:
            //        dtDefault = (Hashtable)hash["100"];

            //        break;
            //}


            string length = dtDefault.Count.ToString();

            int index = 0;

            builder.Append("<script type=\"text/javascript\" language=\"javascript\">" + Environment.NewLine);

            builder.Append("var fRange = 100;" + Environment.NewLine);

            builder.Append("var us = 3500;" + Environment.NewLine);

            builder.Append("var sortedCars;" + Environment.NewLine);

            builder.Append("$('#rangeNav span').each(function() {" + Environment.NewLine);

            builder.Append("	var id = this.id;	" + Environment.NewLine);

            builder.Append("$(this).mouseover(function() {" + Environment.NewLine);

            builder.Append("if (!$(this).hasClass('rSelected')) {" + Environment.NewLine);

            builder.Append("	$(this).click(function(){" + Environment.NewLine);

            builder.Append("fRange = this.id;" + Environment.NewLine);

            builder.Append("$('#rangeNav span').each(function(){" + Environment.NewLine);

            builder.Append("if ($(this).id != id) {" + Environment.NewLine);

            builder.Append("$(this).removeClass('rSelected');" + Environment.NewLine);

            builder.Append("	}" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);


            builder.Append("$(this).addClass('rSelected');" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);

            builder.Append("	}" + Environment.NewLine);

            builder.Append("	});" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);

            builder.Append("function filterSet(array) {" + Environment.NewLine);

            builder.Append("return function (element) {" + Environment.NewLine);

            builder.Append("	return array.indexOf(element) !=-1" + Environment.NewLine);

            builder.Append("}" + Environment.NewLine);

            builder.Append("};" + Environment.NewLine);

            builder.Append("function sortCars(array) " + Environment.NewLine);

            builder.Append("{" + Environment.NewLine);

            builder.Append("var trimFilter = [];" + Environment.NewLine);

            builder.Append("var optionFilter = [];" + Environment.NewLine);

            //builder.Append("var trimList = document.getElementById('trim');" + Environment.NewLine);

            //builder.Append("var optionList = document.getElementById('option');" + Environment.NewLine);

            //builder.Append("for (i=0;i<trimList.children.length;i++) {" + Environment.NewLine);

            //builder.Append("if (trimList.children[i].selected == true) {trimFilter.push(trimList.children[i].text);}	" + Environment.NewLine);
            //builder.Append("}" + Environment.NewLine);
            //builder.Append("for (i=0;i<optionList.children.length;i++) {" + Environment.NewLine);
            //builder.Append("	if (optionList.children[i].selected == true) {optionFilter.push(optionList.children[i].text);}	" + Environment.NewLine);
            //builder.Append("}" + Environment.NewLine);
            builder.Append("array = array.slice(0);" + Environment.NewLine);
            builder.Append("var certify = document.getElementById('certifiedChk').checked;" + Environment.NewLine);
            //builder.Append("var trim = document.getElementById('trim').selectedIndex;" + Environment.NewLine);
            //builder.Append("var option = document.getElementById('option').selectedIndex;" + Environment.NewLine);
            builder.Append("if (certify == false /*&& trim == 0 && option == 0*/) {	return array;" + Environment.NewLine);
            builder.Append("} else if (certify == true /*|| trim != 0 || option != 0*/) {" + Environment.NewLine);
            builder.Append("var newArray" + Environment.NewLine);
            builder.Append("	if (certify === true) {" + Environment.NewLine);
            builder.Append("for (a=0;a<=array.length-1;a++) {" + Environment.NewLine);
            builder.Append("if (array[a][4] === 0) {" + Environment.NewLine);
            builder.Append("array.splice(a, 1);" + Environment.NewLine);
            builder.Append("a--;" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            //builder.Append("if (trim != 0 && trimFilter.length != 0) {" + Environment.NewLine);
            //builder.Append("	for (a=0;a<=array.length-1;a++) {" + Environment.NewLine);
            //builder.Append("	var res = array[a][6].filter(filterSet(trimFilter));" + Environment.NewLine);
            //builder.Append("if (res.length != 0) {} else {" + Environment.NewLine);
            //builder.Append("array.splice(a,1); a--;}" + Environment.NewLine);
            //builder.Append("}" + Environment.NewLine);
            //builder.Append("}" + Environment.NewLine);
            //builder.Append("if (option != 0 && optionFilter.length != 0) {" + Environment.NewLine);
            //builder.Append("for (a=0;a<=array.length-1;a++) {" + Environment.NewLine);
            //builder.Append("var res = array[a][5].filter(filterSet(optionFilter));" + Environment.NewLine);
            //builder.Append("if (res.length != 0) {} else {" + Environment.NewLine);
            //builder.Append("array.splice(a,1); a--;}" + Environment.NewLine);
            //builder.Append("}" + Environment.NewLine);
            //builder.Append("}" + Environment.NewLine);
            builder.Append("return array;" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("var d11;" + Environment.NewLine);
            builder.Append("var d12;" + Environment.NewLine);
            builder.Append("var d13;" + Environment.NewLine);
            builder.Append("var d14;" + Environment.NewLine);
            builder.Append("var d15;" + Environment.NewLine);
            builder.Append("var d16;" + Environment.NewLine);
            builder.Append("var d17;" + Environment.NewLine);
            builder.Append("var d18;" + Environment.NewLine);
            builder.Append("var d19;" + Environment.NewLine);
            builder.Append("var d20;" + Environment.NewLine);
            builder.Append("var ranges = new Array();" + Environment.NewLine);
            builder.Append("var options;" + Environment.NewLine);
            builder.Append("var certified;" + Environment.NewLine);
            builder.Append("function randomFromTo(from, to){ return Math.round(Math.random() * (to - from + 1) + from);}" + Environment.NewLine);

            builder.Append("    function between(x, min, max) { return x >= min && x < max;}" + Environment.NewLine);
            builder.Append("  function posNeg(x) {	if (x > 0) {		return '+'+x;		} else {		return x;	}};" + Environment.NewLine);
            builder.Append(" function addCommas(nStr) {	nStr+='';	x=nStr.split('.');	x1=x[0];	x2=x.length>1?'.'+x[1]:'';	var rgx=/(\\d+)(\\d{3})/;	while(rgx.test(x1)){		x1=x1.replace(rgx,'$1'+','+'$2');	}  return x1+x2;}" + Environment.NewLine);


            builder.Append("var carPrices = new Array();" + Environment.NewLine);

            builder.Append("var carMiles = new Array();" + Environment.NewLine);

            builder.Append("var allCars = new Array();" + Environment.NewLine);

            builder.Append(" var carDistances = new Array();" + Environment.NewLine);

            builder.Append(" var percent = new Array();" + Environment.NewLine);

            builder.Append("var redraw = false;" + Environment.NewLine);
            builder.Append("var carRange = [];" + Environment.NewLine);



            builder.Append("var i=0;" + Environment.NewLine);
            builder.Append("while (i<=1) {" + Environment.NewLine);
            builder.Append("	i = Math.round(i*100)/100;" + Environment.NewLine);
            builder.Append("	percent.push(i);" + Environment.NewLine);
            builder.Append("	i = i+.1;" + Environment.NewLine);
            builder.Append("	i = Math.round(i*100)/100;" + Environment.NewLine);
            builder.Append("  }" + Environment.NewLine);

            builder.Append("if (allCars.length == 0) {" + Environment.NewLine);




            foreach (int ListingId in dtDefault.Keys)
            {
                CarInfoFormViewModel similarCar = (CarInfoFormViewModel)dtDefault[ListingId];

                //int carNumber = index + 1;

                int milage = 0; decimal price = 0;

                string PriceInfo = similarCar.SalePrice;

                PriceInfo = PriceInfo.Replace("$", "");

                PriceInfo = PriceInfo.Replace(",", "");

                bool flag = Int32.TryParse(similarCar.Mileage, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out milage);

                if (!String.IsNullOrEmpty(PriceInfo))

                    flag = Decimal.TryParse(PriceInfo, out price);

                if (flag)
                {
                    builder.Append("allCars[" + index + "] = new Array();" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0] =[] " + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][1] = " + index + ";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][1] = \"" + similarCar.ModelYear + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][2] = \"" + similarCar.Make + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][3] = \"" + similarCar.Model + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][4] = \"" + similarCar.Vin + "\";" + Environment.NewLine);



                    builder.Append("allCars[" + index + "][1] =" + milage + " ;" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][2]= " + price + ";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][3]= " + similarCar.DistanceFromDealerShip + ";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][4]= 1;" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][5]= [];" + Environment.NewLine);


                    //builder.Append("allCars[" + index + "][4]= 1;" + Environment.NewLine);


                    //for (v=0;v<randomFromTo(0,2);v++) {
                    //  allCars[i][5].push('Option '+(v+1)); // options
                    //}


                    builder.Append("allCars[" + index + "][6] = [];" + Environment.NewLine);

                    string potientialTrim = similarCar.CarName.Substring(similarCar.CarName.LastIndexOf(" ") + 1);

                    if (car.TrimList != null)
                    {
                        if (car.TrimList.Count > 0)
                        {
                            bool tmp = car.TrimList.Any(p => p.ToLowerInvariant().Equals(potientialTrim.ToLowerInvariant()));

                            if (tmp)
                                builder.Append("allCars[" + index + "][6][0] = \"" + potientialTrim + "\";" + Environment.NewLine);
                            else
                                builder.Append("allCars[" + index + "][6][0] = \"Not specified\";" + Environment.NewLine);
                        }
                        else
                        {
                            builder.Append("allCars[" + index + "][6][0] = \"Not specified\";" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        builder.Append("allCars[" + index + "][6][0] = \"Not specified\";" + Environment.NewLine);
                    }



                    builder.Append("allCars[" + index + "][7] = randomFromTo(0,150);" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][8] = \"" + similarCar.CarImageUrl + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][9] = [];" + Environment.NewLine);

                    //           for (v=0;v<randomFromTo(0,10);v++) {
                    //  allCars[i][9][v] = [];
                    //  allCars[i][9][v][0] = '10/10/10';
                    //  allCars[i][9][v][1] = randomFromTo(3000,45000) // options
                    //}// price changes


                    builder.Append("allCars[" + index + "][10] = \"" + similarCar.ExteriorColor + "\";" + Environment.NewLine);


                    similarCar.Mileage = milage.ToString();

                    similarCar.SalePrice = price.ToString();

                    index++;
                }


            }

            builder.Append(" } else {" + Environment.NewLine);

            builder.Append(" 	allCars = newArray();" + Environment.NewLine);


            index = 0;

            foreach (int ListingId in dtDefault.Keys)
            {
                CarInfoFormViewModel similarCar = (CarInfoFormViewModel)dtDefault[ListingId];

                //int carNumber = index + 1;

                int milage = 0; decimal price = 0;

                string PriceInfo = similarCar.SalePrice;

                PriceInfo = PriceInfo.Replace("$", "");

                PriceInfo = PriceInfo.Replace(",", "");

                bool flag = Int32.TryParse(similarCar.Mileage, System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out milage);

                if (!String.IsNullOrEmpty(PriceInfo))

                    flag = Decimal.TryParse(PriceInfo, out price);


                if (flag)
                {
                    builder.Append("allCars[" + index + "] = new Array();" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0] =[] " + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][1] =" + index + ";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][1] = \"" + similarCar.ModelYear + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][2] = \"" + similarCar.Make + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][3] = \"" + similarCar.Model + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][0][4] = \"" + similarCar.Vin + "\";" + Environment.NewLine);


                    builder.Append("allCars[" + index + "][1] =" + milage + " ;" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][2]= " + price + ";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][3]= " + similarCar.DistanceFromDealerShip + ";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][4]= 1;" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][5]= [];" + Environment.NewLine);


                    //builder.Append("allCars[" + index + "][4]= 1;" + Environment.NewLine);


                    //for (v=0;v<randomFromTo(0,2);v++) {
                    //  allCars[i][5].push('Option '+(v+1)); // options
                    //}


                    builder.Append("allCars[" + index + "][6] = [];" + Environment.NewLine);

                    string potientialTrim = similarCar.CarName.Substring(similarCar.CarName.LastIndexOf(" ") + 1);

                    if (car.TrimList != null)
                    {
                        if (car.TrimList.Count > 0)
                        {
                            bool tmp = car.TrimList.Any(p => p.ToLowerInvariant().Equals(potientialTrim.ToLowerInvariant()));

                            if (tmp)
                                builder.Append("allCars[" + index + "][6][0] = \"" + potientialTrim + "\";" + Environment.NewLine);
                            else
                                builder.Append("allCars[" + index + "][6][0] = \"Not specified\";" + Environment.NewLine);
                        }
                        else
                        {
                            builder.Append("allCars[" + index + "][6][0] = \"Not specified\";" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        builder.Append("allCars[" + index + "][6][0] = \"Not specified\";" + Environment.NewLine);
                    }


                    builder.Append("allCars[" + index + "][7] = randomFromTo(0,150);" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][8] = \"" + similarCar.CarImageUrl + "\";" + Environment.NewLine);

                    builder.Append("allCars[" + index + "][9] = [];" + Environment.NewLine);




                    builder.Append("allCars[" + index + "][10] = \"" + similarCar.ExteriorColor + "\";" + Environment.NewLine);



                    similarCar.Mileage = milage.ToString();

                    similarCar.SalePrice = price.ToString();

                    index++;
                }


            }

            builder.Append("}" + Environment.NewLine);

            builder.Append(" function abbrNum(number, decPlaces) {" + Environment.NewLine);

            builder.Append("  decPlaces = Math.pow(10,decPlaces);" + Environment.NewLine);

            builder.Append("   var abbrev = [ \"k\", \"m\", \"b\", \"t\" ];" + Environment.NewLine);

            builder.Append(" for (var i=abbrev.length-1; i>=0; i--) {" + Environment.NewLine);


            builder.Append("   var size = Math.pow(10,(i+1)*3);" + Environment.NewLine);

            builder.Append(" if(size <= number) {" + Environment.NewLine);

            builder.Append(" number = Math.round(number*decPlaces/size)/decPlaces;" + Environment.NewLine);

            builder.Append("  number += abbrev[i];" + Environment.NewLine);

            builder.Append("    break; " + Environment.NewLine);

            builder.Append("}" + Environment.NewLine);

            builder.Append("}" + Environment.NewLine);

            builder.Append("return number;" + Environment.NewLine);

            builder.Append("}" + Environment.NewLine);

            builder.Append("var totalMile = 0;" + Environment.NewLine);

            builder.Append("var totalPrice = 0;" + Environment.NewLine);

            builder.Append(" if (totalMile == 0 || totalPrice == 0){" + Environment.NewLine);

            builder.Append("for(var i = 0; i <= allCars.length - 1; ++i){" + Environment.NewLine);

            builder.Append(" totalMile = totalMile + allCars[i][1];" + Environment.NewLine);

            builder.Append("totalPrice = totalPrice + allCars[i][2];" + Environment.NewLine);

            builder.Append("carMiles [i] = allCars[i][1];" + Environment.NewLine);

            builder.Append("carPrices [i] = allCars[i][2];" + Environment.NewLine);

            builder.Append(" carDistances[i] = allCars[i][3];" + Environment.NewLine);


            builder.Append("}" + Environment.NewLine);

            builder.Append("}else{" + Environment.NewLine);


            builder.Append(" totalMile = 0;" + Environment.NewLine);

            builder.Append(" totalPrice = 0;" + Environment.NewLine);

            builder.Append("for(var i = 0; i <= allCars.length - 1; ++i){" + Environment.NewLine);

            builder.Append(" totalMile = totalMile + allCars[i][1];" + Environment.NewLine);

            builder.Append("totalPrice = totalPrice + allCars[i][2];" + Environment.NewLine);

            builder.Append("carMiles [i] = allCars[i][1];" + Environment.NewLine);

            builder.Append("carPrices [i] = allCars[i][2];" + Environment.NewLine);

            //builder.Append(" carDistances[i] = allCars[i][3];" + Environment.NewLine);


            builder.Append("}" + Environment.NewLine);

            builder.Append("}" + Environment.NewLine);


            builder.Append("var totalCars = allCars.length;" + Environment.NewLine);

            builder.Append("var largestPrice = Math.max.apply(Math, carPrices);" + Environment.NewLine);


            builder.Append("var largestMiles = Math.max.apply(Math, carMiles);" + Environment.NewLine);

            builder.Append("  var farthestCar = Math.max.apply(Math, carDistances); " + Environment.NewLine);

            builder.Append(" var nearestCar = Math.min.apply(Math, carDistances); " + Environment.NewLine);

            builder.Append(" var smallestPrice = Math.min.apply(Math, carPrices);" + Environment.NewLine);

            builder.Append(" var smallestMiles = Math.min.apply(Math, carMiles);" + Environment.NewLine);


            builder.Append("var yourCar = new Array();" + Environment.NewLine);

            builder.Append("yourCar[0] = new Array();" + Environment.NewLine);

            ///////////////////////////////////////////////////////////////
            builder.Append("yourCar[0][0] = []" + Environment.NewLine);

            builder.Append("yourCar[0][0][0] = " + index + Environment.NewLine);

            builder.Append("yourCar[0][0][1] = \"" + car.ModelYear + "\";" + Environment.NewLine);

            builder.Append("yourCar[0][0][2] = \"" + car.Make + "\";" + Environment.NewLine);


            builder.Append("yourCar[0][0][3] = \"" + car.Model + "\";" + Environment.NewLine);

            builder.Append("yourCar[0][0][4] = \"" + car.Vin + "\";" + Environment.NewLine);

            car.Mileage = "100000";

            car.Mileage = car.Mileage.Replace(",", "");

            builder.Append(" yourCar[0][1] = " + car.Mileage + ";" + Environment.NewLine);

            string yourCarPrice = "0";

            if (String.IsNullOrEmpty(car.SalePrice))
                yourCarPrice = car.SalePrice;

            builder.Append("yourCar[0][2] =  " + yourCarPrice + ";" + Environment.NewLine);

            builder.Append("yourCar[0][3] =  0 ;" + Environment.NewLine);

            builder.Append("yourCar[0][4] =  1 ;" + Environment.NewLine);

            builder.Append("yourCar[0][5] =  [] ;" + Environment.NewLine);

            builder.Append("yourCar[0][6] = \"" + car.Trim + "\";" + Environment.NewLine);

            builder.Append(" yourCar[0][7] = " + car.DaysInInvenotry + Environment.NewLine);


            builder.Append("yourCar[0][8] = \"" + car.DefaultImageUrl + "\";" + Environment.NewLine);



            builder.Append("  yourCar[0][9] = [];" + Environment.NewLine);
            //    for (v=0;v<randomFromTo(0,10);v++) {
            //  yourCar[0][9][v] = [];
            //  yourCar[0][9][v][0] = '10/10/10';
            //  yourCar[0][9][v][1] = randomFromTo(3000,45000) // options
            //}// price changes


            builder.Append(" yourCar[0][10] = \"" + car.ExteriorColor + "\";" + Environment.NewLine);

            builder.Append("var yourCarMiles = yourCar[0][1];" + Environment.NewLine);

            builder.Append("var yourCarPrice = yourCar[0][2];" + Environment.NewLine);
            ////////////////////////////////////////////////////////////////////////////////////////////////


            builder.Append("var startPriceRange = smallestPrice-(largestPrice*.25);" + Environment.NewLine);

            builder.Append("var startMileRange = smallestMiles;" + Environment.NewLine);


            builder.Append("var mileRangeEnd = largestMiles;" + Environment.NewLine);

            builder.Append("  var priceRangeEnd = largestPrice+(largestPrice*.25);" + Environment.NewLine);


            builder.Append("var mileRange = mileRangeEnd;" + Environment.NewLine);

            builder.Append(" var priceRange = priceRangeEnd;" + Environment.NewLine);


            builder.Append("var averageMile = totalMile/totalCars;" + Environment.NewLine);

            builder.Append("var averagePrice = totalPrice/totalCars;" + Environment.NewLine);

            builder.Append(" for (i = 0; i <= 9; i++)" + Environment.NewLine);


            builder.Append(" {" + Environment.NewLine);

            builder.Append("ranges[i] = new Array();" + Environment.NewLine);


            builder.Append("    ranges[i][0] = Math.round((percent[i]*100)*100)/100;" + Environment.NewLine);

            builder.Append(" ranges[i][1] = Math.round((percent[i+1]*100)*100)/100;" + Environment.NewLine);
            builder.Append("  }" + Environment.NewLine);


            builder.Append("   var addEnd = ranges.length;" + Environment.NewLine);

            builder.Append("  ranges[addEnd] = [];" + Environment.NewLine);

            builder.Append("  ranges[addEnd][0] = 100;" + Environment.NewLine);

            builder.Append("  ranges[addEnd][1] = 3500;" + Environment.NewLine);

            builder.Append("  $('#any').attr('id', us);" + Environment.NewLine);
            builder.Append("   var rDistances = [];" + Environment.NewLine);
            builder.Append(" for (i=0;i<allCars.length;i++) {" + Environment.NewLine);
            builder.Append("if (allCars[i][3] <= fRange) {" + Environment.NewLine);
            builder.Append("carRange[i] = [];" + Environment.NewLine);
            builder.Append("carRange[i][0] = allCars[i][0];" + Environment.NewLine);
            builder.Append("	carRange[i][1] = allCars[i][1];" + Environment.NewLine);
            builder.Append("	carRange[i][2] = allCars[i][2];" + Environment.NewLine);

            builder.Append("carRange[i][3] = allCars[i][3];" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("  }" + Environment.NewLine);
            builder.Append("  carRange = $.grep(carRange,function(n){return(n);});" + Environment.NewLine);


            builder.Append("  for (v=0;v<=ranges.length-1;v++) {" + Environment.NewLine);
            builder.Append("ranges[v][2] = new Array();" + Environment.NewLine);
            builder.Append("var b = 0;" + Environment.NewLine);
            builder.Append("for (i=0; i<carRange.length;i++) {" + Environment.NewLine);
            builder.Append("if (between(carRange[i][3],ranges[v][0],ranges[v][1])) {" + Environment.NewLine);
            builder.Append("ranges[v][2][b] = [carRange[i][1], carRange[i][2]];" + Environment.NewLine);
            builder.Append("	b++;" + Environment.NewLine);
            builder.Append("	}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append(" }" + Environment.NewLine);


            builder.Append("$(function () {" + Environment.NewLine);

            builder.Append("var d11 = [];" + Environment.NewLine);
            builder.Append("for (i=0;i<ranges[0][2].length;i++) {		d11.push(ranges[0][2][i])	}" + Environment.NewLine);


            builder.Append("var d12 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[1][2].length;i++) {		d12.push(ranges[1][2][i])	}" + Environment.NewLine);

            builder.Append("var d13 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[2][2].length;i++) {		d13.push(ranges[2][2][i])  }" + Environment.NewLine);


            builder.Append("var d14 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[3][2].length;i++) {		d14.push(ranges[3][2][i])	}" + Environment.NewLine);

            builder.Append("var d15 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[4][2].length;i++) {		d15.push(ranges[4][2][i])	}" + Environment.NewLine);

            builder.Append("var d16 = [];" + Environment.NewLine);

            builder.Append("	for (i=0;i<ranges[5][2].length;i++) {		d16.push(ranges[5][2][i])	}" + Environment.NewLine);


            builder.Append("var d17 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[6][2].length;i++) {		d17.push(ranges[6][2][i])	}" + Environment.NewLine);


            builder.Append("var d18 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[7][2].length;i++) {		d18.push(ranges[7][2][i])	}" + Environment.NewLine);

            builder.Append("var d19 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[8][2].length;i++) {		d19.push(ranges[8][2][i])	}" + Environment.NewLine);

            builder.Append("var d20 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[9][2].length;i++) {		d20.push(ranges[9][2][i])	}" + Environment.NewLine);

            builder.Append("d21 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[10][2].length;i++) {		d11.push(ranges[10][2][i])	}" + Environment.NewLine);

            builder.Append("	$('#rangeNav span').click(function(){" + Environment.NewLine);
            builder.Append("fRange = this.id;" + Environment.NewLine);
            builder.Append("});" + Environment.NewLine);
            builder.Append("	sortedCars = allCars.slice();" + Environment.NewLine);
            builder.Append("$('#sorting input, #trim, #option, .ui-dropdownchecklist ').focus(function(){" + Environment.NewLine);
            builder.Append("sortedCars = 0;" + Environment.NewLine);
            builder.Append(" sortedCars = sortCars(allCars);" + Environment.NewLine);
            builder.Append("  ranges = new Array();" + Environment.NewLine);
            builder.Append(" for (i = 0; i <= 9; i++)" + Environment.NewLine);


            builder.Append(" {" + Environment.NewLine);

            builder.Append("ranges[i] = new Array();" + Environment.NewLine);


            builder.Append("    ranges[i][0] = Math.round((percent[i]*100)*100)/100;" + Environment.NewLine);

            builder.Append(" ranges[i][1] = Math.round((percent[i+1]*100)*100)/100;" + Environment.NewLine);
            builder.Append("  }" + Environment.NewLine);


            builder.Append("   var addEnd = ranges.length;" + Environment.NewLine);

            builder.Append("  ranges[addEnd] = [];" + Environment.NewLine);

            builder.Append("  ranges[addEnd][0] = 100;" + Environment.NewLine);

            builder.Append("  ranges[addEnd][1] = 3500;" + Environment.NewLine);



            builder.Append("  carRange = [];" + Environment.NewLine);
            builder.Append("if (fRange == us) {" + Environment.NewLine);
            builder.Append("  for (i=0;i<=sortedCars.length-1;i++) {" + Environment.NewLine);
            builder.Append("	carRange[i] = [];" + Environment.NewLine);

            builder.Append("	carRange[i][0] = sortedCars[i][0];" + Environment.NewLine);
            builder.Append("	carRange[i][1] = sortedCars[i][1];" + Environment.NewLine);
            builder.Append("carRange[i][2] = sortedCars[i][2];" + Environment.NewLine);
            builder.Append("carRange[i][3] = sortedCars[i][3];" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("  } else {" + Environment.NewLine);
            builder.Append(" for (i=0;i<sortedCars.length;i++) {" + Environment.NewLine);
            builder.Append("if (sortedCars[i][3] <= fRange) {" + Environment.NewLine);
            builder.Append("carRange[i] = [];" + Environment.NewLine);
            builder.Append("	carRange[i][0] = sortedCars[i][0];" + Environment.NewLine);
            builder.Append("	carRange[i][1] = sortedCars[i][1];" + Environment.NewLine);
            builder.Append("carRange[i][2] = sortedCars[i][2];" + Environment.NewLine);
            builder.Append("carRange[i][3] = sortedCars[i][3];" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("  carRange = $.grep(carRange,function(n){return(n);});" + Environment.NewLine);
            builder.Append("  for (v=0;v<=ranges.length-1;v++) {" + Environment.NewLine);
            builder.Append("	ranges[v][2] = new Array();" + Environment.NewLine);
            builder.Append("var b = 0;" + Environment.NewLine);

            builder.Append("for (i=0; i<=carRange.length-1;i++) {" + Environment.NewLine);

            builder.Append("	if (between(carRange[i][3],ranges[v][0],ranges[v][1])) {" + Environment.NewLine);

            builder.Append("	ranges[v][2][b] = [carRange[i][1], carRange[i][2]];" + Environment.NewLine);

            builder.Append("b++;" + Environment.NewLine);

            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);

            builder.Append("d11 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[0][2].length;i++) {		d11.push(ranges[0][2][i])	}" + Environment.NewLine);

            builder.Append("d12 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[1][2].length;i++) {		d11.push(ranges[1][2][i])	}" + Environment.NewLine);



            builder.Append("d13 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[2][2].length;i++) {		d11.push(ranges[2][2][i])	}" + Environment.NewLine);



            builder.Append("d14 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[3][2].length;i++) {		d11.push(ranges[3][2][i])	}" + Environment.NewLine);



            builder.Append("d15 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[4][2].length;i++) {		d11.push(ranges[4][2][i])	}" + Environment.NewLine);



            builder.Append("var d16 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[5][2].length;i++) {		d11.push(ranges[5][2][i])	}" + Environment.NewLine);



            builder.Append("d17 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[6][2].length;i++) {		d11.push(ranges[6][2][i])	}" + Environment.NewLine);



            builder.Append("d18 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[7][2].length;i++) {		d11.push(ranges[7][2][i])	}" + Environment.NewLine);

            builder.Append("d19 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[8][2].length;i++) {		d11.push(ranges[8][2][i])	}" + Environment.NewLine);



            builder.Append("d20 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[9][2].length;i++) {		d11.push(ranges[9][2][i])	}" + Environment.NewLine);


            builder.Append("d21 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[10][2].length;i++) {		d11.push(ranges[10][2][i])	}" + Environment.NewLine);


            builder.Append("$.plot($(\"#placeholder\"), [d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},			{data: d12, points: {radius: 9}},			{data: d13, points: {radius: 8}},			{data: d14, points: {radius: 7}},			{data: d15, points: {radius: 6}},			{data: d16, points: {radius: 5}},			{data: d17, points: {radius: 4}},			{data: d18, points: {radius: 3}},			{data: d19, points: {radius: 2}},			{data: d20, points:{radius: 2}},			{data: d21, points:{radius: 2}}], options);" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);


            builder.Append("$('#sorting input, #rangeNav span').click(function(){" + Environment.NewLine);
            builder.Append("  sortedCars = 0;" + Environment.NewLine);
            builder.Append("  sortedCars = sortCars(allCars);" + Environment.NewLine);
            builder.Append(" ranges = new Array();" + Environment.NewLine);
            builder.Append(" for (i = 0; i <= 9; i++)" + Environment.NewLine);


            builder.Append(" {" + Environment.NewLine);

            builder.Append("ranges[i] = new Array();" + Environment.NewLine);


            builder.Append("    ranges[i][0] = Math.round((percent[i]*100)*100)/100;" + Environment.NewLine);

            builder.Append(" ranges[i][1] = Math.round((percent[i+1]*100)*100)/100;" + Environment.NewLine);
            builder.Append("  }" + Environment.NewLine);


            builder.Append("   var addEnd = ranges.length;" + Environment.NewLine);

            builder.Append("  ranges[addEnd] = [];" + Environment.NewLine);

            builder.Append("  ranges[addEnd][0] = 100;" + Environment.NewLine);

            builder.Append("  ranges[addEnd][1] = 3500;" + Environment.NewLine);



            builder.Append("  carRange = [];" + Environment.NewLine);
            builder.Append("if (fRange == us) {" + Environment.NewLine);
            builder.Append(" for (i=0;i<=sortedCars.length-1;i++) {" + Environment.NewLine);
            builder.Append("	carRange[i] = [];" + Environment.NewLine);
            builder.Append("carRange[i][0] = sortedCars[i][0];" + Environment.NewLine);
            builder.Append("carRange[i][1] = sortedCars[i][1];" + Environment.NewLine);
            builder.Append("carRange[i][2] = sortedCars[i][2];" + Environment.NewLine);
            builder.Append("carRange[i][3] = sortedCars[i][3];" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append(" } else {" + Environment.NewLine);
            builder.Append("  for (i=0;i<sortedCars.length;i++) {" + Environment.NewLine);
            builder.Append("	if (sortedCars[i][3] <= fRange) {" + Environment.NewLine);
            builder.Append("carRange[i] = [];" + Environment.NewLine);
            builder.Append("carRange[i][0] = sortedCars[i][0];" + Environment.NewLine);
            builder.Append("carRange[i][1] = sortedCars[i][1];" + Environment.NewLine);
            builder.Append("carRange[i][2] = sortedCars[i][2];" + Environment.NewLine);
            builder.Append("carRange[i][3] = sortedCars[i][3];" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("carRange = $.grep(carRange,function(n){return(n);});		  " + Environment.NewLine);
            builder.Append("  for (v=0;v<=ranges.length-1;v++) {" + Environment.NewLine);
            builder.Append("	ranges[v][2] = new Array();" + Environment.NewLine);
            builder.Append("var b = 0;" + Environment.NewLine);
            builder.Append("for (i=0; i<=carRange.length-1;i++) {" + Environment.NewLine);
            builder.Append("	if (between(carRange[i][3],ranges[v][0],ranges[v][1])) {" + Environment.NewLine);
            builder.Append("ranges[v][2][b] = [carRange[i][1], carRange[i][2]];" + Environment.NewLine);
            builder.Append("b++;" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("d11 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[0][2].length;i++) {		d11.push(ranges[0][2][i])	}" + Environment.NewLine);

            builder.Append("d12 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[1][2].length;i++) {		d11.push(ranges[1][2][i])	}" + Environment.NewLine);



            builder.Append("d13 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[2][2].length;i++) {		d11.push(ranges[2][2][i])	}" + Environment.NewLine);



            builder.Append("d14 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[3][2].length;i++) {		d11.push(ranges[3][2][i])	}" + Environment.NewLine);



            builder.Append("d15 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[4][2].length;i++) {		d11.push(ranges[4][2][i])	}" + Environment.NewLine);



            builder.Append("d16 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[5][2].length;i++) {		d11.push(ranges[5][2][i])	}" + Environment.NewLine);



            builder.Append("d17 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[6][2].length;i++) {		d11.push(ranges[6][2][i])	}" + Environment.NewLine);



            builder.Append("d18 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[7][2].length;i++) {		d11.push(ranges[7][2][i])	}" + Environment.NewLine);

            builder.Append("d19 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[8][2].length;i++) {		d11.push(ranges[8][2][i])	}" + Environment.NewLine);



            builder.Append("d20 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[9][2].length;i++) {		d11.push(ranges[9][2][i])	}" + Environment.NewLine);


            builder.Append("d21 = [];" + Environment.NewLine);

            builder.Append("for (i=0;i<ranges[10][2].length;i++) {d21.push(ranges[10][2][i])}" + Environment.NewLine);


            builder.Append("	$.plot($(\"#placeholder\"), [ 	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},			{data: d12, points: {radius: 9}},			{data: d13, points: {radius: 8}},			{data: d14, points: {radius: 7}},			{data: d15, points: {radius: 6}},			{data: d16, points: {radius: 5}},			{data: d17, points: {radius: 4}},			{data: d18, points: {radius: 3}},			{data: d19, points: {radius: 2}},			{data: d20, points:{radius: 2}},			{data: d21, points:{radius: 2}}], options);" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);


            builder.Append("var d4 = {  label: 'range',				points: {show:false},				lines: {show:true, fill:false,},				color: 'green',				grid: {hoverable: false, clickable: false},				data: [					[startMileRange,averagePrice+(averagePrice*.20)],					[mileRangeEnd,averagePrice-(averagePrice*.20)]				]	}" + Environment.NewLine);


            builder.Append("var d5 = {  label: 'range',				points: {show:false},				lines: {show:true, fill: true},				color: 'lime',				grid: {hoverable: false, clickable: false},				data: [					[startMileRange,averagePrice+(averagePrice*.225)],					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.025)],					[mileRangeEnd,averagePrice-(averagePrice*.225)],					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.025)],					[startMileRange,averagePrice+(averagePrice*.225)]				]	}" + Environment.NewLine);



            builder.Append("	var d6 = {  label: 'range',				points: {show:false},				lines: {show:true, fill: true},				color: 'yellow',				grid: {hoverable: false, clickable: false},				data: [					[startMileRange,averagePrice+(averagePrice*.275)],					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.075)],					[mileRangeEnd,averagePrice-(averagePrice*.275)],					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.075)],					[startMileRange,averagePrice+(averagePrice*.275)]				]	}" + Environment.NewLine);




            builder.Append("var d7 = {  label: 'range',				points: {show:false},				lines: {show:true, fill: true},				color: 'orange',				grid: {hoverable: false, clickable: false},				data: [					[startMileRange,averagePrice+(averagePrice*.425)],					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.225)],					[mileRangeEnd,averagePrice-(averagePrice*.425)],					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.225)],					[startMileRange,averagePrice+(averagePrice*.425)]				]	}" + Environment.NewLine);

            builder.Append("var d8 = {  label: 'range',				points: {show:false},				lines: {show:true, fill: true},				color: 'red',				grid: {hoverable: false, clickable: false},				data: [					[startMileRange,averagePrice+(averagePrice*.7)],					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.5)],					[mileRangeEnd,averagePrice-(averagePrice*.7)],					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.5)],					[startMileRange,averagePrice+(averagePrice*.7)]				]	}	" + Environment.NewLine);



            builder.Append("var d3 = {label: yourCar[0][0], 				color: 'blue',				points: {radius: 6, symbol: 'diamond', fillColor: 'white'},				data: [[yourCarMiles,yourCarPrice]]	};" + Environment.NewLine);



            builder.Append("function mileFormat(v, axis) {" + Environment.NewLine);

            builder.Append(" var miles = v.toFixed(axis.tickDecimals);" + Environment.NewLine);



            builder.Append("return abbrNum(miles,0)+\" mi\";}" + Environment.NewLine);

            builder.Append("function priceFormat(v, axis) {" + Environment.NewLine);

            builder.Append(" var price = v.toFixed(axis.tickDecimals);" + Environment.NewLine);

            builder.Append("return '$'+abbrNum(price,0);}" + Environment.NewLine);





            builder.Append(" var options = {			legend: {show: false},			series: {color: '#000'},			lines: {show: false},			points: {show: true, radius: 4, fill: true, fillColor: false},			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},			grid: {hoverable: false, clickable: false}		}" + Environment.NewLine);

            builder.Append("$.plot($(\"#placeholder\"), [ 	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 11}},	{data: d12, points: {radius: 10}},	{data: d13, points: {radius: 9}},	{data: d14, points: {radius: 8}},	{data: d15, points: {radius: 7}},	{data: d16, points: {radius: 6}},	{data: d17, points: {radius: 5}},	{data: d18, points: {radius: 4}},	{data: d19, points: {radius: 3}},{data: d20, points: {radius: 2}}], options);" + Environment.NewLine);


            builder.Append("$('input[name=\"toggleGraph\"]').click(function(){" + Environment.NewLine);

            builder.Append("var width = document.getElementById('placeholder').clientWidth;" + Environment.NewLine);

            builder.Append("var height = document.getElementById('placeholder').clientHeight;" + Environment.NewLine);

            builder.Append("if (width <= 300 && height <= 143) {" + Environment.NewLine);

            builder.Append("document.getElementById('placeholder').style.width=553+'px';" + Environment.NewLine);

            builder.Append("	document.getElementById('placeholder').style.height=450+'px';" + Environment.NewLine);

            builder.Append("	document.getElementById('placeholder').title = '';" + Environment.NewLine);

            builder.Append("	document.getElementById('graphButton').value = 'Close';" + Environment.NewLine);

            builder.Append("  options = { " + Environment.NewLine);

            builder.Append("legend: { show: false }," + Environment.NewLine);

            builder.Append(" series: { color: '#000' }, " + Environment.NewLine);

            builder.Append("   lines: { show: false }, " + Environment.NewLine);

            builder.Append("   points: { show: true, radius: 4, fill: true, fillColor: false }," + Environment.NewLine);

            builder.Append("   xaxis: { show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat }, " + Environment.NewLine);

            builder.Append("   yaxis: { show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat }," + Environment.NewLine);

            builder.Append("   grid: {hoverable: false, clickable: true }" + Environment.NewLine);

            builder.Append("  } " + Environment.NewLine);





            builder.Append("$.plot($(\"#placeholder\"), [d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},		{data: d12, points: {radius: 9}},			{data: d13, points: {radius: 8}},			{data: d14, points: {radius: 7}},			{data: d15, points: {radius: 6}},			{data: d16, points: {radius: 5}},			{data: d17, points: {radius: 4}},			{data: d18, points: {radius: 3}},			{data: d19, points: {radius: 2}},			{data: d20, points:{radius: 2}},			{data: d21, points:{radius: 2}}], options);" + Environment.NewLine);

            builder.Append("$(\"#placeholder\").bind(\"plotclick\", function (event, pos, item) { " + Environment.NewLine);

            builder.Append("if (item.datapoint[0] == yourCar[0][1] && item.datapoint[1] == yourCar[0][2]) {" + Environment.NewLine);

            builder.Append("$('#graphCarImage').attr('src', yourCar[0][8]);" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = " + Environment.NewLine);
            builder.Append("	yourCar[0][0][1]+'<br />'+" + Environment.NewLine);
            builder.Append("yourCar[0][0][2]+'<br />'+" + Environment.NewLine);
            builder.Append("yourCar[0][0][3]+' '+" + Environment.NewLine);
            builder.Append("yourCar[0][6]+'<br /><div class=\"clear\"></div>'+" + Environment.NewLine);
            builder.Append("yourCar[0][10]+'<br /> VIN: '+" + Environment.NewLine);
            builder.Append("	yourCar[0][0][4]+'<br />';" + Environment.NewLine);
            builder.Append("	document.getElementById('daysOnMarket').innerHTML = addCommas(yourCar[0][7])+' days';" + Environment.NewLine);
            builder.Append("var changeList = ''" + Environment.NewLine);
            builder.Append("for (b=0;b<yourCar[0][9].length;b++) {" + Environment.NewLine);
            builder.Append("				changeList += '<li>'+yourCar[0][9][b][0]+' - $'+addCommas(yourCar[0][9][b][1])+'</li>';" + Environment.NewLine);
            builder.Append("	}" + Environment.NewLine);
            builder.Append("document.getElementById('priceChanges').innerHTML = changeList;" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = addCommas(yourCar[0][1]);" + Environment.NewLine);
            builder.Append("		document.getElementById('price').innerHTML = addCommas(yourCar[0][2]);" + Environment.NewLine);
            builder.Append("document.getElementById('diffM').innerHTML = addCommas(posNeg(yourCar[0][1]-yourCar[0][1]));" + Environment.NewLine);
            builder.Append("document.getElementById('diffP').innerHTML = addCommas(posNeg(yourCar[0][2]-yourCar[0][2]));" + Environment.NewLine);
            builder.Append("document.getElementById('distance').innerHTML = addCommas(yourCar[0][3])+' Miles';" + Environment.NewLine);
            builder.Append("if (yourCar[0][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} " + Environment.NewLine);
            builder.Append("else {document.getElementById('certified').innerHTML = 'No';}			" + Environment.NewLine);
            builder.Append("} else if (item.datapoint[0] == averageMile && item.datapoint[1] == averagePrice) {" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = '';" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = 'Midrange Mileage: '+mileMedian;" + Environment.NewLine);
            builder.Append("document.getElementById('price').innerHTML = priceMedian+' is your Midrange Pricing';" + Environment.NewLine);
            builder.Append("} else if (item.series.label == 'range') {" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = '';" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = '';" + Environment.NewLine);
            builder.Append("document.getElementById('price').innerHTML = '';" + Environment.NewLine);
            builder.Append("} else {" + Environment.NewLine);
            builder.Append("for (i=0;i<allCars.length;i++) {" + Environment.NewLine);
            builder.Append("if (item.datapoint[0] == allCars[i][1] && item.datapoint[1] == allCars[i][2]) {" + Environment.NewLine);
            builder.Append("	$('#graphCarImage').attr('src',allCars[i][8]);" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = " + Environment.NewLine);
            builder.Append("	allCars[i][0][1]+'<br />'+" + Environment.NewLine);
            builder.Append("allCars[i][0][2]+'<br />'+" + Environment.NewLine);
            builder.Append("allCars[i][0][3]+' '+" + Environment.NewLine);
            builder.Append("allCars[i][6]+'<br /><div class=\"clear\"></div>'+" + Environment.NewLine);
            builder.Append("allCars[i][10]+'<br /> VIN: '+" + Environment.NewLine);
            builder.Append("	allCars[i][0][4]+'<br />';" + Environment.NewLine);
            builder.Append("	document.getElementById('daysOnMarket').innerHTML = addCommas(allCars[i][7])+' days';" + Environment.NewLine);
            builder.Append("var changeList = ''" + Environment.NewLine);
            builder.Append("for (b=0;b<allCars[i][9].length;b++) {" + Environment.NewLine);
            builder.Append("changeList += '<li>'+allCars[i][9][b][0]+' - $'+addCommas(allCars[i][9][b][1])+'</li>';" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("document.getElementById('priceChanges').innerHTML = changeList;" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = addCommas(allCars[i][1]);" + Environment.NewLine);
            builder.Append("document.getElementById('price').innerHTML = addCommas(allCars[i][2]);" + Environment.NewLine);
            builder.Append("document.getElementById('diffM').innerHTML = addCommas(posNeg(allCars[i][1]-yourCar[0][1]))" + Environment.NewLine);
            builder.Append("document.getElementById('diffP').innerHTML = addCommas(posNeg(allCars[i][2]-yourCar[0][2]));" + Environment.NewLine);
            builder.Append("document.getElementById('distance').innerHTML = addCommas(allCars[i][3])+' Miles';" + Environment.NewLine);
            builder.Append("if (allCars[i][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} " + Environment.NewLine);
            builder.Append("	else {document.getElementById('certified').innerHTML = 'No';}" + Environment.NewLine);
            builder.Append("	} " + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);
            builder.Append("} else {" + Environment.NewLine);
            builder.Append("document.getElementById('placeholder').title = 'Click on the graph to change its size.';" + Environment.NewLine);

            builder.Append("	document.getElementById('placeholder').style.width=300+'px';" + Environment.NewLine);

            builder.Append("	document.getElementById('placeholder').style.height=143+'px';" + Environment.NewLine);

            builder.Append("	document.getElementById('graphButton').value = 'Expand';" + Environment.NewLine);


            builder.Append("  options = { " + Environment.NewLine);

            builder.Append("legend: { show: false }," + Environment.NewLine);

            builder.Append(" series: { color: '#000' }, " + Environment.NewLine);

            builder.Append("   lines: { show: false }, " + Environment.NewLine);

            builder.Append("   points: { show: true, radius: 4, fill: true, fillColor: false }," + Environment.NewLine);

            builder.Append("   xaxis: { show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat }, " + Environment.NewLine);

            builder.Append("   yaxis: { show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat }," + Environment.NewLine);

            builder.Append("grid: {hoverable: false, clickable: false}" + Environment.NewLine);

            builder.Append("  } " + Environment.NewLine);


            builder.Append("$.plot($(\"#placeholder\"), [	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},			{data: d12, points: {radius: 9}},			{data: d13, points: {radius: 8}},			{data: d14, points: {radius: 7}},			{data: d15, points: {radius: 6}},			{data: d16, points: {radius: 5}},			{data: d17, points: {radius: 4}},			{data: d18, points: {radius: 3}},			{data: d19, points: {radius: 2}},			{data: d20, points:{radius: 2}},			{data: d21, points:{radius: 2}}], options);" + Environment.NewLine);
            builder.Append("  } " + Environment.NewLine);

            builder.Append("  }); " + Environment.NewLine);



            builder.Append("$('#placeholder').dblclick(function(){" + Environment.NewLine);
            builder.Append("var width = document.getElementById('placeholder').clientWidth;" + Environment.NewLine);
            builder.Append("	var height = document.getElementById('placeholder').clientHeight;" + Environment.NewLine);
            builder.Append("if (width <= 300 && height <= 143) {" + Environment.NewLine);
            builder.Append("	document.getElementById('placeholder').style.width=553+'px';" + Environment.NewLine);
            builder.Append("document.getElementById('placeholder').style.height=350+'px';" + Environment.NewLine);
            builder.Append("	document.getElementById('placeholder').title = '';" + Environment.NewLine);
            builder.Append("	document.getElementById('graphButton').value = 'Close';" + Environment.NewLine);
            builder.Append("options = {			legend: {show: false},			series: {color: '#000'},			lines: {show: false},			points: {show: true, radius: 4, fill: true, fillColor: false},			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},			grid: {hoverable: false, clickable: true }		}" + Environment.NewLine);

            builder.Append("$.plot($(\"#placeholder\"), [ " + Environment.NewLine);
            builder.Append("d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},			{data: d12, points: {radius: 9}},			{data: d13, points: {radius: 8}},			{data: d14, points: {radius: 7}},			{data: d15, points: {radius: 6}},			{data: d16, points: {radius: 5}},			{data: d17, points: {radius: 4}},			{data: d18, points: {radius: 3}},			{data: d19, points: {radius: 2}},			{data: d20, points:{radius: 2}},			{data: d21, points:{radius: 2}}], options);" + Environment.NewLine);

            builder.Append("$(\"#placeholder\").bind(\"plotclick\", function (event, pos, item) {" + Environment.NewLine);
            builder.Append("if (item.datapoint[0] == yourCar[0][1] && item.datapoint[1] == yourCar[0][2]) {" + Environment.NewLine);
            builder.Append("$('#graphCarImage').attr('src', yourCar[0][8]);" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = " + Environment.NewLine);
            builder.Append("	yourCar[0][0][1]+'<br />'+" + Environment.NewLine);
            builder.Append("yourCar[0][0][2]+'<br />'+" + Environment.NewLine);
            builder.Append("yourCar[0][0][3]+' '+" + Environment.NewLine);
            builder.Append("yourCar[0][6]+'<br /><div class=\"clear\"></div>'+" + Environment.NewLine);
            builder.Append("yourCar[0][10]+'<br /> VIN: '+" + Environment.NewLine);
            builder.Append("	yourCar[0][0][4]+'<br />';" + Environment.NewLine);
            builder.Append("	document.getElementById('daysOnMarket').innerHTML = addCommas(yourCar[0][7])+' days';" + Environment.NewLine);
            builder.Append("var changeList = ''" + Environment.NewLine);
            builder.Append("for (b=0;b<yourCar[0][9].length;b++) {" + Environment.NewLine);
            builder.Append("				changeList += '<li>'+yourCar[0][9][b][0]+' - $'+addCommas(yourCar[0][9][b][1])+'</li>';" + Environment.NewLine);
            builder.Append("	}" + Environment.NewLine);
            builder.Append("document.getElementById('priceChanges').innerHTML = changeList;" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = addCommas(yourCar[0][1]);" + Environment.NewLine);
            builder.Append("		document.getElementById('price').innerHTML = addCommas(yourCar[0][2]);" + Environment.NewLine);
            builder.Append("document.getElementById('diffM').innerHTML = addCommas(posNeg(yourCar[0][1]-yourCar[0][1]));" + Environment.NewLine);
            builder.Append("document.getElementById('diffP').innerHTML = addCommas(posNeg(yourCar[0][2]-yourCar[0][2]));" + Environment.NewLine);
            builder.Append("document.getElementById('distance').innerHTML = addCommas(yourCar[0][3])+' Miles';" + Environment.NewLine);
            builder.Append("if (yourCar[0][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} " + Environment.NewLine);
            builder.Append("else {document.getElementById('certified').innerHTML = 'No';}			" + Environment.NewLine);
            builder.Append("} else if (item.datapoint[0] == averageMile && item.datapoint[1] == averagePrice) {" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = '';" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = 'Midrange Mileage: '+mileMedian;" + Environment.NewLine);
            builder.Append("document.getElementById('price').innerHTML = priceMedian+' is your Midrange Pricing';" + Environment.NewLine);
            builder.Append("} else if (item.series.label == 'range') {" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = '';" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = '';" + Environment.NewLine);
            builder.Append("document.getElementById('price').innerHTML = '';" + Environment.NewLine);
            builder.Append("} else {" + Environment.NewLine);
            builder.Append("for (i=0;i<allCars.length;i++) {" + Environment.NewLine);
            builder.Append("if (item.datapoint[0] == allCars[i][1] && item.datapoint[1] == allCars[i][2]) {" + Environment.NewLine);
            builder.Append("	$('#graphCarImage').attr('src',allCars[i][8]);" + Environment.NewLine);
            builder.Append("document.getElementById('car').innerHTML = " + Environment.NewLine);
            builder.Append("	allCars[i][0][1]+'<br />'+" + Environment.NewLine);
            builder.Append("allCars[i][0][2]+'<br />'+" + Environment.NewLine);
            builder.Append("allCars[i][0][3]+' '+" + Environment.NewLine);
            builder.Append("allCars[i][6]+'<br /><div class=\"clear\"></div>'+" + Environment.NewLine);
            builder.Append("allCars[i][10]+'<br /> VIN: '+" + Environment.NewLine);
            builder.Append("	allCars[i][0][4]+'<br />';" + Environment.NewLine);
            builder.Append("	document.getElementById('daysOnMarket').innerHTML = addCommas(allCars[0][7])+' days';" + Environment.NewLine);
            builder.Append("var changeList = ''" + Environment.NewLine);
            builder.Append("for (b=0;b<allCars[i][9].length;b++) {" + Environment.NewLine);
            builder.Append("changeList += '<li>'+allCars[i][9][b][0]+' - $'+addCommas(allCars[i][9][b][1])+'</li>';" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("document.getElementById('priceChanges').innerHTML = changeList;" + Environment.NewLine);
            builder.Append("document.getElementById('miles').innerHTML = addCommas(allCars[i][1]);" + Environment.NewLine);
            builder.Append("document.getElementById('price').innerHTML = addCommas(allCars[i][2]);" + Environment.NewLine);
            builder.Append("document.getElementById('diffM').innerHTML = addCommas(posNeg(allCars[i][1]-yourCar[0][1]))" + Environment.NewLine);
            builder.Append("document.getElementById('diffP').innerHTML = addCommas(posNeg(allCars[i][2]-yourCar[0][2]));" + Environment.NewLine);
            builder.Append("document.getElementById('distance').innerHTML = addCommas(allCars[i][3])+' Miles';" + Environment.NewLine);
            builder.Append("if (allCars[i][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} " + Environment.NewLine);
            builder.Append("	else {document.getElementById('certified').innerHTML = 'No';}" + Environment.NewLine);
            builder.Append("	} " + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);
            builder.Append("}" + Environment.NewLine);

            builder.Append("});" + Environment.NewLine);
            builder.Append("} else {" + Environment.NewLine);
            builder.Append("document.getElementById('placeholder').title = 'Click on the graph to change its size.';" + Environment.NewLine);

            builder.Append("	document.getElementById('placeholder').style.width=300+'px';" + Environment.NewLine);

            builder.Append("	document.getElementById('placeholder').style.height=143+'px';" + Environment.NewLine);

            builder.Append("	document.getElementById('graphButton').value = 'Expand';" + Environment.NewLine);


            builder.Append("  options = { " + Environment.NewLine);

            builder.Append("legend: { show: false }," + Environment.NewLine);

            builder.Append(" series: { color: '#000' }, " + Environment.NewLine);

            builder.Append("   lines: { show: false }, " + Environment.NewLine);

            builder.Append("   points: { show: true, radius: 4, fill: true, fillColor: false }," + Environment.NewLine);

            builder.Append("   xaxis: { show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat }, " + Environment.NewLine);

            builder.Append("   yaxis: { show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat }," + Environment.NewLine);

            builder.Append("grid: {hoverable: false, clickable: false}" + Environment.NewLine);

            builder.Append("  } " + Environment.NewLine);


            builder.Append("$.plot($(\"#placeholder\"), [ 	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},			{data: d12, points: {radius: 9}},			{data: d13, points: {radius: 8}},			{data: d14, points: {radius: 7}},			{data: d15, points: {radius: 6}},			{data: d16, points: {radius: 5}},			{data: d17, points: {radius: 4}},			{data: d18, points: {radius: 3}},			{data: d19, points: {radius: 2}},			{data: d20, points:{radius: 2}},			{data: d21, points:{radius: 2}}], options);" + Environment.NewLine);
            builder.Append("  } " + Environment.NewLine);

            builder.Append("  }); " + Environment.NewLine);

            builder.Append("document.getElementById('totalCars').innerHTML = totalCars;" + Environment.NewLine);
            builder.Append("});" + Environment.NewLine);


            builder.Append(" </script>");

            return builder.ToString();


        }

        public static string DynamicHtmlControlAppraisal(this HtmlHelper<AppraisalViewFormModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";

            switch (fieldName)
            {


                case "Date":
                    result = "<input class=\"z-index\" type=\"text\" disabled=\"disabled\" id=\"" + name + "\" name=\"" + name + "\" value=\"" +
                             DateTime.Now.ToShortDateString() + "\" />";
                    break;
                default:
                    result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                    break;

            }

            return result;
        }

        public static string DynamicHtmlControl(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";

            if (model.Success)
            {
                switch (fieldName)
                {
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealershipId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.StockNumber))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.StockNumber + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;
                    case "ExteriorColor":


                        result = "<select class=\"z-index\" style=\"width:70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            foreach (Color ec in model.ExteriorColorList)
                            {
                                result += "<option>" + ec.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;
                    case "InteriorColor":


                        result = "<select class=\"z-index\" style=\"width:70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            foreach (Color ic in model.InteriorColorList)
                            {
                                result += "<option>" + ic.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;

                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;



                    case "Odometer":
                        if (String.IsNullOrEmpty(model.Mileage))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litters + "\" />";
                        break;
                    case "Tranmission":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.MSRP + "\" />";
                        break;
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\" name=\"Packages\">";

                            result += "<ul class=\"options\">";

                            foreach (var fo in model.FactoryPackageOptions)
                            {
                                var newString = regex.Replace(fo.getName(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.getMSRP() + "</li>";
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\" >";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages available for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\" name=\"Options\">";
                        result += "<ul class=\"options\">";
                        if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                var newString = regex.Replace(fo.getName(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.getMSRP() + "</li>";
                            }
                        }

                        result += "</ul>";
                        result += "</div>";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "JavaScript":
                        //result = generateGraphJavaScript(model);
                        break;
                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;

                }
            }
            else
            {
                string defaultName = "Select a make";
                string defaultId = "0";
                switch (fieldName)
                {
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + DateTime.Now.ToShortDateString() + " />";
                        break;
                    case "Stock":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + model.ModelYear + " />";
                        break;
                    case "Make":
                        result += "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "<option value=\"" + defaultId + "****" + defaultName + "\">" + defaultName + "</option>";
                        if (model.MakeNameList != null)
                        {
                            foreach (var dv in model.MakeNameList)
                            {
                                result += "<option value=\"" + dv.id + "****" + dv.Value + "\">" + dv.Value + "</option>";
                            }
                        }
                        result += "</select>";


                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Model":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "Trim":
                        result = "<select class=\"z-index\"  id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "ExteriorColor":
                        result = "<select class=\"z-index\"  style=\"width: 70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "InteriorColor":
                        result = "<select class=\"z-index\"  style=\"width: 70px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 40px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 40px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;


                    case "Odometer":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Cylinders":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break; ;
                    case "Litters":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "Tranmission":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "Type":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "BodyType":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "FuelType":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        //result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        //result += "</select>";

                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        break;
                    case "FactoryPackageOption":
                        result += "<div id=\"Packages\" name=\"Packages\">";
                        for (int i = 0; i < 10; i++)
                        {
                            result += "<li>";
                            result += "<input type=\"checkbox\" class=\"z-index hider\" onclick=\"javascript:changeMSRP(this)\"   name=\"selectedOptions\" price=\"PackagePrice\" value=\"Package\"  />";
                            result += "<label class=\"z-index hider\"  for=\"" + name + "\">" + "</label>";
                            result += "</li>";
                        }
                        result += "</div>";
                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\" name=\"Options\">";
                        for (int i = 0; i < 40; i++)
                        {
                            result += "<li>";
                            result += "<input type=\"checkbox\" class=\"z-index hider\" onclick=\"javascript:changeMSRP(this)\"  name=\"selectedOptions\" price=\"OptionPrice\" value=\"Options\" />";
                            result += "<label class=\"z-index hider\"  for=\"" + name + "\">" + "</label>";
                            result += "</li>";


                        }
                        result += "</div>";
                        break;

                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarImageUrl + "\"" + " />"; ;
                        break;
                    case "HiddenPhotoProfile":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                        break;

                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "JavaScript":
                        result = generateGraphJavaScript(model);
                        break;
                    default:
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"error\"" + " />";

                        break;

                }
            }
            return result;
        }

        public static string DynamicHtmlControlForProfile(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";

            if (model.Success)
            {
                switch (fieldName)
                {
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealershipId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.StockNumber))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.StockNumber + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;
                    case "ExteriorColor":
                        var ExteriorColorList = model.ExteriorColorList;
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        foreach (Color ec in ExteriorColorList)
                        {
                            result += "<option value=\"" + ec.colorName + "\">" + ec.colorName + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "InteriorColor":
                        var InteriorColorList = model.InteriorColorList;
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        foreach (Color ic in InteriorColorList)
                        {
                            result += "<option value=\"" + ic.colorName + "\">" + ic.colorName + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusExteriorColor + "\" />";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusInteriorColor + "\" />";

                        break;
                    case "Odometer":
                        if (String.IsNullOrEmpty(model.Mileage))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litters + "\" />";
                        break;
                    case "Tranmission":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;

                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.MSRP + "\" />";
                        break;
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\" name=\"Packages\">";

                            result += "<ul class=\"options\">";

                            foreach (var fo in model.FactoryPackageOptions)
                            {
                                var newString = regex.Replace(fo.getName(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.getMSRP() + "</li>";
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages available for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\" name=\"Options\">";
                        result += "<ul class=\"options\">";
                        if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                var newString = regex.Replace(fo.getName(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + newString + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + newString + fo.getMSRP() + "</li>";
                            }
                        }

                        result += "</ul>";
                        result += "</div>";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "HiddenEngineType":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Engine + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "JavaScript":
                        result = generateGraphJavaScript(model);
                        break;
                    case "HiddenTrim":


                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + "\" />";

                        break;
                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;

                }
            }
            else
            {

                switch (fieldName)
                {

                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + DateTime.Now.ToShortDateString() + " />";
                        break;
                    case "Stock":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + model.ModelYear + " />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=" + model.Make + " />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Model":
                        var modelList = model.ModelList;

                        if (modelList != null && modelList.Count > 0)
                        {

                            result += "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";


                            foreach (var md in modelList)
                            {
                                result += "<option value=\"" + md.id + "****" + md.Value + "\">" + md.Value + "</option>";
                            }
                            result += "</select>";
                        }
                        else
                        {
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        }

                        break;
                    case "Trim":
                        var styleList = model.StyleList;

                        if (styleList != null && styleList.Count > 0)
                        {
                            result += "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                            foreach (var st in styleList)
                            {
                                result += "<option value=\"" + st.id + "****" + st.trim + "\">" + st.trim + "</option>";
                            }
                            result += "</select>";
                        }
                        else
                        {
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        }

                        break;
                    case "HiddenEngineType":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Engine + "\" />";
                        break;
                    case "HiddenTrim":

                        modelList = model.ModelList;
                        if (modelList != null && modelList.Count > 0)
                        {
                            result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + modelList.First().id + "\" />";
                        }
                        break;
                    case "ExteriorColor":
                        var ExteriorColorList = model.ExteriorColorList;
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (ExteriorColorList != null && ExteriorColorList.Count > 0)
                        {
                            foreach (Color ec in ExteriorColorList)
                            {
                                result += "<option value=\"" + ec.colorName + "\">" + ec.colorName + "</option>";
                            }
                        }
                        else
                        {
                            result += "<option value=\"" + model.ExteriorColor + "\">" + model.ExteriorColor + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "InteriorColor":
                        var InteriorColorList = model.InteriorColorList;
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (InteriorColorList != null && InteriorColorList.Count > 0)
                        {
                            foreach (Color ic in InteriorColorList)
                            {
                                result += "<option value=\"" + ic.colorName + "\">" + ic.colorName + "</option>";
                            }
                        }
                        else
                        {
                            result += "<option value=\"" + model.InteriorColor + "\">" + model.InteriorColor + "</option>";
                        }
                        result += "</select>";

                        break;
                    case "CustomExtColor":

                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusExteriorColor + "\" />";

                        break;
                    case "CustomIntColor":


                        result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CusInteriorColor + "\" />";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;


                    case "Odometer":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litters + "\" />";
                        break;
                    case "Tranmission":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Type":
                        result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        result += "</select>";

                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.MSRP + "\" />";
                        break;
                    case "FactoryPackageOption":
                        result += "<div id=\"Packages\">";

                        result += "</div>";
                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\">";
                        result += "</div>";
                        break;

                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />"; ;
                        break;
                    case "HiddenPhotoProfile":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                        break;

                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "JavaScript":
                        result = generateGraphJavaScript(model);
                        break;
                    default:
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"error\"" + " />";

                        break;

                }
            }
            return result;
        }

        public static string DynamicHtmlLabel(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            //var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string result = "";
            switch (fieldName)
            {
                case "MarketRangeEditProfile":

                    if (model.MarketRange == 3)
                        result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) +"\"><img src=\"" + urlHelper.Content("~/images/above.jpg") +
                                       "\" style=\"height: 30px; width: 25px;\" /></a>" ;
                    else if (model.MarketRange == 2)
                        result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) +"\"><img src=\"" + urlHelper.Content("~/images/at.jpg") +
                                       "\" style=\"height: 30px; width: 25px;\" /></a>";
                        
                    else if (model.MarketRange == 1)
                        result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) +"\"><img src=\"" + urlHelper.Content("~/images/below.jpg") +
                                       "\" style=\"height: 30px; width: 25px;\" /></a>";
                        
                    else
                    result = "<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = model.ListingId }) +"\"><img src=\"" + urlHelper.Content("~/images/question.gif") +
                                       "\" style=\"height: 30px; width: 25px;\" /></a>";

                    break;


                case "Vin":
                    result = "Vin:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Vin + "</label>";
                    break;
                case "Date":
                    result = "<label  for=\"" + name + "\">" + DateTime.Now.ToShortDateString() + "</label>";
                    break;
                case "Stock":
                    result = "Stock:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.StockNumber + "</label>";
                    break;
                case "AppraisalID":
                    result = "Appraisal:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.AppraisalGenerateId + "</label>";
                    break;
                case "Year":
                    result = "Year:<label  for=\"" + name + "\">" + model.ModelYear + "</label>";
                    break;
                case "Make":
                    result = "Make:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Make + "</label>";
                    break;
                case "Model":
                    result = "Model:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Model, 15) + "</label>";
                    break;
                case "Trim":
                    result = "Trim:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Trim, 15) + "</label>";
                    break;
                case "ExteriorColor":
                    if (String.IsNullOrEmpty(model.CusExteriorColor))
                        result = "Exterior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.ExteriorColor, 15) + "</label>";
                    else
                        result = "Exterior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusExteriorColor, 15) + "</label>";
                    break;
                case "InteriorColor":
                    if (String.IsNullOrEmpty(model.CusInteriorColor))
                        result = "Interior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.InteriorColor, 15) + "</label>";
                    else
                        result = "Interior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusInteriorColor, 15) + "</label>";

                    break;
                case "DaysInInventory":
                    result = "Age:<label id=\"" + name + "\" for=\"" + name + "\">" + model.DaysInInvenotry + " days </label>";
                    break;
                case "Odometer":
                    result = "Odometer:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Mileage + "</label>";
                    break;
                case "Cylinders":
                    result = "Cylinders:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Cylinder + "</label>";
                    break;
                case "Litters":
                    result = "Liters/CC:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Litters + "</label>";
                    break;
                case "Tranmission":
                    result = "Transmission:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Tranmission, 10) + "</label>";
                    break;
                case "Doors":
                    result = "Doors:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Door + "</label>";

                    break;

                case "BodyType":
                    result = "BodyType:<label id=\"" + name + "\" for=\"" + name + "\">" + model.BodyType + "</label>";
                    break;
                case "FuelType":
                    result = "Fuel:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Fuel + "</label>";
                    break;
                case "DriveType":
                    result = "Drive Type:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.GetShortDrive(model.WheelDrive) + "</label>";
                    break;
                case "MSRP":
                    result = "MSRP:<label id=\"" + name + "\" for=\"" + name + "\">" + model.MSRP + "</label>";
                    break;
                case "SalePrice":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.SalePrice + "</label>";

                    break;
                case "DealerCost":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.DealerCost + "</label>";

                    break;
                case "ACV":
                    result = "<label  id=\"" + name + "\">" + model.ACV + "</label>";

                    break;
                case "TruckType":
                    result = "Truck Type: <label  id=\"" + name + "\">" + model.SelectedTruckType + "</label>";

                    break;
                case "TruckClass":
                    result = "Truck Class: <label  id=\"" + name + "\">" + model.SelectedTruckClass + "</label>";

                    break;
                case "TruckCategory":
                    result = "Truck Category: <label  id=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedTruckCategory, 10) + "</label>";

                    break;
                case "Title":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.OrginalName + "</label>";
                    break;
                case "TitleWithoutTrim":
                    string title = model.ModelYear + " " + model.Make + " " + model.Model;
                    if (title.Length > 21)
                    {
                        if (title.Length < 30)
                            result = "<label style=\"font-size: .8em\" id=\"" + name + "\" for=\"" + name + "\">" +
                                     title + "</label>";
                        else
                        {
                            result = "<label style=\"font-size: .8em\" id=\"" + name + "\" for=\"" + name + "\">" +
                                title.Substring(0, 30) + "</label>";
                        }
                    }
                    else
                        result = "<label id=\"" + name + "\" for=\"" + name + "\">" + title + "</label>";
                    break;
                case "MarketPriceLowest":
                    result = "<label for=\"" + name + "\">" + model.LowestPrice + "</label>";
                    break;
                case "MarketPriceAverage":
                    result = "<label for=\"" + name + "\">" + model.AveragePriceOnMarket + "</label>";
                    break;
                case "MarketPriceHighest":
                    result = "<label for=\"" + name + "\">" + model.HighestPrice + "</label>";
                    break;


                case "CarImage":

                    result = "<img id=\"pIdImage\" name=\"pIdImage\" src=\"" + model.SinglePhoto + "\"" + " width=\"160\" />" + Environment.NewLine;
                    result += "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";

                    break;
                case "HiddenPhotoProfile":
                    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                    break;
                case "UploadCarImage":

                    if (model.UploadPhotosURL != null)
                    {
                        int index = 1;
                        foreach (string tmp in model.UploadPhotosURL)
                        {

                            result += " <li class=\"selector\">" + Environment.NewLine;
                            result += " <div class=\"centerImage\">" + Environment.NewLine;
                            string fullSizeImage = tmp.Replace("ThumbnailSizeImages", "NormalSizeImages");

                            result += " <a id=\"" + index + "\" class=\"image\" rel=\"group1\" href=\"" + fullSizeImage + "\">" + Environment.NewLine;
                            //result += "<img id=\"" + index + "\" class=\"image\" src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Upload\" />" + Environment.NewLine;

                            result += "</a>" + Environment.NewLine;
                            result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index + "\" name=\"image" + index++ + "\" />" + Environment.NewLine;

                            result += "</div>" + Environment.NewLine;
                            result += "</li>" + Environment.NewLine;

                        }

                        if (model.UploadPhotosURL.Count < 100)
                        {
                            int loopNumber = 100 - model.UploadPhotosURL.Count;


                            for (int i = 0; i < loopNumber; i++)
                            {
                               
                                string tmp = "";

                                if (i % 3 == 0)
                                    tmp += urlHelper.Content("~/Images/40x40grey1.jpg");
                                else if (i % 3 == 1)
                                    tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                                else
                                    tmp += urlHelper.Content("~/Images/40x40grey3.jpg");

                                result += " <li class=\"selector\">" + Environment.NewLine;
                                result += " <div class=\"centerImage\">" + Environment.NewLine;

                                result += " <a class=\"image\" rel=\"group1\" href=\"" + tmp + "\">" + Environment.NewLine;
                                result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" value=\"Default\" />" + Environment.NewLine;

                                result += "</a>" + Environment.NewLine;
                                result += " <input type=\"checkbox\" checked=\"false\"  id=\"image" + index + "\" name=\"image" + index++ + "\" />" + Environment.NewLine;

                                result += "</div>";
                                result += "</li>" + Environment.NewLine; ;

                            }
                        }
                    }
                    else
                    {
                        //int index = 1;

                        for (int i = 0; i < 100; i++)
                        {
                         
                            string tmp = "";

                            if (i % 3 == 0)

                                tmp += urlHelper.Content("~/Images/40x40grey1.jpg");


                            else if (i % 3 == 1)

                                tmp += urlHelper.Content("~/Images/40x40grey2.jpg");
                            else
                                tmp += urlHelper.Content("~/Images/40x40grey3.jpg");
                            result += " <li class=\"selector\">";
                            result += " <div class=\"centerImage\">";
                            result += "<img src=\"" + tmp + "\" width=\"40\" height=\"40\" />" + Environment.NewLine;
                            //result += " <input type=\"checkbox\" name=\"image" + index++ + "\" />";
                            result += "</div>";
                            result += "</li>" + Environment.NewLine; ;
                        }
                    }

                    break;
                case "SmallCarImage":
                    if (model.MutiplePhotos != null)
                    {
                        if (model.MutiplePhotos.Count != 0)
                        {
                            foreach (string url in model.MutiplePhotos)
                                result += "<li class=\"float\"><img src=\"" + url + "\"" + " /></li>";
                        }
                    }

                    break;
                case "GoogleMap":

                    result = "<img id=\"googleMap\" src=\"http://maps.googleapis.com/maps/api/staticmap?center=" + model.Latitude + "," + model.Longtitude + "&markers=color:red%7C" + model.Latitude + "," + model.Longtitude + "&zoom=14&size=320x320&sensor=false" + "\"/>" + Environment.NewLine;
                    break;

                case "Description":
                    if (!String.IsNullOrEmpty(model.Description))
                    {
                        if (model.Description.Length > 150)
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Description, 150) + "..." + "</label>";

                        else
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Description) + "</label>";

                    }
                    else
                        result = "<label id=\"" + name + "\" for=\"" + name + "\"></label>";
                    break;



                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }

        public static string DynamicHtmlLabel(this HtmlHelper<AppraisalViewFormModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";
            switch (fieldName)
            {
                case "TitleWithoutTrim":
                    string title = model.ModelYear + " " + model.Make + " " + model.SelectedModel;
                    if (title.Length > 21)
                        result = "<label style=\"font-size: .8em\" id=\"" + name + "\" for=\"" + name + "\">" + title + "</label>";
                    else
                        result = "<label id=\"" + name + "\" for=\"" + name + "\">" + title + "</label>";
                    break;
                case "Vin":
                    result = "Vin:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.VinNumber + "</label>";
                    break;
                case "Date":
                    result = "<label  for=\"" + name + "\">" + DateTime.Now.ToShortDateString() + "</label>";
                    break;
                case "Stock":
                    result = "Stock:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.StockNumber + "</label>";
                    break;
                case "AppraisalID":
                    result = "Appraisal:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.AppraisalGenerateId + "</label>";
                    break;
                case "Year":
                    result = "Year:<label  for=\"" + name + "\">" + model.ModelYear + "</label>";
                    break;
                case "Make":
                    result = "Make:<label  id=\"" + name + "\"  for=\"" + name + "\">" + model.Make + "</label>";
                    break;
                case "Model":
                    result = "Model:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.AppraisalModel, 15) + "</label>";
                    break;
                case "Trim":
                    result = "Trim:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedTrim, 15) + "</label>";
                    break;

                case "ExteriorColor":
                    if (String.IsNullOrEmpty(model.CusExteriorColor))
                        result = "Exterior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedExteriorColorValue, 15) + "</label>";
                    else
                        result = "Exterior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusExteriorColor, 15) + "</label>";
                  

                    //result = "Exterior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.trimString(model.CusExteriorColor==null?String.Empty:model.CusExteriorColor, 15) + "</label>";
                    break;
                case "InteriorColor":
                    if (String.IsNullOrEmpty(model.CusInteriorColor))
                        result = "Interior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedInteriorColor, 15) + "</label>";
                    else
                        result = "Interior:<label  id=\"" + name + "\"  for=\"" + name + "\">" + CommonHelper.TrimString(model.CusInteriorColor, 15) + "</label>";

                    break;
                case "TruckType":
                    result = "Truck Type: <label  id=\"" + name + "\">" + model.SelectedTruckType + "</label>";

                    break;
                case "TruckClass":
                    result = "Truck Class: <label  id=\"" + name + "\">" + model.SelectedTruckClass + "</label>";

                    break;
                case "TruckCategory":
                    result = "Truck Category: <label  id=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedTruckCategory, 10) + "</label>";
                    break;
                case "Odometer":
                    result = "Odometer:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Mileage + "</label>";
                    break;
                case "Cylinders":
                    result = "Cylinders:<label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedCylinder + "</label>";
                    break;
                case "Litters":
                    result = "Liters/CC:<label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedLiters + "</label>";
                    break;
                case "Tranmission":
                    result = "Transmission:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.SelectedTranmission, 10) + "</label>";
                    break;
                case "Doors":
                    result = "Doors:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Door + "</label>";

                    break;

                case "BodyType":
                    result = "BodyType:<label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedBodyType + "</label>";
                    break;
                case "FuelType":
                    result = "Fuel:<label id=\"" + name + "\" for=\"" + name + "\">" + model.SelectedFuel + "</label>";
                    break;
                case "DriveType":
                    result = "Drive Type:<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.GetShortDrive(model.SelectedDriveTrain) + "</label>";
                    break;
                case "MSRP":
                    result = "MSRP:<label id=\"" + name + "\" for=\"" + name + "\">" + model.MSRP + "</label>";
                    break;
                case "SalePrice":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.SalePrice + "</label>";

                    break;
                case "DealerCost":
                    result = "<label id=\"" + name + "\" for=\"" + name + "\">" + model.DealerCost + "</label>";

                    break;
                case "ACV":
                    result = "<label  for=\"" + name + "\">" + model.ACV + "</label>";

                    break;

                case "Title":
                    result = "<label style=\"font-size: .9em\" id=\"" + name + "\" for=\"" + name + "\">" + model.Title + "</label>";
                    break;
                case "MarketPriceLowest":
                    result = "<label for=\"" + name + "\">" + model.LowestPrice + "</label>";
                    break;

                case "MarketPriceHighest":
                    result = "<label for=\"" + name + "\">" + model.HighestPrice + "</label>";
                    break;

                case "CarImage":

                    result = "<img id=\"pIdImage\" name=\"pIdImage\" src=\"" + model.DefaultImageUrl + "\"" + " width=\"160\" height=\"160\"  />" + Environment.NewLine;

                    break;
                case "HiddenPhotoProfile":
                    result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DefaultImageUrl + "\"" + " />"; ;
                    break;
                case "Description":
                    if (!String.IsNullOrEmpty(model.Descriptions))
                    {
                        if (model.Descriptions.Length > 150)
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Descriptions, 150) + "..." + "</label>";

                        else
                            result = "<label id=\"" + name + "\" for=\"" + name + "\">" + CommonHelper.TrimString(model.Descriptions) + "</label>";

                    }
                    else
                    {
                        result = "<label id=\"" + name + "\" for=\"" + name + "\"></label>";
                    }
                    break;
                case "Location":
                    result = "Location:<label id=\"" + name + "\" for=\"" + name + "\">" + model.Location + "</label>";

                    break;
                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }
        
        public static string DynamicHtmlLabelAppraisal(this HtmlHelper<AppraisalListViewModel> htmlHelper, string fieldName)
        {

            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var builder = new StringBuilder();

            bool flag = false;

            switch (fieldName)
            {

                case "txtPendingAppraisalGridMiddle":

                    foreach (var car in model.RecentAppraisals)
                    {
                        if (flag == false)
                        {
                            builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = true;
                        }
                        else
                        {
                            builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = false;

                        }
                        
                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);

                      
                        if(car.IsPhotoFromVingenie)

                            builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", /*"/Appraisal/RenderPhoto?appraisalId=" + car.AppraisalID*/car.PhotoUrl, "", new { AppraisalId = car.AppraisalID }, null, new { width = 47, height = 47 }) + Environment.NewLine);
                        else
                        {
                            builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal",car.DefaultImageUrl, "", new { AppraisalId = car.AppraisalID }, null, new { width = 47, height = 47 }) + Environment.NewLine);
                        }
                        
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("  <div class=\"innerRow1 clear\">" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid noBorder  column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.ModelYear.ToString(), new { AppraisalId = car.AppraisalID }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.Make, new { AppraisalId = car.AppraisalID }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.AppraisalModel, new { AppraisalId = car.AppraisalID }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.StockNumber, new { AppraisalId = car.AppraisalID }));


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\">" + Environment.NewLine);

                        builder.Append(car.AppraisalDate + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div id=\"acvTitle" + car.AppraisalID + "\" class=\"cell shortest column\" style=\"font-size: 1.6em; font-weight:bold;\">" + Environment.NewLine);

                        builder.Append("ACV" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);

                        builder.Append(" <div class=\"cell longer noBorder marketSection column\">" + Environment.NewLine);

                        builder.Append(car.VinNumber + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" <div class=\"cell longer noBorder marketSection column\">" + Environment.NewLine);

                        builder.Append(car.AppraisalBy + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("  <div class=\"cell short column\">" + Environment.NewLine);

                        builder.Append("<a target=\"_blank\"  href=\"" + urlHelper.Content("~/Market/OpenManaheimLoginWindow?Vin=" + car.VinNumber) + "\">MMR</a> / <a  class=\"iframe\"  href=\"" + urlHelper.Content("~/Market/GetKellyBlueBookSummaryAppraisal?AppraisalId=" + car.AppraisalID) + "\">KBB </a> " + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("   <div class=\"cell shortest column\">" + Environment.NewLine);

                        int ACV = 0;

                        bool ACVFlag = Int32.TryParse(car.ACV, out ACV);

                        if (ACVFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.AppraisalID + "\" name=\"Acv\" class=\"sForm\" onblur=\"javascript:updateACV(this);\" value=\"" + ACV.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.AppraisalID + "\" name=\"Acv\" class=\"sForm\" onblur=\"javascript:updateACV(this);\" value=\"" + car.ACV + "\" />" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" <div class=\"clear\">" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);



                    }

                    break;
                case "txtAppraisalGridMiddle":

                    foreach (var car in model.RecentAppraisals)
                    {
                        if (flag == false)
                        {
                            builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = true;
                        }
                        else
                        {
                            builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = false;

                        }

                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                        
                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", car.DefaultImageUrl, "", new { AppraisalId = car.AppraisalID }, null, new { width = 47, height = 47 }) + Environment.NewLine);
                        

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("  <div class=\"innerRow1 clear\">" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid noBorder  column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.ModelYear.ToString(), new { AppraisalId = car.AppraisalID }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.Make, new { AppraisalId = car.AppraisalID }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.AppraisalModel, new { AppraisalId = car.AppraisalID }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewProfileForAppraisal", car.StockNumber, new { AppraisalId = car.AppraisalID }));


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\">" + Environment.NewLine);

                        builder.Append(car.AppraisalDate + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div id=\"acvTitle" + car.AppraisalID + "\" class=\"cell shortest column\" style=\"font-size: 1.6em; font-weight:bold;\">" + Environment.NewLine);

                        builder.Append("ACV" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);

                        builder.Append(" <div class=\"cell longer noBorder marketSection column\">" + Environment.NewLine);

                        builder.Append(car.VinNumber + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        //builder.Append("  <div class=\"cell marketSection column\">" + Environment.NewLine);

                        //builder.Append("<span class=\"blue\"></span>" + Environment.NewLine);

                        //builder.Append(" </div>" + Environment.NewLine);

                        //builder.Append("  <div class=\"cell short marketSection column\">" + Environment.NewLine);

                        //builder.Append("<span class=\"green\"></span>" + Environment.NewLine);

                        //builder.Append(" </div>" + Environment.NewLine);

                        //builder.Append("  <div class=\"cell short marketSection column\">" + Environment.NewLine);

                        //builder.Append("<span class=\"red\"></span>" + Environment.NewLine);

                        //builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" <div class=\"cell longer noBorder marketSection column\">" + Environment.NewLine);

                        builder.Append(car.AppraisalBy + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("  <div class=\"cell short column\">" + Environment.NewLine);

                        builder.Append("<a target=\"_blank\"  href=\"" + urlHelper.Content("~/Market/OpenManaheimLoginWindow?Vin=" + car.VinNumber) + "\">MMR</a> / <a  class=\"iframe\"  href=\"" + urlHelper.Content("~/Market/GetKellyBlueBookSummaryAppraisal?AppraisalId=" + car.AppraisalID) + "\">KBB </a> " + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("   <div class=\"cell shortest column\">" + Environment.NewLine);

                        int ACV = 0;

                        bool ACVFlag = Int32.TryParse(car.ACV, out ACV);

                        if (ACVFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.AppraisalID + "\" name=\"Acv\" class=\"sForm\" onblur=\"javascript:updateACV(this);\" value=\"" + ACV.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.AppraisalID + "\" name=\"Acv\" class=\"sForm\" onblur=\"javascript:updateACV(this);\" value=\"" + car.ACV + "\" />" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" <div class=\"clear\">" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);



                    }

                    break;
                case "txtAppraisalGridSideBar":

                    foreach (AppraisalViewFormModel car in model.UnlimitedAppraisals)
                    {
                        builder.AppendLine("<font size=\"2\">");
                        builder.Append("<li>" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", car.CarImagesUrl, "", new { AppraisalId = car.AppraisalID }, null, new { @class = "mThumb" }) + Environment.NewLine);
                        builder.Append(" <ul class=\"info\">" + Environment.NewLine);
                        builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + CommonHelper.TrimString(car.Make, 10) + "<br />" + Environment.NewLine);
                        builder.Append(car.AppraisalModel + "</span></a></li>" + Environment.NewLine);
                        builder.Append("<li><span class=\"value\">" + car.ACV + "</span> <span class=\"date\">" + car.AppraisalDate + "</span></li>" + Environment.NewLine);
                        builder.Append("</ul>" + Environment.NewLine);
                        builder.Append("</li>" + Environment.NewLine);
                        builder.Append("</font>" + Environment.NewLine);

                    }
                    break;

                case "txtPendingAppraisalGridSideBar":

                    foreach (AppraisalViewFormModel car in model.UnlimitedAppraisals)
                    {
                        builder.AppendLine("<font size=\"2\">");
                        builder.Append("<li>" + Environment.NewLine);

                        if (car.IsPhotoFromVingenie)

                            builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", /*"/Appraisal/RenderPhoto?appraisalId=" + car.AppraisalID*/car.PhotoUrl, "", new { AppraisalId = car.AppraisalID }, null, new { @class = "mThumb" }) + Environment.NewLine);
                        else
                        {
                            builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewProfileForAppraisal", car.DefaultImageUrl, "", new { AppraisalId = car.AppraisalID }, null, new { @class = "mThumb" }) + Environment.NewLine);
                        }
                        builder.Append(" <ul class=\"info\">" + Environment.NewLine);
                        builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + CommonHelper.TrimString(car.Make, 10) + "<br />" + Environment.NewLine);
                        builder.Append(car.AppraisalModel + "</span></a></li>" + Environment.NewLine);
                        builder.Append("<li><span class=\"value\">" + car.ACV + "</span> <span class=\"date\">" + car.AppraisalDate + "</span></li>" + Environment.NewLine);
                        builder.Append("</ul>" + Environment.NewLine);
                        builder.Append("</li>" + Environment.NewLine);
                        builder.Append("</font>" + Environment.NewLine);

                    }
                    break;

                case "SortOption":
                    builder.Append("<select name=\"sortBy\" id=\"sortBy\" onchange=\"javascript:submitFormSort(this);\" > " + Environment.NewLine);

                    builder.Append("<option> Select an option...</option>" + Environment.NewLine);

                    builder.Append("<option>Year</option>" + Environment.NewLine);

                    builder.Append("<option>Make</option>" + Environment.NewLine);

                    builder.Append("<option>Model</option>" + Environment.NewLine);

                    builder.Append("<option>Age</option>" + Environment.NewLine);

                    builder.Append("</select>" + Environment.NewLine);
                    break;


                default:
                    builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" value=\"error\"" + " />");

                    break;



            }



            return builder.ToString();
        }

        public static string DynamicHtmlLabelForKPI(this HtmlHelper<InventoryFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            string result = "";

            var carsList = model.CarsList;

            var queryAbove = carsList.Where(tmp => tmp.MarketRange == 3);

            var queryAve = carsList.Where(tmp => tmp.MarketRange == 2);

            var queryBelow = carsList.Where(tmp => tmp.MarketRange == 1);

            //var queryUnknown = carsList.Where(tmp => tmp.MarketRange == 0  );


            var queryHasPic = carsList.Where(tmp => tmp.HasImage);


            var queryHasDescriptions = carsList.Where(tmp => tmp.HasDescription);


            var queryHasSalePrice = carsList.Where(tmp => tmp.HasSalePrice);



            if (fieldName.Equals("AboveMarket"))
            {
                result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=9") + "\">" + queryAbove.Count() + "</a></td>";


            }
            else if (fieldName.Equals("AverageMarket"))
            {
                result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=10") + "\">" + queryAve.Count() + "</a></td>";
            }
            else if (fieldName.Equals("BelowMarket"))
            {
                result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=11") + "\">" + queryBelow.Count() + "</a></td>";
            }
            else if (fieldName.Equals("PercentAboveMarket"))
            {
                result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=9") + "\">" + GetPercentageString((double)queryAbove.Count() / carsList.Count) + "</a>";

            }
            else if (fieldName.Equals("PercentAverageMarket"))
            {
                result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=10") + "\">" + GetPercentageString((double)queryAve.Count() / carsList.Count) + "</a>";

            }
            else if (fieldName.Equals("PercentBelowMarket"))
            {
                double lastPercentage = 1 - (double)queryAbove.Count() / carsList.Count - (double)queryAve.Count() / carsList.Count;

                result = "<a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=11") + "\">" + GetPercentageString(lastPercentage) + "</a>";

            }


            else if (fieldName.Equals("PercentPics"))
            {
                double lastPercentage = (double)queryHasPic.Count() / carsList.Count;

                if (lastPercentage >= 0 && lastPercentage <= 0.33)
                    result = "<td class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                    result = "<td class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else
                    result = "<td class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
            }

            else if (fieldName.Equals("PercentDescriptions"))
            {
                double lastPercentage = (double)queryHasDescriptions.Count() / carsList.Count;

                if (lastPercentage >= 0 && lastPercentage <= 0.33)
                    result = "<td class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                    result = "<td class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else
                    result = "<td class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
            }

            else if (fieldName.Equals("PercentSalePrice"))
            {
                double lastPercentage = (double)queryHasSalePrice.Count() / carsList.Count;

                if (lastPercentage >= 0 && lastPercentage <= 0.33)
                    result = "<td class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                    result = "<td class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else
                    result = "<td class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
            }

            else if (fieldName.Equals("HiddenInventoryGauge"))
            {
                double percent = Math.Ceiling((((queryAbove.Count() + queryBelow.Count()) * .5 + queryAve.Count()) / carsList.Count) * 100);

                result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            }

            else if (fieldName.Equals("HiddenContentGauge"))
            {
                double percent = Math.Ceiling(((queryHasPic.Count() + (double)queryHasDescriptions.Count() + queryHasSalePrice.Count()) * 100) / (3 * carsList.Count));

                result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            }

            else if (fieldName.Equals("0-15InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

                result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=1") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("16-30InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

                result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=2") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("31-60InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

                result = "<a  class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=3") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("61-90InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

                result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=4") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("90OverInInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

                result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=5") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("Percent0-15InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

                result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=1") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("Percent16-30InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

                result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=2") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("Percent31-60InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

                result = "<a class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=3") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("Percent61-90InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

                result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=4") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("Percent90OverInInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

                result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewKPIForCarsWithCondition?Condition=5") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }



            else if (fieldName.Equals("NPercentPics"))
            {
                double lastPercentage = (double)queryHasPic.Count() / carsList.Count;

                if (lastPercentage >= 0 && lastPercentage <= 0.33)
                    result = "<td class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                    result = "<td class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else
                    result = "<td class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=6") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
            }

            else if (fieldName.Equals("NPercentDescriptions"))
            {
                double lastPercentage = (double)queryHasDescriptions.Count() / carsList.Count;

                if (lastPercentage >= 0 && lastPercentage <= 0.33)
                    result = "<td class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                    result = "<td class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else
                    result = "<td class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=7") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
            }

            else if (fieldName.Equals("NPercentSalePrice"))
            {
                double lastPercentage = (double)queryHasSalePrice.Count() / carsList.Count;

                if (lastPercentage >= 0 && lastPercentage <= 0.33)
                    result = "<td class=\"low\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else if (lastPercentage >= 0.34 && lastPercentage <= 0.66)
                    result = "<td class=\"mid\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
                else
                    result = "<td class=\"high\"><a href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=8") + "\">" + GetPercentageString(lastPercentage) + "</a></td>";
            }

            else if (fieldName.Equals("NHiddenInventoryGauge"))
            {
                double percent = Math.Ceiling((((queryAbove.Count() + queryBelow.Count()) * .5 + queryAve.Count()) / carsList.Count) * 100);

                result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            }

            else if (fieldName.Equals("NHiddenContentGauge"))
            {
                double percent = Math.Ceiling(((queryHasPic.Count() + (double)queryHasDescriptions.Count() + queryHasSalePrice.Count()) * 100) / (3 * carsList.Count));

                result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=" + percent + " />";
            }

            else if (fieldName.Equals("N0-15InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

                result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=1") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("N16-30InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

                result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=2") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("N31-60InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

                result = "<a  class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=3") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("N61-90InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

                result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=4") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("N90OverInInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

                result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=5") + "\">" + queryInInventory.ToList().Count + "</a>";
            }
            else if (fieldName.Equals("NPercent0-15InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry <= 15);

                result = "<a class=\"greenL\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=1") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("NPercent16-30InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 15 && tmp.DaysInInvenotry <= 30);

                result = "<a class=\"greenD\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=2") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("NPercent31-60InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 30 && tmp.DaysInInvenotry <= 60);

                result = "<a class=\"blue\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=3") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("NPercent61-90InInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 60 && tmp.DaysInInvenotry <= 90);

                result = "<a class=\"orange\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=4") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("NPercent90OverInInventory"))
            {
                IEnumerable<CarInfoFormViewModel> queryInInventory = carsList.Where(tmp => tmp.DaysInInvenotry > 90);

                result = "<a class=\"red\" href=\"" + urlHelper.Content("~/Market/ViewNewKPIForCarsWithCondition?Condition=5") + "\">" + GetPercentageString((double)queryInInventory.ToList().Count / carsList.Count) + "</a>"; ;
            }
            else if (fieldName.Equals("KPISideBar"))
            {

                var builder = new StringBuilder();
                builder.AppendLine("<font size=\"1\">");
                foreach (CarInfoFormViewModel car in model.SubSetList)
                {
                    builder.Append("<li>" + Environment.NewLine);
                    builder.Append(ImageLinkHelper.ImageLinkToInventoryFromMarket(htmlHelper, "ViewIProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { @class = "mThumb", width = "97px", height = "97px" }) + Environment.NewLine);

                    builder.Append("<ul class=\"info\">" + Environment.NewLine);
                    builder.Append("<li><span class=\"item_title\">" + car.ModelYear + " " + car.Make + "<br />" + Environment.NewLine);
                    builder.Append(car.Model + "</span></a></li>" + Environment.NewLine);
                    builder.Append("<li><span class=\"value\">" + "$" + car.SalePrice + "</span> <span class=\"date\">" + car.DateInStock.Value.ToShortDateString() + "</span></li>" + Environment.NewLine);
                    builder.Append("</ul>" + Environment.NewLine);
                    builder.Append("</li>" + Environment.NewLine);

                }
                builder.AppendLine("</font>");
                result = builder.ToString();
            }
            else
            {
                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + " value=\"error\"" + " />";
            }


            return result;
        }

        public static string DynamicHtmlLabelForInventory(this HtmlHelper<InventoryFormViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var builder = new StringBuilder();

            bool flag = true;


            switch (fieldName)
            {
                case "Numeric":
                    builder.Append("<script type=\"text/javascript\" language=\"javascript\">" + Environment.NewLine);
                    foreach (CarInfoFormViewModel car in model.CarsList.Where(p => p.IsSold == false))
                    {
                        builder.Append("$(\"#" + car.ListingId + "\").numeric({ decimal: false, negative: false }, function() { alert(\"Positive integers only\"); this.value = \"\"; this.focus(); }" + Environment.NewLine);
                        builder.Append("$(\"#" + car.ListingId + "\").numeric({ decimal: false, negative: false }, function() { alert(\"Positive integers only\"); this.value = \"\"; this.focus(); }" + Environment.NewLine);
                    }
                    builder.Append("</script>");
                    break;
                case "ExpressBucketJump":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {
                        if (flag)
                        {
                            builder.Append("<div id=\"div_" + car.ListingId + "\" class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            builder.Append("<div id=\"div_" + car.ListingId + "\" class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;
                        }

                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                        builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewIProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { width = 640, height = 480 }) + Environment.NewLine);
                        builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);
                        builder.Append("  </div>  " + Environment.NewLine);
                        builder.Append("  </div>  " + Environment.NewLine);
                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);
                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px\" >" + Environment.NewLine);
                        //if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                        //    builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" style=\"position: absolute;left: 2px\" value=\"True\" checked />" + Environment.NewLine);
                        //else
                        //    builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" style=\"position: absolute;left: 2px\" value=\"False\" />" + Environment.NewLine);

                        if (car.MarketRange == 3)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(CultureInfo.InvariantCulture), new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        if (car.NotDoneBucketJump)
                            builder.Append("<a id=\"doneTodayBucketJump_" + car.ListingId + "_" + car.DaysInInvenotry + "\" href=\"javascript:;\" style=\"background-color:#860000;padding: 2px 12px;\">Done</a>");
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                        builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.Vin))
                        {
                            if (car.Vin.Length > 7)
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { ListingID = car.ListingId }));
                            }
                            else
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", "VIN " + car.Vin, new { ListingID = car.ListingId }));
                        }

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                     
                        builder.Append(car.CarRanking + "-" + car.NumberOfCar + " Market" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                    
                        builder.Append(car.MarketLowestPrice + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        // set market ranking
                        builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                      
                        builder.Append(car.MarketAveragePrice + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        // set market ranking

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                      
                        builder.Append(car.MarketHighestPrice + Environment.NewLine);
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        
    
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        int odometerNumber = 0;
                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        int salePriceNumber = 0;
                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;

                case "InventoryGrid":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }


                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);

                        builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                            {
                                if (car.IsTruck)
                                    builder.Append(
                                        ImageLinkHelper.ImageLink(htmlHelper, "EditIProfileForTruck", car.SinglePhoto,
                                                                  "",
                                                                  new {ListingID = car.ListingId}, null,
                                                                  new {width = 640, height = 480}) + Environment.NewLine);

                                else
                                {
                                    builder.Append(
                                        ImageLinkHelper.ImageLink(htmlHelper, "EditIProfile", car.SinglePhoto, "",
                                                                  new {ListingID = car.ListingId}, null,
                                                                  new {width = 640, height = 480}) + Environment.NewLine);
                                }
                            }
                            else
                            {
                                builder.Append(
                                        ImageLinkHelper.ImageLink(htmlHelper, "ViewIProfile", car.SinglePhoto, "",
                                                                  new { ListingID = car.ListingId }, null,
                                                                  new { width = 640, height = 480 }) + Environment.NewLine);
                                
                                //builder.Append("<img src=\"" + car.SinglePhoto + "\" style=\"width:640px;height:480px;\" />" + Environment.NewLine);
                            }

                        }
                        else
                        {
                            builder.Append(
                                ImageLinkHelper.ImageLink(htmlHelper, "ViewISoldProfile", car.SinglePhoto, "",
                                                          new {ListingID = car.ListingId}, null,
                                                          new {width = 640, height = 480}) + Environment.NewLine);
                            
                        }

                        builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);

                        builder.Append("  </div>  " + Environment.NewLine);

                        builder.Append("  </div>  " + Environment.NewLine);

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px\" >" + Environment.NewLine);
                        if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" style=\"position: absolute;left: 2px\" value=\"True\" checked />" + Environment.NewLine);
                        else
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" style=\"position: absolute;left: 2px\" value=\"False\" />" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            
                                if (car.MarketRange == 3)
                                    builder.Append("<a href=\"" +
                                                   urlHelper.Action("ViewIProfile", new {ListingID = car.ListingId}) +
                                                   "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") +
                                                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                                   Environment.NewLine);
                                else if (car.MarketRange == 2)
                                    builder.Append("<a href=\"" +
                                                   urlHelper.Action("ViewIProfile", new {ListingID = car.ListingId}) +
                                                   "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") +
                                                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                                   Environment.NewLine);
                                else if (car.MarketRange == 1)
                                    builder.Append("<a href=\"" +
                                                   urlHelper.Action("ViewIProfile", new {ListingID = car.ListingId}) +
                                                   "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") +
                                                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                                   Environment.NewLine);
                                else
                                    builder.Append("<a href=\"" +
                                                   urlHelper.Action("ViewIProfile", new {ListingID = car.ListingId}) +
                                                   "\"><img src=\"" + urlHelper.Content("~/images/question.gif") +
                                                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                                   Environment.NewLine);
                            
                            
                        }
                        else
                        {
                            
                                //if (car.MarketRange == 3)
                                //    builder.Append("<a href=\"" +
                                //                   urlHelper.Action("ViewISoldProfile", new {ListingID = car.ListingId}) +
                                //                   "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") +
                                //                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                //                   Environment.NewLine);
                                //else if (car.MarketRange == 2)
                                //    builder.Append("<a href=\"" +
                                //                   urlHelper.Action("ViewISoldProfile", new {ListingID = car.ListingId}) +
                                //                   "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") +
                                //                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                //                   Environment.NewLine);
                                //else if (car.MarketRange == 1)
                                //    builder.Append("<a href=\"" +
                                //                   urlHelper.Action("ViewISoldProfile", new {ListingID = car.ListingId}) +
                                //                   "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") +
                                //                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                //                   Environment.NewLine);
                                //else
                                //    builder.Append("<a href=\"" +
                                //                   urlHelper.Action("ViewISoldProfile", new {ListingID = car.ListingId}) +
                                //                   "\"><img src=\"" + urlHelper.Content("~/images/question.gif") +
                                //                   "\" style=\"height: 20px; width: 15px;\" /></a>" +
                                //                   Environment.NewLine);
                            
                            
                        }

                        builder.Append(" </div>" + Environment.NewLine);
                      
                        // add IsFeatured? checkbox

                        //builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);

                        //builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile",
                                                                              car.ModelYear.ToString(),
                                                                              new { ListingID = car.ListingId }));
                            else
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile",
                                                                              car.ModelYear.ToString(),
                                                                              new { ListingID = car.ListingId }));
                                //builder.Append(car.ModelYear.ToString() + Environment.NewLine);
                            }
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.Make, new {ListingID = car.ListingId}));
                            else
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                //builder.Append(car.Make + Environment.NewLine);
                            }
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make, new { ListingID = car.ListingId }));
                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile",
                                                                              CommonHelper.TrimString(car.Model, 12),
                                                                              new { ListingID = car.ListingId }));
                            else
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile",
                                                                              CommonHelper.TrimString(car.Model, 12),
                                                                              new { ListingID = car.ListingId }));
                                //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                            }
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                            {
                                if (car.IsTruck)
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck",
                                                                                  CommonHelper.TrimString(car.Trim, 12),
                                                                                  new {ListingID = car.ListingId}));
                                else
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile",
                                                                                  CommonHelper.TrimString(car.Trim, 12),
                                                                                  new {ListingID = car.ListingId}));
                            }
                            else
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile",
                                                                                  CommonHelper.TrimString(car.Trim, 12),
                                                                                  new { ListingID = car.ListingId }));
                                //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                            }
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                            {
                                if (car.IsTruck)
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck",
                                                                                  car.StockNumber,
                                                                                  new {ListingID = car.ListingId}));
                                else
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile",
                                                                                  car.StockNumber,
                                                                                  new {ListingID = car.ListingId}));
                            }
                            else
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile",
                                                                                  car.StockNumber,
                                                                                  new { ListingID = car.ListingId }));
                                //builder.Append(car.StockNumber + Environment.NewLine);   
                            }
                        }

                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Trim, 12),
                                                                          new {ListingID = car.ListingId}));
                        }
                        
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(car.DaysInInvenotry + " Days");

                        builder.Append(" </div>" + Environment.NewLine);
                        
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int odometerNumber = 0;

                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int salePriceNumber = 0;

                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);
                        
                        builder.Append("</div>" + Environment.NewLine);
                        
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                        
                        builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.Vin))
                        {
                            if (model.CurrentOrSoldInventory)
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                {
                                    if (car.IsTruck)
                                    {
                                        if (car.Vin.Length > 7)
                                        {
                                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper,
                                                                                          "EditIProfileForTruck",
                                                                                          "VIN " +
                                                                                          car.Vin.Substring(
                                                                                              car.Vin.Length - 7),
                                                                                          new
                                                                                              {
                                                                                                  ListingID = car.ListingId
                                                                                              }));

                                        }
                                        else
                                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper,
                                                                                          "EditIProfileForTruck",
                                                                                          "VIN " + car.Vin,
                                                                                          new
                                                                                              {
                                                                                                  ListingID = car.ListingId
                                                                                              }));
                                    }
                                    else
                                    {
                                        if (car.Vin.Length > 7)
                                        {
                                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile",
                                                                                          "VIN " +
                                                                                          car.Vin.Substring(
                                                                                              car.Vin.Length - 7),
                                                                                          new
                                                                                              {
                                                                                                  ListingID = car.ListingId
                                                                                              }));

                                        }
                                        else
                                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile",
                                                                                          "VIN " + car.Vin,
                                                                                          new
                                                                                              {
                                                                                                  ListingID = car.ListingId
                                                                                              }));
                                    }
                                }
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile",
                                                                                          "VIN " + car.Vin,
                                                                                          new
                                                                                          {
                                                                                              ListingID = car.ListingId
                                                                                          }));
                                    //builder.Append(car.Vin.Length > 7 ? car.Vin.Substring(car.Vin.Length - 7) : car.Vin + Environment.NewLine);
                                }

                            }
                            else
                            {
                                if (car.Vin.Length > 7)
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                                  "VIN " +
                                                                                  car.Vin.Substring(car.Vin.Length - 7),
                                                                                  new { ListingID = car.ListingId }));

                                }
                                else
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                                  "VIN " + car.Vin,
                                                                                  new { ListingID = car.ListingId }));
                            }
                        }
                        
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        
                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                            builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.InteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        // set market ranking
                        builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                        builder.Append(car.CarRanking + "-" + car.NumberOfCar + " Market" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        // set market ranking

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                        //if (car.Reconstatus)
                        //    builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);
                        //else
                        //    builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);

                        if(car.Loaner)
                            builder.Append("Loaner" + Environment.NewLine);
                        else if (car.Auction)
                            builder.Append("Auction" + Environment.NewLine);
                        else
                        {
                            if (car.Reconstatus)
                                builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);
                            else
                                builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);

                        }

                        //builder.Append(car.NumberofCarOnEbay + " Ebay" + Environment.NewLine);
                        //builder.Append(0 + " Ebay" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        
                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        builder.Append("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Market/ViewEbay?ListingId=" + car.ListingId) + "\">Ebay</a>" + Environment.NewLine);
                        //builder.Append("<a href=\"\">EB</a>/<a href=\"https://post.craigslist.org/c/orc?lang=en\" onclick=\"window.open('https://post.craigslist.org/c/orc?lang=en')\">CL</a>" + Environment.NewLine);
                        //<a href="/VinControl/Inventory/ViewInventory">Inventory</a>
                        //builder.Append("<a href=\"\">EB</a>/<a href=\"/VinControl/Market/ViewCraigslist\">CL</a>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        //builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" + urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) + "\">BG</a>/<a target=\"_blank\" href=\"" + urlHelper.Content("~/Report/ViewSimpleSticker?ListingId=" + car.ListingId) + "\">WS</a>" + Environment.NewLine);

                        //builder.Append("<input class=\"pad_tab\" type=\"button\" name=\"ws\" value=\"WS\" onclick=\"javascript:openWindowSticker(" + model.ListingId + ")\">" + Environment.NewLine);
                        
                        builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" + urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) + "\">BG</a>/<a title=\"Window Sticker\" target=\"_blank\" onclick=\"javascript:openWindowSticker(" + car.ListingId + ")" + "\">WS</a>" + Environment.NewLine);
                        
                        //builder.Append("<a href=\"/Report/ViewBuyerGuide?ListingId=" + car.ListingId + "\">BG</a>/<a href=\"\">WS</a>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;

                case "SmallInventoryGrid":
                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {
                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;
                        }

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px;\" >" + Environment.NewLine);
                        if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"True\" checked />" + Environment.NewLine);
                        else
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"False\" />" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        // add IsFeatured? checkbox

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);
                        
                        if (car.MarketRange == 3)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else  if (car.MarketRange == 1)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        
                        builder.Append(" </div>" + Environment.NewLine);
                        
                        builder.Append("<div class=\"cell noBorder shorter column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (car.IsTruck)
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.StockNumber, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                                    //builder.Append(car.StockNumber + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                    //builder.Append(car.ModelYear + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.Make, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                    //builder.Append(car.Make + Environment.NewLine);
                                }
                                
                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);
                            }
                            else
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                                    //builder.Append(car.StockNumber + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                    //builder.Append(car.ModelYear + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.Make, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                    //builder.Append(car.Make + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);
                            }

                         
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.StockNumber, new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          car.ModelYear.ToString(),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make,
                                                                          new { ListingID = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Model, 12),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Trim, 12),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);
                        }

                        builder.Append("<div class=\"short cell column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\" title=\"Number of Owners (Based on Carfax)\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"Days in Inventory\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int odometerNumber = 0;

                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter wide column\">" + Environment.NewLine);

                        int salePriceNumber = 0;

                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\"  onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        //builder.Append("</div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;
                case "SmallReconInventoryGrid":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px;\" >" + Environment.NewLine);
                        if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"True\" checked />" + Environment.NewLine);
                        else
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"False\" />" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        // add IsFeatured? checkbox

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        if (car.MarketRange == 3)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action("ViewIProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell noBorder shorter column\">" + Environment.NewLine);

                        if (model.CurrentOrSoldInventory)
                        {
                            if (car.IsTruck)
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.StockNumber, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                                    //builder.Append(car.StockNumber + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                    //builder.Append(car.ModelYear + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", car.Make, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                    //builder.Append(car.Make + Environment.NewLine);
                                }
                                
                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfileForTruck", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);
                            }
                            else
                            {
                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.StockNumber, new { ListingID = car.ListingId }));
                                    //builder.Append(car.StockNumber + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));
                                    //builder.Append(car.ModelYear + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", car.Make, new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", car.Make, new { ListingID = car.ListingId }));
                                    //builder.Append(car.Make + Environment.NewLine);
                                }
                                
                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Model, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);

                                builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                                if (CanSeeButton(Constanst.ProfileButton.EditProfile))
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "EditIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                else
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));
                                    //builder.Append(CommonHelper.TrimString(car.Trim, 12) + Environment.NewLine);
                                }

                                builder.Append(" </div>" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.StockNumber,
                                                                        new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          car.ModelYear.ToString(),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make,
                                                                          new { ListingID = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Model, 12),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);

                            builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                            builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile",
                                                                          CommonHelper.TrimString(car.Trim, 12),
                                                                          new { ListingID = car.ListingId }));

                            builder.Append(" </div>" + Environment.NewLine);
                        }

                        builder.Append("<div class=\"short cell column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        //builder.Append("<div class=\"cell short column\" title=\"Number of Owners (Based on Carfax)\">" + Environment.NewLine);
                        //if (car.CarFaxOwner == -1)
                        //    builder.Append("Unknown" + Environment.NewLine);
                        //else if (car.CarFaxOwner == 1)

                        //    builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        //else
                        //{
                        //    builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        //}

                        //builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        if (car.Reconstatus)
                            builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"Days in Inventory\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int odometerNumber = 0;

                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter wide column\">" + Environment.NewLine);

                        int salePriceNumber = 0;

                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\"  onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);

                    

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;
                case "SmallSoldInventoryGrid":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        // add IsFeatured? checkbox
                        builder.Append("<div class=\"cell evenShorter column\" style=\"padding-right: 5px; margin-left:-5px;\" >" + Environment.NewLine);
                        if (car.IsFeatured.HasValue && car.IsFeatured.Value)
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"True\" checked />" + Environment.NewLine);
                        else
                            builder.Append(" <input type=\"checkbox\" title=\"Is featured car?\" name=\"IsFeatured_" + car.ListingId + "\" id=\"IsFeatured_" + car.ListingId + "\" value=\"False\" />" + Environment.NewLine);
                        builder.Append(" </div>" + Environment.NewLine);
                        // add IsFeatured? checkbox
                        
                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        //if (car.MarketRange == 3)
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        //else if (car.MarketRange == 2)
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        //else if (car.MarketRange == 1)
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        //else
                        //    builder.Append("<a href=\"" + urlHelper.Action("ViewISoldProfile", new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell noBorder shorter column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.StockNumber, new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"short cell column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\" title=\"Number of Owners (Based on Carfax)\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"Days in Inventory\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int odometerNumber = 0;

                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter wide column\">" + Environment.NewLine);

                        int salePriceNumber = 0;

                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\"  onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        //builder.Append("</div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);





                    }




                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;

                case "SmallWholeSaleInventoryGrid":

                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList)
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }

                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        if (car.MarketRange == 3)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell noBorder shorter column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", car.StockNumber, new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", car.Make, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"mid cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"short cell column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short column\" title=\"Number of Owners (Based on Carfax)\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"Days in Inventory\">" + Environment.NewLine);
                        builder.Append(car.DaysInInvenotry + " Days");
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int odometerNumber = 0;

                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\"  value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter wide column\">" + Environment.NewLine);

                        int salePriceNumber = 0;

                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\"  onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        //builder.Append("</div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;
                case "InventoryWholesaleGrid":
                    builder.Append("<div id=\"inven\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (CarInfoFormViewModel car in model.CarsList.Where(p => p.IsSold == false))
                    {

                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }


                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);

                        builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewIWholesaleProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { width = 640, height = 480 }) + Environment.NewLine);



                        builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);


                        builder.Append("  </div>  " + Environment.NewLine);

                        builder.Append("  </div>  " + Environment.NewLine);


                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        if (car.MarketRange == 3)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", car.Make, new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", car.StockNumber, new { ListingID = car.ListingId }));
                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(car.DaysInInvenotry + " Days");

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int odometerNumber = 0;

                        bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                        if (odometerFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" />" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        int salePriceNumber = 0;

                        bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                        if (salePriceFlag)

                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" />" + Environment.NewLine);

                        else
                            builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);


                        builder.Append("</div>" + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);



                        builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                        if (!String.IsNullOrEmpty(car.Vin))
                        {
                            if (car.Vin.Length > 7)
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { ListingID = car.ListingId }));

                            }
                            else
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewIWholesaleProfile", "VIN " + car.Vin, new { ListingID = car.ListingId }));

                        }


                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                            builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.InteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                        builder.Append(0 + " Market" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                        if (car.Reconstatus)
                            builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);
                        else
                            builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);
                        //builder.Append(car.NumberofCarOnEbay + " Ebay" + Environment.NewLine);
                        //builder.Append(0 + " Ebay" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        builder.Append("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Market/ViewEbay?ListingId=" + car.ListingId) + "\">Ebay</a>" + Environment.NewLine);
                        //builder.Append("<a href=\"\">EB</a>/<a href=\"https://post.craigslist.org/c/orc?lang=en\" onclick=\"window.open('https://post.craigslist.org/c/orc?lang=en')\">CL</a>" + Environment.NewLine);
                        //<a href="/VinControl/Inventory/ViewInventory">Inventory</a>
                        //builder.Append("<a href=\"\">EB</a>/<a href=\"/VinControl/Market/ViewCraigslist\">CL</a>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                        //builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" + urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) + "\">BG</a>/<a target=\"_blank\" href=\"" + urlHelper.Content("~/Report/ViewSimpleSticker?ListingId=" + car.ListingId) + "\">WS</a>" + Environment.NewLine);

                        //builder.Append("<input class=\"pad_tab\" type=\"button\" name=\"ws\" value=\"WS\" onclick=\"javascript:openWindowSticker(" + model.ListingId + ")\">" + Environment.NewLine);



                        builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" + urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) + "\">BG</a>/<a title=\"Window Sticker\" target=\"_blank\" onclick=\"javascript:openWindowSticker(" + car.ListingId + ")" + "\">WS</a>" + Environment.NewLine);


                        //builder.Append("<a href=\"/Report/ViewBuyerGuide?ListingId=" + car.ListingId + "\">BG</a>/<a href=\"\">WS</a>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                    }

                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    break;
                case "InventorySoldGrid":

                    builder.Append("<div id=\"sold\">" + Environment.NewLine);
                    builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                    builder.Append("<div id=\"table\" >" + Environment.NewLine);
                    foreach (var car in model.CarsList.Where(p => p.IsSold == true))
                    {
                        //string idPrice = car.Vin + "price";

                        //string idMilage = car.Vin + "milage";



                        if (flag)
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter dark acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            if (car.ACar) builder.Append("<div class=\"rowOuter light acar\">" + Environment.NewLine);
                            else builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                            flag = true;

                        }


                        builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);




                        builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);

                        builder.Append("<div class=\"text\">" + Environment.NewLine);

                        builder.Append("<h2>SOLD</h2>" + Environment.NewLine);


                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.ImageLink(htmlHelper, "ViewISoldProfile", car.SinglePhoto, "", new { ListingID = car.ListingId }, null, new { width = 640, height = 480 }) + Environment.NewLine);


                        builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);



                        builder.Append("  </div>  " + Environment.NewLine);

                        builder.Append("  </div>  " + Environment.NewLine);


                        builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);

                        builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);


                        if (car.MarketRange == 3)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 2)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else if (car.MarketRange == 1)
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        else
                            builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.ModelYear.ToString(), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.Make, new { ListingID = car.ListingId }));


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Model, 12), new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", CommonHelper.TrimString(car.Trim, 12), new { ListingID = car.ListingId }));


                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", car.StockNumber, new { ListingID = car.ListingId }));

                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);

                        builder.Append(car.DaysInInvenotry + " Days Old");

                        builder.Append(" </div>" + Environment.NewLine);



                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" />" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);



                        builder.Append(" </div>" + Environment.NewLine);

                        builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);



                        builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.Vin))
                        {
                            if (car.Vin.Length > 7)
                            {
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { ListingID = car.ListingId }));

                            }
                            else
                                builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "ViewISoldProfile", "VIN " + car.Vin, new { ListingID = car.ListingId }));

                        }
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                        if (car.ExteriorColor.Length > 13)
                            builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.ExteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                        if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                            builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                        else
                            builder.Append(car.InteriorColor + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                        builder.Append(0 + " Market" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        //builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        ////builder.Append(car.NumberofCarOnCarsCom + " Cars.com" + Environment.NewLine);
                        //builder.Append(Environment.NewLine);
                        ////builder.Append(0 + " Cars.com" + Environment.NewLine);
                        //builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                        if (car.CarFaxOwner == -1)
                            builder.Append("Unknown" + Environment.NewLine);
                        else if (car.CarFaxOwner == 1)

                            builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                        else
                        {
                            builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                        }
                        builder.Append(Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                        //builder.Append(car.NumberofCarOnEbay + " Ebay" + Environment.NewLine);
                        builder.Append(Environment.NewLine);
                        //builder.Append(0 + " Ebay" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);


                        builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                        //builder.Append("<a href=\"\">EB</a>/<a href=\"https://post.craigslist.org/c/orc?lang=en\" onclick=\"window.open('https://post.craigslist.org/c/orc?lang=en')\">CL</a>" + Environment.NewLine);

                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, "MarkUnsold", "Unsold", new { ListingID = car.ListingId }));

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("<div class=\"cell shorter column\" title=\"" + car.SoldOutDaysLeft + " Days Left " + " \">" + Environment.NewLine);

                        builder.Append(car.SoldOutDaysLeft + " Days");

                        builder.Append("</div>" + Environment.NewLine);

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);



                    }
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);
                    builder.Append("</div>" + Environment.NewLine);

                    break;
                case "SortOption":
                    builder.Append("<select name=\"sortBy\" id=\"sortBy\" onchange=\"this.form.submit();\" > " + Environment.NewLine);

                    builder.Append("<option> Select an option...</option>" + Environment.NewLine);

                    builder.Append("<option>Year</option>" + Environment.NewLine);

                    builder.Append("<option>Make</option>" + Environment.NewLine);

                    builder.Append("<option>Model</option>" + Environment.NewLine);


                    builder.Append("<option>Price</option>" + Environment.NewLine);


                    builder.Append("<option>Color</option>" + Environment.NewLine);

                    builder.Append("<option>Age</option>" + Environment.NewLine);

                    builder.Append("</select>" + Environment.NewLine);
                    break;

                default:
                    builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" value=\"error\"" + " />");

                    break;
            }




            return builder.ToString();
        }

        private static void DynamicHtmlLabelForAdvancedSearchByCategory(HtmlHelper<List<CarInfoFormViewModel>> htmlHelper, UrlHelper urlHelper, StringBuilder builder, List<CarInfoFormViewModel> model, bool flag, string name)
        {
            builder.Append("<div id=\"table\" class=\"" + name + "\" >" + Environment.NewLine);
            builder.Append("<div style=\"width: 100%; height: 20px; overflow: hidden; border-bottom: 4px solid #DDD; font-size:16px\">" + name + "</div>");

            foreach (var car in model)
            {
                var controllerName = "Inventory";
                var actionName = string.Empty;
                var actionForViewOnly = string.Empty;
                var paramName = string.Empty;

                switch (car.Type)
                {
                    case 2:
                        actionForViewOnly = "ViewIWholesaleProfile";
                        actionName = "ViewIWholesaleProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                        controllerName = "Wholesale";
                        break;
                    case 3:
                        actionForViewOnly = "ViewProfileForAppraisal";
                        actionName = car.IsTruck ? "EditAppraisalForTruck" : "EditAppraisal";
                        controllerName = "Appraisal";
                        break;
                    case 4:
                        actionForViewOnly = "ViewISoldProfile";
                        actionName = "ViewISoldProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                        break;
                    default:
                        actionForViewOnly = "ViewIProfile";
                        actionName = car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                        break;
                }


                if (flag)
                {
                    builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                    flag = false;
                }
                else
                {
                    builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                    flag = true;
                }

                builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                builder.Append(" <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" + Environment.NewLine);
                if (controllerName != "Appraisal")
                    builder.Append(ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto, "", new { car.ListingId }, null, new { width = 50, height = 50 }) + Environment.NewLine);
                else
                    builder.Append(ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto, "", new { AppraisalId = car.ListingId }, null, new { width = 50, height = 50 }) + Environment.NewLine);
                builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" + car.HealthLevel + "\" />" + Environment.NewLine);
                builder.Append("  </div>  " + Environment.NewLine);
                builder.Append("  </div>  " + Environment.NewLine);
                builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);
                builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);
                builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);

                switch (car.MarketRange)
                {
                    case 3:
                        //builder.Append("<img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        if (controllerName != "Appraisal")
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        break;
                    case 2:
                        //builder.Append("<img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        if (controllerName != "Appraisal")
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        break;
                    case 1:
                        //builder.Append("<img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        if (controllerName != "Appraisal")
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        else
                            builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                        break;
                    default:
                        builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                        break;
                }

                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.ModelYear.ToString(), new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.ModelYear.ToString(), new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.Make, new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.Make, new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Model, 12),
                                                                    new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Model, 12),
                                                                    new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Trim, 12),
                                                                    new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    CommonHelper.TrimString(car.Trim, 12),
                                                                    new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                builder.Append(controllerName != "Appraisal"
                                   ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.StockNumber, new { car.ListingId })
                                   : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                    car.StockNumber, new { AppraisalId = car.ListingId }));
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                builder.Append(car.DaysInInvenotry + " Days");
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                int odometerNumber = 0;
                bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                if (odometerFlag)
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + odometerNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                else
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" + car.Mileage + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                int salePriceNumber = 0;
                bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                if (salePriceFlag)
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + salePriceNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                else
                    builder.Append("<input type=\"text\" id=\"" + car.ListingId + "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" + car.SalePrice + "\" disabled=\"disabled\"/>" + Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);
                builder.Append(" </div>" + Environment.NewLine);
                builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                if (!String.IsNullOrEmpty(car.Vin) && controllerName != "Appraisal")
                {
                    if (car.Vin.Length > 7)
                    {
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName, "VIN " + car.Vin.Substring(car.Vin.Length - 7), new { car.ListingId }));
                    }
                    else
                        builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName, "VIN " + car.Vin, new { car.ListingId }));
                }

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                    builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                else
                    builder.Append(car.ExteriorColor + Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                    builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                else
                    builder.Append(car.InteriorColor + Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                builder.Append(0 + " Market" + Environment.NewLine);
                builder.Append(Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);

                builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                if (car.CarFaxOwner == -1)
                    builder.Append("Unknown" + Environment.NewLine);
                else if (car.CarFaxOwner == 1)

                    builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                else
                {
                    builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                }
                builder.Append(Environment.NewLine);

                builder.Append("</div>" + Environment.NewLine);

                builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                //if (car.Reconstatus)
                //    builder.Append("<input type=\"checkbox\" checked=\"checked\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);
                //else
                //    builder.Append("<input type=\"checkbox\" id=\"" + car.ListingId + "\" name=\"reconcheckbox\" onclick=\"javascript:updateReconStatus(this);\"/>Recon" + Environment.NewLine);

                builder.Append(Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                //builder.Append("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Market/ViewEbay?ListingId=" + car.ListingId) + "\">Ebay</a>" + Environment.NewLine);
                builder.Append("<a class=\"iframe\" href=\"javascript:;\"></a>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);

                builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                if (controllerName != "Appraisal" && controllerName != "Wholesale")
                {
                    builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" +
                                   urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) +
                                   "\">BG</a>/<a title=\"Window Sticker\" target=\"_blank\" onclick=\"javascript:openWindowSticker(" +
                                   car.ListingId + ")" + "\">WS</a>" + Environment.NewLine);
                }
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
                builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
            }
            builder.Append("</div>" + Environment.NewLine);
        }

        public static string DynamicHtmlLabelForAdvancedSearch(this HtmlHelper<List<CarInfoFormViewModel>> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var builder = new StringBuilder();
            bool flag = true;
            int noOfType = model.Select(e => e.Type).Distinct().Count();

            if (noOfType > 1)
            {
                builder.Append("<div id=\"inven\">" + Environment.NewLine);
                builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);

                var newInventory = model.Where(e => e.Type == 0).ToList();
                if (newInventory.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, newInventory, flag, "New");
                }

                var usedInventory = model.Where(e => e.Type == 1).ToList();
                if (usedInventory.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, usedInventory, flag, "Used");
                }

                var soldInventory = model.Where(e => e.Type == 4).ToList();
                if (soldInventory.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, soldInventory, flag, "Sold");
                }

                var wholeSale = model.Where(e => e.Type == 2).ToList();
                if (wholeSale.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, wholeSale, flag, "Wholesale");
                }

                var appraisal = model.Where(e => e.Type == 3).ToList();
                if (appraisal.Count > 0)
                {
                    DynamicHtmlLabelForAdvancedSearchByCategory(htmlHelper, urlHelper, builder, appraisal, flag, "Appraisals");
                }

                builder.Append("</div>" + Environment.NewLine);
                builder.Append("</div>" + Environment.NewLine);
            }
            else
            {
                switch (fieldName)
                {
                    case "InventoryGrid":

                        builder.Append("<div id=\"inven\">" + Environment.NewLine);
                        builder.Append("<div class=\"scroll-pane\">" + Environment.NewLine);
                        builder.Append("<div id=\"table\" >" + Environment.NewLine);
                        foreach (var car in model)
                        {
                            var controllerName = "Inventory";
                            var actionName = string.Empty;
                            var actionForViewOnly = string.Empty;
                            var paramName = string.Empty;
                            
                            switch (car.Type)
                            {
                                case 2:
                                    actionForViewOnly = "ViewIWholesaleProfile";
                                    actionName = "ViewIWholesaleProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                                    controllerName = "Inventory";
                                    break;
                                case 3:
                                    actionForViewOnly = "ViewProfileForAppraisal";
                                    actionName = car.IsTruck ? "EditAppraisalForTruck" : "EditAppraisal";
                                    controllerName = "Appraisal";
                                    break;
                                case 4:
                                    actionForViewOnly = "ViewISoldProfile";
                                    actionName = "ViewISoldProfile";//car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                                    break;
                                default:
                                    actionForViewOnly = "ViewIProfile";
                                    actionName = car.IsTruck ? "EditIProfileForTruck" : "EditIProfile";
                                    break;
                            }

                            if (flag)
                            {
                                builder.Append("<div class=\"rowOuter dark\">" + Environment.NewLine);
                                flag = false;
                            }
                            else
                            {
                                builder.Append("<div class=\"rowOuter light\">" + Environment.NewLine);
                                flag = true;
                            }

                            builder.Append("<div class=\"imageCell column\">" + Environment.NewLine);
                            builder.Append(
                                " <div class=\"imageWrap\" style=\"width: 47px; height: 47px; overflow: hidden;\">" +
                                Environment.NewLine);
                            if (controllerName != "Appraisal")
                                builder.Append(
                                    ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto,
                                                              "", new { car.ListingId }, null,
                                                              new { width = 50, height = 50 }) + Environment.NewLine);
                            else
                                builder.Append(
                                    ImageLinkHelper.ImageLink(htmlHelper, actionName, controllerName, car.SinglePhoto,
                                                              "", new { AppraisalId = car.ListingId }, null,
                                                              new { width = 50, height = 50 }) + Environment.NewLine);
                            builder.Append(" <input type=\"hidden\" name=\"status\" class=\"status\" value=\"" +
                                           car.HealthLevel + "\" />" + Environment.NewLine);
                            builder.Append("  </div>  " + Environment.NewLine);
                            builder.Append("  </div>  " + Environment.NewLine);
                            builder.Append("<div class=\"infoCell column\">" + Environment.NewLine);
                            builder.Append("<div class=\"innerRow1 clear\">" + Environment.NewLine);
                            builder.Append("<div class=\"cell evenShorter column\">" + Environment.NewLine);

                            switch (car.MarketRange)
                            {
                                case 3:
                                    if (controllerName != "Appraisal")
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    else
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/above.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    break;
                                case 2:
                                    if (controllerName != "Appraisal")
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    else
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/at.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    break;
                                case 1:
                                    if (controllerName != "Appraisal")
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { ListingID = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    else
                                        builder.Append("<a href=\"" + urlHelper.Action(actionForViewOnly, controllerName, new { AppraisalId = car.ListingId }) + "\"><img src=\"" + urlHelper.Content("~/images/below.jpg") + "\" style=\"height: 20px; width: 15px;\" /></a>" + Environment.NewLine);
                                    break;
                                default:
                                    builder.Append("<img src=\"" + urlHelper.Content("~/images/question.gif") + "\" style=\"height: 20px; width: 15px;\" />" + Environment.NewLine);
                                    break;
                            }

                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell short noBorder column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.ModelYear.ToString(),
                                                                                new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.ModelYear.ToString(),
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.Make, new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.Make,
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Model, 12),
                                                                                new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Model, 12),
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Trim, 12),
                                                                                new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                CommonHelper.TrimString(car.Trim, 12),
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                            builder.Append(controllerName != "Appraisal"
                                               ? ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.StockNumber, new { car.ListingId })
                                               : ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                car.StockNumber,
                                                                                new { AppraisalId = car.ListingId }));
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column\">" + Environment.NewLine);
                            builder.Append(car.DaysInInvenotry + " Days");
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            int odometerNumber = 0;
                            bool odometerFlag = Int32.TryParse(car.Mileage, out odometerNumber);

                            if (odometerFlag)
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" +
                                               odometerNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                            else
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"odometer\" class=\"sForm\" onblur=\"javascript:updateMileage(this);\" value=\"" +
                                               car.Mileage + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            int salePriceNumber = 0;
                            bool salePriceFlag = Int32.TryParse(car.SalePrice, out salePriceNumber);

                            if (salePriceFlag)
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" +
                                               salePriceNumber.ToString("#,##0") + "\" disabled=\"disabled\"/>" + Environment.NewLine);
                            else
                                builder.Append("<input type=\"text\" id=\"" + car.ListingId +
                                               "\" name=\"price\" class=\"sForm\" onblur=\"javascript:updateSalePrice(this);\" value=\"" +
                                               car.SalePrice + "\" disabled=\"disabled\"/>" + Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append(" </div>" + Environment.NewLine);
                            builder.Append("<div class=\"innerRow2 clear\">" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid noBorder column\">" + Environment.NewLine);

                            if (!String.IsNullOrEmpty(car.Vin) && controllerName != "Appraisal")
                            {
                                if (car.Vin.Length > 7)
                                {
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                  "VIN " +
                                                                                  car.Vin.Substring(car.Vin.Length - 7),
                                                                                  new { car.ListingId }));
                                }
                                else
                                    builder.Append(ImageLinkHelper.BlankImageLink(htmlHelper, actionName, controllerName,
                                                                                  "VIN " + car.Vin, new { car.ListingId }));
                            }

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell long column\">" + Environment.NewLine);
                            if (!String.IsNullOrEmpty(car.ExteriorColor) && car.ExteriorColor.Length > 13)
                                builder.Append(car.ExteriorColor.Substring(0, 12) + Environment.NewLine);
                            else
                                builder.Append(car.ExteriorColor + Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);
                            if (!String.IsNullOrEmpty(car.InteriorColor) && car.InteriorColor.Length > 13)
                                builder.Append(car.InteriorColor.Substring(0, 12) + Environment.NewLine);
                            else
                                builder.Append(car.InteriorColor + Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell column marketSection\">" + Environment.NewLine);
                            builder.Append(0 + " Market" + Environment.NewLine);
                            builder.Append(Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid cars column marketSection\">" + Environment.NewLine);
                            if (car.CarFaxOwner == -1)
                                builder.Append("Unknown" + Environment.NewLine);
                            else if (car.CarFaxOwner == 1)

                                builder.Append(car.CarFaxOwner + " Owner" + Environment.NewLine);
                            else
                            {
                                builder.Append(car.CarFaxOwner + " Owners" + Environment.NewLine);

                            }
                            builder.Append(Environment.NewLine);

                            builder.Append("</div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell mid column marketSection\">" + Environment.NewLine);

                            builder.Append(Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);

                            builder.Append("<a class=\"iframe\" href=\"javascript:;\"></a>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);

                            builder.Append("<div class=\"cell shorter column\">" + Environment.NewLine);
                            if (controllerName != "Appraisal" && controllerName != "Wholesale")
                            {
                                builder.Append("<a title=\"Buyer Guide\" class=\"iframe\" href=\"" +
                                               urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + car.ListingId) +
                                               "\">BG</a>/<a title=\"Window Sticker\" target=\"_blank\" onclick=\"javascript:openWindowSticker(" +
                                               car.ListingId + ")" + "\">WS</a>" + Environment.NewLine);
                            }
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);
                            builder.Append("<div class=\"clear\"></div>    " + Environment.NewLine);
                            builder.Append("</div>" + Environment.NewLine);

                        }

                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        builder.Append("</div>" + Environment.NewLine);
                        break;

                    default:
                        builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" +
                                       fieldName + "\" value=\"error\"" + " />");
                        break;
                }
            }
            return builder.ToString();
        }

        public static string DynamicHtmlForm(this HtmlHelper<CarInfoFormViewModel> htmlHelper)
        {
            var model = htmlHelper.ViewData.Model;
            string result = "";
            if (model.Success)
            {
                result = " <form name=\"viewProfileForm\"  action=\"../Appraisal/ViewProfileByVin\" method=\"post\"  onsubmit=\"return validateForm()\" >";
            }
            else
            {
                result = " <form name=\"viewProfileForm\" action=\"../Appraisal/ViewProfileByYear\" method=\"post\"  onsubmit=\"return validateForm()\" >";
            }



            return result;
        }

        public static string DynamicHtmlImageForm(this HtmlHelper<ImageViewModel> htmlHelper)
        {
            var model = htmlHelper.ViewData.Model;
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string result = "";

            result = "<form id=\"ImageHandlerForm\" action=\"" + urlHelper.Content("~/ImageHandlers/Handler.ashx") + "?ListingId=" + model.ListingId + "&DealerId=" + model.DealerId + "&Vin=" + model.Vin + "&Overlay=1" + "\" method=\"POST\" enctype=\"multipart/form-data\" >";

            return result;
        }

        private static string GetPercentageString(double ratio)
        {
            return ratio.ToString("0%");
        }

        public static string DynamicHtmlLabelAdmin(this HtmlHelper<AdminViewModel> htmlHelper, string fieldName)
        {


            var model = htmlHelper.ViewData.Model;

            var builder = new StringBuilder();


            switch (fieldName)
            {

                case "SortSetting":

                    if (model.SortSet.Equals("Make"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Make\">Make (Default)</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }
                    else if (model.SortSet.Equals("Model"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Model\">Model (Default)</option>");
                        builder.Append("<option value=\"Make\">Make</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }
                    else if (model.SortSet.Equals("Year"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Model\">Year (Default)</option>");
                        builder.Append("<option value=\"Make\">Make</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }
                    else if (model.SortSet.Equals("Market"))
                    {

                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Market\">Market (Default)</option>");
                        builder.Append("<option value=\"Make\">Make</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("</select>");
                    }
                    else
                    {
                        builder.Append(" <select id=\"SortSet\" name=\"SortSet\">");
                        builder.Append("<option selected=\"selected\" value=\"Make\">Make (Default)</option>");
                        builder.Append("<option value=\"Model\">Model</option>");
                        builder.Append("<option value=\"Year\">Year</option>");
                        builder.Append("<option value=\"Market\">Market</option>");
                        builder.Append("</select>");
                    }



                    break;
                case "SoldOut":

                    if (model.SortSet.Equals("Delete (Default)"))
                    {

                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"0\">Delete (Default)</option>");
                        builder.Append("<option value=\"3\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"5\">Display as Sold (5 Days)</option>");
                        builder.Append("<option value=\"7\">Display as Sold (7 Days)</option>");
                        builder.Append("<option value=\"30\">Display as Sold (30 Days)</option>");
                        builder.Append("</select>");
                    }
                    else if (model.SortSet.Equals("Display as Sold (3 Days)"))
                    {

                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"3\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"0\"> Delete (Default)</option>");
                        builder.Append("<option value=\"5\">Display as Sold (5 Days)</option>");
                        builder.Append("<option value=\"7\">Display as Sold (7 Days)</option>");
                        builder.Append("<option value=\"7\">Display as Sold (30 Days)</option>");
                        builder.Append("</select>");
                    }
                    else if (model.SortSet.Equals("Display as Sold (5 Days)"))
                    {

                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"0\">Display as Sold (5 Days)</option>");
                        builder.Append("<option value=\"0\">Delete (Default)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (7 Days)</option>");
                        builder.Append("</select>");
                    }
                    else
                    {
                        builder.Append(" <select id=\"SoldAction\" name=\"SoldAction\">");
                        builder.Append("<option selected=\"selected\" value=\"0\">Display as Sold (7 Days)</option>");
                        builder.Append("<option value=\"0\">Delete (Default)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (3 Days)</option>");
                        builder.Append("<option value=\"0\">Display as Sold (5 Days)</option>");
                        builder.Append("</select>");
                    }



                    break;

                case "UsersList":
                    bool flag = true;

                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (flag)
                        {
                            builder.Append("<tr class=\"l\">" + Environment.NewLine);
                            flag = false;
                        }
                        else
                        {
                            builder.Append("<tr>" + Environment.NewLine);
                            flag = true;
                        }
                        builder.Append("<td>" + user.Name + "</td>" + Environment.NewLine);

                        builder.Append("<td>" + user.UserName + "</td>" + Environment.NewLine);

                        builder.Append("<td><input type=\"password\" id=\"" + user.UserName + "\" name=\"pass\"  value=\"" + user.PassWord + "\" onblur=\"javascript:updatePass(this);\"/></td>" + Environment.NewLine);

                        builder.Append("<td><input type=\"text\" id=\"" + user.UserName + "\" name=\"email\" value=\"" + user.Email + "\" onblur=\"javascript:updateEmail(this);\"/></td>" + Environment.NewLine);

                        builder.Append("<td><input type=\"text\" id=\"" + user.UserName + "\" name=\"phone\"  value=\"" + user.Cellphone + "\" onblur=\"javascript:updatePhone(this);\"/></td>" + Environment.NewLine);

                        builder.Append("<td><select  id=\"" + user.UserName + "\"  name=\"userlevel\" onchange=\"javascript:changeRole(this);\">" + Environment.NewLine);

                        switch (user.Role)
                        {
                            case "Manager":
                                builder.Append("<option selected=\"selected\" value=\"Manager\" >Manager</option>" + Environment.NewLine);

                                builder.Append("<option  value=\"Employee\">Employee</option>" + Environment.NewLine);

                                builder.Append("<option value=\"Admin\">Admin</option>" + Environment.NewLine);
                                break;
                            case "Employee":
                                builder.Append("<option selected=\"selected\" value=\"Employee\">Employee</option>" + Environment.NewLine);

                                builder.Append("<option  value=\"Admin\">Admin</option>" + Environment.NewLine);

                                builder.Append("<option value=\"Manager\">Manager</option>" + Environment.NewLine);
                                break;
                            case "Admin":
                                builder.Append("<option selected=\"selected\" value=\"Admin\">Admin</option>" + Environment.NewLine);

                                builder.Append("<option  value=\"Employee\">Employee</option>" + Environment.NewLine);

                                builder.Append("<option value=\"Manager\">Manager</option>" + Environment.NewLine);
                                break;
                            default:
                                break;
                        }



                        builder.Append("</select>" + Environment.NewLine);

                        builder.Append("<br/></td>" + Environment.NewLine);

                        if (model.MutipleDealer)

                            builder.Append("<td><input class=\"editBTN\" type=\"button\" id=\"" + user.UserName + "\" name=\"Edit\" value=\"Edit\" onclick=\"javascript:editUser(this);\" /></td>" + Environment.NewLine);

                        builder.Append("<td><input class=\"deleteBTN\" type=\"button\" id=\"" + user.UserName + "\" name=\"delete\" value=\"Delete\" onclick=\"javascript:deleteUser(this);\" /></td>" + Environment.NewLine);

                        builder.Append("</tr>" + Environment.NewLine);
                    }

                    break;
                case "ApprasialUsersNotificationList":

                    int index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.AppraisalNotification)
                            builder.Append(" <li id=\"Appraisal" + user.UserName + "\"><input type=\"checkbox\" checked=\"checked\" onclick=\"javascript:appraisalNotifyPerUser(this);\"   id=\"AppraisalCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"Appraisal" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:appraisalNotifyPerUser(this);\"   id=\"AppraisalCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "WholeSaleUsersNotificationList":


                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.WholeSaleNotfication)
                            builder.Append(" <li id=\"WholeSale" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:wholeSaleNotifyPerUser(this);\" id=\"WholeSaleCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"WholeSale" + user.UserName + "\"><input type=\"checkbox\" disabled=true  onclick=\"javascript:wholeSaleNotifyPerUser(this);\" id=\"WholeSaleCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "InventoryUsersNotificationList":


                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.InventoryNotfication)
                            builder.Append(" <li id=\"Inventory" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:inventoryNotifyPerUser(this);\" id=\"InventoryCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"Inventory" + user.UserName + "\"><input type=\"checkbox\" disabled=true  onclick=\"javascript:inventoryNotifyPerUser(this);\" id=\"InventoryCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "24HUsersNotificationList":


                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.TwentyFourHourNotification)
                            builder.Append(" <li id=\"24H" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\"  onclick=\"javascript:twentyfourhourNotifyPerUser(this);\" id=\"24HCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"24H" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:twentyfourhourNotifyPerUser(this);\" id=\"24HCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "NoteUsersNotificationList":


                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.NoteNotification)
                            builder.Append(" <li id=\"Note" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:noteNotifyPerUser(this);\" id=\"NoteCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"Note" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:noteNotifyPerUser(this);\" id=\"NoteCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");


                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "PriceUsersNotificationList":

                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.PriceChangeNotification)
                            builder.Append(" <li id=\"Price" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:priceNotifyPerUser(this);\" id=\"PriceCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"Price" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:priceNotifyPerUser(this);\" id=\"PriceCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "BucketJumpReportUsersNotificationList":

                    index = 0;


                    foreach (var user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.BucketJumpReportNotification)
                            builder.Append(" <li id=\"BucketJump" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:bucketJumpReportNotifyPerUser(this);\" id=\"BucketJumpCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"BucketJump" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:bucketJumpReportNotifyPerUser(this);\" id=\"BucketJumpCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "AgeingBucketJumpUsersNotificationList":

                    index = 0;


                    foreach (var user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.AgeingBucketJumpNotification)
                            builder.Append(" <li id=\"Age" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:ageNotifyPerUser(this);\" id=\"AgeCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"Age" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:ageNotifyPerUser(this);\" id=\"AgeCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "MarketPriceRangeNotificationList":

                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.MarketPriceRangeChangeNotification)
                            builder.Append(" <li id=\"MarketPriceRange" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:marketPriceRangeNotifyPerUser(this);\" id=\"MarketPriceRangeCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"MarketPriceRange" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:marketPriceRangeNotifyPerUser(this);\" id=\"MarketPriceRangeCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;

                case "ImageUploadNotificationList":

                    index = 0;


                    foreach (UserRoleViewModel user in model.Users)
                    {
                        if (index % 5 == 0)
                            builder.Append("<ul class=\"column\">");

                        if (user.ImageUploadNotification)
                            builder.Append(" <li id=\"ImageUpload" + user.UserName + "\"><input type=\"checkbox\"  checked=\"checked\" onclick=\"javascript:imageUploadNotifyPerUser(this);\" id=\"ImageUploadCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");
                        else
                            builder.Append(" <li id=\"ImageUpload" + user.UserName + "\"><input type=\"checkbox\" disabled=true onclick=\"javascript:imageUploadNotifyPerUser(this);\" id=\"ImageUploadCheckbox" + user.UserName + "\"/> " + user.UserName + " </li>");

                        index++;

                        if (index % 5 == 0)
                            builder.Append("</ul>");


                    }


                    break;
                case "AppraisalNotification":
                    if (model.AppraisalNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:appraisalNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:appraisalNotify(this);\" />On/Off");
                    break;
                case "WholeSaleNotification":
                    if (model.WholeSaleNotfication)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:WholesaleNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:WholesaleNotify(this);\" />On/Off");
                    break;
                case "InventoryNotfication":
                    if (model.InventoryNotfication)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:InventoryNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:InventoryNotify(this);\" />On/Off");
                    break;
                case "TwentyFourHourNotification":
                    if (model.TwentyFourHourNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:TwentyFourHourNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:TwentyFourHourNotify(this);\" />On/Off");
                    break;
                case "NoteNotification":
                    if (model.NoteNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:NoteNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:NoteNotify(this);\" />On/Off");
                    break;
                case "PriceChangeNotification":
                    if (model.PriceChangeNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:PriceNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:PriceNotify(this);\" />On/Off");
                    break;
                case "MarketPriceRangeChangeNotification":
                    if (model.MarketPriceRangeChangeNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:MarketPriceRangeNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:MarketPriceRangeNotify(this);\" />On/Off");
                    break;
                case "ImageUploadNotification":
                    if (model.ImageUploadNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:ImageUploadNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:ImageUploadNotify(this);\" />On/Off");
                    break;
                case "BucketJumpReportNotification":
                    if (model.BucketJumpReportNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:BucketJumpReportNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:BucketJumpReportNotify(this);\" />On/Off");
                    break;
                case "AgeingBucketJumpNotification":
                    if (model.AgeingBucketJumpNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:AgeNotify(this);\" />On/Off");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:AgeNotify(this);\" />On/Off");
                    break;
                case "OverrideStockImage":
                    if (model.OverrideStockImage)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:OverideStockImage(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:OverideStockImage(this);\" />");
                    break;

                case "RetailPriceWSNotification":
                    if (model.RetailPriceWSNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:retailPriceWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:retailPriceWindowStickerNotify(this);\" />");
                    break;

                case "DealerDiscountWSNotification":
                    if (model.DealerDiscountWSNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:dealerDiscountWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:dealerDiscountWindowStickerNotify(this);\" />");
                    break;

                case "ManufacturerReabateNotification":
                    if (model.ManufacturerReabteWsNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:manufacturerRebateWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:manufacturerRebateWindowStickerNotify(this);\" />");
                    break;

                case "SalePriceNotification":
                    if (model.SalePriceWsNotification)
                        builder.Append("<input type=\"checkbox\" name=\"on\" checked=\"checked\"  onclick=\"javascript:salePriceWindowStickerNotify(this);\" />");
                    else
                        builder.Append("<input type=\"checkbox\" name=\"on\" onclick=\"javascript:salePriceWindowStickerNotify(this);\" />");
                    break;
                default:
                    builder.Append("<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" value=\"error\"" + " />");

                    break;



            }



            return builder.ToString();
        }

        public static string DynamicHtmlLabelForEbay(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            //var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";
            switch (fieldName)
            {
                case "Vin":
                    result = "VIN <label for=\"" + fieldName + "\">" + model.Vin + "</label>";
                    break;

                case "Year":
                    result = "<label for=\"" + fieldName + "\">" + model.ModelYear + "</label>";
                    break;

                case "Make":
                    result = "<label for=\"" + fieldName + "\">" + model.Make + "</label>";
                    break;

                case "Model":
                    result = "<label for=\"" + fieldName + "\">" + model.Model + "</label>";
                    break;

                case "Stock":
                    result = "Stock #<label for=\"" + fieldName + "\">" + model.StockNumber + "</label>";
                    break;


                case "Mileage":
                    result = "<label for=\"" + fieldName + "\">" + model.Mileage + " Miles" + "</label>";
                    break;

                case "ExtColor":
                    result = "<label for=\"" + fieldName + "\">" + model.ExteriorColor + "</label>";
                    break;

                case "LargeImage":
                    result = "<img id=\"largeImage\" name=\"largeImage\" src=\"" + model.DefaultImageUrl + "\"/>" + Environment.NewLine;
                    break;


                case "LoadImage":

                    result = "<img id=\"image1\" name=\"image1\" class=\"selected\" src=\"" + model.DefaultImageUrl + "\"/>" + Environment.NewLine;
                    break;


                case "FactoryPackageOption":

                    string[] carOptions = model.CarsOptions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var fo in carOptions)
                    {

                        result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo + "\"/>" + fo + "</li>";
                    }

                    break;

                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }

        public static string DynamicHtmlLabelForSticker(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;
            //var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
            string result = "";
            switch (fieldName)
            {

                case "CarName":
                    result = model.CarName;
                    break;
                case "Tranmission":
                    result = model.Tranmission;
                    break;
                case "ExteriorColor":
                    result = model.ExteriorColor;
                    break;
                case "InteriorColor":
                    result = model.InteriorColor;
                    break;
                case "Vin":
                    result = "Vin :" + model.Vin;
                    break;

                case "Stock":
                    result = "Stock #" + model.StockNumber;
                    break;

                case "Year":
                    result = "<label for=\"" + fieldName + "\">" + model.ModelYear + "</label>";
                    break;

                case "Make":
                    result = "<label for=\"" + fieldName + "\">" + model.Make + "</label>";
                    break;

                case "FuelEconomyCity":
                    result = "<label for=\"" + fieldName + "\">" + model.FuelEconomyCity + "</label>";
                    break;
                case "FuelEconomyHighWay":
                    result = "<label for=\"" + fieldName + "\">" + model.FuelEconomyHighWay + "</label>";
                    break;
                case "SalePrice":
                    double salePriceNumber = 0;
                    bool salePriceflag = double.TryParse(model.SalePrice, out salePriceNumber);
                    if (salePriceflag)
                        result = "<label for=\"" + fieldName + "\">" + salePriceNumber.ToString("C") + "</label>";
                    else
                        result = "<label for=\"" + fieldName + "\">" + model.SalePrice + "</label>";
                    break;
                case "MSRP":
                    result = "<label for=\"" + fieldName + "\">" + model.MSRP + "</label>";
                    break;
                case "BarCode":
                    result = "<img height=\"70\" style=\"position: relative; z-index:300;\" src=\"http://generator.onbarcode.com/linear.aspx?TYPE=4&DATA=" + model.Vin + "&UOM=0&X=0&Y=62&LEFT-MARGIN=0&RIGHT-MARGIN=0&TOP-MARGIN=0&BOTTOM-MARGIN=0&RESOLUTION=72&ROTATE=0&BARCODE-WIDTH=0&BARCODE-HEIGHT=0&SHOW-TEXT=true&TEXT-FONT=Arial%7c9%7cRegular&TextMargin=6&FORMAT=gif&ADD-CHECK-SUM=false&I=1.0&N=2.0&SHOW-START-STOP-IN-TEXT=true&PROCESS-TILDE=false\" />";
                    break;

                case "Logo":
                    result = "<img height=\"60\" width=\"260\" src=\"" + model.LogoUrl + "\"/>";
                    break;

                case "MECHANICALLIST":
                    foreach (string tmp in model.MechanicalList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "EXTERIORLIST":
                    foreach (string tmp in model.ExteriorList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "ENTERTAINMENTLIST":
                    foreach (string tmp in model.EntertainmentList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "INTERIORLIST":
                    foreach (string tmp in model.InteriorList)
                        result += "<li class=\"\">" + tmp + "</li>";
                    break;
                case "SAFETYLIST":
                    foreach (string tmp in model.SafetyList)
                        result += "<li class=\"\">" + tmp + "</li>";

                    break;

                case "FactoryPackageOption":


                    if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                    {
                        foreach (var fo in model.FactoryPackageOptions)
                        {
                            result += "<li>" + fo.getName() + "</li>";
                            result += "<li class=\"pricing\">" + fo.getMSRP() + " </li>";
                        }
                    }

                    break;
                case "FactoryOption":

                    if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                    {
                        foreach (var fo in model.FactoryNonInstalledOptions)
                        {
                            result += "<li>" + fo.getName() + "</li>";
                            result += "<li class=\"pricing\">" + fo.getMSRP() + " </li>";
                        }
                    }
                    break;

                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + " value=\"error\"" + " />";
                    break;

            }

            return result;
        }

        public static string DynamicHtmlControlForIprofile(this HtmlHelper<CarInfoFormViewModel> htmlHelper, string name, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            string result = "";

            if (model.Success)
            {
                switch (fieldName)
                {
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            if (model.ExistOptions == null)
                            {
                                foreach (var fo in model.FactoryPackageOptions)
                                {

                                    result += "<li> <input type=\"checkbox\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                }
                            }
                            else
                            {

                                var packageExist = model.ExistOptions.Where(t => t.Contains("Pkg") || t.Contains("Package"));

                                if (packageExist == null || packageExist.Count() == 0)
                                {

                                    foreach (var fo in model.FactoryPackageOptions)
                                    {

                                        result += "<li> <input type=\"checkbox\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                    }
                                }
                                else
                                {
                                    foreach (var fo in model.FactoryPackageOptions)
                                    {
                                        var tmp = model.ExistOptions.Where(t => fo.getName().Contains(t));
                                        if (tmp == null || tmp.Count() == 0)

                                            result += "<li> <input type=\"checkbox\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                        else
                                            result += "<li> <input type=\"checkbox\" checked=\"yes\"  class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                    }
                                }
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages available for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\">";
                        result += "<ul class=\"options\">";

                        if (model.ExistOptions == null)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                            }
                        }
                        else
                        {
                            var optionExist = model.ExistOptions.Where(t => !t.Contains("Pkg") && !t.Contains("Package"));
                            if (optionExist == null || optionExist.Count() == 0)
                            {
                                if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                                {
                                    foreach (var fo in model.FactoryNonInstalledOptions)
                                    {
                                        result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                    }
                                }
                            }
                            else
                            {
                                foreach (var fo in model.FactoryNonInstalledOptions)
                                {
                                    var tmp = model.ExistOptions.Where(t => fo.getName().Contains(t));
                                    if (tmp == null || tmp.Count() == 0)

                                        result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                    else
                                        result += "<li> <input type=\"checkbox\" checked=\"yes\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                                }
                            }
                        }


                        result += "</ul>";
                        result += "</div>";
                        break;
                    case "hiddenListingId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\"" + " />"; ;
                        break;
                    case "hiddenDealershipId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealershipId + "\"" + " />"; ;
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarImageUrl + "\"" + " />"; ;
                        break;
                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealershipId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.StockNumber))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.StockNumber + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;
                    case "ExteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            var selectedExteriorColor = model.ExteriorColorList.Where(x => x.colorName.ToUpperInvariant().Equals(model.ExteriorColor.ToUpperInvariant()));

                            var extractExteriorColorList = model.ExteriorColorList.Where(x => !x.colorName.ToUpperInvariant().Equals(model.ExteriorColor.ToUpperInvariant()));

                            if (selectedExteriorColor == null || selectedExteriorColor.Count() == 0)
                            {
                                result += "<option>" + "Other Colors" + "</option>";

                                foreach (Color ec in model.ExteriorColorList)
                                {
                                    result += "<option>" + ec.colorName + "</option>";
                                }


                            }
                            else
                            {
                                var exteriorColorList = new List<Color>();

                                exteriorColorList.Add(selectedExteriorColor.First());

                                exteriorColorList.AddRange(extractExteriorColorList);
                                foreach (Color ec in exteriorColorList)
                                {
                                    result += "<option>" + ec.colorName + "</option>";
                                }
                                result += "<option>" + "Other Colors" + "</option>";
                            }
                        }
                        else
                        {
                            result += "<option>" + "Other Colors" + "</option>";
                        }

                        result += "</select>";


                        break;
                    case "InteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            var selectedInteriorColor = model.InteriorColorList.Where(x => x.colorName.ToUpperInvariant().Equals(model.InteriorColor.ToUpperInvariant()));

                            var extractInteriorColorList = model.InteriorColorList.Where(x => !x.colorName.ToUpperInvariant().Equals(model.InteriorColor.ToUpperInvariant()));

                            if (selectedInteriorColor == null || selectedInteriorColor.Count() == 0)
                            {
                                result += "<option>" + "Other Colors" + "</option>";

                                foreach (Color ec in model.InteriorColorList)
                                {

                                    result += "<option>" + ec.colorName + "</option>";
                                }
                            }
                            else
                            {
                                var interiorColorList = new List<Color>();

                                interiorColorList.Add(selectedInteriorColor.First());

                                interiorColorList.AddRange(extractInteriorColorList);

                                foreach (Color ic in interiorColorList)
                                {
                                    result += "<option>" + ic.colorName + "</option>";
                                }
                                result += "<option>" + "Other Colors" + "</option>";

                            }
                        }
                        else
                        {
                            result += "<option>" + "Other Colors" + "</option>";
                        }

                        result += "</select>";


                        break;

                    case "CustomExtColor":
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            var list = model.ExteriorColorList.Where(t => t.colorName.Equals(model.ExteriorColor));

                            if (list == null || list.Count() == 0)
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                            else
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";
                        }
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";

                        break;
                    case "CustomIntColor":

                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            var list = model.InteriorColorList.Where(t => t.colorName.Equals(model.InteriorColor));

                            if (list == null || list.Count() == 0)
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";

                            else
                                result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\"" + ">";
                        }
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"  name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";



                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;



                    case "Odometer":
                        if (String.IsNullOrEmpty(model.Mileage))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litters + "\" />";
                        break;
                    case "Tranmission":
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        if (String.IsNullOrEmpty(model.Tranmission))
                        {
                            result += "<option>Automatic</option>";
                            result += "<option>Manual</option>";
                            result += "<option>Shiftable Automatic Tranmission</option>";
                        }
                        else
                        {
                            if (model.Tranmission.Equals("Automatic"))
                            {
                                result += "<option>Automatic</option>";
                                result += "<option>Manual</option>";
                                result += "<option>Shiftable Automatic Tranmission</option>";
                            }
                            else if (model.Tranmission.Equals("Manual"))
                            {
                                result += "<option>Manual</option>";
                                result += "<option>Automatic</option>";
                                result += "<option>Shiftable Automatic Tranmission</option>";
                            }

                            else if (model.Tranmission.Equals("Shiftable Automatic Tranmission"))
                            {
                                result += "<option>Shiftable Automatic Tranmission</option>";
                                result += "<option>Manual</option>";
                                result += "<option>Automatic</option>";

                            }
                            else
                            {
                                result += "<option>Automatic</option>";
                                result += "<option>Manual</option>";
                                result += "<option>Shiftable Automatic Tranmission</option>";
                            }
                        }

                        result += "<option>" + "Other Tranmission" + "</option>";
                        result += "</select>";
                        //result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Tranmission + "\" />";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<select class=\"z-index\" style=\"width:130px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        XmlNode driveNode = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Value=" + model.WheelDrive);
                        XmlNodeList allNode = XMLHelper.selectElements("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"));
                        if (driveNode != null)
                        {
                            result += "<option>" + driveNode.Attributes["Value"].Value + "</option>";

                            foreach (XmlNode node in allNode)
                            {
                                if (!node.Attributes["Value"].Value.Equals(driveNode.Attributes["Value"].Value))
                                    result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        else
                        {
                            foreach (XmlNode node in allNode)
                            {
                                result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        result += "<option>" + "Other Drives" + "</option>";
                        result += "</select>";


                        break;

                    //result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WheelDrive + "\" />";
                    //break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.MSRP + "\" />";
                        break;
                    case "SalePrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SalePrice + "\" />";
                        break;
                    case "RetailPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "DealerDiscount":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "ManufacturerRebate":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;

                    case "WindowStickerPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;

                    case "EditRetailPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "EditDealerDiscount":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "EditManufacturerRebate":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;

                    case "EditWindowStickerPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;
                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;

                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "Certified":
                        if (model.IsCertified)
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\" checked=\"checked\" />";
                        else
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\"/>";
                        break;
                    case "PriorRental":
                        if (model.PriorRental)
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\" checked=\"checked\" />";
                        else
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\"/>";
                        break;
                    case "EditPriorRental":
                        if (model.PriorRental)
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditDealerDemo":
                        if (model.DealerDemo)
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditUnwind":
                        if (model.Unwind)
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "Warranty":
                        if (model.WarrantyInfo == 0)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";


                        }
                        else if (model.WarrantyInfo == 1)
                        {
                            result +=
                                "<input type=\"radio\" id=\"warrantyasis\" checked=\"checked\" value=\"1\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }

                        else if (model.WarrantyInfo == 2)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 3)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantydealercertified\"  value=\"3\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 4)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }


                        break;

                    case "JavaScript":
                        result = generateGraphJavaScript(model);
                        break;
                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                }
            }
            else
            {
                var regex = new Regex(@"(?<=\w)\w", RegexOptions.Compiled);
                switch (fieldName)
                {
                    case "FactoryPackageOption":


                        if (model.FactoryPackageOptions != null && model.FactoryPackageOptions.Count > 0)
                        {

                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            foreach (var fo in model.FactoryPackageOptions)
                            {
                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedPackages\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                            }

                            result += "</ul>";

                            result += "</div>";
                        }
                        else
                        {
                            result += "<div id=\"Packages\">";

                            result += "<ul class=\"options\">";

                            result += "<li><label for=\"" + name + "\">" + "No packages selected for this model" + "</label></li>";

                            result += "</ul>";

                            result += "</div>";
                        }


                        break;
                    case "NonInstalledOption":

                        result += "<div id=\"Options\">";
                        result += "<ul class=\"options\">";
                        if (model.FactoryNonInstalledOptions != null && model.FactoryNonInstalledOptions.Count > 0)
                        {
                            foreach (var fo in model.FactoryNonInstalledOptions)
                            {
                                result += "<li> <input type=\"checkbox\" class=\"z-index\" name=\"selectedOptions\" value=\"" + fo.getName() + fo.getMSRP() + "\" onclick=\"javascript:changeMSRP(this)\"" + " />" + fo.getName() + fo.getMSRP() + "</li>";
                            }
                        }

                        result += "</ul>";
                        result += "</div>";
                        break;

                    case "Vin":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "HiddenVin":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Vin + "\" />";
                        break;
                    case "ListingId":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\" />";
                        break;
                    case "Dealership":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealershipId + "\" />";
                        break;
                    case "Stock":
                        if (String.IsNullOrEmpty(model.StockNumber))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.StockNumber + "\" />";
                        break;
                    case "AppraisalId":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalGenerateId + "\" />";
                        break;
                    case "Date":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + DateTime.Now.ToShortDateString() + "\" />";
                        break;
                    case "Year":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ModelYear + "\" />";
                        break;
                    case "Make":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Make + "\" />";
                        break;
                    case "Model":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Model + "\" />";
                        break;
                    case "Trim":
                        if (!String.IsNullOrEmpty(model.Trim))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Trim + "\" />";
                        else
                        {
                            if (model.TrimList != null)
                            {
                                if (model.TrimList.Count > 0)
                                {
                                    result = "<select class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                                    foreach (string trim in model.TrimList)
                                    {
                                        result += "<option>" + trim + "</option>";
                                    }
                                    result += "</select>";
                                }
                                else
                                    result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                            }
                            else

                                result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";

                        }
                        break;
                    case "TrimListForProfile":
                        if (model.TrimList != null)
                        {
                            if (model.TrimList.Count > 0)
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";
                                foreach (string trim in model.TrimList)
                                {
                                    result += "<option>" + trim + "</option>";

                                }
                                result += "<option>Not specified</option>";
                                result += "</select>";
                            }
                            else
                            {
                                result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                                result += "<option>All</option>";

                                result += "</select>";
                            }
                        }
                        else
                        {
                            result = "<select id=\"trim\" multiple=\"multiple\" style=\"width: 75px !important;\">";
                            result += "<option>All</option>";
                            result += "</select>";
                        }

                        break;

                    case "ExteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.ExteriorColorList != null && model.ExteriorColorList.Count() > 0)
                        {
                            foreach (Color ec in model.ExteriorColorList)
                            {
                                result += "<option>" + ec.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;
                    case "InteriorColor":


                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        if (model.InteriorColorList != null && model.InteriorColorList.Count() > 0)
                        {
                            foreach (Color ic in model.InteriorColorList)
                            {
                                result += "<option>" + ic.colorName + "</option>";
                            }
                        }
                        result += "<option>" + "Other Colors" + "</option>";
                        result += "</select>";


                        break;
                    case "EditPriorRental":
                        if (model.PriorRental)
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"PriorRentalYes\" value=\"true\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"PriorRentalNo\" value=\"false\" onclick=\"javascript:priorRentalUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditDealerDemo":
                        if (model.DealerDemo)
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"DealerDemoYes\" value=\"true\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"DealerDemoNo\" value=\"false\" onclick=\"javascript:dealerDemoUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "EditUnwind":
                        if (model.Unwind)
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\"  />No";
                        }
                        else
                        {
                            result += "<input type=\"radio\" id=\"UnwindYes\" value=\"true\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                   "\"  />Yes";

                            result += "<input type=\"radio\" id=\"UnwindNo\" value=\"false\" onclick=\"javascript:unwindUpdate(this);\" name=\"" + name +
                                      "\" checked=\"checked\" />No";
                        }
                        break;
                    case "CustomExtColor":

                        if (!String.IsNullOrEmpty(model.ExteriorColor))

                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"/>";

                        break;
                    case "CustomIntColor":

                        if (!String.IsNullOrEmpty(model.InteriorColor))
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        else
                            result = "<em style=\"font-size:.7em;\">Other: </em><input style=\"width: 70px !important;\" type=\"text\" id=\"" + name + "\"/>";

                        break;
                    case "ExteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ExteriorColor + "\" />";
                        break;


                    case "InteriorColorSingle":


                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.InteriorColor + "\" />";
                        break;

                    case "RetailPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "DealerDiscount":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "ManufacturerRebate":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;
                    case "SalePrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SalePrice + "\" />";
                        break;
                    case "Warranty":
                        if (model.WarrantyInfo == 0)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";


                        }
                        else if (model.WarrantyInfo == 1)
                        {
                            result +=
                                "<input type=\"radio\" id=\"warrantyasis\" checked=\"checked\" value=\"1\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";
                        }

                        else if (model.WarrantyInfo == 2)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 3)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantydealercertified\"  value=\"3\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" + name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }
                        else if (model.WarrantyInfo == 4)
                        {
                            result += "<input type=\"radio\" id=\"warrantyasis\" value=\"1\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>As Is<br />";
                            result += "<input type=\"radio\" id=\"warrantymanufacturerwarranty\"  value=\"2\" name=\"" +
                                      name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer warranty<br />";
                            result += "<input type=\"radio\" id=\"warrantydealercertified\"  value=\"3\" name=\"" + name +
                                      "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Dealer Certified<br />";
                            result +=
                                "<input type=\"radio\" checked=\"checked\" id=\"warrantymanufacturercertified\"  value=\"4\" name=\"" +
                                name +
                                "\" onclick=\"javascript:warrantyInfoUpdate(this);\"/>Manufacturer Certified<br />";

                        }

                        break;


                    case "WindowStickerPrice":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;

                    case "EditRetailPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.RetailPrice + "\" />";
                        break;
                    case "EditDealerDiscount":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealerDiscount + "\" />";
                        break;
                    case "EditManufacturerRebate":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ManufacturerRebate + "\" />";
                        break;

                    case "EditWindowStickerPrice":
                        result = "<input class=\"small\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.WindowStickerPrice + "\" />";
                        break;
                    case "Odometer":
                        if (String.IsNullOrEmpty(model.Mileage))
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        else
                            result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Mileage + "\" />";
                        break;
                    case "Cylinders":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Cylinder + "\" />";
                        break;
                    case "Litters":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Litters + "\" />";
                        break;
                    case "Tranmission":
                        result = "<select class=\"z-index\" style=\"width:100px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";

                        if (String.IsNullOrEmpty(model.Tranmission))
                        {
                            result += "<option>Automatic</option>";
                            result += "<option>Manual</option>";
                        }
                        else
                        {
                            if (model.Tranmission.Equals("Automatic"))
                            {
                                result += "<option>Automatic</option>";
                                result += "<option>Manual</option>";
                            }
                            else
                            {
                                result += "<option>Manual</option>";
                                result += "<option>Automatic</option>";
                            }
                        }

                        result += "<option>" + "Other Tranmission" + "</option>";
                        result += "</select>";
                        break;
                    case "Doors":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Door + "\" />";
                        break;
                    case "BodyType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.BodyType + "\" />";
                        break;
                    case "FuelType":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Fuel + "\" />";
                        break;
                    case "DriveType":
                        result = "<select class=\"z-index\" style=\"width:130px !important;\" id=\"" + name + "\" name=\"" + name + "\"" + ">";
                        XmlNode driveNode = XMLHelper.selectOneElement("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"), "Value=" + model.WheelDrive);
                        XmlNodeList allNode = XMLHelper.selectElements("Drive", System.Web.HttpContext.Current.Server.MapPath("~/App_Data/WheelDrive.xml"));
                        if (driveNode != null)
                        {
                            result += "<option>" + driveNode.Attributes["Value"].Value + "</option>";

                            foreach (XmlNode node in allNode)
                            {
                                if (!node.Attributes["Value"].Value.Equals(driveNode.Attributes["Value"].Value))
                                    result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        else
                        {
                            foreach (XmlNode node in allNode)
                            {
                                result += "<option>" + node.Attributes["Value"].Value + "</option>";
                            }
                        }
                        result += "<option>" + "Other Drives" + "</option>";
                        result += "</select>";

                        break;
                    case "MSRP":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.MSRP + "\" />";
                        break;

                    case "Description":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"50\" rows=\"15\"></textarea>";
                        break;
                    case "Notes":
                        result = "<textarea  class=\"z-index\" id=\"" + name + "\" name=\"" + name + "\" cols=\"87\" rows=\"3\" ></textarea>";
                        break;
                    case "HiddenPhotos":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.SinglePhoto + "\" />";
                        break;
                    case "AppraisalType":


                        result += "<input type=\"radio\" id=\"customerV\" value=\"Customer\" name=\"" + name + "\"/> Customer Vehicle? <br />";
                        result += "<input type=\"radio\" id=\"auctionV\"  value=\"Auction\" name=\"" + name + "\"/> Auction Vehicle?";
                        break;

                    case "HiddenDescription":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.Description + "\" />";
                        break;
                    case "HiddenOptions":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.CarsOptions + "\" />";
                        break;
                    case "Username":
                        result = "<input class=\"z-index\" type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "Password":
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                    case "HiddenAppraisalID":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalId + "\"" + " />";
                        break;
                    case "HiddenAppraisalType":
                        result = "<input type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.AppraisalType + "\"" + " />";
                        break;
                    case "MECHANICALLIST":
                        foreach (string tmp in model.MechanicalList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "EXTERIORLIST":
                        foreach (string tmp in model.ExteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "ENTERTAINMENTLIST":
                        foreach (string tmp in model.EntertainmentList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "INTERIORLIST":
                        foreach (string tmp in model.InteriorList)
                            result += "<li class=\"\">" + tmp + "</li>";
                        break;
                    case "SAFETYLIST":
                        foreach (string tmp in model.SafetyList)
                            result += "<li class=\"\">" + tmp + "</li>";

                        break;
                    case "hiddenListingId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.ListingId + "\"" + " />"; ;
                        break;
                    case "hiddenDealershipId":
                        result = "<input class=\"z-index\" type=\"hidden\" id=\"" + name + "\" name=\"" + name + "\" value=\"" + model.DealershipId + "\"" + " />"; ;
                        break;
                    case "Certified":
                        if (model.IsCertified)
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\" checked=\"checked\" />";
                        else
                            result += "<input type=\"checkbox\" id=\"" + name + "\" name=\"" + name + "\"/>";
                        break;
                    case "JavaScript":
                        result = generateGraphJavaScript(model);
                        break;
                    default:
                        result = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" value=\"\"" + " />";
                        break;
                }
            }

            return result;
        }

        public static string DynamicHtmlControlForBuyerGuide(this HtmlHelper<BuyerGuideViewModel> htmlHelper, string fieldName)
        {
            var model = htmlHelper.ViewData.Model;

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            string result = "";

            switch (fieldName)
            {
                case "BackGroundImage":



                    if (model.PriorRental)
                        result += "<img src=\"" + urlHelper.Content("~/images/bGuideBG-prior-rental.jpg") + "\"/>";
                    else
                    {
                        result += "<img src=\"" + urlHelper.Content("~/images/bGuideBG.png") + "\"/>";
                    }
                    break;
                case "BackGroundImageInSpanish":


                    if (model.PriorRental)
                        result += "<img src=\"" + urlHelper.Content("~/images/bg-spanish-prior-rental.jpg") + "\"/>";
                    else
                    {
                        result += "<img src=\"" + urlHelper.Content("~/images/bGuideBG-spanish-resize.png") + "\"/>";
                    }
                    break;
                case "Warranty":


                    if (model.Warranty == 1)
                        result += "<span id=\"asIs\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                    else
                    {
                        result += "<span id=\"fullLimit\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                        if (model.Warranty == 3)
                            result += "<span id=\"limited\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></img></span>";
                    }
                    break;
                case "WarrantySpanish":


                    if (model.Warranty == 1)
                        result += "<span id=\"asIsSpanish\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                    else
                    {
                        result += "<span id=\"fullLimitSpanish\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                        if (model.Warranty == 3)
                            result += "<span id=\"limitedSpanish\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></img></span>";
                    }
                    break;
                case "WarrantySpanishBi":


                    if (model.Warranty == 1)
                        result += "<span id=\"asIsSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                    else
                    {
                        result += "<span id=\"fullLimitSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-lrg.png") + "\"></img></span>";
                        if (model.Warranty == 3)
                            result += "<span id=\"limitedSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></img></span>";
                    }
                    break;
                case "WarrantyInfo":
                    if (model.Warranty == 1 && !String.IsNullOrEmpty(model.AsWarranty))
                        result += "<p id=\"systems\"><img src=\"" + model.AsWarranty + "\"></img></p>";
                    if (model.Warranty == 2 && !String.IsNullOrEmpty(model.ManufacturerWarranty))
                        result += "<p id=\"systems\"><img src=\"" + model.ManufacturerWarranty + "\"></img></p>";
                    else if (model.Warranty == 3 && !String.IsNullOrEmpty(model.DealerCertified))
                        result += "<p id=\"systems\"><img src=\"" + model.DealerCertified + "\"></img></p>";
                    else if (model.Warranty == 4 && !String.IsNullOrEmpty(model.ManufacturerCertified))
                        result += "<p id=\"systems\"><img src=\"" + model.ManufacturerCertified + "\"></img></p>";

                    break;
                case "WarrantyInfoSpanish":
                    if (model.Warranty == 1 && !String.IsNullOrEmpty(model.AsWarrantyInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.AsWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 2 && !String.IsNullOrEmpty(model.ManufacturerWarrantyInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.ManufacturerWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 3 && !String.IsNullOrEmpty(model.DealerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.DealerCertifiedInSpanish + "\"></img></p>";
                    if (model.Warranty == 4 && !String.IsNullOrEmpty(model.ManufacturerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanish\"><img src=\"" + model.ManufacturerCertifiedInSpanish + "\"></img></p>";

                    break;
                case "WarrantyInfoSpanishBi":
                    if (model.Warranty == 1 && !String.IsNullOrEmpty(model.AsWarrantyInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.AsWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 2 && !String.IsNullOrEmpty(model.ManufacturerWarrantyInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.ManufacturerWarrantyInSpanish + "\"></img></p>";
                    if (model.Warranty == 3 && !String.IsNullOrEmpty(model.DealerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.DealerCertifiedInSpanish + "\"></img></p>";
                    if (model.Warranty == 4 && !String.IsNullOrEmpty(model.ManufacturerCertifiedInSpanish))
                        result += "<p id=\"systemsSpanishBi\"><img src=\"" + model.ManufacturerCertifiedInSpanish + "\"></img></p>";

                    break;
                case "ServiceContract":
                    if (model.Warranty == 4)
                        result += "<span id=\"contract\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    else if (model.ServiceContract)
                        result += "<span id=\"contract\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    break;
                case "ServiceContractSpanish":
                    if (model.Warranty == 4)
                        result += "<span id=\"contractSpanish\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    else if (model.ServiceContract)
                        result += "<span id=\"contractSpanish\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";

                    break;
                case "ServiceContractSpanishBi":
                    if (model.Warranty == 4)
                        result += "<span id=\"contractSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";
                    else if (model.ServiceContract)
                        result += "<span id=\"contractSpanishBi\"><img src=\"" + urlHelper.Content("~/images/check-sml.png") + "\"></span>";

                    break;
                case "WarrantyFullLimit":
                    if (model.Warranty == 3)
                        result += " <span id=\"labor\">100*</span><span id=\"parts\">100*</span>";
                    else if (model.Warranty == 4)
                        result += " <span id=\"labor\">100*</span><span id=\"parts\">100*</span>";

                    break;
                case "WarrantyFullLimitSpanish":
                    if (model.Warranty == 3)
                        result += " <span id=\"laborSpanish\">100*</span><span id=\"partsSpanish\">100*</span>";
                    else if (model.Warranty == 4)
                        result += " <span id=\"laborSpanish\">100*</span><span id=\"partsSpanish\">100*</span>";

                    break;
                case "WarrantyFullLimitSpanishBi":
                    if (model.Warranty == 3)
                        result += " <span id=\"laborSpanishBi\">100*</span><span id=\"partsSpanish\">100*</span>";
                    else if (model.Warranty == 4)
                        result += " <span id=\"laborSpanishBi\">100*</span><span id=\"partsSpanish\">100*</span>";

                    break;
                default:
                    result = "<input class=\"z-index\" type=\"text\" id=\"" + fieldName + "\" name=\"" + fieldName + " value=\"error\"" + " />";
                    break;
            }


            return result;
        }

        public static MvcHtmlString ToJson(this HtmlHelper html, object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return MvcHtmlString.Create(serializer.Serialize(obj));
        }

        public static MvcHtmlString ToJson(this HtmlHelper html, object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return MvcHtmlString.Create(serializer.Serialize(obj));
        }
    }
}
