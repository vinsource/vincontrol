<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InventoryFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    KPI Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container_right_btn_holder">
        <%--<div id="container_right_btns">
            <div class="stockingGuide_btn"><a href="<%=Url.Action("StockingGuideBrand") %>">Stocking Guide</a></div>
        </div>--%>
        <div id="container_right_btns">
            <div class="kpi_fixed_title">
                KPI Dashboard
            </div>
        </div>
    </div>
    <div class="v3_kpi_container">
        <div class="v3_kpi_top">
            <div id="containerChartTop" style="min-width: 310px; height: 150px; margin: 0 auto"></div>
        </div>
        <div class="v3_kpi_statistic">
            <div class="v3_kpi_statistic_chart" id="v3Kpi_chartLeft">
                <div class="v3Kpi_sc_holder" style="margin-left: 65px;">
                    <div class="gauges" id="iHealthInventory">
                    </div>
                </div>
                <!--<div class="kpi_top_charts_text">
					Inventory Vs Market
					</div>-->
                <div class="v3Kpi_sc_info">
                    <div class="v3Kpi_st_chart_items kpi_pdp_price_above">
                        <div class="v3Kpi_stc_items_key">
                            Above Market
                        </div>
                        <div class="v3Kpi_stc_items_value" id="kpiData_4" style="cursor: pointer">
                            <div class="v3Kpi_stci_icon">
                                <img src="../Content/images/vincontrol/market_higher.gif">
                            </div>
                            <div class="v3Kpi_stci_value">
                                <%=Html.DynamicHtmlLabelForKPI("txtPercentAboveMarket", "PercentAboveMarket")%>
                                    (<%=Html.DynamicHtmlLabelForKPI("txtAboveMarket", "AboveMarket")%>)
                            </div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items kpi_pdp_price_average">
                        <div class="v3Kpi_stc_items_key">
                            At Market
                        </div>
                        <div class="v3Kpi_stc_items_value" id="kpiData_5" style="cursor: pointer">
                            <div class="v3Kpi_stci_icon">
                                <img src="../Content/images/vincontrol/market_equal.gif">
                            </div>
                            <div class="v3Kpi_stci_value">
                                <%=Html.DynamicHtmlLabelForKPI("txtPercentAverageMarket", "PercentAverageMarket")%>
                                    (<%=Html.DynamicHtmlLabelForKPI("txtAverageMarket", "AverageMarket")%>)
                            </div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items kpi_pdp_price_below">
                        <div class="v3Kpi_stc_items_key">
                            Below Market
                        </div>
                        <div class="v3Kpi_stc_items_value" id="kpiData_6" style="cursor: pointer">
                            <div class="v3Kpi_stci_icon">
                                <img src="../Content/images/vincontrol/market_lower.gif">
                            </div>
                            <div class="v3Kpi_stci_value">
                                <%=Html.DynamicHtmlLabelForKPI("txtPercentBelowMarket", "PercentBelowMarket")%>
                                    (<%=Html.DynamicHtmlLabelForKPI("txtBelowMarket", "BelowMarket")%>)
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="v3_kpi_statistic_bars">
                <div class="v3Kpi_bars_select" style="margin-top: 72px">
                    <label style="display: block; float: left; width: 80px;">Segments</label>
                    <% if (ViewData["Segments"] != null) %>
                    <%
                       {
                           List<Vincontrol.Web.Controllers.SegmentInfo> segments = (List<Vincontrol.Web.Controllers.SegmentInfo>)ViewData["Segments"];
                    %>

                    <select id="v3Kpi_bars_segments">
                        <option value="<%=segments.Sum(x=>x.History) %>|<%=segments.Sum(x=>x.Stock) %>|<%= (segments.Sum(x=>x.History)==0 || segments.Sum(x=>x.Stock)==0)?0: (int)Math.Ceiling(Convert.ToDecimal((double)(segments.Sum(x=>x.Stock) / (double)segments.Sum(x=>x.History)) * 30)) %>|<%=(segments.Sum(x=>x.History)==0 || segments.Sum(x=>x.Stock)==0)?"0.00": Convert.ToDecimal(((double)segments.Sum(x=>x.History) / (double)segments.Sum(x=>x.Stock)) * 12).ToString("0.00") %>|<%=Convert.ToDecimal(segments.Sum(x=>x.Profit)).ToString("C0") %>">All</option>
                        <% List<int> listType = new List<int>(); %>
                        <% foreach (var item in segments)
                           { %>
                        <% if (!listType.Contains(item.Type)) %>
                        <%
                           {
                               listType.Add(item.Type);  %>
                        <% if (item.Type == 1) %>
                        <%
                           {
                               var segmentBrand = segments.Where(x => x.Type == 1);
                        %>
                        <option value="<%=segmentBrand.Sum(x=>x.History) %>|<%=segmentBrand.Sum(x=>x.Stock) %>|<%=(segmentBrand.Sum(x=>x.History)==0 || segmentBrand.Sum(x=>x.Stock)==0)?0: (int)Math.Ceiling(Convert.ToDecimal(((double)segmentBrand.Sum(x=>x.Stock) / (double)segmentBrand.Sum(x=>x.History)) * 30)) %>|<%=(segmentBrand.Sum(x=>x.History)==0 || segmentBrand.Sum(x=>x.Stock)==0)?"0.00": Convert.ToDecimal(((double)segmentBrand.Sum(x=>x.History) / (double)segmentBrand.Sum(x=>x.Stock)) * 12).ToString("0.00") %>|<%=Convert.ToDecimal(segmentBrand.Sum(x=>x.Profit)).ToString("C0") %>">Brand</option>
                        <% } %>
                        <%
                           else
                           {
                               var segment = segments.Where(x => x.Type == 2);
                        %>
                        <option value="<%=segment.Sum(x=>x.History) %>|<%=segment.Sum(x=>x.Stock) %>|<%=(segment.Sum(x=>x.History)==0 || segment.Sum(x=>x.Stock)==0)?0: (int)Math.Ceiling(Convert.ToDecimal(((double)segment.Sum(x=>x.Stock) / (double)segment.Sum(x=>x.History)) * 30)) %>|<%=(segment.Sum(x=>x.History)==0 || segment.Sum(x=>x.Stock)==0)?"0.00":Convert.ToDecimal(((double)segment.Sum(x=>x.History) / (double)segment.Sum(x=>x.Stock)) * 12).ToString("0.00") %>|<%=Convert.ToDecimal(segment.Sum(x=>x.Profit)).ToString("C0") %>">Segments</option>
                        <% } %>
                        <% } %>
                        <option value="<%=(int)Math.Ceiling(Convert.ToDecimal(item.History)) %>|<%=item.Stock %>|<%=(int)Math.Ceiling(Convert.ToDecimal(item.Supply)) %>|<%=item.StrTurn %>|<%=Convert.ToDecimal(item.Profit).ToString("C0") %>">&nbsp;&nbsp;&nbsp;&nbsp;<%=item.Name %></option>
                        <% } %>
                    </select>
                    <% } %>
                    <%
                       else
                       { %>
                    <select id="v3Kpi_bars_segments">
                        <option>All Segments</option>
                    </select>
                    <% } %>
                </div>
                <br />
                <br />
                <div class="v3Kpi_bars_left" style="padding-left: 5px; width: 100%">
                    <%--<img src="../Content/images/vincontrol/v3_kpi_barChart_left.png">--%>
                    <div class="v3Kpi_st_chart_items_New kpi_pdp_price_below_New">
                        <div class="v3Kpi_stc_items_key">
                            History
                        </div>
                        <div class="v3Kpi_stc_items_value">
                            <div class="v3Kpi_stci_value" id="divHistory">
                                10
                            </div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items_New kpi_pdp_price_below_New">
                        <div class="v3Kpi_stc_items_key">
                            Stock
                        </div>
                        <div class="v3Kpi_stc_items_value">
                            <div class="v3Kpi_stci_value" id="divStock">
                                10
                            </div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items_New kpi_pdp_price_below_New">
                        <div class="v3Kpi_stc_items_key">
                            Supply
                        </div>
                        <div class="v3Kpi_stc_items_value">
                            <div class="v3Kpi_stci_value" id="divSupply">
                                10
                            </div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items_New kpi_pdp_price_below_New">
                        <div class="v3Kpi_stc_items_key">
                            Turn
                        </div>
                        <div class="v3Kpi_stc_items_value">
                            <div class="v3Kpi_stci_value" id="divTurn">
                                10
                            </div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items_NewProfit kpi_pdp_price_below_New">
                        <div class="v3Kpi_stc_items_key">
                            Est. Yearly Gross Profit
                        </div>
                        <div class="v3Kpi_stc_items_value">
                            <div class="v3Kpi_stci_value" id="divProfit">
                                10
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div class="v3Kpi_bars_right">
                    
                </div>--%>
            </div>
            <div class="v3_kpi_statistic_chart" id="v3Kpi_chartRight">

                <div class="v3Kpi_sc_holder" style="margin-left: 65px;">
                    <%--<img src="../Content/images/vincontrol/v3_kpi_rightChart.png">--%>
                    <div class="gauges" id="iHealthContent">
                    </div>
                </div>
                <!--<div class="kpi_top_charts_text">
					Content
					</div>-->
                <div class="v3Kpi_sc_info">
                    <div class="v3Kpi_st_chart_items kpi_pdp_price_above">
                        <div class="v3Kpi_stc_items_key">
                            Pictures
                        </div>
                        <div class="v3Kpi_stc_items_value" id="kpiData_1" style="cursor: pointer">

                            <div class="v3Kpi_stci_value"><%=Html.DynamicHtmlLabelForKPI("txtPercentPics", "PercentPics")%></div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items kpi_pdp_price_average">
                        <div class="v3Kpi_stc_items_key">
                            Descriptions
                        </div>
                        <div class="v3Kpi_stc_items_value" id="kpiData_3" style="cursor: pointer">

                            <div class="v3Kpi_stci_value"><%=Html.DynamicHtmlLabelForKPI("txtPercentDescriptions", "PercentDescriptions")%></div>
                        </div>
                    </div>
                    <div class="v3Kpi_st_chart_items kpi_pdp_price_below">
                        <div class="v3Kpi_stc_items_key">
                            Price
                        </div>
                        <div class="v3Kpi_stc_items_value" id="kpiData_2" style="cursor: pointer">

                            <div class="v3Kpi_stci_value">
                                <%=Html.DynamicHtmlLabelForKPI("txtPercentSalePrice", "PercentSalePrice")%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="v3_kpi_times">
            <div class="v3Kpi_times_top">
                <div class="v3Kpi_times_top_items v3Kpi_times_top_title" style="width: 17%">
                    Inventory Age (% of inventory)
                </div>
                <div class="v3Kpi_times_top_items" style="width: 11%">
                    <div class="v3Kpi_tti_icon v3Kpi_tti_un15"></div>
                    <div class="v3Kpi_tti_text">
                        Under 15 Days
                    </div>
                </div>
                <div class="v3Kpi_times_top_items" style="width: 20%">
                    <div class="v3Kpi_tti_icon v3Kpi_tti_un30"></div>
                    <div class="v3Kpi_tti_text">
                        Over 15 Days and Under 30 Days
                    </div>
                </div>
                <div class="v3Kpi_times_top_items" style="width: 22%">
                    <div class="v3Kpi_tti_icon v3Kpi_tti_un60"></div>
                    <div class="v3Kpi_tti_text">
                        Over 30 Days and Under 60 Days
                    </div>
                </div>
                <div class="v3Kpi_times_top_items" style="width: 20%">
                    <div class="v3Kpi_tti_icon v3Kpi_tti_un90"></div>
                    <div class="v3Kpi_tti_text">
                        Over 60 Days and Under 90 Days
                    </div>
                </div>
                <div class="v3Kpi_times_top_items" style="width: 10%">
                    <div class="v3Kpi_tti_icon v3Kpi_tti_over"></div>
                    <div class="v3Kpi_tti_text">
                        Over 90 Days
                    </div>
                </div>
            </div>
            <div class="v3Kpi_times_bottom">
                <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory") != "0%") %>
                <%
                   { %>
                <div style="width: <%= Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory") %>" class="v3Kpi_tb_items v3Kpi_tti_un15 hasValue" id="kpiData_7">
                    <%= Html.DynamicHtmlLabelForKPI("txt15Inventory", "0-15InInventory") %>
                    <div class="bubble_times bubble_times_first">
                        <div class="bubble_times_price">
                            <div class="btp_title">
                                Avg Price
                            </div>
                            <div class="btp_value">
                                <%= Html.DynamicHtmlLabelForKPI("txt15InventoryPriceAvg", "0-15InInventoryPriceAvg") %>
                            </div>
                        </div>
                        <div class="bubble_times_market">
                            <div class="btp_title">
                                Market
                            </div>

                            <div class="btp_market_list">
                                <div class="btp_market_items" id="kpiData_17">
                                    <div class="btp_market_icon">
                                        <img src="../Content/images/vincontrol/market_higher.gif">
                                    </div>
                                    <div class="btp_market_num">
                                        <%= Html.DynamicHtmlLabelForKPI("txt15InventoryAbove", "0-15InInventoryAbove") %>
                                    </div>
                                </div>
                                <div class="btp_market_items" id="kpiData_18">
                                    <div class="btp_market_icon">
                                        <img src="../Content/images/vincontrol/market_equal.gif">
                                    </div>
                                    <div class="btp_market_num">
                                        <%= Html.DynamicHtmlLabelForKPI("txt15InventoryAvg", "0-15InInventoryAvg") %>
                                    </div>
                                </div>
                                <div class="btp_market_items" id="kpiData_19">
                                    <div class="btp_market_icon">
                                        <img src="../Content/images/vincontrol/market_lower.gif">
                                    </div>
                                    <div class="btp_market_num">
                                        <%= Html.DynamicHtmlLabelForKPI("txt15InventoryBelow", "0-15InInventoryBelow") %>
                                    </div>
                                </div>
                                <div class="btp_market_items" id="kpiData_20">
                                    <div class="btp_market_icon">
                                        <img src="../Content/images/market_question.gif">
                                    </div>
                                    <div class="btp_market_num">
                                        <%= Html.DynamicHtmlLabelForKPI("txt15InventoryOther", "0-15InInventoryOther") %>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <% } %>

                <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent16-30InInventory") != "0%") %>
                <%
                   { %>
                <div style="width: <%=Html.DynamicHtmlLabelForKPI("txtPercent30Inventory", "Percent16-30InInventory")%>" class="v3Kpi_tb_items v3Kpi_tti_un30 hasValue" id="kpiData_9">
                    <%=Html.DynamicHtmlLabelForKPI("txt30Inventory", "16-30InInventory")%>
                    <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory") == "0%") %>
                    <%
                       { %>
                    <div class="bubble_times bubble_times_first">
                        <% } %>
                        <%
                       else
                       { %>
                        <div class="bubble_times">
                            <% } %>
                            <div class="bubble_times_price">
                                <div class="btp_title">
                                    Avg Price
                                </div>
                                <div class="btp_value">
                                    <%= Html.DynamicHtmlLabelForKPI("txt16-30InventoryPriceAvg", "16-30InInventoryPriceAvg") %>
                                </div>
                            </div>
                            <div class="bubble_times_market">
                                <div class="btp_title">
                                    Market
                                </div>

                                <div class="btp_market_list">
                                    <div class="btp_market_items" id="kpiData_21">
                                        <div class="btp_market_icon">
                                            <img src="../Content/images/vincontrol/market_higher.gif">
                                        </div>
                                        <div class="btp_market_num">
                                            <%=Html.DynamicHtmlLabelForKPI("txt30InventoryAbove", "16-30InInventoryAbove")%>
                                        </div>
                                    </div>
                                    <div class="btp_market_items" id="kpiData_22">
                                        <div class="btp_market_icon">
                                            <img src="../Content/images/vincontrol/market_equal.gif">
                                        </div>
                                        <div class="btp_market_num">
                                            <%=Html.DynamicHtmlLabelForKPI("txt30InventoryAvg", "16-30InInventoryAvg")%>
                                        </div>
                                    </div>
                                    <div class="btp_market_items" id="kpiData_23">
                                        <div class="btp_market_icon">
                                            <img src="../Content/images/vincontrol/market_lower.gif">
                                        </div>
                                        <div class="btp_market_num">
                                            <%=Html.DynamicHtmlLabelForKPI("txt30InventoryBelow", "16-30InInventoryBelow")%>
                                        </div>
                                    </div>
                                    <div class="btp_market_items" id="kpiData_24">
                                        <div class="btp_market_icon">
                                            <img src="../Content/images/market_question.gif">
                                        </div>
                                        <div class="btp_market_num">
                                            <%=Html.DynamicHtmlLabelForKPI("txt30InventoryOther", "16-30InInventoryOther")%>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <% } %>

                    <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent31-60InInventory") != "0%") %>
                    <%
                       { %>
                    <div style="width: <%=Html.DynamicHtmlLabelForKPI("txtPercent60Inventory", "Percent31-60InInventory")%>" class="v3Kpi_tb_items v3Kpi_tti_un60 hasValue" id="kpiData_11">
                        <%=Html.DynamicHtmlLabelForKPI("txt60Inventory", "31-60InInventory")%>
                        <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory") == "0%" && Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent16-30InInventory") == "0%") %>
                        <%
                           { %>
                        <div class="bubble_times bubble_times_first">
                            <% } %>
                            <%
                           else
                           { %>
                            <div class="bubble_times">
                                <% } %>
                                <div class="bubble_times_price">
                                    <div class="btp_title">
                                        Avg Price
                                    </div>
                                    <div class="btp_value">
                                        <%= Html.DynamicHtmlLabelForKPI("txt31-60InventoryPriceAvg", "31-60InInventoryPriceAvg") %>
                                    </div>
                                </div>
                                <div class="bubble_times_market">
                                    <div class="btp_title">
                                        Market
                                    </div>

                                    <div class="btp_market_list">
                                        <div class="btp_market_items" id="kpiData_25">
                                            <div class="btp_market_icon">
                                                <img src="../Content/images/vincontrol/market_higher.gif">
                                            </div>
                                            <div class="btp_market_num">
                                                <%=Html.DynamicHtmlLabelForKPI("txt60InventoryAbove", "31-60InInventoryAbove")%>
                                            </div>
                                        </div>
                                        <div class="btp_market_items" id="kpiData_26">
                                            <div class="btp_market_icon">
                                                <img src="../Content/images/vincontrol/market_equal.gif">
                                            </div>
                                            <div class="btp_market_num">
                                                <%=Html.DynamicHtmlLabelForKPI("txt60InventoryAvg", "31-60InInventoryAvg")%>
                                            </div>
                                        </div>
                                        <div class="btp_market_items" id="kpiData_27">
                                            <div class="btp_market_icon">
                                                <img src="../Content/images/vincontrol/market_lower.gif">
                                            </div>
                                            <div class="btp_market_num">
                                                <%=Html.DynamicHtmlLabelForKPI("txt60InventoryBelow", "31-60InInventoryBelow")%>
                                            </div>
                                        </div>
                                        <div class="btp_market_items" id="kpiData_28">
                                            <div class="btp_market_icon">
                                                <img src="../Content/images/market_question.gif">
                                            </div>
                                            <div class="btp_market_num">
                                                <%=Html.DynamicHtmlLabelForKPI("txt60InventoryOther", "31-60InInventoryOther")%>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <% } %>

                        <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent61-90InInventory") != "0%") %>
                        <%
                           { %>
                        <div style="width: <%=Html.DynamicHtmlLabelForKPI("txtPercent90Inventory", "Percent61-90InInventory")%>" class="v3Kpi_tb_items v3Kpi_tti_un90 hasValue" id="kpiData_13">
                            <%=Html.DynamicHtmlLabelForKPI("txt90Inventory", "61-90InInventory")%>
                            <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory") == "0%" && Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent16-30InInventory") == "0%" && Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent31-60InInventory") == "0%") %>
                            <%
                               { %>
                            <div class="bubble_times bubble_times_first">
                                <% } %>
                                <%
                               else
                               { %>
                                <div class="bubble_times">
                                    <% } %>
                                    <div class="bubble_times_price">
                                        <div class="btp_title">
                                            Avg Price
                                        </div>
                                        <div class="btp_value">
                                            <%= Html.DynamicHtmlLabelForKPI("txt61-90InventoryPriceAvg", "61-90InInventoryPriceAvg") %>
                                        </div>
                                    </div>
                                    <div class="bubble_times_market">
                                        <div class="btp_title">
                                            Market
                                        </div>

                                        <div class="btp_market_list">
                                            <div class="btp_market_items" id="kpiData_29">
                                                <div class="btp_market_icon">
                                                    <img src="../Content/images/vincontrol/market_higher.gif">
                                                </div>
                                                <div class="btp_market_num">
                                                    <%=Html.DynamicHtmlLabelForKPI("txt90InventoryAbove", "61-90InInventoryAbove")%>
                                                </div>
                                            </div>
                                            <div class="btp_market_items" id="kpiData_30">
                                                <div class="btp_market_icon">
                                                    <img src="../Content/images/vincontrol/market_equal.gif">
                                                </div>
                                                <div class="btp_market_num">
                                                    <%=Html.DynamicHtmlLabelForKPI("txt90InventoryAvg", "61-90InInventoryAvg")%>
                                                </div>
                                            </div>
                                            <div class="btp_market_items" id="kpiData_31">
                                                <div class="btp_market_icon">
                                                    <img src="../Content/images/vincontrol/market_lower.gif">
                                                </div>
                                                <div class="btp_market_num">
                                                    <%=Html.DynamicHtmlLabelForKPI("txt90InventoryBelow", "61-90InInventoryBelow")%>
                                                </div>
                                            </div>
                                            <div class="btp_market_items" id="kpiData_32">
                                                <div class="btp_market_icon">
                                                    <img src="../Content/images/market_question.gif">
                                                </div>
                                                <div class="btp_market_num">
                                                    <%=Html.DynamicHtmlLabelForKPI("txt90InventoryOther", "61-90InInventoryOther")%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <% } %>

                            <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent90OverInInventory") != "0%") %>
                            <%
                               { %>
                            <div style="width: <%=Html.DynamicHtmlLabelForKPI("txtPercentOverInventory", "Percent90OverInInventory")%>" class="v3Kpi_tb_items v3Kpi_tti_over hasValue" id="kpiData_15">
                                <%=Html.DynamicHtmlLabelForKPI("txtOverInventory", "90OverInInventory")%>
                                <% if (Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent0-15InInventory") == "0%" && Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent16-30InInventory") == "0%" && Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent31-60InInventory") == "0%" && Html.DynamicHtmlLabelForKPI("txtPercent15Inventory", "Percent61-90InInventory") == "0%") %>
                                <%
                                   { %>
                                <div class="bubble_times bubble_times_first">
                                    <% } %>
                                    <%
                                   else
                                   { %>
                                    <div class="bubble_times">
                                        <% } %>
                                        <div class="bubble_times_price">
                                            <div class="btp_title">
                                                Avg Price
                                            </div>
                                            <div class="btp_value">
                                                <%= Html.DynamicHtmlLabelForKPI("txt90OverInventoryPriceAvg", "90OverInInventoryPriceAvg") %>
                                            </div>
                                        </div>
                                        <div class="bubble_times_market">
                                            <div class="btp_title">
                                                Market
                                            </div>

                                            <div class="btp_market_list">
                                                <div class="btp_market_items" id="kpiData_33">
                                                    <div class="btp_market_icon">
                                                        <img src="../Content/images/vincontrol/market_higher.gif">
                                                    </div>
                                                    <div class="btp_market_num">
                                                        <%=Html.DynamicHtmlLabelForKPI("txtOverInventoryAbove", "90OverInInventoryAbove")%>
                                                    </div>
                                                </div>
                                                <div class="btp_market_items" id="kpiData_34">
                                                    <div class="btp_market_icon">
                                                        <img src="../Content/images/vincontrol/market_equal.gif">
                                                    </div>
                                                    <div class="btp_market_num">
                                                        <%=Html.DynamicHtmlLabelForKPI("txtOverInventoryAvg", "90OverInInventoryAvg")%>
                                                    </div>
                                                </div>
                                                <div class="btp_market_items" id="kpiData_35">
                                                    <div class="btp_market_icon">
                                                        <img src="../Content/images/vincontrol/market_lower.gif">
                                                    </div>
                                                    <div class="btp_market_num">
                                                        <%=Html.DynamicHtmlLabelForKPI("txtOverInventoryBelow", "90OverInInventoryBelow")%>
                                                    </div>
                                                </div>
                                                <div class="btp_market_items" id="kpiData_36">
                                                    <div class="btp_market_icon">
                                                        <img src="../Content/images/market_question.gif">
                                                    </div>
                                                    <div class="btp_market_num">
                                                        <%=Html.DynamicHtmlLabelForKPI("txtOverInventoryOther", "90OverInInventoryOther")%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <% } %>
                            </div>
                        </div>

                        <div class="kpi_list_holder">
                            <div class="kpi_list_header">
                                <div class="kpi_list_collum kpi_list_img" style="width: 54px!important;">
                                    <a class="iframeCommon" href="<%=Url.Action("PrintOption","Report",new {Condition=1}) %>"
                                        id="kpiPrint">
                                        <div id="addNewBG" class="btns_shadow kpi_print_btns">
                                            Print
                                        </div>
                                    </a>
                                </div>
                                <a href="javascript:void(0);">
                                    <div class="kpi_list_collum" style="width: 43px; padding-left: 38px;" id="kpisort_9" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="age">
                                        Age
                            <img class="imgSort" id="imgSortAge" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a>
                                <a href="javascript:void(0);">
                                    <div class="kpi_list_collum kpi_list_marketData" style="width: 52px; margin-left: 15px;" id="kpisort_10" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="market">
                                        Rank
                            <img class="imgSort" id="imgSortMarket" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a>
                                <a href="javascript:void(0);">
                                    <div class="kpi_list_collum kpi_list_vin" id="kpisort_1" style="width: 60px !important;" onmouseover="this.style.background='gray';" onmouseout="this.style.background='white';" value="vin">
                                        VIN
                            <img class="imgSort" id="imgSortVin" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum" style="width: 56px; margin-left: 19px;" id="kpisort_2" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="stock">
                                        Stock
                            <img class="imgSort" id="imgSortStock" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum" style="width: 51px; margin-left: 15px;" id="kpisort_3" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="year">
                                        Year
                            <img class="imgSort" id="imgSortYear" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum_mediumHeader" id="kpisort_4" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="make">
                                        Make
                            <img class="imgSort" id="imgSortMake" src="../Content/images/vincontrol/up.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum_longHeader" style="margin-left: 9px;" id="kpisort_5" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="model">
                                        Model
                            <img class="imgSort" id="imgSortModel" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum_mediumHeader" style="width: 48px!important; padding-right: 14px;" id="kpisort_6" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="trim">
                                        Trim
                            <img class="imgSort" id="imgSortTrim" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum_longHeader" style="width: 69px" id="kpisort_7" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="color">
                                        Color
                            <img class="imgSort" id="imgSortColor" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum" style="width: 70px; padding-left: 12px;" id="kpisort_11" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="miles">
                                        Miles
                            <img class="imgSort" id="imgSortMiles" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a><a href="javascript:void(0);">
                                    <div class="kpi_list_collum" style="padding-left: 8px;" id="kpisort_12" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="price">
                                        Price
                            <img class="imgSort" id="imgSortPrice" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a>
                                <a href="javascript:void(0);">
                                    <div class="kpi_list_collum" style="width: 90px; padding-left: 7px;" id="kpisort_8" onmouseover="this.style.background='gray';"
                                        onmouseout="this.style.background='white';" value="owners">
                                        Owners
                            <img class="imgSort" id="imgSortOwners" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                                    </div>
                                </a>

                            </div>
                            <div class="kpi_list_container" id="kpicontanier">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <input type="hidden" id="NeedToRedirectToDetailActivity" name="NeedToRedirectToDetailActivity" />
                    <input type="hidden" id="ActivityId" name="ActivityId" />
                    <%=Html.DynamicHtmlLabelForKPI("txtHiddenInventoryGauge", "HiddenInventoryGauge")%>
                    <%=Html.DynamicHtmlLabelForKPI("txtHiddenContentGauge", "HiddenContentGauge")%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="/Content/kpi.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/VinControl/v3_kpi.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        .right_content_items {
            margin-right: 0px;
            margin-top: 0px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/underscore.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/v3_kpi.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/kpi.js")%>" type="text/javascript"></script>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <script type='text/javascript' src="http://code.highcharts.com/highcharts.js"></script>
    <script type='text/javascript' src="http://code.highcharts.com/modules/exporting.js"></script>
    <script type="text/javascript">

        var inventoryObj = [];
        var categories = [];
        var chartData = [];
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';
        var viewInfo = { sortFieldName: 'make', isUp: true };

        var kpiListUrl = "/Market/GetKpiList";
        var chartTurnUrl = "/StockingGuide/GetChartData";

        var name = "Content";
        var value = 20;
        google.load('visualization', '1', { packages: ['gauge'] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var percentageInventory = $("#txtHiddenInventoryGauge").val();
            var percentageContent = $("#txtHiddenContentGauge").val();
            var numberInventory = parseInt(percentageInventory);
            var numberContent = parseInt(percentageContent);
            var dataInventory = google.visualization.arrayToDataTable([
                ['Label', 'Value'],
                ['Inventory', numberInventory]
            ]);
            var dataContent = google.visualization.arrayToDataTable([
                ['Label', 'Value'],
                ['Content', numberContent]
            ]);

            var options = {
                width: 565,
                height: 143,
                greenFrom: 66,
                greenTo: 100,
                redFrom: 0,
                redTo: 33,
                yellowFrom: 33,
                yellowTo: 66,
                minorTicks: 15
            };
            var chartInventory = new google.visualization.Gauge(document.getElementById('iHealthInventory'));
            chartInventory.draw(dataInventory, options);

            var chartContent = new google.visualization.Gauge(document.getElementById('iHealthContent'));
            chartContent.draw(dataContent, options);
        }

        function LoadChartData() {
            $.ajax({
                type: "POST",
                //                contentType: "text/JSON; charset=utf-8",
                dataType: "JSON",
                url: chartTurnUrl,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $.each(result.data, function (index, obj) {
                        categories.push(obj.WeekText);
                        var objTest = {};
                        objTest.week = obj.Week;
                        objTest.y = obj.Turn;
                        chartData.push(objTest);
                    });
                    $('#containerChartTop').highcharts({

                        title: {
                            text: 'Turn by Week'
                        },
                        xAxis: {
                            categories: categories
                        },
                        yAxis: {
                            title: {
                                text: 'Turn'
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        series: [{
                            data: chartData
                        }],
                        tooltip: {
                            formatter: function () {
                                return ' ' +
                                    'week: ' + this.point.week + ', turn: ' + Highcharts.numberFormat(this.point.y, 2);
                            }
                        }
                    });
                    //$.unblockUI();
                }
            });
        }

        $(function () {
            grid = new InventoryGrid(viewInfo);
            grid.bindSorting();
            grid.loadGrid();

            LoadChartData();

            $("div[id^=kpiData]").live('click', function (event) {
                event.stopPropagation();
                $("#kpicontanier").html("         <div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");
                var idValue = this.id.split('_')[1];
                kpiListUrl = "/Market/ViewConditionKpiJson?Condition=" + idValue;
                grid.loadGrid(kpiListUrl);

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="admin_top_btns_holder">
        <div id="btnKPI" class="admin_top_btns admin_top_btns_active">
            KPI Dashboard
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
        <div id="btnMarketSearch" class="admin_top_btns">
            <a href="<%=Url.Action("MarketSearch","Chart") %>">Market Search</a>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("../Market/_TemplateKPI")%>
</asp:Content>
