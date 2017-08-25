<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<List<vincontrol.Data.Model.SGDealerBrand>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Stocking Guide - Brand
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="stockingGuide_tab" style="float: left; width: 100%;">
                Stocking Guide
            </div>
            <a href="<%=Url.Action("StockingGuideBrand") %>">
                <div class="stockingGuide_tabBrand stockingGuide_tabActive">
                    Brand
                </div>
            </a>
            <a href="<%=Url.Action("StockingGuideOther") %>">
                <div class="stockingGuide_tabOther">
                    Other
                </div>
            </a>
            <a href="<%=Url.Action("StockingGuideRecommendation") %>">
                <div class="stockingGuide_tabOther">
                    Recommendation
                </div>
            </a>
            <div class="stockingGuide_filterModel">
                <div style="float: left">
                    <select id="DDLFilterModel" multiple="multiple" style="height: 25px; width: 195px;">
                    </select>
                </div>
                <div class="btns_shadow profile_top_items_btn" style="float: left" id="btnSaveBrand">Save</div>
            </div>
            <%--<div class="stockingGuide_btn"><a href="<%=Url.Action("Index") %>">KPI Dashboard</a></div>--%>
        </div>
    </div>
    <div class="v3_kpi_container">
        <div class="brandList_title">
            This information is based on a 90 day history, stocking guide is relevant to your inventory and market data is based on a 100 mile radius around your dealership.
        </div>
        <div class="v3BrandOther_list_holder" id="divBrand">
            <%--<div class="v3BrandOther_list">
                <div id="divBrandHeader">
                    
                </div>    
                <div id="divBrandContent">
                    
                </div>    
            </div>--%>
        </div>

    </div>
    
    <input type="hidden" id="hdBrandSelection" value="<%=ViewData["BrandSelection"] %>" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="/Content/kpi.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/VinControl/v3_kpi.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/VinControl/multiple-select.css" rel="stylesheet" type="text/css"/>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/underscore.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/jquery.multiple.select.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/StockingGuideBrand.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/kpi.js")%>" type="text/javascript"></script>
    
    <script type="text/javascript">
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';
        $("select#DDLFilterModel").multipleSelect({
            multiple: true,
            multipleWidth: 55
        });
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
    <%=Html.Partial("_TemplateBrand")  %>
    <%=Html.Partial("_TemplateFilterModel")  %>
</asp:Content>