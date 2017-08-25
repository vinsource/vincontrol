<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.EbayFormViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.VehicleInfo.Year %>
    <%=Model.VehicleInfo.Make %>
    <%=Model.VehicleInfo.Model %>
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    <input type="hidden" id="hdDetailType" value="<%=ViewData["detailType"] %>" />
    <input type="hidden" id="NumberOfWSTemplate" name="NumberOfWSTemplate" value="<%= (int) SessionHandler.NumberOfWSTemplate %>" />

    <div id="admin_top_btns_holder">
        <% InventoryUserRight userRight = SessionHandler.UserRight.Inventory; %>

        <a href="<%=Url.Action("EditIProfile","Inventory",new {ListingID=Model.ListingId}) %>"
            style="color: black">
            <div class="admin_top_btns">
                Edit Profile
            </div>
        </a>

        <% if (userRight.ViewProfile_Ebay) %>
        <% { %>
        <div id="Ebay_Tab" class="admin_top_btns">
            <a title="Window Sticker" href="<%=Url.Action("ViewEbayWithoutPopUp", "Market", new {Model.ListingId, inventoryStatus=Model.InventoryStatus }) %>">Ebay </a>
        </div>
        <% } %>
        <% if (userRight.Craigslist) %>
        <% { %>
        <a href="javascript:;" style="color: black">
            <div id="postCL_Tab" class="admin_top_btns">
                Craigslist
            </div>
        </a>
        <% } %>
        <% if (userRight.ViewProfile_WS) %>
        <% { %>
        <div id="profile_ws_tab" class="pf_has_popup admin_top_btns">
            <a title="Window Sticker" class="iframe iframeCommon" onclick="windowSticker(<%= Model.ListingId %>)">WS </a>
        </div>
        <% } %>

        <% if (userRight.ViewProfile_BG) %>
        <% { %>
        <div id="BG_Tab" class="admin_top_btns">
            <a title="Window Sticker" href="<%=Url.Action("ViewBuyerGuideWithoutPopUp", "Report", new { listingId = Model.ListingId, inventoryStatus=Model.InventoryStatus }) %>">BG </a>
        </div>
        <% } %>

        <% if (SessionHandler.DealerGroup != null && SessionHandler.DealerGroup.DealerList.Count() > 1 && userRight.ViewProfile_Transfer == true) %>
        <% { %>
        <div id="profile_transfer_tab" class="pf_has_popup admin_top_btns">
            <a class="iframe iframeTransfer" href="<%=Url.Action("OpenVehicleTransferWindow", "Inventory", new { listingId = Model.ListingId }) %>">Transfer </a>
        </div>
        <% } %>

        <% if (userRight.ViewProfile_Status) %>
        <% { %>
        <a class="iframe iframeStatus" href="<%=Url.Action("OpenStatus", "Inventory", new { listingId = Model.ListingId,inventoryStatus=Model.InventoryStatus,isSoldOut=false }) %>"
            style="color: black">
            <div id="profile_status_tab" class="pf_has_popup admin_top_btns">
                Status
            </div>
        </a>
        <% } %>

        <% if (userRight.ViewProfile_PriceTracking) %>
        <% { %>
        <div id="profile_pricetracking_tab" class="pf_has_popup admin_top_btns">
            <a class="iframe iframeTrackingPrice" href="<%=Url.Action("ViewPriceTracking","Inventory", new { listingId = Model.ListingId }) %>">Price Tracking</a>
        </div>
        <% } %>

        <% if (userRight.ViewProfile_BucketJump) %>
        <% { %>
        <% if (Model.VehicleInfo.Type == Constanst.CarInfoType.Sold) %>
        <% { %>
        <div id="profile_bucketjump_tab" class="pf_has_popup admin_top_btns">
            <a class="iframe iframeBucketJump" href="<%= Url.Action("ViewBucketJumpTrackingForSold", "Inventory", new {listingId = Model.ListingId}) %>">Bucket Jump</a>
        </div>
        <% } %>
        <% else %>
        <% { %>
        <div id="profile_bucketjump_tab" class="pf_has_popup admin_top_btns">
            <a class="iframe iframeBucketJump" href="<%= Url.Action("ViewBucketJumpTracking", "Inventory", new {listingId = Model.ListingId, type = Constanst.VehicleStatus.Inventory}) %>">Bucket Jump</a>
        </div>
        <% } %>
        <% } %>

        <%if (Model.VehicleInfo.Type == Constanst.CarInfoType.Wholesale) %>
        <%
          {
        %>
        <div id="back_to_inventory" class="pf_has_popup admin_top_btns">
            <a href="<%=Url.Action("TransferToInventoryFromWholesale","Inventory",new { listingId = Model.ListingId }) %>">Add To Inventory </a>
        </div>
        <% } %>
        <% if (Model.InventoryStatus == Constanst.InventoryStatus.Wholesale) %>
        <%
           { %>
        <a title="Back" href="<%= Url.Action("ViewIProfile",
                                             "Inventory", new {ListingID = Model.ListingId}) %>"
            style="color: black">
            <div id="profile_back_tab" class="pf_has_popup admin_top_btns">
                Back
            </div>
        </a>
        <% }
           else
           { %>
        <a title="Back" href="<%= Url.Action("ViewIProfile",
                                             "Inventory", new {ListingID = Model.ListingId}) %>"
            style="color: black">
            <div id="profile_back_tab" class="pf_has_popup admin_top_btns">
                Back
            </div>
        </a>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="profile_tab_holder" id="profile_ebay_holder" style="display: block">
        <% Html.BeginForm("GetEbayAdsPrice", "Market", FormMethod.Post, new { id = "PreviewEbayForm" }); %>
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <a target="_blank" onclick="SubmitPreview();">
                    <div class="btns_shadow profile_ebay_preview_btns">
                        <%--<%=Html.SubmitButton("Preview Listing")%>--%>
                        Preview Listing
                    </div>
                </a>
            </div>
        </div>
        <div id="container_right_content" style="min-height: 585px!important;">
            <div class="profile_ebay_top_text">
                The standard insertion fee for an ad eBay Motors can be $50 (Cars, Trucks, RV's/Campers,
                and Commercial Trucks). This is the base pricing with none of the below options
                selected.
            </div>
            <div class="profile_ebay_second_text">
                You will be given total pricing information when you preview your generated eBay
                listing.
            </div>
            <div class="profile_ebay_content_holder">
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Auction Type
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.DropDownListFor(x=>x.SelectedAuctionType,Model.AuctionType) %>
                    </div>
                </div>
                <%--    <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Exterior Color</div>
                    <div class="profile_ebay_items_right">
                        <%=Html.DropDownListFor(x=>x.SelectedExteriorColor,Model.ExteriorColorList) %>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Interior Color</div>
                    <div class="profile_ebay_items_right">
                        <%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %>
                    </div>
                </div>--%>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Warranty
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.CheckBoxFor(x=>x.LimitedWarranty) %><span class="subtext">Vehicle has existing
                            factory or dealer warranty.</span>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Listing Options
                    </div>
                    <div class="profile_ebay_items_right profile_ebay_listingoptions">
                        <%=Html.CheckBoxFor(x=>x.BoldTitle) %><span class="subtext">BoldTitle $5.00</span>
                        <%=Html.CheckBoxFor(x=>x.Highlight) %><span class="subtext">Highlight $5.00</span>
                        <%=Html.CheckBoxFor(x=>x.Border) %><span class="subtext">Border $5.00</span>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Listing Packages
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.CheckBoxFor(x=>x.Propackbundle) %><span class="subtext">ProPackBundle $34.95
                            (<em>This contains all three above listing options.</em>)</span>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Auction Starting Price
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.TextBoxFor(x => x.StartingPrice, new { @placeholder = "Insert a Starting Price",@readonly="readonly" })%>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Auction Reserve
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.TextBoxFor(x => x.ReservePrice, new { @placeholder = "Insert a Reserve Price",@readonly="readonly" })%>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Advertised/Buy It Now Price
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.TextBoxFor(x => x.BuyItNowPrice, new { @placeholder = "Insert an Advertised Price" })%>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Auction Decline
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.TextBoxFor(x => x.MinimumPrice, new { @placeholder = "Decline Offers Less Than This Amount",@readonly="readonly" })%>
                        <span class="subtext">For use with "Buy It Now/Best Offer".</span>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Hours to Deposit
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.DropDownListFor(x=>x.SelectedHoursToDeposit,Model.HoursToDeposit) %>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Auction Length
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.DropDownListFor(x=>x.SelectedAuctionLength,Model.AuctionLength) %>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Counter
                    </div>
                    <div class="profile_ebay_items_right">
                        <%=Html.CheckBoxFor(x=>x.HitCounter) %>
                        <span class="subtext">Include Hit Counter</span>
                    </div>
                </div>
                <div class="profile_ebay_items">
                    <div class="profile_ebay_items_left">
                        Seller Provided Title
                    </div>
                    <div class="profile_ebay_items_right">
                        <div class="profile_ebay_small_text">
                            Remain:
                            <nobr class="text_limit_number"><label id="charRemain">100</label></nobr>
                        </div>
                        <div class="profile_ebay_small_text">
                            **Limit 80 Characters
                        </div>
                        <%=Html.TextAreaFor(x => x.SellerProvidedTitle, 3, 50, new { @class = "profile_ebay_textareaNew" })%>
                    </div>
                </div>
            </div>
        </div>
        <% Html.EndForm(); %>
    </div>
    <%=Html.HiddenFor(x=>x.ListingId) %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/windowsticker.js")%>" type="text/javascript"></script>
    <script type="text/javascript">



        $(document).ready(function () {

            $('#SellerProvidedTitle').keyup(function () {
                var left = 80 - $(this).val().length;
                var text = $(this).val();
                if (left <= 0) {
                    left = 0;
                    //and if there are use substr to get the text before the limit
                    var new_text = text.substr(0, 80);

                    //and change the current text with the new text
                    $(this).val(new_text);
                }
                $('#charRemain').text(left);
            });

            var detailType = $('#hdDetailType').val();
            $('div.admin_top_btns').each(function () {
                $(this).removeClass('admin_top_btns_active');
                if ($(this).attr('id') == detailType) {
                    $(this).addClass('admin_top_btns_active');
                }
            });

            $('a:not(.iframe)').click(function (e) {
                if ($(this).attr('target') == '') {
                    $('#elementID').removeClass('hideLoader');
                }

            });

            $("a.iframeCommon").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeTransfer").fancybox({ 'width': 350, 'height': 257, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeMarkSold").fancybox({ 'width': 455, 'height': 306, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeWholeSale").fancybox({ 'width': 360, 'height': 127, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeTrackingPrice").fancybox({ 'width': 870, 'height': 509, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeBucketJump").fancybox({ 'width': 929, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("a.iframeStatus").fancybox({ 'margin': 0, 'padding': 0, 'width': 500, 'height': 230, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            $("#PreviewEbayForm").submit(function (event) {

                $('#elementID').removeClass('hideLoader');
                $thisval = $('#SelectedAuctionType>option:selected').val();
                if ($("#SellerProvidedTitle") == null || $("#SellerProvidedTitle").val() == '') {
                    ShowWarningMessage("Seller provided title is required.");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                else if ($("#SellerProvidedTitle").val().length > 80) {
                    ShowWarningMessage("Seller provided title must be less han 80 characters.");
                    $('#elementID').addClass('hideLoader');
                    return false;
                }
                if ($thisval == 'Chinese' || $thisval == 'ChineseNoBuyItNow') {

                    if ($("#StartingPrice").val() == '' || !IsPositiveNumeric($("#StartingPrice").val())) {
                        ShowWarningMessage("Auction starting price for standard auction type is required and positive number");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }
                    else if ($("#ReservePrice").val() == '' || !IsPositiveNumeric($("#ReservePrice").val())) {
                        ShowWarningMessage("Auction reserve price for standard auction type is required and positive number");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }

                    else {
                        if (Number($("#StartingPrice").val()) > Number($("#ReservePrice").val())) {
                            ShowWarningMessage("Auction reserve price must be greater than auction starting price in auction");
                            $('#elementID').addClass('hideLoader');
                            return false;
                        }
                        else {
                            event.preventDefault();


                            PreviewEbay(this);
                            window.parent.close();

                        }
                    }

                }
                else if ($thisval == 'BuyItNowBestOffer') {

                    if ($("#MinimumPrice").val() == '' || !IsPositiveNumeric($("#MinimumPrice").val())) {
                        ShowWarningMessage("Auto Decline Price for Buy It Now with Best Offer is required and positive number");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }
                    else if ($("#SellerProvidedTitle").val() == '' || $("#SellerProvidedTitle").val() == null) {
                        ShowWarningMessage("Seller provided title is required.");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }
                    else if ($("#SellerProvidedTitle").val().length > 80) {
                        ShowWarningMessage("Seller provided title must be less or equal than 80 characters.");
                        $('#elementID').addClass('hideLoader');
                        return false;
                    }
                    else {
                        if (Number($("#MinimumPrice").val()) > Number($("#BuyItNowPrice").val())) {
                            ShowWarningMessage("Auto Decline price must be less than advertised/buy it now price");
                            $('#elementID').addClass('hideLoader');
                            return false;
                        } else {
                            event.preventDefault();


                            PreviewEbay(this);

                        }
                    }
                }

                else {
                    event.preventDefault();


                    PreviewEbay(this);

                }





            });

            $("#SelectedAuctionType").change(function () {
                $thisval = $('#SelectedAuctionType>option:selected').val();

                if ($thisval == 'Chinese') {
                    $("#SelectedAuctionLength option[value='Days_21']").remove();
                    $("#StartingPrice").val("");
                    $("#ReservePrice").val("");
                    $("#MinimumPrice").val("");
                    $("#StartingPrice").removeAttr('readonly');
                    $("#ReservePrice").removeAttr('readonly');
                    $("#MinimumPrice").attr('readonly', 'readonly');
                }
                else if ($thisval == 'ChineseNoBuyItNow') {
                    $("#SelectedAuctionLength option[value='Days_21']").remove();
                    $("#StartingPrice").val("");
                    $("#ReservePrice").val("");
                    $("#MinimumPrice").val("");
                    $("#StartingPrice").removeAttr('readonly');
                    $("#ReservePrice").removeAttr('readonly');
                    $("#MinimumPrice").attr('readonly', 'readonly');
                }
                else if ($thisval == 'BuyItNowBestOffer') {
                    $("#StartingPrice").attr('readonly', 'readonly');
                    $("#StartingPrice").val("");
                    $("#ReservePrice").attr('readonly', 'readonly');
                    $("#ReservePrice").val("");
                    $("#MinimumPrice").removeAttr('readonly');

                }
                else {
                    $("#StartingPrice").attr('readonly', 'readonly');
                    $("#StartingPrice").val("");
                    $("#ReservePrice").attr('readonly', 'readonly');
                    $("#ReservePrice").val("");
                    $("#MinimumPrice").attr('readonly', 'readonly');
                    $("#MinimumPrice").val("");
                    if ($("#SelectedAuctionLength option[value='Days_21']").length == 0)
                        $("#SelectedAuctionLength").append('<option value="Days_21"> 21 days $32.00</option>');
                }

            });

            $("div[id='postCL_Tab']").click(function (e) {
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: "/Craigslist/AuthenticationChecking",
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        if (result == 302) {
                            $.ajax({
                                type: "GET",
                                dataType: "html",
                                url: "/Craigslist/LocationChecking",
                                data: {},
                                cache: false,
                                traditional: true,
                                success: function (result) {
                                    if (result == false)
                                        ShowWarningMessage("Please choose State/City/Location in Admin section.");
                                    else {
                                        var url = '/Craigslist/GoToPostingPreviewPage?listingId=' + $('#ListingId').val();
                                        $.fancybox({
                                            href: url,
                                            'width': 600,
                                            'height': 600,
                                            'hideOnOverlayClick': false,
                                            'centerOnScroll': true,
                                            'padding': 0,
                                            'scrolling': 'no',
                                            'onCleanup': function () {
                                            },
                                            isLoaded: function () {

                                            },
                                            onClosed: function () {

                                            }
                                        });
                                    }
                                }
                            });

                        } else {
                            ShowWarningMessage("Please enter valid Email/Password in Admin section.");
                        }
                    },
                    error: function (err) {

                    }
                });
            });
        });

        function SubmitPreview() {
            $('#PreviewEbayForm').submit();
        }

        function IsPositiveNumeric(num) {
            return (num >= 0);

        }

        function PreviewEbay(form) {

            blockUI();

            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: PreviewEbayClose

            });


        }

        function PreviewEbayClose(result) {

            window.parent.close();
            var actionUrl = '<%= Url.Action("PreviewEbayAds", "Market") %>';
            window.parent.PopupNewWindow(actionUrl);
            unblockUI();
        }


    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        input[type="text"], select
        {
            background-color: #D1D7D4;
            border: 1px solid black;
            height: 20px;
            margin-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>