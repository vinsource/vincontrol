<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    WishListDetail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="stockingGuide_tab">
                WishList
            </div>

            <div class="stockingGuide_tabBrand">
                <a href="<%=Url.Action("WishList") %>">List</a>
            </div>
            <div class="stockingGuide_tabBrand stockingGuide_tabActive">
                09-14 <%=ViewData["Make"] %> <%=ViewData["Model"] %> <%=ViewData["Trim"] %>
            </div>
        </div>
    </div>
    <div id="container_right_content">
        <div style="padding: 20px;">
            Here is the vehicle information for the <%=ViewData["Make"] %> <%=ViewData["Model"] %> <%=ViewData["Trim"] %>. This list shows the vehicles currently on the market and in auction.
        </div>
        <div class="divWishListInfo">
            <div class="divWishListInfoLeft">
                <img src="../Content/images/car.jpg" height="100" />
            </div>
            <div class="divWishListInfoMiddle">
                <div class="WishListInfoHeader"><%=ViewData["Make"] %> <%=ViewData["Model"] %> <%=ViewData["Trim"] %></div>
                <div style="clear: both"></div>
                <div class="WishListInfoContent">
                    <div class="WishListInfoContentLeft">
                        Engine:<br />
                        Style:<br />
                        Segement:<br />
                        Base MSRP:<br />
                    </div>
                    <div class="WishListInfoContentRight">
                        Exterior Colors<br />
                        <div style="width: 100%">
                            <div style="width: 30px; height: 20px; background-color: black; float: left"></div>
                            <div style="width: 30px; height: 20px; background-color: blue; float: left; margin-left: 5px;"></div>
                            <div style="width: 30px; height: 20px; background-color: yellow; float: left; margin-left: 5px;"></div>
                            <div style="width: 30px; height: 20px; background-color: red; float: left; margin-left: 5px;"></div>
                            <div style="width: 30px; height: 20px; background-color: purple; float: left; margin-left: 5px;"></div>
                        </div>
                        <div style="clear: both; height: 5px;"></div>
                        <div style="width: 100%">
                            <div style="width: 30px; height: 20px; background-color: blue; float: left"></div>
                            <div style="width: 30px; height: 20px; background-color: black; float: left; margin-left: 5px;"></div>
                            <div style="width: 30px; height: 20px; background-color: red; float: left; margin-left: 5px;"></div>
                            <div style="width: 30px; height: 20px; background-color: purple; float: left; margin-left: 5px;"></div>
                            <div style="width: 30px; height: 20px; background-color: yellow; float: left; margin-left: 5px;"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divWishListInfoRight">
                <div class="WishListInfoHeader">Market Statistics</div>
                <div style="clear: both"></div>
                <div class="divWishListStatistics">
                    <div class="divWishListStatisticsHeader">
                        <div class="divHighest">Highest</div>
                        <div class="divAverage">Average</div>
                        <div class="divLowest">Lowest</div>
                        <div class="divTurn">Turn</div>
                        <div class="divHistory">History</div>
                    </div>
                    <div class="divWishListStatisticsContent">
                        <div class="divHighestContent" id="Highest"></div>
                        <div class="divAverageContent" id="Average"></div>
                        <div class="divLowestContent" id="Lowest"></div>
                        <div class="divTurnContent"></div>
                        <div class="divHistoryContent"></div>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both"></div>
        <div class="WishListMarket WishListMarket_tabActive" id="divMarketLink" onclick="ViewMarket();">
            Market
        </div>
        <div class="WishListAuction" id="divAuctionLink" onclick="ViewAuction();">
            Auction
        </div>
        <div style="clear: both"></div>
        <div id="right_content_nav" class="right_content_nav">
            <div class="right_content_nav_items nav_Year">
                <a href="javascript:void(0)">Year</a>
                <img class="imgSort" id="img1" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Make">
                <a href="javascript:void(0)">Make</a>
                <img class="imgSort" id="imgSortAge" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Model">
                <a href="javascript:void(0)">Model</a>
                <img class="imgSort" id="imgSortMarket" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Trim">
                <a href="javascript:void(0)">Trim</a>
                <img class="imgSort" id="imgSortVin" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Seller">
                <a href="javascript:void(0)">Seller</a>
                <img class="imgSort" id="imgSortStock" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Distance">
                <a href="javascript:void(0)">Distance</a>
                <img class="imgSort" id="imgSortYear" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Age">
                <a href="javascript:void(0)">Age</a>
                <img class="imgSort" id="imgSortMake" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Price">
                <a href="javascript:void(0)">Price</a>
                <img class="imgSort" id="imgSortModel" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
            <div class="right_content_nav_items nav_Mileage">
                <a href="javascript:void(0)">Mileage</a>
                <img class="imgSort" id="imgSortTrim" src="../Content/images/vincontrol/dot.png" width="12px">
            </div>
        </div>
        <div id="DivContent">
            <div id="divHasContent" class="vin_listVehicle_wishListDetail_holder">
                
            </div>
            <div id="divNoContent" style="display: none; padding-left: 400px; margin-top: 10px;">There are no vehicles in this category.</div>
        </div>
    </div>
    
    <input type="hidden" id="hdMake" value="<%=ViewData["Make"] %>"/>
    <input type="hidden" id="hdModel" value="<%=ViewData["Model"] %>"/>
    <input type="hidden" id="hdTrim" value="<%=ViewData["Trim"] %>"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="/Content/kpi.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/VinControl/v3_kpi.css" rel="stylesheet" type="text/css"/>
    <link href="<%=Url.Content("~/Content/Vincontrol/StockingGuide/WishList.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/StockingGuide/WishListDetail.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/kpi.js")%>" type="text/javascript"></script>
    
    <script type="text/javascript">
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';

    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="admin_top_btns_holder">
        <div id="btnKPI" class="admin_top_btns">
            <a href="<%=Url.Action("Index","StockingGuide") %>">KPI Dashboard</a>
        </div>
        <div id="btnStockingGuide" class="admin_top_btns">
            <a href="<%=Url.Action("StockingGuideBrand","StockingGuide") %>">Stocking Guide</a>
        </div>
        <div id="btnWishList" class="admin_top_btns admin_top_btns_active">
            Wishlist
        </div>
        <div id="btnInventoryStatisticCalculator" class="admin_top_btns">
            <a href="<%=Url.Action("InventoryStatisticCalculator","StockingGuide") %>">ISC</a>
        </div>
        <div id="btnMarketSearch" class="admin_top_btns">
            <a href="<%=Url.Action("MarketSearch","Chart") %>">Market Search</a>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("_TemplateWishListDetail")  %>
</asp:Content>