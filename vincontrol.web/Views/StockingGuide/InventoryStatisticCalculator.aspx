<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Inventory Statistic Calculator
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="kpi_fixed_title">
                Inventory Statistic Calculator
            </div>
        </div>
    </div>
    <%--<div class="inv_statistics_title"></div>--%>
    <div id="container_right_content" style="padding-left: 30px; padding-top: 10px;">
        <div class="inv_statistics_subtitle">Stock Correctly, Sell More; Maximize Turn!</div>
        <div class="inv_statistics_container">
            <div class="inv_statistics_header inv_statistics_black_background">Gross Calculator - Used</div>
            <div class="inv_statistics_item_container inv_statistics_items_gross_container">
                <div><span>Units in Inventory</span></div>
                <div><span>Sales Per Month(Average)</span></div>
                <div><span>Days Supply</span></div>
                <div><span>Total Sales Value of Inventory</span></div>
                <div><span>Average Cost of Inventory(%)</span></div>
                <div><span>Dollar Cost of Inventory($)</span></div>
                <div><span>Average Dollar Cost Per Unit($)</span></div>
                <div><span>Gross Profit Per Unit(%)</span></div>
                <div><span>Average Price Per Unit</span></div>
                <div><span>Total Profit Per Turn of Inventory</span></div>
                <div><span>Turn Rate of Inventory(Yearly)</span></div>
                <div><span>Desired Turn Rate(Yearly)</span></div>
                <div><span>Monthly Profit($)</span></div>
                <div><span>Annual Profit($)</span></div>
                <div><span>Annual ROI(%)</span></div>
            </div>
        </div>
        <div data-bind="with: currentInventoryStatus" class="inv_statistics_container inv_statistics_yourvalues">
            <form id="inventoryStatisticForm">
                <div class="inv_statistics_header inv_statistics_blue_background">Your Values</div>
                <div class="inv_statistics_item_container inv_statistics_items_values_container " style="background-color: #e6e6e6">
                    <div class="divValue">
                        <input type="text" id="txtUnits" data-bind='value: funits, valueUpdate: "blur", event: { blur: saveUnits }, attr: { "data-value": ounits }' data-validation-engine="validate[required,min[1]]" data-errormessage-value-missing="Units in Inventory is required!" />
                        <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" data-bind="visible: isSavingUnits" height="20" />
                    </div>
                    <div class="divValue">
                        <input type="text" id="txtSalesPerMonth" data-bind='value: fsalesPerMonth, valueUpdate: "blur", event: { blur: saveSalesPerMonth }' data-validation-engine="validate[required]" data-errormessage-value-missing="Sales Per Month is required!" />
                        <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" data-bind="visible: isSavingSalesPerMonth" height="20" />
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div class="divValue">
                        <input type="text" id="txtTotalSalesValue" data-bind="value: ftotalSalesValue, valueUpdate: 'blur', event: { blur: saveTotalSalesValue }" data-validation-engine="validate[required]" data-errormessage-value-missing="Total Sales Value of Inventory is required!" />
                        <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" data-bind="visible: isSavingTotalSalesValue" height="20" />
                    </div>
                    <div class="divValue">
                        <input type="text" id="txtAverageCost" data-bind="value: faverageCost, valueUpdate: 'blur', event: { blur: saveAverageCost }" maxlength="2" data-validation-engine="validate[required,max[100]]" data-errormessage-value-missing="Average Cost of Inventory is required!" />
                        <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" data-bind="visible: isSavingAverageCost" height="20" />
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div class="divValue">
                        <input type="text" id="txtGrossProfitPerUnit" data-bind="value: fgrossProfitPerUnit, valueUpdate: 'blur', event: { blur: saveGrossProfitPerUnit }" maxlength="2" data-validation-engine="validate[required,max[100]]" data-errormessage-value-missing="Gross Profit Per Unit is required!" />
                        <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" data-bind="visible: isSavingGrossProfitPerUnit" height="20" />
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div style="margin-top: 5px;">
                        <label data-bind="text: fturnRate"></label>
                    </div>
                    <div>
                        <%--<input type="text" id="txtDesiredTurnRate" data-bind="value: fturnRate" data-validation-engine="validate[required]" data-errormessage-value-missing="Desired Turn Rate is required!" />--%>
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <%--<div style="margin-bottom: 10px;">
                        <input type="button" value="Save my values" class="btnSaveInventoryStatistic" data-bind="click: save" />
                    </div>
                    <div style="clear: both; height: 49px;"></div>--%>
                </div>
            </form>
        </div>
        <div data-bind="with: currentInventoryStatus" class="inv_statistics_container">
            <div class="inv_statistics_header inv_statistics_black_background">Current Dealership Stats</div>
            <div class="inv_statistics_item_container inv_statistics_items_stats_container">
                <div><span data-bind="text: formatNumber(units())" /></div>
                <div><span data-bind="text: formatNumber(salesPerMonth())" /></div>
                <div><span data-bind="text: formatNumber(daysSupply())" /></div>
                <div><span data-bind="text: formatDolar(totalSalesValue())" /></div>
                <div><span data-bind="text: formatPercent(averageCost())" /></div>
                <div><span data-bind="text: formatDolar(dollarCost())" /></div>
                <div><span data-bind="text: formatDolar(averageDollarCostPerUnit())" /></div>
                <div><span data-bind="text: formatPercent(grossProfitPerUnit())" /></div>
                <div><span data-bind="text: formatDolar(averagePricePerUnit())" /></div>
                <div><span data-bind="text: formatDolar(totalProfitPerTurn())" /></div>
                <div><span style="background-color: red; color: white" data-bind="text: turnRate()"></span></div>
                <div><span style="background-color: #e6e6e6" />&nbsp;</div>
                <div><span data-bind="text: formatDolar(monthlyProfit())" /></div>
                <div><span data-bind="text: formatDolar(annualProfit())" /></div>
                <div><span data-bind="text: formatPercent(annualROI())" /></div>
            </div>
        </div>
        <div data-bind="with: desiredInventoryStatus" class="inv_statistics_container">
            <div class="inv_statistics_header inv_statistics_green_background">Optimal/Desired Stats</div>
            <div class="inv_statistics_item_container inv_statistics_items_stats_container">
                <div>
                    <input type="text" class="input_opt" id="txtUnits_opt" data-bind='value: funits, valueUpdate: "blur"' data-validation-engine="validate[required]" data-errormessage-value-missing="Units in Inventory is required!" />
                    <%--<span data-bind="text: formatNumber(units())" />--%>
                </div>
                <%--<div><span data-bind="text: formatNumber(salesPerMonth())" /></div>--%>
                <div>
                    <input type="text" class="input_opt" id="txtSalesPerMonth_opt" data-bind='value: fsalesPerMonth, valueUpdate: "blur"' data-validation-engine="validate[required]" data-errormessage-value-missing="Sales Per Month is required!" />
                </div>
                <div><span data-bind="text: formatNumber(daysSupply())" /></div>
                <%--<div><span data-bind="text: formatDolar(totalSalesValue())" /></div>--%>
                <div>
                    <input type="text" class="input_opt" id="txtTotalSalesValue_opt" data-bind='value: ftotalSalesValue, valueUpdate: "blur"' data-validation-engine="validate[required]" data-errormessage-value-missing="Sales Per Month is required!" />
                </div>
                <%--<div><span data-bind="text: formatPercent(averageCost())" /></div>--%>
                <div>
                    <input type="text" class="input_opt" id="txtAverageCost_opt" data-bind='value: faverageCost, valueUpdate: "blur"' maxlength="2" data-validation-engine="validate[required]" data-errormessage-value-missing="Sales Per Month is required!" />
                </div>
                <div><span data-bind="text: formatDolar(dollarCost())" /></div>
                <div><span data-bind="text: formatDolar(averageDollarCostPerUnit())" /></div>
                <%--<div><span data-bind="text: formatPercent(grossProfitPerUnit())" /></div>--%>
                <div>
                    <input type="text" class="input_opt" id="txtGrossProfitPerUnit_opt" data-bind='value: fgrossProfitPerUnit, valueUpdate: "blur"' maxlength="2" data-validation-engine="validate[required]" data-errormessage-value-missing="Sales Per Month is required!" />
                </div>
                <div><span data-bind="text: formatDolar(averagePricePerUnit())" /></div>
                <div><span data-bind="text: formatDolar(totalProfitPerTurn())" /></div>
                <div>
                    <span>&nbsp;</span>
                    <%--<span data-bind="text: formatNumber(turnRate())" />--%>
                </div>
                <%--<div><span style="background-color: #019343">&nbsp;</span></div>--%>
                <div>
                    <input type="text" class="input_opt" id="txtDesiredTurnRate" data-bind='value: fturnRate, valueUpdate: "blur"' data-validation-engine="validate[required]" data-errormessage-value-missing="Desired Turn Rate is required!" />
                </div>
                <div><span data-bind="text: formatDolar(monthlyProfit())" /></div>
                <div><span data-bind="text: formatDolar(annualProfit())" /></div>
                <div><span data-bind="text: formatPercent(annualROI())" /></div>
            </div>
        </div>
        <div class="inv_statistics_container" style="width: 200px;">
            <div class="inv_statistics_header inv_statistics_black_background">Difference</div>
            <div class="inv_statistics_item_container inv_statistics_items_stats_container">
                <div><span data-bind="text: formatNumber(unitsDiffence())" /></div>
                <div><span data-bind="text: formatNumber(salesPerMonthDiffence())" /></div>
                <div><span data-bind="text: formatNumber(daysSupplyDiffence())" /></div>
                <div><span data-bind="text: formatDolar(totalSalesValueDiffence())" /></div>
                <div><span data-bind="text: formatPercent(averageCostDiffence())" /></div>
                <div><span data-bind="text: formatDolar(dollarCostDiffence())" /></div>
                <div><span data-bind="text: formatDolar(averageDollarCostPerUnitDiffence())" /></div>
                <div><span data-bind="text: formatPercent(grossProfitPerUnitDiffence())" /></div>
                <div><span data-bind="text: formatDolar(averagePricePerUnitDiffence())" /></div>
                <div><span data-bind="text: formatDolar(totalProfitPerTurnDiffence())" /></div>
                <div><span data-bind="text: formatNumber(turnRateDiffence())" /></div>
                <div><span style="background-color: #019343">&nbsp;</span></div>
                <div><span data-bind="text: formatDolar(monthlyProfitDiffence())" /></div>
                <div><span data-bind="text: formatDolar(annualProfitDiffence())" /></div>
                <div><span data-bind="text: formatPercent(annualROIDiffence())" /></div>
            </div>
        </div>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="/Content/VinControl/StockingGuide/InventoryStatisticsCalculator.css" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/kpi.css")%>" rel="stylesheet" type="text/css" />
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="/Scripts/knockout-3.0.0.js" type="text/javascript"></script>
    <script src="/Scripts/knockout.mapping-latest.js" type="text/javascript"></script>
    <script src="/js/VinControl/StockingGuide/InventoryStatistics.js" type="text/javascript"></script>
    
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
        <div id="btnWishList" class="admin_top_btns">
            <a href="<%=Url.Action("Wishlist","StockingGuide") %>">Wishlist</a>
        </div>
        <div id="btnInventoryStatisticCalculator" class="admin_top_btns admin_top_btns_active">
            ISC
        </div>
        <div id="btnMarketSearch" class="admin_top_btns">
            <a href="<%=Url.Action("MarketSearch","Chart") %>">Market Search</a>
        </div>
    </div>
</asp:Content>
