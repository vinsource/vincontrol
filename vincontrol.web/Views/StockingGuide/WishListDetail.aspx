<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>ViewAllInventory</title>
    <link href="/Content/kpi.css" rel="stylesheet" type="text/css">
    <link href="/Content/VinControl/v3_kpi.css" rel="stylesheet" type="text/css">
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Content/Vincontrol/StockingGuide/WishList.css")%>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <div id="container_right_content">
        <div style="padding: 20px;">
            <label style="font-weight: bold;color:#3366cc; ">Here is the vehicle information for the <%=ViewData["Make"] %> <%=ViewData["Model"] %> <%=ViewData["Trim"] %></label>. This list shows the vehicles currently on the market and in auction.
        </div>
        <div style="clear: both"></div>
        <div class="WishListMarket WishListMarket_tabActive" id="divMarketLink" onclick="ViewMarket();">
            Market
        </div>
        <div class="WishListAuction" id="divAuctionLink" onclick="ViewAuction();">
            Auction
        </div>
        <div style="clear: both"></div>
        <div id="market_right_content_nav" class="right_content_nav">
            <div class="right_content_nav_items nav_Year">
                <a href="javascript:void(0)">Year</a>
                <img class="imgSort" id="imgSortYear" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Market_Make">
                <a href="javascript:void(0)">Make</a>
                <img class="imgSort" id="imgSortMake" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Market_Model">
                <a href="javascript:void(0)">Model</a>
                <img class="imgSort" id="imgSortModel" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Market_Trim">
                <a href="javascript:void(0)">Trim</a>
                <img class="imgSort" id="imgSortTrim" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
             <div class="right_content_nav_items nav_Vin">
                <a href="javascript:void(0)">VIN</a>
                <img class="imgSort" id="imgSortVin" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
             <div class="right_content_nav_items nav_Color">
                <a href="javascript:void(0)">Exterior Color</a>
                <img class="imgSort" id="imgSortColor" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Seller">
                <a href="javascript:void(0)">Seller</a>
                <img class="imgSort" id="imgSortSeller" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Distance">
                <a href="javascript:void(0)">Distance</a>
                <img class="imgSort" id="imgSortDistance" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Age">
                <a href="javascript:void(0)">Age</a>
                <img class="imgSort" id="imgSortAge" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Price">
                <a href="javascript:void(0)">Price</a>
                <img class="imgSort" id="imgSortPrice" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Market_Mileage">
                <a href="javascript:void(0)">Mileage</a>
                <img class="imgSort" id="imgSortMileage" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
        </div>
           <div id="auction_right_content_nav" class="right_content_nav">
            <div class="right_content_nav_items nav_Year">
                <a href="javascript:void(0)">Year</a>
                <img class="imgSort" id="img2" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Make">
                <a href="javascript:void(0)">Make</a>
                <img class="imgSort" id="img3" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Model">
                <a href="javascript:void(0)">Model</a>
                <img class="imgSort" id="img4" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Trim">
                <a href="javascript:void(0)">Trim</a>
                <img class="imgSort" id="img5" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Seller">
                <a href="javascript:void(0)">Seller</a>
                <img class="imgSort" id="img6" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Distance">
                <a href="javascript:void(0)">Distance</a>
                <img class="imgSort" id="img7" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Age">
                <a href="javascript:void(0)">Age</a>
                <img class="imgSort" id="img8" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Price">
                <a href="javascript:void(0)">Price</a>
                <img class="imgSort" id="img9" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Mileage">
                <a href="javascript:void(0)">Mileage</a>
                <img class="imgSort" id="img10" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
        </div>
        <div id="DivContent">
            <div id="divHasContent" class="vin_listVehicle_wishListDetail_holder">
            </div>
            <div id="divNoContent" style="display: none; padding-left: 400px; margin-top: 10px;">There are no vehicles in this category.</div>
        </div>
    </div>
     
    <script src="<%=Url.Content("~/Scripts/jquery-1.8.3.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/StockingGuide/WishListDetail.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jsrender.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/kpi.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';

    </script>
    <%=Html.Partial("_TemplateWishListDetail")  %>
    <%=Html.Partial("_TemplateAuctionWishListDetail")  %>
    <input type="hidden" id="hdMake" value="<%=ViewData["Make"] %>" />
    <input type="hidden" id="hdModel" value="<%=ViewData["Model"] %>" />
    <input type="hidden" id="hdTrim" value="<%=ViewData["Trim"] %>" />
</body>
</html>





