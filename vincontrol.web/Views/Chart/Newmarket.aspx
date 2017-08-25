<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Market Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("MarketSearch"); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ui.dropdownchecklist-1.4-min.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="admin_top_btns_holder">
        <div id="btnKPI" class="admin_top_btns">
            <a href="<%=Url.Action("Index","StockingGuide") %>">KPI Dashboard</a>
            <div class="number_below" id="inventory_used_tab_number">
            </div>
        </div>
        <div id="btnStockingGuide" class="admin_top_btns">
            <a href="<%=Url.Action("StockingGuideBrand","StockingGuide") %>">Stocking Guide</a>
        </div>
        <div id="btnWishList" class="admin_top_btns">
            <a href="<%=Url.Action("Wishlist","StockingGuide") %>">Wishlist</a>
        </div>
        <div id="btnInventoryStatisticCalculator" class="admin_top_btns">
            <a href="<%=Url.Action("InventoryStatisticCalculator","StockingGuide") %>">ISC</a>
        </div>
        <div id="btnMarketSearch" class="admin_top_btns admin_top_btns_active">
            Market Search
                <div class="number_below number_belowActive" id="market_search_v2_number">0</div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientTemplates" runat="server">
    
    <%=Html.Partial("_TemplateMarketChart")  %>
</asp:Content>
