<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.CarStatusViewModel>" %>
<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<div id="admin_top_btns_holder">
        <% InventoryUserRight userRight = SessionHandler.UserRight.Inventory; %>

        <% if (Model.Type == Constanst.CarInfoType.New || Model.Type == Constanst.CarInfoType.Used || Model.Type == Constanst.CarInfoType.Wholesale) %>
        <% { %>

        <a href="<%= Url.Action("EditIProfile", "Inventory", new {ListingID = Model.ListingId}) %>"
            style="color: black">
            <div id="profile_edit_tab" class="admin_top_btns">
                Edit Profile
            </div>
        </a>
        

        <% if (userRight.ViewProfile_Ebay == true) %>
        <% { %>
        <a href="<%=Url.Action("ViewEbayWithoutPopUp", "Market", new { ListingId = Model.ListingId, inventoryStatus=Model.InventoryStatus }) %>"
            style="color: black">
            <div id="Ebay_Tab" class="admin_top_btns">
                Ebay
            </div>
        </a>
        <% } %>
        <% if (userRight.Craigslist) %>
        <% { %>
        <a href="javascript:;" style="color: black">
            <div id="postCL_Tab" class="admin_top_btns">
                Craigslist
            </div>
        </a>
        <% } %>
            <% if (userRight.ViewProfile_WS == true) %>
            <% { %>
        <a title="Window Sticker" class="iframe iframeCommon" onclick="windowSticker(<%= Model.ListingId %>)"
            style="color: black">
            <div id="profile_ws_tab" class="pf_has_popup admin_top_btns">
                WS
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_BG == true) %>
            <% { %>
        <a title="Buyer's Guide" href="<%=Url.Action("ViewBuyerGuideWithoutPopUp", "Report", new { listingId = Model.ListingId, inventoryStatus=Model.InventoryStatus }) %>"
            style="color: black">
            <div id="BG_Tab" class="admin_top_btns">
                BG
            </div>
        </a>
            <% } %>

            <% if (SessionHandler.DealerGroup != null && SessionHandler.DealerGroup.DealerList.Count() > 1 && userRight.ViewProfile_Transfer == true) %>
            <% { %>
        <a class="iframe iframeTransfer" href="<%=Url.Action("OpenVehicleTransferWindow", "Inventory", new { listingId = Model.ListingId }) %>"
            style="color: black">
            <div id="profile_transfer_tab" class="pf_has_popup admin_top_btns">
                Transfer
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_Status == true) %>
            <% { %>
        <a class="iframe iframeStatus" href="<%=Url.Action("OpenStatus", "Inventory", new { listingId = Model.ListingId,inventoryStatus=Model.InventoryStatus,isSoldOut=false }) %>"
            style="color: black">
            <div id="profile_status_tab" class="pf_has_popup admin_top_btns">
                Status
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_PriceTracking == true) %>
            <% { %>
        <a class="iframe iframeTrackingPrice" href="<%=Url.Action("ViewPriceTracking","Inventory", new { listingId = Model.ListingId }) %>"
            style="color: black">
            <div id="profile_pricetracking_tab" class="pf_has_popup admin_top_btns">
                Price Tracking
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_BucketJump) %>
            <% { %>
        <a class="iframe iframeBucketJump" href="<%= Url.Action("ViewBucketJumpTracking", "Inventory", new {listingId = Model.ListingId, type = Constanst.VehicleStatus.Inventory}) %>"
            style="color: black">
             <% if ( Model.Type == Constanst.CarInfoType.Used) %>
                    <%
                {
                    %>
            <div id="profile_bucketjump_tab" class="pf_has_popup admin_top_btns">
                Bucket Jump
            </div>
            <%
                }
           %>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_Ebay == true || userRight.ViewProfile_BG) %>
            <% { %>
        <a title="Back" onclick="window.history.back()" style="color: black">
            <div id="profile_back_tab" class="pf_has_popup admin_top_btns">
                Back
            </div>
        </a>
            <% } %>
        <% } %>

        <% if (Model.Type == Constanst.CarInfoType.Sold) %>
        <% {%>
        <a href="<%= Url.Action("EditIProfile", "Inventory", new {ListingID = Model.ListingId}) %>"
            style="color: black">
            <div id="Div1" class="admin_top_btns">
                Edit Profile
            </div>
        </a>
        <% if (userRight.ViewProfile_WS == true) %>
            <% { %>
        <a title="Window Sticker" class="iframe iframeCommon" onclick="windowSticker(<%= Model.ListingId %>)"
            style="color: black">
            <div id="Div2" class="pf_has_popup admin_top_btns">
                WS
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_BG == true) %>
            <% { %>
        <a title="Buyer's Guide" href="<%=Url.Action("ViewBuyerGuideWithoutPopUp", "Report", new { listingId = Model.ListingId, inventoryStatus=Model.InventoryStatus }) %>"
            style="color: black">
            <div id="Div3" class="admin_top_btns">
                BG
            </div>
        </a>
            <% } %>
        <% if (userRight.ViewProfile_Status == true) %>
            <% { %>
        <a class="iframe iframeStatus" href="<%=Url.Action("OpenStatus", "Inventory", new { listingId = Model.ListingId,inventoryStatus=Model.InventoryStatus,isSoldOut=true }) %>"
            style="color: black">
            <div id="Div4" class="pf_has_popup admin_top_btns">
                Status
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_PriceTracking == true) %>
            <% { %>
        <a class="iframe iframeTrackingPrice" href="<%=Url.Action("ViewPriceTrackingForSold","Inventory", new { listingId = Model.ListingId }) %>"
            style="color: black">
            <div id="Div5" class="pf_has_popup admin_top_btns">
                Price Tracking
            </div>
        </a>
            <% } %>

            <% if (userRight.ViewProfile_BucketJump == true) %>
            <% { %>
        <a class="iframe iframeBucketJump" href="<%= Url.Action("ViewBucketJumpTrackingForSold", "Inventory", new {listingId = Model.ListingId}) %>"
            style="color: black">
            <div id="Div6" class="pf_has_popup admin_top_btns">
                Bucket Jump
            </div>
        </a>
            <% } %>

        <a class="iframe iframeCustomerInfo" href="<%=Url.Action("CustomerInfo","Inventory", new { listingId = Model.ListingId }) %>"
            style="color: black">
            <div id="profile_customerInfo_tab" class="pf_has_popup admin_top_btns">
                Customer Information
            </div>
        </a>

            <% if (userRight.ViewProfile_Ebay == true || userRight.ViewProfile_BG) %>
            <% { %>
        <a title="Back" onclick="window.history.back()" style="color: black">
            <div id="Div7" class="pf_has_popup admin_top_btns">
                Back
            </div>
        </a>
            <% } %>
        <% } %>
    </div>