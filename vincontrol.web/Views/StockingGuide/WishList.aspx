<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    WishList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="stockingGuide_tab" style="float: left; width: 100%;">
                WishList
            </div>

            <div class="stockingGuide_tabBrand stockingGuide_tabActive">
                <a href="<%=Url.Action("StockingGuideBrand") %>">List</a>
            </div>
        </div>
    </div>
    <div id="container_right_content">
        <div style="padding: 20px;">
            This is a list of all the vehicles you have currently marked as wished. You can manage the list here, as well  as view the vehicles on the market within that category.
        </div>
        <div id="DivContent">
            <div id="divHasContent" class="vin_listVehicle_holder">
            </div>
            <div id="divNoContent" style="display: none; padding-left: 400px; margin-top: 10px;">There are no vehicles in this category.</div>
        </div>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="/Content/kpi.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/VinControl/v3_kpi.css" rel="stylesheet" type="text/css"/>
    <link href="<%=Url.Content("~/Content/Vincontrol/StockingGuide/WishList.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/kpi.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/StockingGuide/WishList.js")%>" type="text/javascript"></script>
    
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
    <%=Html.Partial("_TemplateWishList")  %>
</asp:Content>