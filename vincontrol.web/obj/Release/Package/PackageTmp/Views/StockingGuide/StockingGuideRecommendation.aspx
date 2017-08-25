<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Stocking Guide - Recommendation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="stockingGuide_tab" style="float: left; width: 100%;">
                Stocking Guide
            </div>
            <a href="<%=Url.Action("StockingGuideBrand") %>">
                <div class="stockingGuide_tabBrand">
                    Brand
                </div>
            </a>
            <a href="<%=Url.Action("StockingGuideOther") %>">
                <div class="stockingGuide_tabOther">
                    Other
                </div>
            </a>
            <a href="<%=Url.Action("StockingGuideRecommendation") %>">
                <div class="stockingGuide_tabOther stockingGuide_tabActive">
                    Recommendation
                </div>
            </a>
            <div style="float: right">
                <select id="ddlMake" style="padding: 2px 4px;"><option value="">Make</option></select>
                <select id="ddlModel" style="padding: 2px 4px;"><option value="">Model</option></select>
            </div>
        </div>
    </div>
    <div class="v3_kpi_container">
        <div class="brandList_title">
            This information is based on a 90 day history, stocking guide is relevant to your inventory and market data is based on a 100 mile radius around your dealership.
        </div>
        <div class="v3BrandOther_list_holder" id="v3OtherListHolder">
            <div class="v3BrandOther_row">
                <div class="v3BrandOther_Segments_Detail" style="display: block;">
                    <div id="SDTableMarket" class="v3BrandOther_SD_table">
                        <div id="header"></div>
                        <div id="body"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="/Content/kpi.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/VinControl/v3_kpi.css" rel="stylesheet" type="text/css"/>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    
    <script src="<%=Url.Content("~/js/underscore.js")%>" type="text/javascript"></script>
    <%--<script src="<%=Url.Content("~/js/VinControl/StockingGuideBrandRecommendation.js")%>" type="text/javascript"></script>--%>
    <script src="<%=Url.Content("~/js/VinControl/StockingGuideSoldOutRecommendation.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
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
        <div id="btnStockingGuide" class="admin_top_btns admin_top_btns_active">
            Stocking Guide
        </div>
        <div id="btnWishList" class="admin_top_btns">
            <a href="<%=Url.Action("Wishlist","StockingGuide") %>">Wishlist</a>
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
    <%--<%=Html.Partial("_TemplateBrandRecommendation")  %>
    <%=Html.Partial("_TemplateBrandInventory")  %>
    <%=Html.Partial("_TemplateBrandOtherDetail") %>
    <%=Html.Partial("_TemplateBrandOtherInventoryDetail") %>--%>
</asp:Content>